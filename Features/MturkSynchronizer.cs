using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing
{
    public delegate void NotifyAssignment(Models.HIT localHIT, Amazon.MTurk.Model.Assignment assignment, Amazon.MTurk.Model.CreateHITResponse validationHIT);
    public delegate void NotifyValidation(Models.ValidationHIT localValidationHIT);

    class MturkSynchronizer
    {
        private MTurkConnector mTurkConnector { get; set; }
        private Validation1 validation1 { get; set; }

        public event NotifyAssignment NewAssignmentEvent;
        public event NotifyValidation NewValidationEvent;


        public MturkSynchronizer(MTurkConnector _mTurkConnector, Validation1 _validation1)
        {
            this.mTurkConnector = _mTurkConnector;
            this.validation1 = _validation1;
        }

        public async Task RunAsync(int secondsToSync)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(secondsToSync));

            while (await timer.WaitForNextTickAsync())
            {
                this.SynchronizeAssignments();
                this.SynchronizeValidations();
            }
        }

        private void SynchronizeAssignments()
        {
            var localHITs = mTurkConnector.db.HITs.Include(x => x.Assignments);
            foreach(var localHIT in localHITs)
            {
                var assignmentsFromHITMTurk = this.mTurkConnector.GetHITAssignments(localHIT.HITId);

                if (assignmentsFromHITMTurk.Count == localHIT.Assignments.Count)
                {
                    continue;
                }

                foreach (var assignment in assignmentsFromHITMTurk)
                {
                    var assignmentExistsLocally = this.mTurkConnector.db.Assignments.Where(x => x.AssignmentId == assignment.AssignmentId).Any();
                    if (assignmentExistsLocally)
                    {
                        continue;
                    }

                    var newAssignment = new Models.Assignment
                    {
                        AssignmentId = assignment.AssignmentId,
                        HITId = localHIT.Id,
                        IsValid = Question1.IsValid(assignment.Answer)
                    };

                    this.mTurkConnector.db.Assignments.Add(newAssignment);
                    this.mTurkConnector.db.SaveChanges();

                    // Se a resposta for inválida, não cria hit de validação nem invoca evento
                    if (!newAssignment.IsValid)
                    {
                        continue;
                    }
                  
                    // Cria o hit de validação para a resposta
                    var assignmentAnswer = Question1.parseAnswer(assignment.Answer);
                    var newValidationHIT = this.validation1.CreateHIT(newAssignment.Id, assignmentAnswer.filename, assignmentAnswer.creator);

                    NewAssignmentEvent.Invoke(localHIT, assignment, newValidationHIT);

                }
            }
        }

        private void SynchronizeValidations()
        {
            var notEvaluatedLocalValidationHITs = mTurkConnector.db.ValidationHITs.Include(x => x.Assignment).Where(x => x.Assignment.IsValid == true && x.Assignment.Evaluated == false);
            foreach (var notEvaluatedLocalValidationHIT in notEvaluatedLocalValidationHITs)
            {
                var assignmentsFromValidationHITMTurk = this.mTurkConnector.GetHITAssignments(notEvaluatedLocalValidationHIT.HITId);
                if(assignmentsFromValidationHITMTurk.Count == 0)
                {
                    continue;
                }

                var updatingAssignment = this.mTurkConnector.db.Assignments.Find(notEvaluatedLocalValidationHIT.AssignmentId);

                updatingAssignment.Evaluated = true;
                updatingAssignment.IsValid = Validation1.IsValid(assignmentsFromValidationHITMTurk.First().Answer, updatingAssignment.HIT.Instrument);

                this.mTurkConnector.db.Assignments.Update(updatingAssignment);
                this.mTurkConnector.db.SaveChanges();

                // Se não for válido, não invoca o evento nem dá bonificação
                if (!updatingAssignment.IsValid)
                {
                    continue;
                }

                var originalAssignmentMTurk = this.mTurkConnector.GetAssignment(updatingAssignment.AssignmentId);
                this.mTurkConnector.SendBonus(originalAssignmentMTurk.Assignment, Question1.Bonus);

                NewValidationEvent.Invoke(notEvaluatedLocalValidationHIT);

            }
        }
    }
}
