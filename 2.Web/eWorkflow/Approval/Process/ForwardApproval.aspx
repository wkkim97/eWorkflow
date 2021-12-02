<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="ForwardApproval.aspx.cs" Inherits="Approval_Process_ForwardApproval" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script src="/eWorks/Scripts/Common/InterFaseService.js"></script>
    <script type="text/javascript">
        function fn_CancelClick(sender, args) {
            var oArg = new Object();
            oArg.returnValue = false;
            var oWnd = GetRadWindow();
            GetRadWindow().BrowserWindow.location.href = GetRadWindow().BrowserWindow.location.href;
            if (oArg.returnValue) {
                oWnd.close(oArg);
            }
            else
                oWnd.close();
        }

        function fn_Selected(sender, args)
        {
            var grid = $find("<%=grdUserList.ClientID %>");
            var masterView = grid.get_masterTableView();
            fn_setUserNameBox(grid, masterView.get_selectedItems()[0]._itemIndex);
        }

        function fn_gridDbClick(sender, args)
        {
            var grid = $find("<%=grdUserList.ClientID %>");
            var masterView = grid.get_masterTableView();
            fn_setUserNameBox(grid, sender._selectedIndexes);
        }

        function fn_setUserNameBox(grid, selIndex)
        {
            var view = grid.get_masterTableView();
            var userid = getDataItemKeyValue(grid, view.get_dataItems()[selIndex], 'USER_ID');
            var userName = getDataItemKeyValue(grid, view.get_dataItems()[selIndex], 'FULL_NAME');
            document.getElementById("<%= hddUserID.ClientID %>").value = userid;
            $find("<%= txtUserName.ClientID %>").set_value(userName); 
        }

        function getDataItemKeyValue(radGrid, item, colName) {
            return radGrid.get_masterTableView().getCellByColumnUniqueName(item, colName).innerHTML;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" runat="Server">
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>

    <telerik:RadAjaxManager runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdUserList">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdUserList" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtSearchName"  />
                    <telerik:AjaxUpdatedControl ControlID="grdUserList" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
 
        </AjaxSettings>
    </telerik:RadAjaxManager> 

    <div class="data_type1">
        <table>
            <tbody>
                <tr>
                    <th>Employees Search</th>
                </tr>
                <tr>
                    <td>
                        <div style="padding-top: 10px;">
                            <span>
                                <span style="float: left; margin-left: 5px;margin-top:2px;">Search</span>
                                <span style="float: left; margin-left: 5px;margin-bottom:4px">
                                    <telerik:RadTextBox runat="server" ID="txtSearchName"  Width="200" CssClass="input"></telerik:RadTextBox></span>
                                <span style="float: left; margin-left: 5px;">
                                    <telerik:RadButton runat="server" ID="btnSearch" CssClass="btn btn-blue btn-size1 bold" ButtonType="LinkButton"  EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"  OnClick="btnSearch_Click" Text="Search" Height="18px"></telerik:RadButton>
                                </span>
                                <span style="float: left; margin-left: 5px;">
                                    <telerik:RadButton runat="server" ID="btnSelect" CssClass="btn btn-green btn-size1 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"  OnClientClicked="fn_Selected" Text="Select" Height="18px"></telerik:RadButton>
                                </span>
                            </span>
                        </div>
                        <div style="clear: left; padding-top: 5px;">
                            <telerik:RadGrid runat="server" ID="grdUserList" AllowPaging="true" AllowSorting="true" AllowMultiRowSelection="false" AutoGenerateColumns="false" GridLines="None" OnNeedDataSource="grdUserList_NeedDataSource" Skin="EXGrid">
                                <MasterTableView ShowHeadersWhenNoRecords="true" DataKeyNames="FULL_NAME">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="USER_ID" HeaderText="USER_ID" HeaderStyle-Width="20%"  ></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="FULL_NAME" HeaderText="NAME" ></telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings>
                                    <Selecting AllowRowSelect="true" />
                                    <ClientEvents OnRowDblClick="fn_gridDbClick" />
                                </ClientSettings>
                                <PagerStyle PageSizeControlType="None" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </telerik:RadGrid>
                        </div>
                    </td>
                </tr> 
                <tr>
                    <th>Approval Target User Name</th>
                </tr>
                <tr>
                    <td><telerik:RadTextBox TextMode="SingleLine" Width="100%" ID="txtUserName" ReadOnly="true" runat="server"></telerik:RadTextBox></td>
                </tr>
                <!--eWorkflow Optimization START-->
                <tr>
                    <th>Comment</th>
                </tr>
                <tr>
                    <td>
                        <telerik:RadTextBox TextMode="MultiLine" Height="110" Width="100%" ID="txtComment" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <!--eWorkflow Optimization END-->
            </tbody>
        </table>

    </div>
    <div class="align_center pt20">
        <telerik:RadButton ID="btnForward" CssClass="btn btn-blue btn-size3 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" OnClick="btnForward_Click" Text="Forward Approval" runat="server"></telerik:RadButton>
        <telerik:RadButton ID="btnCancel" CssClass="btn btn-darkgray btn-size3 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" OnClientClicked="fn_CancelClick" Text="Cancel" runat="server"></telerik:RadButton>
    </div>
     
    <input id="hddProcessID" type="hidden" runat="server" />
    <input id="hddDocumentID" type="hidden" runat="server" />
    <input id="hddProcessType" type="hidden" runat="server" />
    <input id="hddUserID" type="hidden" runat="server" />
</asp:Content>

