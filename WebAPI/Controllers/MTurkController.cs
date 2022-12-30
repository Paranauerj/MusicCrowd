using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoCrowdsourcing;
using ProjetoCrowdsourcing.Models;

namespace MusicCrowdWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HITsController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly BaseContext _db;
        private readonly MTurkConnector _mtc;
        private readonly Question1 _q1m;

        public HITsController(ILogger<WeatherForecastController> logger)
        {
            _db = new BaseContext();
            _logger = logger;
            _mtc = new MTurkConnector(_db);
            _q1m = new Question1(_db);
        }

        [HttpGet("/hits")]
        public IEnumerable<HIT> GetHITs()
        {
            return this._db.HITs.ToArray();
        }

        [HttpGet("/hits/{hitID}")]
        public HIT GetHIT(int hitID)
        {
            return this._db.HITs.Find(hitID);
        }

        [HttpPost("/hits/")]
        public HIT CreateHIT(string instrument)
        {
            var hit = this._q1m.CreateHIT(instrument);
            return this._db.HITs.OrderBy(x => x.CreationDate).First();
        }

        [HttpGet("/hits-with-valid-answers")]
        public IEnumerable<HIT> GetHITsWithValidAnswers()
        {
            var validAssignments = this._db.Assignments.Where(a => a.IsValid && a.Evaluated);
            return this._db.HITs.Where(x => x.Assignments.Intersect(validAssignments).Any()).ToArray();
        }

        [HttpGet("/assignments")]
        public IEnumerable<Assignment> GetAssignments()
        {
            return this._db.Assignments.ToArray();
        }

        [HttpGet("/assignments/{assignmentID}")]
        public Assignment GetAssignments(int assignmentID)
        {
            return this._db.Assignments.Find(assignmentID);
        }

        [HttpGet("/validated-assignments")]
        public IEnumerable<Assignment> GetValidatedAssignments()
        {
            return this._db.Assignments.Where(x => x.IsValid && x.Evaluated).ToArray();
        }

        [HttpGet("/validation-hits")]
        public IEnumerable<ValidationHIT> GetValidationHITs()
        {
            return this._db.ValidationHITs.ToArray();
        }

        [HttpGet("/validation-hits/{validationHITId}")]
        public ValidationHIT GetValidationHIT(int validationHITId)
        {
            return this._db.ValidationHITs.Find(validationHITId);
        }

        [HttpGet("/samples")]
        public IEnumerable<SamplesReturn> GetSamples()
        {
            var answers = new List<SamplesReturn>();
            
            foreach(var assignment in this._db.Assignments.Include(x => x.HIT))
            {
                var mturkAssignment = this._mtc.GetAssignment(assignment.AssignmentId);
                var answer = Question1.parseAnswer(mturkAssignment.Assignment.Answer);

                var sample = new SamplesReturn
                {
                    assignmentResponse = answer,
                    fileUrl = MTurkUtils.GetURLFromFileName(answer.filename),
                    instrument = assignment.HIT.Instrument,
                    CreationDate = assignment.HIT.CreationDate,
                    HITId = assignment.HIT.Id,
                    MTurkHITId = assignment.HIT.HITId
                };

                answers.Add(sample);
            }

            return answers.ToArray();
        }

    }
}