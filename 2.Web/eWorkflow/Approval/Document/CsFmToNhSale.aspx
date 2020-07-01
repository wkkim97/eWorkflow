<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="CsFmToNhSale.aspx.cs" Inherits="Approval_Document_CsFmToNhSlaes" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
   <script type="text/javascript">

        function pageLoad() {
            var addRow = $('#<%= hddAddRow.ClientID %>').val();

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

        function getSelectedBG() {
            var controls = $('#<%= divBU.ClientID %>').children();
            var selectedValue;

            for (var i = 0; i < controls.length; i++) {
                var bu = controls[i];
                if ($find(bu.id).get_checked()) {
                    selectedValue = $find(bu.id).get_value();
                    break;
                }
            }
            return selectedValue;
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

        //PRODUCT POPUP
        function fn_PopupItem(sender, args) {
            fn_UpdateGridData(null);
            var dataItems = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView().get_dataItems();

            if (dataItems.length > 0) {
                for (var i = 0; i < dataItems.length; i++) {

                    var ProductCode = dataItems[i].get_cell("PRODUCT_CODE").innerText.trim();
                    var ProductName = dataItems[i].get_cell("PRODUCT_NAME").innerText.trim();
                    var qty = dataItems[i].get_cell("QTY").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                    var price = dataItems[i].get_cell("PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                    var amount = dataItems[i].get_cell("AMOUNT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                    if (ProductCode == '' || ProductName == '') {
                        fn_OpenDocInformation('자료를 입력하시기 바랍니다.');
                        return false;
                    }
                }
            }
            var selectedValue = "";
            var selectedCategoryValue = "";

            // 2017-06-02 Youngwoo Lee
            // CS FM to NH sale에서는 Invoice price를 사용한다(I). 만약 base price 를 사용할 경우에는 'Y'를 이용한다.
            // procedure eManage.dbo.USP_SELECT_PRODUCT 수정

            selectedValue = getSelectedBG();
            selectedCategoryValue = "I";

            var wnd = $find("<%= radWinSampleRequestItemList.ClientID %>");

            wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=" + selectedValue + "&baseprice=" + selectedCategoryValue);
            wnd.show();
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

                var price = dataItems[i].get_cell("PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var amount = dataItems[i].get_cell("AMOUNT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var ship_to_code = dataItems[i].get_cell("SHIP_TO_CUSTOMER_NH").innerText.trim();
                var ship_to_name = dataItems[i].get_cell("SHIP_TO_CUSTOMER_NH_NAME").innerText.trim();

                var product = {
                    IDX: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    QTY: null,
                    PRICE: null,
                    AMOUNT: null,
                    SHIP_TO_CUSTOMER_NH: null,
                    SHIP_TO_CUSTOMER_NH_NAME: null,

                }
                maxIdx = idx;
                product.IDX = parseInt(idx);
                product.PRODUCT_CODE = ProductCode;
                product.PRODUCT_NAME = ProductName;
                product.QTY = qty;
                product.PRICE = price;
                product.AMOUNT = amount;
                product.SHIP_TO_CUSTOMER_NH = ship_to_code;
                product.SHIP_TO_CUSTOMER_NH_NAME = ship_to_name;

                list.push(product);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

            if (data) {
                var product = {
                    IDX: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    QTY: null,
                    PRICE: null,
                    AMOUNT: null,
                    SHIP_TO_CUSTOMER_NH: null,
                    SHIP_TO_CUSTOMER_NH_NAME: null,

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
                    product.PRICE = data.BASE_PRICE;
                    product.AMOUNT = 0;
                    product.SHIP_TO_CUSTOMER_NH = ship_to_code;
                    product.SHIP_TO_CUSTOMER_NH_NAME = ship_to_name;
                    
                    list.push(product);
                   
                    $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));
                    

                }
                is_val = false;
            }
            
            return true;
        }


        //Product Popup Close
        function fn_setItem(oWnd, args) {
            var product = args.get_argument();
            
            if (product != null) {
                
                fn_UpdateGridData(product);
                $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("Rebind");
            }
        }


        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdSampleItemList.ClientID%>');

            var dataItems = grid.get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
            }
        }

        function fn_OnGridKeyUp(sender) {
            var row = $find('<%= radGrdSampleItemList.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            
            if (row == null) return;
            
            var dataItem = $find(row.id);
            
            if (dataItem) {
                
                var qty = 0, price = 0, additional=0;

                var ctlQty = dataItem.findElement('radGrdNumQty');
                if (ctlQty) qty = ctlQty.value.replace(/,/gi, '').replace(/ /gi, '');

                var strPrice = dataItem.get_cell('PRICE').innerText;
                if (strPrice) price = strPrice.replace(/,/gi, '').replace(/ /gi, '');

                dataItem.get_cell('AMOUNT').innerText = (qty * price).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");


            } else {
                sender.value = 0;
            }

            SetTotal();
        }

       function SetTotal() {
            var masterTable = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var ProductAmount = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var amount = dataItems[i].get_cell("AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');
                amount = parseInt(amount);
                ProductAmount += amount;
            }

            masterTable.get_element().tFoot.rows[0].cells[5].innerText = ProductAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            fn_UpdateGridData();
         }

        function fn_OnGridNumBlur(sender, args) {
            setNumberFormat(sender);
        }


       function fn_OpenCustomerWinSoldTo(sender, args) {
            var wnd = $find("<%= RadWinPopupSoldTo.ClientID %>");
            wnd.setUrl("/eWorks/Common/Popup/CustomerList.aspx");
            wnd.show();
            sender.set_autoPostBack(false);
        }

        function fn_ClientClose_SoldTo(oWnd, args) {
            var item = args.get_argument();

            if (item != null) {
                var txtcustomer = $find("<%= radTxtSoldTo.ClientID%>");
                txtcustomer.set_value(item.CUSTOMER_NAME.trim() + " (" + item.CUSTOMER_CODE + ")");
                $('#<%= hddSOLD_TO_CUSTOMER_FM.ClientID %>').val(item.CUSTOMER_CODE);
            }
            else {
                oWnd.close();
            }
        }



        //Ship to customer NH -- CostCenter Popup
        function fn_OpenShipToNH(sender, eventArgs) {
            var masterTable = $find('<%= radGrdSampleItemList.ClientID%>').get_masterTableView();
            var num = masterTable.get_dataItems().length;
            var wnd = $find("<%= radWinCostCenter.ClientID %>");

            wnd.setUrl("/eWorks/Common/Popup/CustomerList.aspx");
            wnd.show();
            sender.set_autoPostBack(false);
        }
        
        function fn_setShipToNH(oWnd, args) {
            var product = args.get_argument();
            if (product != null) {
                fn_UpdateGridData();
                var row = $find('<%= radGrdSampleItemList.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
                var dataItem = $find(row.id);
                var currentIdx = '0';
                if (dataItem)
                    currentIdx = dataItem.get_cell('IDX').innerText;
                $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("Ship_to_nh:" + currentIdx + ":" + product.CUSTOMER_CODE + ":" + product.CUSTOMER_NAME.trim() + " (" + product.CUSTOMER_CODE + ")");
            }
        }


<%--       function fn_OpenCustomerWinShipTo(sender, args) {
            var wnd = $find("<%= RadWinPopupShipTo.ClientID %>");
            wnd.setUrl("/eWorks/Common/Popup/CustomerList.aspx");
            wnd.show();
            sender.set_autoPostBack(false);
        }--%>

<%--        function fn_ClientClose_ShipTo(oWnd, args) {
            var item = args.get_argument();

            if (item != null) {
                var txtcustomer = $find("<%= radTxtShipTo.ClientID%>");
                txtcustomer.set_value(item.CUSTOMER_NAME.trim() + " (" + item.CUSTOMER_CODE + ")");
                $('#<%= hddSHIP_TO_CUSTOMER_NH.ClientID %>').val(item.CUSTOMER_CODE);
            }
            else {
                oWnd.close();
            }
        }--%>
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" Runat="Server">
   <div class="doc_style">
        
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%" />
                    <col />
                </colgroup>
                    <tr>
                        <th>BG <span class="text_red">*</span></th>
                        <td colspan="3">
                            <div id="divBU" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton ID="radBtnCP" runat="server" Text="CP" Value="CP" GroupName="BU"
                                    AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" Checked="true">
                                </telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                <tr>
                    <th>Soldto Customer(FM) <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtSoldTo" runat="server" ReadOnly="true" Width="96%"></telerik:RadTextBox>
                        <telerik:RadButton ID="radBtnCustomer" runat="server" AutoPostBack="false" CssClass="btn_grid" Width="18px" Height="18px" 
                            OnClientClicked="fn_OpenCustomerWinSoldTo">
                            <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                        </telerik:RadButton>
                        <input type="hidden" id="hddSOLD_TO_CUSTOMER_FM" runat="server" />
                    </td>
                </tr>
<%--                <tr>
                    <th>Shipto Customer(NH) <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtShipTo" runat="server" ReadOnly="true" Width="96%"></telerik:RadTextBox>
                        <telerik:RadButton ID="RadButton1" runat="server" AutoPostBack="false" CssClass="btn_grid" Width="18px" Height="18px" 
                            OnClientClicked="fn_OpenCustomerWinShipTo">
                            <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                        </telerik:RadButton>
                        <input type="hidden" id="hddSHIP_TO_CUSTOMER_NH" runat="server" />
                    </td>
               </tr>--%>
               <tr>
                    <th>File Template </th>
                    <td>
                        <asp:Label runat="server" ID="lblLink" Visible="true">* <a href="http://sp-coll-bbs.ap.bayer.cnb/sites/000141/Teamsites/NEW%20e-workflow/LINKED_DATA/Template_CS_FM_to_NH_Sale.xlsx?Web=1" target="_blank">Link</a>를 클릭하여 양식을 작성한 후에 첨부하여 주시기 바랍니다.</asp:Label>
                    </td>
                </tr>
            </table>

            <div id="divProduct">
            <h3>Product <span class="text_red">*</span>
            <div class="title_btn">
                <telerik:RadButton ID="radBtnAdd" runat="server" Text="Add" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" AutoPostBack="false"  Enabled="true" 
                    ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" OnClientClicked="fn_PopupItem">
                </telerik:RadButton>
            </div>
        </h3>
        <telerik:RadGrid ID="radGrdSampleItemList" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" ShowFooter="true"
            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" GridLines="None"
            OnItemCommand="radGrdSampleItemList_ItemCommand"  OnItemDataBound="radGrdSampleItemList_ItemBound" OnPreRender="radGrdSampleItemList_PreRender" AllowAutomaticUpdates="true">
            <MasterTableView EditMode="Batch" DataKeyNames="IDX">
                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderStyle-Width="30px" UniqueName="IDX" ReadOnly="true"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PRODUCT_CODE" HeaderText="Item Num" UniqueName="PRODUCT_CODE" HeaderStyle-Width="90px" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PRODUCT_NAME" HeaderText="Description" UniqueName="PRODUCT_NAME" HeaderStyle-Width="50%" ReadOnly="true"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="QTY" HeaderText="Qty" UniqueName="QTY" HeaderStyle-Width="60px" DataType="System.Decimal">
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
                    <telerik:GridTemplateColumn DataField="PRICE" HeaderText="Price" UniqueName="PRICE" HeaderStyle-Width="80px" DataType="System.Decimal">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("PRICE")) %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridCalculatedColumn HeaderText="Amount" UniqueName="AMOUNT" DataType="System.Double" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Right" 
                        DataFields="QTY, PRICE" Expression='{0}*{1}' Aggregate="Sum" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}" FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                    </telerik:GridCalculatedColumn>

                    <telerik:GridBoundColumn DataField="SHIP_TO_CUSTOMER_NH" HeaderText="" HeaderStyle-Width="40px" UniqueName="SHIP_TO_CUSTOMER_NH" Display="false">
                    </telerik:GridBoundColumn>


                    <telerik:GridTemplateColumn DataField="SHIP_TO_CUSTOMER_NH_NAME" UniqueName="SHIP_TO_CUSTOMER_NH_NAME" HeaderText="Shipto Customer(NH)" HeaderStyle-Width="300px">
                        <ItemTemplate><%# Eval("SHIP_TO_CUSTOMER_NH_NAME")%></ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="radGrdTxtCostCenter" runat="server" Width="85%" ReadOnly="true"></telerik:RadTextBox>
                            <telerik:RadButton ID="radGrdBtncostCenter" runat="server" AutoPostBack="false" Width="18px" Height="18px" CssClass="btn_grid" OnClientClicked="fn_OpenShipToNH">
                                <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                            </telerik:RadButton>
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
        </div>



        </div>
    </div>

    <%--Popup--%>  
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinSampleRequestItemList" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product" Width="520px" Height="580px" Behaviors="Default" NavigateUrl="./ProductList.aspx" OnClientClose="fn_setItem"></telerik:RadWindow>
        </Windows>
        <Windows>
            <telerik:RadWindow ID="RadWinPopupSoldTo" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer"
                Width="430px" Height="600px" Behaviors="Default" Modal="true" OnClientClose="fn_ClientClose_SoldTo" NavigateUrl="./CustomerList.aspx">
            </telerik:RadWindow>
        </Windows>
<%--        <Windows>
            <telerik:RadWindow ID="RadWinPopupShipTo" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer"
                Width="430px" Height="600px" Behaviors="Default" Modal="true" OnClientClose="fn_ClientClose_ShipTo" NavigateUrl="./CustomerList.aspx">
            </telerik:RadWindow>
        </Windows>--%>

        <Windows>
            <telerik:RadWindow ID="radWinCostCenter" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="CUSTOMER" Width="430px" Height="580px" Behaviors="Default" NavigateUrl="./CustomerList.aspx" OnClientClose="fn_setShipToNH"></telerik:RadWindow>
        </Windows>      
                
    </telerik:RadWindowManager>



    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddCompanyCode" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
    <input type="hidden" id="hddGridItems" runat="server" />
    <input type="hidden" id="hddCostCenter" runat="server" />

</asp:Content>


