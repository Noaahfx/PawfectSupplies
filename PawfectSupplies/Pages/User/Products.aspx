<%@ Page Title="Products" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="PawfectSupplies.Pages.User.Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-6 py-10">
        <h1 class="text-3xl font-bold text-teal-600 text-center mb-10">Our Products</h1>
        <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
            <asp:Repeater ID="ProductsRepeater" runat="server" OnItemCommand="ProductsRepeater_ItemCommand" OnItemDataBound="ProductsRepeater_ItemDataBound">
                <ItemTemplate>
                    <div class="card bg-white shadow-lg rounded-lg overflow-hidden transition-transform transform hover:scale-105">
                        <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("ProductName") %>' class="w-full h-48 object-cover">
                        <div class="p-4">
                            <h5 class="text-lg font-semibold text-teal-700 mb-2"><%# Eval("ProductName") %></h5>
                            <p class="text-gray-700 mb-4">$<%# Eval("Price", "{0:0.00}") %></p>

                            <!-- Add to Cart Placeholder -->
                            <asp:PlaceHolder ID="phAddToCart" runat="server">
                                <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart"
                                    CommandName="AddToCart" CommandArgument='<%# Eval("ProductID") %>'
                                    CssClass="btn btn-primary mt-4 px-4 py-2 bg-teal-600 hover:bg-teal-700 rounded-full transition-all duration-300 hover:shadow-md transform hover:scale-110" />
                            </asp:PlaceHolder>

                            <!-- Quantity Counter Placeholder -->
                            <asp:PlaceHolder ID="phQuantityControls" runat="server" Visible="false">
                                <div class="flex items-center gap-2">
                                    <asp:Button ID="btnDecrease" runat="server" Text="-" 
                                        CommandName="DecreaseQuantity" CommandArgument='<%# Eval("ProductID") %>' 
                                        CssClass="btn btn-light px-3 py-1 rounded-md bg-gray-200 hover:bg-gray-300" />
                                    <asp:Label ID="lblQuantity" runat="server" CssClass="font-bold text-lg text-gray-800"></asp:Label>
                                    <asp:Button ID="btnIncrease" runat="server" Text="+" 
                                        CommandName="IncreaseQuantity" CommandArgument='<%# Eval("ProductID") %>' 
                                        CssClass="btn btn-light px-3 py-1 rounded-md bg-gray-200 hover:bg-gray-300" />
                                </div>
                            </asp:PlaceHolder>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
