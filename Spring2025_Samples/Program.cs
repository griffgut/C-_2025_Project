//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Amazon!");

            char choice;
            double total = 0;

            do
            {
                Console.WriteLine("Are you an employee or a customer?");
                Console.WriteLine("E: Employee");
                Console.WriteLine("C: Customer");
                Console.WriteLine("Q: Quit");
                string? input = Console.ReadLine();
                choice = input[0];
                switch (choice)
                {
                    case 'E':
                    case 'e':
                        Employee();
                        break;
                    case 'C':
                    case 'c':
                        Customer();
                        break;
                    case 'Q':
                    case 'q':
                        if (ShoppingServiceProxy.Current.CartItems.Any())
                        {
                            Console.WriteLine("Receipt");
                            ShoppingServiceProxy.Current.CartItems.ForEach(Console.WriteLine);
                            double p = 0;
                            double q = 0;
                            foreach(var item in ShoppingServiceProxy.Current.CartItems)
                            {
                                p = item.Price;
                                q = item.Quantity;
                                total += p * q;
                            }
                            total = (total * .07) + total;
                            var stotal = $"{total:0.00}";
                            Console.WriteLine("Total with .7% tax added: $" + stotal);
                        }
                        else
                        {
                            Console.WriteLine("You had no items in the shopping cart");
                        }
                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                }
            }
            while (choice != 'Q' && choice != 'q');
            Console.ReadLine();
        }
        private static void Employee()
        {
            Console.WriteLine("Welcome to the Inventory.");
            Console.WriteLine("C. Create new inventory item");
            Console.WriteLine("R. Read all inventory items");
            Console.WriteLine("U. Update an inventory item");
            Console.WriteLine("D. Delete an inventory item");
            Console.WriteLine("Q. Quit");

            List<Item?> list = ProductServiceProxy.Current.Products;
           
            char choice;
            do
            {
                string? input = Console.ReadLine();
                choice = input[0];
                switch (choice)
                {
                    case 'C':
                    case 'c':
                        Item i = new Item();
                        Console.WriteLine("What is the name of the product: ");
                        i.Product.Name = Console.ReadLine();
                        Console.WriteLine("What is the price: ");
                        i.Price = double.Parse(Console.ReadLine() ?? "0");
                        Console.WriteLine("What is the quantity: ");
                        i.Quantity = int.Parse(Console.ReadLine() ?? "0");
                        ProductServiceProxy.Current.AddOrUpdate(i);
                        break;
                    case 'R':
                    case 'r':

                        list.ForEach(Console.WriteLine);
                        break;
                    case 'U':
                    case 'u':
                        //select one of the products
                        Console.WriteLine("Which product would you like to update? Please input the ID");
                        int selection = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProd = list.FirstOrDefault(p => p.Id == selection);

                        if (selectedProd != null)
                        {
                            Console.WriteLine("Please enter a new name: ");
                            selectedProd.Product.Name = Console.ReadLine() ?? "ERROR";
                            Console.WriteLine("Please enter a new price: ");
                            selectedProd.Price = double.Parse(Console.ReadLine() ?? "0");
                            Console.WriteLine("Please enter a new quantity: ");
                            selectedProd.Quantity = int.Parse(Console.ReadLine() ?? "0");
                            ProductServiceProxy.Current.AddOrUpdate(selectedProd);
                        }
                        break;
                    case 'D':
                    case 'd':
                        //select one of the products
                        //throw it away
                        Console.WriteLine("Which product would you like to delete? Please enter the ID");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        ProductServiceProxy.Current.Delete(selection);
                        break;
                    case 'Q':
                    case 'q':
                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                }
            } while (choice != 'Q' && choice != 'q');

            return;
        }
        private static void Customer()
        {
            Console.WriteLine("Welcome to the Cart.");
            Console.WriteLine("B. Buy an item");
            Console.WriteLine("R. Read all shopping cart");
            Console.WriteLine("I. Read all inventory items");
            Console.WriteLine("U. Update amount of item in a cart");
            Console.WriteLine("D. Return an item from the shopping cart");
            Console.WriteLine("Q. Quit");
            List<Item?> inv = ProductServiceProxy.Current.Products; // shallow copy of products
            List<Item?> sCart = ShoppingServiceProxy.Current.CartItems;   // shallow copy of cart
            char choice;
            do
            {
                string? input = Console.ReadLine();
                choice = input[0];
                switch (choice)
                {
                    case 'B':
                    case 'b':
                        Console.WriteLine("Which product would you like to buy? Please enter it's ID");
                        int select = int.Parse(Console.ReadLine() ?? "-1");
                        var selectProd = inv.FirstOrDefault(p => p.Id == select);
                        if (selectProd != null)
                        {
                            ShoppingServiceProxy.Current.AddOrUpdate(selectProd);
                        }
                        break;
                    case 'R':
                    case 'r':

                        sCart.ForEach(Console.WriteLine);
                        break;
                    case 'I':
                    case 'i':
                        inv.ForEach(Console.WriteLine);
                        break;
                    case 'U':
                    case 'u':
                        //select one of the products
                        Console.WriteLine("Which product's quantity would you like to update? Please enter the ID");
                        int selection = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProd = sCart.FirstOrDefault(p => p.Product.Id == selection);

                        if (selectedProd != null)
                        {
                            //Console.WriteLine("What is the new quantity of this item: ");
                            //int q = int.Parse(Console.ReadLine() ?? "0");
                            ShoppingServiceProxy.Current.AddOrUpdate(selectedProd);
                        }
                        break;
                    case 'D':
                    case 'd':
                        //select one of the products
                        //throw it away
                        Console.WriteLine("Which product would you like to return? Please enter the ID");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        var sProd = sCart.FirstOrDefault(p => p.Product.Id == selection);

                        ShoppingServiceProxy.Current.Delete(sProd);
                        break;
                    case 'Q':
                    case 'q':
                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                }
            } while (choice != 'Q' && choice != 'q');
            return;
        }
    }


}
