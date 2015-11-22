using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetStore.Models
{
    public class ProductDetailListModel
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public ProductDetailListModel(int id, string productName, string description, string image, string category, int quantity, double price)
        {
            ID = id; ProductName = productName; Description = description; Image = image; Category = category; Quantity = quantity; Price = price;
        }
    }
}