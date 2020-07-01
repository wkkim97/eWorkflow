<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="PIActivity.aspx.cs" Inherits="Approval_Document_PIActivity" %>

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
            var masterTable = $find('<%= radGrdCostDetail.ClientID %>').get_masterTableView();
            $find('<%= radGrdCostDetail.ClientID %>').get_batchEditingManager().saveChanges(masterTable);

            var dataItems = masterTable.get_dataItems();
            var maxIdx = 0;
            for (var i = 0; i < dataItems.length; i++) {

                var idx = dataItems[i].get_cell("IDX").innerText;
                var categoryCode = dataItems[i].get_cell("CATEGORY_CODE").innerText;
                var categoryName = dataItems[i].get_cell("CATEGORY_NAME").children[0].innerText;

                var description = dataItems[i].get_cell("DESCRIPTION").children[0].innerText;
                var qty = dataItems[i].get_cell("QTY").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
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
                conObj.QTY = qty;
                conObj.UNIT_PRICE = price;
                conObj.AMOUNT = parseFloat(qty) * parseFloat(price);
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
                conObj.QTY = 0;
                conObj.UNIT_PRICE = 0;
                conObj.AMOUNT = 0;

                list.push(conObj);
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

        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdCostDetail.ClientID%>');
            if (grid.get_masterTableView()) {
                var dataItems = grid.get_masterTableView().get_dataItems();
                if (dataItems.length > 0) {
                    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                }
            }
        }

        function SetTotal() {
            var masterTable = $find('<%= radGrdCostDetail.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var total = 0;
            for (var i = 0; i < dataItems.length; i++) {
                var amount = dataItems[i].get_cell("AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');

                amount = parseInt(amount);

                total += amount;
            }

            masterTable.get_element().tFoot.rows[0].cells[6].innerText = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function fn_OnGridNumBlur(sender, args) {
            setNumberFormat(sender);
        }

        function fn_OnGridKeyUp(sender) {
            var row = $find('<%= radGrdCostDetail.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
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

        function fn_OnAttendeeKeyUp(sender) {
            var noGo = $('#<%= radNumGO.ClientID %>').val();
            noGo = noGo.replace(/,/gi, '').replace(/ /gi, '');
            if (!noGo) noGo = 0;
            var noNonGo = $('#<%= radNumNonGO.ClientID %>').val();
            noNonGo = noNonGo.replace(/,/gi, '').replace(/ /gi, '');
            if (!noNonGo) noNonGo = 0;
            var noPrivate = $('#<%= radNumFarmer.ClientID %>').val();
            noPrivate = noPrivate.replace(/,/gi, '').replace(/ /gi, '');
            if (!noPrivate) noPrivate = 0;
            var noBayerEmp = $('#<%= radNumBayerEmp.ClientID %>').val();
            noBayerEmp = noBayerEmp.replace(/,/gi, '').replace(/ /gi, '');
            if (!noBayerEmp) noBayerEmp = 0;

            var total = parseInt(noGo) + parseInt(noNonGo) + parseInt(noPrivate) + parseInt(noBayerEmp);

            $('#<%= lblTotalParticipants.ClientID %>').html(total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        }

        function setVisibleControl(value) {
            if (value == "IP")
                $('#tblCheckBox').show();
            else
                $('#tblCheckBox').hide();
        }

        function fn_OnIncentiveCheckedChanged(sender, args) {
            if (args.get_checked())
                $('#tblCheckBox').show();
            else
                $('#tblCheckBox').hide();
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
                        <th>Subject <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadTextBox ID="radTxtTitle" runat="server" Width="100%"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <th>BG <span class="text_red">*</span></th>
                        <td colspan="3">
                            <telerik:RadButton ID="radBtnCP" runat="server" Text="CP" Value="CP" GroupName="BU"
                                AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnIS" runat="server" Text="IDS" Value="IS" GroupName="BU"
                                AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnBVS" runat="server" Text="BVS" Value="BVS" GroupName="BU"
                                AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" Visible="false">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnES" runat="server" Text="ES" Value="ES" GroupName="BU"
                                AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Request Type<span class="text_red">*</span></th>
                        <td colspan="3">
                            <telerik:RadButton ID="radBtnRequestTypePlan" runat="server" Text="P&I Budget & Planning" Value="Plan" GroupName="REQUESTTYPE"
                                AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnRequestTypeActivity" runat="server" Text="세부 Activity 신청" Value="Activity" GroupName="REQUESTTYPE"
                                AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Activity Type <span class="text_red">*</span></th>
                        <td>
                            <div id="divActivityType" style="margin: 0 0 0 0">
                                <telerik:RadButton ID="radBtnProfessional" runat="server" Text="Professional Event" Value="PE" GroupName="ActivityType"
                                    AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnIncentive" runat="server" Text="Incentive Program" Value="IP" GroupName="ActivityType"
                                    AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" OnClientCheckedChanged="fn_OnIncentiveCheckedChanged">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnProduct" runat="server" Text="Product Campaign" Value="PC" GroupName="ActivityType"
                                    AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnCampaign" runat="server" Text="Sponsorship" Value="SS" GroupName="ActivityType"
                                    AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnTradePromotion" runat="server" Text="Trade Promotion" Value="TP" GroupName="ActivityType"
                                    AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" ToolTip="Trade Promotion 시행 시에는 반드시 해당규정의 owner /caretaker또는 LPC 담당자 사전 협의를 거쳐주시기 바랍니다.">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radBtnOther" runat="server" Text="기타" Value="Other" GroupName="ActivityType"
                                    AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" >
                                </telerik:RadButton>

                                <table id="tblCheckBox" style="margin-bottom: 5px; display: none;">
                                    <colgroup>
                                        <col style="width: 25%" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <th>Incentive Agreement</th>
                                        <td>
                                            <telerik:RadButton ID="radChkIncentiveAgree" runat="server" Text="Yes" AutoPostBack="false"
                                                ButtonType="ToggleButton" ToggleType="CheckBox">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Incentive Checklist</th>
                                        <td>
                                            <telerik:RadButton ID="radChkIncentiveCheck" runat="server" Text="Yes" AutoPostBack="false"
                                                ButtonType="ToggleButton" ToggleType="CheckBox">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>Cost Center & Approval Budget <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadDropDownList ID="radDropCostCenter" runat="server" Width="400px" DataTextField="ACTIVITY_NAME" DataValueField="ACTIVITY_CODE"></telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Meeting venue</th>
                        <td>
                            <telerik:RadTextBox ID="radTxtMeetingVenue" runat="server" Width="100%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Date & Time</th>
                        <td>
                            <telerik:RadDatePicker ID="radDatFromMeeting" runat="server" Width="100px" Calendar-ShowRowHeaders="false"
                                MinDate="1900-01-01" MaxDate="2050-12-31" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                            <telerik:RadTimePicker ID="radTimeFromMeeting" runat="server" Width="100px"></telerik:RadTimePicker>
                            ~
                            <telerik:RadDatePicker ID="radDateToMeeting" runat="server" Width="100px" Calendar-ShowRowHeaders="false"
                                MinDate="1900-01-01" MaxDate="2050-12-31" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                            <telerik:RadTimePicker ID="radTimeToMeeting" runat="server" Width="100px"></telerik:RadTimePicker>

                        </td>
                    </tr>
                    <tr>
                        <th>Address venue</th>
                        <td>
                            <telerik:RadTextBox ID="radTxtAddressVenue" runat="server" Width="100%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Purpose / Objective <span class="text_red">*</span> </th>
                        <td colspan="3">
                            <telerik:RadTextBox ID="radTxtPurpose" runat="server" TextMode="MultiLine" Height="50px" Width="100%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Approved P&I Budget e-WF number reference</th>
                        <td>
                            <telerik:RadTextBox ID="radTxtRelevant_e_WorkflowNo" EmptyMessage="YYYY-BPI-000" runat="server" Width="100%"></telerik:RadTextBox></td>
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
                    <th>Governmental Officer(공무원)</th>
                    <td>
                        <%--                        <telerik:RadNumericTextBox ID="radNumGO" runat="server" NumberFormat-DecimalDigits="0"
                            MinValue="0" MaxValue="999999999" EnabledStyle-HorizontalAlign="Right" Value="0"
                            ClientEvents-OnBlur="fn_OnAttendeeBlur">
                        </telerik:RadNumericTextBox>--%>
                        <asp:TextBox ID="radNumGO" runat="server" Width="70px" CssClass="input align_right" 
                            onblur="return fn_OnGridNumBlur(this)"
                            onfocus="return fn_OnGridNumFocus(this)"
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="return fn_OnAttendeeKeyUp(this)"
                            DecimalDigits="0" Text="0">                                
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>Non-Governmental Officer(비공무원)</th>
                    <td>
                        <%--                        <telerik:RadNumericTextBox ID="radNumNonGO" runat="server" NumberFormat-DecimalDigits="0"
                            MinValue="0" MaxValue="999999999" EnabledStyle-HorizontalAlign="Right" Value="0"
                            ClientEvents-OnBlur="fn_OnAttendeeBlur">
                        </telerik:RadNumericTextBox>--%>
                        <asp:TextBox ID="radNumNonGO" runat="server" Width="70px" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)"
                            onfocus="return fn_OnGridNumFocus(this)"
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="return fn_OnAttendeeKeyUp(this)"
                            DecimalDigits="0" Text="0">                                
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>Farmer(농민)</th>
                    <td>
                        <%--                        <telerik:RadNumericTextBox ID="radNumFarmer" runat="server" NumberFormat-DecimalDigits="0"
                            MinValue="0" MaxValue="999999999" EnabledStyle-HorizontalAlign="Right" Value="0"
                            ClientEvents-OnBlur="fn_OnAttendeeBlur">
                        </telerik:RadNumericTextBox>--%>
                        <asp:TextBox ID="radNumFarmer" runat="server" Width="70px" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)"
                            onfocus="return fn_OnGridNumFocus(this)"
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="return fn_OnAttendeeKeyUp(this)"
                            DecimalDigits="0" Text="0">                                
                        </asp:TextBox>
                    </td>
                </tr>
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
                    <th>Total Participants</th>
                    <td>
                        <asp:Label ID="lblTotalParticipants" runat="server" Text="0" Width="70px" CssClass="align_right" Style="font-weight: bold;"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <h3>Cost Detail
            <div class="title_btn">
                <telerik:RadDropDownList ID="radDdlCategory" runat="server" Width="250px">
                    <Items>
                        <telerik:DropDownListItem Text="Meal & Beverage" Value="0001" />
                        <telerik:DropDownListItem Text="Venue & Banquet Room" Value="0002" />
                        <telerik:DropDownListItem Text="Transportation" Value="0003" />
                        <telerik:DropDownListItem Text="Promotional Item & Gimmick" Value="0004" />
                        <telerik:DropDownListItem Text="Accommodation" Value="0005" />
                        <telerik:DropDownListItem Text="Prints & DM" Value="0006" />
                        <telerik:DropDownListItem Text="Honorarium" Value="0007" />
                        <telerik:DropDownListItem Text="Agency fee" Value="0008" />
                        <telerik:DropDownListItem Text="Equpment rental fee" Value="0009" />
                        <telerik:DropDownListItem Text="Sample" Value="0010" />
                        <telerik:DropDownListItem Text="Trade Promotional Item" Value="0011" />
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
                                    <telerik:DropDownListItem Text="Meal & Beverage" Value="0001" />
                                    <telerik:DropDownListItem Text="Venue & Banquet Room" Value="0002" />
                                    <telerik:DropDownListItem Text="Transportation" Value="0003" />
                                    <telerik:DropDownListItem Text="Promotional Item & Gimmick" Value="0004" />
                                    <telerik:DropDownListItem Text="Accommodation" Value="0005" />
                                    <telerik:DropDownListItem Text="Prints & DM" Value="0006" />
                                    <telerik:DropDownListItem Text="Honorarium" Value="0007" />
                                    <telerik:DropDownListItem Text="Agency fee" Value="0008" />
                                    <telerik:DropDownListItem Text="Equpment rental fee" Value="0009" />
                                    <telerik:DropDownListItem Text="Sample" Value="0010" />
                                    <telerik:DropDownListItem Text="Trade Promotional Item" Value="0011" />
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
                    <telerik:GridTemplateColumn DataField="QTY" HeaderText="Qty" UniqueName="QTY" HeaderStyle-Width="80px">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("Qty")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<telerik:RadNumericTextBox runat="server" ID="radGrdNumQty" NumberFormat-DecimalDigits="0"
                                MinValue="0" MaxValue="999999999"
                                Width="100%" EnabledStyle-HorizontalAlign="Right" ClientEvents-OnBlur="fn_OnGridNumBlur">
                            </telerik:RadNumericTextBox>--%>
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
                            <%--<telerik:RadNumericTextBox runat="server" ID="radGrdNumPrice" NumberFormat-DecimalDigits="0"
                                MinValue="0" MaxValue="99999999999"
                                Width="100%" EnabledStyle-HorizontalAlign="Right" ClientEvents-OnBlur="fn_OnGridNumBlur">
                            </telerik:RadNumericTextBox>--%>
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

                    <%--                    <telerik:GridCalculatedColumn HeaderText="Amount" UniqueName="AMOUNT" DataType="System.Double" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right"
                        DataFields="QTY, UNIT_PRICE" Expression='{0}*{1}' Aggregate="Sum" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:#,##0}"
                        FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                    </telerik:GridCalculatedColumn>--%>
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
    <input type="hidden" id="HddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />

</asp:Content>

