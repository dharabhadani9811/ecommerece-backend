using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        public int UserID { get; set; }

        public int ProductID { get; set; }

        public int ProductQty { get; set; }

        public DateTime OrderDate { get; set; }

        public int oID { get; set; }
    }
}
