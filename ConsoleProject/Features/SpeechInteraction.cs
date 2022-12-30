using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Globalization;
using System.Speech.Synthesis;
using Microsoft.EntityFrameworkCore;
using NAudio.Wave;

namespace ProjetoCrowdsourcing
{
    public class SpeechInteraction
    {
        private CultureInfo Culture { get; }
        public SpeechRecognitionEngine speechRecoEngine { get; }
        public SpeechSynthesizer speechSynthesizer { get; }
        public Question1 Question1Manager { get; set; }
        private Menu menu { get; set; }
        private bool play { get;set; }

        public SpeechInteraction(Question1 _question1Manager)
        {
            this.Question1Manager = _question1Manager;
            this.menu = new Menu();

            this.Culture = new CultureInfo("en-US");
            this.speechRecoEngine = new SpeechRecognitionEngine(this.Culture);
            this.speechSynthesizer = new SpeechSynthesizer();

            // Synthesizer configuration
            this.speechSynthesizer.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult, 0, this.Culture);

            this.setupGrammar();

            // Triggers function when command is recognized - maybe put outside this class
            speechRecoEngine.SpeechRecognized += SpeechRecoEngine_SpeechRecognized;

            this.speechRecoEngine.SetInputToDefaultAudioDevice();
            
        }

        public void StartMenu()
        {
            this.menu.Run();
        }

        /// <summary>
        ///  Triggered when a command is recognized - maybe also put outside this class
        /// </summary>
        private void SpeechRecoEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Console.WriteLine("\nSpeech Recognized: \t{0}" + e.Result.Confidence, e.Result.Text);

            if (e.Result.Confidence < 0.85)
                return;

            var result = e.Result.Text;
            if(e.Result.Text.Contains("ask for sample of"))
            {
                var instrument = result.Replace("ask for sample of", "");
                instrument = instrument.Trim();

                if(!MTurkUtils.AvailableInstruments.Contains(instrument)){
                    this.speechSynthesizer.Speak("we do not have this instrument available yet!");
                    return;
                }

                this.speechSynthesizer.Speak("asking the crowd");
                var createdHIT = this.Question1Manager.CreateHIT(instrument);
                Console.WriteLine(MTurkUtils.GetURLFromHIT(createdHIT.HIT.HITTypeId));
            }

            if (e.Result.Text == "show my samples")
            {
                foreach (var assignment in this.Question1Manager.db.Assignments.Include(x => x.HIT).Where(x => x.IsValid && x.Evaluated))
                {
                    this.speechSynthesizer.Speak("assignment " + assignment.Id + " - " + assignment.HIT.Instrument);
                    Console.WriteLine(assignment.Id + " - " + assignment.HIT.Instrument);
                }
            }

            if (e.Result.Text.Contains("play sample"))
            {
                var sampleID = result.Replace("play sample", "");
                sampleID = sampleID.Trim();

                var localAssignment = this.Question1Manager.db.Assignments.Include(x => x.HIT).Where(x => x.IsValid && x.Evaluated && x.Id.ToString() == sampleID).First();

                if (localAssignment == null)
                {
                    this.speechSynthesizer.Speak("this assignment is not available");
                }

                var assignmentMTurk = this.Question1Manager.GetAssignment(localAssignment.AssignmentId);
                var answer = Question1.parseAnswer(assignmentMTurk.Assignment.Answer);

                this.speechSynthesizer.Speak("playing");

                var url = MTurkUtils.GetURLFromFileName(answer.filename);
                new Task(() => { this.playAudio(url); }).Start();                
            }

            if (e.Result.Text == "ask for help")
            {
                this.speechSynthesizer.Speak("say one instrument that you want samples and let us ask the crowd for it!");
            }

            if (e.Result.Text == "stop")
            {
                this.play = false;
            }

        }

        private void setupGrammar()
        {
            // Set what commands should be listened to
            Choices commands = new Choices();
            foreach (var instrument in MTurkUtils.AvailableInstruments)
            {
                commands.Add("ask for sample of " + instrument.ToLower());
            }
            foreach (var assignment in this.Question1Manager.db.Assignments.Include(x => x.HIT).Where(x => x.IsValid && x.Evaluated))
            {
                commands.Add("play sample " + assignment.Id);
            }

            commands.Add("stop");
            commands.Add("ask for help");
            commands.Add("show my samples");

            // Set grammar configuration and commands
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = this.Culture;
            gb.Append(commands);

            // Create the actual Grammar instance, and then load it into the speech recognizer.
            Grammar g = new Grammar(gb);
            this.speechRecoEngine.LoadGrammar(g);
        }

        private void playAudio(string url)
        {
            using (var mf = new MediaFoundationReader(url))
            using (var wo = new WasapiOut())
            {
                wo.Init(mf);
                this.play = true;
                wo.Play();

                while (wo.PlaybackState == PlaybackState.Playing)
                {
                    if (!this.play)
                    {
                        wo.Stop();
                        break;
                    }
                    Thread.Sleep(1000);
                }

            }
        }
    }
}
