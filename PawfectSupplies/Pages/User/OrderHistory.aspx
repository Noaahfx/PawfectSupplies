<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/UserMaster.master" CodeBehind="OrderHistory.aspx.cs" Inherits="PawfectSupplies.Pages.User.OrderHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-6 py-10">
        <h1 class="text-3xl font-bold text-teal-600 text-center mb-6">Your Orders</h1>

        <div class="w-full max-w-4xl mx-auto bg-white shadow-lg rounded-lg overflow-hidden border border-gray-200 transition-all duration-300 hover:shadow-2xl hover:bg-gray-100 hover:scale-105">

            <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" CssClass="w-full table-auto border-collapse text-gray-900 ">
                <HeaderStyle CssClass="bg-teal-600 text-white uppercase text-center font-semibold text-sm tracking-wider" />
                <RowStyle CssClass="border-b border-gray-200 text-center text-gray-800 font-medium transition-all duration-300" />
                <AlternatingRowStyle CssClass="bg-gray-50" />
                <EmptyDataRowStyle CssClass="text-gray-500 text-center font-medium p-5" />

                <Columns>
                    <asp:BoundField DataField="OrderID" HeaderText="Order ID" ItemStyle-CssClass="px-6 py-4 text-center font-semibold" />
                    <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-CssClass="px-6 py-4 text-gray-600 text-center" />
                    <asp:BoundField DataField="TotalPrice" HeaderText="Total" DataFormatString="{0:C}" ItemStyle-CssClass="px-6 py-4 text-teal-700 font-semibold text-center" />

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# GetStatusClass(Eval("OrderStatus").ToString()) %>'>
                                <%# Eval("OrderStatus") %>
                            </span>
                        </ItemTemplate>
                        <ItemStyle CssClass="px-6 py-4 text-center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
