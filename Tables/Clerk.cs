// Clerk { id_clerk, totalOrder, disponibility, isDispo(id_clerk), takeOrder(id_client), updateOrder(id_order), validateOrder(facture,id_order,id_deliveryman), sendMessage(), receiveMessage()}

using System;

namespace Pizzayolo.Tables
{
    public class Clerk : Person
    {
        #region Fields
        private uint _id;
        private uint _totalOrder;
        private bool _disponibility;
        #endregion

        #region Properties
        public uint Id { get => _id; set => _id = value; }
        public uint TotalOrder { get => _totalOrder; set => _totalOrder = value; }
        public bool Disponibility { get => _disponibility; set => _disponibility = value; }
        #endregion

        #region Constructors
        public Clerk()
        {
            Id = 0;
            TotalOrder = 0;
            Disponibility = true;
        }

        public Clerk(uint id, bool disponibility)
        {
            Id = id;
            Disponibility = disponibility;
        }
        #endregion

        #region Methods
        public void TakeOrder(uint idClient)
        {
            if (Disponibility)
            {
                Console.WriteLine("Order " + idClient + " is being taken");
            }
            else
            {
                Console.WriteLine("Order " + idClient + " is not being taken");
            }
        }

        public void UpdateOrder(uint idOrder)
        {
            Console.WriteLine("Order " + idOrder + " is being updated");
        }

        public void ValidateOrder(uint facture, uint idOrder, uint idDeliveryman)
        {
            Console.WriteLine("Order " + idOrder + " is being validated");
        }

        public override bool SendMessage()
        {
            throw new NotImplementedException();
        }

        public override T ReceiveMessage<T>()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Id : " + Id
                + "\nTotalOrder : " + TotalOrder;
        }
        #endregion
    }
}