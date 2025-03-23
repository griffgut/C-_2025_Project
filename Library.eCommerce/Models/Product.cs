﻿using System;
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
        public string? Display
        {
            get
            {
                return $"{Id}. {Name}.";
            }
        }

        public Product()
        {
            Name = string.Empty;
        }
        public Product(Product product)
        {
            Name = product.Name;
            Id = product.Id;
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
}
