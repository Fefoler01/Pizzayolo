using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzayolo.Model
{
    public interface IndividualInterface
    {
        public bool SendCommand();

        public T ReceiveCommand<T>();

    }
}
