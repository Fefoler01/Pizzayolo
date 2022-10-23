using System;
using Pizzayolo.MessageBroker.Consumer;
using Pizzayolo.MessageBroker.Producer;
using Pizzayolo.Tables;

namespace Pizzayolo.Model
{
    public class Admin : Individual
    {
        public Admin()
        {
        }

        public override Client ReceiveCommand<Client>()
        {
            throw new NotImplementedException();
        }

        public override bool SendCommand()
        {
            throw new NotImplementedException();
        }

        public override Client ReceiveSupervision<Client>() {
            throw new NotImplementedException();
        }

        public override bool SendSupervision() {
            throw new NotImplementedException();
        }
    }
}

