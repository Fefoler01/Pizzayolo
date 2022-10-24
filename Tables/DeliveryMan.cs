// DeliveryMan deliver the order to the customer, he can't take multiple orders at the same time

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pizzayolo.MessageBroker.Consumer;
using Pizzayolo.MessageBroker.Producer;

namespace Pizzayolo.Tables
{
    public sealed class DeliveryMan : Individual
    {

        // Properties
        public Order order{ get; set; }

        // Constructors
        public DeliveryMan() { }

        public DeliveryMan(string firstName, string lastName) : base(firstName, lastName) { }

        // Methods
        public override bool SendCommand() {
            return Publisher.Publish<Order>(order, "deliveryman-client");
        }

        public override Order ReceiveCommand<Order>() {
            return Receiver.Receive<Order>("kitchen-deliveryman");
        }

        public override T ReceiveSupervision<T>() {
            throw new NotImplementedException();
        }

        public override bool SendSupervision() {
            throw new NotImplementedException();
        }

        public bool SendSupervisionNewOrderDeliveryMan() {
            return Publisher.PublishTopic<DeliveryMan>(this, "deliveryman-admin");
        }

        public bool SendSupervisionAddDeliveryMan()
        {
            return Publisher.PublishTopic<DeliveryMan>(this, "deliveryman-admin-add");
        }
    }
}
