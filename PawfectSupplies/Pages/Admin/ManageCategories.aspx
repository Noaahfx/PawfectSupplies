<%@ Page Title="Manage Categories" Language="C#" 
    MasterPageFile="~/MasterPages/AdminMaster.master" 
    AutoEventWireup="true" 
    CodeBehind="ManageCategories.aspx.cs" 
    Inherits="PawfectSupplies.Pages.Admin.ManageCategories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-6 py-8">
        <h1 class="text-3xl font-bold text-white text-center bg-teal-600 py-4 rounded-md shadow-md">Manage Categories</h1>

        <!-- 🔎 Search Bar -->
        <div class="flex justify-center mt-6 mb-6">
            <asp:Panel runat="server" DefaultButton="btnSearchCategory">
                <div class="flex items-center bg-white rounded-md shadow-lg overflow-hidden border border-gray-300 transition-transform transform hover:scale-105" style="max-width: 400px;">
                    <asp:TextBox ID="txtSearchCategory" runat="server" CssClass="form-control border-none focus:ring-0 px-4 py-2 text-gray-800 w-full" placeholder="Search categories..." AutoComplete="off" />
                    <asp:Button ID="btnSearchCategory" runat="server" Text="Search" CssClass="bg-teal-700 hover:bg-teal-800 text-white px-4 py-2 rounded-md transition" OnClick="btnSearchCategory_Click" />
                </div>
            </asp:Panel>
        </div>

        <!-- 📂 Categories Table -->
        <div class="bg-white shadow-xl rounded-lg p-6 overflow-x-auto transition-transform transform hover:scale-105">
            <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False" CssClass="table table-striped w-full text-left border border-gray-300"
                HeaderStyle-CssClass="bg-teal-600 text-white text-center" GridLines="None" OnRowCommand="gvCategories_RowCommand">
                <Columns>
                    <asp:BoundField DataField="CategoryID" HeaderText="Category ID" ReadOnly="True" ItemStyle-CssClass="text-center px-4 py-3" />
                    <asp:BoundField DataField="Name" HeaderText="Category Name" ItemStyle-CssClass="text-center px-4 py-3 font-semibold" />

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <button type="button" class="btn btn-warning text-white px-3 py-1 rounded-md shadow-md transition hover:scale-110"
                                onclick='<%# "openEditModal(\"" + Eval("CategoryID") + "\", \"" + Eval("Name") + "\")" %>'>
                                Edit
                            </button>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger text-white px-3 py-1 rounded-md ml-2 shadow-md transition hover:scale-110"
                                CommandName="DeleteCategory" CommandArgument='<%# Eval("CategoryID") %>' 
                                OnClientClick="return confirm('Are you sure you want to delete this category?');" />
                        </ItemTemplate>
                        <ItemStyle CssClass="text-center px-4 py-3" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- ➕ Add Category Button -->
        <div class="mt-6 flex justify-center gap-4">
            <button type="button" class="btn btn-success bg-teal-600 text-white px-6 py-2 rounded-md shadow-md transition hover:bg-teal-700 hover:scale-105"
                onclick="openAddModal()">+ Add Category</button>
            <a href="AdminDashboard.aspx" class="btn btn-primary bg-gray-700 text-white px-6 py-2 rounded-md shadow-md transition hover:bg-gray-800 hover:scale-105">
                ← Back to Dashboard
            </a>
        </div>

        <!-- ✏️ Edit Category Modal -->
        <div id="editCategoryModal" class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 hidden">
            <div class="bg-white rounded-lg shadow-2xl p-6 w-full max-w-md transition-transform transform hover:scale-105">
                <h2 class="text-2xl font-bold text-teal-600 mb-4 text-center">Edit Category</h2>

                <asp:HiddenField ID="hfCategoryID" runat="server" />

                <div>
                    <label class="block text-gray-700 font-semibold mb-1">Category Name</label>
                    <asp:TextBox ID="txtEditCategoryName" runat="server" CssClass="form-control w-full border px-3 py-2 rounded-md" />
                </div>

                <div class="flex justify-end gap-4 mt-6">
                    <button type="button" class="btn btn-danger bg-red-500 text-white px-6 py-2 rounded-md shadow-md transition hover:bg-red-600 hover:scale-105" onclick="closeEditModal()">Cancel</button>
                    <asp:Button ID="btnSaveCategory" runat="server" Text="Save Changes" CssClass="btn btn-success bg-teal-600 text-white px-6 py-2 rounded-md shadow-md transition hover:bg-teal-700 hover:scale-105" OnClick="btnSaveCategory_Click" />
                </div>
            </div>
        </div>

        <!-- ➕ Add Category Modal -->
        <div id="addCategoryModal" class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 hidden">
            <div class="bg-white rounded-lg shadow-2xl p-6 w-full max-w-md transition-transform transform hover:scale-105">
                <h2 class="text-2xl font-bold text-teal-600 mb-4 text-center">Add New Category</h2>

                <div>
                    <label class="block text-gray-700 font-semibold mb-1">Category Name</label>
                    <asp:TextBox ID="txtNewCategoryName" runat="server" CssClass="form-control w-full border px-3 py-2 rounded-md" />
                </div>

                <div class="flex justify-end gap-4 mt-6">
                    <button type="button" class="btn btn-danger bg-red-500 text-white px-6 py-2 rounded-md shadow-md transition hover:bg-red-600 hover:scale-105" onclick="closeAddModal()">Cancel</button>
                    <asp:Button ID="btnAddCategory" runat="server" Text="Add Category" CssClass="btn btn-success bg-teal-600 text-white px-6 py-2 rounded-md shadow-md transition hover:bg-teal-700 hover:scale-105" OnClick="btnAddCategory_Click" />
                </div>
            </div>
        </div>

        <script>
            function openEditModal(categoryId, name) {
                document.getElementById("<%= hfCategoryID.ClientID %>").value = categoryId;
                document.getElementById("<%= txtEditCategoryName.ClientID %>").value = name;
                document.getElementById("editCategoryModal").classList.remove("hidden");
                document.getElementById("editCategoryModal").classList.add("flex");
            }

            function closeEditModal() {
                document.getElementById("editCategoryModal").classList.add("hidden");
                document.getElementById("editCategoryModal").classList.remove("flex");
            }

            function openAddModal() {
                document.getElementById("addCategoryModal").classList.remove("hidden");
                document.getElementById("addCategoryModal").classList.add("flex");
            }

            function closeAddModal() {
                document.getElementById("addCategoryModal").classList.add("hidden");
                document.getElementById("addCategoryModal").classList.remove("flex");
            }
        </script>
    </div>
</asp:Content>
