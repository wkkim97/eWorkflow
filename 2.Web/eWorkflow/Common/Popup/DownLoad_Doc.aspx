<%@ Page Title="Detail Download" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="DownLoad_Doc.aspx.cs" Inherits="Common_Popup_DetailSearch" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function CloseWnd(sender, args) {
            var oWnd = GetRadWindow();
            oWnd.close();
        }

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" runat="Server">
    <div id="pop_content">
        <div class="data_type1 pt20">
            <table>
                <tbody>
                    <tr>
                        <th>Document Name</th>
                    </tr>
                    <tr>
                        <td>
                           
                            <telerik:RadDropDownList ID="radListDocument" runat="server" Width="99%" DataTextField="DOC_NAME" DataValueField="DOCUMENT_ID"></telerik:RadDropDownList>
                        </td>
                    </tr>
                                       
                        <th>Period (Request Date)111</th>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadDatePicker ID="radFromDate" runat="server" DateInput-DisplayDateFormat="yyyy-MM-dd"
                                DateInput-DateFormat="yyyy-MM-dd" Width="100px" Calendar-ShowRowHeaders="false" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                            <telerik:RadDatePicker ID="radToDate" runat="server" DateInput-DisplayDateFormat="yyyy-MM-dd"
                                DateInput-DateFormat="yyyy-MM-dd" Width="100px" Calendar-ShowRowHeaders="false" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="align_center pt20">
             <telerik:RadButton ID="RadButton2" runat="server" Text="DOWNLOAD" CssClass="btn btn-blue btn-size3 bold"  
                    EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="ToggleButton"  OnClick="btnDownload_NEW_Click">

             </telerik:RadButton>
            <telerik:RadButton ID="radBtnClose" runat="server" Text="CLOSE"
                ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
                CssClass="btn btn-darkgray btn-size3 bold" OnClientClicked="CloseWnd">
            </telerik:RadButton>
            
        </div>
    </div>
</asp:Content>

