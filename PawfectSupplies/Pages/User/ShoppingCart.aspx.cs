using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using PawfectSupplies.DataAccess;

namespace PawfectSupplies.Pages.User
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        private CartDAL cartDAL = new CartDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCart();
            }
        }

        private void LoadCart()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            DataTable cartItems = cartDAL.GetCartItems(userId);

            gvCart.DataSource = cartItems;
            gvCart.DataBind();

            decimal grandTotal = 0;
            foreach (DataRow row in cartItems.Rows)
            {
                grandTotal += Convert.ToDecimal(row["TotalPrice"]);
            }

            lblGrandTotal.Text = grandTotal.ToString("C");
        }

        protected void gvCart_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int cartId = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "IncreaseQuantity":
                    UpdateCartQuantity(cartId, 1); // Increase quantity by 1
                    break;

                case "DecreaseQuantity":
                    UpdateCartQuantity(cartId, -1); // Decrease quantity by 1
                    break;

                case "Remove":
                    cartDAL.RemoveFromCart(cartId); // Remove item from the cart
                    break;
            }

            // Reload the cart and update the cart count
            LoadCart();

            // Update cart count in the master page
            var masterPage = this.Master as PawfectSupplies.MasterPages.UserMaster;
            if (masterPage != null)
            {
                masterPage.UpdateCartCount();
            }
        }

        private void UpdateCartQuantity(int cartId, int change)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "IF EXISTS (SELECT 1 FROM Cart WHERE CartID = @CartID) " +
                    "BEGIN " +
                    "   IF ((SELECT Quantity FROM Cart WHERE CartID = @CartID) + @Change > 0) " +
                    "       UPDATE Cart SET Quantity = Quantity + @Change WHERE CartID = @CartID " +
                    "   ELSE " +
                    "       DELETE FROM Cart WHERE CartID = @CartID " +
                    "END", con);

                cmd.Parameters.AddWithValue("@Change", change);
                cmd.Parameters.AddWithValue("@CartID", cartId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
