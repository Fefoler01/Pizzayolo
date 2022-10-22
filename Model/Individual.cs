using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Threading;

namespace Pizzayolo.Model
{
    public abstract class Individual : IndividualInterface
    {
        #region Properties
        public string firstName { get; set; }
        public string lastName { get; set; }
        #endregion

        #region Constructors
        public Individual() { }

        public Individual(string firstName, string lastName) {
            this.firstName = firstName;
            this.lastName = lastName;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return "Individual(firstName: " + firstName + ", lastName: " + lastName + ")";
        }
        public abstract bool SendCommand();
        public abstract T ReceiveCommand<T>();

        #endregion
    }
}
