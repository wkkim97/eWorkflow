<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="CustomerMasterList_0695.aspx.cs" Inherits="Reporting_CustomerMasterList_0695" %>

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
       
        var clickedKey = null;
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
            //$find("<%= radLoading.ClientID %>").show("<%= radgrdCustomerMaster.ClientID %>");
            grid.get_batchEditingManager().saveChanges(grid.get_masterTableView());
          //  $find("<%= radLoading.ClientID %>").hide("<%= radgrdCustomerMaster.ClientID %>");
            //$find("<%= radLoading.ClientID %>").show("<%= radgrdCustomerMaster.ClientID %>");
            
        }

       
        function fn_OnGridNumBlur(sender) {
            setNumberFormat(sender);
        }


        
        
        function fn_UpdateKPIS(sender, args) {
            var masterTable = $find('<%= radgrdCustomerMaster.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var rowindex = sender.get_element().parentNode.parentNode.rowIndex;
            if (dataItems.length > 0) {
                var customercode = dataItems[rowindex - 1].get_cell("CUSTOMER_CODE").innerText.trim();
                var customername = dataItems[rowindex - 1].get_cell("CUSTOMER_NAME").innerText.trim();
                var customername_kr = dataItems[rowindex - 1].get_cell("CUSTOMER_NAME_KR").innerText.trim();
                var radwin = $find('<%= RadWindow1.ClientID%>')
                radwin.set_title(customercode);
                radwin.show();
                $find('<%= CustomerCode_TB.ClientID%>').set_value(customercode);
                $find('<%= CustomerName_TB.ClientID%>').set_value(customername);
                $find('<%= CustomerName_KOR_TB.ClientID%>').set_value(customername_kr);
                $find('<%= License_TB.ClientID%>').set_value("");
                $find('<%= CEO_TB.ClientID%>').set_value("");
                
                $find('<%= TYPE_TB.ClientID%>').set_value("");
                $find('<%= Tel_TB.ClientID%>').set_value("");
                $find('<%= PostCode_TB.ClientID%>').set_value("");
                $find('<%= Address_TB.ClientID%>').set_value("");                
                
            }

        }
        function fn_Delete(sender, args) {
            var masterTable = $find('<%= radgrdCustomerMaster.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var rowindex = sender.get_element().parentNode.parentNode.rowIndex;
            if (dataItems.length > 0) {
                var code = dataItems[rowindex - 1].get_cell("CUSTOMER_CODE").innerText.trim();
                var bu = dataItems[rowindex - 1].get_cell("BU").innerText.trim();
                var parvw = dataItems[rowindex - 1].get_cell("PARVW").innerText.trim();
                var VISIBILITY = dataItems[rowindex - 1].get_cell("VISIBILITY").innerText.trim();
                clickedKey = code + "|" + bu + "|" + parvw;
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
                var masterTable = $find('<%= radgrdCustomerMaster.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }
        function confirmRelive(arg) {
            if (arg) {
                //  fn_UpdateGridData(false);
                var masterTable = $find('<%= radgrdCustomerMaster.ClientID %>').get_masterTableView();
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
            $find('<%= radgrdCustomerMaster.ClientID%>').get_masterTableView().fireCommand("UpdateCount", $find('<%= RadWindow1.ClientID%>').get_title());
            var radwin = $find('<%= RadWindow1.ClientID%>')
            radwin.close();
            args.set_cancel(true);
        }
       
        function fn_OpenNewCustomer(sender, args) {
            var wnd = $find("<%= radWinNewCustomer.ClientID %>");
            var parvw = "IE";
            wnd.setUrl("/eWorks/Common/Popup/PopupNewCustomer.aspx");
            wnd.show();
        }
       
        
        

      
        <%--
          function fn_OnResponseEnd(sender, args) {
            if (args.get_eventArgument().indexOf('Update') == 0)
                fn_OpenDocInformation('CreditLimit금액이 업데이트 되었습니다.');
        }
        function fn_SelectedIndexChanged(sender, args) {
            var masterTable = $find('<%= radgrdCustomerMaster.ClientID%>').get_masterTableView();
            var num = masterTable.get_dataItems().length;
            var selectedValue = null;

            for (var i = 0; i < num; i++) {
                var type = masterTable.get_dataItems()[i].findControl('radGrdDdLevel');
                var btnsap = masterTable.get_dataItems()[i].findControl('radBtnSap');
                if (type) {
                    selectedValue = type.get_selectedItem().get_value();
                    break;
                }
            }
            if (selectedValue == 'S') {
                btnsap.set_visible(true);
            }
            else 
                btnsap.set_visible(false);
            }
         function fn_UpdateSap(sender, args) {
            var masterTable = $find('<%= radgrdCustomerMaster.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var rowindex = sender.get_element().parentNode.parentNode.rowIndex;
            if (dataItems.length > 0) {
                var customercode = dataItems[rowindex - 1].get_cell("CUSTOMER_CODE").innerText.trim();
                var parvw = dataItems[rowindex - 1].get_cell("PARVW").innerText.trim();

                executeCalculateAjax(customercode, parvw);
            }

        }
        function executeCalculateAjax(customerCode, parvw) {
            try {
                var loadingPanel = "#<%= radLoading.ClientID %>";
                
                $(loadingPanel).show();
                var svcUrl = WCFSERVICE + "/AfterTreatmentServices.svc/GetSapCreditLimit/" + customerCode;
                $.support.cors = true;
                $.ajax({
                    type: "GET",
                    url: svcUrl,
                    dataType: "JSON",
                    success: function (data) {
                        setTimeout(function () { }, 1000);
                        
                        var limitAmount = parseFloat(data).toFixedDown(0)
                        var callbackFn = Function.createDelegate(null, function (shouldSubmit) {
                            if (shouldSubmit) {
                                $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("Update:" + customerCode + ":" + data + ":" + parvw);
                            }
                        });
                        fn_OpenConfirm(limitAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + '금액으로 업데이트 하시겠습니까?', callbackFn)

                        

                    },
                    error: function (e) {
                        fn_OpenErrorMessage('error>>' + e)
                    },
                    complete: function () {

                        $(loadingPanel).hide();
                    }
                });
            }
            catch (exception) {
                fn_OpenErrorMessage(exception)
            }
        }
        
        --%>

    	
    </script>
    <script src="/eWorks/Scripts/Approval/List.min.js"></script>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="Server">
    <style>
        .panel{height:100% !important}
    </style>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="Server">
    <div id="content">
        <h2>Customer Master(0695)</h2>
        <div style="display: inline-block; width: 100%">
            <div id="divBuArea" runat="server" style="display: inline-block; width: 100%">
                <ul>
                   <%--  <li style="float: left; margin-right: 4px;"><span style="width: 70px; text-align: right; display: inline-block; margin-right: 3px">Company</span>
                       <div id="divComapnyCode" runat="server" style="display: inline-block">
                        <telerik:RadButton ID="radBtnCompanycode1" runat="server" Text="0695" Value="0695" checked="true"
                            ButtonType="ToggleButton" ToggleType="CheckBox" >
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnCompanycode2" runat="server" Text="0963" Value="0963"
                            ButtonType="ToggleButton" ToggleType="CheckBox" >
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnCompanycode3" runat="server" Text="1117" Value="1117" Visible="false"
                            ButtonType="ToggleButton" ToggleType="CheckBox" >
                        </telerik:RadButton>
                        </div>
                    </li>--%>
                    <li style="float: left; margin-right: 4px;">
                        <span style="width: 70px; text-align: left; display: inline-block; margin-right: 3px">BU</span>
                        <telerik:RadDropDownList ID="radDropDown_BU" runat="server" Width="100%" DropDownWidth="60px">
                                    <Items>
                                        <telerik:DropDownListItem Text="08" Value="08" />
                                        <telerik:DropDownListItem Text="16" Value="16" />
                                        <telerik:DropDownListItem Text="18" Value="18" />
                                        <telerik:DropDownListItem Text="ALL" Value="ALL" selected="true" />
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
                    <li style="float: left; margin-right: 4px;">
                        <span style="width: 70px; text-align: left; display: inline-block; margin-right: 3px"> Type </span>
                        <telerik:RadDropDownList ID="RadDropDownList_PARVW" runat="server" Width="100%" DropDownWidth="60px">
                                    <Items>
                                        <telerik:DropDownListItem Text="IE" Value="IE" selected="true" />                                                                     
                                        <telerik:DropDownListItem Text="IF" Value="IF" />                                                                     
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
                            AutoPostBack="false" OnClientClicked="fn_Update">
                        </telerik:RadButton>
                    </li>
                    <li style="float: left; margin-right: 4px;">
                        <telerik:RadButton ID="btnDownload" runat="server" Text="Excel" EnableEmbeddedSkins="false"
                             EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton"
                             Height="42px" Width="80px" Font-Size="16px"
                            CssClass="btn btn-green btn-size2 bold" OnClick="btnDownload_Click">

                        </telerik:RadButton>
                    </li>
                    <li style="float: left; margin-right: 4px;">
                        <telerik:RadButton ID="radBtnNew" runat="server" Text="New" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" 
                            EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size2 bold" 
                            Height="42px" Width="80px" Font-Size="16px"
                            AutoPostBack="false" OnClientClicked="fn_OpenNewCustomer">
                            </telerik:RadButton>
                    </li>              
                    
                    <li style="clear: both;"></li>
                </ul>
            </div>
        </div>
        <div class="board_list pt20">
            <telerik:RadGrid ID="radgrdCustomerMaster" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                 AllowSorting="false" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                 Skin="EXGrid" GridLines="None" AllowMultiRowSelection="false" 
                OnNeedDataSource="radgrdCustomerMaster_NeedDataSource"
                 OnBatchEditCommand="radgrdCustomerMaster_BatchEditCommand"
                 OnItemDataBound="radgrdCustomerMaster_ItemDataBound"
                OnItemCommand="radgrdCustomerMaster_ItemCommand"  >
                <MasterTableView EnableHeaderContextMenu="true" EditMode="Batch"
                     DataKeyNames="CUSTOMER_CODE, PARVW, CUSTOMER_NAME, CUSTOMER_NAME_KR, COMPANY_CODE, CREDIT_LIMIT,BU, MORTAGE">
                    <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="CUSTOMER_CODE" HeaderText="Code" HeaderStyle-Width="30px" UniqueName="CUSTOMER_CODE" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PARVW" HeaderText="Parvw" HeaderStyle-Width="20px" UniqueName="PARVW" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CUSTOMER_NAME" HeaderText="Customer Name" HeaderStyle-Width="60px" UniqueName="CUSTOMER_NAME" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="CUSTOMER_NAME_KR" HeaderStyle-Width="60px" HeaderText="Customer Name(K)"  UniqueName="CUSTOMER_NAME_KR">
                            <ItemTemplate><%# Eval("CUSTOMER_NAME_KR")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radCUSTOMER_NAME_KR" runat="server" Width="100%" CssClass="input align_left">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridBoundColumn DataField="COMPANY_CODE" HeaderText="Com Code" HeaderStyle-Width="30px" UniqueName="COMPANY_CODE" Visible="false" ReadOnly="true"></telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="BU" HeaderText="BU" UniqueName="BU" HeaderStyle-Width="15px" ReadOnly="true">                            
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="LEVEL" HeaderText="Level" UniqueName="LEVEL" HeaderStyle-Width="18px" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("LEVEL")%>'></asp:Label></ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="radGrdDdLevel" runat="server" Width="100%" DropDownWidth="60px">
                                    <Items>
                                        <telerik:DropDownListItem Text="S" Value="S" />
                                        <telerik:DropDownListItem Text="G" Value="G" />
                                        <telerik:DropDownListItem Text="" Value="" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="SALES_RATE" HeaderText="Sales Rate" UniqueName="SALES_RATE" HeaderStyle-Width="22px" Visible="false">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate><%# Eval("SALES_RATE")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radGrdSalesRate" runat="server" Width="100%" CssClass="input align_right" MaxLength="3" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="SCORING_RATE" HeaderText="Scoring Rate" UniqueName="SCORING_RATE" HeaderStyle-Width="25px" Visible="false">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate><%# Eval("SCORING_RATE")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radGrdScoringRate" runat="server" Width="100%" CssClass="input align_right" MaxLength="3" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- <telerik:GridBoundColumn DataField="CREDIT_LIMIT" HeaderStyle-Width="30px" HeaderText="Credit Limit" UniqueName="CREDIT_LIMIT" ReadOnly="true" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridTemplateColumn DataField="CREDIT_LIMIT" HeaderText="Credit Limit" UniqueName="CREDIT_LIMIT" HeaderStyle-Width="30px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate><%# String.Format("{0:#,##0}", Eval("CREDIT_LIMIT"))%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radGrdCreditLimit" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn DataField="MORTAGE" HeaderText="Collateral Amt." UniqueName="MORTAGE" HeaderStyle-Width="30px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate><%# String.Format("{0:#,##0}", Eval("MORTAGE"))%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radGrdMortage" runat="server" Width="100%" CssClass="input align_right" onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        
                        <telerik:GridTemplateColumn HeaderText="SAP" UniqueName="SAP" HeaderStyle-Width="20px" Visible="false">
                            <ItemTemplate>
                                <telerik:RadButton ID="radBtnSap" runat="server" Text="SAP" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                    ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" AutoPostBack="false" Visible="false" OnClientClicked='fn_UpdateSap'>
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="KPIS" UniqueName="KPIS_D"  HeaderStyle-Width="20px" >                        
                            <ItemTemplate>
                                <telerik:RadButton ID="radBtnKPIS" runat="server" Text="NEW" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                    ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" AutoPostBack="false" Visible="false" OnClientClicked='fn_UpdateKPIS'>
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="15px" HeaderText="DEL"  ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnRemove" runat="server" Text="D" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                    ButtonType="LinkButton" CssClass="btn btn-red btn-size1 bold" AutoPostBack="false" Visible="false" OnClientClicked='fn_Delete'>
                                </telerik:RadButton>
                                <%--<asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" AutoPostBack="false" ButtonType="LinkButton"
                                    OnClientClicked='<%# String.Format("return openDeleteCustomerPopUp(\"{0}\",\"{1}\",\"{2}\");",Eval("CUSTOMER_CODE"),Eval("BU"),Eval("PARVW"))%>'
                                     
                                    ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="UPDATER_ID" HeaderText="UDATED ID" HeaderStyle-Width="30px" UniqueName="UPDATER_ID" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UPDATE_DATE" HeaderText="UDATED DATE" HeaderStyle-Width="30px" UniqueName="UPDATE_DATE" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="KPIS" HeaderText="K" HeaderStyle-Width="5px" UniqueName="KPIS" Visible="true">                            
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="VISIBILITY" HeaderText="U" HeaderStyle-Width="5px" UniqueName="VISIBILITY" Visible="true" ReadOnly="true">                            
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <HeaderStyle HorizontalAlign="Left" />
            </telerik:RadGrid>
        </div>
    </div>
	<telerik:RadWindowManager runat="server" ID="RadWindowManager">
		<Windows>
			<telerik:RadWindow ID="radWinNewCustomer" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer Master"
                 Width="500px" Height="500px" Behaviors="Default" NavigateUrl="./PopupNewCustomer.aspx" Modal="true" ></telerik:RadWindow>
		</Windows>
	</telerik:RadWindowManager>

    <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" VisibleTitlebar="false" Modal="true" Width="550px" Height="380px"
                Behaviors="None" VisibleStatusbar="false">
                <ContentTemplate>
                    <asp:Panel ID="FirstStepPanel" runat="server">
                        <div class="bookNowFrame">
                            <div class="bookNowTitle">
                                Fill in the form to make your reservation
                            </div>
                            <hr class="lineSeparator" style="margin: 12px 0 12px 0" />
                            <table cellspacing="8">
                                <colgroup>
                                    <col width="90px" />
                                    <col width="150px" />
                                    <col width="90px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>Customer Code
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="CustomerCode_TB" runat="server" Width="190px" /><br />
                                    </td>
                                    <td>Customer Name
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="CustomerName_TB" Width="130px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Customer Name-Kor
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="CustomerName_KOR_TB" Width="130px" runat="server" />
                                    </td>
                                    <td>License
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="License_TB" Width="130px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>CEO
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="CEO_TB" Width="130px" runat="server" />
                                    </td>
                                    <td>Type
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="TYPE_TB" Width="130px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Tel
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="Tel_TB" Width="130px" runat="server" />
                                    </td>
                                    <td>Post Code
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="PostCode_TB" Width="130px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Address
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="Address_TB" Width="230px" runat="server" />
                                    </td>
                                    
                                </tr>
                            </table>
                            <hr class="lineSeparator" style="margin: 12px 0 12px 0" />                       
                            <hr class="lineSeparator" style="margin: 12px 0 15px 0" />
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
    <input type="hidden" id="informationMessage" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="Server">
</asp:Content>

