using Amazon.MTurk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing
{
    class MTurkUtils : MTurkConnector
    {
        public MTurkUtils(BaseContext db) : base(db)
        {

        }
        public GetAccountBalanceResponse GetBalance()
        {
            GetAccountBalanceRequest request = new GetAccountBalanceRequest();
            GetAccountBalanceResponse balance = this.mturkClient.GetAccountBalanceAsync(request).Result;
            return balance;
        }

        public static string GetURLFromHIT(string HITTypeId)
        {
            return "https://workersandbox.mturk.com/projects/" + HITTypeId + "/tasks";
        }

        public static string GetURLFromFileName(string filename)
        {
            return "https://mturk-worker-uploads-musiccrowd-utad.s3.us-east-1.amazonaws.com/" + filename;
        }

    }
}
