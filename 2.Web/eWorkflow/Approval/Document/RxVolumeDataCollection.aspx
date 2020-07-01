<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="RxVolumeDataCollection.aspx.cs" Inherits="Approval_Document_RxVolumeDataCollection" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" Runat="Server">    
    <script type="text/javascript">
        function fn_DoSave(sender, args) {
            return true;
        }

        function fn_keyPress(sender, args) {           
            var text = sender.get_value() + args.get_keyCharacter();
            if (!text.match('^[0-9,]+$'))
                args.set_cancel(true);
        }

        function fn_RemoveZeros(sender, args) {
            var tbValue = sender._textBoxElement.value;
            if (tbValue.indexOf(".00") != -1)
                sender._textBoxElement.value = tbValue.substr(0, tbValue.indexOf(".00"));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" Runat="Server">
    <!-- doc Style -->
			<div class="doc_style">
				<div class="data_type1">
					<table>
						<colgroup>
							<col />
							<col style="width:75%;" />
						</colgroup>
						<tbody>
							<tr>
								<th>Collected Product <span class="text_red">*</span> 자료 제품명</th>
								<td><telerik:RadTextBox runat="server" TextMode="MultiLine" Height="25px" ID="RadtxtCollectedProduct"  Width="98%"></telerik:RadTextBox></td>
							</tr>
							<tr>
								<th>Data Source <span class="text_red">*</span> 자료 수집처</th>
								<td><telerik:RadTextBox runat="server" TextMode="MultiLine" Height="25px" ID="RadtxtDataSource"  Width="98%"></telerik:RadTextBox></td>
							</tr>
							
							<tr>
								<th>Details of Rx Volume Data <span class="text_red">*</span><br />수집하려는 데이터 항목 및 처방의사 정보 포함 유무 </th>
								<td><telerik:RadTextBox runat="server" TextMode="MultiLine" Height="50px" ID="RadtxtDetailsOfRxVolumeData" Width="98%"></telerik:RadTextBox>
                                <table id="tblCheckBox_1" style="margin-bottom: 5px;width:98%">
                                    <tr>
                                        <th>처방의사 정보를 삭제하였습니까? </th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadButton ID="radChkDetailsOfRxVolumeData_Yes" runat="server" Text="Yes" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" GroupName="DetailsOfRxVolumeData"> </telerik:RadButton>
                                            <telerik:RadButton ID="radChkDetailsOfRxVolumeData_No" runat="server" Text="No" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio"  GroupName="DetailsOfRxVolumeData"> </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                                </td>
							</tr>
							<tr>
								<th>Including Competitive Data & Details<span class="text_red">*</span><br />경쟁사 정보 포함 유무 및 항목 </th>
								<td>
                                    <table id="tblCheckBox_2" style="margin-bottom: 5px;width:98%">
                                        <tr>
                                            <th>포함 시, 세부 항목 기입해주십시오. </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadButton ID="radChkIncludingCompetitiveDataAndDetails_Yes" runat="server" Text="Yes" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" GroupName="kIncludingCompetitiveDataAndDetails"> </telerik:RadButton>
                                                <telerik:RadButton ID="radChkIncludingCompetitiveDataAndDetails_No" runat="server" Text="No" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" GroupName="kIncludingCompetitiveDataAndDetails"> </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <telerik:RadTextBox runat="server" TextMode="MultiLine" Height="50px" ID="RadtxtIncludingCompetitiveDataAndDetails" Width="98%"></telerik:RadTextBox>
                                </td>
							</tr>
							<tr>
								<th>Purpose of Collection <span class="text_red">*</span> <br />수집하려는 목적 </th>
								<td><telerik:RadTextBox runat="server" TextMode="MultiLine"  Height="50px" ID="RadtxtPurpose" Width="98%"></telerik:RadTextBox></td>
							</tr>
							<tr>
								<th>Collection Method <span class="text_red">*</span> <br />수집방식 </th>
								<td><telerik:RadTextBox runat="server" TextMode="MultiLine"  Height="50px" ID="RadtxtCollectionMethod" Width="98%"></telerik:RadTextBox></td>
							</tr>
							<tr>
								<th>Validity Date <span class="text_red">*</span> <br />자료 유효 기간 (이후 삭제) </th>
								<td>
                                    <telerik:RadDatePicker ID="radDateValidityDate" runat="server" Width="100px" Calendar-ShowRowHeaders="false"
                                        MinDate="1900-01-01" MaxDate="2050-12-31" Culture="ko-KR">
                                        <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                        <Calendar runat="server" RangeMinDate="2010-01-01" RangeMaxDate="2050-12-31">
                                            <SpecialDays>
                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                            </SpecialDays>
                                        </Calendar>
                                    </telerik:RadDatePicker>
								</td>
							</tr>
							<tr>
								<th>Archiving of Data  <span class="text_red">*</span><br />자료 저장 방식</th>
								<td><telerik:RadTextBox runat="server" TextMode="MultiLine" Height="50px" ID="RadtxtArchiving"  Width="98%"></telerik:RadTextBox></td>
							</tr>
							<tr>
								<th>Access Right  <span class="text_red">*</span><br />자료의 접근 권한자</th>
								<td><telerik:RadTextBox runat="server" TextMode="MultiLine" Height="50px" ID="RadtxtAccessRight"  Width="98%"></telerik:RadTextBox></td>
							</tr>
						</tbody>
					</table>
				</div>
			</div><!-- //doc Style -->
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
	<input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

