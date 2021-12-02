<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PopupNewRequest.aspx.cs" Inherits="Approval_Common_PopupNewRequest" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>New Request</title>
    <script src="/eWorks/Scripts/Approval/List.min.js"></script>
    <script src="/eWorks/Scripts/Common.js"></script>
    <script type="text/javascript">
 
        function ClientNodeClick(sender, eventArgs)
        {
           
            var curPage = new Object();
            var radWin = GetRadWindow();
            radWin.close(curPage);
            
            //RadOpenTest(sender, eventArgs);
            PopupOpenTest(sender, eventArgs);
        }

        //RadWindow
        function RadOpenTest(sender, eventArgs)
        {
            var node = eventArgs.get_node();
            var nodeText = node.get_text();
            var nodeValue = node.get_value();

            var parentPage = GetRadWindow().BrowserWindow;
            var parentRadWindowManager = parentPage.GetRadWindowManager();
            window.setTimeout(function ()
            {
                parentRadWindowManager.open("/eWorks/Approval/Document/DocSample.aspx?documentid=" + nodeValue, "RadWindowTest");
            }, 0);
        }

        //Browser Popup
        function PopupOpenTest(sender, eventArgs)
        {

            var node = eventArgs.get_node();
            var nodeText = node.get_text();
            var nodeValue = node.get_value(); 
            //window.open("/eWorks/Approval/Document/" + node.get_attributes()._data.formName + "?documentid=" + nodeValue, "WindowPopup", "width=670px, height=800px, top=400px, left=750px");
            fn_ShowDocument(node.get_attributes()._data.formName, nodeValue, "");
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />            
            <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server" Skin="Metro" />

        </div>
        <div>
            <telerik:RadTreeView runat="server" ID="RadTreeViewNewRequest" OnClientNodeClicked="ClientNodeClick">
                <DataBindings>
                    <telerik:RadTreeNodeBinding Expanded="True"></telerik:RadTreeNodeBinding>
                </DataBindings>
            </telerik:RadTreeView>
        </div>
    </form>
</body>
</html>
