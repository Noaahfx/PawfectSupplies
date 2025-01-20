<%@ Page Title="Home" Language="C#" MasterPageFile="~/MasterPages/UserMaster.master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="PawfectSupplies.Pages.User.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="grid grid-cols-3 gap-6 p-6">
        <asp:Repeater ID="ProductsRepeater" runat="server">
            <ItemTemplate>
                <div class="card shadow-md hover:shadow-lg transition-all duration-200">
                    <img src='<%# Eval("Image") %>' class="card-img-top rounded-t">
                    <div class="card-body bg-white p-4">
                        <h5 class="card-title text-lg font-bold text-teal-700"><%# Eval("Name") %></h5>
                        <p class="text-gray-700">$<%# Eval("Price") %></p>
                        <a href="Login.aspx" class="btn btn-primary mt-4">Add to Cart</a>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
