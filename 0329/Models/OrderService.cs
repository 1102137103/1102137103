using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace _0329.Models
{
    public class OrderService
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
        /// 新增訂單
        /// </summary>
        /// <param name="order"></param>
        /// <returns>訂單編號</returns>
        public int InsertOrder(Models.Order order)
        {
            string sql = @"Insert INTO Sales.Orders
						 (
							CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipperID,Freight,
							ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry
						)
						VALUES
						(
							@CustomerID,@EmployeeID,@OrderDate,@RequiredDate,@ShippedDate,@ShipperID,@Freight,
							@ShipName,@ShipAddress,@ShipCity,@ShipRegion,@ShipPostalCode,@ShipCountry
						)
						Select SCOPE_IDENTITY()
						";
            int orderId;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@CustomerID", order.CustId));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", order.EmpId));
                cmd.Parameters.Add(new SqlParameter("@OrderDate", order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@RequiredDate", order.RequireDdate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@ShipName", order.ShipName));
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));

                orderId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return orderId;

        }
            
            
            

        ///}
        /// <summary>
        /// 依照Id 取得訂單資料
        /// </summary>
        /// <returns></returns>
        public Models.Order GetOrderById(string OrderID)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT 
					O.OrderId,O.CustomerID,C.Companyname As CustName,
	                O.EmployeeID,E.lastname+ E.firstname As EmpName,
	                O.OrderDate,O.RequiredDate,O.ShippedDate,
	                O.ShipperID,S.companyname As ShipperName,O.Freight,
	                O.ShipName,O.ShipAddress,O.ShipCity,O.ShipRegion,O.ShipPostalCode,O.ShipCountry
                    From Sales.Orders As O 
	                INNER JOIN Sales.Customers As C ON O.CustomerID=C.CustomerID
	                INNER JOIN HR.Employees As E On O.EmployeeID=E.EmployeeID
	                inner JOIN Sales.Shippers As S ON O.ShipperID=S.ShipperID
					Where O.OrderID = @OrderID";


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapOrderDataToList(dt).FirstOrDefault();
        }
        /// <summary>
        /// 依照條件取得訂單資料
        /// </summary>
        /// <returns></returns>
        public List<Models.Order> GetOrderByCondtioin(Models.OrderSearchArg arg)
        {

            DataTable dt = new DataTable();
            string sql = @"SELECT 
					O.OrderId,O.CustomerID,C.Companyname As CustName,
	                O.EmployeeID,E.lastname+ E.firstname As EmpName,
	                O.OrderDate,O.RequiredDate,O.ShippedDate,
	                O.ShipperID,S.companyname As ShipperName,O.Freight,
	                O.ShipName,O.ShipAddress,O.ShipCity,O.ShipRegion,O.ShipPostalCode,O.ShipCountry
                    From Sales.Orders As O 
	                INNER JOIN Sales.Customers As C ON O.CustomerID=C.CustomerID
	                INNER JOIN HR.Employees As E On O.EmployeeID=E.EmployeeID
	                inner JOIN Sales.Shippers As S ON O.ShipperID=S.ShipperID
					Where (C.Companyname Like @CustName Or @CustName='') And 
						  (O.OrderDate=@OrderDate Or @OrderDate='')  And 
						  (O.EmployeeID=@EmployeeID Or @EmployeeID='') And 
						  (O.OrderId=@OrderId Or @OrderId='') And 
						  (O.ShipperID=@ShipperId Or @ShipperId='') And 
						  (O.ShippedDate=@ShippedDate Or @ShippedDate='') And 
						  (O.RequiredDate=@RequiredDate Or @RequiredDate='')";


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@CustName", arg.CustName == null ? string.Empty : arg.CustName));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", arg.OrderDate == null ? string.Empty : arg.OrderDate));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", arg.EmployeeID == null ? string.Empty : arg.EmployeeID));
                cmd.Parameters.Add(new SqlParameter("@OrderId", arg.OrderId == null ? string.Empty : arg.OrderId));
                cmd.Parameters.Add(new SqlParameter("@ShipperID", arg.ShipperID == null ? string.Empty : arg.ShipperID));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", arg.ShippedDate == null ? string.Empty : arg.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@RequiredDate", arg.RequiredDate == null ? string.Empty : arg.RequiredDate));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }


            return this.MapOrderDataToList(dt);
        }
        /// <summary>
        /// 刪除訂單
        /// </summary>
        public void DeleteOrderById(string orderId)
        {
            try
            {
                string sql = "Delete FROM Sales.Orders Where orderid=@orderid";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@orderid", orderId));
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
        /// 更新訂單
        /// </summary>
        /// <param name="order"></param>
        public void UpdateOrder(Models.Order order)
        {
            try
            {
                string sql = @"UPDATE Sales.Orders SET 
	                        CustomerID=@custid,EmployeeID=@empid,orderdate=@orderdate,requireddate=@requireddate,
                            shippeddate=@shippeddate,shipperid=@shipperid,freight=@freight,shipname=@shipname,
                            shipaddress=@shipaddress,shipcity=@shipcity,shipregion=@shipregion,
                            shippostalcode=@shippostalcode,shipcountry=@shipcountry                          
                            WHERE orderid=@orderid";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@orderid", order.OrderId));
                    cmd.Parameters.Add(new SqlParameter("@custid", order.CustId));
                    cmd.Parameters.Add(new SqlParameter("@empid", order.EmpId));
                    cmd.Parameters.Add(new SqlParameter("@orderdate", order.Orderdate));
                    cmd.Parameters.Add(new SqlParameter("@requireddate", order.RequireDdate));
                    cmd.Parameters.Add(new SqlParameter("@shippeddate", order.ShippedDate));
                    cmd.Parameters.Add(new SqlParameter("@shipperid", order.ShipperId));
                    cmd.Parameters.Add(new SqlParameter("@freight", order.Freight));
                    cmd.Parameters.Add(new SqlParameter("@shipname", order.ShipName));
                    cmd.Parameters.Add(new SqlParameter("@shipaddress", order.ShipAddress));
                    cmd.Parameters.Add(new SqlParameter("@shipcity", order.ShipCity));
                    cmd.Parameters.Add(new SqlParameter("@shipregion", order.ShipRegion));
                    cmd.Parameters.Add(new SqlParameter("@shippostalcode", order.ShipPostalCode));
                    cmd.Parameters.Add(new SqlParameter("@shipcountry", order.ShipCountry));

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Models.Order> MapOrderDataToList(DataTable orderData)
        {
            List<Models.Order> result = new List<Order>();


            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new Order()
                {
                    CustId = row["CustomerId"].ToString(),
                    CustName = row["CustName"].ToString(),
                    EmpId = (int)row["EmployeeId"],
                    EmpName = row["EmpName"].ToString(),
                    Freight = (decimal)row["Freight"],
                    Orderdate = row["Orderdate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["Orderdate"],
                    OrderId = (int)row["OrderId"],
                    RequireDdate = row["RequiredDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["RequireDdate"],
                    ShipAddress = row["ShipAddress"].ToString(),
                    ShipCity = row["ShipCity"].ToString(),
                    ShipCountry = row["ShipCountry"].ToString(),
                    ShipName = row["ShipName"].ToString(),
                    ShippedDate = row["ShippedDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["ShippedDate"],
                    ShipperId = (int)row["ShipperID"],
                    ShipPostalCode = row["ShipPostalCode"].ToString(),
                    ShipRegion = row["ShipRegion"].ToString()
                });
            }
            return result;
        }

    }
}
