using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzayolo.Model
{
    public class Pizza
    {
        #region Properties
        public PizzaSize size { get ; set; }
        public PizzaKind kind { get; set; }
        public double price { get; set; }
        #endregion

        #region Constructor
        public Pizza(PizzaSize size, PizzaKind kind, double price) {
            this.size = size;
            this.kind = kind;
            this.price = price;
        }
        #endregion

        #region Method
        public override string ToString() {
            return "Pizza(kind: " + kind.ToString() + ", size: " + size.ToString() + ", price: " + price.ToString() + "€)";
        }
        #endregion
    }
}
