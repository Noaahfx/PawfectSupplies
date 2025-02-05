using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using BCrypt.Net;

namespace PawfectSupplies.Pages.User
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure the user has verified OTP before accessing this page
            if (Session["VerifiedPasswordResetUserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            lblMessage.Text = ""; // Clear previous messages

            int userId = Convert.ToInt32(Session["VerifiedPasswordResetUserID"]);
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmNewPassword.Text.Trim();

            // ✅ **Validation 1: Ensure fields are not empty**
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                lblMessage.Text = "<span class='text-red-500'>⚠ All fields are required.</span>";
                return;
            }

            // ✅ **Validation 2: Check if passwords match**
            if (newPassword != confirmPassword)
            {
                lblMessage.Text = "<span class='text-red-500'>❌ Passwords do not match. Please try again.</span>";
                return;
            }

            // ✅ **Validation 3: Check password complexity**
            if (!IsValidPassword(newPassword))
            {
                lblMessage.Text = "<span class='text-red-500'>🔒 Password must be at least 8 characters long and include an uppercase letter, lowercase letter, number, and special character.</span>";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // ✅ **Validation 4: Ensure new password is different from old password**
                    string oldPasswordHash = "";
                    using (SqlCommand cmdCheck = new SqlCommand("SELECT PasswordHash FROM Users WHERE UserID = @UserID", con))
                    {
                        cmdCheck.Parameters.AddWithValue("@UserID", userId);
                        object result = cmdCheck.ExecuteScalar();
                        if (result != null)
                        {
                            oldPasswordHash = result.ToString();
                        }
                    }

                    if (!string.IsNullOrEmpty(oldPasswordHash) && BCrypt.Net.BCrypt.Verify(newPassword, oldPasswordHash))
                    {
                        lblMessage.Text = "<span class='text-red-500'>❌ New password cannot be the same as the old password.</span>";
                        return;
                    }

                    // ✅ **Hash the new password**
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

                    // ✅ **Update Password in Database**
                    string query = "UPDATE Users SET PasswordHash = @PasswordHash WHERE UserID = @UserID";
                    using (SqlCommand cmdUpdate = new SqlCommand(query, con))
                    {
                        cmdUpdate.Parameters.AddWithValue("@UserID", userId);
                        cmdUpdate.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                        int rowsAffected = cmdUpdate.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // ✅ **Clear session and redirect user to AccountUpdated page**
                            Session.Clear();
                            Response.Redirect("AccountUpdated.aspx");
                        }
                        else
                        {
                            lblMessage.Text = "<span class='text-red-500'>⚠ Something went wrong. Please try again later.</span>";
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                lblMessage.Text = "<span class='text-red-500'>⚠ Database error. Please try again later.</span>";
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "<span class='text-red-500'>⚠ An unexpected error occurred. Please try again.</span>";
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // ✅ **Password Complexity Validation**
        private bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
        }
    }
}
