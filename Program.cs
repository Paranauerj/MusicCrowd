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
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var db = new BaseContext();

            var speechInteraction = new SpeechInteraction();
            var mturkConnector = new MTurkConnector();

            /*speechInteraction.speechSynthesizer.Speak("Starting project");

            // Recognizes multiple commands. Without the argument, just recognizes one
            speechInteraction.speechRecoEngine.RecognizeAsync(RecognizeMode.Multiple);
            Console.WriteLine("Starting to listen commands...");

            // Keep program listening for commands
            Console.WriteLine("Press any key to stop voice reco.");
            Console.ReadKey();

            // Stops recognition
            speechInteraction.speechRecoEngine.RecognizeAsyncStop();

            Console.WriteLine("Available balance: " + mturkConnector.GetBalance().AvailableBalance);

            var createdHIT = mturkConnector.CreateQuestionOneHIT();
            Console.WriteLine(mturkConnector.GetURLFromHIT(createdHIT.HIT.HITTypeId) + " ID: " + createdHIT.HIT.HITId);

            Console.WriteLine("Responde lá, pressiona enter e espera um pouquinho pro server da amazon processar");
            Console.ReadLine();
            */

            // Exemplo de HITId: 3S829FDFUFMLU4S21R3UP8KJITKXDG
            // var listaDeRespostas = mturkConnector.GetHITAssignments(createdHIT.HIT.HITId);

            db.HITs.Add(new Models.HIT
            {
                HITId = "3S829FDFUFMLU4S21R3UP8KJITKXDG"
            });

            //db.SaveChanges();

            var listaDeRespostas = mturkConnector.GetHITAssignments("3S829FDFUFMLU4S21R3UP8KJITKXDG");
            Console.WriteLine(listaDeRespostas[0].Answer);

            // TPC => Implementar XML Parser para tratar dos dados
            // GetAnswersOfHIT(mturkClient, "3ZZAYRN1JJC5HKA7MQGDBZBC8PVTO9");

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to stop program");
            Console.ReadKey();

        }
    }
}
