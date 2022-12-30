using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing
{
    public class Validation1Response
    {
        public string assignmentId { get; set; }
        public TernaryChoice confirmArtist { get; set; }
        public string instrument { get; set; }
        public BinaryChoice knowTheArtist { get;set; }
        public int sampleQuality { get; set; }
    }
}
