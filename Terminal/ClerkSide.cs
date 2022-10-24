using System;
using System.Text;
using Pizzayolo.Tables;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Pizzayolo.MessageBroker.Consumer;
using Pizzayolo.MessageBroker.Producer;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Pizzayolo.Terminal
{
    internal class ClerkSide
    {
        public static async Task ActivateClerkSide()
        {
            Console.WriteLine("\n=====\nClerk\n=====");

            Console.WriteLine("Choose your name :");
            string name = Console.ReadLine();
            Console.WriteLine("Choose your surname :");
            string surname = Console.ReadLine();

            Clerk clerk = new Clerk(name, surname);
            clerk.SendSupervisionAddClerk();

            bool chose = true;

            while (chose)
            {
                Func<Clerk, uint, Task> ClerksReceive = (Clerk clerk, uint idOrder) =>
                {
                    return Task.Run(async () =>
                    {
                        Client clientReceived = clerk.ReceiveCommand<Client>();

                        await Task.Delay(0);

                        Console.WriteLine(
                            "\nThe clerk : " + clerk.firstName + " " + clerk.lastName + " Receive the folowing Client: " + clientReceived.firstName + " " + clientReceived.lastName + 
                            "\nVerify first order of client ... \n" +
                            (!clerk.VerifyFirstOrderClient(clientReceived) ?
                            "\nNo order yet, setting first order of client " + clientReceived.dateFirstOrder.ToString("dd-MM-yyyy HH:mm:ss") + "...\n" :
                            "\n It's already a client since " + clientReceived.dateFirstOrder.ToString("dd-MM-yyyy HH:mm:ss") + "\n")
                        );


                        clerk.CreateOrder(
                            idOrder,
                            DateTime.Now,
                            clientReceived,
                            clerk,
                            clientReceived.orderItems
                        );

                        Console.WriteLine("\nThe Clerk " + clerk.firstName + " " + clerk.lastName + " is preparing the order : " + clerk.processedOrder);

                        await Task.Delay(0);

                        clerk.SendCommand();
                        clerk.SendSupervisionNewOrderClerk();
                    });
                };

                var ManageTwoClientClerk = new List<Task>()
                {
                    ClerksReceive(clerk,1),
                    ClerksReceive(clerk,2)
                };

                Task.WaitAll(ManageTwoClientClerk.ToArray());

                ManageTwoClientClerk.Clear();

                Thread.Sleep(5000);

                ManageTwoClientClerk = new List<Task>()
                {
                    ClerksReceive(clerk,3),
                    ClerksReceive(clerk,4)
                };

                await Task.WhenAll(ManageTwoClientClerk.ToArray());

            }
        }
    }
}
