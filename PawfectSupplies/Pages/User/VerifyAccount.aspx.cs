using System;
using System.Configuration;
using System.Data.SqlClient;

namespace PawfectSupplies.Pages.User
{
    public partial class VerifyAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string token = Request.QueryString["token"];

            if (string.IsNullOrEmpty(token))
            {
                lblMessage.Text = "Invalid verification link.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Users SET IsVerified = 1, VerificationToken = NULL WHERE VerificationToken = @Token";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Token", token);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Your account has been verified! You can now log in.";
                    }
                    else
                    {
                        lblMessage.Text = "Invalid or expired verification token.";
                    }
                }
            }
        }
    }
}
