<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="Approve.aspx.cs" Inherits="Approval_Process_Approve" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script src="/eWorks/Scripts/Common/InterFaseService.js"></script>
    <script type="text/javascript">
        function fn_CancelClick(sender, args) {
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

    <div class="data_type1">
        <table>
            <tbody>
                <tr>
                    <th>Comment</th>
                </tr>
                <tr>
                    <td>
                        <telerik:RadTextBox TextMode="MultiLine" Height="110" Width="100%" ID="txtComment" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="align_center pt20">
        <telerik:RadButton ID="btnOk" CssClass="btn btn-blue btn-size3 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" OnClick="btnOk_Click" Text="Ok" runat="server"></telerik:RadButton>
        <telerik:RadButton ID="btnCancel" CssClass="btn btn-darkgray btn-size3 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" OnClientClicked="fn_CancelClick" Text="Cancel" runat="server"></telerik:RadButton>
    </div>
     
    <input id="hddProcessID" type="hidden" runat="server" />
    <input id="hddDocumentID" type="hidden" runat="server" />
    <input id="hddProcessType" type="hidden" runat="server" />
</asp:Content>

