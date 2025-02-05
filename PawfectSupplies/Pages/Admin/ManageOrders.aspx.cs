using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PawfectSupplies.Pages.Admin
{
    public partial class ManageOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Pages/User/Login.aspx");
            }
            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        protected void btnSearchOrder_Click(object sender, EventArgs e)
        {
            LoadOrders(txtSearchOrder.Text.Trim());
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditOrderStatus")
            {
                int orderId = Convert.ToInt32(e.CommandArgument);

                // Retrieve order details from database
                string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT OrderStatus FROM Orders WHERE OrderID = @OrderID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", orderId);
                        con.Open();
                        object status = cmd.ExecuteScalar();

                        if (status != null)
                        {
                            hfOrderID.Value = orderId.ToString();
                            ddlOrderStatus.SelectedValue = status.ToString();
                            ScriptManager.RegisterStartupScript(this, GetType(), "openModal", "openEditModal();", true);
                        }
                    }
                }
            }
        }

        private void LoadOrders(string searchQuery = "")
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT OrderID, UserID, OrderDate, TotalPrice, OrderStatus 
                                 FROM Orders
                                 WHERE (CAST(OrderID AS NVARCHAR) LIKE '%' + @Search + '%' 
                                        OR CAST(UserID AS NVARCHAR) LIKE '%' + @Search + '%')
                                 ORDER BY OrderDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Search", searchQuery);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    gvOrders.DataSource = dt;
                    gvOrders.DataBind();
                }
            }
        }

        protected void btnSaveOrder_Click(object sender, EventArgs e)
        {
            int orderID = Convert.ToInt32(hfOrderID.Value);
            string status = ddlOrderStatus.SelectedValue;

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Orders SET OrderStatus = @Status WHERE OrderID = @OrderID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderID);
                    cmd.Parameters.AddWithValue("@Status", status);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            LoadOrders();
        }

        protected string GetStatusClass(string status)
        {
            switch (status)
            {
                case "Pending":
                    return "bg-yellow-100 text-yellow-800 px-3 py-1 rounded-full text-xs font-semibold";
                case "Completed":
                    return "bg-green-100 text-green-800 px-3 py-1 rounded-full text-xs font-semibold";
                case "Canceled":
                    return "bg-red-100 text-red-800 px-3 py-1 rounded-full text-xs font-semibold";
                default:
                    return "bg-gray-100 text-gray-800 px-3 py-1 rounded-full text-xs font-semibold"; // Fallback for unknown statuses
            }
        }

    }
}
