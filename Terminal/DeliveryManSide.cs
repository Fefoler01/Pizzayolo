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
    internal class DeliveryManSide
    {
        public static async Task ActivateDeliveryManSide()
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
                        d.order = orderReceived;
                        d.order.state = OrderStatus.Delivered;
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
    }
}
