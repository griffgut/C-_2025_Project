using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using Spring2025_Samples.Models;

namespace Library.eCommerce.Services
{
    public class ShoppingServiceProxy
    {
        private List<Item?> items;
        private ProductServiceProxy products = ProductServiceProxy.Current;
        public List<Item?> CartItems
        {
            get
            {
                return items;
            }
        }
        private ShoppingServiceProxy()
        {
            items = new List<Item?>();
        }
        /*
        private int LastKey
        {
            get
            {
                if (!items.Any())
                {
                    return 0;
                }

                return items.Select(p => p?.Id ?? 0).Max();
            }
        }
        */
        private static ShoppingServiceProxy? instance;
        public static ShoppingServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                        instance = new ShoppingServiceProxy();
                }

                return instance;
            }
        }

        public Item? AddOrUpdate(Item item)
        {
            var existingInvItem = products.GetById(item.Product.Id);
            if (existingInvItem == null || existingInvItem.Quantity == 0)
            {
                return null;
            }
            if (existingInvItem != null)    // subtract from inventory
            {
                existingInvItem.Quantity--;

            }
            var existingItem = CartItems.FirstOrDefault(p => p.Product.Id == item.Product.Id);
            if (existingItem == null)   // if item not in shopping cart/Add
            {
                var newItem = new Item(item);
                newItem.Quantity = 1;
                CartItems.Add(newItem);
            }
            else   // if item exists in cart/Update
            {
                existingItem.Quantity++;
            }
            return existingInvItem;
        }
        // Return for assignment one
        // Returns all items at once
        public Item? Delete(Item? item)
        {
            if (item?.Product.Id <= 0 || item == null)
            {
                return null;
            }
            var itemReturn = CartItems.FirstOrDefault(p => p.Product.Id == item.Product.Id);
            if (itemReturn != null)
            {
                //itemReturn.Quantity--;
               
                var invItem = products.Products.FirstOrDefault(p => p.Id == itemReturn.Product.Id);
                if (invItem == null)
                {
                    products.AddOrUpdate(new Item(itemReturn));
                }
                else
                {
                    invItem.Quantity += itemReturn.Quantity;
                }

                CartItems.Remove(itemReturn); // delete return item because we return all items in of that id
            }
            return itemReturn;
        }
        // Assignment 2 return returns one item at a time
        public Item? ReturnItem(Item? item)
        {
            if (item?.Id <= 0 || item == null)
            {
                return null;
            }
            var itemToReturn = CartItems.FirstOrDefault(c => c.Id == item.Id);
            if (itemToReturn != null)
            {
                itemToReturn.Quantity--;
                var invItem = products.Products.FirstOrDefault(p=>p.Id == itemToReturn.Id);
                if (invItem == null) 
                {
                    products.AddOrUpdate(new Item(itemToReturn));
                }
                else
                {
                    invItem.Quantity++;
                }
            }

            return itemToReturn;
        }
    }

}

