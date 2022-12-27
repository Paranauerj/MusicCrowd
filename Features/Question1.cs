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
        public Question1(BaseContext db) : base(db)
        {

        }

        public CreateHITResponse CreateHIT(string instrument)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("$$__instrument__$$", instrument);

            var newHIT = this.CreateHIT("question1.xml", "Titulo de teste", "Descricao de teste", "0.50", 20, parameters);

            db.HITs.Add(new Models.HIT
            {
                HITId = newHIT.HIT.HITId
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

            if(response.yearsOfExperience == "-1")
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
