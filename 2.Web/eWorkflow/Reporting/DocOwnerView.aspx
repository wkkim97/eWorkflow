<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Sub_Reporting.master" AutoEventWireup="true" CodeFile="DocOwnerView.aspx.cs" Inherits="Reporting_DocOwnerView" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Common/SearchBar.ascx" TagPrefix="uc1" TagName="SearchBar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
     <style>
        input.rgFilterBox {
            border-color: #84AD88;
            font-weight: 100;
            line-height: 20px;
            color: #000;
            height: 20px;
            width: 90%;
            font-family:Arial,dotum;
            font-size:12px;


        }
         </style>
    <script type="text/javascript">
        function FormLoad() {
            fn_InitControl();
        }

        function fn_OpenDetailSearch(sender, eventArgs) {
            var wnd = $find("<%= radWinDetailSearch.ClientID %>");

            var userid = $("[id$=hddUserId]").val();    
            wnd.setUrl("/eWorks/Common/Popup/DetailSearch.aspx?userid=" + userid);
            wnd.show();
            sender.set_autoPostBack(false);
        }

        function fn_DownloadDoc(sender, eventArgs) {
            var wnd = $find("<%= radWinDetailSearch.ClientID %>");

            var userid = $("[id$=hddUserId]").val();
            wnd.setUrl("/eWorks/Common/Popup/DownLoad_Doc.aspx?userid=" + userid);
            wnd.show();
            sender.set_autoPostBack(false);
        }
        function fn_ReadOnClick(sender, args) {
            var strUrl = "";
            var bAuth = null;

            try {
                var grid = $find("<%= grdSearch.ClientID %>");
                var formName = getDataItemKeyValue(grid, grid.get_masterTableView().get_dataItems()[sender._selectedIndexes], 'FORM_NAME');
                var processid = getDataItemKeyValue(grid, grid.get_masterTableView().get_dataItems()[sender._selectedIndexes], 'PROCESS_ID');
                var documentid = getDataItemKeyValue(grid, grid.get_masterTableView().get_dataItems()[sender._selectedIndexes], 'DOCUMENT_ID');
                var status = getDataItemKeyValue(grid, grid.get_masterTableView().get_dataItems()[sender._selectedIndexes], 'PROCESS_STATUS');

                if (status != 'Saved')
                    fn_ShowDocument(formName, documentid, processid);
                else
                    fn_OpenDocInformation('Request이후부터 조회 가능합니다.');
            }
            catch (exception) {
                fn_OpenErrorMessage(exception.description);
            }
        }

        function fn_SetItem(oWnd, args) {
            var item = args.get_argument();
            if (item != null) {
                $('#<%= hddPoppupItem.ClientID%>').val(JSON.stringify(item));
                $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("Rebind");
            }
        }

    </script>
    <script src="/eWorks/Scripts/Approval/List.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="ajaxMgr" runat="server" OnAjaxRequest="ajaxMgr_AjaxRequest">
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
            <telerik:AjaxSetting AjaxControlID="ajaxMgr">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSearch" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="Server">

    <h2>Admin View</h2>
    <div class="align_right pb10" style="display: none;">
        <a class="btn btn-blue btn-size1 bold" href="#">상세검색</a>
    </div>
    <div class="align_right" style="display: none;">
        <uc1:SearchBar runat="server" ID="SearchBar" />
        <telerik:RadButton ID="btnSearch" Text="Search" CssClass="btn btn-gray btn-size2 bold" ButtonType="ToggleButton" runat="server" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" OnClick="btnSearch_Click"></telerik:RadButton>
    </div>
    <div class="reporting_btn">
        <ul>
            <li style="float: right;">
                <telerik:RadButton ID="btnDetailSearch" Text="Detail Search" CssClass="btn btn-gray btn-size2 bold" ButtonType="ToggleButton" runat="server"
                    EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" OnClientClicked="fn_OpenDetailSearch" OnClick="btnSearch_Click">
                </telerik:RadButton>
                
                <telerik:RadButton ID="btnDownload" runat="server" Text="ExcelDownload" CssClass="btn btn-gray btn-size2 bold" 
                    EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="ToggleButton"  OnClick="btnExcel_Click"></telerik:RadButton>
                <telerik:RadButton ID="RadButton1" Text="Detail ExcelDownLoad" CssClass="btn btn-gray btn-size2 bold" ButtonType="ToggleButton" runat="server"
                    EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" OnClientClicked="fn_DownloadDoc" OnClick="btnREPORT_Click">
                </telerik:RadButton>
                
               
            </li>
            
        </ul>

    </div>
    <div style="min-height: 300px">
        <telerik:RadGrid ID="grdSearch" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true"
            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" GridLines="None" AllowMultiRowSelection="false"
            OnItemDataBound="grdSearch_ItemDataBound" OnNeedDataSource="grdSearch_NeedDataSource" AllowFilteringByColumn="true" >
            <GroupingSettings CaseSensitive="false" />

            <MasterTableView ShowHeadersWhenNoRecords="true" ItemStyle-Wrap="false" TableLayout="Fixed">
                <Columns>
                    <telerik:GridBoundColumn Display="false" DataField="PROCESS_ID" AllowFiltering ="false" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn Display="false" DataField="DOCUMENT_ID" AllowFiltering ="false" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn Display="false" DataField="FORM_NAME" AllowFiltering ="false" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Req Date" DataField="REQUEST_DATE" AllowFiltering ="false"  DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-Width="80px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Status" DataField="PROCESS_STATUS" AllowFiltering ="false" HeaderStyle-Width="80px">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Comment" Display="false" AllowFiltering ="false" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Image ID="iconComment" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="Title" DataField="DOC_NAME" AllowFiltering ="false" HeaderStyle-Width="150px" >
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Subject" DataField="SUBJECT" HeaderStyle-Width="200px" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                            ShowFilterIcon="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Requester" DataField="REQUESTER" HeaderStyle-Width="120px" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                            ShowFilterIcon="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Current Approver" DataField="CURRENT_APPROVER_NAME" AllowFiltering ="false" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Final Approver" DataField="FINAL_APPROVER_NAME" AllowFiltering ="false" HeaderStyle-Width="120px" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Doc No." Display="true" DataField="DOC_NUM" HeaderStyle-Width="90px" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                            ShowFilterIcon="false">
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
            <HeaderStyle HorizontalAlign="Left" />
            <ClientSettings>
                <Selecting AllowRowSelect="true" />
                <ClientEvents OnRowDblClick="fn_ReadOnClick"></ClientEvents>
            </ClientSettings>
        </telerik:RadGrid>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="Server">
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinDetailSearch" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Detail Search" Width="520px" Height="660px" Behaviors="Default" NavigateUrl="./DetailSearch.aspx" OnClientClose="fn_SetItem"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <input type="hidden" id="hddPoppupItem" runat="server" />
</asp:Content>

