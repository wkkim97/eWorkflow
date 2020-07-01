<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApproveMenuBar.ascx.cs" Inherits="Approval.Common.ApproveMenuBar" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<table style="border: 0; padding: 0; margin: 0;">
 
    <tr>
        <td colspan="4">
            <telerik:RadButton ID="btnRequest" runat="server" Text="Request"></telerik:RadButton>
            <telerik:RadButton ID="btnApproval" runat="server" Text="Approval"></telerik:RadButton>
            <telerik:RadButton ID="btnForwardApproval" runat="server" Text="Forward"></telerik:RadButton>
            <telerik:RadButton ID="btnReject" runat="server" Text="Reject"></telerik:RadButton>
            <telerik:RadButton ID="btnForward" runat="server" Text="Forward"></telerik:RadButton>
            <telerik:RadButton ID="btnRecall" runat="server" Text="Recall"></telerik:RadButton>
            <telerik:RadButton ID="btnWithdraw" runat="server" Text="Withdraw"></telerik:RadButton>
            <telerik:RadButton ID="btnRemind" runat="server" Text="Remind"></telerik:RadButton>
            <telerik:RadButton ID="btnExit" runat="server" Text="Exit"></telerik:RadButton>
            <telerik:RadButton ID="btnSave" runat="server" Text="Save"></telerik:RadButton>
            <telerik:RadButton ID="btnInputCommand" runat="server" Text="Input Command"></telerik:RadButton>
        </td>
    </tr>
    <tr>
        <th colspan="4"><span id="hspanTitle" runat="server">Title</span></th>
    </tr>
    <tr>
        <th >Requester</th>
        <td colspan="3" ><span id="hspanRequester" runat="server"></span></td>
    </tr>
    <tr>
        <th >Company</th>
        <td><span id="hspanCompany" runat="server"></span></td>
        <th>Organization</th>
        <td><span id="hspanOrganization" runat="server"></span></td>
    </tr>
    <tr>
        <th>Life Cycle</th>
        <td><span id="hspanLifeCycle" runat="server"></span></td>
        <th>Document No.</th>
        <td><span id="hspanDocumentNo" runat="server"></span></td>
    </tr>
</table>


