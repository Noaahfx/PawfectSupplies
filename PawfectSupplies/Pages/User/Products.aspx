<%@ Page Title="Products" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="PawfectSupplies.Pages.User.Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-6 py-10">
        <h1 class="text-3xl font-bold text-teal-600 text-center mb-10">Our Products</h1>
        <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
            <asp:Repeater ID="ProductsRepeater" runat="server" OnItemCommand="ProductsRepeater_ItemCommand">
                <ItemTemplate>
                    <div class="card bg-white shadow-lg rounded-lg overflow-hidden transition-transform transform hover:scale-105">
                        <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("ProductName") %>' class="w-full h-48 object-cover">
                        <div class="p-4">
                            <h5 class="text-lg font-semibold text-teal-700 mb-2"><%# Eval("ProductName") %></h5>
                            <p class="text-gray-700 mb-4">$<%# Eval("Price", "{0:0.00}") %></p>
                            <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart"
                                CommandName="AddToCart" CommandArgument='<%# Eval("ProductID") %>'
                                CssClass="btn btn-primary mt-4 px-4 py-2 bg-teal-600 hover:bg-teal-700 rounded-full transition-all duration-300 hover:shadow-md transform hover:scale-110" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>