<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerifyAccount.aspx.cs" Inherits="PawfectSupplies.Pages.User.VerifyAccount" MasterPageFile="~/MasterPages/UserMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="px-4 mt-12 bg-white p-8 rounded-lg shadow-md max-w-md w-full mx-auto text-center mt-10 bg-white shadow">
        <h1 class="text-2xl font-bold text-gray-700">Account Verification</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="text-gray-500 mt-2 block"></asp:Label>
        <br />
        <asp:Button ID="btnLogin" runat="server" Text="Go to Login" CssClass="w-full bg-teal-600 text-white py-3 rounded-lg hover:bg-teal-700 transition-all duration-300 shadow-md hover:shadow-lg" PostBackUrl="Login.aspx" />
    </div>
</asp:Content>
