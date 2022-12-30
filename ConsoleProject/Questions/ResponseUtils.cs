using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing
{
    public class BinaryChoice
    {
        public bool yes { get; set; }
        public bool no { get; set; }
    }

    public class TernaryChoice : BinaryChoice
    {
        public bool dontKnow { get; set; }
    }
}
