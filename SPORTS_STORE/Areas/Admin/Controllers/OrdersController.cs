using Microsoft.AspNet.Identity;
using SPORTS_STORE.Areas.Admin.Models;
using SPORTS_STORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPORTS_STORE.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private SportsStoreEntities db = new SportsStoreEntities();
        
        public ActionResult Index()
        {
            var userOrders =
                from checkout in db.Checkouts
                join order in db.Orders on checkout.Id equals order.CheckoutId
                join product in db.Products on order.ProductId equals product.ProductId
                select new OrderViewModel
                {
                    User = checkout.Name,
                    Name = product.Name,
                    Quantity = order.Quantity,
                    Address = checkout.Address + ", " + checkout.City + ", " + checkout.State,
                    Zip = checkout.Zip,
                };

            return View(userOrders);
        }
    }
}
