using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetStore.Models
{
    public class OrderHistory_ProductModel
    {
        public string ProductName { get; set; }
        public int Id { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public OrderHistory_ProductModel(int id,string productName,string image,double price,int quantity)
        {
            Id = id; ProductName = productName;Image = image;Price = price;Quantity = quantity;
        }
    }
}