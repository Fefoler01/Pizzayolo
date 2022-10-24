// Kitchen takes the order from the clerk and prepares it, it represents a person and can't take multiple orders at the same time

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pizzayolo.MessageBroker.Consumer;
using Pizzayolo.MessageBroker.Producer;

namespace Pizzayolo.Tables
{
    // Kitchen is a singleton, it's not possible to instantiate it, but it's possible to inherit from it.
    public sealed class Kitchen : Individual
    {
        // Properties
        public Order orderGenerated { get; set; }

        // Constructors
        public Kitchen() { }

        public Kitchen(string firstName, string lastName) : base(firstName, lastName) { }

        // Methods
        public override bool SendCommand() {
            return Publisher.Publish<Order>(orderGenerated, "kitchen-deliveryman");
        }

        public override Order ReceiveCommand<Order>() {
            return Receiver.Receive<Order>("clerk-kitchen");
        }

        public override T ReceiveSupervision<T>() {
            throw new NotImplementedException();
        }

        public override bool SendSupervision() {
            throw new NotImplementedException();
        }

        public bool SendSupervisionNewOrderKitchen() {
            return Publisher.PublishTopic<Kitchen>(this, "kitchen-admin");
        }

        public bool SendSupervisionAddKitchen() {
            return Publisher.PublishTopic<Kitchen>(this, "kitchen-admin-add");
        }
    }
}
