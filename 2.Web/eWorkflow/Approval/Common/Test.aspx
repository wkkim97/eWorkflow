<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Approval_Common_Test" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>TEST</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server" Skin="Metro" />
        </div>
        <div>
            <telerik:RadTextBox runat="server" ID="RadTextBox1" Label="DOCUMENT NAME : " Width="300px"></telerik:RadTextBox>
            <telerik:RadTextBox runat="server" ID="RadTextBox2" Label="DOCUMENT ID : " Width="300px"></telerik:RadTextBox>
        </div>
    </form>
</body>
</html>
