<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="CompetitorContract.aspx.cs" Inherits="Approval_Document_CompetitorContract" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <style type="text/css">
        .tbl_participants colgroup table tr td {
            border:none !important;
            margin:1px 1px 1px 1px !important;
        }
    </style>
    <script type="text/javascript">

        function pageLoad() {

        }

        function fn_OnCategoryIndexChanged(sender, args) {
            var index = args.get_index();
            var ddlCategory = $find('<%= radDdlCategory.ClientID %>');
            var selectedValue = ddlCategory.get_items().getItem(index).get_value();
            setVisibleControl(selectedValue);
        }

        function setVisibleControl(value) {
            if (value == '0001') {
                $('#divHostAndMeeting').show();
                $('#rowTACategory').show();
                $('#rowHost').show();
                $('#rowCounterParty').hide();
                $('#rowContractNo').hide();
                $('#rowContractPeriod').hide();
                $('#rowMeetingDate').show();
                $('#rowVenue').show();
                $('#rowParticipants').show();
                $('#rowAgenda').show();
                $('#rowProduct').hide();
                $('#rowActivity').hide();
            } else if (value == '0002') {
                $('#divHostAndMeeting').show();
                $('#rowTACategory').hide();
                $('#rowHost').hide();
                $('#rowCounterParty').show();
                $('#rowContractNo').hide();
                $('#rowContractPeriod').hide();
                $('#rowMeetingDate').show();
                $('#rowVenue').show();
                $('#rowParticipants').show();
                $('#rowAgenda').show();
                $('#rowProduct').hide();
                $('#rowActivity').hide();

            } else if (value == '0003') {
                $('#divHostAndMeeting').show();
                $('#rowTACategory').hide();
                $('#rowHost').hide();
                $('#rowCounterParty').show();
                $('#rowContractNo').show();
                $('#rowContractPeriod').show();
                $('#rowMeetingDate').hide();
                $('#rowVenue').hide();
                $('#rowParticipants').show();
                $('#rowAgenda').hide();
                $('#rowProduct').show();
                $('#rowActivity').show();

            } else if (value == '0004') {
                $('#divHostAndMeeting').show();
                $('#rowTACategory').hide();
                $('#rowHost').hide();
                $('#rowCounterParty').show();
                $('#rowContractNo').hide();
                $('#rowContractPeriod').hide();
                $('#rowMeetingDate').show();
                $('#rowVenue').show();
                $('#rowParticipants').show();
                $('#rowAgenda').show();
                $('#rowProduct').hide();
                $('#rowActivity').hide();
            } else {
                $('#divHostAndMeeting').hide();
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
                    <th>Category <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadDropDownList ID="radDdlCategory" runat="server" Width="100%" AutoPostBack="false" DefaultMessage="---Select---"
                            OnClientSelectedIndexChanged="fn_OnCategoryIndexChanged" OnSelectedIndexChanged="radDdlCategory_SelectedIndexChanged" DropDownWidth="400px">
                            <Items>
                                <telerik:DropDownListItem Text="Trade Association Meeting" Value="0001" />
                                <telerik:DropDownListItem Text="Occasional Vertical Relation Meeting" Value="0002" />
                                <telerik:DropDownListItem Text="Alliance Meeting(계약 체결 이후)" Value="0003" />
                                <telerik:DropDownListItem Text="Other General Meeting(협회 미등록 된 단타성 세미나 포함)" Value="0004" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divHostAndMeeting" class="data_type1" style="display: none">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr id="rowTACategory" >
                    <th>TA Category  <span class="text_red">*</span></th>
                    <td>
                        <div id="divTACategory" runat="server" style="margin: 0 0 0 0">
                            <telerik:RadButton ID="radBtnWhitelisted" runat="server" Text="Whitelisted" Value="Whitelisted" GroupName="TA"
                                ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnNon_Whitelisted" runat="server" Text="Non-Whitelisted" Value="Non-Whitelisted" GroupName="TA"
                                ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr> 

                <tr id="rowHost">
                    <th>Host <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadDropDownList ID="radDdlHost" runat="server" DataTextField="E_NAME" DataValueField="E_NAME" Width="100%"  DropDownWidth="400px"></telerik:RadDropDownList>
                    </td>
                </tr>
                <tr id="rowCounterParty" style="display: none">
                    <th>Counter Party <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radTxtCounterParty" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="rowContractNo" style="display: none">
                    <th>Contract No. <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radTxtContractNo" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="rowContractPeriod" style="display: none">
                    <th>Contract Period <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtContractPeriod" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="rowMeetingDate">
                    <th>Meeting Date and Time <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadDatePicker ID="radDatMeeting" runat="server" Width="100px" Culture="ko-KR" MinDate="1900-01-01" MaxDate="2050-12-31">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            <Calendar runat="server" ShowRowHeaders="false" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" />
                                </SpecialDays>
                            </Calendar>

                        </telerik:RadDatePicker>
                        &nbsp;
                        <telerik:RadTimePicker ID="radTimStartMeeting" runat="server" Width="100px"></telerik:RadTimePicker>
                        ~&nbsp;<telerik:RadTimePicker ID="radTimFinishMeeting" runat="server" Width="100px"></telerik:RadTimePicker>
                    </td>
                </tr>
                <tr id="rowVenue">
                    <th>Venue <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtVenue" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="rowParticipants">
                    <th>Participants <span class="text_red">*</span>
                    </th>
                    <td>
                        <table class="tbl_participants">
                            <colgroup>
                                <col style="width: 70px" />
                                <col />
                            </colgroup>
                            <tr style="border: none !important">
                                <td>Internal</td>
                                <td>
                                    <telerik:RadAutoCompleteBox ID="radAcomParticipants" runat="server" AllowCustomEntry="false" Width="100%">
                                        <WebServiceSettings Method="SearchUserByName" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                                    </telerik:RadAutoCompleteBox>
                                </td>
                            </tr>
                            <tr style="border: none !important">
                                <td>External</td>
                                <td>
                                    <telerik:RadTextBox ID="radTxtExternal" runat="server" Width="100%"></telerik:RadTextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="rowAgenda">
                    <th>Agenda <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtAgenda" runat="server" TextMode="MultiLine" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="rowProduct">
                    <th>Product <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtProduct" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="rowActivity">
                    <th>Activity <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtActivity" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>

                <tr>
                    <th>Purpose
                    </th>
                    <td>
                        <div id="divPurpose" runat="server" style="width: 100%">
                            <telerik:RadButton ID="radChkPurpose1" runat="server" Text="Sales volumes or prices.(판매량 또는 판매가격)" ButtonType="ToggleButton" ToggleType="CheckBox" Value="0001" AutoPostBack="false"></telerik:RadButton>
                            <br />
                            <telerik:RadButton ID="radChkPurpose2" runat="server" Text="Detailed information on the current market position.(회사의 현재 시장 지위에 대한 상세 정보)" ButtonType="ToggleButton" ToggleType="CheckBox" Value="0002" AutoPostBack="false"></telerik:RadButton>
                            <br />
                            <telerik:RadButton ID="radChkPurpose3" runat="server" Text="Current sales efforts of companies.(회사의 판매 노력)" ButtonType="ToggleButton" ToggleType="CheckBox" Value="0003" AutoPostBack="false"></telerik:RadButton>
                            <br />
                            <telerik:RadButton ID="radChkPurpose4" runat="server" Text="Non-public information regarding specific customers.(특정 고객에 해한 비공개 정보)" ButtonType="ToggleButton" ToggleType="CheckBox" Value="0004" AutoPostBack="false"></telerik:RadButton>
                            <br />
                            <telerik:RadButton ID="radChkPurpose5" runat="server" Text="Planned modification of production capacity, technological items and related cost structures.(생산량의 변경 계획, 기술력과 원가구조에 대한 정보교환)" ButtonType="ToggleButton" ToggleType="CheckBox" Value="0005" AutoPostBack="false"></telerik:RadButton>
                            <br />
                            <telerik:RadButton ID="radChkPurpose6" runat="server" Text="Projections and forecasts unless aggregated level.(합계 수준이 아닌 예상과 예측)" ButtonType="ToggleButton" ToggleType="CheckBox" Value="0006" AutoPostBack="false"></telerik:RadButton>
                            <br />
                        </div>

                        Other Purpose:
                        <br />
                        <telerik:RadTextBox ID="radTxtOtherPurpose" runat="server" TextMode="MultiLine" Height="100px" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
           <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>

                <tr>
                    <th> <span class="text_red">※ Attachment file name example</span>
                    </th>
                    <td>
                        Meeting Minutes ⇒ MM_CWID_DDMMYYYY
                    </td>
                    <td>
                        Deviation Report ⇒ DR_CWID_DDMMYYYY
                   </td>
                </tr>
            </table>
        </div>
<%--        2022.01.29, change request, start, --%>
        <%--*회의록(meeting minutes)은 회의 후, 반드시 input comment를 통하여 보고 완료(문서 업로드 혹은, 별도 문서가 없을 시 간단한 회의내용을 코멘트로 입력)해 주시기 바랍니다.--%>
        <span class="text_red" style="font-weight:bold">*회의록(meeting minutes)은 회의 후, 반드시 input comment를 통하여 보고 완료(문서 업로드 혹은, 별도 문서가 없을 시 간단한 회의내용을 코멘트로 입력)해 주시기 바랍니다.</span>
<%--        2022.01.29, change request, end, --%>
    </div>
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

