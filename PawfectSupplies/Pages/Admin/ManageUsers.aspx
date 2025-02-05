<%@ Page Title="Manage Users" Language="C#" 
    MasterPageFile="~/MasterPages/AdminMaster.master" 
    AutoEventWireup="true" 
    CodeBehind="ManageUsers.aspx.cs" 
    Inherits="PawfectSupplies.Pages.Admin.ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-6 py-8">
        <h1 class="text-3xl font-bold text-white text-center bg-teal-600 py-4 rounded-md shadow-md">Manage Users</h1>

        <!-- Feedback Message -->
        <div id="feedbackMessage" class="hidden text-center py-2 text-white font-semibold rounded-md mt-4"></div>

        <!-- Search Bar -->
        <div class="flex justify-center mt-6 mb-6">
            <asp:Panel runat="server" DefaultButton="btnSearchUser">
                <div class="flex items-center bg-white rounded-md shadow-lg overflow-hidden border border-gray-300 transition-transform transform hover:scale-105" style="max-width: 400px;">
                    <asp:TextBox ID="txtSearchUser" runat="server" CssClass="form-control border-none focus:ring-0 px-4 py-2 text-gray-800 w-full" placeholder="Search users..." AutoComplete="off" />
                    <asp:Button ID="btnSearchUser" runat="server" Text="Search" CssClass="bg-teal-700 hover:bg-teal-800 text-white px-4 py-2 rounded-md transition" OnClick="btnSearchUser_Click" />
                </div>
            </asp:Panel>
        </div>

        <!-- Users Table -->
        <div class="bg-white shadow-xl rounded-lg p-6 overflow-x-auto transition-transform transform hover:scale-105">
            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" CssClass="table table-striped w-full text-left border border-gray-300"
                HeaderStyle-CssClass="bg-teal-600 text-white" GridLines="None" OnRowCommand="gvUsers_RowCommand">
                <Columns>
                    <asp:BoundField DataField="UserID" HeaderText="User ID" ReadOnly="True" />
                    <asp:BoundField DataField="Username" HeaderText="Username" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Role" HeaderText="Role" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:PlaceHolder ID="phActions" runat="server" Visible='<%# Eval("Role").ToString() != "Admin" %>'>
                                <button type="button" class="btn btn-warning text-white px-3 py-1 rounded-md shadow-md transition hover:scale-110"
                                    onclick='<%# "openEditModal(\"" + Eval("UserID") + "\", \"" + Eval("Username") + "\", \"" + Eval("Email") + "\", \"" + Eval("Role") + "\", \"" + Eval("PhoneNumber") + "\", \"" + Eval("FirstName") + "\", \"" + Eval("LastName") + "\", \"" + Eval("MobileCountryCode") + "\")" %>'>
                                    Edit
                                </button>

                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger text-white px-3 py-1 rounded-md ml-2 shadow-md transition hover:scale-110"
                                    CommandName="DeleteUser" CommandArgument='<%# Eval("UserID") %>' 
                                    OnClientClick="return confirm('Are you sure you want to delete this user?');" />
                            </asp:PlaceHolder>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- Back to Dashboard Button -->
        <div class="mt-6 flex justify-center">
            <a href="AdminDashboard.aspx" class="btn btn-primary px-6 py-2 text-white rounded-md shadow-md transition hover:bg-gray-700 hover:scale-105">
                ← Back to Dashboard
            </a>
        </div>
    </div>

    <!-- Edit User Modal -->
    <div id="editUserModal" class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 hidden">
        <div class="bg-white rounded-lg shadow-2xl p-6 w-full max-w-lg transition-transform transform hover:scale-105">
            <h2 class="text-2xl font-bold text-teal-600 mb-4 text-center">Edit User Details</h2>

            <asp:HiddenField ID="hfUserID" runat="server" />

            <!-- Username -->
            <div>
                <label class="block text-gray-700 font-semibold mb-1">Username</label>
                <asp:TextBox ID="txtEditUsername" runat="server" CssClass="form-control w-full border px-3 py-2 rounded-md" ReadOnly="true" />
            </div>

            <!-- First Name & Last Name -->
            <div class="grid grid-cols-2 gap-4">
                <div>
                    <label class="block text-gray-700 font-semibold mb-1">First Name</label>
                    <asp:TextBox ID="txtEditFirstName" runat="server" CssClass="form-control w-full border px-3 py-2 rounded-md" />
                </div>
                <div>
                    <label class="block text-gray-700 font-semibold mb-1">Last Name</label>
                    <asp:TextBox ID="txtEditLastName" runat="server" CssClass="form-control w-full border px-3 py-2 rounded-md" />
                </div>
            </div>

            <!-- Email -->
            <div>
                <label class="block text-gray-700 font-semibold mb-1">Email</label>
                <asp:TextBox ID="txtEditEmail" runat="server" CssClass="form-control w-full border px-3 py-2 rounded-md" />
            </div>

            <!-- Country Code & Phone Number -->
            <div class="flex gap-2">
                <div class="w-1/3">
                    <label class="block text-gray-700 font-semibold mb-1">Country Code</label>
                    <asp:DropDownList ID="ddlCountryCode" runat="server" CssClass="form-control border px-3 py-2 rounded-md w-full">
                        <asp:ListItem Text="Select Country Code" Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="+65 (Singapore)" Value="+65"></asp:ListItem>
                        <asp:ListItem Text="+1 (USA)" Value="+1"></asp:ListItem>
                        <asp:ListItem Text="+44 (UK)" Value="+44"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="w-2/3">
                    <label class="block text-gray-700 font-semibold mb-1">Phone Number</label>
                    <asp:TextBox ID="txtEditPhoneNumber" runat="server" CssClass="form-control border px-3 py-2 rounded-md w-full" />
                </div>
            </div>

            <!-- Role -->
            <div>
                <label class="block text-gray-700 font-semibold mb-1">Role</label>
                <asp:DropDownList ID="ddlEditRole" runat="server" CssClass="form-control w-full border px-3 py-2 rounded-md">
                    <asp:ListItem Text="User" Value="User" />
                    <asp:ListItem Text="Admin" Value="Admin" />
                </asp:DropDownList>
            </div>

            <!-- Action Buttons -->
            <div class="flex justify-end gap-4 mt-6">
                <button type="button" class="btn btn-danger bg-red-500 text-white px-6 py-2 rounded-md shadow-md transition hover:bg-red-600 hover:scale-105" onclick="closeEditModal()">Cancel</button>
                <asp:Button ID="btnSaveUser" runat="server" Text="Save Changes" CssClass="btn btn-success bg-teal-600 text-white px-6 py-2 rounded-md shadow-md transition hover:bg-teal-700 hover:scale-105" OnClick="btnSaveUser_Click" />
            </div>
        </div>
    </div>

    <script>
        function openEditModal(userId, username, email, role, phoneNumber, firstName, lastName, mobileCode) {
            console.log("Opening modal for UserID:", userId);

            document.getElementById("<%= hfUserID.ClientID %>").value = userId;
            document.getElementById("<%= txtEditUsername.ClientID %>").value = username;
            document.getElementById("<%= txtEditEmail.ClientID %>").value = email;
            document.getElementById("<%= ddlEditRole.ClientID %>").value = role;
            document.getElementById("<%= txtEditPhoneNumber.ClientID %>").value = phoneNumber;
            document.getElementById("<%= txtEditFirstName.ClientID %>").value = firstName;
            document.getElementById("<%= txtEditLastName.ClientID %>").value = lastName;

            // Handle Country Code Selection
            let countryDropdown = document.getElementById("<%= ddlCountryCode.ClientID %>");
            if (mobileCode && mobileCode.trim() !== "") {
                for (let i = 0; i < countryDropdown.options.length; i++) {
                    if (countryDropdown.options[i].value === mobileCode) {
                        countryDropdown.selectedIndex = i;
                        break;
                    }
                }
            } else {
                // If no country code exists, keep "Select Country Code" selected
                countryDropdown.selectedIndex = 0;
            }

            // Show the modal
            document.getElementById("editUserModal").classList.remove("hidden");
            document.getElementById("editUserModal").classList.add("flex");
        }

        function closeEditModal() {
            document.getElementById("editUserModal").classList.add("hidden");
            document.getElementById("editUserModal").classList.remove("flex");
            }
    </script>

</asp:Content>
