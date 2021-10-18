<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="Approval.Manage.Authentication.Logon" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="IE=edge" http-equiv="X-UA-Compatible"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>

    <title>Bayer eWorkflow System</title>
    <script src="/eWorks/Scripts/jquery/jquery-1.11.1.min.js"></script>
    <link href="/eWorks/Styles/css/Logon.css" rel="stylesheet" />
    <link href="/eWorks/Styles/css/Telerik/RadWindow_style.css" rel="stylesheet" />
    <script type="text/javascript" src="/eWorks/Scripts/Common.js"></script>
    <script type="text/javascript" src="/eWorks/Scripts/FormEvent.js"></script>
    <script type="text/javascript">
        var DIALOGWIDTH = 405;
        var DIALOGHEIGHT = 420;
        var DIALOGSMALLHEIGHT = 180;

        function fn_PageInit() {
            try {
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
        <telerik:RadWindowManager ID="masterWinMgr" runat="server" EnableShadow="true" EnableAjaxSkinRendering="false" EnableEmbeddedSkins="false" Skin="radWin"></telerik:RadWindowManager>
        <!-- TOP BAR -->
        <div id="top-bar">
        </div>

        <!-- HEADER -->
        <div id="header">
            <div class="page-full-width cf">
                <div id="login-intro" class="fl">
                    Bayer eWorksflow
                </div>
            </div>
        </div> 
        <!-- MAIN CONTENT -->
        <div id="content"> 
            <div id="login-form"> 
                <fieldset> 
                    <p>
                        <label for="login-username">username</label>
                        <input type="text" id="htxtUserID" class="round full-width-input" autofocus  runat="server"  />
                    </p> 
                    <p>
                        <label for="login-password">password</label>
                        <input type="password" id="htxtUserPassword" class="round full-width-input" onkeydown="onEnter(event)" runat="server" />
                    </p> 
                    <a id="hacConfirm" class="button round blue image-right ic-right-arrow"  href="#" runat="server">LOG IN</a>
                </fieldset>
            </div> 
        </div>
        <!-- end content -->

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