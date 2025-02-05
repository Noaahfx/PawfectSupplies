using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace PawfectSupplies.Pages.Admin
{
    public partial class ManageProducts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Pages/User/Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadProducts();
                LoadCategories();
            }
        }

        private void LoadProducts(string searchQuery = "")
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Products WHERE ProductName LIKE '%' + @Search + '%' ORDER BY ProductID DESC";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Search", searchQuery);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    gvProducts.DataSource = dt;
                    gvProducts.DataBind();
                }
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
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    ddlCategory.DataSource = dt;
                    ddlCategory.DataTextField = "Name";
                    ddlCategory.DataValueField = "CategoryID";
                    ddlCategory.DataBind();

                    ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", ""));
                }
            }
        }

        protected void btnSearchProduct_Click(object sender, EventArgs e)
        {
            LoadProducts(txtSearchProduct.Text.Trim());
        }

        protected void btnSaveProduct_Click(object sender, EventArgs e)
        {
            int productID;
            bool isEditMode = int.TryParse(hfProductID.Value, out productID) && productID > 0;

            string name = txtProductName.Text.Trim();
            decimal price = Convert.ToDecimal(txtPrice.Text.Trim());
            int stock = Convert.ToInt32(txtStock.Text.Trim());
            decimal rating = Convert.ToDecimal(txtRating.Text.Trim());
            int categoryID = Convert.ToInt32(ddlCategory.SelectedValue);
            string imageUrl = txtImageUrl.Text.Trim();

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query;

                if (isEditMode)
                {
                    query = @"UPDATE Products SET 
                                ProductName = @Name, 
                                Price = @Price, 
                                Stock = @Stock, 
                                Rating = @Rating, 
                                CategoryID = @CategoryID, 
                                ImageUrl = @ImageUrl 
                              WHERE ProductID = @ProductID";
                }
                else
                {
                    query = @"INSERT INTO Products 
                                (ProductName, Price, Stock, Rating, CategoryID, ImageUrl) 
                              VALUES 
                                (@Name, @Price, @Stock, @Rating, @CategoryID, @ImageUrl)";
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Stock", stock);
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);

                    if (isEditMode)
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productID);
                    }

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            LoadProducts();
        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProduct")
            {
                int productID = Convert.ToInt32(e.CommandArgument);
                DeleteProduct(productID);
                LoadProducts();
            }
        }

        private void DeleteProduct(int productID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Products WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}