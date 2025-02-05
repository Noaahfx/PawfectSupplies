<%@ Page Title="Reset Password" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="PawfectSupplies.Pages.User.ResetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-4 mt-12 max-w-md">
        <div class="bg-white shadow-lg rounded-lg p-8">
            <h2 class="text-3xl font-bold text-teal-600 text-center mb-6">Reset Your Password</h2>
            <p class="text-gray-500 text-center mb-6">Enter your new password below.</p>

            <!-- Feedback Message -->
            <asp:Label ID="lblMessage" runat="server" CssClass="text-red-500 text-sm text-center block mb-4"></asp:Label>

            <!-- Panel to Ensure 'Enter' Key Triggers Reset Button -->
            <asp:Panel runat="server" DefaultButton="btnResetPassword">
                <div class="mb-4">
                    <label for="txtNewPassword" class="block text-gray-700 font-semibold mb-2">New Password</label>
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" 
                        CssClass="form-control px-4 py-2 border rounded-lg w-full focus:outline-none focus:ring-2 focus:ring-teal-500"></asp:TextBox>
                </div>

                <div class="mb-4">
                    <label for="txtConfirmNewPassword" class="block text-gray-700 font-semibold mb-2">Confirm New Password</label>
                    <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="Password"
                        CssClass="form-control px-4 py-2 border rounded-lg w-full focus:outline-none focus:ring-2 focus:ring-teal-500"></asp:TextBox>
                </div>

                <!-- Show Password Checkbox -->
                <div class="mb-4 flex items-center">
                    <input type="checkbox" id="chkShowPassword" class="mr-2">
                    <label for="chkShowPassword" class="text-gray-600">Show Password</label>
                </div>

                <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password"
                    CssClass="w-full bg-teal-600 text-white py-3 rounded-lg hover:bg-teal-700 transition-all duration-300 shadow-md hover:shadow-lg"
                    OnClick="btnResetPassword_Click" />
            </asp:Panel>

        </div>
    </div>

    <!-- JavaScript for Show Password -->
    <script>
        document.getElementById("chkShowPassword").addEventListener("change", function () {
            let passwordFields = ["<%= txtNewPassword.ClientID %>", "<%= txtConfirmNewPassword.ClientID %>"];
            passwordFields.forEach(id => {
                let input = document.getElementById(id);
                if (input) {
                    input.type = this.checked ? "text" : "password";
                }
            });
        });
    </script>

</asp:Content>
