using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCrowdsourcing
{
    public class MenuRender
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
            Console.WriteLine("Interaja através da interface via consola!\n");
            Console.WriteLine("Os comandos disponíveis são:");
            Console.WriteLine("1- Ask sample of {instrument}");
            Console.WriteLine("2- Show my samples");
            Console.WriteLine("3- Play sample {sampleID} (audio interface only)");
            Console.WriteLine("4- Back");
            Console.WriteLine("5- Quit");
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
            Console.WriteLine("My available samples - choose one to play\n");
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
