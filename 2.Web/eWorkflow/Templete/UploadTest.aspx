<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadTest.aspx.cs" Inherits="Templete_UploadTest" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/Common/FileUpload.ascx" TagPrefix="uc1" TagName="FileUpload" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <script src="/eWorks/Scripts/jquery/jquery-1.11.1.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <uc1:FileUpload runat="server" ID="FileUpload" />
    </div>
    </form>
</body>
</html>
