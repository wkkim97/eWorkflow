<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="RebatePolicy.aspx.cs" Inherits="Approval_Document_RebatePolicy" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function pageLoad() {
            var addRow = $('#<%= hddAddRow.ClientID %>').val();

            //if (addRow == 'Y') {
            var grid = $find('<%=radGrdProduct_NEW.ClientID%>');

            if (grid.get_masterTableView()) {
                var dataItems = grid.get_masterTableView().get_dataItems();
                if (dataItems.length > 0) {
                    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                }
            }
            //   $('#<%= hddAddRow.ClientID %>').val('Y');
            //}
        }
        function fn_UpdateGridDataFromUploader() {
            var type = getRebateType();
            if (type == "Change" || type == "Create")
                 fn_UpdateGridData(false);
            else
                 fn_UpdateGridData_NEW(false);
        }
        function fn_DoRequest(sender, args) {
            var type = getRebateType();
            if (type == "Change" || type == "Create")
                return fn_UpdateGridData();
            else
                return fn_UpdateGridData_NEW();
        }

        function fn_DoSave(sender, args) {
            var type = getRebateType();
            console.log("DoSave");
            console.log("type");
            if (type == "Change" || type == "Create")
                return fn_UpdateGridData();
            else
                return fn_UpdateGridData_NEW();
        }

        function fn_Radio(sender, args) {
            var value = sender.get_value();
            setVisible(value);
        }
        function getRebateType() {
            var selectedValue;
            if ($find('<%= RadrdoSellingCreate.ClientID %>').get_checked())
                selectedValue = $find('<%= RadrdoSellingCreate.ClientID %>').get_value();
            else if ($find('<%= RadrdoSellingChange.ClientID %>').get_checked())
                selectedValue = $find('<%= RadrdoSellingChange.ClientID %>').get_value();
            else if ($find('<%= RadrdoReturnCreate.ClientID%>').get_checked())
                selectedValue = $find('<%= RadrdoReturnCreate.ClientID %>').get_value();
            else if ($find('<%= RadrdoReturnChange.ClientID%>').get_checked())
                selectedValue = $find('<%= RadrdoReturnChange.ClientID %>').get_value();
            else if ($find('<%= RadrdoCreate.ClientID%>').get_checked())
                selectedValue = $find('<%= RadrdoCreate.ClientID %>').get_value();
            else if ($find('<%= RadrdoChange.ClientID%>').get_checked())
                selectedValue = $find('<%= RadrdoChange.ClientID %>').get_value();
            return selectedValue;
        }


        function setVisible(value, type) {
            if (value == 'CP') {
                $('#Dis').show();
            }
            if (value == 'IS' || value == 'BVS' || value == 'ES')
                $('#Dis').hide();
        }

       
        //--------------------------
        //   Product Popup 창을 띄어준다
        //--------------------------
        var selectedValue;
        var selectedType;
        function fn_PopupProduct(sender, args) {
            var wnd = $find("<%= RadPopupProduct.ClientID %>");
            
            var controlsBg = $('#<%= divBG.ClientID %>').children();
            var controlsType = $('#<%= divType.ClientID %>').children();
            
            for (var i = 0; i < controlsBg.length; i++) {
                var bg = controlsBg[i];                                       //BG
                if ($find(bg.id).get_checked()) {
                    selectedValue = $find(bg.id).get_value();               //선택한 BG의 Value
                    break;
                }
            }
            for (var j = 0; j < controlsType.length; j++) {
                var type = controlsType[j];                                       //Type
                if ($find(type.id).get_checked()) {
                    selectedType = $find(type.id).get_value();               //선택한 Type의 Value
                    break;
                }
            }
           // console.log(selectedValue);
           // console.log(selectedType);
            if (selectedValue && selectedType) {
                wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=" + selectedValue);
                wnd.show();
                sender.set_autoPostBack(false);
            }
            else {
                fn_OpenDocInformation('Please Select a BG, Type');
                sender.set_autoPostBack(false);
            }
        }

        //--------------------------
        //   Product Popup의 ClientClose Event
        //--------------------------
        
        function fn_setProduct(sender, args) {
           // console.log(args);
            var product = args.get_argument();
            if (product != null) {
                var type = getRebateType();
                if (type == "Change" || type == "Create") {
                    fn_UpdateGridData(product);
                } else {
                    fn_UpdateGridData_NEW(product);
                }
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind");
             }
             else
                 return false;
         }
        function fn_UpdateGridData(data) {
            var Gridlist = [];
            var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var type = getRebateType();

            var BGcheck = $find('<%= radRdoCP.ClientID%>').get_checked();
            var NHcheck = $find('<%= radRdoNH.ClientID%>').get_checked();

            var is_val = false;
            for (var i = 0; i < dataItems.length; i++) {
                var as_to = dataItems[i].get_cell("AS_TO").innerText;
                var productcode = dataItems[i].get_cell("PRODUCT_CODE").innerText;
                var productname = dataItems[i].get_cell("PRODUCT_NAME").innerText;
                //var channelcode = dataItems[i].get_cell("CHANNEL_CODE").innerText;
                //var channelname = dataItems[i].get_cell("CHANNEL_NAME").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                //var ctrl = dataItems[i].findControl('radDropDistrubution');
                //var distrubution = null;
                //if (ctrl)
                //    distrubution = ctrl._selectedText;
                //else
                //    distrubution = dataItems[i].get_cell("DISTRIBUTION").children[0].innerText;

                var list = dataItems[i].get_cell("LIST").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                var invoice = dataItems[i].get_cell("INVOICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                var net1 = dataItems[i].get_cell("NET1").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                var net2 = dataItems[i].get_cell("NET2").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;

                var product = {
                    AS_TO: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    //CHANNEL_CODE: null,
                    //CHANNEL_NAME: null,
                    //DISTRIBUTION: null,
                    LIST: null,
                    INVOICE: null,
                    NET1: null,
                    NET2: null
                }
                product.AS_TO = as_to;
                product.PRODUCT_CODE = productcode;
                product.PRODUCT_NAME = productname;
                // product.CHANNEL_CODE = channelcode;
                // product.CHANNEL_NAME = channelname;
                // product.DISTRIBUTION = distrubution;
                product.LIST = list;
                product.INVOICE = invoice;
                product.NET1 = net1;
                product.NET2 = net2;

                Gridlist.push(product);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(Gridlist));

            if (data) {
                for (var i = 0; i < dataItems.length; i++) {
                    var productcode = dataItems[i].get_cell("PRODUCT_CODE").innerText;
                    if (data.PRODUCT_CODE == productcode) {
                        fn_OpenDocInformation('동일한 product 가 존재합니다.');
                        var is_val = true;
                        break;
                    }
                }
                if (is_val == false) {
                    var productAs = {
                        AS_TO: null,
                        PRODUCT_CODE: null,
                        PRODUCT_NAME: null,
                        //    CHANNEL_CODE: null,
                        //    CHANNEL_NAME: null,
                        //    DISTRIBUTION: null,
                        LIST: null,
                        INVOICE: null,
                        NET1: null,
                        NET2: null
                    }
                    productAs.AS_TO = 'AS-IS';
                    productAs.PRODUCT_NAME = data.PRODUCT_NAME + '(' + data.PRODUCT_CODE + ')';
                    productAs.PRODUCT_CODE = data.PRODUCT_CODE;
                    //  productAs.CHANNEL_CODE = '';
                    //  productAs.CHANNEL_NAME = '';
                    //  productAs.DISTRIBUTION = '';
                    productAs.LIST = data.BASE_PRICE;
                    if (BGcheck && NHcheck) {
                        productAs.INVOICE = data.INVOICE_PRICE_NH;
                        productAs.NET1 = data.NET1_PRICE_NH;
                        productAs.NET2 = data.NET2_PRICE_NH;
                    } else {
                        productAs.INVOICE = data.INVOICE_PRICE;
                        productAs.NET1 = data.NET1_PRICE;
                        productAs.NET2 = data.NET2_PRICE;
                    }
                    Gridlist.push(productAs);  // as-is                    
                    if ($find('<%= RadrdoChange.ClientID %>').get_checked()) {
                        var productTo = {
                            AS_TO: null,
                            PRODUCT_CODE: null,
                            PRODUCT_NAME: null,
                            //CHANNEL_CODE: null,
                            //CHANNEL_NAME: null,
                            //DISTRIBUTION: null,
                            LIST: null,
                            INVOICE: null,
                            NET1: null,
                            NET2: null
                        }
                        productTo.AS_TO = 'TO-BE';
                        productTo.PRODUCT_NAME = data.PRODUCT_NAME + '(' + data.PRODUCT_CODE + ')';
                        productTo.PRODUCT_CODE = data.PRODUCT_CODE;
                        //productTo.CHANNEL_CODE = '';
                        //productTo.CHANNEL_NAME = '';
                        //productTo.DISTRIBUTION = '';
                        productTo.LIST = 0;
                        productTo.INVOICE = 0;
                        productTo.NET1 = 0;
                        productTo.NET2 = 0;
                        Gridlist.push(productTo);    //to-be
                    }

                    $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(Gridlist));
                }
                is_val = false;
            }
            return true;
        }
        //-----------------------------------
        //그리드 클라이언트 데이터 업데이트
        //-----------------------------------
        function fn_UpdateGridData_NEW(data) {
            var Gridlist = [];
            var masterTable = $find('<%= radGrdProduct_NEW.ClientID %>').get_masterTableView();
            //$find('<%= radGrdProduct.ClientID %>').get_batchEditingManager().saveChanges(masterTable);
            var dataItems = masterTable.get_dataItems();
            console.log(dataItems);
            var type = getRebateType();

            var BGcheck = $find('<%= radRdoCP.ClientID%>').get_checked();
            var NHcheck = $find('<%= radRdoNH.ClientID%>').get_checked();

            var is_val = false;
            for (var i = 0; i < dataItems.length; i++) {
                //var as_to = dataItems[i].get_cell("AS_TO").innerText;
                var productcode = dataItems[i].get_cell("PRODUCT_CODE").innerText;
                var productname = dataItems[i].get_cell("PRODUCT_NAME").innerText;
                var LIST_PRICE = dataItems[i].get_cell("LIST_PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var LIST_PRICE_NEW = dataItems[i].get_cell("LIST_PRICE_NEW").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var INVOICE_PRICE = dataItems[i].get_cell("INVOICE_PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var INVOICE_PRICE_NEW = dataItems[i].get_cell("INVOICE_PRICE_NEW").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var NET_PRICE_NEW = dataItems[i].get_cell("NET_PRICE_NEW").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var SELLING_DIFF = dataItems[i].get_cell("SELLING_DIFF").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var SELLING_EXPECTED = dataItems[i].get_cell("SELLING_EXPECTED").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var SELLING_TOTAL = dataItems[i].get_cell("SELLING_TOTAL").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var RETURN_PRICE = dataItems[i].get_cell("RETURN_PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var RETURN_PRICE_NEW = dataItems[i].get_cell("RETURN_PRICE_NEW").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var RETURN_DIFF = dataItems[i].get_cell("RETURN_DIFF").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var RETURN_EXPECTED = dataItems[i].get_cell("RETURN_EXPECTED").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var RETURN_TOTAL = dataItems[i].get_cell("RETURN_TOTAL").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');


                var product = {
                    
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,                  
                    LIST_PRICE: null,
                    LIST_PRICE_NEW: null,
                    INVOICE_PRICE: null,
                    INVOICE_PRICE_NEW: null,
                    NET_PRICE_NEW:null,
                    SELLING_DIFF: null,
                    SELLING_EXPECTED: null,
                    SELLING_TOTAL: null,
                    RETURN_PRICE: null,
                    RETURN_PRICE_NEW: null,
                    RETURN_DIFF: null,
                    RETURN_EXPECTED: null,
                    RETURN_TOTAL: null
                }
                
                product.PRODUCT_CODE = productcode;
                product.PRODUCT_NAME = productname;               
                product.LIST_PRICE = LIST_PRICE;
                product.LIST_PRICE_NEW = LIST_PRICE_NEW;
                product.INVOICE_PRICE = INVOICE_PRICE;
                product.INVOICE_PRICE_NEW = INVOICE_PRICE_NEW;
                product.NET_PRICE_NEW = NET_PRICE_NEW;
                product.SELLING_DIFF = SELLING_DIFF;
                product.SELLING_EXPECTED = SELLING_EXPECTED;
                product.SELLING_TOTAL = SELLING_TOTAL;
                product.RETURN_PRICE = RETURN_PRICE;
                product.RETURN_PRICE_NEW = RETURN_PRICE_NEW;
                product.RETURN_DIFF = RETURN_DIFF;
                product.RETURN_EXPECTED = RETURN_EXPECTED;
                product.RETURN_TOTAL = RETURN_TOTAL;

                Gridlist.push(product);
            }
            

            if (data) {
                console.log(data);
                for (var i = 0; i < dataItems.length; i++) {
                    var productcode = dataItems[i].get_cell("PRODUCT_CODE").innerText;
                    if (data.PRODUCT_CODE == productcode) {
                        fn_OpenDocInformation('동일한 product 가 존재합니다.');
                        var is_val = true;
                        break;
                    }
                }
                
                if (is_val == false) {                   
                    var product = {
                        PRODUCT_CODE: null,
                        PRODUCT_NAME: null,
                        LIST_PRICE: null,
                        LIST_PRICE_NEW: null,
                        INVOICE_PRICE: null,
                        INVOICE_PRICE_NEW: null,
                        NET_PRICE_NEW: null,
                        SELLING_DIFF: null,
                        SELLING_EXPECTED: null,
                        SELLING_TOTAL: null,
                        RETURN_PRICE: null,
                        RETURN_PRICE_NEW: null,
                        RETURN_DIFF: null,
                        RETURN_EXPECTED: null,
                        RETURN_TOTAL: null
                    }
                    
                    product.PRODUCT_CODE = data.PRODUCT_CODE;
                    product.PRODUCT_NAME = data.PRODUCT_NAME.trim() + "(" + data.PRODUCT_CODE + ")";
                    product.LIST_PRICE = data.BASE_PRICE;
                    product.LIST_PRICE_NEW = 0;
                    product.INVOICE_PRICE = data.INVOICE_PRICE;
                    if (NHcheck) product.INVOICE_PRICE = data.INVOICE_PRICE_NH;                    
                    product.INVOICE_PRICE_NEW = 0;
                    product.NET_PRICE_NEW = 0;
                    product.SELLING_DIFF = 0;
                    product.SELLING_EXPECTED = 0;
                    product.SELLING_TOTAL = 0;
                    product.RETURN_PRICE = 0;
                    product.RETURN_PRICE_NEW = 0;
                    product.RETURN_DIFF = 0;
                    product.RETURN_EXPECTED = 0;
                    product.RETURN_TOTAL = 0;
                   
                    Gridlist.push(product);
                   
                }
                is_val = false;
            }
            $('#<%= hddGridItems_NEW.ClientID%>').val(JSON.stringify(Gridlist));
            cal_total_amount();
           
            return true;
        }

        //--------------------------
        //   Grid Column delete
        //--------------------------
        var clickedKey = null;
        function openConfirmPopUp(TO_BE_PRODUCT_CODE) {

            clickedKey = TO_BE_PRODUCT_CODE;
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);
            fn_UpdateGridData(null);
            return false;
        }
        function openConfirmPopUp_NEW(TO_BE_PRODUCT_CODE) {
            clickedKey = TO_BE_PRODUCT_CODE;
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);
            fn_UpdateGridData_NEW(null);
            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                var type = getRebateType();
                if (type == "Change" || type == "Create") {
                    var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
                    masterTable.fireCommand("Remove", clickedKey);
                } else {
                    var masterTable = $find('<%= radGrdProduct_NEW.ClientID %>').get_masterTableView();
                    //masterTable.fireCommand("Remove", clickedKey);
                    masterTable.fireCommand("Remove", clickedKey);
                }
            }
        }

        // Column Focus
        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdProduct.ClientID%>');

            var dataItems = grid.get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                if ($find('<%= RadrdoChange.ClientID %>').get_checked()) {
                    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 2].get_element());
                }
                else if ($find('<%= RadrdoCreate.ClientID %>').get_checked()) {
                    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                }
            }
        }
        function openGridRowForEdit_NEW(sender, args) {
            var grid = $find('<%=radGrdProduct_NEW.ClientID%>');
            if (grid.get_masterTableView()) {
                var dataItems = grid.get_masterTableView().get_dataItems();
                if (dataItems.length > 0) {
                    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                }
            }

        }
        function fn_OnGridKeyUp_selling(sender) {
            //if (getTypeValue() != "Special Supply w R.P." && getTypeValue() != "Special Supply w.o. R.P.") return;
            var row = $find('<%= radGrdProduct_NEW.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            //console.log(row.id);
            if (row == null) return;
            var dataItem = $find(row.id);

            console.log(dataItem);
            if (dataItem) {
                var INVOICE_PRICE_NEW = 0;
                var SELLING_EXPECTED = 0;
                var SELLING_TOTAL = 0; 
                var RETURN_PRICE_NEW = 0;
                var RETURN_EXPECTED = 0;
                
                var type = getRebateType();
                if (type == "SellingCreate") {
                    INVOICE_PRICE_NEW = parseFloat(dataItem.findElement('RadtxtINVOICE_PRICE_NEW').value.replace(/,/gi, ''));
                    SELLING_EXPECTED = parseFloat(dataItem.findElement('RadtxtSELLING_EXPECTED').value.replace(/,/gi, ''));
                    dataItem.get_cell('SELLING_TOTAL').innerText = (INVOICE_PRICE_NEW * SELLING_EXPECTED).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                   
                    
                } else if (type == "SellingChange") {
                    
                    INVOICE_PRICE_NEW = parseFloat(dataItem.findElement('RadtxtINVOICE_PRICE_NEW').value.replace(/,/gi, ''));
                    var INVOICE_PRICE = parseFloat(dataItem.get_cell("INVOICE_PRICE").innerText.replace(/,/gi, ''));
                    var SELLING_DIFF = (INVOICE_PRICE_NEW - INVOICE_PRICE) / INVOICE_PRICE_NEW;
                    SELLING_EXPECTED = parseFloat(dataItem.findElement('RadtxtSELLING_EXPECTED').value.replace(/,/gi, ''));
                    dataItem.get_cell('SELLING_DIFF').innerText = SELLING_DIFF.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    dataItem.get_cell('SELLING_TOTAL').innerText = ((INVOICE_PRICE_NEW - INVOICE_PRICE) * SELLING_EXPECTED).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                } else if (type == "ReturnCreate" || type == "ReturnChange" ) {
                    RETURN_PRICE_NEW = parseFloat(dataItem.findElement('RadtxtRETURN_PRICE_NEW').value.replace(/,/gi, ''));
                    var RETURN_PRICE = parseFloat(dataItem.findElement('RadtxtRETURN_PRICE').value.replace(/,/gi, ''));
                    var RETURN_DIFF = (RETURN_PRICE_NEW - RETURN_PRICE) / RETURN_PRICE_NEW;
                    RETURN_EXPECTED = parseFloat(dataItem.findElement('RadtxtRETURN_EXPECTED').value.replace(/,/gi, ''));
                    dataItem.get_cell('RETURN_DIFF').innerText = RETURN_DIFF.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    dataItem.get_cell('RETURN_TOTAL').innerText = (RETURN_PRICE * RETURN_EXPECTED).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                }
                //SELLING_EXPECTED = parseFloat(dataItem.findElement('RadtxtSELLING_EXPECTED').value.replace(/,/gi, ''));
                //if (!listprice) listprice = 0;
                //
                //var tobediscount = parseFloat(dataItem.findElement('radNumToBeDiscount').value.replace(/,/gi, ''));
                //if (!tobediscount) tobediscount = 0;
                //var volume = parseFloat(dataItem.findElement('radNumVOLUME').value.replace(/,/gi, ''));
                //if (!volume) volume = 0;


                //var w_margin = parseFloat(dataItem.findElement('radNumWHOLESALER_MARGIN').value.replace(/,/gi, ''));
                //if (!w_margin) w_margin = 0;
                //if (orginal_margin < w_margin) {
                //    alert('기본 Margin보다 설정값이 높습니다.'); dataItem.findElement('radNumWHOLESALER_MARGIN').value = orginal_margin.toString();
                //    w_margin = orginal_margin;
                //}

                //dataItem.get_cell('NET_AMOUNT').innerText = (listprice * (1 - (tobediscount / 100)) * volume).toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

            }
            cal_total_amount();


        }
        function cal_total_amount() {
            var masterTable = $find('<%= radGrdProduct_NEW.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();

            var maxIdx = 0;
            var total_amount = 0;
            var type = getRebateType();
            for (var i = 0; i < dataItems.length; i++) {
                var netamount = 0;
                console.log(type);
                if (type == "SellingCreate" || type == "SellingChange") {
                    netamount = dataItems[i].get_cell("SELLING_TOTAL").innerText.replace(/,/gi, '').replace(/ /gi, '');
                } else {
                    netamount = dataItems[i].get_cell("RETURN_TOTAL").innerText.replace(/,/gi, '').replace(/ /gi, '');
                }
                
                total_amount += parseFloat(netamount);
            }
            console.log(total_amount);
            $('#<%= radNumTotalAmount.ClientID%>').val(total_amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
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
                    <col />
                    <col style="width: 75%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th>BG <span class="text_red">*</span></th>
                        <td>
                            <div id="divBG" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton ID="radRdoCP" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="CP" Value="CP" AutoPostBack="false" OnClientClicked="fn_Radio"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoIS" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="IDS" Value="IS" AutoPostBack="false" OnClientClicked="fn_Radio"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBVS" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="BVS" Value="BVS" AutoPostBack="false" OnClientClicked="fn_Radio" Visible="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoES" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="ES" Value="ES" AutoPostBack="false" OnClientClicked="fn_Radio"></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr id="Dis" style="display:none">
                        <th>Distribution channel</th>
                        <td>
                            <telerik:RadButton ID="radRdoFM" runat="server" Text="FM" Value="FM" GroupName="Distribution"  ButtonType="ToggleButton" ToggleType="Radio" OnClick="RadRebateType_Click"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoNH" runat="server" Text="NH" Value="NH" GroupName="Distribution"  ButtonType="ToggleButton" ToggleType="Radio" OnClick="RadRebateType_Click"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Type <span class="text_red">*</span></th>
                        <td>
                            <div id="divType" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton runat="server" ID="RadrdoCreate" ButtonType="ToggleButton" ToggleType="Radio" Text="Create" Value="Create" GroupName="Type"  OnClick="RadGrd_Reset" Visible="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="RadrdoChange" ButtonType="ToggleButton" ToggleType="Radio" Text="Change" Value="Change" GroupName="Type"  OnClick="RadGrd_Reset" Visible="false"></telerik:RadButton>
                                <!--eWorkflow Optimization 2020 START-->
                                <telerik:RadButton runat="server" ID="RadrdoSellingCreate" ButtonType="ToggleButton" ToggleType="Radio" Text="SellingCreate" Value="SellingCreate" GroupName="Type"  OnClick="RadRebateType_Click"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="RadrdoSellingChange" ButtonType="ToggleButton" ToggleType="Radio" Text="SellingChange" Value="SellingChange" GroupName="Type"  OnClick="RadRebateType_Click"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="RadrdoReturnCreate" ButtonType="ToggleButton" ToggleType="Radio" Text="ReturnCreate" Value="ReturnCreate" GroupName="Type"  OnClick="RadRebateType_Click"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="RadrdoReturnChange" ButtonType="ToggleButton" ToggleType="Radio" Text="ReturnChange" Value="ReturnChange" GroupName="Type"  OnClick="RadRebateType_Click"></telerik:RadButton>
                                 <!--eWorkflow Optimization 2020 END-->
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th >거래처
                        </th>
                        <td>
                            <telerik:RadTextBox ID="radTxtCustomer" runat="server" Width="100%"></telerik:RadTextBox>                        
                    </td>
                    </tr>
                    <tr>
                        <th>Start Date <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadDatePicker runat="server" ID="RadDateStart" Calendar-ShowRowHeaders="false" Width="120px" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" />
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <th>REMARK</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtRemark" TextMode="MultiLine" Height="80" Width="99%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th> Total 예상 금액적 영향</th>
                        <td>
                            <telerik:RadNumericTextBox ID="radNumTotalAmount" runat="server" NumberFormat-DecimalDigits="0"
                            EnabledStyle-HorizontalAlign="Right" ReadOnly="true">
                             </telerik:RadNumericTextBox>

                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <h3>Product Information
			<div class="title_btn">
                <telerik:RadButton runat="server" ID="RadbtnProduct" CssClass="btn btn-blue btn-size1 bold" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" AutoPostBack="false" Text="Add" OnClientClicked="fn_PopupProduct">
                </telerik:RadButton>
            </div>
        </h3>
        <!--eWorkflow Optimization 2020 START-->
        <telerik:RadGrid runat="server" ID="radGrdProduct_NEW" Skin="EXGrid" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Left"
            AllowAutomaticUpdates="true" ShowFooter="false" Width="100%" 
            OnItemDataBound="radGrdProduct_ItemDataBound_NEW"  OnItemCommand="radGrdProduct_ItemCommand_NEW">
            <MasterTableView EditMode="Batch"  ClientDataKeyNames="PRODUCT_CODE" TableLayout="Fixed" DataKeyNames="PRODUCT_CODE" >
                
                 <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                    <Columns>
                        <telerik:GridBoundColumn Display="false" DataField="PRODUCT_CODE" HeaderText="PRODUCT_CODE" UniqueName="PRODUCT_CODE" HeaderStyle-Width="5%" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PRODUCT_NAME" HeaderText="Product Name" UniqueName="PRODUCT_NAME" HeaderStyle-Width="100px" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right" DataField="LIST_PRICE" UniqueName="LIST_PRICE" HeaderText="표준 <br/>List Price" HeaderStyle-Width="50px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="LblLIST_PRICT" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("LIST_PRICE")) %>'></asp:Label>
                            </ItemTemplate>                        
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right" DataField="LIST_PRICE_NEW" UniqueName="LIST_PRICE_NEW" HeaderText="List Price" HeaderStyle-Width="50px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="LblLIST_PRICE_NEW" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("LIST_PRICE_NEW")) %>'></asp:Label>
                            </ItemTemplate>      
                            <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="RadtxtInvoice" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="99%" EnabledStyle-HorizontalAlign="Right">
                            </telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="RadtxtLIST_PRICE_NEW" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeyup="return fn_OnGridKeyUp_selling(this)"
                                DecimalDigits="0">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right" DataField="INVOICE_PRICE" UniqueName="INVOICE_PRICE" HeaderText="표준 <br/>INVOICE PRICE" HeaderStyle-Width="60px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="LblINVOICE_PRICE" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("INVOICE_PRICE")) %>'></asp:Label>
                            </ItemTemplate>
                            
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right" DataField="INVOICE_PRICE_NEW" UniqueName="INVOICE_PRICE_NEW" HeaderText="INVOICE PRICE" HeaderStyle-Width="60px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="LblINVOICE_PRICE_NEW" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("INVOICE_PRICE_NEW")) %>'></asp:Label>
                            </ItemTemplate>      
                            <EditItemTemplate>                          
                            <asp:TextBox ID="RadtxtINVOICE_PRICE_NEW" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeyup="return fn_OnGridKeyUp_selling(this)"
                                DecimalDigits="0">                                
                            </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right" DataField="NET_PRICE_NEW" UniqueName="NET_PRICE_NEW" HeaderText="NET Price" HeaderStyle-Width="50px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="LblNET_PRICE_NEW" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("NET_PRICE_NEW")) %>'></asp:Label>
                            </ItemTemplate>      
                            <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="RadtxtInvoice" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="99%" EnabledStyle-HorizontalAlign="Right">
                            </telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="RadtxtNET_PRICE_NEW" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="0">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn DataField="SELLING_DIFF" UniqueName="SELLING_DIFF" HeaderText="차이" HeaderStyle-Font-Size="10px" HeaderStyle-Width="60px" Display="false">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="LblSELLING_DIFF" runat="server" Text='<%# String.Format("{0:#,##0.#0}", Eval("SELLING_DIFF")) %>'></asp:Label>
                            </ItemTemplate>                            
                         </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn DataField="SELLING_EXPECTED" UniqueName="SELLING_EXPECTED" HeaderText="예상물량" HeaderStyle-Font-Size="10px" HeaderStyle-Width="60px" Display="false">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="LblSELLING_EXPECTED" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("SELLING_EXPECTED")) %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>                          
                            <asp:TextBox ID="RadtxtSELLING_EXPECTED" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeyup="return fn_OnGridKeyUp_selling(this)"
                                DecimalDigits="0">                                
                            </asp:TextBox>
                            </EditItemTemplate>
                         </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn DataField="SELLING_TOTAL" UniqueName="SELLING_TOTAL" HeaderText="예상 금액적 영향" HeaderStyle-Font-Size="10px" HeaderStyle-Width="60px" Display="false">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="LblSELLING_TOTAL" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("SELLING_TOTAL")) %>'></asp:Label>
                            </ItemTemplate>                            
                         </telerik:GridTemplateColumn>



                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right" DataField="RETURN_PRICE" UniqueName="RETURN_PRICE" HeaderText="표준 <br/>RETURN PRICE" HeaderStyle-Width="50px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="LblRETURN_PRICE" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("RETURN_PRICE")) %>'></asp:Label>
                            </ItemTemplate>      
                            <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="RadtxtInvoice" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="99%" EnabledStyle-HorizontalAlign="Right">
                            </telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="RadtxtRETURN_PRICE" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="0">                                
                            </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right" DataField="RETURN_PRICE_NEW" UniqueName="RETURN_PRICE_NEW" HeaderText="Proposal <br/>RETURN PRICE" HeaderStyle-Width="50px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="LblRETURN_PRICE_NEW" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("RETURN_PRICE_NEW")) %>'></asp:Label>
                            </ItemTemplate>      
                            <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="RadtxtInvoice" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="99%" EnabledStyle-HorizontalAlign="Right">
                            </telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="RadtxtRETURN_PRICE_NEW" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyUp_return(this, event)"
                                DecimalDigits="0">                                
                            </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="RETURN_DIFF" UniqueName="RETURN_DIFF" HeaderText="차이" HeaderStyle-Font-Size="10px" HeaderStyle-Width="60px" Display="false">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="LblRETURN_DIFF" runat="server" Text='<%# String.Format("{0:#,##0.#0}", Eval("RETURN_DIFF")) %>'></asp:Label>
                            </ItemTemplate>                            
                         </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn DataField="RETURN_EXPECTED" UniqueName="RETURN_EXPECTED" HeaderText="예상물량" HeaderStyle-Font-Size="10px" HeaderStyle-Width="60px" Display="false">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="LblRETURN_EXPECTED" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("RETURN_EXPECTED")) %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>                          
                            <asp:TextBox ID="RadtxtRETURN_EXPECTED" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeyup="return fn_OnGridKeyUp_return(this)"
                                DecimalDigits="0">                                
                            </asp:TextBox>
                            </EditItemTemplate>
                         </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn DataField="RETURN_TOTAL" UniqueName="RETURN_TOTAL" HeaderText="예상 금액적 규모" HeaderStyle-Font-Size="10px" HeaderStyle-Width="60px" Display="false">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="LblRETURN_TOTAL" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("RETURN_TOTAL")) %>'></asp:Label>
                            </ItemTemplate>                            
                         </telerik:GridTemplateColumn>




                        <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="20px" HeaderText="" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" ImageUrl="~/Styles/images/ico_del.png"
                                OnClientClick='<%# String.Format("return openConfirmPopUp_NEW(\"{0}\");",Eval("PRODUCT_CODE"))%> ' BorderStyle="None" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    </Columns>

            </MasterTableView>

        </telerik:RadGrid>

   
        <!--eWorkflow Optimization 2020 END-->

        <telerik:RadGrid runat="server" ID="radGrdProduct" Skin="EXGrid" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Left" AllowAutomaticUpdates="true" ShowFooter="false" Width="100%" OnItemDataBound="radGrdProduct_ItemDataBound" OnPreRender="radGrdProduct_PreRender" OnItemCommand="radGrdProduct_ItemCommand" Visible="false">
            <MasterTableView EditMode="Batch">
                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                <Columns>
                    <telerik:GridBoundColumn Display="false" DataField="AS_TO" HeaderText="" UniqueName="AS_TO" HeaderStyle-Width="6%" ReadOnly="true"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn Display="false" DataField="PRODUCT_CODE" HeaderText="PRODUCT_CODE" UniqueName="PRODUCT_CODE" HeaderStyle-Width="15%" ReadOnly="true"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PRODUCT_NAME" HeaderText="Product Name" UniqueName="PRODUCT_NAME" HeaderStyle-Width="51%" ReadOnly="true"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn Display="false" DataField="CHANNEL_CODE" HeaderText="CHANNEL_CODE" UniqueName="CHANNEL_CODE" HeaderStyle-Width="10%" ReadOnly="true"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn Display="false" ItemStyle-HorizontalAlign="Left" DataField="CHANNEL_NAME" UniqueName="CHANNEL_NAME" HeaderText="Channel" HeaderStyle-Width="10%">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "CHANNEL_NAME")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<telerik:RadTextBox runat="server" ID="Radtxtchannel" Width="99%"></telerik:RadTextBox>--%>
                            <asp:TextBox ID="Radtxtchannel" runat="server" CssClass="input" Width="99%"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Display="false" ItemStyle-HorizontalAlign="Left" DataField="DISTRIBUTION" UniqueName="DISTRIBUTION" HeaderText="Distribution" HeaderStyle-Width="10%">
                        <ItemTemplate>
                            <%# Eval("DISTRIBUTION")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDropDownList ID="radDropDistrubution" runat="server" Width="100%">
                                <Items>
                                    <telerik:DropDownListItem Text="YES" Value="Y" Selected="true" />
                                    <telerik:DropDownListItem Text="NO" Value="N" />
                                </Items>
                            </telerik:RadDropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" DataField="LIST" UniqueName="LIST" HeaderText="List"  HeaderStyle-Width="10%">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server"  Text='<%# String.Format("{0:#,##0}", Eval("LIST")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="RadtxtList" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="99%" EnabledStyle-HorizontalAlign="Right">
                            </telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="RadtxtList" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="3">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" DataField="INVOICE" UniqueName="INVOICE" HeaderText="Invoice" HeaderStyle-Width="10%">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("INVOICE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="RadtxtInvoice" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="99%" EnabledStyle-HorizontalAlign="Right">
                            </telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="RadtxtInvoice" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="3">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" DataField="NET1" UniqueName="NET1" HeaderText="NET1" HeaderStyle-Width="10%">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("NET1")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="RadtxtNet1" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="99%" EnabledStyle-HorizontalAlign="Right"></telerik:RadNumericTextBox>--%>
                              <asp:TextBox ID="RadtxtNet1" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="3">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" DataField="NET2" UniqueName="NET2" HeaderText="NET2" HeaderStyle-Width="10%">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("NET2")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="RadtxtNet2" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="99%" EnabledStyle-HorizontalAlign="Right"></telerik:RadNumericTextBox>--%>
                              <asp:TextBox ID="RadtxtNet2" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="3">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="20px" HeaderText="" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" ImageUrl="~/Styles/images/ico_del.png"
                                OnClientClick='<%# String.Format("return openConfirmPopUp(\"{0}\");",Eval("PRODUCT_CODE"))%> ' BorderStyle="None" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>

    </div>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="RadPopupProduct" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product" Width="500px" Height="640px"
                Behaviors="Default" Modal="true" CssClass="windowscroll" OnClientClose="fn_setProduct" NavigateUrl="/eWorks/Common/Popup/ProductList.aspx">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <input type="hidden" id="hddGridItems" runat="server" />
     <input type="hidden" id="hddGridItems_NEW" runat="server" />
    <input type="hidden" id="hddType" runat="server" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
	<input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

