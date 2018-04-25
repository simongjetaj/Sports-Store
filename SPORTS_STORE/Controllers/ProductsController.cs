using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SPORTS_STORE.Models;
using PagedList;
using PagedList.Mvc;

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

            ViewBag.categories = db.Products.Select(m => m.Category).Distinct().ToList();
            ViewBag.selectedCategory = category;

            if (String.IsNullOrEmpty(category) && String.IsNullOrEmpty(search))
            {
                return View(db.Products.ToList().ToPagedList(page ?? 1, 3));
            }
            else if (!String.IsNullOrEmpty(category) && !String.IsNullOrEmpty(search))
            {
                return View(db.Products
                    .Where(p => p.Category.Equals(category) && p.Name.Contains(search))
                    .ToList().ToPagedList(page ?? 1, 3));
            }
            else
            {
                return View(db.Products
                    .Where(p => p.Category.Equals(category) || p.Name.Contains(search))
                    .ToList().ToPagedList(page ?? 1, 3));
            }
        }


        //public JsonResult GetProducts(string search)
        //{
        //    List<Product> products;

        //    products = db.Products
        //        .Where(x => x.Name.Contains(search))
        //        .Select(x => new Product
        //        {
        //            ProductId = x.ProductId,
        //            Name = x.Name,
        //            Price = x.Price,
        //            Description = x.Description
        //        }
        //        )
        //        .ToList();

        //    return Json(products, JsonRequestBehavior.AllowGet);
        //}
    }
}
