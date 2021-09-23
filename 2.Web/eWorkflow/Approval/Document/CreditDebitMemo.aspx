<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="CreditDebitMemo.aspx.cs" Inherits="Approval_Document_CreditDebitMemo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function getSelectedBU() {
            var controls = $('#<%= divBU.ClientID %>').children();
            var selectedValue;

            for (var i = 0; i < controls.length; i++) {
                var bu = controls[i];
                if ($find(bu.id).get_checked()) {
                    selectedValue = $find(bu.id).get_value();
                    break;
                }
            }

            return selectedValue;
        }
        function fn_OnWholeSalerRequesting(sender, args) {

            var selectedValue = getSelectedBU();

            if (selectedValue) {
                var context = args.get_context();
                context["company"] = $('#<%= hddCompanyCode.ClientID %>').val();
                context["bu"] = selectedValue;
                context["parvw"] = "IE";
                context["level"] = "Y";
            } else {
                fn_OpenInformation('Please fill out below fields. <br/><hr>BU');
                args.set_cancel(true);
            }
        }

        function fn_OnProductRequesting(sender, args) {

            var selectedValue = getSelectedBU();

            if (selectedValue) {
                var context = args.get_context();
                context["company"] = $('#<%= hddCompanyCode.ClientID %>').val();
                context["bu"] = selectedValue;
                context["baseprice"] = "Y"
            } else {
                fn_OpenInformation('Please fill out below fields. <br/><hr>BU');
                args.set_cancel(true);
            }
        }

        function setVisibleControl(value) {
            if (value == "CREDIT") {
                $('#rowCreditPurpose').show();
                $('#rowDebitPurpose').hide();
            } else if (value == 'DEBIT') {
                $('#rowCreditPurpose').hide();
                $('#rowDebitPurpose').show();
            }
        }

        function fn_OnTitleCheckedChanged(sender, args) {
            if (sender.get_text() == "Credit Memo") {
                if (args.get_checked()) {
                    $('#rowCreditPurpose').show();
                    $('#rowDebitPurpose').hide();
                }
            } else {
                if (args.get_checked()) {
                    $('#rowCreditPurpose').hide();
                    $('#rowDebitPurpose').show();
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <h3>Title and Purpose of Credit & Debit memo distribution</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>

                <tr>
                    <th>Title <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadButton ID="radRdoCredit" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                            Text="Credit Memo" Value="CREDIT" AutoPostBack="false" GroupName="Title"
                            OnClientCheckedChanged="fn_OnTitleCheckedChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radRdoDebit" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                            Text="Debit Memo" Value="DEBIT" AutoPostBack="false" GroupName="Title"
                            OnClientCheckedChanged="fn_OnTitleCheckedChanged">
                        </telerik:RadButton>
                    </td>
                </tr>
                <tr id="rowCreditPurpose" style="display: none">
                    <th>Purpose <span class="text_red">*</span></th>
                    <td>
                        <div id="divCreditPurpose" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoCrePur1" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                                AutoPostBack="false" GroupName="CreditPurpose"
                                Text="MRP price cut-down(Adjustment with difference Credit and Debit in SAP)" Value="0001">
                            </telerik:RadButton>
                            <br />
                            <telerik:RadButton ID="radRdoCrePur2" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                                AutoPostBack="false" GroupName="CreditPurpose"
                                Text="Price adjustment" Value="0002">
                            </telerik:RadButton>
                            <br />
                            <telerik:RadButton ID="radRdoCrePur3" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                                AutoPostBack="false" GroupName="CreditPurpose"
                                Text="Others (Describe as below reason)" Value="0003">
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr id="rowDebitPurpose" style="display: none">
                    <th>Purpose <span class="text_red">*</span></th>
                    <td>
                        <div id="divDebitPurpose" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoDebPur1" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                                AutoPostBack="false" GroupName="DebitPurpose"
                                Text="Price adjustment" Value="0002">
                            </telerik:RadButton>
                            <br />
                            <telerik:RadButton ID="radRdoDebPur2" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                                AutoPostBack="false" GroupName="DebitPurpose"
                                Text="Others (Describe as below reason)" Value="0003">
                            </telerik:RadButton>
                        </div>

                    </td>
                </tr>
                <tr>
                    <th>BU <span class="text_red">*</span></th>
                    <td>
                        <div id="divBU" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoBuHH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="HH" Value="HH" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuWH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="WH" Value="WH" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuSM" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="SM" Value="SM" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuRI" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="R" Value="R" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuCC" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="CC" Value="CC" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuDC" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="DC" Value="DC" AutoPostBack="false"></telerik:RadButton>
                            
                            <telerik:RadButton ID="radRdoBuRMD" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="RMD" Value="RMD" AutoPostBack="false"></telerik:RadButton>
                            
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Customer Name / Number</th>
                    <td>
                        <telerik:RadAutoCompleteBox ID="radAcomWholesaler" runat="server" AllowCustomEntry="false" Width="100%" DropDownWidth="300px"
                            OnClientRequesting="fn_OnWholeSalerRequesting">
                            <WebServiceSettings Method="SearchCustomer" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                        </telerik:RadAutoCompleteBox>
                        <asp:Label ID="lblNotCustomer" runat="server" Width="100%" Visible="false">Refer to the customer list attached.</asp:Label>

                    </td>
                </tr>
                <tr>
                    <th>Product Name / Number</th>
                    <td>
                        <telerik:RadAutoCompleteBox ID="radAcomProduct" runat="server" AllowCustomEntry="false" Width="100%" DropDownWidth="400px"
                            OnClientRequesting="fn_OnProductRequesting">
                            <WebServiceSettings Method="SearchProduct" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                        </telerik:RadAutoCompleteBox>
                        <asp:Label ID="lblNotProduct" runat="server" Width="100%" Visible="false">Refer to the product list attached.</asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>Adjustment Amount <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadNumericTextBox ID="radNumAmount" runat="server" NumberFormat-DecimalDigits="0"
                            EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Left" Width="120px"
                            >
                        </telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Reason</th>
                    <td>
                        <telerik:RadTextBox ID="radTxtReason" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddCompanyCode" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />

</asp:Content>

