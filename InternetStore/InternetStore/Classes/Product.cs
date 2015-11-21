using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace InternetStore.Classes
{
    [Table(Name = "Products")]
    public class Product
    {
        public Product()
        {
            Random keygen = new Random();
            this._id = keygen.Next(int.MaxValue);
        }

        private int _id;
        [Column(Storage = "_id", Name = "ID", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true)]
        public int ID
        {
            get { return this._id; }
            set { this._id = value; }
        }

        private string _productName;
        [Column(Storage = "_productName", Name = "productname")]
        public string ProductName
        {
            get { return this._productName; }
            set { this._productName = value; }
        }

        private string _description;
        [Column(Storage = "_description", Name = "description")]
        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        private string _image;
        [Column(Storage = "_image", Name = "image")]
        public string Image
        {
            get { return this._image; }
            set { this._image = value; }
        }

        private int _categoryID;
        [Column(Storage = "_categoryID", Name = "category_id")]
        public int CategoryID
        {
            get { return this._categoryID; }
            set { this._categoryID = value; }
        }

        private int _quantity;
        [Column(Storage = "_quantity", Name = "quantity")]
        public int Quantity
        {
            get { return this._quantity; }
            set { this._quantity = value; }
        }

        private double _price;
        [Column(Storage = "_price", Name = "price")]
        public double Price
        {
            get { return this._price; }
            set { this._price = value; }
        }

        private bool _featured;
        [Column(Storage = "_featured", Name = "featured")]
        public bool Featured
        {
            get { return this._featured; }
            set { this._featured = value; }
        }
    }
}