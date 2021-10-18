<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="RejectList.aspx.cs" Inherits="Approval_List_RejectList" %>
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

        function fn_OpenDetailSearch(sender, eventArgs) {
            var wnd = $find("<%= radWinDetailSearch.ClientID %>");

            wnd.setUrl("/eWorks/Common/Popup/DetailSearch.aspx");
            wnd.show();
            sender.set_autoPostBack(false);
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
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" Runat="Server">
     <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server" ></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="ajaxMgr" runat="server" OnAjaxRequest="Approval_List_RejectList_AjaxRequest">
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
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" Runat="Server">
    <div id="content">
        <h2>Reject</h2>
        <div class="align_right pb10" style="display: none;">
            <a class="btn btn-blue btn-size1 bold" href="#">상세검색</a>
        </div>
        <div class="align_right" style="display: none;">
            <uc1:SearchBar runat="server" ID="SearchBar" />
            <telerik:RadButton ID="btnSearch" Text="Search" CssClass="btn btn-gray btn-size2 bold" ButtonType="ToggleButton" runat="server" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" OnClick="btnSearch_Click"></telerik:RadButton>
        </div>
        <div class="align_right">
            <telerik:RadButton ID="btnDetailSearch" Text="Detail Search" CssClass="btn btn-gray btn-size2 bold" ButtonType="ToggleButton" runat="server" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" OnClientClicked="fn_OpenDetailSearch" OnClick="btnSearch_Click"></telerik:RadButton>
        </div>
        <div class="board_list pt20" style="min-height:300px">
            <telerik:RadGrid ID="grdSearch" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true"  EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid"  GridLines="None" AllowMultiRowSelection="false" OnItemDataBound="grdSearch_ItemDataBound" OnNeedDataSource="grdSearch_NeedDataSource">
                <MasterTableView ShowHeadersWhenNoRecords="true" ItemStyle-Wrap="false" TableLayout="Fixed">
                    <Columns>
                        <telerik:GridBoundColumn Display="false" DataField="PROCESS_ID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn Display="false" DataField="DOCUMENT_ID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn Display="false" DataField="FORM_NAME">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn Display="false" DataField="PROCESS_STATUS">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Req Date" DataField="REQUEST_DATE" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Attach" Display="false" >
                            <ItemTemplate>
                                <asp:Image ID="iconAttach" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comment" Display="false" >
                            <ItemTemplate>
                                <asp:Image ID="iconComment" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Title" DataField="DOC_NAME" HeaderStyle-Width="40%" ItemStyle-Width="40%" >
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Subject" DataField="SUBJECT" HeaderStyle-Width="60%" ItemStyle-Width="60%" >
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Requester" DataField="REQUESTER" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Rejecter" DataField="CURRENT_APPROVER_NAME" HeaderStyle-Width="120px" ItemStyle-Width="120px" >
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
    </div>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinDetailSearch" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Detail Search" Width="520px" Height="660px" Behaviors="Default" NavigateUrl="./DetailSearch.aspx" OnClientClose="fn_SetItem"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <input type="hidden" id="hddPoppupItem" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" Runat="Server">
</asp:Content>


