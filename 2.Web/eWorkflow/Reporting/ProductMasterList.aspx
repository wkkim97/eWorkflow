<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="ProductMasterList.aspx.cs" Inherits="Reporting_ProductMasterList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function FormLoad() {
            fn_InitControl();
        }

        function onEnter(e) {
            var theKey = 0;
            e = (window.event) ? event : e;
            theKey = (e.keyCode) ? e.keyCode : e.charCode;
            if (theKey == "13") {
                $find("<%=radBtnSearch.ClientID %>").click();
            }
        }

        function fn_UpdateType(sender, args) {

            var grid = $find('<%= GrdProductMaster.ClientID %>');
            grid.get_batchEditingManager().saveChanges(grid.get_masterTableView(), args.get_commandArgument());
        }

        function fn_OnGridNumBlur(sender) {
            setNumberFormat(sender);
        }
        
    	function fn_OpenNewProduct(sender, args) {

    		var wnd = $find("<%= radWinNewProduct.ClientID %>");
    		var parvw = "IE";
    		wnd.setUrl("/eWorks/Common/Popup/PopupNewProduct.aspx");
    		wnd.show();
    	}
        function fn_Delete(sender, args) {
            var masterTable = $find('<%= GrdProductMaster.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var rowindex = sender.get_element().parentNode.parentNode.rowIndex;
            if (dataItems.length > 0) {
                var code = dataItems[rowindex - 1].get_cell("PRODUCT_CODE").innerText.trim();
                var bu = dataItems[rowindex - 1].get_cell("BU").innerText.trim();

                var VISIBILITY = dataItems[rowindex - 1].get_cell("VISIBILITY").innerText.trim();
                clickedKey = code + "|" + bu;
                if (VISIBILITY == "Y") {
                    fn_OpenConfirm('Do you want to remove this Item ?', confirmDelete);
                } else {
                    fn_OpenConfirm('Do you want to relive this Item ?', confirmRelive);
                }


            }

        }
        function confirmDelete(arg) {
            if (arg) {
                //  fn_UpdateGridData(false);
                var masterTable = $find('<%= GrdProductMaster.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }
        function confirmRelive(arg) {
            if (arg) {
                //  fn_UpdateGridData(false);
                var masterTable = $find('<%= GrdProductMaster.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Relive", clickedKey);
            }
        }

        <%--        function fn_buCheck(sender, args) {
            var CheckedValue = '';
            var controls = $('#<%= divBU.ClientID %>').children();
            for (var i = 0; i < controls.length; i++) {
                var bu = controls[i];                                       //BU 
                if ($find(bu.id).get_checked()) {
                    CheckedValue += $find(bu.id)._value + ',';
                }
            }
            $('#<%= hhdChkBu.ClientID%>').val(CheckedValue);            
        }--%>
    </script>
    <script src="/eWorks/Scripts/Approval/List.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radBtnUpdate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="GrdProductMaster" LoadingPanelID="loadingPanel" />
                </UpdatedControls>                
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radBtnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="GrdProductMaster" LoadingPanelID="loadingPanel" />
                </UpdatedControls>                
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radBtnCompany01">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divBU" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radBtnCompany02">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divBU" />
                </UpdatedControls>
            </telerik:AjaxSetting>  
            <telerik:AjaxSetting AjaxControlID="radBtnCompany03">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divBU" />
                </UpdatedControls>
            </telerik:AjaxSetting>                          
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="Server">
    <div id="content">
        <h2>Product Master</h2>        
        <div style="display: inline-block; width: 100%">
            <div id="divBuArea" runat="server" style="display: inline-block; width: 100%">
                <ul>
                    <li style="float: left; margin-right: 4px;display:none">
                        <span style="width: 70px; text-align: right; display: inline-block; margin-right: 3px">Company : </span>
                        <div id="divCompany" runat="server" style="display: inline-block">
                            <telerik:RadButton ID="radBtnCompany01" runat="server" Text="0963" Value="0963"
                                ButtonType="ToggleButton" ToggleType="CheckBox" OnCheckedChanged="radBtnCompany01_CheckedChanged">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnCompany02" runat="server" Text="0695" Value="0695"
                                ButtonType="ToggleButton" ToggleType="CheckBox" OnCheckedChanged="radBtnCompany01_CheckedChanged">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnCompany03" runat="server" Text="1117" Value="1117"
                                ButtonType="ToggleButton" ToggleType="CheckBox" OnCheckedChanged="radBtnCompany01_CheckedChanged">
                            </telerik:RadButton>
                        </div>
                    </li>
                    <li style="float: left; margin-right: 4px;display:none">
                        <span style="width: 70px; text-align: right; display: inline-block; margin-right: 3px">BU : </span>
                    </li>
 
                    <li style="float: left; margin-right: 4px;">
                        <span style="width: 70px; text-align: left; display: inline-block; margin-right: 3px">BU</span>
                        <telerik:RadDropDownList ID="radDropDown_BU" runat="server" Width="100%" DropDownWidth="60px">
                                    <Items>
                                        <telerik:DropDownListItem Text="CP" Value="CP" Selected="true" />
                                        <telerik:DropDownListItem Text="ES" Value="ES"  />
                                        <telerik:DropDownListItem Text="IS" Value="IS" />
                                        <telerik:DropDownListItem Text="BVS" Value="BVS" />
                                        <telerik:DropDownListItem Text="ALL" Value="ALL"  />
                                    </Items>
                        </telerik:RadDropDownList>
                    </li>

                    <li style="float: left; margin-right: 4px;">
                        <span style="width: 70px; text-align: left; display: inline-block; margin-right: 3px">VISIBILITY </span>
                        <telerik:RadDropDownList ID="RadDropDown_VISIBILITY" runat="server" Width="100%" DropDownWidth="60px">
                                    <Items>
                                        <telerik:DropDownListItem Text="Y" Value="Y" selected="true" />
                                        <telerik:DropDownListItem Text="N" Value="N" />                                        
                                        <telerik:DropDownListItem Text="ALL" Value="ALL"  />
                                    </Items>
                        </telerik:RadDropDownList>
                    </li>
                  
                     <li style="float: left; margin-right: 4px;width:150px;">
                        <span style="width: 70px; text-align: left; display: inline-block; margin-right: 3px"> Key Word </span>
                        <telerik:RadTextBox ID="radTxtKeyword" runat="server" Width="100%" ClientEvents-OnKeyPress="onEnter"></telerik:RadTextBox>
                    </li>
                    <li style="float: left; margin-right: 4px;">
                        <telerik:RadButton ID="radBtnSearch" runat="server" Text="Search"
                                ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-gray btn-size2 bold"
                                Height="42px" Width="80px" Font-Size="16px"
                                OnClick="radBtnSearch_Click" AutoPostBack="true" >
                            </telerik:RadButton>
                    </li>
                    <li style="float: left; margin-right: 4px;">
                        <telerik:RadButton ID="RadButton" runat="server" Text="Update" ButtonType="LinkButton" 
                            EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-gray btn-size2 bold" 
                            Height="42px" Width="80px"  Font-Size="16px"
                            AutoPostBack="false" OnClientClicked="fn_UpdateType">
                        </telerik:RadButton>
                    </li>
                    
                    <li style="float: left; margin-right: 4px;">
                        <telerik:RadButton ID="radBtnNew" runat="server" Text="New" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" 
                            EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size2 bold" 
                            Height="42px" Width="80px" Font-Size="16px"
                            AutoPostBack="false" OnClientClicked="fn_OpenNewProduct">
                            </telerik:RadButton>
                    </li>   
                    <li style="clear:both;"></li>

                    <li style="clear:both;"></li>
                </ul>
            </div>
            
        </div>        
        <div class="board_list pt20">
            <telerik:RadGrid ID="GrdProductMaster" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="true" GridLines="None" Skin="EXGrid" Height="400px"
                EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" 
                OnBatchEditCommand  ="GrdProductMaster_BatchEditCommand" 
                HeaderStyle-HorizontalAlign="Left"  
                OnNeedDataSource="GrdProductMaster_NeedDataSource" 
                OnItemDataBound="GrdProductMaster_ItemDataBound"
                OnItemCommand="GrdProductMaster_ItemCommand" >
                <MasterTableView EditMode="Batch" ShowHeadersWhenNoRecords="true" DataKeyNames="PRODUCT_CODE, PRODUCT_NAME, COMPANY_CODE" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" AlternatingItemStyle-Wrap="false">
                    <HeaderStyle Font-Size="10px" />
                    <ItemStyle Font-Size="10px" />
                    <AlternatingItemStyle Font-Size="10px" />
                    <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Product<br/>Code" DataField="PRODUCT_CODE" UniqueName="PRODUCT_CODE" ReadOnly="true" HeaderStyle-Width="80px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Product Name" DataField="PRODUCT_NAME" UniqueName="PRODUCT_NAME" HeaderStyle-Width="250px">
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="COMPANY_CODE" DataField="COMPANY_CODE" UniqueName="COMPANY_CODE" Display="false">
                        </telerik:GridBoundColumn>


                        <telerik:GridTemplateColumn DataField="PRODUCT_NAME_KR" HeaderText="Product name<br/>Korea" UniqueName="PRODUCT_NAME_KR" HeaderStyle-Width="100px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("PRODUCT_NAME_KR")%></ItemTemplate>
                            <EditItemTemplate>                                 
                                <asp:TextBox ID="radGrdPRODUCT_NAME_KR" runat="server" Width="100%" CssClass="input align_right">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>  


                        <telerik:GridTemplateColumn DataField="BU" HeaderText="BU" UniqueName="BU" HeaderStyle-Width="50px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("BU")%></ItemTemplate>
                            <EditItemTemplate>                                 
                                <asp:TextBox ID="radGrdBu" runat="server" Width="100%" CssClass="input align_right">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>                        
                        <telerik:GridTemplateColumn DataField="BASE_PRICE" HeaderText="Base<br/>Price" UniqueName="BASE_PRICE"  HeaderStyle-Width="70px">
                            <ItemStyle HorizontalAlign="Right"  />
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate>                                
                                <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("BASE_PRICE")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtbaseprice" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                                         
                        <telerik:GridTemplateColumn DataField="MARGIN" HeaderText="Margin" UniqueName="MARGIN"  HeaderStyle-Width="50px">
                            <ItemStyle HorizontalAlign="Right"  />
                            <ItemTemplate>                                
                                <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("MARGIN")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtMargin" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn DataField="INVOICE_PRICE" HeaderText="Invoice<br/>Price(R)" UniqueName="INVOICE_PRICE"  HeaderStyle-Width="70px">
                            <ItemStyle HorizontalAlign="Right"  />
                            <ItemTemplate>                                
                                <asp:Label ID="Label3" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("INVOICE_PRICE")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtInvoiceprice" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                            </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn DataField="NET1_PRICE" HeaderText="Invoice<br/>Price(WS)" UniqueName="NET1_PRICE"  HeaderStyle-Width="70px">
                            <ItemStyle HorizontalAlign="Right"  />
                            <ItemTemplate>                                
                                <asp:Label ID="Label4" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("NET1_PRICE")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtNet1" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn DataField="NET2_PRICE" HeaderText="Net2<br/>Price" UniqueName="NET2_PRICE"  HeaderStyle-Width="70px">
                            <ItemStyle HorizontalAlign="Right"  />
                            <ItemTemplate>                                
                                <asp:Label ID="Label5" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("NET2_PRICE")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtNet2" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn DataField="INVOICE_PRICE_NH" HeaderText="Invoice<br/>Price NH" UniqueName="INVOICE_PRICE_NH"  HeaderStyle-Width="70px">
                            <ItemStyle HorizontalAlign="Right"  />
                            <ItemTemplate>                                
                                <asp:Label ID="Label6" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("INVOICE_PRICE_NH")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtInvoicepriceNH" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                            </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn DataField="NET1_PRICE_NH" HeaderText="Net1<br/>Price NH" UniqueName="NET1_PRICE_NH"  HeaderStyle-Width="70px">
                            <ItemStyle HorizontalAlign="Right"  />
                            <ItemTemplate>                                
                                <asp:Label ID="Label7" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("NET1_PRICE_NH")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtNet1NH" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn DataField="NET2_PRICE_NH" HeaderText="Net2<br/>Price NH" UniqueName="NET2_PRICE_NH"  HeaderStyle-Width="70px">
                            <ItemStyle HorizontalAlign="Right"  />
                            <ItemTemplate>                                
                                <asp:Label ID="Label8" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("NET2_PRICE_NH")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtNet2NH" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="25px" HeaderText="DEL"  ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnRemove" runat="server" Text="D" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                    ButtonType="LinkButton" CssClass="btn btn-red btn-size1 bold" AutoPostBack="false" Visible="false" OnClientClicked='fn_Delete'>
                                </telerik:RadButton>                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="VISIBILITY" HeaderText="V" HeaderStyle-Width="25px" UniqueName="VISIBILITY" Visible="true" ReadOnly="true">                            
                        </telerik:GridBoundColumn>



                        <telerik:GridTemplateColumn DataField="SAMPLE_CODE" HeaderText="Sample<br/>Code" UniqueName="SAMPLE_CODE" HeaderStyle-Width="40px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("SAMPLE_CODE")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtsamplecode" runat="server" Width="100%" CssClass="input align_right">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="SAMPLE_TYPE" HeaderText="Sample<br/>Type" UniqueName="SAMPLE_TYPE" HeaderStyle-Width="40px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("SAMPLE_TYPE")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtsampletype" runat="server" Width="100%" CssClass="input align_right">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
<%--                        <telerik:GridTemplateColumn DataField="USE_SAMPLE_DC" HeaderText="Use<br/>Sample DC" UniqueName="USE_SAMPLE_DC" HeaderStyle-Width="50px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("USE_SAMPLE_DC")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtsampleDc" runat="server" Width="100%" CssClass="input align_right">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn DataField="MRP_FLAG" HeaderText="MRP<br/>Flag" UniqueName="MRP_FLAG" HeaderStyle-Width="40px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("MRP_FLAG")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtsampleDc" runat="server" Width="100%" CssClass="input align_right">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>       
                    </Columns>
                </MasterTableView>
                <HeaderStyle HorizontalAlign="Left" />
                <ClientSettings>
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true"  />
                </ClientSettings>
            </telerik:RadGrid>
        </div>
    </div>
    <input type="hidden" runat="server" id="hhdChkCompany" />
    <input type="hidden" runat="server" id="hhdChkBu" />
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
		<Windows>
			<telerik:RadWindow ID="radWinNewProduct" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product Master" Width="500px" Height="400px" Behaviors="Default" NavigateUrl="./PopupNewProduct.aspx" Modal="true" ></telerik:RadWindow>
		</Windows>
	</telerik:RadWindowManager>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="Server">
</asp:Content>

