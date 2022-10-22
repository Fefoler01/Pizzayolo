// DeliveryMan { Id, Date, StatusDelivery, totalDelivery, receiveOrder(id), giveOrder(id), open(), close(), sendMessage(), receiveMessage()}

using System;

namespace Pizzayolo.Tables
{
    public class DeliveryMan : Person
    {
        #region Enums
        public enum StatusDelivery
        {
            InProgress,
            Open,
            Close
        }
        #endregion

        #region Fields
        private uint _id;
        private DateTime _date;
        private StatusDelivery _stateDelivery;
        private uint _totalDelivery;
        #endregion

        #region Properties
        public uint Id { get => _id; set => _id = value; }
        public DateTime Date { get => _date; set => _date = value; }
        public StatusDelivery StateDelivery { get => _stateDelivery; set => _stateDelivery = value; }
        public uint TotalDelivery { get => _totalDelivery; set => _totalDelivery = value; }
        #endregion

        #region Constructors
        public DeliveryMan()
        {
            Id = 0;
            Date = DateTime.Now;
            StateDelivery = StatusDelivery.Close;
            TotalDelivery = 0;
        }

        public DeliveryMan(uint id, DateTime date, StatusDelivery stateDelivery, uint totalDelivery)
        {
            Id = id;
            Date = date;
            StateDelivery = stateDelivery;
            TotalDelivery = totalDelivery;
        }
        #endregion

        #region Methods
        public void ReceiveOrder(uint id)
        {
            Console.WriteLine("Order " + id + " is being delivered");
        }

        public void GiveOrder(uint id)
        {
            Console.WriteLine("Order " + id + " is delivered");
        }

        public void Open()
        {
            StateDelivery = StatusDelivery.Open;
        }

        public void Close()
        {
            StateDelivery = StatusDelivery.Close;
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