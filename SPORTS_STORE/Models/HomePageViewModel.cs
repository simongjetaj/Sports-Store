using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPORTS_STORE.Models
{
    public class HomePageViewModel
    {
        public PagedList.IPagedList<Product> Products { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}