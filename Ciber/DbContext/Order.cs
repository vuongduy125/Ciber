using System;
using System.Collections.Generic;

namespace Ciber.CiberDbContext
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
