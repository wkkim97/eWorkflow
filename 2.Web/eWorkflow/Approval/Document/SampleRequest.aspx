<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="SampleRequest.aspx.cs" Inherits="Approval_Document_SampleRequest" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function fn_OnBUChanged(sender, args) {
          //  if (args.get_checked())
                //alert(sender.get_value());
            //alert($("#hspanOrganization").text);
        }

        function fn_keyPress(sender, args) {
            var text = sender.get_value() + args.get_keyCharacter();
            if (!text.match('^[0-9-,]+$'))
                args.set_cancel(true);
        }

        function fn_DoRequest(sender, args) {
            return fn_UpdateGridData(null);
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData(null);
        }

        //PRODUCT POPUP
        function fn_PopupItem(sender, args) {
            fn_UpdateGridData(null);
            var dataItems = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                for (var i = 0; i < dataItems.length; i++) {

                    var itemcode = dataItems[i].get_cell("ITEM_CODE").innerText.trim();
                    var itemdesc = dataItems[i].get_cell("ITEM_DESC").innerText.trim();
                    var qty = dataItems[i].get_cell("QTY").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                    var price = dataItems[i].get_cell("PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                    var amount = dataItems[i].get_cell("AMOUNT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                    
                    if (itemcode == '' || itemdesc == '') {
                        fn_OpenDocInformation('자료를 입력하시기 바랍니다.');
                        return false;
                    }
                }                
            }
            var selectedValue = getSelectedBU();

            var selectedCategoryValue = getSelectedCategory();

            // 2015-09-16 : Youngwoo Lee
            // Claim compensation  일 경우에는 Invoice price 를 그 외에는 base price 를 가져올 수 있게 파라미터 값 수정
            // procedure eManage.dbo.USP_SELECT_PRODUCT 수정

            if (selectedCategoryValue == "Claim") {
                selectedCategoryValue = "I";
            }
            else {
                selectedCategoryValue = "Y";
            }

            var wnd = $find("<%= radWinSampleRequestItemList.ClientID %>");
            if (selectedValue == "") {
                fn_OpenDocInformation("BG을 선택하여 주세요");
                return;
            }
            wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=" + selectedValue + "&baseprice=" + selectedCategoryValue);
            wnd.show();
        }

        function getSelectedBU() {
            var controls = $('#<%= divSalesGroup.ClientID %>').children();
            var selectedValue;
            //alert("3333");
            for (var i = 0; i < controls.length; i++) {
                var bu = controls[i];
                if ($find(bu.id).get_checked()) {
                    selectedValue = $find(bu.id).get_value();
                    break;
                }
                else
                {
                    selectedValue = "";
                }
            }
            return selectedValue;
        }

        function getSelectedCategory() {
            var controls = $('#<%= divCagegory.ClientID %>').children();
            var selectedCategoryValue;

            for (var i = 0; i < controls.length; i++) {
                var Category = controls[i];
                if ($find(Category.id).get_checked()) {
                    selectedCategoryValue = $find(Category.id).get_value();
                    break;
                }
                else {
                    selectedCategoryValue = "";
                }
            }
            return selectedCategoryValue;
        }

        //CostCenter Popup
        function fn_OpenCostCenter(sender, eventArgs) {
            var masterTable = $find('<%= radGrdSampleItemList.ClientID%>').get_masterTableView();
            var num = masterTable.get_dataItems().length;
            var wnd = $find("<%= radWinCostCenter.ClientID %>");
            var classcode = "S019";

            wnd.setUrl("/eWorks/Common/Popup/CostCenter.aspx?classcode=" + classcode);
            wnd.show();
            sender.set_autoPostBack(false);
        }

        function fn_UpdateGridData(data) {
            var list = [];
            var masterTable = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var is_val = false;
            var maxIdx = 0;
            for (var i = 0; i < dataItems.length; i++) {

                var idx = dataItems[i].get_cell("IDX").innerText.trim();
                var itemcode = dataItems[i].get_cell("ITEM_CODE").innerText.trim();
                var itemdesc = dataItems[i].get_cell("ITEM_DESC").innerText.trim();
                var qty = '0';
                if (dataItems[i].findElement('radGrdNumQty'))
                    qty = dataItems[i].findElement('radGrdNumQty').value.replace(/,/gi, '').replace(/ /gi, '');
                else
                    qty = dataItems[i].get_cell("QTY").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var price = dataItems[i].get_cell("PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var amount = dataItems[i].get_cell("AMOUNT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var costcode = dataItems[i].get_cell("COST_CODE").innerText.trim();
                var costname = dataItems[i].get_cell("COST_NAME").innerText.trim();
                var locationcode = dataItems[i].get_cell("LOCATION_CODE").innerText.trim();

                var ctrlLocation = dataItems[i].findControl('radDropLocation');
                var location = null;
                if (ctrlLocation)
                    location = ctrlLocation._selectedText;
                else
                    location = dataItems[i].get_cell("LOCATION_NAME").children[0].innerText.trim();
               
                var remark = '';
//                if(dataItems[i].findControl('radGrdTxtRemark'))
//                    remark = dataItems[i].findControl('radGrdTxtRemark')._text;
//                else
                    remark = dataItems[i].get_cell("REMARK").innerText.trim();
                

                var item = {
                    IDX: null,
                    ITEM_CODE: null,
                    ITEM_DESC: null,
                    QTY: null,
                    PRICE: null,
                    AMOUNT: null,
                    COST_CODE: null,
                    COST_NAME: null,
                    LOCATION_CODE: null,
                    LOCATION_NAME: null,
                    REMARK: null,
                }
                maxIdx = idx;
                item.IDX = parseInt(idx);
                item.ITEM_CODE = itemcode;
                item.ITEM_DESC = itemdesc;
                item.QTY = qty;
                item.PRICE = price;
                item.AMOUNT = amount;
                item.COST_CODE = costcode;
                item.COST_NAME = costname;
                item.LOCATION_CODE = locationcode;
                item.LOCATION_NAME = location;
                item.REMARK = remark;
                list.push(item);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

            if (data) {
                var item = {
                    IDX: null,
                    ITEM_CODE: null,
                    ITEM_DESC: null,
                    QTY: null,
                    PRICE: null,
                    AMOUNT: null,
                    COST_CODE: null,
                    COST_NAME: null,
                    LOCATION_CODE: null,
                    LOCATION_NAME: null,
                    REMARK: null,
                }
                for (var i = 0; i < dataItems.length; i++) {
                    var itemcode = dataItems[i].get_cell("ITEM_CODE").innerText;
                    var itemdesc = dataItems[i].get_cell("ITEM_DESC").innerText;

                    if (data.PRODUCT_CODE == itemcode && data.PRODUCT_NAME == itemdesc) {
                        fn_OpenDocInformation('동일한 product 가 존재합니다.');
                        var is_val = true;
                        break;
                    }
                }
                if (is_val == false) {
                    maxIdx++;
                    item.IDX = parseInt(maxIdx);
                    item.ITEM_CODE = data.PRODUCT_CODE;
                    item.ITEM_DESC = data.PRODUCT_NAME + '(' + data.PRODUCT_CODE + ')';
                    item.QTY = 0;
                    item.PRICE = data.BASE_PRICE;
                    item.AMOUNT = 0;
                    item.COST_CODE = '';
                    item.COST_NAME = '';
                    item.LOCATION_CODE = '';
                    item.LOCATION_NAME = '';
                    item.REMARK = '';
                    list.push(item);
                    $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));                  

                }
                is_val = false;
            }
            return true;
        }

        //Product Popup Close
        function fn_setItem(oWnd, args) {
            var item = args.get_argument();
            if (item != null) {
                fn_UpdateGridData(item);
                $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("Rebind");
            }
        }

        function fn_setCostCenter(oWnd, args) {
            var item = args.get_argument();
            if (item != null) {
                fn_UpdateGridData();
                var row = $find('<%= radGrdSampleItemList.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
                var dataItem = $find(row.id);
                var currentIdx = '0';
                if(dataItem) 
                    currentIdx = dataItem.get_cell('IDX').innerText;
                $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("Costcenter:" + currentIdx + ":" + item.SUB_CODE + ":" + item.CODE_NAME);
            }
        }

        function openConfirmPopUp(index) {
            clickedKey = index;
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);
            fn_UpdateGridData(null);
            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                var masterTable = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdSampleItemList.ClientID%>');

            var dataItems = grid.get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
            }
        }

        function fn_OnGridNumBlur(sender, args)
        {
            setNumberFormat(sender);
        }

        function SetTotal() {
            var masterTable = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var total = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var amount = dataItems[i].get_cell("AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');

                amount = parseInt(amount);
                total += amount;
            }
            masterTable.get_element().tFoot.rows[0].cells[5].innerText = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function fn_UpdateGridDataFromUploader() {
            fn_UpdateGridData(false);
        }

        function fn_OnGridKeyUp(sender) {
            var row = $find('<%= radGrdSampleItemList.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            if (row == null) return;

            var dataItem = $find(row.id);
            if (dataItem) {
                var qty = 0, price = 0;
                var ctlQty = dataItem.findElement('radGrdNumQty');
                if (ctlQty) qty = ctlQty.value.replace(/,/gi, '').replace(/ /gi, '');
                var strPrice = dataItem.get_cell('PRICE').innerText;
                if (strPrice) price = strPrice.replace(/,/gi, '').replace(/ /gi, '');

                dataItem.get_cell('AMOUNT').innerText = (qty * price).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

            }

            SetTotal();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th>BG <span class="text_red">*</span></th>
                        <td colspan="3">
                            <div id="divSalesGroup" runat="server" style="margin: 0 0 0 0" >
                                <telerik:RadButton ID="radBtnCP" runat="server" Text="CP" Value="CP" GroupName="BU"
                                    AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" OnClientCheckedChanged="fn_OnBUChanged">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnIS" runat="server" Text="IS" Value="IS" GroupName="BU"
                                    AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" OnClientCheckedChanged="fn_OnBUChanged">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnBVS" runat="server" Text="BVS" Value="BVS" GroupName="BU"
                                    AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" OnClientCheckedChanged="fn_OnBUChanged" Visible="false">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnES" runat="server" Text="ES" Value="ES" GroupName="BU"
                                    AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" OnClientCheckedChanged="fn_OnBUChanged">
                                </telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>Category <span class="text_red">*</span></th>
                        <td>
                            <div id="divCagegory" runat="server" style="margin: 0 0 0 0">
                                <telerik:RadButton runat="server" ID="radRdoCategory1" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Demo & Free Sample" Value="Demo" GroupName="Category" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="radRdoCategory5" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="CMKT Campaign" Value="CMKT" GroupName="Category" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="radRdoCategory2" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="R&D Trial" Value="RD" GroupName="Category" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="radRdoCategory3" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="QC Test" Value="QC" GroupName="Category" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="radRdoCategory6" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Return Sample" Value="Return" GroupName="Category" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="radRdoCategory4" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Claim compensation" Value="Claim" GroupName="Category" AutoPostBack="false" Visible="false" ></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>목적 (Purpose) <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadTextBox ID="radTxtPurpose" runat="server" Width="98%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>배송지</th>
                        <td>
                            <telerik:RadTextBox ID="radTxtAddress" runat="server" Width="98%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>직송처 주소</th>
                        <td>
                            <telerik:RadTextBox ID="radTxtDirectAddress" runat="server" Width="98%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>직송처 전화번호</th>
                        <td>
                            <telerik:RadTextBox ID="radTxtDirectPhone" runat="server" Width="30%">
                                <ClientEvents OnKeyPress="fn_keyPress" />
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Approved e-WORKFlow number</th>
                        <td>
                            <telerik:RadTextBox ID="radTxtRelevant_e_WorkflowNo" runat="server" Width="100%"></telerik:RadTextBox></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <h3>Product <span class="text_red">*</span>
        <div class="title_btn">
            <telerik:RadButton ID="radBtnAdd" runat="server" Text="Add" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" AutoPostBack="false"
                ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" OnClientClicked="fn_PopupItem">
            </telerik:RadButton>
        </div>
    </h3>
    <telerik:RadGrid ID="radGrdSampleItemList" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" ShowFooter="true"
        EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" GridLines="None"
        OnItemCommand="radGrdSampleItemList_ItemCommand" OnPreRender="radGrdSampleItemList_PreRender" AllowAutomaticUpdates="true">
        <MasterTableView EnableHeaderContextMenu="true" EditMode="Batch" DataKeyNames="IDX">
            <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
            <Columns>
                <telerik:GridBoundColumn DataField="IDX" HeaderStyle-Width="30px" UniqueName="IDX" ReadOnly="true"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ITEM_CODE" HeaderText="Item Num" UniqueName="ITEM_CODE" HeaderStyle-Width="90px" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ITEM_DESC" HeaderText="Product<br/>제품명(코드)" UniqueName="ITEM_DESC" HeaderStyle-Width="60%" ReadOnly="true"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="QTY" HeaderText="Quantity<br/>수량" UniqueName="QTY" HeaderStyle-Width="60px" DataType="System.Decimal">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("QTY")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <%--<telerik:RadNumericTextBox runat="server" ID="radGrdNumQty" NumberFormat-DecimalDigits="0" 
                                Width="100%" EnabledStyle-HorizontalAlign="Right" ClientEvents-OnBlur="fn_OnGridNumBlur"></telerik:RadNumericTextBox>--%>
                        <asp:TextBox ID="radGrdNumQty" runat="server" Width="100%" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="return fn_OnGridKeyUp(this)"
                            DecimalDigits="3">                                
                        </asp:TextBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="PRICE" HeaderText="Price<br/>단가" UniqueName="PRICE" HeaderStyle-Width="90px" DataType="System.Decimal">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("PRICE")) %>'></asp:Label>
                    </ItemTemplate>
                    </telerik:GridTemplateColumn>
                <telerik:GridCalculatedColumn HeaderText="Amount" UniqueName="AMOUNT" DataType="System.Double" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Right" 
                    DataFields="QTY, PRICE" Expression='{0}*{1}' Aggregate="Sum" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}" FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                </telerik:GridCalculatedColumn>
                <telerik:GridBoundColumn DataField="COST_CODE" HeaderText="" HeaderStyle-Width="40px" UniqueName="COST_CODE" Display="false">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="COST_NAME" UniqueName="COST_NAME" HeaderText="Cost Center" HeaderStyle-Width="180px">
                    <ItemTemplate><%# Eval("COST_NAME")%></ItemTemplate>
                    <EditItemTemplate>
                        <telerik:RadTextBox ID="radGrdTxtCostCenter" runat="server" Width="85%" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadButton ID="radGrdBtncostCenter" runat="server" AutoPostBack="false" Width="18px" Height="18px" CssClass="btn_grid" OnClientClicked="fn_OpenCostCenter">
                            <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                        </telerik:RadButton>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="LOCATION_CODE" HeaderText="" HeaderStyle-Width="40px" UniqueName="LOCATION_CODE" Display="false">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="LOCATION_NAME" UniqueName="LOCATION_NAME" HeaderText="Location" HeaderStyle-Width="100px">
                    <ItemTemplate><%# Eval("LOCATION_NAME")%></ItemTemplate>
                    <EditItemTemplate>
                        <telerik:RadDropDownList ID="radDropLocation" runat="server" Width="100%" DropDownWidth="75px"></telerik:RadDropDownList>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn DataField="REMARK" UniqueName="REMARK" HeaderText="Remark" HeaderStyle-Width="40%">
                    <ItemTemplate><%# Eval("REMARK")%></ItemTemplate>
                    <EditItemTemplate>
                        <%--<telerik:RadTextBox ID="radGrdTxtRemark" runat="server" Width="98%"></telerik:RadTextBox>--%>
                        <asp:TextBox ID="radGrdTxtRemark" runat="server" CssClass="input" Width="100%"></asp:TextBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="40px">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" OnClientClick='<%# String.Format("return openConfirmPopUp({0});",Eval("IDX"))%> '
                            ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinSampleRequestItemList" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product" Width="520px" Height="580px" Behaviors="Default" NavigateUrl="./ProductList.aspx" OnClientClose="fn_setItem"></telerik:RadWindow>
        </Windows>
        <Windows>
            <telerik:RadWindow ID="radWinCostCenter" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Cost Center" Width="430px" Height="580px" Behaviors="Default" NavigateUrl="./CostCenter.aspx" OnClientClose="fn_setCostCenter"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div id="hiddenArea" runat="server">
        <input type="hidden" id="hddProcessID" runat="server" />
        <input type="hidden" id="hddProcessStatus" runat="server" />
        <input type="hidden" id="hddDocumentID" runat="server" />
        <input type="hidden" id="hddGridItems" runat="server" />
        <input type="hidden" id="hddCostCenter" runat="server" />
        <input type="hidden" id="hddReuse" runat="server" />
	    <input type="hidden" id="hddAddRow" runat="server" value="N" />
    </div>
</asp:Content>

