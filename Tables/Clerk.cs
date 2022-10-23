// Clerk is the person who is responsible for the order, he can take multiple orders at the same time

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Pizzayolo.Tables;
using Pizzayolo.MessageBroker.Consumer;
using Pizzayolo.MessageBroker.Producer;
using System.Globalization;

namespace Pizzayolo.Tables
{
    // class Clerk is a subclass of Individual and implements the interface IIndividual 
    public sealed class Clerk : Individual
    {
        // Properties
        public Order orderGenerated;

        // Constructors
        public Clerk(string firstName, string lastName) : base(firstName, lastName) { }

        // Methods
        public Order CreateOrder(uint number, DateTime orderSchedule, string nameClient, string adressClient, string nameClerk, OrderItems order) {
            orderGenerated = new Order(number, orderSchedule, nameClient, nameClerk, adressClient, order);
            return orderGenerated;
        }

        public bool VerifyFirstOrderClient(Client client) {
            if(client.dateFirstOrder == DateTime.MinValue) {
                client.dateFirstOrder = DateTime.Now;
                return false;
            }
            return true;
        }

        public override bool SendCommand() {
            return Publisher.Publish<Order>(orderGenerated,"clerk-kitchen");
        }

        public override Client ReceiveCommand<Client>() {
            return  Receiver.Receive<Client>("client-clerk");
        }
    }
}
