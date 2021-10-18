<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="FreeGoods.aspx.cs" Inherits="Approval_Document_FreeGoods" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        var infoConfirm = '';
        function fn_DoRequest(sender, args) {
            if ($('#<%= hddGridSelect.ClientID%>').val() == 'Gridinfo')
                return fn_UpdateGridData(null);
            else
                return fn_UpdateCustomer(false);
        }

        function fn_DoSave(sender, args) {
            if ($('#<%= hddGridSelect.ClientID%>').val() == 'Gridinfo') {
                return fn_UpdateGridData(null);
            }
            else
                return fn_UpdateCustomer(false);
        }

        function fn_UpdateGridDataFromUploader() {
            fn_UpdateGridData(null);
            fn_UpdateCustomer(false);
        }

        //---------------------
        //     Set Visible Event
        //---------------------

        // 그리드 구분자
        var GridCustomer = 'GridCustomer';
        var Gridinfo = 'Gridinfo';

        function fn_ClinetDisplay(sender, args) {
            $('#<%= hddBu.ClientID%>').val(sender.get_value());

            if ($find("<%= radRdoBuAH.ClientID %>").get_checked())
                var location = 'AH';
            else
                var location = 'else'
            SetVisible(location, '', '', '');

        }

        function fn_purpose(sender, args) {
            var sample = sender.get_value();
            SetVisible('', sample, '', '');
        }

        function SetVisible(location, sample, Dropindex, RadioCheck) {
            if (location == 'AH') {
                $('#divlocation').hide();
            }
            if (location == 'else') {
                $('#divlocation').show();
            }
            if (sample == 'Sample') {
                $('#purposeSample').show();
                $('#purposeOther').hide();
                $('#purposeText').hide();
            }
            if (sample == 'Other') {
                $('#purposeSample').hide();
                $('#purposeOther').show();
                $('#purposeText').hide();
                $('#rowDBoxCode').hide();
                $('#rowPO_NO').hide();
            }
            if (RadioCheck == 'customer') {
                $('#info').hide();
                $('#customer').show();
                $('#<%= hddGridSelect.ClientID%>').val(GridCustomer);
            }
            
            if (RadioCheck == 'info' && Dropindex == 2) {
                $('#info').hide();
                $('#customer').show();
                $('#rowDBoxCode').show();
               $('#rowPO_NO').show();
                $('#<%= hddGridSelect.ClientID%>').val(GridCustomer);
            }
            if (RadioCheck == 'info' && Dropindex != 2) {
                $('#info').show();
                $('#customer').hide();
                $('#<%= hddGridSelect.ClientID%>').val(Gridinfo);
            }
        }

        function fn_SetGridVisible(sender, args) {
            $('#<%= hddGridItems.ClientID%>').val("");
            $('#<%= hddGridItemsCustomer.ClientID%>').val("");


            if ($find("<%= radRdoSample.ClientID %>").get_checked())
                var RadioCheck = 'info';
            if ($find("<%= radRdoOther.ClientID %>").get_checked())
                var RadioCheck = 'customer';

            var Dropindex = args.get_index(); // drop index number
            if (RadioCheck == 'info' && Dropindex == 2) {
                $('#rowDBoxCode').show();
                $('#rowPO_NO').show();
            }
            else {
                $('#rowDBoxCode').hide();
                $('#rowPO_NO').hide();
            }
            SetVisible('', '', Dropindex, RadioCheck);
        }

        //---------------------
        //     Poup Event
        //---------------------
        var currentIdx = null;
        var customerIdx = null;

        // Sample 팝업 열기
        function fn_OpenSample(sender, args) {
            // info grid
            var masterTable = $find('<%= RadGrdSampleInfo.ClientID%>').get_masterTableView();
            var rowlength = masterTable.get_dataItems().length;
            // customer grid
            var masterCustomer = $find('<%= RadGrdCustomer.ClientID%>').get_masterTableView();
            var rowlengthCustomer = masterCustomer.get_dataItems().length;

            var selectedBu = $('#<%= hddBu.ClientID%>').val();   ///BU
            var num = masterCustomer.get_dataItems().length;

            if (rowlength > 0) {
                for (var i = 0; i < rowlength; i++) {
                    var type = masterTable.get_dataItems()[i].findControl('RadtxtSample');
                    if (type) {
                        currentIdx = masterTable.get_dataItems()[i].get_cell('IDX').innerText;
                        break;
                    }
                }
            }
            if (rowlengthCustomer > 0) {

                for (var i = 0; i < rowlengthCustomer; i++) {
                    var type = masterCustomer.get_dataItems()[i].findControl('RadtxtCustomer');
                    if (type) {
                        customerIdx = masterCustomer.get_dataItems()[i].get_cell('IDX').innerText;
                        break;
                    }
                }
            }
            if (selectedBu) {                
                var purpose = fn_getPurpose();
                var sampletype = '';
                var existSample = '';
                //alert(purpose);
                if (purpose == 'DOSAGE' || purpose == 'DC_U') {
                    sampletype = 'HCP';
                }
                else {
                    existSample = 'Y'
                }
                    var wnd = $find("<%= radWinPopupSample.ClientID %>");
                wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=" + selectedBu + "&sampletype=" + sampletype + "&existsample=" + existSample);
                    wnd.show();
                    sender.set_autoPostBack(false);                
            }
            else {
                fn_OpenDocInformation('Please Select a BU');
                sender.set_autoPostBack(false);
            }
        }

        //--------------------------------------------------
        // BU 선택값에 따라서 Doctor & Pharmacy Popup을 열어준다 
        // 1. BU - CC   : Doctor&Pharmacy 둘다 선택 가능
        // 2. BU - 그외  : Doctor 만 선택 가능
        //--------------------------------------------------
        function fn_OnAddButtonClicked01(sender, args) {
            var masterTable = $find('<%= RadGrdSampleInfo.ClientID%>').get_masterTableView();
            var num = masterTable.get_dataItems().length;
            var wnd = $find("<%= radWininfo.ClientID %>");
            var selectedBu = $('#<%= hddBu.ClientID%>').val();   // BU
           
            if (selectedBu) {
                wnd.setUrl("/eWorks/Common/Popup/Doctor_PharmacyList.aspx?bu=" + selectedBu);
                wnd.show();
                sender.set_autoPostBack(false);
            }
            else {
                fn_OpenDocInformation('Please Select a BU');
                sender.set_autoPostBack(false);
            }
        }
        // 그리드 clientclose event
        function fn_ClinetCloseinfo(oWnd, args) {
            var infodata = args.get_argument();

            if (infodata != null) {
                if (!isDuplication(infodata.INS_CODE, infodata.DOC_CODE, infodata.SAP_PRODUCT_CODE)) {
                    fn_UpdateGridData(infodata)
                    $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind");
                }
                else {
                    fn_OpenDocInformation('동일한 Code 가 존재합니다.');
                }
            }
            else
                return false;
        }
        // sample clientclose event
        function fn_ClientClose(oWnd, args) {
            var masterTable = $find('<%= RadGrdSampleInfo.ClientID%>').get_masterTableView();
            var masterCustomer = $find('<%= RadGrdCustomer.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var dataItemsCustomer = masterCustomer.get_dataItems();


            var sample = args.get_argument();
            if (sample != null) {
                if (dataItems.length > 0) {
                    fn_UpdateGridData(null);
                    $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("ApplySample:" + currentIdx + ":" + sample.PRODUCT_CODE + ":" + sample.PRODUCT_NAME + "(" + sample.PRODUCT_CODE + ")" + ":" + sample.SAMPLE_CODE);
                }
                if (dataItemsCustomer.length > 0) {
                    fn_UpdateCustomer(false);
                    $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("ApplyCustomer:" + customerIdx + ":" + sample.PRODUCT_CODE + ":" + sample.PRODUCT_NAME + "(" + sample.PRODUCT_CODE + ")" + ":" + sample.SAMPLE_CODE);
                }
                //else
                //    fn_OpenInformation('Sample이 선택되지 않았습니다.');

            oWnd.close();
        }
    }
    //
    function isDuplication(inscode, doccode, sapsamplecode) {
        var masterTable = $find('<%= RadGrdSampleInfo.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var DropSeletedValue = $find('<%= RadDropSample.ClientID%>').get_selectedItem().get_value();  //Sample Drop Value
            var RadioCC = $find('<%= radRdoBuCH.ClientID%>').get_checked();

            //DC Use  - institue 중복체크
            if (DropSeletedValue == 'DC_U') {
                for (var i = 0; i < dataItems.length; i++) {
                    var InstitueCode = dataItems[i].get_cell('INSTITUE_CODE').innerText.trim();
                    var SapSampleCode = dataItems[i].get_cell('SAP_PRODUCT_CODE').innerText.trim();

                    if (InstitueCode == inscode && SapSampleCode == sapsamplecode) return true;
                }
            }
            //dosage - hcp code 중복체크
            if (DropSeletedValue == 'DOSAGE' && RadioCC != true) {
                for (var i = 0; i < dataItems.length; i++) {
                    var DocterCode = dataItems[i].get_cell('HCP_CODE').innerText.trim();
                    var SapSampleCode = dataItems[i].get_cell('SAP_PRODUCT_CODE').innerText.trim();

                    if (DocterCode == doccode && SapSampleCode == sapsamplecode) return true;
                }
            }

            return false;
        }
        // dasage & CC 에 대한 병원코드(Institue & HCPNAME) 에 대한 중복체크
        function isDuplicationCC(sender, args) {

            var masterTable = $find('<%= RadGrdSampleInfo.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var DropSeletedValue = $find('<%= RadDropSample.ClientID%>').get_selectedItem().get_value();  //Sample Drop Value
            var RadioCC = $find('<%= radRdoBuCH.ClientID%>').get_checked();
            var currentInstitue = '';
            var oldHcpName = '';
            
            if (dataItems.length > 1) {
                for (var i = 0; i < dataItems.length; i++) {
                    var type = masterTable.get_dataItems()[i].findControl('RadtxtHcpname');
                    var hcpName = dataItems[i].get_cell('HCP_NAME').innerText.trim();
                    if (type) {
                        currentInstitue = masterTable.get_dataItems()[i].get_cell('INSTITUE_CODE').innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                        oldHcpName = dataItems[i].findControl('RadtxtHcpname').get_element().value;
                    }
                }
                return go_validate(oldHcpName, currentInstitue, DropSeletedValue, RadioCC);
            }
            return true;
        }

        function go_validate(oldHcpName, currentInstitue, DropSeletedValue, RadioCC) {
            var masterTable = $find('<%= RadGrdSampleInfo.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            if (DropSeletedValue == 'Sample01' && RadioCC == true) {
                for (var i = 0; i < dataItems.length; i++) {
                    var hcpName = dataItems[i].get_cell('HCP_NAME').innerText.trim();
                    var institueP = dataItems[i].get_cell('INSTITUE_CODE').innerText.trim();

                    if (oldHcpName == hcpName && currentInstitue == institueP) {
                        return false;
                    }
                }
            }
            return true;
        }

        //--------------------------
        //   Grid Column delete
        //--------------------------

        //info grid
        function openConfirmPopUp(index) {
            clickedKey = parseInt(index);
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);
            fn_UpdateGridData(null);
            return false;
        }
        //customer grid
        function openConfirmPopUp2(index) {
            clickedKey = parseInt(index);
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn2);
            fn_UpdateCustomer(false);
            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                var masterTable = $find('<%= RadGrdSampleInfo.ClientID %>').get_masterTableView();
            masterTable.fireCommand("Remove", clickedKey);
        }
    }

    function confirmCallBackFn2(arg) {
        if (arg) {
            var masterTable = $find('<%= RadGrdCustomer.ClientID %>').get_masterTableView();
            masterTable.fireCommand("Remove", clickedKey);
        }
    }

    //--------------------------------
    //      Grid Event ROW ADD
    //--------------------------------


    function fn_UpdateGridData(infodata) {
        var list = [];
        var masterTable = $find('<%= RadGrdSampleInfo.ClientID%>').get_masterTableView();
        var dataItems = masterTable.get_dataItems();
        var maxIdx = 0;

        //RadtxtHcpname
        for (var i = 0; i < dataItems.length; i++) {
            var idx = dataItems[i].get_cell("IDX").innerText;
            var doctor_pharmacy = dataItems[i].get_cell("IS_DOCTOR_PHARMACY").innerText;
            var purpose = fn_getPurpose();                                                          // PURPOSE
            var institueName = dataItems[i].get_cell("INSTITUE_NAME").innerText.trim();
            var institueCode = dataItems[i].get_cell("INSTITUE_CODE").innerText.trim();
            var type = masterTable.get_dataItems()[i].findControl('RadtxtHcpname');
            if (type) {
                var hcpName = dataItems[i].findControl('RadtxtHcpname').get_element().value;
            }
            else                
                var hcpName = dataItems[i].get_cell("HCP_NAME").innerText.trim();
            var hcpCode = dataItems[i].get_cell("HCP_CODE").innerText.trim();
            var sampleName = dataItems[i].get_cell("SAMPLE_NAME").children[0].innerText.trim();
            var sampleCode = dataItems[i].get_cell("SAMPLE_CODE").innerText.trim();
            var sapCode = dataItems[i].get_cell("SAP_PRODUCT_CODE").innerText.trim();
            var specialty = dataItems[i].get_cell("SPECIALTY_NAME").innerText;
            var state = dataItems[i].get_cell("STATES").innerText.trim();
            var qty = dataItems[i].get_cell("QTY").innerText;

            var info = {
                IDX: null,
                IS_DOCTOR_PHARMACY: null,
                INSTITUE_NAME: null,
                INSTITUE_CODE: null,
                PURPOSE: null,
                SAP_PRODUCT_CODE: null,
                STATES: null,
                OLD_DOC_NUMBER: null,
                HCP_NAME: null,
                HCP_CODE: null,
                SAMPLE_NAME: null,
                SAMPLE_CODE: null,
                SPECIALTY_NAME: null,
                QTY: 1
            }
            info.IDX = idx;
            info.IS_DOCTOR_PHARMACY = doctor_pharmacy;
            info.INSTITUE_CODE = institueCode;
            info.PURPOSE = purpose;
            info.SAP_PRODUCT_CODE = sapCode;  //
            info.STATES = "";
            info.OLD_DOC_NUMBER = "";
            info.INSTITUE_NAME = institueName;
            info.HCP_NAME = hcpName;
            info.HCP_CODE = hcpCode;
            info.SAMPLE_NAME = sampleName;
            info.SAMPLE_CODE = sampleCode;
            info.SPECIALTY_NAME = specialty;
            info.QTY = qty;

            list.push(info);
            maxIdx = parseInt(idx);
        }
        if (infodata) {
            var info = {
                IDX: null,
                IS_DOCTOR_PHARMACY: null,
                INSTITUE_NAME: null,
                INSTITUE_CODE: null,
                PURPOSE: null,
                SAP_PRODUCT_CODE: null,
                STATES: null,
                OLD_DOC_NUMBER: null,
                HCP_NAME: null,
                HCP_CODE: null,
                SAMPLE_NAME: null,
                SAMPLE_CODE: null,
                SPECIALTY_NAME: null,
                QTY: 1
            }
            info.IDX = maxIdx + 1;
            if (infodata.radioValue == 'Doctor') {
                var doctor_pharmacy = 'D';
                info.IS_DOCTOR_PHARMACY = doctor_pharmacy;    // Docter:D , Pharmacy: P  (구분자)
                info.PURPOSE = fn_getPurpose();               // PURPOSE                
                info.STATES = "";
                info.OLD_DOC_NUMBER = "";
                info.INSTITUE_CODE = infodata.INS_CODE;
                info.INSTITUE_NAME = infodata.INS_NAME + "(" + infodata.INS_CODE + ")";
                info.HCP_CODE = infodata.DOC_CODE;
                info.HCP_NAME = infodata.DOC_NAME + "(" + infodata.DOC_CODE + ")";
                info.SPECIALTY_NAME = infodata.SPECIALITY;
            }
            if (infodata.radioValue == 'Pharmacy') {
                var doctor_pharmacy = 'P';
                info.IS_DOCTOR_PHARMACY = doctor_pharmacy;    // Docter:D , Pharmacy: P  (구분자)
                info.PURPOSE = fn_getPurpose();               // PURPOSE                
                info.STATES = "";
                info.OLD_DOC_NUMBER = "";
                info.INSTITUE_CODE = infodata.PHAR_CODE;
                info.INSTITUE_NAME = infodata.PHAR_NAME + "(" + infodata.PHAR_CODE + ")";
                info.HCP_CODE = '';
                info.HCP_NAME = '';
                info.SPECIALTY_NAME = '약사';
            }
            info.QTY = 1;

            list.push(info);
        }
        $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));
        return true;
    }

    //purpose get Value 
    function fn_getPurpose() {
        if ($find("<%= radRdoSample.ClientID %>").get_checked()) {
            var Getpurpose = $find('<%= RadDropSample.ClientID%>').get_selectedItem().get_value();
            
                return Getpurpose;
            }
            else if ($find("<%= radRdoOther.ClientID %>").get_checked()) {
                var Getpurpose = $find('<%= RadDropOther.ClientID%>').get_selectedItem().get_value();
                return Getpurpose;
            }
    }

    // Customer GRID
    function fn_UpdateCustomer(checkValidate) {
        var list = [];
        var masterTable = $find('<%= RadGrdCustomer.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var maxIdx = 0;
            //if (checkValidate && dataItems.length > 0) {
            //    for (var i = 0; i < dataItems.length; i++) {
            //        var type = masterTable.get_dataItems()[i].findControl('RadtxtCustomer');
            //        if (type) {
            //            var customeritem = dataItems[i].findControl('RadtxtCustomer').get_element().value;
            //            var receiper = dataItems[i].findControl('RadtxtReceipter').get_element().value;
            //            var qty = dataItems[i].findControl('RadtxtQty').get_element().value;
            //        }
            //        else {
            //            var customeritem = dataItems[i].get_cell("CUSTOMER").children[0].innerText;
            //            var receiper = dataItems[i].get_cell("RECEIPTER").children[0].innerText;
            //            var qty = dataItems[i].get_cell("QTY").children[0].innerText;
            //        }
            //        if (customeritem.length < 1 || receiper.length < 1 || qty.length < 1) {
            //            fn_OpenInformation('자료를 입력바랍니다.');
            //            return false;
            //        }
            //    }
            //}

            for (var i = 0; i < dataItems.length; i++) {
                var idx = dataItems[i].get_cell("IDX").innerText;

                var type = masterTable.get_dataItems()[i].findControl('RadtxtCustomer');
                if (type) {
                    var customeritem = dataItems[i].findControl('RadtxtCustomer').get_element().value;
                    var receiper = dataItems[i].findControl('RadtxtReceipter').get_element().value;
                    var qty = dataItems[i].get_cell("QTY").innerText.replace(/,/gi, '').replace(/ /gi, '');                    
                }
                else {
                    var customeritem = dataItems[i].get_cell("CUSTOMER").children[0].innerText;
                    var receiper = dataItems[i].get_cell("RECEIPTER").children[0].innerText;
                    var qty = dataItems[i].get_cell("QTY").innerText.replace(/,/gi, '').replace(/ /gi, '');
                }

                var sampleName = dataItems[i].get_cell("SAMPLE_NAME").children[0].innerText;
                var sampleCode = dataItems[i].get_cell("SAMPLE_CODE").innerText;
                var sapCode = dataItems[i].get_cell("SAP_PRODUCT_CODE").innerText;
                var state = dataItems[i].get_cell("STATES").innerText.trim();

                var customer = {
                    IDX: null,
                    PURPOSE: null,
                    SAP_PRODUCT_CODE: null,
                    STATES: null,
                    OLD_DOC_NUMBER: null,
                    CUSTOMER: null,
                    RECEIPTER: null,
                    SAMPLE_NAME: null,
                    SAMPLE_CODE: null,
                    QTY: 0
                }
                customer.IDX = idx;
                customer.CUSTOMER = customeritem;
                customer.PURPOSE = fn_getPurpose();
                customer.SAP_PRODUCT_CODE = sapCode;
                customer.STATES = "";
                customer.OLD_DOC_NUMBER = "";
                customer.RECEIPTER = receiper;
                customer.SAMPLE_NAME = sampleName;
                customer.SAMPLE_CODE = sampleCode;
                customer.QTY = parseInt(qty.replace(/,/gi, '').replace(/ /gi, ''));

                list.push(customer);
            }
            $('#<%= hddGridItemsCustomer.ClientID%>').val(JSON.stringify(list));
            return true;
        }

        //--------------------------------------------------------------------
        //      GRID Empty Check : 그리드의 빈값을 체크 true:false 를 반환해준다.
        //--------------------------------------------------------------------
        function isEmptyCheck(checkEmpty) {
            //info
            var masterTable = $find('<%= RadGrdSampleInfo.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();

            if (dataItems.length > 0) {
                for (var i = 0; i < dataItems.length; i++) {
                    var sampleItem = dataItems[i].get_cell("SAMPLE_NAME").children[0].innerText;

                    if (sampleItem.length < 1) {
                        fn_OpenDocInformation('Please Sample Select');
                        return false;
                    }
                }
            }
            return true;
        }
        function isEmptyCheckCustomer(checkEmpty) {
            //customer
            var masterCustomer = $find('<%= RadGrdCustomer.ClientID%>').get_masterTableView();
            var dataItemsCustomer = masterCustomer.get_dataItems();

            if (dataItemsCustomer.length > 0) {
                for (var i = 0; i < dataItemsCustomer.length; i++) {
                    var sampleItem = dataItemsCustomer[i].get_cell("SAMPLE_NAME").children[0].innerText;

                    if (sampleItem.length < 1) {
                        fn_OpenDocInformation('Please Sample Select');
                        return false;
                    }
                }
            }
            return true;
        }
        // customer grid add
        function fn_OnAddButtonClicked02(sender, args) {
            var masterTable = $find('<%= RadGrdCustomer.ClientID%>').get_masterTableView();
            var num = masterTable.get_dataItems().length;

            if (fn_UpdateCustomer(true)) {               
                sender.set_autoPostBack(true);
            }
            else
                sender.set_autoPostBack(false);
        }


        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=RadGrdSampleInfo.ClientID%>');
            var dataItems = grid.get_masterTableView().get_dataItems();

            var customer = $find('<%=RadGrdCustomer.ClientID%>');
            var dataItemscus = customer.get_masterTableView().get_dataItems();

            if (dataItems.length > 0) {
                grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
            }
            else if (dataItemscus.length > 0) {
                customer.get_batchEditingManager().openRowForEdit(dataItemscus[dataItemscus.length - 1].get_element());
            }
        }

        //fn_OpenReceipt // 후처리 준비중 
        function fn_OpenReceipt(button, args) {
            var nWidth = 924;
            var nHeight = 680;
            var left = (screen.width / 2) - (nWidth / 2);
            var top = (screen.height / 2) - (nHeight / 2) - 10;

            var SchemeProcessId = $('#<%= hddProcessID.ClientID %>').val();

            //var formName = '파일명 쓰세요';
            //var url = fn_GetWebRoot() + "Approval/Document/" + formName + "?SchemeProcessId=" + SchemeProcessId;
            //window.close();
            //return window.open(url, "", "width=" + nWidth + "px, height=" + nHeight + "px, top=" + top + "px, left=" + left + "px, toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes");
        }

        function fn_OnGridNumBlur(sender) {
            setNumberFormat(sender);

            //SetTotal();
        }
        function fn_ConfrimFreeGood(sender, args) {
            var gridInfo = $find('<%= RadGrdSampleInfo.ClientID %>');            
            var gridCustomer = $find('<%= RadGrdCustomer.ClientID %>');
            var dataItemsInfo = gridInfo.get_masterTableView().get_dataItems();
            var dataItemsCustomer = gridCustomer.get_masterTableView().get_dataItems();
            var rowindex = sender.get_element().parentNode.parentNode.rowIndex;

            if (dataItemsInfo.length > 0) {                
                var state = dataItemsInfo[rowindex - 1].get_cell("STATES").innerText.trim();  //현재 버튼의 상태값  DB:STATE 값 
                if (state == '') {
                    var callBackFunction = Function.createDelegate(sender, function (shouldSubmit) {
                        if (shouldSubmit) {
                            fn_CallCancel(rowindex);
                        }
                    });
                    fn_OpenConfirm("Free Goods 은 취소됩니다.<br /> 정말로 취소하시겠습니까?<br />취소를 원하시면,<br/><font style='color:red'>Freee Good과<br/>Cancel Request(Link) Form을 작성하여</font><br/>창고(PH,CC,DC,R), 혹은 공장(AH)으로 보내셔야합니다.<br/><a href='http://sp-coll-bbs.ap.bayer.cnb/sites/000141/Teamsites/NEW%20e-workflow/LINKED_DATA/Free%20Goods%20Cancel%20Request.xlsx' target='_blank' >Link주소</a>", callBackFunction);
                    return false;
                }
                else 
                    fn_CallCancel(rowindex);
            }
            else if (dataItemsCustomer.length > 0) {
                var state = dataItemsCustomer[rowindex - 1].get_cell("STATES").innerText.trim();  //현재 버튼의 상태값  DB:STATE 값 
                if (state == '') {
                    var callBackFunction = Function.createDelegate(sender, function (shouldSubmit) {
                        if (shouldSubmit) {
                            fn_CallCancel(rowindex);
                        }
                    });
                    fn_OpenConfirm("Free Goods 은 취소됩니다.<br /> 정말로 취소하시겠습니까?<br />취소를 원하시면,<br/><font style='color:red'>Freee Good과<br/>Cancel Request(Link) Form을 작성하여</font><br/>창고(PH,CC,DC,R), 혹은 공장(AH)으로 보내셔야합니다.<br/><a href='http://sp-coll-bbs.ap.bayer.cnb/sites/000141/Teamsites/NEW%20e-workflow/LINKED_DATA/Free%20Goods%20Cancel%20Request.xlsx' target='_blank' >Link주소</a>", callBackFunction);
                    return false;
                }
                else
                    fn_CallCancel(rowindex);
            }
        }

        // 후처리 Process
        function fn_CallCancel(rowindex) {
            try {
                var gridInfo = $find('<%= RadGrdSampleInfo.ClientID %>');
                var Idx = null;
                var gridCustomer = $find('<%= RadGrdCustomer.ClientID %>');
                var HowGrid = "";                                                            // 2개 그리드에 대한 구분값

                var dataItemsInfo = gridInfo.get_masterTableView().get_dataItems();
                var dataItemsCustomer = gridCustomer.get_masterTableView().get_dataItems();

                //var rowindex = sender.get_element().parentNode.parentNode.rowIndex;          // 현재 그리드 로우의 index
                var rowindex = rowindex;

                if (dataItemsInfo.length > 0) {
                    HowGrid = 'info';
                    var state = dataItemsInfo[rowindex - 1].get_cell("STATES").innerText.trim();  //현재 버튼의 상태값  DB:STATE 값 
                    if (state == '') state = 'N';

                    Idx = dataItemsInfo[rowindex - 1].get_cell("IDX").innerText;         // Cell(IDX)의 Text
                    Idx = parseInt(Idx);
                }
                else if (dataItemsCustomer.length > 0) {
                    HowGrid = 'customer';
                    var state = dataItemsCustomer[rowindex - 1].get_cell("STATES").innerText.trim();
                    if (state == '') state = 'N';

                    Idx = dataItemsCustomer[rowindex - 1].get_cell("IDX").innerText;     // Cell(IDX)의 Text
                    Idx = parseInt(Idx);
                }


                var SchemeProcessId = $('#<%= hddProcessID.ClientID %>').val();              // processID
                var userId = $('#<%= hddRequestId.ClientID %>').val();                       // Request UserID
                var receipientId = $('#<%= hddReceipientId.ClientID %>').val();              // Receipient ID


                var loadingPanel = "#<%= radLoading.ClientID %>";

                $(loadingPanel).show();
                var svcUrl = WCFSERVICE + "/AfterTreatmentServices.svc/UpdataFreeGoods/" + SchemeProcessId + "/" + Idx + "/" + HowGrid + "/" + state + "/" + userId;
                $.support.cors = true;
                $.ajax({
                    type: "GET",
                    url: svcUrl,
                    dataType: "JSON",
                    success: function (data) {
                        setTimeout(function () { }, 1000);
                        if (data == 'success') {
                            if (HowGrid == 'info') {
                                if (state == 'N') {
                                    __doPostBack('Cancel', 'CI');
                                    alert("Cancel");
                                }
                                else if (state == 'C') {
                                    __doPostBack('Canceled', 'CD');
                                    alert("Canceled");
                                }
                            }
                            else if (HowGrid == 'customer') {
                                if (state == 'N') {
                                    __doPostBack('Cancel', 'CI');
                                    alert("Cancel");
                                }
                                else if (state == 'C') {
                                    __doPostBack('Canceled', 'CD');
                                    alert("Canceled");
                                }
                            }
                        }
                    },
                    error: function (e) {
                        fn_OpenErrorMessage('error>>' + e)
                    },
                    complete: function () {

                        $(loadingPanel).hide();
                    }
                });

            } catch (exception) {
                fn_OpenErrorMessage(exception);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
     <telerik:RadAjaxLoadingPanel ID="radLoading" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <div class="doc_style">
        <div class="data_type1">
            <table>
                <colgroup>
                    <col />
                    <col style="width: 75%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th>BU</th>
                        <td>
                            <div id="divBU" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton ID="radRdoBuHH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="HH" Value="HH" AutoPostBack="false" OnClientCheckedChanged="fn_ClinetDisplay"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBuWH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="WH" Value="WH" AutoPostBack="false" OnClientCheckedChanged="fn_ClinetDisplay"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBuSM" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="SM" Value="SM" AutoPostBack="false" OnClientCheckedChanged="fn_ClinetDisplay"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBuR" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="R" Value="R" AutoPostBack="false" OnClientCheckedChanged="fn_ClinetDisplay"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBuCH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="CH" Value="CH" AutoPostBack="false" OnClientCheckedChanged="fn_ClinetDisplay"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBuDC" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="DC" Value="DC" AutoPostBack="false" OnClientCheckedChanged="fn_ClinetDisplay"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBuAH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="AH" Value="AH" OnClientCheckedChanged="fn_ClinetDisplay" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBuCC" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="CC" Value="CC" AutoPostBack="false" OnClientCheckedChanged="fn_ClinetDisplay" Visible="false"></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr id="divlocation">
                        <th>Location</th>
                        <td>
                            <div id="divLocation" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton ID="radRdo01" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Location" Text="서울" Value="서울" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdo02" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Location" Text="대전" Value="대전" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdo03" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Location" Text="대구" Value="대구" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdo04" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Location" Text="부산" Value="부산" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdo05" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Location" Text="광주" Value="광주" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdo06" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Location" Text="안성공장" Value="안성공장" AutoPostBack="false"></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <h3>Title and Purpose of Free Goods distribution</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col />
                    <col style="width: 75%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Title</th>
                        <td>
                            <telerik:RadButton ID="radRdoSample" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="purpose" Text="Sample(HCP)" Value="Sample" OnClientCheckedChanged="fn_purpose" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoOther" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="purpose" Text="Other" Value="Other" OnClientCheckedChanged="fn_purpose" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Purpose</th>
                        <td>
                            <span id="purposeText" class="text_red text_size11">Please Select Title</span>
                            <div id="purposeSample" style="display: none">
                                <telerik:RadDropDownList ID="RadDropSample" runat="server" Width="300" DefaultMessage="--- Select ---" OnClientSelectedIndexChanged="fn_SetGridVisible" OnItemSelected="RadDropOther_ItemSelected"  AutoPostBack="true">
                                    <Items>
                                        <telerik:DropDownListItem Text="Identification of form. dosage" Value="DOSAGE"/>
                                        <telerik:DropDownListItem Text="DC Use" Value="DC_U"    />
                                        <telerik:DropDownListItem Text="Investigational Medical Product" Value="IMP" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </div>
                            <div id="purposeOther" style="display: none">
                                <telerik:RadDropDownList ID="RadDropOther" runat="server" Width="300" DefaultMessage="--- Select ---" OnClientSelectedIndexChanged="fn_SetGridVisible" OnItemSelected="RadDropOther_ItemSelected"  AutoPostBack="true">
                                    <Items>
                                        <telerik:DropDownListItem Text="Liquid for machine testing(only DC)" Value="Other01" />
                                        <telerik:DropDownListItem Text="Replacement for breakdown & mending(only DC)" Value="Other02" />
                                        <telerik:DropDownListItem Text="medical appliance for injection(only HH & SM)" Value="Other03" />
                                        <telerik:DropDownListItem Text="Phantom Study(only Radiology)" Value="Other10" />
                                        <telerik:DropDownListItem Text="Product testing(only AH)" Value="Other04" />
                                        <telerik:DropDownListItem Text="Clinical sample(only AH)" Value="Other09" />
                                        <telerik:DropDownListItem Text="Promotion (non-pharmaceutical products only)" Value="Other05" />
                                        <telerik:DropDownListItem Text="Product photograph" Value="Other06" />
                                        <telerik:DropDownListItem Text="PPL-product placement for drama of other TVC programs" Value="Other07" />
                                        <telerik:DropDownListItem Text="BIK(Bonus in Kind)" Value="Other08" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr id="rowDBoxCode" style="display: none">
                        <th>IMPACT no.</th>
                        <td>
                            <telerik:RadTextBox ID="rxtTxtDBoxCode" runat="server"></telerik:RadTextBox></td>
                    </tr>
                    <tr id="rowPO_NO" style="display: none">
                        <th>PO NO</th>
                        <td>
                            <telerik:RadTextBox ID="rxtTxtPO_NO" runat="server"></telerik:RadTextBox></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="info" style="display: none">
            <h3>Sample Information                          
                <div class="title_btn">
                    <telerik:RadButton runat="server" ID="RadButton1" CssClass="btn btn-blue btn-size1 bold" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" Text="Add" OnClientClicked="fn_OnAddButtonClicked01"></telerik:RadButton>
                </div>
            </h3>
            <%--<div class="data_type1">--%>
                <telerik:RadGrid runat="server" ID="RadGrdSampleInfo" Skin="EXGrid" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Left" ShowFooter="false" Width="100%" OnItemDataBound="RadGrdSampleInfo_ItemDataBound1"  OnItemCommand="RadGrdSampleInfo_ItemCommand" AllowAutomaticUpdates="true">
                    <MasterTableView EditMode="Batch">
                        <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />                        
                        <Columns>
                            <telerik:GridBoundColumn Display="false" DataField="IDX" HeaderText="" UniqueName="IDX" ReadOnly="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn Display="false" DataField="IS_DOCTOR_PHARMACY" HeaderText="IS_DOCTOR_PHARMACY" UniqueName="IS_DOCTOR_PHARMACY" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn Display="false" DataField="PURPOSE" HeaderText="PURPOSE" UniqueName="PURPOSE" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="INSTITUE_NAME" HeaderText="Institue" UniqueName="INSTITUE_NAME" HeaderStyle-Width="22%" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn Display="false" DataField="INSTITUE_CODE" HeaderText="INSTITUE_CODE" UniqueName="INSTITUE_CODE"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn Display="false" DataField="HCP_CODE" HeaderText="HCP_CODE" UniqueName="HCP_CODE"></telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn  ItemStyle-HorizontalAlign="Left" DataField="HCP_NAME" UniqueName="HCP_NAME" HeaderText="HCP Name" HeaderStyle-Width="15%">
                                <ItemTemplate >                                    
                                    <%#DataBinder.Eval(Container.DataItem, "HCP_NAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox runat="server" ID="RadtxtHcpname" Width="99%" ClientEvents-OnBlur="isDuplicationCC" ></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="SPECIALTY_NAME" HeaderText="Specialty" UniqueName="SPECIALTY_NAME" HeaderStyle-Width="10%" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="SAMPLE_NAME" UniqueName="SAMPLE_NAME" HeaderText="Sample Name" HeaderStyle-Width="38%">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "SAMPLE_NAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="RadtxtSample" runat="server" ReadOnly="true" AutoPostBack="false"  Width="90%" >
                                    </telerik:RadTextBox>
                                    <telerik:RadButton ID="RadbtnSample" runat="server" Text="" OnClientClicked="fn_OpenSample" CssClass="btn_grid" Width="18px" Height="18px">
                                        <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                                    </telerik:RadButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn Display="false" DataField="SAMPLE_CODE" HeaderText="SAMPLE_CODE" UniqueName="SAMPLE_CODE" HeaderStyle-Width="15%"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn Display="false" DataField="SAP_PRODUCT_CODE" HeaderText="SAP_PRODUCT_CODE" UniqueName="SAP_PRODUCT_CODE" HeaderStyle-Width="15%"></telerik:GridBoundColumn>                            
                            <telerik:GridBoundColumn DataField="QTY" HeaderText="Quantity" UniqueName="QTY" HeaderStyle-Width="8%" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn Display="false" DataField="STATES" HeaderText="" UniqueName="STATES">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn Display="false"  UniqueName="CANCEL01" HeaderStyle-Width="15px" HeaderText="" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <telerik:RadButton runat="server" ID="RadbtnCancel01" Width="100%" ButtonType="LinkButton" Text="C"  OnClientClicking="fn_ConfrimFreeGood"  ></telerik:RadButton>
                                </ItemTemplate>
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
            <%--</div>--%>
        </div>

        <div id="customer" style="display: none">
            <h3>Sample Information					
                <div class="title_btn">
                    <telerik:RadButton runat="server" ID="RadbtnAdd" CssClass="btn btn-blue btn-size1 bold" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" Text="Add" OnClientClicked="fn_OnAddButtonClicked02" OnClick="RadButton2_Click"></telerik:RadButton>
                </div>
            </h3>
                <telerik:RadGrid runat="server" ID="RadGrdCustomer" Skin="EXGrid" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Left" ShowFooter="false" Width="100%" OnItemCommand="RadGrdCustomer_ItemCommand" OnItemDataBound="RadGrdCustomer_ItemDataBound1" AllowAutomaticUpdates="true">
                    <MasterTableView EditMode="Batch">
                        <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                        <Columns>
                            <telerik:GridBoundColumn Display="false" DataField="IDX" HeaderText="" UniqueName="IDX" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn Display="false" DataField="PURPOSE" HeaderText="PURPOSE" UniqueName="PURPOSE" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" DataField="CUSTOMER" UniqueName="CUSTOMER" HeaderText="Customer" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "CUSTOMER")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox runat="server" ID="RadtxtCustomer" Width="99%"></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" DataField="RECEIPTER" UniqueName="RECEIPTER" HeaderText="Receipter" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "RECEIPTER")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox runat="server" ID="RadtxtReceipter" Width="99%"></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="SAMPLE_NAME" UniqueName="SAMPLE_NAME" HeaderText="Sample Name" HeaderStyle-Width="43%">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "SAMPLE_NAME")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="RadtxtSample2" runat="server" ReadOnly="true" AutoPostBack="false" Width="90%">
                                    </telerik:RadTextBox>
                                    <telerik:RadButton ID="RadbtnSample2" runat="server" Text="" OnClientClicked="fn_OpenSample" CssClass="btn_grid" Width="18px" Height="18px">
                                        <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                                    </telerik:RadButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn Display="false" DataField="SAMPLE_CODE" HeaderText="SAMPLE_CODE" UniqueName="SAMPLE_CODE" HeaderStyle-Width="15%"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn Display="false" DataField="SAP_PRODUCT_CODE" HeaderText="SAP_PRODUCT_CODE" UniqueName="SAP_PRODUCT_CODE" HeaderStyle-Width="15%"></telerik:GridBoundColumn>                            
                            <telerik:GridTemplateColumn  DataField="QTY" UniqueName="QTY" HeaderText="Quantity" HeaderStyle-Width="8%">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("QTY")) %>' CssClass="lbl_align_right"></asp:Label>                                                                      
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="RadtxtQty" runat="server" Width="100%" CssClass="input align_right"
                                    onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                    DecimalDigits="0" AllowNegative="false"> 
                                </asp:TextBox>                                    
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn Display="false" DataField="STATES" HeaderText="" UniqueName="STATES">
                            </telerik:GridBoundColumn>
                             <telerik:GridTemplateColumn Display="false"  UniqueName="CANCEL02" HeaderStyle-Width="15px" HeaderText="" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <telerik:RadButton runat="server" ID="RadbtnCancel02" Width="100%" ButtonType="LinkButton" Text="C" OnClientClicking="fn_ConfrimFreeGood"  ></telerik:RadButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>                            
                            <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON2" HeaderStyle-Width="10px" HeaderText="" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" ImageUrl="~/Styles/images/ico_del.png"
                                        OnClientClick='<%# String.Format("return openConfirmPopUp2({0});",Eval("IDX"))%> ' BorderStyle="None" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>                    
                </telerik:RadGrid>
        </div>

        <div class="data_type1">
            <table>
                <colgroup>
                    <col />
                    <col style="width: 75%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Comment</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="radTextComment" Width="100%" TextMode="MultiLine" Height="80px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

      <%--  <div id="Recipt" class="align_right pb10" style="display: none" runat="server">
            <telerik:RadButton runat="server" ID="RadbtnRecipt" ButtonType="ToggleButton" OnClientClicked="fn_OpenReceipt" ForeColor="White" CssClass="btn btn-blue btn-size1 bold" Width="100" Text="Receipt" AutoPostBack="false"></telerik:RadButton>
        </div>--%>
    </div>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinPopupSample" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Sample" Modal="true" Width="550px" Height="640px" Behaviors="Default" OnClientClose="fn_ClientClose" NavigateUrl="./ProductList.aspx"></telerik:RadWindow>
        </Windows>        
        <Windows>
             <telerik:RadWindow ID="radWininfo" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Docter&Pharmacy" Width="750px" Height="640px"
                Behaviors="Default" Modal="true" CssClass="windowscroll" OnClientClose="fn_ClinetCloseinfo" NavigateUrl="./Doctor_PharmacyList.aspx">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <input type="hidden" id="hddGridItemsCustomer" runat="server" />
    <input type="hidden" id="hddGridItems" runat="server" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddBu" runat="server" />
    <input type="hidden" id="hddGridSelect" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
	<input type="hidden" id="hddAddRow" runat="server" value="N" />

    <input type="hidden" id="hddRequestId" runat="server"  />
    <input type="hidden" id="hddReceipientId" runat="server"  />
</asp:Content>

