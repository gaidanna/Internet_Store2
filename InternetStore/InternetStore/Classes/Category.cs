using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace InternetStore.Classes
{
    [Table(Name = "Category")]
    public class Category
    {
        public Category()
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

        private string _categoryName;
        [Column(Storage = "_categoryName", Name = "categoryname")]
        public string CategoryName
        {
            get { return this._categoryName; }
            set { this._categoryName = value; }
        }

        private string _details;
        [Column(Storage = "_details", Name = "details")]
        public string Details
        {
            get { return this._details; }
            set { this._details = value; }
        }
    }
}