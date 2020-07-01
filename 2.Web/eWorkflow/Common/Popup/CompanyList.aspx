<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="CompanyList.aspx.cs" Inherits="Common_Popup_CompanyList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">

        function pageLoad() {
            var oWnd = GetRadWindow();
            oWnd.set_width(500);
            oWnd.set_height(600);
        }

        function returnToParent(sender, args) {
            var gridSelectedItems = $find("<%= radGrdCompany.ClientID %>").get_masterTableView().get_selectedItems();

            var retVal = false;
            if (gridSelectedItems.length < 1) {
                alert("선택한 항목이 없습니다.");
            }
            else {

                var uList = [];
                for (var i = 0; i < gridSelectedItems.length; i++) {
                    var row = gridSelectedItems[i];
                    var item = {};
                    item.COMPANY_ID = row.get_cell("COMPANY_ID").innerHTML;
                    item.NAME = row.get_cell("NAME").innerHTML;

                    uList.push(item);
                }
                uListData = JSON.stringify(uList);
                //parent.setSelectedCompany(JSON.parse(uListData));
                GetRadWindow().close(JSON.parse(uListData));
            }

        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.radWindow;
            else if (window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function cancelAndClose(sender, args) {
            var oWindow = GetRadWindow();
            oWindow.close();
        }

        function fn_OnRowDblClick(sender, args) {
            var gridSelectedItems = $find("<%= radGrdCompany.ClientID %>").get_masterTableView().get_selectedItems();
            var uList = [];
            var row = args.get_gridDataItem();
            var item = {};
            item.COMPANY_ID = row.get_cell("COMPANY_ID").innerHTML;
            item.NAME = row.get_cell("NAME").innerHTML;

            uList.push(item);
            uListData = JSON.stringify(uList);
            //parent.setSelectedCompany(JSON.parse(uListData));
            GetRadWindow().close(JSON.parse(uListData));
        }

        function onEnter(e) {
            var theKey = 0;
            e = (window.event) ? event : e;
            theKey = (e.keyCode) ? e.keyCode : e.charCode;
            if (theKey == "13") {
                $find("<%=radBtnSearch.ClientID %>").click();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" runat="Server">
    <div id="wrap">
        <div id="pop_content">

            <div class="align_right">
                <telerik:RadTextBox ID="radTxtKeyword" runat="server" ClientEvents-OnKeyPress="onEnter"></telerik:RadTextBox>
                <telerik:RadButton ID="radBtnSearch" runat="server" Text="Search"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-gray btn-size2 bold"
                    OnClick="radBtnSearch_Click">
                </telerik:RadButton>
            </div>
            <div class="data_type2 pt20">
                <telerik:RadGrid ID="radGrdCompany" runat="server" AutoGenerateColumns="false" Height="400px" OnItemDataBound="radGrdCompany_ItemCreated"
                    Skin="EXGrid" OnNeedDataSource="radGrdCompany_NeedDataSource">
                    <HeaderStyle HorizontalAlign="Left" />
                    <MasterTableView>
                        <Columns>
                            <telerik:GridClientSelectColumn HeaderStyle-Width="30px" UniqueName="ClientSelectColumn">
                            </telerik:GridClientSelectColumn>
                            <telerik:GridBoundColumn DataField="COMPANY_ID" HeaderText="Code" HeaderStyle-Width="50px" ItemStyle-Wrap="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NAME" UniqueName="NAME"  HeaderText="Name" ItemStyle-Wrap="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn  DataField="ADDRESS"  HeaderText="Address" ItemStyle-Wrap="true" Display="false"></telerik:GridBoundColumn>
                        </Columns>

                    </MasterTableView>
                    <ClientSettings>
                        <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                        <Selecting AllowRowSelect="true" />
                        <ClientEvents OnRowDblClick="fn_OnRowDblClick" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
            <div class="align_center pt20">
                <telerik:RadButton ID="radBtnOk" runat="server" Text="Ok"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
                    CssClass="btn btn-blue btn-size3 bold" OnClientClicked="returnToParent">
                </telerik:RadButton>
                <telerik:RadButton ID="radBtnCancel" runat="server" Text="Cancel"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
                    CssClass="btn btn-darkgray btn-size3 bold" OnClientClicked="cancelAndClose">
                </telerik:RadButton>
            </div>

        </div>
    </div>
</asp:Content>

