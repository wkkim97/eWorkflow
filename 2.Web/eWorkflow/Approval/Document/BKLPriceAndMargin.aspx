<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="BKLPriceAndMargin.aspx.cs" Inherits="Approval_Document_BKLPriceAndMargin" %>

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
            return fn_UpdateGridData();
        }

        function getTypeValue() {
            var selectedValue;
            if ($find('<%= radBtnTender.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnTender.ClientID %>').get_value();
            else if ($find('<%= radBtnNormal.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnNormal.ClientID %>').get_value();
            else if ($find('<%= radBtnGateKeeper.ClientID%>').get_checked())
                selectedValue = $find('<%= radBtnGateKeeper.ClientID %>').get_value();
            else if ($find('<%= radBtnMax.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnMax.ClientID %>').get_value();
            else if ($find('<%= radBtnVol.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnVol.ClientID %>').get_value();
            else if ($find('<%= radBtnQC.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnQC.ClientID %>').get_value();
            else if ($find('<%= radBtnCustomer.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnCustomer.ClientID %>').get_value();
            else if ($find('<%= radBtnCustomerA.ClientID %>').get_checked())
                selectedValue = $find('<%= radBtnCustomerA.ClientID %>').get_value();
            
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
            //$find('<%= radGrdProduct.ClientID %>').get_batchEditingManager().saveChanges(masterTable);
            var dataItems = masterTable.get_dataItems();
            console.log(dataItems);
            var maxIdx = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var idx = dataItems[i].get_cell("IDX").innerText;
                var code = dataItems[i].get_cell("PRODUCT_CODE").innerText;
                var name = dataItems[i].get_cell("PRODUCT_NAME").innerText;
                var basePrice = dataItems[i].get_cell("BASE_PRICE").innerText.replace(/,/gi, '').replace(/ /gi, '');
                var SupplyPrice = dataItems[i].get_cell("SUPPLY_PRICE").innerText.replace(/,/gi, '').replace(/ /gi, '');
                var marginAsIs = dataItems[i].get_cell("MARGIN_AS_IS").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var marginToBe = dataItems[i].get_cell("MARGIN_TO_BE").innerText.replace(/,/gi, '').replace(/ /gi, '');
                var NormalDiscount = dataItems[i].get_cell("NORMAL_DISCOUNT").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var ConditionalProductDiscount = dataItems[i].get_cell("CONDITIONAL_PRODUCT_DISCOUNT").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var PartnerBasedDiscount = dataItems[i].get_cell("PARTNER_BASED_DISCOUNT").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var TransactionDiscount = dataItems[i].get_cell("TRANSACTION_DISCOUNT").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var discountAmt = dataItems[i].get_cell("DISCOUNT_AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');
                var qty = dataItems[i].get_cell("QTY").innerText.replace(/,/gi, '').replace(/ /gi, '');
                var remark = dataItems[i].get_cell("REMARKS").children[0].innerText;
                var WholesalerMargin = dataItems[i].get_cell("WHOLESALER_MARGIN").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var PassThroughMargin = dataItems[i].get_cell("PASS_THROUGH_MARGIN").innerText.replace(/,/gi, '').replace(/ /gi, '');
                var TotalMargin = dataItems[i].get_cell("TOTAL_MARGIN").innerText.replace(/,/gi, '').replace(/ /gi, '');
                var Volume = dataItems[i].get_cell("VOLUME").innerText.replace(/,/gi, '').replace(/ /gi, '');


                 var conObj = {
                    IDX:null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    BASE_PRICE: null,
                    SUPPLY_PRICE:null,
                    MARGIN_AS_IS: null,
                    MARGIN_TO_BE: null,
                    NORMAL_DISCOUNT: null,
                    CONDITIONAL_PRODUCT_DISCOUNT: null,
                    PARTNER_BASED_DISCOUNT: null,
                    TRANSACTION_DISCOUNT: null,
                    DISCOUNT_AMOUNT: null,
                    QTY: null,
                    REMARKS: null,
                    WHOLESALER_MARGIN: null,
                    PASS_THROUGH_MARGIN: null,
                    TOTAL_MARGIN: null,
                    VOLUME:null,
                }
                conObj.IDX = idx;
                conObj.PRODUCT_CODE = code;
                conObj.PRODUCT_NAME = name;
                conObj.BASE_PRICE = basePrice;
                conObj.SUPPLY_PRICE = SupplyPrice;
                conObj.MARGIN_AS_IS = marginAsIs;
                conObj.MARGIN_TO_BE = marginToBe;
                conObj.NORMAL_DISCOUNT = NormalDiscount;
                conObj.CONDITIONAL_PRODUCT_DISCOUNT = ConditionalProductDiscount;
                conObj.PARTNER_BASED_DISCOUNT = PartnerBasedDiscount;
                conObj.TRANSACTION_DISCOUNT = TransactionDiscount;
                conObj.DISCOUNT_AMOUNT = discountAmt;
                conObj.QTY = qty;
                conObj.REMARKS = remark;
                conObj.WHOLESALER_MARGIN = WholesalerMargin;
                conObj.PASS_THROUGH_MARGIN = PassThroughMargin;
                conObj.TOTAL_MARGIN = TotalMargin;
                conObj.VOLUME = Volume;
                list.push(conObj);

                maxIdx = idx;
            }
            console.log(list);
            if (product) {
                var conObj = {
                    IDX:null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    BASE_PRICE: null,
                    SUPPLY_PRICE: null,
                    MARGIN_AS_IS: null,
                    MARGIN_TO_BE: null,
                    NORMAL_DISCOUNT: null,
                    CONDITIONAL_PRODUCT_DISCOUNT: null,
                    PARTNER_BASED_DISCOUNT: null,
                    TRANSACTION_DISCOUNT: null,
                    DISCOUNT_AMOUNT: null,
                    QTY: null,
                    REMARKS: null,
                    WHOLESALER_MARGIN: null,
                    PASS_THROUGH_MARGIN: null,
                    TOTAL_MARGIN: null,
                    VOLUME: null,
                }
                conObj.IDX = ++maxIdx;
                conObj.PRODUCT_CODE = product.PRODUCT_CODE.toString();
                conObj.PRODUCT_NAME = product.PRODUCT_NAME.trim() + "(" + product.PRODUCT_CODE + ")";
                conObj.BASE_PRICE = product.BASE_PRICE;
                conObj.SUPPLY_PRICE = product.BASE_PRICE;
                conObj.MARGIN_AS_IS = product.MARGIN;
                conObj.MARGIN_TO_BE = 0;
                conObj.NORMAL_DISCOUNT = 0;
                conObj.CONDITIONAL_PRODUCT_DISCOUNT = 0;
                conObj.PARTNER_BASED_DISCOUNT = 0;
                conObj.TRANSACTION_DISCOUNT = 0;
                conObj.DISCOUNT_AMOUNT = 0;
                conObj.QTY = 0;
                conObj.REMARKS = '';
                conObj.WHOLESALER_MARGIN = product.MARGIN;
                conObj.PASS_THROUGH_MARGIN = 0;
                conObj.TOTAL_MARGIN = product.MARGIN ;                
                conObj.VOLUME = 0;
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
            setVisible(type);
        }
        function setVisible(type) {
            if (type == 'AH') {
                $('#typeAH').show();
                $('#typeNonAH').hide();
                $('#ProductFamily').hide();
            }
            else if (type == 'HH') {
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
       

        function fn_OnGridKeyUp(sender) {
            var row = $find('<%= radGrdProduct.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            if (row == null) return;

            var dataItem = $find(row.id);
            
            

            if (dataItem) {
                var base = 0, supply = 0, w_margin=0, h_margin=0 ;
                var orginal_margin = parseFloat(dataItem.get_cell('MARGIN_AS_IS').innerText.replace(/,/gi, ''));
                

                var supply = parseFloat(dataItem.findElement('radNumSUPPLY_PRICE').value.replace(/,/gi, ''));
                if (!supply) supply = 0;
                var base = parseFloat(dataItem.get_cell('BASE_PRICE').innerText.replace(/,/gi, '')); 
                if (!base) base = 0;

                var w_margin = parseFloat(dataItem.findElement('radNumWHOLESALER_MARGIN').value.replace(/,/gi, ''));
                if (!w_margin) w_margin = 0;
                if (orginal_margin < w_margin) {
                    alert('기본 Margin보다 설정값이 높습니다.'); dataItem.findElement('radNumWHOLESALER_MARGIN').value = orginal_margin.toString();
                    w_margin = orginal_margin;
                }
                
                dataItem.get_cell('PASS_THROUGH_MARGIN').innerText = ((base - supply) / (base / 100)).toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                var pass_through_margin = parseFloat(dataItem.get_cell('PASS_THROUGH_MARGIN').innerText.replace(/,/gi, ''));
                dataItem.get_cell('TOTAL_MARGIN').innerText = (w_margin + pass_through_margin).toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

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

        function fn_OnGridKeyUp_MARGIN_TO_BE(sender) {
            
            var row = $find('<%= radGrdProduct.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            console.log(row.id);
            if (row == null) return;
            var dataItem = $find(row.id);
            
            if (dataItem) {
                var NormalDiscount = 0, ConditionalProductDiscount = 0, PartnerBasedDiscount = 0, TransactionDiscount = 0, MarginTobe = 0;

                var NormalDiscount = parseFloat(dataItem.findElement('radNumNormalDiscount').value.replace(/,/gi, ''));
                if (!NormalDiscount) NormalDiscount = 0;
                
                var ConditionalProductDiscount = parseFloat(dataItem.findElement('radNumConditioanlProductDiscount').value.replace(/,/gi, ''));
                if (!ConditionalProductDiscount) ConditionalProductDiscount = 0;

                var PartnerBasedDiscount = parseFloat(dataItem.findElement('radNumPartnerBasedDiscount').value.replace(/,/gi, ''));
                if (!PartnerBasedDiscount) PartnerBasedDiscount = 0;
                
                var TransactionDiscount = parseFloat(dataItem.findElement('radNumTransactionDiscount').value.replace(/,/gi, ''));
                if (!TransactionDiscount) TransactionDiscount = 0;
                
                MarginTobe = NormalDiscount + ConditionalProductDiscount + PartnerBasedDiscount + TransactionDiscount;

                dataItem.get_cell('MARGIN_TO_BE').innerText = MarginTobe.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

            }
            
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
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible" ReadOnly="true">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnHH" runat="server" Text="HH" Value="HH" GroupName="SalesGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible"  ReadOnly="true">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnWH" runat="server" Text="WH" Value="WH" GroupName="SalesGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible"  ReadOnly="true">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnRI" runat="server" Text="R" Value="R" GroupName="SalesGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible"  ReadOnly="true">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnCH" runat="server" Text="CH" Value="CH" GroupName="SalesGroup"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" OnClientClicked="fn_RadioVisible" ReadOnly="true">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnAH" runat="server" Text="AH" Value="AH" GroupName="SalesGroup"
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
                            <telerik:RadButton ID="radBtnQC" runat="server" Text="Campaign" Value="QC" GroupName="AH"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnSpc" runat="server" Text="Special" Value="Special" GroupName="AH"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" Visible="false">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnVirKon" runat="server" Text="Virkon-S" Value="Virkon" GroupName="AH"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" Visible="false">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnCustomerA" runat="server" Text="Customer Specific Price" Value="CustomerA" GroupName="AH"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click"> 
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr>               
                <tr id="typeNonAH" style="display: none">
                    <th>Type <span class="text_red">*</span></th>
                    <td>
                        
                        <telerik:RadButton ID="radBtnNormal" runat="server" Text="Normal" Value="Normal" GroupName="Type"
                            ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click">
                        </telerik:RadButton>                        
                            <telerik:RadButton ID="radBtnTender" runat="server" Text="Special Supply w R.P." Value="Special Supply w R.P." GroupName="Type"  ToolTip="Special Supply with Reference Price (Tender)"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click" >
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnSuEui" runat="server" Text="Su-Eui Contract" Value="SuEui Contract" GroupName="Type" Visible="false"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnGateKeeper" runat="server" Text="Special Supply w.o. R.P." Value="Special Supply w.o. R.P." GroupName="Type" ToolTip="Special Supply without Reference Price (GateKeepr)"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnCustomer" runat="server" Text="Customer Specific Price" Value="Customer Specific Price" GroupName="Type"
                                ButtonType="ToggleButton" ToggleType="Radio" OnClick="radBtnSalesGroup_Click">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnDirect"  runat="server" Text="Direct Contract" Value="Direct Contract" GroupName="Type"
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
                    <telerik:GridBoundColumn DataField="BASE_PRICE" HeaderText="Base Price<br/>(-vat)" HeaderStyle-Width="80px" UniqueName="BASE_PRICE" ReadOnly="true"
                        DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10px">
                    </telerik:GridBoundColumn>
                   
                    <telerik:GridTemplateColumn DataField="MARGIN_AS_IS" HeaderText="Margin%<br/>(As-Is)" HeaderStyle-Width="60px" UniqueName="MARGIN_AS_IS" ReadOnly="true"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10px">                        
                        <ItemTemplate>
                            <asp:Label ID="Label0" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("MARGIN_AS_IS")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumMarginAsIs" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"                                                                                               
                                DecimalDigits="2" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                     <telerik:GridTemplateColumn DataField="SUPPLY_PRICE" HeaderText="SUPPLY<br/>PRICE" UniqueName="SUPPLY_PRICE" HeaderStyle-Font-Size="10px"
                        HeaderStyle-Width="60px" Display="false" HeaderStyle-HorizontalAlign ="Center" ColumnGroupName="Between">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("SUPPLY_PRICE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumSUPPLY_PRICE" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur2(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                
                                onkeyup="return fn_OnGridKeyUp(this)"                                
                                DecimalDigits="0" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn DataField="PASS_THROUGH_MARGIN" HeaderText="Pass Through<br/>Margin" UniqueName="PASS_THROUGH_MARGIN" HeaderStyle-Font-Size="10px"
                        HeaderStyle-Width="60px" Display="false" ColumnGroupName="Between">
                        <ItemStyle HorizontalAlign="Right" Wrap="false" />
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("PASS_THROUGH_MARGIN")) %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn DataField="WHOLESALER_MARGIN" HeaderText="WHOLESALER<br>MARGIN" HeaderStyle-Width="60px" UniqueName="WHOLESALER_MARGIN" ReadOnly="false"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10px"  Display="false">                        
                        <ItemTemplate>
                            <asp:Label ID="Label10" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("WHOLESALER_MARGIN")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumWHOLESALER_MARGIN" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                
                                 onkeyup="return fn_OnGridKeyUp(this)"                                
                                DecimalDigits="2" AllowNegative="true" >                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn DataField="TOTAL_MARGIN" HeaderText="TOTAL<br/>MARGIN" UniqueName="TOTAL_MARGIN" HeaderStyle-Font-Size="10px"
                        HeaderStyle-Width="60px" Display="false">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label7" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("TOTAL_MARGIN")) %>'></asp:Label>
                        </ItemTemplate>
                        
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn DataField="NORMAL_DISCOUNT" HeaderText="Normal<br/>Discount %" UniqueName="NORMAL_DISCOUNT" HeaderStyle-Width="60px" HeaderStyle-Font-Size="10px">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="lblNormalDiscount" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("NORMAL_DISCOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumNormalDiscount" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                
                                onkeyup="return fn_OnGridKeyUp_MARGIN_TO_BE(this)"
                                DecimalDigits="2" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn DataField="CONDITIONAL_PRODUCT_DISCOUNT" HeaderText="Conditional<br/>product discount %" UniqueName="CONDITIONAL_PRODUCT_DISCOUNT" HeaderStyle-Width="60px" HeaderStyle-Font-Size="10px">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="lblConditionalProductDiscount" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("CONDITIONAL_PRODUCT_DISCOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumConditioanlProductDiscount" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                
                                onkeyup="return fn_OnGridKeyUp_MARGIN_TO_BE(this)"
                                DecimalDigits="2" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn DataField="PARTNER_BASED_DISCOUNT" HeaderText="Partner-based discount<br/>(Hospital) %" UniqueName="PARTNER_BASED_DISCOUNT" HeaderStyle-Width="60px" HeaderStyle-Font-Size="10px">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="lblPartnerBasedDiscount" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("PARTNER_BASED_DISCOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumPartnerBasedDiscount" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                
                                onkeyup="return fn_OnGridKeyUp_MARGIN_TO_BE(this)"                               
                                DecimalDigits="2" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn DataField="TRANSACTION_DISCOUNT" HeaderText="Transaction<br/>discount %" UniqueName="TRANSACTION_DISCOUNT" HeaderStyle-Width="60px" HeaderStyle-Font-Size="10px">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="lblTransactionDiscount" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("TRANSACTION_DISCOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumTransactionDiscount" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                
                                onkeyup="return fn_OnGridKeyUp_MARGIN_TO_BE(this)" 
                                DecimalDigits="2" AllowNegative="false">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn DataField="MARGIN_TO_BE" HeaderText="Margin%<br/>(To-Be)" UniqueName="MARGIN_TO_BE" HeaderStyle-Width="60px" HeaderStyle-Font-Size="10px">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="lblMargin_Tobe" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("MARGIN_TO_BE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumMargin" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                              
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
                        FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red" Display="false" ReadOnly="true" HeaderStyle-Font-Size="10px">
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridNumericColumn>


                    <telerik:GridTemplateColumn DataField="VOLUME" HeaderText="VOLUME" UniqueName="VOLUME" HeaderStyle-Font-Size="10px"
                        HeaderStyle-Width="60px" Display="false">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label8" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("VOLUME")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radNumVOLUME" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur2(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                                            
                                DecimalDigits="0" AllowNegative="false">                                
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
    <input type="hidden" id="hddGridItems" runat="server" width="800" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="HddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

