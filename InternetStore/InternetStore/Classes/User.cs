using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace InternetStore.Classes
{
    [Table(Name = "Users")]
    public class User
    {
        public User()
        {
            Random keygen = new Random();
            this._id = keygen.Next(int.MaxValue);
            this._orders = new EntitySet<Order>();
        }

        private int _id;
        [Column(Storage = "_id", Name = "ID", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true)]
        public int ID
        {
            get { return this._id; }
            set { this._id = value; }
        }

        private string _firstName;
        [Column(Storage = "_firstName", Name = "firstname")]
        public string FirstName
        {
            get { return this._firstName; }
            set { this._firstName = value; }
        }

        private string _lastName;
        [Column(Storage = "_lastName", Name = "lastname")]
        public string LastName
        {
            get { return this._lastName; }
            set { this._lastName = value; }
        }

        private string _email;
        [Column(Storage = "_email", Name = "email")]
        public string Email
        {
            get { return this._email; }
            set { this._email = value; }
        }

        private string _password;
        [Column(Storage = "_password", Name = "password")]
        public string Password
        {
            get { return this._password; }
            set { this._password = value; }
        }

        private string _address;
        [Column(Storage = "_address", Name = "address")]
        public string Address
        {
            get { return this._address; }
            set { this._address = value; }
        }

        private string _phone;
        [Column(Storage = "_phone", Name = "phone")]
        public string Phone
        {
            get { return this._phone; }
            set { this._phone = value; }
        }

        private string _userName;
        [Column(Storage = "_userName", Name = "username")]
        public string UserName
        {
            get { return this._userName; }
            set { this._userName = value; }
        }

        private EntitySet<Order> _orders;

        [Association(Storage = "_orders", OtherKey = "UserID")]
        public EntitySet<Order> Orders
        {
            get { return this._orders; }
            set { this._orders.Assign(value); }
        }
    }
}