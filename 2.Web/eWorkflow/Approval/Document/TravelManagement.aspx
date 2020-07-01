<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="TravelManagement.aspx.cs" Inherits="Approval_Document_TravelManagement" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript" src="../../Scripts/Common/Outlook.js"></script>
    <script type="text/javascript">

function pageLoad() {
    var addRow = $('#<%= hddAddRow.ClientID %>').val();
    var grid = null;
    if (addRow == 'Rebind_Employee') {
        grid = $find('<%=radGrdEmployee.ClientID%>');
    } else if (addRow == 'Rebind_External') {
        grid = $find('<%=radGrdExternal.ClientID%>');
    } else if (addRow == 'Rebind_TripRoute') {
        grid = $find('<%=radGrdTripRoute.ClientID%>');
    } else if (addRow == 'Rebind_Accommodation') {
        grid = $find('<%= radGrdEstimationDetails.ClientID%>');
    }
    if (grid && grid.get_masterTableView()) {
        var dataItems = grid.get_masterTableView().get_dataItems();
        if (dataItems.length > 0) {
            grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
        }
    }

    if (addRow.indexOf('Rebind') == 0)
        $('#<%= hddAddRow.ClientID %>').val('N');

            var quoation = $('#<%= hddQuotationAttachFiles.ClientID %>').val();
            if(quoation.length > 0){
                var obj = JSON.parse(quoation);
                $('#<%= hrefQuotationFile.ClientID %>').val(obj.DISPLAY_FILE_NAME);
            }
            

}
function fn_UpdateGridDataFromUploader() {
    fn_UpdateGridData(null, false, false, false);
}

function fn_DoRequest(sender, arsg) {
    var requestAgency = $('#<%= hddIsAfterSendMail.ClientID%>').val();
    if (requestAgency == 'Y')
        return fn_UpdateGridData(null, false, false, false);
    else {
        fn_OpenDocInformation('Agency에 메일 발송 바랍니다.')
        return false;
    }
}

function fn_DoSave(sender, args) {
    return fn_UpdateGridData(null, false, false, false);
}

function fn_UpdateGridData(uListData, addExternalRow, addTripRoute, addAccommodation) {
    fn_UpdateEmployeeGridData(uListData);
    fn_UpdateExternalGridData(addExternalRow);
    fn_UpdateTripRouteGridData(addTripRoute);
    fn_UpdateAccommodationGridData(addAccommodation);
    return true;
}

/*----------------------------------------------------------------------------------
Employee Traveler 관련

------------------------------------------------------------------------------------*/
//Employee마지막 Row Validation
function checkLastEmployee() {
    var masterTable = $find('<%= radGrdEmployee.ClientID %>').get_masterTableView();
    var dataItems = masterTable.get_dataItems();
    if (dataItems.length > 0) {
        var lastItem = dataItems[dataItems.length - 1];
        var costCenter = lastItem.get_cell("COST_CODE").children[0].innerText.trim();
        if (costCenter.length < 1) {
            return false;
        } else {
            return true;
        }
    }
    return true;
}

//Open 사용자
function fn_ShowUserPopup(sender, Args) {
    if (checkLastEmployee()) {
        var wnd = $find("<%= winPopupUser.ClientID %>");
        wnd.setUrl("/eWorks/Common/Popup/UserList.aspx");
        wnd.show();
        sender.set_autoPostBack(false);
    } else {
        fn_OpenDocInformation('자료를 입력바랍니다.');
        sender.set_autoPostBack(false);
    }
}

function OnClientClose(sender, args) {
    if (sender.get_navigateUrl() == '/eWorks/Common/Popup/UserList.aspx') {
        var uListData = args.get_argument();
        if (uListData != null) {
            fn_UpdateGridData(uListData, false, false, false);
            $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind_Employee");
        }
    } else if (sender.get_navigateUrl() == '/eWorks/Common/Popup/CountryCity.aspx') {
        var city = args.get_argument();
        if (city) {
            fn_UpdateGridData(null, false, false, false); //row 추가는 아니다.
            $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("ApplyCity:" + tripIdx + ":" + tripTarget + ":" + city);
        }
    }
}

function fn_UpdateEmployeeGridData(uListData) {
    var list = [];
    var masterTable = $find('<%= radGrdEmployee.ClientID %>').get_masterTableView();
    $find('<%= radGrdEmployee.ClientID %>').get_batchEditingManager().saveChanges(masterTable);

    var dataItems = masterTable.get_dataItems();

    var maxIdx = 0;
    for (var i = 0; i < dataItems.length; i++) {

        var idx = dataItems[i].get_cell("IDX").innerText;
        var type = dataItems[i].get_cell("TRAVELER_TYPE").innerText;
        var id = dataItems[i].get_cell("TRAVELER_ID").innerText;
        var name = dataItems[i].get_cell("TRAVELER_NAME").innerText;
        var sex = '';
        if (dataItems[i].get_cell("SEX").children[0]) //ReadOnly인경우
            sex = dataItems[i].get_cell("SEX").children[0].innerText;
        else
            sex = dataItems[i].get_cell("SEX").innerText;
        var empNo = dataItems[i].get_cell("EMP_NO").innerText;
        var div = dataItems[i].get_cell("DIV_DEPT").innerText;
        var cos = '';
        if (dataItems[i].get_cell("COST_CODE").children[0])
            cost = dataItems[i].get_cell("COST_CODE").children[0].innerText;
        else
            cost = dataItems[i].get_cell("COST_CODE").innerText;

        var conObj = {
            IDX: null,
            TRAVELER_TYPE: null,
            TRAVELER_ID: null,
            TRAVELER_NAME: null,
            SEX: null,
            EMP_NO: null,
            DIV_DEPT: null,
            COMPANY_ORG: null,
            COST_CODE: null,
            INTERNAL_ORDER: null,
        }
        conObj.IDX = idx;
        conObj.TRAVELER_TYPE = type;
        conObj.TRAVELER_ID = id;
        conObj.TRAVELER_NAME = name;
        conObj.SEX = sex;
        conObj.EMP_NO = empNo;
        conObj.DIV_DEPT = div;
        conObj.COMPANY_ORG = '';
        conObj.COST_CODE = cost;
        conObj.INTERNAL_ORDER = '';
        list.push(conObj);
        maxIdx = parseInt(idx);
    }

    if (uListData) {
        if (uListData.length > 0) {
            var user = uListData[0];
            var conObj = {
                IDX: null,
                TRAVELER_TYPE: null,
                TRAVELER_ID: null,
                TRAVELER_NAME: null,
                SEX: null,
                EMP_NO: null,
                DIV_DEPT: null,
                COMPANY_ORG: null,
                COST_CODE: null,
                INTERNAL_ORDER: null,
            }

            conObj.IDX = maxIdx + 1;
            conObj.TRAVELER_TYPE = 'I';
            conObj.TRAVELER_ID = user.USER_ID;
            var name = user.FULL_NAME;
            var startIdx = name.indexOf('(');
            if (startIdx && startIdx > 0)
                name = name.substr(0, startIdx);

            conObj.TRAVELER_NAME = name;
            conObj.SEX = user.FORM_OF_ADDRESS;
            conObj.DIV_DEPT = user.ORG_ACRONYM;
            conObj.EMP_NO = user.IPIN;
            conObj.DIV_DEPT = user.ORG_ACRONYM;
            conObj.COMPANY_ORG = '';
            conObj.COST_CODE = user.COST_CENTER;
            conObj.INTERNAL_ORDER = '';
            list.push(conObj);
        }
    }

    $('#<%= hddEmployeeGridItems.ClientID%>').val(JSON.stringify(list));
    return true;
}

/*-----------------------------------------------------------------------------------
External Traveler관련
-------------------------------------------------------------------------------------*/

        function fn_UpdateExternalGridData(addRow) {
            var list = [];
            var masterTable = $find('<%= radGrdExternal.ClientID %>').get_masterTableView();
            $find('<%= radGrdExternal.ClientID %>').get_batchEditingManager().saveChanges(masterTable);
            var dataItems = masterTable.get_dataItems();

            var maxIdx = 0;
            for (var i = 0; i < dataItems.length; i++) {

                var idx = dataItems[i].get_cell("IDX").innerText;
                var type = dataItems[i].get_cell("TRAVELER_TYPE").innerText;
                var id = dataItems[i].get_cell("TRAVELER_ID").innerText;
                var sex = '', companyOrg = '', cost = '', internal = '', name = '';
                if (dataItems[i].get_cell("TRAVELER_NAME").children[0])
                    name = dataItems[i].get_cell("TRAVELER_NAME").children[0].innerText;
                else
                    name = dataItems[i].get_cell("TRAVELER_NAME").innerText;
                var sex = '', companyOrg = '', cost = '', internal = '';
                if (dataItems[i].get_cell("SEX").children[0])
                    sex = dataItems[i].get_cell("SEX").children[0].innerText;
                else
                    sex = dataItems[i].get_cell("SEX").innerText;
                if (dataItems[i].get_cell("COMPANY_ORG").children[0])
                    companyOrg = dataItems[i].get_cell("COMPANY_ORG").children[0].innerText.trim();
                else
                    companyOrg = dataItems[i].get_cell("COMPANY_ORG").innerText.trim();
                if (dataItems[i].get_cell("COST_CODE").children[0])
                    cost = dataItems[i].get_cell("COST_CODE").children[0].innerText.trim();
                else
                    cost = dataItems[i].get_cell("COST_CODE").innerText.trim();
                if (dataItems[i].get_cell("INTERNAL_ORDER").children[0])
                    internal = dataItems[i].get_cell("INTERNAL_ORDER").children[0].innerText.trim();
                else
                    internal = dataItems[i].get_cell("INTERNAL_ORDER").innerText.trim();
                var conObj = {
                    IDX: null,
                    TRAVELER_TYPE: null,
                    TRAVELER_ID: null,
                    TRAVELER_NAME: null,
                    SEX: null,
                    EMP_NO: null,
                    DIV_DEPT: null,
                    COMPANY_ORG: null,
                    COST_CODE: null,
                    INTERNAL_ORDER: null,
                }
                conObj.IDX = idx;
                conObj.TRAVELER_TYPE = type;
                conObj.TRAVELER_ID = id;
                conObj.TRAVELER_NAME = name;
                conObj.SEX = sex;
                conObj.EMP_NO = '';
                conObj.DIV_DEPT = '';
                conObj.COMPANY_ORG = companyOrg;
                conObj.COST_CODE = cost;
                conObj.INTERNAL_ORDER = internal;
                list.push(conObj);
                maxIdx = parseInt(idx);
            }

            if (addRow) {
                var conObj = {
                    IDX: null,
                    TRAVELER_TYPE: null,
                    TRAVELER_ID: null,
                    TRAVELER_NAME: null,
                    SEX: null,
                    EMP_NO: null,
                    DIV_DEPT: null,
                    COMPANY_ORG: null,
                    COST_CODE: null,
                    INTERNAL_ORDER: null,
                }

                conObj.IDX = maxIdx + 1;
                conObj.TRAVELER_TYPE = 'I';
                conObj.TRAVELER_ID = '';
                conObj.TRAVELER_NAME = '';
                conObj.SEX = "Mr.";
                conObj.DIV_DEPT = '';
                conObj.EMP_NO = '';
                conObj.DIV_DEPT = '';
                conObj.COMPANY_ORG = '';
                conObj.COST_CODE = '';
                conObj.INTERNAL_ORDER = '';
                list.push(conObj);
            }
            $('#<%= hddExternalGridItems.ClientID%>').val(JSON.stringify(list));
        }

        function fn_OnExternalClicked(sender, args) {
            fn_UpdateGridData(null, true, false, false);
            $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind_External");
            sender.set_autoPostBack(false);
        }

        /*-------------------------------------------------------------------------------------
        Trip Route
        --------------------------------------------------------------------------------------*/
        function checkLastTripRoute() {
            var masterTable = $find('<%= radGrdTripRoute.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            if (dataItems.length > 0) {
                var lastItem = dataItems[dataItems.length - 1];
                var ctlDeparture = lastItem.findControl('radGrdDatDeparture');
                var departureDt = null;
                var returnDt = null;
                if (ctlDeparture) {
                    if (!(ctlDeparture._dateInput._value)) return false;
                    departureDt = ctlDeparture.get_element().value;

                    var ctlReturn = lastItem.findControl('radGrdDatReturn');
                    if ((ctlReturn._dateInput._value))
                        returnDt = lastItem.findControl('radGrdDatReturn').get_element().value;
                } else {
                    departureDt = lastItem.get_cell("DEPARTURE_DATE").children[0].innerText;
                    returnDt = lastItem.get_cell("RETURN_DATE").children[0].innerText;
                }
                var departure = lastItem.get_cell("DEPARTURE_CODE").children[0].innerText.trim();
                var arrival = lastItem.get_cell("ARRIVAL_CODE").children[0].innerText.trim();
                var tripType = lastItem.get_cell("TRIP_TYPE").children[0].innerText.trim();

                var airplaneClass = lastItem.get_cell("AIRPLANE_CLASS").children[0].innerText.trim();
                if (tripType == 'Round') {
                    if (departureDt.length < 1 && returnDt.length < 1 && departure.length < 1 && arrival.length < 1 && airplaneClass.length < 1)
                        return false;
                    else
                        return true;
                } else {
                    if (departureDt.length < 1 && departure.length < 1 && arrival.length < 1 && airplaneClass.length < 1) {
                        return false;
                    } else {
                        return true;
                    }
                }
            }
            return true;
        }

        function fn_UpdateTripRouteGridData(addRow) {
            var list = [];
            var masterTable = $find('<%= radGrdTripRoute.ClientID %>').get_masterTableView();
            $find('<%= radGrdTripRoute.ClientID %>').get_batchEditingManager().saveChanges(masterTable);
            var dataItems = masterTable.get_dataItems();

            var maxIdx = 0;
            for (var i = 0; i < dataItems.length; i++) {

                var idx = dataItems[i].get_cell("IDX").innerText;
                var ctlDeparture = dataItems[i].findControl('radGrdDatDeparture');
                var departureDt = null;
                var returnDt = null;
                if (ctlDeparture) {
                    departureDt = dataItems[i].findControl('radGrdDatDeparture').get_element().value;
                    returnDt = dataItems[i].findControl('radGrdDatReturn').get_element().value;
                } else {
                    departureDt = dataItems[i].get_cell("DEPARTURE_DATE").children[0].innerText;
                    returnDt = dataItems[i].get_cell("RETURN_DATE").children[0].innerText;
                }
                var departure = '', arrival = '', tripType = '', airplaneClass = '';
                if (dataItems[i].get_cell("DEPARTURE_CODE").children[0])
                    departure = dataItems[i].get_cell("DEPARTURE_CODE").children[0].innerText;
                else
                    departure = dataItems[i].get_cell("DEPARTURE_CODE").innerText;
                if (dataItems[i].get_cell("ARRIVAL_CODE").children[0])
                    arrival = dataItems[i].get_cell("ARRIVAL_CODE").children[0].innerText;
                else
                    arrival = dataItems[i].get_cell("ARRIVAL_CODE").innerText;
                if (dataItems[i].get_cell("TRIP_TYPE").children[0])
                    tripType = dataItems[i].get_cell("TRIP_TYPE").children[0].innerText;
                else
                    tripType = dataItems[i].get_cell("TRIP_TYPE").innerText;
                if (dataItems[i].get_cell("AIRPLANE_CLASS").children[0])
                    airplaneClass = dataItems[i].get_cell("AIRPLANE_CLASS").children[0].innerText;
                else
                    airplaneClass = dataItems[i].get_cell("AIRPLANE_CLASS").innerText;

                var conObj = {
                    IDX: null,
                    DEPARTURE_DATE: null,
                    RETURN_DATE: null,
                    DEPARTURE_CODE: null,
                    ARRIVAL_CODE: null,
                    TRIP_TYPE: null,
                    TRIP_TYPE_NAME: null,
                    AIRPLANE_CLASS: null,
                    AIRPLANE_CLASS_NAME: null,
                }
                conObj.IDX = idx;
                conObj.DEPARTURE_DATE = departureDt;
                conObj.RETURN_DATE = returnDt;
                conObj.DEPARTURE_CODE = departure;
                conObj.ARRIVAL_CODE = arrival;
                conObj.TRIP_TYPE = tripType;
                conObj.TRIP_TYPE_NAME = tripType;
                conObj.AIRPLANE_CLASS = airplaneClass;
                conObj.AIRPLANE_CLASS_NAME = airplaneClass;
                list.push(conObj);
                maxIdx = parseInt(idx);
            }

            if (addRow) {
                var conObj = {
                    IDX: null,
                    DEPARTURE_DATE: null,
                    RETURN_DATE: null,
                    DEPARTURE_CODE: null,
                    ARRIVAL_CODE: null,
                    TRIP_TYPE: null,
                    TRIP_TYPE_NAME: null,
                    AIRPLANE_CLASS: null,
                    AIRPLANE_CLASS_NAME: null,
                }

                conObj.IDX = maxIdx + 1;
                conObj.DEPARTURE_DATE = '';
                conObj.RETURN_DATE = '';
                conObj.DEPARTURE_CODE = '';
                conObj.ARRIVAL_CODE = '';
                conObj.TRIP_TYPE = '';
                conObj.TRIP_TYPE_NAME = '';
                conObj.AIRPLANE_CLASS = '';
                conObj.AIRPLANE_CLASS_NAME = '';
                list.push(conObj);
            }
            $('#<%= hddTripRouteGridItems.ClientID%>').val(JSON.stringify(list));
            return true;
        }

        function fn_OnTripRouteClicked(sender, args) {
            if (checkLastTripRoute()) {
                fn_UpdateGridData(null, false, true, false);
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind_TripRoute");
            } else {
                fn_OpenDocInformation('자료를 입력바랍니다.');
            }

        }

        var tripIdx = null;
        function getDataItemKeyValue(radGrid, item, name) {
            return radGrid.get_masterTableView().getCellByColumnUniqueName(item, name).innerHTML;
        }

        var tripTarget = null;
        function openDepartureCityPopup() {
            tripTarget = 'departure';
            openCityPopup();
        }

        function openArrivalCityPopup() {
            tripTarget = 'arrival';
            openCityPopup();
        }

        function openCityPopup() {
            var masterTable = $find('<%= radGrdTripRoute.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            for (var i = 0; i < dataItems.length; i++) {
                var btnCity = dataItems[i].findControl('radBtnDepartureCity');
                if (btnCity) {
                    tripIdx = dataItems[i].get_cell('IDX').innerText;
                }
            }
            var row = $find('<%= radGrdTripRoute.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            var dataItem = $find(row.id);
            if (dataItem)
                tripIdx = parseInt(dataItem.get_cell('IDX').innerText);

            var wnd = $find("<%= winPopupCity.ClientID %>");
            wnd.set_title('Country/City')
            wnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close);
            wnd.set_height(250);
            wnd.set_width(450);
            wnd.setUrl("/eWorks/Common/Popup/CountryCity.aspx");
            wnd.show();
            return false;
        }

        /*-------------------------------------------------------------------------------------
        Estimation
        --------------------------------------------------------------------------------------*/

        function fn_UpdateAccommodationGridData(addRow) {
            var list = [];
            var grid = $find('<%= radGrdEstimationDetails.ClientID %>');
            //Agency에 메일을 보내기전에는 Grid가 보이지 않는다.
            if (grid) {
                var masterTable = $find('<%= radGrdEstimationDetails.ClientID %>').get_masterTableView();
                $find('<%= radGrdEstimationDetails.ClientID %>').get_batchEditingManager().saveChanges(masterTable);
                var dataItems = masterTable.get_dataItems();

                var maxIdx = 0;
                for (var i = 0; i < dataItems.length; i++) {

                    var idx = dataItems[i].get_cell("IDX").innerText;
                    var code = dataItems[i].get_cell("ACCOMMODATION_CODE").children[0].innerText;

                    var ctlFrom = dataItems[i].findControl('radGrdDatFrom');
                    var fromDt = null;
                    var toDt = null;

                    if (ctlFrom) {
                        fromDt = dataItems[i].findControl('radGrdDatFrom').get_element().value;
                        toDt = dataItems[i].findControl('radGrdDatTo').get_element().value;
                    } else {
                        fromDt = dataItems[i].get_cell("FROM_DATE").children[0].innerText;
                        toDt = dataItems[i].get_cell("TO_DATE").children[0].innerText;
                        //fn_OpenDocInformation('DATE 를 입력바랍니다.');
                    }



                    var amount = dataItems[i].get_cell("AMOUNT_PER_NIGHT").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                    var total = 0;
                    var reason = dataItems[i].get_cell("REASON_HOTEL").children[0].innerText;

                    var conObj = {
                        IDX: null,
                        ACCOMMODATION_CODE: null,
                        ACCOMMODATION_NAME: null,
                        FROM_DATE: null,
                        TO_DATE: null,
                        AMOUNT_PER_NIGHT: null,
                        AMOUNT_TOTAL: null,
                        REASON_HOTEL: null,
                    }
                    conObj.IDX = idx;
                    conObj.ACCOMMODATION_CODE = code;
                    conObj.ACCOMMODATION_NAME = code;
                    conObj.FROM_DATE = fromDt;
                    conObj.TO_DATE = toDt;
                    conObj.AMOUNT_PER_NIGHT = amount;
                    conObj.AMOUNT_TOTAL = total;
                    conObj.REASON_HOTEL = reason;
                    list.push(conObj);
                    maxIdx = parseInt(idx);
                }

                if (addRow) {
                    var conObj = {
                        IDX: null,
                        ACCOMMODATION_CODE: null,
                        ACCOMMODATION_NAME: null,
                        FROM_DATE: null,
                        TO_DATE: null,
                        AMOUNT_PER_NIGHT: null,
                        AMOUNT_TOTAL: null,
                        REASON_HOTEL: null,
                    }

                    conObj.IDX = maxIdx + 1;
                    conObj.ACCOMMODATION_CODE = '';
                    conObj.ACCOMMODATION_NAME = '';
                    conObj.FROM_DATE = '';
                    conObj.TO_DATE = '';
                    conObj.AMOUNT_PER_NIGHT = 0;
                    conObj.AMOUNT_TOTAL = 0;
                    conObj.REASON_HOTEL = '';
                    list.push(conObj);
                }
                $('#<%= hddAccommodationGridItems.ClientID%>').val(JSON.stringify(list));
            }
        }

        function fn_OnAccommodationClicked(sender, args) {
            fn_UpdateGridData(null, false, false, true);
            $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind_Accommodation");
        }

        /*-------------------------------------------------------------------------------------
        Row 삭제처리
        --------------------------------------------------------------------------------------*/
        var clickedKey = null;
        function confirmEmployeeCallBackFn(arg) {
            if (arg) {
                fn_UpdateGridData(null, false, false, false);
                var masterTable = $find('<%= radGrdEmployee.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        function confirmExternalCallBackFn(arg) {
            if (arg) {
                fn_UpdateGridData(null, false, false, false);
                var masterTable = $find('<%= radGrdExternal.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        function confirmTripRouteCallBackFn(arg) {
            if (arg) {
                fn_UpdateGridData(null, false, false, false);
                var masterTable = $find('<%= radGrdTripRoute.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        function confirmAccommodationCallBackFn(arg) {
            if (arg) {
                fn_UpdateGridData(null, false, false, false);
                var masterTable = $find('<%= radGrdEstimationDetails.ClientID %>').get_masterTableView();
            masterTable.fireCommand("Remove", clickedKey);
        }
    }

    function openConfirmPopUp(gridName, index) {
        clickedKey = parseInt(index);
        var msg = 'Do you want to delete this Item ?';
        if (gridName == 'Employee')
            fn_OpenConfirm(msg, confirmEmployeeCallBackFn);
        else if (gridName == 'External')
            fn_OpenConfirm(msg, confirmExternalCallBackFn);
        else if (gridName == 'TripRoute')
            fn_OpenConfirm(msg, confirmTripRouteCallBackFn);
        else if (gridName == 'Accommodation')
            fn_OpenConfirm(msg, confirmAccommodationCallBackFn);

        return false;
    }

    function fn_OnTrypTypeIndexChanged(sender, args) {
        var row = $find('<%= radGrdTripRoute.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
        if (row) {
            var dataItem = $find(row.id);
            var datReturn = dataItem.findControl('radGrdDatReturn');
            if (datReturn) {
                if (args.get_index() == 1) {
                    datReturn.set_enabled(false);
                    datReturn.set_selectedDate(null);
                } else {
                    datReturn.set_enabled(true);
                }
            }
        }
    }

    function setVisibleControl(num) {
        if (num == '1')
            $("tr[quotation='2']").hide();
        else
            $("tr[quotation='2']").show();
    }

    function fn_OnQuotationIndexChanged(sender, args) {
        var item = args.get_item();
        setVisibleControl(item.get_value());
    }

    function fn_OnRequestToAgencyClick(sender, args) {
        var selectedItem = $find('<%= radDdlPurpose.ClientID %>').get_selectedItem();
        var dtStart = $find('<%= radDatFrom.ClientID %>').get_selectedDate();
        var dtReturn = $find('<%= radDatTo.ClientID %>').get_selectedDate();
        var detail = $find('<%= radTxtDetailInformation.ClientID %>').get_element().value;
        var contactPoint = $find('<%= radTxtContactPoint.ClientID %>').get_element().value;

        var message = "";

        if (!selectedItem) message += 'Purpose';
        if (!dtStart || !dtReturn) {
            if (message.length > 0) message += ",Period";
            else message += "Period";
        }
        if (!contactPoint) {
            if (message.length > 0) message += ",Traveler's Contact Point";
            else message += "Traveler's Contact Point";
        }


        var masterTable = $find('<%= radGrdTripRoute.ClientID %>').get_masterTableView();
        var dataItems = masterTable.get_dataItems();
        if (dataItems.length <= 0) {
            message += ',Trip Route';
        }

        /*if (!detail) {
            if (message.length > 0) message += ",Detail Information";
            else message += "Detail Information";
        }*/
        if (message.length > 0) {
            fn_OpenDocInformation('Please fill out below fields.<hr><b>' + message + "</b>");
        }
        else {
            fn_UpdateGridData(null, false, false, false);;
            $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("RequestToAgency");
        }
    }

    function fn_ClientQuotationFilesUploaded(sender, args) {
        $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest();
    }

    function fn_ClientApplicationFilesUploaded(sender, args) {
        $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest();
}

function SetTotal() {
    var masterTable = $find('<%= radGrdEstimationDetails.ClientID %>').get_masterTableView();
    var dataItems = masterTable.get_dataItems();
    var total = 0;
    for (var i = 0; i < dataItems.length; i++) {
        var amount = dataItems[i].get_cell("AMOUNT_TOTAL").innerText.replace(/,/gi, '').replace(/ /gi, '');

        amount = parseInt(amount);

        total += amount;
    }

    masterTable.get_element().tFoot.rows[0].cells[5].innerText = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

    var airfareAmount = $('#<%= radNumAmount.ClientID %>').val().trim().replace(/,/gi, '').replace(/ /gi, '');;
    if (!airfareAmount) airfareAmount = 0;
    else airfareAmount = parseFloat(airfareAmount);

    $('#<%= radNumExtimationTotal.ClientID %>').html((total + airfareAmount).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
}

function fn_OnGridNumBlur(sender) {
    setNumberFormat(sender);
}

function fn_OnGridKeyUp(sender) {
    var row = $find('<%= radGrdEstimationDetails.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            if (row) {
                var dataItem = $find(row.id);
                if (dataItem) {

                    var dtFrom = null, dtTo = null, price = 0;
                    var ctlFrom = dataItem.findControl('radGrdDatFrom');
                    if (ctlFrom) dtFrom = ctlFrom.get_selectedDate();
                    var ctlTo = dataItem.findControl('radGrdDatTo');
                    if (ctlTo) dtTo = ctlTo.get_selectedDate();
                    var ctlPrice = dataItem.findElement('radGrdNumPrice');
                    if (ctlPrice) price = parseInt(ctlPrice.value.replace(/,/gi, '').replace(/ /gi, ''));
                    if (dtFrom && dtTo) {
                        if (dtFrom <= dtTo) {
                            var diff = new Date(dtTo - dtFrom);
                            var days = diff / 1000 / 60 / 60 / 24;
                            var iDay = parseInt(days);
                            price * iDay;
                            dataItem.get_cell('AMOUNT_TOTAL').innerText = (price * iDay).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                            SetTotal();
                        }
                    }
                }
            }
            return true;
        }



function fn_OnGridDateBlur(sender, args) {
    var row = $find('<%= radGrdEstimationDetails.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
    var dataItem = $find(row.id);
    var newDate = args.get_newDate();
    var dtFrom = null, dtTo = null, price = 0;

    if (sender.get_id().toLowerCase().indexOf("from_date") >= 0)
        dtFrom = newDate;
    else
        dtTo = newDate;



    var ctlFrom = dataItem.findControl('radGrdDatFrom');
    var ctlTo = dataItem.findControl('radGrdDatTo');

    //alert(dtFrom);
    //alert(dtTo);




    if (dataItem) {
        if (dtFrom) {
            var ctlTo = dataItem.findControl('radGrdDatTo');
            if (ctlTo) dtTo = ctlTo.get_selectedDate();
        } else {
            var ctlFrom = dataItem.findControl('radGrdDatFrom');
            if (ctlFrom) dtFrom = ctlFrom.get_selectedDate();
        }

        var ctlPrice = dataItem.findElement('radGrdNumPrice');
        if (ctlPrice) price = parseInt(ctlPrice.value.replace(/,/gi, '').replace(/ /gi, ''));

        if (dtFrom && dtTo) {
            if (dtFrom > dtTo) {
                alert('날자를 확인 바랍니다.');
            }
            else {
                var diff = new Date(dtTo - dtFrom);
                var days = diff / 1000 / 60 / 60 / 24;
                var iDay = parseInt(days);
                price * iDay;
                dataItem.get_cell('AMOUNT_TOTAL').innerText = (price * iDay).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                SetTotal();
            }
        }

    }
}

function fn_OnAmountKeyUp(sender) {
    var masterTable = $find('<%= radGrdEstimationDetails.ClientID %>').get_masterTableView();
    var sTotal = masterTable.get_element().tFoot.rows[0].cells[5].innerText.replace(/,/gi, '').replace(/ /gi, '');
    var total = 0, amount = 0;
    if (sTotal.length > 0) total = parseInt(sTotal);
    amount = $('#<%= radNumAmount.ClientID%>').val();
    amount = parseFloat(amount.replace(/,/gi, '').replace(/ /gi, ''));

    $('#<%= radNumExtimationTotal.ClientID %>').html((amount + total).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));

}

function fn_OnAccoSelectedChanged(sender, args) {
    if (args.get_index() == 1) {
        fn_OpenDocInformation('반드시 이유를 Reason/Hotel에 입력하셔야 합니다.');
    }
}


function fn_OnResponseEnd(sender, args) {
    var argument = args.get_eventArgument();
    if (argument == 'RequestToAgency') {
        fn_RequestToAgency();
    }
}

function fn_RequestToAgency() {
    try {
        var processId = $('#<%= hddProcessID.ClientID %>').val();
        var userId = $('#<%= hddUserId.ClientID %>').val();
        var loadingPanel = "#<%= radLoading.ClientID %>";
        //$(loadingPanel).center();
        $(loadingPanel).show();
        var svcUrl = WCFSERVICE + "/MailServices.svc/SendToAgency/" + processId + "/" + userId;
        $.support.cors = true;
        $.ajax({
            type: "GET",
            url: svcUrl,
            dataType: "JSON",
            success: function (data) {
                //setTimeout(function () { }, 500);
                $('#<%= hddIsAfterSendMail.ClientID%>').val('Y');
                if (data == 'success')
                    fn_OpenDocInformation('정상적으로 메일이 발송되었습니다.', function () { window.close(); });
                else
                    fn_OpenDocInformation('메일발송 문제가 발생 했습니다. 관리자에게 문의바랍니다.', function () { window.close(); });

            },
            error: function (e) {
                fn_OpenErrorMessage('error>>' + e)
            },
            complete: function () {
                // no matter the result, complete will fire, so it's a good place
                // to do the non-conditional stuff, like hiding a loading image.

                $(loadingPanel).hide();
            }
        });
        //$.get(WCFSERVICE + "/MailServices.svc/SendToAgency/" + processid + "/" + userId, function (data) { });
        }
        catch (exception) {
            fn_OpenErrorMessage(exception)
        }
    }
    jQuery.fn.center = function () {
        this.css("position", "absolute");

        this.css("top", ($(window).height() - this.height()) / 2 + $(window).scrollTop() + "px");
        this.css("left", ($(window).width() - this.width()) / 2 + $(window).scrollLeft() + "px");
    }

    function fn_CreateAppointment(subject, body, location, start, end) {
        //alert('outlook');
        //var start = new Date(2014, 11, 21, 15, 20, 0, 0);
        //var end = new Date(2014, 11, 21, 16, 20, 0, 0);
        //AddCalendarAppointment(subject, body, location, start, end);

        return false;
    }

    function fn_CancelClicked(sender, args) {
        try {
            var processId = $('#<%= hddProcessID.ClientID %>').val();
                    var loadingPanel = "#<%= radLoading.ClientID %>";
                    $(loadingPanel).show();
                    var svcUrl = WCFSERVICE + "/AfterTreatmentServices.svc/CancelToAgencyTravelManagement/" + processId;
                    $.support.cors = true;
                    $.ajax({
                        type: "GET",
                        url: svcUrl,
                        dataType: "JSON",
                        success: function (data) {
                            setTimeout(function () { }, 1000);
                            if (data == 'success') {
                                fn_OpenDocInformation('취소되었습니다.', function () { window.close(); });
                            } else if (data == 'fail') {
                                fn_OpenDocInformation('취소 메일발송에 문제가 발생하였습니다. 관리자에게 문의바랍니다.');
                            }
                        },
                        error: function (e) {
                            fn_OpenErrorMessage('error>>' + e)
                        },
                        complete: function () {
                            $(loadingPanel).hide();
                        }
                    });
                }
                catch (exception) {
                    fn_OpenErrorMessage(exception)
                }
            }

            function fn_OnRemoveQuotation() {
                $('#<%= divLinkQuotation.ClientID %>').hide();
                $('#<%= divUpLoadQuotation.ClientID %>').show();
                fn_UpdateGridData(null, false, false, false);
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("DeleteQuotation");
        return false;
    }

    function fn_OnRemoveApplication() {
        $('#<%= divLinkApplication.ClientID %>').hide();
        $('#<%= divUpLoadApplication.ClientID %>').show();
        fn_UpdateGridData(null, false, false, false);
        $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("DeleteApplication");
        return false;
    }

        function fn_OnRemoveQuotationFile(sender, args) {
            fn_UpdateGridData(null, false, false, false);
            $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("DeleteQuotation");
        }

        function fn_OnRemoveApplicationFile(sender, args) {
            fn_UpdateGridData(null, false, false, false);
            $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("DeleteApplication");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">

    <telerik:RadAjaxLoadingPanel ID="radLoading" runat="server">
    </telerik:RadAjaxLoadingPanel>

    <div class="doc_style">
        <div id="divRequestToAgency" runat="server" class=" pb10">
            <table style="width: 100%;">
                <colgroup>
                    <col style="width: 400px" />
                    <col />
                </colgroup>
                <tr>
                    <td>
                        <!-- <span style="display: inline-block; width: 35px">Ticket</span> 담당자 : 강인영 차장 phone &nbsp; 02)3700-9840 &nbsp;| mobile 010)4747-9598 | iykang@carlsonwagonlit.kr -->
                        <!-- <span style="display: inline-block; width: 35px">Ticket</span> 담당자 : 임다희 과장 phone &nbsp; 02)3700-9851 &nbsp;| mobile 010)3903-4238 | dhlim@carlsonwagonlit.kr -->
                        <!-- <span style="display: inline-block; width: 35px">Ticket</span> 담당자 : 홍송이 님  &nbsp; &nbsp; &nbsp; phone +82 (0)2 3700- 9851 | bayer@carlsonwagonlit.kr --> 
<%--                        <span style="display: inline-block; width: 35px">Ticket</span> 담당자 : 박주연 님  &nbsp; &nbsp; &nbsp; phone +82 (0)2 3700- 9814 | bayer@carlsonwagonlit.kr--%>
<%--                        <span style="display: inline-block; width: 35px">Ticket</span> 담당자 : 최 웅 과장  &nbsp; &nbsp; &nbsp; phone +82 (0)2 3700- 9832 | bayer@carlsonwagonlit.kr
                        <span style="display: inline-block; width: 35px">Ticket</span> 담당자 : 최 웅 과장  &nbsp; &nbsp; &nbsp; phone +82 (0)2 3700- 9832 | bayer@mycwt.kr--%>
                        
                        <!-- <span style="display: inline-block; width: 35px">VISA</span> 담당자 : 남현모 주임 &nbsp; phone 02)3700-9889 | mobile 010)6230-6201 | fax 02)785-6777 | hmnam@carlsonwagonlit.kr -->
<%--                        <span style="display: inline-block; width: 35px">VISA</span> 담당자 : VISA TEAM &nbsp; phone 070)8233-8234 | fax 02)785-6777 | visa@carlsonwagonlit.kr
                        <span style="display: inline-block; width: 35px">VISA</span> 담당자 : VISA TEAM &nbsp; phone 070)8233-8234 | fax 02)785-6777 | visa@mycwt.kr--%>
                        <p>
                            예약문의
                            <br />
                            <strong>02 3276 2254</strong> (월 - 금 / 09:00 - 18:00)
                            <br />
                            ※ 근무시간 외/비상시(추가비용발생) : 02 399 7777
                            <br />
                            <br />
                            Ticket 담당자
                            <br />
                            김소담 대리
                            <br />
                            <strong>bcd.bayer.kr@bcdtravel.co.kr</strong>
                            <br />
                            <br />
                            Visa 담당자
                            <br />
                            박현우 대리
                            <br />
                            <strong>bcd.bayer.kr@bcdtravel.co.kr</strong>

                        </p>
                    </td>
                    <td class="align_right">
                        <telerik:RadButton ID="radBtnRequestToAgency" runat="server" Text="Request to agency" ButtonType="ToggleButton"
                            EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-green btn-size3 bold"
                            OnClientClicked="fn_OnRequestToAgencyClick" AutoPostBack="false">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnCancel" runat="server" Text="Cancel" Visible="false" AutoPostBack="false"
                            OnClientClicked="fn_CancelClicked"
                            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold">
                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
            <br />
            <table style="width: 100%; border:1px solid #808080">
               <col style="width: 40%" />
                <col style="width: 60%" />
                
                <tr>
                    <td colspan="2" style="border:1px solid #808080;padding:3px">
                       <font style="color:red" > Please note:</font> 직원의 해외출장업무와 관련하여 BCD Korea로 출장관련 업무대행 및 해외출장자 안전관리 업무를 위탁하고 있습니다. </br>
                            이와 관련하여 전달되는 개인정보 및 보관기한은 아래와 같습니다. 이 개인정보들은 아래의 목적 이외에 기타 목적으로 활용되지 않으며, 명시된 보관기한 이후 BCD Korea에 제공된 개인정보는 안전하게 폐기됩니다.
                    </td>
                </tr>
                <tr>
                    <td style="border:1px solid #808080"> &nbsp;Personal Information Included (취급되는 개인정보)

                    </td>
                    <td style="border:1px solid #808080"> &nbsp;성명, 소속회사명, 이메일주소, 핸드폰번호. (BCD Korea 웹사이트상의 추가 수집정보: IPIN)<br />
                        <font style="color:red" >&nbsp;(※ 비자신청을 하는 경우 여권정보)</font>
                    </td>
                </tr>
                <tr>
                    <td style="border:1px solid #808080"> &nbsp;Retention Period/ Destruction Schedule (보관기간/폐기일정)
                    </td>
                    <td style="border:1px solid #808080"> &nbsp;BCD Korea에서는 매월 정산자료정리 이후 폐기(이벤트 종료 후 1달간 보관)<br />
                        <font style="color:red" > &nbsp;※ BCD본사 웹사이트 계정 등록시, 퇴사시점에 폐기함</font>
                    </td>
                </tr>
            </table>
        </div>

        <h3>Traveler Information
            <div class="title_btn">
                <telerik:RadButton ID="radBtnAddEmployee" runat="server" Text="Employee"
                    OnClientClicked="fn_ShowUserPopup" AutoPostBack="false"
                    ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold">
                </telerik:RadButton>
                <telerik:RadButton ID="radBtnAddExternal" runat="server" Text="External user" OnClientClicked="fn_OnExternalClicked" AutoPostBack="false"
                    ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold">
                </telerik:RadButton>
            </div>
        </h3>
        <telerik:RadGrid ID="radGrdEmployee" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Left"
            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" AllowSorting="false" GridLines="None"
            OnItemCommand="radGrdEmployee_ItemCommand">
            <MasterTableView EditMode="Batch">
                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderText="Index" HeaderStyle-Width="40px" UniqueName="IDX" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TRAVELER_TYPE" HeaderText="Type" HeaderStyle-Width="40px" UniqueName="TRAVELER_TYPE" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TRAVELER_ID" HeaderText="ID" HeaderStyle-Width="40px" UniqueName="TRAVELER_ID" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TRAVELER_NAME" HeaderText="Name" HeaderStyle-Width="150px" UniqueName="TRAVELER_NAME" ReadOnly="true"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="SEX" UniqueName="SEX" HeaderText="" HeaderStyle-Width="70px">
                        <ItemTemplate><%# Eval("SEX")%></ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDropDownList ID="radGrdDdlSex" runat="server" Width="60px">
                                <Items>
                                    <telerik:DropDownListItem Text="Mr." Value="Mr." />
                                    <telerik:DropDownListItem Text="Ms." Value="Ms." />
                                </Items>
                            </telerik:RadDropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="EMP_NO" HeaderText="EmpNo." HeaderStyle-Width="100px" UniqueName="EMP_NO" ReadOnly="true"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DIV_DEPT" HeaderText="Div/Dept" HeaderStyle-Width="100%" UniqueName="DIV_DEPT" ReadOnly="true"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="COST_CODE" UniqueName="COST_CODE" HeaderText="Cost Center" HeaderStyle-Width="140px">
                        <ItemTemplate><%# Eval("COST_CODE")%></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radGrdTxtCodeCode" runat="server" Width="100%" ReadOnly></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" OnClientClick='<%# String.Format("return openConfirmPopUp(\"{0}\", {1});", "Employee", Eval("IDX"))%> '
                                ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <telerik:RadGrid ID="radGrdExternal" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Left"
            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" AllowSorting="false" GridLines="None"
            OnItemCommand="radGrdExternal_ItemCommand">
            <MasterTableView EditMode="Batch">
                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />

                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderText="Index" HeaderStyle-Width="40px" UniqueName="IDX" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TRAVELER_TYPE" HeaderText="Type" HeaderStyle-Width="40px" UniqueName="TRAVELER_TYPE" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TRAVELER_ID" HeaderText="ID" HeaderStyle-Width="40px" UniqueName="TRAVELER_ID" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="TRAVELER_NAME" UniqueName="TRAVELER_NAME" HeaderText="Guest" HeaderStyle-Width="150px">
                        <ItemTemplate><%# Eval("TRAVELER_NAME")%></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radGrdTxtTravelerName" runat="server" CssClass="input" Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn DataField="SEX" UniqueName="SEX" HeaderText="" HeaderStyle-Width="70px">
                        <ItemTemplate><%# Eval("SEX")%></ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDropDownList ID="radGrdDdlSex" runat="server" Width="60px">
                                <Items>
                                    <telerik:DropDownListItem Text="Mr." Value="Mr." />
                                    <telerik:DropDownListItem Text="Ms." Value="Ms." />
                                </Items>
                            </telerik:RadDropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="COMPANY_ORG" UniqueName="COMPANY_ORG" HeaderText="Company Org." HeaderStyle-Width="150px">
                        <ItemTemplate><%# Eval("COMPANY_ORG")%></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radGrdTxtCompanyOrg" runat="server" CssClass="input" Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="COST_CODE" UniqueName="COST_CODE" HeaderText="Contact Point" HeaderStyle-Width="150px">
                        <ItemTemplate><%# Eval("COST_CODE")%></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radGrdTxtCodeCode" runat="server" CssClass="input" Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="INTERNAL_ORDER" UniqueName="INTERNAL_ORDER" HeaderText="Cost Center / Internal Order" HeaderStyle-Width="100%">
                        <ItemTemplate><%# Eval("INTERNAL_ORDER")%></ItemTemplate>
                        <EditItemTemplate>

                            <asp:TextBox ID="grdTxtInternalOrder" runat="server" CssClass="input" Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" OnClientClick='<%# String.Format("return openConfirmPopUp(\"{0}\", {1});", "External", Eval("IDX"))%> '
                                ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <font color=red>※ Cost Center 변경은 "Comment To Agency" 에 입력 하셔야 합니다.</font>
        <br />

        <h3>Trip Information</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Purpose <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadDropDownList ID="radDdlPurpose" runat="server" DefaultMessage="---Select---" Width="100%" DropDownWidth="300px">
                            <Items>
                                <telerik:DropDownListItem Text="Internal – Bayer meeting(6821100)" Value="I_BM" />
                                <telerik:DropDownListItem Text="Internal – Training Seminar(6701000)" Value="I_TS" />
                                <telerik:DropDownListItem Text="External – meeting with customer(6821100)" Value="E_MC" />
                                <telerik:DropDownListItem Text="External – meeting with supplier(6821100)" Value="E_MS" />
                                <telerik:DropDownListItem Text="External – conferences, congresses, Fairs(6821100)" Value="E_CCF" />
                                <telerik:DropDownListItem Text="Guest – Traveller(6833200)" Value="G_T" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <th>Period <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadDatePicker ID="radDatFrom" runat="server" Width="100px" Calendar-ShowRowHeaders="false"
                            DateInput-DisplayDateFormat="yyyy-MM-dd" DateInput-DateFormat="yyyy-MM-dd"
                            Culture="ko-KR" MinDate="1900-01-01" MaxDate="2050-12-31">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                </SpecialDays>
                            </Calendar>
                        </telerik:RadDatePicker>
                        ~
                        <telerik:RadDatePicker ID="radDatTo" runat="server" Width="100px" Calendar-ShowRowHeaders="false"
                            DateInput-DisplayDateFormat="yyyy-MM-dd" DateInput-DateFormat="yyyy-MM-dd"
                            Culture="ko-KR" MinDate="1900-01-01" MaxDate="2050-12-31">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                </SpecialDays>
                            </Calendar>
                        </telerik:RadDatePicker>

                    </td>
                </tr>
                <tr style="height: 50px">
                    <th>Detail Information</th>
                    <td>
                        <telerik:RadTextBox ID="radTxtDetailInformation" runat="server" Width="100%" TextMode="MultiLine"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Comment To Agency</th>
                    <td colspan="3">
                        <telerik:RadTextBox ID="radTxtCommentToAgency" runat="server" TextMode="MultiLine" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Traveler's Contact Point <span class="text_red">*</span><br />(Mobile Phone Number)</th>
                    <td colspan="3">
                        <telerik:RadTextBox ID="radTxtContactPoint" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </div>

        <h3>Trip Route <span class="text_red">*</span>
            <div class="title_btn">
                <telerik:RadButton ID="radBtnAddTrip" runat="server" Text="Add" OnClientClicked="fn_OnTripRouteClicked" AutoPostBack="false"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size1 bold">
                </telerik:RadButton>
            </div>
        </h3>
        <telerik:RadGrid ID="radGrdTripRoute" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center"
            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" AllowSorting="false" GridLines="None"
            OnPreRender="radGrdTripRoute_PreRender" OnItemCommand="radGrdTripRoute_ItemCommand">
            <MasterTableView EditMode="Batch" DataKeyNames="IDX">

                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderText="Index" HeaderStyle-Width="40px" UniqueName="IDX" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="DEPARTURE_DATE" HeaderText="Departure Dt." HeaderStyle-Width="110px"
                        UniqueName="DEPARTURE_DATE">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:yyyy-MM-dd}", Eval("DEPARTURE_DATE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDatePicker ID="radGrdDatDeparture" runat="server" DateInput-DisplayDateFormat="yyyy-MM-dd" Culture="ko-KR"
                                DateInput-DateFormat="yyyy-MM-dd" Width="100px" Calendar-ShowRowHeaders="false">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            </telerik:RadDatePicker>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="RETURN_DATE" HeaderText="Return Dt." HeaderStyle-Width="110px"
                        UniqueName="RETURN_DATE">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:yyyy-MM-dd}", Eval("RETURN_DATE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDatePicker ID="radGrdDatReturn" runat="server" DateInput-DisplayDateFormat="yyyy-MM-dd" Culture="ko-KR"
                                DateInput-DateFormat="yyyy-MM-dd" Width="100px" Calendar-ShowRowHeaders="false">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            </telerik:RadDatePicker>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="DEPARTURE_CODE" UniqueName="DEPARTURE_CODE" HeaderText="Departure" HeaderStyle-Width="100%">
                        <ItemTemplate>
                            <%# Eval("DEPARTURE_CODE")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="radGrdTxtDeparture" runat="server" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                            <telerik:RadButton ID="radBtnDepartureCity" runat="server" AutoPostBack="false" OnClientClicked='openDepartureCityPopup'
                                Width="16px" Height="16px" CssClass="btn_grid">
                                <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                            </telerik:RadButton>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="ARRIVAL_CODE" UniqueName="ARRIVAL_CODE" HeaderText="Arrival" HeaderStyle-Width="180px">
                        <ItemTemplate>
                            <%# Eval("ARRIVAL_CODE")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="radGrdTxtApproval" runat="server" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                            <telerik:RadButton ID="radBtnArrivalCity" runat="server" AutoPostBack="false" OnClientClicked='openArrivalCityPopup'
                                Width="16px" Height="16px" CssClass="btn_grid">
                                <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                            </telerik:RadButton>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="TRIP_TYPE" UniqueName="TRIP_TYPE" HeaderText="Trip Type" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <%# Eval("TRIP_TYPE_NAME")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDropDownList ID="radGrdDdlTripType" runat="server" Width="90px" DefaultMessage="---Select---"
                                OnClientSelectedIndexChanged="fn_OnTrypTypeIndexChanged">
                            </telerik:RadDropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="AIRPLANE_CLASS" UniqueName="AIRPLANE_CLASS" HeaderText="Class" HeaderStyle-Width="130px">
                        <ItemTemplate><%# Eval("AIRPLANE_CLASS_NAME")%></ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDropDownList ID="radGrdDdlClass" runat="server" Width="120px" DefaultMessage="---Select----">
                            </telerik:RadDropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" OnClientClick='<%# String.Format("return openConfirmPopUp(\"{0}\", {1});", "TripRoute", Eval("IDX"))%> '
                                ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>

        </telerik:RadGrid>

        <div id="divAfterSendMail" runat="server" style="margin: 0 0 0 0">
            <h3>Quotation</h3>
            <h6 style="border-left:1px solid #81c23d;border-right:1px solid #81c23d">본 문서의 승인완료와 함께 항공권이 발권되며. 변경 및 취소의 경우 발권취소 수수료가 발생하므로, 승인요청 및 승인에 신중을 바랍니다.
또한  발권 완료된 항공권의 변경 및 취소 경우는, TMS상에서 기존문서 cancel 후 새로운 승인요청문서를 작성해야 합니다
</h6>
            <div class="data_type1">
                <table>
                    <colgroup>
                        <col style="width: 25%;" />
                        <col />
                    </colgroup>
                    <tr>
                        <th>Quotaion No.</th>
                        <td>
                            <telerik:RadDropDownList ID="radDdlQuotationNo" runat="server" OnClientItemSelected="fn_OnQuotationIndexChanged" AutoPostBack="false">
                                <Items>
                                    <telerik:DropDownListItem Text="1" Value="1" />
                                    <telerik:DropDownListItem Text="2" Value="2" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr style="display: none" quotation="2">
                        <th>Reason Codes</th>
                        <td>
                            <telerik:RadDropDownList ID="radDdlReason" runat="server" Width="100%" DefaultMessage="---Select---" DropDownWidth="500px">
                                <Items>
                                    <%--                                    <telerik:DropDownListItem Text="Declined due to carrier / passenger requested specific airline" Value="0001" />
                                    <telerik:DropDownListItem Text="Declined due to time preference / passenger requested specific schedule or date" Value="0002" />
                                    <telerik:DropDownListItem Text="Declined due to penalty or restriction / passenger declined restricted fare" Value="0003" />
                                    <telerik:DropDownListItem Text="Authorized exception to travel policy / passenger authorized to travel policy" Value="0004" />
                                    <telerik:DropDownListItem Text="Declined due to airport preferences" Value="0005" />--%>
                                </Items>
                            </telerik:RadDropDownList>
                        </td>

                    </tr>
                    <tr style="display: none" quotation="2">
                        <th>Detail Description</th>
                        <td>
                            <telerik:RadTextBox ID="radTxtDetailDesc" runat="server" TextMode="MultiLine" Width="100%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Quotaion</th>
                        <td>

                            <div id="divLinkQuotation" runat="server" style="display: inline-block">
                                <a href="#" id="hrefQuotationFile" runat="server"></a>
                                <asp:ImageButton Style="display: inline-block;" ID="btnRemoveQuotation" runat="server" OnClientClick="return fn_OnRemoveQuotation()"
                                    ImageUrl="~/Styles/images/icon_remove.png" BorderStyle="None" />
                            </div>
                            <div id="divUpLoadQuotation" runat="server" style="display: inline-block">
                                <telerik:RadAsyncUpload ID="radUpLoadQuotation" runat="server"
                                    OnClientFilesUploaded="fn_ClientQuotationFilesUploaded" OnFileUploaded="radUpLoadQuotation_FileUploaded" OnClientFileUploadRemoved="fn_OnRemoveQuotationFile"
                                    AutoAddFileInputs="false" Localization-Select="파일선택" MultipleFileSelection="Disabled" Width="100%" />
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <th>VISA Application</th>
                        <td>
                            <div id="divLinkApplication" runat="server" style="display: inline-block">
                                <a href="#" id="hrefApplicationFile" runat="server"></a>
                                <asp:ImageButton Style="display: inline-block;" ID="btnRemoveApplication" runat="server" OnClientClick="return fn_OnRemoveApplication()"
                                    ImageUrl="~/Styles/images/icon_remove.png" BorderStyle="None" />
                            </div>
                            <div id="divUpLoadApplication" runat="server" style="display: inline-block">
                                <telerik:RadAsyncUpload ID="radUpLoadApplication" runat="server"
                                    OnClientFilesUploaded="fn_ClientApplicationFilesUploaded" OnFileUploaded="radUpLoadApplication_FileUploaded" OnClientFileUploadRemoved="fn_OnRemoveApplicationFile"
                                    AutoAddFileInputs="false" Localization-Select="파일선택" MultipleFileSelection="Disabled" Width="100%" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

            <h3>Estimation Details
                <div class="title_btn">
                    <%--                    <telerik:RadNumericTextBox ID="radNumExtimationTotal" runat="server" Width="200px" ReadOnly="true" BackColor="Transparent"
                        BorderWidth="0" Font-Bold="true" Font-Size="15px" NumberFormat-DecimalDigits="0" Value="0"
                        ReadOnlyStyle-HorizontalAlign="Right" BorderStyle="Outset">
                    </telerik:RadNumericTextBox>--%>
                    <asp:Label ID="radNumExtimationTotal" runat="server"
                        Style="text-align: right; font-size: 18px; font-weight: bold; color: red; background-color: transparent; border: none"></asp:Label>
                </div>
            </h3>
            <div class="data_type1">
                <table>
                    <colgroup>
                        <col style="width: 25%;" />
                        <col style="width: 25%;" />
                        <col />
                    </colgroup>
                    <tr>
                        <th rowspan="2">Airfare</th>
                        <th>Amount(KRW)</th>
                        <th>Comments, if needed.</th>
                    </tr>
                    <tr>
                        <td>
                            <%--<telerik:RadNumericTextBox ID="radNumAmount" runat="server" Width="100%" NumberFormat-DecimalDigits="0"
                                EnabledStyle-HorizontalAlign="Right" Value="0" ClientEvents-OnBlur="fn_OnAirfareBlur">
                            </telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="radNumAmount" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeypress="return fn_OnGridKeyPress(this, event)"
                                onkeyup="return fn_OnAmountKeyUp(this)"
                                DecimalDigits="0" AllowNegative="false">                                
                            </asp:TextBox>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="radTxtCommentsIfNeeded" runat="server" Width="100%"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>

            <h3>
                <div class="title_btn">
                    <telerik:RadButton ID="radBtnAddAccommodation" runat="server" Text="Add"
                        EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" ButtonType="LinkButton"
                        CssClass="btn btn-blue btn-size1 bold"
                        OnClientClicked="fn_OnAccommodationClicked" AutoPostBack="false">
                    </telerik:RadButton>
                </div>
            </h3>

            <telerik:RadGrid ID="radGrdEstimationDetails" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center"
                EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" AllowSorting="false" GridLines="None" ShowFooter="true"
                OnPreRender="radGrdEstimationDetails_PreRender" OnItemCommand="radGrdEstimationDetails_ItemCommand">
                <HeaderStyle CssClass="grid_header" />
                <MasterTableView EditMode="Batch" DataKeyNames="IDX">
                    <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="IDX" HeaderText="Index" HeaderStyle-Width="40px" UniqueName="IDX" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="ACCOMMODATION_CODE" UniqueName="ACCOMMODATION_CODE"
                            HeaderText="Accommodation" HeaderStyle-Width="50%">
                            <ItemTemplate>
                                <%# Eval("ACCOMMODATION_NAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="radGrdDdlAccommodation" runat="server" Width="95%" DefaultMessage="---Select---" OnClientSelectedIndexChanged="fn_OnAccoSelectedChanged">
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="FROM_DATE" HeaderText="From" HeaderStyle-Width="110px"
                            UniqueName="FROM_DATE">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:yyyy-MM-dd}", Eval("FROM_DATE")) %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDatePicker ID="radGrdDatFrom" runat="server" Culture="ko-KR" DateInput-DisplayDateFormat="yyyy-MM-dd"
                                    DateInput-DateFormat="yyyy-MM-dd" Width="100px"
                                    Calendar-ShowRowHeaders="false">
                                    <ClientEvents OnDateSelected="fn_OnGridDateBlur" />
                                    <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                </telerik:RadDatePicker>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="TO_DATE" HeaderText="To" HeaderStyle-Width="110px"
                            UniqueName="TO_DATE">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:yyyy-MM-dd}", Eval("TO_DATE")) %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDatePicker ID="radGrdDatTo" runat="server" Culture="ko-KR" DateInput-DisplayDateFormat="yyyy-MM-dd"
                                    DateInput-DateFormat="yyyy-MM-dd" Width="100px" Calendar-ShowRowHeaders="false">
                                    <ClientEvents OnDateSelected="fn_OnGridDateBlur" />
                                    <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                </telerik:RadDatePicker>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="AMOUNT_PER_NIGHT" UniqueName="AMOUNT_PER_NIGHT" HeaderText="Amount(KRW) <br/> Per Night"
                            HeaderStyle-Width="110px"
                            DataType="System.Decimal">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("AMOUNT_PER_NIGHT")) %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--                                <telerik:RadNumericTextBox runat="server" ID="radGrdNumPrice" NumberFormat-DecimalDigits="0"
                                    MinValue="0" MaxValue="99999999999"
                                    Width="100%" EnabledStyle-HorizontalAlign="Right" ClientEvents-OnBlur="fn_OnGridNumBlur">
                                </telerik:RadNumericTextBox>--%>
                                <asp:TextBox ID="radGrdNumPrice" runat="server" Width="100%" CssClass="input align_right"
                                    onblur="return fn_OnGridNumBlur(this)"
                                    onfocus="return fn_OnGridNumFocus(this)"
                                    onkeypress="return fn_OnGridKeyPress(this, event)"
                                    onkeyup="return fn_OnGridKeyUp(this)"
                                    DecimalDigits="0" AllowNegative="false">                                
                                </asp:TextBox>

                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridNumericColumn DataField="AMOUNT_TOTAL" UniqueName="AMOUNT_TOTAL" HeaderText="Amount(KRW) <br/> Total" HeaderStyle-Width="110px" ReadOnly="true"
                            Aggregate="Sum" DataType="System.Decimal" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}"
                            FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                            <ItemStyle HorizontalAlign="Right" />
                        </telerik:GridNumericColumn>
                        <telerik:GridTemplateColumn DataField="REASON_HOTEL" HeaderText="Reason/Hotel" HeaderStyle-Width="50%"
                            UniqueName="REASON_HOTEL">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("REASON_HOTEL") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radGrdTxtReason" runat="server" CssClass="input" Width="100%"></asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="40px">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" OnClientClick='<%# String.Format("return openConfirmPopUp(\"{0}\", {1});", "Accommodation", Eval("IDX"))%> '
                                    ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>

            </telerik:RadGrid>
        </div>
    </div>
    <%--ADD POPUP--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" OnClientClose="OnClientClose">
        <Windows>
            <telerik:RadWindow ID="winPopupUser" runat="server" NavigateUrl="/eWorks/Common/Popup/PopupUserList.aspx" Title="User List" Modal="true" Width="500" Height="600" VisibleStatusbar="false" Skin="Metro">
            </telerik:RadWindow>
            <telerik:RadWindow ID="winPopupCity" runat="server" NavigateUrl="/eWorks/Common/Popup/CountryCity.aspx" Title="User List" Modal="true" Width="450" Height="250" VisibleStatusbar="false" Skin="Metro">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <input type="hidden" id="hddIsAfterSendMail" runat="server" value="N" />
    <input type="hidden" id="hddEmployeeGridItems" runat="server" />
    <input type="hidden" id="hddExternalGridItems" runat="server" />
    <input type="hidden" id="hddTripRouteGridItems" runat="server" />
    <input type="hidden" id="hddAccommodationGridItems" runat="server" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="HddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input id="hddUploadFolder" type="hidden" runat="server" />
    <input id="hddQuotationAttachFiles" type="hidden" runat="server" />
    <input id="hddApplicationAttachFiles" type="hidden" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
    <input type="hidden" id="hddUserId" runat="server" value="" />
</asp:Content>

