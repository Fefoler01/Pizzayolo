using System;
using System.Text;

using Pizzayolo.Model;
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
        static async Task ClientSide()
        {
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
                    
                    List<Drinks> DrinksList = new List<Drinks>();

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
                                DrinksList.Add(Drinks.Coca);
                                break;

                            case 2:
                                DrinksList.Add(Drinks.Orangina);
                                break;

                            case 3:
                                DrinksList.Add(Drinks.Sevenup);
                                break;

                            default:
                                Console.WriteLine("\nRetry.");
                                break;
                        }
                    }

                    client.OrderItems = new OrderItems(PizzaList, DrinksList);
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
                    Console.WriteLine("\nYour order is : " + client.OrderItems);
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

        static async Task ClerkSide()
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
                            "\nVerify first order of client " + clientReceived.DateFirstOrder + " : \n" +
                            (!clerk.VerifyFirstOrderClient(clientReceived) ?
                            "\nNo order yet, setting first order of client " + clientReceived.DateFirstOrder.ToString("HH:mm:ss.ff") + "...\n" :
                            "\n It'a already a client\n")
                        );


                        clerk.CreateOrder(
                            idOrder,
                            clientReceived.DateFirstOrder,
                            clientReceived.firstName,
                            clientReceived.Adress,
                            clerk.firstName,
                            clientReceived.OrderItems
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

        static async Task CookerSide()
        {
            Console.WriteLine("\nChoose your name:");
            string name = Console.ReadLine();
            Console.WriteLine("Choose your surname:");
            string surname = Console.ReadLine();

            Cooker cooker = new Cooker(name, surname);

            bool chose = true;

            while (chose)
            {

                Func<Cooker, Order, int, Task> CookingPizza = (Cooker c, Order order, int timeCooking) =>
                {
                    return Task.Run(() =>
                    {
                        Thread.Sleep(timeCooking);
                        order.State = Order.Status.Delivery;
                        c.OrderGenerated = order;
                        Console.WriteLine("\n" + c.firstName + " Are preparing the order number : " + order.Number +
                        "\n" + c.firstName + " Send order to delivery man\n");
                        c.SendCommand();
                    });
                };

                var OrdersPreparing = new List<Task>()
                {
                    CookingPizza(
                        cooker,
                        cooker.ReceiveCommand<Order>(),
                        4000
                    )
                };

                await Task.WhenAll(OrdersPreparing.ToArray());

                
            }
        }

        static async Task DeliveryManSide()
        {
            Console.WriteLine("\nChoose your name:");
            string name = Console.ReadLine();
            Console.WriteLine("Choose your surname:");
            string surname = Console.ReadLine();

            DeliveryMan d = new DeliveryMan(name, surname);

            bool chose = true;

            while (chose)
            {

                Func<DeliveryMan, int, Task> Deliver = (DeliveryMan d, int timeToDeliver) =>
                {
                    return Task.Run(() =>
                    {
                        Thread.Sleep(timeToDeliver);
                        Order orderReceived = d.ReceiveCommand<Order>();
                        Console.WriteLine(d.firstName +
                            " \nReceived the following Command : \n"
                            + orderReceived
                            + "\nFinish Delivering\n"
                        );
                        d.Order = orderReceived;
                        d.Order.State = Order.Status.Delivered;
                        d.SendCommand();

                    });
                };

                var Delivers = new List<Task>()
                {
                    Deliver(d,5000),
                };

                await Task.WhenAll(Delivers.ToArray());

                
            }
        }
       
        
        static async Task Main(string[] args)
        {

            Console.WriteLine("Who are you ?" +
                "\n 1. Client" +
                "\n 2. Clerk" +
                "\n 3. Cooker" +
                "\n 4. DeliveryMan" +
                "\nTo quit tap 0.\n");

            int chose = Int32.Parse(Console.ReadLine());
            bool stop = false;

            switch (chose)
            {
                case 1:
                    Console.WriteLine("\n");
                    Console.Title = "Client";
                    await ClientSide();
                    break;

                case 2:
                    Console.WriteLine("\n");
                    Console.Title = "Clerk";
                    await ClerkSide();
                    break;

                case 3:
                    Console.WriteLine("\n");
                    Console.Title = "Cooker";
                    await CookerSide();
                    break;

                case 4:
                    Console.WriteLine("\n");
                    Console.Title = "DeliveryMan";
                    await DeliveryManSide();
                    break;

                case 0:
                    Console.WriteLine("\nProgram stopped.");
                    stop = true;
                    break;

                default:
                    Console.WriteLine("\nRetry, BAD Input.\n");
                    break;

            }
            if (stop == true) { return; } 
        }
    }
}