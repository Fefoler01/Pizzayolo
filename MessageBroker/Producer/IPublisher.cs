using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks; 

namespace Pizzayolo.MessageBroker {
    public interface IPublisher {
        static bool Publish<T>(T message, string nameQueue) => throw new NotImplementedException();
    }
}
