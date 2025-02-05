<%@ Page Title="Verify OTP" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="VerifyOTP.aspx.cs" Inherits="PawfectSupplies.Pages.User.Verify2FA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-4 mt-12">
        <div class="max-w-md mx-auto bg-white shadow-lg rounded-lg p-8">
            <h2 class="text-3xl font-bold text-teal-600 text-center mb-6">Verify Your Login</h2>
            <p class="text-gray-600 text-center mt-2">We sent a 6-digit OTP to your email. Enter it below.</p>

            <!-- Feedback Message -->
            <span class="text-red-500 text-sm block mt-2">
                <asp:Literal ID="litErrorMessage" runat="server"></asp:Literal>
            </span>

            <!-- Panel to Ensure 'Enter' Key Triggers Submit OTP Button -->
            <asp:Panel runat="server" DefaultButton="btnVerifyOTP">
                <!-- OTP Input -->
                <div class="mt-4">
                    <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control w-full p-3 border border-gray-300 rounded-lg text-center focus:ring focus:ring-teal-300" placeholder="Enter OTP"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvOTP" runat="server" ControlToValidate="txtOTP"
                        ErrorMessage="OTP is required." CssClass="text-red-500 text-sm block mt-1" Display="Dynamic"
                        ValidationGroup="SubmitOTP"></asp:RequiredFieldValidator>
                </div>

                <!-- Submit OTP Button -->
                <div class="mt-4">
                    <asp:Button ID="btnVerifyOTP" runat="server" Text="Submit OTP" CssClass="w-full bg-teal-600 text-white py-3 rounded-lg hover:bg-teal-700 transition-all duration-300 shadow-md hover:shadow-lg"
                        OnClick="btnVerifyOTP_Click" ValidationGroup="SubmitOTP" />
                </div>
            </asp:Panel>

            <!-- Resend OTP -->
            <div class="mt-3 text-center">
                <asp:Button ID="btnResendOTP" runat="server" Text="Resend OTP" CssClass="btn btn-primary bg-gray-500 hover:bg-gray-600 text-white py-2 px-4 rounded mt-2"
                    OnClick="btnResendOTP_Click" ValidationGroup="ResendOTP" />
                <asp:Label ID="lblResendError" runat="server" CssClass="text-red-500 text-sm block mt-1 hidden"></asp:Label>
            </div>
        </div>
    </div>

    <!-- JavaScript for Resend OTP Lockout -->
    <script>
        let resendLockout = 30;
        let resendAllowed = false;
        let resendBtn = document.getElementById("<%= btnResendOTP.ClientID %>");
        let errorLabel = document.getElementById("<%= lblResendError.ClientID %>");

        function startResendLockout() {
            resendAllowed = false;
            errorLabel.classList.add("hidden");

            let interval = setInterval(() => {
                resendLockout--;
                resendBtn.innerText = `Resend OTP (${resendLockout}s)`;

                if (resendLockout <= 0) {
                    clearInterval(interval);
                    resendBtn.innerText = "Resend OTP";
                    resendAllowed = true;
                }
            }, 1000);
        }

        resendBtn.addEventListener("click", function (e) {
            if (!resendAllowed) {
                e.preventDefault();
                errorLabel.innerText = `Please wait ${resendLockout}s before resending.`;
                errorLabel.classList.remove("hidden");
            }
        });

        startResendLockout();
    </script>
</asp:Content>
