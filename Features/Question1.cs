using Amazon.MTurk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var newHIT = this.CreateHIT("question1.xml", "Titulo de teste", "Descricao de teste", "0.50", parameters);

            db.HITs.Add(new Models.HIT
            {
                HITId = newHIT.HIT.HITId
            });

            db.SaveChanges();

            // Link gerado para o HIT
            // Console.WriteLine(this.GetURLFromHIT(newHIT.HIT.HITTypeId));

            return newHIT;
        }
    }
}
