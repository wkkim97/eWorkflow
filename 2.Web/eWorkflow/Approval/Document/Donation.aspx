<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="Donation.aspx.cs" Inherits="Approval_Document_Donation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function fn_UpdateGridDataFromUploader() {
            fn_UpdateGridData(false);
        }

        function fn_DoRequest(sender, args) {
            return fn_UpdateGridData(false);
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData(true);
        }

        function checkLastRow() {
            var masterTable = $find('<%= radGrdProduct.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();


            if (dataItems.length > 0) {
                var lastItem = dataItems[dataItems.length - 1];

                var product = lastItem.get_cell("PRODUCT_NAME").children[0].innerText;
                var fairMarket = lastItem.get_cell("FAIR_MARKET_VALUE").children[0].innerText;
                var quantity = lastItem.get_cell("QTY").children[0].innerText;
                if (product.length < 1 || fairMarket.length < 1 || quantity.length < 1) {
                    fn_OpenInformation('자료를 입력바랍니다.');
                    return false;
                }
            }
            return true;
        }

        function fn_UpdateGridData(addRow) {
            var list = [];
            var masterTable = $find('<%= radGrdProduct.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();

            var maxIdx = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var idx = dataItems[i].get_cell("IDX").innerText;
                var productcode = dataItems[i].get_cell("PRODUCT_CODE").innerText;
                var ctrlBu = dataItems[i].findControl('radGrdDropBuList');
                var bu = null;
                if (ctrlBu)
                    bu = ctrlBu._selectedText;
                else
                    bu = dataItems[i].get_cell("BU").children[0].innerText;
                var product = dataItems[i].get_cell("PRODUCT_NAME").children[0].innerText;
                var fairMarket = dataItems[i].get_cell("FAIR_MARKET_VALUE").children[0].innerText;
                var quantity = dataItems[i].get_cell("QTY").children[0].innerText;
                var amount = dataItems[i].get_cell("AMOUNT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var conObj = {
                    IDX: null,
                    BU: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    FAIR_MARKET_VALUE: null,
                    QTY: null,
                    AMOUNT: null,

                }
                conObj.IDX = idx;
                conObj.BU = bu;
                conObj.PRODUCT_CODE = productcode;
                conObj.PRODUCT_NAME = product;
                conObj.FAIR_MARKET_VALUE = fairMarket.replace(/,/gi, '').replace(/ /gi, '');
                conObj.QTY = quantity.replace(/,/gi, '').replace(/ /gi, '');
                conObj.AMOUNT = amount.replace(/,/gi, '').replace(/ /gi, '');
                maxIdx = idx;
                list.push(conObj);
            }

            if (addRow) {
                var conObj = {
                    IDX: null,
                    BU: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    FAIR_MARKET_VALUE: null,
                    QTY: null,
                    AMOUNT: null,

                }
                conObj.IDX = ++maxIdx;
                conObj.BU = '';
                conObj.PRODUCT_CODE = '';
                conObj.PRODUCT_NAME = '';
                conObj.FAIR_MARKET_VALUE = 0;
                conObj.QTY = 0;
                conObj.AMOUNT = 0;
                list.push(conObj);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

            return true;
        }


        function fn_OnAddButtonClicked(sender, args) {
            if (checkLastRow()) {
                fn_UpdateGridData(true);
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind");
            }
        }


        function OnClientCheckedChanged(sender, args) {
            var checked = sender.get_checked();
            var product;

            if ($find('<%= radChkType2.ClientID%>').get_checked())
                product = "Y";
            else
                product = "N";

            setVisibleControl(product)
        }

        function setVisibleControl(product) {
            $('#divProduct').hide();

            if (product == "Y")
                $('#divProduct').show();
            else $('#divProduct').hide();

        }

        var currentIdx = null;
        function fn_OpenProduct(sender, args) {

            var masterTable = $find('<%= radGrdProduct.ClientID%>').get_masterTableView();
            var num = masterTable.get_dataItems().length;
            var wnd = $find("<%= radWinPopupProduct.ClientID %>");
            var row = $find('<%= radGrdProduct.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            var dataItem = $find(row.id);

            var selectedValue = null;
            for (var i = 0; i < num; i++) {
                var type = masterTable.get_dataItems()[i].findControl('radGrdDropBuList');
                if (type) {
                    selectedValue = type.get_selectedItem().get_value();
                    currentIdx = dataItem.get_cell('IDX').innerText;
                    //currentIdx = masterTable.get_dataItems()[i].get_cell('IDX').innerText;
                    break;
                }
            }

            if (selectedValue) {
                wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=" + selectedValue + "&baseprice=Y");
                wnd.show();
                sender.set_autoPostBack(false);
            }
            else {
                fn_OpenInformation('Please Select a BU');
                sender.set_autoPostBack(false);
            }
        }

        function isDuplication(code) {
            var masterTable = $find('<%= radGrdProduct.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();

            for (var i = 0; i < dataItems.length; i++) {
                var proCode = dataItems[i].get_cell('PRODUCT_CODE').innerText.trim();

                if (proCode == code) return true;
            }

            return false;
        }

        function fn_ClientClose(oWnd, args) {
            var item = args.get_argument();
            var masterTable = $find('<%= radGrdProduct.ClientID%>').get_masterTableView();
            var num = masterTable.get_dataItems().length;
            var dataItems = masterTable.get_dataItems();
            var lastItem = dataItems[dataItems.length - 1];

            if (item != null) {
                if (!isDuplication(item.PRODUCT_CODE)) {

                    fn_UpdateGridData(false);
                    $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("ApplyProduct:" + currentIdx + ":" + item.PRODUCT_CODE + ":" + item.PRODUCT_NAME + "(" + item.PRODUCT_CODE + ")");
                } else
                    fn_OpenInformation('동일한 product가 존재합니다.');
            }
            else {
                oWnd.close();
            }
        }

        var clickedKey = null;
        function openConfirmPopUp(index) {
            clickedKey = parseInt(index);
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);

            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                fn_UpdateGridData(false);
                var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        function fn_keyPress(sender, args) {
            var text = sender.get_value() + args.get_keyCharacter();
            if (!text.match('^[0-9-,]+$'))
                args.set_cancel(true);
        }

        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdProduct.ClientID%>');

            var dataItems = grid.get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                if (args.get_eventArgument().indexOf('ApplyProduct') == 0) {
                    var numbox = dataItems[dataItems.length - 1].findElement('radGrdNumFairMarketValue');
                    setTimeout(function () {
                        if (numbox) numbox.focus();
                    }, 100);

                    //if (numbox) {

                    //    dataItems[dataItems.length - 1].get_cell('FAIR_MARKET_VALUE').focus();
                    //    //numbox.focus();
                    //}
                }

            }
        }


        function SetTotal() {
            var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var total = 0;
            for (var i = 0; i < dataItems.length; i++) {

                var ctlPrice = dataItems[i].findElement('radGrdNumFairMarketValue');
                var ctlQty = dataItems[i].findElement('radGrdNumQty');
                var price = 0, qty = 0, amount = 0;
                if (ctlPrice && ctlQty) {
                    price = ctlPrice.value.replace(/,/gi, '').replace(/ /gi, '');
                    qty = ctlQty.value.replace(/,/gi, '').replace(/ /gi, '');
                } else {
                    price = dataItems[i].get_cell("FAIR_MARKET_VALUE").innerText.replace(/,/gi, '').replace(/ /gi, '');
                    qty = dataItems[i].get_cell("QTY").innerText.replace(/,/gi, '').replace(/ /gi, '');
                }
                amount = (parseFloat(price) * parseFloat(qty));
                dataItems[i].get_cell('AMOUNT').innerText = amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                total += amount;
            }

            masterTable.get_element().tFoot.rows[0].cells[6].innerText = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function fn_OnGridNumBlur(sender) {
            setNumberFormat(sender);
        }

        function fn_OnGridKeyUp(sender) {
            SetTotal();
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <!-- doc Style -->
    <div class="doc_style">
        <h3>The Purpose of Donations</h3>
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
                            <telerik:RadButton runat="server" ID="radChkType1" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Monetary Donation" Value="Monetary Donation" OnClientCheckedChanged="OnClientCheckedChanged" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkType2" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Product Donation" Value="Product Donation" OnClientCheckedChanged="OnClientCheckedChanged" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkType3" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Other Goods and Services" Value="Other Goods and Services" OnClientCheckedChanged="OnClientCheckedChanged" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Value[KRW] <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadNumericTextBox ID="radTxtValue" runat="server" Width="50%" NumberFormat-DecimalDigits="0" Value="0"></telerik:RadNumericTextBox>
                            <br />
                            - 물품(제품)을 기부하는 경우, 기부금액을 시장가(권장소비자가)로 산정합니다.<br />
                            &nbsp;&nbsp;In case of non-monetary donation, the value shall be calculated with fair market value
                            <br />
                            - EUR 10,000초과 시 Global Subgroup Head 또는 / 그리고 Management Board Chairperson of Bayer AG의 승인필요
                        </td>
                    </tr>
                    <tr>
                        <th>Purpose</th>
                        <td>
                            <telerik:RadButton runat="server" ID="radChkPurpose1" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Science & Education" Value="Science Education" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkPurpose2" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Health & Social" Value="Health Social" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkPurpose3" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Sports, Culture, Municipal" Value="Sports Culture Municipal" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Explanation</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="radTxtExplanation" Width="80%"></telerik:RadTextBox></td>
                    </tr>
                </tbody>
            </table>
        </div>

        <h3>Recipient Information</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col style="width: 25%;" />
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Recipient <span class="text_red">*</span></th>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="radTxtRecipient" Width="80%"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <th>Address</th>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="radTxtAddress" Width="80%"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <th>Tel.</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="radTxtTel" Width="90%">
                                <ClientEvents OnKeyPress="fn_keyPress" />
                            </telerik:RadTextBox></td>
                        <th>E-mail</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="radTxtEmail" Width="90%"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <th>Category <span class="text_red">*</span></th>
                        <td colspan="3">
                            <telerik:RadButton runat="server" ID="radChkHealthCare" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Healthcare Professional Organization" Value="Healthcare Professional Organization" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkEducational" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Educational Organization" Value="Educational Organization" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkCharity" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Charity Organization" Value="Charity Organization" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkOthers" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Others" Value="Others" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>

                </tbody>
            </table>
        </div>

        <div class="data_type1" id="divProduct" style="display: none">
            <h3>Product Information
					<div class="title_btn">
                        <telerik:RadButton ID="radBtnAdd" runat="server" Text="Add" OnClientClicked="fn_OnAddButtonClicked"
                            ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" AutoPostBack="false">
                        </telerik:RadButton>
                    </div>
            </h3>
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Location</th>
                        <td>
                            <telerik:RadButton runat="server" ID="radRdoLocation1" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="서울" Value="서울" GroupName="Location" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoLocation2" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="대전" Value="대전" GroupName="Location" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoLocation3" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="대구" Value="대구" GroupName="Location" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoLocation4" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="부산" Value="부산" GroupName="Location" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoLocation5" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="광주" Value="광주" GroupName="Location" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoLocation6" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="안성공장" Value="안성공장" GroupName="Location" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoLocation7" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="기타" Value="기타" GroupName="Location" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                </tbody>
            </table>
            <telerik:RadGrid ID="radGrdProduct" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                OnItemCommand="radGrdProduct_ItemCommand" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" GridLines="None">
                <MasterTableView EditMode="Batch" ClientDataKeyNames="IDX">
                    <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                    <HeaderStyle CssClass="grid_header" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="IDX" HeaderText="" HeaderStyle-Width="40px" UniqueName="IDX" 
                            ReadOnly="true" Display="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="PRODUCT_CODE" HeaderText="Product Code" HeaderStyle-Width="40px"
                            UniqueName="PRODUCT_CODE" ReadOnly="true" Display="false">
                            <ItemTemplate>
                                <%# Eval("PRODUCT_CODE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="BU" UniqueName="BU" HeaderText="BU" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <%# Eval("BU")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="radGrdDropBuList" runat="server" Width="100%" DropDownWidth="50px">
                                    <Items>
                                        <telerik:DropDownListItem Text="HH" Value="HH" />
                                        <telerik:DropDownListItem Text="WH" Value="WH" />
                                        <telerik:DropDownListItem Text="SM" Value="SM" />
                                        <telerik:DropDownListItem Text="R" Value="R" />
                                        <telerik:DropDownListItem Text="CC" Value="CC" />
                                        <telerik:DropDownListItem Text="DC" Value="DC" />
                                        <telerik:DropDownListItem Text="AH" Value="AH" />
                                        <telerik:DropDownListItem Text="BVS" Value="BVS" />
                                        <telerik:DropDownListItem Text="CP" Value="CP" />
                                        <telerik:DropDownListItem Text="ES" Value="ES" />
                                        <telerik:DropDownListItem Text="IS" Value="IS" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="PRODUCT_NAME" UniqueName="PRODUCT_NAME" HeaderText="Product" HeaderStyle-Width="100%">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "PRODUCT_NAME")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="radGrdTxtProduct" runat="server" ReadOnly="true" AutoPostBack="false" Width="85%">
                                </telerik:RadTextBox>
                                <telerik:RadButton ID="radGrdBtnProduct" runat="server" AutoPostBack="false" Width="18px" Height="18px" CssClass="btn_grid" OnClientClicked="fn_OpenProduct">
                                    <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                                </telerik:RadButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="FAIR_MARKET_VALUE" HeaderText="Fair Market Value (KRW)"
                            UniqueName="FAIR_MARKET_VALUE" HeaderStyle-Width="120px">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("FAIR_MARKET_VALUE")) %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--                                <telerik:RadNumericTextBox runat="server" ID="radGrdNumFairMarketValue" NumberFormat-DecimalDigits="0"
                                    Width="100%" EnabledStyle-HorizontalAlign="Right" ClientEvents-OnBlur="fn_OnGridNumBlur">
                                </telerik:RadNumericTextBox>--%>
                                <asp:TextBox ID="radGrdNumFairMarketValue" runat="server" Width="100%" CssClass="input align_right"
                                    onblur="return fn_OnGridNumBlur(this)"
                                    onfocus="return fn_OnGridNumFocus(this)"
                                    onkeypress="return fn_OnGridKeyPress(this, event)"
                                    onkeyup="return fn_OnGridKeyUp(this)"
                                    DecimalDigits="0" AllowNegative="false">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="QTY" HeaderText="Qty" UniqueName="QTY" HeaderStyle-Width="120px">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("QTY")) %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--                                <telerik:RadNumericTextBox runat="server" ID="radGrdNumQty" NumberFormat-DecimalDigits="0"
                                    Width="100%" EnabledStyle-HorizontalAlign="Right" ClientEvents-OnBlur="fn_OnGridNumBlur">
                                </telerik:RadNumericTextBox>--%>
                                <asp:TextBox ID="radGrdNumQty" runat="server" Width="100%" CssClass="input align_right"
                                    onblur="return fn_OnGridNumBlur(this)"
                                    onfocus="return fn_OnGridNumFocus(this)"
                                    onkeypress="return fn_OnGridKeyPress(this, event)"
                                    onkeyup="return fn_OnGridKeyUp(this)"
                                    DecimalDigits="0" AllowNegative="false">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridCalculatedColumn HeaderText="Amount" UniqueName="AMOUNT" DataType="System.Double" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right"
                            DataFields="FAIR_MARKET_VALUE, QTY" Expression='{0}*{1}' Aggregate="Sum" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}"
                            FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                        </telerik:GridCalculatedColumn>--%>
                        <telerik:GridTemplateColumn DataField="AMOUNT" HeaderText="Amount" UniqueName="AMOUNT" HeaderStyle-Width="150px" ReadOnly="true"
                            Aggregate="Sum" FooterAggregateFormatString="{0:#,##0}"
                            FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("AMOUNT"))  %>' CssClass="align_right"></asp:Label>
                            </ItemTemplate>
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
        </div>
    </div>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinPopupProduct" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product" Width="500px" Height="500px" Behaviors="Default" OnClientClose="fn_ClientClose" NavigateUrl="./ProductList.aspx"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <!-- //doc Style -->
    <div id="hiddenArea" runat="server">
        <input type="hidden" id="hddProcessID" runat="server" />
        <input type="hidden" id="hddProcessStatus" runat="server" />
        <input type="hidden" id="hddDocumentID" runat="server" />
        <input type="hidden" id="hddGridItems" runat="server" />
        <input type="hidden" id="hddProductCode" runat="server" />
        <input type="hidden" id="hddReuse" runat="server" />
        <input type="hidden" id="hddAddRow" runat="server" value="N" />
    </div>

</asp:Content>

