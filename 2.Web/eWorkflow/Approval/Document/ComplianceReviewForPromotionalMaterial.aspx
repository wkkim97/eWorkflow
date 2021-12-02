<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="ComplianceReviewForPromotionalMaterial.aspx.cs" Inherits="Approval_Document_ComplianceReviewForPromotionlMaterial" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
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

        function getSelectedMaterialType() {
            var controls = $('#<%= divMaterialType.ClientID %>').children();
            var selectedValue;

            for (var i = 0; i < controls.length; i++) {
                var bu = controls[i];
                if ($find(MaterialType.id ).get_checked()) {
                    selectedValue = $find(MaterialType.id).get_value();
                    break;
                }
            }

            return selectedValue;
        }

        function fn_OnProductRequesting(sender, args) {

            var selectedValue = getSelectedBU();

            if (selectedValue) {
                var context = args.get_context();
                context["company"] = $('#<%= hddCompanyCode.ClientID %>').val();
                context["bu"] = selectedValue;
                context["baseprice"] = "C"  // C : PROMOTIOANL_MATERIAL 테이블에서 조회한다.
            } else {
                fn_OpenInformation('Please fill out below fields. <br/><hr>BU');
                args.set_cancel(true);
            }
        }

<%--        //금액계산
        function CalculationAmount() {
            var txtUnitPrice = $('#<%=radNumUnitPrice.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');
            var txtQTY = $('#<%=radNumQTY.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');
            var txtAMT = $('#<%=radNumAMT.ClientID%>');

            var total = parseFloat(txtUnitPrice) * parseFloat(txtQTY);
            txtAMT.val(total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));

            if (txtUnitPrice == "" || txtQTY == "")
                txtAMT.val(0);
        }--%>

        function fn_UpdateGridDataFromUploader() {
            fn_UpdateGridData(false);
        }
        function fn_DoRequest(sender, arsg) {
            return fn_UpdateGridData(false);
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData(false);
        }

        function fn_OnAddClicked(sender, args) {
            if (checkLastRow()) {
                fn_UpdateGridData(true);
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind");
            }
        }

        function checkLastRow() {

            var masterTable = $find('<%= radGrdPromtionalMaterial.ClientID %>').get_masterTableView();
            $find('<%= radGrdPromtionalMaterial.ClientID %>').get_batchEditingManager().saveChanges(masterTable);

            var dataItems = masterTable.get_dataItems();
            if (dataItems.length > 0) {
                var lastItem = dataItems[dataItems.length - 1];
                var qty = lastItem.get_cell("QTY").innerText;
                var price = lastItem.get_cell("UNIT_PRICE").innerText;
                if (qty.length < 1 || price.length < 1) {
                    fn_OpenDocInformation('자료를 입력바랍니다.');
                    return false;
                }
            }
            return true;
        }

        function fn_UpdateGridData(addRow) {
            var list = [];
            var masterTable = $find('<%= radGrdPromtionalMaterial.ClientID %>').get_masterTableView();
            $find('<%= radGrdPromtionalMaterial.ClientID %>').get_batchEditingManager().saveChanges(masterTable);

            var dataItems = masterTable.get_dataItems();
            var maxIdx = 0;

            for (var i = 0; i < dataItems.length; i++) {

                var idx = dataItems[i].get_cell("IDX").innerText;

                var description = dataItems[i].get_cell("DESCRIPTION").children[0].innerText;
                var qty = dataItems[i].get_cell("QTY").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                var price = dataItems[i].get_cell("UNIT_PRICE").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');

                var conObj = {
                    IDX: null,
                    DESCRIPTION: null,
                    QTY: null,
                    UNIT_PRICE: null,
                    AMOUNT: null,
                }
                conObj.IDX = idx;
                conObj.DESCRIPTION = description;
                conObj.QTY = qty;
                conObj.UNIT_PRICE = price;
                conObj.AMOUNT = parseFloat(qty) * parseFloat(price);
                maxIdx = idx;
                list.push(conObj);
            }

            if (addRow) {

                var conObj = {
                    IDX: null,
                    DESCRIPTION: null,
                    QTY: null,
                    UNIT_PRICE: null,
                    AMOUNT: null,
                }
                conObj.IDX = ++maxIdx;
                conObj.DESCRIPTION = '';
                conObj.QTY = 0;
                conObj.UNIT_PRICE = 0;
                conObj.AMOUNT = 0;

                list.push(conObj);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

            return true;
        }

        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdPromtionalMaterial.ClientID%>');
            if (grid.get_masterTableView()) {
                var dataItems = grid.get_masterTableView().get_dataItems();
                if (dataItems.length > 0) {
                    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                }
            }
        }

        function SetTotal() {
            var masterTable = $find('<%= radGrdPromtionalMaterial.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var total = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var amount = dataItems[i].get_cell("AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');

                amount = parseInt(amount);

                total += amount;
            }

            masterTable.get_element().tFoot.rows[0].cells[4].innerText = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function fn_OnGridNumBlur(sender, args) {
            setNumberFormat(sender);
        }

        function fn_OnGridKeyUp(sender) {
            var row = $find('<%= radGrdPromtionalMaterial.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            var dataItem = $find(row.id);
            if (dataItem) {
                var qty = 0, price = 0;
                var ctlQty = dataItem.findElement('radGrdNumQty');
                if (ctlQty) qty = parseFloat(ctlQty.value.replace(/,/gi, '').replace(/ /gi, ''));
                var ctlPrice = dataItem.findElement('radGrdNumPrice');
                if (ctlPrice) price = parseFloat(ctlPrice.value.replace(/,/gi, '').replace(/ /gi, ''));
                if (!qty) qty = 0;
                if (!price) price = 0;

                dataItem.get_cell('AMOUNT').innerText = (qty * price).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
            SetTotal();
        }

        function openConfirmPopUp(index) {
            clickedKey = parseInt(index);

            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);

            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                fn_UpdateGridData(false);
                var masterTable = $find('<%= radGrdPromtionalMaterial.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <h3>Basic Informaton for compliance review</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>

                <tr>
                    <th>BU <span class="text_red">*</span></th>
                    <td colspan="3">
                        <div id="divBU" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoBuHH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="HH" Value="HH" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuWH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="WH" Value="WH" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuSM" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="SM" Value="SM" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuRI" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="R"  Value="R"  AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuCC" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="CC" Value="CC" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuDC" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="DC" Value="DC" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuAH" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="AH" Value="AH" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuMEDI" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="MEDICAL" Value="MEDICAL" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoBuMACS" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="BU" Text="MACS" Value="MACS" AutoPostBack="false"></telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Product Name / Number <span class="text_red">*</span></th>
                    <td colspan="3">
                        <telerik:RadAutoCompleteBox ID="radAcomProduct" runat="server" AllowCustomEntry="false" Width="100%" DropDownWidth="400px"
                            OnClientRequesting="fn_OnProductRequesting">
                            <WebServiceSettings Method="SearchProduct" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                        </telerik:RadAutoCompleteBox>
                        <asp:Label ID="lblNotProduct" runat="server" Width="100%" Visible="false">Refer to the product list attached.</asp:Label>
                    </td>
                </tr>
<%--                <tr>
                    <th rowspan="3"> Promotional Material <span class="text_red">*</span></th>
                    <td colspan="3">
                        <telerik:RadTextBox ID="radTxtPromotionalMaterial" runat="server" Width="100%" TextMode="MultiLine" Height="25px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Unit Price <span class="text_red">*</span></th>
                    <th>Quantity   <span class="text_red">*</span></th>
                    <th>Amount     <span class="text_red">*</span></th>
                </tr>
                <tr>
                    <td class="align_center">
                        <telerik:RadNumericTextBox ID="radNumUnitPrice" runat="server" Width="150px" NumberFormat-DecimalDigits="0" 
                            MinValue="0" MaxValue="99999999999" Value="0" EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right" ClientEvents-OnBlur="CalculationAmount"></telerik:RadNumericTextBox></td>
                    <td class="align_center">
                        <telerik:RadNumericTextBox ID="radNumQTY" runat="server" Width="150px" NumberFormat-DecimalDigits="0"
                            MinValue="0" MaxValue="99999999999" Value="0" EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right" ClientEvents-OnBlur="CalculationAmount"></telerik:RadNumericTextBox>
                    </td>
                    <td class="align_center">
                        <telerik:RadNumericTextBox ID="radNumAMT" runat="server" Width="150px" NumberFormat-DecimalDigits="0" BorderColor="White"
                            MinValue="0" MaxValue="99999999999" Value="0" EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right" ClientEvents-OnBlur="CalculationAmount"></telerik:RadNumericTextBox>
                    </td>
                </tr>--%>
                <tr>
                    <th>Item Type <span class="text_red">*</span></th>
                    <td colspan="3">
                        <div id="divMaterialType" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoMTHCP_SOU" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="MT" Text="HCP_SOUVENIR(3만원 이하)" Value="HCP_Souvenir" ToolTip="제품 설명회(Product Presentation Meeting(PPM))에서 보건의료전문가(Healthcare professional(HCP))에게 제공되는 제공되는 기념품"   AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoMTHCP_GIM" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="MT" Text="HCP_GIMMICK(1만원 이하)"  Value="HCP_Gimmick"  ToolTip="개별 요양기관을 방문하여 시행하는 제품 디테일링의 경우 보건의료전문가(Healthcare professional(HCP))에게 제공되는 회사명 또는 제품명이 기입된 1만원 이하의 판촉물"  AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoMTHCP_NON" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="MT" Text="Non-HCP Item"             Value="Non_HCP_Item" ToolTip="AHCP 또는 HCP가 아닌 다른 대상(Patient, Public, Nurse/Medical technician, Government officer)에게 제공되는 제품의 경우" AutoPostBack="false"></telerik:RadButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Target Audience <span class="text_red">*</span></th>
                    <td colspan="3">
                        <div id="divTargetAudience" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radRdoTA_HCP"          runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="TA" Text="HCP"          Value="HCP"         ToolTip="Healthcare professional/보건의료전문가" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoTA_Patient_PSP"  runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="TA" Text="Patient(PSP)" Value="Patient_PSP" ToolTip="Patient support program의 일환으로 환자에게 제공되는 제품의 경우 선택" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoTA_AHCP"         runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="TA" Text="AHCP"         Value="AHCP"        ToolTip="Animal Healthcare professional/수의사" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoTA_Nurse"        runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="TA" Text="Nurse/Medical technician" ToolTip="간호사, 의료기사 (전문의약품에 대한 홍보물품일 경우 제공 불가)" Value="Nurse_MedicalTechinician" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoTA_Public"       runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="TA" Text="Other public(일반대중)"   ToolTip="대중에게 제공되는 판촉물의 경우 선택(전문의약품 또는 의료 기기에 대한 홍보물품일 경우 제공 불가)" Value="Other_Public" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton ID="radRdoTA_GO"           runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="TA" Text="Government officer"       ToolTip="공무원" Value="Government_Officer" AutoPostBack="false"></telerik:RadButton>
                        
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Purpose <span class="text_red">*</span></th>
                    <td colspan="3">
                        <telerik:RadTextBox ID="radTxtPurpose" runat="server" Width="100%" TextMode="MultiLine"  Rows="2"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </div>
        <h3>Promotional Item
            <div class="title_btn">
                <telerik:RadButton ID="radBtnAdd" runat="server" AutoPostBack="false"
                    OnClientClicked="fn_OnAddClicked" Text="Add"
                    EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton"
                    CssClass="btn btn-blue btn-size1 bold">
                </telerik:RadButton>
            </div>
        </h3>
        <telerik:RadGrid ID="radGrdPromtionalMaterial" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" ShowFooter="true"
            OnItemCommand="radGrdPromtionalMaterial_ItemCommand" Skin="EXGrid">
            <MasterTableView EditMode="Batch" ClientDataKeyNames="IDX">
                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderText="" HeaderStyle-Width="40px" UniqueName="IDX" ReadOnly="true"></telerik:GridBoundColumn>

                    <telerik:GridTemplateColumn DataField="DESCRIPTION" HeaderText="Description" UniqueName="DESCRIPTION" HeaderStyle-Width="50%">
                        <ItemTemplate><%# Eval("DESCRIPTION")%></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radGrdTxtDescription" runat="server" Width="100%" CssClass="input"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="QTY" HeaderText="Qty" UniqueName="QTY" HeaderStyle-Width="80px">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("Qty")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radGrdNumQty" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeypress="return fn_OnGridKeyPress(this, event)"
                                onkeyup="return fn_OnGridKeyUp(this)"
                                DecimalDigits="0">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="UNIT_PRICE" HeaderText="Unit Price" UniqueName="UNIT_PRICE" HeaderStyle-Width="100px">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("UNIT_PRICE")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radGrdNumPrice" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeypress="return fn_OnGridKeyPress(this, event)"
                                onkeyup="return fn_OnGridKeyUp(this)"
                                DecimalDigits="0">                                
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="AMOUNT" HeaderText="Amount" UniqueName="AMOUNT" HeaderStyle-Width="200px" ReadOnly="true"
                        Aggregate="Sum" FooterAggregateFormatString="{0:#,##0}"
                        FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("AMOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove"
                                OnClientClick='<%# String.Format("return openConfirmPopUp({0});",Eval("IDX"))%> '
                                ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                </Columns>

            </MasterTableView>
        </telerik:RadGrid>
    </div>

    <input type="hidden" id="hddGridItems" runat="server" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddCompanyCode" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />

</asp:Content>



