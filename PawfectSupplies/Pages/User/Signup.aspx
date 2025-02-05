<%@ Page Title="Sign Up" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="PawfectSupplies.Pages.User.Signup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-4 mt-12">
        <div class="max-w-lg mx-auto bg-white shadow-lg rounded-lg p-8">
            <h2 class="text-3xl font-bold text-teal-600 text-center mb-6">Create an Account</h2>
            <p class="text-gray-500 text-center mb-6">Join Pawfect Supplies and enjoy exclusive benefits!</p>
            
            <!-- Error & Success Messages -->
            <asp:Label ID="lblSuccessMessage" runat="server" CssClass="text-green-500 text-sm text-center block mb-4" />
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="text-red-500 text-sm text-center block mb-4" />

            <!-- Form with Default Button Set to Signup -->
            <asp:Panel runat="server" DefaultButton="btnSignup">
                <div class="mb-4">
                    <label for="username" class="block text-gray-700 font-semibold mb-2">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control px-4 py-2 border rounded-lg w-full focus:outline-none focus:ring-2 focus:ring-teal-500" />
                </div>
                <div class="mb-4">
                    <label for="email" class="block text-gray-700 font-semibold mb-2">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control px-4 py-2 border rounded-lg w-full focus:outline-none focus:ring-2 focus:ring-teal-500" />
                </div>
                <div class="mb-4">
                    <label for="password" class="block text-gray-700 font-semibold mb-2">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control px-4 py-2 border rounded-lg w-full focus:outline-none focus:ring-2 focus:ring-teal-500" />
                </div>
                <div class="mb-4">
                    <label for="confirmPassword" class="block text-gray-700 font-semibold mb-2">Confirm Password</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control px-4 py-2 border rounded-lg w-full focus:outline-none focus:ring-2 focus:ring-teal-500" />
                </div>

                <!-- 🔹 Centered hCaptcha -->
                <div class="mb-4 flex justify-center">
                    <div class="h-captcha" data-sitekey="71507e60-1e46-401c-a152-0fc0d2a28cbc"></div>
                </div>

                <asp:Button ID="btnSignup" runat="server" Text="Sign Up" OnClick="btnSignup_Click"
                    CssClass="w-full bg-teal-600 text-white py-3 rounded-lg hover:bg-teal-700 transition-all duration-300 shadow-md hover:shadow-lg" />
            </asp:Panel>

            <div class="text-center mt-4">
                <p class="text-gray-500">Already have an account? <a href="Login.aspx" class="text-teal-600 font-bold hover:underline">Login</a></p>
            </div>
        </div>
    </div>

    <!-- hCaptcha Script -->
    <script src="https://js.hcaptcha.com/1/api.js" async defer></script>

</asp:Content>

