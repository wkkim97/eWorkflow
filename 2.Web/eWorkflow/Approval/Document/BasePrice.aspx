<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="BasePrice.aspx.cs" Inherits="Approval_Document_BasePrice" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <style type="text/css">
        .lbl_align_right{ text-align:right !important ;}
    </style>
    <script type="text/javascript">

        function fn_UpdateGridDataFromUploader() {
            fn_UpdateGridData(false);
        }

        function fn_DoRequest(sender, args) {
            return fn_UpdateGridData(null);
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData(null);
        }

        function fn_Radio(sender, args) {
            return fn_UpdateGridData(null);
        }
        //wk
        //PRODUCT POPUP
        function fn_PopupProduct(sender, args) {
            var wnd = $find("<%= RadPopupProduct.ClientID %>");
            var New = $find('<%= RadrdoNew.ClientID %>').get_checked();
            var Change = $find('<%= RadrdoChange.ClientID %>').get_checked();
            var dataItems = $find('<%= RadGrdProduct.ClientID %>').get_masterTableView().get_dataItems();

            var bu = getSelectedBU();
            var Category = getSelectCagegory();
            //var value = true;
            if (bu && Category) {
                <%--if ($find('<%= RadGrdProduct.ClientID %>').get_masterTableView().get_dataItems().length > 0) {
                    if (New) {
                        for (var i = 0; i < dataItems.length; i++) {
                            var current = dataItems[i].get_cell("CURRENT_PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                            if (current == '') {
                                fn_OpenInformation('Plz Current Price value Input ');
                                return false;
                            }
                        }
                    }
                    else if (Change) {
                        for (var i = 0; i < dataItems.length; i++) {
                            var includeVat = dataItems[i].get_cell("REVISED_PRICE_INCLUDE_VAT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                            var excludeVat = dataItems[i].get_cell("REVISED_PRICE_EXCLUDE_VAT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                            if (includeVat == '' || excludeVat == '') {
                                fn_OpenInformation('Plz Revised Price value Input');
                                return false;
                            }
                        }
                    }--%>
                wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=" + bu + "&baseprice=Y");
                wnd.show();
                sender.set_autoPostBack(false);
                }
            else {
                fn_OpenDocInformation('Please select a check box value.<br/><hr>required Check Value <b>BU & Category</b>');
                sender.set_autoPostBack(false);
            }
        }

        //--------------------------
        //BU & CATEGORY 선택값
        //--------------------------
        function getSelectedBU() {
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

        function getSelectCagegory() {
            var controls = $('#<%= divCategory.ClientID %>').children();
            var selectedValue;

            for (var i = 0; i < controls.length; i++) {
                var category = controls[i];
                if ($find(category.id).get_checked()) {
                    selectedValue = $find(category.id).get_value();
                    break;
                }
            }
            return selectedValue;
        }

        // ClientClose EVENT
        function fn_setProduct(oWnd, args) {
            var product = args.get_argument();
            if (product != null) {
                fn_UpdateGridData(product);
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind");
            }
            else
                return false;
        }

        //-----------------------------------
        //그리드 클라이언트 데이터 업데이트
        //-----------------------------------
        function fn_UpdateGridData(data) {
            var list = [];
            var masterTable = $find('<%= RadGrdProduct.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var is_val = false;
            for (var i = 0; i < dataItems.length; i++) {

                var name = dataItems[i].get_cell("PRODUCT_NAME").innerText;
                var code = dataItems[i].get_cell("PRODUCT_CODE").innerText;
                var current = dataItems[i].get_cell("CURRENT_PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                var currentVat = dataItems[i].get_cell("CURRENT_PRICE_INCLUDE_VAT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var includeVat = dataItems[i].get_cell("REVISED_PRICE_INCLUDE_VAT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                var excludeVat = dataItems[i].get_cell("REVISED_PRICE_EXCLUDE_VAT").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                var baseprice = dataItems[i].get_cell("BASE_PRICE").innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;

                var product = {
                    PRODUCT_NAME: null,
                    PRODUCT_CODE: null,
                    CURRENT_PRICE: null,
                    CURRENT_PRICE_INCLUDE_VAT:null,
                    REVISED_PRICE_INCLUDE_VAT: null,
                    REVISED_PRICE_EXCLUDE_VAT: null,
                    BASE_PRICE: null
                }
                product.PRODUCT_NAME = name;
                product.PRODUCT_CODE = code;
                product.CURRENT_PRICE = current;
                product.CURRENT_PRICE_INCLUDE_VAT = currentVat;
                product.REVISED_PRICE_INCLUDE_VAT = includeVat;
                product.REVISED_PRICE_EXCLUDE_VAT = excludeVat;
                product.BASE_PRICE = baseprice;
                list.push(product);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

            if (data) {
                var product = {
                    PRODUCT_NAME: null,
                    PRODUCT_CODE: null,
                    CURRENT_PRICE: null,
                    CURRENT_PRICE_INCLUDE_VAT: null,
                    REVISED_PRICE_INCLUDE_VAT: null,
                    REVISED_PRICE_EXCLUDE_VAT: null,
                    BASE_PRICE: null
                }
                for (var i = 0; i < dataItems.length; i++) {
                    var code = dataItems[i].get_cell("PRODUCT_CODE").innerText;
                    if (data.PRODUCT_CODE == code) {
                        fn_OpenDocInformation('동일한 product 가 존재합니다.');
                        var is_val = true;
                        break;
                    }
                }
                if (is_val == false) {
                    product.PRODUCT_NAME = data.PRODUCT_NAME + '(' + data.PRODUCT_CODE + ')';
                    product.PRODUCT_CODE = data.PRODUCT_CODE;
                    product.CURRENT_PRICE = 0;
                    product.CURRENT_PRICE_INCLUDE_VAT = 0;
                    product.REVISED_PRICE_INCLUDE_VAT = 0;
                    product.REVISED_PRICE_EXCLUDE_VAT = 0;
                    product.BASE_PRICE = data.BASE_PRICE;
                    list.push(product);
                    $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

                }
                is_val = false;
            }

            return true;
        }

        //--------------------------
        //Grid Column delete
        //--------------------------
        var clickedKey = "";
        function openConfirmPopUp(PRODUCT_CODE) {
            clickedKey = PRODUCT_CODE;
            fn_UpdateGridData(null);
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);
            
            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                var masterTable = $find('<%= RadGrdProduct.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=RadGrdProduct.ClientID%>');

            if (grid.get_masterTableView() != null) {
                var dataItems = grid.get_masterTableView().get_dataItems();
                if (dataItems.length > 0) {
                    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                }
            }
        }

        function fn_OnGridNumBlur(sender) {
            setNumberFormat(sender);

            //SetTotal();
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
                    <tr>
                        <th>Category <span class="text_red">*</span></th>
                        <td>
                            <div id="divCategory" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton runat="server" ID="RadrdoNew" ButtonType="ToggleButton" ToggleType="Radio" Text="New" Value="New" GroupName="Category" OnClientClicked="fn_Radio" OnClick="RadrdoNew_Click"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="RadrdoChange" ButtonType="ToggleButton" ToggleType="Radio" Text="Change" Value="Change" GroupName="Category" OnClientClicked="fn_Radio" OnClick="RadrdoNew_Click"></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>Type <span class="text_red">*</span></th>
                        <td>
                            <div id="divType" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton runat="server" ID="RadrdoMRP" ButtonType="ToggleButton" ToggleType="Radio" Text="MRP" Value="MRP" GroupName="Type" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="RadrdoNonMRP" ButtonType="ToggleButton" ToggleType="Radio" Text="Non-MRP" Value="Non-MRP" GroupName="Type" AutoPostBack="false"></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>BU <span class="text_red">*</span></th>
                        <td>
                            <div id="divBU" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton ID="radRdoBuHH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="HH" Value="HH" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBuWH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="WH" Value="WH" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBuSM" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="SM" Value="SM" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBuRI" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="R" Value="R" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBuCC" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="CC" Value="CC" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBuDC" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="DC" Value="DC" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoBuAH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="AH" Value="AH" AutoPostBack="false"></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>Description <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtDescription" Width="98%"  ></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <th>Contract Period <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadDatePicker runat="server" ID="RadDatePeriod" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" />
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <h3>Product Information
			<div class="title_btn">
                <telerik:RadButton runat="server" ID="RadbtnProduct" CssClass="btn btn-blue btn-size1 bold" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" AutoPostBack="false"
                    OnClientClicked="fn_PopupProduct" Text="Add">
                </telerik:RadButton>
            </div>
        </h3>
        
            <telerik:RadGrid runat="server" ID="RadGrdProduct" Skin="EXGrid" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" 
                OnItemDataBound="RadGrdProduct_ItemDataBound" AllowAutomaticUpdates="true"
                OnItemCommand="RadGrdProduct_ItemCommand" ShowFooter="false" Width="100%" HeaderStyle-CssClass="grid_header">
                <MasterTableView EditMode="Batch">
                    <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Current Price" Name="CurrentPrice" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100%"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Revised Price" Name="RevisedPrice" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100%"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridBoundColumn DataField="PRODUCT_NAME" HeaderText="Product Name Packing" UniqueName="PRODUCT_NAME" HeaderStyle-Width="100%" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn Display="false" DataField="PRODUCT_CODE" HeaderText="Product Code" UniqueName="PRODUCT_CODE" HeaderStyle-Width="15%" ReadOnly="true" DataType="System.String"></telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="CURRENT_PRICE" UniqueName="CURRENT_PRICE" HeaderText="Base Price(-vat)" ColumnGroupName="CurrentPrice" HeaderStyle-Width="120px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("CURRENT_PRICE")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                            <%--    <telerik:RadNumericTextBox runat="server" ID="RadtxtCurrent"  
                                    NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="100%"
                                    EnabledStyle-HorizontalAlign="Right">
                                </telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="RadtxtCurrent" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="0" AllowNegative="false">                                
                            </asp:TextBox>

                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="CURRENT_PRICE_INCLUDE_VAT" UniqueName="CURRENT_PRICE_INCLUDE_VAT" HeaderText="Base Price(+vat)" ColumnGroupName="CurrentPrice" HeaderStyle-Width="120px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="Label11" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("CURRENT_PRICE_INCLUDE_VAT")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<telerik:RadNumericTextBox runat="server" ID="RadtxtCurrentIncVat" CssClass="input"
                                    NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="100%"
                                    EnabledStyle-HorizontalAlign="Right">
                                </telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="RadtxtCurrentIncVat" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="0" AllowNegative="false">                                
                            </asp:TextBox>                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ReadOnly="true" Display="false" DataField="BASE_PRICE" UniqueName="BASE_PRICE" HeaderText="Base Price(-vat)" ColumnGroupName="CurrentPrice" HeaderStyle-Width="120px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("BASE_PRICE")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<telerik:RadNumericTextBox runat="server" ID="Radtxtbaseparice"   InputType="Number"
                                    NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="100%"
                                    EnabledStyle-HorizontalAlign="Right">
                                </telerik:RadNumericTextBox>--%>
                                <asp:TextBox ID="Radtxtbaseparice" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="0" AllowNegative="false">                                
                            </asp:TextBox>    
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>                        
                        <telerik:GridTemplateColumn UniqueName="REVISED_PRICE_INCLUDE_VAT" DataField="REVISED_PRICE_INCLUDE_VAT" HeaderText="Base Price(+vat)" ColumnGroupName="RevisedPrice" HeaderStyle-Width="120px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("REVISED_PRICE_INCLUDE_VAT")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<telerik:RadNumericTextBox runat="server" ID="RadtxtRevised1"   InputType="Number" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="100%"
                                    EnabledStyle-HorizontalAlign="Right">
                                </telerik:RadNumericTextBox>--%>
                                <asp:TextBox ID="RadtxtRevised1" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="0" AllowNegative="false"> 
                                    </asp:TextBox>    
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="REVISED_PRICE_EXCLUDE_VAT" DataField="REVISED_PRICE_EXCLUDE_VAT" HeaderText="Base Price(-vat)" ColumnGroupName="RevisedPrice" HeaderStyle-Width="120px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("REVISED_PRICE_EXCLUDE_VAT")) %>' CssClass="lbl_align_right"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<telerik:RadNumericTextBox runat="server" ID="RadtxtRevised2"   InputType="Number" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="100%"
                                    EnabledStyle-HorizontalAlign="Right">
                                </telerik:RadNumericTextBox>--%>
                                <asp:TextBox ID="RadtxtRevised2" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)" onkeypress="return fn_OnGridKeyPress(this, event)"
                                DecimalDigits="0" AllowNegative="false"> 
                                    </asp:TextBox>    
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="40px" HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" ImageUrl="~/Styles/images/ico_del.png"
                                    OnClientClick='<%# String.Format("return openConfirmPopUp(\"{0}\");",Eval("PRODUCT_CODE"))%> ' BorderStyle="None" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        
        <telerik:RadWindowManager runat="server" ID="RadWindowManager">
            <Windows>
                <telerik:RadWindow ID="RadPopupProduct" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product" Width="500px" Height="640px"
                    Behaviors="Default" Modal="true" CssClass="windowscroll" OnClientClose="fn_setProduct" NavigateUrl="/eWorks/Common/Popup/ProductList.aspx">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
    </div>
    <input type="hidden" id="hddGridItems" runat="server" />
    <input type="hidden" id="hddProductCode" runat="server" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />

</asp:Content>
