using System;

namespace PawfectSupplies.Pages.User
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear the session and redirect to the login page
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Pages/User/Login.aspx");
        }
    }
}
