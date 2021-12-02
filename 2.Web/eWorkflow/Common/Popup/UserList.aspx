<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="UserList.aspx.cs" Inherits="Common_Popup_UserList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function returnToParent(sender, args) {
            var gridSelectedItems = $find("<%= RadGridUserList.ClientID %>").get_masterTableView().get_selectedItems();

            var retVal = false;
            if (gridSelectedItems.length == 0) {
                //alert("선택한 항목이 없습니다.");
                fn_OpenDocInformation('선택한 항목이 없습니다.');

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
                    item.COST_CENTER = getDataItemKeyValue($find("<%= RadGridUserList.ClientID  %>"), row, 'COST_CENTER');
                    item.FORM_OF_ADDRESS = getDataItemKeyValue($find("<%= RadGridUserList.ClientID  %>"), row, 'FORM_OF_ADDRESS');
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
        function GetRadWindow1() {
            
            

            var oWindow = null;
            if (window.radWindow)
                oWindow = window.radWindow;
            else if (window.frameElement.radWindow){
                oWindow = window.frameElement.radWindow;               
            }
            return oWindow;
        }

        // Cancel 창닫기
        function cancelAndClose(sender, args) {
            var oWindow = GetRadWindow();
            oWindow.close();
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
            item.COST_CENTER = row.get_cell("COST_CENTER").innerText;
            item.FORM_OF_ADDRESS = row.get_cell("FORM_OF_ADDRESS").innerText;

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
        <div id="popup_content">
            <div class="align_right">
                <telerik:RadTextBox ID="radTxtKeyword" runat="server" ClientEvents-OnKeyPress="onEnter"></telerik:RadTextBox>
                <telerik:RadButton ID="radBtnSearch" runat="server" Text="Search"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-gray btn-size2 bold"
                    OnClick="radBtnSearch_Click">
                </telerik:RadButton>
            </div>
            <div class="data_type2 pt20">
                <telerik:RadGrid runat="server" ID="RadGridUserList" AllowPaging="true" AllowSorting="true" AllowMultiRowSelection="false"
                    AutoGenerateColumns="false" OnNeedDataSource="RadGridUserList_NeedDataSource" Skin="EXGrid" ItemStyle-Wrap="false" GridLines="None">
                    <HeaderStyle HorizontalAlign="Left" />
                    <MasterTableView ShowHeadersWhenNoRecords="true">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderStyle-Width="20px" UniqueName="chkYN" Display="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkboxbody" runat="server" OnCheckedChanged="ToggleRowSelection" AutoPostBack="true" />
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="headerChkbox" runat="server" OnCheckedChanged="ToggleSelectedState" AutoPostBack="true" />
                                </HeaderTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="USER_ID" HeaderText="ID" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FULL_NAME" HeaderText="NAME" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="COMPANY_NAME" HeaderText="Company" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MAIL_ADDRESS" HeaderText="e-Mail" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ORG_ACRONYM" HeaderText="Org" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="IPIN" HeaderText="EmpNo." Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="COST_CENTER" HeaderText="CostCenter" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FORM_OF_ADDRESS" HeaderText="" Display="false"></telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <Selecting AllowRowSelect="true" />
                        <ClientEvents OnRowDblClick="fn_OnRowDblClick" />
                    </ClientSettings>
                    <PagerStyle PageSizeControlType="None" />
                </telerik:RadGrid>
            </div>
            <div class="align_center pt20">
                <telerik:RadButton ID="radBtnOk" runat="server" Text="OK"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
                    CssClass="btn btn-blue btn-size3 bold"
                    OnClientClicked="returnToParent">
                </telerik:RadButton>
                <telerik:RadButton ID="radBtnClose" runat="server" Text="Close"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
                    CssClass="btn btn-darkgray btn-size3 bold"
                    OnClientClicked="cancelAndClose">
                </telerik:RadButton>
            </div>

        </div>
    </div>
    <input type="hidden" runat="server" id="hddSearch" />
</asp:Content>

