using Amazon.MTurk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing
{
    public class Validation1 : HITManager
    {
        public Validation1(BaseContext db) : base(db)
        {
        }

        public CreateHITResponse CreateHIT(int assignmentId, string filename, string artist)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("$$__filename__$$", filename);
            parameters.Add("$$__artist__$$", artist);

            var newHIT = this.CreateHIT("question1validation.xml", "Evaluate a song sample", "Evaluate a sample and answer a little few questions about it", "0.10", 1, parameters);

            db.ValidationHITs.Add(new Models.ValidationHIT
            {
                HITId = newHIT.HIT.HITId,
                AssignmentId = assignmentId
            });

            db.SaveChanges();

            return newHIT;
        }

        public static bool IsValid(string answer)
        {
            return true;
        }

    }
}
