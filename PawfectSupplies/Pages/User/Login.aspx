<%@ Page Title="Login" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PawfectSupplies.Pages.User.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-4 mt-12">
        <div class="max-w-md mx-auto bg-white shadow-lg rounded-lg p-8">
            <h2 class="text-3xl font-bold text-teal-600 text-center mb-6">Welcome Back!</h2>
            <p class="text-gray-500 text-center mb-6">Login to access your account and explore our products.</p>

            <!-- Error Message -->
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="text-red-500 text-sm text-center block mb-4"></asp:Label>

            <!-- Panel to Ensure 'Enter' Key Targets Login Button -->
            <asp:Panel runat="server" DefaultButton="btnLogin">
                <div class="mb-4">
                    <label for="username" class="block text-gray-700 font-semibold mb-2">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control px-4 py-2 border rounded-lg w-full focus:outline-none focus:ring-2 focus:ring-teal-500"></asp:TextBox>
                </div>
                <div class="mb-4">
                    <label for="password" class="block text-gray-700 font-semibold mb-2">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control px-4 py-2 border rounded-lg w-full focus:outline-none focus:ring-2 focus:ring-teal-500"></asp:TextBox>
                </div>
                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click"
                    CssClass="w-full bg-teal-600 text-white py-3 rounded-lg hover:bg-teal-700 transition-all duration-300 shadow-md hover:shadow-lg" />
            </asp:Panel>

            <div class="text-center mt-4">
                <p class="text-gray-500">Don't have an account? <a href="Signup.aspx" class="text-teal-600 font-bold hover:underline">Sign Up</a></p>
                <p class="text-gray-500">Forgot your password? <a href="ForgotPassword.aspx" class="text-teal-600 font-bold hover:underline">Reset it here</a></p>
                <p class="text-gray-500">Have a Google account? <a href="GoogleLogin.aspx" class="text-teal-600 font-bold hover:underline">Sign in with Google</a></p>
            </div>
        </div>
    </div>
</asp:Content>
