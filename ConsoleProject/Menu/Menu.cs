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
            //this.RenderAskForSample();
        }

        public void RenderMain()
        {
            Console.Clear();
            Console.WriteLine("------ Music Crowd ---------");
            Console.WriteLine("Interaja através da interface de voz!\n");
            Console.WriteLine("Os comandos disponíveis são:");
            Console.WriteLine("Ask sample of {instrument}");
            Console.WriteLine("Show my samples");
            Console.WriteLine("Play sample {sampleID}");
            Console.WriteLine("Back");
        }

        public void RenderAskForSample()
        {
            Console.Clear();
            Console.WriteLine("------ Music Crowd ---------");
            Console.WriteLine("Peça um dos seguintes instrumentos através do comando: Ask sample of {instrument}");
            foreach(var instrument in MTurkUtils.AvailableInstruments)
            {
                Console.WriteLine(instrument);
            }
            Console.WriteLine("\nBack");
        }

        public void RenderShowMySamples()
        {
            Console.Clear();
            Console.WriteLine("------ Music Crowd ---------");
            Console.WriteLine("My available samples\n");
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
