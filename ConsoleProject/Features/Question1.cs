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
    public class Question1 : HITManager
    {
        public static string Reward = "0.70";
        public static string Bonus = "1.00";


        public Question1(BaseContext db) : base(db)
        {

        }

        public CreateHITResponse CreateHIT(string instrument)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("$$__instrument__$$", instrument);

            var newHIT = this.CreateHIT("question1.xml", "Audio Sample Upload", "Upload a instrumental sample with the required parameters", Reward, 20, parameters);

            db.HITs.Add(new Models.HIT
            {
                HITId = newHIT.HIT.HITId,
                Instrument = instrument
            });

            db.SaveChanges();

            // Link gerado para o HIT
            // Console.WriteLine(this.GetURLFromHIT(newHIT.HIT.HITTypeId));

            return newHIT;
        }

        /// <summary>
        ///  Checks if an answer is valid
        /// </summary>
        /// <param name="answer">answer string</param>
        /// <returns></returns>
        public static bool IsValid(string answer)
        {
            var response = parseAnswer(answer);
            if(response == null)
            {
                return false;
            }

            if(response.yearsOfExperience < 0)
            {
                return false;
            }

            if (response.knowTheInstrument.yes && response.yearsOfExperience == 0)
            {
                return false;
            }

            if (response.knowTheInstrument.no && response.yearsOfExperience > 0)
            {
                return false;
            }

            return true;

        }

        public static Question1Response? parseAnswer(string answer)
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
                Question1Response deserializedProduct = JsonConvert.DeserializeObject<Question1Response>(answerJSON);
                return deserializedProduct;
            }
            catch (Exception e)
            {
                return null;
            }
        }


    }
}
