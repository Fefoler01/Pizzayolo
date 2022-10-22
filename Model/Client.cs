using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;


using Pizzayolo.MessageBroker.Producer;

namespace Pizzayolo.Model
{
    public sealed class Client : Individual
    {
        #region Properties
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateFirstOrder { get; set; }
        #endregion

        #region Constructors
        public Client() { }

        public Client(string firstName, string lastName, string adress, string phoneNumber) : base(firstName,lastName) {
            Adress = adress;
            PhoneNumber = phoneNumber;
        }

        public Client(string firstName, string lastName, string adress, string phoneNumber, DateTime dateFirstOrder) : this(firstName, lastName, adress,  phoneNumber) {
            DateFirstOrder = dateFirstOrder;
        }

        public Client(Client other) : this(other.firstName, other.lastName, other.Adress, other.PhoneNumber, other.DateFirstOrder) { }
        #endregion

        #region Methods
        public override string ToString() {
            return base.ToString()
                + "\nAdress : " + Adress
                + "\nPhone Number" + PhoneNumber
                + "\nDate First Order" + DateFirstOrder.ToString();
                
        }

        public override bool SendCommand() {
              return Publisher.Publish<Client>(this, "client-clerk"); 
        }

        public override T ReceiveCommand<T>() {
            throw new NotImplementedException();
        }
        #endregion
    }
}
