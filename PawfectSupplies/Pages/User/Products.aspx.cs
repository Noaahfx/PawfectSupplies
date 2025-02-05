using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace PawfectSupplies.Pages.User
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadProducts(null); // Load all products initially
            }
        }

        private void LoadCategories()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT CategoryID, Name FROM Categories ORDER BY Name";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    ddlCategoryFilter.DataSource = dt;
                    ddlCategoryFilter.DataTextField = "Name";
                    ddlCategoryFilter.DataValueField = "CategoryID";
                    ddlCategoryFilter.DataBind();
                    ddlCategoryFilter.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All Categories", ""));
                }
            }
        }

        protected void ddlCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            int? categoryID = string.IsNullOrEmpty(ddlCategoryFilter.SelectedValue) ? (int?)null : Convert.ToInt32(ddlCategoryFilter.SelectedValue);
            LoadProducts(categoryID);
        }

        private void LoadProducts(int? categoryID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT C.CategoryID, C.Name AS CategoryName, 
                   P.ProductID, P.ProductName, P.Price, P.ImageUrl, P.Rating,
                   ISNULL(Cart.Quantity, 0) AS Quantity
            FROM Categories C
            INNER JOIN Products P ON C.CategoryID = P.CategoryID
            LEFT JOIN Cart ON P.ProductID = Cart.ProductID AND Cart.UserID = @UserID";

                if (categoryID.HasValue)
                    query += " WHERE C.CategoryID = @CategoryID";

                query += " ORDER BY C.Name, P.ProductName";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", Session["UserID"] ?? -1);
                    if (categoryID.HasValue)
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID.Value);

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    // Check if 'Rating' column exists before using it
                    if (!dt.Columns.Contains("Rating"))
                    {
                        dt.Columns.Add("Rating", typeof(decimal));
                        foreach (DataRow row in dt.Rows)
                        {
                            row["Rating"] = 0; // Default rating to 0 if not found
                        }
                    }

                    // Group products by category
                    var groupedData = dt.AsEnumerable()
                        .GroupBy(row => new { CategoryID = row["CategoryID"], CategoryName = row["CategoryName"] })
                        .Select(g => new
                        {
                            CategoryID = g.Key.CategoryID,
                            CategoryName = g.Key.CategoryName,
                            Products = g.CopyToDataTable()
                        }).ToList();

                    CategoryRepeater.DataSource = groupedData;
                    CategoryRepeater.DataBind();
                }
            }
        }
        protected void ProductsRepeater_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item ||
                e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                var row = e.Item.DataItem as DataRowView;
                int quantity = Convert.ToInt32(row["Quantity"]);
                decimal rating = row["Rating"] != DBNull.Value ? Convert.ToDecimal(row["Rating"]) : 0;

                var phAddToCart = e.Item.FindControl("phAddToCart") as System.Web.UI.WebControls.PlaceHolder;
                var phQuantityControls = e.Item.FindControl("phQuantityControls") as System.Web.UI.WebControls.PlaceHolder;
                var lblQuantity = e.Item.FindControl("lblQuantity") as System.Web.UI.WebControls.Label;
                var phRating = e.Item.FindControl("phRating") as System.Web.UI.WebControls.PlaceHolder;
                var litStars = e.Item.FindControl("litStars") as System.Web.UI.WebControls.Literal;
                var lblRatingValue = e.Item.FindControl("lblRatingValue") as System.Web.UI.WebControls.Label;

                // Handle cart visibility
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
                if (rating > 0)
                {
                    phRating.Visible = true;
                    lblRatingValue.Text = rating.ToString("0.0");
                    litStars.Text = GenerateStarRatingHTML(rating);
                }
                else
                {
                    phRating.Visible = false; // Hide ratings section if no rating
                }
            }
        }

        // Function to generate star rating HTML
        private string GenerateStarRatingHTML(decimal rating)
        {
            int fullStars = (int)Math.Floor(rating);
            decimal fractionalPart = rating - fullStars;
            string starsHTML = "";

            // Full stars
            for (int i = 0; i < fullStars; i++)
            {
                starsHTML += "<i class='fas fa-star text-yellow-500'></i>";
            }

            // Half-filled star if rating has a decimal part
            if (fractionalPart > 0)
            {
                starsHTML += "<i class='fas fa-star-half-alt text-yellow-500'></i>";
            }

            // Empty stars to complete 5
            int remainingStars = 5 - (fullStars + (fractionalPart > 0 ? 1 : 0));
            for (int i = 0; i < remainingStars; i++)
            {
                starsHTML += "<i class='far fa-star text-yellow-500'></i>";
            }

            return starsHTML;
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

            LoadProducts(null);
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
