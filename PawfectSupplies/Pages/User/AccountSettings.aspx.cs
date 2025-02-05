using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace PawfectSupplies.Pages.User
{
    public partial class AccountSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx"); // Redirect unauthorized users
                return;
            }

            if (!IsPostBack)
            {
                LoadMFAStatus();
            }
        }


        private void LoadMFAStatus()
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx"); // Redirect if user is not logged in
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MFA_Type, Is2FAEnabled FROM Users WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        bool is2FAEnabled = reader["Is2FAEnabled"] != DBNull.Value && Convert.ToBoolean(reader["Is2FAEnabled"]);
                        string mfaType = reader["MFA_Type"] != DBNull.Value ? reader["MFA_Type"].ToString() : "None";

                        // ✅ Change MFA text color to **gray** instead of red
                        litMFAStatus.Text = $"<span class='text-gray-600'>Current 2FA Method: <strong>{mfaType}</strong></span>";

                        // ✅ Ensure dropdown selects the current MFA method
                        ListItem selectedItem = ddlMFA.Items.FindByValue(mfaType);
                        if (selectedItem != null)
                        {
                            ddlMFA.ClearSelection();
                            selectedItem.Selected = true;
                        }
                    }
                    else
                    {
                        litMFAStatus.Text = "<span class='text-gray-600'>No Multi-Factor Authentication enabled.</span>";
                    }
                }
            }
        }


        protected void btnSaveMFA_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            string selectedMFA = ddlMFA.SelectedValue;
            string password = txtMfaPassword.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // 🔹 Verify Password Before Changing MFA Settings
                string checkPasswordQuery = "SELECT PasswordHash FROM Users WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(checkPasswordQuery, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    string storedHash = cmd.ExecuteScalar()?.ToString();

                    if (storedHash == null || !BCrypt.Net.BCrypt.Verify(password, storedHash))
                    {
                        lblMFAStatus.Text = "<span class='text-red-500'>❌ Incorrect password. Please try again.</span>";
                        return;
                    }
                }

                // 🔹 Update MFA Settings
                string updateMFAQuery = "UPDATE Users SET MFA_Type = @MFA, Is2FAEnabled = @Enabled WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(updateMFAQuery, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@MFA", selectedMFA);
                    cmd.Parameters.AddWithValue("@Enabled", selectedMFA == "None" ? 0 : 1);
                    cmd.ExecuteNonQuery();
                }
            }

            // ✅ Store MFA Update Message in Session Before Logging Out
            Session["MFA_Confirmation"] = $"Multi-Factor Authentication updated to <strong>{selectedMFA}</strong>";

            // ✅ Clear the session (logs out the user)
            Session.Clear();

            // ✅ Redirect to `AccountUpdated.aspx`
            Response.Redirect("AccountUpdated.aspx");
        }



        private bool ValidatePassword(int userId, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT PasswordHash FROM Users WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    con.Open();
                    string storedHash = cmd.ExecuteScalar()?.ToString();
                    return BCrypt.Net.BCrypt.Verify(password, storedHash);
                }
            }
        }

        private void GenerateAndSendEmailOTP(int userId)
        {
            string otpCode = new Random().Next(100000, 999999).ToString();
            DateTime expiryTime = DateTime.Now.AddMinutes(3);
            string email = "";

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Users SET Email2FA_Code = @Code, Email2FA_Expiry = @Expiry WHERE UserID = @UserID; " +
                               "SELECT Email FROM Users WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@Code", otpCode);
                    cmd.Parameters.AddWithValue("@Expiry", expiryTime);
                    con.Open();
                    email = cmd.ExecuteScalar()?.ToString();
                }
            }

            if (!string.IsNullOrEmpty(email))
                SendEmail(email, otpCode);
        }

        private void SendEmail(string toEmail, string otpCode)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("your-email@example.com");
            mail.To.Add(toEmail);
            mail.Subject = "Your OTP Code for 2FA Login";
            mail.Body = $"Your One-Time Password (OTP) is: <strong>{otpCode}</strong>. This code expires in 3 minutes.";
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.your-email-provider.com");
            smtp.Credentials = new NetworkCredential("your-email@example.com", "your-email-password");
            smtp.EnableSsl = true;
            smtp.Port = 587;

            smtp.Send(mail);
        }

        private bool IsValidPassword(string password)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
        }


        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx"); // Ensure user is logged in
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            string currentPassword = txtCurrentPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmNewPassword.Text.Trim();

            // 🔹 Validate Inputs
            lblPasswordMessage.Text = ""; // Clear previous messages
            lblPasswordMessage.CssClass = "text-sm block mt-2"; // Reset CSS

            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                lblPasswordMessage.Text = "❌ All fields are required.";
                lblPasswordMessage.CssClass += " text-red-500"; // Apply red color
                return;
            }

            if (newPassword != confirmPassword)
            {
                lblPasswordMessage.Text = "❌ New passwords do not match.";
                lblPasswordMessage.CssClass += " text-red-500";
                return;
            }

            if (!IsValidPassword(newPassword))
            {
                lblPasswordMessage.Text = "❌ Password must be at least 8 characters long and contain letters, numbers, and special characters.";
                lblPasswordMessage.CssClass += " text-red-500";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // 🔹 Verify Old Password
                string checkPasswordQuery = "SELECT PasswordHash FROM Users WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(checkPasswordQuery, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    string storedHash = cmd.ExecuteScalar()?.ToString();

                    if (storedHash == null || !BCrypt.Net.BCrypt.Verify(currentPassword, storedHash))
                    {
                        lblPasswordMessage.Text = "❌ Current password is incorrect.";
                        lblPasswordMessage.CssClass += " text-red-500";
                        return;
                    }
                }

                // 🔹 Update Password in Database
                string updatePasswordQuery = "UPDATE Users SET PasswordHash = @NewPassword WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(updatePasswordQuery, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@NewPassword", BCrypt.Net.BCrypt.HashPassword(newPassword));
                    cmd.ExecuteNonQuery();
                }
            }

            // ✅ Store Success Message in Session
            Session["Password_Confirmation"] = "Your password has been updated successfully. Please log in again.";

            // ✅ Clear Session (Log Out User)
            Session.Clear();

            // ✅ Redirect to `AccountUpdated.aspx`
            Response.Redirect("AccountUpdated.aspx");
        }


    }
}
