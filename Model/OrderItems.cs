using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzayolo.Model
{
    public sealed class OrderItems
    {
        #region Properties
        public static double drinkPrice = 2.0;
        public List<Pizza> pizzas { get; set; }
        public List<Drinks> drinks { get; set; }
        #endregion

        #region Constructor
        public OrderItems(List<Pizza> pizzas, List<Drinks> drinks) {
            this.pizzas = pizzas;
            this.drinks = drinks;
        }
        #endregion

        #region Methods
        public double totalPrice() {
            double total = 0.0;
            pizzas.ForEach(p => total += p.price);
            drinks.ForEach(d => total += drinkPrice);
            return total;
        }

        public string Invoice() {
            return "Price : " + totalPrice() + "\n" + ToString();
        }

        public override string ToString() {
            string listPizzas = "", listDrinks = "";

            pizzas.ForEach(e => listPizzas += e.ToString() + ' ');
            drinks.ForEach(e => listDrinks += e.ToString() + ' ');

            return listPizzas + "\n" + listDrinks;
        }
        #endregion
    }

}
