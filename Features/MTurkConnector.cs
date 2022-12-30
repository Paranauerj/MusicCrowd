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
        private string awsAccessKeyId { get; } = "AKIA554JUFFVH4OWVT77";
        private string awsSecretAccessKey { get; } = "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";
        private string SANDBOX_URL { get; } = "https://mturk-requester-sandbox.us-east-1.amazonaws.com";
        private string PROD_URL { get; } = "https://mturk-requester.us-east-1.amazonaws.com";
        public string url { get; }
        public AmazonMTurkClient mturkClient { get; set; }
        public BaseContext db { get; set; }

        public MTurkConnector(BaseContext db)
        {
            this.db = db;
            this.url = this.SANDBOX_URL;

            AmazonMTurkConfig config = new AmazonMTurkConfig();
            config.ServiceURL = this.url;

            this.mturkClient = new AmazonMTurkClient(this.awsAccessKeyId, this.awsSecretAccessKey, config);
        }

        public GetHITResponse GetHITDetails(string HITId)
        {
            GetHITRequest getHitReq = new GetHITRequest();
            getHitReq.HITId = HITId;
            return mturkClient.GetHITAsync(getHitReq).Result;
        }

        public List<Assignment> GetHITAssignments(string HITId)
        {
            ListAssignmentsForHITRequest listRequest = new ListAssignmentsForHITRequest();
            listRequest.HITId = HITId;

            // Podia ser ListAssignmentsForHITAsync(listRequest).Results;
            ListAssignmentsForHITResponse listResponse = mturkClient.ListAssignmentsForHITAsync(listRequest).Result;
            List<Assignment> listaDeRespostas = listResponse.Assignments;

            return listaDeRespostas;
        }

        public GetAssignmentResponse GetAssignment(string assignmentID)
        {
            GetAssignmentRequest getAssignmentReq = new GetAssignmentRequest();
            getAssignmentReq.AssignmentId = assignmentID;

            return mturkClient.GetAssignmentAsync(getAssignmentReq).Result;

        }

        public void SendBonus(Amazon.MTurk.Model.Assignment assignment, string bonusAmount)
        {
            SendBonusRequest sendBonusRequest = new SendBonusRequest();

            sendBonusRequest.AssignmentId = assignment.AssignmentId;
            sendBonusRequest.WorkerId = assignment.WorkerId;
            sendBonusRequest.Reason = "Good job on uploading a music sample";
            sendBonusRequest.BonusAmount = bonusAmount;

            _ = mturkClient.SendBonusAsync(sendBonusRequest).Result;
        }
    }
}
