<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="ProductMasterList_KPIS.aspx.cs" Inherits="Reporting_ProductMasterList_KPIS" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <style>
         .RadGrid_EXGrid .rgBatchChanged {
            background-image: url('../Styles/images/TelerikIcon/icn_gridCheck.png') !important;
            background-position: 0 0;
            background-repeat: no-repeat;
            }

     </style>
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
        
        var clickedKey;
        function fn_Delete(sender, args) {
            var masterTable = $find('<%= GrdProductMaster.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var rowindex = sender.get_element().parentNode.parentNode.rowIndex;
            if (dataItems.length > 0) {
                var code = dataItems[rowindex - 1].get_cell("PRDUCT_CODE").innerText.trim();
                clickedKey = code;
                
                fn_OpenConfirm('Do you want to remove this Item ?', confirmDelete);
                

            }

        }
        function confirmDelete(arg) {
            if (arg) {
                //  fn_UpdateGridData(false);
                var masterTable = $find('<%= GrdProductMaster.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }
        
       

          <%-- function fn_buCheck(sender, args) {
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
    <telerik:RadAjaxLoadingPanel ID="radLoading" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="radLoading">
        <AjaxSettings>
             <telerik:AjaxSetting AjaxControlID="GrdProductMaster" >
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="GrdProductMaster" UpdatePanelCssClass="panel"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radBtnUpdate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="GrdProductMaster" />
                </UpdatedControls>                
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radBtnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="GrdProductMaster" />
                </UpdatedControls>                
            </telerik:AjaxSetting>
                            
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="Server">
    <div id="content">
        <h2>Product Master(KPIS)</h2>        
        <div style="display: inline-block; width: 100%">
            <div id="divBuArea" runat="server" style="display: inline-block; width: 100%">
                <ul>
                    <li style="float: left; margin-right: 4px; vertical-align:bottom">
                        <telerik:RadButton ID="RadButton" runat="server" Text="Update" ButtonType="LinkButton" 
                            EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size2 bold" 
                            Height="30px" Width="80px"  Font-Size="16px"
                            AutoPostBack="false" OnClientClicked="fn_UpdateType">
                        </telerik:RadButton>
                    </li>                   
                    <li style="float: right; margin-right: 4px;width:150px;padding-top:2px;">                        
                        <telerik:RadTextBox ID="radTxtKeyword" runat="server" Width="100%" ClientEvents-OnKeyPress="onEnter" Height="32px"></telerik:RadTextBox>
                    </li>
                    <li style="float: right; margin-right: 4px;">
                        <telerik:RadButton ID="radBtnSearch" runat="server" Text="Search"
                                ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-gray btn-size2 bold"
                                Height="30px" Width="80px" Font-Size="16px"
                                OnClick="radBtnSearch_Click" AutoPostBack="true" >
                            </telerik:RadButton>
                    </li>
                    
                    
                    
                    <li style="clear:both;"></li>
                </ul>
            </div>
            
        </div>        
        <div class="board_list pt20">
            <telerik:RadGrid ID="GrdProductMaster" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="true" GridLines="None"
                 Skin="EXGrid"
                 EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" OnBatchEditCommand="GrdProductMaster_BatchEditCommand"
                 HeaderStyle-HorizontalAlign="Left"  OnNeedDataSource="GrdProductMaster_NeedDataSource"
                
                OnItemCommand="GrdProductMaster_ItemCommand" 
                  >

                <MasterTableView EditMode="Batch" ShowHeadersWhenNoRecords="true" DataKeyNames="PRDUCT_CODE" 
                    >
                    <HeaderStyle Font-Size="9px" HorizontalAlign="Left" />
                    <ItemStyle Font-Size="9px" />
                    <AlternatingItemStyle Font-Size="9px" />
                    <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Code" DataField="PRDUCT_CODE" UniqueName="PRDUCT_CODE" ReadOnly="true" HeaderStyle-Width="30px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="PRODUCT_NAME_KR" DataField="PRODUCT_NAME_KR" UniqueName="PRODUCT_NAME_KR"
                             ReadOnly="true" HeaderStyle-Width="80px">
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridTemplateColumn DataField="STD_CODE" HeaderText="표준코드" UniqueName="STD_CODE"  HeaderStyle-Width="45px"  HeaderStyle-HorizontalAlign="left">                           
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate>                                
                                <%# Eval("STD_CODE") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtSTD_CODE" runat="server" Width="100%">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="REPRSNT_CODE" HeaderText="대표코드" UniqueName="REPRSNT_CODE" HeaderStyle-Width="50px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("REPRSNT_CODE")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtREPRSNT_CODE" runat="server" Width="100%" CssClass="input align_left">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="REPRSNT_NM" HeaderText="대표이름" UniqueName="REPRSNT_NM" HeaderStyle-Width="120px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("REPRSNT_NM")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtREPRSNT_NM" runat="server" Width="100%" CssClass="input align_left">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>                        
                        <telerik:GridTemplateColumn DataField="REPORT_YN" HeaderText="Report" UniqueName="REPORT_YN" HeaderStyle-Width="20px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("REPORT_YN")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtREPORT_YN" runat="server" Width="100%" CssClass="input align_left">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>                                                
                        <telerik:GridTemplateColumn DataField="PACKNG_QY" HeaderText="Qty" UniqueName="PACKNG_QY"  HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle HorizontalAlign="Right"  />
                            <ItemTemplate>                                
                                <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("PACKNG_QY")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtPACKNG_QY" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="C_PACKNG_QY" HeaderText="변경될Qty" UniqueName="C_PACKNG_QY"  HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle HorizontalAlign="Right"  />
                            <ItemTemplate>                                
                                <asp:Label ID="Label3" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("C_PACKNG_QY")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtPACKNG_QY_C" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="MULTIPLICATION" HeaderText="MULTIPLICATION" UniqueName="MULTIPLICATION"  HeaderStyle-Width="22px" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle HorizontalAlign="Right"  />
                            <ItemTemplate>                                
                                <asp:Label ID="Label4" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("MULTIPLICATION")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtMULTIPLICATION" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="20px" HeaderText="DEL"  ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnRemove" runat="server" Text="X" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                    ButtonType="LinkButton" CssClass="btn btn-red btn-size1 bold" AutoPostBack="false" Visible="true" OnClientClicked='fn_Delete'>
                                </telerik:RadButton>                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                       
                         
                        
                         
                    </Columns>
                </MasterTableView>
                <HeaderStyle HorizontalAlign="Left" />
                <%--<ClientSettings>
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true"  />
                </ClientSettings>--%>
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

