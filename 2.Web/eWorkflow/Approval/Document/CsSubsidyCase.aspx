<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="CsSubsidyCase.aspx.cs" Inherits="Approval_Document_CsSubSidy" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">

        function pageLoad() {
            var addRow = $('#<%= hddAddRow.ClientID %>').val();
            $('#<%= exceptioncase.ClientID %>').hide();
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
            var NHCcheck = $find('<%= radRdoNH.ClientID%>').get_checked();
            var FMCcheck = $find('<%= radRdoFM.ClientID%>').get_checked();
            if (!NHCcheck && !FMCcheck) {
                fn_OpenDocInformation("Distribution Channel 을 선택하여 주세요");
                return;
            }
            var wnd = $find("<%= radWinPopupCustomer.ClientID %>");
            wnd.setUrl("/eWorks/Common/Popup/CustomerListForCS.aspx");
            wnd.show();
            sender.set_autoPostBack(false);
            
         }

        function fn_setCustomer(oWnd, args) {
            var item = args.get_argument();

            if (item != null) {
                var DCcheck = $find('<%= radRdoNH.ClientID%>').get_checked();
                if (DCcheck) {
                    item.PARVW = "NH";
                } else {
                    if (item.PARVW == "IE") {
                        item.PARVW = "Retailer";
                    } else {
                        item.PARVW = "WS";
                    };
                }
                
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
        function fn_OnBgCheckedChanged(sender, args) {
            var bg = sender.get_value();
        }
    
        function fn_OnTitleCheckedChanged(sender, args) {
            var title = sender.get_value();
            setVisibleControl('', title, '');
        }
        function setVisibleControl(value, title, channel) {
            if (title == "Exception") {
                $('#<%= exceptioncase.ClientID %>').show();    
                $('#<%= subsidycase.ClientID %>').hide(); 
            } else {
                $('#<%= exceptioncase.ClientID %>').hide();
                $('#<%= subsidycase.ClientID %>').show(); 
            }
        }

        function Radiocheck(sender, args) {
            var channel = sender.get_text();
            $('#<%= hddNH.ClientID%>').val(channel);            
            //setVisibleControl('', '', channel);
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

            var customervalue= $find("<%= radGrdTxtCustomer.ClientID %>").get_value();
            var NHCcheck = $find('<%= radRdoNH.ClientID%>').get_checked();
            var FMCcheck = $find('<%= radRdoFM.ClientID%>').get_checked();
            if (customervalue == "") {
                fn_OpenDocInformation("customer 을 선택하여 주세요");
                return;
            }
            if (!NHCcheck && !FMCcheck ) {
                fn_OpenDocInformation("Distribution Channel 을 선택하여 주세요");
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
            //if($find('<%= radRdoTitle1.ClientID%>').get_checked()) return true;
            var masterTable = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var is_val = false;
            var maxIdx = 0;    
            var NHcheck = $find('<%= radRdoNH.ClientID%>').get_checked();
            
            for (var i = 0; i < dataItems.length; i++) {
                var idx = dataItems[i].get_cell("IDX").innerText.trim();
                var ProductCode = dataItems[i].get_cell("PRODUCT_CODE").innerText.trim();
                var ProductName = dataItems[i].get_cell("PRODUCT_NAME").innerText.trim();

                var qty = '0';
                if (dataItems[i].findElement('radGrdNumQty'))
                    qty = dataItems[i].findElement('radGrdNumQty').value.replace(/,/gi, '').replace(/ /gi, '');
                else
                    qty = dataItems[i].get_cell("QTY").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var invoice_price = dataItems[i].get_cell("INVOICE_PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var invoice_amount = dataItems[i].get_cell("INVOICE_AMOUNT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                //var basic_br = dataItems[i].get_cell("BASIC_RB").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var basic_br = 0;
               
                var volume_rb = 0;
                if (dataItems[i].findElement('radGrdNumVolumeRB'))
                    volume_rb = dataItems[i].findElement("radGrdNumVolumeRB").value.replace(/,/gi, '').replace(/ /gi, '');
                else
                    volume_rb = dataItems[i].get_cell("VOLUME_RB").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var additional = '0';
                if (dataItems[i].findElement('radGrdNumAdditional'))
                    additional = dataItems[i].findElement('radGrdNumAdditional').value.replace(/,/gi, '').replace(/ /gi, '');
                else
                    additional = dataItems[i].get_cell("ADDITIONAL").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var vs_inv = dataItems[i].get_cell("VS_INV").innerText;

                var proposal_price = dataItems[i].get_cell("PROPOSAL_PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                //if (dataItems[i].findElement('radGrdNumProposalPrice'))
                //    proposal_price = dataItems[i].findElement('radGrdNumProposalPrice').value.replace(/,/gi, '').replace(/ /gi, '');
                //else
                //    proposal_price = dataItems[i].get_cell("PROPOSAL_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var proposal_amount = dataItems[i].get_cell("PROPOSAL_AMOUNT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var subsidy_unit_price = '0';
                if (dataItems[i].findElement('radGrdNumSubsidePrice'))
                    subsidy_unit_price = dataItems[i].findElement('radGrdNumSubsidePrice').value.replace(/,/gi, '').replace(/ /gi, '');
                else
                    subsidy_unit_price = dataItems[i].get_cell("SUBSIDY_UNIT_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                
                var net2_price = dataItems[i].get_cell("NET2_PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                
                


                var product = {
                    IDX: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    QTY: null,
                    INVOICE_PRICE: null,
                    INVOICE_AMOUNT: null,                    
                    VOLUME_RB: null,
                    BASIC_RB: null,
                    ADDITIONAL: null,
                    VS_INV: null,
                    PROPOSAL_PRICE: null,
                    PROPOSAL_AMOUNT: null,
                    SUBSIDY_UNIT_PRICE: null,
                    NET2_PRICE: null
                }
                maxIdx = idx;
                product.IDX = parseInt(idx);
                product.PRODUCT_CODE = ProductCode;
                product.PRODUCT_NAME = ProductName;
                product.QTY = qty;
                product.INVOICE_PRICE = invoice_price;
                product.INVOICE_AMOUNT = invoice_amount;                
                product.VOLUME_RB = volume_rb;
                product.BASIC_RB = basic_br;
                product.ADDITIONAL = additional;
                product.VS_INV = vs_inv;
                product.PROPOSAL_PRICE = proposal_price;
                product.PROPOSAL_AMOUNT = proposal_amount;
                product.SUBSIDY_UNIT_PRICE = subsidy_unit_price;
                product.NET2_PRICE = net2_price;
                list.push(product);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

            if (data) {
                var product = {
                    IDX: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    QTY: null,
                    INVOICE_PRICE: null,
                    INVOICE_AMOUNT: null,
                    PRODUCT_RB: null,
                    VOLUME_RB: null,
                    BASIC_RB: null,
                    ADDITIONAL: null,
                    VS_INV: null,
                    PROPOSAL_PRICE: null,
                    PROPOSAL_AMOUNT: null,
                    SUBSIDY_UNIT_PRICE: null,
                    NET2_PRICE: null

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
                    if($('#<%= hddCustomerType.ClientID%>').val()=="Retailer")
                        product.INVOICE_PRICE = data.INVOICE_PRICE;
                    else
                        product.INVOICE_PRICE = data.NET1_PRICE;

                    if ( $find('<%= radRdoNH.ClientID%>').get_checked()){
                        product.INVOICE_PRICE = data.INVOICE_PRICE_NH;
                        //product.BASIC_RB = data.INVOICE_PRICE_NH * 0.05;
                        product.BASIC_RB = 0;
                    }
                    if (product.INVOICE_PRICE==0) {
                        alert("please check invoice price with sales admin team.");
                        return;
                    }
                    product.INVOICE_AMOUNT = 0;
                    product.PRODUCT_RB = 0;
                    product.VOLUME_RB = 0;
                    product.ADDITIONAL = 0;
                    product.VS_INV = 0;
                    product.PROPOSAL_PRICE = 0;
                    product.PROPOSAL_AMOUNT = 0;
                    product.SUBSIDY_UNIT_PRICE = 0;

                    if ($find('<%= radRdoFM.ClientID%>').get_checked())
                        product.NET2_PRICE = data.NET2_PRICE;
                    else
                        product.NET2_PRICE = data.NET2_PRICE_NH;

                    
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

                var strPrice = dataItem.get_cell('INVOICE_PRICE').innerText;                
                if (strPrice) price = strPrice.replace(/,/gi, '').replace(/ /gi, '');

                var ctlAdditional = dataItem.findElement('radGrdNumAdditional');
                if (ctlAdditional) additional = ctlAdditional.value.replace(/,/gi, '').replace(/ /gi, '');

                var ctlVolumn = dataItem.findElement('radGrdNumVolumeRB');
                if (ctlVolumn) volume = ctlVolumn.value.replace(/,/gi, '').replace(/ /gi, '');

                //var strBasie = dataItem.get_cell('BASIC_RB').innerText;
                //if (strBasie) basic_rb = strBasie.replace(/,/gi, '').replace(/ /gi, '');
                basic_rb = 0;
                dataItem.get_cell('INVOICE_AMOUNT').innerText = (qty * price).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                dataItem.get_cell('PROPOSAL_PRICE').innerText = (price - additional - volume - basic_rb).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
               
                dataItem.get_cell('PROPOSAL_AMOUNT').innerText = (qty * (price - additional - volume - basic_rb)).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                
                dataItem.get_cell('VS_INV').innerText = (additional / price * 100).toFixed(1).toString();

            }

            //SetTotal();
        }

        function SetTotal() {
            var masterTable = $find('<%= radGrdSampleItemList.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var total = 0, addition_total=0, GrandTotal = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var amount = dataItems[i].get_cell("INVOICE_AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');
                var addition_amount = dataItems[i].get_cell("PROPOSAL_AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');
                amount = parseInt(amount);
                total += amount;

                addition_amount = parseInt(addition_amount);
                addition_total += addition_amount;
            }

           // masterTable.get_element().tFoot.rows[0].cells[5].innerText = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
           // masterTable.get_element().tFoot.rows[0].cells[9].innerText = addition_total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $find('<%= radNumAmount.ClientID %>').set_value(addition_total);
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
                    <th>Business Unit <span class="text_red">*</span></th>
                    <td>
                        <div id="divBU" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoBgCP" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="CP" Value="CP" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBgIS" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="IS" Value="IS" AutoPostBack="false" ></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBgBVS" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="BVS" Visible="false" Value="BVS" AutoPostBack="false" OnClientClicked="fn_OnBgCheckedChanged"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBgES" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BG" Text="ES" Value="ES" AutoPostBack="false" ></telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Title <span class="text_red">*</span></th>
                    <td>
                        <telerik:RadButton ID="radRdoTitle1" runat="server" ButtonType="ToggleButton" ToggleType="Radio" Visible="false"
                            Text="Exception request" Value="Exception" AutoPostBack="false" GroupName="Title" OnClientClicked="fn_OnTitleCheckedChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radRdoTitle2" runat="server" ButtonType="ToggleButton" ToggleType="Radio" Checked="true"
                            Text="Subsidy supply" Value="Subsidy" AutoPostBack="false" GroupName="Title" OnClientClicked="fn_OnTitleCheckedChanged">
                        </telerik:RadButton>
                        
                    </td>
                </tr>
                
                <tr id="rowDistributionChannel">
                    <th>Distribution Channel<span class="text_red">*</span></th>
                    <td>
                          <telerik:RadButton ID="radRdoFM" runat="server" Text="FM" Value="FM" GroupName="Distribution" ButtonType="ToggleButton" ToggleType="Radio" OnClick="RadrdoNew_Click"></telerik:RadButton>
                          <telerik:RadButton ID="radRdoNH" runat="server" Text="NH" Value="NH" GroupName="Distribution" ButtonType="ToggleButton" ToggleType="Radio" OnClick="RadrdoNew_Click" ></telerik:RadButton>                       
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
                
                </table>
                <div id="exceptioncase" runat="server"  style="display:none">
                    <table>
                            <tr>
                                <th>TITLE <span class="text_red">*</span></th>
                                <td>
                                    <telerik:RadTextBox ID="RadTxtTitle" runat="server"  Width="90%">
                                    </telerik:RadTextBox>
                        
                                </td>
                            </tr>
                            <tr>
                                <th>Product</th>
                                <td>
                                    <telerik:RadAutoCompleteBox ID="radAutoProduct" runat="server" AllowCustomEntry="false" Width="100%" DropDownWidth="300px"
                                        OnClientRequesting="fn_OnProduct">
                                        <WebServiceSettings Method="SearchProduct" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                                    </telerik:RadAutoCompleteBox>

                                </td>
                            </tr>
                            <tr>
                                <th>Exception Reason </th>
                                <td>
                                    <telerik:RadDropDownList ID="radDropReason" runat="server" Width="300px" DefaultMessage="--- Select ---" >
                                        <Items>
                                            <telerik:DropDownListItem Text="Discounts and rebate (product)" Value="Discounts and rebate (product)" />
                                            <telerik:DropDownListItem Text="Discounts and rebate (Customer)" Value="Discounts and rebate (Customer)" />
                                            <telerik:DropDownListItem Text="Payment terms change" Value="Payment terms change" />
                                            <telerik:DropDownListItem Text="Urgent customer crdit note" Value="Urgent customer crdit note" />
                                            <telerik:DropDownListItem Text="Others" Value="Others" />
                                        </Items>
                                    </telerik:RadDropDownList>    
                                </td>
                            </tr>
                            <tr>
                                <th>Backgroud </th>
                                <td>
                                    <telerik:RadTextBox ID="RadTextBackground" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>Proposal </th>
                                <td>
                                    <telerik:RadTextBox ID="RadTextProposal" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>Process </th>
                                <td>
                                    <telerik:RadTextBox ID="RadTextProcess" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>Financial impact <span class="text_red">*</span></th>
                                <td>
                                    <telerik:RadTextBox ID="RadTextFinancial" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                                </td>
                            </tr>

                            <tr>
                                <th>Comment </th>
                                <td>
                                    <telerik:RadTextBox ID="RadTextExceptionComment" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                                </td>
                            </tr>
                    </table>




                </div>
            




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
                            <telerik:GridTemplateColumn DataField="INVOICE_PRICE" HeaderText="Inv<br>Price" UniqueName="INVOICE_PRICE" HeaderStyle-Width="60px" >
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("INVOICE_PRICE")) %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="INVOICE_AMOUNT" HeaderText="Amount" UniqueName="INVOICE_AMOUNT" HeaderStyle-Width="80px" >
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label2241" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("INVOICE_AMOUNT")) %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="BASIC_RB" HeaderText="Basic<br> R/B" UniqueName="BASIC_RB" HeaderStyle-Width="50px" Visible="false" >
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label2242" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("BASIC_RB")) %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            
                            <telerik:GridTemplateColumn DataField="VOLUME_RB" HeaderText="Volume R/B" UniqueName="VOLUME_RB" HeaderStyle-Width="80px"  >
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label21" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("VOLUME_RB")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="radGrdNumVolumeRB" runat="server" Width="100%" CssClass="input align_right"
                                        onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                        onkeyup="return fn_OnGridKeyUp(this)"
                                        DecimalDigits="3">                                
                                    </asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
               

                            <telerik:GridTemplateColumn DataField="ADDITIONAL" HeaderText="Additional" UniqueName="ADDITIONAL" HeaderStyle-Width="60px" >
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

                            
                            <telerik:GridTemplateColumn DataField="VS_INV" HeaderText="vs<br>INV%" UniqueName="VS_INV" HeaderStyle-Width="50px">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# String.Format("{0:#,##0.##}", Eval("VS_INV")) %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>  

                            <telerik:GridTemplateColumn DataField="PROPOSAL_PRICE" HeaderText="Proposa<br>Price" UniqueName="PROPOSAL_PRICE" HeaderStyle-Width="80px" >
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label51" runat="server" Text='<%# String.Format("{0:#,##0.##}", Eval("PROPOSAL_PRICE")) %>'></asp:Label>
                                </ItemTemplate>
                                
                            </telerik:GridTemplateColumn> 

                            <telerik:GridTemplateColumn 
                                HeaderText="Proposal<br> Amount" UniqueName="PROPOSAL_AMOUNT" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" 
                                DataField="PROPOSAL_AMOUNT">
                                 <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label511" runat="server" Text='<%# String.Format("{0:#,##0.##}", Eval("PROPOSAL_AMOUNT")) %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                             <telerik:GridTemplateColumn DataField="SUBSIDY_UNIT_PRICE" HeaderText="Subsidy<br>unit price" UniqueName="SUBSIDY_UNIT_PRICE" HeaderStyle-Width="80px">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label61" runat="server" Text='<%# String.Format("{0:#,##0.##}", Eval("SUBSIDY_UNIT_PRICE")) %>'></asp:Label>
                                </ItemTemplate>
                                 <EditItemTemplate>
                                    <asp:TextBox ID="radGrdNumSubsidePrice" runat="server" Width="100%" CssClass="input align_right"
                                        onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                        DecimalDigits="3">                                
                                    </asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn> 
                            <telerik:GridTemplateColumn DataField="NET2_PRICE" HeaderText="Net<BR>Price" UniqueName="NET2_PRICE" HeaderStyle-Width="60px" ReadOnly="true" >
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="Label211" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("NET2_PRICE")) %>'></asp:Label>
                                </ItemTemplate>
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
                
                *Subsidy unit price 는 보조사업 대농민 공급가 입니다. 
                <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Amount difference(차액) </th>
                    <td>
                        <telerik:RadNumericTextBox ID="radNumAmount" runat="server" NumberFormat-DecimalDigits="0"
                            EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Left" Width="120px">
                        </telerik:RadNumericTextBox>
                        * 차액(Total amount – Proposal amount) 는 Manual 로 입력 합니다 
                    </td>
                </tr>
                <tr>
                    <th>Comment </th>
                    <td>
                        <telerik:RadTextBox ID="radTextSubsidyCommnet" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        * 내용에는 보조사업에 대한 전반적인 내용 및 가격 제안 근거 기록
                    </td>
                </tr>
            </table>
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

