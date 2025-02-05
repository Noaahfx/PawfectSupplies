using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using PawfectSupplies.Helpers; // Ensure EmailHelper.cs is referenced

namespace PawfectSupplies.Pages.User
{
    public partial class Verify2FA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is coming from 2FA login or password reset
            if (Session["Pending2FAUserID"] == null && Session["PendingPasswordResetUserID"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void btnVerifyOTP_Click(object sender, EventArgs e)
        {
            // Determine if the OTP is for 2FA login or password reset
            int userId;
            bool isPasswordReset = false;

            if (Session["Pending2FAUserID"] != null)
            {
                userId = Convert.ToInt32(Session["Pending2FAUserID"]);
            }
            else if (Session["PendingPasswordResetUserID"] != null)
            {
                userId = Convert.ToInt32(Session["PendingPasswordResetUserID"]);
                isPasswordReset = true;
            }
            else
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Check if OTP field is empty before proceeding
            if (string.IsNullOrWhiteSpace(txtOTP.Text))
            {
                litErrorMessage.Text = ""; // Prevents showing an error when empty
                return;
            }

            string enteredOTP = txtOTP.Text.Trim();
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT UserID, Username, Email, Role, Email2FA_Code, Email2FA_Expiry FROM Users WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            string storedOTP = reader["Email2FA_Code"].ToString();
                            DateTime expiryTime = Convert.ToDateTime(reader["Email2FA_Expiry"]);

                            // 🔹 Check if OTP has expired
                            if (DateTime.Now > expiryTime)
                            {
                                litErrorMessage.Text = "<span class='text-red-500'>OTP expired. Please request a new one.</span>";
                                return;
                            }

                            // 🔹 Check if OTP is correct
                            if (enteredOTP == storedOTP)
                            {
                                if (isPasswordReset)
                                {
                                    // ✅ OTP verified for password reset → Redirect to ResetPassword.aspx
                                    Session["VerifiedPasswordResetUserID"] = userId;
                                    Session.Remove("PendingPasswordResetUserID"); // Remove temporary session
                                    Response.Redirect("ResetPassword.aspx");
                                }
                                else
                                {
                                    // ✅ OTP verified for 2FA login → Store user session
                                    Session["UserID"] = reader["UserID"].ToString();
                                    Session["Username"] = reader["Username"].ToString();
                                    Session["Email"] = reader["Email"].ToString();
                                    Session["Role"] = reader["Role"].ToString();

                                    // ✅ Remove temporary session
                                    Session.Remove("Pending2FAUserID");

                                    // ✅ Redirect to AccountDetails.aspx after successful login
                                    Response.Redirect("AccountDetails.aspx");
                                }
                            }
                            else
                            {
                                litErrorMessage.Text = "<span class='text-red-500'>Invalid OTP. Please try again.</span>";
                            }
                        }
                        else
                        {
                            Response.Redirect("Login.aspx");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                litErrorMessage.Text = "<span class='text-red-500'>Database error. Please try again later.</span>";
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                litErrorMessage.Text = "<span class='text-red-500'>An unexpected error occurred. Please try again.</span>";
                Console.WriteLine("Error: " + ex.Message);
            }
        }



        protected void btnResendOTP_Click(object sender, EventArgs e)
        {
            if (Session["Pending2FAUserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["Pending2FAUserID"]);
            string email = "";
            string newOTP = new Random().Next(100000, 999999).ToString();
            DateTime expiryTime = DateTime.Now.AddMinutes(3);

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Users SET Email2FA_Code = @OTP, Email2FA_Expiry = @Expiry WHERE UserID = @UserID; " +
                                   "SELECT Email FROM Users WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@OTP", newOTP);
                        cmd.Parameters.AddWithValue("@Expiry", expiryTime);
                        con.Open();
                        email = cmd.ExecuteScalar()?.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(email))
                {
                    EmailHelper.SendEmailOTP(email, newOTP);
                    litErrorMessage.Text = "<span class='text-green-500'>✅ A new OTP has been sent to your email.</span>";
                }
                else
                {
                    litErrorMessage.Text = "<span class='text-red-500'>Failed to resend OTP. Please try again later.</span>";
                }
            }
            catch (Exception ex)
            {
                litErrorMessage.Text = "<span class='text-red-500'>An unexpected error occurred. Please try again.</span>";
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
