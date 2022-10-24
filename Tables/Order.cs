// Order is composed by OrderItems, which is a list of Item (pizzas and snacks). Client can make multiple orders at the same time.

using Pizzayolo.MessageBroker.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzayolo.Tables
{
    public class Order
    {
        // Properties
        public uint number { get; set; }
        public DateTime orderSchedule { get; set; }
        public Clerk clerk { get; set; }
	    public Client client {get; set;}
        public OrderItems items { get; set; }
        public OrderStatus state { get; set; }

        // Constructors
        public Order() { }

        public Order(uint number, DateTime orderSchedule, Client client, Clerk clerk) {
            this.number = number;
            this.orderSchedule = orderSchedule;
            this.client = client;
            this.clerk = clerk;
            state = OrderStatus.Preparing;
        }

        public Order(uint number, DateTime orderSchedule, Client client, Clerk clerk, OrderItems itemsOrder) : this(number, orderSchedule, client, clerk) {
            items = itemsOrder;
        }

        public Order(uint number, DateTime orderSchedule, Client client, Clerk clerk, List<Pizza> pizzas, List<Snack> snacks) : this(number, orderSchedule, client, clerk, new OrderItems(pizzas, snacks)) { }

        // Methods
        public string Invoice() {
            return items.Invoice();
        }

        public override string ToString() {
            return "\n-----------------------------------------------\n"
                + "Order Number : " + number.ToString()
                + "\nSchedule Order : " + orderSchedule.ToString()
                + "\nName of client : " + client.firstName + " " + client.lastName
                + "\nAdress of client : " + client.address
                + "\nName of clerk : " + clerk.firstName + " " + clerk.lastName
                + "\nItems : " + (items != default(OrderItems) ? items.ToString() : "")
                + "\n" + Invoice()
                + "\nState : " + state.ToString()
                + "\n-----------------------------------------------\n";
        }

        public bool SendSupervisionAddOrder()
        {
            return Publisher.PublishTopic<Order>(this, "order-admin-add");
        }
    }
}
