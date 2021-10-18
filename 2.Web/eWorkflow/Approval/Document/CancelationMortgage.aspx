<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="CancelationMortgage.aspx.cs" Inherits="Approval_Document_CancelationMortgage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function fn_OpenCustomerWin(sender, args) {
            var wnd = $find("<%= RadWinPopupCustomer.ClientID %>");
            wnd.setUrl("/eWorks/Common/Popup/CustomerList.aspx");
            wnd.show();
            sender.set_autoPostBack(false);
        }

        function fn_ClientClose(oWnd, args) {
            var item = args.get_argument();

            if (item != null) {
                var txtcustomer = $find("<%= radTxtCustomer.ClientID%>");
                txtcustomer.set_value(item.CUSTOMER_NAME.trim() + " (" + item.CUSTOMER_CODE + ")");
                $('#<%= hddCustomerCode.ClientID %>').val(item.CUSTOMER_CODE);
            }
            else {
                oWnd.close();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%" />
                    <col />
                </colgroup>
                <tr>
                    <th>Business Unit <span class="text_red">*</span></th>
                    <td>
                        <div id="divBU" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoCP" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="CP" Value="CP" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoIS" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="IS" Value="IS" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoES" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="ES" Value="ES" AutoPostBack="false" ></telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr id="rowType" >
                    <th>Type <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadButton runat="server" ID="RadTypeNew" ButtonType="ToggleButton" ToggleType="Radio" Text="New" Value="New" GroupName="Category"></telerik:RadButton>
                        <telerik:RadButton runat="server" ID="RadTypeReturn" ButtonType="ToggleButton" ToggleType="Radio" Text="Return" Value="Return" GroupName="Category" ></telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <th>Customer <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtCustomer" runat="server" ReadOnly="true" Width="96%"></telerik:RadTextBox>
                        <telerik:RadButton ID="radBtnCustomer" runat="server" AutoPostBack="false" CssClass="btn_grid" Width="18px" Height="18px" 
                            OnClientClicked="fn_OpenCustomerWin">
                            <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                        </telerik:RadButton>
                        <input type="hidden" id="hddCustomerCode" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>Reason <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtReason" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Mortgage Address <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radTxtMortgage" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Mortgage Amount <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadNumericTextBox ID="radNumAmount" runat="server" NumberFormat-DecimalDigits="0"
                            EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right" MinValue="0" IncrementSettings-InterceptMouseWheel="false">
                        </telerik:RadNumericTextBox></td>
                </tr>
            </table>
        </div>
    </div>
    <%--Popup--%>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="RadWinPopupCustomer" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer"
                Width="430px" Height="600px" Behaviors="Default" Modal="true" OnClientClose="fn_ClientClose" NavigateUrl="./CustomerList.aspx">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />

</asp:Content>

