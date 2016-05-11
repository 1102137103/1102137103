using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _0329.Controllers
{
    public class OrderController : Controller
    {

        Models.CodeService codeService = new Models.CodeService();
        Models.OrderService orderService = new Models.OrderService();
        /// <summary>
        /// 訂單管理首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.ShipCodeData = this.codeService.GetShip(-1);
            return View();
        }
        /// <summary>
        /// 取得訂單查詢結果
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult Index(Models.OrderSearchArg arg)
        {
            Models.OrderService orderService = new Models.OrderService();
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.EmpResult = orderService.GetOrderByCondtioin(arg);
            ViewBag.ShipCodeData = this.codeService.GetShip(-1);
            ViewBag.ShipResult = orderService.GetOrderByCondtioin(arg);
            List<Models.Order> result = orderService.GetOrderByCondtioin(arg);
            ViewBag.SearchResult = result;
            return View("Index");
        }
        /// <summary>
        /// 新增訂單畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult InsertOrder()
        {
            ViewBag.CustCodeData = this.codeService.GetCustomer(-1);
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.ProductCodeData = this.codeService.GetProduct();
            ViewBag.ShipCodeData = this.codeService.GetShip(-1);
            return View(new Models.Order());
        }
        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult InsertOrder(Models.Order order)
        {
            if (ModelState.IsValid)
            {
                Models.OrderService orderService = new Models.OrderService();
                orderService.InsertOrder(order);
                return RedirectToAction("Index");
            }
            return View(order);
            //return View();
        }
        /// <summary>
        /// 更新訂單畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult UpdateOrder(string id)
        {
            Models.OrderService orderService = new Models.OrderService();
            Models.Order order = orderService.GetOrderById(id);
            ViewBag.CustCodeData = this.codeService.GetCustomer(-1);
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.ProductCodeData = this.codeService.GetProduct();
            ViewBag.ShipCodeData = this.codeService.GetShip(-1);
            ViewBag.Orderdate = string.Format("{0:yyyy-MM-dd}", order.Orderdate);
            ViewBag.RequireDdate = string.Format("{0:yyyy-MM-dd}", order.RequireDdate);
            ViewBag.ShippedDate = string.Format("{0:yyyy-MM-dd}", order.ShippedDate);
            ViewBag.SearchResult = order;
            return View(new Models.Order());
        }

        /// <summary>
        /// 更新訂單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateOrder(Models.Order order)
        {
            Models.OrderService orderService = new Models.OrderService();
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.ShipCodeData = this.codeService.GetShip(-1);
            orderService.UpdateOrder(order);
            return View();
        }

        /// <summary>
        /// 刪除訂單
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteOrder(string orderId)
        {
            try
            {
                Models.OrderService orderService = new Models.OrderService();
                orderService.DeleteOrderById(orderId);
                return this.Json(true);
            }
            catch (Exception)
            {
                return this.Json(false);
            }
        }

    }
}