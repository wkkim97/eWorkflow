<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="ProductList.aspx.cs" Inherits="Common_Popup_ProductList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">

        function pageLoad() {
            var oWnd = GetRadWindow();
            oWnd.set_height(600);
        }

        //선택한 값을 부모 페이지로 넘긴다.
        function fn_ReturnToParent(sender, args) {
            var gridSelectdItems = $find('<%= radGridProductList.ClientID%>').get_masterTableView().get_selectedItems();

            var item = {};
            var retVal = false;
            if (gridSelectdItems.length == 0) {
                alert("선택한 항목이 없습니다.");

            }
            else {
                var row = gridSelectdItems[0];
                item.PRODUCT_NAME = row.get_cell("PRODUCT_NAME").innerText;
                item.SAMPLE_CODE = row.get_cell("SAMPLE_CODE").innerText;
                item.PRODUCT_CODE = row.get_cell("PRODUCT_CODE").innerText;
                item.BASE_PRICE = row.get_cell("BASE_PRICE").innerText;
                item.INVOICE_PRICE = row.get_cell("INVOICE_PRICE").innerText;
                item.INVOICE_PRICE_NH = row.get_cell("INVOICE_PRICE_NH").innerText;
                item.NET1_PRICE = row.get_cell("NET1_PRICE").innerText;
                item.NET1_PRICE_NH = row.get_cell("NET1_PRICE_NH").innerText;
                item.NET2_PRICE = row.get_cell("NET2_PRICE").innerText;
                item.NET2_PRICE_NH = row.get_cell("NET2_PRICE_NH").innerText;
                item.MARGIN = row.get_cell("MARGIN").innerText;
                item.TP_PRICE = row.get_cell("TP_PRICE").innerText;
                item.CROP = row.get_cell("CROP").innerText;
                item.VARIETY = row.get_cell("VARIETY").innerText;
                item.SF_ST = row.get_cell("SF_ST").innerText;
                item.FACTOR = row.get_cell("FACTOR").innerText;
                var oWnd = GetRadWindow();
                GetRadWindow().close(item);
            }

        }

        function CloseWnd(sender, args) {
            var oWnd = GetRadWindow();
            oWnd.close();
        }

        function fn_OnRowDblClick(sender, args) {
            var gridSelectedItems = $find("<%= radGridProductList.ClientID %>").get_masterTableView().get_selectedItems();
            var row = gridSelectedItems[0];
            var item = {};
            item.PRODUCT_NAME = row.get_cell("PRODUCT_NAME").innerHTML;
            item.SAMPLE_CODE = row.get_cell("SAMPLE_CODE").innerHTML;
            item.PRODUCT_CODE = row.get_cell("PRODUCT_CODE").innerHTML;
            item.BASE_PRICE = row.get_cell("BASE_PRICE").innerHTML;
            item.INVOICE_PRICE = row.get_cell("INVOICE_PRICE").innerText;
            item.INVOICE_PRICE_NH = row.get_cell("INVOICE_PRICE_NH").innerText;
            item.NET1_PRICE = row.get_cell("NET1_PRICE").innerText;
            item.NET1_PRICE_NH = row.get_cell("NET1_PRICE_NH").innerText;
            item.NET2_PRICE = row.get_cell("NET2_PRICE").innerText;
            item.NET2_PRICE_NH = row.get_cell("NET2_PRICE_NH").innerText;
            item.MARGIN = row.get_cell("MARGIN").innerText;
            item.TP_PRICE = row.get_cell("TP_PRICE").innerText;
            item.CROP = row.get_cell("CROP").innerText;
            item.VARIETY = row.get_cell("VARIETY").innerText;
            item.SF_ST = row.get_cell("SF_ST").innerText;
            item.FACTOR = row.get_cell("FACTOR").innerText;

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
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radGridProductList">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radGridProductList" />
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
            <div class="data_type2 pt20">
                <telerik:RadGrid ID="radGridProductList" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false"
                    OnNeedDataSource="radGridProductList_NeedDataSource" Skin="EXGrid" GridLines="None" 
                    Height="400px">
                    <ClientSettings EnablePostBackOnRowClick="true">
                        <Selecting AllowRowSelect="true" />
                    </ClientSettings>
                    <HeaderStyle HorizontalAlign="Left" />
                    <MasterTableView ShowHeadersWhenNoRecords="true">
                        <Columns>
                            <telerik:GridBoundColumn DataField="PRODUCT_CODE" HeaderText="Code"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PRODUCT_NAME" HeaderText="Name"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SAMPLE_CODE" HeaderText="Sample Code" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BASE_PRICE" HeaderText="" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="INVOICE_PRICE" HeaderText="" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="INVOICE_PRICE_NH" HeaderText="" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NET1_PRICE" HeaderText="" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NET1_PRICE_NH" HeaderText="" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NET2_PRICE" HeaderText="" Display="false"></telerik:GridBoundColumn>                            
                            <telerik:GridBoundColumn DataField="NET2_PRICE_NH" HeaderText="" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MARGIN" HeaderText="" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TP_PRICE" HeaderText="" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CROP" HeaderText="" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="VARIETY" HeaderText="" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SF_ST" HeaderText="" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FACTOR" HeaderText="" Display="false"></telerik:GridBoundColumn>
                            
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
                    CssClass="btn btn-blue btn-size3 bold" OnClientClicked="fn_ReturnToParent">
                </telerik:RadButton>
                <telerik:RadButton ID="radBtnClose" runat="server" Text="CLOSE"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
                    CssClass="btn btn-darkgray btn-size3 bold" OnClientClicked="CloseWnd">
                </telerik:RadButton>
            </div>

            <div id="hiddenArea">
                <input type="hidden" id="hddBu" runat="server" />
                <input type="hidden" id="hddBasePrice" runat="server" />
                <input type="hidden" id="hddSampletype" runat="server" />
                <input type="hidden" id="hddExistsample" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
