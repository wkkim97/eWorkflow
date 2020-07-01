<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="StrategicChangeReport.aspx.cs" Inherits="Approval_Document_StrategicChangeReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">

    <script type="text/javascript">

        function fn_RemoveZeros(sender, args) {
            var tbValue = sender._textBoxElement.value;
            if (tbValue.indexOf(".00") != -1)
                sender._textBoxElement.value = tbValue.substr(0, tbValue.indexOf(".00"));
        }

        function fn_OnClientCheckedChanged(sender, args) {
            var checkitem = sender.get_checked();
            setVisibleControl(checkitem);
        }
        // control 활성화
        function setVisibleControl(checkitem) {
            if (checkitem == true)
                $('#<%= productTb.ClientID %>').show();
            else if (checkitem == false)
                $('#<%= productTb.ClientID %>').hide();
    }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <!--doc-->
    <div class="doc_style">

        <h3>Please select a planned Strategic Change</h3>
        <div class="doc_style2">
            <div class="data_type1">
                <table>
                    <colgroup>
                        <col style="width: 25%" />
                        <col />
                    </colgroup>
                    <tr>
                        <th>Product Name <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtProduct" Width="99%"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>                
                <telerik:RadButton runat="server" ID="RadchkPrice" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Value="Price" Text=" Price dumping (price decrease more than - 15% compared to list price), tying or bundling" AutoPostBack="false" OnClientCheckedChanged="fn_OnClientCheckedChanged"></telerik:RadButton>
                <br />
                <telerik:RadButton runat="server" ID="RadchkInerruption" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Value="Interruption" Text=" Interruption of supply: sales of a dominant product decrease by at least 10% in volume" AutoPostBack="false"></telerik:RadButton>
                <br />
                <telerik:RadButton runat="server" ID="RadchkCapacity" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Value="Capacity" Text=" Capacity reductions (when leading to not fully supplying market demand) in product supply planned or to be expected" AutoPostBack="false"></telerik:RadButton>
                <br />
                <telerik:RadButton runat="server" ID="RadchkProduct" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Value="Product" Text=" Product launches, withdrawals, formulation changes, change of distribution channel, of strategic segmentation" AutoPostBack="false"></telerik:RadButton>
                <br />
                <br />
                <div id="productTb" style="display: none; width: 90%; margin-left: 17px;" runat="server">
                    <table style="width: 70%">
                        <colgroup>
                            <col />
                            <col style="width: 70%;" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th>이전 판매량</th>
                                <td>
                                    <telerik:RadNumericTextBox runat="server" CssClass="input" ID="RadtxtQty" Width="150px" NumberFormat-DecimalDigits="0" Value="0"
                                        EnabledStyle-HorizontalAlign="Right">                                        
                                    </telerik:RadNumericTextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>이전 판매금액</th>
                                <td>
                                    <telerik:RadNumericTextBox runat="server" ID="RadtxtPreSale" CssClass="input"  NumberFormat-DecimalDigits="0" Width="150px" Value="0"
                                        EnabledStyle-HorizontalAlign="Right">                                        
                                    </telerik:RadNumericTextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>변경 판매 가격</th>
                                <td>
                                    <telerik:RadNumericTextBox runat="server" ID="RadtxtChangeSale" CssClass="input"  NumberFormat-DecimalDigits="0" Width="150px" Value="0"
                                        EnabledStyle-HorizontalAlign="Right">                                        
                                    </telerik:RadNumericTextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>묶어팔기</th>
                                <td>
                                    <telerik:RadButton runat="server" ID="RadchkTie" ButtonType="ToggleButton" ToggleType="CheckBox" Value="Product" Text="Yes" AutoPostBack="false"></telerik:RadButton>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <strong>Pupose and Justification</strong>
                <telerik:RadTextBox runat="server" TextMode="MultiLine" Height="80px" ID="RadtxtPurpose" Width="97%"></telerik:RadTextBox>
            </div>
        </div>
    </div>
    <!--doc-->
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
	<input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

