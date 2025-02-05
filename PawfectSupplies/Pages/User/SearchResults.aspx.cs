using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using PawfectSupplies.Utilities;

namespace PawfectSupplies.Pages.User
{
    public partial class SearchResults : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string query = Request.QueryString["query"];

                if (string.IsNullOrEmpty(query))
                {
                    ShowErrorMessage("Please enter a search term.");
                    return;
                }

                LoadSearchResults(query.Trim());
            }
        }

        private void LoadSearchResults(string query)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery = @"
                    SELECT P.ProductID, P.ProductName, P.Price, P.ImageUrl, P.Rating, 
                           ISNULL(C.Quantity, 0) AS Quantity
                    FROM Products P
                    LEFT JOIN Cart C ON P.ProductID = C.ProductID AND C.UserID = @UserID
                    WHERE LOWER(P.ProductName) LIKE LOWER(@Query)
                    OR LOWER(P.Description) LIKE LOWER(@Query)";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Query", $"%{query}%");
                    cmd.Parameters.AddWithValue("@UserID", Session["UserID"] ?? -1);

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    // Ensure 'Rating' column exists before binding
                    if (!dt.Columns.Contains("Rating"))
                    {
                        dt.Columns.Add("Rating", typeof(decimal));
                        foreach (DataRow row in dt.Rows)
                        {
                            row["Rating"] = 0; // Default to 0 if not available
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        SearchResultsRepeater.DataSource = dt;
                        SearchResultsRepeater.DataBind();
                    }
                    else
                    {
                        ShowErrorMessage("No products found for your search term.");
                    }
                }
            }
        }

        protected void SearchResultsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var row = e.Item.DataItem as DataRowView;
                int quantity = Convert.ToInt32(row["Quantity"]);
                decimal rating = row["Rating"] != DBNull.Value ? Convert.ToDecimal(row["Rating"]) : 0;

                var phAddToCart = e.Item.FindControl("phAddToCart") as PlaceHolder;
                var phQuantityControls = e.Item.FindControl("phQuantityControls") as PlaceHolder;
                var lblQuantity = e.Item.FindControl("lblQuantity") as Label;
                var phRating = e.Item.FindControl("phRating") as PlaceHolder;
                var litStars = e.Item.FindControl("litStars") as Literal;
                var lblRatingValue = e.Item.FindControl("lblRatingValue") as Label;

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

                // Use reusable rating logic
                if (rating > 0)
                {
                    phRating.Visible = true;
                    lblRatingValue.Text = rating.ToString("0.0");
                    litStars.Text = RatingHelper.GenerateStarRatingHTML(rating);
                }
                else
                {
                    phRating.Visible = false;
                }
            }
        }

        protected void SearchResultsRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
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

            // 🔹 Refresh Cart Count in Navbar
            var masterPage = this.Master as PawfectSupplies.MasterPages.UserMaster;
            if (masterPage != null)
            {
                masterPage.UpdateCartCount();
            }

            // 🔹 Refresh Search Results to reflect updated quantities
            LoadSearchResults(Request.QueryString["query"]);
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

        private void ShowErrorMessage(string message)
        {
            errorMessage.Visible = true;
            litErrorMessage.Text = message;
            SearchResultsRepeater.DataSource = null;
            SearchResultsRepeater.DataBind();
        }
    }
}
