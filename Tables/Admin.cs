using System;
using System.Collections.Generic;
using Pizzayolo.MessageBroker.Consumer;
using Pizzayolo.MessageBroker.Producer;
using Pizzayolo.Tables;

namespace Pizzayolo.Model
{
    public class Admin : Individual
    {

        Dictionary<DeliveryMan, int> numberOrderDeliveryMan;
        Dictionary<Clerk, int> numberOrderClerk;
        List<Order> orderList;
        List<Client> clientList;

        public Admin() {
            this.numberOrderDeliveryMan = new Dictionary<DeliveryMan, int>();
            this.numberOrderClerk = new Dictionary<Clerk, int>();
            this.orderList = new List<Order>();
            this.clientList = new List<Client>();
        }

        public bool VerifyFirstOrderClient(Client client)
        {

            foreach (Client savedclient in clientList)
            {
                if (savedclient.firstName == client.firstName && savedclient.lastName == client.lastName && savedclient.address == client.address && savedclient.phoneNumber == client.phoneNumber && savedclient.dateFirstOrder != DateTime.MinValue)
                {
                    client.dateFirstOrder = savedclient.dateFirstOrder;
                    return true;
                }
            }

            client.dateFirstOrder = DateTime.Now;
            clientList.Add(client);
            return false;
        }

        public void AddOrder(Order order)
        {
            orderList.Add(order);
        }

        public void NewOrderDeliveryMan(DeliveryMan d)
        {
            numberOrderDeliveryMan[d] = numberOrderDeliveryMan[d] + 1;
        }

        public void NewOrderClerk(Clerk c)
        {
            numberOrderClerk[c] = numberOrderClerk[c] + 1;
        }

        public override Client ReceiveCommand<Client>()
        {
            throw new NotImplementedException();
        }

        public override bool SendCommand()
        {
            throw new NotImplementedException();
        }

        public override Client ReceiveSupervision<Client>() {
            throw new NotImplementedException();
        }

        public override bool SendSupervision() {
            throw new NotImplementedException();
        }
    }
}

