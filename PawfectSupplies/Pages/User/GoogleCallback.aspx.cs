using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using PawfectSupplies.Helpers; // Importing Email Helper

namespace PawfectSupplies.Pages.User
{
    public partial class GoogleCallback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["code"] == null)
            {
                Response.Redirect("Login.aspx?error=no_code");
                return;
            }

            string authorizationCode = Request.QueryString["code"];
            string clientId = "1082695824872-198hcdslmiukr8duji401k10ckkeqgqg.apps.googleusercontent.com"; // Replace with your actual Google OAuth Client ID
            string clientSecret = "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"; // Replace with your actual Google OAuth Client Secret
            string redirectUri = "https://localhost:44351/Pages/User/GoogleCallback.aspx"; 
            using (HttpClient client = new HttpClient())
            {
                var tokenRequest = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("code", authorizationCode),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                });

                HttpResponseMessage tokenResponse = client.PostAsync("https://oauth2.googleapis.com/token", tokenRequest).Result;

                if (!tokenResponse.IsSuccessStatusCode)
                {
                    Response.Redirect("Login.aspx?error=token_exchange_failed");
                    return;
                }

                var tokenContent = tokenResponse.Content.ReadAsStringAsync().Result;
                var tokenJson = JObject.Parse(tokenContent);
                string accessToken = tokenJson["access_token"]?.ToString();

                if (string.IsNullOrEmpty(accessToken))
                {
                    Response.Redirect("Login.aspx?error=no_access_token");
                    return;
                }

                // 🔹 Retrieve user info
                HttpResponseMessage userInfoResponse = client.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo?access_token=" + accessToken).Result;
                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    Response.Redirect("Login.aspx?error=userinfo_failed");
                    return;
                }

                var userInfoContent = userInfoResponse.Content.ReadAsStringAsync().Result;
                var userInfoJson = JObject.Parse(userInfoContent);
                string email = userInfoJson["email"]?.ToString();

                if (string.IsNullOrEmpty(email))
                {
                    Response.Redirect("Login.aspx?error=no_email");
                    return;
                }

                Session["GoogleEmail"] = email;

                // 🔹 Check if user already exists in DB
                if (UserExists(email))
                {
                    AutoLogin(email);
                }
                else
                {
                    Response.Redirect("CompleteSignup.aspx");
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
                string query = "SELECT UserID, Username, Role, Is2FAEnabled, MFA_Type FROM Users WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string userId = reader["UserID"].ToString();
                        string username = reader["Username"].ToString();
                        string role = reader["Role"].ToString();
                        bool is2FAEnabled = reader["Is2FAEnabled"] != DBNull.Value && Convert.ToBoolean(reader["Is2FAEnabled"]);
                        string mfaType = reader["MFA_Type"] != DBNull.Value ? reader["MFA_Type"].ToString() : "None";

                        // ✅ If 2FA is enabled, require OTP verification before login
                        if (is2FAEnabled && mfaType == "Email")
                        {
                            string otpCode = GenerateOTP();
                            StoreOTP(userId, otpCode);
                            EmailHelper.SendEmailOTP(email, otpCode);

                            // 🚨 **Only store `Pending2FAUserID` and do NOT log in yet!**
                            Session["Pending2FAUserID"] = userId;
                            Session["Pending2FAEmail"] = email;

                            Response.Redirect("VerifyOTP.aspx");
                            return;
                        }

                        // ✅ If no 2FA, log in immediately
                        Session["UserID"] = userId;
                        Session["Username"] = username;
                        Session["Email"] = email;
                        Session["Role"] = role;

                        Response.Redirect("AccountDetails.aspx");
                    }
                }
            }
        }

        private void StoreOTP(string userId, string otpCode)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Users SET Email2FA_Code = @Code, Email2FA_Expiry = @Expiry WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@Code", otpCode);
                    cmd.Parameters.AddWithValue("@Expiry", DateTime.Now.AddMinutes(3)); // Expiry time = 3 minutes
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Generates a 6-digit OTP
        }


    }
}
