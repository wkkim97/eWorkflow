<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="SpecialPricing.aspx.cs" Inherits="Approval_Document_SpecialPricing" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">

        function fn_DoRequest(sender, arsg) {
            if (checkLastProduct()) {
                fn_UpdateGridData();
                return true;
            }
            else
                return false;
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData();
        }

        function checkLastProduct() {
            var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            if (dataItems.length > 0) {
                var lastItem = dataItems[dataItems.length - 1];
                var code = lastItem.get_cell("PRODUCT_CODE").innerText.trim();
                var qty = lastItem.get_cell("ORDER_QTY").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var requestDate = dataItems[i].get_cell("REQUEST_DATE").children[0].innerText.trim();
                if (code.length < 1 || qty.length < 1 || requestDate.length < 1) {
                    return false;
                } else {
                    return true;
                }
            }
            return true;
        }

        function checkDuplication(productCode) {
            var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var isDuplicate = false;
            for (var i = 0; i < dataItems.length; i++) {
                var code = dataItems[i].get_cell("PRODUCT_CODE").innerText;
                if (code == productCode) {
                    isDuplicate = true;
                    break;
                }
            }

            return isDuplicate;
        }

        function fn_UpdateGridData(product) {
            var list = [];
            var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            for (var i = 0; i < dataItems.length; i++) {
                var code = dataItems[i].get_cell("PRODUCT_CODE").innerText;

                var name = dataItems[i].get_cell("PRODUCT_NAME").innerText;
                var qty = dataItems[i].get_cell("ORDER_QTY").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var beforePrice = dataItems[i].get_cell("PRICE_BEFORE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var afterPrice = dataItems[i].get_cell("PRICE_AFTER").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var dc = dataItems[i].get_cell("DC").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var totalSales = dataItems[i].get_cell("TOTAL_SALES").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var totalDC = dataItems[i].get_cell("TOTAL_DC").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var requestDate = dataItems[i].get_cell("REQUEST_DATE").children[0].innerText;
                var requestQty = dataItems[i].get_cell("REQUEST_QTY").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var requestPrict = dataItems[i].get_cell("REQUEST_PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var conObj = {
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    ORDER_QTY: null,
                    PRICE_BEFORE: null,
                    PRICE_AFTER: null,
                    DC: null,
                    TOTAL_SALES: null,
                    TOTAL_DC: null,
                    REQUEST_DATE: null,
                    REQUEST_QTY: null,
                    REQUEST_PRICE: null,
                }
                conObj.PRODUCT_CODE = code;
                conObj.PRODUCT_NAME = name;
                conObj.ORDER_QTY = qty;
                conObj.PRICE_BEFORE = beforePrice;
                conObj.PRICE_AFTER = afterPrice;
                conObj.DC = dc;
                conObj.TOTAL_SALES = totalSales;
                conObj.TOTAL_DC = totalDC;
                conObj.REQUEST_DATE = requestDate;
                conObj.REQUEST_QTY = requestQty;
                conObj.REQUEST_PRICE = requestPrict;
                list.push(conObj);
            }


            if (product && product.PRODUCT_NAME) {

                var conObj = {
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    ORDER_QTY: null,
                    PRICE_BEFORE: null,
                    PRICE_AFTER: null,
                    DC: null,
                    TOTAL_SALES: null,
                    TOTAL_DC: null,
                    REQUEST_DATE: null,
                    REQUEST_QTY: null,
                    REQUEST_PRICE: null,
                }

                conObj.PRODUCT_CODE = product.PRODUCT_CODE;
                conObj.PRODUCT_NAME = product.PRODUCT_NAME + '(' + product.PRODUCT_CODE + ')';
                conObj.ORDER_QTY = 0;
                conObj.PRICE_BEFORE = product.BASE_PRICE;
                conObj.PRICE_AFTER = 0;
                conObj.DC = 0;
                conObj.TOTAL_SALES = 0;
                conObj.TOTAL_DC = 0;
                conObj.REQUEST_DATE = '';
                conObj.REQUEST_QTY = 0;
                conObj.REQUEST_PRICE = product.BASE_PRICE;
                list.push(conObj);

            }

            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));
            return true;
        }

        function fn_ShowProcudePopup(sender, args) {
            if (checkLastProduct()) {
                var wnd = $find("<%= modalWin.ClientID %>");
                wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=SM");
                wnd.show();
                sender.set_autoPostBack(false);
            } else {
                fn_OpenInformation('자료를 입력바랍니다.');
                sender.set_autoPostBack(false);
            }

        }

        function OnClientClose(sender, args) {
            if (sender.get_navigateUrl() == '/eWorks/Common/Popup/CustomerList.aspx?parvw=IE') {
                var customer = args.get_argument();
                if (customer) {
                    $find('<%= radTxtCustomer.ClientID %>').set_value(customer.CUSTOMER_NAME + '(' + customer.CUSTOMER_CODE + ')');
                    $('#<%= hddCustomerId.ClientID %>').val(customer.CUSTOMER_CODE);
                }
            } else {
                var product = args.get_argument();
                if (product) {
                    if (checkDuplication(product.PRODUCT_CODE) != true) {
                        fn_UpdateGridData(product);
                        $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind");
                    } else {
                        fn_OpenInformation('이미 등록된 Product입니다.');
                    }
                }
            }
        }

        function fn_OpenCustomerList(sender, args) {
            var wnd = $find("<%= modalWin.ClientID %>");
            wnd.setUrl("/eWorks/Common/Popup/CustomerList.aspx?parvw=IE");
            wnd.show();
            sender.set_autoPostBack(false);
        }

        var clickedKey = null;
        function confirmCallBackFn(arg) {
            if (arg) {
                var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        function openConfirmPopUp(index) {
            clickedKey = parseInt(index);
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);

            return false;
        }

        function fn_TypeCheckedChanged(sender, args) {
            if (sender.get_value() == '0004') {
                $('#<%= divPriceInfo.ClientID%>').hide();
            } else {
                $('#<%= divPriceInfo.ClientID%>').show();
            }
        }

        function fn_OnCustomerCheckedChanged(sender, args) {

            if (sender.get_text().toLowerCase() == 'all') {
                $('#rowCustomer').hide();
            } else {
                $('#rowCustomer').show();
            }
        }

        function setVisibleControl(type, isAll) {
            if (type == '0004') $('#<%= divPriceInfo.ClientID%>').hide();
            else $('#<%= divPriceInfo.ClientID%>').show();

            if (isAll == 'Y') $('#rowCustomer').hide();
            else if (isAll == 'N') $('#rowCustomer').show();
            else $('#rowCustomer').hide();
        }

        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdProduct.ClientID%>');

            var dataItems = grid.get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <h3>Type
        </h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Type</th>
                    <td>
                        <div id="divType" runat="server" style="margin: 0 0 0 0">
                            <telerik:RadButton ID="radBtnVolumn" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Type" AutoPostBack="false"
                                Text="Volumn" Value="0001" OnClientCheckedChanged="fn_TypeCheckedChanged">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnQuaterly" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Type" AutoPostBack="false"
                                Text="Quaterly/Campaign" Value="0002" OnClientCheckedChanged="fn_TypeCheckedChanged">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnSpecial" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Type" AutoPostBack="false"
                                Text="Special" Value="0003" OnClientCheckedChanged="fn_TypeCheckedChanged">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnMaximum" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Type" AutoPostBack="false"
                                Text="Maximum Price Discount Range" Value="0004" OnClientCheckedChanged="fn_TypeCheckedChanged">
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <h3>Customer Information</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <td colspan="2" style="text-align: right">
                        <telerik:RadButton ID="radBtnSelected" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Selected"
                            Text="Selected" Value="0001" OnClientCheckedChanged="fn_OnCustomerCheckedChanged" AutoPostBack="false">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnAll" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Selected"
                            Text="All" Value="0002" OnClientCheckedChanged="fn_OnCustomerCheckedChanged" AutoPostBack="false">
                        </telerik:RadButton>
                    </td>
                </tr>
                <tr id="rowCustomer" style="display: none">
                    <th>Customer</th>
                    <td>
                        <telerik:RadTextBox ID="radTxtCustomer" runat="server" ReadOnly="true" Width="85%"></telerik:RadTextBox>
                        <telerik:RadButton ID="radBtnCustomer" runat="server" Text="Search" OnClientClicked="fn_OpenCustomerList"></telerik:RadButton>
                        <input type="hidden" id="hddCustomerId" runat="server" />
                    </td>
                </tr>
            </table>
        </div>

        <div id="divPriceInfo" runat="server" style="margin: 0 0 0 0">
            <h3>Price Information
            <div class="title_btn">
                <telerik:RadButton ID="radBtnAddProduct" runat="server" OnClientClicked="fn_ShowProcudePopup" Text="Add" AutoPostBack="false"></telerik:RadButton>
            </div>
            </h3>
            <telerik:RadGrid ID="radGrdProduct" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" Skin="EXGrid"
                OnItemCommand="radGrdProduct_ItemCommand">
                <MasterTableView EditMode="Batch" ClientDataKeyNames="PRODUCT_CODE">
                    <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                    <ItemStyle Wrap="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="PRODUCT_CODE" HeaderText="Material No." HeaderStyle-Width="10px" UniqueName="PRODUCT_CODE"
                            ReadOnly="true" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PRODUCT_NAME" HeaderText="Product Name" HeaderStyle-Width="140px" UniqueName="PRODUCT_NAME"
                            ReadOnly="true" ItemStyle-Wrap="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="ORDER_QTY" UniqueName="ORDER_QTY" HeaderText="Order Qty" HeaderStyle-Width="70px"
                            Aggregate="Sum" DataType="System.Decimal" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}"
                            FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                            <ItemStyle HorizontalAlign="Right" />

                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="PRICE_BEFORE" HeaderText="Base Price" HeaderStyle-Width="70px" UniqueName="PRICE_BEFORE"
                            ReadOnly="true" DataType="System.Decimal" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right">
                        </telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="PRICE_AFTER" UniqueName="PRICE_AFTER" HeaderText="After Price" HeaderStyle-Width="70px"
                            Aggregate="Sum" DataType="System.Decimal" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}"
                            FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="DC" HeaderText="DC" HeaderStyle-Width="70px" UniqueName="DC"
                            ReadOnly="true" DataType="System.Decimal" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TOTAL_SALES" HeaderText="Total Sales" HeaderStyle-Width="70px" UniqueName="TOTAL_SALES"
                            ReadOnly="true" DataType="System.Decimal" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TOTAL_DC" HeaderText="Total DC" HeaderStyle-Width="70px" UniqueName="TOTAL_DC"
                            ReadOnly="true" DataType="System.Decimal" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="REQUEST_DATE" HeaderText="일시" HeaderStyle-Width="110px"
                            UniqueName="REQUEST_DATE">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:yyyy-MM-dd}", Eval("REQUEST_DATE")) %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDatePicker ID="radGrdRequest" runat="server" DateInput-DisplayDateFormat="yyyy-MM-dd"
                                    DateInput-DateFormat="yyyy-MM-dd" Width="100px" Calendar-ShowRowHeaders="false">
                                    <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                </telerik:RadDatePicker>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridNumericColumn DataField="REQUEST_QTY" UniqueName="REQUEST_QTY" HeaderText="Qty" HeaderStyle-Width="70px"
                            Aggregate="Sum" DataType="System.Decimal" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}"
                            FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red" ReadOnly="true" Display="false">
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="REQUEST_PRICE" HeaderText="단가" HeaderStyle-Width="70px" UniqueName="REQUEST_PRICE"
                            ReadOnly="true" DataType="System.Decimal" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="40px">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" OnClientClick='<%# String.Format("return openConfirmPopUp({0});",Eval("PRODUCT_CODE"))%> '
                                    ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>

            </telerik:RadGrid>

            <div class="data_type1">
                <table>
                    <colgroup>
                        <col style="width: 25%;" />
                        <col />
                        <col style="width: 25%;" />
                        <col style="width: 25%;" />
                    </colgroup>
                    <tr>
                        <th>Period</th>
                        <td>
                            <telerik:RadTextBox ID="radTxtPeriod" runat="server" Width="100%"></telerik:RadTextBox>
                        </td>
                        <th>Date od Enforcement
                        </th>
                        <td>
                            <telerik:RadDatePicker ID="radDatEnforcement" runat="server" Width="100px" Calendar-ShowRowHeaders="false">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <h3>The Reason of special pricing
        </h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 100%;" />
                </colgroup>
                <tr>
                    <td>
                        <telerik:RadTextBox ID="radTxtReason" runat="server" TextMode="MultiLine" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%--ADD POPUP--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" OnClientClose="OnClientClose">
        <Windows>
            <telerik:RadWindow ID="modalWin" runat="server" NavigateUrl="/eWorks/Common/Popup/PopupUserList.aspx" Title="Product List" Modal="true" Width="500" Height="700" VisibleStatusbar="false" Skin="Metro">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <input type="hidden" id="hddGridItems" runat="server" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="HddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
	<input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

