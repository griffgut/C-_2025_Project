using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Spring2025_Samples.Models;

namespace Library.eCommerce.Services
{
    public class ShoppingServiceProxy
    {
        private List<Product?> items;
        private ShoppingServiceProxy()
        {
            
            Cart = new List<Product?>();
        }
        private int LastKey
        {
            get
            {
                if (!Cart.Any())
                {
                    return 0;
                }

                return Cart.Select(p => p?.Id ?? 0).Max();
            }
        }

        private static ShoppingServiceProxy? instance;
        private static object instanceLock = new object();
        public static ShoppingServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ShoppingServiceProxy();
                    }
                }

                return instance;
            }
        }
        public List<Product?> CartItems
        {
            get
            {
                return items;
            }
        }


        public Product AddOrUpdate(Product product)
        {
            int buy_num = 0;
            int up_num = 0;

            Product copy = new Product();
            // need to check if product is in cart already before adding
            if (product.Id == 0)   // ADD
            {
                Console.Write("How much of this product do you want to buy? ");
                buy_num = int.Parse(Console.ReadLine() ?? "-1");
                if(product.Quantity >= buy_num)
                {
                    copy.Quantity = buy_num;
                    copy.Price = product.Price;
                    copy.Id = LastKey + 1;
                    copy.Name = product.Name;
                    product.Quantity = product.Quantity - buy_num;
                    if(product.Quantity == 0)
                    {
                        c_inv.Current.Delete(product.Id);
                    }
                    Cart.Add(copy);
                }
                else
                {
                    Console.WriteLine("There isn't enough of this item to buy for the specified amount");
                }
            }
            else
            {
                int diff;
                copy = new Product();
                Console.Write("How much of this item do you want to add? ");
                up_num = int.Parse(Console.ReadLine() ?? "-1");
                string? name = product.Name;
                diff = product.Quantity - up_num;
                foreach (var item in ProductServiceProxy.Current.Products)
                {
                    if (item.Name == product.Name)
                    {
                        if (item.Quantity >= up_num) // product here is a shopping cart object
                        {
                            if (item.Quantity != 0)
                            {
                                item.Quantity = item.Quantity + diff;
                                copy = item;
                                copy.Quantity = up_num;
                            }
                        }
                        else
                        {
                            Console.WriteLine("There isn't enough of this item to buy for the specified amount");
                        }
                    }
                    else
                    {
                        Console.WriteLine("There is no product named " + product.Name + "in the inventory");
                    }
                }

            }
            return copy;

        }

        public Product? Delete(int id)
        {
            if (id == 0)
            {
                return null;
            }

            Product? product = Cart.FirstOrDefault(p => p.Id == id);
            Cart.Remove(product);

            return product;
        }

    }

}

