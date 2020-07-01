<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="CustomerListForCS.aspx.cs" Inherits="Common_Popup_CustomerListForCS" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function pageLoad() {
            var oWnd = GetRadWindow();
            oWnd.set_width(500);
            oWnd.set_height(600);
        }
        //선택한 값을 부모 페이지로 넘긴다.
        function fn_ReturnToParent(sender, args) {
            var gridSelectdItems = $find('<%= radGridCustomerList.ClientID%>').get_masterTableView().get_selectedItems();

            var item = {};
            var retVal = false;
            if (gridSelectdItems.length == 0) {
                alert("선택한 항목이 없습니다.");
            }
            else {
                var row = gridSelectdItems[0];
                item.CUSTOMER_CODE = row.get_cell("CUSTOMER_CODE").innerText;
                item.CUSTOMER_NAME = row.get_cell("CUSTOMER_NAME").innerText;
                item.CREDIT_LIMIT = row.get_cell("CREDIT_LIMIT").innerText;
                item.MORTAGE = row.get_cell("MORTAGE").innerText;
                item.BU = row.get_cell("BU").innerText;
                item.PARVW = row.get_cell("PARVW").innerText;
                var oWnd = GetRadWindow();
                GetRadWindow().close(item);
            }
        }

        function CloseWnd(sender, args) {
            var oWnd = GetRadWindow();
            oWnd.close();
        }

        function fn_OnRowDblClick(sender, args) {
            var gridSelectedItems = $find("<%= radGridCustomerList.ClientID %>").get_masterTableView().get_selectedItems();
            var row = gridSelectedItems[0];
            var item = {};
            item.CUSTOMER_CODE = row.get_cell("CUSTOMER_CODE").innerHTML;
            item.CUSTOMER_NAME = row.get_cell("CUSTOMER_NAME").innerHTML;
            item.CREDIT_LIMIT = row.get_cell("CREDIT_LIMIT").innerHTML;
            item.MORTAGE = row.get_cell("MORTAGE").innerHTML;
            item.BU = row.get_cell("BU").innerText;
            item.PARVW = row.get_cell("PARVW").innerText;
            GetRadWindow().close(item);
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
    CS Customer
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radGridCustomerList">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radGridCustomerList" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div id="wrap">
        <div id="pop_content">
            <div class="align_right">
                <telerik:RadTextBox ID="radTxtKeyword" runat="server" ClientEvents-OnKeyPress="onEnter"></telerik:RadTextBox>
                <telerik:RadButton ID="radBtnSearch" runat="server" Text="Search"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-gray btn-size2 bold"
                    OnClick="radBtnSearch_Click">
                </telerik:RadButton>
            </div>
        </div>
        <div class="data_type2 pt20">
            <telerik:RadGrid ID="radGridCustomerList" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                OnNeedDataSource="radGridCustomerList_NeedDataSource" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                Skin="EXGrid" GridLines="None" Height="400px">

                <ClientSettings EnablePostBackOnRowClick="true">
                    <Selecting AllowRowSelect="true" />
                </ClientSettings>
                <HeaderStyle HorizontalAlign="Left" />
                <MasterTableView ShowHeadersWhenNoRecords="true" EnableHeaderContextMenu="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="CUSTOMER_CODE" HeaderText="Code" ItemStyle-HorizontalAlign="Left">
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CUSTOMER_NAME" HeaderText="Name">
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CREDIT_LIMIT" HeaderText="Credit"
                            DataFormatString="{0:#,##0}">
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MORTAGE" HeaderText=""
                            DataFormatString="{0:#,##0}" Display="false">
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BU" HeaderText="BU" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PARVW" HeaderText="PARVW" Display="true"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings>
                    <ClientEvents OnRowDblClick="fn_OnRowDblClick" />
                </ClientSettings>
                <PagerStyle PageSizeControlType="None" />
            </telerik:RadGrid>
        </div>
        <div class="align_center pt20">
            <telerik:RadButton ID="radBtnOk" runat="server" Text="OK"
                ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
                CssClass="btn btn-blue btn-size3 bold"
                OnClientClicked="fn_ReturnToParent">
            </telerik:RadButton>
            <telerik:RadButton ID="radBtnClose" runat="server" Text="Close"
                ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
                CssClass="btn btn-darkgray btn-size3 bold"
                OnClientClicked="CloseWnd">
            </telerik:RadButton>
        </div>
    </div>
    <div id="hddArea">
        <input type="hidden" id="hddBu" runat="server" />
        <input type="hidden" id="hddParvw" runat="server" />
        <input type="hidden" id="hddLevel" runat="server" />
    </div>
</asp:Content>

 