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
    internal class KitchenSide
    {
        public static async Task ActivateKitchenSide()
        {
            Console.WriteLine("\n=====\nKitchen\n=====");
            Console.WriteLine("\nChoose your name:");
            string name = Console.ReadLine();
            Console.WriteLine("Choose your surname:");
            string surname = Console.ReadLine();

            Kitchen kitchen = new Kitchen(name, surname);
            kitchen.SendSupervisionAddKitchen();

            bool chose = true;

            while (chose)
            {

                Func<Kitchen, Order, int, Task> CookingPizza = (Kitchen k, Order order, int timeCooking) =>
                {
                    return Task.Run(async () =>
                    {
                        await Task.Delay(timeCooking);
                        order.state = OrderStatus.Cooking;
                        k.orderGenerated = order;
                        Console.WriteLine("\nThe Kitchen " + k.firstName + " " + k.lastName + " is cooking the order : " + order +
                        "\nThe Kitchen " + k.firstName + " " + k.lastName + " is sending the order to the delivery man\n");
                        await Task.Delay(timeCooking);
                        k.SendCommand();
                        k.SendSupervisionNewOrderKitchen();
                    });
                };

                var OrdersPreparing = new List<Task>()
                {
                    CookingPizza(
                        kitchen,
                        kitchen.ReceiveCommand<Order>(),
                        4000
                    )
                };

                Thread.Sleep(5000);

                await Task.WhenAll(OrdersPreparing.ToArray());


            }
        }
    }
}
