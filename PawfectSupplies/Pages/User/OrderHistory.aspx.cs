using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PawfectSupplies.Pages.User
{
    public partial class OrderHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx"); // Redirect if user is not logged in
                return;
            }

            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        private void LoadOrders()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT OrderID, OrderDate, TotalPrice, OrderStatus FROM Orders WHERE UserID = @UserID ORDER BY OrderDate DESC";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        gvOrders.DataSource = dt;
                        gvOrders.DataBind();
                    }
                    else
                    {
                        gvOrders.EmptyDataText = "No orders found.";
                        gvOrders.DataBind();
                    }
                }
            }
        }

        public string GetStatusClass(string status)
        {
            switch (status)
            {
                case "Pending":
                    return "bg-yellow-200 text-yellow-900 px-3 py-1 rounded-full text-xs font-semibold !important";
                case "Completed":
                    return "bg-green-200 text-green-900 px-3 py-1 rounded-full text-xs font-semibold !important";
                case "Canceled":
                    return "bg-red-200 text-red-900 px-3 py-1 rounded-full text-xs font-semibold !important";
                default:
                    return "bg-gray-200 text-gray-800 px-3 py-1 rounded-full text-xs font-semibold !important";
            }
        }

    }
}
