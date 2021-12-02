<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="CostCenter.aspx.cs" Inherits="Common_Popup_CostCenter" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        //선택한 값을 부모 페이지로 넘긴다.
        function fn_ReturnToParent(sender, args) {
            var gridSelectdItems = $find('<%= radGrdCostCenter.ClientID%>').get_masterTableView().get_selectedItems();

            var item = {};
            var retVal = false;
            if (gridSelectdItems.length == 0) {
                alert("선택한 항목이 없습니다.");

            }
            else {
                var row = gridSelectdItems[0];
                item.CODE_NAME = row.get_cell("CODE_NAME").innerText;
                item.SUB_CODE = row.get_cell("SUB_CODE").innerText;
            }
            var oWnd = GetRadWindow();
            GetRadWindow().close(item);

        }

        function CloseWnd(sender, args) {
            var oWnd = GetRadWindow();
            oWnd.close();
        }

        function fn_OnRowDblClick(sender, args) {
            var gridSelectedItems = $find("<%= radGrdCostCenter.ClientID %>").get_masterTableView().get_selectedItems();
            var row = gridSelectedItems[0];
            var item = {};
            item.CODE_NAME = row.get_cell("CODE_NAME").innerHTML;
            item.SUB_CODE = row.get_cell("SUB_CODE").innerHTML;

            GetRadWindow().close(item);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radGrdCostCenter">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radGrdCostCenter" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div id="wrap">
        <div id="pop_content">
            <div class="data_type2 pt20">
                <telerik:RadGrid ID="radGrdCostCenter" runat="server" AllowPaging="true" AutoGenerateColumns="false" Skin="EXGrid" GridLines="None" Height="400px" OnNeedDataSource="radGrdCostCenter_NeedDataSource" PageSize="20">
                    <ClientSettings EnablePostBackOnRowClick="true">
                        <Selecting AllowRowSelect="true" />
                    </ClientSettings>
                    <HeaderStyle HorizontalAlign="Left" />
                    <MasterTableView ShowHeadersWhenNoRecords="true" EnableHeaderContextMenu="true">
                        <Columns>
                            <telerik:GridBoundColumn DataField="CODE_NAME" HeaderText="Cost Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SUB_CODE" HeaderText="" Display="false"></telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <ClientEvents OnRowDblClick="fn_OnRowDblClick" />
                    </ClientSettings>
                </telerik:RadGrid>
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

            <div id="hiddenArea">
                <input type="hidden" id="hddClassCode" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>

