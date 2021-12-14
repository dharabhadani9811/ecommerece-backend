using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class OrderDetails
    {
        public string UserName { get; set; }

        public string EmailID { get; set; }

        public string Address { get; set; }

        public int oID { get; set; }

        public string ProductName { get; set; }

        public int ProductQty { get; set; }

        public double TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
