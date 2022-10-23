using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks; 

namespace Pizzayolo.MessageBroker {
    public interface PublisherInterface {
        static bool Publish<T>(T message, string nameQueue) => throw new NotImplementedException();
        static bool PublishTopic<T>(T message, string nameTopic) => throw new NotImplementedException();
    }
}
