<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PopupUserList.aspx.cs" Inherits="Common_Popup_PopupUserList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="../../Scripts/ControlUtil.js"></script>
    <script src="../../Scripts/Common.min.js"></script>
    <title>User List</title>

    <telerik:RadCodeBlock runat="server" ID="rdbScripts">
        <script type="text/javascript">

            //체크 한 리스트값을 부모로 돌려준다.
            function returnToParent(sender, args) {
                var gridSelectedItems = $find("<%= RadGridUserList.ClientID %>").get_masterTableView().get_selectedItems();

                var retVal = false;
                if (gridSelectedItems.length == 0) {
                    //alert("선택한 항목이 없습니다.");
                    fn_OpenInformation('선택한 항목이 없습니다.');

                }
                else {

                    var uList = [];
                    for (var i = 0; i < gridSelectedItems.length; i++) {
                        var row = gridSelectedItems[i];
                        var item = {};
                        item.USER_ID = getDataItemKeyValue($find("<%= RadGridUserList.ClientID  %>"), row, 'USER_ID');
                        item.FULL_NAME = getDataItemKeyValue($find("<%= RadGridUserList.ClientID  %>"), row, 'FULL_NAME');
                        item.COMPANY_NAME = getDataItemKeyValue($find("<%= RadGridUserList.ClientID  %>"), row, 'COMPANY_NAME');
                        item.MAIL_ADDRESS = getDataItemKeyValue($find("<%= RadGridUserList.ClientID  %>"), row, 'MAIL_ADDRESS');
                        item.ORG_ACRONYM = getDataItemKeyValue($find("<%= RadGridUserList.ClientID  %>"), row, 'ORG_ACRONYM');
                        item.IPIN = getDataItemKeyValue($find("<%= RadGridUserList.ClientID  %>"), row, 'IPIN');
                        uList.push(item);
                    }
                    uListData = JSON.stringify(uList);
                    // parent.setSelectData(JSON.parse(uListData));
                    GetRadWindow().close(JSON.parse(uListData));
                }

            }

            function getDataItemKeyValue(radGrid, item, colName) {
                return radGrid.get_masterTableView().getCellByColumnUniqueName(item, colName).innerHTML;
            }
            function fn_getUserList() {
                return JSON.parse(fn_GetHtmlObject("hddUserList").value);
            }

            // 부모 자식 의 radwindow 값 을 돌려준다
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow)
                    oWindow = window.radWindow;
                else if (window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            // Cancel 창닫기
            function cancelAndClose(sender, args) {
                var oWindow = GetRadWindow();
                oWindow.close();
            }

            // Key Event
            function OnKeyPress(sender, eventArgs) {
                var c = eventArgs.get_keyCode();
                if (c == "13") {
                    $find("<%= RadbtnSearch.ClientID %>").click();
                }
            }

            function fn_OnRowDblClick(sender, args) {
                var gridSelectedItems = $find("<%= RadGridUserList.ClientID %>").get_masterTableView().get_selectedItems();
                var uList = [];
                var row = args.get_gridDataItem();
                var item = {};

                item.USER_ID = row.get_cell("USER_ID").innerHTML;
                item.FULL_NAME = row.get_cell("FULL_NAME").innerHTML;
                item.COMPANY_NAME = row.get_cell("COMPANY_NAME").innerHTML;
                item.MAIL_ADDRESS = row.get_cell("MAIL_ADDRESS").innerHTML;
                item.ORG_ACRONYM = row.get_cell("ORG_ACRONYM").innerHTML;
                item.IPIN = row.get_cell("IPIN").innerHTML;

                uList.push(item);
                uListData = JSON.stringify(uList);
                //parent.setSelectedCompany(JSON.parse(uListData));
                GetRadWindow().close(JSON.parse(uListData));
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadFormDecorator ID="QsfFromDecorator" runat="server" DecoratedControls="All" EnableRoundedCorners="false" Skin="Metro" />
            <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>

            <telerik:RadAjaxManager runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="RadGridUserList">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGridUserList" LoadingPanelID="RadAjaxLoadingPanel1" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>

            <div>
                <span>
                    <span>
                        <asp:Label runat="server" Text="USER NAME SEARCH : " ID="lbSerach"></asp:Label></span>
                    <telerik:RadTextBox runat="server" ID="RadSearchBox" Width="200">
                        <ClientEvents OnKeyPress="OnKeyPress" />
                    </telerik:RadTextBox>
                    <telerik:RadButton runat="server" ID="RadbtnSearch" ButtonType="LinkButton" Text="SEARCH" OnClick="RadbtnSearch_Click"></telerik:RadButton>
                </span>
            </div>
            <p />
            <div>
                <telerik:RadGrid runat="server" ID="RadGridUserList" AllowPaging="true" AllowSorting="true" AllowMultiRowSelection="false" AutoGenerateColumns="false" Culture="ko-KR" OnNeedDataSource="RadGridUserList_NeedDataSource" OnPageIndexChanged="RadGridUserList_PageIndexChanged" OnPageSizeChanged="RadGridUserList_PageSizeChanged" OnSortCommand="RadGridUserList_SortCommand" Skin="EXGrid">
                    <MasterTableView ShowHeadersWhenNoRecords="true" DataKeyNames="FULL_NAME" EnableHeaderContextMenu="true" CommandItemDisplay="Top" CommandItemStyle-Height="30">
                        <CommandItemTemplate>
                            <telerik:RadButton ID="RadbtnMultiCheck" runat="server" Text=" Multi User Select" ToggleType="CheckBox" ButtonType="ToggleButton" Visible="false"
                                OnCheckedChanged="RadbtnMultiCheck_CheckedChanged">
                            </telerik:RadButton>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderStyle-Width="30px" UniqueName="chkYN" Display="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkboxbody" runat="server" OnCheckedChanged="ToggleRowSelection" AutoPostBack="true" />
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="headerChkbox" runat="server" OnCheckedChanged="ToggleSelectedState" AutoPostBack="true" />
                                </HeaderTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="USER_ID" HeaderText="USER_ID" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FULL_NAME" HeaderText="NAME" HeaderStyle-Width="400px" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="COMPANY_NAME" HeaderText="Company" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MAIL_ADDRESS" HeaderText="e-Mail" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ORG_ACRONYM" HeaderText="Org" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="IPIN" HeaderText="EmpNo." Display="false"></telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" />
                        <ClientEvents OnRowDblClick="fn_OnRowDblClick" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
            <div>
                <p />
                <telerik:RadButton ID="BtnOk" Text="OK" OnClientClicked="returnToParent" runat="server"></telerik:RadButton>
                <telerik:RadButton ID="BtnCancel" Text="Cancel" OnClientClicked="cancelAndClose" runat="server"></telerik:RadButton>
            </div>
        </div>
        <input type="hidden" runat="server" id="hddSearch" />
    </form>

</body>
</html>
