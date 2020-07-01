<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="MaterialPrice.aspx.cs" Inherits="Approval_Document_MaterialPrice" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">

        function fn_DoRequest(sender, arsg) {
            return fn_UpdateGridData(false);
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData(false);
        }

        //--------------------------------
        //     라디오 버튼 이벤트
        //--------------------------------
        function fn_RdoCategory(sender, args) {
            var category = sender.get_value();
            var masterTable = $find('<%= RadGrdMaterial.ClientID %>').get_masterTableView();           
            fn_setVisible(category);
            fn_UpdateGridData(false);
        }

        function fn_ShowKreach(sender, args) {
            if ($find('<%= RadRdoFormulation.ClientID %>').get_checked())
                var materialtype = sender.get_value();
            else
                var materialtype = 'else';
            fn_setVisible('', materialtype, '');
        }

        function fn_ShowCounter(sender, args) {
            var counter = sender.get_value();
            fn_setVisible('', '', counter);
        }

        function fn_setVisible(category, materialtype, counter) {            
            if (category == 'Create') {
                $('#Create').show();
                $('#Change').hide();                
            }
            if (category == 'Change') {
                $('#Change').show();
                $('#Create').hide();                
            }
            if (materialtype == 'Material02') {
                $('#material').attr('rowspan', 2);
                $('#Kreach').show();
            }
            if (materialtype == 'else') {
                $('#material').removeAttr('rowspan');
                $('#Kreach').hide();
            }
            if (counter == 'New') {
                $('#material').attr('rowspan', 3);
                $('#Counter').show();
            }
            if (counter == 'Existing') {
                $('#material').attr('rowspan', 2);
                $('#Counter').hide();
            }

        }


        //--------------------------------
        //      product 자동완성
        //--------------------------------
        function fn_OnProductRequesting(sender, args) {
            var context = args.get_context();
            context["company"] = $('#<%= hddCompanyCode.ClientID %>').val();
            context["bu"] = '';
        }

        //--------------------------------
        //      Grid Event ROW ADD
        //--------------------------------
        function fn_OnAddButtonClicked(sender, args) {
            if (fn_UpdateGridData(true))
                sender.set_autoPostBack(true);
            else
                sender.set_autoPostBack(false);
        }

        function fn_UpdateGridData(checkValidate) {
            var list = [];
            var masterTable = $find('<%= RadGrdMaterial.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var change = ($find('<%= RadrdoChange.ClientID %>').get_checked())


            if (checkValidate && dataItems.length > 0) {
                var empty = isEmptyCheck();
                if (!empty)
                    return false;
            }
            for (var i = 0; i < dataItems.length; i++) {
                var idx = dataItems[i].get_cell("IDX").innerText;
                var materialcode = dataItems[i].get_cell("MATERIAL_CODE").innerText;
                var materialname = dataItems[i].get_cell("MATERIAL_NAME").innerText;
                //DropdownList
                var Dropcurrency = dataItems[i].findControl('radDropCurrency');
                var currency = null;
                if (Dropcurrency) {
                    currency = Dropcurrency._selectedText;
                }
                else {
                    currency = dataItems[i].get_cell("CURRENCY").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                }
                var unitprice = dataItems[i].get_cell("UNIT_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                //Change 가 Checked 됐을때
                if (change) {
                    var tobeUnitprice = dataItems[i].get_cell("TO_BE_UNIT_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                }
                var manufacturer = dataItems[i].get_cell("MANUFACTURER").children[0].innerText;

                var material = {
                    IDX: null,
                    MATERIAL_CODE: null,
                    MATERIAL_NAME: null,
                    CURRENCY: null,
                    UNIT_PRICE: null,
                    TO_BE_UNIT_PRICE: null,
                    MANUFACTURER: null
                }
                material.IDX = idx;
                material.MATERIAL_CODE = materialcode;
                material.MATERIAL_NAME = materialname;
                material.CURRENCY = currency;
                material.UNIT_PRICE = unitprice;
                material.TO_BE_UNIT_PRICE = tobeUnitprice;
                material.MANUFACTURER = manufacturer;

                list.push(material);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));
            return true;
        }

        //--------------------------------
        //      GRID Empty Check : 그리드의 빈값을 체크 true:false 를 반환해준다.
        //--------------------------------
        function isEmptyCheck(checkEmpty) {
            var masterTable = $find('<%= RadGrdMaterial.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var change = ($find('<%= RadrdoChange.ClientID %>').get_checked())
            for (var i = 0; i < dataItems.length; i++) {
                var materialcode = dataItems[i].get_cell("MATERIAL_CODE").children[0].innerText;
                var materialname = dataItems[i].get_cell("MATERIAL_NAME").children[0].innerText;
                var unitprice = dataItems[i].get_cell("UNIT_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                if (change) {
                    var tobeUnitprice = dataItems[i].get_cell("TO_BE_UNIT_PRICE").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');;
                    if (tobeUnitprice.length < 1) {
                        fn_OpenDocInformation('Please value Input');
                        return false;
                    }
                }
                var manufacturer = dataItems[i].get_cell("MANUFACTURER").children[0].innerText;
                if (materialcode.length < 1 || materialname.length < 1 || unitprice.length < 1 || manufacturer.length < 1) {
                    fn_OpenDocInformation('Please value Input');
                    return false;
                }
            }
            return true;
        }

        //--------------------------
        //   Grid Column delete
        //--------------------------
        var clickedKey = null;
        function openConfirmPopUp(index) {
            clickedKey = parseInt(index);
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);
            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                fn_UpdateGridData(false);
                var masterTable = $find('<%= RadGrdMaterial.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        function openGridRowForEdit() {
            var grid = $find('<%=RadGrdMaterial.ClientID%>');

            var dataItems = grid.get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
            }
        }
        function fn_OnGridNumBlur(sender) {
            setNumberFormat(sender);
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <h3>Please fill out below fields Material Price</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col style="width: 25%;" />
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Category</th>
                        <td colspan="4">
                            <telerik:RadButton ID="radRdoCreate" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Category" Text="Create" Value="Create" OnClientCheckedChanged="fn_RdoCategory" OnClick="RadrdoChange_Click"></telerik:RadButton>
                            <telerik:RadButton ID="RadrdoChange" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Category" Text="Change" Value="Change" OnClientCheckedChanged="fn_RdoCategory" OnClick="RadrdoChange_Click"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr id="Create" style="display: none">
                        <th>Purpose</th>
                        <td colspan="4">
                            <div id="divCreate" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton ID="radRdoNewProduct" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divCreate" Text="New product launching" Value="Create01" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoProcess" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divCreate" Text="Process change" Value="Create02" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoAlternate" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divCreate" Text="Alternate source" Value="Create03" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoNewExport" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divCreate" Text="New export" Value="Create04" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoMaterial" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divCreate" Text="Material spec. change" Value="Create05" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="radRdoOther" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divCreate" Text="Other" Value="Create06" AutoPostBack="false"></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr id="Change" style="display: none">
                        <th>Purpose</th>
                        <td colspan="4">
                            <div id="divChange" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton ID="RadRdoCost" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divChange" Text="Cost Savings" Value="Change01" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="RadRdoPrice" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divChange" Text="Price increse" Value="Change02" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="RadRdoMaterial2" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divChange" Text="Material spec. change" Value="Change03" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="RadRdoAlternate2" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divChange" Text="Alternate source" Value="Change04" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="RadRdoOther2" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divChange" Text="Other" Value="Change05" AutoPostBack="false"></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th id="material">Material Type</th>
                        <td colspan="4">
                            <div id="divType" runat="server" style="width: 100%; margin: 0 0 0 0">
                                <telerik:RadButton ID="RadRdoAi" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divMaterial" Text="Ai" Value="Material01" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="RadRdoFormulation" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divMaterial" Text="Formulation chemical" Value="Material02" OnClientCheckedChanged="fn_ShowKreach" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="RadRdoPacking" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divMaterial" Text="Packing Material" Value="Material03" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="RadTrade" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divMaterial" Text="Trade Goods" Value="Material04" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="RadPassive" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divMaterial" Text="Passive Toll" Value="Material05" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton ID="RadRdoOther3" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="divMaterial" Text="Other" Value="Material06" AutoPostBack="false"></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr id="Kreach" style="display: none">
                        <th>K-REACH verification</th>
                        <td colspan="3">
                            <telerik:RadButton ID="RadRdoExist" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="k-reach" Text="Existing" Value="Existing" OnClientCheckedChanged="fn_ShowCounter" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="RadRdoNew" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="k-reach" Text="New" Value="New" OnClientCheckedChanged="fn_ShowCounter" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr id="Counter" style="display: none">
                        <th>Counter measure</th>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="RadTextCounter" Width="99%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Use for (Product)</th>
                        <td colspan="4">
                            <telerik:RadTextBox runat="server" ID="RadTextProduct" Width="99%"></telerik:RadTextBox>
                            <%--<telerik:RadAutoCompleteBox ID="radAcomProduct" runat="server" AllowCustomEntry="false" Width="100%" DropDownWidth="400px"
                                OnClientRequesting="fn_OnProductRequesting">
                                <WebServiceSettings Method="SearchProduct" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                            </telerik:RadAutoCompleteBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <th>Supplier Number</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadTextSnumber" Width="99%"></telerik:RadTextBox>
                        </td>
                        <th>Supplier Name</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadTextSname" Width="99%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Incoterms</th>
                        <td colspan="4">
                            <telerik:RadDropDownList ID="RadDropIncoterm" runat="server" DefaultMessage="--- Select ---" DataTextField="CODE_NAME" DataValueField="SUB_CODE"></telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Payment Terms</th>
                        <td colspan="4">
                            <telerik:RadDropDownList ID="RadDropPayment" runat="server" Width="300" DefaultMessage="--- Select ---" DataTextField="CODE_NAME" DataValueField="SUB_CODE"></telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Remark</th>
                        <td colspan="4">
                            <telerik:RadTextBox runat="server" ID="RadTextRemark" Width="99%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Secondary seal</th>
                        <td colspan="4">
                            <telerik:RadButton ID="radRdoYes" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="SecondarySeal" Text="Yes" Value="Y" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoNo" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="SecondarySeal" Text="No" Value="N" AutoPostBack="false"></telerik:RadButton>

                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <h3>Material Information
            <div class="title_btn">
                <telerik:RadButton ID="radBtnAdd" runat="server" CssClass="btn btn-blue btn-size1 bold" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" Text="Add" OnClientClicked="fn_OnAddButtonClicked" OnClick="radBtnAdd_Click"></telerik:RadButton>
            </div>
        </h3>
        <%--<div class="data_type1">--%>
        <telerik:RadGrid runat="server" ID="RadGrdMaterial" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" Skin="EXGrid" ShowFooter="false" Width="100%" OnItemCommand="RadGrdMaterial_ItemCommand" >
            <MasterTableView EditMode="Batch" ClientDataKeyNames="IDX" HeaderStyle-CssClass="grid_header">
                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                <HeaderStyle HorizontalAlign="Left" />
                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderText="" HeaderStyle-Width="2%" UniqueName="IDX" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" DataField="MATERIAL_CODE" UniqueName="MATERIAL_CODE"
                        HeaderText="Material Code" HeaderStyle-Width="15%">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "MATERIAL_CODE")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="RadtxtMaterialCode" Width="99%" CssClass="input"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="MATERIAL_NAME" UniqueName="MATERIAL_NAME" HeaderText="Material Name" HeaderStyle-Width="15%">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "MATERIAL_NAME")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="RadtxtMaterialName" Width="99%" CssClass="input"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="CURRENCY" UniqueName="CURRENCY" HeaderText="Currency" HeaderStyle-Width="15%">
                        <ItemTemplate>
                            <%# Eval("CURRENCY")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDropDownList ID="radDropCurrency" runat="server" DefaultMessage="--- Select ---" CssClass="sel" Width="140px">
                                <Items>
                                    <telerik:DropDownListItem Text="KRW" Value="KRW" />
                                    <telerik:DropDownListItem Text="USD" Value="USD" />
                                    <telerik:DropDownListItem Text="EUR" Value="EUR" />
                                    <telerik:DropDownListItem Text="JPY" Value="JPY" />
                                    <telerik:DropDownListItem Text="GBP" Value="GBP" />
                                </Items>
                            </telerik:RadDropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="UNIT_PRICE" DataField="UNIT_PRICE" HeaderText="Unit Price<br/>(To-Be)" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# String.Format("{0:#,##0.###}", Eval("UNIT_PRICE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="RadtxtUnitPrice" EnabledStyle-HorizontalAlign="Right" InputType="Number" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="100%"></telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="RadtxtUnitPrice" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" 
                                onfocus="return fn_OnGridNumFocus(this)" 
                                onkeypress="return fn_OnGridKeyPress(this, event)"                                
                                DecimalDigits="3" AllowNegative="false"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="TO_BE_UNIT_PRICE" DataField="TO_BE_UNIT_PRICE" HeaderText="Unit Price<br/>(To-Be)" Display="false" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Right" /> 
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# String.Format("{0:#,##0.###}", Eval("TO_BE_UNIT_PRICE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="RadtxtTobePrice" EnabledStyle-HorizontalAlign="Right" InputType="Number" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="100%"></telerik:RadNumericTextBox>--%>
                            <asp:TextBox ID="RadtxtTobePrice" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)" 
                                onfocus="return fn_OnGridNumFocus(this)" 
                                onkeypress="return fn_OnGridKeyPress(this, event)"                                
                                DecimalDigits="3" AllowNegative="false"></asp:TextBox>

                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="MANUFACTURER" UniqueName="MANUFACTURER" HeaderText="Manufacturer" HeaderStyle-Width="15%">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "MANUFACTURER")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="RadtxtManufacturer" Width="99%" CssClass="input"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="5%" HeaderText="" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" ImageUrl="~/Styles/images/ico_del.png"
                                OnClientClick='<%# String.Format("return openConfirmPopUp({0});",Eval("IDX"))%> ' BorderStyle="None" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>

    </div>
    <!-- //doc Style -->
    <input type="hidden" id="hddCompanyCode" runat="server" />
    <input type="hidden" id="hddGridItems" runat="server" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
	<input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

