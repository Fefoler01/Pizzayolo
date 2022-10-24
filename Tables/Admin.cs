using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Pizzayolo.MessageBroker.Consumer;
using Pizzayolo.MessageBroker.Producer;
using Pizzayolo.Tables;

namespace Pizzayolo.Model
{
    public class Admin : Individual
    {

        Dictionary<DeliveryMan, int> numberOrderDeliveryMan;
        Dictionary<Clerk, int> numberOrderClerk;
        Dictionary<Client, int> numberOrderClient;
        Dictionary<Kitchen, int> numberOrderKitchen;
        List<Order> orderList;
        List<Client> clientList;
        List<Clerk> clerkList;
        List<DeliveryMan> deliveryManList;
        List<Kitchen> kitchenList;

        public Admin() { }

        public Admin(string firstName, string lastName) : base(firstName, lastName) {
            this.numberOrderDeliveryMan = new Dictionary<DeliveryMan, int>();
            this.numberOrderClerk = new Dictionary<Clerk, int>();
            this.orderList = new List<Order>();
            this.clientList = new List<Client>();
            this.clerkList = new List<Clerk>();
            this.deliveryManList = new List<DeliveryMan>();
            this.kitchenList = new List<Kitchen>();
        }

        public void AddClient(Client client) { // implementation of signal

            foreach (Client savedclient in clientList) {
                if (savedclient.firstName == client.firstName && savedclient.lastName == client.lastName && savedclient.address == client.address && savedclient.phoneNumber == client.phoneNumber && savedclient.dateFirstOrder != DateTime.MinValue) {
                    client.dateFirstOrder = savedclient.dateFirstOrder;
                    return;
                }
            }

            client.dateFirstOrder = DateTime.Now;
            clientList.Add(client);
            return;
        }

        public void AddClerk(Clerk clerk) { // implementation of signal

            foreach (Clerk savedclerk in clerkList) {
                if (savedclerk.firstName == clerk.firstName && savedclerk.lastName == clerk.lastName) {
                    return;
                }
            }

            clerkList.Add(clerk);
            return;
        }

        public void AddDeliveryMan(DeliveryMan deliveryMan) { // implementation of signal

            foreach (DeliveryMan saveddeliveryMan in deliveryManList) {
                if (saveddeliveryMan.firstName == deliveryMan.firstName && saveddeliveryMan.lastName == deliveryMan.lastName) {
                    return;
                }
            }

            deliveryManList.Add(deliveryMan);
            return;
        }

        public void AddKitchen(Kitchen kitchen) { // implementation of signal

            foreach (Kitchen savedkitchen in kitchenList) {
                if (savedkitchen.firstName == kitchen.firstName && savedkitchen.lastName == kitchen.lastName) {
                    return;
                }
            }

            kitchenList.Add(kitchen);
            return;
        }

        public void AddOrder(Order order) { // implementation of signal
            orderList.Add(order);
        }

        //Client
        public void SortClientByName() {
            this.clientList.Sort(delegate (Client x, Client y) {
                return x.firstName.CompareTo(y.firstName);
            });
        }

        public void SortClientByAddress() {
            this.clientList.Sort(delegate (Client x, Client y) {
                return x.address.CompareTo(y.address);
            });
        }

        public void SortClientByTotalPrice() {
            this.clientList.Sort(delegate (Client x, Client y) {
                return GetTotalPriceOrder(x).CompareTo(GetTotalPriceOrder(y));
            });
        }

        public double GetTotalPriceOrder(Client client)
        {
            double totalPrice = 0;
            foreach (Order savedorder in orderList)
            {
                Client savedclient = savedorder.client;
                if (savedclient.firstName == client.firstName && savedclient.lastName == client.lastName)
                {
                    totalPrice += savedorder.items.totalPrice();
                }
            }
            return totalPrice;
        }

        public void ShowTotalPriceOrder(Client client)
        {
            Console.WriteLine("\nThe client " + client.firstName + " " + client.lastName + " paid a total of " + GetTotalPriceOrder(client) + " € to the pizzeria.");
        }

        public void ShowAveragePriceOrder()
        {
            double totalPrice = 0;
            foreach (Client client in clientList)
            {
                totalPrice += GetTotalPriceOrder(client);
            }
            totalPrice /= clientList.Count;
            Console.WriteLine("\nThe average cumulate paid by clients is " + totalPrice + " €.");
        }

        //Clerk
        public void SortClerkByName() {
            this.clerkList.Sort(delegate (Clerk x, Clerk y) {
                return x.firstName.CompareTo(y.firstName);
            });
        }

        public void SortClerkByTotalPrice() {
            this.clerkList.Sort(delegate (Clerk x, Clerk y) {
                return GetTotalPriceOrder(x).CompareTo(GetTotalPriceOrder(y));
            });
        }

        public double GetTotalPriceOrder(Clerk clerk) {
            double totalPrice = 0;
            foreach (Order savedorder in orderList) {
                Clerk savedclerk = savedorder.clerk;
                if (savedclerk.firstName == clerk.firstName && savedclerk.lastName == clerk.lastName) {
                    totalPrice += savedorder.items.totalPrice();
                }
            }
            return totalPrice;
        }

        public void ShowTotalPriceOrder(Clerk clerk) {
            Console.WriteLine("\nThe clerk " + clerk.firstName + " " + clerk.lastName + " sold a total of " + GetTotalPriceOrder(clerk) + " € for the pizzeria.");
        }



        public void ShowOrderListDate(DateTime start, DateTime end)
        {
            if (start <= end)
            {
                int nbOrderDate = 0;
                foreach (Order order in orderList)
                {
                    if (order.orderSchedule >= start && order.orderSchedule <= end)
                    {
                        nbOrderDate++;
                        order.ToString();
                    }
                }
                Console.WriteLine("\nWe find " + nbOrderDate + " order between " + start + " and " + end);
            }
            else
            {
                Console.WriteLine("\nDate error, first date must be before or the same as the second date.");
            }
        }

        public void ShowMoyPriceOrder()
        {
            double totalPrice = 0;
            foreach (Order order in orderList)
            {
                totalPrice += order.items.totalPrice();
            }
            totalPrice /= orderList.Count;
            Console.WriteLine("\nThe average price of an order is " + totalPrice + " €.");
        }

        public void NewOrderDeliveryMan(DeliveryMan d) { // implementation of signal
            numberOrderDeliveryMan[d] = numberOrderDeliveryMan[d] + 1;
        }

        public void NewOrderClerk(Clerk c) { // implementation of signal
            numberOrderClerk[c] = numberOrderClerk[c] + 1;
        }

        public void NewOrderClient(Client c)
        { // implementation of signal
            numberOrderClient[c] = numberOrderClient[c] + 1;
        }

        public void NewOrderKitchen(Kitchen k)
        { // implementation of signal
            numberOrderKitchen[k] = numberOrderKitchen[k] + 1;
        }

        // Signal
        public override T ReceiveCommand<T>() {
            throw new NotImplementedException();
        }

        public override bool SendCommand() {
            throw new NotImplementedException();
        }

        public override T ReceiveSupervision<T>() {
            throw new NotImplementedException();
        }

        public Order ReceiveSupervisionAddOrder<Order>()
        {
            return Receiver.ReceiveTopic<Order>("order-admin-add");
        }

        public DeliveryMan ReceiveSupervisionNewOrderDeliveryMan<DeliveryMan>() {
            return Receiver.ReceiveTopic<DeliveryMan>("deliveryman-admin");
        }

        public DeliveryMan ReceiveSupervisionAddDeliveryMan<DeliveryMan>() {
            return Receiver.ReceiveTopic<DeliveryMan>("deliveryman-admin-add");
        }

        public Clerk ReceiveSupervisionNewOrderClerk<Clerk>() {
            return Receiver.ReceiveTopic<Clerk>("clerk-admin");
        }

        public Clerk ReceiveSupervisionAddClerk<Clerk>()
        {
            return Receiver.ReceiveTopic<Clerk>("clerk-admin-add");
        }

        public Client ReceiveSupervisionNewOrderClient<Client>() {
            return Receiver.ReceiveTopic<Client>("client-admin");
        }

        public Client ReceiveSupervisionAddClient<Client>()
        {
            return Receiver.ReceiveTopic<Client>("client-admin-add");
        }

        public Kitchen ReceiveSupervisionNewOrderKitchen<Kitchen>() {
            return Receiver.ReceiveTopic<Kitchen>("kitchen-admin");
        }

        public Kitchen ReceiveSupervisionAddKitchen<Kitchen>() {
            return Receiver.ReceiveTopic<Kitchen>("kitchen-admin-add");
        }

        public override bool SendSupervision() {
            throw new NotImplementedException();
        }
    }
}

