<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/MasterPages/AdminMaster.master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="PawfectSupplies.Pages.Admin.AdminDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-6 py-10">
        <h1 class="text-3xl font-bold text-white text-center bg-teal-600 py-4 rounded-md">Admin Dashboard</h1>
        <p class="text-gray-700 text-center my-4">Welcome, Admin! Choose an option below:</p>

        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <!-- Manage Users -->
            <div class="bg-white shadow-lg rounded-lg overflow-hidden transition-transform transform hover:scale-105 border border-gray-200">
                <div class="p-6">
                    <h3 class="text-lg font-semibold text-gray-700">Manage Users</h3>
                    <p class="text-gray-500 mt-2">View, edit, or remove registered users.</p>
                    <a href="ManageUsers.aspx" class="btn btn-success mt-4 w-full">Go to Manage Users</a>
                </div>
            </div>

            <!-- Manage Products -->
            <div class="bg-white shadow-lg rounded-lg overflow-hidden transition-transform transform hover:scale-105 border border-gray-200">
                <div class="p-6">
                    <h3 class="text-lg font-semibold text-gray-700">Manage Products</h3>
                    <p class="text-gray-500 mt-2">Add, edit, or remove products.</p>
                    <a href="ManageProducts.aspx" class="btn btn-primary mt-4 w-full">Go to Manage Products</a>
                </div>
            </div>

            <!-- Manage Orders -->
            <div class="bg-white shadow-lg rounded-lg overflow-hidden transition-transform transform hover:scale-105 border border-gray-200">
                <div class="p-6">
                    <h3 class="text-lg font-semibold text-gray-700">Manage Orders</h3>
                    <p class="text-gray-500 mt-2">Track and process customer orders.</p>
                    <a href="ManageOrders.aspx" class="btn btn-warning mt-4 w-full">Go to Manage Orders</a>
                </div>
            </div>

            <!-- Manage Categories -->
            <div class="bg-white shadow-lg rounded-lg overflow-hidden transition-transform transform hover:scale-105 border border-gray-200">
                <div class="p-6">
                    <h3 class="text-lg font-semibold text-gray-700">Manage Categories</h3>
                    <p class="text-gray-500 mt-2">Organize product categories.</p>
                    <a href="ManageCategories.aspx" class="btn btn-info mt-4 w-full">Go to Manage Categories</a>
                </div>
            </div>

            <!-- View Reports -->
            <div class="bg-white shadow-lg rounded-lg overflow-hidden transition-transform transform hover:scale-105 border border-gray-200">
                <div class="p-6">
                    <h3 class="text-lg font-semibold text-gray-700">View Reports</h3>
                    <p class="text-gray-500 mt-2">Analyze sales and user activity.</p>
                    <a href="ViewReports.aspx" class="btn btn-secondary mt-4 w-full">Go to Reports</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
