using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PawfectSupplies.Pages.User
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT ProductID, ProductName, Price, ImageUrl FROM Products", con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                ProductsRepeater.DataSource = dt;
                ProductsRepeater.DataBind();
            }
        }

        protected void ProductsRepeater_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                // Check if user is logged in
                if (Session["UserID"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                // Get the ProductID from the CommandArgument
                int productId = Convert.ToInt32(e.CommandArgument);
                int userId = Convert.ToInt32(Session["UserID"]);

                // Add the product to the cart
                AddToCart(userId, productId);

                // Update the cart count in the master page
                var masterPage = this.Master as PawfectSupplies.MasterPages.UserMaster;
                if (masterPage != null)
                {
                    masterPage.UpdateCartCount(); // Call the method from the master page
                }
            }
        }


        private void AddToCart(int userId, int productId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "IF EXISTS (SELECT 1 FROM Cart WHERE UserID = @UserID AND ProductID = @ProductID) " +
                    "BEGIN " +
                    "   UPDATE Cart SET Quantity = Quantity + 1 WHERE UserID = @UserID AND ProductID = @ProductID " +
                    "END " +
                    "ELSE " +
                    "BEGIN " +
                    "   INSERT INTO Cart (UserID, ProductID, Quantity) VALUES (@UserID, @ProductID, 1) " +
                    "END", con);

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@ProductID", productId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}