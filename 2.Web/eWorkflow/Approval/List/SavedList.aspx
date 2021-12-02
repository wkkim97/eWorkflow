<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="SavedList.aspx.cs" Inherits="Approval_List_SavedList" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Common/SearchBar.ascx" TagPrefix="uc1" TagName="SearchBar" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" Runat="Server">
    <script type="text/javascript">

        function FormLoad()
        {
            fn_InitControl();
        }

        function fn_ReadOnClick(sender, args) {
            var strUrl = "";
            var bAuth = null;

            try {
                var grid = $find("<%= grdSearch.ClientID %>");
                var formName = getDataItemKeyValue(grid, grid.get_masterTableView().get_dataItems()[sender._selectedIndexes], 'FORM_NAME');
                var processid = getDataItemKeyValue(grid, grid.get_masterTableView().get_dataItems()[sender._selectedIndexes], 'PROCESS_ID');
                var documentid = getDataItemKeyValue(grid, grid.get_masterTableView().get_dataItems()[sender._selectedIndexes], 'DOCUMENT_ID');

                fn_ShowDocument(formName, documentid, processid);
            }
            catch (exception) {
                fn_OpenErrorMessage(exception.description);
            }
        }

        function fn_ConfirmDelete(sender, args) {

            //args.set_cancel(!fn_OpenConfirm('Do you want to delete this Item ?'));
            var callBackFunction = Function.createDelegate(sender, function (argument) {

                if (argument) {

                    this.click();

                }

            });
            fn_OpenConfirm("Do you want to delete this Item ?", callBackFunction);

            args.set_cancel(true);
        }
    </script>
    <script src="/eWorks/Scripts/Approval/List.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" Runat="Server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="ajaxMgr" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSearch" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSearch" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnDelete">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSearch" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" Runat="Server"> 
 	<!-- content -->
	<div id="content">
        <h2>Not Submitted</h2>
        <div class="align_right" style="display:none;" >
            <a class="btn btn-blue btn-size1 bold" href="#">Delete</a>
        </div>
        <div class="align_right" style="display:none;">
            <uc1:SearchBar runat="server" ID="SearchBar" />
            <telerik:RadButton ID="btnSearch" Text="Search" CssClass="btn btn-gray btn-size2 bold" ButtonType="ToggleButton" runat="server" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" OnClick="btnSearch_Click"></telerik:RadButton>
        </div>
        <div class="align_right">
            <telerik:RadButton ID="btnDelete" Text="Delete" CssClass="btn btn-gray btn-size2 bold" ButtonType="ToggleButton" runat="server" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" OnClientClicking="fn_ConfirmDelete" OnClick="btnDelete_Click"></telerik:RadButton>
        </div>

    <div class="board_list pt20" style="min-height:300px">
        <telerik:RadGrid ID="grdSearch" runat="server" AutoGenerateColumns="False" AllowPaging="True"  EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid"  AllowSorting="true"  GridLines="None" AllowMultiRowSelection="true"  OnNeedDataSource="grdSearch_NeedDataSource"
            >
            <MasterTableView ShowHeadersWhenNoRecords="true" ItemStyle-Wrap="false" TableLayout="Fixed" >
                <Columns> 
                    <telerik:GridClientSelectColumn UniqueName="selChk" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridClientSelectColumn>
                    <telerik:GridBoundColumn Display="false" DataField="PROCESS_ID">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn Display="false" DataField="DOCUMENT_ID">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn Display="false" DataField="FORM_NAME">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn Display="false" DataField="PROCESS_STATUS">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Req Date" DataField="REQUEST_DATE" DataFormatString="{0:yyyy-MM-dd}"   HeaderStyle-Width="80px" ItemStyle-Width="80px" >
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Title" DataField="DOC_NAME"  HeaderStyle-Width="40%" ItemStyle-Width="40%">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Subject" DataField="SUBJECT" HeaderStyle-Width="60%" ItemStyle-Width="60%">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Requester" DataField="REQUESTER" HeaderStyle-Width="120px" ItemStyle-Width="120px" >
                    </telerik:GridBoundColumn> 
                </Columns>
            </MasterTableView>
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle Wrap="false" />
            <ClientSettings>
                <Selecting AllowRowSelect="true" />
                <ClientEvents OnRowDblClick="fn_ReadOnClick"></ClientEvents>
            </ClientSettings>
        </telerik:RadGrid>
    </div>
 </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" Runat="Server">
</asp:Content>


