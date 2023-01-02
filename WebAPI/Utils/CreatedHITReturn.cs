using Microsoft.AspNetCore.Mvc;
using ProjetoCrowdsourcing;
using ProjetoCrowdsourcing.Models;

namespace MusicCrowdWebAPI
{
    public class CreatedHITReturn
    {
        public HIT hit { get; set; }
        public string link { get;set; }

    }
}
