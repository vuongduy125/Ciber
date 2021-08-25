﻿using System;
using System.Collections.Generic;

namespace Ciber.CiberDbContext
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
