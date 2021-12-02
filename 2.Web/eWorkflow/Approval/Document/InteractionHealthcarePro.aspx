<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="InteractionHealthcarePro.aspx.cs" Inherits="Approval_Document_InteractionHealthcarePro" %>



<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">

        function pageLoad() {
            var addRow = $('#<%= hddAddRow.ClientID %>').val();            
            if (addRow == 'Y') {
                var grid = $find('<%=radGrdCostDetail.ClientID%>');
                if (grid.get_masterTableView()) {
                    var dataItems = grid.get_masterTableView().get_dataItems();
                    if (dataItems.length > 0) {
                        grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                    }
                }
                $('#<%= hddAddRow.ClientID %>').val('Y');
            }
        }
        function fn_UpdateGridDataFromUploader() {
            fn_UpdateGridData(false);
        }

        function fn_DoRequest(sender, arsg) {
            return fn_UpdateGridData(false);
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData(false);
        }

        function fn_OnActivityChanged(sender, args) {
            var index = args.get_index();
            var ddlCategory = $find('<%= radDdlActivity.ClientID %>');
            var selectedValue = ddlCategory.get_items().getItem(index).get_value();
            setVisibleControl(selectedValue);
            var items = $find('<%=radDdlCategory.ClientID  %>').get_items();
            for (var i = 0 ; i < items.get_count() ; i++) {
                var element = items.getItem(i).get_element();
                if (selectedValue == 'PS') {
                    if (items.getItem(i).get_value() == '0005') {
                        items.getItem(i).set_selected(true);
                        element.style.display = "";
                    }
                    else { element.style.display = "none"; }
                }
                else {
                    element.style.display = "";
                }
            }
        }

        function setVisibleControl(value) {
            if (value == 'PPM') {
                $('#divClinicalStudy').hide();
                $('#rowReason').show();
                $('#rowPurpose').show();
                $('#divPPM_CM_EM').show();
                $('#divCS').hide();
                $('#divAccommodationDesc').show();
                $('#divPurposeObjective').show();
                $('#divPS').hide();
                $('#divPSWH').hide();
            } else if (value == 'CS') {
                $('#divClinicalStudy').show();
                $('#rowReason').hide();
                $('#rowPurpose').hide();
                $('#divPPM_CM_EM').hide();
                $('#divCS').show();
                $('#divAccommodationDesc').show();
                $('#divPurposeObjective').hide();
                $('#divPS').hide();
                $('#divPSWH').hide();
            } else if (value == 'PSH') {
                $('#rowReason').show();
                $('#rowPurpose').show();
                $('#divPPM_CM_EM').hide();
                $('#divCS').hide();
                $('#divAccommodationDesc').show();
                $('#divPurposeObjective').show();
                $('#divPS').hide();
                $('#divPSWH').show();
            }
            else if (value == 'PS') {
                $('#rowReason').show();
                $('#rowPurpose').show();
                $('#divPPM_CM_EM').hide();
                $('#divCS').hide();
                $('#divAccommodationDesc').show();
                $('#divPurposeObjective').show();
                $('#divPS').show();
                $('#divPSWH').hide();
            } else if (value == 'CM' || value == 'ABM') {
                $('#divClinicalStudy').hide();
                $('#rowReason').hide();
                $('#rowPurpose').show();
                $('#divPPM_CM_EM').show();
                $('#divCS').hide();
                $('#divAccommodationDesc').hide();
                $('#divPurposeObjective').show();
                $('#divPS').hide();
                $('#divPSWH').hide();
            } else {
                $('#divClinicalStudy').hide();
                $('#rowReason').hide();
                $('#rowPurpose').show();
                $('#divPPM_CM_EM').show();
                $('#divCS').hide();
                $('#divAccommodationDesc').hide();
                $('#divPurposeObjective').show();
                $('#divPS').hide();
                $('#divPSWH').hide();
            }
        }

        function checkLastRow() {
            var masterTable = $find('<%= radGrdCostDetail.ClientID %>').get_masterTableView();
            $find('<%= radGrdCostDetail.ClientID %>').get_batchEditingManager().saveChanges(masterTable);

            var dataItems = masterTable.get_dataItems();
            if (dataItems.length > 0) {
                var lastItem = dataItems[dataItems.length - 1];
                var qty = lastItem.get_cell("QTY").innerText;
                var price = lastItem.get_cell("PRICE").innerText;
                if (qty.length < 1 || price.length < 1) {
                    fn_OpenDocInformation('자료를 입력바랍니다.');
                    return false;
                }
            }
            return true;
        }
        function fn_UpdateGridData(addRow) {
            var list = [];
            var masterTable = $find('<%= radGrdCostDetail.ClientID %>').get_masterTableView();
            if ($find('<%= radGrdCostDetail.ClientID %>').get_batchEditingManager())
                $find('<%= radGrdCostDetail.ClientID %>').get_batchEditingManager().saveChanges(masterTable);

            var dataItems = masterTable.get_dataItems();

            var total = 0;
            var maxIdx = 0;
            for (var i = 0; i < dataItems.length; i++) {

                var idx = dataItems[i].get_cell("IDX").innerText;
                var categoryCode = dataItems[i].get_cell("CATEGORY_CODE").innerText;
                var categoryName = dataItems[i].get_cell("CATEGORY_NAME").children[0].innerText;

                if($.trim(categoryName) == "Accommodation") categoryCode = "0001";
                else if ($.trim(categoryName) == "Agency Cost") categoryCode = "0002";
                else if ($.trim(categoryName) == "Banquet room & Venue Sytem") categoryCode = "0003";
                else if ($.trim(categoryName) == "Gimmick / Souvenir") categoryCode = "0004";
                else if ($.trim(categoryName) == "Meal & Beverage") categoryCode = "0005";
                else if ($.trim(categoryName) == "Prints") categoryCode = "0006";
                else if ($.trim(categoryName) == "Transportation") categoryCode = "0007";
                else if ($.trim(categoryName) == "Honorarium") categoryCode = "0008";

                var description = dataItems[i].get_cell("DESCRIPTION").children[0].innerText;
                var qty = dataItems[i].get_cell("QTY").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');;
                var price = dataItems[i].get_cell("PRICE").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');;
                //var amount = dataItems[i].get_cell("AMOUNT").firstElementChild.innerText;
                var comment = dataItems[i].get_cell("COMMENT").children[0].innerText;

                var conObj = {
                    IDX: null,
                    CATEGORY_CODE: null,
                    CATEGORY_NAME: null,
                    DESCRIPTION: null,
                    QTY: null,
                    PRICE: null,
                    AMOUNT: null,
                    COMMENT: null,
                }
                conObj.IDX = idx;
                conObj.CATEGORY_CODE = categoryCode;
                conObj.CATEGORY_NAME = categoryName;
                conObj.DESCRIPTION = description;
                conObj.QTY = qty;
                conObj.PRICE = price;
                conObj.AMOUNT = parseFloat(qty) * parseFloat(price);
                conObj.COMMENT = comment;

                list.push(conObj);
                maxIdx = idx;
            }

            if (addRow) {
                var categoryValue = $find('<%= radDdlCategory.ClientID %>').get_selectedItem().get_value();
                var categoryText = $find('<%= radDdlCategory.ClientID %>').get_selectedItem().get_text();
                var conObj = {
                    IDX: null,
                    CATEGORY_CODE: null,
                    CATEGORY_NAME: null,
                    DESCRIPTION: null,
                    QTY: null,
                    PRICE: null,
                    AMOUNT: null,
                    COMMENT: null,
                }
                conObj.IDX = ++maxIdx;
                conObj.CATEGORY_CODE = categoryValue;
                conObj.CATEGORY_NAME = categoryText;
                conObj.DESCRIPTION = '';
                conObj.QTY = 0;
                conObj.PRICE = 0;
                conObj.AMOUNT = 0;
                conObj.COMMENT = '';

                list.push(conObj);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

            return true;
        }

        function fn_OnAddCategoryClicked(sender, args) {
            if (checkLastRow()) {
                fn_UpdateGridData(true);
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind");
            }
        }

        function openConfirmPopUp(index) {
            clickedKey = parseInt(index);

            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);

            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                fn_UpdateGridData(false);
                var masterTable = $find('<%= radGrdCostDetail.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        function fn_OnAttendeeKeyUp(sender) {
            var noGo = $('#<%= radNumGO.ClientID %>').val();
            noGo = noGo.replace(/,/gi, '').replace(/ /gi, '');
            if (!(!isNaN(parseFloat(noGo)) && isFinite(noGo))) noGo = 0;
            var noNonGo = $('#<%= radNumNonGO.ClientID %>').val();
            noNoGo = noNonGo.replace(/,/gi, '').replace(/ /gi, '');
            if (!(!isNaN(parseFloat(noNoGo)) && isFinite(noNoGo))) noNonGo = 0;
            var noPrivate = $('#<%= radNumPrivate.ClientID %>').val();
            noPrivate = noPrivate.replace(/,/gi, '').replace(/ /gi, '');
            if (!(!isNaN(parseFloat(noPrivate)) && isFinite(noPrivate))) noPrivate = 0;

            var noForeignHCP = $('#<%= radNumForeignHCP.ClientID %>').val();
            noForeignHCP = noForeignHCP.replace(/,/gi, '').replace(/ /gi, '');
            if (!(!isNaN(parseFloat(noForeignHCP)) && isFinite(noForeignHCP))) noForeignHCP = 0;

            var noBayer = $('#<%= radNumBayer.ClientID %>').val();
            noBayer = noBayer.replace(/,/gi, '').replace(/ /gi, '');
            if (!(!isNaN(parseFloat(noBayer)) && isFinite(noBayer))) noBayer = 0;

            var total = parseInt(noGo) + parseInt(noNonGo) + parseInt(noPrivate) + parseInt(noForeignHCP) + parseInt(noBayer);

            $('#<%= lblTotalParticipants.ClientID %>').html(total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        }

        function openGridRowForEdit(sender, args) {
           <%-- var grid = $find('<%=radGrdCostDetail.ClientID%>');
            if (grid.get_masterTableView()) {
                var dataItems = grid.get_masterTableView().get_dataItems();
                if (dataItems.length > 0) {
                    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                }
            }--%>
        }

        function SetTotal() {
            var masterTable = $find('<%= radGrdCostDetail.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var total = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var amount = dataItems[i].get_cell("AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');

                amount = parseInt(amount);

                total += amount;
            }

            masterTable.get_element().tFoot.rows[0].cells[6].innerText = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function fn_OnGridNumBlur(sender) {
            setNumberFormat(sender);
        }
        function fn_OnGridKeyUp(sender) {
            var row = $find('<%= radGrdCostDetail.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            var dataItem = $find(row.id);
            
            if (dataItem) {
                var qty = 0, price = 0;
                var ctlQty = dataItem.findElement('radGrdNumQty');
                if (ctlQty) qty = parseFloat(ctlQty.value.replace(/,/gi, '').replace(/ /gi, ''));
                var ctlPrice = dataItem.findElement('radGrdNumPrice');
                if (ctlPrice) price = parseFloat(ctlPrice.value.replace(/,/gi, '').replace(/ /gi, ''));
                if (!qty) qty = 0;
                if (!price) price = 0;

                dataItem.get_cell('AMOUNT').innerText = (qty * price).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
            SetTotal();
        }

        function fn_OnCategorySelectedChanged(sender, args) {

            var row = $find('<%= radGrdCostDetail.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            if (row) {
                var dataItem = $find(row.id);
                if (dataItem) {
                    dataItem.get_cell('CATEGORY_CODE').innerText = args.get_item().get_value();
                }
            }
        }
        function fn_OnCountryRequesting(sender, args) {
            var context = args.get_context();
            context["company"] = $('#<%= hddCompanyCode.ClientID %>').val();

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <div class="doc_style">
        <div id="divYourDoces" class="align_right pb10" style="display: none;" runat="server">
                <telerik:RadButton runat="server" ID="RadbtnYourDoces" Text="YourDocs" AutoPostBack="false" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" NavigateUrl="http://sp-coll-bbs.ap.bayer.cnb/sites/000141/Teamsites/NEW%20e-workflow/LINKED_DATA/Medical%20Event%20Cover-Letter.xlsx" Target="_blank"></telerik:RadButton>
            </div>
        <h3>Event Information</h3>
        <div class="data_type1">

            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Subject <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radTxtSubject" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Activity</th>
                    <td>
                        <telerik:RadDropDownList ID="radDdlActivity" runat="server" Width="100%" DropDownWidth="650px"
                            DefaultMessage="---Select---"
                            OnClientSelectedIndexChanged="fn_OnActivityChanged">
                            <Items>
                                <telerik:DropDownListItem Text="Product Presentation Meeting" Value="PPM" />
                                <telerik:DropDownListItem Text="Consulting Meeting (MKT)" Value="CM" />
                                <telerik:DropDownListItem Text="Advisory Board Meeting (Medical)" Value="ABM" />
                                <telerik:DropDownListItem Text="Employee Medical Training" Value="EMT" />
                                <telerik:DropDownListItem Text="Clinical study related meeting" Value="CS" />
                                <telerik:DropDownListItem Text="Product Seminar with honorarium" Value="PSH" />
                                <telerik:DropDownListItem Text="Product Seminar" Value="PS" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <th>Event Date <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadDatePicker ID="radDatFromEvent" runat="server" Width="100px" Calendar-ShowRowHeaders="false" MinDate="1900-01-01" MaxDate="2050-12-31" Culture="ko-KR">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                </SpecialDays>
                            </Calendar>
                        </telerik:RadDatePicker>
                        <telerik:RadTimePicker ID="radTimFromEvent" runat="server" Width="100px"></telerik:RadTimePicker>
                        ~
                        <telerik:RadDatePicker ID="radDatToEvent" runat="server" Width="100px" Calendar-ShowRowHeaders="false" MinDate="1900-01-01" MaxDate="2050-12-31" Culture="ko-KR">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                             <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                </SpecialDays>
                             </Calendar>
                       </telerik:RadDatePicker>
                       <telerik:RadTimePicker ID="radTimToEvent" runat="server" Width="100px"></telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <th>Meeting Venue <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radTxtMeetingVenue" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Address of Venue <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radTxtAddressOfVenue" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Venue Selection Reason</th>
                    <td>
                        <telerik:RadTextBox ID="radTxtVenueSelectionReason" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="rowReason" style="display: none">

                    <td style="text-align: left" colspan="2">
                        <div id="divReason" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radChkReason1" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" Checked="false"
                                Text="제품설명회에 적합한 비즈니스 장소임" Value="0001" AutoPostBack="false">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkReason2" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" Checked="false"
                                Text="대다수 참석자의 근무지역 (시,군)" Value="0002" AutoPostBack="false">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkReason3" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" Checked="false"
                                Text="온천,해수욕장,골프,카지노,스키,워터파크가 없는 곳" Value="0003" AutoPostBack="false">
                            </telerik:RadButton>
                            <br />
                            <font color="red">*&nbsp위 3가지를 모두 충족하지 않는경우 반드시 증빙첨부</font>
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <div id="divClinicalStudy" class="data_type1" style="display: none">
            <table id="tblClinicalStudy">
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>

                <tr>
                    <th>Compound/Study Number <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radTxtCompound" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Sponsor of Study acc. to protocol <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radTxtSponsor" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>If outsourced, name of CRO <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radTxtCROName" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Objective <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radTxtObjective" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <h3>This Meeting is</h3>
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Investigator Meeting <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadButton ID="radRdoInvestigatorYes" runat="server" Text="Yes" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Investigator" AutoPostBack="false"></telerik:RadButton>
                        <telerik:RadButton ID="radRdoInvestigatorNo" runat="server" Text="No" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Investigator" AutoPostBack="false"></telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <th>On-site monitoring meeting <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadButton ID="radRdoMonitoringYes" runat="server" Text="Yes" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Monitoring" AutoPostBack="false"></telerik:RadButton>
                        <telerik:RadButton ID="radRdoMonitoringNo" runat="server" Text="No" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Monitoring" AutoPostBack="false"></telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <th>Other, please specify <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radTxtOtherspecify" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Contract <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadButton ID="radRdoContractYes" runat="server" Text="Yes" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Contract" AutoPostBack="false"></telerik:RadButton>
                        <telerik:RadButton ID="radRdoContractNo" runat="server" Text="No" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Contract" AutoPostBack="false"></telerik:RadButton>
                    </td>
                </tr>
            </table>
        </div>
        <div class="data_type1" id="divPurposeObjective">
            <h3>Purpose / Objective</h3>
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr id="rowPurpose">
                    <th>Purpose/Object <span class="text_red">*</span></th>
                    <td>
                        <div id="divPurpose" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radChkPurpose1" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="To provide general scientific & medical information related to product" Value="0001">
                            </telerik:RadButton>
                            <br />
                            <telerik:RadButton ID="radChkPurpose2" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="To deliver safety related information including major adverse effects" Value="0002">
                            </telerik:RadButton>
                            <br />
                            <telerik:RadButton ID="radChkPurpose3" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="To provide product’s insurance review standards of the Health Insurance Review & Assessment Service" Value="0003">
                            </telerik:RadButton>
                            <br />
                            <telerik:RadButton ID="radChkPurpose4" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="To provide up-to-date clinical trial information related to product" Value="0004">
                            </telerik:RadButton>
                            <br />
                            <telerik:RadButton ID="radChkPurpose5" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="To update the current issues of product related disease treatment & introduce related clinical journal" Value="0005">
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr id="rowScientific">
                    <th>Scientific Material</th>
                    <td>
                        <telerik:RadTextBox ID="radTxtScientificMaterial" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </div>

        <div class="data_type1">
            <h3>Attendee Information</h3>
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Target Participants
                        <br />
                        (예:서울 지역 내과 개원)</th>
                    <td>
                        <telerik:RadTextBox ID="radTxtTargetParticipants" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>GO(공무원)</th>
                    <td>
                        <%--<telerik:RadNumericTextBox ID="radNumGO" runat="server" NumberFormat-DecimalDigits="0"
                            MinValue="0" MaxValue="999999999" EnabledStyle-HorizontalAlign="Right" Value="0"
                            ClientEvents-OnBlur="fn_OnAttendeeBlur">
                        </telerik:RadNumericTextBox>--%>
                        <asp:TextBox ID="radNumGO" runat="server" Width="70px" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)"
                            onfocus="return fn_OnGridNumFocus(this)"
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="return fn_OnAttendeeKeyUp(this)"
                            DecimalDigits="0" Text="0">                                
                        </asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <th>Non-GO, Public(공직자)</th>
                    <td>
                        <%--                        <telerik:RadNumericTextBox ID="radNumNonGO" runat="server" NumberFormat-DecimalDigits="0"
                            MinValue="0" MaxValue="999999999" EnabledStyle-HorizontalAlign="Right" Value="0"
                            ClientEvents-OnBlur="fn_OnAttendeeBlur">
                        </telerik:RadNumericTextBox>--%>
                        <asp:TextBox ID="radNumNonGO" runat="server" Width="70px" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)"
                            onfocus="return fn_OnGridNumFocus(this)"
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="return fn_OnAttendeeKeyUp(this)"
                            DecimalDigits="0" Text="0">                                
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>Private</th>
                    <td>
                        <%--<telerik:RadNumericTextBox ID="radNumPrivate" runat="server" NumberFormat-DecimalDigits="0"
                            MinValue="0" MaxValue="999999999" EnabledStyle-HorizontalAlign="Right" Value="0"
                            ClientEvents-OnBlur="fn_OnAttendeeBlur">
                        </telerik:RadNumericTextBox>--%>
                        <asp:TextBox ID="radNumPrivate" runat="server" Width="70px" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)"
                            onfocus="return fn_OnGridNumFocus(this)"
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="return fn_OnAttendeeKeyUp(this)"
                            DecimalDigits="0" Text="0">                                
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th rowspan="2">Foreign HCP</th>
                    <td >
                        <asp:TextBox ID="radNumForeignHCP" runat="server" Width="70px" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)"
                            onfocus="return fn_OnGridNumFocus(this)"
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="return fn_OnAttendeeKeyUp(this)"
                            DecimalDigits="0" Text="0">                                
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadAutoCompleteBox ID="radAcomCountry" runat="server" AllowCustomEntry="false" Width="100%" DropDownWidth="300px"
                            OnClientRequesting="fn_OnCountryRequesting">
                            <WebServiceSettings Method="SearchCountry" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                        </telerik:RadAutoCompleteBox>
                        <asp:Label ID="lblNotCountry" runat="server" Width="100%" Visible="false">Refer to the country list attached.</asp:Label>
                    </td>
                </tr>

                <tr>
                    <th>Bayer</th>
                    <td>
                        <%--<telerik:RadNumericTextBox ID="radNumBayer" runat="server" NumberFormat-DecimalDigits="0"
                            MinValue="0" MaxValue="999999999" EnabledStyle-HorizontalAlign="Right" Value="0"
                            ClientEvents-OnBlur="fn_OnAttendeeBlur">
                        </telerik:RadNumericTextBox>--%>
                        <asp:TextBox ID="radNumBayer" runat="server" Width="70px" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)"
                            onfocus="return fn_OnGridNumFocus(this)"
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="return fn_OnAttendeeKeyUp(this)"
                            DecimalDigits="0" Text="0">                                
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>Total Participants</th>
                    <td>
                        <asp:Label ID="lblTotalParticipants" runat="server" Text="0" Width="70px" CssClass="align_right" Style="font-weight: bold;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>Accommodation Needed?</th>
                    <td>
                        <telerik:RadButton ID="radRdoNeededYes" runat="server" Text="Yes" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Accommodation" AutoPostBack="false"></telerik:RadButton>
                        <telerik:RadButton ID="radRdoNeededNo" runat="server" Text="No" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Accommodation" AutoPostBack="false"></telerik:RadButton>
                        <br />
                        <div id="divAccommodationDesc">
                            *&nbsp;숙박이 필요한 경우, KRPIA 에 60일 이전 사전 신고 요망<br />
                            *&nbsp;숙박이 필요하지 않은 경우, KRPIA에  <font color="red"> 7일</font> 이전 사전 신고 요망 (협회 규정 변경:기존 30일에서 7일로 변경되었습니다.)
                        </div>

                    </td>
                </tr>
            </table>

        </div>

        <h3>Cost Detail
            <div class="title_btn">
                <telerik:RadDropDownList ID="radDdlCategory" runat="server" Width="200px">
                    <Items>
                        <telerik:DropDownListItem Text="Accommodation" Value="0001" />
                        <telerik:DropDownListItem Text="Agency Cost" Value="0002" />
                        <telerik:DropDownListItem Text="Banquet room & Venue Sytem" Value="0003" />
                        <telerik:DropDownListItem Text="Gimmick / Souvenir" Value="0004" />
                        <telerik:DropDownListItem Text="Meal & Beverage" Value="0005" />
                        <telerik:DropDownListItem Text="Prints" Value="0006" />
                        <telerik:DropDownListItem Text="Transportation" Value="0007" />
                        <telerik:DropDownListItem Text="Honorarium" Value="0008" />
                    </Items>
                </telerik:RadDropDownList>
                <telerik:RadButton ID="radBtnAddCategory" runat="server" AutoPostBack="false"
                    OnClientClicked="fn_OnAddCategoryClicked" Text="Add"
                    EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton"
                    CssClass="btn btn-blue btn-size1 bold">
                </telerik:RadButton>
            </div>
        </h3>
        <telerik:RadGrid ID="radGrdCostDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" ShowFooter="true"
            OnItemCommand="radGrdCostDetail_ItemCommand" OnItemDataBound="radGrdCostDetail_ItemDataBound" Skin="EXGrid">
            <MasterTableView EditMode="Batch" ClientDataKeyNames="IDX">
                <HeaderStyle HorizontalAlign="Left" />
                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderText="" HeaderStyle-Width="20px" UniqueName="IDX" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CATEGORY_CODE" HeaderText="Category" HeaderStyle-Width="40px" UniqueName="CATEGORY_CODE" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <%--<telerik:GridBoundColumn DataField="CATEGORY_NAME" HeaderText="Category" HeaderStyle-Width="120px" UniqueName="CATEGORY_NAME" ReadOnly="true"></telerik:GridBoundColumn>--%>
                    <telerik:GridTemplateColumn DataField="CATEGORY_NAME" UniqueName="CATEGORY_NAME"
                        HeaderText="Category" HeaderStyle-Width="160px">
                        <ItemTemplate>
                            <%# Eval("CATEGORY_NAME")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDropDownList ID="radGrdDdlCategory" runat="server" Width="95%" OnClientItemSelected="fn_OnCategorySelectedChanged">
                                <Items>
                                    <telerik:DropDownListItem Text="Accommodation" Value="0001" />
                                    <telerik:DropDownListItem Text="Agency Cost" Value="0002" />
                                    <telerik:DropDownListItem Text="Banquet room & Venue Sytem" Value="0003" />
                                    <telerik:DropDownListItem Text="Gimmick / Souvenir" Value="0004" />
                                    <telerik:DropDownListItem Text="Meal & Beverage" Value="0005" />
                                    <telerik:DropDownListItem Text="Prints" Value="0006" />
                                    <telerik:DropDownListItem Text="Transportation" Value="0007" />
                                    <telerik:DropDownListItem Text="Honorarium" Value="0008" />
                                </Items>
                            </telerik:RadDropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="DESCRIPTION" HeaderText="Description" HeaderStyle-Width="50%">
                        <ItemTemplate><%#DataBinder.Eval(Container.DataItem, "DESCRIPTION")%></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radGrdTxtDescription" runat="server" Width="100%" CssClass="input"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="QTY" HeaderText="Qty" UniqueName="QTY" HeaderStyle-Width="80px">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("Qty")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radGrdNumQty" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeypress="return fn_OnGridKeyPress(this, event)"
                                onkeyup="return fn_OnGridKeyUp(this)"
                                DecimalDigits="0">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="PRICE" HeaderText="Price" UniqueName="PRICE" HeaderStyle-Width="100px">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("PRICE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="radGrdNumPrice" NumberFormat-DecimalDigits="0"
                                MinValue="0" MaxValue="99999999999"
                                Width="100%" EnabledStyle-HorizontalAlign="Right" ClientEvents-OnBlur="fn_OnGridNumBlur">
                            </telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="radGrdNumPrice" runat="server" Width="100%" CssClass="input align_right" AllowNegative="true"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeypress="return fn_OnGridKeyPress(this, event)"
                                onkeyup="return fn_OnGridKeyUp(this)"
                                DecimalDigits="0">                                
                            </asp:TextBox>

                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="AMOUNT" HeaderText="Amount" UniqueName="AMOUNT" HeaderStyle-Width="150px" ReadOnly="true"
                        Aggregate="Sum" FooterAggregateFormatString="{0:#,##0}"
                        FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("AMOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%--                    <telerik:GridCalculatedColumn HeaderText="Amount" UniqueName="AMOUNT" DataType="System.Double" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Right"
                        DataFields="QTY, PRICE" Expression='{0}*{1}' Aggregate="Sum" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}"
                        FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true" FooterStyle-Font-Size="Large">
                    </telerik:GridCalculatedColumn>--%>
                    <telerik:GridTemplateColumn UniqueName="COMMENT" HeaderText="Comment" HeaderStyle-Width="50%">
                        <ItemTemplate><%#DataBinder.Eval(Container.DataItem, "COMMENT")%></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radGrdTxtComment" runat="server" Width="100%" CssClass="input"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove"
                                OnClientClick='<%# String.Format("return openConfirmPopUp({0});",Eval("IDX"))%> '
                                ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                </Columns>

            </MasterTableView>
            <ClientSettings>
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <div id="divPPM_CM_EM" style="margin-left: 3px; display: none">
            <b>Attachment</b><br />
            (1)	Agenda/Invitation letter<br />
            (2)	Quotation(In case of accommodation provision to participating HCPs only)<br />
            (3)	Consultancy Proposal(In Case of honorarium payment)<br />
            (4)	CV(In case of honorarium payment)<br />
        </div>
        <div id="divCS" style="margin-left: 3px; display: none">
            <b>Attachment</b><br />
            (1)	Clinical Contract Nr. / DB Nr.<br />
            (2)	Delegation log / Invitation letter / Contact list<br />

        </div>
        <div id="divPS" style="margin-left: 3px; display: none">
            <b>Attachment</b><br />
            (1)	Agenda/Invitation letter<br />
            

        </div>
        <div id="divPSWH" style="margin-left: 3px; display: none">
            <b>Attachment</b><br />
            (1)	Agenda/Invitation letter<br />
            (2)	Speaker Proposal(In Case of honorarium payment)<br />
            (3)	CV(In case of honorarium payment)<br />
            

        </div>
    </div>
    <input type="hidden" id="hddCompanyCode" runat="server" />
    <input type="hidden" id="hddGridItems" runat="server" value="" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
    <input type="hidden" id="hddReuse" runat="server" />

</asp:Content>

