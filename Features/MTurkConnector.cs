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
    // Its possible to create more methods of MTurk through mturkClient requests.
    public class MTurkConnector
    {
        private string awsAccessKeyId { get; } = ConfigurationManager.AppSettings["AwsAccessKeyId"];
        private string awsSecretAccessKey { get; } = ConfigurationManager.AppSettings["AwsSecretAccessKey"];
        private string SANDBOX_URL { get; } = "https://mturk-requester-sandbox.us-east-1.amazonaws.com";
        private string PROD_URL { get; } = "https://mturk-requester.us-east-1.amazonaws.com";
        public string url { get; }
        public AmazonMTurkClient mturkClient { get; }

        public MTurkConnector()
        {
            this.url = this.SANDBOX_URL;

            AmazonMTurkConfig config = new AmazonMTurkConfig();
            config.ServiceURL = this.url;

            this.mturkClient = new AmazonMTurkClient(this.awsAccessKeyId, this.awsSecretAccessKey, config);
        }

        public GetAccountBalanceResponse GetBalance()
        {
            GetAccountBalanceRequest request = new GetAccountBalanceRequest();
            GetAccountBalanceResponse balance = this.mturkClient.GetAccountBalance(request);
            return balance;
        }

        public CreateHITResponse CreateQuestionOneHIT()
        {
            var newHIT = this.CreateHIT("question1.xml", "Titulo de teste", "Descricao de teste", "0.50");

            // Link gerado para o HIT
            // Console.WriteLine(this.GetURLFromHIT(newHIT.HIT.HITTypeId));

            return newHIT;
        }

        public string GetURLFromHIT(string HITTypeId)
        {
            return "https://workersandbox.mturk.com/projects/" + HITTypeId + "/tasks";
        }

        private CreateHITResponse CreateHIT(string filename, string title, string description, string reward)
        {
            string questionXML = System.IO.File.ReadAllText(Environment.CurrentDirectory + @"..\..\..\Questions\" + filename);

            CreateHITRequest hitRequest = new CreateHITRequest();

            hitRequest.Title = title;
            hitRequest.Description = description;
            hitRequest.Reward = reward;
            hitRequest.AssignmentDurationInSeconds = 60 * 1; // 1 minuto para o worker completar o HIT
            hitRequest.LifetimeInSeconds = 60 * 60 * 24; // 1 dia para ficar indisponível
            hitRequest.AutoApprovalDelayInSeconds = 60 * 1; // 1 minuto pra auto approval
            hitRequest.Question = questionXML;
            hitRequest.Keywords = "music,fl studio,accessibility";

            CreateHITResponse hit = mturkClient.CreateHIT(hitRequest);

            return hit;
        }

        public GetHITResponse GetHITDetails(string HITId)
        {
            GetHITRequest getHitReq = new GetHITRequest();
            getHitReq.HITId = HITId;
            return mturkClient.GetHIT(getHitReq);
        }

        public List<Assignment> GetHITAssignments(string HITId)
        {
            ListAssignmentsForHITRequest listRequest = new ListAssignmentsForHITRequest();
            listRequest.HITId = HITId;

            // Podia ser ListAssignmentsForHITAsync(listRequest).Results;
            ListAssignmentsForHITResponse listResponse = mturkClient.ListAssignmentsForHIT(listRequest);
            List<Assignment> listaDeRespostas = listResponse.Assignments;

            return listaDeRespostas;
        }
    }
}
