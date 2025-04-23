using Library.eCommerce.Models;
using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Services
{
    public class ProductServiceProxy
    {
        private ProductServiceProxy()
        {
            Products = new List<Item?>() {
            new Item { Product = new Product { Id = 1, Name = "Product 1" }, Id = 1, Quantity = 1, Price = 2 },
            new Item {Product = new Product{Id = 2, Name = "Product 2"}, Id = 2, Quantity = 25, Price = 3 },
            };
        }

        private int LastKey
        {
            get
            {
                if (!Products.Any())
                {
                    return 0;
                }

                return Products.Select(p => p?.Id ?? 0).Max();
            }
        }

        private static ProductServiceProxy? instance;
        private static object instanceLock = new object();
        public static ProductServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductServiceProxy();
                    }
                }

                return instance;
            }
        }

        public List<Item?> Products { get; private set; }


        public Item AddOrUpdate(Item item)
        {
            if (item.Id == 0)
            {
                item.Id = LastKey + 1;
                item.Product.Id = item.Id;
                Products.Add(item);
            }
            else
            {
                var existingItem = Products.FirstOrDefault(p => p.Id == item.Id);
                //Products.Add(new Item(item));
                //existingItem = new Item(item);
                var idx = Products.IndexOf(existingItem);
                Products.RemoveAt(idx);
                Products.Insert(idx, new Item(item));
            }

            return item;
        }

        public Item? Delete(int id)
        {
            if (id == 0)
            {
                return null;
            }

            Item? product = Products.FirstOrDefault(p => p.Id == id);
            Products.Remove(product);

            return product;
        }
        public Item? GetById(int id)
        {
            return Products.FirstOrDefault(p => p.Id == id);
        }
        public Item? PurchaseItem(Item? item)
        {
            if(item?.Id <= 0 || item == null)
            {
                return null;
            }
            var itemToPurchase = GetById(item.Id);
            if(itemToPurchase != null)
            {
                itemToPurchase.Quantity--;
            }

            return itemToPurchase;
        }
    }

    
}
