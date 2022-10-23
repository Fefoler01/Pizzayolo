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
            Console.WriteLine("\nChoose your name:");
            string name = Console.ReadLine();
            Console.WriteLine("Choose your surname:");
            string surname = Console.ReadLine();

            Kitchen cooker = new Kitchen(name, surname);

            bool chose = true;

            while (chose)
            {

                Func<Kitchen, Order, int, Task> CookingPizza = (Kitchen c, Order order, int timeCooking) =>
                {
                    return Task.Run(() =>
                    {
                        Thread.Sleep(timeCooking);
                        order.state = OrderStatus.Delivery;
                        c.orderGenerated = order;
                        Console.WriteLine("\n" + c.firstName + " Are preparing the order number : " + order.number +
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
    }
}
