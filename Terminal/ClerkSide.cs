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
            Console.WriteLine("\nChoose your name:");
            string name = Console.ReadLine();
            Console.WriteLine("Choose your surname:");
            string surname = Console.ReadLine();

            Clerk clerk = new Clerk(name, surname);

            bool chose = true;

            while (chose)
            {

                Func<Clerk, uint, Task> ClerksReceive = (Clerk clerk, uint idOrder) =>
                {
                    return Task.Run(() =>
                    {
                        Client clientReceived = clerk.ReceiveCommand<Client>();

                        Console.WriteLine(
                            "\nThe clerk : " + clerk.firstName + " Receive the folowing Client" + clientReceived.firstName +
                            "\nVerify first order of client " + clientReceived.dateFirstOrder + " : \n" +
                            (!clerk.VerifyFirstOrderClient(clientReceived) ?
                            "\nNo order yet, setting first order of client " + clientReceived.dateFirstOrder.ToString("HH:mm:ss.ff") + "...\n" :
                            "\n It'a already a client\n")
                        );


                        clerk.CreateOrder(
                            idOrder,
                            clientReceived.dateFirstOrder,
                            clientReceived.firstName,
                            clientReceived.address,
                            clerk.firstName,
                            clientReceived.orderItems
                        );

                        clerk.SendCommand();
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
