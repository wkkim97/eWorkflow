<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="IncentivePayment.aspx.cs" Inherits="Approval_Document_IncentivePayment" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script src="../../Scripts/Approval/List.min.js"></script>
    <script type="text/javascript">
        function fn_SchemeDoc() {
            var hddSchemeProcessId = $('#<%= hddSchemeProcessId.ClientID %>').val();
            var hddSchemeDocumentID = $('#<%= hddSchemeProcessId.ClientID %>').val();
            fn_ShowDocument('IncentiveScheme.aspx', hddSchemeDocumentID, hddSchemeProcessId);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <!-- doc Style -->
    <div class="doc_style">
        <h3>Incentive Program</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col />
                    <col style="width: 75%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Subject</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtSubject" ReadOnly="true" Width="98%" CssClass="input"></telerik:RadTextBox></td>
                    </tr>

                    <tr>
                        <th>BU</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtDiv" ReadOnly="true" Width="50%" CssClass="input"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <th>Settlement Type</th>
                        <td><telerik:RadTextBox runat="server" ID="RadtxtSettlement" ReadOnly="true" Width="98%" CssClass="input"></telerik:RadTextBox></td>
                    </tr>

                    <tr>
                        <th>Document No.</th>
                        <td class="bold"><a href="#" onclick="fn_SchemeDoc();">
                            <asp:Label runat="server" ID="lbDocumentNo"></asp:Label></a></td>

                    </tr>
                </tbody>
            </table>
        </div>

        <h3>Payment Information</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col />
                    <col style="width: 75%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Total Amount</th>
                        <td>
                            <telerik:RadNumericTextBox ID="RadnuTotalAmount" Width="98%" CssClass="input" runat="server" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>No. of customers</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtDealer" Width="98%" CssClass="input"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <th>Payment Date</th>
                        <td>
                            <telerik:RadDatePicker runat="server" ID="RadDatePicker1" Calendar-ShowRowHeaders="false" Width="150px" MinDate="1900-01-01" MaxDate="2050-12-31">
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
                        <th>Payment system</th>
                        <td><%--<telerik:RadTextBox runat="server" ID="RadtxtPaymentSystem" Width="98%" CssClass="input"></telerik:RadTextBox>--%>
                                    P2R System
                        </td>
                    </tr>
                    <tr>
                        <th>Pay to</th>
                        <td><%--<telerik:RadTextBox runat="server" ID="RadtxtPayto"  Width="98%" CssClass="input"></telerik:RadTextBox>--%>
                                    Refer to payment report attached.
                        </td>
                    </tr>
                    <tr>
                        <th>Comment</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtRemark" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="HddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddSchemeProcessId" runat="server" />
    <input type="hidden" id="hddSchemeDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
	<input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

