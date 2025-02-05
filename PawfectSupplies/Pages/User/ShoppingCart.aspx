<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/UserMaster.master" CodeBehind="ShoppingCart.aspx.cs" Inherits="PawfectSupplies.Pages.User.ShoppingCart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-6 py-10">
        <h1 class="text-3xl font-bold text-teal-600 text-center mb-10">Your Shopping Cart</h1>

        <!-- Cart Panel (Hidden when empty) -->
        <asp:Panel ID="pnlCart" runat="server" CssClass="shadow-lg rounded-lg bg-white p-6" Visible="false">
            <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" CssClass="w-full text-gray-900"
                GridLines="None" ShowHeader="true" OnRowCommand="gvCart_RowCommand">
                <HeaderStyle CssClass="bg-gray-100 text-teal-700 font-bold uppercase px-4 py-3 text-center" />
                <RowStyle CssClass="border-b border-gray-200 text-center align-middle" />

                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="Product" ItemStyle-CssClass="px-4 py-3 font-semibold text-left" />

                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" ItemStyle-CssClass="px-4 py-3 text-teal-600" />

                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <div class="flex items-center justify-center gap-4">
                                <asp:Button ID="btnMinus" runat="server" Text="−"
                                    CommandName="DecreaseQuantity" CommandArgument='<%# Eval("CartID") %>'
                                    CssClass="px-4 py-2 bg-teal-600 hover:bg-teal-700 text-white rounded-lg text-md shadow-md" />

                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="text-lg font-bold min-w-[40px] text-center" />

                                <asp:Button ID="btnPlus" runat="server" Text="+"
                                    CommandName="IncreaseQuantity" CommandArgument='<%# Eval("CartID") %>'
                                    CssClass="px-4 py-2 bg-teal-600 hover:bg-teal-700 text-white rounded-lg text-md shadow-md" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="TotalPrice" HeaderText="Total Price" DataFormatString="{0:C}" ItemStyle-CssClass="px-4 py-3 text-teal-600 font-semibold" />

                    <asp:TemplateField HeaderText="Remove">
                        <ItemTemplate>
                        <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove"
                            CommandArgument='<%# Eval("CartID") %>' ImageUrl="~/Content/Images/Icons/dustbin-icon.png"
                            AlternateText="Remove"
                            Style="width: 24px; height: 24px; object-fit: contain; cursor: pointer; opacity: 1; transition: opacity 0.3s ease-in-out;"
                            onmouseover="this.style.opacity='0.7'" onmouseout="this.style.opacity='1'" />

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <!-- Total Amount & Checkout Button -->
            <div class="flex justify-between items-center mt-6">
                <h3 class="text-lg font-bold text-teal-700">Total: <asp:Label ID="lblGrandTotal" runat="server"></asp:Label></h3>
                <asp:Button ID="btnCheckout" runat="server" Text="Checkout"
                    CssClass="px-5 py-2 bg-teal-600 hover:bg-teal-700 text-white rounded-md transition-all shadow-md text-md"
                    OnClick="btnCheckout_Click" />
            </div>
        </asp:Panel>

        <!-- Empty Cart Message -->
        <asp:Panel ID="pnlEmptyCart" runat="server" CssClass="text-center mt-10" Visible="false">
            <h2 class="text-2xl font-bold text-gray-700">Your cart is empty.</h2>
            <p class="text-gray-500 mt-2">Browse our shop and add items to your cart!</p>
            <a href="Products.aspx" class="mt-4 inline-block bg-teal-600 hover:bg-teal-700 text-white px-6 py-2 rounded-md transition-all">Shop Now</a>
        </asp:Panel>
    </div>
</asp:Content>
