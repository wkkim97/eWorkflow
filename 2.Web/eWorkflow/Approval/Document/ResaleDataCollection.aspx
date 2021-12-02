<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="ResaleDataCollection.aspx.cs" Inherits="Approval_Document_ResaleDataCollection" %>
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
								<th>Data Source(자료 수집처) <span class="text_red">*</span></th>
								<td><telerik:RadTextBox runat="server" TextMode="MultiLine" Height="50px" ID="RadtxtDataSource"  Width="98%"></telerik:RadTextBox></td>
							</tr>
							
							<tr>
								<th>Details of Resale Data <br />수집하려는 재판매 데이터 항목 <span class="text_red">*</span></th>
								<td><telerik:RadTextBox runat="server" TextMode="MultiLine" Height="100px" ID="RadtxtResaleData" Width="98%"></telerik:RadTextBox></td>
							</tr>
                            <tr>
                                <th>Including Price or Value<br />수집항목에 Price, Value 포함 여부<span class="text_red">*</span></th>
                                <td>
                                    <telerik:RadButton runat="server" ID="RadrdoPriceValue_Y" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="Y" Text="포함" GroupName="PRICEVALUE" AutoPostBack="false" ></telerik:RadButton>
                                    <telerik:RadButton runat="server" ID="RadrdoPriceValue_N" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="N" Text="미포함" GroupName="PRICEVALUE" AutoPostBack="false"></telerik:RadButton>

                                </td>
                            </tr>
                            <tr>
                                <th>Employee to Access<br /> 접속 권한자을 가진 직원의 부서/이름 <span class="text_red">*</span></th>
                                <td><telerik:RadTextBox runat="server" TextMode="MultiLine" Height="100px" ID="RadtxtEmployee" Width="98%"></telerik:RadTextBox></td>
                            </tr>
							<%--<tr>
								<th>&nbsp;&nbsp;&nbsp;-&nbsp;Resale Price</th>
                                <td>
                                    <telerik:RadNumericTextBox runat="server" ID="RadtxtResalePrice"  InputType="Number" 
                                        NumberFormat-DecimalDigits="0" Width="120px" Value="0" EnabledStyle-HorizontalAlign="Right">
                                        
                                    </telerik:RadNumericTextBox></td>
							</tr>
							<tr>
								<th>&nbsp;&nbsp;&nbsp;-&nbsp;Others(e.g Resale Quantity)</th>

								<td>
                                    <telerik:RadNumericTextBox ID="RadtxtOthres" runat="server" Width="120px" Value="0"
                                        NumberFormat-DecimalDigits="0" EnabledStyle-HorizontalAlign="Right"></telerik:RadNumericTextBox>
                                    <telerik:RadTextBox runat="server" ID="RadtxtOthres"  Width="120px" ></telerik:RadTextBox></td>
							</tr>--%>
							<tr>
								<th>Purpose of Collection <br />수집하려는 목적 <span class="text_red">*</span></th>
								<td><telerik:RadTextBox runat="server" TextMode="MultiLine"  Height="50px" ID="RadtxtPurpose" Width="98%"></telerik:RadTextBox></td>
							</tr>
							<tr>
								<th>Archiving of Data<br />자료 저장 방식 / 보관 연한 (예:1년 후 폐기)  <span class="text_red">*</span></th>
								<td><telerik:RadTextBox runat="server" TextMode="MultiLine" Height="50px" ID="RadtxtArchiving"  Width="98%"></telerik:RadTextBox></td>
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

