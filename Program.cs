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
            speechInteraction.speechRecoEngine.RecognizeAsyncStop();

            Console.WriteLine("Available balance: " + mturkConnector.GetBalance().AvailableBalance);

            var createdHIT = mturkConnector.CreateQuestionOneHIT();
            Console.WriteLine(mturkConnector.GetURLFromHIT(createdHIT.HIT.HITTypeId) + " ID: " + createdHIT.HIT.HITId);

            Console.WriteLine("Responde lá, pressiona enter e espera um pouquinho pro server da amazon processar");
            Console.ReadLine();
            */

            // Exemplo de HITId: 3S829FDFUFMLU4S21R3UP8KJITKXDG
            // var listaDeRespostas = mturkConnector.GetHITAssignments(createdHIT.HIT.HITId);

            /*db.HITs.Add(new Models.HIT
            {
                HITId = "3S829FDFUFMLU4S21R3UP8KJITKXDG"
            });

            db.SaveChanges();

            var assigId = mturkConnector.GetHITAssignments("3S829FDFUFMLU4S21R3UP8KJITKXDG")[0].AssignmentId;
            db.Assignments.Add(new Models.Assignment
            {
                AssignmentId = assigId,
                HITId = 1
            });

            db.SaveChanges();

            db.ValidationHITs.Add(new Models.ValidationHIT
            {
                AssignmentId = 1,
                HITId = "any"
            });

            db.SaveChanges();*/

            /*var counted = db.HITs.Include(x => x.Assignments).Where(h => h.HITId == "3S829FDFUFMLU4S21R3UP8KJITKXDG").Count();
            Console.WriteLine(counted);*/

            /*var listaDeRespostas = mturkConnector.GetHITAssignments("3S829FDFUFMLU4S21R3UP8KJITKXDG");
            Console.WriteLine(listaDeRespostas[0].Answer);*/

            // Console.WriteLine(mturkConnector.GetURLFromHIT(mturkConnector.GetHITDetails("3S829FDFUFMLU4S21R3UP8KJITKXDG").HIT.HITTypeId));

            /*var hit = mturkConnector.CreateQuestionOneHIT("Electric Guitar");
            Console.WriteLine(MTurkConnector.GetURLFromHIT(hit.HIT.HITTypeId));*/

            var hitsStored = db.HITs.OrderBy(x => x.CreationDate);
            // var assignmentsFromLatestHIT = mturkConnector.GetHITAssignments(latestHIT.HITId);
            // var assignmentsFromLatestHIT = mturkConnector.GetHITAssignments("3NI0WFPPJM1EC57COS69AONGR9P60X");
            /*foreach (var hit in hitsStored)
            {
                var assignments = mturkConnector.GetHITAssignments(hit.HITId);
                Console.WriteLine(hit.HITId + "(" + hit.CreationDate.ToString() + "): " + assignments.Count);
                foreach(var assignment in assignments)
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
                        Console.WriteLine(MTurkConnector.GetURLFromFileName(deserializedProduct.filename));
                    }
                    catch (Exception e)
                    {

                    }



                }
                Console.WriteLine("--------------------------------------");
            }*/

            /*var hit = mturkConnector.CreateQuestionOneValidationHIT("Perfect - Ed Sheeran.mp3", "Ed Sheeran");
            Console.WriteLine(MTurkConnector.GetURLFromHIT(hit.HIT.HITTypeId));*/


            /*
             * TODO:
             * Após gerar o HIT, criar task paralela que vai pesquisar respostas ao HIT a cada n segundos
             * Quando achar uma resposta, cria HIT para validá-la e cria task paralela para procurar resposta a cada n segundos
             * Quando achar resposta, caso seja válida, marcar a resposta original como válida e apresentar ao utilizador.
             */

            // TPC => Implementar XML Parser para tratar dos dados

            // Keep the console window open in debug mode.
            /*Console.WriteLine("Press any key to stop program");
            Console.ReadKey();*/

        }
    }
}
