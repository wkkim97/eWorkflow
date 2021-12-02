<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="ARAdjustment.aspx.cs" Inherits="Approval_Document_ARAdjustment" %>

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
            var controls = $('#<%= divBG.ClientID %>').children();
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

        function fn_OnWholeSalerRequesting(sender, args) {
            var context = args.get_context();
            context["company"] = $('#<%= hddCompanyCode.ClientID %>').val();
            context["bu"] = '';
            context["parvw"] = '';
            context["level"] = 'N';
        }

        function fn_OnProductRequesting(sender, args) {
            var context = args.get_context();
            context["company"] = $('#<%= hddCompanyCode.ClientID %>').val();
            context["bu"] = '';
            context["baseprice"] = 'N';
        }

        function setVisibleControl(value, title, channel) {
            if (value == "CP") {
                $('#rowDistributionChannel').show();               
            }            
            else if (value == 'IS' || value == 'BVS' || value == 'ES') {
                $('#rowDistributionChannel').hide();
            }

            if (title == "PAR")  {
                $('#<%= divOverLimit.ClientID %>').show();  
                $('#<%= divLink.ClientID %>').show();                
            }
            else if (title == 'FRS' || title == 'MPL' || title == 'CRE') {
                $('#<%= divOverLimit.ClientID %>').hide();
                $('#<%= divLink.ClientID %>').hide();
            }

            if (channel == 'NH') {
                $('#rowDistributionChannel').show();
                $('#<%= divNH.ClientID%>').show();
            }
            else if (channel == 'FM') {
                $('#rowDistributionChannel').show();
                $('#<%= divNH.ClientID%>').hide();
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

        function Radiocheck(sender, args) {
            var channel = sender.get_text();
            $('#<%= hddNH.ClientID%>').val(channel);            
            setVisibleControl('', '', channel);
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
                    var additional = dataItems[i].get_cell("ADDITIONAL").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                    var addition_amount = dataItems[i].get_cell("ADDITION_AMOUNT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                    var affected_percent = dataItems[i].get_cell("AFFECTED_PERCENT").innerText;
                    if (ProductCode == '' || ProductName == '') {
                        fn_OpenDocInformation('자료를 입력하시기 바랍니다.');
                        return false;
                    }
                }
            }
            var selectedValue = getSelectedBG();


            //var selectedCategoryValue = getSelectedCategory();
            var selectedCategoryValue = "";

            // 2015-09-11 : Youngwoo Lee
            // Sample Request 에서는 Claim compensation  일 경우에는 Invoice price 를 그 외에는 base price 를 가져올 수 있게 파라미터 값 수정
            // 2017-01-11 : Youngwoo Lee
            // BCS AR Adjustment 에서는 Invoice price를 사용한다. 만약 base price 를 사용할 경우에는 'Y'를 이용한다.
            // 2017-02-01 : Youngwoo Lee
            // BCS AR Adjustment 에서는 radRdoDisCha2 버튼이 선택되었을 경우(NH 일 경우) INVOICE_PRICE_NH를 사용한다. "N' 으로 설정

            var strNH = $find('<%=radRdoDisCha2.ClientID%>');
            if (strNH.get_checked()) {
                selectedCategoryValue = "N"
            }
            else {
                selectedCategoryValue = "I";
            }

           var wnd = $find("<%= radWinSampleRequestItemList.ClientID %>");
           if (selectedValue == "") {
              fn_OpenDocInformation("BG을 선택하여 주세요");
                return;
           }
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
  
        //-----------------------------------
        //그리드 클라이언트 데이터 업데이트
        //-----------------------------------        
        function fn_UpdateGridData(data) {
            var list = [];
            var masterTable = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var is_val = false;
            var maxIdx = 0;
    
            var NHcheck = $find('<%= radRdoDisCha2.ClientID%>').get_checked();
            

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

                var product_rb = dataItems[i].get_cell("PRODUCT_RB").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var additional = '0';
                if (dataItems[i].findElement('radGrdNumAdditional'))
                    additional = dataItems[i].findElement('radGrdNumAdditional').value.replace(/,/gi, '').replace(/ /gi, '');
                else
                    additional = dataItems[i].get_cell("ADDITIONAL").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

               

                var addition_amount = dataItems[i].get_cell("ADDITION_AMOUNT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var affected_percent = dataItems[i].get_cell("AFFECTED_PERCENT").innerText;

               // var remark = '';
                //remark = dataItems[i].get_cell("REMARK").innerText.trim();


                var product = {
                    IDX: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    QTY: null,
                    PRICE: null,
                    AMOUNT: null,
                    PRODUCT_RB: null,
                    ADDITIONAL: null,
                    ADDITION_AMOUNT: null,
                    AFFECTED_PERCENT: null,
              //      REMARK: null,
                }
                maxIdx = idx;
                product.IDX = parseInt(idx);
                product.PRODUCT_CODE = ProductCode;
                product.PRODUCT_NAME = ProductName;
                product.QTY = qty;
                product.PRICE = price;
                product.AMOUNT = amount;
                product.PRODUCT_RB = product_rb;
                product.ADDITIONAL = additional;
                product.ADDITION_AMOUNT = addition_amount;
                product.AFFECTED_PERCENT = affected_percent;
               // product.REMARK = remark;
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
                    PRODUCT_RB: null,
                    ADDITIONAL: null,
                    ADDITION_AMOUNT: null,
                    AFFECTED_PERCENT: null,

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
                    product.PRODUCT_RB = 0;
                    product.ADDITIONAL = 0;
                    product.ADDITION_AMOUNT = 0;
                    product.AFFECTED_PERCENT = 0;
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
            var grid = $find('<%=radGrdSampleItemList.ClientID%>');

            var dataItems = grid.get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
            }
        }

        function SetTotal() {
            var masterTable = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var total = 0, addition_total=0, GrandTotal = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var amount = dataItems[i].get_cell("AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');
                var addition_amount = dataItems[i].get_cell("ADDITION_AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');
                amount = parseInt(amount);
                total += amount;

                addition_amount = parseInt(addition_amount);
                addition_total += addition_amount;
            }

            

            masterTable.get_element().tFoot.rows[0].cells[5].innerText = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            masterTable.get_element().tFoot.rows[0].cells[9].innerText = addition_total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $find('<%= radNumAmount.ClientID %>').set_value(addition_total);
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

                var ctlAdditional = dataItem.findElement('radGrdNumAdditional');
                if (ctlAdditional) additional = ctlAdditional.value.replace(/,/gi, '').replace(/ /gi, '');


                dataItem.get_cell('AMOUNT').innerText = (qty * price).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                dataItem.get_cell('ADDITION_AMOUNT').innerText = (qty * additional).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                dataItem.get_cell('AFFECTED_PERCENT').innerText = (additional/price*100).toFixed(1).toString();

            }

            SetTotal();
        }
        function fn_OnGridNumBlur(sender, args) {
            setNumberFormat(sender);
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <h3></h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Title <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadButton ID="radRdoTitle1" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                            Text="Final Rebate Settlement" Value="FRS" AutoPostBack="false" GroupName="Title" OnClientClicked="fn_OnTitleCheckedChanged"  Visible="false" >
                        </telerik:RadButton>
                        <telerik:RadButton ID="radRdoTitle2" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                            Text="Proposal Negotiation Rebate" Value="PAR" AutoPostBack="false" GroupName="Title" OnClientClicked="fn_OnTitleCheckedChanged"  Visible="false" >
                        </telerik:RadButton>
                        <telerik:RadButton ID="radRdoTitle3" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                            Text="Miscellaneous profit and Loss (잡수익/잡손실)" Value="MPL" AutoPostBack="false" GroupName="Title" OnClientClicked="fn_OnTitleCheckedChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radRdoTitle4" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                            Text="Customer reimbursement" Value="CRE" AutoPostBack="false" GroupName="Title" OnClientClicked="fn_OnTitleCheckedChanged">
                        </telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <th>BG <span class="text_red">*</span></th>
                    <td>
                        <div id="divBG" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoBgCP" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="CP" Value="CP" AutoPostBack="false" OnClientClicked="fn_OnBgCheckedChanged"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBgIS" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="IS" Value="IS" AutoPostBack="false" OnClientClicked="fn_OnBgCheckedChanged"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBgBVS" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="BVS" Value="BVS" AutoPostBack="false" OnClientClicked="fn_OnBgCheckedChanged"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBgES" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="ES" Value="ES" AutoPostBack="false" OnClientClicked="fn_OnBgCheckedChanged"></telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr id="rowDistributionChannel" style="display:none" value="channel">
                    <th>Distribution Channel</th>
                    <td>
                        <telerik:RadButton ID="radRdoDisCha1" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                            AutoPostBack="false" GroupName="Distribution" Checked="true"
                            Text="FM" Value="FM" OnClientClicked="Radiocheck" >
                        </telerik:RadButton>
                        <telerik:RadButton ID="radRdoDisCha2" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                            AutoPostBack="false" GroupName="Distribution"
                            Text="NH" Value="NH" OnClientClicked="Radiocheck">
                        </telerik:RadButton>
                        <div id="divNH" runat="server" style="display:none" >                            
                            <telerik:RadButton ID="radRdoGp" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                                AutoPostBack="false" GroupName="NHGroup"
                                Text="Group purchasing(연합구매)" Value="GP">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radRdoEP" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                                AutoPostBack="false" GroupName="NHGroup"
                                Text="Extend new market / Price competition(개별조정)" Value="EP">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radRdoSub" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                                AutoPostBack="false" GroupName="NHGroup"
                                Text="Subsidy(보조사업)" Value="Subsidy">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radRdoOther" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                                AutoPostBack="false" GroupName="NHGroup"
                                Text="Others(기타)" Value="Others">
                            </telerik:RadButton>                            
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Customer Name / Number</th>
                    <td>
                        <telerik:RadAutoCompleteBox ID="radAcomWholesaler" runat="server" AllowCustomEntry="false" Width="100%" DropDownWidth="300px"
                            OnClientRequesting="fn_OnWholeSalerRequesting">
                            <WebServiceSettings Method="SearchCustomer" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                        </telerik:RadAutoCompleteBox>
                        <asp:Label ID="lblNotCustomer" runat="server" Width="100%" Visible="false">Refer to the customer list attached.</asp:Label>

                    </td>
                </tr>
                </table>
            
                    <h3>Product
                    <div class="title_btn">
                        <telerik:RadButton ID="radBtnAdd" runat="server" Text="Add" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" AutoPostBack="false"
                            ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" OnClientClicked="fn_PopupItem">
                        </telerik:RadButton>
                    </div>
                </h3>
                <telerik:RadGrid ID="radGrdSampleItemList" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" ShowFooter="true"
                    EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" GridLines="None"
                    OnItemCommand="radGrdSampleItemList_ItemCommand" AllowAutomaticUpdates="true">
                    <MasterTableView EnableHeaderContextMenu="true" EditMode="Batch" DataKeyNames="IDX">
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
                            <telerik:GridTemplateColumn DataField="PRICE" HeaderText="Unit Price" UniqueName="PRICE" HeaderStyle-Width="80px" DataType="System.Decimal">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("PRICE")) %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridCalculatedColumn HeaderText="Amount" UniqueName="AMOUNT" DataType="System.Double" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Right" 
                                DataFields="QTY, PRICE" Expression='{0}*{1}' Aggregate="Sum" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}" FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                            </telerik:GridCalculatedColumn>

                            <telerik:GridTemplateColumn DataField="PRODUCT_RB" HeaderText="Product R/B" UniqueName="PRODUCT_RB" HeaderStyle-Width="90px" DataType="System.Decimal">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label21" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("PRODUCT_RB")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="radGrdNumProductRB" runat="server" Width="100%" CssClass="input align_right"
                                        onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                        DecimalDigits="3">                                
                                    </asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
               

                            <telerik:GridTemplateColumn DataField="ADDITIONAL" HeaderText="Additional" UniqueName="ADDITIONAL" HeaderStyle-Width="90px" DataType="System.Decimal">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("ADDITIONAL")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="radGrdNumAdditional" runat="server" Width="100%" CssClass="input align_right"
                                        onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                        onkeyup="return fn_OnGridKeyUp(this)"
                                        DecimalDigits="3">                                
                                    </asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn DataField="AFFECTED_PERCENT" HeaderText="%" UniqueName="AFFECTED_PERCENT" HeaderStyle-Width="50px" DataType="System.Decimal">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# String.Format("{0:#,##0.##}", Eval("AFFECTED_PERCENT")) %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>  

                            <telerik:GridCalculatedColumn 
                                HeaderText="Addition amount" UniqueName="ADDITION_AMOUNT" DataType="System.Double" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Right" 
                                DataFields="QTY, ADDITIONAL" Expression='{0}*{1}' Aggregate="Sum" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}" FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
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

                <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
               
<%--                <tr>
                    <th>Product Name / Number</th>
                    <td>
                        <telerik:RadAutoCompleteBox ID="radAcomProduct" runat="server" AllowCustomEntry="false" Width="100%" DropDownWidth="400px"
                            OnClientRequesting="fn_OnProductRequesting">
                            <WebServiceSettings Method="SearchProduct" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                        </telerik:RadAutoCompleteBox>
                        <asp:Label ID="lblNotProduct" runat="server" Width="100%" Visible="false">Refer to the product list attached.</asp:Label>
                    </td>
                </tr>--%>

                <tr>
                    <th>Amount / Reason <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadNumericTextBox ID="radNumAmount" runat="server" NumberFormat-DecimalDigits="0"
                            EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Left" Width="120px">
                        </telerik:RadNumericTextBox>
                        <span id="divOverLimit" runat="server" style="display:none">
                            <telerik:RadButton runat="server" ID="radChkOverLimit" ButtonType="ToggleButton" ToggleType="CheckBox" Text="기준 초과 (Over Limit)" Value="Y" AutoPostBack="false"></telerik:RadButton>                                                       
                        </span>
                        <div id="divLink" runat="server" style="display:none">
                            <asp:Label runat="server" ID="lblLink" Visible="true">* <a href="http://sp-coll-bbs.ap.bayer.cnb/sites/000141/Teamsites/NEW%20e-workflow/LINKED_DATA/Template_Adjustment_BCS.xlsx" target="_blank">Link</a>을 클릭하여 조정장려금 양식을 작성한 후에 첨부하여 주시기 바랍니다.</asp:Label>
                                <br> 
                                    &nbsp;&nbsp; Reason &nbsp;&nbsp;
                                   <telerik:RadDropDownList ID="radDdlReason" runat="server" Width="90%" DropDownWidth="590px" DefaultMessage="---Select---">
                                        <Items>
                                            <telerik:DropDownListItem Text="Maintain big volume by customer : Sales history (min. 1000 package)" Value="Maintain" />
                                            <telerik:DropDownListItem Text="Negotiation for volume increase vs. PY : Sales history (+20% over vs. PY)" Value="Volume" />
                                            <telerik:DropDownListItem Text="Development of new market: crops Strategic/business decision ( Sales history)" Value="Development" />
                                            <telerik:DropDownListItem Text="Low competitive price (Price information on competitors)" Value="Low" />
                                            <telerik:DropDownListItem Text="Pre-season sales( NH safety-stock sales for next season Business decision)" Value="Pre-season" />
                                            <telerik:DropDownListItem Text="Cash sales(capturing market chance of late season, business decision)" Value="Cash" />
                                            <telerik:DropDownListItem Text="Applied system: non-coverage area by system(Description on system issues)" Value="Applied" />
                                            <telerik:DropDownListItem Text="Subsidy business ( NH channel)Documentation subsidy business" Value="Subsidy" />
                                            <telerik:DropDownListItem Text="Others: ( Detail information)Case by case" Value="Others" />
                                        </Items>
                                    </telerik:RadDropDownList>

                                <div id="divCheck" runat="server" style="width: 100%; margin: 0 0 0 0">
                                    <telerik:RadButton ID="radChkPurpose1" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                        Text="Sales story check" Value="0001">
                                    </telerik:RadButton>
                                    <telerik:RadButton ID="radChkPurpose2" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                        Text="Minimum 1,000" Value="0002">
                                    </telerik:RadButton>
                                    <telerik:RadButton ID="radChkPurpose3" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                        Text="Volume increase 120%" Value="0003">
                                    </telerik:RadButton>
                                    <telerik:RadButton ID="radChkPurpose4" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                        Text="Competition product price check" Value="0004">
                                    </telerik:RadButton>
                                </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Comment <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radTxtPurpose" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>





    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinSampleRequestItemList" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product" Width="520px" Height="580px" Behaviors="Default" NavigateUrl="./ProductList.aspx" OnClientClose="fn_setItem"></telerik:RadWindow>
        </Windows>
        
    </telerik:RadWindowManager>
        </div>



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

