using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using PawfectSupplies.Helpers; // Importing Email Helper

namespace PawfectSupplies.Pages.User
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = ""; // Reset error message

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // 🔹 Validate Inputs
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblErrorMessage.Text = "Username and Password are required.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT UserID, Username, Email, PasswordHash, Role, IsVerified, Is2FAEnabled, MFA_Type FROM Users WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read()) // User found
                        {
                            string storedPasswordHash = reader["PasswordHash"].ToString();
                            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);

                            if (!isPasswordCorrect)
                            {
                                lblErrorMessage.Text = "Invalid username or password.";
                                return;
                            }

                            bool isVerified = Convert.ToBoolean(reader["IsVerified"]);
                            bool is2FAEnabled = reader["Is2FAEnabled"] != DBNull.Value && Convert.ToBoolean(reader["Is2FAEnabled"]);
                            string mfaType = reader["MFA_Type"] != DBNull.Value ? reader["MFA_Type"].ToString() : "None";

                            // 🔹 Check if the account is verified after password check
                            if (!isVerified)
                            {
                                lblErrorMessage.Text = "Your account is not verified. Please check your email and verify your account before logging in.";
                                return;
                            }

                            // 🔹 Check if Email OTP is required for login
                            if (is2FAEnabled && mfaType == "Email")
                            {
                                string email = reader["Email"].ToString();
                                string otpCode = GenerateOTP(); // Generate a 6-digit OTP

                                // Store OTP & Expiry in DB
                                StoreOTP(reader["UserID"].ToString(), otpCode);

                                // Send OTP Email
                                EmailHelper.SendEmailOTP(email, otpCode);

                                // Store UserID temporarily for OTP verification
                                Session["Pending2FAUserID"] = reader["UserID"].ToString();
                                Session["Pending2FAUsername"] = username;

                                // Redirect to OTP Verification Page
                                Response.Redirect("~/Pages/User/VerifyOTP.aspx");
                            }
                            else
                            {
                                // 🔹 Set Secure Session Variables
                                Session["UserID"] = reader["UserID"].ToString();
                                Session["Username"] = HttpUtility.HtmlEncode(reader["Username"].ToString());
                                Session["Email"] = HttpUtility.HtmlEncode(reader["Email"].ToString());
                                Session["Role"] = reader["Role"].ToString();

                                // 🔹 Redirect Based on Role
                                if (Session["Role"].ToString() == "Admin")
                                {
                                    Response.Redirect("~/Pages/Admin/AdminDashboard.aspx");
                                }
                                else
                                {
                                    Response.Redirect("~/Pages/User/AccountDetails.aspx");
                                }
                            }
                        }
                        else
                        {
                            lblErrorMessage.Text = "Invalid username or password.";
                        }
                    }
                }
            }
            catch (SqlException ex) // Database Error Handling
            {
                lblErrorMessage.Text = "Database error occurred. Please try again later.";
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex) // General Error Handling
            {
                lblErrorMessage.Text = "An unexpected error occurred. Please try again.";
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // 🔹 Generates a 6-digit OTP
        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Returns a 6-digit number
        }

        // 🔹 Stores OTP & Expiry Time in Database
        private void StoreOTP(string userID, string otpCode)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    UPDATE Users 
                    SET Email2FA_Code = @OTP, Email2FA_Expiry = DATEADD(MINUTE, 3, GETDATE()) 
                    WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OTP", otpCode);
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
