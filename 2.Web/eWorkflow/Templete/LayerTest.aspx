<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LayerTest.aspx.cs" Inherits="Templete_LayerTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .opa {
            position: fixed;
            width: 100%;
            height: 100%;
            z-index: 1000; /* some high enough value so it will render on top */
            opacity: .5;
            /*filter: alpha(opacity=50);*/
            filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#99000000, endColorstr=#99000000);  /* For IE 5.5 - 7*/
            -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr=#99000000, endColorstr=#99000000)";  /* For IE 8*/
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="opa"></div>
        <asp:Button ID="btnTest" Text="test" runat="server" />
    </div>
    </form>
</body>
</html>
