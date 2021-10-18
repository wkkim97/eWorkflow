<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Sub_ReturnGoods.master" AutoEventWireup="true" CodeFile="Statistics.aspx.cs" Inherits="ReturnGoods_Statistics" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" Runat="Server">
    <script type="text/javascript">
        function fn_Reload(sender, args)
        {
            $find('<%=RadAjaxManager1.ClientID%>').ajaxRequest("reload");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" Runat="Server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest"> 
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdReturnGoods" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdReturnGoods">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdReturnGoods" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radListYear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdReturnGoods" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radListMonth">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdReturnGoods" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" Runat="Server">
    <h2>Statistics</h2>
    <div class="returnGoods_btn">
        <ul>
            <li style="float: left; margin-right: 5px;">
                <telerik:RadDropDownList ID="radListYear" runat="server" Width="60px" OnSelectedIndexChanged="radListYear_SelectedIndexChanged" AutoPostBack="true"></telerik:RadDropDownList>년
            </li>
            <li style="float: left; margin-right: 5px;">
                <telerik:RadDropDownList ID="radListMonth" runat="server" Width="60px" OnSelectedIndexChanged="radListYear_SelectedIndexChanged" AutoPostBack="true"></telerik:RadDropDownList>월
            </li>
            <%--<li style="float: left; margin-right: 5px;">
                <telerik:RadButton ID="radRdoType1" runat="server" GroupName="Type" ButtonType="ToggleButton" ToggleType="Radio" Text="All" Value="A" OnClientCheckedChanged="fn_Reload" Checked="true" AutoPostBack="false"></telerik:RadButton>
            </li>--%>
            <li style="float: left; margin-right: 5px;">
                <telerik:RadButton ID="radRdoType2" runat="server" GroupName="Type" ButtonType="ToggleButton" ToggleType="Radio" Text="Completed" Value="S" OnClientCheckedChanged="fn_Reload" AutoPostBack="false"></telerik:RadButton>
            </li>
            <li style="float: left; margin-right: 5px;">
                <telerik:RadButton ID="radRdoType3" runat="server" GroupName="Type" ButtonType="ToggleButton" ToggleType="Radio" Text="Reject" Value="R" OnClientCheckedChanged="fn_Reload" AutoPostBack="false"></telerik:RadButton>
            </li>
            <li style="float: right; margin-left: 5px;">
                <telerik:RadButton ID="btnDownload" runat="server" Text="ExcelDownload" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-gray btn-size2 bold" OnClick="btnDownload_Click"></telerik:RadButton>
            </li>
        </ul>
    </div>
    <div class="returnGoods_list">
        <telerik:RadGrid ID="grdReturnGoods" runat="server" OnItemDataBound="grdReturnGoods_ItemDataBound" OnNeedDataSource="grdReturnGoods_NeedDataSource" AutoGenerateColumns="False" AllowPaging="False" AllowSorting="true" GridLines="None"
            Skin="EXGrid" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false">
            <MasterTableView ShowHeadersWhenNoRecords="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" AlternatingItemStyle-Wrap="false" TableLayout="Fixed">
                <Columns>
                    <telerik:GridBoundColumn HeaderText="IDX" DataField="IDX" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="TYPE" DataField="TYPE" HeaderStyle-Width="40px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="STATUS" DataField="STATUS" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="SN" DataField="SN" HeaderStyle-Width="40px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Date" DataField="Date" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-Width="80px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Div" DataField="Div" HeaderStyle-Width="40px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="CUSTOMER_CODE" DataField="CUSTOMER_CODE" UniqueName="CUSTOMER_CODE" HeaderStyle-Width="150px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="CUSTOMER_NAME" DataField="CUSTOMER_NAME" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="SHIPTO_CODE" DataField="SHIPTO_CODE" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="PRODUCT_CODE" DataField="PRODUCT_CODE" HeaderStyle-Width="150px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="PRODUCT_NAME" DataField="PRODUCT_NAME" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="MRP_FLAG" DataField="MRP_FLAG" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Qty" DataField="Qty" HeaderStyle-Width="80px" DataFormatString="{0:#.##0.###}">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Batch" DataField="Batch" HeaderStyle-Width="80px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Expiry" DataField="Expiry" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-Width="80px">
                    </telerik:GridBoundColumn>                    
                    <telerik:GridBoundColumn HeaderText="INVOICE_PRICE" DataField="INVOICE_PRICE" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="TOTAL_AMOUNT" DataField="TOTAL_AMOUNT" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="UNIT_PRICE" DataField="UNIT_PRICE" DataFormatString="{0:#.##0.###}" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="SAP_AMOUNT" DataField="SAP_AMOUNT" DataFormatString="{0:#.##0.###}" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>                    
                    <telerik:GridBoundColumn HeaderText="S1" DataField="s1" HeaderStyle-Width="40px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="S2" DataField="s2" HeaderStyle-Width="40px">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="REASON(Only AH)" DataField="REASON" HeaderStyle-Width="80px" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="VC_MANAGER(Only AH)" DataField="VC_MANAGER" HeaderStyle-Width="80px" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="SALES_ADMIN" DataField="SALES_ADMIN" Display="false">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="SALES_ADMIN_STATUS" DataField="SALES_ADMIN_STATUS" Display="false">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="SALES_ADMIN_CAO" DataField="SALES_ADMIN_CAO" Display="false">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="WHOLESALES_MANAGER" DataField="WHOLESALES_MANAGER" Display="false">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="WHOLESALES_MANAGER_STATUS" DataField="WHOLESALES_MANAGER_STATUS" Display="false">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="WHOLESALES_SPECIALIST" DataField="WHOLESALES_SPECIALIST" Display="false">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="WHOLESALES_SPECIALIST_STATUS" DataField="WHOLESALES_SPECIALIST_STATUS" Display="false">
                    </telerik:GridBoundColumn> 
                </Columns>
            </MasterTableView>
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" />
            <AlternatingItemStyle HorizontalAlign="Left" />
        </telerik:RadGrid>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" Runat="Server">
</asp:Content>

