<%@ Page Title="Account Settings" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="AccountSettings.aspx.cs" Inherits="PawfectSupplies.Pages.User.AccountSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-4 mt-10 max-w-2xl">
        <h2 class="text-3xl font-bold text-gray-700 mb-6">Account Settings</h2>

        <!-- Multi-Factor Authentication Section -->
        <asp:Panel ID="pnlMFA" runat="server" DefaultButton="btnSaveMFA">
            <div class="bg-white shadow-lg rounded-lg p-6 mb-6">
                <h3 class="text-xl font-semibold text-gray-800">Multi-Factor Authentication</h3>

                <p id="mfaStatus" class="text-red-500 text-sm mt-2">
                    <asp:Literal ID="litMFAStatus" runat="server"></asp:Literal>
                </p>

                <!-- Move the Error Message Here -->
                <asp:Label ID="lblMFAStatus" runat="server" CssClass="text-red-500 text-sm block mt-1"></asp:Label>

                <label for="ddlMFA" class="block text-gray-700 font-medium mt-4">Select MFA Method</label>
                <asp:DropDownList ID="ddlMFA" runat="server" CssClass="form-select w-full p-2 border rounded-lg">
                    <asp:ListItem Text="None" Value="None" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Authenticator App" Value="Authenticator"></asp:ListItem>
                    <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                </asp:DropDownList>

                <!-- Password Input Before Saving -->
                <label for="txtMfaPassword" class="block text-gray-700 font-medium mt-4">Enter Password</label>
                <asp:TextBox ID="txtMfaPassword" runat="server" TextMode="Password" CssClass="form-control w-full p-2 border rounded-lg"></asp:TextBox>

                <asp:Button ID="btnSaveMFA" runat="server" Text="Save MFA"
                    CssClass="btn btn-primary mt-4"
                    OnClick="btnSaveMFA_Click" 
                    CausesValidation="false"
                    UseSubmitBehavior="false"/>
            </div>
        </asp:Panel>

        <!-- Change Password Section -->
        <asp:Panel ID="pnlChangePassword" runat="server" DefaultButton="btnChangePassword">
            <div class="bg-white shadow-lg rounded-lg p-6">
                <h3 class="text-xl font-semibold text-gray-800">Change Password</h3>

                <!-- Feedback Message -->
                <asp:Label ID="lblPasswordMessage" runat="server" CssClass="text-red-500 text-sm block mt-1"></asp:Label>

                <label class="block text-gray-700 font-medium mt-4">Current Password</label>
                <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password" CssClass="form-control w-full p-2 border rounded-lg" />

                <label class="block text-gray-700 font-medium mt-4">New Password</label>
                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control w-full p-2 border rounded-lg" />
                <p class="text-gray-500 text-sm mt-1">New password must be at least 8 characters long, contain letters, numbers, and special characters.</p>

                <label class="block text-gray-700 font-medium mt-4">Confirm New Password</label>
                <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="Password" CssClass="form-control w-full p-2 border rounded-lg" />

                <!-- Show Password Checkbox -->
                <div class="flex items-center mt-2 space-x-2">
                    <input type="checkbox" id="chkShowPassword" class="w-5 h-5 cursor-pointer" onclick="togglePasswordVisibility()" />
                    <label for="chkShowPassword" class="text-gray-600 cursor-pointer text-base">Show Passwords</label>
                </div>

                <asp:Button ID="btnChangePassword" runat="server" Text="Change Password"
                    CssClass="btn btn-primary mt-4"
                    OnClick="btnChangePassword_Click" />
            </div>
        </asp:Panel>

        <script>
            function togglePasswordVisibility() {
                let fields = [
                    '<%= txtCurrentPassword.ClientID %>',
                    '<%= txtNewPassword.ClientID %>',
                    '<%= txtConfirmNewPassword.ClientID %>'
                ];

                fields.forEach(fieldId => {
                    let input = document.getElementById(fieldId);
                    if (input) {
                        input.type = input.type === "password" ? "text" : "password";
                    }
                });
            }
        </script>
</asp:Content>
