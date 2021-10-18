<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoComplete.aspx.cs" Inherits="Templete_AutoComplete" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
 
<%@ Register Src="~/Common/UserAutoCompleteBox.ascx" TagPrefix="uc1" TagName="UserAutoCompleteBox" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <div>
        <uc1:UserAutoCompleteBox runat="server" id="UserAutoCompleteBox" />
    </div>
    </form>
</body>
</html>
