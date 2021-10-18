<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="CustomerMasterList.aspx.cs" Inherits="Reporting_CustomerMasterList" %>

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

        function fn_Update(sender, args) {
            var grid = $find('<%= radgrdCustomerMaster.ClientID %>');
            grid.get_batchEditingManager().saveChanges(grid.get_masterTableView());
            fn_OpenDocInformation('모든 data 가 update 되었습니다..');
        }

        function fn_OnGridNumBlur(sender) {
            setNumberFormat(sender);
        }

        function fn_buCheck(sender, args) {
            var CheckedValue = '';
            var controls = $('#<%= divBU.ClientID %>').children();
            for (var i = 0; i < controls.length; i++) {
                var bu = controls[i];                                       //BU
                if ($find(bu.id).get_checked()) {
                    CheckedValue += $find(bu.id)._value + ',';
                }
            }
            $('#<%= hhdChkBu.ClientID%>').val(CheckedValue);
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

        function fn_OnResponseEnd(sender, args) {
            if (args.get_eventArgument().indexOf('Update') == 0)
                fn_OpenDocInformation('CreditLimit금액이 업데이트 되었습니다.');
        }
        <%--function fn_SelectedIndexChanged(sender, args) {
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
            }--%>

    	function fn_OpenNewCustomer(sender, args) {
    		var wnd = $find("<%= radWinNewCustomer.ClientID %>");
    		var parvw = "IE";
    		wnd.setUrl("/eWorks/Common/Popup/PopupNewCustomer.aspx");
    		wnd.show();
    	}
    </script>
    <script src="/eWorks/Scripts/Approval/List.min.js"></script>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="Server">
   
    <telerik:RadAjaxLoadingPanel ID="radLoading" runat="server"></telerik:RadAjaxLoadingPanel>
   
    <telerik:RadAjaxManager ID="ajaxMgr" runat="server" OnAjaxRequest="Reporting_CustomerMasterList_AjaxRequest" ClientEvents-OnResponseEnd="fn_OnResponseEnd">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radBtnUpdate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radgrdCustomerMaster" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radBtnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radgrdCustomerMaster" LoadingPanelID="loadingPanel" />
                    <%--<telerik:AjaxUpdatedControl ControlID="divBuArea" />--%>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ajaxMgr">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radgrdCustomerMaster" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radBtnCompanycode1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divBU" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radBtnCompanycode2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divBU" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radBtnCompanycode3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divBU" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="Server">
    <div id="content">
        <h2>Customer Master</h2>
        <div style="display: inline-block; width: 100%">
            <div id="divBuArea" runat="server" style="display: inline-block; width: 100%">
                <ul>
                    <li style="float: left; margin-right: 4px;display:none">
                        <span style="width: 70px; text-align: right; display: inline-block; margin-right: 3px">Company</span><div id="divComapnyCode" runat="server" style="display: inline-block">
                        <telerik:RadButton ID="radBtnCompanycode1" runat="server" Text="0695" Value="0695"
                            ButtonType="ToggleButton" ToggleType="CheckBox" OnCheckedChanged="radBtnCompanycode1_CheckedChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnCompanycode2" runat="server" Text="0963" Value="0963"
                            ButtonType="ToggleButton" ToggleType="CheckBox" OnCheckedChanged="radBtnCompanycode1_CheckedChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnCompanycode3" runat="server" Text="1117" Value="1117" Visible="false"
                            ButtonType="ToggleButton" ToggleType="CheckBox" OnCheckedChanged="radBtnCompanycode1_CheckedChanged">
                        </telerik:RadButton>
                    </div>
                    </li>
                    <li style="float: left; margin-right: 4px;display:none"><span style="width: 70px; text-align: right; display: inline-block; margin-right: 3px">BU</span></li>
                    <li style="float: left; margin-right: 4px;display:none">
                        <div id="divBU" runat="server" style="display: inline-block"></div>
                    </li>

                    <li style="float: right; margin-right: 4px;">
                        <div style="float: right; display: inline-block">
                            <telerik:RadTextBox ID="radTxtKeyword" runat="server" ClientEvents-OnKeyPress="onEnter"></telerik:RadTextBox>
                            <telerik:RadButton ID="radBtnSearch" runat="server" Text="Search"
                                ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-gray btn-size2 bold"
                                OnClick="radBtnSearch_Click" OnClientClicked="fn_buCheck">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnUpdate" runat="server" Text="Update" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-gray btn-size2 bold" AutoPostBack="false" OnClientClicked="fn_Update"></telerik:RadButton>
                            <telerik:RadButton ID="btnDownload" runat="server" Text="ExcelDownload" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-gray btn-size2 bold" OnClick="btnDownload_Click"></telerik:RadButton>
							<telerik:RadButton ID="radBtnNew" runat="server" Text="New" ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-gray btn-size2 bold" AutoPostBack="false" OnClientClicked="fn_OpenNewCustomer">
                            </telerik:RadButton>
                        </div>
                    </li>
                    <li style="clear: both;"></li>


                </ul>
            </div>
        </div>
        Parvw : IE - Retailer,NH / IF - WS
        <div class="board_list pt20">
            <telerik:RadGrid ID="radgrdCustomerMaster" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                 AllowSorting="true" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                 Skin="EXGrid" GridLines="None" AllowMultiRowSelection="false" 
                OnNeedDataSource="radgrdCustomerMaster_NeedDataSource"
                 OnBatchEditCommand="radgrdCustomerMaster_BatchEditCommand"
                 OnItemDataBound="radgrdCustomerMaster_ItemDataBound"  >
                <MasterTableView EnableHeaderContextMenu="true" EditMode="Batch"
                     DataKeyNames="CUSTOMER_CODE, PARVW, CUSTOMER_NAME, CUSTOMER_NAME_KR, COMPANY_CODE, CREDIT_LIMIT, CHANNEL">
                    <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="CUSTOMER_CODE" HeaderText="Code" HeaderStyle-Width="30px" UniqueName="CUSTOMER_CODE" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PARVW" HeaderText="Parvw" HeaderStyle-Width="20px" UniqueName="PARVW" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CUSTOMER_NAME" HeaderText="Customer Name" HeaderStyle-Width="60px" UniqueName="CUSTOMER_NAME" ReadOnly="true"></telerik:GridBoundColumn>


                        <telerik:GridTemplateColumn DataField="CUSTOMER_NAME_KR" HeaderText="Customer Name(K)" UniqueName="CUSTOMER_NAME_KR" HeaderStyle-Width="60px">
                            <ItemTemplate><%# Eval("CUSTOMER_NAME_KR")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radGrdCustomer_Name_Kr" runat="server" Width="100%" CssClass="input align_right">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>





                        <telerik:GridBoundColumn DataField="COMPANY_CODE" HeaderText="Com Code" HeaderStyle-Width="30px" UniqueName="COMPANY_CODE" Visible="false" ReadOnly="true"></telerik:GridBoundColumn>
                        
                        <telerik:GridTemplateColumn DataField="BU" HeaderText="BU" UniqueName="BU" HeaderStyle-Width="15px">
                            <ItemTemplate><%# Eval("BU")%></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="radGrdBu" runat="server" Width="100%" CssClass="input align_right">                                
                                </asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn DataField="CHANNEL" HeaderText="CHANNEL" UniqueName="CHANNEL" HeaderStyle-Width="40px" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblChannel" runat="server" Text='<%# Eval("CHANNEL")%>'></asp:Label></ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="radGrdChannel" runat="server" Width="100%" DropDownWidth="60px"  CssClass="input align_right">
                                    <Items>
                                        <telerik:DropDownListItem Text="FM" Value="FM" />
                                        <telerik:DropDownListItem Text="NH" Value="NH" />
                                        <telerik:DropDownListItem Text="" Value="" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

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
                        <telerik:GridTemplateColumn HeaderText="SAP" UniqueName="SAP" HeaderStyle-Width="20px" Visible="false">
                            <ItemTemplate>
                                <telerik:RadButton ID="radBtnSap" runat="server" Text="SAP" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                    ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" AutoPostBack="false" Visible="false" OnClientClicked='fn_UpdateSap'>
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="KPIS" HeaderText="KPIS" HeaderStyle-Width="15px" UniqueName="KPIS" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="KPIS_D" UniqueName="KPIS_D" HeaderStyle-Width="20px" Visible="false">
                            <ItemTemplate>
                                <telerik:RadButton ID="radBtnKPIS" runat="server" Text="SAP" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                                    ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" AutoPostBack="false" Visible="false" OnClientClicked='fn_UpdateSap'>
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="UPDATER_ID" HeaderText="UDATED ID" HeaderStyle-Width="30px" UniqueName="UPDATER_ID" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UPDATE_DATE" HeaderText="UDATED DATE" HeaderStyle-Width="50px" UniqueName="UPDATE_DATE" ReadOnly="true"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <HeaderStyle HorizontalAlign="Left" />
            </telerik:RadGrid>
        </div>
    </div>
	<telerik:RadWindowManager runat="server" ID="RadWindowManager">
		<Windows>
			<telerik:RadWindow ID="radWinNewCustomer" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer Master" Width="500px" Height="500px" Behaviors="Default" NavigateUrl="./PopupNewCustomer.aspx" Modal="true" ></telerik:RadWindow>
		</Windows>
	</telerik:RadWindowManager>
    <input type="hidden" runat="server" id="hhdChkCompany" />
    <input type="hidden" runat="server" id="hhdChkBu" />
    <input type="hidden" id="informationMessage" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="Server">
</asp:Content>

