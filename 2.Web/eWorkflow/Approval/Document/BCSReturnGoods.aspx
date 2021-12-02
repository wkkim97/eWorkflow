<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="BCSReturnGoods.aspx.cs" Inherits="Approval_Document_BCSReturnGoods" %>

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

        var currentIdx = 0;
        function fn_PopupCustomer(sender, args) {
            var dataItems = $find('<%= radGrdReturnGoods.ClientID %>').get_masterTableView().get_dataItems();

            //삭제 : &&$find('<%= radRdoType1.ClientID %>').get_checked() == false && $find('<%= radRdoType2.ClientID %>').get_checked() == false && $find('<%= radRdoType3.ClientID%>').get_checked() == false) {
            if ($find('<%= radRdoType4.ClientID %>').get_checked() == false && $find('<%= radRdoType5.ClientID %>').get_checked() == false && $find('<%= radRdoType6.ClientID %>').get_checked() == false && $find('<%= radRdoType7.ClientID %>').get_checked() == false) {
                fn_OpenDocInformation('Please Select Type');
                return false;
            }
            //else if (dataItems.length > 0) {
            //    for (var i = 0; i < dataItems.length; i++) {
            //        var customername = dataItems[i].get_cell("CUSTOMER_NAME").innerText.trim();
            //        var productname = dataItems[i].get_cell("PRODUCT_NAME").children[0].innerText.trim();
            //        var qty = dataItems[i].get_cell("QTY").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
            //        var price = dataItems[i].get_cell("UNIT_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

            //        if (customername == '' || productname == '' || qty == '' || qty == 0 || price == '' || price == 0) {
            //            fn_OpenDocInformation('자료를 입력하시기 바랍니다.');
            //            return false;
            //        }
            //    }
            //}
            var wnd = $find("<%= radWinPopupCustomer.ClientID %>");
            wnd.setUrl("/eWorks/Common/Popup/CustomerListForCS.aspx");
            wnd.show();
        }


        //-----------------------------------
        // 그리드 클라이언트 데이터 업데이트
        //-----------------------------------
        function fn_UpdateGridData(data) {
            var list = [];
            var masterTable = $find('<%= radGrdReturnGoods.ClientID %>').get_masterTableView();
         <%--   if ($find('<%= radGrdReturnGoods.ClientID %>').get_batchEditingManager())
                $find('<%= radGrdReturnGoods.ClientID %>').get_batchEditingManager().saveChanges(masterTable);--%>
            var dataItems = masterTable.get_dataItems();
            var is_val = false;
            var maxIdx = 0;
            var maxGIdx = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var Gindex = dataItems[i].get_cell("GRID_INDEX").innerText.trim();
                var idx = dataItems[i].get_cell("IDX").innerText.trim();
                var cu_re = dataItems[i].get_cell("CU_RE").innerText.trim();
                var customercode = dataItems[i].get_cell("CUSTOMER_CODE").innerText.trim();
                var customername = dataItems[i].get_cell("CUSTOMER_NAME").innerText.trim();
                var customername_new = dataItems[i].get_cell("CUSTOMER_NAME_NEW").innerText.trim();
                var customertype = dataItems[i].get_cell("PARVW").innerText.trim();
                var productcode = dataItems[i].get_cell("PRODUCT_CODE").innerText.trim();
                var productname = dataItems[i].get_cell("PRODUCT_NAME").children[0].innerText.trim();
                var qty = dataItems[i].get_cell("QTY").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var return_price = dataItems[i].get_cell("RETURN_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var return_price_new = dataItems[i].get_cell("RETURN_PRICE_NEW").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var difference = dataItems[i].get_cell("DIFFERENCE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var total_amount = dataItems[i].get_cell("TOTAL_AMOUNT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var reason = dataItems[i].get_cell("REASON").innerText.trim();
                var price = dataItems[i].get_cell("INVOICE_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var amount = dataItems[i].get_cell("AMOUNT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                
                var item = {
                    GRID_INDEX: null,
                    IDX: null,
                    CU_RE: null,
                    CUSTOMER_CODE: null,
                    CUSTOMER_NAME: null,
                    CUSTOMER_NAME_NEW:null,
                    PARVW: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    QTY: null,
                    RETURN_PRICE: null,
                    RETURN_PRICE_NEW: null,
                    DIFFERENCE: null,
                    TOTAL_AMOUNT:null,
                    REASON: null,
                    INVOICE_PRICE: null,
                    AMOUNT: null
                }
                maxGIdx = Gindex;
                maxIdx = idx;
                item.GRID_INDEX = parseInt(Gindex);
                item.IDX = parseInt(idx);
                item.CU_RE = cu_re
                item.CUSTOMER_CODE = customercode;
                item.CUSTOMER_NAME = customername;
                item.CUSTOMER_NAME_NEW = customername_new;
                item.PARVW = customertype;
                item.PRODUCT_CODE = productcode;
                item.PRODUCT_NAME = productname;
                item.QTY = qty;
                item.RETURN_PRICE= return_price,
                item.RETURN_PRICE_NEW= return_price_new,
                item.DIFFERENCE= difference,
                item.TOTAL_AMOUNT= total_amount,
                item.REASON = reason;
                item.INVOICE_PRICE = price;
                item.AMOUNT = amount;

                list.push(item);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

            if (data) {
                var CUitem = {
                    GRID_INDEX: null,
                    IDX: null,
                    CU_RE: null,
                    CUSTOMER_CODE: null,
                    CUSTOMER_NAME: null,
                    CUSTOMER_NAME_NEW:null,
                    PARVW: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    QTY: null,                    
                    REASON: null,
                    INVOICE_PRICE: null,
                    AMOUNT: null
                }
                //if ($find('<%= radRdoType1.ClientID %>').get_checked() == false && $find('<%= radRdoType2.ClientID %>').get_checked() == false && $find('<%= radRdoType3.ClientID%>').get_checked() == false) {
                //
                //    if (is_val == false) {
                //        maxIdx++;
                //        maxGIdx++;
                //
                //        CUitem.GRID_INDEX = parseInt(maxGIdx);
                //        CUitem.IDX = parseInt(maxIdx);
                //        CUitem.CU_RE = 'C';
                //        CUitem.CUSTOMER_CODE = data.CUSTOMER_CODE;
                //        CUitem.CUSTOMER_NAME = data.CUSTOMER_NAME.trim() + '(' + data.CUSTOMER_CODE + ')';
                //        CUitem.PARVW = data.PARVW;
                //        CUitem.QTY = 0;
                //        CUitem.REASON = '';
                //        CUitem.INVOICE_PRICE = 0;
                //        CUitem.AMOUNT = 0;
                //
                //        list.push(CUitem);   // Current 
                //
                //        if ($find('<%= radRdoType2.ClientID %>').get_checked() || $find('<%= radRdoType3.ClientID %>').get_checked()) {
                //            var REitem = {
                //                GRID_INDEX: null,
                //                IDX: null,
                //                CU_RE: null,
                //                CUSTOMER_CODE: null,
                //                CUSTOMER_NAME: null,
                //                CUSTOMER_TYPE: null,
                //                PRODUCT_CODE: null,
                //                PRODUCT_NAME: null,
                //                QTY: null,
                //                REASON: null,
                //                INVOICE_PRICE: null,
                //                AMOUNT: null
                //            }
                //
                //            REitem.GRID_INDEX = parseInt(maxGIdx + 1);
                //            REitem.IDX = parseInt(maxIdx);
                //            REitem.CU_RE = 'R';
                //            REitem.CUSTOMER_CODE = data.CUSTOMER_CODE;
                //            REitem.CUSTOMER_NAME = data.CUSTOMER_NAME + '(' + data.CUSTOMER_CODE + ')';
                //            REitem.PARVW = data.PARVW;
                //            REitem.QTY = 0;
                //            REitem.REASON = '';
                //            REitem.INVOICE_PRICE = 0;
                //            REitem.AMOUNT = 0;
                //
                //            list.push(REitem);   // Replacement
                //        }
                //        $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));
                //    }
                //    is_val = false;
                //} else {
                    var CUitem = {
                        GRID_INDEX: null,
                        IDX: null,
                        CU_RE: null,
                        CUSTOMER_CODE: null,
                        CUSTOMER_NAME: null,
                        CUSTOMER_NAME_NEW:null,
                        PARVW: null,
                        PRODUCT_CODE: null,
                        PRODUCT_NAME: null,
                        QTY: null,
                        RETURN_PRICE: null,
                        RETURN_PRICE_NEW: null,
                        DIFFERENCE: null,
                        TOTAL_AMOUNT: null,
                        REASON: null,
                        INVOICE_PRICE: null,
                        AMOUNT: null
                    }
                    maxIdx++;
                    maxGIdx++;

                    CUitem.GRID_INDEX = parseInt(maxGIdx);
                    CUitem.IDX = parseInt(maxIdx);
                    
                    CUitem.CUSTOMER_CODE = data.CUSTOMER_CODE;
                    CUitem.CUSTOMER_NAME = data.CUSTOMER_NAME.trim() + '(' + data.CUSTOMER_CODE + ')';
                    CUitem.CUSTOMER_NAME_NEW = "";
                    CUitem.PARVW = data.PARVW;
                    CUitem.QTY = 0;
                    CUitem.REASON = '';
                    CUitem.INVOICE_PRICE = 0;
                    CUitem.RETURN_PRICE = 0;
                    CUitem.RETURN_PRICE_NEW = 0;
                    CUitem.DIFFERENCE = 0;
                    CUitem.TOTAL_AMOUNT = 0;
                    CUitem.AMOUNT = 0;
                    list.push(CUitem)
                    $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

               // }
            }
            return true;
        }


        // Cutomer PopUP
        function fn_setCustomer(oWnd, args) {
            var item = args.get_argument();
            if (item != null) {
                var DCcheck = $find('<%= radRdoNH.ClientID%>').get_checked();
                if (DCcheck) {
                    item.PARVW = "NH";
                } else {
                    if (item.PARVW == "IE") {
                        item.PARVW = "Retailer";
                    } else {
                        item.PARVW = "WS";
                    };
                }
                fn_UpdateGridData(item);
                
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("SetCustomer:" + currentIdx + ":" + item.CUSTOMER_CODE + ":" + item.CUSTOMER_NAME + "(" + item.CUSTOMER_CODE + ")" + ":" + item.PARVW);
            }
            else
                return false;
        }

        var selectedValue;
        function fn_OpenProduct(sender, args) {
            var wnd = $find("<%= radWinPopupCurProduct.ClientID %>");
            var controls = $('#<%= divBG.ClientID %>').children();
            for (var i = 0; i < controls.length; i++) {
                var bg = controls[i];                                       //Business Unit
                if ($find(bg.id).get_checked()) {
                    selectedValue = $find(bg.id).get_value();
                    break;
                }
            }
            if (selectedValue) {
                wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=" + selectedValue);
                wnd.show();
                sender.set_autoPostBack(false);
            }
            else {
                fn_OpenDocInformation('Please Select a Business Unit');
                sender.set_autoPostBack(false);
            }
        }

        function fn_setProduct(oWnd, args) {
            var item = args.get_argument();
            var BGcheck = $find('<%= radRdoBG1.ClientID%>').get_checked();
            var DCcheck = $find('<%= radRdoNH.ClientID%>').get_checked();
            fn_UpdateGridData(null);
            if (item != null) {
                var row = $find('<%= radGrdReturnGoods.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();  // 현재 로우 찾기
                var dataItem = $find(row.id);
                var currentIdx = '0';

                if (dataItem) {
                    Gindex = dataItem.get_cell('GRID_INDEX').innerText;
                    currentIdx = dataItem.get_cell('IDX').innerText;

                    if (BGcheck && DCcheck) {
                        $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("SetCurProduct:" + Gindex + ":" + currentIdx + ":" + item.PRODUCT_CODE + ":" + item.PRODUCT_NAME + "(" + item.PRODUCT_CODE + ")" + ":" + item.INVOICE_PRICE_NH);
                    }
                    else {
                       
                        if (dataItem.get_cell('PARVW').innerText == "WS") {
                            $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("SetCurProduct:" + Gindex + ":" + currentIdx + ":" + item.PRODUCT_CODE + ":" + item.PRODUCT_NAME + "(" + item.PRODUCT_CODE + ")" + ":" + item.NET1_PRICE);
                        } else {
                            $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("SetCurProduct:" + Gindex + ":" + currentIdx + ":" + item.PRODUCT_CODE + ":" + item.PRODUCT_NAME + "(" + item.PRODUCT_CODE + ")" + ":" + item.INVOICE_PRICE);
                        }
                    }
                }
            }
            else
                fn_OpenDocInformation('Product 가 선택되지 않았습니다.');
        }




        //-----------------------------
        //  Grid Delete Event
        //-----------------------------
        function openConfirmPopUp(IDX) {
            clickedKey = parseInt(IDX);
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);
            fn_UpdateGridData(null);

            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                var masterTable = $find('<%= radGrdReturnGoods.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        //-----------------------------
        //  Grid Focus
        //-----------------------------
        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdReturnGoods.ClientID%>');

            if (grid.get_masterTableView()) {
                var dataItems = grid.get_masterTableView().get_dataItems();
                if (dataItems.length > 0) {
                    //if ($find('<%= radRdoType1.ClientID%>').get_checked()) {
                    //    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                    //}
                    //else if ($find('<%= radRdoType2.ClientID%>').get_checked() || $find('<%= radRdoType3.ClientID %>').get_checked()) {
                    //    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 2].get_element());
                    //}
                    // eWorkflow Optimization 2020 - C/R 표시 안함
                    //if ($find('<%= radRdoType2.ClientID%>').get_checked() || $find('<%= radRdoType3.ClientID %>').get_checked()) {
                    //    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 2].get_element());
                    //}
                    //else {
                        grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                    //}
                }
            }
        }

        // Visible
        function fn_VisibleDiv(sender, args) {
            var type = sender.get_value();
            setVisible('', type);
        }

        function fn_Radio(sender, args) {
            var value = sender.get_value();
            setVisible(value, '');

        }
        function setVisible(value, type) {
            if (value == 'CP') {
                $('#Dis').show();
            }
            if (value == 'IS' || value == 'BVS' || value == 'ES')
                $('#Dis').hide();

            if (type == 'Unexpired' || type == 'Unexpired_2020') {
                $('#CuRe').show();
                $('#reason').show();
                $('#divPaper').hide();
                $('#divExpired').hide();
                $('#divUnexpired').show();
            }
            if (type == 'Paper' || type == 'Paper_2020' ) {
                $('#CuRe').show();
                $('#reason').show();
                $('#divExpired').hide();
                $('#divUnexpired').hide();
                $('#divPaper').show();
            }
            if (type == 'Expired' || type == 'Expired_2020') {
                $('#CuRe').hide();
                $('#reason').show();
                $('#divUnexpired').hide();
                $('#divPaper').hide();
                $('#divExpired').show();
            }
            if (type == 'Customer_2020') {
                $('#CuRe').hide();
                $('#reason').show();
                $('#divUnexpired').show();
                $('#divPaper').hide();
                $('#divExpired').hide();
            }
            
        }

        function fn_OnGridNumBlur(sender) {
            setNumberFormat(sender);

            //SetTotal();
        }

        // 텍스트 박스 변경
        function SetTotal() {
            var row = $find('<%= radGrdReturnGoods.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            if (row == null) return;

            var dataItem = $find(row.id);
            if (dataItem) {
                console.log(dataItem);
                //var qty = 0, price = 0;
                //var ctlQty = dataItem.findElement('radGrdNumCurQty');
                //if (ctlQty)
                //    //qty = parseInt(ctlQty.value());
                //    qty = ctlQty.value.replace(/,/gi, '').replace(/ /gi, '');
                //var ctlPrice = dataItem.findElement('radGrdNumCurPrice');
                //if (ctlPrice)
                //    price = ctlPrice.value.replace(/,/gi, '').replace(/ /gi, '');
                //
                //dataItem.get_cell('AMOUNT').innerText = (qty * price).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                var RETURN_PRICE = parseFloat(dataItem.findElement('radtxtRETURN_PRICE').value.replace(/,/gi, ''));
                var RETURN_PRICE_NEW = parseFloat(dataItem.findElement('radtxtRETURN_PRICE_NEW').value.replace(/,/gi, ''));
                dataItem.get_cell('DIFFERENCE').innerText = ((RETURN_PRICE_NEW - RETURN_PRICE) / RETURN_PRICE).toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                var QTY = parseFloat(dataItem.findElement('radGrdNumCurQty').value.replace(/,/gi, ''));
                dataItem.get_cell('TOTAL_AMOUNT').innerText = (RETURN_PRICE_NEW * QTY).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                //INVOICE_PRICE_NEW = parseFloat(dataItem.findElement('RadtxtINVOICE_PRICE_NEW').value.replace(/,/gi, ''));
                //SELLING_EXPECTED = parseFloat(dataItem.findElement('RadtxtSELLING_EXPECTED').value.replace(/,/gi, ''));
                //dataItem.get_cell('SELLING_TOTAL').innerText = (INVOICE_PRICE_NEW * SELLING_EXPECTED).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }

        }

        function fn_OnGridKeyUp(sender) {
            SetTotal();
        }

        // aMount 변경
        <%--function fn_OnGridNumBlur(sender, args) {
            var row = $find('<%= radGrdReturnGoods.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            var dataItem = $find(row.id);
            if (dataItem) {
                var qty = 0, price = 0;
                var ctlQty = dataItem.findControl('radGrdNumCurQty');
                if (ctlQty) qty = parseInt(ctlQty.get_value());
                var ctlPrice = dataItem.findControl('radGrdNumCurPrice');
                if (ctlPrice) price = parseInt(ctlPrice.get_value());

                dataItem.get_cell('AMOUNT').innerText = (qty * price).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
        }--%>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <div class="data_type1">
            <table>
                <colgroup>
                    <col />
                    <col style="width: 75%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Business Unit <span class="text_red">*</span></th>
                        <td>
                            <div id="divBG" runat="server">
                                <telerik:RadButton ID="radRdoBG1" runat="server" Text="CP" Value="CP" GroupName="BG" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" OnClientClicked="fn_Radio">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoBG2" runat="server" Text="IDS" Value="IS" GroupName="BG" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" OnClientClicked="fn_Radio">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoBG3" runat="server" Text="BVS" Value="BVS" GroupName="BG" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" Visible="false" OnClientClicked="fn_Radio">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoBG4" runat="server" Text="ES" Value="ES" GroupName="BG" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" OnClientClicked="fn_Radio">
                                </telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr id="Dis" style="display: none">
                        <th>Distribution channel</th>
                        <td>
                            <telerik:RadButton ID="radRdoFM" runat="server" Text="FM" Value="FM" GroupName="Distribution" ButtonType="ToggleButton" ToggleType="Radio" OnClick="radRdoType1_Click"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoNH" runat="server" Text="NH" Value="NH" GroupName="Distribution" ButtonType="ToggleButton" ToggleType="Radio" OnClick="radRdoType1_Click"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Type <span class="text_red">*</span></th>
                        <td>
                            <div id="divType" runat="server">
                                <telerik:RadButton ID="radRdoType1" runat="server" Text="Expired return good price" Value="Expired" GroupName="Type" ButtonType="ToggleButton" ToggleType="Radio" OnClick="radRdoType1_Click" OnClientClicked="fn_VisibleDiv" Visible="false">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoType2" runat="server" Text="Unexpired return good" Value="Unexpired" GroupName="Type" ButtonType="ToggleButton" ToggleType="Radio" OnClick="radRdoType1_Click" OnClientClicked="fn_VisibleDiv" Visible="false">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoType3" runat="server" Text="Paper order" Value="Paper" GroupName="Type" ButtonType="ToggleButton" ToggleType="Radio" OnClick="radRdoType1_Click" OnClientClicked="fn_VisibleDiv" Visible="false">
                                </telerik:RadButton>

                                <!--eWorkflow Optimization 2020 -->
                                 <telerik:RadButton ID="radRdoType4" runat="server" Text="Expired Product (유효기간 경과)" Value="Expired_2020" GroupName="Type" ButtonType="ToggleButton" ToggleType="Radio" OnClick="radRdoType1_Click" OnClientClicked="fn_VisibleDiv">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoType5" runat="server" Text="Unexpired Product (유효기간 이내 정상품)" Value="Unexpired_2020" GroupName="Type" ButtonType="ToggleButton" ToggleType="Radio" OnClick="radRdoType1_Click" OnClientClicked="fn_VisibleDiv">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoType6" runat="server" Text="Paper Order (제품이동 없이 이루어지는 오더)" Value="Paper_2020" GroupName="Type" ButtonType="ToggleButton" ToggleType="Radio" OnClick="radRdoType1_Click" OnClientClicked="fn_VisibleDiv">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoType7" runat="server" Text="전수배" Value="Customer_2020" GroupName="Type" ButtonType="ToggleButton" ToggleType="Radio" OnClick="radRdoType1_Click" OnClientClicked="fn_VisibleDiv">
                                </telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr id="reason" style="display: none">
                        <th>Reason</th>
                        <td>
                            <div id="divExpired" style="display: none">
                                <telerik:RadTextBox ID="RadtextReason" runat="server" Text="K61 - KR expired return" ReadOnly="true"></telerik:RadTextBox>
                            </div>
                            <div id="divPaper" style="display: none">
                                <telerik:RadDropDownList ID="RadDropPaper" runat="server" Width="300" DefaultMessage="--- Select ---" AutoPostBack="false">
                                    <Items>
                                        <telerik:DropDownListItem Text="K67 - KR Paper-price change" Value="K67_NEW_2019" />
                                        <telerik:DropDownListItem Text="K69 - KR by Invoice- Transfer" Value="K69_NEW"/>
                                        <telerik:DropDownListItem Text="K67 - KR System error" Value="K67_NEW" Visible="false"  />
                                        <telerik:DropDownListItem Text="K67 - KR by Invoice- Transfer" Value="K67" Visible="false" />
                                        <telerik:DropDownListItem Text="K69 - KR System error" Value="K69" Visible="false" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </div>
                            <div id="divUnexpired" style="display: none">
                                <telerik:RadDropDownList ID="RadDropUnexpired" runat="server" Width="500" DefaultMessage="--- Select ---" AutoPostBack="false" >
                                    <Items>
                                        <telerik:DropDownListItem Text="K60 - KR Replacement return for normal (교체출고분에 대한 반품)" Value="K60" />
                                        <telerik:DropDownListItem Text="K62 - KR Overstock (정상품 반품-거래처 재고 과다로 인한 정상반품)" Value="K62" />
                                        <telerik:DropDownListItem Text="K63 - KR Reject by Customer (정상품 반품 -거래처 인수거부로 인한 정상반품)" Value="K63" />                                        
                                        <telerik:DropDownListItem Text="K66 - KR Request error by sales (요쳥 오류로 인한 반품 by 영업부)" Value="K66_NEW_2019" />
                                        <telerik:DropDownListItem Text="K68 - KR Policy return (정상품 반품- 회사 또는 국가정책에 기인한 반품)" Value="K68" />
                                        <telerik:DropDownListItem Text="K70 - KR Damage (정상품 반품-박스파손,물리적파손에 의한 반품)" Value="K70" />
                                        <telerik:DropDownListItem Text="K71 - KR Return Government Stockpile Business  (농협 중앙회 비축분 오더에 대한 반품)" Value="K71" />
                                        <telerik:DropDownListItem Text="K72 - KR Delivery Error (정상품 반품-배송팀 오류로 물건이 잘못 출고된 경우의 반품)" Value="K72" />
                                        <telerik:DropDownListItem Text="K73 - KR Quality Problem (정상품 반품-약효함량미달등 화학적문제로 인한 반품)" Value="K73" />
                                        <telerik:DropDownListItem Text="K75 - KR Error by O2C (입력 오류로 인한 반품 by O2C)" Value="K75_NEW_2019" />
                                        <telerik:DropDownListItem Text="K89 - KR Return Off season (농협 비수기오더에 대한 반품)" Value="K89" />
                                        <telerik:DropDownListItem Text="K64 - KR Insolvent (Expect)" Value="K64" Visible="false"/>
                                        <telerik:DropDownListItem Text="K65 - KR Business terminate" Value="K65" Visible="false" />
                                        <telerik:DropDownListItem Text="K66 - KR Input Error (입력오류로 인한 반품)" Value="K66" Visible="false" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <h3>Return Goods
        <div class="title_btn">
            <telerik:RadButton ID="radBtnAdd" runat="server" Text="Add" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" AutoPostBack="false"
                ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" OnClientClicked="fn_PopupCustomer">
            </telerik:RadButton>
        </div>
        </h3>
    </div>
    <telerik:RadGrid ID="radGrdReturnGoods" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" GridLines="None" AllowAutomaticUpdates="true"
        OnPreRender="radGrdReturnGoods_PreRender" OnItemCommand="radGrdReturnGoods_ItemCommand" 
         HeaderStyle-CssClass="grid_header">
        <MasterTableView EnableHeaderContextMenu="true" EditMode="Batch" DataKeyNames="IDX">
            <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
            <Columns>
                <telerik:GridBoundColumn Display="false" DataField="GRID_INDEX" UniqueName="GRID_INDEX" ReadOnly="true"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="IDX" HeaderStyle-Width="5px" UniqueName="IDX" ItemStyle-HorizontalAlign="Center" ReadOnly="true"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn Display="false" DataField="CU_RE" HeaderStyle-Width="5px" ItemStyle-HorizontalAlign="Center" UniqueName="CU_RE" ReadOnly="true"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CUSTOMER_CODE" HeaderText="" UniqueName="CUSTOMER_CODE" Display="false" ReadOnly="true"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CUSTOMER_NAME" HeaderText="Customer Name / Code" UniqueName="CUSTOMER_NAME" HeaderStyle-Width="150px" ReadOnly="true"></telerik:GridBoundColumn> 
                
                <telerik:GridBoundColumn DataField="PARVW" HeaderText="Type" UniqueName="PARVW" HeaderStyle-Width="10%" ReadOnly="true" Display="false"></telerik:GridBoundColumn>  
                                
                <telerik:GridBoundColumn DataField="PRODUCT_CODE" HeaderText="" UniqueName="PRODUCT_CODE" Display="false" ReadOnly="true"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="PRODUCT_NAME" UniqueName="PRODUCT_NAME" HeaderText="Product Name / Code" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# String.Format("{0}", Eval("PRODUCT_NAME")) %>' CssClass="lbl_align_right"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <telerik:RadTextBox ID="radGrdTxtCurProduct" runat="server" ReadOnly="true" AutoPostBack="false" Width="90%">
                        </telerik:RadTextBox>
                        <telerik:RadButton ID="radGrdBtnCurProduct" runat="server" CssClass="btn_grid" AutoPostBack="false" Width="18px" Height="18px" OnClientClicked="fn_OpenProduct">
                            <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                        </telerik:RadButton>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="CUSTOMER_NAME_NEW" HeaderText="수배 Customer" UniqueName="CUSTOMER_NAME_NEW" HeaderStyle-Width="150px" >
                    <ItemStyle HorizontalAlign="left" />
                    <ItemTemplate>
                        <asp:Label ID="labCUSTOMER_NAME_NEW" runat="server" Text='<%# Eval("CUSTOMER_NAME_NEW") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="radtxtCUSTOMER_NAME_NEW" runat="server" Width="100%" CssClass="input">                                
                        </asp:TextBox>
                    </EditItemTemplate>

                </telerik:GridTemplateColumn> 
                <telerik:GridTemplateColumn DataField="RETURN_PRICE" HeaderText="표준<br />Return Price" UniqueName="RETURN_PRICE" HeaderStyle-Width="50px" DataType="System.Decimal">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="labRETRUN_PRICE" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("RETURN_PRICE")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="radtxtRETURN_PRICE" runat="server" Width="100%" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)" 
                            onfocus="return fn_OnGridNumFocus(this)" 
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="fn_OnGridKeyUp(this)"
                            DecimalDigits="0" AllowNegative="false">                                
                        </asp:TextBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                 <telerik:GridTemplateColumn DataField="RETURN_PRICE_NEW" HeaderText="Propoal<br />Return Price" UniqueName="RETURN_PRICE_NEW" HeaderStyle-Width="50px" DataType="System.Decimal">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="labRETURN_PRICE_NEW" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("RETURN_PRICE_NEW")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="radtxtRETURN_PRICE_NEW" runat="server" Width="100%" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)" 
                            onfocus="return fn_OnGridNumFocus(this)" 
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="fn_OnGridKeyUp(this)"
                            DecimalDigits="0" AllowNegative="false">                                
                        </asp:TextBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                
                <telerik:GridTemplateColumn DataField="QTY" HeaderText="Qty" UniqueName="QTY" HeaderStyle-Width="50px" DataType="System.Decimal" Aggregate="Sum">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("QTY")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="radGrdNumCurQty" runat="server" Width="100%" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)"
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="fn_OnGridKeyUp(this)"
                            DecimalDigits="0" AllowNegative="false">                                
                        </asp:TextBox>
                        <%--<telerik:RadNumericTextBox runat="server" ID="radGrdNumCurQty" NumberFormat-DecimalDigits="0"
                            Width="100%" EnabledStyle-HorizontalAlign="Right" ClientEvents-OnBlur="fn_OnGridNumBlur">
                        </telerik:RadNumericTextBox>--%>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="DIFFERENCE" HeaderText="차이" UniqueName="DIFFERENCE" HeaderStyle-Width="30px" DataType="System.Decimal">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="labDIFFERENCE" DecimalDigits="2" runat="server" Text='<%# String.Format("{0:#,##0.#0}", Eval("DIFFERENCE")) %>'></asp:Label>
                    </ItemTemplate>
                   
                   
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn DataField="TOTAL_AMOUNT" HeaderText="예상금액" UniqueName="TOTAL_AMOUNT" HeaderStyle-Width="50px" DataType="System.Decimal">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="labTOTAL_AMOUNT" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("TOTAL_AMOUNT")) %>'></asp:Label>
                    </ItemTemplate>
                   
                </telerik:GridTemplateColumn>
                
                <telerik:GridTemplateColumn DataField="INVOICE_PRICE" HeaderText="Unit<br />Price" UniqueName="INVOICE_PRICE" HeaderStyle-Width="10%" DataType="System.Decimal">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("INVOICE_PRICE")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="radGrdNumCurPrice" runat="server" Width="100%" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)" 
                            onfocus="return fn_OnGridNumFocus(this)" 
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="fn_OnGridKeyUp(this)"
                            DecimalDigits="0" AllowNegative="false">                                
                        </asp:TextBox>
                        <%--<telerik:RadNumericTextBox runat="server" ID="radGrdNumCurPrice" NumberFormat-DecimalDigits="0"
                            Width="100%" EnabledStyle-HorizontalAlign="Right" ClientEvents-OnBlur="fn_OnGridNumBlur">
                        </telerik:RadNumericTextBox>--%>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridCalculatedColumn HeaderText="Amt" UniqueName="AMOUNT" DataType="System.Double" HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Right" DataFields="QTY, INVOICE_PRICE" Expression='{0}*{1}' Aggregate="Sum" DataFormatString="{0:#,##0}">
                </telerik:GridCalculatedColumn>


                <telerik:GridTemplateColumn Display="false" DataField="REASON" UniqueName="REASON" HeaderText="Reason" HeaderStyle-Width="10%">
                    <ItemTemplate><%# Eval("REASON")%></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="radGrdTxtCurReason" runat="server" Width="100%" CssClass="input"></asp:TextBox>
                        <%--<telerik:RadTextBox ID="radGrdTxtCurReason" runat="server" Width="100%"></telerik:RadTextBox>--%>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="10px" HeaderText="" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" ImageUrl="~/Styles/images/ico_del.png"
                            OnClientClick='<%# String.Format("return openConfirmPopUp({0});",Eval("IDX"))%> ' BorderStyle="None" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <div id="CuRe" style="display: none">C :<span class="text_red">Current</span> / R :<span class="text_red">Replacement</span></div>
    <br />
    <div class="data_type1">
        <table>
            <colgroup>
                <col />
                <col style="width: 75%;" />
            </colgroup>
            <tbody>
                <tr>
                    <th>REMARK</th>
                    <td>
                        <telerik:RadTextBox ID="RadtextRemark" runat="server" TextMode="MultiLine" Height="80px" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinPopupCustomer" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer" Width="360px" Height="470px" Behaviors="Default" NavigateUrl="./CustomerList.aspx" OnClientClose="fn_setCustomer" Modal="true"></telerik:RadWindow>
        </Windows>
        <Windows>
            <telerik:RadWindow ID="radWinPopupCurProduct" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product" Width="550px" Height="500px" Behaviors="Default" NavigateUrl="./ProductList.aspx" OnClientClose="fn_setProduct"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div id="hiddenArea" runat="server">
        <input type="hidden" id="hddProcessID" runat="server" />
        <input type="hidden" id="hddProcessStatus" runat="server" />
        <input type="hidden" id="hddDocumentID" runat="server" />
        <input type="hidden" id="hddGridItems" runat="server" />
        <input type="hidden" id="hddCurProductCode" runat="server" />
        <input type="hidden" id="hddRepProductCode" runat="server" />
        <input type="hidden" id="hddReuse" runat="server" />
        <input type="hidden" id="hddAddRow" runat="server" value="N" />
    </div>
</asp:Content>

