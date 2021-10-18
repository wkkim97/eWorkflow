<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="MortgageManagement.aspx.cs" Inherits="Approval_Document_MortgageManagement" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function fn_DoRequest(sender, args) {
            if ($find('<%= radChkCredit.ClientID %>').get_checked()) {
                var execute = $find('<%= radBtnCalNewCreditLimit.ClientID %>').get_value();
                if (execute == 'execute') return true;
                else {

                    fn_OpenDocInformation('Calculation Credit Limit을 계산바랍니다.');

                    return false;
                }
            } else
                return true;
        }

        <%--function fn_OpenCustomer(sender, eventArgs) {
            var wnd = $find("<%= RadWinPopupCustomer.ClientID %>");
            wnd.setUrl("/eWorks/Common/Popup/CustomerList.aspx");
            wnd.show();
            sender.set_autoPostBack(false);
        }--%>

        function GetBG() {
            var selectedValue;
            var controls = $('#<%= divBG.ClientID %>').children();
            for (var i = 0; i < controls.length; i++) {
                var bu = controls[i];
                if ($find(bu.id).get_checked()) {
                    selectedValue = $find(bu.id).get_value();
                    if (selectedValue == '08')
                        selectedValue = 'PH';
                    else if (selectedValue == '16')
                        selectedValue = 'CC';
                    else if (selectedValue == '18')
                        selectedValue = 'AH';
                    break;
                }
            }
            return selectedValue;
        }
        function GetBGValue() {
            var selectedValue;
            var controls = $('#<%= divBG.ClientID %>').children();
            for (var i = 0; i < controls.length; i++) {
                var bu = controls[i];
                if ($find(bu.id).get_checked()) {
                    selectedValue = $find(bu.id).get_value();
                    break;
                }
            }
            return selectedValue;
        }

        function fn_OpenCustomer(sender, eventArgs) {

            var selectedValue = GetBG();
            if (selectedValue) {
                var wnd = $find("<%= RadWinPopupCustomer.ClientID %>");

                wnd.setUrl("/eWorks/Common/Popup/CustomerList.aspx?bu=" + selectedValue);
                wnd.show();
                sender.set_autoPostBack(false);
            }
            else {
                fn_OpenDocInformation('Please Select a BU');
                sender.set_autoPostBack(false);
            }
        }

        function fn_ClientClose(oWnd, args) {
            var item = args.get_argument();

            if (item != null) {
                var txtcustomer = $find("<%= RadtxtCustomer.ClientID%>");
                txtcustomer.set_value(item.CUSTOMER_NAME.trim() + " (" + item.CUSTOMER_CODE + ")");
                <%--$('#<%=RadtxtBG.ClientID%>').val(item.BU);--%>
                $('#<%=hddCustomerName.ClientID%>').val(item.CUSTOMER_NAME);
                $('#<%=hddCustomerCode.ClientID%>').val(item.CUSTOMER_CODE);
                $('#<%=hddCreditLimit.ClientID%>').val(item.CREDIT_LIMIT);
            }
            else {
                oWnd.close();
            }
        }

        function fn_OnStatusSelected(sender, args) {
            var item = args.get_item();
            if (item.get_value() == 'Currently') {
                $find('<%= RadDateReturned.ClientID %>').set_selectedDate(null);
                $find('<%= RadDateReturned.ClientID %>').set_enabled(false);
            } else {
                $find('<%= RadDateReturned.ClientID %>').set_enabled(true);
            }
        }

        function executeCalculateAjax() {
            try {

                var customerCode = $('#<%= hddCustomerCode.ClientID %>').val();
                var bgCode = GetBGValue();
                var status = $('#<%= lblStatus.ClientID %>').text();
                var amount = $find('<%= RadNuRevaluation.ClientID%>').get_value();
                amount = amount.toString().replace(/,/gi, '').replace(/ /gi, '');
                if (amount.length < 1) amount = '0';
                var loadingPanel = "#<%= radLoading.ClientID %>";

                $(loadingPanel).show();
                var svcUrl = WCFSERVICE + "/AfterTreatmentServices.svc/CalculateCreditLimit/" + customerCode + "/" + bgCode + "/" + status + "/" + amount;
                $.support.cors = true;
                $.ajax({
                    type: "GET",
                    url: svcUrl,
                    dataType: "JSON",
                    success: function (data) {
                        setTimeout(function () { }, 1000);
                        $find('<%= RadNuNewCreditLimit.ClientID %>').set_value(data.Amount);
                        $find('<%= RadNuRevaluation.ClientID%>').get_element().readOnly = true;
                        $find('<%= radBtnCalNewCreditLimit.ClientID %>').set_value('execute');
                        $find('<%= radBtnCalNewCreditLimit.ClientID %>').set_text('Calculated.')
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

        function fn_CalNewCreditLimit(sender, args) {
            var btnValue = $find('<%= radBtnCalNewCreditLimit.ClientID %>').get_value();

            if (btnValue == 'not') {
                var customer = $find('<%= RadtxtCustomer.ClientID%>').get_value();
                if (customer.length < 1) {
                    fn_OpenDocInformation('Customer를 선택바랍니다.');
                } else {
                    executeCalculateAjax();
                }
            }
            else {
                $find('<%= RadNuRevaluation.ClientID%>').get_element().readOnly = false;
                $find('<%= radBtnCalNewCreditLimit.ClientID %>').set_value('not');
                $find('<%= radBtnCalNewCreditLimit.ClientID %>').set_text('Calculation Credit Limit')
            }

        }

        function executeUpdateAjax() {
            try {
                var processId = $('#<%= hddProcessID.ClientID %>').val();
                var customerCode = $('#<%= hddCustomerCode.ClientID %>').val();
                var bgCode = GetBGValue();
                var status = $('#<%= lblStatus.ClientID %>').text();
                var creditLimitStatus = '';
                if (status == 'Currently') creditLimitStatus = 'valid';
                else creditLimitStatus = 'Termination';
                var userId = $('#<%= hddUserId.ClientID %>').val();
                var amount = $find('<%= RadNuNewCreditLimit.ClientID%>').get_value();
                amount = amount.toString().replace(/,/gi, '').replace(/ /gi, '');
                if (amount.length < 1) amount = '0';

                var currentlyId = $('#<%= hddCurrentlyProcessID.ClientID %>').val();
                if (currentlyId.trim().length < 1) currentlyId = 'currentlyid'; //임의로 할당
                var loadingPanel = "#<%= radLoading.ClientID %>";

                $(loadingPanel).show();
                var svcUrl = WCFSERVICE + "/AfterTreatmentServices.svc/UpdateCreditLimit/" + processId + "/" + customerCode + "/" + bgCode + "/" + creditLimitStatus + "/" + userId + "/" + amount + "/" + currentlyId;
                $.support.cors = true;
                $.ajax({
                    type: "GET",
                    url: svcUrl,
                    dataType: "JSON",
                    success: function (data) {
                        setTimeout(function () { }, 1000);
                        if (data == 'success') {
                            $find('<%= radBtnCreditLimit.ClientID%>').set_visible(false);
                            $('#divValidated').empty();
                            $('#divValidated').html("<font color='red'>Validated by " + userId + "</font>")
                        } else if (data == 'fail') {
                            fn_OpenDocInformation('CreditLimit금액 업데이트에 문제가 발생하였습니다. 관리자에게 문의바랍니다.');
                        } else if (data == 'different') {
                            fn_OpenDocInformation('CreditLimit금액을 확인 바랍니다.');
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

        function fn_CreditLimit(sender, args) {
            executeUpdateAjax();
        }

        function fn_OnCheckCreditChanged(sender, args) {
            var chk = args.get_checked();
            if (chk == false) {
                $find('<%= radBtnCalNewCreditLimit.ClientID %>').set_visible(false);
                $find('<%= RadNuNewCreditLimit.ClientID %>').set_value('');
                $find('<%= RadNuNewCreditLimit.ClientID %>').disable(true);
            }
            else if (chk == true) {
                $find('<%= radBtnCalNewCreditLimit.ClientID %>').set_visible(true);
                $find('<%= RadNuNewCreditLimit.ClientID %>').enable(true);
            }
    }

    function fn_OpenReturn(sender, args) {
        var nWidth = 924;
        var nHeight = 680;
        var left = (screen.width / 2) - (nWidth / 2);
        var top = (screen.height / 2) - (nHeight / 2) - 10;

        var processId = $('#<%= hddProcessID.ClientID %>').val();
        var documentId = $('#<%= hddDocumentID.ClientID%>').val();
        var formName = 'MortgageManagement.aspx';
        var url = fn_GetWebRoot() + "Approval/Document/" + formName + "?CurrentlyProcessId=" + processId + "&documentid=" + documentId;
        window.close();
        return window.open(url, "", "width=" + nWidth + "px, height=" + nHeight + "px, top=" + top + "px, left=" + left + "px, toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes");

    }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="radLoading" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <div class="doc_style">
        <h3>Please fill out below fields Mortgage Management</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                    <col style="width: 25%;" />
                    <col style="width: 25%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th>BG</th>
                        <td colspan="3">
                            <div id="divBG" runat="server">
                                <telerik:RadButton ID="radBtnPHRDC" runat="server" Value="08" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="PH, R, DC" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radBtnCC" runat="server" Value="16" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="CC" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radBtnAH" runat="server" Value="18" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="AH" AutoPostBack="false"></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>Customer</th>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="RadtxtCustomer" CssClass="input" Width="95%" ReadOnly="true"></telerik:RadTextBox>
                            <%--&nbsp;&nbsp;&nbsp;&nbsp;BG : &nbsp;
                            <telerik:RadTextBox runat="server" ID="RadtxtBG" CssClass="input" Width="40px" ReadOnly="true"></telerik:RadTextBox>--%>
                            <%--<telerik:RadButton runat="server" ButtonType="ToggleButton" Width="70" ID="RadbtnSearch" Text="Search" CssClass="btn btn-gray btn-size2" OnClientClicked="fn_OpenCustomer"></telerik:RadButton>--%>
                            <telerik:RadButton ID="RadbtnSearch" runat="server" Text="" OnClientClicked="fn_OpenCustomer"
                                CssClass="btn_grid" Width="18px" Height="18px">
                                <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                            </telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Status</th>
                        <td colspan="3">
                            <asp:Label ID="lblStatus" runat="server">Currently</asp:Label>
                            <%--<telerik:RadDropDownList ID="RadDropStatus" runat="server" CssClass="sel" Width="215"
                                DefaultMessage="--- Select ---" AutoPostBack="false" OnClientItemSelected="fn_OnStatusSelected">
                                <Items>
                                    <telerik:DropDownListItem Text="Currently" Value="Currently" />
                                    <telerik:DropDownListItem Text="Returned" Value="Returned" />
                                </Items>
                            </telerik:RadDropDownList>--%>
                        </td>
                    </tr>
                    <tr>
                        <th>Mortgage Type</th>
                        <td colspan="3">
                            <telerik:RadDropDownList ID="radDropMortgageType" runat="server" CssClass="sel" Width="465" DefaultMessage="--- Select ---" DataTextField="CODE_NAME" DataValueField="SUB_CODE"></telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Book Value(KRW)</th>
                        <td>
                            <telerik:RadNumericTextBox runat="server" ID="RadNubookValue" Width="100"
                                NumberFormat-DecimalDigits="0" MinValue="0" MaxValue="99999999999"
                                EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right">
                            </telerik:RadNumericTextBox>
                        </td>
                        <th>Revaluation(KRW) </th>
                        <td>
                            <telerik:RadNumericTextBox runat="server" ID="RadNuRevaluation" Width="100"
                                NumberFormat-DecimalDigits="0" MinValue="0" MaxValue="99999999999"
                                EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right">
                            </telerik:RadNumericTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <telerik:RadButton runat="server" ID="radChkCredit" ButtonType="ToggleButton" ToggleType="CheckBox"
                                Text="New Credit Limit" Value="Y" AutoPostBack="false" Checked="true" OnClientCheckedChanged="fn_OnCheckCreditChanged">
                            </telerik:RadButton>
                        </th>
                        <td colspan="3">
                            <telerik:RadNumericTextBox runat="server" ID="RadNuNewCreditLimit" Width="100"
                                NumberFormat-DecimalDigits="0" MinValue="0" MaxValue="99999999999"
                                EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right">
                            </telerik:RadNumericTextBox>
                            <telerik:RadButton ID="radBtnCalNewCreditLimit" runat="server" Text="Calculation Credit Limit" AutoPostBack="false" OnClientClicked="fn_CalNewCreditLimit"
                                ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size1 bold"
                                Value="not">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnCreditLimit" runat="server" Text="Credit Limit" AutoPostBack="false"
                                OnClientClicked="fn_CreditLimit"
                                ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size1 bold"
                                Visible="false">
                            </telerik:RadButton>
                            <div id="divValidated" style="display: inline-block; text-align: left">
                                <asp:Label ID="lblValidated" runat="server" Style="visibility: visible"></asp:Label>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <th>Received Date</th>
                        <td>
                            <telerik:RadDatePicker runat="server" ID="RadDateReceived" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                        <th>Returned Date</th>
                        <td>
                            <telerik:RadDatePicker runat="server" ID="RadDateReturned" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <th>Issue Date</th>
                        <td>
                            <telerik:RadDatePicker runat="server" ID="RadDateIssue" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                        <th>Due Date</th>
                        <td>
                            <telerik:RadDatePicker runat="server" ID="RadDateDue" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <th>Publisher</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtPublisher" Width="100%"></telerik:RadTextBox>
                        </td>
                        <th>Published No.</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtPublished" Width="100%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Comment</th>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="RadtxtComment" Width="100%"></telerik:RadTextBox></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="divReturn" class="align_right pb10" style="display: none" runat="server">
            <telerik:RadButton runat="server" ID="radBtnReturn" ButtonType="ToggleButton" OnClientClicked="fn_OpenReturn"
                ForeColor="White" CssClass="btn btn-blue btn-size1 bold" Width="100" Text="Return" AutoPostBack="false" Visible="false">
            </telerik:RadButton>
        </div>
        <div id="Calculation" style="display: none">
            <h3>Credit Limit Calculation</h3>
            <div class="data_type1">
                <table>
                    <colgroup>
                        <col style="width: 15%;" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>New</th>
                            <td>
                                <telerik:RadTextBox runat="server" ID="RadtxtCalculation" CssClass="input" Width="300"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <%--Popup--%>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="RadWinPopupCustomer" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer" Width="430px" Height="470px" Behaviors="Default" Modal="true" OnClientClose="fn_ClientClose" NavigateUrl="./CustomerList.aspx"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <input type="hidden" id="hddCustomerName" runat="server" />
    <input type="hidden" id="hddCustomerCode" runat="server" />
    <input type="hidden" id="hddCreditLimit" runat="server" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddCurrentlyProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
    <input type="hidden" id="hddUserId" runat="server" />
    <input type="hidden" id="hddApplyMaster" runat="server" />
    <input type="hidden" id="hddIsCalledReturn" runat="server" />
</asp:Content>

