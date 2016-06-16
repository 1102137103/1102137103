using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _0329.Models
{
    public class OrderDetails
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 產品代號
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public short Qty { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }
    }
}