using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace InternetStore.Classes
{
    [Table(Name = "Sales")]
    public class Sale
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

        private int _salesAmount;
        [Column(Storage = "_salesAmount", Name = "sales_amount")]
        public int SalesAmount
        {
            get { return this._salesAmount; }
            set { this._salesAmount = value; }
        }
    }
}