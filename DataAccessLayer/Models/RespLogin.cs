using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class RespLogin
    {
        public string UserID {get;set;}

        public string EmailID { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }
    }
}
