<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="alertTest.aspx.cs" Inherits="Approval.Templete.alertTest" %>
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
        function confirmCallBack(args) {
            alert(args);
        }

        function onClientBut(sender, args) {
            fn_OpenConfirm("dddd", confirmCallBack);

            sender.set_autoPostBack(false);
        }

        function onClientBut2(sender, args) {
            fn_OpenInformation("테스트");
            sender.set_autoPostBack(false);
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadWindowManager ID="masterWinMgr" runat="server" EnableShadow="true"></telerik:RadWindowManager>
    <div>
          <telerik:RadButton ID="RadButton1" runat="server" Text="RadButton" OnClientClicked="onClientBut"></telerik:RadButton>
        <telerik:RadButton ID="radButt2" runat="server" Text="radAlertTest" OnClientClicked="onClientBut2"></telerik:RadButton>
    </div>
    </form>
</body>
</html>
