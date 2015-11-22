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
                var products = (from c in dbc.Products select c).ToList();
                List<Product> newList = new List<Product>();
                foreach (var product in products)
                    if (product.Featured)
                        newList.Add(product);

                ViewBag.Products = newList;
            }
            return View();
        }

        #region ProductsList
        public ActionResult ProductsList()
        {
            ProductsListModel model = CreateProductList(null, 0, 10000, 1);
            return View(model);
        }

        [HttpPost]
        public ActionResult ProductsList(string Category, int PriceFrom, int PriceTo, int Page)
        {
            ProductsListModel model = CreateProductList(Category, PriceFrom, PriceTo, Page);
            return View(model);
        }

        private ProductsListModel CreateProductList(string category, int priceFrom, int priceTo, int page)
        {
            //initialize data
            ProductsListModel model = new ProductsListModel();
            model.Categories = new List<string>();
            model.Products = new List<ProductDetailListModel>();
            List<Category> categories = null;
            List<Product> products = null;

            //Loading data from DB
            using (InternetStoreDBContext dbc = new InternetStoreDBContext())
            {
                categories = (from item in dbc.Categories select item).ToList();
                if (category == null || category == "All")
                    products = (from item in dbc.Products where item.Price >= priceFrom && item.Price <= priceTo select item).ToList();
                else
                {
                    int categ = (from item in categories where item.CategoryName == category select item).ToList().FirstOrDefault().ID;
                    products = (from item in dbc.Products where item.Price >= priceFrom && item.Price <= priceTo && item.CategoryID == categ select item).ToList();
                }
            }

            //Fill Catagory
            if (categories != null)
                foreach (Category item in categories)
                    model.Categories.Add(item.CategoryName);

            //Fill Products
            List<ProductDetailListModel> tempProsuctDetailListModel = new List<ProductDetailListModel>();
            if (categories != null && products != null)
                foreach (Product prod in products)
                {
                    string categ = (from item in categories where item.ID == prod.CategoryID select item).ToList().FirstOrDefault().CategoryName;
                    ProductDetailListModel productDetailModel = new ProductDetailListModel(
                        prod.ID,
                        prod.ProductName,
                        prod.Description,
                        prod.Image,
                        categ,
                        prod.Quantity,
                        prod.Price);
                    tempProsuctDetailListModel.Add(productDetailModel);
                }

            //Determine list of Products in one Page
            int ProductsOnPage = 6;
            int CurrentPage = page;
            int TotalPages = 0;
            List<ProductDetailListModel> ProsuctDetailListModelToView = new List<ProductDetailListModel>();
            if (tempProsuctDetailListModel.Count > 0)
            {
                tempProsuctDetailListModel.Sort(delegate(ProductDetailListModel m1, ProductDetailListModel m2) { return m1.ProductName.CompareTo(m2.ProductName); });
                double TotalPagesDouble = (double)(tempProsuctDetailListModel.Count) / (double)ProductsOnPage;
                if ((TotalPagesDouble - Math.Truncate(TotalPagesDouble)) == 0 || TotalPagesDouble < 1)
                    TotalPages = Convert.ToInt32(TotalPagesDouble);
                else
                    TotalPages = (int)(Math.Truncate(TotalPagesDouble)) + 1;
                int startElement = (CurrentPage - 1) * ProductsOnPage;
                int endElement = CurrentPage * ProductsOnPage - 1;
                if (startElement > (tempProsuctDetailListModel.Count - 1))
                {
                    startElement = 0;
                    endElement = ProductsOnPage;
                }
                if (endElement > (tempProsuctDetailListModel.Count - 1))
                    endElement = tempProsuctDetailListModel.Count - 1;
                for (int i = startElement; i <= endElement; i++)
                    ProsuctDetailListModelToView.Add(tempProsuctDetailListModel[i]);
            }

            //Create model for View
            model.Page = CurrentPage;
            model.CountOfPages = TotalPages;
            model.PriceFrom = priceFrom;
            model.PriceTo = priceTo;
            model.SelectedCategory = category;
            model.Products = ProsuctDetailListModelToView;
            return model;
        }
        #endregion

        #region ProductDetailList
        public ActionResult ProductDetailList(int id)
        {
            Product product;
            ProductDetailListModel model = null;
            using (InternetStoreDBContext dbc = new InternetStoreDBContext())
            {
                product = (from item in dbc.Products where item.ID == id select item).ToList().FirstOrDefault();
                if (product != null)
                {
                    string category = (from item in dbc.Categories where item.ID == product.CategoryID select item).ToList().FirstOrDefault().CategoryName;
                    model = new ProductDetailListModel(
                        product.ID,
                        product.ProductName,
                        product.Description,
                        product.Image,
                        category,
                        product.Quantity,
                        product.Price);
                }
            }
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        #endregion

        public ActionResult OrderHistory()
        {
            List<OrderHistory_OrderModel> model = new List<OrderHistory_OrderModel>();
            if (User.Identity.IsAuthenticated)
            {
                using (InternetStoreDBContext dbc = new InternetStoreDBContext())
                {
                    var currentUser = (from u in dbc.Users where u.Email == User.Identity.Name select u).ToList().FirstOrDefault();
                    if (currentUser != null)
                    {
                        List<Order> orders = (from item in dbc.Orders where item.UserID==currentUser.ID select item).ToList();
                        foreach (Order o in orders)
                        {
                            List<OrderDetails> tempOrderDetails = (from od in dbc.OrderDetails where od.OrderID==o.ID select od).ToList();
                            double summ = (from s in dbc.Sales where s.OrderID == o.ID select s.SalesAmount).ToList().FirstOrDefault();
                            OrderHistory_OrderModel oh_om = new OrderHistory_OrderModel(o.ID,o.ShippingAddress, DateTime.FromOADate(o.ShippingDate),o.ShippingStatus,summ);
                            foreach (OrderDetails od in tempOrderDetails)
                                oh_om.Products.Add(new OrderHistory_ProductModel(od.Product.ID, od.Product.ProductName, od.Product.Image, od.Product.Price,od.Quantity));
                            model.Add(oh_om);
                        }
                        
                    }
                }
            }
            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            UserMessageViewModel user = new UserMessageViewModel();

            if (User.Identity.IsAuthenticated)
            {
                using (InternetStoreDBContext dbc = new InternetStoreDBContext())
                {
                    var currentUser = (from u in dbc.Users where u.Email == User.Identity.Name select u).ToList().FirstOrDefault();
                    if (currentUser != null)
                    {
                        user.FirstName = currentUser.FirstName ?? "";
                        user.LastName = currentUser.LastName ?? "";
                        user.Email = currentUser.Email ?? "";
                        user.Phone = currentUser.Phone ?? "";
                    }
                }
            }
            return View("Contact", user);
        }

        [HttpPost]
        public ActionResult Contact(UserMessageViewModel userMessage)
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
                var product = (from item in dbc.Products where item.ID == productId select item).ToList().FirstOrDefault();
                if (product != null)
                {
                    cart.AddItem(product, 1);
                }
            }
            return RedirectToAction("Cart", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(CartViewModel cart, int productId, string returnUrl)
        {
            using (InternetStoreDBContext dbc = new InternetStoreDBContext())
            {
                var product = (from item in dbc.Products where item.ID == productId select item).ToList().FirstOrDefault();
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
                var product = (from item in dbc.Products where item.ID == productId select item).ToList().FirstOrDefault();
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

        #region Checkout
        public ViewResult Checkout(User user, Order order)
        {
            ViewBag.IsAuthenticated = User.Identity.IsAuthenticated;
            return View();
        }

        [HttpPost]
        public ViewResult Checkout(CartViewModel cart, User user, Order order)
        {
            if (cart.OrdersDetails.Count() == 0)
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "Sorry, your cart is empty!";
            }
            else
            {
                ViewBag.IsSuccess = true;
                ViewBag.Message = "Thanks for placing your order. We'll ship your goods as soon as possible.";

                using (InternetStoreDBContext dbc = new InternetStoreDBContext())
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        dbc.Users.InsertOnSubmit(user);
                        dbc.SubmitChanges();
                        order.UserID = user.ID;
                    }
                    else
                    {
                        User currentUser = (from u in dbc.Users where u.Email == User.Identity.Name select u).ToList().FirstOrDefault();
                        order.UserID = currentUser.ID;
                    }

                    order.ShippingDate = DateTime.Now.ToOADate();
                    if (order.ShippingStatus == null)
                        order.ShippingStatus = "Check";
                    dbc.Orders.InsertOnSubmit(order);
                    dbc.SubmitChanges();

                    Sale sale = new Sale() { OrderID = order.ID, SalesAmount = cart.ComputeTotalValue() };
                    dbc.Sales.InsertOnSubmit(sale);
                    dbc.SubmitChanges();

                    foreach (var orderDetails in cart.OrdersDetails)
                    {
                        var orderDetails2 = new OrderDetails() { OrderID = order.ID, ProductID = orderDetails.ProductID, Quantity = orderDetails.Quantity };
                        dbc.OrderDetails.InsertOnSubmit(orderDetails2);
                        dbc.SubmitChanges();
                    }

                    cart.Clear();
                }
            }

            ViewBag.IsAuthenticated = User.Identity.IsAuthenticated;
            return View();
        }
        #endregion
    }
}