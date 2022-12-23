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
using Microsoft.EntityFrameworkCore;
using System.Xml;
using Newtonsoft.Json;

namespace ProjetoCrowdsourcing
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Ótimo tutorial
            // https://docs.aws.amazon.com/sdk-for-javascript/v2/developer-guide/s3-example-photo-album.html

            var db = new BaseContext();

            var speechInteraction = new SpeechInteraction();
            var mturkConnector = new MTurkConnector(db);

            /*speechInteraction.speechSynthesizer.Speak("Starting project");

            // Recognizes multiple commands. Without the argument, just recognizes one
            speechInteraction.speechRecoEngine.RecognizeAsync(RecognizeMode.Multiple);
            Console.WriteLine("Starting to listen commands...");

            // Keep program listening for commands
            Console.WriteLine("Press any key to stop voice reco.");
            Console.ReadKey();

            // Stops recognition
            */

            /*var question1Manager = new Question1(db);
            var novoHIT = question1Manager.CreateHIT("Piano");
            Console.WriteLine(MTurkUtils.GetURLFromHIT(novoHIT.HIT.HITTypeId));*/


            var latestHIT = db.HITs.OrderByDescending(x => x.CreationDate).First();
            var assignmentsFromLatestHIT = mturkConnector.GetHITAssignments(latestHIT.HITId);

            foreach (var assignment in assignmentsFromLatestHIT)
            {
                // Console.WriteLine(assignment.Answer);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(assignment.Answer);
                var answerJSON = xmlDoc.InnerText.ToString();
                answerJSON = answerJSON.Replace("taskAnswers", "");
                answerJSON = answerJSON.Remove(0, 1);
                answerJSON = answerJSON.Remove(answerJSON.Length - 1, 1);

                try
                {
                    Question1Response deserializedProduct = JsonConvert.DeserializeObject<Question1Response>(answerJSON);
                    Console.WriteLine(MTurkUtils.GetURLFromFileName(deserializedProduct.filename));
                }
                catch (Exception e)
                {

                }
            }



            

            /*
             * TODO:
             * Após gerar o HIT, criar task paralela que vai pesquisar respostas ao HIT a cada n segundos
             * Quando achar uma resposta, cria HIT para validá-la e cria task paralela para procurar resposta a cada n segundos
             * Quando achar resposta, caso seja válida, marcar a resposta original como válida e apresentar ao utilizador.
             */

        }
    }
}
