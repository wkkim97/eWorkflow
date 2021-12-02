<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="CreditRelease.aspx.cs" Inherits="Approval_Document_CreditRelease" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">

        function fn_DoRequest(sender, args) {
            return true;
        }

        function fn_DoSave(sender, args) {
            return true;
        }

        function fn_OpenCustomer(sender, eventArgs) {
            var controls = $('#<%= divBU.ClientID %>').children();
            var selectedValue;

            for (var i = 0; i < controls.length; i++) {
                var bu = controls[i];
                if ($find(bu.id).get_checked()) {
                    selectedValue = $find(bu.id).get_value();
                    break;
                }
            }
            if (selectedValue) {
                var wnd = $find("<%= radWinPopupCustomer.ClientID %>");
                var parvw = "IE";

                wnd.setUrl("/eWorks/Common/Popup/CustomerList.aspx?bu=" + selectedValue + "&parvw=" + parvw + "&creditlimit=Y&level=Y");
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
                var txtcustomer = $find("<%= radtxtCustomer.ClientID%>");
                var txtCreditLimit = $find("<%= radtxtCreditLimit.ClientID%>");
                var txtCollateral = $find("<%= radtxtCollateral.ClientID%>");
                txtcustomer.set_value(item.CUSTOMER_NAME.trim() + " (" + item.CUSTOMER_CODE + ")");
                txtCreditLimit.set_value(item.CREDIT_LIMIT);
                txtCollateral.set_value(item.MORTAGE);
                $('#<%=hddCustomerCode.ClientID%>').val(item.CUSTOMER_CODE);
            }
            else {
                oWnd.close();
            }
        }

        //초과일수 계산
        function CalculationOverDue() {
            var txtPaymentTerm = $('#<%=radtxtPaymentTerm.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');
            var txtAgingDay = $('#<%=radtxtAgingDay.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');
            var txtOverDue = $('#<%=radtxtOverDue.ClientID%>');
            var total = parseFloat(txtAgingDay) - parseFloat(txtPaymentTerm);
            txtOverDue.val(total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));

            if (txtAgingDay == "" || txtPaymentTerm == "")
                txtOverDue.val(0);
        }
        //초과액 계산
        function CalculationExceeded() {
            var txtAmount = $('#<%=radNumAmount.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');
            var txtInclNote = $('#<%=radtxtInclNote.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');
            var txtOpenOrder = $('#<%=radtxtOpenOrder.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');
            var txtOrdered = $('#<%=radtxtOrdered.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');
            var txtCreditLimit = $('#<%=radtxtCreditLimit.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');
            var txtExceeded = $('#<%=radtxtExceededAmount.ClientID%>');

            var total = parseFloat(txtAmount) + parseFloat(txtInclNote) + parseFloat(txtOpenOrder) + parseFloat(txtOrdered) - parseFloat(txtCreditLimit);
            txtExceeded.val(total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));

            if (txtAmount == "" || txtInclNote == "" || txtOpenOrder == "" || txtOrdered == "" || txtCreditLimit == "")
                txtExceeded.val(0);
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <!-- doc Style -->
    <div class="doc_style">
        <h3>The Information</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col />
                    <col style="width: 35%;" />
                    <col style="width: 35%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th>SAP Order No. <span class="text_red">*</span></th>
                        <td colspan="2">
                            <telerik:RadTextBox ID="radtxtOrderNo" runat="server" Width="98%"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <th>Division <span class="text_red">*</span></th>
                        <td colspan="2">
                            <div id="divBU" runat="server">
                                <telerik:RadButton ID="radBtnBuPH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Bu"
                                     Value="PH" Checked="false" AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="PH &amp; R" Value="PH"></telerik:RadButtonToggleState>
                                    </ToggleStates>
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnBuCH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Bu" Value="CH" Checked="false" AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="CH" Value="CH"></telerik:RadButtonToggleState>
                                    </ToggleStates>
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnBuDC" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Bu" Value="DC" Checked="false" AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="DC" Value="DC"></telerik:RadButtonToggleState>
                                    </ToggleStates>
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnBuAH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Bu" Value="AH" Checked="false" AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="AH" Value="AH"></telerik:RadButtonToggleState>
                                    </ToggleStates>
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnBuCC" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Bu" Value="CC" Checked="false" AutoPostBack="false" Visible="false" >
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="CC" Value="CC"></telerik:RadButtonToggleState>
                                    </ToggleStates>
                                </telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>고객 번호와 이름<br />
                            (Customer No. & Name) <span class="text_red">*</span>
                        </th>
                        <td colspan="2">
                            <telerik:RadTextBox ID="radtxtCustomer" runat="server" Width="90%" ReadOnly="true"></telerik:RadTextBox>
                            <telerik:RadButton ID="radBtnSearchCustomer" runat="server" AutoPostBack="false" Width="18px" Height="18px" CssClass="btn_grid" OnClientClicked="fn_OpenCustomer">
                                <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                            </telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Total Recivable</th>
                        <td colspan="2">
                            <%--<telerik:RadTextBox ID="radtxtAmount" runat="server" Width="200px"></telerik:RadTextBox>--%>
                            <telerik:RadNumericTextBox ID="radNumAmount" runat="server" Width="200px" NumberFormat-DecimalDigits="0"
                                MinValue="0" MaxValue="99999999999" Value="0" EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right" ClientEvents-OnBlur="CalculationExceeded">
                            </telerik:RadNumericTextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Incl Note :
                    <telerik:RadNumericTextBox ID="radtxtInclNote" runat="server" Width="150px" NumberFormat-DecimalDigits="0"
                        MinValue="0" MaxValue="99999999999" Value="0" EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right" ClientEvents-OnBlur="CalculationExceeded"></telerik:RadNumericTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Payment Term</th>
                        <th>Aging Day</th>
                        <th>초과일수 (Overdue)</th>
                    </tr>
                    <tr>
                        <td class="align_center">
                            <telerik:RadNumericTextBox ID="radtxtPaymentTerm" runat="server" Width="150px" NumberFormat-DecimalDigits="0" 
                                MinValue="0" MaxValue="99999999999" Value="0" EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right" ClientEvents-OnBlur="CalculationOverDue"></telerik:RadNumericTextBox></td>
                        <td class="align_center">
                            <telerik:RadNumericTextBox ID="radtxtAgingDay" runat="server" Width="150px" NumberFormat-DecimalDigits="0" 
                                MinValue="0" MaxValue="99999999999" Value="0" EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right" ClientEvents-OnBlur="CalculationOverDue"></telerik:RadNumericTextBox>
                        </td>
                        <td class="align_center">
                            <telerik:RadNumericTextBox ID="radtxtOverDue" runat="server" Width="150px" NumberFormat-DecimalDigits="0" BorderColor="White"
                                MinValue="0" MaxValue="99999999999" Value="0" EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right"></telerik:RadNumericTextBox>
                            <%--<telerik:RadButton ID="radBtnOverCAL" runat="server" Text="CAL" OnClientClicked="CalculationOverDue" AutoPostBack="false"
                                Visible="false"></telerik:RadButton>--%>
                        </td>
                    </tr>
                    <tr>
                        <th>신용한도액(Credit Limit)</th>
                        <th>Open order / 주문액(Ordered)</th>
                        <th>초과액 (Exceeded Amount)</th>
                    </tr>
                    <tr>
                        <td class="align_center">
                            <telerik:RadNumericTextBox ID="radtxtCreditLimit" runat="server" Width="150px" NumberFormat-DecimalDigits="0" BorderColor="White"
                                MinValue="0" MaxValue="99999999999" Value="0" EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right" ClientEvents-OnBlur="CalculationExceeded"></telerik:RadNumericTextBox></td>
                        <td class="align_center">
                            <telerik:RadNumericTextBox ID="radtxtOpenOrder" runat="server" Width="80px" NumberFormat-DecimalDigits="0"
                                MinValue="0" MaxValue="99999999999" Value="0" EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right" ClientEvents-OnBlur="CalculationExceeded"></telerik:RadNumericTextBox>
                            /
                            <telerik:RadNumericTextBox ID="radtxtOrdered" runat="server" Width="80px" NumberFormat-DecimalDigits="0" 
                                MinValue="0" MaxValue="99999999999" Value="0" EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right" ClientEvents-OnBlur="CalculationExceeded"></telerik:RadNumericTextBox>
                        </td>
                        <td class="align_center">
                            <telerik:RadNumericTextBox ID="radtxtExceededAmount" runat="server" Width="150px" NumberFormat-DecimalDigits="0" BorderColor="White"
                                MinValue="0" MaxValue="99999999999" Value="0" EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right"></telerik:RadNumericTextBox>
                            <%--<telerik:RadButton ID="radBtnExceededCAL" runat="server" Text="CAL" AutoPostBack="false" 
                                OnClientClicked="CalculationExceeded" Visible="false"></telerik:RadButton>--%>
                        </td>
                    </tr>
                    <tr>
                        <th>Collateral Amount is</th>
                        <td colspan="2">
                            <telerik:RadNumericTextBox ID="radtxtCollateral" runat="server" Width="150px" NumberFormat-DecimalDigits="0" 
                                MinValue="0" MaxValue="99999999999" Value="0" EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right"></telerik:RadNumericTextBox></td>
                    </tr>
                    <tr>
                        <th>Reason <span class="text_red">*</span></th>
                        <td colspan="2">
                            <div id="divReason">
                                <telerik:RadDropDownList ID="RadDropReason" runat="server" Width="600" DefaultMessage="--- Select ---" AutoPostBack="false" >
                                    <Items>
                                        
                                        <telerik:DropDownListItem Text="01 - Credit limit reviewed" Value="01" />
                                        <telerik:DropDownListItem Text="02 - Payment status / history reviewed" Value="02" />
                                        <telerik:DropDownListItem Text="03 - Payment promise made" Value="03" />
                                        <telerik:DropDownListItem Text="04 - Additional guarantees received" Value="04" />
                                        <telerik:DropDownListItem Text="05 - Payment received and not posted" Value="05" />
                                        <telerik:DropDownListItem Text="06 - Account current & within credit limit" Value="06" />
                                        <telerik:DropDownListItem Text="07 - Business decision or approval" Value="07" />
                                        <telerik:DropDownListItem Text="08 - Cash in advance payment received" Value="08" />
                                        <telerik:DropDownListItem Text="09 - Pending credit note / offsets" Value="09" />
                                        <telerik:DropDownListItem Text="10 - Pending material return" Value="10" />
                                        <telerik:DropDownListItem Text="11 - Overdue related to barter process" Value="11" />
                                        <telerik:DropDownListItem Text="12 - Other (comment required)" Value="12" />
                                        <telerik:DropDownListItem Text="13 - Obligated to ship" Value="13" />
                                        <telerik:DropDownListItem Text="14 - Keep order blocked" Value="14" />
                                        <telerik:DropDownListItem Text="15 - Pending system release" Value="15" />
                                        <telerik:DropDownListItem Text="003 - Collection plan" Value="003" Visible="false" />
                                        <telerik:DropDownListItem Text="004 - Collateral renewal" Value="004" Visible="false" />
                                        <telerik:DropDownListItem Text="005 - Cash in advance for the exceeding amounts/ Late postings by SCM / Posting error by SCM" Value="005" Visible="false" />                                        
                                        <telerik:DropDownListItem Text="006 - Overdue case due to late clearing" Value="006" Visible="false" />
                                        <telerik:DropDownListItem Text="009 - Government demand" Value="009" Visible="false" />
                                        <telerik:DropDownListItem Text="100 - Additional collateral plan / Others (needs comments)" Value="100" Visible="false" />

                                    </Items>
                                </telerik:RadDropDownList>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <h3>초과 승인 요청의 이유 및 그 해결안 (Explanation of reason & improving plan)</h3>
        <div class="doc_style2">
            <telerik:RadTextBox ID="radtxtExplanation" TextMode="MultiLine" Height="50px" Width="100%" runat="server"></telerik:RadTextBox>
        </div>
    </div>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinPopupCustomer" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer" Width="360px" Height="470px" Behaviors="Default" NavigateUrl="./CustomerList.aspx" OnClientClose="fn_ClientClose"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div id="hiddenArea" runat="server">
        <input type="hidden" id="hddProcessID" runat="server" />
        <input type="hidden" id="hddProcessStatus" runat="server" />
        <input type="hidden" id="hddDocumentID" runat="server" />
        <input type="hidden" id="hddCustomerCode" runat="server" />
        <input type="hidden" id="hddReuse" runat="server" />
        <input type="hidden" id="hddAddRow" runat="server" value="N" />
    </div>
</asp:Content>

