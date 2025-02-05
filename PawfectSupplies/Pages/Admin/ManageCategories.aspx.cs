using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PawfectSupplies.Pages.Admin
{
    public partial class ManageCategories : System.Web.UI.Page
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

        protected void btnSearchCategory_Click(object sender, EventArgs e)
        {
            LoadCategories(txtSearchCategory.Text.Trim());
        }

        private void LoadCategories(string searchQuery = "")
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT CategoryID, Name FROM Categories 
                                 WHERE Name LIKE '%' + @Search + '%' 
                                 ORDER BY Name ASC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Search", searchQuery);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    gvCategories.DataSource = dt;
                    gvCategories.DataBind();
                }
            }
        }

        protected void gvCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCategory")
            {
                int categoryID = Convert.ToInt32(e.CommandArgument);
                DeleteCategory(categoryID);
                LoadCategories();
            }
        }

        private void DeleteCategory(int categoryID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Categories WHERE CategoryID = @CategoryID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void btnSaveCategory_Click(object sender, EventArgs e)
        {
            int categoryID = Convert.ToInt32(hfCategoryID.Value);
            string name = txtEditCategoryName.Text.Trim();

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Categories SET Name = @Name WHERE CategoryID = @CategoryID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    cmd.Parameters.AddWithValue("@Name", name);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            LoadCategories();
        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtNewCategoryName.Text.Trim();

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Categories (Name) VALUES (@Name)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            LoadCategories();
        }

    }
}
