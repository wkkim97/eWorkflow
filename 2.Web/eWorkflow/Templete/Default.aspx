<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Templete_Default" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" Runat="Server">
        <script type="text/javascript">
        function confirmCallBack( args) {
            if(args)
                __doPostBack("RadButton1", "");
        }



        function onClientBut(sender, args) {
            var callBackFunction = Function.createDelegate(sender, function (shouldSubmit) {

                if (shouldSubmit) {

                    this.click();

                }

            });
            fn_OpenConfirm("Are You OK?", callBackFunction);

            sender.set_autoPostBack(false);
        }

        function RadConfirm(sender, args) {

            var callBackFunction = Function.createDelegate(sender, function (shouldSubmit) {

                if (shouldSubmit) {
                    sender.set_autoPostBack(true);
                    this.click();

                }

            });



            var text = "Are you sure you want to submit the page?";

            fn_OpenConfirm("Are You OK?", callBackFunction);
            sender.set_autoPostBack(false);

        }

        function onClientBut2(sender, args) {
            fn_OpenInformation("테스트");
            sender.set_autoPostBack(false);
        }

 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" Runat="Server">
      
    <telerik:RadWindowManager ID="masterWinMgr" runat="server"  ></telerik:RadWindowManager>
    <div>
        <telerik:RadButton ID="RadButton1" runat="server" Text="openConfirm" OnClientClicking="RadConfirm" OnClick="RadButton1_Click"></telerik:RadButton>
        <telerik:RadButton ID="radButt2" runat="server" Text="radAlertTest" OnClientClicked="onClientBut" ></telerik:RadButton>
    </div>
 
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" Runat="Server">
</asp:Content>

