using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetStore.Models
{
    public class OrderHistory_OrderModel
    {
        public int Id { get; set; }
        public string Adress { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public double Summ { get; set; }
        public List<OrderHistory_ProductModel> Products = new List<OrderHistory_ProductModel>();
        public OrderHistory_OrderModel(int id, string adress,DateTime date,string status,double summ)
        {
            Id = id;Adress = adress;Date = date;Status = status;Summ = summ;
        }
    }
}