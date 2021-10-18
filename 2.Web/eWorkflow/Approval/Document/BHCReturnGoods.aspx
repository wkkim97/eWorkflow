<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="BHCReturnGoods.aspx.cs" Inherits="Approval_Document_BHCReturnGoods" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" Runat="Server">
    <script type="text/javascript">
        function fn_ReturnGo(sender, args) {
            var CheckedBU = '';
            var controls = $('#<%= divBU.ClientID %>').children();            
            var month = $find("<%= RadtxtMonth.ClientID%>").get_textBoxValue();

            for (var i = 0; i < controls.length; i++) {
                var bu = controls[i];                                       //BU
                if ($find(bu.id).get_checked()) {
                    CheckedBU = $find(bu.id)._value;
                }
            }

            window.open("http://bkleassvr/RETURN_GOOD_REPORT/main_return_good.asp?bu=" + CheckedBU + "&update_date=" + month);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" Runat="Server">
    <div class="data_type1" >
        <h3>BHC RETRUN GOODS</h3>
        <table>
            <colgroup>
                <col />
                <col style="width:75%" />
            </colgroup>
            <tbody>
                <tr>
                    <th>BU</th>
                    <td>
                        <div id="divBU" runat="server">
                            <telerik:RadButton ID="radRdoBuWH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="WH" Value="WH" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuSM" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="SM" Value="SM" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuHH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="HH" Value="HH" AutoPostBack="false" ></telerik:RadButton>                            
                            <telerik:RadButton ID="radRdoBuCH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="CH" Value="CH" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuDC" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="DC" Value="DC" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuR" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="R" Value="R" AutoPostBack="false" ></telerik:RadButton>                            
                            <telerik:RadButton ID="radRdoBuAH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="AH" Value="AH"  AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuCC" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="CC" Value="CC" AutoPostBack="false" visible="false"></telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>The Cost for return goods</th>
                    <td>
                        <telerik:RadNumericTextBox ID="radNumAmount" runat="server" NumberFormat-DecimalDigits="0" Width="25%"
                            EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right">
                        </telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Commet</th>
                    <td>
                        <telerik:RadTextBox ID="radtxtComment" runat="server" Width="100%" TextMode="MultiLine" Height="70px"></telerik:RadTextBox>
                    </td>
                </tr>
                 <tr>
                    <th>Month of Return Goods</th>
                    <td>
                        <telerik:RadTextBox ID="RadtxtMonth" runat="server" Width="100px" MaxLength="7"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">                        
                        <div>Here is Summary report for Return Goods. Please click below button, and then you can find report form.</div>
                        <div id="divReport" runat="server">
                              <telerik:RadButton ID="radBtnReturn" runat="server" Text="Report" OnClientClicked="fn_ReturnGo" AutoPostBack="false"
                            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-blue btn-size3 bold">
                        </telerik:RadButton>
                        </div>
                      
                    </td>
                </tr>
            </tbody>
        </table>
       
    </div>
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />        
</asp:Content>

