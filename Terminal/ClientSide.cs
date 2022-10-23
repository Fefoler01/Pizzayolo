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
            Console.WriteLine("\n======\nClient\n======");

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
            bool invalid = true;

            while (chose)
            {
                if (orderUnlimited)
                {
                    Console.WriteLine("\nSelect your pizza type :");
                    foreach (PizzaKind pizzakind in PizzaKind.GetValues(typeof(PizzaKind))) {
                        Console.WriteLine((int) pizzakind + 1 + ". " + pizzakind);
                    }
                    PizzaKind kindPizza = new PizzaKind();
                    while (invalid) {
                        choice = Int32.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case 1:
                                kindPizza = PizzaKind.Margarita;
                                invalid = false;
                                break;

                            case 2:
                                kindPizza = PizzaKind.Hawaïan;
                                invalid = false;
                                break;

                            case 3:
                                kindPizza = PizzaKind.FourSeasons;
                                invalid = false;
                                break;

                            case 4:
                                kindPizza = PizzaKind.Regina;
                                invalid = false;
                                break;

                            default:
                                Console.WriteLine("\nRetry.");
                                invalid = true;
                                break;
                        }
                    }
                    invalid = true;

                    Console.WriteLine("\nSelect the size of your pizza :");
                    foreach (PizzaSize pizzasize in PizzaSize.GetValues(typeof(PizzaSize))) {
                        Console.WriteLine((int)pizzasize + 1 + ". " + pizzasize);
                    }
                    PizzaSize sizePizza = new PizzaSize();
                    while (invalid) {
                        choice = Int32.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case 1:
                                sizePizza = PizzaSize.Small;
                                invalid = false;
                                break;

                            case 2:
                                sizePizza = PizzaSize.Medium;
                                invalid = false;
                                break;

                            case 3:
                                sizePizza = PizzaSize.Large;
                                invalid = false;
                                break;

                            default:
                                Console.WriteLine("\nRetry.");
                                invalid = true;
                                break;
                        }
                    }
                    invalid = true;

                    List<Pizza> PizzaList = new List<Pizza>();

                    PizzaList.Add(new Pizza(sizePizza, kindPizza));

                    stop = true;

                    while (stop)
                    {
                        Console.WriteLine("\nSelect your pizza type :");
                        foreach (PizzaKind pizzakind in PizzaKind.GetValues(typeof(PizzaKind)))
                        {
                            Console.WriteLine((int)pizzakind + 1 + ". " + pizzakind);
                        }
                        Console.WriteLine("To quit tap 0.");
                        while (invalid) {
                            choice = Int32.Parse(Console.ReadLine());
                            switch (choice)
                            {
                                case 0:
                                    stop = false;
                                    invalid = false;
                                    break;

                                case 1:
                                    kindPizza = PizzaKind.Margarita;
                                    invalid = false;
                                    break;

                                case 2:
                                    kindPizza = PizzaKind.Hawaïan;
                                    invalid = false;
                                    break;

                                case 3:
                                    kindPizza = PizzaKind.FourSeasons;
                                    invalid = false;
                                    break;

                                case 4:
                                    kindPizza = PizzaKind.Regina;
                                    invalid = false;
                                    break;

                                default:
                                    Console.WriteLine("\nRetry.");
                                    invalid = true;
                                    break;
                            }
                        }
                        invalid = true;


                        if (stop == true)
                        {
                            Console.WriteLine("\nSelect the size of your pizza :");
                            foreach (PizzaSize pizzasize in PizzaSize.GetValues(typeof(PizzaSize)))
                            {
                                Console.WriteLine((int)pizzasize + 1 + ". " + pizzasize);
                            }
                            Console.WriteLine("To quit tap 0.");
                            while (invalid) {
                                choice = Int32.Parse(Console.ReadLine());
                                switch (choice)
                                {
                                    case 0:
                                        stop = false;
                                        invalid = false;
                                        break;

                                    case 1:
                                        sizePizza = PizzaSize.Small;
                                        invalid = false;
                                        break;

                                    case 2:
                                        sizePizza = PizzaSize.Medium;
                                        invalid = false;
                                        break;

                                    case 3:
                                        sizePizza = PizzaSize.Large;
                                        invalid = false;
                                        break;

                                    default:
                                        Console.WriteLine("\nRetry.");
                                        invalid = true;
                                        break;
                                }
                            }
                            invalid = true;
                            if (stop == true)
                            {
                                PizzaList.Add(new Pizza(sizePizza, kindPizza));
                            }
                        }
                    }

                    List<Snack> SnackList = new List<Snack>();

                    stop = true;

                    Console.WriteLine("\n");
                    while (stop)
                    {
                        Console.WriteLine("\nChoose your snack :");
                        foreach (Snacks snacks in Snacks.GetValues(typeof(Snacks)))
                        {
                            Console.WriteLine((int)snacks + 1 + ". " + snacks);
                        }
                        Console.WriteLine("To quit tap 0.");
                        while (invalid) {
                            choice = Int32.Parse(Console.ReadLine());
                            if (choice == 0)
                            {
                                stop = false;
                                invalid = false;
                            }
                            else if (choice > 0 && choice <= Snacks.GetNames(typeof(Snacks)).Length)
                            {
                                SnackList.Add(new Snack((Snacks) choice-1));
                                invalid = false;
                            }
                            else
                            {
                                Console.WriteLine("\nRetry.");
                                invalid = true;
                            }
                        }
                        invalid = true;
                    }

                    client.orderItems = new OrderItems(PizzaList, SnackList);
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

                Func<int, Task> ReceiveOrder = (int delayDelivery) =>
                {
                    return Task.Run(async () =>
                    {
                        await Task.Delay(delayDelivery);
                        Order orderReceived = Receiver.Receive<Order>("deliveryman-client");
                        Console.WriteLine("\nOrder received! Enjoy your meal, " + client.firstName + " " + client.lastName + "! " + orderReceived);
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
                    Console.WriteLine("Your order is : " + client.orderItems);
                    Console.WriteLine("-----------------------------------------------\n");
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
                        return;
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
