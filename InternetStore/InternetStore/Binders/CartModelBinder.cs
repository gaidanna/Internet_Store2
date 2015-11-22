using InternetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetStore.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) 
        {
            CartViewModel cart = null;
            if (controllerContext.HttpContext.Session != null) 
            {
                cart = (CartViewModel)controllerContext.HttpContext.Session[sessionKey];
            }
            if (cart == null) 
            {
                cart = new CartViewModel();
                if (controllerContext.HttpContext.Session != null) 
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }
            return cart;
        }
    }
}