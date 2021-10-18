<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Sub_ReturnGoods.master" AutoEventWireup="true" CodeFile="ReturnGoods.aspx.cs" Inherits="ReturnGoods_ReturnGoods" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" Runat="Server">
    <script type="text/javascript">
        function batchEditOpening(sender, args) {
            var cellValue = args.get_cell().innerText;
            
            if (args.get_columnUniqueName() == "PRODUCT_NAME")
            {
                //PRODUCT_NAME 이 빈값일 경우 수정 가능
                args.set_cancel(true);
            }
            else if (args.get_columnUniqueName() == "INVOICE_PRICE")
            {
                //MRP_FLAG 가 "Y"가 아닐 경우 수정 불가
                if (args.get_row().cells[6].innerText == "AH" || args.get_row().cells[12].innerText == "Y")
                    args.set_cancel(true);
            }
        }

        function fn_UpdateType(sender, args)
        {
            $("#<%= hhdUpdateType.ClientID %>").val(args.get_commandArgument());
            var list = [];
            var grid = $find('<%= grdReturnGoods.ClientID %>');                          
            var masterTable = grid.get_masterTableView();
            var dataItems = $find('<%= grdReturnGoods.ClientID %>').get_masterTableView().get_dataItems();

            for (var i = 0; i < dataItems.length; i++) {
                var CheckedItem = masterTable.get_dataItems()[i].findElement("chkbox");

                if (CheckedItem.checked == true) {

                    
                    var idx = dataItems[i].get_cell("IDX").innerText;
                    var Ctrltype = dataItems[i].findControl('radGrdDdlType');
                    var type = null;
                    if (Ctrltype)
                        type = Ctrltype._selectedText;
                    else
                        type = dataItems[i].get_cell("TYPE").children[0].innerText.trim();
                    var status = dataItems[i].get_cell("STATUS").children[0].innerText.trim();
                    var shiptoCode = dataItems[i].get_cell("SHIPTO_CODE").innerText.trim();
                    var invoicePrice = dataItems[i].get_cell("INVOICE_PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                    var wManager = dataItems[i].get_cell("WHOLESALES_MANAGER").innerText.trim();                    
                    var wManagerS = dataItems[i].get_cell("WHOLESALES_MANAGER_STATUS").innerText;

                    var wSpecialist = dataItems[i].get_cell("WHOLESALES_SPECIALIST").innerText.trim();
                    var wManagerSS = dataItems[i].get_cell("WHOLESALES_SPECIALIST_STATUS").innerText;

                    var sAdmin = dataItems[i].get_cell("SALES_ADMIN").innerText.trim();
                    var sAdminS = dataItems[i].get_cell("SALES_ADMIN_STATUS").innerText;

                    var rgood = {
                        IDX : null,
                        TYPE: null,
                        STATUS : null,
                        SHIPTO_CODE: null,                        
                        INVOICE_PRICE: null,
                        WHOLESALES_MANAGER : null,
                        WHOLESALES_MANAGER_STATUS: null,
                        WHOLESALES_SPECIALIST: null,
                        WHOLESALES_SPECIALIST_STATUS: null,
                        SALES_ADMIN: null,
                        SALES_ADMIN_STATUS:null
                    }
                    rgood.IDX = idx;
                    rgood.TYPE = type;
                    rgood.STATUS = status;
                    rgood.SHIPTO_CODE = shiptoCode;
                    rgood.INVOICE_PRICE = invoicePrice;
                    rgood.WHOLESALES_MANAGER = wManager;
                    rgood.WHOLESALES_MANAGER_STATUS = wManagerS;
                    rgood.WHOLESALES_SPECIALIST = wSpecialist;
                    rgood.WHOLESALES_SPECIALIST_STATUS = wManagerSS;
                    rgood.SALES_ADMIN = sAdmin;
                    rgood.SALES_ADMIN_STATUS = sAdminS;
                    list.push(rgood);
                }
            }
            $('#<%= hhdCheckItem.ClientID%>').val(JSON.stringify(list));


           // grid.get_batchEditingManager().saveChanges(grid.get_masterTableView(), args.get_commandArgument());
        }


        function fn_OnTypeSelectedChanged(sender, args) {
            var row = $find('<%= grdReturnGoods.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            if (row) {
                var dataItem = $find(row.id);
                if (dataItem) {
                    dataItem.get_cell('TYPE').innerText = args.get_item().get_value();
                }
            }
        }

        function changeEditor(sender, args) {
            var grid = $find("<%=grdReturnGoods.ClientID%>");
            if (sender.id == fn_GetFullID("headerChkbox"))
            {
                var btnValue = sender.checked;
                var masterTable = grid.get_masterTableView();
                var rows = masterTable.get_dataItems();
                for (i = 0; i < rows.length; i++)
                {
                    //grid.get_batchEditingManager().changeCellValue(rows[i].get_cell("CHECKBOX"), sender.checked);
                    var gridItemElement = masterTable.get_dataItems()[i].findElement("chkbox");
                    if (btnValue) 
                        gridItemElement.checked = true;                                            
                    else 
                        gridItemElement.checked = false;
                }
            }
            //else
            //{
            //    grid.get_batchEditingManager().openCellForEdit(sender);
            //    sender.checked = !sender.checked;
            //    grid.get_batchEditingManager()._tryCloseEdits(document.body);
            //}
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" Runat="Server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"> 
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdReturnGoods" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdReturnGoods">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hhdUpdateType" />
                    <telerik:AjaxUpdatedControl ControlID="grdReturnGoods" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnApproval">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hhdUpdateType" />
                    <telerik:AjaxUpdatedControl ControlID="grdReturnGoods" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnReject">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hhdUpdateType" />
                    <telerik:AjaxUpdatedControl ControlID="grdReturnGoods" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" Runat="Server">
    <h2>Return Goods</h2>
    <div class="returnGoods_btn">
        <ul>
            <li style="float: right; margin-left: 5px;">
                <telerik:RadButton ID="btnReject" runat="server" Text="REJECT" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-gray btn-size2 bold" CommandArgument="R" OnClientClicked="fn_UpdateType" OnClick="btnApproval_Click"></telerik:RadButton>
            </li>
            <li style="float: left; margin-left: 5px;">
                <telerik:RadButton ID="btnApproval" runat="server" Text="APPROVAL" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-gray btn-size2 bold" CommandArgument="A" OnClientClicked="fn_UpdateType" OnClick="btnApproval_Click"></telerik:RadButton>
            </li>
        </ul>
    </div>
    <div class="returnGoods_list">
        <telerik:RadGrid ID="grdReturnGoods" runat="server" AllowAutomaticUpdates="true" OnItemDataBound="grdReturnGoods_ItemDataBound" OnNeedDataSource="grdReturnGoods_NeedDataSource" AutoGenerateColumns="False" AllowPaging="False" AllowSorting="true" GridLines="None"
            Skin="EXGrid" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false">
            <MasterTableView EditMode="Batch" ClientDataKeyNames="IDX" ShowHeadersWhenNoRecords="true" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" AlternatingItemStyle-Wrap="false" TableLayout="Fixed">
                  <HeaderStyle Font-Size="9px" />
                <ItemStyle Font-Size="9px" />
                <AlternatingItemStyle Font-Size="9px" />
                <BatchEditingSettings EditType="Cell" OpenEditingEvent="Click" />
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="CHECKBOX" HeaderStyle-Width="35px" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="headerChkbox" runat="server" onclick="changeEditor(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" id="chkbox" runat="server" onclick="changeEditor(this)" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <input type="checkbox" id="chkbox" runat="server" onclick="changeEditor(this)" />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="IDX" DataField="IDX" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="TYPE" DataField="TYPE" UniqueName="TYPE" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <%# Eval("TYPE")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDropDownList ID="radGrdDdlType" runat="server" Width="100%" OnClientItemSelected="fn_OnTypeSelectedChanged">
                                <Items>
                                    <telerik:DropDownListItem Text="R" Value="R" />
                                    <telerik:DropDownListItem Text="E" Value="E" />
                                </Items>
                            </telerik:RadDropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="STATUS" DataField="STATUS" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="SN" DataField="SN" UniqueName="SN" HeaderStyle-Width="50px" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Date" DataField="Date" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Div" DataField="Div" UniqueName="Div" HeaderStyle-Width="40px" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="CUSTOMER_CODE" DataField="CUSTOMER_CODE" UniqueName="CUSTOMER_CODE" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="CUSTOMER" DataField="CUSTOMER_NAME" UniqueName="CUSTOMER_NAME" ReadOnly="true" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="SHIPTO_CODE" DataField="SHIPTO_CODE" UniqueName="SHIPTO_CODE" HeaderStyle-Width="60px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="PRODUCT_CODE" DataField="PRODUCT_CODE" UniqueName="PRODUCT_CODE" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="PRODUCT" DataField="PRODUCT_NAME" UniqueName="PRODUCT_NAME" ReadOnly="true" HeaderStyle-Width="220px" ItemStyle-HorizontalAlign="Left">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Qty" DataField="Qty" UniqueName="Qty" HeaderStyle-Width="40px" ReadOnly="true" DataFormatString="{0:0.##}">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Batch" DataField="Batch" UniqueName="Batch" HeaderStyle-Width="60px" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Expiry" DataField="Expiry" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="80px" ReadOnly="true" >
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="MRP_FLAG" DataField="MRP_FLAG" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="S1" DataField="s1"  HeaderStyle-Width="30px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="S2" DataField="s2"  HeaderStyle-Width="30px">
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderText="Price(Invoice)" DataField="INVOICE_PRICE" UniqueName="INVOICE_PRICE" HeaderStyle-Width="60px" DataFormatString="{0:N0}" Display="false">
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="TOTAL_AMOUNT" DataField="TOTAL_AMOUNT" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="UNIT_PRICE" DataField="UNIT_PRICE" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="SAP_AMOUNT" DataField="SAP_AMOUNT" Display="false">
                    </telerik:GridBoundColumn>                                        
                    <telerik:GridBoundColumn HeaderText="REASON(Only AH)" DataField="REASON" HeaderStyle-Width="50px" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="VC_MANAGER(Only AN)" DataField="VC_MANAGER" HeaderStyle-Width="50px" ReadOnly="true">
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
            <ClientSettings>
                <Selecting AllowRowSelect="true" />
                <ClientEvents OnBatchEditOpening="batchEditOpening" />
            </ClientSettings>
        </telerik:RadGrid>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" Runat="Server">
    <input type="text" runat="server" id="hhdUpdateType" style="display:none;" />
    <input type="text" runat="server" id="hhdCheckItem" style="display:none;" />
</asp:Content>

