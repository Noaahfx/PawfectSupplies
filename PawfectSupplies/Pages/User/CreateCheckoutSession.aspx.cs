using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Stripe;
using Stripe.Checkout;
using System.Web;

namespace PawfectSupplies.Pages.User
{
    public partial class CreateCheckoutSession : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Stripe API Key (Use Secret Key from Stripe)
            StripeConfiguration.ApiKey = "sk_test_your_secret_key";

            int userId = Convert.ToInt32(Session["UserID"]);
            decimal totalAmount = GetTotalAmount(userId);
            string successUrl = "https://yourdomain.com/CheckoutSuccess.aspx";
            string cancelUrl = "https://yourdomain.com/ShoppingCart.aspx";

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
                            UnitAmount = (long)(totalAmount * 100),
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

            Response.Redirect(session.Url);
        }

        private decimal GetTotalAmount(int userId)
        {
            decimal total = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["PawfectSuppliesDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT SUM(TotalPrice) FROM Cart WHERE UserID = @UserID";
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
