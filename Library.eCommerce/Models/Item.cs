using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;

namespace Library.eCommerce.Models
{
    public class Item
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int? Quantity {  get; set; }
        public double? Price { get; set; }
        public override string ToString()
        {
            return $"{Product} Price: ${Price} Quantity: {Quantity}";
        }
        public string Display
        {
            get
            {
                return $"{Product?.Display ?? string.Empty} Price: ${Price} Quantity: {Quantity}";
            }
        }
        public Item() 
        {
            Product = new Product();
            Quantity = 0;
            Price = 0;
        }
        private void DoAdd()
        {
            ShoppingServiceProxy.Current.AddOrUpdate(this);
        }
        public Item(Item i)
        {
            Product = new Product(i.Product);
            Id = i.Id;
            Quantity = i.Quantity;
            Price = i.Price;
        }
    }
}
