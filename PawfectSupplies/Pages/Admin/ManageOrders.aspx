<%@ Page Title="Manage Orders" Language="C#" 
    MasterPageFile="~/MasterPages/AdminMaster.master" 
    AutoEventWireup="true" 
    CodeBehind="ManageOrders.aspx.cs" 
    Inherits="PawfectSupplies.Pages.Admin.ManageOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mx-auto px-6 py-8">
        <h1 class="text-3xl font-bold text-white text-center bg-teal-600 py-4 rounded-md shadow-md">Manage Orders</h1>

        <!-- 🔎 Search Bar -->
        <div class="flex justify-center mt-6 mb-6">
            <asp:Panel runat="server" DefaultButton="btnSearchOrder">
                <div class="flex items-center bg-white rounded-md shadow-lg overflow-hidden border border-gray-300 transition-transform transform hover:scale-105" style="max-width: 400px;">
                    <asp:TextBox ID="txtSearchOrder" runat="server" CssClass="form-control border-none focus:ring-0 px-4 py-2 text-gray-800 w-full" placeholder="Search orders..." AutoComplete="off" />
                    <asp:Button ID="btnSearchOrder" runat="server" Text="Search" CssClass="bg-teal-700 hover:bg-teal-800 text-white px-4 py-2 rounded-md transition" OnClick="btnSearchOrder_Click" />
                </div>
            </asp:Panel>
        </div>

        <!-- 🛒 Orders Table -->
        <div class="bg-white shadow-xl rounded-lg p-6 overflow-x-auto transition-transform transform hover:scale-105">
            <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" CssClass="table table-striped w-full text-left border border-gray-300"
                HeaderStyle-CssClass="bg-teal-600 text-white text-center" GridLines="None" OnRowCommand="gvOrders_RowCommand">
                <Columns>
                    <asp:BoundField DataField="OrderID" HeaderText="Order ID" ReadOnly="True" ItemStyle-CssClass="text-center px-4 py-3" />
                    <asp:BoundField DataField="UserID" HeaderText="User ID" ItemStyle-CssClass="text-center px-4 py-3" />
                    <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:MM/dd/yyyy HH:mm}" ItemStyle-CssClass="text-center px-4 py-3" />
                    <asp:BoundField DataField="TotalPrice" HeaderText="Total Amount" DataFormatString="{0:C}" ItemStyle-CssClass="text-center px-4 py-3 text-teal-700 font-semibold" />

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# GetStatusClass(Eval("OrderStatus").ToString()) %> px-4 py-2 block w-fit mx-auto rounded-full text-xs font-semibold'>
                                <%# Eval("OrderStatus") %>
                            </span>
                        </ItemTemplate>
                        <ItemStyle CssClass="text-center px-4 py-3" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <button type="button" class="btn btn-warning text-white px-3 py-1 rounded-md shadow-md transition hover:scale-110"
                                onclick='<%# "openEditModal(\"" + Eval("OrderID") + "\", \"" + Eval("OrderStatus") + "\")" %>'>
                                Edit
                            </button>
                        </ItemTemplate>
                        <ItemStyle CssClass="text-center px-4 py-3" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- 🔙 Back to Dashboard -->
        <div class="mt-6 flex justify-center">
            <a href="AdminDashboard.aspx" class="btn btn-primary px-6 py-2 text-white rounded-md shadow-md transition hover:bg-gray-700 hover:scale-105">
                ← Back to Dashboard
            </a>
        </div>

        <!-- ✏️ Edit Order Status Modal -->
        <div id="editOrderModal" class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 hidden">
            <div class="bg-white rounded-lg shadow-2xl p-6 w-full max-w-md transition-transform transform hover:scale-105">
                <h2 class="text-2xl font-bold text-teal-600 mb-4 text-center">Edit Order Status</h2>

                <asp:HiddenField ID="hfOrderID" runat="server" />

                <div>
                    <label class="block text-gray-700 font-semibold mb-1">Order Status</label>
                    <asp:DropDownList ID="ddlOrderStatus" runat="server" CssClass="form-control w-full border px-3 py-2 rounded-md">
                        <asp:ListItem Text="Pending" Value="Pending" />
                        <asp:ListItem Text="Completed" Value="Completed" />
                        <asp:ListItem Text="Canceled" Value="Canceled" />
                    </asp:DropDownList>
                </div>

                <div class="flex justify-end gap-4 mt-6">
                    <button type="button" class="btn btn-danger bg-red-500 text-white px-6 py-2 rounded-md shadow-md transition hover:bg-red-600 hover:scale-105" onclick="closeEditModal()">Cancel</button>
                    <asp:Button ID="btnSaveOrder" runat="server" Text="Save Changes" CssClass="btn btn-success bg-teal-600 text-white px-6 py-2 rounded-md shadow-md transition hover:bg-teal-700 hover:scale-105" OnClick="btnSaveOrder_Click" />
                </div>
            </div>
        </div>

        <script>
            function openEditModal(orderId, currentStatus) {
                document.getElementById("<%= hfOrderID.ClientID %>").value = orderId;
                document.getElementById("<%= ddlOrderStatus.ClientID %>").value = currentStatus;

                document.getElementById("editOrderModal").classList.remove("hidden");
                document.getElementById("editOrderModal").classList.add("flex");
            }

            function closeEditModal() {
                document.getElementById("editOrderModal").classList.add("hidden");
                document.getElementById("editOrderModal").classList.remove("flex");
            }
        </script>
    </div>
</asp:Content>
