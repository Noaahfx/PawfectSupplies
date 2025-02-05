using System;
using System.Web;

namespace PawfectSupplies.Pages.User
{
    public partial class GoogleLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string clientId = System.Configuration.ConfigurationManager.AppSettings["GoogleClientID"];
            string redirectUri = System.Configuration.ConfigurationManager.AppSettings["GoogleRedirectUri"];
            string scope = "https://www.googleapis.com/auth/userinfo.email";

            string googleAuthUrl = $"https://accounts.google.com/o/oauth2/auth?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope={scope}&access_type=online";

            Response.Redirect(googleAuthUrl);
        }
    }
}
