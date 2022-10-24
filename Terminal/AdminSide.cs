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
using Pizzayolo.Model;

namespace Pizzayolo.Terminal
{
    internal class AdminSide
    {
        public static async Task ActivateAdminSide()
        {
            Console.WriteLine("\n======\nAdmin\n======");

            Console.WriteLine("Choose your name :");
            string name = Console.ReadLine();
            Console.WriteLine("Choose your surname :");
            string surname = Console.ReadLine();

            Admin admin = new Admin(name, surname);

            while (true) {

                // Add
                Func<int, Task> AddClient = (int delayDelivery) =>
                {
                    return Task.Run(async () =>
                    {
                        await Task.Delay(delayDelivery);
                        Client clientReceived = admin.ReceiveSupervisionAddClient<Client>();
                        admin.AddClient(clientReceived);
                        Console.WriteLine("\nRecorded client " + clientReceived.firstName + " " + clientReceived.lastName);
                    });
                };

                Func<int, Task> AddClerk = (int delayDelivery) =>
                {
                    return Task.Run(async () =>
                    {
                        await Task.Delay(delayDelivery);
                        Clerk clerkReceived = admin.ReceiveSupervisionAddClerk<Clerk>();
                        admin.AddClerk(clerkReceived);
                        Console.WriteLine("\nRecorded clerk " + clerkReceived.firstName + " " + clerkReceived.lastName);
                    });
                };

                Func<int, Task> AddOrder = (int delayDelivery) =>
                {
                    return Task.Run(async () =>
                    {
                        await Task.Delay(delayDelivery);
                        Order orderReceived = admin.ReceiveSupervisionAddOrder<Order>();
                        admin.AddOrder(orderReceived);
                        Console.WriteLine("\nRecorded order " + orderReceived);
                    });
                };

                Func<int, Task> AddDeliveryMan = (int delayDelivery) =>
                {
                    return Task.Run(async () =>
                    {
                        await Task.Delay(delayDelivery);
                        DeliveryMan deliveryManReceived = admin.ReceiveSupervisionAddDeliveryMan<DeliveryMan>();
                        admin.AddDeliveryMan(deliveryManReceived);
                        Console.WriteLine("\nRecorded deliveryMan " + deliveryManReceived.firstName + " " + deliveryManReceived.lastName);
                    });
                };

                Func<int, Task> AddKitchen = (int delayDelivery) =>
                {
                    return Task.Run(async () =>
                    {
                        await Task.Delay(delayDelivery);
                        Kitchen kitchenReceived = admin.ReceiveSupervisionAddKitchen<Kitchen>();
                        admin.AddKitchen(kitchenReceived);
                        Console.WriteLine("\nRecorded kitchen " + kitchenReceived.firstName + " " + kitchenReceived.lastName);
                    });
                };

                // NewOrder
                Func<int, Task> NewOrderClient = (int delayDelivery) =>
                {
                    return Task.Run(async () =>
                    {
                        await Task.Delay(delayDelivery);
                        Client clientReceived = admin.ReceiveSupervisionNewOrderClient<Client>();
                        admin.NewOrderClient(clientReceived);
                        Console.WriteLine("\nNew order for client " + clientReceived.firstName + " " + clientReceived.lastName);
                    });
                };

                Func<int, Task> NewOrderClerk = (int delayDelivery) =>
                {
                    return Task.Run(async () =>
                    {
                        await Task.Delay(delayDelivery);
                        Clerk clerkReceived = admin.ReceiveSupervisionNewOrderClerk<Clerk>();
                        admin.NewOrderClerk(clerkReceived);
                        Console.WriteLine("\nNew order for clerk " + clerkReceived.firstName + " " + clerkReceived.lastName);
                    });
                };

                Func<int, Task> NewOrderDeliveryMan = (int delayDelivery) =>
                {
                    return Task.Run(async () =>
                    {
                        await Task.Delay(delayDelivery);
                        DeliveryMan deliveryManReceived = admin.ReceiveSupervisionNewOrderDeliveryMan<DeliveryMan>();
                        admin.NewOrderDeliveryMan(deliveryManReceived);
                        Console.WriteLine("\nNew order for deliveryMan " + deliveryManReceived.firstName + " " + deliveryManReceived.lastName);
                    });
                };

                Func<int, Task> NewOrderKitchen = (int delayDelivery) =>
                {
                    return Task.Run(async () =>
                    {
                        await Task.Delay(delayDelivery);
                        Kitchen kitchenReceived = admin.ReceiveSupervisionNewOrderKitchen<Kitchen>();
                        admin.NewOrderKitchen(kitchenReceived);
                        Console.WriteLine("\nNew order for kitchen " + kitchenReceived.firstName + " " + kitchenReceived.lastName);
                    });
                };

                var OrderReceives = new List<Task>() // list of signals
                {
                    AddClient(0),
                    AddClerk(0),
                    AddOrder(0),
                    AddDeliveryMan(0),
                    AddKitchen(0),
                    NewOrderClerk(0),
                    NewOrderClient(0),
                    NewOrderDeliveryMan(0),
                    NewOrderKitchen(0)
                };

                Task.WhenAll(OrderReceives.ToArray());

                bool keepAlive = true;
                bool invalid = true;
                string choseNext;

                while (keepAlive)
                {
                    Console.WriteLine("\nWhat do you want to do ?" +
                        "\n 1. Show list of client" +
                        "\n 2. Show list of clerk" +
                        "\n 3. Show the cumulate paid by a client" +
                        "\n 4. Show orders between two date" +
                        "\n 5. Show the average price of orders" +
                        "\n 6. Show the average cumulate paid by clients" +
                        "\nTo quit tap 0.");
                    while (invalid)
                    {
                        choseNext = Console.ReadLine();
                        switch (choseNext)
                        {
                            case "0":
                                keepAlive = false;
                                invalid = false;
                                return;

                            case "1":
                                //;
                                invalid = false;
                                break;

                            case "2":
                                //;
                                invalid = false;
                                break;

                            case "3":
                                Console.WriteLine("Choose the firstname of the client :");
                                string firstname = Console.ReadLine();
                                Console.WriteLine("Choose the lastname of the client :");
                                string lastname = Console.ReadLine();

                                Client client = new Client(firstname, lastname);
                                admin.ShowTotalPriceOrder(client);

                                invalid = false;
                                break;

                            case "4":
                                Console.WriteLine("Choose the start date (dd-MM-yyyy) :");
                                DateTime start = DateTime.ParseExact(Console.ReadLine(), "dd-MM-yyyy", null);
                                Console.WriteLine("Choose the end date (dd-MM-yyyy) :");
                                DateTime end = DateTime.ParseExact(Console.ReadLine(), "dd-MM-yyyy", null);
                                admin.ShowOrderListDate(start, end);

                                invalid = false;
                                break;

                            default:
                                Console.WriteLine("\nRetry.");
                                invalid = true;
                                break;
                        }
                    }
                    invalid = true;

                }
            }
        }
    }
}
