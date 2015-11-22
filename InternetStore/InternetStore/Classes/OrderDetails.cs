using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace InternetStore.Classes
{
    [Table(Name = "Order_Details")]
    public class OrderDetails
    {
        public OrderDetails()
        {
            Random keygen = new Random();
            this._id = keygen.Next(int.MaxValue);
            this._order = new EntityRef<Order>();
            this._product = new EntityRef<Product>();
        }

        private int _id;
        [Column(Storage = "_id", Name = "ID", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true)]
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

        private EntityRef<Order> _order;

        [Association(Storage = "_order", ThisKey = "OrderID")]
        public Order Order
        {
            get { return this._order.Entity; }
            set { this._order.Entity = value; }
        }

        private EntityRef<Product> _product;

        [Association(Storage = "_product", ThisKey = "ProductID")]
        public Product Product
        {
            get { return this._product.Entity; }
            set {this._product.Entity = value;}
        }
    }
}