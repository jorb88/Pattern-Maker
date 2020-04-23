<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PatternMaker._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="jumbotron">
        <h1>Pattern Maker</h1>
        <p class="lead">Generate C# or Java controller classes by entering a state transition table. Controller implementation will use the State and Observer design patterns.</p>
        <p>
            <a href="https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller" target="_blank" class="btn btn-primary btn-lg">Learn more about MVC &raquo;</a>
            <a href="https://en.wikipedia.org/wiki/State_pattern" target="_blank" class="btn btn-primary btn-lg">Learn more about the state pattern &raquo;</a>
            <a href="https://en.wikipedia.org/wiki/Observer_pattern" target="_blank" class="btn btn-primary btn-lg">Learn more about the observer pattern &raquo;</a>
        </p>
    </div>
    <div class="row">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Please select a controller:&nbsp;
        <asp:DropDownList ID="ddlSelectContoller" runat="server" Height="19px" Width="169px">
        </asp:DropDownList>
    &nbsp;<asp:Button ID="btnSelect" runat="server" OnClick="btnSelect_Click" Text="Select" />
    &nbsp;<asp:Button ID="txtDelete" runat="server" OnClick="txtDelete_Click" Text="Delete" />
&nbsp;<asp:Button ID="btnCreateNew" runat="server" OnClick="btnCreateNew_Click" Text="Create New State Transition Table" Width="270px" />
    &nbsp;<br /><br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblAreYouSure" runat="server" Text="Are you sure you want to delete this state transition table?" Visible="False" Font-Bold="True" ForeColor="Red"></asp:Label>
    &nbsp;<asp:Button ID="btnYes" runat="server" OnClick="btnYes_Click" Text="Yes" Visible="False" />
&nbsp;<asp:Button ID="btnNo" runat="server" OnClick="btnNo_Click" Text="No" Visible="False" />
    </div>

</asp:Content>
