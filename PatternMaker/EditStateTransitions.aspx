<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditStateTransitions.aspx.cs" Inherits="PatternMaker.EditStateTransitions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=PatternEntities" DefaultContainerName="PatternEntities" EnableDelete="True" EnableFlattening="False" EnableInsert="True" EnableUpdate="True" EntitySetName="StateTransitions" OnQueryCreated="EntityDataSource1_QueryCreated">
    </asp:EntityDataSource>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                        <br />
            <table>
                <tr>
                    <td><asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Namespace for controller:"></asp:Label>
</td>
                    <td><asp:TextBox ID="txtNamespace" runat="server" Width="179px"></asp:TextBox></td>
                    <td><asp:CheckBox ID="cbIncludeMessages" runat="server" Text="Include user instruction messages for each state" /></td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Name of controller class:"></asp:Label>
</td>
                    <td><asp:TextBox ID="txtControlClassName" runat="server" Width="179px"></asp:TextBox></td>
                    <td><asp:CheckBox ID="cbIncludeMethodAvailability" runat="server" Text="Include operation availability profile for each state" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" /></td>
                    <td><asp:CheckBox ID="cbUseInnerClasses" runat="server" Text="Generate controller code using inner classes" /></td>
                </tr>
                
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="2"><asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="#CC0000"></asp:Label></td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnAddNewRow" runat="server" OnClick="btnAddNewRow_Click1" Text="Add New Row" />
            &nbsp;<asp:Button ID="btnGenerate" runat="server" OnClick="btnGenerate_Click" Text="Generate Controller Code" />
            &nbsp;<asp:Button ID="btnGenerateTestCode" runat="server" OnClick="btnGenerateTestCode_Click" Text="Generate Unit Tests" />
            <br />
            <br />

            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="EntityDataSource1" ForeColor="Black" GridLines="Vertical" Width="98%" DataKeyNames="Id">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="ControllerID" HeaderText="CID" ReadOnly="True" SortExpression="ControllerID" />
                    <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id" />
                    <asp:BoundField DataField="StateName" HeaderText="State" SortExpression="StateName" />
                    <asp:BoundField DataField="Description" HeaderText="User instructions for this state" SortExpression="Description" />
                    <asp:BoundField DataField="MethodName" HeaderText="Method" SortExpression="MethodName" />
                    <asp:BoundField DataField="MethodParameter" HeaderText="Parameter Type" SortExpression="MethodParameter" />
                    <asp:BoundField DataField="NextState" HeaderText="Next State" SortExpression="NextState" />
                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ButtonType="Button" />
                </Columns>
                <FooterStyle BackColor="#CCCC99" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <RowStyle BackColor="#F7F7DE" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                <SortedAscendingHeaderStyle BackColor="#848384" />
                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                <SortedDescendingHeaderStyle BackColor="#575357" />
            </asp:GridView>
            <br />
            <asp:Label ID="lblHints" runat="server" Text="Helpful hints..."></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
