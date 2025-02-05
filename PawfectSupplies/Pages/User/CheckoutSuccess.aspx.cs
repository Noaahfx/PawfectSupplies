using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace PawfectSupplies.Pages.User
{
    public partial class CheckoutSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            SaveOrder(userId);
            ClearCart(userId);
        }

        private void SaveOrder(int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // 🛠 FIX: Calculate total dynamically (Quantity * Price)
                string orderQuery = @"
            INSERT INTO Orders (UserID, TotalPrice, OrderDate, OrderStatus) 
            OUTPUT INSERTED.OrderID
            VALUES (@UserID, 
                (SELECT SUM(c.Quantity * p.Price)
                 FROM Cart c
                 INNER JOIN Products p ON c.ProductID = p.ProductID
                 WHERE c.UserID = @UserID),
                GETDATE(), 'Pending')";

                int orderId;
                using (SqlCommand cmd = new SqlCommand(orderQuery, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    orderId = (int)cmd.ExecuteScalar();
                }

                // ✅ Insert Order Details
                string orderDetailsQuery = @"
            INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Price) 
            SELECT @OrderID, c.ProductID, c.Quantity, p.Price 
            FROM Cart c 
            INNER JOIN Products p ON c.ProductID = p.ProductID 
            WHERE c.UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(orderDetailsQuery, con))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ClearCart(int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Cart WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
