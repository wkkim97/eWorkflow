<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Sub_ReturnGoods.master" AutoEventWireup="true" CodeFile="ReturnPending.aspx.cs" Inherits="ReturnGoods_ReturnPending" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radgrdPending" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="Server">
    <h2>Pending</h2>
    <div class="returnGoods_btn">
        <ul>
            <li style="float: left; margin-right: 5px;">
                <telerik:RadButton ID="btnDownload" runat="server" Text="ExcelDownload" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-gray btn-size2 bold" OnClick="btnDownload_Click"></telerik:RadButton>
            </li>
            <li style="float: left; margin-right: 5px;">
                <span style="width: 500px; text-align: right; display: inline-block; margin-right: 3px">DIV : </span>                
            </li>            
            <li>
                <div id="divDIV" runat="server" style="display: inline-block; float: left"></div>
            </li>
            <li style="float:right">
                <telerik:RadButton ID="btnSearch" Text="Search" CssClass="btn btn-gray btn-size2 bold" ButtonType="ToggleButton" runat="server"
                    EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" OnClick="btnSearch_Click">
                </telerik:RadButton>
            </li>
        </ul>
    </div>
    <div class="returnGoods_list">
        <telerik:RadGrid ID="radgrdPending" runat="server" AllowAutomaticUpdates="true" OnNeedDataSource="radgrdPending_NeedDataSource" AutoGenerateColumns="False" AllowPaging="False" AllowSorting="true" GridLines="None"  
            Skin="EXGrid" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" OnItemDataBound="radgrdPending_ItemDataBound">
            <MasterTableView ClientDataKeyNames="IDX" ShowHeadersWhenNoRecords="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" AlternatingItemStyle-Wrap="false" TableLayout="Fixed">                
                <HeaderStyle Font-Size="10px" />
                <ItemStyle Font-Size="10px" />
                <AlternatingItemStyle Font-Size="10px" />
                <Columns>
                    <telerik:GridBoundColumn HeaderText="IDX" DataField="IDX" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="TYPE" DataField="TYPE" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="STATUS" DataField="STATUS" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="SN" DataField="SN" UniqueName="SN" HeaderStyle-Width="55px" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Date" DataField="Date" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Div" DataField="Div" UniqueName="Div" HeaderStyle-Width="40px" ReadOnly="true">
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn HeaderText="W. Spec." DataField="WHOLESALES_SPECIALIST" HeaderStyle-Width="60px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="A/R" DataField="WHOLESALES_SPECIALIST_STATUS" HeaderStyle-Width="40px">
                    </telerik:GridBoundColumn>                    
                    <telerik:GridBoundColumn HeaderText="SALES_ADMIN_CAO" DataField="SALES_ADMIN_CAO" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="W. Manager" DataField="WHOLESALES_MANAGER" HeaderStyle-Width="60px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="A/R" DataField="WHOLESALES_MANAGER_STATUS" HeaderStyle-Width="40px">
                    </telerik:GridBoundColumn>
                   <telerik:GridBoundColumn HeaderText="S. Admin" DataField="SALES_ADMIN" HeaderStyle-Width="60px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="A/R" DataField="SALES_ADMIN_STATUS" HeaderStyle-Width="40px" >
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="CUSTOMER_CODE" DataField="CUSTOMER_CODE" UniqueName="CUSTOMER_CODE" HeaderStyle-Width="80px" ReadOnly="true" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="CUSTOMER" DataField="CUSTOMER_NAME" UniqueName="CUSTOMER_NAME" ReadOnly="true" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="SHIPTO_CODE" DataField="SHIPTO_CODE" UniqueName="SHIPTO_CODE" HeaderStyle-Width="80px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="PRODUCT_CODE" DataField="PRODUCT_CODE" UniqueName="PRODUCT_CODE" HeaderStyle-Width="100px" ReadOnly="true" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="PRODUCT" DataField="PRODUCT_NAME" UniqueName="PRODUCT_NAME" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Qty" DataField="Qty" UniqueName="Qty" HeaderStyle-Width="40px" ReadOnly="true" DataFormatString="{0:N0}">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Batch" DataField="Batch" UniqueName="Batch" HeaderStyle-Width="70px" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Expiry" DataField="Expiry" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="70px" >
                    </telerik:GridBoundColumn>                    
                    <telerik:GridBoundColumn HeaderText="InvoicePrice" DataField="INVOICE_PRICE" UniqueName="INVOICE_PRICE" HeaderStyle-Width="70px" DataFormatString="{0:N0}">
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="MRP_FLAG" DataField="MRP_FLAG" Display="false">
                    </telerik:GridBoundColumn>                    
                    <telerik:GridBoundColumn HeaderText="TOTAL_AMOUNT" DataField="TOTAL_AMOUNT" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="UNIT_PRICE" DataField="UNIT_PRICE" DataFormatString="{0:0.##0.###}" HeaderStyle-Width="80px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="S1" DataField="s1"  HeaderStyle-Width="30px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="S2" DataField="s2"  HeaderStyle-Width="30px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="SAP_AMOUNT" DataField="SAP_AMOUNT" Display="false">
                    </telerik:GridBoundColumn>                                        
                    <telerik:GridBoundColumn HeaderText="REASON(Only AH)" DataField="REASON" HeaderStyle-Width="80px" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="VC_MANAGER(Only AH)" DataField="VC_MANAGER" HeaderStyle-Width="80px" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    
                </Columns>
            </MasterTableView>
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" />
            <AlternatingItemStyle HorizontalAlign="Left" />
        </telerik:RadGrid>
    </div>
    <input type="hidden" runat="server" id="hhdChkDiv" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="Server">
</asp:Content>

