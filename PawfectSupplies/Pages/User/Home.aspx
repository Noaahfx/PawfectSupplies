<%@ Page Title="Home" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="PawfectSupplies.Pages.User.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-4">
        <!-- Hero Section: Database-driven Interactive Carousel -->
        <div id="heroCarousel" class="carousel slide relative bg-teal-100 rounded-lg shadow-lg mb-12 overflow-hidden" data-bs-ride="carousel" data-bs-interval="5000">
    
            <!-- Indicator Dots -->
            <div class="carousel-indicators">
                <asp:Repeater ID="HeroIndicatorsRepeater" runat="server">
                    <ItemTemplate>
                        <button type="button" data-bs-target="#heroCarousel" data-bs-slide-to="<%# Container.ItemIndex %>"
                            class='<%# Container.ItemIndex == 0 ? "active" : "" %>' aria-label="Slide <%# Container.ItemIndex + 1 %>">
                        </button>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="carousel-inner">
                <asp:Repeater ID="HeroCarouselRepeater" runat="server">
                    <ItemTemplate>
                        <div class='carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>'>
                            <div class="relative w-full h-96">
                                <!-- Background Image -->
                                <img src="<%# Eval("ImageUrl") %>" class="d-block w-full h-96 object-cover">
                        
                                <!-- Fullscreen Overlay (Fixed Issue) -->
                                <div class="absolute inset-0 bg-black bg-opacity-50 flex flex-col items-center justify-center w-full h-full">
                                    <h1 class="text-4xl md:text-5xl font-extrabold text-white drop-shadow-lg"><%# Eval("Title") %></h1>
                                    <p class="text-xl text-white mt-2"><%# Eval("Subtitle") %></p>
                                    <a href="<%# Eval("Link") %>" class="btn btn-primary mt-6 px-6 py-2 rounded-full bg-teal-600 hover:bg-teal-700 transition-transform transform hover:scale-105 hover:shadow-lg">Shop Now</a>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- Carousel Controls -->
            <button class="carousel-control-prev" type="button" data-bs-target="#heroCarousel" data-bs-slide="prev">
                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            </button>
            <button class="carousel-control-next" type="button" data-bs-target="#heroCarousel" data-bs-slide="next">
                <span class="carousel-control-next-icon" aria-hidden="true"></span>
            </button>
        </div>

        <!-- Featured Products -->
        <div class="mb-12">
            <h2 class="text-3xl font-bold text-teal-700 mb-8 text-center">Featured Products</h2>

            <div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                <asp:Repeater ID="FeaturedProductsRepeater" runat="server" OnItemCommand="FeaturedProductsRepeater_ItemCommand" OnItemDataBound="FeaturedProductsRepeater_ItemDataBound">
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
        </div>

        <!-- Benefits Section -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6 text-center mb-12">
            <div class="p-6 shadow-lg bg-teal-50 rounded-lg hover:shadow-xl transition-transform transform hover:scale-105 hover:bg-teal-100">
                <img src="<%= ResolveUrl("~/Content/Images/fast-delivery.png") %>" alt="Fast Delivery" class="mx-auto w-20">
                <h4 class="text-lg font-bold text-teal-700 mt-4">Fast Delivery</h4>
                <p class="text-gray-600 mt-2">Get your pet supplies delivered quickly.</p>
            </div>
            <div class="p-6 shadow-lg bg-teal-50 rounded-lg hover:shadow-xl transition-transform transform hover:scale-105 hover:bg-teal-100">
                <img src="<%= ResolveUrl("~/Content/Images/high-quality.png") %>" alt="High Quality" class="mx-auto w-20">
                <h4 class="text-lg font-bold text-teal-700 mt-4">High-Quality Products</h4>
                <p class="text-gray-600 mt-2">We provide only the best for your pets.</p>
            </div>
            <div class="p-6 shadow-lg bg-teal-50 rounded-lg hover:shadow-xl transition-transform transform hover:scale-105 hover:bg-teal-100">
                <img src="<%= ResolveUrl("~/Content/Images/customer-service.png") %>" alt="Support" class="mx-auto w-20">
                <h4 class="text-lg font-bold text-teal-700 mt-4">Customer Support</h4>
                <p class="text-gray-600 mt-2">Our team is here to help you 24/7.</p>
            </div>
        </div>
    </div>
</asp:Content>
