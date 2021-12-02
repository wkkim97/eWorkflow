<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="BmskRebate.aspx.cs" Inherits="Approval_Document_BmskRebate" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function fn_UpdateGridDataFromUploader() {
            fn_UpdateGridData(false);
        }

        function fn_DoRequest(sender, args) {
            return fn_UpdateGridData(null);
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData(null);
        }

        function fn_OnCustomerRequesting(sender, args) {
            var context = args.get_context();
            context["company"] = $('#<%= hddCompanyCode.ClientID %>').val();
            context["bu"] = '';
            context["parvw"] = '';
            context["level"] = 'N';
        }

        //PRODUCT POPUP
        function fn_PopupItem(sender, args) {
            fn_UpdateGridData(null);
            var dataItems = $find('<%= radGrdRebateProduct.ClientID %>').get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                for (var i = 0; i < dataItems.length; i++) {

                    var productcode = dataItems[i].get_cell("PRODUCT_CODE").innerText.trim();
                    var productname = dataItems[i].get_cell("PRODUCT_NAME").innerText.trim();
                    var accrual = dataItems[i].get_cell("ACCRUAL").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');


                    if (productcode == '' || productname == '' || accrual == '' || accrual == 0) {
                        fn_OpenDocInformation('자료를 입력하시기 바랍니다.');
                        return false;
                    }
                }
            }
            var bu = '';
            var btnPCS = $find('<%= radRdoBu1.ClientID %>');
            var btnPUR = $find('<%= radRdoBu2.ClientID %>');
            if (btnPCS.get_checked()) bu = btnPCS.get_value();
            else if (btnPUR.get_checked()) bu = btnPUR.get_value();

            if (bu == '') {
                fn_OpenDocInformation('BU를 선택바랍니다.');
                return false;
            } else {
                var wnd = $find("<%= radWinBmskRebateProduct.ClientID %>");
                wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=" + bu);
                wnd.show();
            }
        }

        function fn_setItem(oWnd, args) {
            var item = args.get_argument();
            if (item != null) {
                fn_UpdateGridData(item);
                $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("Rebind");
            }
        }

        function fn_UpdateGridData(data) {
            var list = [];
            var masterTable = $find('<%= radGrdRebateProduct.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var is_val = false;
            var maxIdx = 0;
            for (var i = 0; i < dataItems.length; i++) {

                var idx = dataItems[i].get_cell("IDX").innerText.trim();
                var productcode = dataItems[i].get_cell("PRODUCT_CODE").innerText.trim();
                var productname = dataItems[i].get_cell("PRODUCT_NAME").innerText.trim();
                //var accrual = '0';
                //if (dataItems[i].findControl('RadGrdtxtAccrual'))
                //    accrual = dataItems[i].findControl('RadGrdtxtAccrual')._text.replace(/,/gi, '').replace(/ /gi, '');
                //else
                //    accrual = dataItems[i].get_cell("ACCRUAL").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var accrual = dataItems[i].get_cell("ACCRUAL").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var item = {
                    IDX: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    ACCRUAL: null,
                }
                maxIdx = idx;
                item.IDX = parseInt(idx);
                item.PRODUCT_CODE = productcode;
                item.PRODUCT_NAME = productname;
                item.ACCRUAL = accrual;
                list.push(item);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

            if (data) {
                var item = {
                    IDX: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    ACCRUAL: null,
                }
                for (var i = 0; i < dataItems.length; i++) {
                    var productcode = dataItems[i].get_cell("PRODUCT_CODE").innerText.trim();
                    var productname = dataItems[i].get_cell("PRODUCT_NAME").innerText.trim();

                    if (data.PRODUCT_CODE == productcode && data.PRODUCT_NAME == productname) {
                        fn_OpenDocInformation('동일한 product 가 존재합니다.');
                        var is_val = true;
                        break;
                    }
                }
                if (is_val == false) {
                    maxIdx++;
                    item.IDX = parseInt(maxIdx);
                    item.PRODUCT_CODE = data.PRODUCT_CODE;
                    item.PRODUCT_NAME = data.PRODUCT_NAME;
                    item.ACCRUAL = 0;
                    list.push(item);
                    $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

                }
                is_val = false;
            }
            return true;
        }

        function openConfirmPopUp(index) {
            clickedKey = parseInt(index);
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);

            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                var masterTable = $find('<%= radGrdRebateProduct.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdRebateProduct.ClientID%>');

            var dataItems = grid.get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
            }
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
                        <th>Subject <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadTextBox ID="radTxtSubject" runat="server" Width="98%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>BU</th>
                        <td>
                            <telerik:RadButton runat="server" ID="radRdoBu1" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="PCS" Value="PCS" GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoBu2" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="PUR" Value="PUR" GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Customer</th>
                        <td>
                            <telerik:RadAutoCompleteBox ID="radAcomCustomer" runat="server" AllowCustomEntry="false" Width="100%" DropDownWidth="300px"
                                OnClientRequesting="fn_OnCustomerRequesting">
                                <WebServiceSettings Method="SearchCustomer" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                            </telerik:RadAutoCompleteBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Validity (From)</th>
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
                        </td>
                    </tr>
                    <tr>
                        <th>Validity (To)</th>
                        <td>
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
                    <tr>
                        <th>Target Performance</th>
                        <td>
                            <telerik:RadTextBox ID="radTxtTarget" runat="server" Width="98%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Estimated Perfomance</th>
                        <td>
                            <telerik:RadTextBox ID="radTxtEstimated" runat="server" Width="98%"></telerik:RadTextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <h3>
        <div class="title_btn">
            <telerik:RadButton ID="radBtnAdd" runat="server" Text="Add" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" AutoPostBack="false"
                ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" OnClientClicked="fn_PopupItem">
            </telerik:RadButton>
        </div>
    </h3>
    <telerik:RadGrid ID="radGrdRebateProduct" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" GridLines="None" AllowAutomaticUpdates="true" OnItemCommand="radGrdRebateProduct_ItemCommand">
        <MasterTableView EnableHeaderContextMenu="true" EditMode="Batch" DataKeyNames="IDX">
            <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
            <Columns>
                <telerik:GridBoundColumn DataField="IDX" HeaderStyle-Width="30px" UniqueName="IDX" ReadOnly="true"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PRODUCT_CODE" HeaderText="" HeaderStyle-Width="40px" UniqueName="PRODUCT_CODE" Display="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PRODUCT_NAME" HeaderText="Material" UniqueName="PRODUCT_NAME" HeaderStyle-Width="60%" ReadOnly="true" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="ACCRUAL" UniqueName="ACCRUAL" HeaderText="Accrual (%)" HeaderStyle-Width="200px" DataType="System.Decimal">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("ACCRUAL")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <%--<telerik:RadNumericTextBox runat="server" ID="RadGrdtxtAccrual" CssClass="input" InputType="Number" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="100%"></telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="radGrdNumAmount" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" 
                                onfocus="return fn_OnGridNumFocus(this)" 
                                onkeypress="return fn_OnGridKeyPress(this, event)"
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
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinBmskRebateProduct" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product" Width="520px" Height="580px" Behaviors="Default" NavigateUrl="./ProductList.aspx" OnClientClose="fn_setItem"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div id="hiddenArea" runat="server">
        <input type="hidden" id="hddProcessID" runat="server" />
        <input type="hidden" id="hddProcessStatus" runat="server" />
        <input type="hidden" id="hddDocumentID" runat="server" />
        <input type="hidden" id="hddGridItems" runat="server" />
        <input type="hidden" id="hddCompanyCode" runat="server" />
        <input type="hidden" id="hddReuse" runat="server" />
        <input type="hidden" id="hddAddRow" runat="server" value="N" />
    </div>
</asp:Content>

