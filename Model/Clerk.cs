using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Pizzayolo.Model;
using Pizzayolo.MessageBroker.Consumer;
using Pizzayolo.MessageBroker.Producer;
using System.Globalization;

namespace Pizzayolo.Model
{
    public sealed class Clerk : Individual
    {
        #region Properties
        public Order OrderGenerated;
        #endregion

        #region Constructors
        public Clerk(string firstName, string lastName) : base(firstName, lastName) { }
        #endregion

        #region Methods
        public Order CreateOrder(uint number, DateTime orderSchedule, string nameClient, string adressClient, string nameClerk) {
            OrderGenerated = new Order(number, orderSchedule, nameClient, nameClerk,adressClient);
            return OrderGenerated;
        }

        public bool VerifyFirstOrderClient(Client c) {
            if(c.DateFirstOrder == DateTime.MinValue) {
                c.DateFirstOrder = DateTime.Now;
                return false;
            }
            return true;
        }

        public override Client ReceiveCommand<Client>() {
            return  Receiver.Receive<Client>("client-clerk");
        }

        public override bool SendCommand() {
            return Publisher.Publish<Order>(OrderGenerated,"clerk-cooker");
        }
        #endregion
    }
}
