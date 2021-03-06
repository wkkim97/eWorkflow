<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="BCSCreditLimitSetting.aspx.cs" Inherits="Approval_Document_BCSCreditLimitSetting" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">

    <script type="text/javascript">

        function fn_OnBgCheckedChanged(sender, args) {
            var value = sender.get_text();
            
           
           

           //setVisibleCategory(value, controls, '');
        }

        function setVisibleCategory(value, title, channel) {
            var controls = $find("<%= radBtnAllCustomer.ClientID%>").get_checked();
           
            console.log(controls);
            if (value == "CS") {
               
                $('#rowCategory').show();
                $('#rowBusinessGroup').show();
                if (controls) {
                    $('#rowCategory').hide();
                }
                
            }
            else {
                $('#rowCategory').hide();
                $('#rowBusinessGroup').hide();
            }
        }

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
                $('#rowCategory').hide();
            }
            else {
                $('#rowCustomer').show();
                if ($('#rowBusinessGroup').css("display") == "none") {
                    $('#rowCategory').hide();
                } else {
                    $('#rowCategory').show();
                };
                
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
                    <tr style="display:none">
                        <th>Division <span class="text_red">*</span></th>
                        <td>
                            <div id="divBU" runat="server" >
                                
                                <telerik:RadButton ID="radBtnBuCS" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Bu" Value="CS" Checked="true" AutoPostBack="false" OnClientClicked="fn_OnBgCheckedChanged">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="CS" Value="CS"></telerik:RadButtonToggleState>
                                    </ToggleStates>
                                </telerik:RadButton>
                                

                            </div>
                        </td>
                    </tr>
                    <tr id="rowBusinessGroup" >
                            <th>Business Group<span class="text_red">*</span></th>
                            <td>
                                <telerik:RadButton ID="radBtnSub_CP" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="SUB" Text="CP" Value="CP" Checked="false" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radBtnSub_IB" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="SUB" Text="IDS" Value="IDS" Checked="false" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radBtnSub_ES" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="SUB" Text="ES" Value="ES" Checked="false" AutoPostBack="false"></telerik:RadButton>
                            </td>
                     </tr>
                    <tr>
                        <th>Change Scope <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadButton ID="radBtnAllCustomer" runat="server" Value="All Customer" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Scope" Text="다수의 고객들을 대상으로 한 변경 (Multiple Customers)" AutoPostBack="false" OnClientCheckedChanged="OnClientCheckedChanged"></telerik:RadButton>
                            <telerik:RadButton ID="radBtnCustomer" runat="server" Value="Customer" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Scope" Text="개별 고객을 대상으로 한 변경 (Individual Customer)" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Type <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadButton ID="radBtnTypeCredit" runat="server" Value="Credit" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Type" Text="Customer Credit Limit" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radBtnTypeMortgage" runat="server" Value="Mortgage" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Type" Text="Mortgage" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>              
                    

                    
                    <tr id="rowCategory" style="display:none">
                        <th>Category</th>
                        <td>
                               
                               <telerik:RadButton ID="radBtnCategoryNew" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Category" Value="New" Checked="false" AutoPostBack="false">
                                   <ToggleStates>
                                       <telerik:RadButtonToggleState Text="New Customer Credit / Mortgage" Value="New"></telerik:RadButtonToggleState>
                                   </ToggleStates>
                               </telerik:RadButton>
                               <telerik:RadButton ID="radBtnCategoryIncrease" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Category" Value="Increase" Checked="false" AutoPostBack="false">
                                   <ToggleStates>
                                       <telerik:RadButtonToggleState Text="Increase / Decrease / Withdrawal of existing customer credit/mortgage" Value="Increase"></telerik:RadButtonToggleState>
                                   </ToggleStates>
                               </telerik:RadButton>
                           </td>
                    </tr>
   
                    <tr id="rowCustomer">
                        <th>고객 번호와 이름<br />
                            (Customer No. & Name) <span class="text_red">*</span>
                        </th>
                        <td>
                            <telerik:RadTextBox ID="radtxtCustomer" runat="server" Width="95%"></telerik:RadTextBox>
                            <telerik:RadButton ID="radBtnSearchCustomer" runat="server" AutoPostBack="false" Width="18px" Height="18px" CssClass="btn_grid" OnClientClicked="fn_OpenCustomer">
                                <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                            </telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Credit Limit Amount <span class="text_red">*</span></th>
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

