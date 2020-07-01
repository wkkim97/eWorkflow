<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="CsSalesRecognition.aspx.cs" Inherits="Approval_Document_CsSalesRecognition" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
<script type="text/javascript">

    function pageLoad() {
        var addRow = $('#<%= hddAddRow.ClientID %>').val();
        $('#<%= subsidycase.ClientID %>').show(); 
        if (addRow == 'Y') {
            var grid = $find('<%=radGrdSampleItemList.ClientID%>');

            if (grid.get_masterTableView()) {
                var dataItems = grid.get_masterTableView().get_dataItems();
                if (dataItems.length > 0) {
                    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                }
            }
            $('#<%= hddAddRow.ClientID %>').val('Y');
        }
    }
        function checkLastProduct() {
        var masterTable = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView();
        var dataItems = masterTable.get_dataItems();
        if (dataItems.length > 0) {
            var lastItem = dataItems[dataItems.length - 1];
        }
        return true;
        }

    function fn_OnProduct(sender, args) {
        var context = args.get_context();
        context["company"] = $('#<%= hddCompanyCode.ClientID %>').val();
        context["bu"] = '';
        context["baseprice"] = 'N';
    }

    function fn_OpenCustomer(sender, args) {
        var wnd = $find("<%= radWinPopupCustomer.ClientID %>");
        wnd.setUrl("/eWorks/Common/Popup/CustomerListForCS.aspx");
        wnd.show();
        sender.set_autoPostBack(false);
            
        }

    function fn_setCustomer(oWnd, args) {
        var item = args.get_argument();

        if (item != null) {
                
            var txt = $find("<%= radGrdTxtCustomer.ClientID %>");
            txt.set_value(item.CUSTOMER_NAME.trim() + "(" + item.CUSTOMER_CODE + ")_" + item.PARVW);
            $('#<%= hddCustomerCode.ClientID %>').val(item.CUSTOMER_CODE);
            $('#<%= hddCustomerType.ClientID %>').val(item.PARVW);
            //$('#<%= Labelcustomertype.ClientID %>').text($('#<%= hddCustomerType.ClientID %>').val());
                
            $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("removegrid");
        }
        else
            return false;
    }

    var clickedKey = null;
    function openConfirmPopUp(index) {
        clickedKey = index;
        fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);
           
        return false;
    }

    function confirmCallBackFn(arg) {
        if (arg) {
            fn_UpdateGridData();
            var masterTable = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView();
            masterTable.fireCommand("Remove", clickedKey);
        }
    }

    //PRODUCT POPUP
    function fn_PopupItem(sender, args) {                        
        var selectedCategoryValue = "";
        var controlsBU = $('#<%= divBU.ClientID %>').children();
        for (var i = 0; i < controlsBU.length; i++) {
            var bg = controlsBU[i];                                     //BU
            if ($find(bg.id).get_checked()) {
                selectedValue = $find(bg.id).get_value();               //선택한 BU의 Value
                break;
            }
        }
        var customervalue = $find("<%= radGrdTxtCustomer.ClientID %>").get_value();
        if (customervalue == "") {
            fn_OpenDocInformation("customer 을 선택하여 주세요");
            return;
        }
        if (checkLastProduct()) {
            var wnd = $find("<%= radWinSampleRequestItemList.ClientID %>");
            //wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=CP");
            wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=" + selectedValue);
            wnd.show();
            sender.set_autoPostBack(false);
        } else {
            fn_OpenDocInformation('자료를 입력바랍니다.');
            sender.set_autoPostBack(false);
        }
    }
        //Product Popup Close
    function fn_setItem(oWnd, args) {
        var product = args.get_argument();
        if (product != null) {
            fn_UpdateGridData(product);
            $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("Rebind");
        }
    }
    function fn_UpdateGridDataFromUploader() {
        fn_UpdateGridData(null);
    }
    function fn_DoRequest(sender, arsg) {
        return fn_UpdateGridData();
    }
    function fn_DoSave(sender, args) {
        return fn_UpdateGridData();
    }
  
    //-----------------------------------
    //그리드 클라이언트 데이터 업데이트
    //-----------------------------------        
    function fn_UpdateGridData(data) {
        var list = [];
        var masterTable = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView();
        var dataItems = masterTable.get_dataItems();
        var is_val = false;
        var maxIdx = 0;    
            
        for (var i = 0; i < dataItems.length; i++) {
            var idx = dataItems[i].get_cell("IDX").innerText.trim();
            var ProductCode = dataItems[i].get_cell("PRODUCT_CODE").innerText.trim();
            var ProductName = dataItems[i].get_cell("PRODUCT_NAME").innerText.trim();

            var qty = '0';
            if (dataItems[i].findElement('radGrdNumQty'))
                qty = dataItems[i].findElement('radGrdNumQty').value.replace(/,/gi, '').replace(/ /gi, '');
            else
                qty = dataItems[i].get_cell("QTY").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');


            var product = {
                IDX: null,
                PRODUCT_CODE: null,
                PRODUCT_NAME: null,
                QTY: null
            }
            maxIdx = idx;
            product.IDX = parseInt(idx);
            product.PRODUCT_CODE = ProductCode;
            product.PRODUCT_NAME = ProductName;
            product.QTY = qty;
            list.push(product);
        }
        $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

        if (data) {
            var product = {
                IDX: null,
                PRODUCT_CODE: null,
                PRODUCT_NAME: null,
                QTY: null

            }
            for (var i = 0; i < dataItems.length; i++) {
                var ProductCode = dataItems[i].get_cell("PRODUCT_CODE").innerText;
                var ProductName = dataItems[i].get_cell("PRODUCT_NAME").innerText;

                if (data.PRODUCT_CODE == ProductCode && data.PRODUCT_NAME == ProductName) {
                    fn_OpenDocInformation('동일한 product 가 존재합니다.');
                    var is_val = true;
                    break;
                }
            }
               
            if (is_val == false) {
                maxIdx++;
                product.IDX = parseInt(maxIdx);
                product.PRODUCT_CODE = data.PRODUCT_CODE;
                product.PRODUCT_NAME = data.PRODUCT_NAME + '(' + data.PRODUCT_CODE + ')';
                product.QTY = 0;                    
                   
                list.push(product);
                   
                $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));
                    

            }
            is_val = false;
        }
            
        return true;
    }

       

    function openGridRowForEdit(sender, args) {
        if (args.get_eventArgument() == 'Rebind') {
            var grid = $find('<%=radGrdSampleItemList.ClientID%>');

            var dataItems = grid.get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
            }
        }
    }

        

    function fn_OnGridKeyUp(sender) {
        var row = $find('<%= radGrdSampleItemList.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
        if (row == null) return;

        var dataItem = $find(row.id);
        if (dataItem) {
            var qty = 0, strPrice = 0, additional = 0, proposal_price = 0, volume = 0;

            var ctlQty = dataItem.findElement('radGrdNumQty');
            if (ctlQty) qty = ctlQty.value.replace(/,/gi, '').replace(/ /gi, '');
        }

    }

    function fn_OnGridNumBlur(sender, args) {
        setNumberFormat(sender);
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <h3>Customer & Reason</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Business Unit <span class="text_red">*</span></th>
                    <td>
                        <div id="divBU" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoCP" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="CP" Value="CP" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoIS" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="IS" Value="IS" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoES" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="ES" Value="ES" AutoPostBack="false" ></telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Customer Name <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radGrdTxtCustomer" runat="server" ReadOnly="true" AutoPostBack="false" Width="90%">
                        </telerik:RadTextBox>
                        <telerik:RadButton ID="radGrdBtnCustomer" runat="server" CssClass="btn_grid" AutoPostBack="false" Width="18px" Height="18px" OnClientClicked="fn_OpenCustomer">
                            <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                        </telerik:RadButton>
                        <div style="display:none;width:100%">
                            <asp:Label ID="Labelcustomertype" runat="server" ></asp:Label>
                        </div>
                    </td>
                </tr>
 
                <tr>
                    <th>Reason <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadDropDownList ID="radDdlReason" runat="server" Width="100%" AutoPostBack="false" DefaultMessage="---Select---"  DropDownWidth="350px">
                            <Items>
                                <telerik:DropDownListItem Text="보조사업" Value="보조사업" />
                                <telerik:DropDownListItem Text="긴급방제" Value="긴급방제" />
                                <telerik:DropDownListItem Text="긴급납품" Value="긴급납품" />
                                <telerik:DropDownListItem Text="기타" Value="기타" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <th>Comment </th>
                    <td>
                        <telerik:RadTextBox ID="RadTextComment" runat="server" Width="100%" TextMode="MultiLine" Height="50px"></telerik:RadTextBox>
                    </td>
                </tr>               
                               
                </table>
 
               <div id="subsidycase" runat="server"  >
                    <h3>Product
                    <div class="title_btn">
                        <telerik:RadButton ID="radBtnAdd" runat="server" Text="Add" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" AutoPostBack="false"
                            ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" OnClientClicked="fn_PopupItem">
                        </telerik:RadButton>
                    </div>
                </h3>
                <telerik:RadGrid ID="radGrdSampleItemList" runat="server" runat="server" AutoGenerateColumns="false"
                    EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid"
                     AllowSorting="false" GridLines="None" 
                    OnItemCommand="radGrdSampleItemList_ItemCommand" OnItemDataBound="radGrdProduct_ItemDataBound"   >
                    <MasterTableView  EditMode="Batch" DataKeyNames="IDX">
                        <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="IDX" HeaderText="IDX" HeaderStyle-Width="30px" UniqueName="IDX" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PRODUCT_CODE" HeaderText="Item Num" UniqueName="PRODUCT_CODE" HeaderStyle-Width="0px" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PRODUCT_NAME" HeaderText="Description" UniqueName="PRODUCT_NAME" HeaderStyle-Width="100px" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="QTY" HeaderText="Qty" UniqueName="QTY" HeaderStyle-Width="60px" >
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("QTY")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="radGrdNumQty" runat="server" Width="100%" CssClass="input align_right"
                                        onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                        onkeyup="return fn_OnGridKeyUp(this)"
                                        DecimalDigits="3">                                
                                    </asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="20px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" OnClientClick='<%# String.Format("return openConfirmPopUp({0});",Eval("IDX"))%> '
                                        ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>

    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinSampleRequestItemList" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product" Width="520px" Height="580px" Behaviors="Default" NavigateUrl="./ProductList.aspx" OnClientClose="fn_setItem"></telerik:RadWindow>
        </Windows>
         <Windows>
            <telerik:RadWindow ID="radWinPopupCustomer" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer" Width="550px" Height="500px" Behaviors="Default" NavigateUrl="./customerforcs.aspx" OnClientClose="fn_setCustomer"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
        </div>



    </div>
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddCompanyCode" runat="server" />
    <input type="hidden" id="hddCustomerCode" runat="server" />
    <input type="hidden" id="hddCustomerType" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
    <input type="hidden" id="hddGridItems" runat="server" />
    <input type="hidden" id="hddNH" runat="server" />
</asp:Content>

