using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace InternetStore.Classes
{
    [Table(Name = "Orders")]
    public class Order
    {
        public Order()
        {
            Random keygen = new Random();
            this._id = keygen.Next(int.MaxValue);
            this._user = new EntityRef<User>();
        }

        private int _id;
        [Column(Storage = "_id", Name = "ID", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true)]
        public int ID
        {
            get { return this._id; }
            set { this._id = value; }
        }

        private int _userID;
        [Column(Storage = "_userID", Name = "user_id")]
        public int UserID
        {
            get { return this._userID; }
            set { this._userID = value; }
        }

        private string _shippingAddress;
        [Column(Storage = "_shippingAddress", Name = "shipping_address")]
        public string ShippingAddress
        {
            get { return this._shippingAddress; }
            set { this._shippingAddress = value; }
        }

        private double _shippingDate;
        [Column(Storage = "_shippingDate", Name = "shipping_date")]
        public double ShippingDate
        {
            get { return this._shippingDate; }
            set { this._shippingDate = value; }
        }

        private string _shippingStatus;
        [Column(Storage = "_shippingStatus", Name = "shipping_status")]
        public string ShippingStatus
        {
            get { return this._shippingStatus; }
            set { this._shippingStatus = value; }
        }

        private EntityRef<User> _user;

        [Association(Storage = "_user", ThisKey = "UserID")]
        public User User
        {
            get { return this._user.Entity; }
            set { this._user.Entity = value; }
        }
    }
}