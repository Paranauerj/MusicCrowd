using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing
{
    public class TextMenu : Menu
    {
        public TextMenu(Question1 _question1Manager) : base(_question1Manager)
        {

        }

        public override void StartMenu()
        {
            menu.Run();

            while (true)
            {
                var input = Console.ReadLine();
                if (input == "1")
                {
                    this.menu.RenderAskForSample();
                    var selectedInstrument = Console.ReadLine();
                    if (selectedInstrument == null) { this.Say("Selecione um input valido"); continue; }
                    if (selectedInstrument == "back") { this.menu.RenderMain(); continue; }
                    this.AskSampleOfMenu(selectedInstrument);
                }
                if (input == "2")
                {
                    this.ShowMySamplesMenu(false);
                    var sampleID = Console.ReadLine();
                    if (sampleID == null) { this.Say("Selecione um sample valido"); continue; }
                    if(sampleID == "back") { this.menu.RenderMain(); continue; }
                    this.PlaySample(sampleID);
                }
                if (input == "4")
                {
                    this.menu.RenderMain();
                }
                if (input == "back")
                {
                    this.menu.RenderMain();
                }
                if (input == "stop")
                {
                    this.play = false;
                }
                if (input == "5")
                {
                    this.Quit();
                }
            }
        }

        protected override void Say(string s)
        {
            Console.WriteLine(s);
        }
    }
}
