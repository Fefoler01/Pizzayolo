using Pizzayolo.MessageBroker.Consumer;
using Pizzayolo.MessageBroker.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pizzayolo.Model
{
    public sealed class DeliveryMan : Individual
    {

        #region Properties
        public Order Order{ get; set; }
        #endregion

        #region Constructors
        public DeliveryMan(string firstName, string lastName ) : base(firstName,lastName )
        {
          
           
        }
        #endregion

        #region Methods
        public override bool SendCommand()
        {
            return Publisher.Publish<Order>(Order,"deliveryman-client");
        }

        public override Order ReceiveCommand<Order>()
        {
            return Receiver.Receive<Order>("cooker-deliveryman");
        }
        #endregion 
    }
}
