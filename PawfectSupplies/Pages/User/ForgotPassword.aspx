<%@ Page Title="Forgot Password" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="PawfectSupplies.Pages.User.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-4 mt-12">
        <div class="max-w-md mx-auto bg-white shadow-lg rounded-lg p-8">
            <h2 class="text-3xl font-bold text-teal-600 text-center mb-6">Forgot Password</h2>
            <p class="text-gray-500 text-center mb-6">Enter your username to reset your password.</p>

            <!-- Feedback Message -->
            <asp:Label ID="lblMessage" runat="server" CssClass="text-sm text-center block mb-4"></asp:Label>

            <asp:Panel runat="server" DefaultButton="btnSubmit">
                <div class="mb-4">
                    <label for="username" class="block text-gray-700 font-semibold mb-2">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control px-4 py-2 border rounded-lg w-full focus:outline-none focus:ring-2 focus:ring-teal-500"></asp:TextBox>
                </div>

                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                    CssClass="w-full bg-teal-600 text-white py-3 rounded-lg hover:bg-teal-700 transition-all duration-300 shadow-md hover:shadow-lg" />
            </asp:Panel>

            <div class="text-center mt-4">
                <p class="text-gray-500"><a href="Login.aspx" class="text-teal-600 font-bold hover:underline">Back to Login</a></p>
            </div>
        </div>
    </div>
</asp:Content>
