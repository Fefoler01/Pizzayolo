// Client is the person who is ordering the pizza (one or more) and some snacks (zero or more)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using Pizzayolo.MessageBroker.Producer;
using Pizzayolo.MessageBroker.Consumer;

namespace Pizzayolo.Tables
{
    // class Client is a subclass of Individual and implements the interface IIndividual
    public sealed class Client : Individual
    {
        // Properties
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public DateTime dateFirstOrder { get; set; }
        public OrderItems orderItems { get; set; }

        // Constructors
        public Client() { }

        public Client(string firstName, string lastName, string address, string phoneNumber) : base(firstName, lastName) {
            this.address = address;
            this.phoneNumber = phoneNumber;
        }

        public Client(string firstName, string lastName, string address, string phoneNumber, DateTime dateFirstOrder) : this(firstName, lastName, address,  phoneNumber) {
            this.dateFirstOrder = dateFirstOrder;
        }

        public Client(Client other) : this(other.firstName, other.lastName, other.address, other.phoneNumber, other.dateFirstOrder) { }

        // Methods
        public override string ToString() {
            return base.ToString()
                + "\nAddress: " + address
                + "\nPhone Number: " + phoneNumber
                + "\nDate First Order: " + dateFirstOrder.ToString()
                + "\nOrder: " + orderItems.ToString();
        }

        public override bool SendCommand() {
            return Publisher.Publish<Client>(this, "client-clerk"); 
        }

        public override Order ReceiveCommand<Order>() {
            return Receiver.Receive<Order>("deliveryman-client");
        }
    }
}
