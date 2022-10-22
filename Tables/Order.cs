// Order { id, Date, Status, FinalState, calcSubTotal(), calcTax(), calcTotal(), calcNumberArticles()}

using System;

namespace Pizzayolo.Tables
{
    public class Order
    {
        #region Enums
        public enum Status
        {
            Prepartion,
            Delivery,
            Delivered
        }
        #endregion
        
        #region Fields
        private uint _number;
        private DateTime _orderSchedule;
        private Status _state;
        private string _finalState;
        private string _nameClerk;
        private string _nameClient;
        #endregion

        #region Properties
        public uint Number { get => _number; set => _number = value; }
        public DateTime OrderSchedule { get => _orderSchedule; set => _orderSchedule = value; }
        public Status State { get => _state; set => _state = value; }
        public string FinalState { get => _finalState; set => _finalState = value; }
        public string NameClerk { get => _nameClerk; set => _nameClerk = value; }
        public string NameClient { get => _nameClient; set => _nameClient = value; }
        #endregion

        #region Constructors
        public Order()
        {
            Number = 0;
            OrderSchedule = DateTime.Now;
            State = Status.Prepartion;
            FinalState = string.Empty;
            NameClerk = string.Empty;
            NameClient = string.Empty;
        }

        public Order(uint number,
            DateTime orderSchedule,
            Status state,
            string finalState,
            string nameClerk,
            string nameClient)
        {
            Number = number;
            OrderSchedule = orderSchedule;
            State = state;
            FinalState = finalState;
            NameClerk = nameClerk;
            NameClient = nameClient;
        }

        public Order(uint number,
            DateTime orderSchedule,
            string nameClerk,
            string nameClient)
        {
            Number = number;
            OrderSchedule = orderSchedule;
            State = Status.Prepartion;
            FinalState = string.Empty;
            NameClerk = nameClerk;
            NameClient = nameClient;
        }

        public Order(uint number,
            DateTime orderSchedule,
            string nameClerk)
        {
            Number = number;
            OrderSchedule = orderSchedule;
            State = Status.Prepartion;
            FinalState = string.Empty;
            NameClerk = nameClerk;
            NameClient = string.Empty;
        }
        #endregion

        #region Methods
        public double calcSubTotal()
        {
            return 0;
        }

        public double calcTax()
        {
            return 0;
        }

        public double calcTotal()
        {
            return 0;
        }

        public int calcNumberArticles()
        {
            return 0;
        }

        public override string ToString()
        {
            return $"Order: {Number}, {OrderSchedule}, {State}, {FinalState}, {NameClerk}, {NameClient}";
        }
        #endregion

    }
}