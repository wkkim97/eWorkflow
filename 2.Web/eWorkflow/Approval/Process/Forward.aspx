<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="Forward.aspx.cs" Inherits="Approval_Process_Forward" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/Common/UserAutoCompleteBox.ascx" TagPrefix="uc1" TagName="UserAutoCompleteBox" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script src="/eWorks/Scripts/Common/InterFaseService.js"></script>
    <script type="text/javascript">
        
        function fn_CancelClick(sender, args) {
            alert("Forwardclose");
            var oArg = new Object();
            oArg.returnValue = false;
            var oWnd = GetRadWindow();
            GetRadWindow().BrowserWindow.location.href = GetRadWindow().BrowserWindow.location.href;
            if (oArg.returnValue) {
                oWnd.close(oArg);
            }
            else
                oWnd.close();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" runat="Server">
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
    <div class="data_type1">
        <table>
            <tbody>
                <tr>
                    <th>User Name</th>
                </tr>
                <tr>
                    <td>
                        <uc1:UserAutoCompleteBox runat="server" ID="UserAutoCompleteBox" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="align_center pt20">
        <telerik:RadButton ID="btnForward" CssClass="btn btn-blue btn-size3 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" OnClick="btnForward_Click"  Text="Forward" runat="server"></telerik:RadButton>
        <telerik:RadButton ID="btnCancel" CssClass="btn btn-darkgray btn-size3 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" OnClientClicked="fn_CancelClick" Text="Cancel" runat="server"></telerik:RadButton>
    </div>
 

    <input id="hddProcessID" type="hidden" runat="server" />
    <input id="hddDocumentID" type="hidden" runat="server" />
    <input id="hddProcessType" type="hidden" runat="server" />
    <input id="hddUserID" type="hidden" runat="server" />
</asp:Content>

