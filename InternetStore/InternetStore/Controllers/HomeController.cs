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

namespace InternetStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (InternetStoreDBContext dbc = new InternetStoreDBContext())
            {
                /* PAY ATTENTION TO THIS PIECE OF CRAP: */
                var time = DateTime.Now.ToOADate();
                Order o = new Order() { ShippingAddress = "sda", UserID = 1, ShippingStatus = "bsdf", ShippingDate = time };
                dbc.Orders.InsertOnSubmit(o);

                var orders = (from item in dbc.Orders select item).ToList().FirstOrDefault();
                var date = DateTime.FromOADate(orders.ShippingDate);

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

                dbc.SubmitChanges(); //Commit changes to DB
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
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}