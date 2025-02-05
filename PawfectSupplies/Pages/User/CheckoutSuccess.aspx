<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/UserMaster.master" CodeBehind="CheckoutSuccess.aspx.cs" Inherits="PawfectSupplies.Pages.User.CheckoutSuccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-6 py-10 text-center">
        <h1 class="text-3xl font-bold text-teal-600">🎉 Payment Successful!</h1>
        <p class="text-gray-600 mt-2">Your order has been placed successfully.</p>

        <a href="OrderHistory.aspx" class="mt-6 inline-block bg-teal-600 hover:bg-teal-700 text-white px-6 py-2 rounded-md transition-all">
            View Order History
        </a>
    </div>
</asp:Content>
