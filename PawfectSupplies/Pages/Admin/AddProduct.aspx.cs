using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace PawfectSupplies.Pages.Admin
{
    public partial class AddProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Pages/User/Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadCategories();
            }
        }

        private void LoadCategories()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT CategoryID, Name FROM Categories ORDER BY Name";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    ddlCategory.DataSource = dt;  // ✅ Correctly setting the DataTable as DataSource
                    ddlCategory.DataTextField = "Name";
                    ddlCategory.DataValueField = "CategoryID";
                    ddlCategory.DataBind();

                    // Add default option at the top
                    ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", ""));
                }
            }
        }


        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtProductName.Text.Trim();
                string priceText = txtPrice.Text.Trim();
                string stockText = txtStock.Text.Trim();
                string ratingText = txtRating.Text.Trim();
                string categoryIDText = ddlCategory.SelectedValue;
                string imageUrl = txtImageUrl.Text.Trim();

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(priceText) || string.IsNullOrWhiteSpace(stockText) || string.IsNullOrWhiteSpace(imageUrl) || string.IsNullOrWhiteSpace(categoryIDText))
                {
                    errorMessage.Text = "⚠️ All fields are required!";
                    errorMessage.CssClass = "bg-red-500 text-white text-center p-3 rounded-md shadow-md";
                    errorMessage.Visible = true;
                    return;
                }

                decimal price;
                int stock, categoryID;
                decimal rating = 0;

                if (!decimal.TryParse(priceText, out price) || !int.TryParse(stockText, out stock) || !int.TryParse(categoryIDText, out categoryID))
                {
                    errorMessage.Text = "⚠️ Invalid number format for price, stock, or category!";
                    errorMessage.CssClass = "bg-red-500 text-white text-center p-3 rounded-md shadow-md";
                    errorMessage.Visible = true;
                    return;
                }

                // Ensure rating is between 0 and 5
                if (!string.IsNullOrWhiteSpace(ratingText) && (!decimal.TryParse(ratingText, out rating) || rating < 0 || rating > 5))
                {
                    errorMessage.Text = "⚠️ Rating must be between 0 and 5!";
                    errorMessage.CssClass = "bg-red-500 text-white text-center p-3 rounded-md shadow-md";
                    errorMessage.Visible = true;
                    return;
                }

                string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Products 
                            (ProductName, Price, Stock, Rating, CategoryID, ImageUrl) 
                          VALUES 
                            (@Name, @Price, @Stock, @Rating, @CategoryID, @ImageUrl)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@Stock", stock);
                        cmd.Parameters.AddWithValue("@Rating", rating);
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                        cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                Response.Redirect("ManageProducts.aspx");
            }
            catch (Exception ex)
            {
                errorMessage.Text = "⚠️ Error: " + ex.Message;
                errorMessage.CssClass = "bg-red-500 text-white text-center p-3 rounded-md shadow-md";
                errorMessage.Visible = true;
            }
        }
    }
}
