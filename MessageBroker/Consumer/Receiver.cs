using Apache.NMS;
using Newtonsoft.Json;
using System;


namespace Pizzayolo.MessageBroker.Consumer
{
    public sealed class Receiver : IReceiver
    {
        public static T Receive<T>(string nameQueue)
        {
            NMSConnectionFactory factory = new NMSConnectionFactory("activemq:tcp://localhost:61616");
            using (IConnection connection = factory.CreateConnection())
            {
                connection.Start();
                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetQueue(nameQueue))
                using (IMessageConsumer consumer = session.CreateConsumer(dest))
                {
                    IMessage msg = consumer.Receive();
                    if (msg is ITextMessage)
                    {
                        ITextMessage txtMsg = msg as ITextMessage;
                        string body = txtMsg.Text;
                        return JsonConvert.DeserializeObject<T>(body); 
                    }
                    else
                    {
                        Console.WriteLine("Unexpected message type: " + msg.GetType().Name);
                    }
                }
            }
            return default;  
        }
    }
}
