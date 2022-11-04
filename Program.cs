using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.MTurk;
using Amazon.MTurk.Model;

namespace ProjetoCrowdsourcing
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            /* string awsAccessKeyId = "AKIA554JUFFVH4OWVT77";
             string awsSecretAccessKey = "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"; */

            string awsAccessKeyId = ConfigurationManager.AppSettings["AwsAccessKeyId"];
            string awsSecretAccessKey = ConfigurationManager.AppSettings["AwsSecretAccessKey"];

            string SANDBOX_URL = "https://mturk-requester-sandbox.us-east-1.amazonaws.com";
            string PROD_URL = "https://mturk-requester.us-east-1.amazonaws.com";

            AmazonMTurkConfig config = new AmazonMTurkConfig();
            config.ServiceURL = SANDBOX_URL;

            AmazonMTurkClient mturkClient = new AmazonMTurkClient(awsAccessKeyId, awsSecretAccessKey, config);
            GetAccountBalanceRequest request = new GetAccountBalanceRequest();

            GetAccountBalanceResponse balance = await mturkClient.GetAccountBalanceAsync(request);
            Console.WriteLine("Your account balance is $" + balance.AvailableBalance);

            // ---------------------- AULA 2 ---------------------------------

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
            GetAnswersOfHIT(mturkClient, "3ZZAYRN1JJC5HKA7MQGDBZBC8PVTO9");

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
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
