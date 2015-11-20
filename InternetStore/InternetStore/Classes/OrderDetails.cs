using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace InternetStore.Classes
{
    [Table(Name = "Order_Details")]
    public class OrderDetails
    {
        private int _id;
        [Column(Storage = "_id", Name = "ID", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID
        {
            get { return this._id; }
            set { this._id = value; }
        }

        private int _orderID;
        [Column(Storage = "_orderID", Name = "order_id")]
        public int OrderID
        {
            get { return this._orderID; }
            set { this._orderID = value; }
        }

        private int _productID;
        [Column(Storage = "_productID", Name = "product_id")]
        public int ProductID
        {
            get { return this._productID; }
            set { this._productID = value; }
        }

        private int _quantity;
        [Column(Storage = "_quantity", Name = "quantity")]
        public int Quantity
        {
            get { return this._quantity; }
            set { this._quantity = value; }
        }
    }
}