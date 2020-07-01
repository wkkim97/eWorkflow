<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="CollateralList.aspx.cs" Inherits="Approval_List_CollateralList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function FormLoad() {
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

        function onEnter(e) {

            var theKey = 0;
            e = (window.event) ? event : e;
            theKey = (e.keyCode) ? e.keyCode : e.charCode;
            if (theKey == "13") {
                $find("<%=btnSearch.ClientID %>").click();
            }
        }

    </script>
    <script src="/eWorks/Scripts/Approval/List.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="Server">
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
        </AjaxSettings>
    </telerik:RadAjaxManager>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="Server">
    <div id="content">
        <h2>Collateral</h2>
        <div style="display: inline-block; width: 100%">

            <span style="display: inline-block; width: 50px; text-align: right; margin-right: 3px">BG</span><div id="divBG" runat="server" style="display: inline-block"></div>
            <span style="display: inline-block; width: 50px; text-align: right; margin-right: 3px">Type</span><div id="divType" runat="server" style="display: inline-block">
                <telerik:RadButton ID="radBtnTypeCurrently" runat="server" Text="Currently" Value="Currently" AutoPostBack="false"
                    ButtonType="ToggleButton" ToggleType="CheckBox">
                </telerik:RadButton>
                <telerik:RadButton ID="radBtnTypeReturned" runat="server" Text="Returned" Value="Returned" AutoPostBack="false"
                    ButtonType="ToggleButton" ToggleType="CheckBox">
                </telerik:RadButton>
            </div>
            <div style="display: inline-block; float: right">
                <telerik:RadTextBox ID="radTxtKeyWord" runat="server" Width="150px" ClientEvents-OnKeyPress="onEnter"></telerik:RadTextBox>
                <telerik:RadButton ID="btnSearch" Text="Search" CssClass="btn btn-gray btn-size2 bold" ButtonType="ToggleButton" runat="server"
                    EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" OnClick="btnSearch_Click">
                </telerik:RadButton>
                <telerik:RadButton ID="btnExcel" Text="Excel" CssClass="btn btn-gray btn-size2 bold" ButtonType="ToggleButton" runat="server"
                    EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" OnClick="btnExcel_Click">
                </telerik:RadButton>
            </div>
        </div>

        <div class="board_list pt20" style="min-height:300px">
            <telerik:RadGrid ID="grdSearch" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="true" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" GridLines="None" PageSize="300" AllowMultiRowSelection="false" OnNeedDataSource="grdSearch_NeedDataSource"
                ShowGroupPanel="true" CssClass="grid_header" OnGroupsChanging="grdSearch_GroupsChanging">
                <MasterTableView ShowHeadersWhenNoRecords="true" ItemStyle-Wrap="false" TableLayout="Fixed">
                    <HeaderStyle Font-Size="10px" />
                    <ItemStyle Font-Size="10px" />
                    <AlternatingItemStyle Font-Size="10px" />
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="CUSTOMER_NAME" HeaderText="Customer Name" />
                                <telerik:GridGroupByField FieldName="CREDIT_LIMIT" HeaderText="Credit Limit" FormatString="{0:#,##0}" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="CUSTOMER_NAME" SortOrder="Ascending" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>

                    <Columns>
                        <telerik:GridBoundColumn Display="false" DataField="PROCESS_ID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn Display="false" DataField="DOCUMENT_ID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn Display="false" DataField="FORM_NAME">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderText="Customer<br />Code" DataField="CUSTOMER_CODE" HeaderStyle-Width="80px" ItemStyle-Width="80px" Display="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Customer Name" DataField="CUSTOMER_NAME">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="CreditLimit" DataField="CREDIT_LIMIT" HeaderStyle-Width="40px" ItemStyle-Width="40px" DataFormatString="{0:#,##0}" Display="false">
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="BG" DataField="BG_CODE" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Status" DataField="STATUS_CODE" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Mortgage Type " DataField="MORTGAGE_TYPE">
                        </telerik:GridBoundColumn>
                        <telerik:GridNumericColumn HeaderText="Book Value" DataField="BOOK_VALUE" DataFormatString="{0:#,##0}" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridNumericColumn>
                        <telerik:GridNumericColumn HeaderText="Revaluation" DataField="REVALUTION_VALUE" DataFormatString="{0:#,##0}" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn HeaderText="Received<br/>Date" DataField="RECEIVED_DATE" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Due Date" DataField="DUE_DATE" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Issue<br/>Date" DataField="ISSUE_DATE" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Return<br/>Date" DataField="RETURN_DATE" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Doc Number" DataField="DOC_NUM" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn Display="false" HeaderText="CREATE<br/>DATE" DataField="CREATE_DATE" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Publisher" DataField="PUBLISHER" HeaderStyle-Width="80px" ItemStyle-Width="80px" Display="true">
                        </telerik:GridBoundColumn>
						<telerik:GridBoundColumn HeaderText="Publisher<br />Num" DataField="PUBLISHED_NUM" HeaderStyle-Width="80px" ItemStyle-Width="80px" Display="true" >
                        </telerik:GridBoundColumn>

                    </Columns>
                </MasterTableView>
                <HeaderStyle HorizontalAlign="Left" />
                <ClientSettings AllowDragToGroup="true">
                    <Selecting AllowRowSelect="true" />
                    <ClientEvents OnRowDblClick="fn_ReadOnClick"></ClientEvents>
                </ClientSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            </telerik:RadGrid>
        </div>
    </div>
    <%--<telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinDetailSearch" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Detail Search" Width="520px" Height="660px" Behaviors="Default" NavigateUrl="./DetailSearch.aspx" OnClientClose="fn_SetItem"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>--%>
    <input type="hidden" id="hddPoppupItem" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="Server">
</asp:Content>

