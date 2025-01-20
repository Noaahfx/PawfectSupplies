using System;
using System.Web.UI;

namespace PawfectSupplies.MasterPages
{
    public partial class UserMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is logged in (session contains username)
            if (Session["Username"] != null)
            {
                // User is logged in, show the user dropdown and display username
                litUsername.Text = "Welcome, " + Session["Username"].ToString();
                phUserDropdown.Visible = true; // Show the dropdown
                phLoginLink.Visible = false;   // Hide the login link
            }
            else
            {
                // User is not logged in, show the login link
                phUserDropdown.Visible = false;  // Hide the dropdown
                phLoginLink.Visible = true;      // Show the login link
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear session on logout
            Session.Clear();
            Session.Abandon();

            // Redirect to login page
            Response.Redirect("~/Pages/User/Login.aspx");
        }
    }
}
