using System;
using System.Web.UI;
using PawfectSupplies.DataAccess;

namespace PawfectSupplies.MasterPages
{
    public partial class AdminMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                // Redirect non-admin users to login page
                Response.Redirect("~/Login.aspx");
            }

            if (Session["Username"] != null)
            {
                phAdminDropdown.Visible = true;
                litAdminUsername.Text = Session["Username"]?.ToString() ?? "Admin";
            }
            else
            {
                phAdminDropdown.Visible = false;
            }
        }
    }
}
