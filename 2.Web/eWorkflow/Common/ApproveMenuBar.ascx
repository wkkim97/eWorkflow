<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApproveMenuBar.ascx.cs" Inherits="Common_ApproveMenuBar" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Common/UserAutoCompleteBox.ascx" TagPrefix="uc1" TagName="UserAutoCompleteBox" %>


<script src="/eWorks/Scripts/Common/Command.js"></script> 
<style type="text/css">
                              
    .RadButton_cssType1  .rbText {
	 
     border-right:1px solid #fff!important;
     font-size:13px!important;
     padding-right:20px!important;
     line-height:15px!important;
     padding-bottom:4px!important;
     margin-bottom:2px!important;
    
}

</style>
<div id="doc_head">
    <div class="doc_logo">
        <h1 class="ci_small">
            <img src="/eWorks/Styles/images/ci_small.png" alt="e-workflow"></h1>
        <h1 class="bayer_small">
            <img src="/eWorks/Styles/images/bayer_small.png" alt="bayer"></h1>
    </div>
    <ul class="doc_gnb">
        <li>
            <a href="#" ><telerik:RadButton style="margin-left:5px;margin-right:5px;border:0;padding-left:5px; padding-right:0;" ID="btnRequest" runat="server" Text="Request" OnClientClicked="fn_RequestClicked" OnClick="btnRequest_Click" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" Skin="cssType1"   ></telerik:RadButton></a>
            <a href="#" ><telerik:RadButton style="margin-left:5px;margin-right:5px;border:0;padding-left:5px; padding-right:0;" ID="btnApproval" runat="server" Text="Approval" OnClientClicked="fn_ApprovalClicked" OnClick="btnApproval_Click" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" Skin="cssType1"></telerik:RadButton></a>
            <a href="#" ><telerik:RadButton style="margin-left:5px;margin-right:5px;border:0;padding-left:5px; padding-right:0;" ID="btnForwardApproval" runat="server" Text="Approval&amp;Forward" OnClientClicked="fn_FowardApprovalClicked" OnClick="btnForwardApproval_Click" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" ButtonType="LinkButton" Skin="cssType1"></telerik:RadButton></a>
            <a href="#" ><telerik:RadButton style="margin-left:5px;margin-right:5px;border:0;padding-left:5px; padding-right:0;" ID="btnReject" runat="server" Text="Reject" OnClientClicked="fn_RejectClicked" OnClick="btnReject_Click" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" Skin="cssType1"></telerik:RadButton></a>
            <a href="#" ><telerik:RadButton style="margin-left:5px;margin-right:5px;border:0;padding-left:5px; padding-right:0;" ID="btnForward" runat="server" Text="Forward" OnClientClicked="fn_FowardClicked" OnClick="btnForward_Click" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" Skin="cssType1"></telerik:RadButton></a>
            <a href="#" ><telerik:RadButton style="margin-left:5px;margin-right:5px;border:0;padding-left:5px; padding-right:0;" ID="btnRecall" runat="server" Text="Recall" OnClientClicking="fn_RecallClicked" OnClick="btnRecall_Click" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" Skin="cssType1"></telerik:RadButton></a>
            <a href="#" ><telerik:RadButton style="margin-left:5px;margin-right:5px;border:0;padding-left:5px; padding-right:0;" ID="btnWithdraw" runat="server" Text="Withdraw" OnClientClicked="fn_WithdrawClicked" OnClick="btnWithdraw_Click" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" Skin="cssType1"></telerik:RadButton></a>
            <a href="#" ><telerik:RadButton style="margin-left:5px;margin-right:5px;border:0;padding-left:5px; padding-right:0;" ID="btnRemind" runat="server" Text="Remind" OnClientClicked="fn_RemindClicked" OnClick="btnRemind_Click" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" Skin="cssType1"></telerik:RadButton></a>
            <a href="#" ><telerik:RadButton style="margin-left:5px;margin-right:5px;border:0;padding-left:5px; padding-right:0;" ID="btnExit" runat="server" Text="Exit" OnClientClicked="fn_ExitClicked" OnClick="btnExit_Click" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" Skin="cssType1"></telerik:RadButton></a>
            <a href="#" ><telerik:RadButton style="margin-left:5px;margin-right:5px;border:0;padding-left:5px; padding-right:0;" ID="btnSave" runat="server" Text="Save" OnClientClicked="fn_SaveClicked" OnClick="btnSave_Click" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" Skin="cssType1"></telerik:RadButton></a>
            <a href="#" ><telerik:RadButton style="margin-left:5px;margin-right:5px;border:0;padding-left:5px; padding-right:0;" ID="btnInputCommand" runat="server" Text="Input Comment" OnClientClicked="fn_InputCommentClicked" OnClick="btnInputCommand_Click" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" Skin="cssType1"></telerik:RadButton></a>
            <a href="#" ><telerik:RadButton style="margin-left:5px;margin-right:5px;border:0;padding-left:5px; padding-right:0;" ID="btnReUse" runat="server" Text="ReUse" OnClick="btnReUse_Click" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" Skin="cssType1"></telerik:RadButton></a>
        </li>
        
        <li style="float:right;padding-right:5px">
            <a href="#" ><asp:ImageButton runat="server" ID="btnHelp" ImageUrl="~/Styles/images/Common/Help.png" Width="20px" Height="20px" OnClientClick="return fn_HelpClicked();" BorderWidth="0"/></a>
        </li>
        <li style="float:right;padding-right:5px;margin-top:-9px;">
            <img src="/eWorks/Styles/images/ic_image/restricted.png" >
        </li>
    </ul>
</div>
<div id="doc_content" >
    <div class="doc_title">
        <h2><span id="hspanTitle" runat="server">Title</span></h2>
        <p></p>
    </div>
    <div class="data_type1">
        <table >
            <colgroup>
                <col style="width: 25%;" />
                <col />
                <col style="width: 25%;" />
                <col style="width: 25%;" />
            </colgroup>
            <tbody>
                <tr>
                    <th>Requester</th>
                    <td colspan="3"><span id="hspanRequester" runat="server"></span></td>
                </tr>
                <tr>
                    <th>Company</th>
                    <td><span id="hspanCompany" runat="server"></span></td>
                    <th>Organization</th>
                    <td><span id="hspanOrganization" runat="server"></span></td>
                </tr>
                <tr>
                    <th>Retention Period</th>
                    <td><span id="hspanLifeCycle" runat="server"></span></td>
                    <th>Document No.</th>
                    <td><span id="hspanDocumentNo" runat="server"></span></td>
                </tr>
                <tr id="rowAdditionalApproval" runat="server">
                    <th>Additional Approval</th>
                    <td colspan="3">
                        <uc1:UserAutoCompleteBox runat="server" ID="ucAdditionalApprover" ApprovalType="A" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>


