<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchBar.ascx.cs" Inherits="Common_SearchBar" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script type="text/javascript">
    function fn_chkUseSearchDate(sender, args)
    {
        fn_EnabledDateControls(args.get_checked());
    }

    function fn_PopupOpening(sender, args) {
        var chk = $find("<%=chkUseDateSearch.ClientID%>").get_checked()
        args.set_cancel(!chk);
    }

    function fn_InitControl()
    {
        fn_EnabledDateControls(false);
        $find("<%=dtStartDate.ClientID%>").set_cancel(false);;
        $find("<%=dtEndDate.ClientID%>").set_cancel(false);;
    }

    function fn_EnabledDateControls(val)
    {
        var startDate = $find("<%=dtStartDate.ClientID%>");
        var endDate = $find("<%=dtEndDate.ClientID%>");
        if (!val) {
            startDate.get_dateInput().disable();
            endDate.get_dateInput().disable();
        }
        else {
            startDate.get_dateInput().enable();
            endDate.get_dateInput().enable();
        }
    }
</script>

    <telerik:RadComboBox ID="cboSearchType" runat="server">
        <Items>
            <telerik:RadComboBoxItem Value="" Text="" Selected="true" />
            <telerik:RadComboBoxItem Value="DOC_NAME" Text="Document Name" />
            <telerik:RadComboBoxItem Value="SUBJECT" Text="Subject" />
            <telerik:RadComboBoxItem Value="REQUESTER" Text="Requester" />
        </Items>
    </telerik:RadComboBox>
    <telerik:RadTextBox ID="txtSearchText" Width="200px" runat="server"></telerik:RadTextBox>
    <telerik:RadButton ID="chkUseDateSearch" runat="server" ToggleType="CheckBox" ButtonType="ToggleButton"
        AutoPostBack="false" OnClientCheckedChanged="fn_chkUseSearchDate">
        <ToggleStates>
            <telerik:RadButtonToggleState Value="N" PrimaryIconCssClass="rbToggleCheckboxChecked"></telerik:RadButtonToggleState>
            <telerik:RadButtonToggleState Value="Y" PrimaryIconCssClass="rbToggleCheckbox"></telerik:RadButtonToggleState>
        </ToggleStates>
    </telerik:RadButton>
    <telerik:RadDatePicker ID="dtStartDate" runat="server">
          <DatePopupButton ImageUrl="/eWorks/Styles/images/ico_calendar.png" HoverImageUrl="/eWorks/Styles/images/ico_calendar.png" />
        <ClientEvents OnPopupOpening="fn_PopupOpening" />
    </telerik:RadDatePicker>
    <telerik:RadDatePicker ID="dtEndDate" runat="server">
         <DatePopupButton ImageUrl="/eWorks/Styles/images/ico_calendar.png" HoverImageUrl="/eWorks/Styles/images/ico_calendar.png" />
        <ClientEvents OnPopupOpening="fn_PopupOpening" />
    </telerik:RadDatePicker>
 
