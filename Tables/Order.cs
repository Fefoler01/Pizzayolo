// Order is composed by OrderItems, which is a list of Item (pizzas and snacks). Client can make multiple orders at the same time.

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
        public string nameClerk { get; set; }
	    public string nameClient {get; set;}
        public string addressClient { get; set; }
        public OrderItems items { get; set; }
        public OrderStatus state { get; set; }

        // Constructors
        public Order() { }

        public Order(uint number, DateTime orderSchedule, string nameClient, string nameClerk, string addressClient) {
            this.number = number;
            this.orderSchedule = orderSchedule;
            this.nameClient = nameClient;
            this.nameClerk = nameClerk;
            this.addressClient = addressClient;
            state = OrderStatus.Preparation;
        }

        public Order(uint number, DateTime orderSchedule, string nameClient, string nameClerk, string addressClient, OrderItems itemsOrder) : this(number, orderSchedule, nameClient, nameClerk, addressClient) {
            items = itemsOrder;
        }

        public Order(uint number, DateTime orderSchedule, string nameClient, string nameClerk, string addressClient, List<Pizza> pizzas, List<Snack> snacks) : this(number, orderSchedule, nameClient, nameClerk, addressClient, new OrderItems(pizzas, snacks)) { }

        // Methods
        public string Invoice() {
            return items.Invoice();
        }

        public override string ToString() {
            return "Order Number : " + number.ToString()
                + "\nSchedule Order : " + orderSchedule.ToString()
                + "\nName of client : " + nameClient
                + "\nAdress of client : " + addressClient
                + "\nName of clerk : " + nameClerk
                + "\nItems : " + (items != default(OrderItems) ? items.ToString() : "")
                + "\n" + Invoice() + "€"
                + "\nState : " + state.ToString();
        }
    }
}
