<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateTests.aspx.cs" Inherits="PatternMaker.GenerateTests" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="EditStateTransitions.aspx">Back to state transition table entry</asp:HyperLink><br />
            <asp:TextBox ID="txtCode" runat="server" Align="center" Font-Names="Consolas" TextMode="MultiLine" Width="90%" Height="699px"></asp:TextBox>
        </div>
    </form>
</body>
</html>
