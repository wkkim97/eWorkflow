<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="ApproveList.aspx.cs" Inherits="Approval_List_ApproveList" %>
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
    </script>
    <script src="/eWorks/Scripts/Approval/List.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" Runat="Server">
     <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server" ></telerik:RadAjaxLoadingPanel>
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
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" Runat="Server">
    <div id="content">
        <h2>Pending Approval</h2>
        <div class="align_right" style="display: none;">
            <uc1:SearchBar runat="server" ID="SearchBar" />
            <telerik:RadButton ID="btnSearch" Text="Search" CssClass="btn btn-gray btn-size2 bold" ButtonType="LinkButton" runat="server" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" OnClick="btnSearch_Click" ></telerik:RadButton>
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
                        <telerik:GridTemplateColumn HeaderText="Attach" Display="false" >
                            <ItemTemplate>
                                <asp:Image ID="iconAttach" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comment" Display="false">
                            <ItemTemplate>
                                <asp:Image ID="iconComment" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="Req Date" DataField="REQUEST_DATE" DataFormatString="{0:yyyy-MM-dd}"   HeaderStyle-Width="80px" ItemStyle-Width="80px" >
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn HeaderText="Title" DataField="DOC_NAME"  HeaderStyle-Width="40%" ItemStyle-Width="40%">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Subject" DataField="SUBJECT"  HeaderStyle-Width="60%" ItemStyle-Width="60%">
                    </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Requester" DataField="REQUESTER" HeaderStyle-Width="120px" ItemStyle-Width="120px" >
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Current Approver" DataField="CURRENT_APPROVER_NAME" HeaderStyle-Width="120px" ItemStyle-Width="120px">
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" Runat="Server">
</asp:Content>

