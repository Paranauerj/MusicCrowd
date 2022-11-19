using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing.Models
{
    public class ValidationHIT
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HITId { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        /// <summary>
        ///     AssignmentId é o ID da resposta a ser validada
        /// </summary>
        [ForeignKey("Assignment")]
        public int AssignmentId { get; set;}
        public Assignment Assignment { get; set; }
    }
}
