// 

using System;
using System.Text;
using Pizzayolo.Tables;
using Pizzayolo.Terminal;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Pizzayolo.MessageBroker.Consumer;
using Pizzayolo.MessageBroker.Producer;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Pizzayolo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome in the service of Pizzayolo !\n");
            Console.WriteLine("To start, define who are you ?" +
                "\n 1. Client" +
                "\n 2. Clerk" +
                "\n 3. Kitchen" +
                "\n 4. DeliveryMan" +
                "\nTo quit tap 0.");

            bool stop = false;

            while (!stop)
            {
                string chose = Console.ReadLine();

                switch (chose)
                {
                    case "1":
                        Console.Title = "Client";
                        await ClientSide.ActivateClientSide();
                        break;

                    case "2":
                        Console.Title = "Clerk";
                        await ClerkSide.ActivateClerkSide();
                        break;

                    case "3":
                        Console.Title = "Kitchen";
                        await KitchenSide.ActivateKitchenSide();
                        break;

                    case "4":
                        Console.Title = "DeliveryMan";
                        await DeliveryManSide.ActivateDeliveryManSide();
                        break;

                    case "0":
                        Console.WriteLine("Program stopped.\n");
                        stop = true;
                        break;

                    default:
                        Console.WriteLine("Retry, BAD Input.\n");
                        break;

                }
            }
        }
    }
}