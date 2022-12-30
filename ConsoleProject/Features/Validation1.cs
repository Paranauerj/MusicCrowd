using Amazon.MTurk.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ProjetoCrowdsourcing
{
    public class Validation1 : HITManager
    {
        public static string Reward = "0.10";

        public Validation1(BaseContext db) : base(db)
        {
        }

        public CreateHITResponse CreateHIT(int assignmentId, string filename, string artist)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("$$__filename__$$", filename);
            parameters.Add("$$__artist__$$", artist);

            var newHIT = this.CreateHIT("question1validation.xml", "Evaluate a song sample", "Evaluate a sample and answer a little few questions about it", Reward, 1, parameters);

            db.ValidationHITs.Add(new Models.ValidationHIT
            {
                HITId = newHIT.HIT.HITId,
                AssignmentId = assignmentId
            });

            db.SaveChanges();

            return newHIT;
        }

        public static bool IsValid(string validationAnswer, string instrument)
        {
            var response = parseAnswer(validationAnswer);

            if (response == null)
            {
                return false;
            }

            if (response.instrument.ToLower() != instrument.ToLower())
            {
                return false;
            }

            if (response.knowTheArtist.yes && response.confirmArtist.no)
            {
                return false;
            }

            if(response.knowTheArtist.no && response.confirmArtist.yes)
            {
                return false;
            }

            if(response.sampleQuality < 5)
            {
                return false;
            }

            return true;
        }

        public static Validation1Response? parseAnswer(string answer)
        {
            // Console.WriteLine(assignment.Answer);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(answer);
            var answerJSON = xmlDoc.InnerText.ToString();
            answerJSON = answerJSON.Replace("taskAnswers", "");
            answerJSON = answerJSON.Remove(0, 1);
            answerJSON = answerJSON.Remove(answerJSON.Length - 1, 1);

            try
            {
                Validation1Response deserializedProduct = JsonConvert.DeserializeObject<Validation1Response>(answerJSON);
                return deserializedProduct;
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}
