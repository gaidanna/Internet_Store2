using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetStore.Classes
{
    public class Cart
    {
        private List<OrderDetails> _ordersDetails = new List<OrderDetails>();

        public void AddItem(int productID, int quantity)
        {
            var orderDetails = _ordersDetails.Where(od => od.ProductID == productID).FirstOrDefault();
            if (orderDetails == null)
            {
                _ordersDetails.Add(new OrderDetails
                {
                    ID = productID,
                    Quantity = quantity
                });
            }
            else
            {
                orderDetails.Quantity += quantity;
            }
        }

        public void RemoveItem(int productID, int quantity)
        {
            var orderDetails = _ordersDetails.Where(od => od.ProductID == productID).FirstOrDefault();
            if (orderDetails != null)
            {
                if (orderDetails.Quantity <= quantity)
                    _ordersDetails.RemoveAll(od => od.ProductID == productID);
                else
                    orderDetails.Quantity -= quantity;
            }
        }

        public double ComputeTotalValue()
        {
            double sum = 0;
            using (InternetStoreDBContext dbc = new InternetStoreDBContext())
            {
                foreach (var od in _ordersDetails)
                {
                    var product = (from item in dbc.Products where item.ID == od.ProductID select item).ToList().FirstOrDefault();
                    sum += product.Price * od.Quantity;
                }
            }
            return sum;
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