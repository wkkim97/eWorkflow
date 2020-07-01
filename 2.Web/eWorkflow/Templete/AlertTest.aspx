<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AlertTest.aspx.cs" Inherits="Templete_AlertTest" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <link href="/eWorks/Styles/css/style.css" rel="stylesheet" />
    <script src="/eWorks/Scripts/Common.js"></script>
    <script src="/eWorks/Scripts/ControlUtil.js"></script>
    <script src="/eWorks/Scripts/FormEvent.js"></script>
    <script type="text/javascript">
        function confirmCallBack( args) {
            if(args)
                __doPostBack("RadButton1", "");
        }

        function onClientBut(sender, args) {
            fn_OpenConfirm("Are You OK?", confirmCallBack);

            sender.set_autoPostBack(false);
        }

        function onClientBut2(sender, args) {
            fn_OpenInformation("테스트");
            sender.set_autoPostBack(false);
        }

 
    </script>
</head>
<body onload="fn_WindowOnLoad()">
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadWindowManager ID="masterWinMgr" runat="server"  ></telerik:RadWindowManager>
    <div>
          <telerik:RadButton ID="RadButton1" runat="server" Text="openConfirm" OnClientClicked="onClientBut" OnClick="RadButton1_Click"></telerik:RadButton>
        <telerik:RadButton ID="radButt2" runat="server" Text="radAlertTest" OnClientClicked="onClientBut" OnClick="radButt2_Click"></telerik:RadButton>
    </div>
 
    </form>
</body>
</html>
