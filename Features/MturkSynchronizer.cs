using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing
{
    class MturkSynchronizer : MTurkConnector
    {
        public MturkSynchronizer(BaseContext db) : base(db)
        {
        }

        public async Task RunAsync()
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));

            while (await timer.WaitForNextTickAsync())
            {
                this.SynchronizeAssignments();
                this.SynchronizeValidations();
            }
        }

        private void SynchronizeAssignments()
        {
            //this.GetHITAssignments();
        }

        private void SynchronizeValidations()
        {

        }
    }
}
