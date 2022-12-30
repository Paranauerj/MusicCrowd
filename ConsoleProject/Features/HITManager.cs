using Amazon.MTurk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing
{
    public abstract class HITManager : MTurkConnector
    {
        public HITManager(BaseContext db) : base(db)
        {
        }

        protected CreateHITResponse CreateHIT(string filename, string title, string description, string reward, int maxAssignments, Dictionary<string, string>? parameters)
        {
            string questionXML = "";
            try
            {
                questionXML = System.IO.File.ReadAllText(Environment.CurrentDirectory + @"..\..\..\..\Questions\" + filename);
            }
            catch
            {
                questionXML = System.IO.File.ReadAllText(Environment.CurrentDirectory + @"..\..\ConsoleProject\Questions\" + filename);
            }

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    questionXML = questionXML.Replace(parameter.Key, parameter.Value);
                }
            }

            CreateHITRequest hitRequest = new CreateHITRequest();

            hitRequest.Title = title;
            hitRequest.Description = description;
            hitRequest.Reward = reward;
            hitRequest.AssignmentDurationInSeconds = 60 * 10; // 10 minutos para o worker completar o HIT
            hitRequest.LifetimeInSeconds = 60 * 60 * 24; // 1 dia para ficar indisponível
            hitRequest.AutoApprovalDelayInSeconds = 5; // 5 segundos pra auto approval
            hitRequest.Question = questionXML;
            hitRequest.Keywords = "music,fl studio,accessibility";
            hitRequest.MaxAssignments = maxAssignments;

            CreateHITResponse hit = mturkClient.CreateHITAsync(hitRequest).Result;

            return hit;
        }
    }
}
