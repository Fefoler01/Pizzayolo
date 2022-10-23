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
            Console.WriteLine("Choose your name");
            string name = Console.ReadLine();
            Console.WriteLine("Choose your surname");
            string surname = Console.ReadLine();
            Console.WriteLine("Choose your address");
            string address = Console.ReadLine();
            Console.WriteLine("Choose your phone number");
            string phonenumber = Console.ReadLine();

            Client client = new Client(name, surname, address, phonenumber);

            Console.WriteLine("Choose your pizza");
            int choice = Int32.Parse(Console.ReadLine());
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

            Console.WriteLine("Choose your size");
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

            bool stop = true;
            while (stop)
            {
                Console.WriteLine("Choose another pizza");
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
                    Console.WriteLine("Choose your size");
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
            stop = true;
            List<Drinks> DrinksList = new List<Drinks>();

            while (stop)
            {
                Console.WriteLine("Choose your snack");
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

            client.OrderItems = new OrderItems(PizzaList,DrinksList);

            bool chose = true;
            bool test = true;
            bool test2 = true;
            string chose2;

            while (chose)
            {
                Func<Client, int, Task<bool>> SendingMessage = (Client c, int TimeToLeave) =>
                {
                    return Task.Run(() => {

                        Thread.Sleep(TimeToLeave);
                        Console.WriteLine("\nClient : " + c.firstName + " are calling ...");
                        return c.SendCommand();
                    });
                };

                var tasks = new List<Task<bool>>()
                {
                    SendingMessage(client,0)
                };

                await Task.WhenAll(tasks.ToArray());
                
                while (test && test2)
                {
                    Console.WriteLine(client);
                    Console.WriteLine("Y for pass a new command; N for not; U for unlimited!!!!!!!!!!!!!!!!!");
                    chose2 = Console.ReadLine();
                    if (chose2 == "Y" || chose2 == "y")
                    {
                        Console.WriteLine("Order Again.");
                        test = false;
                    }
                    else if (chose2 == "N" || chose2 == "n")
                    {
                        Console.WriteLine("Quit.");
                        test = false;
                        chose = false;
                    }
                    else if (chose2 == "U" || chose2 == "u")
                    {
                        Console.WriteLine("Quit.");
                        test2 = false;
                    }
                    else
                    {
                        Console.WriteLine("Dont understand");
                    }
                }
                test = true;
            }
        }

        static async Task ClerkSide()
        {
            Console.WriteLine("Choose your name");
            string name = Console.ReadLine();
            Console.WriteLine("Choose your surname");
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
                            "\nThe clerk : " + clerk.firstName + " Receive the folowing Client : \n" +
                            "\n" + clientReceived.firstName + "\n" +
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
            Console.WriteLine("Choose your name");
            string name = Console.ReadLine();
            Console.WriteLine("Choose your surname");
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
                        Console.WriteLine("\n" + c.firstName + " Are preparing the order number : " + order +
                        "\n" + c.firstName + " Send order to delivery man");
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
            Console.WriteLine("Choose your name");
            string name = Console.ReadLine();
            Console.WriteLine("Choose your surname");
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
        
        static async Task  ConsumeOrdersDelivered()
        {
            bool chose = true;

            while (chose)
            {
                Func<int, Task> ReceiveOrder = (int timeToLeave) =>
                {
                    return Task.Run(() =>
                    {
                        Thread.Sleep(timeToLeave);
                        Order orderReceived = Receiver.Receive<Order>("deliveryman-client");
                        Console.WriteLine(orderReceived);
                    });
                };

                var OrderReceives = new List<Task>()
                {
                    ReceiveOrder(0),
                    ReceiveOrder(0),
                    ReceiveOrder(0),
                    ReceiveOrder(0)
                };

                await Task.WhenAll(OrderReceives.ToArray());
                
            }
        }
        
        static async Task Main(string[] args)
        {
            Console.WriteLine("Choose");
            int chose = Int32.Parse(Console.ReadLine());
            bool stop = false;

            switch (chose)
            {
                case 1:
                    await ClientSide();
                    break;

                case 2:
                    await ClerkSide();
                    break;

                case 3:
                    await CookerSide();
                    break;

                case 4:
                    await DeliveryManSide();
                    break;

                case 5:
                    await ConsumeOrdersDelivered();
                    break;

                case 6:
                    Console.WriteLine("\nProgram stopped.");
                    stop = true;
                    break;

                default:
                    Console.WriteLine("\nRetry.");
                    break;

            }
            if (stop == true) { return; } 
        }
    }
}