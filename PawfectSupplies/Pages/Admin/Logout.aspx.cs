using System;
namespace PawfectSupplies.Pages.Admin
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear all session data
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Pages/User/Login.aspx"); // Redirect to user login page
        }
    }
}
