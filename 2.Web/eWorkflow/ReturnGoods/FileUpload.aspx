<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Sub_ReturnGoods.master" AutoEventWireup="true" CodeFile="FileUpload.aspx.cs" Inherits="ReturnGoods_FileUpload" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function onAttachFileUploaded() {
            $find('<%=RadAjaxManager1.ClientID%>').ajaxRequest("ExcelUpload");
        }

        function onAttachFileRemoved() {
            $find('<%=RadAjaxManager1.ClientID%>').ajaxRequest("GridClear");
        }

        function fn_UpdateData(sender, args) {
            var upload = $find("<%= RadAsyncUpload.ClientID %>");
            var grid = $find("<%= grdExcelFile.ClientID %>");

            if (upload._selectedFilesCount > 0)
                sender.set_autoPostBack(true);
            else
                sender.set_autoPostBack(false);
        }

        function fn_DeleteExcelData(sender, args) {
            var grid = $find("<%= grdExcelFile.ClientID %>");

            if (grid._selectedIndexes.length < 1) {
                fn_OpenInformation("삭제할 항목을 선택해주세요.");
                sender.set_autoPostBack(false);
                return;
            }
            else {
                sender.set_autoPostBack(true);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdExcelFile" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdExcelFile">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdExcelFile" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnDeleteData">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdExcelFile" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnUpdateExcel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAsyncUpload" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="grdExcelFile" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="Server">
    <h2>File Upload</h2>
    <div class="returnGoods_btn">      
        <ul>
             <li style="float: left; margin-left: 5px">
                <telerik:RadAsyncUpload ID="RadAsyncUpload" runat="server" MaxFileInputsCount="1" AllowedFileExtensions=".xls,.xlsx" MultipleFileSelection="Automatic" HideFileInput="true" ManualUpload="false" OnClientFilesUploaded="onAttachFileUploaded" OnClientFileUploadRemoved="onAttachFileRemoved" Width="65px" />
            </li>
            <li style="float: left; margin-left: 5px;">
                <telerik:RadButton ID="btnDeleteData" runat="server" Text="DELETE" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-gray btn-size2 bold" OnClientClicked="fn_DeleteExcelData" OnClick="btnDeleteData_Click"></telerik:RadButton>
            </li>
            <li style="float: right; margin-left: 5px;">
                <telerik:RadButton ID="btnUpdateExcel" runat="server" Text="UPDATE" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-gray btn-size2 bold" OnClientClicked="fn_UpdateData" OnClick="btnUpdateExcel_Click"></telerik:RadButton>
            </li>
        </ul>
    </div>
    <div class="returnGoods_list">
        <telerik:RadGrid ID="grdExcelFile" runat="server" OnItemDataBound="grdExcelFile_ItemDataBound" OnNeedDataSource="grdExcelFile_NeedDataSource" AutoGenerateColumns="False" AllowPaging="False" AllowSorting="true" GridLines="None" AllowMultiRowSelection="true"
            Skin="EXGrid" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false">
            <MasterTableView ShowHeadersWhenNoRecords="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" AlternatingItemStyle-Wrap="false" TableLayout="Fixed">
                <HeaderStyle Font-Size="8px" />
                <ItemStyle Font-Size="8px" />
                <AlternatingItemStyle Font-Size="8px" />
                <Columns>
                    <telerik:GridClientSelectColumn UniqueName="selChk" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridClientSelectColumn>
                    <telerik:GridBoundColumn HeaderText="IDX" DataField="IDX" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="SN" DataField="SN" HeaderStyle-Width="40px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Date" DataField="Date" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-Width="80px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Div" DataField="Div" HeaderStyle-Width="40px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="CUSTOMER CODE" DataField="CUSTOMER_CODE" Display="false" >
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="CUSTOMER" DataField="CUSTOMER_NAME" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="PRODUCT CODE" DataField="PRODUCT_CODE" Display="false" >
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="PRODUCT" DataField="PRODUCT_NAME" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Batch" DataField="Batch" HeaderStyle-Width="80px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Qty" DataField="Qty" HeaderStyle-Width="40px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Expiry" DataField="Expiry" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-Width="80px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="S1" DataField="s1" HeaderStyle-Width="30px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="S2" DataField="s2" HeaderStyle-Width="30px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="REASON(Only AH)" DataField="REASON" HeaderStyle-Width="80px" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="VC_MANAGER(Only AN)" DataField="VC_MANAGER" HeaderStyle-Width="80px" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="S. Admin" DataField="SALES_ADMIN" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="W. Manger" DataField="WHOLESALES_MANAGER" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="W. Spec." DataField="WHOLESALES_SPECIALIST" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" />
            <AlternatingItemStyle HorizontalAlign="Left" />
            <ClientSettings>
                <Selecting AllowRowSelect="true" />
                <Scrolling SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="5" />
            </ClientSettings>
        </telerik:RadGrid>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="Server">
</asp:Content>

