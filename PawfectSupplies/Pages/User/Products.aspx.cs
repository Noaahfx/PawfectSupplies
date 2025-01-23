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
                SqlCommand cmd = new SqlCommand(
                    @"SELECT P.ProductID, P.ProductName, P.Price, P.ImageUrl,
                             ISNULL(C.Quantity, 0) AS Quantity
                      FROM Products P
                      LEFT JOIN Cart C ON P.ProductID = C.ProductID AND C.UserID = @UserID", con);

                cmd.Parameters.AddWithValue("@UserID", Session["UserID"] ?? -1);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                ProductsRepeater.DataSource = dt;
                ProductsRepeater.DataBind();
            }
        }

        protected void ProductsRepeater_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item ||
                e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                var row = e.Item.DataItem as DataRowView;

                // Extract data from the current row
                int quantity = Convert.ToInt32(row["Quantity"]);

                // Find controls
                var phAddToCart = e.Item.FindControl("phAddToCart") as System.Web.UI.WebControls.PlaceHolder;
                var phQuantityControls = e.Item.FindControl("phQuantityControls") as System.Web.UI.WebControls.PlaceHolder;
                var lblQuantity = e.Item.FindControl("lblQuantity") as System.Web.UI.WebControls.Label;

                // Toggle UI based on quantity
                if (quantity > 0)
                {
                    phAddToCart.Visible = false;
                    phQuantityControls.Visible = true;
                    lblQuantity.Text = quantity.ToString();
                }
                else
                {
                    phAddToCart.Visible = true;
                    phQuantityControls.Visible = false;
                }
            }
        }

        protected void ProductsRepeater_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);
            int userId = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : -1;

            if (userId == -1)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            switch (e.CommandName)
            {
                case "AddToCart":
                    ModifyCartItem(connectionString, userId, productId, 1);
                    break;

                case "IncreaseQuantity":
                    ModifyCartItem(connectionString, userId, productId, 1);
                    break;

                case "DecreaseQuantity":
                    int quantity = GetCartQuantity(connectionString, userId, productId) - 1;
                    if (quantity > 0)
                    {
                        ModifyCartItem(connectionString, userId, productId, -1);
                    }
                    else
                    {
                        RemoveCartItem(connectionString, userId, productId);
                    }
                    break;
            }

            // Reload products to ensure UI state reflects the updated cart
            LoadProducts();

            // Update cart count in master page
            var masterPage = this.Master as PawfectSupplies.MasterPages.UserMaster;
            if (masterPage != null)
            {
                masterPage.UpdateCartCount();
            }
        }

        private void ModifyCartItem(string connectionString, int userId, int productId, int change)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "IF EXISTS (SELECT 1 FROM Cart WHERE UserID = @UserID AND ProductID = @ProductID) " +
                    "BEGIN " +
                    "   UPDATE Cart SET Quantity = Quantity + @Change WHERE UserID = @UserID AND ProductID = @ProductID " +
                    "END " +
                    "ELSE " +
                    "BEGIN " +
                    "   INSERT INTO Cart (UserID, ProductID, Quantity) VALUES (@UserID, @ProductID, 1) " +
                    "END", con);

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@Change", change);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void RemoveCartItem(string connectionString, int userId, int productId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Cart WHERE UserID = @UserID AND ProductID = @ProductID", con);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@ProductID", productId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private int GetCartQuantity(string connectionString, int userId, int productId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT Quantity FROM Cart WHERE UserID = @UserID AND ProductID = @ProductID", con);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@ProductID", productId);

                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
    }
}
