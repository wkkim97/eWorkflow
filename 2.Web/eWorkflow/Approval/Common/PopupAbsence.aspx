<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="PopupAbsence.aspx.cs" Inherits="Approval_Common_PopupAbsence" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">

        //save
        function fn_SaveAbsence(sender, args) {
            var selectedApprover = $find('<%= radDduserlist.ClientID %>').get_selectedItem();
            var txtfromDate = $find('<%= radFromDate.ClientID%>').get_selectedDate();
            var txttoDate = $find('<%= radToDate.ClientID%>').get_selectedDate();

            var message = '';

            if (!selectedApprover) {
                message += "Approver";
            }
            if (!(txtfromDate && txttoDate)) {
                message += (message.length = 0 ? "" : ",") + "Period";
            }
            if (Date.parse(txtfromDate) > Date.parse(txttoDate)) {
                message += (message.length = 0 ? "" : ",") + "The Second date must be after the first one";
            }
            if (message.length > 0) {
                fn_OpenDocInformation(message);
                args.set_cancel(true);
            }
            else
                args.set_cancel(false);
        }

        function fn_DeleteAbsence(sender, args) {
            var selectedApprover = $find('<%= radDduserlist.ClientID %>').get_selectedItem();
            if (!selectedApprover) {
                fn_OpenDocInformation("Select a Approver, First")
                args.set_cancel(true);
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" runat="Server">
    <div id="container" runat="server" style="float: left; margin-right: 13px">
        <telerik:RadGrid ID="RadAbsenceList" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="true" OnSelectedIndexChanged="RadAbsenceList_SelectedIndexChanged"
            OnNeedDataSource="RadAbsenceList_NeedDataSource" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" GridLines="None" Width="260px">
            <ClientSettings EnablePostBackOnRowClick="true">
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
            <MasterTableView>
                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderText="Index" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="APPROVER_ID" HeaderText="ApproerID" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FROM_DATE" HeaderText="From Date" DataFormatString="{0:yyyy/MM/dd}"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TO_DATE" HeaderText="To Date" DataFormatString="{0:yyyy/MM/dd}"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="APPROVER_NAME" HeaderText="Delegation to"></telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>
    <div class="doc_style" style="float: left; width: 350px">
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 30%;" />
                    <col />
                </colgroup>
                <tbody>
                    <div id="btnArea" runat="server">
                        <telerik:RadButton ID="radBtnReset" runat="server" Text="RESET" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton"
                            CssClass="btn btn-blue btn-size1 bold" OnClick="radBtnReset_Click" Visible="false">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnSave" runat="server" Text="SAVE" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton"
                            CssClass="btn btn-blue btn-size1 bold" OnClientClicking="fn_SaveAbsence" OnClick="radBtnSave_Click" Height="20px">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnDelete" runat="server" Text="DELETE" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton"
                            CssClass="btn btn-blue btn-size1 bold" OnClientClicking="fn_DeleteAbsence" OnClick="radBtnDelete_Click" Height="20px">
                        </telerik:RadButton>
                    </div>
                    <br />
                    <tr>
                        <th>Delegation to</th>
                        <td>
                            <telerik:RadDropDownList ID="radDduserlist" runat="server" DataTextField="FULL_NAME" DataValueField="USER_ID" DefaultMessage="--- Select ---" Width="90%" DropDownWidth="215px"></telerik:RadDropDownList>
                            <%--<telerik:RadTextBox ID="RadtxtApprover" runat="server" ReadOnly="true"  Width="90%"></telerik:RadTextBox>
                            <telerik:RadButton ID="radBtnSearch" runat="server" AutoPostBack="false" Width="18px" Height="18px" CssClass="btn_grid" OnClientClicked="fn_OpenUserList">
                                <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                            </telerik:RadButton>--%>
                        </td>
                    </tr>
                    <tr>
                        <th>Period</th>
                        <td>
                            <telerik:RadDatePicker ID="radFromDate" runat="server" DateInput-DisplayDateFormat="yyyy-MM-dd" Culture="ko-KR"
                                DateInput-DateFormat="yyyy-MM-dd" Width="100px" Calendar-ShowRowHeaders="false">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            </telerik:RadDatePicker>
                            ~
                            <telerik:RadDatePicker ID="radToDate" runat="server" DateInput-DisplayDateFormat="yyyy-MM-dd" Culture="ko-KR"
                                DateInput-DateFormat="yyyy-MM-dd" Width="100px" Calendar-ShowRowHeaders="false">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            </telerik:RadDatePicker>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <th>Description</th>
                        <td>
                            <%--<telerik:RadTextBox runat="server" ID="radtxtDescription" Width="80%"></telerik:RadTextBox></td>--%>
                            <telerik:RadTextBox ID="radtxtDescription" TextMode="MultiLine" Height="50px" Width="80%" runat="server"></telerik:RadTextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <%--<telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinPopupUser" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="UserList" Width="500px" Height="350px" Behaviors="Default" NavigateUrl="./UserList.aspx" OnClientClose="fn_ClientClose"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>--%>

    <input type="hidden" id="hddIndex" runat="server" />
    <input type="hidden" id="hddApproverId" runat="server" />
    <input type="hidden" id="hddApproverName" runat="server" />
</asp:Content>

