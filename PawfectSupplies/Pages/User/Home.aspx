<%@ Page Title="Home" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="PawfectSupplies.Pages.User.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-4">
        <!-- Hero Section -->
        <div class="relative bg-teal-100 rounded-lg shadow-lg mb-12 overflow-hidden">
            <img src="<%= ResolveUrl("~/Content/Images/hero-pet.jpg") %>" alt="Hero" class="w-full object-cover h-96">
            <div class="absolute inset-0 bg-black bg-opacity-50 flex items-center justify-center">
                <div class="text-center animate-fade-in">
                    <h1 class="text-4xl md:text-5xl font-extrabold text-white drop-shadow-lg">Welcome to Pawfect Supplies</h1>
                    <p class="text-xl text-white mt-4">Your one-stop shop for quality pet products</p>
                    <a href="Products.aspx" class="btn btn-primary mt-6 px-6 py-2 rounded-full bg-teal-600 hover:bg-teal-700 transition-transform transform hover:scale-105 hover:shadow-lg">Shop Now</a>
                </div>
            </div>
        </div>

        <!-- Featured Products -->
        <div class="mb-12">
            <h2 class="text-3xl font-bold text-teal-700 mb-8 text-center">Featured Products</h2>
            <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
                <asp:Repeater ID="FeaturedProductsRepeater" runat="server">
                    <ItemTemplate>
                        <div class="card shadow-lg hover:shadow-xl transition-transform transform hover:scale-105 rounded-lg overflow-hidden">
                            <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("ProductName") %>' class="w-full h-48 object-cover hover:opacity-90 transition-opacity">
                            <div class="p-4">
                                <h3 class="text-lg font-semibold text-teal-700"><%# Eval("ProductName") %></h3>
                                <p class="text-gray-600 mt-2">$<%# Eval("Price") %></p>
                                <a href="Login.aspx" class="btn btn-primary mt-4 px-4 py-2 bg-teal-600 hover:bg-teal-700 rounded-full transition-all duration-300 hover:shadow-md transform hover:scale-110">Add to Cart</a>
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
