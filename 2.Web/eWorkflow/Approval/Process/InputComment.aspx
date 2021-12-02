<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="InputComment.aspx.cs" Inherits="Approval_Process_InputComment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <style type="text/css">
        div.RadListBox .rlbTemplate {
            display: inline-block;
            width: 90%;
            margin-top: 0px;
            margin-bottom: 0px;
            padding-top: 3px;
            vertical-align: middle;
        }
    </style>
    <script src="/eWorks/Scripts/Common/InterFaseService.js"></script>
    <script type="text/javascript">

        function fn_OnClientClicked(sender, args) {
            var listSelectdItems = $find('<%= radListApprover.ClientID%>').get_checkedItems();
            var targetUsers = '';
            for (var i = 0; i < listSelectdItems.length; i++) {
                var item = listSelectdItems[i];
                var userId = $telerik.findElement(item.get_element(), 'lblApproverId').innerText;
                targetUsers += userId + ',';
            }
            $('#<%= hddSelectedId.ClientID%>').val(targetUsers);
        }

        function fn_CancelClick(sender, args) {
            var oArg = new Object();
            oArg.returnValue = false;
            var oWnd = GetRadWindow();

            if (oArg.returnValue) {
                oWnd.close(oArg);
            }
            else
                oWnd.close();
        }

        function fn_ClientFilesUploaded(sender, args) {

            $find('<%=ajaxMgrUpload.ClientID %>').ajaxRequest();

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" runat="Server">
    <telerik:RadAjaxManager ID="ajaxMgrUpload" runat="server" EnablePageHeadUpdate="false">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ajaxMgrUpload">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="asyncImageUpload" />
                    <telerik:AjaxUpdatedControl ControlID="hddAttachFiles" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="data_type1">
        <table>
            <tbody>
                <tr>
                    <th>comment</th>
                </tr>
                <tr>
                    <td>
                        <telerik:RadTextBox TextMode="MultiLine" Height="100" Width="100%" ID="txtComment" runat="server"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <th>Attachment</th>
                </tr>
                <tr>
                    <td>
                        <telerik:RadAsyncUpload ID="asyncUpload" runat="server"
                            OnClientFilesUploaded="fn_ClientFilesUploaded" OnFileUploaded="asyncUpload_FileUploaded"
                            AutoAddFileInputs="false" Localization-Select="파일선택" MultipleFileSelection="Disabled" />
                    </td>
                </tr>
                <tr>
                    <th>To Mail  &nbsp;<font size="1" color="blue"> *Send the notification mail to whom is selected in below list.</font></th>
                </tr>
                <tr>
                    <td>
                        <telerik:RadListBox ID="radListApprover" runat="server" Width="100%" Height="130px"
                            CheckBoxes="true" ShowCheckAll="true" OnItemDataBound="radListApprover_ItemDataBound">

                            <ItemTemplate>

                                <asp:Label ID="lblApprover" runat="server" Width="40%" Style="float: left;"><%# Eval("APPROVER") %></asp:Label>
                                <asp:Label ID="lblApproverOrg" runat="server" Width="60%" Style="float: left;"> <%# Eval("APPROVER_ORG_NAME") %></asp:Label>
                                <asp:Label ID="lblApproverId" runat="server" style="display:none"><%# Eval("APPROVER_ID") %></asp:Label>
                            </ItemTemplate>
                        </telerik:RadListBox>
                    </td>
                </tr>
            </tbody>
        </table>

    </div>
    <div class="align_center pt20">
        <telerik:RadButton ID="btnSaved" CssClass="btn btn-blue btn-size3 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
            OnClientClicked="fn_OnClientClicked" OnClick="btnSaved_Click" Text="Save" runat="server">
        </telerik:RadButton>
        <telerik:RadButton ID="btnCancel" CssClass="btn btn-darkgray btn-size3 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" OnClientClicked="fn_CancelClick" Text="Cancel" runat="server"></telerik:RadButton>
    </div>

    <input id="hddProcessID" type="hidden" runat="server" />
    <input id="hddDocumentID" type="hidden" runat="server" />
    <input id="hddUploadFolder" type="hidden" runat="server" />
    <input id="hddAttachFiles" type="hidden" runat="server" />
    <input id="hddSelectedId" type="hidden" runat="server" />

</asp:Content>

