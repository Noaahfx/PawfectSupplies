<%@ Page Title="Add Product" Language="C#" 
    MasterPageFile="~/MasterPages/AdminMaster.master" 
    AutoEventWireup="true" 
    CodeBehind="AddProduct.aspx.cs" 
    Inherits="PawfectSupplies.Pages.Admin.AddProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-6 py-8">
        <h1 class="text-3xl font-bold text-white text-center bg-teal-600 py-4 rounded-md shadow-md">Add New Product</h1>

        <!-- Error Message -->
        <asp:Label ID="errorMessage" runat="server" CssClass="hidden bg-red-500 text-white text-center p-3 rounded-md shadow-md mt-4"></asp:Label>

        <div class="bg-white shadow-xl rounded-lg p-6 mt-6 max-w-lg mx-auto">
            <asp:Panel runat="server" CssClass="space-y-4">
                <div>
                    <label class="block text-gray-700 font-semibold">Product Name <span class="text-red-500">*</span></label>
                    <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control w-full" placeholder="Enter product name" />
                </div>

                <div>

                <div>
                    <label class="block text-gray-700 font-semibold">Price ($) <span class="text-red-500">*</span></label>
                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control w-full" TextMode="Number" step="0.01" oninput="validateDecimal(this)" />
                </div>

                <div>
                    <label class="block text-gray-700 font-semibold">Stock <span class="text-red-500">*</span></label>
                    <asp:TextBox ID="txtStock" runat="server" CssClass="form-control w-full" TextMode="Number" />
                </div>

                <div>
                    <label class="block text-gray-700 font-semibold">Rating (0-5)</label>
                    <asp:TextBox ID="txtRating" runat="server" CssClass="form-control w-full" TextMode="Number" min="0" max="5" step="0.1" oninput="validateRating(this)" />
                </div>

                <div>
                    <label class="block text-gray-700 font-semibold">Category <span class="text-red-500">*</span></label>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control w-full"></asp:DropDownList>
                </div>

                <div>
                    <label class="block text-gray-700 font-semibold">Image URL <span class="text-red-500">*</span></label>
                    <asp:TextBox ID="txtImageUrl" runat="server" CssClass="form-control w-full" />
                </div>

                <div class="flex justify-between mt-6">
                    <asp:HyperLink ID="lnkCancel" runat="server" NavigateUrl="ManageProducts.aspx" CssClass="btn btn-danger px-6 py-2 text-white">Cancel</asp:HyperLink>
                    <asp:Button ID="btnAddProduct" runat="server" Text="Add Product" CssClass="btn btn-success px-6 py-2 text-white" OnClick="btnAddProduct_Click" />
                </div>
            </asp:Panel>
        </div>
    </div>

    <script>
        // Allow decimals in number input fields
        function validateDecimal(input) {
            input.value = input.value.replace(/[^0-9.]/g, ''); // Remove non-numeric except '.'
            if (input.value.includes('.')) {
                let parts = input.value.split('.');
                if (parts[1].length > 2) input.value = parts[0] + '.' + parts[1].slice(0, 2); // Limit to 2 decimal places
            }
        }

        // Ensure rating is between 0-5
        function validateRating(input) {
            if (input.value < 0) input.value = 0;
            if (input.value > 5) input.value = 5;
        }
    </script>

</asp:Content>
