using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace InternetStore.Classes
{
    public class InternetStoreDBContext : DataContext
    {
        private static OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|final_project_db.accdb");

        public InternetStoreDBContext() : base(connection) { }

        public Table<User> Users;
        public Table<Order> Orders;
        public Table<Product> Products;
        public Table<OrderDetails> OrderDetails;
        public Table<Category> Categories;
        public Table<Sale> Sales;
    }
}