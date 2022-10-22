// Kitchen { id, cookOrder(id)}

using System;

namespace Pizzayolo.Tables
{
    public class Kitchen : Person
    {
        #region Fields
        private uint _id;
        #endregion

        #region Properties
        public uint Id { get => _id; set => _id = value; }
        #endregion

        #region Constructors
        public Kitchen()
        {
            Id = 0;
        }

        public Kitchen(uint id)
        {
            Id = id;
        }
        #endregion

        #region Methods
        public void CookOrder(uint id)
        {
            Console.WriteLine("Order " + id + " is being cooked");
        }

        public override bool SendMessage()
        {
            throw new NotImplementedException();
        }

        public override T ReceiveMessage<T>()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}