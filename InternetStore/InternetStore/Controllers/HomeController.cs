using InternetStore.Classes;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web;
using System.Web.Mvc;
using System.Data.Common;
using InternetStore.Models;
using System.Net.Mail;
using System.Net;

namespace InternetStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (InternetStoreDBContext dbc = new InternetStoreDBContext())
            {
                ///* PAY ATTENTION TO THIS PIECE OF CRAP: */
                //var time = DateTime.Now.ToOADate();
                //Order o = new Order() { ShippingAddress = "sda", UserID = 1, ShippingStatus = "bsdf", ShippingDate = time };
                //dbc.Orders.InsertOnSubmit(o);

                //var orders = (from item in dbc.Orders select item).ToList().FirstOrDefault();
                //var date = DateTime.FromOADate(orders.ShippingDate);

                #region picking existing data
                //var categories = (from item in dbc.Categories select item).ToList().FirstOrDefault();
                //var orderDetails = (from item in dbc.OrderDetails select item).ToList().FirstOrDefault();
                //var products = (from item in dbc.Products select item).ToList().FirstOrDefault();
                //var sales = (from item in dbc.Sales select item).ToList().FirstOrDefault();
                //var users = (from item in dbc.Users select item).ToList().FirstOrDefault(); 
                #endregion

                #region creating new objects examples
                //Sale s = new Sale() { OrderID = 1, SalesAmount = 123.12 };
                //dbc.Sales.InsertOnSubmit(s);

                //User u = new Classes.User() { Address = "asd", Email = "mail", Password = "pass", FirstName = "name", LastName = "name", UserName = "name", Phone = "000000000" };
                //dbc.Users.InsertOnSubmit(u);

                //OrderDetails od = new OrderDetails() { OrderID = 1, ProductID = 1, Quantity = 1 };
                //dbc.OrderDetails.InsertOnSubmit(od);

                //var c = new Category();
                //c.CategoryName = "newCategory777";
                //c.Details = "details";
                //dbc.Categories.InsertOnSubmit(c); //Allow attached changes to be commited to DB on submiting changes 
                #endregion

                #region deleting existing objects from db
                //var oldCategory = (from c in dbc.Categories where c.ID == 18 select c).ToList().FirstOrDefault();
                //dbc.Categories.DeleteOnSubmit(oldCategory); 
                #endregion

                #region updating objects - these examples still throw exception!!!
                //IQueryable<InternetStore.Classes.User> custQuery =
                //    from cust in dbc.Users
                //    where cust.ID == 1
                //    select cust;
                //foreach (User cust in custQuery)
                //{
                //    string addr = cust.Address;
                //    cust.Address = "NewAdress";
                //}

                //List<Category> custQuery =
                //    (from cust in dbc.Categories
                //    where cust.ID == 1
                //    select cust).ToList();

                //foreach (var cust in custQuery)
                //{
                //    string addr = cust.Details;
                //    cust.Details = "Some Other New Details";
                //} 
                #endregion

                #region updating objects Badaboo style - works perfect lol!!!
                //var oldCategory = (from c in dbc.Categories where c.ID == 18 select c).ToList().FirstOrDefault();
                //dbc.Categories.DeleteOnSubmit(oldCategory);

                //dbc.SubmitChanges();//some crap spilled here

                //var newCategory = new Category();
                //newCategory.ID = 18;
                //newCategory.CategoryName = "little tities";
                //newCategory.Details = "some boobs here";
                //dbc.Categories.InsertOnSubmit(newCategory);//Hell yeah! 
                #endregion

                //dbc.SubmitChanges(); //Commit changes to DB

                var product = (from item in dbc.Products select item).ToList().FirstOrDefault();
                GetCart().AddItem(product, 1);
            }
            return View();
        }

        public ActionResult ProductsList()
        {
            return View();
        }

        public ActionResult ProductDetailList()
        {
            return View();
        }

        public ActionResult OrderHistory()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (InternetStoreDBContext dbc = new InternetStoreDBContext())
                {
                    var currentUser = (from u in dbc.Users where u.UserName == User.Identity.Name select u).ToList().FirstOrDefault();
                    if (currentUser != null)
                    {
                        ViewBag.FirstName = currentUser.FirstName ?? "";
                        ViewBag.LastName = currentUser.LastName ?? "";
                        ViewBag.Email = currentUser.Email ?? "";
                        ViewBag.Phone = currentUser.Phone ?? "";
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Contact(UserMessage userMessage)
        {
            if (ModelState.IsValid)
            {
                //Your manager's email address:
                string to = "kovalsergey91@gmail.com";
                //Auto-sender account:
                string from = "kovalsergey91@yandex.ru";

                MailMessage emailMessage = new MailMessage(from, to);
                emailMessage.Subject = "Message from customer; ABS Technologies web site;";
                emailMessage.Body = userMessage.Complete();

                //Your company's post server's settings:
                SmtpClient client = new SmtpClient();
                client.Host = "Smtp.yandex.ru";
                client.Port = 25;
                client.EnableSsl = true;
                //Auto-sender account+password:
                client.Credentials = new NetworkCredential(from, "opopop");

                try
                {
                    client.Send(emailMessage);
                    ViewBag.Success = true;
                }
                catch
                {
                    ViewBag.Success = false;
                }
            }
            else
            {
                ViewBag.Success = false;
            }
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        #region Cart Page
        public ViewResult Cart(CartViewModel cart, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View(cart);
        }

        public PartialViewResult CartSummary()
        {
            return PartialView(GetCart());
        }
        #endregion

        #region Cart
        public RedirectToRouteResult AddToCart(CartViewModel cart, int productId, string returnUrl)
        {
            using (InternetStoreDBContext dbc = new InternetStoreDBContext())
            {
                var product = (from item in dbc.Products select item).ToList().FirstOrDefault();
                if(product != null)
                {
                    cart.AddItem(product, 1);
                }
            }
            return RedirectToAction("Cart", new { returnUrl});
        }

        public RedirectToRouteResult RemoveFromCart(CartViewModel cart, int productId, string returnUrl)
        {
            using (InternetStoreDBContext dbc = new InternetStoreDBContext())
            {
                var product = (from item in dbc.Products select item).ToList().FirstOrDefault();
                if (product != null)
                {
                    cart.RemoveItem(product, 1);
                }
            }
            return RedirectToAction("Cart", new { returnUrl });
        }

        [HttpPost]
        public RedirectToRouteResult RemoveLineFromCart(CartViewModel cart, int productId, string returnUrl)
        {
            using (InternetStoreDBContext dbc = new InternetStoreDBContext())
            {
                var product = (from item in dbc.Products select item).ToList().FirstOrDefault();
                if (product != null)
                {
                    cart.RemoveLine(product);
                }
            }
            return RedirectToAction("Cart", new { returnUrl });
        }

        private CartViewModel GetCart()
        {
            CartViewModel cart = (CartViewModel)Session["Cart"];
            if (cart == null)
            {
                cart = new CartViewModel();
                Session["Cart"] = cart;
            }
            return cart;
        }
        #endregion
    }
}