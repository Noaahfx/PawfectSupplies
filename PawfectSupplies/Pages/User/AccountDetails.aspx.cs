using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace PawfectSupplies.Pages.User
{
    public partial class AccountDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Pages/User/Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadUserData();
                displaySection.Visible = true;
                editSection.Visible = false;
            }
        }

        private void LoadUserData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;
            int userId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT FirstName, LastName, Email, MobileCountryCode, PhoneNumber FROM Users WHERE UserID = @UserID", con);
                cmd.Parameters.AddWithValue("@UserID", userId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    litName.Text = $"{reader["FirstName"]} {reader["LastName"]}";
                    litEmail.Text = reader["Email"].ToString();
                    litPhone.Text = $"{reader["MobileCountryCode"]} {reader["PhoneNumber"]}";

                    txtFirstName.Text = reader["FirstName"].ToString();
                    txtLastName.Text = reader["LastName"].ToString();
                    txtEmail.Text = reader["Email"].ToString(); // Email is read-only
                    ddlCountryCode.SelectedValue = reader["MobileCountryCode"].ToString();
                    txtPhone.Text = reader["PhoneNumber"].ToString();
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            displaySection.Visible = false;
            editSection.Visible = true;
            validationErrors.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            validationErrors.Visible = false;
            validationErrors.Text = ""; // Clear previous errors

            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string countryCode = ddlCountryCode.SelectedValue;
            string phone = txtPhone.Text.Trim();

            int userId = Convert.ToInt32(Session["UserID"]);
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            // ✅ **Phone number validation (must be exactly 8 digits)**
            if (!string.IsNullOrEmpty(phone) && !Regex.IsMatch(phone, @"^\d{8}$"))
            {
                validationErrors.Visible = true;
                validationErrors.Text = "❌ Phone number must be exactly 8 digits.";
                return;
            }

            // ✅ **Check if phone number already exists**
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand checkCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Users WHERE MobileCountryCode = @MobileCountryCode AND PhoneNumber = @PhoneNumber AND UserID <> @UserID", con);
                checkCmd.Parameters.AddWithValue("@MobileCountryCode", countryCode);
                checkCmd.Parameters.AddWithValue("@PhoneNumber", phone);
                checkCmd.Parameters.AddWithValue("@UserID", userId);

                con.Open();
                int phoneExists = Convert.ToInt32(checkCmd.ExecuteScalar());
                con.Close();

                if (phoneExists > 0)
                {
                    validationErrors.Visible = true;
                    validationErrors.Text = "❌ Phone number is already in use.";
                    return;
                }
            }

            // ✅ **Update User Details**
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
        UPDATE Users 
        SET FirstName = @FirstName, 
            LastName = @LastName, 
            MobileCountryCode = @MobileCountryCode, 
            PhoneNumber = @PhoneNumber 
        WHERE UserID = @UserID", con);

                cmd.Parameters.AddWithValue("@FirstName", string.IsNullOrEmpty(firstName) ? (object)DBNull.Value : firstName);
                cmd.Parameters.AddWithValue("@LastName", string.IsNullOrEmpty(lastName) ? (object)DBNull.Value : lastName);
                cmd.Parameters.AddWithValue("@MobileCountryCode", countryCode);
                cmd.Parameters.AddWithValue("@PhoneNumber", string.IsNullOrEmpty(phone) ? (object)DBNull.Value : phone);
                cmd.Parameters.AddWithValue("@UserID", userId);

                con.Open();
                int rowsUpdated = cmd.ExecuteNonQuery();
                con.Close();

                if (rowsUpdated > 0)
                {
                    LoadUserData();
                    displaySection.Visible = true;
                    editSection.Visible = false;
                    validationErrors.Visible = true;
                    validationErrors.CssClass = "alert alert-success text-green-500 text-sm";
                    validationErrors.Text = "✅ Your account details have been updated successfully!";
                }
                else
                {
                    validationErrors.Visible = true;
                    validationErrors.Text = "⚠ Something went wrong. Please try again later.";
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            displaySection.Visible = true;
            editSection.Visible = false;
            LoadUserData();
        }
    }
}
