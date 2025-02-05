<%@ Page Title="Account Updated" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="AccountUpdated.aspx.cs" Inherits="PawfectSupplies.Pages.User.AccountUpdated" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="flex justify-center items-center">
        <div class="bg-white shadow-lg rounded-lg p-8 max-w-md w-full border border-gray-200 text-center">
            <h2 class="text-2xl font-bold text-gray-700">Account Updated</h2>
            <p class="text-gray-600 mt-2">Your account settings have been successfully updated.</p>

            <!-- ✅ Dynamic Feedback Message -->
            <p id="mfaStatusMessage" class="text-teal-600 font-medium mt-4">
                <asp:Literal ID="litMFAStatusMessage" runat="server"></asp:Literal>
            </p>

            <!-- ✅ "Login Again" Button -->
            <asp:Button ID="btnLogin" runat="server" Text="Login Again"
                CssClass="btn btn-success bg-teal-600 hover:bg-teal-700 text-white py-2 px-4 rounded mt-4"
                OnClick="btnLogin_Click" />
        </div>
    </div>
</asp:Content>
