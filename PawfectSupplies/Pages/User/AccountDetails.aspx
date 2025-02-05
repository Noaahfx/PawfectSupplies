<%@ Page Title="Account Details" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="AccountDetails.aspx.cs" Inherits="PawfectSupplies.Pages.User.AccountDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="container mx-auto mt-8">
        <div class="max-w-3xl mx-auto bg-white p-6 shadow-lg rounded-lg">
            <h2 class="text-2xl font-bold text-teal-600 mb-6">Account Details</h2>

            <asp:UpdatePanel ID="updatePanel" runat="server">
                <ContentTemplate>
                    <!-- Alert Box for Error Messages -->
                    <asp:Label ID="validationErrors" runat="server" CssClass="alert alert-danger text-red-500 text-sm mb-4" Visible="false"></asp:Label>

                    <!-- Display Section -->
                    <asp:Panel ID="displaySection" runat="server">
                        <p class="text-gray-700"><strong>Name:</strong> <asp:Literal ID="litName" runat="server"></asp:Literal></p>
                        <p class="text-gray-700"><strong>Email:</strong> <asp:Literal ID="litEmail" runat="server"></asp:Literal></p>
                        <p class="text-gray-700"><strong>Mobile Number:</strong> <asp:Literal ID="litPhone" runat="server"></asp:Literal></p>
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary mt-4" OnClick="btnEdit_Click" />
                    </asp:Panel>

                    <!-- Edit Section -->
                    <asp:Panel ID="editSection" runat="server" Visible="false" DefaultButton="btnSave">
                        <div class="grid grid-cols-2 gap-4">
                            <div>
                                <label for="txtFirstName" class="block text-gray-600 font-medium mb-1">First Name</label>
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div>
                                <label for="txtLastName" class="block text-gray-600 font-medium mb-1">Last Name</label>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-span-2">
                                <label for="txtEmail" class="block text-gray-600 font-medium mb-1">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control bg-gray-200 cursor-not-allowed" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="flex items-center col-span-2 gap-4">
                                <div>
                                    <label for="ddlCountryCode" class="block text-gray-600 font-medium mb-1">Country Code</label>
                                    <asp:DropDownList ID="ddlCountryCode" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="+65" Value="+65"></asp:ListItem>
                                        <asp:ListItem Text="+1" Value="+1"></asp:ListItem>
                                        <asp:ListItem Text="+44" Value="+44"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <label for="txtPhone" class="block text-gray-600 font-medium mb-1">Phone Number</label>
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <!-- Buttons with proper spacing -->
                        <div class="mt-6 flex space-x-6">
                            <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn btn-success px-4 py-2" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger px-4 py-2" OnClick="btnCancel_Click" />
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
