using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using PawfectSupplies.DataAccess;
using Stripe.Checkout;
using Stripe;

namespace PawfectSupplies.Pages.User
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        private CartDAL cartDAL = new CartDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Pages/User/Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadCart();
            }
        }


        private void LoadCart()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            DataTable cartItems = cartDAL.GetCartItems(userId);

            if (cartItems.Rows.Count > 0)
            {
                gvCart.DataSource = cartItems;
                gvCart.DataBind();

                // ✅ Fix: Calculate total dynamically instead of using "TotalPrice" column
                decimal grandTotal = 0;
                foreach (DataRow row in cartItems.Rows)
                {
                    decimal price = Convert.ToDecimal(row["Price"]);
                    int quantity = Convert.ToInt32(row["Quantity"]);
                    grandTotal += (price * quantity);
                }

                lblGrandTotal.Text = grandTotal.ToString("C");

                pnlCart.Visible = true;
                pnlEmptyCart.Visible = false;
            }
            else
            {
                pnlCart.Visible = false;
                pnlEmptyCart.Visible = true;
            }
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

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            string sessionUrl = CreateStripeCheckoutSession(userId);
            if (!string.IsNullOrEmpty(sessionUrl))
            {
                Response.Redirect(sessionUrl);
            }
            else
            {
                lblGrandTotal.Text = "<span class='text-red-500'>❌ Failed to create checkout session. Please try again.</span>";
            }
        }

        private string CreateStripeCheckoutSession(int userId)
        {
            StripeConfiguration.ApiKey = "sk_test_51QorNq07tJQwR1fO48dTYp6DYg3RbxnWwzMVJynXtGw4gUjwUzdO5zlAiGKkqG7Hmppn8jElMDZK0PddPhXbcVhy00rUQFvqms";

            decimal totalAmount = GetTotalAmount(userId);
            string successUrl = "https://localhost:44351/Pages/User/CheckoutSuccess.aspx";
            string cancelUrl = "https://localhost:44351/Pages/User/ShoppingCart.aspx";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)(totalAmount * 100), // Convert to cents
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Pawfect Supplies Order"
                    },
                },
                Quantity = 1,
            },
        },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session.Url;  // ✅ Return the Stripe Checkout URL instead of session ID
        }


        private decimal GetTotalAmount(int userId)
        {
            decimal total = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // 🛠 FIX: Calculate total dynamically (Quantity * Price)
                string query = @"
                            SELECT SUM(c.Quantity * p.Price)
                            FROM Cart c
                            INNER JOIN Products p ON c.ProductID = p.ProductID
                            WHERE c.UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        total = Convert.ToDecimal(result);
                    }
                }
            }
            return total;
        }

    }
}
