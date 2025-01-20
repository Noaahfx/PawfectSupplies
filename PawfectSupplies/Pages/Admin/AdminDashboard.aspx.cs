using System;
using System.Data.SqlClient;
using System.Configuration;

namespace PawfectSupplies.Pages.Admin
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardStats();
            }
        }

        private void LoadDashboardStats()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Total Products
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Products", con);
                lblTotalProducts.Text = cmd.ExecuteScalar().ToString();

                // Total Users
                cmd.CommandText = "SELECT COUNT(*) FROM Users";
                lblTotalUsers.Text = cmd.ExecuteScalar().ToString();

                // Total Orders
                cmd.CommandText = "SELECT COUNT(*) FROM Orders";
                lblTotalOrders.Text = cmd.ExecuteScalar().ToString();
            }
        }
    }
}
