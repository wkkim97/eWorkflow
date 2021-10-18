<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="Board_Edit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
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

    <div class="align_left">
        <telerik:RadButton ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-blue btn-size1 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Text="Save" runat="server"></telerik:RadButton>
        <telerik:RadButton ID="btnDelete" OnClick="btnDelete_Click" CssClass="btn btn-blue btn-size1 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Text="Delete" runat="server"></telerik:RadButton>
        <telerik:RadButton ID="btnCancel" OnClientClicked="fn_CancelClick" CssClass="btn btn-darkgray btn-size1 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Text="Cancel" runat="server"></telerik:RadButton>
    </div>
    <div class="data_type3 pt10">
        <table class="style_table">
            <colgroup>
                <col style="width: 20%;" />
                <col style="width: 30%;" />
                <col style="width: 20%;" />
                <col style="width: 30%;" />
            </colgroup>
            <tbody>
                <tr >
                    <th class='style_th'>Register</th>
                    <td class='style_td'><span id="hspanRegister" runat="server"></span></td>
                    <th class='style_th'>Regist Date</th>
                    <td class='style_td'><span id="hspanRegistDate" runat="server"></span></td>
                </tr>
                <tr>
                    <th  class='style_th'>Subject</th>
                    <td   class='style_td' colspan="3">
                        <telerik:RadTextBox ID="txtSubject" runat="server"  Width="100%"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <th  class='style_th'>Body</th>
                    <td  class='style_td' colspan="3">
                        <telerik:RadEditor ID="txtBody" runat="server" Width="100%" Height="300px">
                            <Tools>
                                <telerik:EditorToolGroup>
                                    <telerik:EditorTool Name="RichEditor" Text="Open Advanced Editor"></telerik:EditorTool>
                                    <telerik:EditorTool Name="Bold"></telerik:EditorTool>
                                    <telerik:EditorTool Name="Italic"></telerik:EditorTool>
                                    <telerik:EditorTool Name="Underline"></telerik:EditorTool>
                                    <telerik:EditorTool Name="Cut"></telerik:EditorTool>
                                    <telerik:EditorTool Name="Copy"></telerik:EditorTool>
                                    <telerik:EditorTool Name="Paste"></telerik:EditorTool>
                                    <telerik:EditorTool Name="FontName"></telerik:EditorTool>
                                    <telerik:EditorTool Name="RealFontSize"></telerik:EditorTool>
                                </telerik:EditorToolGroup>
                            </Tools>
                        </telerik:RadEditor>
                    </td>
                </tr>
                <tr>
                    <th  class='style_th'>Attachment</th>
                    <td  class='style_td' colspan="3"><a href="#" id="hrefFIle" runat="server" ></a><telerik:RadAsyncUpload ID="asyncUpload" runat="server"
                            OnClientFilesUploaded="fn_ClientFilesUploaded" OnFileUploaded="asyncUpload_FileUploaded"
                            AutoAddFileInputs="false" Localization-Select="파일선택" MultipleFileSelection="Disabled" /></td>
                </tr>
            </tbody>
        </table>

    </div>

    <input id="hddIDX" type="hidden" runat="server" />
    <input id="hddUploadFolder" type="hidden"  runat="server" /> 
    <input id="hddAttachFiles" type="hidden"  runat="server" /> 
      <iframe id="filedownframe" width="0" height="0"></iframe>
</asp:Content>

