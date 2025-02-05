using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using Newtonsoft.Json;

namespace PawfectSupplies.Pages.Admin
{
    public partial class ViewReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Pages/User/Login.aspx");
            }
        }

        private static string GetConnectionString() => ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

        [WebMethod]
        public static string GetRevenueData()
        {
            List<string> labels = new List<string>();
            List<decimal> values = new List<decimal>();

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT FORMAT(OrderDate, 'yyyy-MM') AS Month, SUM(TotalPrice) AS Revenue FROM Orders GROUP BY FORMAT(OrderDate, 'yyyy-MM') ORDER BY Month";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        labels.Add(reader["Month"].ToString());
                        values.Add(Convert.ToDecimal(reader["Revenue"]));
                    }
                }
            }

            return JsonConvert.SerializeObject(new { labels, values });
        }

        [WebMethod]
        public static string GetTopSellingProducts()
        {
            List<string> labels = new List<string>();
            List<int> values = new List<int>();

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = @"
            SELECT TOP 5 P.ProductName, SUM(O.Quantity) AS TotalSold
            FROM OrderDetails O
            INNER JOIN Products P ON O.ProductID = P.ProductID
            GROUP BY P.ProductName
            ORDER BY TotalSold DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        labels.Add(reader["ProductName"].ToString());
                        values.Add(Convert.ToInt32(reader["TotalSold"]));
                    }
                }
            }

            return JsonConvert.SerializeObject(new { labels, values });
        }

        [WebMethod]
        public static string GetSalesDistribution()
        {
            List<string> labels = new List<string>();
            List<int> values = new List<int>();

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT C.Name, COUNT(O.OrderID) AS Sales FROM Orders O INNER JOIN OrderDetails OD ON O.OrderID = OD.OrderID INNER JOIN Products P ON OD.ProductID = P.ProductID INNER JOIN Categories C ON P.CategoryID = C.CategoryID GROUP BY C.Name";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        labels.Add(reader["Name"].ToString());
                        values.Add(Convert.ToInt32(reader["Sales"]));
                    }
                }
            }

            return JsonConvert.SerializeObject(new { labels, values });
        }

        [WebMethod]
        public static string GetUserRegistrations()
        {
            List<string> labels = new List<string>();
            List<int> values = new List<int>();

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = @"
            SELECT FORMAT(CreatedAt, 'yyyy-MM') AS Month, COUNT(UserID) AS NewUsers
            FROM Users
            WHERE Role = 'User'
            GROUP BY FORMAT(CreatedAt, 'yyyy-MM')
            ORDER BY Month";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        labels.Add(reader["Month"].ToString());
                        values.Add(Convert.ToInt32(reader["NewUsers"]));
                    }
                }
            }

            return JsonConvert.SerializeObject(new { labels, values });
        }
    }
}
