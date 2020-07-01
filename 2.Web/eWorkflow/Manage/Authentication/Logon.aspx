<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logon.aspx.cs" Inherits="Manage_Authentication_Logon" ValidateRequest="false" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="UTF-8" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Bayer e-Workflow </title>
	<meta content="IE=edge" http-equiv="X-UA-Compatible"/>
	<meta name="viewport" content="width=device-width, initial-scale=1"/>
    <meta name="description" content="Bayer, Bayer e-Workflow"/>
	<meta name="keywords" content="Bayer, Bayer e-Workflow"/>

    <script src="/eWorks/Scripts/jquery/jquery-1.11.1.min.js"></script> 
    <link rel="shortcut icon" href="/eWorks/Styles/images/e_workflow_icon.ico"/>
    <link href="/eWorks/Styles/css/style.min.css" rel="stylesheet" />

    <script type="text/javascript" src="/eWorks/Scripts/Common.js"></script>
    <script type="text/javascript" src="/eWorks/Scripts/FormEvent.js"></script>
    <script type="text/javascript">
        var DIALOGWIDTH = 405;
        var DIALOGHEIGHT = 420;
        var DIALOGSMALLHEIGHT = 180;

        function fn_DisabledFireFox()
        {
            $(".inall").css("display", "none"); 
        }
        function fn_PageInit() {
            try {
                if ($telerik.isFirefox || $telerik.isFirefox2 || $telerik.isFirefox3)
                {
                    radalert("eWorkflow는 Firefox에서 사용할 수 없습니다.<br/>Internet Explorer에서 사용하시기 바랍니다.", 350, 150, "Result", fn_DisabledFireFox);
                    netscape.security.PrivilegeManager.enablePrivilege('UniversalBrowserWrite');
                }


                fn_WindowOnLoad();
                if (hddLoginCheck == "OK")
                    closeWin();
            }
            catch (exception) { }
        }

        function closeWin() {

            var obj = GetRadWindow();
            obj.BrowserWindow.location.reload();
            obj.close();
           
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.radWindow;
            else if (window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow;

           
            return oWindow;
        }
 
    </script>

</head>
<body onload="fn_PageInit();">
    <form id="frmLogin" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager ID="masterWinMgr" runat="server" EnableShadow="true"  Skin="Metro"></telerik:RadWindowManager>

        <div class="login">
            <div class="myin">
                <ul class="inall">
                    <li><input type="text" id="htxtUserID" class="login_input" autofocus runat="server" /></li>
                    <li><input type="password" id="htxtUserPassword" class="login_input" onkeydown="onEnter(event)" runat="server" /></li>
                    <li><a id="hacConfirm" class="btn login_btn" href="#" runat="server">LOG IN</a></li>
                     <li style="float:right;padding-right:5px;margin-top:10px;">
                        <img src="/eWorks/Styles/images/ic_image/restricted.png" >
                    </li>
                </ul>
            </div>
        </div>

        <input type="hidden" id="htxtMessage" runat="server" />
        <input type="hidden" id="eWLanguage" runat="server" />
        <input type="hidden" id="hReturnURL" runat="server" />

        <input type="hidden" id="errorMessage" runat="server" />
        <input type="hidden" id="informationMessage" runat="server" />
        <input type="hidden" id="confirmMessage" runat="server" />
        <input type="hidden" id="hddLoginCheck" runat="server" />
   </form>
 
 
</body>
<script type="text/javascript">
    function onEnter(e) {
        var theKey = 0;
        e = (window.event) ? event : e;
        theKey = (e.keyCode) ? e.keyCode : e.charCode;
        if (theKey == "13") {
            document.getElementById("<%=hacConfirm.ClientID %>").click();
                }
    }
  
    function confirmCallBackFn(arg) {

        radalert("<strong>radconfirm</strong> returned the following result: <h3 style='color: #ff0000;'>" + arg + "</h3>", 350, 250, "Result");

    }

</script>
</html>