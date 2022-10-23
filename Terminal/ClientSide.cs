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
    internal class ClientSide
    {
        public static async Task ActivateClientSide()
        {
            Console.WriteLine("======\nClient\n======");

            Console.WriteLine("Choose your name :");
            string name = Console.ReadLine();
            Console.WriteLine("Choose your surname :");
            string surname = Console.ReadLine();
            Console.WriteLine("Choose your address :");
            string address = Console.ReadLine();
            Console.WriteLine("Choose your phone number :");
            string phonenumber = Console.ReadLine();

            Client client = new Client(name, surname, address, phonenumber);

            int choice;
            bool chose = true;
            bool stop = true;
            bool orderAgain = true;
            bool orderUnlimited = true;
            string choseNext;

            while (chose)
            {
                if (orderUnlimited)
                {
                    Console.WriteLine("\nChoose the number of your pizza :" +
                        "\n 1. Margarita" +
                        "\n 2. Hawaïan" +
                        "\n 3. Four Seasons" +
                        "\n 4. Regina");
                    choice = Int32.Parse(Console.ReadLine());
                    PizzaKind kindPizza = new PizzaKind();
                    switch (choice)
                    {
                        case 1:
                            kindPizza = PizzaKind.Margarita;
                            break;

                        case 2:
                            kindPizza = PizzaKind.Hawaïan;
                            break;

                        case 3:
                            kindPizza = PizzaKind.FourSeasons;
                            break;

                        case 4:
                            kindPizza = PizzaKind.Regina;
                            break;

                        default:
                            Console.WriteLine("\nRetry.");
                            break;
                    }

                    Console.WriteLine("Choose the number of your size" +
                        "\n 1. Small" +
                        "\n 2. Medium" +
                        "\n 3. Large");
                    choice = Int32.Parse(Console.ReadLine());
                    PizzaSize sizePizza = new PizzaSize();
                    switch (choice)
                    {
                        case 1:
                            sizePizza = PizzaSize.Small;
                            break;

                        case 2:
                            sizePizza = PizzaSize.Medium;
                            break;

                        case 3:
                            sizePizza = PizzaSize.Large;
                            break;

                        default:
                            Console.WriteLine("\nRetry.");
                            break;
                    }

                    List<Pizza> PizzaList = new List<Pizza>();

                    PizzaList.Add(new Pizza(sizePizza, kindPizza));

                    stop = true;

                    while (stop)
                    {
                        Console.WriteLine("\nChoose the number of your another pizza" +
                        "\n 1. Margarita" +
                        "\n 2. Hawaïan" +
                        "\n 3. Four Seasons" +
                        "\n 4. Regina" +
                        "\nTo quit tap 0.");
                        choice = Int32.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case 0:
                                stop = false;
                                break;

                            case 1:
                                kindPizza = PizzaKind.Margarita;
                                break;

                            case 2:
                                kindPizza = PizzaKind.Hawaïan;
                                break;

                            case 3:
                                kindPizza = PizzaKind.FourSeasons;
                                break;

                            case 4:
                                kindPizza = PizzaKind.Regina;
                                break;

                            default:
                                Console.WriteLine("\nRetry.");
                                break;

                        }


                        if (stop == true)
                        {
                            Console.WriteLine("Choose the number of your size" +
                            "\n 1. Small" +
                            "\n 2. Medium" +
                            "\n 3. Large" +
                            "\nTo quit tap 0.");
                            choice = Int32.Parse(Console.ReadLine());
                            switch (choice)
                            {
                                case 0:
                                    stop = false;
                                    break;

                                case 1:
                                    sizePizza = PizzaSize.Small;
                                    break;

                                case 2:
                                    sizePizza = PizzaSize.Medium;
                                    break;

                                case 3:
                                    sizePizza = PizzaSize.Large;
                                    break;

                                default:
                                    Console.WriteLine("\nRetry.");
                                    break;

                            }
                            if (stop == true)
                            {
                                PizzaList.Add(new Pizza(sizePizza, kindPizza));
                            }

                        }
                    }

                    List<Snacks> DrinksList = new List<Snacks>();

                    stop = true;

                    Console.WriteLine("\n");
                    while (stop)
                    {
                        Console.WriteLine("Choose your snack" +
                        "\n 1. Coca" +
                        "\n 2. Orangina" +
                        "\n 3. Sevenup" +
                        "\nTo quit tap 0.");
                        choice = Int32.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case 0:
                                stop = false;
                                break;

                            case 1:
                                DrinksList.Add(Snacks.Coca);
                                break;

                            case 2:
                                DrinksList.Add(Snacks.Orangina);
                                break;

                            case 3:
                                DrinksList.Add(Snacks.Sevenup);
                                break;

                            default:
                                Console.WriteLine("\nRetry.");
                                break;
                        }
                    }

                    client.orderItems = new OrderItems(PizzaList, DrinksList);
                }


                orderAgain = true;


                Func<Client, int, Task<bool>> SendingMessage = (Client c, int TimeToLeave) =>
                {
                    return Task.Run(() => {

                        Thread.Sleep(TimeToLeave);
                        Console.WriteLine("\nClient : " + c.firstName + " " + c.lastName + " is placing order ...");
                        return c.SendCommand();
                    });
                };

                var tasks = new List<Task<bool>>()
                {
                    SendingMessage(client,0)
                };

                await Task.WhenAll(tasks.ToArray());

                Func<int, Task> ReceiveOrder = (int timeToLeave) =>
                {
                    return Task.Run(() =>
                    {
                        Thread.Sleep(timeToLeave);
                        Order orderReceived = Receiver.Receive<Order>("deliveryman-client");
                        Console.WriteLine("\n-----------------------------------------------");
                        Console.WriteLine(orderReceived);
                        Console.WriteLine("\n-----------------------------------------------\n");
                    });
                };

                var OrderReceives = new List<Task>()
                {
                    ReceiveOrder(0),
                    ReceiveOrder(0),
                    ReceiveOrder(0),
                    ReceiveOrder(0)
                };

                Task.WhenAll(OrderReceives.ToArray());



                while (orderAgain && orderUnlimited)
                {
                    Console.WriteLine("\n-----------------------------------------------");
                    Console.WriteLine("\nYour order is : " + client.orderItems);
                    Console.WriteLine("\n-----------------------------------------------\n");
                    Console.WriteLine("Y for pass a new command; N for not; U for unlimited!");
                    choseNext = Console.ReadLine();
                    if (choseNext == "Y" || choseNext == "y")
                    {
                        Console.WriteLine("Order Again.\n");
                        orderAgain = false;
                    }
                    else if (choseNext == "N" || choseNext == "n")
                    {
                        Console.WriteLine("Quit.");
                        orderAgain = false;
                        chose = false;
                    }
                    else if (choseNext == "U" || choseNext == "u")
                    {
                        orderUnlimited = false;
                    }
                    else
                    {
                        Console.WriteLine("Dont understand.\n");
                    }
                }
                orderAgain = true;
            }
        }
    }
}
