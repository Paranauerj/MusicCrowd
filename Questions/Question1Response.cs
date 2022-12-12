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
        public KnowTheInstrument knowTheInstrument { get;set; }
        public string yearsOfExperience { get; set; }
    }

    public class KnowTheInstrument
    {
        public string yes { get; set; } 
        public string no { get; set; }
    }
}
