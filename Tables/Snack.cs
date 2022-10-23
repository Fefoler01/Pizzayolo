// Snack is already composed.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzayolo.Tables
{
    public class Snack
    {
        // Properties
        public Snacks kind { get; set; }
        public double price { get; set; }

        // Constructor
        public Snack(Snacks kind) {
            this.kind = kind;
            this.price = 2;
        }

        // Method
        public override string ToString() {
            return "Pizza(kind: " + kind.ToString() + ", price: " + price.ToString() + "€)";
        }
    }
}
