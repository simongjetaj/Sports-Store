using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using SPORTS_STORE.Models;
using PagedList;

namespace SPORTS_STORE.Controllers
{
    public class ProductsController : Controller
    {
        private SportsStoreEntities db = new SportsStoreEntities();

        public ActionResult Index(string search, string category, int? page)
        {
            if (Session["checkoutId"] != null || Session["address"] != null)
            {
                Session["checkoutId"] = null;
                Session["address"] = null;
            }

            var categories = db.Products
                .Select(x => new CategoryViewModel
                {
                    Name = x.Category
                })
                .Distinct()
                .ToList();

            ViewBag.selectedCategory = category;

            var products = (dynamic)null;
            if (string.IsNullOrEmpty(category) && string.IsNullOrEmpty(search))
            {
                products = db.Products.ToList().ToPagedList(page ?? 1, 3);

            }
            else if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(search))
            {
                products = db.Products
                    .Where(p => p.Category.Equals(category) && p.Name.Contains(search))
                    .ToList().ToPagedList(page ?? 1, 3);
            }
            else
            {
                products = db.Products
                    .Where(p => p.Category.Equals(category) || p.Name.Contains(search))
                    .ToList().ToPagedList(page ?? 1, 3);
            }

            HomePageViewModel homePageViewModel = new HomePageViewModel
            {
                Products = products,
                Categories = categories
            };

            return View(homePageViewModel);
        }
    }
}
