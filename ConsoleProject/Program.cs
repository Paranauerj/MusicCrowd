﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.MTurk;
using Amazon.MTurk.Model;
using System.Speech.Recognition;
using System.Globalization;
using System.Speech.Synthesis;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using Newtonsoft.Json;

namespace ProjetoCrowdsourcing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Ótimo tutorial
            // https://docs.aws.amazon.com/sdk-for-javascript/v2/developer-guide/s3-example-photo-album.html

            var db = new BaseContext();
            var mturkConnector = new MTurkConnector(db);
            var question1Manager = new Question1(db);
            var validation1Manager = new Validation1(db);
            // var speechInteractionMenu = new SpeechInteractionMenu(question1Manager);
            var mturkSync = new MturkSynchronizer(mturkConnector, validation1Manager);

            mturkSync.NewAssignmentEvent += MturkSync_NewAssignmentEvent;
            mturkSync.NewValidationEvent += MturkSync_NewValidationEvent;

            _ = mturkSync.RunAsync(10);

            // Running Menu
            Menu menu = null;
            int input = 0;

            while(input != 1 && input != 2)
            {
                Console.WriteLine("Qual tipo de interface prefere?\n1- Voz\n2- Texto");
                input = Convert.ToInt32(Console.ReadLine());

                if(input == 1)
                {
                    menu = new SpeechInteractionMenu(question1Manager);
                }
                if (input == 2)
                {
                    menu = new TextMenu(question1Manager);
                }
            }
            
            menu.StartMenu();

            // Keep program listening for commands
            Console.WriteLine("Press any key to stop voice reco.");
            Console.ReadKey();
            
            /*var novoHIT = question1Manager.CreateHIT("classic guitar");
            Console.WriteLine(MTurkUtils.GetURLFromHIT(novoHIT.HIT.HITTypeId));*/

            Console.WriteLine("Press any key to stop voice reco.");
            Console.ReadKey();

        }

        private static void MturkSync_NewAssignmentEvent(Models.HIT localHIT, Assignment assignment, CreateHITResponse validationHIT)
        {
            //Console.WriteLine("New Assignment Received! Local HIT ID: " + localHIT.Id + " | mturk HIT ID: " + localHIT.HITId + " | mturk assignment ID: " + assignment.HITId);
            Console.WriteLine("New Assignment Received!");
            Console.WriteLine("------ Validation HIT: " + MTurkUtils.GetURLFromHIT(validationHIT.HIT.HITTypeId));
        }

        private static void MturkSync_NewValidationEvent(Models.ValidationHIT localValidationHIT)
        {
            Console.WriteLine("New Validation Received!");
        }

    }
}
