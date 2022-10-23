// OrderItems is a list of Item (pizzas and snacks).

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzayolo.Tables
{
    public sealed class OrderItems
    {
        // Properties
        public static double snackPrice = 2.0;
        public List<Pizza> pizzas { get; set; }
        public List<Snacks> snacks { get; set; }

        // Constructor
        public OrderItems(List<Pizza> pizzas, List<Snacks> snacks) {
            this.pizzas = pizzas;
            this.snacks = snacks;
        }

        // Methods
        public double totalPrice() {
            double total = 0.0;

            pizzas.ForEach(pizza => total += pizza.price);
            snacks.ForEach(snack => total += snackPrice);
            
            return total;
        }

        public string Invoice() {
            return "Price : " + totalPrice() + "\n" + ToString();
        }

        public override string ToString() {
            string listPizzas = "", listSnacks = "";

            pizzas.ForEach(e => listPizzas += e.ToString() + ' ');
            snacks.ForEach(e => listSnacks += e.ToString() + ' ');

            return listPizzas + "\n" + listSnacks;
        }
    }
}
