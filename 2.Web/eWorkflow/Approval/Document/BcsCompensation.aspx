<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="BcsCompensation.aspx.cs" Inherits="Approval_Document_BcsCompensation" %>

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

        function fn_OnConpensationChanged(sender, args) {
            var value = sender.get_value();
            var check = sender.get_checked();

            setVisibleCompensation(value, check, '');
            fn_ToTal_Amount_new();
        }

        function fn_OnBUChanged(sender, args) {
            var value = sender.get_value();
            var check = sender.get_checked();
            var productamount = $find('<%=radNumProductAmount.ClientID%>');
            var totalamount = $find('<%=radNumTotalAmount.ClientID%>');
            var cashamount = $('#<%=radNumAmount.ClientID%>');
            productamount.set_value(0);
            totalamount.set_value(0);
            cashamount.val(0);

            if (value == "IS") {
                $find('<%= radChkProduct.ClientID %>').set_enabled(false);
                $find('<%= radChkProduct.ClientID %>').set_checked();
                value = "Product";
                check = false;
                
                
            }
            else if (value == "CP" || value == "ES") {
                $find('<%= radChkProduct.ClientID %>').set_enabled(true);
                $find('<%= radChkProduct.ClientID %>').set_checked(true);
                value = "Product";
                check = true;
            }

            setVisibleCompensation(value, check, '');
            fn_ToTal_Amount_new();
        }

        function fn_OnComplaintTypChanged(sender, args) {
            var value = sender.get_value();
            var check = sender.get_checked();

            var strCommet = "Complaint Type Changed! ==> " + value;

            if (value == "EP") {
                $find('<%= radMaskTxtComplaintReportNo.ClientID %>').set_value("CL-2");
            }
            else if (value == "QL") {
                $find('<%= radMaskTxtComplaintReportNo.ClientID %>').set_value("QA-2");
            }
        }
        


        function setVisibleCompensation(value, check, channel) {
            var masterTable = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView();
            
            var productamount = $find('<%=radNumProductAmount.ClientID%>');
            var totalamount = $find('<%=radNumTotalAmount.ClientID%>');

            if (value == "Product") {
                //  var addbutton = $find('<%=radBtnAdd.ClientID%>');
                var addbutton = $find('<%=radBtnAdd.ClientID%>');
                
                if (check) {
                   // addbutton.set_enabled(true);
                    $('#divProduct').show();
                }
                else {
                   
                    $('#<%= hddGridItems.ClientID%>').val('');
                    productamount.set_value(0);
                    $('#divProduct').hide();
                    
                }
            }
            else if (value == "Cash") {
                var priceamount = $('#<%=radNumAmount.ClientID%>');
                priceamount.val(0);
                if (check) {
                    priceamount.attr('readonly',false);                
                }
                else {
                    priceamount.attr('readonly',true);                 
                }
            }

        }
        
        function fn_ToTal_Amount_new(sender) {
            
            if ($find('<%= radChkCash.ClientID %>').get_checked) {

                CashAmout = $('#<%=radNumAmount.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');

                CashAmout = CashAmout.replace(/,/gi, '').replace(/ /gi, '');
            }

            if ($find('<%= radChkProduct.ClientID %>').get_checked) {
                ProductAmount = $find('<%= radNumProductAmount.ClientID %>').get_editValue();

                ProductAmount = ProductAmount.replace(/,/gi, '').replace(/ /gi, '');
            }

            TotalAmount = parseInt(ProductAmount) + parseInt(CashAmout);

            $find('<%= radNumTotalAmount.ClientID %>').set_value(TotalAmount);
            $('#<%=radNumAmount.ClientID%>').val(CashAmout.replace(/\B(?=(\d{3})+(?!\d))/g, ","));

        }

        function fn_ToTal_Amount(sender, args) {
            
            var strCashAmout, CashAmout, ProductAmount, TotalAmount
            //alert($('#<%=radNumAmount.ClientID%>').val());
            return;
            if ($find('<%= radChkCash.ClientID %>').get_checked) {
                
                // CashAmout = $find('<%= radNumAmount.ClientID %>').val();
                
                CashAmout = $('#<%=radNumAmount.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');

                CashAmout = CashAmout.replace(/,/gi, '').replace(/ /gi, '');
            }

            if ($find('<%= radChkProduct.ClientID %>').get_checked) {
                ProductAmount = $find('<%= radNumProductAmount.ClientID %>').get_editValue();

                ProductAmount = ProductAmount.replace(/,/gi, '').replace(/ /gi, '');
            }


            TotalAmount = parseInt(ProductAmount) + parseInt(CashAmout);

            $find('<%= radNumTotalAmount.ClientID %>').set_value(TotalAmount);
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

        function setVisibleControl(value, title, BusinessUnit) {
            if (BusinessUnit == "IS") {
                $('#divProduct').hide();
            }
            else {
                $('#divProduct').show();
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
                
                var masterTable = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
              
                SetTotal();
                

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
            var selectedValue = getSelectedBG();

            //var selectedCategoryValue = getSelectedCategory();
            var selectedCategoryValue = "";

            // BCS Compensation 에서는 Invoice price를 사용한다. 만약 base price 를 사용할 경우에는 'Y'를 이용한다.
            // procedure eManage.dbo.USP_SELECT_PRODUCT 수정

           selectedCategoryValue = "I";
          

           var wnd = $find("<%= radWinSampleRequestItemList.ClientID %>");
           if (selectedValue == "") {
              fn_OpenDocInformation("Business Unit을 선택하여 주세요");
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

        //CostCenter Popup
        function fn_OpenCostCenter(sender, eventArgs) {
            var masterTable = $find('<%= radGrdSampleItemList.ClientID%>').get_masterTableView();
            var num = masterTable.get_dataItems().length;
            var wnd = $find("<%= radWinCostCenter.ClientID %>");
            var classcode = "S027";

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
                var ProductCode = dataItems[i].get_cell("PRODUCT_CODE").innerText.trim();
                var ProductName = dataItems[i].get_cell("PRODUCT_NAME").innerText.trim();


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



                var remark = dataItems[i].get_cell("REMARK").innerText;

               // var remark = '';
                //remark = dataItems[i].get_cell("REMARK").innerText.trim();


                var product = {
                    IDX: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
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
                product.IDX = parseInt(idx);
                product.PRODUCT_CODE = ProductCode;
                product.PRODUCT_NAME = ProductName;
                product.QTY = qty;
                product.PRICE = price;
                product.AMOUNT = amount;
                product.COST_CODE = costcode;
                product.COST_NAME = costname;
                product.LOCATION_CODE = locationcode;
                product.LOCATION_NAME = location;
                product.REMARK = remark;
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
                    COST_CODE: null,
                    COST_NAME: null,
                    LOCATION_CODE: null,
                    LOCATION_NAME: null,
                    REMARK: null,

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
                    product.COST_CODE = '';
                    product.COST_NAME = '';
                    product.LOCATION_CODE = '';
                    product.LOCATION_NAME = '';
                    product.REMARK = '';
                    
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

        function fn_setCostCenter(oWnd, args) {
            var product = args.get_argument();
            if (product != null) {
                fn_UpdateGridData();
                var row = $find('<%= radGrdSampleItemList.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
                var dataItem = $find(row.id);
                var currentIdx = '0';
                if (dataItem)
                    currentIdx = dataItem.get_cell('IDX').innerText;
                $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("Costcenter:" + currentIdx + ":" + product.SUB_CODE + ":" + product.CODE_NAME);
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
            var CashAmount = 0, ProductAmount = 0, TotalAmount = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var amount = dataItems[i].get_cell("AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');
                amount = parseInt(amount);
                ProductAmount += amount;
            }

            $find('<%= radNumProductAmount.ClientID %>').set_value(ProductAmount);
            masterTable.get_element().tFoot.rows[0].cells[5].innerText = ProductAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            fn_UpdateGridData();
            fn_ToTal_Amount_new();

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
                //alert(sender);
                //alert(sender.value);
                sender.value = 0;
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

        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Title <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadTextBox ID="radTxtTitle" EmptyMessage="10 글자 이상 50 글자 미만으로 입력" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Business Unit <span class="text_red">*</span></th>
                    <td>
                        <div id="divBU" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoBU_CP" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BusinessUnit" Text="Crop Protection" Value="CP" AutoPostBack="true" OnClientClicked="fn_OnBUChanged" OnClick="radChkProduct_ItemSelected" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBU_ES" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BusinessUnit" Text="Environmental Science" Value="ES" AutoPostBack="true" OnClientClicked="fn_OnBUChanged" OnClick="radChkProduct_ItemSelected"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBU_IS" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BusinessUnit" Text="Industrial Sales" Value="IS" AutoPostBack="true" OnClientClicked="fn_OnBUChanged" OnClick="radChkProduct_ItemSelected"></telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Complaint Type <span class="text_red">*</span></th>
                    <td>
                        <div id="divComplaintType" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoCT_EP" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="ComplaintType" Text="Efficacy & Phytotoxicity" Value="EP" AutoPostBack="false" OnClientClicked="fn_OnComplaintTypChanged"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoCT_QL" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="ComplaintType" Text="Quality & Logistics" Value="QL" AutoPostBack="false" OnClientClicked="fn_OnComplaintTypChanged"></telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>No of Customer Complaint Report </th>
                    <td>
                        <telerik:RadMaskedTextBox ID="radMaskTxtComplaintReportNo"  RenderMode="Lightweight" Text='<%# Eval("MaskedData") %>' runat="server" Mask="LL-####-###" > 
                        </telerik:RadMaskedTextBox> &nbsp;&nbsp;&nbsp;&nbsp;(고객 불만보고서 번호)
                    </td>
                </tr>
                <tr>
                    <th>Description / Reason </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtReason" EmptyMessage="보상을 하게된 경위와 합의사항 등 참고할 사항을 서술 합니다. " runat="server" Width="100%" TextMode="MultiLine" Height="40px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr >
                    <th rowspan ="2">People receiving compensation</th>
                    <td>
                        <telerik:RadTextBox ID="radTxtCompensationName"  EmptyMessage="성명(또는 상호)" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                 </tr>
                 <tr>
                    <td>
                        <telerik:RadTextBox ID="radTxtCompensationAddresss" EmptyMessage="주소" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Compensation Type <span class="text_red">*</span></th>
                    <td>
                        <div id="divCompensationType" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radChkProduct" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="true" Text="Product" Value="Product" OnClientClicked="fn_OnConpensationChanged" OnClick="radChkProduct_ItemSelected"></telerik:RadButton>
                            <telerik:RadButton ID="radChkCash" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="true" Text="Cash" Value="Cash" OnClientClicked="fn_OnConpensationChanged"></telerik:RadButton>
                        </div>
                    </td>
                </tr>
            </table>
                 <div id="divProduct" style="display:none">
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
                </div>
                <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                
                <tr id="rowCash">
                    <th>Amount <span class="text_red">*</span></th>
                    <td>Cash
                        <asp:TextBox ID="radNumAmount" runat="server" Width="70px" CssClass="input align_right"
                           
                            onkeyup="return fn_ToTal_Amount_new(this)" 
                            DecimalDigits="0" Text="0" >                                
                        </asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Product Amount
                        <telerik:RadNumericTextBox ID="radNumProductAmount" runat="server" NumberFormat-DecimalDigits="0" 
                            EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Left" Width="120px" Value="0" ReadOnly="true" >
                        </telerik:RadNumericTextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Total Amount
                        <telerik:RadNumericTextBox ID="radNumTotalAmount" runat="server" NumberFormat-DecimalDigits="0"
                            EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Left" Width="120px" Value="0"  ReadOnly="true">
                        </telerik:RadNumericTextBox>
                    </td>

                </tr>
                <tr>
                    <th>Attachment Check <span class="text_red">*</span></th>
                    <td>
                        <div id="divCheck" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radChkCheck1" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="1.증거자료" Value="0001">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkCheck2" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="2.고객불만보고서" Value="0002">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkCheck3" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="3.1단계 실무자협의회 회의록(영업지점장 소집)" Value="0003">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkCheck4" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="4.고객불만 조사보고서" Value="0004">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkCheck5" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="5.2단계 실무자협의회 회의록(코디네이터 소집)" Value="0005">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkCheck6" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="6.PQSC 회의록" Value="0006">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkCheck7" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="7.합의서" Value="0007">
                            </telerik:RadButton>

                        </div>
                    </td>
                </tr>
                </div>
                <tr>
                    <th>Remark </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtRemark" EmptyMessage="제품 보상에 대한 참고사항을 서술합니다." runat="server" Width="100%" TextMode="MultiLine" Height="40px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>* 승인단계에 따른 증빙의 첨부 (보상금액 범위 확인).</tr>
                <tr style="background:rgba(230, 230, 230, 1)">
                    <td width="18%"> 승인자 </td>
                    <td width="28%">  보상금액 범위</td>
                    <td> 첨부 서류 </td>
                </tr>
                <tr>
                    <td> 1. 영업지역팀장 </td>
                    <td> 100만원 이하(요청자의 팀장) </td>
                    <td> 1.증거자료, 2.고객불만보고서, 3.1단계 실무자협의회 회의록(영업지점장 소집), 7.합의서  </td>
                </tr>
                <tr>
                    <td> 2. CCL </td>
                    <td> 100만원 초과 - 500만원 이하 (영업 본부장) </td>
                    <td> 1.증거자료, 2.고객불만보고서, 4.고객불만 조사보고서, </br>5.2단계 실무자협의회 회의록(코디네이터 소집), 7.합의서 </td>
                </tr>
                <tr>
                    <td> 3. CCL </td>
                    <td> 500만원 초과 (대표이사) </td>
                    <td> 1.증거자료, 2.고객불만보고서, 4.고객불만 조사보고서, 6.PQSC 회의록(PQSC에 해당하는 경우), 7.합의서  </td>
                </tr>
            </table>




    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinSampleRequestItemList" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product" Width="520px" Height="580px" Behaviors="Default" NavigateUrl="./ProductList.aspx" OnClientClose="fn_setItem"></telerik:RadWindow>
        </Windows>
        <Windows>
            <telerik:RadWindow ID="radWinCostCenter" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Cost Center" Width="430px" Height="580px" Behaviors="Default" NavigateUrl="./CostCenter.aspx" OnClientClose="fn_setCostCenter"></telerik:RadWindow>
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
    <input type="hidden" id="hddCostCenter" runat="server" />
    <input type="hidden" id="hddNH" runat="server" />
</asp:Content>

