<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="IncentiveScheme.aspx.cs" Inherits="Approval_Document_IncentiveScheme"    %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="server">
 
    <script type="text/javascript">
        function fn_DoRequest(sender, args)
        {
            return true;
        }

        function fn_OpenPayment(button, args) {
            var nWidth = 924;
            var nHeight = 680;
            var left = (screen.width / 2) - (nWidth / 2);
            var top = (screen.height / 2) - (nHeight / 2) - 10;

            var SchemeProcessId = $('#<%= hddProcessID.ClientID %>').val();
            var hddDocpaymnetId = $('#<%= hddDocpaymnetID.ClientID %>').val();
            var formName = 'IncentivePayment.aspx';
            var url = fn_GetWebRoot() + "Approval/Document/" + formName + "?SchemeProcessId=" + SchemeProcessId + "&documentid=" + hddDocpaymnetId;
            window.close();
            return window.open(url, "", "width=" + nWidth + "px, height=" + nHeight + "px, top=" + top + "px, left=" + left + "px, toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes");

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="server">
    <div class="data_type1">
        <h3>Incentive Program</h3>
        <table>
            <colgroup>
                <col />
                <col style="width: 75%;" />
            </colgroup>
            <tbody>
                <tr>
                    <th>Subject <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="txtSubject" runat="server" CssClass="input" Width="100%"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <th>BU <span class="text_red">*</span></th>
                    <td>
                        <div id="divBu" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="rdoDiv01" runat="server" ToggleType="Radio" CssClass="radio" ButtonType="ToggleButton" GroupName="rdoDiv" AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="SM" Value="SM"></telerik:RadButtonToggleState>
                                </ToggleStates>
                            </telerik:RadButton>
                            <telerik:RadButton ID="rdoDiv02" runat="server" ToggleType="Radio" CssClass="radio" ButtonType="ToggleButton" GroupName="rdoDiv" AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="HH" Value="HH"></telerik:RadButtonToggleState>
                                </ToggleStates>
                            </telerik:RadButton>
                            <telerik:RadButton ID="rdoDiv03" runat="server" ToggleType="Radio" CssClass="radio" ButtonType="ToggleButton" GroupName="rdoDiv" AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="WH" Value="WH"></telerik:RadButtonToggleState>
                                </ToggleStates>
                            </telerik:RadButton>
                            <telerik:RadButton ID="rdoDiv04" runat="server" ToggleType="Radio" CssClass="radio" ButtonType="ToggleButton" GroupName="rdoDiv" AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="R" Value="R"></telerik:RadButtonToggleState>
                                </ToggleStates>
                            </telerik:RadButton>
                            <telerik:RadButton ID="rdoDiv05" runat="server" ToggleType="Radio" CssClass="radio" ButtonType="ToggleButton" GroupName="rdoDiv" AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="AH" Value="AH"></telerik:RadButtonToggleState>
                                </ToggleStates>
                            </telerik:RadButton>
                            <telerik:RadButton ID="rdoDiv06" runat="server" ToggleType="Radio" CssClass="radio" ButtonType="ToggleButton" GroupName="rdoDiv" AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="CH" Value="CH"></telerik:RadButtonToggleState>
                                </ToggleStates>
                            </telerik:RadButton>
                            <telerik:RadButton ID="rdoDiv07" runat="server" ToggleType="Radio" CssClass="radio" ButtonType="ToggleButton" GroupName="rdoDiv" AutoPostBack="false" Visible="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="DC" Value="DC"></telerik:RadButtonToggleState>
                                </ToggleStates>
                            </telerik:RadButton>
                            <telerik:RadButton ID="rdoDiv08" runat="server" ToggleType="Radio" CssClass="radio" ButtonType="ToggleButton" GroupName="rdoDiv" AutoPostBack="false" Visible="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="CC" Value="CC"></telerik:RadButtonToggleState>
                                </ToggleStates>
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>PROGRAM <span class="text_red">*</span></th>
                    <td>
                        <div id="divPROGRAM" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="rdoPROGRAM01" runat="server" ToggleType="Radio" CssClass="radio" Text="PAP" Value="PAP" ButtonType="ToggleButton" GroupName="rdoPROGRAM" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="rdoPROGRAM02" runat="server" ToggleType="Radio" CssClass="radio" Text="Secondary Incentive" Value="Secondary Incentive" ButtonType="ToggleButton" GroupName="rdoPROGRAM" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="rdoPROGRAM03" runat="server" ToggleType="Radio" CssClass="radio" Text="BayWINS" Value="BayWINS" ButtonType="ToggleButton" GroupName="rdoPROGRAM" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="rdoPROGRAM04" runat="server" ToggleType="Radio" CssClass="radio" Text="CAP" Value="CAP" ButtonType="ToggleButton" GroupName="rdoPROGRAM" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="rdoPROGRAM05" runat="server" ToggleType="Radio" CssClass="radio" Text="Early payment" Value="Early payment" ButtonType="ToggleButton" GroupName="rdoPROGRAM" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="rdoPROGRAM06" runat="server" ToggleType="Radio" CssClass="radio" Text="Valvet" Value="Valvet" ButtonType="ToggleButton" GroupName="rdoPROGRAM" AutoPostBack="false" Visible="false"></telerik:RadButton>
                            <telerik:RadButton ID="rdoPROGRAM07" runat="server" ToggleType="Radio" CssClass="radio" Text="Discount" Value="Discount" ButtonType="ToggleButton" GroupName="rdoPROGRAM" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="rdoPROGRAM08" runat="server" ToggleType="Radio" CssClass="radio" Text="FAP" Value="FAP" ButtonType="ToggleButton" GroupName="rdoPROGRAM" AutoPostBack="false"></telerik:RadButton>
                        </div>
                    </td>
                </tr>

                <tr>
                    <th>Settlement Type <span class="text_red">*</span></th>
                    <td>
                        <div id="divSettlementType" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="rdoSettlementType01" runat="server" ToggleType="Radio" CssClass="radio" Text="AR Deduction" Value="AR Deduction" ButtonType="ToggleButton" GroupName="rdoSettlementType" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="rdoSettlementType02" runat="server" ToggleType="Radio" CssClass="radio" Text="Product" Value="Product" ButtonType="ToggleButton" GroupName="rdoSettlementType" AutoPostBack="false" Visible="false"></telerik:RadButton>
                            <telerik:RadButton ID="rdoSettlementType03" runat="server" ToggleType="Radio" CssClass="radio" Text="Cash" Value="Cash" ButtonType="ToggleButton" GroupName="rdoSettlementType" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="rdoSettlementType04" runat="server" ToggleType="Radio" CssClass="radio" Text="Invoice" Value="Invoice" ButtonType="ToggleButton" GroupName="rdoSettlementType" AutoPostBack="false"></telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Description</th>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" TextMode="MultiLine" Rows="12" CssClass="input" Width="100%" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="payment" class="align_right pb10" style="display:none" runat="server">
            <telerik:RadButton runat="server" ID="RadbtnPayment"  ButtonType="ToggleButton" OnClientClicked="fn_OpenPayment" ForeColor="White" CssClass="btn btn-blue btn-size1 bold" Width="100" Text="Payment" AutoPostBack="false"></telerik:RadButton>

            <%--<a class="btn btn-blue btn-size1 bold" href="/eWorks/Approval/Document/IncentivePayment.aspx?" onclick="fn_OpenPayment">Payment</a>    --%>
            
		</div>        
    </div> 
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" /> 
    <input type="hidden" id="hddDocpaymnetID" runat="server" /> 
    <input type="hidden" id="hddReuse" runat="server" />
	<input type="hidden" id="hddAddRow" runat="server" value="N" />

</asp:Content>
