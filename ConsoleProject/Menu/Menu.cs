using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing
{
    public class Menu
    {
        private bool loop = true;

        public void Run()
        {
            this.RenderMain();
            this.RenderAskForSample();
        }

        public void RenderMain()
        {
            Console.Clear();
            Console.WriteLine("------ Music Crowd ---------");
            Console.WriteLine("Interaja através da interface de voz!\n");
            Console.WriteLine("Os comandos disponíveis são:");
            Console.WriteLine("Show my samples");
            Console.WriteLine("Ask for sample");
            Console.WriteLine("Quit");
        }

        public void RenderAskForSample()
        {
            Console.WriteLine("------ Music Crowd ---------");
            Console.WriteLine("Peça um dos seguintes instrumentos");
            foreach(var instrument in MTurkUtils.AvailableInstruments)
            {
                Console.WriteLine(instrument);
            }
            Console.WriteLine("Back");
        }

        public void RenderShowMySamples()
        {
            Console.Clear();
            Console.WriteLine("------ Music Crowd ---------");
        }

        public void Quit()
        {
            this.loop = false;
        }

        public void Pause()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}
