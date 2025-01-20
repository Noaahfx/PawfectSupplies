<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/MasterPages/AdminMaster.master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="PawfectSupplies.Pages.Admin.AdminDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="p-6">
        <h1 class="text-2xl font-bold text-teal-600 mb-4">Welcome, Admin</h1>
        <div class="grid grid-cols-3 gap-6">
            <div class="card p-4 bg-teal-100 text-center shadow-md">
                <h2 class="text-xl font-bold">Total Products</h2>
                <asp:Label ID="lblTotalProducts" runat="server" CssClass="text-teal-700 text-lg font-bold"></asp:Label>
            </div>
            <div class="card p-4 bg-green-100 text-center shadow-md">
                <h2 class="text-xl font-bold">Total Users</h2>
                <asp:Label ID="lblTotalUsers" runat="server" CssClass="text-green-700 text-lg font-bold"></asp:Label>
            </div>
            <div class="card p-4 bg-blue-100 text-center shadow-md">
                <h2 class="text-xl font-bold">Total Orders</h2>
                <asp:Label ID="lblTotalOrders" runat="server" CssClass="text-blue-700 text-lg font-bold"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
