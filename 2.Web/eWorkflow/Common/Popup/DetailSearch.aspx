<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="DetailSearch.aspx.cs" Inherits="Common_Popup_DetailSearch" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function CloseWnd(sender, args) {
            var oWnd = GetRadWindow();
            oWnd.close();
        }

        function fn_ReturnToParent(sender, args) {
            var listSelectdItems = $find('<%= radListDocument.ClientID%>').get_checkedItems();
            var documentIDs = '';
            for (var i = 0; i < listSelectdItems.length; i++) {
                var item = listSelectdItems[i];
                
                var value = item.get_value();
                documentIDs += value + ',';
            }
            var txtSubject = $find('<%= radTxtSubject.ClientID%>').get_value();
            var fromdate = $find('<%= radFromDate.ClientID%>').get_selectedDate();
            var todate = $find('<%= radToDate.ClientID%>').get_selectedDate();

            var item = {};
            var retVal = false;

            item.DOCUMENT_ID = documentIDs;
            item.SUBJECT = txtSubject;
            item.FROM_DATE = fromdate;
            item.TO_DATE = todate;
            var oWnd = GetRadWindow();
            GetRadWindow().close(item);

            //if (listSelectdItems.length == 0) {
            //    alert("선택한 항목이 없습니다.");
            //}
            //else {                
            //}
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
                            <telerik:RadListBox ID="radListDocument" runat="server" Width="99%" Height="300px" CheckBoxes="true" ShowCheckAll="true" DataTextField="DOC_NAME" DataValueField="DOCUMENT_ID">
                            </telerik:RadListBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Subject</th>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox ID="radTxtSubject" runat="server" Width="99%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Period</th>
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
            <telerik:RadButton ID="radBtnOk" runat="server" Text="OK"
                ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
                CssClass="btn btn-blue btn-size3 bold" OnClientClicked="fn_ReturnToParent">
            </telerik:RadButton>
            <telerik:RadButton ID="radBtnClose" runat="server" Text="CLOSE"
                ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
                CssClass="btn btn-darkgray btn-size3 bold" OnClientClicked="CloseWnd">
            </telerik:RadButton>
        </div>
    </div>
</asp:Content>

