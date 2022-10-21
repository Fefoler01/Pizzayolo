// Person {abstract id, Surname, Lastname, sendMessage(), receiveMessage()}

namespace Pizzayolo.Tables
{
    public abstract class Person
    {
        #region Fields
        private uint _id;
        private string _surname;
        private string _lastname;
        #endregion

        #region Properties
        public uint Id { get => _id; set => _id = value; }
        public string Surname { get => _surname; set => _surname = value; }
        public string Lastname { get => _lastname; set => _lastname = value; }
        #endregion

        #region Constructors
        public Person()
        {
            Id = 0;
            Surname = string.Empty;
            Lastname = string.Empty;
        }

        public Person(uint id, string surname, string lastname)
        {
            Id = id;
            Surname = surname;
            Lastname = lastname;
        }
        #endregion

        #region Methods
        public abstract bool SendMessage();
        public abstract T ReceiveMessage<T>();
        
        public override string ToString()
        {
            return "Id : " + Id
                + "\nSurname : " + Surname
                + "\nLastname : " + Lastname;
        }
        #endregion
    }
}