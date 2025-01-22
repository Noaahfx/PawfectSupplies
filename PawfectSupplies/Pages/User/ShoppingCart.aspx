<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/UserMaster.master" CodeBehind="ShoppingCart.aspx.cs" Inherits="PawfectSupplies.Pages.User.ShoppingCart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h1>Your Shopping Cart</h1>
        <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvCart_RowCommand">
            <Columns>
                <asp:BoundField DataField="ProductName" HeaderText="Product" />
                <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                <asp:TemplateField HeaderText="Quantity">
                    <ItemTemplate>
                        <div class="flex items-center gap-2">
                            <asp:Button ID="btnMinus" runat="server" Text="-" CommandName="DecreaseQuantity" CommandArgument='<%# Eval("CartID") %>' CssClass="btn btn-light px-2 py-1 rounded-md" />
                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="font-semibold" />
                            <asp:Button ID="btnPlus" runat="server" Text="+" CommandName="IncreaseQuantity" CommandArgument='<%# Eval("CartID") %>' CssClass="btn btn-light px-2 py-1 rounded-md" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TotalPrice" HeaderText="Total Price" DataFormatString="{0:C}" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton 
                            ID="btnRemove" 
                            runat="server" 
                            CommandName="Remove" 
                            CommandArgument='<%# Eval("CartID") %>' 
                            ImageUrl="~/Content/Images/Icons/dustbin-icon.png" 
                            CssClass="h-6 w-6" 
                            AlternateText="Remove" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
        <div class="text-right">
            <h3>Total: <asp:Label ID="lblGrandTotal" runat="server" CssClass="text-success"></asp:Label></h3>
        </div>
    </div>
</asp:Content>
