using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ProductDetails
    {
        public int ProductID { get; set; }

        public string CategoryName { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public double ProductPrice { get; set; }

        public string ProductImage { get; set; }
    }
}
