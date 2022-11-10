using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Globalization;
using System.Speech.Synthesis;

namespace ProjetoCrowdsourcing
{
    public class SpeechInteraction
    {
        private CultureInfo Culture { get; }
        public SpeechRecognitionEngine speechRecoEngine { get; }
        public SpeechSynthesizer speechSynthesizer { get; }

        public SpeechInteraction()
        {
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

        /// <summary>
        ///  Triggered when a command is recognized - maybe also put outside this class
        /// </summary>
        private void SpeechRecoEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("\nSpeech Recognized: \t{0}" + e.Result.Confidence, e.Result.Text);

            if (e.Result.Confidence < 0.85)
                return;

            switch (e.Result.Text)
            {
                case "ask for sample":
                    this.speechSynthesizer.Speak("asking crowd for sample");
                    break;
                case "ask for help":
                    this.speechSynthesizer.Speak("asking crowd for help");
                    break;
                default:
                    this.speechSynthesizer.Speak("Command not implemented yet");
                    break;
            }
        }

        private void setupGrammar()
        {
            // Set what commands should be listened to
            Choices commands = new Choices();
            commands.Add("ask for sample");
            commands.Add("ask for help");

            // Set grammar configuration and commands
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = this.Culture;
            gb.Append(commands);

            // Create the actual Grammar instance, and then load it into the speech recognizer.
            Grammar g = new Grammar(gb);
            this.speechRecoEngine.LoadGrammar(g);
        }
    }
}
