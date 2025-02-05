using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace PawfectSupplies.Pages.Admin
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Pages/User/Login.aspx");
            }
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        protected void btnSearchUser_Click(object sender, EventArgs e)
        {
            LoadUsers(txtSearchUser.Text.Trim());
        }

        private void LoadUsers(string searchQuery = "")
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT UserID, Username, Email, Role, PhoneNumber, FirstName, LastName, MobileCountryCode
                                 FROM Users
                                 WHERE (Username LIKE '%' + @Search + '%' OR Email LIKE '%' + @Search + '%')
                                 ORDER BY UserID DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Search", searchQuery);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    gvUsers.DataSource = dt;
                    gvUsers.DataBind();
                }
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteUser")
            {
                int userID = Convert.ToInt32(e.CommandArgument);
                DeleteUser(userID);
                LoadUsers();
            }
        }

        private void DeleteUser(int userID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string checkRoleQuery = "SELECT Role FROM Users WHERE UserID = @UserID";
                using (SqlCommand checkCmd = new SqlCommand(checkRoleQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@UserID", userID);
                    con.Open();
                    string role = checkCmd.ExecuteScalar()?.ToString();
                    con.Close();

                    if (role == "Admin") return;
                }

                string query = "DELETE FROM Users WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void btnSaveUser_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(hfUserID.Value);
            string email = txtEditEmail.Text.Trim();
            string role = ddlEditRole.SelectedValue;
            string phoneNumber = txtEditPhoneNumber.Text.Trim();
            string firstName = txtEditFirstName.Text.Trim();
            string lastName = txtEditLastName.Text.Trim();
            string mobileCode = ddlCountryCode.SelectedValue;

            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Users 
                        SET Email=@Email, Role=@Role, PhoneNumber=@PhoneNumber, 
                            FirstName=@FirstName, LastName=@LastName, 
                            MobileCountryCode=@MobileCountryCode 
                        WHERE UserID=@UserID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@MobileCountryCode", mobileCode);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            LoadUsers();
        }
    }
}
