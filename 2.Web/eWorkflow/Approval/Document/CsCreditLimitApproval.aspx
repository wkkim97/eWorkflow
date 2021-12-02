<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="CsCreditLimitApproval.aspx.cs" Inherits="Approval_Document_CsCreditLimitApproval" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">

    <script type="text/javascript">

        function fn_DoRequest(sender, args) {
            return true;
        }

        function fn_DoSave(sender, args) {
            return true;
        }

        function OnClientCheckedChanged(sender, eventArgs) {
            var checked = sender.get_checked();
            if (checked)
                setVisibleControl('Y');
            else
                setVisibleControl('N');
        }

        function setVisibleControl(isAll) {
            if (isAll == 'Y') {
                $('#rowCustomer').hide();
            }
            else {
                $('#rowCustomer').show();
            }
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
                txtcustomer.set_value(item.CUSTOMER_NAME.trim() + " (" + item.CUSTOMER_CODE + ")");
                $('#<%=hddCustomerCode.ClientID%>').val(item.CUSTOMER_CODE);
            }
            else {
        oWnd.close();
    }
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
                    <col style="width: 35%;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Type <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadButton ID="radBtnAllCustomer" runat="server" Value="All Customer" ButtonType="ToggleButton" ToggleType="Radio" GroupName="CustomerType" Text="All Customer" AutoPostBack="false" OnClientCheckedChanged="OnClientCheckedChanged"></telerik:RadButton>
                            <telerik:RadButton ID="radBtnCustomer" runat="server" Value="Customer" ButtonType="ToggleButton" ToggleType="Radio" GroupName="CustomerType" Text="Customer" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Business Group <span class="text_red">*</span></th>
                        <td>
                            <div id="divBU" runat="server">
                                <telerik:RadButton ID="radBtnBuCP" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Bu" Value="CP" Checked="false" AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="CP" Value="PH"></telerik:RadButtonToggleState>
                                    </ToggleStates>
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnBuIB" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Bu" Value="IB" Checked="false" AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="IB" Value="IB"></telerik:RadButtonToggleState>
                                    </ToggleStates>
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnBuES" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Bu" Value="ES" Checked="false" AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="ES" Value="ES"></telerik:RadButtonToggleState>
                                    </ToggleStates>
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnBuBVS" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Bu" Value="BVS" Checked="false" AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="BVS" Value="BVS"></telerik:RadButtonToggleState>
                                    </ToggleStates>
                                </telerik:RadButton>
                              </div>
                        </td>
                    </tr>
                    <tr id="rowCustomer">
                        <th>고객 번호와 이름<br />
                            (Customer code & name) <span class="text_red">*</span>
                        </th>
                        <td>
                            <telerik:RadTextBox ID="radtxtCustomer" runat="server" Width="95%"></telerik:RadTextBox>
                            <telerik:RadButton ID="radBtnSearchCustomer" runat="server" AutoPostBack="false" Width="18px" Height="18px" CssClass="btn_grid" OnClientClicked="fn_OpenCustomer">
                                <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                            </telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Requested Credit Limit <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadNumericTextBox ID="radtxtAmount" runat="server" Width="200px" NumberFormat-DecimalDigits="0"
                                EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right" ></telerik:RadNumericTextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <h3>Description</h3>
        <div class="doc_style2">
            <telerik:RadTextBox ID="radtxtDescription" TextMode="MultiLine" Height="90px" Width="100%" runat="server"></telerik:RadTextBox>
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

