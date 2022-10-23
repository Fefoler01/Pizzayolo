// Individual defines the common properties and methods for all the individuals in the system. It implements the IIndividual interface.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace Pizzayolo.Model
{
    // Individual is an abstract class, it's not possible to instantiate it, but it's possible to inherit from it.
    public abstract class Individual : IndividualInterface
    {
        // Properties
        public string firstName { get; set; }
        public string lastName { get; set; }

        // Constructors
        public Individual() { }

        public Individual(string firstName, string lastName) {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        // Methods
        public override string ToString() {
            return "Individual(firstName: " + firstName + ", lastName: " + lastName + ")";
        }

        public abstract bool SendCommand();

        public abstract T ReceiveCommand<T>();
    }
}
