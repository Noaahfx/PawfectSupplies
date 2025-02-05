using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using PawfectSupplies.Utilities;

namespace PawfectSupplies.Pages.User
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFeaturedProducts();
                LoadHeroCarousel();
            }
        }

        private void LoadHeroCarousel()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT ImageUrl, Title, Subtitle, Link FROM HeroImages ORDER BY Priority ASC";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    // Bind images to the carousel
                    HeroCarouselRepeater.DataSource = dt;
                    HeroCarouselRepeater.DataBind();

                    // Bind indicator dots
                    HeroIndicatorsRepeater.DataSource = dt;
                    HeroIndicatorsRepeater.DataBind();
                }
            }
        }

        private void ModifyCartItem(int userId, int productId, int change)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ModifyCartItem", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.Parameters.AddWithValue("@Change", change);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void RemoveCartItem(int userId, int productId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_RemoveCartItem", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@ProductID", productId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private int GetCartQuantity(int userId, int productId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetCartQuantity", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@ProductID", productId);

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        private void LoadFeaturedProducts()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery = @"
            SELECT TOP 6 
                P.ProductID, 
                P.ProductName, 
                P.Price, 
                P.ImageUrl,
                P.Rating, 
                ISNULL(C.Quantity, 0) AS Quantity
            FROM Products P
            LEFT JOIN Cart C ON P.ProductID = C.ProductID AND C.UserID = @UserID
            WHERE P.IsFeatured = 1";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
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

                    FeaturedProductsRepeater.DataSource = dt;
                    FeaturedProductsRepeater.DataBind();
                }
            }
        }

        protected void FeaturedProductsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

                // Use the reusable rating logic
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

        protected void FeaturedProductsRepeater_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);
            int userId = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : -1;

            if (userId == -1)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            switch (e.CommandName)
            {
                case "AddToCart":
                    ModifyCartItem(userId, productId, 1);
                    break;

                case "IncreaseQuantity":
                    ModifyCartItem(userId, productId, 1);
                    break;

                case "DecreaseQuantity":
                    int quantity = GetCartQuantity(userId, productId) - 1;
                    if (quantity > 0)
                    {
                        ModifyCartItem(userId, productId, -1);
                    }
                    else
                    {
                        RemoveCartItem(userId, productId);
                    }
                    break;
            }

            LoadFeaturedProducts();

            var masterPage = this.Master as PawfectSupplies.MasterPages.UserMaster;
            if (masterPage != null)
            {
                masterPage.UpdateCartCount();
            }
        }

    }
}
