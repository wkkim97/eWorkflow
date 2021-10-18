<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="BCSProcessChange.aspx.cs" Inherits="Approval_Document_BCSProcessChange" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <style type="text/css">
        .check .rbText{
            text-align:left !important ;
        }
    </style>
    <script type="text/javascript">
        function GetSelectedCategory() {
            var controls = $('#<%= divCategory.ClientID %>').children();
            var selectedValue;

            for (var i = 0; i < controls.length; i++) {
                var bu = controls[i];
                if ($find(Cateogy.id).get_checked()) {
                    selectedValue = $find(Cateogy.id).get_value();
                    break;
                }
            }

            return selectedValue;
        }

        function GetSelectedPurposeType() {
            var controls = $('#<%= divPurposeType.ClientID %>').children();
            var selectedValue;

            for (var i = 0; i < controls.length; i++) {
                var bu = controls[i];
                if ($find(PurposeType.id ).get_checked()) {
                    selectedValue = $find(PurposeType.id).get_value();
                    break;
                }
            }

            return selectedValue;
        }

        function GetSelectedDurationType() {
            var selectedValue='';

            if ($find('<%= radRdoDuration_PER.ClientID%>').get_checked())
                selectedValue = $find('<%= radRdoDuration_PER.ClientID%>').get_value();
            else if ($find('<%= radRdoDuration_TEP.ClientID%>').get_checked())
                selectedValue = $find('<%= radRdoDuration_TEP.ClientID%>').get_value();

            return selectedValue;
        }

        function fn_OnDurationChanged(sender, args) {

            var duration = sender.get_text();
            if (duration == 'Permanent') {
                $('#<%= divFromTo.ClientID %>').hide();  
            } else if (duration == 'Temporary') {
                $('#<%= divFromTo.ClientID %>').show();
            }

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <h3>Request of change</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>

                <tr>
                    <th>Title <span class="text_red">*</span></th>
                    <td colspan="2">
                        <telerik:RadTextBox ID="radTextTitle" runat="server" Width="100%" TextMode="MultiLine" Height="20px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Category <span class="text_red">*</span></th>
                    <td colspan="2">
                        <div id="divCategory" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton runat="server" ID="radChkCategory1" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Active and Passive toll"         Value="Active and Passive toll"          width="170px" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkCategory2" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Artwork or Design"               Value="Artwork or Design"                width="170px" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkCategory3" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Formulation recipe & process"    Value="Formulation recipe & process"     width="170px" AutoPostBack="false"></telerik:RadButton>
                            <br />
                            <telerik:RadButton runat="server" ID="radChkCategory4" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="New product"                     Value="New product"                      width="170px" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkCategory5" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Packaging material"              Value="Packaging material"               width="170px" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkCategory6" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="SOP or Document"    Value="SOP or Document"     width="170px" AutoPostBack ="false"></telerik:RadButton>
                            <br />
                            <telerik:RadButton runat="server" ID="radChkCategory7" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Supplier, Manufacturer and Site" Value="Supplier, Manufacturer and Site"  width="170px" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkCategory8" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Tech and raw material"           Value="Tech and raw material"            width="170px" AutoPostBack="false"></telerik:RadButton>
                            
                            <telerik:RadButton runat="server" ID="radChkCategory10" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Legal or Compliance (LTO)"    Value="Legal or Compliance (LTO)"     width="170px" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkCategory11" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Organization"    Value="Organization"     width="170px" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radChkCategory9" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Testing and operation method of equipment (MoC)"    Value="Testing and operation method of equipment (MoC)"     width="340px" AutoPostBack="false"></telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Purpose <span class="text_red">*</span></th>
                    <td colspan="2">
                        <div id="divPurposeType" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoPurpose_NEW" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Purpose" Text="NEW"          Value="NEW"         AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoPurpose_IMP" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Purpose" Text="Improvement"  Value="Improvement" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoPurpsoe_COM" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Purpose" Text="Compliance"   Value="Compliance"  AutoPostBack="false"></telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Duration <span class="text_red">*</span></th>
                    <td>
                        <div id="divDuration" runat="server" style="width: 70%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoDuration_PER" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Duration" Text="Permanent" Value="Permanent" OnClientCheckedChanged="fn_OnDurationChanged" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoDuration_TEP" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Duration" Text="Temporary" Value="Temporary" OnClientCheckedChanged="fn_OnDurationChanged" AutoPostBack="false"></telerik:RadButton>
                        </div>
                        <div id="divFromTo" runat="server" style="display:none" > 
                            <telerik:RadDatePicker runat="server" ID="RadDateFrom" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR"
                                MinDate="1900-01-01" MaxDate="2050-12-31" >
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                                ~
                            <telerik:RadDatePicker runat="server" ID="RadDateTo" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR"
                                MinDate="1900-01-01" MaxDate="2050-12-31" >
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Brief Description <span class="text_red">*</span></th>
                    <td colspan="2">
                        <telerik:RadTextBox ID="radTextBrief_Description" runat="server" Width="100%" TextMode="MultiLine" Height="50px" ></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Effect before / after <span class="text_red">*</span></th>
                    <td colspan="2">
                        <telerik:RadTextBox ID="radTextEffect_Before_After" runat="server" Width="100%" TextMode="MultiLine" Height="50px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Affected product <span class="text_red">*</span><br />
                        (Product name & Pack size)
                    </th>
                    <td colspan="2">
                        <telerik:RadTextBox ID="radTextAffectedProduct" runat="server" Width="100%" TextMode="MultiLine" Height="50px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Proposed Due Date <span class="text_red">*</span></th>
                    <td colspan="2">
                        <telerik:RadDatePicker runat="server" ID="RadDateProposedDue" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR"
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



