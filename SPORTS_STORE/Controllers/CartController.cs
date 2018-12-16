using SPORTS_STORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace SPORTS_STORE.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private SportsStoreEntities db = new SportsStoreEntities();

        [HttpGet]
        public ActionResult Index()
        {
            Guid userId = Guid.Parse(User.Identity.GetUserId());
            var cartId = (from x in db.Carts where x.UserId == userId select x.Id).FirstOrDefault();

            // Joining 3 tables based on their references for the current user 
            var userProducts =
                from cart in db.Carts
                join cartProduct in db.CartProducts on cart.Id equals cartProduct.CartId
                join product in db.Products on cartProduct.ProductId equals product.ProductId
                where cart.UserId.Equals(userId)
                select new CartViewModel
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = cartProduct.Quantity,
                    Subtotal = (product.Price * cartProduct.Quantity),
                };

            var userCartItemsCount = userProducts.Count();
            Session["cartItemsCount"] = userCartItemsCount;
            Session["UserId"] = userId;
            Session["cartId"] = cartId;

            return View(userProducts);
        }





        [HttpPost]
        public ActionResult Index(Guid ProductId) //send ProductId as a hidden field from the view
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var userExists = db.Carts.Where(u => u.UserId == userId); // checking if user that is currently on the page exists
            var cartId = (from x in db.Carts where x.UserId == userId select x.Id).FirstOrDefault();

            var cart = new Cart();

            if (userExists.Count() == 0)
            { // if the user doesn't exist (add to the cart)
                cart.UserId = userId;
                db.Carts.Add(cart);
                db.SaveChanges();
            }
            else
            {
                // get the specific card id of the user
                cart.Id = cartId;
            }

            var productExists = db.CartProducts.Where(p => p.ProductId == ProductId).Where(c => c.CartId == cartId);
            var cartExists = db.CartProducts.Any(p => p.CartId == cartId);

            /*
             if the product doesn't exist or there is no cart id for that user
                - add cartId, productId and quantity (from 0 (default) to 1)
             */
            if (productExists.Count() == 0 || !cartExists)
            {
                var cartProducts = new CartProduct
                {
                    CartId = cart.Id,
                    ProductId = ProductId,
                };
                cartProducts.Quantity++;
                db.CartProducts.Add(cartProducts);
                db.SaveChanges();
            }
            else // just increment the quantity of the founded product for the current user
            {
                var product = db.CartProducts.Where(p => p.ProductId == ProductId).Where(c => c.CartId == cartId).SingleOrDefault();
                product.Quantity++;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }





        [HttpPost]
        public ActionResult Delete(Guid ProductId)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var cartId = (from x in db.Carts where x.UserId == userId select x.Id).FirstOrDefault();
            var product = db.CartProducts.Where(p => p.ProductId == ProductId).Where(c => c.CartId == cartId).SingleOrDefault();

            if (product.Quantity == 1)
            {
                db.CartProducts.Remove(product);
                db.SaveChanges();
            }
            else
            {
                product.Quantity--;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }




        public ActionResult Checkout()
        {
            Session["checkoutId"] = null;
            Session["address"] = null;

            if ((int)Session["cartItemsCount"] == 0)
            {
                TempData["message"] = "Sorry, your cart is empty...";
                return RedirectToAction("Index", "Products", null);
            }
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout([Bind(Include = "Id,UserId,Name,Address,City,State,Zip")] Checkout checkout)
        {
            if (ModelState.IsValid)
            {
                db.Checkouts.Add(checkout);
                int cartId = (int)Session["cartId"]; // extract values from session to variable and use it in the query 

                // Set the cartId ordered value to true
                var cartOrder = db.CartProducts.Where(c => c.CartId == cartId).ToList();
                
                foreach (var i in cartOrder)
                {
                    i.Ordered = true;

                    var order = new Order
                    {
                        CheckoutId = checkout.Id,
                        ProductId = i.ProductId,
                        Quantity = i.Quantity
                    };
                    db.Orders.Add(order);
                }   
                
                db.CartProducts.RemoveRange(db.CartProducts.Where(c => c.CartId == cartId));

                Guid userId = (Guid)Session["userId"];
                var cart = db.Carts.Where(p => p.UserId == userId).SingleOrDefault();
                db.Carts.Remove(cart);
                db.SaveChanges();

                Session["checkoutId"] = checkout.Id;
                Session["address"] = checkout.Address;
                Session["cartItemsCount"] = 0;
                return RedirectToAction("Success");
            }

            return View(checkout);
        }




        public ActionResult Success()
        {
            if (Session["checkoutId"] == null || Session["address"] == null)
            {
                TempData["message"] = "What are you trying to do?";
                return RedirectToAction("Index", "Products");
            }

            return View();
        }
    }
}