using System;
using System.Configuration;
using System.Data.SqlClient;

namespace PawfectSupplies.Pages.User
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, Email, PasswordHash FROM Users WHERE Username = @Username";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string storedPasswordHash = reader["PasswordHash"].ToString();

                        // Verify password using BCrypt
                        if (BCrypt.Net.BCrypt.Verify(password, storedPasswordHash))
                        {
                            // Store user details in session
                            Session["UserID"] = reader["UserID"].ToString();
                            Session["Username"] = reader["Username"].ToString();
                            Session["Email"] = reader["Email"].ToString();

                            // Redirect to account details page
                            Response.Redirect("~/Pages/User/AccountDetails.aspx");
                        }
                        else
                        {
                            lblErrorMessage.Text = "Invalid username or password.";
                        }
                    }
                    else
                    {
                        lblErrorMessage.Text = "Invalid username or password.";
                    }
                }
            }
        }
    }
}
