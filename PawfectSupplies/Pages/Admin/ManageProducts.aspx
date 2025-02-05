<%@ Page Title="Manage Products" Language="C#" 
    MasterPageFile="~/MasterPages/AdminMaster.master" 
    AutoEventWireup="true" 
    CodeBehind="ManageProducts.aspx.cs" 
    Inherits="PawfectSupplies.Pages.Admin.ManageProducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-6 py-8">
        <h1 class="text-3xl font-bold text-white text-center bg-teal-600 py-4 rounded-md shadow-md">Manage Products</h1>

        <!-- 🔎 Search Bar -->
        <div class="flex justify-center mt-6 mb-6">
            <asp:Panel runat="server" DefaultButton="btnSearchProduct">
                <div class="flex items-center bg-white rounded-md shadow-lg overflow-hidden border border-gray-300 transition-transform transform hover:scale-105" style="max-width: 400px;">
                    <asp:TextBox ID="txtSearchProduct" runat="server" CssClass="form-control border-none focus:ring-0 px-4 py-2 text-gray-800 w-full" placeholder="Search products..." AutoComplete="off" />
                    <asp:Button ID="btnSearchProduct" runat="server" Text="Search" CssClass="bg-teal-700 hover:bg-teal-800 text-white px-4 py-2 rounded-md transition" OnClick="btnSearchProduct_Click" />
                </div>
            </asp:Panel>
        </div>

        <!-- 📦 Products Table -->
        <div class="bg-white shadow-xl rounded-lg p-6 overflow-x-auto transition-transform transform hover:scale-105">
            <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" CssClass="table table-striped w-full text-left border border-gray-300"
                HeaderStyle-CssClass="bg-teal-600 text-white" GridLines="None" OnRowCommand="gvProducts_RowCommand">
                <Columns>
                    <asp:BoundField DataField="ProductID" HeaderText="ID" ReadOnly="True" />
                    <asp:BoundField DataField="ProductName" HeaderText="Name" />
                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="Stock" HeaderText="Stock" />
                    <asp:BoundField DataField="Rating" HeaderText="Rating" />
                    <asp:BoundField DataField="CategoryID" HeaderText="Category" />
                    <asp:BoundField DataField="ImageUrl" HeaderText="Image URL" />

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <button type="button" class="btn btn-warning text-white px-3 py-1 rounded-md shadow-md transition hover:scale-110"
                                onclick='<%# "openProductModal(\"" + Eval("ProductID") + "\", \"" + Eval("ProductName") + "\", \"" + Eval("Price") + "\", \"" + Eval("Stock") + "\", \"" + Eval("Rating") + "\", \"" + Eval("CategoryID") + "\", \"" + Eval("ImageUrl") + "\", \"edit\")" %>'>
                                Edit
                            </button>

                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger text-white px-3 py-1 rounded-md ml-2 shadow-md transition hover:scale-110"
                                CommandName="DeleteProduct" CommandArgument='<%# Eval("ProductID") %>' 
                                OnClientClick="return confirm('Are you sure you want to delete this product?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- ➕ Add Product Button & 🔙 Back to Dashboard -->
        <div class="mt-6 flex justify-center gap-4">
            <asp:HyperLink ID="lnkAddProduct" runat="server" NavigateUrl="AddProduct.aspx" CssClass="btn btn-success px-6 py-2 text-white rounded-md shadow-md transition hover:bg-teal-700 hover:scale-105">
                + Add Product
            </asp:HyperLink>

            <a href="AdminDashboard.aspx" class="btn btn-primary bg-gray-700 text-white px-6 py-2 rounded-md shadow-md transition hover:bg-gray-800 hover:scale-105">
                ← Back to Dashboard
            </a>
        </div>

        <!-- 📝 Add/Edit Product Modal -->
        <div id="productModal" class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 hidden">
            <div class="bg-white rounded-lg shadow-2xl p-6 w-full max-w-lg">
                <h2 id="modalTitle" class="text-2xl font-bold text-teal-600 mb-4 text-center">Add Product</h2>

                <asp:HiddenField ID="hfProductID" runat="server" />

                <label>Product Name</label>
                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control w-full" />

                <label>Price</label>
                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control w-full" />

                <label>Stock</label>
                <asp:TextBox ID="txtStock" runat="server" CssClass="form-control w-full" />

                <label>Rating</label>
                <asp:TextBox ID="txtRating" runat="server" CssClass="form-control w-full" />

                <label>Category</label>
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control w-full"></asp:DropDownList>

                <label>Image URL</label>
                <asp:TextBox ID="txtImageUrl" runat="server" CssClass="form-control w-full" />

                <div class="flex justify-end gap-4 mt-6">
                    <button type="button" class="btn btn-danger" onclick="closeProductModal()">Cancel</button>
                    <asp:Button ID="btnSaveProduct" runat="server" Text="Save Product" CssClass="btn btn-success" OnClick="btnSaveProduct_Click" />
                </div>
            </div>
        </div>

        <script>
            function openProductModal(id, name, price, stock, rating, category, imageUrl, mode) {
                document.getElementById("<%= hfProductID.ClientID %>").value = id;
                document.getElementById("<%= txtProductName.ClientID %>").value = name;
                document.getElementById("<%= txtPrice.ClientID %>").value = price;
                document.getElementById("<%= txtStock.ClientID %>").value = stock;
                document.getElementById("<%= txtRating.ClientID %>").value = rating;
                document.getElementById("<%= ddlCategory.ClientID %>").value = category;
                document.getElementById("<%= txtImageUrl.ClientID %>").value = imageUrl;

                document.getElementById("modalTitle").innerText = (mode === "edit") ? "Edit Product" : "Add Product";
                document.getElementById("productModal").classList.remove("hidden");
            }

            function closeProductModal() {
                document.getElementById("productModal").classList.add("hidden");
            }
        </script>
    </div>
</asp:Content>