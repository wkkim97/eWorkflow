<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="WholesalerMarginAgreement.aspx.cs" Inherits="Approval_Document_WholesalerMarginAgreement" %>

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

        function getTypeValue() {
            var selectedValue;
            if ($find('<%= radBtnTender.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnTender.ClientID %>').get_value();
            else if ($find('<%= radBtnNormal.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnNormal.ClientID %>').get_value();
            else if($find('<%= radBtnBidding.ClientID%>').get_checked())
                selectedValue = $find('<%= radBtnBidding.ClientID %>').get_value();
            else if ($find('<%= radBtnMax.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnMax.ClientID %>').get_value();
            else if ($find('<%= radBtnVol.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnVol.ClientID %>').get_value();
            else if ($find('<%= radBtnQC.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnQC.ClientID %>').get_value();
            else if ($find('<%= radBtnSpc.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnSpc.ClientID %>').get_value();      
        return selectedValue;
    }

    function getSelectedBU() {
        var controls = $('#<%= divSalesGroup.ClientID %>').children();
        var selectedValue;

        for (var i = 0; i < controls.length; i++) {
            var bu = controls[i];
            if ($find(bu.id).get_checked()) {
                selectedValue = $find(bu.id).get_value();
                break;
            }
        }
        return selectedValue;
    }

    function getTypeValueAH() {
        var controls = $('#<%= divAH.ClientID %>').children();
            var selectedValue;

            for (var i = 0; i < controls.length; i++) {
                var type = controls[i];
                if ($find(type.id).get_checked()) {
                    selectedValue = $find(type.id).get_value();
                    break;
                }
            }
            return selectedValue;
        }

        function fn_OnWholeSalerRequesting(sender, args) {
            var selectedType = getTypeValue();
            var seledtedTypeAH = getTypeValueAH();
            var selectedValue = getSelectedBU();


            if (selectedType && selectedValue || selectedValue == 'AH' && seledtedTypeAH) {
                var context = args.get_context();
                context["company"] = $('#<%= hddCompanyCode.ClientID %>').val();
            context["bu"] = selectedValue;
            context["parvw"] = "IE";
            context["level"] = "Y";
        } else {
            fn_OpenDocInformation('Please fill out below fields. <br/><hr>Type&amp;Sales Group');
            args.set_cancel(true);
        }
    }

    function fn_OnHospitalRequesting(sender, args) {
        var selectedValue = getSelectedBU();

        if (selectedValue) {
            var context = args.get_context();
            context["company"] = $('#<%= hddCompanyCode.ClientID %>').val();
                context["bu"] = "";
                context["parvw"] = "IF";
                context["level"] = "N";
            } else {
                fn_OpenDocInformation('Please fill out below fields. <br/><hr>Sales Group');
                args.set_cancel(true);
            }
        }

        function checkLastProduct() {
            var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            if (dataItems.length > 0) {
                var lastItem = dataItems[dataItems.length - 1];
            }
            return true;
        }

        function fn_UpdateGridData(product) {
            var list = [];
            var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
            $find('<%= radGrdProduct.ClientID %>').get_batchEditingManager().saveChanges(masterTable);
            var dataItems = masterTable.get_dataItems();

            var maxIdx = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var idx = dataItems[i].get_cell("IDX").innerText;
                var code = dataItems[i].get_cell("PRODUCT_CODE").innerText;
                var name = dataItems[i].get_cell("PRODUCT_NAME").innerText;
                var basePrice = dataItems[i].get_cell("BASE_PRICE").innerText.replace(/,/gi, '').replace(/ /gi, '');
                var marginAsIs = dataItems[i].get_cell("MARGIN_AS_IS").innerText.replace(/,/gi, '').replace(/ /gi, '');
                var marginToBe = dataItems[i].get_cell("MARGIN_TO_BE").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var discountAmt = dataItems[i].get_cell("DISCOUNT_AMOUNT").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var qty = dataItems[i].get_cell("QTY").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var remark = dataItems[i].get_cell("REMARKS").children[0].innerText;

                var conObj = {
                    IDX:null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    BASE_PRICE: null,
                    MARGIN_AS_IS: null,
                    MARGIN_TO_BE: null,
                    DISCOUNT_AMOUNT: null,
                    QTY: null,
                    REMARKS: null,
                }
                conObj.IDX = idx;
                conObj.PRODUCT_CODE = code;
                conObj.PRODUCT_NAME = name;
                conObj.BASE_PRICE = basePrice;
                conObj.MARGIN_AS_IS = marginAsIs;
                conObj.MARGIN_TO_BE = marginToBe;
                conObj.DISCOUNT_AMOUNT = discountAmt;
                conObj.QTY = qty;
                conObj.REMARKS = remark;
                list.push(conObj);
                maxIdx = idx;
            }

            if (product) {
                var conObj = {
                    IDX:null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    BASE_PRICE: null,
                    MARGIN_AS_IS: null,
                    MARGIN_TO_BE: null,
                    DISCOUNT_AMOUNT: null,
                    QTY: null,
                    REMARKS: null,
                }
                conObj.IDX = ++maxIdx;
                conObj.PRODUCT_CODE = product.PRODUCT_CODE.toString();
                conObj.PRODUCT_NAME = product.PRODUCT_NAME.trim() + "(" + product.PRODUCT_CODE + ")";
                conObj.BASE_PRICE = product.BASE_PRICE;
                conObj.MARGIN_AS_IS = product.MARGIN;
                conObj.MARGIN_TO_BE = 0;
                conObj.DISCOUNT_AMOUNT = 0;
                conObj.QTY = 0;
                conObj.REMARKS = '';
                list.push(conObj);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));
            return true;
        }

        function fn_PopupProduct(sender, args) {
            var selectedValue = getSelectedBU();
            var selectedType = getTypeValue();
            if (selectedValue && selectedType) {
                if (checkLastProduct()) {
                    var wnd = $find("<%= radWinProduct.ClientID %>");
                    wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=" + selectedValue + "&baseprice=Y");
                    wnd.show();
                    sender.set_autoPostBack(false);
                } else {
                    fn_OpenDocInformation('자료를 입력바랍니다.');
                    sender.set_autoPostBack(false);
                }
            } else {
                fn_OpenDocInformation('Please fill out below fields. <br/><hr>Sales Group Or Type');
            }
        }

        function fn_OnSelectedProduct(sender, args) {
            var product = args.get_argument();
            if (product != null) {
                fn_UpdateGridData(product);
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind");
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

        function openGridRowForEdit(sender, args) {
            if (args.get_eventArgument() == 'Rebind') {
                var grid = $find('<%=radGrdProduct.ClientID%>');

                if (grid.get_masterTableView()) {
                    var dataItems = grid.get_masterTableView().get_dataItems();
                    if (dataItems.length > 0) {
                        grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                    }
                }
            }
        }

        function fn_RadioVisible(sender, args) {
            var type = sender.get_value();
            setVisible(type);
        }
        function setVisible(type) {
            if (type == 'AH') {
                $('#typeAH').show();
                $('#typeNonAH').hide();
            }
            if (type != 'AH') {
                $('#typeNonAH').show();
                $('#typeAH').hide();
            }
        }
        function fn_OnGridNumBlur(sender) {
            var strNum = sender.value;
            var number = 0;
            if (!isNaN(parseFloat(strNum)) && isFinite(strNum))
                number = sender.value;
            if (number > 99) sender.value = 99;
            setNumberFormat(sender);
        }

        function fn_OnGridNumBlur2(sender) {
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
                <tr>
                    <th>BU <span class="text_red">*</span>
                    </th>
                    <td>
                        <div id="divSalesGroup" runat="server" style="margin: 0 0 0 0">
                            <telerik:RadButton ID="radBtnSM" runat="server" Text="SM" Value="SM" GroupName="SalesGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnHH" runat="server" Text="HH" Value="HH" GroupName="SalesGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnWH" runat="server" Text="WH" Value="WH" GroupName="SalesGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnRI" runat="server" Text="R" Value="R" GroupName="SalesGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnCC" runat="server" Text="CC" Value="CC" GroupName="SalesGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnDC" runat="server" Text="DC" Value="DC" GroupName="SalesGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnAH" runat="server" Text="AH" Value="AH" GroupName="SalesGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible">
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr id="typeAH" style="display: none">
                    <th>Type <span class="text_red">*</span></th>
                    <td>
                        <div id="divAH" runat="server" style="margin: 0 0 0 0">
                            <telerik:RadButton ID="radBtnMax" runat="server" Text="Maximum" Value="Maximum" GroupName="AH"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnVol" runat="server" Text="Volume" Value="Volume" GroupName="AH"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnQC" runat="server" Text="Quarterly/Campaign" Value="QC" GroupName="AH"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnSpc" runat="server" Text="Special" Value="Special" GroupName="AH"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click">
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr id="typeNonAH" style="display: none">
                    <th>Type <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadButton ID="radBtnTender" runat="server" Text="Tender" Value="Tender" GroupName="Type"
                            ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnNormal" runat="server" Text="Normal" Value="Normal" GroupName="Type"
                            ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnBidding" runat="server" Text="Bidding" Value="Bidding" GroupName="Type"
                            ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click">
                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
        </div>
        <h3>Wholesaler and Hospital Information</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Wholesaler Name
                    </th>
                    <td>
                        <telerik:RadAutoCompleteBox ID="radAcomWholesaler" runat="server" AllowCustomEntry="false" Width="100%" DropDownWidth="300px"
                            OnClientRequesting="fn_OnWholeSalerRequesting">
                            <WebServiceSettings Method="SearchCustomer" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                        </telerik:RadAutoCompleteBox>
                        <asp:Label ID="lblNotWholesaler" runat="server" Width="100%" Visible="false">Refer to the wholesaler list attached.</asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>Hospital Name</th>
                    <td>
                        <telerik:RadAutoCompleteBox ID="radAcomHospital" runat="server" AllowCustomEntry="false" Width="100%" DropDownWidth="300px"
                            OnClientRequesting="fn_OnHospitalRequesting">
                            <WebServiceSettings Method="SearchCustomer" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                        </telerik:RadAutoCompleteBox>
                        <asp:Label ID="lblNotHospital" runat="server" Width="100%" Visible="false">Refer to the hospital list attached.</asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>Contract Period <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadDatePicker ID="radDatPeriodFrom" runat="server" Width="100px" Calendar-ShowRowHeaders="false" MaxDate="2050-12-31" MinDate="1900-01-01" Culture="ko-KR">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                </SpecialDays>
                            </Calendar>
                        </telerik:RadDatePicker>
                        ~
                        <telerik:RadDatePicker ID="radDatPeriodTo" runat="server" Width="100px" Calendar-ShowRowHeaders="false" MaxDate="2050-12-31" MinDate="1900-01-01" Culture="ko-KR">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                </SpecialDays>
                            </Calendar>
                        </telerik:RadDatePicker>
                    </td>
                </tr>
            </table>
        </div>
        <h3>Products List
            <div class="title_btn">
                <telerik:RadButton ID="radBtnAddTrip" runat="server" Text="Add" AutoPostBack="false" OnClientClicked="fn_PopupProduct"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size1 bold">
                </telerik:RadButton>
            </div>
        </h3>
        <telerik:RadGrid ID="radGrdProduct" runat="server" AutoGenerateColumns="false"
            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" AllowSorting="false" GridLines="None"
            OnItemCommand="radGrdProduct_ItemCommand">
            <HeaderStyle CssClass="grid_header" HorizontalAlign="Center" />
            <MasterTableView EditMode="Batch">
                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderText="IDX" HeaderStyle-Width="40px" UniqueName="IDX" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PRODUCT_CODE" HeaderText="COde" HeaderStyle-Width="40px" UniqueName="PRODUCT_CODE" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PRODUCT_NAME" HeaderText="Product Name" UniqueName="PRODUCT_NAME" ReadOnly="true"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="BASE_PRICE" HeaderText="Base Price<br/>(-vat)" HeaderStyle-Width="80px" UniqueName="BASE_PRICE" ReadOnly="true"
                        DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="MARGIN_AS_IS" HeaderText="Margin%<br/>(As-Is)" HeaderStyle-Width="80px" UniqueName="MARGIN_AS_IS" ReadOnly="true"
                        ItemStyle-HorizontalAlign="Right">                        
                        <ItemTemplate>
                            <asp:Label ID="Label0" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("MARGIN_AS_IS")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumMarginAsIs" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeypress="return fn_OnGridKeyPress(this, event)"                                
                                DecimalDigits="2" AllowNegative="false">                                
                            </asp:TextBox>

                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="MARGIN_TO_BE" HeaderText="Margin%<br/>(To-Be)" UniqueName="MARGIN_TO_BE" HeaderStyle-Width="80px">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("MARGIN_TO_BE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumMargin" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeypress="return fn_OnGridKeyPress(this, event)"                                
                                DecimalDigits="2" AllowNegative="false">                                
                            </asp:TextBox>

                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="DISCOUNT_AMOUNT" HeaderText="Net Selling Price" UniqueName="DISCOUNT_AMOUNT"
                        HeaderStyle-Width="110px" Display="false">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("DISCOUNT_AMOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumDiscount" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur2(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeypress="return fn_OnGridKeyPress(this, event)"                                
                                DecimalDigits="0" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="QTY" HeaderText="Qty" UniqueName="QTY" HeaderStyle-Width="80px" Display="false">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("QTY")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadNumericTextBox runat="server" ID="radNumQty" CssClass="input" NumberFormat-DecimalDigits="0" Width="100%"
                                EnabledStyle-HorizontalAlign="Right">
                            </telerik:RadNumericTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridNumericColumn DataField="AMOUNT" UniqueName="AMOUNT" HeaderText="Amount" HeaderStyle-Width="100px"
                        Aggregate="Sum" DataType="System.Decimal" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}"
                        FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red" Display="false" ReadOnly="true">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridNumericColumn>
                    <telerik:GridTemplateColumn UniqueName="REMARKS" HeaderText="Remarks" HeaderStyle-Width="150px">
                        <ItemTemplate><%#DataBinder.Eval(Container.DataItem, "REMARKS")%></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRemark" runat="server" Width="100%" CssClass="input"></asp:TextBox>
                            <%--<telerik:RadTextBox ID="radGrdTxtRemark" runat="server" Width="100%"></telerik:RadTextBox>--%>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" OnClientClick='<%# String.Format("return openConfirmPopUp(\"{0}\");",Eval("IDX"))%> '
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
                </colgroup>
                <tr>
                    <th>Total Amount
                    </th>
                    <td>
                        <telerik:RadNumericTextBox ID="radNumTotalAmount" runat="server" NumberFormat-DecimalDigits="0"
                            EnabledStyle-HorizontalAlign="Right">
                        </telerik:RadNumericTextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinProduct" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product"
                Width="500px" Height="600px"
                Behaviors="Default" Modal="true" CssClass="windowscroll" OnClientClose="fn_OnSelectedProduct" NavigateUrl="/eWorks/Common/Popup/ProductList.aspx">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <input type="hidden" id="hddCompanyCode" runat="server" />
    <input type="hidden" id="hddGridItems" runat="server" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="HddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

