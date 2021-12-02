<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="CreditDebitNote.aspx.cs" Inherits="Approval_Document_CreditDebitNote" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <style type="text/css">
        
    </style>
    <script type="text/javascript">

        function pageLoad() {
            <%--var addRow = $('#<%= hddAddRow.ClientID %>').val();

            if (addRow == 'Y') {
                var grid = $find('<%=radGrdDescription.ClientID%>');
                if (grid.get_masterTableView()) {
                    var dataItems = grid.get_masterTableView().get_dataItems();
                    if (dataItems.length > 0) {
                        grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                    }
                }
                $('#<%= hddAddRow.ClientID %>').val('N');
            } --%>
        }

        function fn_UpdateGridDataFromUploader() {
            fn_UpdateGridData(false);
        }
        var byFileUploader = false;
        function onRequestStart(sender, args) {
            if (args.get_eventArgument() == 'DeleteAttachFile' || args.get_eventArgument() == 'UploadFiles')
                byFileUploader = true;
            else
                byFileUploader = false;
        }
        var _nameCtrlAttn = null;
        $(document).ready(function () {

            try {
                if (window.ActiveXObject) {
                    _nameCtrlAttn = new ActiveXObject("Name.NameCtrl");

                } else {
                    _nameCtrlAttn = CreateNPApiOnWindowsPlugin("application/x-sharepoint-uc");
                }
            }
            catch (ex) { }
        });

        function CreateNPApiOnWindowsPlugin(b) {
            var c = null;
            if (IsSupportedNPApiBrowserOnWin())
                try {
                    c = document.getElementById(b);
                    if (!Boolean(c) && IsNPAPIOnWinPluginInstalled(b)) {
                        var a = document.createElement("object");
                        a.id = b;
                        a.type = b;
                        a.width = "0";
                        a.height = "0";
                        a.style.setProperty("visibility", "hidden", "");
                        document.body.appendChild(a);
                        c = document.getElementById(b)
                    }
                } catch (d) {
                    c = null
                }
            return c

        }


        function fn_DoRequest(sender, arsg) {
            return fn_UpdateGridData(false);
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData(false);
        }

        function fn_OpenCompanyList(sender, args) {
            var wnd = $find("<%= radWinCompany.ClientID %>");
            wnd.setUrl("/eWorks/Common/Popup/CompanyList.aspx?AllowMultiSelect=false");
            wnd.show();
            sender.set_autoPostBack(false);
        }

        function checkEmail(emailValue) {
            var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
            return emailPattern.test(emailValue);
        }

        //메일형식 확인
        function fn_OnEntryAdding(sender, args) {
            var entry = args.get_entry();
            if (!entry.get_value()) {
                if (entry.get_text().trim().length < 1 || !checkEmail(entry.get_text())) {
                    fn_OpenDocInformation('메일형식을 확인 바랍니다.');
                    args.set_cancel(true);
                }
            }
        }

        function onStatusChange2(name, status, id) {

            switch (status) {
                case 0:
                    document.getElementById("attn_" + id).style.backgroundColor = "#5DD255";
                    break;
                case 1:
                    document.getElementById("attn_" + id).style.backgroundColor = "#B6CFD8";
                    break;
                case 2:
                    document.getElementById("attn_" + id).style.backgroundColor = "#FFD200";
                    break;
                case 3:
                    document.getElementById("attn_" + id).style.backgroundColor = "#D00E0D";
                    break;
                case 4:
                    document.getElementById("attn_" + id).style.backgroundColor = "#FFD200";
                    break;
                case 5:
                    document.getElementById("attn_" + id).style.backgroundColor = "#D00E0D";
                    break;
                case 6:
                    document.getElementById("attn_" + id).style.backgroundColor = "#D00E0D";
                    break;
                case 7:
                    document.getElementById("attn_" + id).style.backgroundColor = "#D00E0D";
                    break;
                case 8:
                    document.getElementById("attn_" + id).style.backgroundColor = "#E57A79";
                    break;
                case 9:
                    document.getElementById("attn_" + id).style.backgroundColor = "#D00E0D";
                    break;
                case 15:
                    document.getElementById("attn_" + id).style.backgroundColor = "#D00E0D";
                    break;
                case 16:
                    document.getElementById("attn_" + id).style.backgroundColor = "#FFD200";
                    break;
                default:
                    document.getElementById("attn_" + id).style.backgroundColor = "#B6CFD8";
                    break;
            }

        }

        function ShowOOUI(sipUri) {
            _nameCtrlAttn.ShowOOUI(sipUri, 0, 15, 15);
        }

        function HideOOUI(sipUri) {
            _nameCtrlAttn.HideOOUI();
        }
        function fn_OnEntryAdded(sender, args) {
            var entry = args.get_entry();

            if (entry.get_value()) {
                var user = JSON.parse(entry.get_value());
                if (_nameCtrlAttn && _nameCtrlAttn.PresenceEnabled) {
                    var token = args.get_entry().get_token();
                    _nameCtrlAttn.OnStatusChange = onStatusChange2;
                    var userAddress = user.MAIL_ADDRESS;
                    var userId = user.USER_ID;
                    if (token.addEventListener) {
                        token.addEventListener('mouseover', function (e) { _nameCtrlAttn.ShowOOUI(userAddress, 0, e.clientX - 10, e.clientY - 10); });
                        token.addEventListener('mouseout', function () { _nameCtrlAttn.HideOOUI(); })
                    }
                    else {
                        token.attachEvent("onmouseover", function (e) { _nameCtrlAttn.ShowOOUI(userAddress, 0, e.clientX - 10, e.clientY - 10); });
                        token.attachEvent("onmouseout", function () { _nameCtrlAttn.HideOOUI(); });
                    }

                    var status = _nameCtrlAttn.GetStatus(userAddress, userId);
                    $telerik.$(args.get_entry().get_token()).prepend("<span class='lync_status' id='attn_" + userId + "' onmouseover=ShowOOUI('" + userAddress + "') onmouseout=HideOOUI('" + userAddress + "') />")
                }
            }
        }

        function setSelectedCompany(oWnd, args) {
            var uListData = args.get_argument();
            if (uListData != null) {
                if (uListData.length > 0) {
                    var companyId = uListData[0].COMPANY_ID;
                    var name = uListData[0].NAME;
                    $find('<%= radTxtCompany.ClientID %>').set_value(name + '(' + companyId + ')');
                    $('#<%= hddCompanyId.ClientID %>').val(companyId);
                }
            }
        }

        var clickedKey = null;

        function openConfirmPopUp(index) {
            clickedKey = parseInt(index);
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);
            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                fn_UpdateGridData(false);
                var masterTable = $find('<%= radGrdDescription.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        function checkLastRow() {
            fn_UpdateGridData(false);
            var masterTable = $find('<%= radGrdDescription.ClientID %>').get_masterTableView();
            $find('<%= radGrdDescription.ClientID %>').get_batchEditingManager().saveChanges(masterTable);
            var dataItems = masterTable.get_dataItems();
            if (dataItems.length > 0) {
                var lastItem = dataItems[dataItems.length - 1];
                var description = lastItem.get_cell("DESCRIPTION").children[0].innerText.trim();
                var amount = lastItem.get_cell("AMOUNT").children[0].innerText.trim();

                if (description.length < 1 || amount.length < 1) {
                    fn_OpenDocInformation('자료를 입력바랍니다.');
                    return false;
                } else
                    return true;
            } else
                return true;
        }
        function fn_UpdateGridData(addRow) {
            var list = [];
            var masterTable = $find('<%= radGrdDescription.ClientID %>').get_masterTableView();
            $find('<%= radGrdDescription.ClientID %>').get_batchEditingManager().saveChanges(masterTable);
            var dataItems = masterTable.get_dataItems();

            var total = 0;
            var maxIdx = 0;
            for (var i = 0; i < dataItems.length; i++) {

                var idx = dataItems[i].get_cell("IDX").innerText;
                var description = dataItems[i].get_cell("DESCRIPTION").children[0].innerText;
                var amount = dataItems[i].get_cell("AMOUNT").children[0].innerText.replace(/,/gi, '').replace(/ /gi, '');
                if (amount.length < 1) amount = 0;

                var conObj = {
                    IDX: null,
                    DESCRIPTION: null,
                    AMOUNT: null,
                }
                conObj.IDX = idx;
                conObj.DESCRIPTION = description;
                conObj.AMOUNT = amount;

                maxIdx = idx;
                list.push(conObj);
            }
            if (addRow) {
                var conObj = {
                    IDX: null,
                    DESCRIPTION: null,
                    AMOUNT: null,
                }
                conObj.IDX = ++maxIdx;
                conObj.DESCRIPTION = '';
                conObj.AMOUNT = 0;
                list.push(conObj);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));


            return true;
        }

        function fn_OnAddButtonClicked(sender, args) {
            if (checkLastRow()) {
                fn_UpdateGridData(true);
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind");
            }
        }

        function fn_OnTypeCheckedChanged(sender, args) {
            if (sender.get_text() == 'Credit' && args.get_checked()) {
                $('#title1').text('Credit Note');
                $('#title2').text('We credit you as follows');
            }
            else if (sender.get_text() == 'Debit' && args.get_checked()) {
                $('#title1').text('Debit Note');
                $('#title2').text('We debit you as follows');
            }
        }

        function fn_OnInvoiceSelected(sender, args) {
            var dtInvoice = args.get_newValue().replace(/-/g, '/'); //ie7/8에서 '-'를 '/'

            var date = new Date(dtInvoice);
            var newdate = new Date(date);

            newdate.setDate(newdate.getDate() + 60);

            var dd = newdate.getDate();
            var mm = newdate.getMonth();
            var y = newdate.getFullYear();

            var addedDate = new Date(y, mm, dd, 0, 0, 0, 0);
            $find('<%= radDateDue.ClientID %>').set_selectedDate(addedDate);
        }

        function SetTotal() {
            var masterTable = $find('<%= radGrdDescription.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var total = 0.00;
            for (var i = 0; i < dataItems.length; i++) {
                var amount = 0;

                var rtxt = dataItems[i].findElement('radGrdNumAmount');
                if (rtxt) {
                    amount = rtxt.value.replace(/,/gi, '').replace(/ /gi, '');
                }
                else
                    amount = dataItems[i].get_cell("AMOUNT").innerText.replace(/,/gi, '').replace(/ /gi, '');

                amount = TruncateDecimals(amount, 3);

                total += amount;
            }
            total = TruncateDecimals(total, 2);
            masterTable.get_element().tFoot.rows[0].cells[2].innerText = total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

        }


        function fn_OnGridNumBlur(sender) {
            setNumberFormat(sender);
        }

        function fn_OnGridKeyUp(sender) {
            SetTotal();
        }

        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdDescription.ClientID%>');

            if (grid.get_masterTableView()) {
                var dataItems = grid.get_masterTableView().get_dataItems();
                if (dataItems.length > 0) {
                    grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                }
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">

    <div class="doc_style">
        <div id="divLink" class="align_right pb10" style="display: none;" runat="server">
            
        </div>
        <h3 id="title1">Debit Note</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                    <col style="width: 25%;" />
                    <col style="width: 25%;" />
                </colgroup>

                <tr>
                    <th>Type
                    </th>
                    <td>
                        <telerik:RadButton ID="radBtnDebit" runat="server" Value="DEBIT"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="DocType" Text="Debit" AutoPostBack="false"  
                            OnClientCheckedChanged="fn_OnTypeCheckedChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnCredit" runat="server" Value="CREDIT"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="DocType" Text="Credit" AutoPostBack="false"
                            OnClientCheckedChanged="fn_OnTypeCheckedChanged">
                        </telerik:RadButton>
                    </td>
                    <th>Company
                    </th>
                    <td>
                        <telerik:RadDropDownList ID="radDdlCompany" runat="server" DataTextField="NAME" DataValueField="COMPANY_ID" Width="99%" DropDownWidth="200px">
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <th>To
                    </th>
                    <td colspan="3">
                        <telerik:RadTextBox ID="radTxtCompany" runat="server" ReadOnly="true" Width="96%" EnableViewState="false"></telerik:RadTextBox>
                        <telerik:RadButton ID="radBtnCompanySearch" runat="server" Text="" OnClientClicked="fn_OpenCompanyList"
                            CssClass="btn_grid" Width="18px" Height="18px">
                            <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                        </telerik:RadButton>
                        <input type="hidden" id="hddCompanyId" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>Attn
                    </th>
                    <td colspan="3">
                        <telerik:RadAutoCompleteBox ID="radAcomAttn" runat="server" AllowCustomEntry="true" OnClientEntryAdding="fn_OnEntryAdding" OnClientEntryAdded="fn_OnEntryAdded" Width="100%"
                            DropDownWidth="300px">
                            <WebServiceSettings Method="SearchGlobalUserByName" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                        </telerik:RadAutoCompleteBox>
                    </td>
                </tr>
                <tr>
                    <th>CC
                    </th>
                    <td colspan="3">
                        <telerik:RadAutoCompleteBox ID="radAcomCC" runat="server" AllowCustomEntry="true" OnClientEntryAdding="fn_OnEntryAdding" OnClientEntryAdded="fn_OnEntryAdded" Width="100%"
                            DropDownWidth="300px">
                            <WebServiceSettings Method="SearchGlobalUserByName" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
                        </telerik:RadAutoCompleteBox>
                    </td>
                </tr>
                <tr>
                    <th>Invoice Date
                    </th>
                    <td>
                        <telerik:RadDatePicker ID="radDateInvoice" runat="server" Width="100px" Calendar-ShowRowHeaders="false" Culture="ko-KR"
                            MaxDate="2050-12-31" MinDate="1900-01-01">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            <Calendar runat="server" RangeMaxDate="2050-12-31" RangeMinDate="1900-01-01">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                </SpecialDays>
                            </Calendar>

                            <ClientEvents OnDateSelected="fn_OnInvoiceSelected" />
                        </telerik:RadDatePicker>
                    </td>
                    <th>Due Date
                    </th>
                    <td>
                        <telerik:RadDatePicker ID="radDateDue" runat="server" Width="100px" Calendar-ShowRowHeaders="false" Culture="ko-KR"
                            MaxDate="2050-12-31" MinDate="1900-01-01">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            <Calendar runat="server" RangeMaxDate="2050-12-31" RangeMinDate="1900-01-01">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                </SpecialDays>
                            </Calendar>
                        </telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
                    <th>Description</th>
                    <td colspan="3">
                        <telerik:RadTextBox ID="radTxtDescription" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </div>
        <h3>
            <p id="title2" style="font-size: 14px; font-weight: bold; color: #446324">We debit you as follows</p>
            <div class="title_btn">
                <telerik:RadDropDownList ID="radDdlCurrency" runat="server">
                    <Items>
                        <telerik:DropDownListItem Text="KRW" Value="KRW" />
                        <telerik:DropDownListItem Text="USD" Value="USD" />
                        <telerik:DropDownListItem Text="EUR" Value="EUR" />
                        <telerik:DropDownListItem Text="SGD" Value="SGD" />
                        <telerik:DropDownListItem Text="JPY" Value="JPY" />
                        <telerik:DropDownListItem Text="AUD" Value="AUD" />
                        <telerik:DropDownListItem Text="TWD" Value="TWD" />
                        <telerik:DropDownListItem Text="NZD" Value="NZD" />
                        <telerik:DropDownListItem Text="HKD" Value="HKD" />
                    </Items>
                </telerik:RadDropDownList>
                <telerik:RadButton ID="radBtnAdd" runat="server" Text="Add" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                    ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" OnClientClicked="fn_OnAddButtonClicked" AutoPostBack="false">
                </telerik:RadButton>
                <%--                <telerik:RadButton ID="radBtnAmount" runat="server" Text="Amount" ButtonType="LinkButton"
                    CssClass="btn btn-blue btn-size1 bold" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" OnClientClicked="fn_OnAmountButtonClicked" OnClick="radBtnAmount_Click">
                </telerik:RadButton>--%>
            </div>
        </h3>
        <telerik:RadGrid ID="radGrdDescription" runat="server" AutoGenerateColumns="false"
            HeaderStyle-HorizontalAlign="Center"
            Skin="EXGrid" ClientDataKeyNames="IDX"
            OnItemCommand="radGrdDescription_ItemCommand" ShowFooter="true">
            <MasterTableView EditMode="Batch" ClientDataKeyNames="IDX">
                <FooterStyle ForeColor="Red" />
                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                <Columns>
                    <telerik:GridBoundColumn DataField="IDX" HeaderText="Index" HeaderStyle-Width="40px" UniqueName="IDX" ReadOnly="true" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn UniqueName="DESCRIPTION" HeaderText="Description" HeaderStyle-Width="100%">
                        <ItemTemplate><%#DataBinder.Eval(Container.DataItem, "DESCRIPTION")%></ItemTemplate>
                        <EditItemTemplate>
                            <%--<telerik:RadTextBox ID="radGrdTxtDescription" runat="server" Width="100%"></telerik:RadTextBox>--%>
                            <asp:TextBox ID="radGrdTxtDescription" runat="server" Width="100%" CssClass="input"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="AMOUNT" HeaderText="Amount" UniqueName="AMOUNT" HeaderStyle-Width="150px"
                        DataType="System.Decimal" Aggregate="Sum" FooterAggregateFormatString="{0:#,##0.###}"
                        FooterStyle-HorizontalAlign="Right" FooterStyle-ForeColor="Red">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0.###}", Eval("AMOUNT")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="radGrdNumAmount" runat="server" Width="100%" CssClass="input align_right"
                                onblur="return fn_OnGridNumBlur(this)"
                                onfocus="return fn_OnGridNumFocus(this)"
                                onkeypress="return fn_OnGridKeyPress(this, event)"
                                onkeyup="return fn_OnGridKeyUp(this)"
                                DecimalDigits="2" AllowNegative="false">                                
                            </asp:TextBox>
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
        <div class="data_type1">
            <table style="border: none !important; margin: 0 0 0 0 !important">
                <colgroup>
                    <col style="width: 50%;" />
                    <col style="width: 50%;" />
                </colgroup>
                <tr style="padding: 0 0 0 0">
                    <td style="border: none; padding: 0 0 0 0"></td>
                    <td style="border: none; padding: 0 0 0 0">
                        <table>
                            <colgroup>
                                <col style="width: 50%;" />
                                <col style="width: 50%;" />
                            </colgroup>
                            <tr>
                                <th>Local Amount (KRW)</th>
                                <td>
                                    <telerik:RadNumericTextBox ID="radNumLocalAmount" runat="server" NumberFormat-DecimalDigits="0" Width="100%"
                                        EnabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right">
                                    </telerik:RadNumericTextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>


    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="radWinCompany" runat="server" NavigateUrl="/eWorks/Common/Popup/CompanyList.aspx?AllowMultiSelect=true" Title="Company" Modal="true" Width="550" Height="600"
                OnClientClose="setSelectedCompany"
                VisibleStatusbar="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <input type="hidden" id="hddGridItems" runat="server" />
    <input type="hidden" id="hddTotalAmount" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />


</asp:Content>

