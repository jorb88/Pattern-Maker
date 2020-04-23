<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PatternMaker.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div class="jumbotron">
        <h1>Pattern Maker</h1>
        <p class="lead">Generate C# or Java controller classes by entering a state transition table. Controller implementation will use the State and Observer design patterns.</p>
        <p>
            <a href="https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller" target="_blank" class="btn btn-primary btn-lg">Learn more about MVC &raquo;</a>
            <a href="https://en.wikipedia.org/wiki/State_pattern" target="_blank" class="btn btn-primary btn-lg">Learn more about the state pattern &raquo;</a>
            <a href="https://en.wikipedia.org/wiki/Observer_pattern" target="_blank" class="btn btn-primary btn-lg">Learn more about the observer pattern &raquo;</a>
        </p>
    </div>
    <table>
        <tr>
            <td><h4>&nbsp;</h4></td>
            <td colspan="2"><h3>Please log in</h3></td>
        </tr>
        <tr>
            <td>User Name:&nbsp;&nbsp;</td>
            <td>
                <asp:TextBox ID="txtUserName" runat="server" Width="200px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="User name  required" ControlToValidate="txtUserName" Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>Password:&nbsp;&nbsp;</td>
            <td>
                <asp:TextBox ID="txtPassowrd" runat="server" Width="200px" TextMode="Password"></asp:TextBox>
            </td>
            <td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Password required" ControlToValidate="txtPassowrd" Font-Bold="True" ForeColor="Red"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click1" />
            &nbsp;<asp:Button ID="btnCreateNew" runat="server" OnClick="btnCreateNew_Click" Text="New Account" Width="142px" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
