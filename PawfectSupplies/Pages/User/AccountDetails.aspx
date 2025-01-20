<%@ Page Title="Account Details" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="AccountDetails.aspx.cs" Inherits="PawfectSupplies.Pages.User.AccountDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto mt-8">
        <div class="max-w-xl mx-auto bg-white p-6 shadow-lg rounded-lg">
            <h2 class="text-2xl font-bold text-teal-600 mb-6">Account Details</h2>
            <p class="text-gray-700">
                <strong>Name:</strong> <%= Session["Username"] %>
            </p>
            <p class="text-gray-700">
                <strong>Email:</strong> <%= Session["Email"] %>
            </p>
            <p class="text-gray-700">
                <strong>Password:</strong> ********
                <a href="#" class="text-teal-600 font-bold hover:underline">Change password</a>
            </p>
            <p class="text-gray-700">
                <strong>Mobile Number:</strong> Add mobile number
            </p>
        </div>
    </div>
</asp:Content>
