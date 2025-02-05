<%@ Page Title="Complete Signup" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="CompleteSignup.aspx.cs" Inherits="PawfectSupplies.Pages.User.CompleteSignup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-4 mt-12">
        <div class="max-w-lg mx-auto bg-white shadow-lg rounded-lg p-8">
            <h2 class="text-3xl font-bold text-teal-600 text-center mb-6">Complete Your Signup</h2>
            <p class="text-gray-500 text-center mb-6">Almost there! Just set up your username and password.</p>

            <!-- Error & Success Messages -->
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="text-red-500 text-sm text-center block mb-4" />
            <asp:Label ID="lblSuccessMessage" runat="server" CssClass="text-green-500 text-sm text-center block mb-4" />

            <!-- Wrap form inside an ASP Panel to enable Enter key trigger -->
            <asp:Panel ID="pnlSignupForm" runat="server" DefaultButton="btnSignup">
                
                <!-- Email Field (Read-only) -->
                <div class="mb-4">
                    <label for="txtEmail" class="block text-gray-700 font-semibold mb-2">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control px-4 py-2 border rounded-lg w-full bg-gray-100" ReadOnly="true" />
                </div>

                <!-- Username Field -->
                <div class="mb-4">
                    <label for="txtUsername" class="block text-gray-700 font-semibold mb-2">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control px-4 py-2 border rounded-lg w-full focus:outline-none focus:ring-2 focus:ring-teal-500" />
                </div>

                <!-- Password Field -->
                <div class="mb-4">
                    <label for="txtPassword" class="block text-gray-700 font-semibold mb-2">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control px-4 py-2 border rounded-lg w-full focus:outline-none focus:ring-2 focus:ring-teal-500" />
                </div>

                <!-- Confirm Password Field -->
                <div class="mb-4">
                    <label for="txtConfirmPassword" class="block text-gray-700 font-semibold mb-2">Confirm Password</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control px-4 py-2 border rounded-lg w-full focus:outline-none focus:ring-2 focus:ring-teal-500" />
                </div>

                <asp:Button ID="btnSignup" runat="server" Text="Complete Signup" OnClick="btnSignup_Click"
                    CssClass="w-full bg-teal-600 text-white py-3 rounded-lg hover:bg-teal-700 transition-all duration-300 shadow-md hover:shadow-lg" />
            
            </asp:Panel>
        </div>
    </div>
</asp:Content>
