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

namespace Pizzayolo
{
    class Program
    { 
        static async Task ClientSide()
        {
            Client client1 = new Client("Jonathan", "ELBAZ", "11 allee Auguste Renoir", "0650404305");
            Client client2 = new Client("Bertrand", "Jasdin", "45 rue du coq", "068547438484");
            Client client3 = new Client("Xavier", "Courrin", "16 rue louis jean baptise", "06847397434");
            Client client4 = new Client("Thomas", "Banner", "47 rue jean paul rouquis", "078498934899");

            Func<Client,int,Task<bool>> SendingMessage = (Client c,int TimeToLeave) =>
            {
                 return Task.Run(() => {

                     Thread.Sleep(TimeToLeave);
                     Console.WriteLine("\nClient : " + c.firstName + " are calling ...");
                     return c.SendCommand();
                 });         
            };
            
            var tasks = new List<Task<bool>>()
            {
                SendingMessage(client1,0),
                SendingMessage(client2,0),
                SendingMessage(client3,0),
                SendingMessage(client4,0)
            };

            await Task.WhenAll(tasks.ToArray());
        }

        static async Task ClerkSide()
        {
            Clerk clerk1 = new Clerk("Baptiste", "Ruskoff");
            Clerk clerk2 = new Clerk("Francois", "xavier");

            Func<Clerk, uint, Task> ClerksReceive = (Clerk clerk, uint idOrder) =>
            {
                  return Task.Run(() =>
                  {
                      Client clientReceived = clerk.ReceiveCommand<Client>();
                      
                      Console.WriteLine(
                          "\nThe clerk : " + clerk.firstName + " Receive the folowing Client : \n" +
                          "\n" + clientReceived + "\n" +
                          "\nVerify first order of client " + clientReceived.firstName + " : \n" +
                          (!clerk.VerifyFirstOrderClient(clientReceived) ?
                          "\nNo order yet, setting first order of client " + clientReceived.DateFirstOrder.ToString("HH:mm:ss.ff") + "...\n" :
                          "\n It'a already a client\n")
                      );

                      
                      clerk.CreateOrder(
                          idOrder,
                          clientReceived.DateFirstOrder,
                          clientReceived.firstName,
                          clientReceived.Adress,
                          clerk.firstName
                      );

                      clerk.SendCommand();
                  });
            };

            var ManageTwoClientClerk = new List<Task>()
            {
                ClerksReceive(clerk1,1),
                ClerksReceive(clerk1,2)
            };

            Task.WaitAll(ManageTwoClientClerk.ToArray());
            
            ManageTwoClientClerk.Clear();

            Thread.Sleep(5000);

            ManageTwoClientClerk = new List<Task>()
            {
                ClerksReceive(clerk2,3),
                ClerksReceive(clerk2,4)
            };

            await Task.WhenAll(ManageTwoClientClerk.ToArray());

        }

        static async Task CookerSide()
        {
            Cooker cooker1 = new Cooker("Bastien", "Philipe");
            Cooker cooker2 = new Cooker("Julie", "Cunolum");
            Cooker cooker3 = new Cooker("Augustin", "Philipe");

            Func<Cooker,OrderItems,Order,int,Task> CookingPizza = (Cooker c,OrderItems orderItems, Order order, int timeCooking) =>
            {
                return Task.Run(() =>
                {
                    Thread.Sleep(timeCooking);
                    order.Items = orderItems;
                    order.State = Order.Status.Delivery;
                    c.OrderGenerated = order;
                    Console.WriteLine("\n" + c.firstName + " Are preparing the order number : " + order.Number +
                    "\n" + c.firstName + " Send order to delivery man");
                    c.SendCommand();
                });
            };

            var OrdersPreparing = new List<Task>()
            {
                CookingPizza(
                    cooker1,
                    new OrderItems(
                        new List<Pizza> (){ 
                            new Pizza(PizzaSize.Large, PizzaKind.Hawaïan, 150),
                            new Pizza(PizzaSize.Medium, PizzaKind.FourSeasons, 250)
                        },
                        new List<Drinks>()
                        {
                            Drinks.Coca,
                            Drinks.Orangina
                        }
                    ),
                    cooker1.ReceiveCommand<Order>(),
                    4000
                ),
                CookingPizza(
                    cooker1,
                    new OrderItems(
                        new List<Pizza> (){
                            new Pizza(PizzaSize.Medium, PizzaKind.Margarita, 150),
                            new Pizza(PizzaSize.Medium, PizzaKind.Regina, 300)
                        },
                        new List<Drinks>()
                        {
                            Drinks.Coca
                        }
                    ),
                    cooker1.ReceiveCommand<Order>(),
                    3000
                ),
                CookingPizza(
                    cooker2,
                    new OrderItems(
                        new List<Pizza> (){
                            new Pizza(PizzaSize.Medium, PizzaKind.Margarita, 150)
                        },
                        new List<Drinks>()
                        {
                            Drinks.Sevenup
                        }
                    ),
                    cooker2.ReceiveCommand<Order>(),
                    1000
                ),
                CookingPizza(
                    cooker3,
                    new OrderItems(
                        new List<Pizza> (){
                            new Pizza(PizzaSize.Large,PizzaKind.Hawaïan,150),
                            new Pizza(PizzaSize.Medium,PizzaKind.Regina,300),
                            new Pizza(PizzaSize.Small,PizzaKind.Margarita,300)
                        },
                        new List<Drinks>()
                        {
                            Drinks.Coca,
                            Drinks.Coca
                        }
                    ),
                    cooker3.ReceiveCommand<Order>(),
                    2000
                )
            };

            await Task.WhenAll(OrdersPreparing.ToArray());
        }

        static async Task DeliveryManSide()
        {
            DeliveryMan d1 = new DeliveryMan("Abdel", "Cuistor");
            DeliveryMan d2 = new DeliveryMan("Rone", "LaTaupe");

            Func<DeliveryMan, int, Task> Deliver = (DeliveryMan d, int timeToDeliver) =>
            {
                return Task.Run(() =>
                {
                    Thread.Sleep(timeToDeliver);
                    Order orderReceived= d.ReceiveCommand<Order>();
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
                Deliver(d1,5000),
                Deliver(d1,5000),
                Deliver(d2,3000),
                Deliver(d2,3000)
            };

            await Task.WhenAll(Delivers.ToArray());
        }
        
        static async Task  ConsumeOrdersDelivered()
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
        
        static async Task Main(string[] args)
        {
           
            await ClientSide();
            
            Console.WriteLine("\nType enter to continue....\n");
            Console.ReadLine();

            await ClerkSide();

            Console.WriteLine("\nType enter to continue....\n");
            Console.ReadLine();
            
            await CookerSide();

            Console.WriteLine("\nType enter to continue....\n");
            Console.ReadLine();
            

            await DeliveryManSide();

            Console.WriteLine("\nType enter to continue....\n");
            Console.ReadLine();
            
            await ConsumeOrdersDelivered ();
            
        }

    }
}
