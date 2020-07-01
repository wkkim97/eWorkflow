<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="ProductMasterList_0695.aspx.cs" Inherits="Reporting_ProductMasterList_0695" %>

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
        function fn_UpdateKPIS(sender, args) {
            var masterTable = $find('<%= GrdProductMaster.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var rowindex = sender.get_element().parentNode.parentNode.rowIndex;
            if (dataItems.length > 0) {
                var customercode = dataItems[rowindex - 1].get_cell("PRODUCT_CODE").innerText.trim();
                var customername = dataItems[rowindex - 1].get_cell("PRODUCT_NAME").innerText.trim();
                var customername_kr = dataItems[rowindex - 1].get_cell("PRODUCT_NAME_KR").innerText.trim();
                var radwin = $find('<%= RadWindow1.ClientID%>')
                radwin.set_title(customercode);
                radwin.show();
                $find('<%= ProductCode_TB.ClientID%>').set_value(customercode);
                $find('<%= ProductName_TB.ClientID%>').set_value(customername);
                $find('<%= ProductName_KOR_TB.ClientID%>').set_value(customername_kr);
                //$find('<%= Report_TB.ClientID%>').set_selected="Y";
                $find('<%= STD_TB.ClientID%>').set_value("");

                $find('<%= REPRSNT_TB.ClientID%>').set_value("");
                $find('<%= REPRSNT_NM_TB.ClientID%>').set_value("");
                $find('<%= Qty_TB.ClientID%>').set_value("");
                $find('<%= Qty_1_TB.ClientID%>').set_value("");

            }

        }
        function fn_Delete(sender, args) {
            var masterTable = $find('<%= GrdProductMaster.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var rowindex = sender.get_element().parentNode.parentNode.rowIndex;
            if (dataItems.length > 0) {
                var code = dataItems[rowindex - 1].get_cell("PRODUCT_CODE").innerText.trim();
                var bu = dataItems[rowindex - 1].get_cell("BU").innerText.trim();
               
                var VISIBILITY = dataItems[rowindex - 1].get_cell("VISIBILITY").innerText.trim();
                clickedKey = code + "|" + bu ;
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
        function bookNowCancelClicking(sender, args) {
            var radwin = $find('<%= RadWindow1.ClientID%>')
            radwin.close();

            args.set_cancel(true);

        }
        function bookNowClicking(sender, args) {
            // togglePanels();
            $find('<%= GrdProductMaster.ClientID%>').get_masterTableView().fireCommand("UpdateCount", $find('<%= RadWindow1.ClientID%>').get_title());
            var radwin = $find('<%= RadWindow1.ClientID%>')
            radwin.close();
            args.set_cancel(true);
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
        <h2>Product Master(0695)</h2>        
        <div style="display: inline-block; width: 100%">
            <div id="divBuArea" runat="server" style="display: inline-block; width: 100%">
                <ul>
                    <li style="float: left; margin-right: 4px;">
                        <span style="width: 70px; text-align: left; display: inline-block; margin-right: 3px">BU</span>
                        <telerik:RadDropDownList ID="radDropDown_BU" runat="server" Width="100%" DropDownWidth="60px">
                                    <Items>
                                        <telerik:DropDownListItem Text="CC" Value="CC" />
                                        <telerik:DropDownListItem Text="HH" Value="HH" Selected="true" />
                                        <telerik:DropDownListItem Text="WH" Value="WH" />
                                        <telerik:DropDownListItem Text="SM" Value="SM" />
                                        <telerik:DropDownListItem Text="AH" Value="AH" />
                                        <telerik:DropDownListItem Text="R" Value="R" />
                                        <telerik:DropDownListItem Text="DC" Value="DC" />
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
                     <li style="float: left; margin-right: 4px;">
                        <span style="width: 70px; text-align: left; display: inline-block; margin-right: 3px">KPIS </span>
                        <telerik:RadDropDownList ID="RadDropDown_KPIS" runat="server" Width="100%" DropDownWidth="60px">
                                    <Items>
                                        <telerik:DropDownListItem Text="Y" Value="Y" />                                                                     
                                        <telerik:DropDownListItem Text="ALL" Value="ALL" selected="true" />
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
                </ul>
            </div>
            
        </div>        
        <div class="board_list pt20">
            <telerik:RadGrid ID="GrdProductMaster" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="true" GridLines="None" Skin="EXGrid"
                 EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" OnBatchEditCommand="GrdProductMaster_BatchEditCommand"
                 HeaderStyle-HorizontalAlign="Left"  OnNeedDataSource="GrdProductMaster_NeedDataSource"
                OnItemDataBound="GrdProductMaster_ItemDataBound"
                OnItemCommand="GrdProductMaster_ItemCommand" >

                <MasterTableView EditMode="Batch" ShowHeadersWhenNoRecords="true" DataKeyNames="PRODUCT_CODE, PRODUCT_NAME, COMPANY_CODE" 
                    >
                    <HeaderStyle Font-Size="9px" HorizontalAlign="Left" />
                    <ItemStyle Font-Size="9px" />
                    <AlternatingItemStyle Font-Size="9px" />
                    <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Code" DataField="PRODUCT_CODE" UniqueName="PRODUCT_CODE" ReadOnly="true" HeaderStyle-Width="40px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Product Name" DataField="PRODUCT_NAME" UniqueName="PRODUCT_NAME" ReadOnly="true" HeaderStyle-Width="120px">
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>                   

                        <telerik:GridTemplateColumn DataField="PRODUCT_NAME_KR" HeaderStyle-Width="120px" HeaderText="Product Name(K)"  UniqueName="PRODUCT_NAME_KR">
                            <ItemTemplate><%# Eval("PRODUCT_NAME_KR")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radPRODUCT_NAME_KR" runat="server" Width="100%" CssClass="input align_left">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="COMPANY_CODE" DataField="COMPANY_CODE" UniqueName="COMPANY_CODE" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="BU" HeaderText="BU" UniqueName="BU" HeaderStyle-Width="20px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("BU")%></ItemTemplate>
                            <EditItemTemplate>                                 
                                <asp:TextBox ID="radGrdBu" runat="server" Width="100%" CssClass="input align_right">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>                        
                        <telerik:GridTemplateColumn DataField="BASE_PRICE" HeaderText="BasePrice" UniqueName="BASE_PRICE"  HeaderStyle-Width="45px"  HeaderStyle-HorizontalAlign="Right">
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
                        <telerik:GridTemplateColumn DataField="SAMPLE_CODE" HeaderText="Sample-Code" UniqueName="SAMPLE_CODE" HeaderStyle-Width="50px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("SAMPLE_CODE")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtsamplecode" runat="server" Width="100%" CssClass="input align_right">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="SAMPLE_TYPE" HeaderText="Sample-Type" UniqueName="SAMPLE_TYPE" HeaderStyle-Width="50px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("SAMPLE_TYPE")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtsampletype" runat="server" Width="100%" CssClass="input align_right">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn DataField="USE_SAMPLE_DC" HeaderText="Use<br/>Sample DC" UniqueName="USE_SAMPLE_DC" HeaderStyle-Width="50px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("USE_SAMPLE_DC")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtsampleDc" runat="server" Width="100%" CssClass="input align_right">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn DataField="MRP_FLAG" HeaderText="MRP" UniqueName="MRP_FLAG" HeaderStyle-Width="20px">
                            <ItemStyle Font-Size="10px" />
                            <ItemTemplate><%# Eval("MRP_FLAG")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtsampleDc" runat="server" Width="100%" CssClass="input align_right">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>                                                
                        <telerik:GridTemplateColumn DataField="MARGIN" HeaderText="Margin" UniqueName="MARGIN"  HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle HorizontalAlign="Right"  />
                            <ItemTemplate>                                
                                <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("MARGIN")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radtxtMargin" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="KPIS" UniqueName="KPIS_D"  HeaderStyle-Width="20px" >                        
                            <ItemTemplate>
                                <telerik:RadButton ID="radBtnKPIS" runat="server" Text="N" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                    ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" AutoPostBack="false" Visible="false" OnClientClicked='fn_UpdateKPIS'>
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="20px" HeaderText="DEL"  ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnRemove" runat="server" Text="D" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                    ButtonType="LinkButton" CssClass="btn btn-red btn-size1 bold" AutoPostBack="false" Visible="false" OnClientClicked='fn_Delete'>
                                </telerik:RadButton>                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="KPIS" HeaderText="K" HeaderStyle-Width="10px" UniqueName="KPIS" Visible="true" ReadOnly="true">                            
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="VISIBILITY" HeaderText="U" HeaderStyle-Width="10px" UniqueName="VISIBILITY" Visible="true" ReadOnly="true">                            
                        </telerik:GridBoundColumn>
                         
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
    <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" VisibleTitlebar="false" Modal="true" Width="550px" Height="380px"
                Behaviors="None" VisibleStatusbar="false">
                <ContentTemplate>
                    <asp:Panel ID="FirstStepPanel" runat="server">
                        <div class="bookNowFrame">
                            
                            <hr class="lineSeparator" style="margin: 12px 0 12px 0" />
                            <table cellspacing="8">
                                <colgroup>
                                    <col width="90px" />
                                    <col width="150px" />
                                    <col width="90px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>Product Code
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="ProductCode_TB" runat="server" Width="190px"  ReadOnly="true" /><br />
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td>
                                        Product Name
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="ProductName_TB" Width="100%" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Product Name(KOR)
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="ProductName_KOR_TB" Width="100%" runat="server"  ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>                                    
                                    <td>심평원보고대상
                                    </td>
                                    <td colspan="3">
                                        
                                        <telerik:RadDropDownList ID="Report_TB" runat="server" Width="100px" DropDownWidth="60px">
                                                    <Items>
                                                        <telerik:DropDownListItem Text="Y" Value="Y" selected="true" />
                                                        <telerik:DropDownListItem Text="N" Value="N" />                                        
                                                        
                                                    </Items>
                                        </telerik:RadDropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>표준코드
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="STD_TB" Width="130px" runat="server" />
                                    </td>
                                    <td>대표코드
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="REPRSNT_TB" Width="130px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>대표코드이름
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="REPRSNT_NM_TB" Width="100%" runat="server" />
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td>Qty
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="Qty_TB" Width="130px" runat="server" />
                                    </td>
                                    <td>예외Qty
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="Qty_1_TB" Width="130px" runat="server" />
                                    </td>
                                </tr>
                                
                            </table>
                            <hr class="lineSeparator" style="margin: 12px 0 12px 0" />                       
                           
                            <telerik:RadButton RenderMode="Lightweight" ID="BookNowButton" runat="server" Text="SAVE"
                                Width="100px" OnClientClicking="bookNowClicking" UseSubmitBehavior="false" />
                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton1" runat="server" Text="Cancel"
                                Width="100px" OnClientClicking="bookNowCancelClicking" UseSubmitBehavior="false" />
                            
                        </div>
                    </asp:Panel>
                    
                </ContentTemplate>
            </telerik:RadWindow>
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

