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
        public Order processedOrder;

        public List<Client> clients;

        // Constructors
        public Clerk() { }

        public Clerk(string firstName, string lastName) : base(firstName, lastName) {
            clients = new List<Client>();
        }

        // Methods
        public Order CreateOrder(uint number, DateTime orderSchedule, Client client, Clerk clerk, OrderItems order) {
            processedOrder = new Order(number, orderSchedule, client, clerk, order);
            return processedOrder;
        }

        public bool VerifyFirstOrderClient(Client client) {

            foreach(Client savedclient in clients)
            {
                if (savedclient.firstName == client.firstName && savedclient.lastName == client.lastName && savedclient.address == client.address && savedclient.phoneNumber == client.phoneNumber && savedclient.dateFirstOrder != DateTime.MinValue)
                {
                    client.dateFirstOrder = savedclient.dateFirstOrder;
                    return true;
                }
            }
            
            client.dateFirstOrder = DateTime.Now;
            clients.Add(client);
            return false;
        }

        public override bool SendCommand() {
            return Publisher.Publish<Order>(processedOrder, "clerk-kitchen");
        }

        public override Client ReceiveCommand<Client>() {
            return  Receiver.Receive<Client>("client-clerk");
        }

        public override T ReceiveSupervision<T>() {
            throw new NotImplementedException();
        }

        public override bool SendSupervision() {
            throw new NotImplementedException();
        }

        public bool SendSupervisionNewOrderClerk() {
            return Publisher.PublishTopic<Clerk>(this, "clerk-admin");
        }

        public bool SendSupervisionAddClerk()
        {
            return Publisher.PublishTopic<Clerk>(this, "clerk-admin-add");
        }
    }
}
