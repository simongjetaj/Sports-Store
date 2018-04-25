using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPORTS_STORE.Areas.Admin.Models
{
    public class OrderViewModel
    {
        public string User { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string Address { get; set; }

        public long Zip { get; set; }
    }
}