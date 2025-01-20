using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PawfectSupplies.Pages.User
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFeaturedProducts();
            }
        }

        private void LoadFeaturedProducts()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT TOP 6 ProductName, Price, ImageUrl FROM Products WHERE IsFeatured = 1", con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                FeaturedProductsRepeater.DataSource = dt;
                FeaturedProductsRepeater.DataBind();
            }
        }
    }
}
