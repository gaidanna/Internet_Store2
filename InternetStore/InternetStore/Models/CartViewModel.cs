using InternetStore.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetStore.Models
{
    public class CartViewModel
    {
        private List<OrderDetails> _ordersDetails = new List<OrderDetails>();

        public void AddItem(Product product, int quantity)
        {
            var orderDetails = _ordersDetails.Where(od => od.ProductID == product.ID).FirstOrDefault();
            if (orderDetails == null)
            {
                _ordersDetails.Add(new OrderDetails
                {
                    ProductID = product.ID,
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                orderDetails.Quantity += quantity;
            }
        }

        public void RemoveItem(Product product, int quantity)
        {
            var orderDetails = _ordersDetails.Where(od => od.ProductID == product.ID).FirstOrDefault();
            if (orderDetails != null)
            {
                if (orderDetails.Quantity <= quantity)
                    _ordersDetails.RemoveAll(od => od.ProductID == product.ID);
                else
                    orderDetails.Quantity -= quantity;
            }
        }

        public void RemoveLine(Product product)
        {
            _ordersDetails.RemoveAll(od => od.ProductID == product.ID);
        }

        public double ComputeTotalValue()
        {
            return OrdersDetails.Sum(od => od.Product.Price * od.Quantity);
        }

        public void Clear()
        {
            _ordersDetails.Clear();
        }

        public IEnumerable<OrderDetails> OrdersDetails
        {
            get { return _ordersDetails; }
        }
    }
}