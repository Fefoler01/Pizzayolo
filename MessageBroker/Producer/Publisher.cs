using Apache.NMS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pizzayolo.MessageBroker.Producer
{
    public sealed class Publisher : PublisherInterface
    {
        public static bool Publish<T>(T message, string nameQueue)
        {
            
            NMSConnectionFactory factory = new NMSConnectionFactory("activemq:tcp://localhost:61616");
            using (IConnection connection = factory.CreateConnection())
            {
                connection.Start();

                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetQueue(nameQueue))
                using (IMessageProducer producer = session.CreateProducer(dest))
                {
                    producer.DeliveryMode = MsgDeliveryMode.Persistent;
                    producer.Send(session.CreateTextMessage(JsonConvert.SerializeObject(message)));
                    
                }
            }

            return true;
        }

        public static bool PublishTopic<T>(T message, string nameTopic)
        {

            NMSConnectionFactory factory = new NMSConnectionFactory("activemq:tcp://localhost:61616");
            using (IConnection connection = factory.CreateConnection())
            {
                connection.Start();

                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetTopic(nameTopic))
                using (IMessageProducer producer = session.CreateProducer(dest))
                {
                    producer.DeliveryMode = MsgDeliveryMode.Persistent;
                    producer.Send(session.CreateTextMessage(JsonConvert.SerializeObject(message)));

                }
            }

            return true;
        }
    }
}
