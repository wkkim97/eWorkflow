<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="IncidentReport.aspx.cs" Inherits="Approval_Document_IncidentReport" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" Runat="Server">
    <style type="text/css">
    .check .rbText{
        text-align:left !important ;
    }
    </style>
    <script type="text/javascript">
        function fn_OnEncryptChanged(sender, args) {

            var Encrypt = sender.get_text();

            if (Encrypt == 'Yes') {
                $('#<%= divExplain.ClientID %>').hide();  
            } else if (Encrypt == 'No') {
                $('#<%= divExplain.ClientID %>').show();  
            }
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" Runat="Server">

    <div class="data_type1">

      <h3>A. Details of Incident</h3>

      <table>
            <colgroup>
                <col />
                <col style="width: 75%;" />
            </colgroup>
            <tbody>
                <tr>
                    <th> a) Date Incident occurred <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadDatePicker runat="server" ID="RadDateOccur" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR"
                            MinDate="1900-01-01" MaxDate="2050-12-31" >
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                </SpecialDays>
                            </Calendar>
                        </telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
                    <th> b) Date Incident detected <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadDatePicker   runat="server" ID="RadDateDetect" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR"
                            MinDate="1900-01-01" MaxDate="2050-12-31" >
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                </SpecialDays>
                            </Calendar>
                        </telerik:RadDatePicker>

                    </td>
                </tr>
                <tr>
                    <th> c) Incident location <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="RadTextLocation" runat="server" CssClass="input" Width="100%" ></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th> d) General description <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="RadTexDescription" TextMode="MultiLine" Rows="2"  runat="server" CssClass="input" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th> e) Media/Device type,<span class="text_red">*</span> <br/>&nbsp;&nbsp;&nbsp;&nbsp; if applicable, check all that apply</th>
                    <td>
                        <div id="divDeviceType" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton runat="server" ID="RadBtnPaper" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox"  Text="Paper (fax, mail, etc.)" Value="PAPER"  width="200px" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="RadBtnLaptop" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Laptop, PC"              Value="LAPTOP" width="200px" AutoPostBack="false"></telerik:RadButton>
                            <br />
                            <telerik:RadButton runat="server" ID="RadBtnMobile" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Company Mobile" Value="MOBILE" width="200px" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="RadBtnIpad" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox"   Text="Company iPad"   Value="IPAD"   width="200px" AutoPostBack="false"></telerik:RadButton>
                            <br />
                            <telerik:RadButton runat="server" ID="RadBtnEmail" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox"  Text="Email"                    Value="EMAIL"  width="200px" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="RadBtnOffice" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Office access card / key" Value="OFFICE" width="200px"  AutoPostBack="false"></telerik:RadButton>
                            <br />
                            <telerik:RadButton runat="server" ID="RadBtnStorage" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Portable data storage (USB,CD,HardDisk)" Value="STORAGE"  AutoPostBack="false"></telerik:RadButton>
                            <br />
                                <div id="divEncrypt" runat="server" style="width: 100%; margin: 0 0 0 0">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Was the portable storage device encrypted? 
                                    <telerik:RadButton runat="server" ID="RadRadYes" ButtonType="ToggleButton" ToggleType="Radio" GroupName="EncryptInit" Text="Yes" Value="Y" OnClientCheckedChanged="fn_OnEncryptChanged" AutoPostBack="false"></telerik:RadButton>
                                    <telerik:RadButton runat="server" ID="RadRadNo"  ButtonType="ToggleButton" ToggleType="Radio" GroupName="EncryptInit" Text="No"  Value="N" OnClientCheckedChanged="fn_OnEncryptChanged" AutoPostBack="false"></telerik:RadButton>
                                    <br />
                                </div>
                                <div id="divExplain" runat="server" style="width: 100%; margin: 0 0 0 0">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;If No, explain
                                    <telerik:RadTextBox ID="RadTextExplain" runat="server" CssClass="input" Width="525px"></telerik:RadTextBox>
                                </div>

                            <telerik:RadButton runat="server" ID="RadBtnOthers" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Others" Value="OTHERS"  AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadTextBox ID="RadTextOthers" runat="server" CssClass="input" Height ="20px" Width="551px"></telerik:RadTextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th> f) The costs associated with <br/> &nbsp;&nbsp;&nbsp;&nbsp;resolving this incident </th>
                    <td>
                         <telerik:RadNumericTextBox ID="RadNumCost" Width="98%" CssClass="input" runat="server" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
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

