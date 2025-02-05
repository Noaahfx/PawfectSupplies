using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using BCrypt.Net;

namespace PawfectSupplies.Pages.User
{
    public partial class CompleteSignup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["GoogleEmail"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            string email = Session["GoogleEmail"].ToString();
            txtEmail.Text = email;
            txtEmail.ReadOnly = true; // Prevents manual changes

            if (!IsPostBack)
            {
                // 🔹 Check if user already exists
                if (UserExists(email))
                {
                    AutoLogin(email);
                }
            }
        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            lblSuccessMessage.Text = "";

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string email = Session["GoogleEmail"].ToString();

            // 🔹 **Validation**
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                lblErrorMessage.Text = "All fields are required.";
                return;
            }

            if (!IsValidPassword(password))
            {
                lblErrorMessage.Text = "Password must be at least 8 characters long and contain a number, uppercase, and lowercase letter.";
                return;
            }

            if (password != confirmPassword)
            {
                lblErrorMessage.Text = "Passwords do not match.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // 🔹 Check if Username already exists
                using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", con))
                {
                    checkCmd.Parameters.AddWithValue("@Username", username);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        lblErrorMessage.Text = "This Username is already taken.";
                        return;
                    }
                }

                // 🔹 Insert New User (Google Sign-up)
                string query = @"
                    INSERT INTO Users (Username, Email, PasswordHash, Role, IsVerified, CreatedAt) 
                    VALUES (@Username, @Email, @PasswordHash, 'User', 1, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", BCrypt.Net.BCrypt.HashPassword(password));

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        AutoLogin(email);
                    }
                    else
                    {
                        lblErrorMessage.Text = "Something went wrong. Please try again.";
                    }
                }
            }
        }

        private bool UserExists(string email)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    con.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        private void AutoLogin(string email)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, Role, Is2FAEnabled FROM Users WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Session["UserID"] = reader["UserID"].ToString();
                        Session["Username"] = reader["Username"].ToString();
                        Session["Email"] = email;
                        Session["Role"] = reader["Role"].ToString();

                        bool is2FAEnabled = reader["Is2FAEnabled"] != DBNull.Value && Convert.ToBoolean(reader["Is2FAEnabled"]);
                        if (is2FAEnabled)
                        {
                            Session["Pending2FAUserID"] = reader["UserID"].ToString();
                            Response.Redirect("VerifyOTP.aspx");
                        }
                        else
                        {
                            Response.Redirect("AccountDetails.aspx");
                        }
                    }
                }
            }
        }

        private bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
        }
    }
}
