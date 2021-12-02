<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="BvsSeedImport.aspx.cs" Inherits="Approval_Document_BvsSeedImport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">


        function fn_Type(sender, args) {
           

        }

        function pageLoad() {
            var addRow = $('#<%= hddAddRow.ClientID %>').val();

            if (addRow == 'Y') {
                var grid = $find('<%=radGrdProduct.ClientID%>');

                if (grid.get_masterTableView()) {
                    var dataItems = grid.get_masterTableView().get_dataItems();
                    if (dataItems.length > 0) {
                        grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                    }
                }
                $('#<%= hddAddRow.ClientID %>').val('Y');
            }

        }

        function fn_OnBgCheckedChanged(sender, args) {
            var value = sender.get_text();
            setVisibleControl(value, '', '');
        }

        function fn_OnTitleCheckedChanged(sender, args) {
            var title = sender.get_value();

            setVisibleControl('', title, '');

        }

        function openConfirmPopUp(index) {
            clickedKey = index;
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);
            fn_UpdateGridData(null);
            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        //PRODUCT POPUP
        function fn_PopupItem(sender, args) {
            fn_UpdateGridData(null);
            var dataItems = $find('<%= radGrdProduct.ClientID %>').get_masterTableView().get_dataItems();

            if (dataItems.length > 0) {
                for (var i = 0; i < dataItems.length; i++) {

                    var ProductCode = dataItems[i].get_cell("PRODUCT_CODE").innerText.trim();
                    var ProductName = dataItems[i].get_cell("PRODUCT_NAME").innerText.trim();
                    var qty_EA = dataItems[i].get_cell("QTY_EA").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                    var price = dataItems[i].get_cell("TP_PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                    var amount = dataItems[i].get_cell("AMOUNT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                    var factor = dataItems[i].get_cell("FACTOR").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                    var qty_TH = dataItems[i].get_cell("QTY_TH").innerText;
                    if (ProductCode == '' || ProductName == '') {
                        fn_OpenDocInformation('자료를 입력하시기 바랍니다.');
                        return false;
                    }
                }
            }
            var selectedValue = "";
            var selectedCategoryValue = "";

            // 2017-01-18 Youngwoo Lee
            // BVS Seeds Import Requst 에서는 Invoice price를 사용한다(I). 만약 base price 를 사용할 경우에는 'Y'를 이용한다.
            // procedure eManage.dbo.USP_SELECT_PRODUCT 수정

            selectedValue = "BVS";
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
            var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var is_val = false;
            var maxIdx = 0;
           
            
            for (var i = 0; i < dataItems.length; i++) {
                
                var idx = dataItems[i].get_cell("IDX").innerText.trim();
                var ProductCode = dataItems[i].get_cell("PRODUCT_CODE").innerText.trim();
                var ProductName = dataItems[i].get_cell("PRODUCT_NAME").innerText.trim();

                var qty_ea = '0';
                if (dataItems[i].findElement('radGrdNumQty'))
                    qty_ea = dataItems[i].findElement('radGrdNumQty').value.replace(/,/gi, '').replace(/ /gi, '');
                else
                    qty_ea = dataItems[i].get_cell("QTY_EA").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var price = dataItems[i].get_cell("TP_PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var amount = dataItems[i].get_cell("AMOUNT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var factor = dataItems[i].get_cell("FACTOR").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var qty_th = dataItems[i].get_cell("QTY_TH").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var crop = dataItems[i].get_cell("CROP").innerText.trim();
                var variety = dataItems[i].get_cell("VARIETY").innerText.trim();
                var sf_st = dataItems[i].get_cell("SF_ST").innerText.trim();


                // var remark = '';
                //remark = dataItems[i].get_cell("REMARK").innerText.trim();


                var product = {
                    IDX: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    CROP: null,
                    VARIETY: null,
                    SF_ST: null,
                    QTY_EA: null,
                    TP_PRICE: null,
                    AMOUNT: null,
                    FACTOR: null,
                    QTY_TH: null,
                    //      REMARK: null,
                }
                maxIdx = idx;
                product.IDX = parseInt(idx);
                product.PRODUCT_CODE = ProductCode;
                product.PRODUCT_NAME = ProductName;
                product.CROP=crop;
                product.VARIETY = variety;
                product.SF_ST = sf_st;

                product.QTY_EA = qty_ea;
                product.TP_PRICE = price;
                product.AMOUNT = amount;
                product.FACTOR = factor;
                product.QTY_TH = qty_th;
                // product.REMARK = remark;
                list.push(product);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

            if (data) {
                var product = {
                    IDX: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    CROP: null,
                    VARIETY: null,
                    SF_ST: null,
                    QTY_EA: null,
                    TP_PRICE: null,
                    AMOUNT: null,
                    FACTOR: null,
                    QTY_TH: null,

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
                    product.CROP = data.CROP;
                    product.VARIETY = data.VARIETY;
                    product.SF_ST = data.SF_ST;
                    product.QTY_EA = 0;
                    product.TP_PRICE = data.TP_PRICE
                    product.AMOUNT = 0;
                    product.FACTOR = data.FACTOR;
                    product.QTY_TH = 0;
                    //   product.REMARK = '';

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
            var grid = $find('<%=radGrdProduct.ClientID%>');

            var dataItems = grid.get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
            }
        }

        function SetTotal() {
            var masterTable = $find('<%= radGrdProduct.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var total = 0, qty_th = 0, qty_th_toal = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var amount = dataItems[i].get_cell("AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');
                var qty_th = dataItems[i].get_cell("QTY_TH").innerText.replace(/,/gi, '').replace(/ /gi, '');
                amount = parseInt(amount);
                total += amount;

                qty_th = parseInt(qty_th);
                qty_th_toal += qty_th;
            }

           // masterTable.get_element().tFoot.rows[0].cells[8].innerText = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
          //  masterTable.get_element().tFoot.rows[0].cells[10].innerText = qty_th_toal.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

        }

        function fn_OnGridKeyUp(sender) {
            var row = $find('<%= radGrdProduct.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            if (row == null) return;
            
            var dataItem = $find(row.id);
           
            if (dataItem) {
                var qty = 0, price = 0, factor= 0, strQty_TH = 0;

                var ctlQty = dataItem.findElement('radGrdNumQty');                
                if (ctlQty) qty = ctlQty.value.replace(/,/gi, '').replace(/ /gi, '');

                var strPrice = dataItem.get_cell('TP_PRICE').innerText;
                if (strPrice) price = strPrice.replace(/,/gi, '').replace(/ /gi, '');

                var strFactor= dataItem.get_cell('FACTOR').innerText;
                if (strFactor) factor = strFactor.replace(/,/gi, '').replace(/ /gi, '');

                var strQty_TH = qty * factor

                dataItem.get_cell('AMOUNT').innerText = (qty * price).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                dataItem.get_cell('QTY_TH').innerText = (qty * factor).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

            }

            SetTotal();
        }
        function fn_OnGridNumBlur(sender, args) {
            setNumberFormat(sender);
        }

        function fn_Radio(sender, args) {
            var value = sender.get_text();

            return fn_UpdateGridData(null);
            SetTotal();
        }

        function setVisibleControl(value) {
            if (value == "Commercial")
                $('#tblCheckBox').hide();
            else
                $('#tblCheckBox').hide();
        }

        function fn_OnIncentiveCheckedChanged(sender, args) {
            if (args.get_checked())
                $('#tblCheckBox').hide();
            else
                $('#tblCheckBox').hide();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <div class="data_type1">
            <table>
                <colgroup>
                    <col />
                    <col style="width: 75%;" />
                </colgroup>
                <tbody>
                    <tr id="rowType" >
                        <th>Type<span class="text_red">*</span></th>
                        <td>
                            <div id="radRaidoType" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton runat="server" ID="RadrdoCommercial" ButtonType="ToggleButton" ToggleType="Radio" Text="Commercial" Value="Commercial" GroupName="Category" OnClientClicked="fn_Radio" OnClick="RadrdoNew_Click" OnClientCheckedChanged="fn_OnIncentiveCheckedChanged"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="RadrdoSample" ButtonType="ToggleButton" ToggleType="Radio" Text="Sample" Value="Sample" GroupName="Category" OnClientClicked="fn_Radio" OnClick="RadrdoNew_Click" ></telerik:RadButton>

                                <table id="tblCheckBox" style="margin-bottom: 5px; display: none;">
                                    <tr>
                                        <th> &nbsp;매 품종별 검역용 샘플 3EA가 추가요청됩니다.</th>
                                    </tr>
                                </table>
                            </div>

                        </td>
                    </tr>
                    <tr id="ExpectedDate">
                        <th>Expected Date <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadDatePicker runat="server" ID="RadExpectedDate" Calendar-ShowRowHeaders="false" Width="120px" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr id="rowVendor" >
                        <th>Vendor<span class="text_red">*</span></th>
                        <td>
                            <telerik:RadDropDownList ID="radDropVendor" runat="server" Width="300px" DefaultMessage="--- Select ---" >
                                    <Items>
                                        <telerik:DropDownListItem Text="Beijing Seeds Co.,Ltd." Value="Beijing Seeds Co.,Ltd." />
                                        <telerik:DropDownListItem Text="India Pvt. Ltd." Value="India Pvt. Ltd." />
                                        <telerik:DropDownListItem Text="Netherlands B.V." Value="Netherlands B.V." />
                                        <telerik:DropDownListItem Text="USA, Inc." Value="USA, Inc." />
                                    </Items>
                                </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Remark</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtRemark" TextMode="MultiLine" Height="40" Width="99%"></telerik:RadTextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
            
        
                    <h3>Product <span class="text_red">*</span>
                    <div class="title_btn">
                        <telerik:RadButton ID="radBtnAdd" runat="server" Text="Add" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" AutoPostBack="false"
                            ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" OnClientClicked="fn_PopupItem">
                        </telerik:RadButton>
                    </div>
                </h3>
                <telerik:RadGrid ID="radGrdProduct" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" ShowFooter="false"
                    EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" GridLines="None"
                    OnItemCommand="radGrdProduct_ItemCommand" AllowAutomaticUpdates="true" OnItemDataBound="radGrdProduct_ItemDataBound" >
                    <MasterTableView EnableHeaderContextMenu="true" EditMode="Batch" DataKeyNames="IDX" >
                        <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="IDX" HeaderStyle-Width="30px" UniqueName="IDX" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PRODUCT_CODE" HeaderText="Code" UniqueName="PRODUCT_CODE" HeaderStyle-Width="90px" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PRODUCT_NAME" HeaderText="Description" UniqueName="PRODUCT_NAME" HeaderStyle-Width="50%" ReadOnly="true"></telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="CROP" HeaderText="Crop" UniqueName="CROP" HeaderStyle-Width="50px" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="VARIETY" HeaderText="VARIETY" UniqueName="VARIETY" HeaderStyle-Width="80px" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SF_ST" HeaderText="SF/ST" UniqueName="SF_ST" HeaderStyle-Width="50px" ReadOnly="true"></telerik:GridBoundColumn>

                            <telerik:GridTemplateColumn DataField="TP_PRICE" HeaderText="Price" UniqueName="TP_PRICE" HeaderStyle-Width="70px" DataType="System.Decimal">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("TP_PRICE")) %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                                       
                             <telerik:GridTemplateColumn DataField="QTY_EA" HeaderText="QTY_EA" UniqueName="QTY_EA" HeaderStyle-Width="70px" DataType="System.Decimal">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("QTY_EA")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="radGrdNumQty" runat="server" Width="100%" CssClass="input align_right"
                                        onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                        onkeyup="return fn_OnGridKeyUp(this)"
                                        DecimalDigits="3">                                
                                    </asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridCalculatedColumn HeaderText="Amount" UniqueName="AMOUNT" DataType="System.Double" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Right" 
                                DataFields="QTY_EA, TP_PRICE" Expression='{0}*{1}' Aggregate="Sum" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}" FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                            </telerik:GridCalculatedColumn>      

                            <telerik:GridTemplateColumn DataField="FACTOR" HeaderText="Factor" UniqueName="FACTOR" HeaderStyle-Width="70px" DataType="System.Decimal">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("FACTOR")) %>'></asp:Label>
                                </ItemTemplate>                               
                            </telerik:GridTemplateColumn>

                            <telerik:GridCalculatedColumn 
                                HeaderText="QTY_TH" UniqueName="QTY_TH" DataType="System.Double" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right" 
                                DataFields="QTY_EA, FACTOR" Expression='{0}*{1}' Aggregate="Sum" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}" FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                            </telerik:GridCalculatedColumn>


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
        
    </telerik:RadWindowManager>

    </div>


    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddCompanyCode" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
    <input type="hidden" id="hddGridItems" runat="server" />
    <input type="hidden" id="hddNH" runat="server" />

</asp:Content>

