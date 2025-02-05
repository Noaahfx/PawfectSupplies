using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using PawfectSupplies.Helpers; // Reuse Email Helper

namespace PawfectSupplies.Pages.User
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            lblMessage.CssClass = "text-sm text-center block mt-2";

            string username = txtUsername.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                lblMessage.Text = "❌ Please enter your username.";
                lblMessage.CssClass += " text-red-500";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT UserID, Email, IsVerified FROM Users WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            bool isVerified = Convert.ToBoolean(reader["IsVerified"]);
                            string email = reader["Email"].ToString();
                            string userId = reader["UserID"].ToString();

                            if (!isVerified)
                            {
                                lblMessage.Text = "❌ This account has not been verified. Please check your email for verification.";
                                lblMessage.CssClass += " text-red-500";
                                return;
                            }

                            // Generate OTP
                            string otpCode = GenerateOTP();
                            StoreOTP(userId, otpCode);

                            // Send OTP Email
                            EmailHelper.SendEmailOTP(email, otpCode);

                            // Store session for OTP verification
                            Session["PendingPasswordResetUserID"] = userId;
                            Session["PendingPasswordResetUsername"] = username;

                            // Redirect to Verify OTP Page
                            Response.Redirect("~/Pages/User/VerifyOTP.aspx");
                        }
                        else
                        {
                            lblMessage.Text = "❌ Username not found.";
                            lblMessage.CssClass += " text-red-500";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ An error occurred. Please try again later.";
                lblMessage.CssClass += " text-red-500";
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void StoreOTP(string userId, string otpCode)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Users SET Email2FA_Code = @OTP, Email2FA_Expiry = DATEADD(MINUTE, 3, GETDATE()) WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OTP", otpCode);
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
