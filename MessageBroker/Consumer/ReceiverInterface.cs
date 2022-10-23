using System;

namespace Pizzayolo.MessageBroker.Consumer {
    public interface ReceiverInterface {
        static T Receive<T>(string nameQueue) => throw new NotImplementedException();
        static T ReceiveTopic<T>(string nameTopic) => throw new NotImplementedException();
    }
}
