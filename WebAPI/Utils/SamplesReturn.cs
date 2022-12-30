using Microsoft.AspNetCore.Mvc;
using ProjetoCrowdsourcing;
using ProjetoCrowdsourcing.Models;

namespace MusicCrowdWebAPI
{
    public class SamplesReturn
    {
        public Question1Response assignmentResponse { get; set; }
        public string fileUrl { get; set; }
        public string instrument { get; set; }
        public int HITId { get; set; }
        public string MTurkHITId { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
