<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="InternalEvent.aspx.cs" Inherits="Approval_Document_InternalEvent" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function fn_UpdateGridDataFromUploader() {
            fn_UpdateGridData(false);
        }
        function fn_DoRequest(sender, arsg) {
            return fn_UpdateGridData(false);
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData(false);
        }

        function checkLastRow() {
            var masterTable = $find('<%= radGrdCostDetail.ClientID %>').get_masterTableView();
            $find('<%= radGrdCostDetail.ClientID %>').get_batchEditingManager().saveChanges(masterTable);

            var dataItems = masterTable.get_dataItems();
            if (dataItems.length > 0) {
                var lastItem = dataItems[dataItems.length - 1];
                var price = lastItem.get_cell("UNIT_PRICE").innerText;
                if (price.length < 1) {
                    fn_OpenDocInformation('자료를 입력바랍니다.');
                    return false;
                }
            }
            return true;
        }

        function fn_UpdateGridData(addRow) {
            var list = [];
            var masterTable = $find('<%= radGrdCostDetail.ClientID %>').get_masterTableView();
            $find('<%= radGrdCostDetail.ClientID %>').get_batchEditingManager().saveChanges(masterTable);

            var dataItems = masterTable.get_dataItems();
            var maxIdx = 0;
            for (var i = 0; i < dataItems.length; i++) {

                var idx = dataItems[i].get_cell("IDX").innerText;
                var categoryCode = dataItems[i].get_cell("CATEGORY_CODE").innerText;
                var categoryName = dataItems[i].get_cell("CATEGORY_NAME").children[0].innerText;

                var description = dataItems[i].get_cell("DESCRIPTION").children[0].innerText;
                var price = dataItems[i].get_cell("UNIT_PRICE").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');

                var conObj = {
                    IDX: null,
                    CATEGORY_CODE: null,
                    CATEGORY_NAME: null,
                    DESCRIPTION: null,
                    QTY: null,
                    UNIT_PRICE: null,
                    AMOUNT: null,
                }
                conObj.IDX = idx;
                conObj.CATEGORY_CODE = categoryCode;
                conObj.CATEGORY_NAME = categoryName;
                conObj.DESCRIPTION = description;
                conObj.QTY = 1;
                conObj.UNIT_PRICE = price;
                conObj.AMOUNT = parseFloat(price);
                maxIdx = idx;
                list.push(conObj);
            }

            if (addRow) {
                var categoryValue = $find('<%= radDdlCategory.ClientID %>').get_selectedItem().get_value();
                var categoryText = $find('<%= radDdlCategory.ClientID %>').get_selectedItem().get_text();

                var conObj = {
                    IDX: null,
                    CATEGORY_CODE: null,
                    CATEGORY_NAME: null,
                    DESCRIPTION: null,
                    QTY: null,
                    UNIT_PRICE: null,
                    AMOUNT: null,
                }
                conObj.IDX = ++maxIdx;
                conObj.CATEGORY_CODE = categoryValue;
                conObj.CATEGORY_NAME = categoryText;
                conObj.DESCRIPTION = '';
                conObj.QTY = 1;
                conObj.UNIT_PRICE = 0;
                conObj.AMOUNT = 0;

                list.push(conObj);
                
                SetTotal();
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));
            return true;
        }

        function fn_OnAddCategoryClicked(sender, args) {
            if (checkLastRow()) {
                fn_UpdateGridData(true);
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind");
            }
        }

        function fn_OnGridKeyUp(sender) {
            var row = $find('<%= radGrdCostDetail.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            var dataItem = $find(row.id);
            if (dataItem) {
                var qty = 1, price = 0;
                var ctlPrice = dataItem.findElement('radGrdNumPrice');
                if (ctlPrice) price = parseFloat(ctlPrice.value.replace(/,/gi, '').replace(/ /gi, ''));
                if (!price) price = 0;

                dataItem.get_cell('AMOUNT').innerText = (qty * price).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
            SetTotal();
        }

        function SetTotal() {
            var masterTable = $find('<%= radGrdCostDetail.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var total = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var amount = dataItems[i].get_cell("AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '') || 0;

                amount = parseInt(amount);

                total += amount;
            }

            masterTable.get_element().tFoot.rows[0].cells[5].innerText = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdCostDetail.ClientID%>');
            if (grid.get_masterTableView()) {
                var dataItems = grid.get_masterTableView().get_dataItems();
                if (dataItems.length > 0) {
                    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                }
            }
        }

        function fn_OnGridNumBlur(sender, args) {
            setNumberFormat(sender);
        }
        
        function fn_OnAttendeeKeyUp(sender) {
            
            var noBayerEmp = $('#<%= radNumBayerEmp.ClientID %>').val();
            noBayerEmp = noBayerEmp.replace(/,/gi, '').replace(/ /gi, '');
            if (!noBayerEmp) noBayerEmp = 0;
            var noExternalGuest = $('#<%= radNumExternalGuest.ClientID %>').val();
            noExternalGuest = noExternalGuest.replace(/,/gi, '').replace(/ /gi, '');
            if (!noExternalGuest) noExternalGuest = 0;

            var total = parseInt(noExternalGuest) + parseInt(noBayerEmp);

            $('#<%= lblTotalParticipants.ClientID %>').html(total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        }

        function openConfirmPopUp(index) {
            clickedKey = parseInt(index);

            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);

            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                fn_UpdateGridData(false);
                var masterTable = $find('<%= radGrdCostDetail.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        function fn_OnCategorySelectedChanged(sender, args) {

            var row = $find('<%= radGrdCostDetail.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            if (row) {
                var dataItem = $find(row.id);
                if (dataItem) {
                    dataItem.get_cell('CATEGORY_CODE').innerText = args.get_item().get_value();
                }
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%" />
                    <col />
                    <%--<col style="width: 25%" />
                    <col style="width: 25%" />--%>
                </colgroup>
                <tbody>
                    <tr>
                        <th>BG <span class="text_red">*</span></th>
                        <td colspan="3">
                            <telerik:RadButton ID="radBtnPH" runat="server" Text="PH" Value="PH" GroupName="BU"
                                AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnCH" runat="server" Text="CH" Value="CH" GroupName="BU"
                                AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnCS" runat="server" Text="CS" Value="CS" GroupName="BU"
                                AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnAH" runat="server" Text="AH" Value="AH" GroupName="BU"
                                AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnCPL" runat="server" Text="CPL" Value="CPL" GroupName="BU"
                                AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Subject <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadTextBox ID="radTxtTitle" runat="server" Width="100%"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <th>Date <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadDatePicker ID="radDatFrom" runat="server" Width="100px" Calendar-ShowRowHeaders="false"
                                MinDate="1900-01-01" MaxDate="2050-12-31" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                            ~
                            <telerik:RadDatePicker ID="radDateTo" runat="server" Width="100px" Calendar-ShowRowHeaders="false"
                                MinDate="1900-01-01" MaxDate="2050-12-31" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>

                        </td>
                    </tr>
                    <tr>
                        <th>Venue <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadTextBox ID="radTxtVenue" runat="server" Width="100%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Comment </th>
                        <td colspan="3">
                            <telerik:RadTextBox ID="radTxtComment" runat="server" TextMode="MultiLine" Height="50px" Width="100%"></telerik:RadTextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="data_type1">
            <h3>Attendee Information</h3>
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Bayer Employee</th>
                    <td>
                        <asp:TextBox ID="radNumBayerEmp" runat="server" Width="70px" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)"
                            onfocus="return fn_OnGridNumFocus(this)"
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="return fn_OnAttendeeKeyUp(this)"
                            DecimalDigits="0" Text="0">                                
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>External Guest</th>
                    <td>
                        <asp:TextBox ID="radNumExternalGuest" runat="server" Width="70px" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)"
                            onfocus="return fn_OnGridNumFocus(this)"
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="return fn_OnAttendeeKeyUp(this)"
                            DecimalDigits="0" Text="0">                                
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>Total Participants</th>
                    <td>
                        <asp:Label ID="lblTotalParticipants" runat="server" Text="0" Width="70px" CssClass="align_right" Style="font-weight: bold;"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <h3>Category
            <div class="title_btn">
                <telerik:RadDropDownList ID="radDdlCategory" runat="server" Width="280px">
                    <Items>
                        <telerik:DropDownListItem Text="Agency" Value="0001" />
                        <telerik:DropDownListItem Text="Venue (F&B, Banquet Room, Accommodation etc)" Value="0002" />
                        <telerik:DropDownListItem Text="Gift" Value="0003" />
                        <telerik:DropDownListItem Text="Printed Materials" Value="0004" />
                        <telerik:DropDownListItem Text="Non-PO items" Value="0005" />
                    </Items>
                </telerik:RadDropDownList>
                <telerik:RadButton ID="radBtnAddCategory" runat="server" AutoPostBack="false"
                    OnClientClicked="fn_OnAddCategoryClicked" Text="Add"
                    EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton"
                    CssClass="btn btn-blue btn-size1 bold">
                </telerik:RadButton>
            </div>
        </h3>
        <telerik:RadGrid ID="radGrdCostDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" ShowFooter="true"
            OnItemCommand="radGrdCostDetail_ItemCommand" Skin="EXGrid">
            <MasterTableView EditMode="Batch" ClientDataKeyNames="IDX">
                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderText="" HeaderStyle-Width="40px" UniqueName="IDX" ReadOnly="true"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CATEGORY_CODE" HeaderText="Category" HeaderStyle-Width="40px" UniqueName="CATEGORY_CODE" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="CATEGORY_NAME" UniqueName="CATEGORY_NAME"
                        HeaderText="Category" HeaderStyle-Width="50%">
                        <ItemTemplate>
                            <%# Eval("CATEGORY_NAME")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDropDownList ID="radGrdDdlCategory" runat="server" Width="105%" OnClientItemSelected="fn_OnCategorySelectedChanged">
                                <Items>
                                    <telerik:DropDownListItem Text="Agency" Value="0001" />
                                    <telerik:DropDownListItem Text="Venue (F&B, Banquet Room, Accommodation etc)" Value="0002" />
                                    <telerik:DropDownListItem Text="Gift" Value="0003" />
                                    <telerik:DropDownListItem Text="Printed Materials" Value="0004" />
                                    <telerik:DropDownListItem Text="Non-PO items" Value="0005" />
                                </Items>
                            </telerik:RadDropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn DataField="DESCRIPTION" HeaderText="Description" UniqueName="DESCRIPTION" HeaderStyle-Width="50%">
                        <ItemTemplate><%# Eval("DESCRIPTION")%></ItemTemplate>
                        <EditItemTemplate>
                            <%--<telerik:RadTextBox ID="radGrdTxtDescription" runat="server" Width="100%"></telerik:RadTextBox>--%>
                            <asp:TextBox ID="radGrdTxtDescription" runat="server" Width="100%" CssClass="input"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="QTY" HeaderText="QTY" HeaderStyle-Width="40px" UniqueName="QTY" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="UNIT_PRICE" HeaderText="Price" UniqueName="UNIT_PRICE" HeaderStyle-Width="100px"
                        Aggregate="Sum" FooterAggregateFormatString="{0:#,##0}"
                        FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
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
                    <telerik:GridBoundColumn DataField="AMOUNT" HeaderText="AMOUNT" HeaderStyle-Width="40px" UniqueName="AMOUNT" ReadOnly="true" Display="false"></telerik:GridBoundColumn>
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
    <div class="data_type1">
        <table style="margin: 20px 0 0 0">
            <colgroup>
                <col style="width: 25%;">
                <col>
            </colgroup>

            <tbody>
            <tr>
                <th> <span class="text_red">※ Attachment file</span>
                </th>
                <td style="word-break:keep-all;">
                     After event, it is <strong>required</strong> to be uploaded the supporting documents.<br />
                     Example) Final invoice of agency(incl. Third Party cost) and venue (incl. F&B, Accommodation cost), Wrap report(incl. feedback result, photo), Post event report (excl file) and other relevant data
                </td>
            </tr>
            </tbody>
        </table>
    </div>
    <input type="hidden" id="hddGridItems" runat="server" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="HddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />

</asp:Content>

