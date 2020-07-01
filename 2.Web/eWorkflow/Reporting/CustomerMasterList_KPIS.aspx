<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="CustomerMasterList_KPIS.aspx.cs" Inherits="Reporting_CustomerMasterList_KPIS" %>

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

        function fn_Update(sender, args) {
            var grid = $find('<%= radgrdCustomerMaster.ClientID %>');
            grid.get_batchEditingManager().saveChanges(grid.get_masterTableView());
          //  fn_OpenDocInformation('모든 data 가 update 되었습니다..');
        }

        function fn_OnGridNumBlur(sender) {
            setNumberFormat(sender);
        }
        
        var clickedKey = null;
        function fn_Delete(sender, args) {
            var masterTable = $find('<%= radgrdCustomerMaster.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var rowindex = sender.get_element().parentNode.parentNode.rowIndex;
            if (dataItems.length > 0) {
                var code = dataItems[rowindex - 1].get_cell("BCNC_CODE").innerText.trim();
                
                clickedKey = code;
                
                fn_OpenConfirm('Do you want to remove this Item ?', confirmDelete);
                

            }

        }

        function confirmDelete(arg) {
            
            if (arg) {
              //  fn_UpdateGridData(false);
                var masterTable = $find('<%= radgrdCustomerMaster.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }
    </script>
    <script src="/eWorks/Scripts/Approval/List.min.js"></script>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="Server">
   
    <telerik:RadAjaxLoadingPanel ID="radLoading" runat="server" Skin="Silk"></telerik:RadAjaxLoadingPanel>
   
    <telerik:RadAjaxManager ID="ajaxMgr" runat="server" DefaultLoadingPanelID="radLoading">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radgrdCustomerMaster">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radgrdCustomerMaster" UpdatePanelCssClass="panel"  />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="radBtnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radgrdCustomerMaster" UpdatePanelCssClass="panel"  />
                    <%--<telerik:AjaxUpdatedControl ControlID="divBuArea" />--%>
                </UpdatedControls>
            </telerik:AjaxSetting>
           <%-- <telerik:AjaxSetting AjaxControlID="ajaxMgr">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radgrdCustomerMaster" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
            <telerik:AjaxSetting AjaxControlID="radBtnUpdate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radgrdCustomerMaster" UpdatePanelCssClass="panel"   />
                </UpdatedControls>
            </telerik:AjaxSetting>
            
        </AjaxSettings>
       
    </telerik:RadAjaxManager>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
		<Windows>
			<telerik:RadWindow ID="radWinNewCustomer" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer Master" Width="500px" Height="500px" Behaviors="Default" NavigateUrl="./PopupNewCustomer.aspx" Modal="true" ></telerik:RadWindow>
		</Windows>
	</telerik:RadWindowManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="Server">
    <div id="content">
        <h2>Customer Master-KPIS</h2>
        <div style="display: inline-block; width: 100%">
            <div id="divBuArea" runat="server" style="display: inline-block; width: 100%">
                <ul>
                    <li style="float: left; margin-right: 4px; vertical-align:bottom">
                        <telerik:RadButton ID="radBtnUpdate" runat="server" Text="Update" ButtonType="LinkButton" 
                            EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size2 bold" 
                            Height="30px" Width="80px"  Font-Size="16px"
                            AutoPostBack="false" OnClientClicked="fn_Update">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btnDownload" runat="server" Text="Excel" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" 
                            ButtonType="LinkButton" CssClass="btn btn-green btn-size2 bold"
                            Height="30px" Width="80px"  Font-Size="16px" OnClick="btnDownload_Click"></telerik:RadButton>
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
                    
                    
                            
                            
							
                    <li style="clear: both;"></li>
                </ul>
            </div>
        </div>
      
         <div class="board_list pt20">
            <telerik:RadGrid ID="radgrdCustomerMaster" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="true" GridLines="None"
                 Skin="EXGrid"
                 EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"                
                OnNeedDataSource="radgrdCustomerMaster_NeedDataSource"
                 OnBatchEditCommand="radgrdCustomerMaster_BatchEditCommand"    OnItemCommand="radGrdProduct_ItemCommand"                
                 OnItemDataBound="radgrdCustomerMaster_ItemDataBound"  >
                <MasterTableView  EditMode="Batch"  DataKeyNames="BCNC_CODE">
                <HeaderStyle HorizontalAlign="Left" />
                    <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                    <Columns>
                        
                        <telerik:GridBoundColumn DataField="BCNC_CODE" HeaderText="Customer Code" HeaderStyle-Width="60px" UniqueName="BCNC_CODE" ReadOnly="true" DataType="System.String"></telerik:GridBoundColumn>                        
                        <telerik:GridBoundColumn DataField="CUSTOMER_NAME" HeaderText="Customer Name" HeaderStyle-Width="100px" UniqueName="CUSTOMER_NAME" ReadOnly="true"></telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="BCNC_NM" HeaderText="Company Name(KR)" UniqueName="BCNC_NM" HeaderStyle-Width="100px" ReadOnly="true">                           
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="BIZR_NO" HeaderText="License" UniqueName="BIZR_NO" HeaderStyle-Width="50px" Visible="true">
                            <ItemTemplate><%# Eval("BIZR_NO")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radBIZR_NO" runat="server" Width="100%" >                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn DataField="BCNC_RPRSNTV" HeaderText="CEO" UniqueName="BCNC_RPRSNTV" HeaderStyle-Width="50px" Visible="true">
                            <ItemTemplate><%# Eval("BCNC_RPRSNTV")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radCEO" runat="server" Width="100%" >                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>




                        <telerik:GridTemplateColumn DataField="BCNC_SE" HeaderText="Type" UniqueName="BCNC_SE" HeaderStyle-Width="50px" Visible="true">
                            <ItemTemplate>
                                <%# Eval("BCNC_SE")%></ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="radBCNC_SE" runat="server" Width="100%" DropDownWidth="50px">
                                    <Items>
                                        <telerik:DropDownListItem Text="도매" Value="도매" />
                                        <telerik:DropDownListItem Text="수출" Value="수출" />                                        
                                    </Items>
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="TELNO" HeaderText="Tel" UniqueName="TELNO" HeaderStyle-Width="60px" Visible="true">                            
                            <ItemTemplate><%# Eval("TELNO")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radGrdSalesRate" runat="server" Width="100%" >                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="POST_CODE" HeaderText="Post Code" UniqueName="POST_CODE" HeaderStyle-Width="50px" Visible="true">                            
                            <ItemTemplate><%# Eval("POST_CODE")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radPOST_CODE" runat="server" Width="100%">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="POST_ADRES" HeaderText="Address" UniqueName="POST_ADRES" HeaderStyle-Width="90px" ItemStyle-Font-Size="9px" Visible="true">                            
                            <ItemTemplate><%# Eval("POST_ADRES")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radPOST_ADRES" runat="server" Width="100%">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                         

                         <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="20px" HeaderText="D"  ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnRemove" runat="server" Text="X" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                    ButtonType="LinkButton" CssClass="btn btn-red btn-size1 bold" AutoPostBack="false" Visible="true"
                                    width="10px"
                                     OnClientClicked='fn_Delete'>
                                </telerik:RadButton>
                               
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                  
                       
                        <telerik:GridBoundColumn DataField="UPDATER_ID" HeaderText="UDATED ID" HeaderStyle-Width="30px" UniqueName="UPDATER_ID" Visible="false" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UPDATE_DATE" HeaderText="UDATED DATE" HeaderStyle-Width="50px" ItemStyle-Font-Size="8px" UniqueName="UPDATE_DATE" Visible="false" ReadOnly="true"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <%--<ClientSettings>
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true"  />
                </ClientSettings>--%>
                
            </telerik:RadGrid>
        </div>
    </div>
	 <input type="hidden" runat="server" id="hhdChkCompany" />
    <input type="hidden" runat="server" id="hhdChkBu" />
    <input type="hidden" id="informationMessage" runat="server" />
    

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="Server">
</asp:Content>

