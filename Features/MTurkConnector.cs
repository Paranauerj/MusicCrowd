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
    }
}
