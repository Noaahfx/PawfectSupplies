using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using PawfectSupplies.DataAccess;

namespace PawfectSupplies.MasterPages
{
    public partial class UserMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] != null)
            {
                phUserDropdown.Visible = true;
                phAuthButtons.Visible = false;
                phCart.Visible = true;

                // Safely set the username
                litUsername.Text = Session["Username"]?.ToString() ?? "User";

                // Update the cart count
                UpdateCartCount();
            }
            else
            {
                phUserDropdown.Visible = false;
                phAuthButtons.Visible = true;
                phCart.Visible = false;
            }
        }

        public void UpdateCartCount()
        {
            try
            {
                if (Session["UserID"] != null)
                {
                    int userId = Convert.ToInt32(Session["UserID"]);
                    DataTable cartItems = new CartDAL().GetCartItems(userId);

                    int itemCount = 0;
                    foreach (DataRow row in cartItems.Rows)
                    {
                        itemCount += Convert.ToInt32(row["Quantity"]);
                    }

                    lblCartCount.Text = itemCount.ToString();
                }
                else
                {
                    lblCartCount.Text = "0";
                }
            }
            catch
            {
                lblCartCount.Text = "0"; // Fallback in case of any error
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();

            // Validation
            if (string.IsNullOrEmpty(searchQuery))
            {
                searchErrorMessage.InnerText = "Please enter a product name to search.";
                searchErrorMessage.Visible = true;
                return;
            }

            if (searchQuery.Contains("'") || searchQuery.Contains("--"))
            {
                searchErrorMessage.InnerText = "Invalid search query. Please try again.";
                searchErrorMessage.Visible = true;
                return;
            }

            // Clear error message if valid
            searchErrorMessage.Visible = false;

            // Redirect with the correct parameter key
            Response.Redirect($"SearchResults.aspx?query={Server.UrlEncode(searchQuery)}");
        }
    }
}
