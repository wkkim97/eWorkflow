<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="BKLSpecialPriceandDiscount.aspx.cs" Inherits="Approval_Document_BKLSpecialPriceandDiscount" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">

        function pageLoad() {
            var addRow = $('#<%= hddAddRow.ClientID %>').val();
            
            //if (addRow == 'Y') {
                var grid = $find('<%=radGrdProduct.ClientID%>');
            
                if (grid.get_masterTableView()) {
                    var dataItems = grid.get_masterTableView().get_dataItems();
                    if (dataItems.length > 0) {                        
                        grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                    }
                }
             //   $('#<%= hddAddRow.ClientID %>').val('Y');
            //}
        }
        function fn_Radio(sender, args) {
            sender.set_autoPostBack(false);

        }

        function fn_UpdateGridDataFromUploader() {
            fn_UpdateGridData(null);
        }
        function fn_DoRequest(sender, arsg) {
            return fn_UpdateGridData();
        }

        function fn_DoSave(sender, args) {
            console.log("3333");
            return fn_UpdateGridData();
        }

        function getTypeValue() {
            var selectedValue;
            if ($find('<%= radBtnBasic.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnBasic.ClientID %>').get_value();
            else if ($find('<%= radBtnWRP.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnWRP.ClientID %>').get_value();
            else if ($find('<%= radBtnWORP.ClientID%>').get_checked())
                selectedValue = $find('<%= radBtnWORP.ClientID %>').get_value();
            else if ($find('<%= radBtnCustomer.ClientID%>').get_checked())
                selectedValue = $find('<%= radBtnCustomer.ClientID %>').get_value();
            
            
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


<%--    function getSelectedProductFamily() {
        var controls = $('#<%= divProductFamily.ClientID %>').children();
        var selectedValue;

        for (var i = 0; i < controls.length; i++) {
            var bu = controls[i];
            if ($find(bu.id).get_checked()) {
                selectedValue = $find(bu.id).get_value();
                break;
            }
        }
        return selectedValue;
    }--%>


        

       // function getTypeValueType() {
       //     var controls = $('#<%= divType.ClientID %>').children();
       //     var selectedValue;
       //
       //     for (var i = 0; i < controls.length; i++) {
       //         var type = controls[i];
       //         if ($find(type.id).get_checked()) {
       //             selectedValue = $find(type.id).get_value();
       //
       //             break;
       //         }
       //     }
       //     return selectedValue;
       // }

        function fn_OnWholeSalerRequesting(sender, args) {
                var selectedType = getTypeValue();
                
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
            //$find('<%= radGrdProduct.ClientID %>').get_batchEditingManager().saveChanges(masterTable);
            var dataItems = masterTable.get_dataItems();
            console.log(dataItems);
            var maxIdx = 0;
           
            for (var i = 0; i < dataItems.length; i++) {
                var idx = dataItems[i].get_cell("IDX").innerText;
                var code = dataItems[i].get_cell("PRODUCT_CODE").innerText;
                var name = dataItems[i].get_cell("PRODUCT_NAME").innerText;
                var listprice = dataItems[i].get_cell("LIST_PRICE").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var basicdiscount = dataItems[i].get_cell("BASIC_DISCOUNT").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var asisdiscount = dataItems[i].get_cell("AS_IS_DISCOUNT").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var whdiscount = dataItems[i].get_cell("W_H_DISCOUNT").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var tobediscount = dataItems[i].get_cell("TO_BE_DISCOUNT").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var volume = dataItems[i].get_cell("VOLUME").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var netamount = dataItems[i].get_cell("NET_AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');
                var netsellingprice = dataItems[i].get_cell("NET_SELLING_PRICE").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var discountrate = dataItems[i].get_cell("DISCOUNT_RATE").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');               
                var remark = dataItems[i].get_cell("REMARKS").children[0].innerText;
                

                 var conObj = {
                    IDX:null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    LIST_PRICE: null,
                    BASIC_DISCOUNT:null,
                    AS_IS_DISCOUNT: null,
                    W_H_DISCOUNT: null,
                    TO_BE_DISCOUNT: null,
                    VOLUME: null,
                    NET_AMOUNT: null,
                    NET_SELLING_PRICE: null,
                    DISCOUNT_RATE: null,                    
                    REMARKS: null
                    
                }
                conObj.IDX = idx;
                conObj.PRODUCT_CODE = code;
                conObj.PRODUCT_NAME = name;
                conObj.LIST_PRICE = listprice;
                conObj.BASIC_DISCOUNT = basicdiscount;
                conObj.AS_IS_DISCOUNT = asisdiscount;
                conObj.W_H_DISCOUNT = whdiscount;
                conObj.TO_BE_DISCOUNT = tobediscount;
                conObj.VOLUME = volume;
                conObj.NET_AMOUNT = netamount;
                conObj.NET_SELLING_PRICE = netsellingprice;
                conObj.DISCOUNT_RATE = discountrate;               
                conObj.REMARKS = remark;

                list.push(conObj);

                maxIdx = idx;
            }
            console.log(list);
            if (product) {
                var conObj = {
                    IDX: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    LIST_PRICE: null,
                    BASIC_DISCOUNT: null,
                    AS_IS_DISCOUNT: null,
                    W_H_DISCOUNT: null,
                    TO_BE_DISCOUNT: null,
                    VOLUME: null,
                    NET_AMOUNT: null,
                    NET_SELLING_PRICE: null,
                    DISCOUNT_RATE: null,
                    REMARKS: null
                }
                conObj.IDX = ++maxIdx;
                conObj.PRODUCT_CODE = product.PRODUCT_CODE.toString();
                conObj.PRODUCT_NAME = product.PRODUCT_NAME.trim() + "(" + product.PRODUCT_CODE + ")";
                conObj.LIST_PRICE = product.BASE_PRICE;
                conObj.BASIC_DISCOUNT = product.MARGIN;
                conObj.AS_IS_DISCOUNT = 0;
                conObj.W_H_DISCOUNT = 0;
                conObj.TO_BE_DISCOUNT = 0;
                conObj.VOLUME = 0;
                conObj.NET_AMOUNT = 0;
                conObj.NET_SELLING_PRICE = 0;
                conObj.DISCOUNT_RATE = 0;                
                conObj.REMARKS = '';
                
                list.push(conObj);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));
            cal_total_amount();
            
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

            //if (args.get_eventArgument() == 'Rebind') {
                var grid = $find('<%=radGrdProduct.ClientID%>');

                if (grid.get_masterTableView()) {
                    var dataItems = grid.get_masterTableView().get_dataItems();
                    if (dataItems.length > 0) {
                        grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                    }
                }
            //}


          
        }

        function fn_RadioVisible(sender, args) {
            var type = sender.get_value();
            $('#<%= radNumTotalAmount.ClientID%>').val(0);
            $('#<%= radNumTotalAmount.ClientID%>').attr("readonly", true);
            if (getTypeValue() != "Special Supply w R.P." && getTypeValue() != "Special Supply w.o. R.P.") 
                $('#<%= radNumTotalAmount.ClientID%>').attr("readonly", false);
            if (getSelectedBU() == "CH") {
                $find("<%= radBtnWRP.ClientID %>").set_visible(false);
                $find("<%= radBtnWORP.ClientID %>").set_visible(false);
                $find("<%= radBtnWRP.ClientID %>").set_checked(false);
                $find("<%= radBtnWORP.ClientID %>").set_checked(false);
            } else {
                $find("<%= radBtnWRP.ClientID %>").set_visible(true);
                $find("<%= radBtnWORP.ClientID %>").set_visible(true);
            }
            

            setVisible(type);
        }
        function setVisible(type) {
            //if (type == 'AH') {
            //    $('#typeAH').show();
            //    $('#typeNonAH').hide();
            //    $('#ProductFamily').hide();
            //}
            
            
            if (type == 'HH') {
                $('#ProductFamily').hide();
                $('#typeNonAH').hide();
            }
            if (type != 'AH') {
                $('#typeNonAH').show();
                $('#typeAH').hide();
                if (type == 'HH') {
                    $('#ProductFamily').hide();
                }
                else {
                    $('#ProductFamily').hide();
                }
                
            }
        }
        function fn_OnGridNumBlur(sender) {
            console.log(sender)
            var strNum = sender.value;
            var number = 0;
            if (!isNaN(parseFloat(strNum)) && isFinite(strNum))
                number = sender.value;
            //if (number > 99) sender.value = 99;
            setNumberFormat(sender);
        }

        function fn_OnGridNumBlur2(sender) {
           
            setNumberFormat(sender);
        }
       

        function fn_OnGridKeyUp(sender) {
            var row = $find('<%= radGrdProduct.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            if (row == null) return;

            var dataItem = $find(row.id);
            
            

            if (dataItem) {
                
                //var listprice = parseFloat(dataItem.get_cell('LIST_PRICE').innerText.replace(/,/gi, ''));
                //if (!listprice) listprice = 0;
                //
                //var tobediscount = parseFloat(dataItem.findElement('TO_BE_DISCOUNT').value.replace(/,/gi, ''));
                //if (!tobediscount) tobediscount = 0;
                //var volume = parseFloat(dataItem.get_cell('VOLUME').innerText.replace(/,/gi, '')); 
                //if (!volume) volume = 0;
                //
                //var w_margin = parseFloat(dataItem.findElement('radNumWHOLESALER_MARGIN').value.replace(/,/gi, ''));
                //if (!w_margin) w_margin = 0;
                //if (orginal_margin < w_margin) {
                //    alert('기본 Margin보다 설정값이 높습니다.'); dataItem.findElement('radNumWHOLESALER_MARGIN').value = orginal_margin.toString();
                //    w_margin = orginal_margin;
                //}
                //
                //dataItem.get_cell('NET_AMOUNT').innerText = (listprice*(1-tobediscount)*volume).toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
               

            }
            
        }

        function fn_OnExtensionAgreementCheckedChanged(sender, args) {
            if (args.get_checked())
                $('#tblCheckBox').show();
            else
                $('#tblCheckBox').hide();
        }

        function setVisibleControl(value) {
            if (value == "Y")
                $('#tblCheckBox').show();
            else
                $('#tblCheckBox').hide();
        }


        // MARGIN_TO_BE 계산       
        // 2019.01.21 이영우
        // MARGIN_TO_BE = NORMAL_DISCOUNT + CONDITIONAL_PRODUCT_DISCOUNT + PARTNER_BASED_DISCOUNT + TRANSACTION_DISCOUNT

        function fn_OnGridKeyUp_netamount(sender) {
            if (getTypeValue() != "Special Supply w R.P." && getTypeValue() != "Special Supply w.o. R.P." ) return;
            var row = $find('<%= radGrdProduct.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            console.log(row.id);
            if (row == null) return;
            var dataItem = $find(row.id);
            
            
            if (dataItem) {
                var listprice = parseFloat(dataItem.findElement('radNumListPrice').value.replace(/,/gi, ''));
                if (!listprice) listprice = 0;

                var tobediscount = parseFloat(dataItem.findElement('radNumToBeDiscount').value.replace(/,/gi, ''));
                if (!tobediscount) tobediscount = 0;
                var volume = parseFloat(dataItem.findElement('radNumVOLUME').value.replace(/,/gi, ''));
                if (!volume) volume = 0;

                
                //var w_margin = parseFloat(dataItem.findElement('radNumWHOLESALER_MARGIN').value.replace(/,/gi, ''));
                //if (!w_margin) w_margin = 0;
                //if (orginal_margin < w_margin) {
                //    alert('기본 Margin보다 설정값이 높습니다.'); dataItem.findElement('radNumWHOLESALER_MARGIN').value = orginal_margin.toString();
                //    w_margin = orginal_margin;
                //}

                dataItem.get_cell('NET_AMOUNT').innerText = (listprice * (1 - (tobediscount/100)) * volume).toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                
            }
            cal_total_amount();
            
            
        }
        function cal_total_amount() {
            var list = [];
            var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            
            var maxIdx = 0;
            var total_amount = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var netamount = dataItems[i].get_cell("NET_AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');
                total_amount += parseFloat(netamount);
            }
            $('#<%= radNumTotalAmount.ClientID%>').val(total_amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        }

        function fn_OnGridKeyPress_wk(sender, evt) {
    
            var charCode = (evt.which) ? evt.which : event.keyCode;
    
            var digit = 0;
            var digits = $(sender).attr('DecimalDigits');
            if (digits) digit = parseInt(digits);
            var allowNegative = $(sender).attr('AllowNegative');
            if (allowNegative == 'true') allowNegative = true;
            else allowNegative = false;
            if (allowNegative) {
                if (charCode != 45) {
                    if (digit == 0) {
                        if ((charCode == 46 || charCode > 31) && (charCode < 48 || charCode > 57))
                            return false;

                    }
                    else {
                        if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                            return false;
                    }
                }
            } else {
                if (digit == 0) {
                    if ((charCode == 46 || charCode > 31) && (charCode < 48 || charCode > 57))
                        return false;
                }
                else {
                    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                        return false;
                }
            }
            // Textbox value       
            var _value = event.srcElement.value;
            // 소수점(.)이 두번 이상 나오지 못하게
            var _pattern0 = /^\d*[.]\d*$/; // 현재 value값에 소수점(.) 이 있으면 . 입력불가
            if (_pattern0.test(_value)) {
                if (charCode == 46) {
                    return false;
                }
            }
            if (allowNegative) {
                if (_value.substr(0, 1) == '-') {
                    if (charCode == 45) return false;
                }
            }
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
                            <telerik:RadButton ID="radBtnCH" runat="server" Text="CH" Value="CH" GroupName="SalesGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnRMD" runat="server" Text="RMD" Value="RMD" GroupName="SalesGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible">
                            </telerik:RadButton>
                            
                        </div>
                    </td>
                </tr>


<%--                <tr id="ProductFamily"  >
                    <th>Product Family <span class="text_red">*</span></th>
                    <td>
                        <div id="divProductFamily" runat="server" style="margin: 0 0 0 0">
                            <telerik:RadButton ID="radBtnXarelto" runat="server" Text="Xarelto" Value="Xarelto" GroupName="HH"
                                ButtonType="ToggleButton" ToggleType="Radio"  OnClientClicked="fn_Radio">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnCVRM" runat="server" Text="CVRM" Value="CVRM" GroupName="HH"
                                ButtonType="ToggleButton" ToggleType="Radio"  OnClientClicked="fn_Radio">
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr> --%>

                
                <tr id="typeNonAH" >
                    <th>Type <span class="text_red">*</span></th>
                    <td>
                        <div id="divType" runat="server" style="margin: 0 0 0 0">
                            <telerik:RadButton ID="radBtnBasic" runat="server" Text="Basic Discount" Value="Basic" GroupName="Type"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible">
                            </telerik:RadButton>                        
                            <telerik:RadButton ID="radBtnWRP" runat="server" Text="Special Supply w R.P." Value="Special Supply w R.P." GroupName="Type"  ToolTip="Special Supply with Reference Price (Tender)"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible">
                            </telerik:RadButton>
                            
                            <telerik:RadButton ID="radBtnWORP" runat="server" Text="Special Supply w.o. R.P." Value="Special Supply w.o. R.P." GroupName="Type" ToolTip="Special Supply without Reference Price (GateKeepr)"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnCustomer" runat="server" Text="Customer Contract Price" Value="Customer Contract Price" GroupName="Type"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible">
                            </telerik:RadButton>
                        </div>

                        
                    </td>
                </tr>
                <tr>
                    <th>Exception</th>
                    <td>
                        <div id="divExceptionGroup" runat="server" >
                            <telerik:RadButton ID="radBtnExceptionYes" runat="server" Text="YES" Value="YES" GroupName="ExceptionGroup"
                                    ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnExceptionGroup_Click" OnClientClicked="">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnExceptionNo" runat="server" Text="NO" Value="NO" GroupName="ExceptionGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnExceptionGroup_Click" OnClientClicked="">
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Same Discount</th>
                    <td>
                        <div id="divSameDiscountGroup" runat="server" >
                            <telerik:RadButton ID="radBtnDiscountYes" runat="server" Text="YES" Value="YES" GroupName="DiscountGroup"
                                    ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSameDiscountGroup_Click" >
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnDiscountNo" runat="server" Text="NO" Value="NO" GroupName="DiscountGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSameDiscountGroup_Click" >
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <h3>Wholesaler and Hospital Information</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 15%;" />
                    <col style="width: 10%;" />
                    <col />
                </colgroup>
                <tr>
                    <th colspan="2">Wholesaler Name
                    </th>
                    <td>
                        <telerik:RadAutoCompleteBox ID="radAcomWholesaler" runat="server" AllowCustomEntry="false" Width="100%" ToolTip="검색하여 입력하시기 바랍니다." DropDownWidth="300px"
                            OnClientRequesting="fn_OnWholeSalerRequesting">
                            <WebServiceSettings Method="SearchCustomer" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                        </telerik:RadAutoCompleteBox>
                        <asp:Label ID="lblNotWholesaler" runat="server" Width="100%" Visible="false">Refer to the wholesaler list attached.</asp:Label>
                    </td>
                </tr>
                <tr>
                    <th colspan="2">Hospital Name</th>
                    <td>
                        <telerik:RadAutoCompleteBox ID="radAcomHospital" runat="server" AllowCustomEntry="false" Width="100%" ToolTip="검색하여 입력하시기 바랍니다." DropDownWidth="300px"
                            OnClientRequesting="fn_OnHospitalRequesting">
                            <WebServiceSettings Method="SearchCustomer" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                        </telerik:RadAutoCompleteBox>
                        <asp:Label ID="lblNotHospital" runat="server" Width="100%" Visible="false">Refer to the hospital list attached.</asp:Label>
                    </td>
                </tr>
                <tr>
                    <th colspan="2">2nd Wholesaler</th>
                    <td>
                        <telerik:RadTextBox ID="radTxtSecondWholesaler" runat="server" TextMode="SingleLine" Width="100%" ToolTip="도도매 거래처 기재 (100자 이내)"></telerik:RadTextBox>                                
                    </td>
                </tr>
                <tr>
                    <th rowspan="2">Contract Period <span class="text_red">*</span></th>
                    <th>H <-> W </th>
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
                <tr>
                    <th>W <-> BKL </th>
                    <td>
                        <telerik:RadDatePicker ID="radDatPeriodFrom_W_BKL" runat="server" Width="100px" Calendar-ShowRowHeaders="false" MaxDate="2050-12-31" MinDate="1900-01-01" Culture="ko-KR">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                </SpecialDays>
                            </Calendar>
                        </telerik:RadDatePicker>
                        ~
                        <telerik:RadDatePicker ID="radDatPeriodTo_W_BKL" runat="server" Width="100px" Calendar-ShowRowHeaders="false" MaxDate="2050-12-31" MinDate="1900-01-01" Culture="ko-KR">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                </SpecialDays>
                            </Calendar>
                        </telerik:RadDatePicker>
                    </td>
                </tr>

                <tr>
                <th colspan="2">Extension Agreement Check</th>
                <td>
                    <div id="divExtensionAgreementCheck" style="margin: 0 0 0 0">
                        <telerik:RadButton ID="radBtnExtension" runat="server" Text="체크하면 연장 계약에 필요한 항목이 활성화 됩니다." Value="Y" GroupName="ExtensionAgreementCheck"
                            AutoPostBack="false" ButtonType="ToggleButton" ToggleType="CheckBox" OnClientCheckedChanged="fn_OnExtensionAgreementCheckedChanged">
                        </telerik:RadButton>

                        <table id="tblCheckBox" style="margin-bottom: 5px; display: none;">
                            <colgroup>
                                <col style="width: 35%" />
                                <col />
                            </colgroup>
                            <tr>
                                <th>Re e-WORKFlow Document No. <span class="text_red">*</span></th>
                                <td>
                                    <telerik:RadTextBox ID="radTxtReDocNo" runat="server" TextMode="SingleLine" Width="100%" ToolTip="기존 승인 문서번호(Document No.)를 기재하세요."></telerik:RadTextBox>                                
                                </td>
                            </tr>
                            <tr>
                                <th>Contract Period (W <-> BKL) <span class="text_red">*</span></th>
                                <td>
                                    <telerik:RadDatePicker ID="radDatPeriodFromExtention" runat="server" Width="100px" Calendar-ShowRowHeaders="false" MaxDate="2050-12-31" MinDate="1900-01-01" Culture="ko-KR">
                                        <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                        <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                            <SpecialDays>
                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                            </SpecialDays>
                                        </Calendar>
                                    </telerik:RadDatePicker>
                                    ~
                                    <telerik:RadDatePicker ID="radDatPeriodToExtention" runat="server" Width="100px" Calendar-ShowRowHeaders="false" MaxDate="2050-12-31" MinDate="1900-01-01" Culture="ko-KR">
                                        <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                        <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                            <SpecialDays>
                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                            </SpecialDays>
                                        </Calendar>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <th>Purpose <span class="text_red">*</span> </th>
                                <td colspan="3">
                                    <telerik:RadTextBox ID="radTxtRemark" runat="server" TextMode="MultiLine" Height="50px" Width="100%" ToolTip="연장 계약이 필요한 이유를 간단하게 작성하시고 필요한 파일은 Attachment에 첨부하시기 바랍니다."></telerik:RadTextBox>
                                </td>
                            </tr>
                            </tr>
                        </table>
                    </div>
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
            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid"
             AllowSorting="false" GridLines="None"  
            OnItemCommand="radGrdProduct_ItemCommand" OnItemDataBound="radGrdProduct_ItemDataBound"           
             >
            <HeaderStyle CssClass="grid_header" HorizontalAlign="Center" />
            

            <MasterTableView EditMode="Batch"  ClientDataKeyNames="IDX" TableLayout="Fixed" DataKeyNames="IDX" >
                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Between Hospital and Wholesaler" Name="Between" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100%" HeaderStyle-Font-Size="10px"></telerik:GridColumnGroup>
                       
                    </ColumnGroups>
                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderText="IDX" HeaderStyle-Width="40px" UniqueName="IDX" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PRODUCT_CODE" HeaderText="COde" HeaderStyle-Width="40px" UniqueName="PRODUCT_CODE" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PRODUCT_NAME"   HeaderStyle-Width="200px" HeaderText="Product Name" 
                        UniqueName="PRODUCT_NAME" ReadOnly="true" HeaderStyle-HorizontalAlign ="Left"  >
                        
                    </telerik:GridBoundColumn>


                    <telerik:GridTemplateColumn DataField="LIST_PRICE" HeaderText="List Price" HeaderStyle-Width="60px" UniqueName="LIST_PRICE" 
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10px">                        
                        <ItemTemplate>
                            <asp:Label ID="Label0" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("LIST_PRICE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumListPrice" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeyup="return fn_OnGridKeyUp_netamount(this)"
                                DecimalDigits="0" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn DataField="BASIC_DISCOUNT" HeaderText="Basic<br/> Discount" HeaderStyle-Width="60px" UniqueName="BASIC_DISCOUNT" 
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10px">                        
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("BASIC_DISCOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumBasicDiscount" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"                                                                                               
                                DecimalDigits="2" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                   
                    <telerik:GridTemplateColumn DataField="AS_IS_DISCOUNT" HeaderText="As-Is<br/>Discount" HeaderStyle-Width="60px" UniqueName="AS_IS_DISCOUNT" 
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10px">                        
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("AS_IS_DISCOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumAsIsDiscount" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"                                                                                               
                                DecimalDigits="2" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn DataField="W_H_DISCOUNT" HeaderText="W-H<br/>Discount" HeaderStyle-Width="60px" UniqueName="W_H_DISCOUNT" 
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10px">                        
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("W_H_DISCOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumWHDiscount" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"                                                                                               
                                DecimalDigits="2" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn DataField="TO_BE_DISCOUNT" HeaderText="To-Be<br/>Discount" HeaderStyle-Width="60px" UniqueName="TO_BE_DISCOUNT" 
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10px">                        
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("TO_BE_DISCOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumToBeDiscount" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"  
                                onkeyup="return fn_OnGridKeyUp_netamount(this)"
                                DecimalDigits="2" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn DataField="VOLUME" HeaderText="Volume" UniqueName="VOLUME" HeaderStyle-Font-Size="10px"
                        HeaderStyle-Width="60px" Display="false">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("VOLUME")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumVOLUME" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur2(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeyup="return fn_OnGridKeyUp_netamount(this)"                            
                                DecimalDigits="0" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                     <telerik:GridTemplateColumn DataField="NET_AMOUNT" HeaderText="Net Amount" UniqueName="NET_AMOUNT" HeaderStyle-Font-Size="10px"
                        HeaderStyle-Width="60px" Display="false">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("NET_AMOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                        
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn DataField="NET_SELLING_PRICE" HeaderText="Net Selling Price" UniqueName="NET_SELLING_PRICE"
                        HeaderStyle-Width="110px" Display="false">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label7" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("NET_SELLING_PRICE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumNetSellingPrice" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur2(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                                            
                                DecimalDigits="0" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn DataField="DISCOUNT_RATE" HeaderText="Dicount<br/>Rate" HeaderStyle-Width="60px" UniqueName="DISCOUNT_RATE" 
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10px">                        
                        <ItemTemplate>
                            <asp:Label ID="Label8" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("DISCOUNT_RATE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumDiscountRate" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"                                                                                               
                                DecimalDigits="2" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn UniqueName="REMARKS" HeaderText="Remarks" HeaderStyle-Width="60px" >
                        <ItemTemplate>                            
                            <%#DataBinder.Eval(Container.DataItem, "REMARKS")%></ItemTemplate>                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRemark" runat="server" Width="100%" CssClass="input" ></asp:TextBox>
                            <%--<telerik:RadTextBox ID="radGrdTxtRemark" runat="server" Width="100%"></telerik:RadTextBox>--%>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="30px">
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
                    <col style="width:322px;" />
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
        <div class="data_type1">
            <h3>Attachment Check</h3>
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Attachment Check </th>
                    <td>
                        <div id="divCheck" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radChkCheck1" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="1. Contract: WS-Hospital" Value="0001">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkCheck2" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="2. PIA" Value="0002">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkCheck3" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="3. Prior Conditions" Value="0003">
                            </telerik:RadButton>
                             <telerik:RadButton ID="radChkCheck4" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="4. Others" Value="0004">
                            </telerik:RadButton>

                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="Dealno" class="align_right" style="display:none" runat="server">
        Deal No.
        <telerik:RadTextBox ID="txtDealno" runat="server" Width="150px" height="25"  ></telerik:RadTextBox> 
        <telerik:RadButton runat="server" ID="radBtnDealNoSave"  ButtonType="ToggleButton" OnClick="radBtnDealNoSave_Click"
                 ForeColor="White" CssClass="btn btn-red btn-size1 bold" Width="50" height="20" Text="Save" AutoPostBack="true"></telerik:RadButton>                    
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
    <input type="text" id="hddGridItems" runat="server" width="800" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="HddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

