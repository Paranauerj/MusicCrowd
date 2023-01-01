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
    public class SpeechInteractionMenu : Menu
    {
        private CultureInfo Culture { get; }
        public SpeechRecognitionEngine speechRecoEngine { get; }
        public SpeechSynthesizer speechSynthesizer { get; }

        public SpeechInteractionMenu(Question1 _question1Manager) : base(_question1Manager)
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new Exception();
            }

            Culture = new CultureInfo("en-US");
            speechRecoEngine = new SpeechRecognitionEngine(Culture);
            speechSynthesizer = new SpeechSynthesizer();

            // Synthesizer configuration
            speechSynthesizer.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult, 0, Culture);
            setupGrammar();

            // Triggers function when command is recognized - maybe put outside this class
            speechRecoEngine.SpeechRecognized += SpeechRecoEngine_SpeechRecognized;
            speechRecoEngine.SetInputToDefaultAudioDevice();
        }

        protected override void Say(string s)
        {
            speechSynthesizer.Speak(s);
        }

        /// <summary>
        ///  Triggered when a command is recognized - maybe also put outside this class
        /// </summary>
        private void SpeechRecoEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Console.WriteLine("\nSpeech Recognized: \t{0}" + e.Result.Confidence, e.Result.Text);

            Console.WriteLine(e.Result.Text + " - " + e.Result.Confidence);

            if (e.Result.Confidence < 0.75)
                return;

            var result = e.Result.Text;
            this.HandleInput(result);
        }

        protected void HandleInput(string input)
        {
            if (input.Contains("ask sample of"))
            {
                var instrument = input.Replace("ask sample of", "");
                instrument = instrument.Trim();

                this.AskSampleOfMenu(instrument);
            }

            if (input == "show my samples")
            {
                this.ShowMySamplesMenu(true);
            }

            if (input.Contains("play sample"))
            {
                var sampleID = input.Replace("play sample", "");
                sampleID = sampleID.Trim();

                this.PlaySample(sampleID);
            }

            if (input == "ask for help")
            {
                this.AskForHelpMenu();
            }

            if (input == "stop")
            {
                play = false;
            }

            if (input == "back")
            {
                menu.RenderMain();
            }
            if (input == "quit")
            {
                this.Quit();
            }
        }

        private void setupGrammar()
        {

            if (!OperatingSystem.IsWindows())
            {
                throw new Exception();
            }

            // Set what commands should be listened to
            Choices commands = new Choices();
            foreach (var instrument in MTurkUtils.AvailableInstruments)
            {
                commands.Add("ask sample of " + instrument.ToLower());
            }
            foreach (var assignment in Question1Manager.db.Assignments.Include(x => x.HIT).Where(x => x.IsValid && x.Evaluated))
            {
                commands.Add("play sample " + assignment.Id);
            }

            commands.Add("stop");
            commands.Add("back");
            commands.Add("quit");
            commands.Add("ask for help");
            commands.Add("show my samples");

            // Set grammar configuration and commands
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = Culture;
            gb.Append(commands);

            // Create the actual Grammar instance, and then load it into the speech recognizer.
            Grammar g = new Grammar(gb);
            speechRecoEngine.LoadGrammar(g);
        }

        public override void StartMenu()
        {
            menu.Run();
            speechRecoEngine.RecognizeAsync(RecognizeMode.Multiple);

            Console.WriteLine("Press any key to stop voice reco.");
            Console.ReadKey();
        }
    }
}
