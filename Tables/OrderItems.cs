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
        public List<Pizza> pizzas { get; set; }
        public List<Snack> snacks { get; set; }

        // Constructor
        public OrderItems(List<Pizza> pizzas, List<Snack> snacks) {
            this.pizzas = pizzas;
            this.snacks = snacks;
        }

        // Methods
        public double totalPrice() {
            double total = 0.0;

            pizzas.ForEach(pizza => total += pizza.price);
            snacks.ForEach(snack => total += snack.price);
            
            return total;
        }

        public string Invoice() {
            return "Price : " + totalPrice() + "\n" + ToString();
        }

        public override string ToString() {
            string listPizzas = "", listSnacks = "";

            pizzas.ForEach(pizza => listPizzas += pizza.ToString() + ' ');
            snacks.ForEach(snack => listSnacks += snack.ToString() + ' ');

            return listPizzas + "\n" + listSnacks;
        }
    }
}
