using Microsoft.EntityFrameworkCore;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing
{
    public abstract class Menu
    {
        public Question1 Question1Manager { get; set; }
        protected MenuRender menu { get; set; }
        protected bool play { get; set; }

        public Menu(Question1 _question1Manager)
        {
            Question1Manager = _question1Manager;
            menu = new MenuRender();
        }

        protected abstract void Say(string s);

        public abstract void StartMenu();

        public void Quit()
        {
            Environment.Exit(0);
        }

        protected void AskForHelpMenu()
        {
            this.Say("These are you commands");
            this.Say("1- Ask sample of {instrument}");
            this.Say("2- Show my samples");
            this.Say("3- Play sample {sampleID}");
            this.Say("4- Back");
            this.Say("5- Quit");
        }

        protected void AskSampleOfMenu(string instrument)
        {
            if (!MTurkUtils.AvailableInstruments.Contains(instrument))
            {
                this.Say("we do not have this instrument available yet!");
                menu.RenderAskForSample();
                return;
            }

            this.Say("asking the crowd");
            var createdHIT = Question1Manager.CreateHIT(instrument);
            Console.WriteLine(MTurkUtils.GetURLFromHIT(createdHIT.HIT.HITTypeId));
        }

        protected void ShowMySamplesMenu(bool say)
        {
            menu.RenderShowMySamples();
            foreach (var assignment in Question1Manager.db.Assignments.Include(x => x.HIT).Where(x => x.IsValid && x.Evaluated))
            {
                Console.WriteLine("assignment " + assignment.Id + " - " + assignment.HIT.Instrument);
                if (say)
                {
                    this.Say("assignment " + assignment.Id + " - " + assignment.HIT.Instrument);
                }
            }
        }

        protected void PlaySample(string sampleID)
        {
            var localAssignment = Question1Manager.db.Assignments.Include(x => x.HIT).Where(x => x.IsValid && x.Evaluated && x.Id.ToString() == sampleID).First();
            if (localAssignment == null)
            {
                this.Say("this assignment is not available");
            }

            var assignmentMTurk = Question1Manager.GetAssignment(localAssignment.AssignmentId);
            var answer = Question1.parseAnswer(assignmentMTurk.Assignment.Answer);

            this.Say("playing");

            var url = MTurkUtils.GetURLFromFileName(answer.filename);
            new Task(() => { playAudio(url); }).Start();
        }

        protected void playAudio(string url)
        {
            using (var mf = new MediaFoundationReader(url))
            using (var wo = new WasapiOut())
            {
                wo.Init(mf);
                play = true;
                wo.Play();

                while (wo.PlaybackState == PlaybackState.Playing)
                {
                    if (!play)
                    {
                        wo.Stop();
                        break;
                    }
                    Thread.Sleep(1000);
                }
            }
        }

    }
}
