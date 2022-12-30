using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing.Models
{
    public class HIT
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public string HITId { get; set; }

        [Required]
        public string Instrument { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public ICollection<Assignment> Assignments { get; set; }
    }
}
