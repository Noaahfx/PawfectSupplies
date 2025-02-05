using System;
using System.Web.UI;

namespace PawfectSupplies.Pages.User
{
    public partial class AccountUpdated : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // ✅ Check if an update message exists
                if (Session["MFA_Confirmation"] != null)
                {
                    litMFAStatusMessage.Text = $"✅ {Session["MFA_Confirmation"].ToString()}";
                    Session.Remove("MFA_Confirmation"); // Remove after displaying
                }
                else
                {
                    litMFAStatusMessage.Text = "✅ Your account settings have been updated successfully.";
                }
            }
        }

        // ✅ Redirect User to Login Page When Clicking "Login Again"
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}
