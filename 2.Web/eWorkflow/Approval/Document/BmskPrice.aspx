<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="BmskPrice.aspx.cs" Inherits="Approval_Document_BmskPrice" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">

        function fn_UpdateGridDataFromUploader() {
            fn_UpdateGridData(null);
        }

        function fn_DoRequest(sender, arsg) {
            return fn_UpdateGridData();
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData();
        }

        function checkLastRow() {
            var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            if (dataItems.length > 0) {
                var lastItem = dataItems[0]
                var basePrice = lastItem.get_cell("BASE_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var tarMinPrice = lastItem.get_cell("TARGET_MIN_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var requestPrice = lastItem.get_cell("REQUEST_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var ctlFrom = lastItem.findControl('radGrdDatFrom');
                //var fromDt = null;
                //var toDt = null;
                //if (ctlFrom) {
                //    fromDt = lastItem.findControl('radGrdDatFrom').get_element().value;
                //    toDt = lastItem.findControl('radGrdDatTo').get_element().value;
                //} else {
                //    fromDt = lastItem.get_cell("FROM_DATE").children[0].innerText;
                //    toDt = lastItem.get_cell("TO_DATE").children[0].innerText;
                //}
                if (basePrice.length < 1 || tarMinPrice.length < 1 || requestPrice.length < 1 ) {
                    fn_OpenDocInformation('자료를 입력바랍니다.');
                    return false;
                } else
                    return true;
            }
            else return true;

        }

        function fn_OpenCustomer(sender, eventArgs) {
            var wnd = $find("<%= radWinCustomer.ClientID %>");
            wnd.setUrl("/eWorks/Common/Popup/CustomerList.aspx");
            wnd.show();
            sender.set_autoPostBack(false);
        }

        function fn_ClientClose(oWnd, args) {
            var item = args.get_argument();

            if (item != null) {
                var txtcustomer = $find("<%= radtxtCustomer.ClientID%>");
                txtcustomer.set_value(item.CUSTOMER_NAME.trim() + " (" + item.CUSTOMER_CODE + ")");
                $('#<%=hddCustomerCode.ClientID%>').val(item.CUSTOMER_CODE);
            }
            else {
                oWnd.close();
            }
        }

        function fn_PopupProduct(sender, args) {
            if (checkLastRow()) {
                var bu = '';
                var btnPCS = $find('<%= radBtnPCS.ClientID %>');
                var btnPUR = $find('<%= radBtnPUR.ClientID %>');
                if (btnPCS.get_checked()) bu = btnPCS.get_value();
                else if (btnPUR.get_checked()) bu = btnPUR.get_value();
                if (bu == '') {
                    fn_OpenDocInformation('BU를 선택바랍니다.');
                } else {
                    var wnd = $find("<%= radWinProduct.ClientID %>");
                    wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=" + bu);
                    wnd.show();
                    sender.set_autoPostBack(false);
                }
            } else {
                sender.set_autoPostBack(false);
            }
        }

        function fn_UpdateGridData(product) {
            var list = [];
            var row = $find('<%= radGrdProduct.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
                    var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
                    $find('<%= radGrdProduct.ClientID %>').get_batchEditingManager().saveChanges(masterTable);
                    var dataItems = masterTable.get_dataItems();

                    var maxIdx = 0;
                    for (var i = 0; i < dataItems.length; i++) {

                        var idx = dataItems[i].get_cell("IDX").innerText;
                        var prodCode = dataItems[i].get_cell("MATERIAL_CODE").innerText;

                        //중복체크
                        //if (prodCode == product.PRODUCT_CODE) {
                        //    return false;
                        //}

                        var prodName = dataItems[i].get_cell("MATERIAL_NAME").innerText;
                        var basePrice = dataItems[i].get_cell("BASE_PRICE").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                        var tarMinPrice = dataItems[i].get_cell("TARGET_MIN_PRICE").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                        var requestPrice = dataItems[i].get_cell("REQUEST_PRICE").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                        var currency = dataItems[i].get_cell("CURRENCY").children[0].innerText;
                        var ctlFrom = dataItems[i].findControl('radGrdDatFrom');
                        var fromDt = null;
                        var toDt = null;
                        if (ctlFrom) {
                            fromDt = dataItems[i].findControl('radGrdDatFrom').get_element().value;
                            toDt = dataItems[i].findControl('radGrdDatTo').get_element().value;
                        } else {
                            fromDt = dataItems[i].get_cell("FROM_DATE").children[0].innerText;
                            toDt = dataItems[i].get_cell("TO_DATE").children[0].innerText;
                        }

                        var conObj = {
                            IDX: null,
                            MATERIAL_CODE: null,
                            MATERIAL_NAME: null,
                            BASE_PRICE: null,
                            TARGET_MIN_PRICE: null,
                            REQUEST_PRICE: null,
                            CURRENCY: null,
                            FROM_DATE: null,
                            TO_DATE: null,
                        }
                        conObj.IDX = parseInt(idx);
                        conObj.MATERIAL_CODE = prodCode;
                        conObj.MATERIAL_NAME = prodName;
                        conObj.BASE_PRICE = basePrice;
                        conObj.TARGET_MIN_PRICE = tarMinPrice;
                        conObj.REQUEST_PRICE = requestPrice;
                        conObj.CURRENCY = currency;
                        conObj.FROM_DATE = fromDt.trim();
                        conObj.TO_DATE = toDt.trim();
                        list.push(conObj);
                        maxIdx = parseInt(idx);
                    }

                    if (product) {
                        var conObj = {
                            IDX: null,
                            //CUSTOMER_CODE: null,
                            //CUSTOMER_NAME: null,
                            MATERIAL_CODE: null,
                            MATERIAL_NAME: null,
                            BASE_PRICE: null,
                            TARGET_MIN_PRICE: null,
                            REQUEST_PRICE: null,
                            CURRENCY: null,
                            FROM_DATE: null,
                            TO_DATE: null,
                        }

                        conObj.IDX = ++maxIdx;
                        //conObj.CUSTOMER_CODE = customer.CUSTOMER_CODE;
                        //conObj.CUSTOMER_NAME = customer.CUSTOMER_NAME + "(" + customer.CUSTOMER_CODE + ")";
                        conObj.MATERIAL_CODE = product.PRODUCT_CODE;
                        conObj.MATERIAL_NAME = product.PRODUCT_NAME + "(" + product.PRODUCT_CODE + ")";
                        conObj.BASE_PRICE = product.BASE_PRICE;
                        conObj.TARGET_MIN_PRICE = 0;
                        conObj.REQUEST_PRICE = 0;
                        conObj.CURRENCY = 'KRW';
                        conObj.FROM_DATE = '';
                        conObj.TO_DATE = '';
                        list.push(conObj);
                    }
                    $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));
                    return true;
                }

                var customer = null;
                function fn_OnClientClose(sender, args) {
                    if (sender.get_navigateUrl() == '/eWorks/Common/Popup/CustomerList.aspx') {
                        customer = args.get_argument();
                        if (customer != null) {
                            var wnd = $find("<%= radWinProduct.ClientID %>");
                    wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx");
                    wnd.show();
                }
            } else if (sender.get_navigateUrl().indexOf('/eWorks/Common/Popup/ProductList.aspx') == 0) {
                var product = args.get_argument();
                if(fn_UpdateGridData(product)){
                    $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind");
                } else {
                    fn_OpenDocInformation("동일한 product 가 존재합니다.");
                }
            }
    }


    function fn_OnBUClicked(sender, args) {
        var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
        var column = masterTable.getColumnByUniqueName("TARGET_MIN_PRICE").get_element();
        if (sender.get_value() == 'PCS') {
            column.innerHTML = 'Target<br/>Price';
        } else {
            column.innerHTML = 'Minimum<br/>Price';
        }
    }
    var clickedKey = null;

    function openConfirmPopUp(idx) {
        clickedKey = idx;
        fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);
        return false;
    }

    function confirmCallBackFn(arg) {
        if (arg) {
            fn_UpdateGridData();
            var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
            masterTable.fireCommand("Remove", clickedKey);
        }
    }

    function fn_OnDataInputBlur(sender, args) {
        try {
            if (!sender.get_invalid())
                sender.set_selectedDate(sender._text);
            else
                sender.set_selectedDate(null);
        } catch (exception) { }
    }

    function openGridRowForEdit(sender, args) {
        var grid = $find('<%=radGrdProduct.ClientID%>');

        if (grid.get_masterTableView() != null) {
            var dataItems = grid.get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
            }
        }
    }

    function fn_OnGridNumBlur(sender) {
        setNumberFormat(sender);
    }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="radDocLoading" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <div class="doc_style">
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>BU <span class="text_red">*</span>
                    </th>
                    <td>
                        <div id="divSalesGroup" runat="server" style="margin: 0 0 0 0">
                            <telerik:RadButton ID="radBtnPCS" runat="server" Text="PCS" Value="PCS" GroupName="BU" AutoPostBack="false"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClientClicked="fn_OnBUClicked">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnPUR" runat="server" Text="PUR" Value="PUR" GroupName="BU" AutoPostBack="false"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClientClicked="fn_OnBUClicked">
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Customer <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radtxtCustomer" runat="server" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadButton ID="radBtnCustomer" runat="server" AutoPostBack="false" Width="18px" Height="18px" CssClass="btn_grid" OnClientClicked="fn_OpenCustomer">
                            <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                        </telerik:RadButton>
                        <input type="hidden" id="hddCustomerCode" runat="server" />
                    </td>

                </tr>
            </table>
        </div>
        <h3>Customer/Material
            <div class="title_btn">
                <telerik:RadButton ID="radBtnAddTrip" runat="server" Text="Add" AutoPostBack="false" OnClientClicked="fn_PopupProduct"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size1 bold">
                </telerik:RadButton>
            </div>
        </h3>
        <telerik:RadGrid ID="radGrdProduct" runat="server" AutoGenerateColumns="false"
            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" AllowSorting="false" ItemStyle-Wrap="true"
            OnItemCommand="radGrdProduct_ItemCommand" OnItemDataBound="radGrdProduct_ItemDataBound">
            <HeaderStyle CssClass="grid_header" HorizontalAlign="Center" />
            <MasterTableView EditMode="Batch">
                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderText="Index" Display="false"></telerik:GridBoundColumn>
                    <%--                    <telerik:GridBoundColumn DataField="CUSTOMER_CODE" HeaderText="Code" UniqueName="CUSTOMER_CODE" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CUSTOMER_NAME" HeaderText="Customer" HeaderStyle-Width="40%" UniqueName="CUSTOMER_NAME" ReadOnly="true"></telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn DataField="MATERIAL_CODE" HeaderText="Codee" UniqueName="MATERIAL_CODE" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="MATERIAL_NAME" HeaderText="Material" HeaderStyle-Width="60%" UniqueName="MATERIAL_NAME" ReadOnly="true"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="BASE_PRICE" HeaderText="Base<br/>Price" UniqueName="BASE_PRICE" HeaderStyle-Width="80px" DataType="System.Decimal">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("BASE_PRICE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
<%--                            <telerik:RadNumericTextBox runat="server" ID="radGrdNumBasePrice" Width="100%"
                                EnabledStyle-HorizontalAlign="Right"
                                MinValue="0" MaxValue="99999999999">
                                <NumberFormat DecimalDigits="0" />
                            </telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="radGrdNumBasePrice" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="3" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="TARGET_MIN_PRICE" HeaderText="Target<br/>Price" UniqueName="TARGET_MIN_PRICE" HeaderStyle-Width="80px">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("TARGET_MIN_PRICE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="radGrdNumTarMinPrice" NumberFormat-DecimalDigits="0" Width="100%" EnabledStyle-HorizontalAlign="Right"></telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="radGrdNumTarMinPrice" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="3" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="REQUEST_PRICE" HeaderText="Request<br/>Price" UniqueName="REQUEST_PRICE" HeaderStyle-Width="80px">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("REQUEST_PRICE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="radGrdNumRequestPrice" NumberFormat-DecimalDigits="0" Width="100%" EnabledStyle-HorizontalAlign="Right"></telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="radGrdNumRequestPrice" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="3" AllowNegative="false">                                
                            </asp:TextBox>

                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="FROM_DATE" HeaderText="From" HeaderStyle-Width="110px"
                        UniqueName="FROM_DATE">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# String.Format("{0:yyyy-MM-dd}", Eval("FROM_DATE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDatePicker ID="radGrdDatFrom" runat="server" Culture="ko-KR" Width="100px" DateInput-ClientEvents-OnBlur="fn_OnDataInputBlur"
                                Calendar-ShowRowHeaders="false">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="TO_DATE" HeaderText="To" HeaderStyle-Width="110px"
                        UniqueName="TO_DATE">
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# String.Format("{0:yyyy-MM-dd}", Eval("TO_DATE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDatePicker ID="radGrdDatTo" runat="server" Culture="ko-KR" Width="100px" DateInput-ClientEvents-OnBlur="fn_OnDataInputBlur"
                                Calendar-ShowRowHeaders="false">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="CURRENCY" UniqueName="CURRENCY" HeaderText="Currency" HeaderStyle-Width="75px">
                        <ItemTemplate><%# Eval("CURRENCY")%></ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDropDownList ID="radGrdDdlSex" runat="server" Width="100%" DropDownWidth="70px">
                                <Items>
                                    <telerik:DropDownListItem Text="KRW" Value="KRW" />
                                    <telerik:DropDownListItem Text="USD" Value="USD" />
                                </Items>
                            </telerik:RadDropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" OnClientClick='<%# String.Format("return openConfirmPopUp({0});",Eval("IDX"))%> '
                                ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                
            </MasterTableView>
            <ClientSettings>
                <Selecting AllowRowSelect="true" CellSelectionMode="None"  />
                
            </ClientSettings>
        </telerik:RadGrid>
    </div>

    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinCustomer" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer"
                Width="500px" Height="600px"
                Behaviors="Default" Modal="true" CssClass="windowscroll" OnClientClose="fn_ClientClose" NavigateUrl="/eWorks/Common/Popup/CustomerList.aspx">
            </telerik:RadWindow>
            <telerik:RadWindow ID="radWinProduct" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product"
                Width="500px" Height="600px"
                Behaviors="Default" Modal="true" CssClass="windowscroll" OnClientClose="fn_OnClientClose" NavigateUrl="/eWorks/Common/Popup/ProductList.aspx">
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

