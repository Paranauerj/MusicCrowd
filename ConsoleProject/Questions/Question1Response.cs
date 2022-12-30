using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing
{
    public class Question1Response
    {
        public string assignmentId { get; set; }
        public string creator { get; set; }
        public string filename { get; set; }
        public BinaryChoice knowTheInstrument { get;set; }
        public int yearsOfExperience { get; set; }

        public static explicit operator Question1Response(Type? v)
        {
            throw new NotImplementedException();
        }
    }
}
