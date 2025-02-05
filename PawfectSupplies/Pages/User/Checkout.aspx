<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/UserMaster.master" CodeBehind="Checkout.aspx.cs" Inherits="PawfectSupplies.Pages.User.Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-6 py-10">
        <h1 class="text-3xl font-bold text-teal-600 text-center mb-6">Checkout</h1>

        <div class="bg-white shadow-lg rounded-lg p-6 max-w-lg mx-auto border">
            <h2 class="text-xl font-semibold text-gray-700 mb-4">Order Summary</h2>

            <asp:Repeater ID="rptCartItems" runat="server">
                <ItemTemplate>
                    <div class="flex justify-between py-2 border-b">
                        <span class="font-medium"><%# Eval("ProductName") %></span>
                        <span class="text-teal-600 font-semibold">$<%# Eval("TotalPrice") %></span>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <div class="flex justify-between font-bold text-lg mt-4">
                <span>Total:</span>
                <asp:Label ID="lblTotalAmount" runat="server" CssClass="text-teal-700"></asp:Label>
            </div>

            <!-- Stripe Checkout Button -->
            <form id="stripeForm" method="post" action="CreateCheckoutSession.aspx">
                <input type="hidden" name="totalAmount" id="totalAmount" />
                <button type="submit" class="w-full text-white bg-teal-600 hover:bg-teal-700 px-4 py-2 rounded-lg mt-4">
                    Proceed to Payment
                </button>
            </form>
        </div>
    </div>
</asp:Content>
