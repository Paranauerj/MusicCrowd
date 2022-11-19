using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing.Models
{
    public class Assignment
    {
        [Key]
        public int Id { get; set; } 
        
        [Required]
        public string AssignmentId { get; set; }

        [Required]
        public bool Evaluated { get; set; } = false;

        [Required]
        public bool IsValid { get; set; } = false;

        /// <summary>
        ///     HITId é o ID do HIT respondido por este assignment
        /// </summary>
        [ForeignKey("HIT")]
        public int HITId { get; set; }
        public HIT HIT { get; set; }

        /// <summary>
        ///     ValidationHIT é o HIT gerado que validará este assignment
        /// </summary>
        public ValidationHIT ValidationHIT { get; set; }

    }
}
