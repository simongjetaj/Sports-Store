using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPORTS_STORE.Models
{
    public class CartViewModel
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal Subtotal { get; set; }
    }
}