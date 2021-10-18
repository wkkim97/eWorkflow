<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TextBoxTest.aspx.cs" Inherits="Templete_TextBoxTest" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Common/UserAutoCompleteBox.ascx" TagPrefix="uc1" TagName="UserAutoCompleteBox" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server"></telerik:RadScriptManager>
        <div> 
                    
            1. RadNumericTextBox<br />
            <telerik:RadNumericTextBox runat="server" ID="RadNu" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="300px" EnabledStyle-HorizontalAlign="Right"></telerik:RadNumericTextBox><br /><br />
            2. ASP.NET TEXTBOX<br />
            <asp:TextBox runat="server" ID="AspText"  Width="300px"></asp:TextBox>
        </div>
    </form>
</body>
</html>
