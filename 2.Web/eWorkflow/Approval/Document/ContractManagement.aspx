<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="ContractManagement.aspx.cs" Inherits="Approval_Document_ContractManagement" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
<script type="text/javascript">

function pageLoad() {

    //alert("START");
    $('#divPrivacyInformation').hide();

    var value = '';
    if ($find('<%= radRdoPrivacyInformationYes.ClientID %>').get_checked())
        value = $find('<%= radRdoPrivacyInformationYes.ClientID %>').get_value();
    else if ($find('<%= radRdoPrivacyInformationNo.ClientID %>').get_checked())
        value = $find('<%= radRdoPrivacyInformationNo.ClientID %>').get_value();

    if (value != '')
    {
        $('#divPrivacyInformation').show();
    }
}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <!-- doc Style -->
    <div class="doc_style">
        <div class="data_type1">
        <h3>ConRADS</h3>
            <table>
                <colgroup>
                    <col style="width: 27%;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Contract Type<br />(표준계약서 양식 사용유무) <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadButton runat="server" ID="radRdoStandard" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Standard" Value="Standard" GroupName="ContractType" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoNonStandard" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Non-Standard" Value="Non" GroupName="ContractType" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Essential Contract<br />(중요계약-150억 이상) <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadButton runat="server" ID="radRdoEssYes" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Yes" Value="Y" GroupName="Essiontial" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoEssNo" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="No" Value="N" GroupName="Essiontial" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Intragroup<br />(바이엘 계열사간 계약) <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadButton runat="server" ID="radRdoIntraYes" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Yes" Value="Y" GroupName="Intragroup" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoIntraNo" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="No" Value="N" GroupName="Intragroup" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Cross-Border<br />(국외계약) <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadButton runat="server" ID="radRdoCrossYes" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Yes" Value="Y" GroupName="Cross" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoCrossNo" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="No" Value="N" GroupName="Cross" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Contract Category <span class="text_red">*</span></th>
                        <td colspan="3">
                            <telerik:RadButton runat="server" ID="radRdoCategory1" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Purchase Order (구매팀만 사용 가능)" Value="PO" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoCategory2" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="TradeMark/IP-related (특허 / 상품 관련)" Value="IP" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoCategory3" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Clinical trial related (임상시험 관련)" Value="CTR" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoCategory5" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Alliance contract" Value="Alliance" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoCategory4" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Others (그 외)" Value="Others" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                         </td>
                    </tr>

                </tbody>
            </table>
        </div>
        <div id="divPrivacyInformation" class="data_type1" style="display: none">
            <table>
                <colgroup>
                    <col style="width: 27%" />
                    <col />
                </colgroup>
                <tbody>
                <tr>
                    <th>Contracts related privacy information
                        (개인정보 관련된 계약인 경우) </th>
                    <td>
                        <telerik:RadButton runat="server" ID="radRdoPrivacyInformationYes" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Yes" Value="Y" GroupName="PrivacyInformatio" AutoPostBack="false"></telerik:RadButton>
                        <telerik:RadButton runat="server" ID="radRdoPrivacyInformationNo" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="No" Value="N" GroupName="PrivacyInformatio" AutoPostBack="false"></telerik:RadButton>
                    </td>
                </tr>
                 </tbody>              
            </table>
        </div>


        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 27%;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Title<br />(계약제목) <span class="text_red">*</span></th>
                        <td >
                            <telerik:RadTextBox runat="server" ID="radTxtTitle" Width="90%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Term<br />(계약기간) <span class="text_red">*</span></th>
                        <td >
                            <telerik:RadDatePicker ID="radDateFrom" runat="server" Width="100px" Calendar-ShowRowHeaders="false" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            </telerik:RadDatePicker>
                            ~
                            <telerik:RadDatePicker ID="radDateTo" runat="server" Width="100px" Calendar-ShowRowHeaders="false" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <th>Contract Partner<br />(계약상대방) <span class="text_red">*</span></th>
                        <td >
                            <telerik:RadTextBox runat="server" ID="radTxtContractPartner" Width="90%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Contract Value<br />(KRW)계약금액 <span class="text_red">*</span></th>
                        <td >
                            <telerik:RadNumericTextBox ID="radTxtContractValue" runat="server" Width="50%" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <!-- //doc Style -->
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

