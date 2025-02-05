<%@ Page Title="Products" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="PawfectSupplies.Pages.User.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-6 py-10">
        <h1 class="text-3xl font-bold text-teal-600 text-center mb-10">Our Products</h1>

        <!-- Category Filter Dropdown -->
        <div class="flex justify-center mb-6">
            <asp:DropDownList ID="ddlCategoryFilter" runat="server" AutoPostBack="true" CssClass="form-select bg-teal-600 text-teal px-4 py-2 rounded-lg shadow-md"
                OnSelectedIndexChanged="ddlCategoryFilter_SelectedIndexChanged">
            </asp:DropDownList>
        </div>

        <!-- Products Grouped by Category -->
        <asp:Repeater ID="CategoryRepeater" runat="server">
            <ItemTemplate>
                <!-- Category Header -->
                <h2 class="text-2xl font-bold text-teal-700 mt-8 mb-4"><%# Eval("CategoryName") %></h2>

                <!-- Products Grid -->
                <div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                    <asp:Repeater ID="ProductsRepeater" runat="server" DataSource='<%# Eval("Products") %>' OnItemCommand="ProductsRepeater_ItemCommand" OnItemDataBound="ProductsRepeater_ItemDataBound">
                        <ItemTemplate>
                            <div class="card shadow-lg bg-white shadow-md rounded-lg overflow-hidden transition-transform transform hover:scale-105 p-3">
                    
                                <!-- Product Image -->
                                <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("ProductName") %>' class="w-full h-60 object-cover rounded-md">

                                <!-- Product Details -->
                                <div class="p-2 text-center">
                                    <h5 class="text-sm font-medium text-teal-700"><%# Eval("ProductName") %></h5>
                                    <p class="text-gray-600 text-sm mt-1">$<%# Eval("Price", "{0:0.00}") %></p>

                                    <!-- Star Ratings Section -->
                                    <asp:PlaceHolder ID="phRating" runat="server">
                                        <div class="flex flex-col items-center justify-center mt-2 mb-3">
                                            <div class="flex items-center gap-1 text-yellow-500 text-lg">
                                                <asp:Literal ID="litStars" runat="server"></asp:Literal>
                                            </div>
                                            <span class="text-sm font-semibold text-gray-700 mt-1">
                                                (<asp:Label ID="lblRatingValue" runat="server"></asp:Label>)
                                            </span>
                                        </div>
                                    </asp:PlaceHolder>

                                    <!-- Add to Cart Button -->
                                    <asp:PlaceHolder ID="phAddToCart" runat="server">
                                        <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart"
                                            CommandName="AddToCart" CommandArgument='<%# Eval("ProductID") %>'
                                            CssClass="w-full text-xs px-3 py-1 bg-teal-600 hover:bg-teal-700 text-white rounded-md transition-all duration-300 hover:shadow-md" />
                                    </asp:PlaceHolder>

                                    <!-- Quantity Controls -->
                                    <asp:PlaceHolder ID="phQuantityControls" runat="server" Visible="false">
                                        <div class="flex items-center justify-center gap-4 mt-2">
                                            <asp:Button ID="btnDecrease" runat="server" Text="-"
                                                CommandName="DecreaseQuantity" CommandArgument='<%# Eval("ProductID") %>'
                                                CssClass="px-3 py-1 bg-teal-600 hover:bg-teal-700 text-white rounded-md transition-all duration-300 hover:shadow-md" />
                                            <asp:Label ID="lblQuantity" runat="server" CssClass="text-lg font-extrabold text-gray-900 min-w-[50px] text-center"></asp:Label>
                                            <asp:Button ID="btnIncrease" runat="server" Text="+"
                                                CommandName="IncreaseQuantity" CommandArgument='<%# Eval("ProductID") %>'
                                                CssClass="px-3 py-1 bg-teal-600 hover:bg-teal-700 text-white rounded-md transition-all duration-300 hover:shadow-md" />
                                        </div>
                                    </asp:PlaceHolder>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
