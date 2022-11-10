using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.MTurk;
using Amazon.MTurk.Model;
using System.Speech.Recognition;
using System.Globalization;
using System.Speech.Synthesis;

namespace ProjetoCrowdsourcing
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var speechInteraction = new SpeechInteraction();
            var mturkConnector = new MTurkConnector();

            speechInteraction.speechSynthesizer.Speak("Starting project");

            // Recognizes multiple commands. Without the argument, just recognizes one
            speechInteraction.speechRecoEngine.RecognizeAsync(RecognizeMode.Multiple);
            Console.WriteLine("Starting to listen commands...");

            // Keep program listening for commands
            Console.WriteLine("Press any key to stop voice reco.");
            Console.ReadKey();

            // Stops recognition
            speechInteraction.speechRecoEngine.RecognizeAsyncStop();

            Console.WriteLine("Available balance: " + mturkConnector.GetBalance().AvailableBalance);

            // ---------------------- AULA 2 --------------------------------- => implementar na classe MTurkConnector

            // string questionXML = System.IO.File.ReadAllText(@"C:\Users\jptin\Desktop\question.xml");
            //Console.WriteLine("------");
            //Console.WriteLine(questionXML);

            /*CreateHITRequest hitRequest = new CreateHITRequest();

             hitRequest.Title = "Test Title";
             hitRequest.Description = "Test Description";
             hitRequest.Reward = "0.50";
             hitRequest.AssignmentDurationInSeconds = 60 * 1; // 1 minutos
             hitRequest.LifetimeInSeconds = 60 * 1; // 1 minuto
             hitRequest.AutoApprovalDelayInSeconds = 60 * 1; // 1 minuto pra auto approval
             hitRequest.Question = questionXML;
             CreateHITResponse hit = mturkClient.CreateHIT(hitRequest);
             // Show a link to the HIT

             Console.WriteLine(hit.HIT.HITId);
             Console.WriteLine("https://workersandbox.mturk.com/projects/" + hit.HIT.HITTypeId + "/tasks");*/

            /*GetHITRequest getHit = new GetHITRequest();
            getHit.HITId = "36FFXPMSUM9FCBJCMWZOXG8OXV0OHU";
            Console.WriteLine(mturkClient.GetHIT(getHit).HIT.AutoApprovalDelayInSeconds);*/

            /*var hitIdExample = "3ZZAYRN1JJC5HKA7MQGDBZBC8PVTO9";
            ListAssignmentsForHITRequest listRequest = new ListAssignmentsForHITRequest();
            listRequest.HITId = hitIdExample;
            ListAssignmentsForHITResponse listResponse = mturkClient.ListAssignmentsForHITAsync(listRequest).Result;
            List<Assignment> listaDeRespostas = listResponse.Assignments;

            Console.WriteLine(listaDeRespostas[0].Answer);*/

            // TPC => Implementar XML Parser para tratar dos dados
            // GetAnswersOfHIT(mturkClient, "3ZZAYRN1JJC5HKA7MQGDBZBC8PVTO9");

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to stop voice reco.");
            Console.ReadKey();

        }

        static void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            SpeechSynthesizer s = new SpeechSynthesizer();
            Console.WriteLine("\nSpeech Recognized: \t{0}" + e.Result.Confidence, e.Result.Text);

            if (e.Result.Confidence < 0.85)
                return;

            switch (e.Result.Text)
            {
                case "light on":
                    light(1);
                    s.Speak("the light has been turned on.");
                    break;
                case "light off":
                    light(0);
                    s.Speak("the light has been turned off.");
                    break;
                case "fan on":
                    fan(1);
                    s.Speak("the fan has been turned on.");
                    break;
                case "fan off":
                    fan(0);
                    s.Speak("the fan has been turned off.");
                    break;
                default:

                    break;
            }
        }
        static void light(int val)
        {
            Console.WriteLine("\nSpeech Recognized:light ");
        }

        static void fan(int val)
        {
            Console.WriteLine("\nSpeech Recognized: fan");
        }

        public static void GetAnswersOfHIT(AmazonMTurkClient mturkClient, string hitId)
        {
            ListAssignmentsForHITRequest listRequest = new ListAssignmentsForHITRequest();
            listRequest.HITId = hitId;
            ListAssignmentsForHITResponse listResponse = mturkClient.ListAssignmentsForHITAsync(listRequest).Result;
            List<Assignment> listaDeRespostas = listResponse.Assignments;

            Console.WriteLine(listaDeRespostas[0].Answer);
        }
    }
}
