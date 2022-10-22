using System;

namespace Pizzayolo.MessageBroker.Consumer {
    public interface IReceiver {
        static T Receive<T>(string nameQueue) => throw new NotImplementedException();
    }
}
