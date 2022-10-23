// IndividualInterface

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzayolo.Tables
{
    // IndividualInterface is an interface, it's not possible to instantiate it, but it's possible to inherit from it.
    public interface IndividualInterface
    {
        public bool SendCommand();

        public T ReceiveCommand<T>();
    }
}
