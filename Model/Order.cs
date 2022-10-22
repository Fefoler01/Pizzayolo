using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzayolo.Model
{
    public class Order
    {
        #region Status
        public enum Status { Prepartion, Delivery, Delivered }
        #endregion

        #region Properties
        public uint Number { get; set; }
        public DateTime OrderSchedule { get; set; }
        public string NameClerk { get; set; }
	    public string NameClient {get; set;}
        public string AdressClient { get; set; }
        public OrderItems Items { get; set; }
        public Status State { get; set; }
        #endregion

        #region Constructors

        public Order() { }

        public Order(uint number, DateTime orderSchedule, string nameClient, string nameClerk, string adressClient) {
            Number = number;
            OrderSchedule = orderSchedule;
            NameClient = nameClient;
            NameClerk = nameClerk;
            AdressClient = adressClient;
            State = Status.Prepartion;
        }

        public Order(uint number, DateTime orderSchedule, string nameClient, string nameClerk, string adressClient, OrderItems itemsOrder) : this(number, orderSchedule, nameClient, nameClerk, adressClient) {
            Items = itemsOrder;
        }

        public Order(uint number, DateTime orderSchedule, string nameClient, string nameClerk, string adressClient, List<Pizza> pizzas, List<Drinks> drinks) : this(number, orderSchedule,nameClient,nameClerk,adressClient,new OrderItems(pizzas,drinks)) { }
        #endregion

        #region Methods

        public string Invoice()
        {
            return Items.Invoice();
        }

        public override string ToString()
        {
            return "Order Number : " + Number.ToString()
                + "\nSchedule Order : " + OrderSchedule.ToString()
                + "\nName of client : " + NameClient
                + "\nAdress of client : " + AdressClient
                + "\nName of clerk : " + NameClerk
                + "\nItems : " + (Items != default(OrderItems) ? Items.ToString() : "")
                + "\nState : " + State.ToString();
        }
        #endregion
    }
}
