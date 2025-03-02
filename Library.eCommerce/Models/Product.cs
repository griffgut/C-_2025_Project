using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spring2025_Samples.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Quantity {  get; set; }  // track the quantity of a product
        public double Price {  get; set; }
        public string? Display
        {
            get
            {
                return $"{Id}. {Name} Total: {Quantity} Price: ${Price}";
            }
        }

        public Product()
        {
            Name = string.Empty;
            Quantity = 0;
            Price = 0;
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
}
