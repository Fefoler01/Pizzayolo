// Pizza is already composed and can have different sizes.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzayolo.Model
{
    public class Pizza
    {
        // Properties
        public PizzaSize size { get ; set; }
        public PizzaKind kind { get; set; }
        public double price { get; set; }

        // Constructor
        public Pizza(PizzaSize size, PizzaKind kind) {
            this.size = size;
            this.kind = kind;
            
            switch (kind)
            {
                case PizzaKind.Margarita:
                    this.price = 10;
                    break;

                case PizzaKind.Hawaïan:
                    this.price = 12;
                    break;

                case PizzaKind.Regina:
                    this.price = 17;
                    break;

                case PizzaKind.FourSeasons:
                    this.price = 15;
                    break;

                default:
                    this.price = 0;
                    break;
            }

            switch (size)
            {
                case PizzaSize.Small:
                    this.price = this.price + 0;
                    break;

                case PizzaSize.Medium:
                    this.price = this.price + 3;
                    break;

                case PizzaSize.Large:
                    this.price = this.price + 5;
                    break;

                default:
                    this.price = 0;
                    break;
            }
        }

        // Method
        public override string ToString() {
            return "Pizza(kind: " + kind.ToString() + ", size: " + size.ToString() + ", price: " + price.ToString() + "€)";
        }
    }
}
