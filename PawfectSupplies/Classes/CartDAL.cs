using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PawfectSupplies.DataAccess
{
    public class CartDAL
    {
        // Fetching connection string from web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

        public DataTable GetCartItems(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT C.CartID, P.ProductName, P.Price, C.Quantity, (P.Price * C.Quantity) AS TotalPrice " +
                               "FROM Cart C " +
                               "INNER JOIN Products P ON C.ProductID = P.ProductID " +
                               "WHERE C.UserID = @UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public void RemoveFromCart(int cartId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Cart WHERE CartID = @CartID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CartID", cartId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCartQuantity(int cartId, int change)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Cart " +
                    "SET Quantity = CASE WHEN Quantity + @Change > 0 THEN Quantity + @Change ELSE 0 END " +
                    "WHERE CartID = @CartID", con);

                cmd.Parameters.AddWithValue("@Change", change);
                cmd.Parameters.AddWithValue("@CartID", cartId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}
