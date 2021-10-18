<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="BaseDiscountPrice.aspx.cs" Inherits="Approval_Document_BaseDiscountPrice" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">

        function pageLoad() {
            var addRow = $('#<%= hddAddRow.ClientID %>').val();

            if (addRow == 'Y') {
                var grid = $find('<%=radGrdBaseDiscountPriceProduct.ClientID%>');
                if (grid.get_masterTableView()) {
                    var dataItems = grid.get_masterTableView().get_dataItems();
                    if (dataItems.length > 0) {
                        grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                    }
                }
                $('#<%= hddAddRow.ClientID %>').val('N');
            }
        }

        function fn_UpdateGridDataFromUploader() {
            fn_UpdateGridData(false);
        }

        function fn_DoRequest(sender, args) {
            return fn_UpdateGridData(false);
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData(false);
        }

        function fn_OnAddButtonClicked(sender, args) {
            var controls = $('#<%= divType.ClientID %>').children();
            var selectedValue = null;

            for (var i = 0; i < controls.length; i++) {
                var type = controls[i];
                if ($find(type.id).get_checked()) {
                    selectedValue = $find(type.id).get_value();
                    break;
                }
            }
            if (selectedValue) {
                if (checkLastRow()) {
                    fn_UpdateGridData(true);
                    $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind");
                }
            } else {
                fn_OpenDocInformation('please Select a Type');
            }
        }

        function checkLastRow() {
            fn_UpdateGridData(false);
            var masterTable = $find('<%= radGrdBaseDiscountPriceProduct.ClientID%>').get_masterTableView();
            $find('<%= radGrdBaseDiscountPriceProduct.ClientID%>').get_batchEditingManager().saveChanges(masterTable);
            var dataItems = masterTable.get_dataItems();
            var newprice = $find('<%= radRdoType1.ClientID%>').get_checked();
            var change = $find('<%= radRdoType2.ClientID%>').get_checked();
            var discount = $find('<%= radRdoType3.ClientID%>').get_checked();

            if (dataItems.length > 0 && newprice) {
                var lastItem = dataItems[dataItems.length - 1];
                var product = lastItem.get_cell("PRODUCT_NAME").children[0].innerText;
                var productcode = lastItem.get_cell("PRODUCT_CODE").children[0].innerText;
                //var baseprice = lastItem.get_cell("BASE_PRICE").children[0].innerText;

                if (product.length < 1 && productcode.length < 1) {
                    fn_OpenDocInformation('자료를 입력바랍니다.');
                    return false;
                }
            }

            else if (dataItems.length > 0 && change) {
                var lastItem = dataItems[dataItems.length - 1];
                var product = lastItem.get_cell("PRODUCT_NAME").children[0].innerText;
                var productcode = lastItem.get_cell("PRODUCT_CODE").children[0].innerText;
                //var baseprice = lastItem.get_cell("BASE_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                //var basepriceToBe = lastItem.get_cell("BASE_PRICE_TO_BE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                //var changeprice = lastItem.get_cell("CHANGE_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                if (product.length < 1 && productcode.length < 1) {
                    fn_OpenDocInformation('자료를 입력바랍니다.');
                    return false;
                }
            }

            else if (dataItems.length > 0 && discount) {
                var lastItem = dataItems[dataItems.length - 1];
                var product = lastItem.get_cell("PRODUCT_NAME").children[0].innerText;
                var productcode = lastItem.get_cell("PRODUCT_CODE").children[0].innerText;
                //var baseprice = lastItem.get_cell("BASE_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                //var discountprice = lastItem.get_cell("DISCOUNT_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                if (product.length < 1 && productcode.length < 1) {
                    fn_OpenDocInformation('자료를 입력바랍니다.');
                    return false;
                }
            }
            return true;
        }

        function fn_UpdateGridData(addRow) {
            var list = [];
            var masterTable = $find('<%= radGrdBaseDiscountPriceProduct.ClientID%>').get_masterTableView();
            $find('<%= radGrdBaseDiscountPriceProduct.ClientID%>').get_batchEditingManager().saveChanges(masterTable);
            var dataItems = masterTable.get_dataItems();
            var maxIdx = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var idx = dataItems[i].get_cell("IDX").innerText;
                var product = dataItems[i].get_cell("PRODUCT_NAME").children[0].innerText;
                var productcode = dataItems[i].get_cell("PRODUCT_CODE").children[0].innerText;
                var unit = dataItems[i].get_cell("UNIT").children[0].innerText;
                var baseprice = dataItems[i].get_cell("BASE_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var basepriceToBe = dataItems[i].get_cell("BASE_PRICE_TO_BE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var changeprice = dataItems[i].get_cell("CHANGE_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var discountprice = dataItems[i].get_cell("DISCOUNT_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var conObj = {
                    IDX: null,
                    PRODUCT_NAME: null,
                    PRODUCT_CODE: null,
                    UNIT: null,
                    BASE_PRICE: null,
                    BASE_PRICE_TO_BE: null,
                    CHANGE_PRICE: null,
                    DISCOUNT_PRICE: null,
                }
                conObj.IDX = idx;
                conObj.PRODUCT_NAME = product;
                conObj.PRODUCT_CODE = productcode;
                conObj.UNIT = unit;
                conObj.BASE_PRICE = baseprice;
                conObj.BASE_PRICE_TO_BE = basepriceToBe;
                conObj.CHANGE_PRICE = changeprice;
                conObj.DISCOUNT_PRICE = discountprice;
                maxIdx = parseInt(idx);
                list.push(conObj);
            }
            if (addRow) {
                var conObj = {
                    IDX: null,
                    PRODUCT_NAME: null,
                    PRODUCT_CODE: null,
                    UNIT: null,
                    BASE_PRICE: null,
                    BASE_PRICE_TO_BE: null,
                    CHANGE_PRICE: null,
                    DISCOUNT_PRICE: null,
                }
                conObj.IDX = ++maxIdx;
                conObj.PRODUCT_NAME = '';
                conObj.PRODUCT_CODE = '';
                conObj.UNIT = 0;
                conObj.BASE_PRICE = 0;
                conObj.BASE_PRICE_TO_BE = 0;
                conObj.CHANGE_PRICE = 0;
                conObj.DISCOUNT_PRICE = 0;

                list.push(conObj);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

            return true;
        }

        function openConfirmPopUp(index) {
            clickedKey = parseInt(index);
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);

            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                var masterTable = $find('<%= radGrdBaseDiscountPriceProduct.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdBaseDiscountPriceProduct.ClientID%>');

            if (grid.get_masterTableView()) {
                var dataItems = grid.get_masterTableView().get_dataItems();
                if (dataItems.length > 0) {
                    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                }
            }
        }

        function fn_OnCustomerRequesting(sender, args) {
            var context = args.get_context();
            context["company"] = $('#<%= hddCompanyCode.ClientID %>').val();
        context["bu"] = '';
        context["parvw"] = '';
        context["level"] = "N";
    }

    function fn_OnGridNumBlur(sender) {
        setNumberFormat(sender);
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Type <span class="text_red">*</span></th>
                        <td>
                            <div id="divType" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton runat="server" ID="radRdoType1" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Base Price-NEW" Value="New" GroupName="Type" OnClick="radRdoType1_Click"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="radRdoType2" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Base Price-Change" Value="Change" GroupName="Type" OnClick="radRdoType1_Click"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="radRdoType3" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Discount" Value="Discount" GroupName="Type" OnClick="radRdoType1_Click"></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>Customer</th>
                        <td>
                            <%--<telerik:RadTextBox ID="radtxtCustomer" runat="server" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                            <telerik:RadButton ID="radBtnCustomer" runat="server" AutoPostBack="false" Width="18px" Height="18px" CssClass="btn_grid" OnClientClicked="fn_OpenCustomer">
                                <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                            </telerik:RadButton>--%>
                            <telerik:RadAutoCompleteBox ID="radAcomCustomer" runat="server" AllowCustomEntry="false" Width="100%" DropDownWidth="300px"
                                OnClientRequesting="fn_OnCustomerRequesting">
                                <WebServiceSettings Method="SearchCustomer" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                            </telerik:RadAutoCompleteBox>
                            <asp:Label ID="lblNotCustomer" runat="server" Width="100%" Visible="false">Refer to the customer list attached.</asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <th>Product <span class="text_red">*</span></th>
                        <td>
                            <div id="divProduct" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton runat="server" ID="radRdoProduct1" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="LCD" Value="LCD" GroupName="Product" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="radRdoProduct2" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Solid Sheet" Value="Solid Sheet" GroupName="Product" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="radRdoProduct3" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="MWS" Value="MWS" GroupName="Product" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="radRdoProduct4" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Scrap" Value="Scrap" GroupName="Product" AutoPostBack="false"></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>Description <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadTextBox ID="radTxtDescription" runat="server" Width="98%" TextMode="MultiLine" Height="50px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Effective Date <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadDatePicker ID="radFromDate" runat="server" DateInput-DisplayDateFormat="yyyy-MM-dd"
                                DateInput-DateFormat="yyyy-MM-dd" Width="100px" Calendar-ShowRowHeaders="false" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                            <telerik:RadDatePicker ID="radToDate" runat="server" DateInput-DisplayDateFormat="yyyy-MM-dd"
                                DateInput-DateFormat="yyyy-MM-dd" Width="100px" Calendar-ShowRowHeaders="false" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <h3>Product Information <span class="text_red">*</span>
        <div class="title_btn">
            <telerik:RadButton ID="radBtnAdd" runat="server" Text="Add" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" OnClientClicked="fn_OnAddButtonClicked" AutoPostBack="false">
            </telerik:RadButton>
        </div>
    </h3>
    <telerik:RadGrid ID="radGrdBaseDiscountPriceProduct" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Left" ShowFooter="false" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" GridLines="None" OnItemCommand="radGrdBaseDiscountPriceProduct_ItemCommand" AllowAutomaticUpdates="true">
        <MasterTableView EnableHeaderContextMenu="true" EditMode="Batch" ClientDataKeyNames="IDX" HeaderStyle-CssClass="grid_header">
            <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
            <Columns>
                <telerik:GridBoundColumn DataField="IDX" HeaderStyle-Width="40px" UniqueName="IDX" ReadOnly="true" Display="false">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="PRODUCT_NAME" UniqueName="PRODUCT_NAME" HeaderText="Product Name" HeaderStyle-Width="50%" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate><%# Eval("PRODUCT_NAME")%></ItemTemplate>
                    <EditItemTemplate>
                        <%--<telerik:RadTextBox ID="radGrdTxtProductName" runat="server" Width="98%"></telerik:RadTextBox>--%>
                        <asp:TextBox ID="radGrdTxtProductName" runat="server" Width="100%" CssClass="input"></asp:TextBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="PRODUCT_CODE" UniqueName="PRODUCT_CODE" HeaderText="Product code" HeaderStyle-Width="50%" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate><%# Eval("PRODUCT_CODE")%></ItemTemplate>
                    <EditItemTemplate>
                        <%--<telerik:RadTextBox ID="radGrdTxtProductCode" runat="server" Width="98%"></telerik:RadTextBox>--%>
                        <asp:TextBox ID="radGrdTxtProductCode" runat="server" Width="100%" CssClass="input"></asp:TextBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="UNIT" UniqueName="UNIT" HeaderText="Unit" 
                    HeaderStyle-Width="80px"  HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate><%# Eval("UNIT")%></ItemTemplate>
                    <EditItemTemplate>
                        <telerik:RadDropDownList ID="radGrdDdlUnit" runat="server" Width="100%" DropDownWidth="70px">
                            <Items>
                                <telerik:DropDownListItem Text="KG" Value="KG" />
                                <telerik:DropDownListItem Text="PCE" Value="PCE" />
                                <telerik:DropDownListItem Text="M2" Value="M2" />
                            </Items>
                        </telerik:RadDropDownList>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn DataField="BASE_PRICE" UniqueName="BASE_PRICE" HeaderText="Base Price<br/>(As-Is)" 
                    HeaderStyle-Width="120px" DataType="System.Decimal" HeaderStyle-HorizontalAlign="Right">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("BASE_PRICE")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <%--<telerik:RadNumericTextBox runat="server" ID="RadGrdtxtBasePrice" CssClass="input" InputType="Number" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="100%"></telerik:RadNumericTextBox>--%>
                        <asp:TextBox ID="RadGrdtxtBasePrice" runat="server" Width="100%" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                            DecimalDigits="2" AllowNegative="false">                                
                        </asp:TextBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="BASE_PRICE_TO_BE" UniqueName="BASE_PRICE_TO_BE" HeaderText="Base Price<br/>(To-Be)" 
                    HeaderStyle-Width="120px" DataType="System.Decimal" HeaderStyle-HorizontalAlign="Right">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="Label11" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("BASE_PRICE_TO_BE")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <%--<telerik:RadNumericTextBox runat="server" ID="RadGrdtxtBasePriceToBe" CssClass="input" InputType="Number" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="100%"></telerik:RadNumericTextBox>--%>
                        <asp:TextBox ID="RadGrdtxtBasePriceToBe" runat="server" Width="100%" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                            DecimalDigits="2" AllowNegative="false">                                
                        </asp:TextBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="CHANGE_PRICE" UniqueName="CHANGE_PRICE" HeaderText="Base Price<br/>Discount" 
                    HeaderStyle-Width="120px" DataType="System.Decimal" Display="false">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("CHANGE_PRICE")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <%--                        <telerik:RadNumericTextBox runat="server" ID="RadGrdtxtChangePrice" CssClass="input" InputType="Number" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="100%"></telerik:RadNumericTextBox>--%>
                        <asp:TextBox ID="RadGrdtxtChangePrice" runat="server" Width="100%" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                            DecimalDigits="2" AllowNegative="false">                                
                        </asp:TextBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="DISCOUNT_PRICE" UniqueName="DISCOUNT_PRICE" HeaderText="Discounted<br/>Price" 
                    HeaderStyle-Width="120px" DataType="System.Decimal" Display="false" HeaderStyle-HorizontalAlign="Right">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("DISCOUNT_PRICE")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <%--<telerik:RadNumericTextBox runat="server" ID="RadGrdtxtDiscountPrice" CssClass="input" InputType="Number" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="100%"></telerik:RadNumericTextBox>--%>
                        <asp:TextBox ID="RadGrdtxtDiscountPrice" runat="server" Width="100%" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                            DecimalDigits="2" AllowNegative="false">                                
                        </asp:TextBox>

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
    </telerik:RadGrid>
    <%--    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinPopupCustomer" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer" Width="500px" Height="470px" Behaviors="Default" NavigateUrl="./CustomerList.aspx" OnClientClose="fn_ClientClose"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>--%>
    <div id="hiddenArea" runat="server">
        <input type="hidden" id="hddProcessID" runat="server" />
        <input type="hidden" id="hddProcessStatus" runat="server" />
        <input type="hidden" id="hddDocumentID" runat="server" />
        <input type="hidden" id="hddGridItems" runat="server" />
        <input type="hidden" id="hddCustomerCode" runat="server" />
        <input type="hidden" id="hddCompanyCode" runat="server" />
        <input type="hidden" id="hddReuse" runat="server" />
        <input type="hidden" id="hddAddRow" runat="server" value="N" />
    </div>
</asp:Content>

