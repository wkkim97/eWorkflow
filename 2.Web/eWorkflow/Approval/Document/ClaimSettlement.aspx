<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="ClaimSettlement.aspx.cs" Inherits="Approval_Document_ClaimSettlement" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function fn_DoRequest(sender, args) {
            return fn_UpdateGridData(true);
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData(false);
        }

        function fn_UpdateGridData(checkValidate) {
            var list = [];
            var masterTable = $find('<%= radGrdClaimSettlementList.ClientID%>').get_masterTableView();
            $find('<%= radGrdClaimSettlementList.ClientID%>').get_batchEditingManager().saveChanges(masterTable);
            var dataItems = masterTable.get_dataItems();
            var cash = $find('<%= radRdoType1.ClientID%>').get_checked();
            var commodity = $find('<%= radRdoType2.ClientID%>').get_checked();


            if (checkValidate && dataItems.length > 0 && cash) {
                var lastItem = dataItems[dataItems.length - 1];
                var name = lastItem.get_cell("NAME").children[0].innerText;
                var amount = lastItem.get_cell("AMOUNT").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var reason = lastItem.get_cell("REASON").children[0].innerText;
                var bank = lastItem.get_cell("BANK_ACCOUNT").children[0].innerText;

                if (name.length < 1 || amount.length < 1 || reason.length < 1 || bank.length < 1) {
                    fn_OpenInformation('자료를 입력바랍니다.');
                    return false;
                }
            }

            else if (checkValidate && dataItems.length > 0 && commodity) {
                var lastItem = dataItems[dataItems.length - 1];
                var name = lastItem.get_cell("NAME").children[0].innerText;
                var amount = lastItem.get_cell("AMOUNT").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var reason = lastItem.get_cell("REASON").children[0].innerText;
                var productcode = lastItem.get_cell("PRODUCT_CODE").children[0].innerText;
                var productName = lastItem.get_cell("PRODUCT_NAME").children[0].innerText;
                var qty = lastItem.get_cell("QTY").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                if (name.length < 1 || amount.length < 1 || reason.length < 1 || productcode.length < 1 || productName.length < 1 || qty.length < 1) {
                    fn_OpenInformation('자료를 입력바랍니다.');
                    return false;
                }
            }

            for (var i = 0; i < dataItems.length; i++) {
                var idx = dataItems[i].get_cell("IDX").innerText;
                var name = dataItems[i].get_cell("NAME").children[0].innerText;
                var amount = dataItems[i].get_cell("AMOUNT").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');
                var reason = dataItems[i].get_cell("REASON").children[0].innerText;
                var bank = dataItems[i].get_cell("BANK_ACCOUNT").children[0].innerText;
                var productcode = dataItems[i].get_cell("PRODUCT_CODE").children[0].innerText;
                var productName = dataItems[i].get_cell("PRODUCT_NAME").children[0].innerText;
                var qty = dataItems[i].get_cell("QTY").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');

                var conObj = {
                    IDX: null,
                    NAME: null,
                    AMOUNT: null,
                    REASON: null,
                    BANK_ACCOUNT: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    QTY: null,

                }
                conObj.IDX = idx;
                conObj.NAME = name;
                conObj.AMOUNT = amount;
                conObj.REASON = reason;
                conObj.BANK_ACCOUNT = bank;
                conObj.PRODUCT_CODE = productcode;
                conObj.PRODUCT_NAME = productName;
                conObj.QTY = qty.replace(/,/gi, '').replace(/ /gi, '');;

                list.push(conObj);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

            return true;
        }

        function fn_OnAddButtonClicked(sender, args) {
            var controls = $('#<%= divType.ClientID %>').children();
            var selectedValue = null;

            for (var i = 0; i < controls.length; i++) {
                var type = controls[i];
                if ($find(type.id).get_checked()) {
                    selectedValue = $find(type.id).get_value();
                }
            }
            if (fn_UpdateGridData(true) && selectedValue)
                sender.set_autoPostBack(true);
            else if (fn_UpdateGridData(true) && selectedValue == null) {
                fn_OpenInformation('please Select a Type');
                sender.set_autoPostBack(false);
            }
            else
                sender.set_autoPostBack(false);
        }

        function openConfirmPopUp(index) {
            clickedKey = parseInt(index);
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);

            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                var masterTable = $find('<%= radGrdClaimSettlementList.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        var currentIdx = null;
        function fn_OpenProduct(sender, eventArgs) {
            var masterTable = $find('<%= radGrdClaimSettlementList.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var num = masterTable.get_dataItems().length;
            var row = $find('<%= radGrdClaimSettlementList.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            var dataItem = $find(row.id);
            currentIdx = dataItem.get_cell('IDX').innerText;
            //for (var j = 0; j < num; j++) {
            //    currentIdx = masterTable.get_dataItems()[j].get_cell('IDX').innerText;
            //    break;
            //}

            var wnd = $find("<%= radWinClaimSettlementList.ClientID %>");
            wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx");
            wnd.show();
            sender.set_autoPostBack(false);

        }

        function fn_ClientClose(oWnd, args) {
            var item = args.get_argument();
            var masterTable = $find('<%= radGrdClaimSettlementList.ClientID%>').get_masterTableView();            
            var num = masterTable.get_dataItems().length;
            var dataItems = masterTable.get_dataItems();
            var lastItem = dataItems[dataItems.length - 1];

            if (item != null) {
                if (!isDuplication(item.PRODUCT_CODE)) {

                    fn_UpdateGridData(false);
                    $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("ApplyProduct:" + currentIdx + ":" + item.PRODUCT_CODE + ":" + item.PRODUCT_NAME);
                } else
                    fn_OpenInformation('동일한 product가 존재합니다.');
            }
            else {
                oWnd.close();
            }
        }


        function isDuplication(code) {
            var masterTable = $find('<%= radGrdClaimSettlementList.ClientID%>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();

            for (var i = 0; i < dataItems.length; i++) {
                var proCode = dataItems[i].get_cell('PRODUCT_CODE').innerText.trim();

                if (proCode == code) return true;
            }

            return false;
        }

        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdClaimSettlementList.ClientID%>');

            var dataItems = grid.get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
            }
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
                        <th>Type</th>
                        <td>
                            <div id="divType" runat="server">
                                <telerik:RadButton runat="server" ID="radRdoType1" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Cash" Value="Cash" GroupName="Type" OnClick="RadrdoBtn_Click"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="radRdoType2" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Commodity" Value="Commodity" GroupName="Type" OnClick="RadrdoBtn_Click"></telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <h3>List of beneficiaries
            <div class="title_btn">
                <telerik:RadButton ID="radBtnAdd" OnClientClicked="fn_OnAddButtonClicked" runat="server" Text="Add" OnClick="radBtnAdd_Click"
                    EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold"></telerik:RadButton>
            </div>
        </h3>
        <telerik:RadGrid ID="radGrdClaimSettlementList" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" ShowFooter="true" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" GridLines="None" OnItemDataBound="radGrdClaimSettlementList_ItemDataBound" OnItemCommand="radGrdClaimSettlementList_ItemCommand">
            <MasterTableView EnableHeaderContextMenu="true" EditMode="Batch" ClientDataKeyNames="IDX">
                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderStyle-Width="40px" UniqueName="IDX" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="NAME" UniqueName="NAME" HeaderText="Name" HeaderStyle-Width="100px">
                        <ItemTemplate><%# Eval("NAME")%></ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="radGrdTxtName" runat="server" Width="98%"></telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="AMOUNT" HeaderText="Amount" UniqueName="AMOUNT" HeaderStyle-Width="100px" DataType="System.Decimal" ColumnGroupName="Current">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("AMOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadNumericTextBox runat="server" ID="radGrdNumAmount" NumberFormat-DecimalDigits="0"
                                Width="100%" EnabledStyle-HorizontalAlign="Right">
                            </telerik:RadNumericTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%--<telerik:GridNumericColumn DataField="AMOUNT" UniqueName="AMOUNT" HeaderText="Amount" HeaderStyle-Width="100px"
                        DataType="System.Decimal" DataFormatString="{0:#,##0}">
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridNumericColumn>--%>
                    <telerik:GridTemplateColumn DataField="REASON" UniqueName="REASON" HeaderText="Reason" HeaderStyle-Width="50%">
                        <ItemTemplate><%# Eval("REASON")%></ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="radGrdTxtReason" runat="server" Width="90%"></telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="BANK_ACCOUNT" UniqueName="BANK_ACCOUNT" HeaderText="Bank Account" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("BANK_ACCOUNT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="radGrdTxtBankAccount" runat="server" Width="95%"></telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="PRODUCT_CODE" HeaderText="Product Code" HeaderStyle-Width="100px" UniqueName="PRODUCT_CODE" Display="false">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%#Eval("PRODUCT_CODE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="radGrdTxtProductCode" ReadOnly="true" runat="server" Width="100%"></telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="PRODUCT_NAME" UniqueName="PRODUCT_NAME" HeaderText="Product" HeaderStyle-Width="50%">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%#Eval("PRODUCT_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="radGrdTxtProductName" ReadOnly="true" runat="server" Width="80%"></telerik:RadTextBox>
                            <telerik:RadButton ID="radGrdBtnProduct" runat="server" AutoPostBack="false" OnClientClicked='fn_OpenProduct'
                                Width="18px" Height="18px" CssClass="btn_grid">
                                <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                            </telerik:RadButton>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="QTY" UniqueName="QTY" HeaderText="Qty" HeaderStyle-Width="60px" DataType="System.Decimal">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("QTY")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadNumericTextBox runat="server" ID="RadGrdtxtQty" CssClass="input" InputType="Number" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="100%"></telerik:RadNumericTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="40px" HeaderText="Del">
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
            <telerik:RadWindow ID="radWinClaimSettlementList" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product" Width="450px" Height="500px" Behaviors="Default" NavigateUrl="./ProductList.aspx" OnClientClose="fn_ClientClose"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div id="hiddenArea" runat="server">
        <input type="hidden" id="hddProcessID" runat="server" />
        <input type="hidden" id="hddProcessStatus" runat="server" />
        <input type="hidden" id="hddDocumentID" runat="server" />
        <input type="hidden" id="hddGridItems" runat="server" />
        <input type="hidden" id="hddProductCode" runat="server" />
        <input type="hidden" id="hddReuse" runat="server" />
        <input type="hidden" id="hddAddRow" runat="server" value="N" />
    </div>
</asp:Content>

