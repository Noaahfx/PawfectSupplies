using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PawfectSupplies.Helpers;

namespace PawfectSupplies.Pages.User
{
    public partial class Signup : System.Web.UI.Page
    {
        protected void btnSignup_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            lblSuccessMessage.Text = "";

            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string hCaptchaResponse = Request.Form["h-captcha-response"];

            // 🔹 Validate Inputs
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                lblErrorMessage.Text = "All fields are required.";
                return;
            }

            if (!IsValidEmail(email))
            {
                lblErrorMessage.Text = "Invalid email format.";
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

            // ✅ Validate hCaptcha Before Proceeding
            if (!ValidateHCaptcha(hCaptchaResponse))
            {
                lblErrorMessage.Text = "hCaptcha verification failed. Please try again.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // 🔹 Check if Username or Email already exists
                bool isUsernameTaken = false;
                bool isEmailTaken = false;

                using (SqlCommand checkCmd = new SqlCommand("SELECT Username, Email FROM Users WHERE Username = @Username OR Email = @Email", con))
                {
                    checkCmd.Parameters.AddWithValue("@Username", username);
                    checkCmd.Parameters.AddWithValue("@Email", email);
                    using (SqlDataReader reader = checkCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Username"].ToString().ToLower() == username.ToLower())
                            {
                                isUsernameTaken = true;
                            }
                            if (reader["Email"].ToString().ToLower() == email.ToLower())
                            {
                                isEmailTaken = true;
                            }
                        }
                    }
                }

                // 🔹 Display specific error messages
                if (isUsernameTaken && isEmailTaken)
                {
                    lblErrorMessage.Text = "Both Username and Email are already taken.";
                    return;
                }
                else if (isUsernameTaken)
                {
                    lblErrorMessage.Text = "This Username is already taken.";
                    return;
                }
                else if (isEmailTaken)
                {
                    lblErrorMessage.Text = "This Email is already in use.";
                    return;
                }

                // 🔹 Generate Verification Token
                string token = Guid.NewGuid().ToString();

                // 🔹 Insert New User (Added CreatedAt column)
                string query = @"
                    INSERT INTO Users (Username, Email, PasswordHash, Role, IsVerified, VerificationToken, CreatedAt) 
                    VALUES (@Username, @Email, @PasswordHash, @Role, 0, @Token, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", BCrypt.Net.BCrypt.HashPassword(password));
                    cmd.Parameters.AddWithValue("@Role", "User");
                    cmd.Parameters.AddWithValue("@Token", token);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Send Verification Email
                        EmailHelper.SendVerificationEmail(email, token);
                        lblSuccessMessage.Text = "Account created successfully! Please verify your email to activate your account and log in.";
                        ClearFields();
                    }
                    else
                    {
                        lblErrorMessage.Text = "Something went wrong. Please try again.";
                    }
                }
            }
        }

        private bool ValidateHCaptcha(string hCaptchaResponse)
        {
            const string secretKey = "ES_8e3863a446304e9293e0d25e1fd453b6";
            string apiUrl = $"https://hcaptcha.com/siteverify?secret={secretKey}&response={hCaptchaResponse}";

            using (WebClient client = new WebClient())
            {
                string jsonResponse = client.DownloadString(apiUrl);
                dynamic jsonData = JsonConvert.DeserializeObject(jsonResponse);
                return jsonData.success == true;
            }
        }

        // 🔹 Validate Email Format
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        // 🔹 Validate Password Strength
        private bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
        }

        // 🔹 Clear Input Fields on Success
        private void ClearFields()
        {
            txtUsername.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
        }
    }
}
