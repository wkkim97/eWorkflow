<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="EarlyPaymentDiscount.aspx.cs" Inherits="Approval_Document_EarlyPaymentDiscount" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <h3>Accroding to the agreement conclude with the customer, we would like to request your approval as below.</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Total Amount <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadNumericTextBox ID="radNumTotalAmount" runat="server" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr>
                    <th>No. of dealers <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtDealersNo" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Payment Date <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadDatePicker ID="radDatPayment" runat="server" Width="100px" Calendar-ShowRowHeaders="false">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                        </telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
                    <th>Payment system</th>
                    <td>P2R sysem(Deduction from AR)</td>
                </tr>
                <tr>
                    <th>Pay to</th>
                    <td>Refer to payment report attached</td>
                </tr>
                <tr>
                    <th>Division</th>
                    <td>AH</td>
                </tr>
            </table>
        </div>
    </div>
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />

</asp:Content>

