using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _0329.Models
{
    public class OrderSearchArg
    {
        public string CustName { get; set; }
        public string OrderDate { get; set; }
        public string ShippedDate { get; set; }
        public string RequiredDate { get; set; }
        public string DeleteOrderId { get; set; }
        public string ShipperID { get; set; }
        public string OrderId { get; set; }
        public string EmployeeID { get; set; }
        public string ShipName { get; set; }
    }
}