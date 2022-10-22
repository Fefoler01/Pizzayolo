using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pizzayolo.MessageBroker.Consumer;
using Pizzayolo.MessageBroker.Producer;

namespace Pizzayolo.Model
{
    public sealed class Cooker : Individual
    {
        #region
        public Order OrderGenerated;
        #endregion

        public Cooker(string firstName, string lastName) : base(firstName,lastName) { }

        public override Order ReceiveCommand<Order>()
        {
            return Receiver.Receive<Order>("clerk-cooker");
        }

        public override bool SendCommand()
        {
            return Publisher.Publish<Order>(OrderGenerated, "cooker-deliveryman");
        }
    }
}
