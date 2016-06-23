using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace _0329.Models
{
    public class OrderDetailService
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }

        /// <summary>
        /// 新增訂單明細
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int InsertOrderDetail(List<Models.OrderDetails> orderDetail, int orderId)
        {
            foreach (Models.OrderDetails row in orderDetail)
            {
                string sql = @"INSERT INTO Sales.OrderDetails(
	                            OrderID, ProductID, UnitPrice, Qty, Discount
                            )VALUES(
							    @OrderId, @ProductID, @UnitPrice, @Qty, @Discount
						    )";
                
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));
                    cmd.Parameters.Add(new SqlParameter("@ProductID", row.ProductId));
                    cmd.Parameters.Add(new SqlParameter("@UnitPrice", row.UnitPrice));
                    cmd.Parameters.Add(new SqlParameter("@Qty", row.Qty));
                    cmd.Parameters.Add(new SqlParameter("@Discount", row.Discount));

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return 0;
        }
        public List<Models.OrderDetails> GetOrderByOrderId(string orderId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT OrderID, OD.ProductID, OD.UnitPrice, Qty, Discount, ProductName
                        FROM Sales.OrderDetails AS OD
                        INNER JOIN Production.Products AS P ON OD.ProductID = P.ProductID
                        WHERE OrderID=@OrderId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapOrderDetailsDataToList(dt);
        }

        public void UpdateOrderDeail(List<Models.OrderDetails> orderDetail, int orderId)
        {
            foreach (Models.OrderDetails row in orderDetail)
            {
                string sql = @"SELECT count(*)
                        FROM Sales.OrderDetails
                        WHERE OrderID=@OrderId and ProductID=@ProductId";
                int result;
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));
                    cmd.Parameters.Add(new SqlParameter("@ProductID", row.ProductId));

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }
                if (result == 1)//已存在
                {
                    sql = @"UPDATE Sales.OrderDetails SET 
                                UnitPrice=@UnitPrice,Qty=@Qty,Discount=@Discount
                            WHERE OrderID=@OrderId and ProductID=@ProductId";
                }
                else
                {
                    sql = @"INSERT INTO Sales.OrderDetails(
	                            OrderID, ProductID, UnitPrice, Qty, Discount
                            )VALUES(
						        @OrderId, @ProductID, @UnitPrice, @Qty, @Discount
					        )";
                }
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));
                    cmd.Parameters.Add(new SqlParameter("@ProductID", row.ProductId));
                    cmd.Parameters.Add(new SqlParameter("@UnitPrice", row.UnitPrice));
                    cmd.Parameters.Add(new SqlParameter("@Qty", row.Qty));
                    cmd.Parameters.Add(new SqlParameter("@Discount", row.Discount));

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 刪除訂單明細 By productId
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        public void DeleteOrderDetailByProductId(String orderId, String productId)
        {
            try
            {
                string sql = "DELETE FROM Sales.OrderDetails  Where OrderID=@orderId and ProductID=@productId";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@orderId", orderId));
                    cmd.Parameters.Add(new SqlParameter("@productId", productId));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刪除訂單明細
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        public void DeleteOrderDetail(String orderId)
        {
            try
            {
                string sql = "DELETE FROM Sales.OrderDetails  Where OrderID=@orderId";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@orderId", orderId));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Models.OrderDetails> MapOrderDetailsDataToList(DataTable orderData)
        {
            List<Models.OrderDetails> result = new List<OrderDetails>();

            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new OrderDetails()
                {
                    OrderId = (int)row["OrderID"],
                    ProductName = (string)row["ProductName"],
                    ProductId = (int)row["ProductID"],
                    UnitPrice = (decimal)row["UnitPrice"],
                    Qty = (short)row["Qty"],
                    Discount = (decimal)row["Discount"]
                });
            }
            return result;
        }
    }
}