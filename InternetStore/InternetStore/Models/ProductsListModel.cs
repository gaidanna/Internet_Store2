using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetStore.Models
{
    public class ProductsListModel
    {
        public string SelectedCategory { get; set; }
        public int PriceFrom { get; set; }
        public int PriceTo { get; set; }
        public int Page { get; set; }
        public int CountOfPages { get; set; }
        public List<string> Categories { get; set; }
        public List<ProductDetailListModel> Products { get; set; }
    }
}