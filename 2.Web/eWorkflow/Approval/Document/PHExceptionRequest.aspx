<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="PHExceptionRequest.aspx.cs" Inherits="Approval_Document_PHExceptionRequest" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">

        function pageLoad() {
            var addRow = $('#<%= hddAddRow.ClientID %>').val();
            <%-- $('#<%= exceptioncase.ClientID %>').show();    
             $('#<%= subsidycase.ClientID %>').hide(); 
            if (addRow == 'Y') {
                var grid = $find('<%=radGrdSampleItemList.ClientID%>');

                if (grid.get_masterTableView()) {
                    var dataItems = grid.get_masterTableView().get_dataItems();
                    if (dataItems.length > 0) {
                        grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                    }
                }
                $('#<%= hddAddRow.ClientID %>').val('Y');
            }--%>
        }
        
        function fn_OpenCustomer(sender, args) {
            <%--var NHCcheck = $find('<%= radRdoNH.ClientID%>').get_checked();
            var FMCcheck = $find('<%= radRdoFM.ClientID%>').get_checked();
            if (!NHCcheck && !FMCcheck) {
                fn_OpenDocInformation("Distribution Channel 을 선택하여 주세요");
                return;
            }--%>
             var controls = $('#<%= divBG.ClientID %>').children();
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

                wnd.setUrl("/eWorks/Common/Popup/CustomerList.aspx?bu=" + selectedValue + "&parvw=" + parvw + "&creditlimit=N&level=Y");
                wnd.show();
                sender.set_autoPostBack(false);
            }
            else {
                fn_OpenDocInformation('Please Select a BU');
                sender.set_autoPostBack(false);
            }
         }

        function fn_setCustomer(oWnd, args) {
            var item = args.get_argument();

            if (item != null) {
                
                var txt = $find("<%= radGrdTxtCustomer.ClientID %>");
                txt.set_value(item.CUSTOMER_NAME.trim() + "(" + item.CUSTOMER_CODE + ")") ;
                $('#<%= hddCustomerCode.ClientID %>').val(item.CUSTOMER_CODE);
                $('#<%= hddCustomerType.ClientID %>').val(item.PARVW);
                //$('#<%= Labelcustomertype.ClientID %>').text($('#<%= hddCustomerType.ClientID %>').val());
                
                $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("removegrid");
            }
            else
                return false;
        }
       
        var clickedKey = null;
       

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <h3></h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                
                
                <tr>
                    <th>Division or BU <span class="text_red">*</span></th>
                    <td>
                        <div id="divBG" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoBUR" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="Radiology" Checked="true" Value="R" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBUSM" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="SM" Value="SM" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBUHH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="HH" Value="HH" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBUWH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="WH"  Value="WH" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBUCH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="CH"  Value="CH" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBUAH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="AH"  Value="AH" AutoPostBack="false" ></telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Exception Reason </th>
                    <td>
                        <telerik:RadDropDownList ID="radDropReason" runat="server" Width="300px" DefaultMessage="--- Select ---" >
                            <Items>
                                <telerik:DropDownListItem Text="Discounts and rebate (product)" Value="Discounts and rebate (product)" />
                                <telerik:DropDownListItem Text="Discounts and rebate (Customer)" Value="Discounts and rebate (Customer)" />
                                <telerik:DropDownListItem Text="Payment terms change" Value="Payment terms change" />
                                <telerik:DropDownListItem Text="Urgent customer credit note" Value="Urgent customer crdit note" />
                                <telerik:DropDownListItem Text="Exception on Credit Policy" Value="Exception on Credit Policy" />
                                <telerik:DropDownListItem Text="Others" Value="Others" />
                            </Items>
                        </telerik:RadDropDownList>    
                    </td>
                </tr>
                <tr>
                    <th>TITLE <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="RadTxtTitle" runat="server"  Width="90%">
                        </telerik:RadTextBox>
                
                    </td>
                </tr>
                <tr>
                    <th>Customer Name </th>
                    <td>
                        <telerik:RadTextBox ID="radGrdTxtCustomer" runat="server" ReadOnly="true" AutoPostBack="false" Width="90%">
                        </telerik:RadTextBox>
                        <telerik:RadButton ID="radGrdBtnCustomer" runat="server" CssClass="btn_grid" AutoPostBack="false" Width="18px" Height="18px" OnClientClicked="fn_OpenCustomer">
                            <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                        </telerik:RadButton>
                        <div style="display:none;width:100%">
                            <asp:Label ID="Labelcustomertype" runat="server" ></asp:Label>
                        </div>
                    </td>
                </tr>
                 <tr>
                     <th>Product</th>
                     <td>
                         <telerik:RadTextBox ID="RadTextProduct" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                       
                     </td>
                 </tr>
                <tr>
                    <th>Backgroud </th>
                    <td>
                        <telerik:RadTextBox ID="RadTextBackground" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Proposal </th>
                    <td>
                        <telerik:RadTextBox ID="RadTextProposal" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Process </th>
                    <td>
                        <telerik:RadTextBox ID="RadTextProcess" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Financial impact </th>
                    <td>
                        <telerik:RadTextBox ID="RadTextFinancial" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <th>Comment </th>
                    <td>
                        <telerik:RadTextBox ID="RadTextExceptionComment" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                    </td>
                </tr>
                
                </table>


    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
       
         <Windows>
            <telerik:RadWindow ID="radWinPopupCustomer" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer" Width="550px" Height="500px" Behaviors="Default" NavigateUrl="./customerforcs.aspx" OnClientClose="fn_setCustomer"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
        </div>



    </div>
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddCompanyCode" runat="server" />
    <input type="hidden" id="hddCustomerCode" runat="server" />
    <input type="hidden" id="hddCustomerType" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
    <input type="hidden" id="hddGridItems" runat="server" />
    <input type="hidden" id="hddNH" runat="server" />
</asp:Content>

