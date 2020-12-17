<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="CorporateCard.aspx.cs" Inherits="Approval_Document_CorporateCard" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function fn_DoSave(sender, args) {
            return true;
        }

        function fn_Category(sender, args) {
            var index = args.get_index();
            var Category = $find('<%= RadDbCategory.ClientID %>');
            var selectedValue = Category.get_items().getItem(index).get_value();

            if (selectedValue == "Re-issue") fn_ReissueCategory(selectedValue);
            if (selectedValue == "Increase") fn_IncreaseCategory(selectedValue);
            if (selectedValue == "Change") fn_ChangeCategory(selectedValue);

        }

        function fn_ReissueCategory(selectedValue) {
            $('#Reissue').show();
            $('#Increase').hide();
            $('#reason').hide();
            $('#change').hide();
        }

        function fn_IncreaseCategory(selectedValue, period, others) {
            $('#Increase').show();
            $('#Limit').show();
            $('#reason').show();
            $('#change').hide();
            $('#Reissue').hide();
            if (period == 'Temporary') {
                $("tr[value='Temporary']").show();
                $("tr[value='Permanent']").hide();
            }
            else if (period == 'Permanent') {
                $("tr[value='Temporary']").hide();
                $("tr[value='Permanent']").show();
            }

            //if (others == 'B')
            //    $('#Others').hide();
            //else if (others == 'S')
            //    $('#Others').hide();
            //else if (others == 'O')
            //    $('#Others').show();
        }

        function fn_ChangeCategory(category, bank, cell, address, name, password) {
            $('#change').show();
            $('#Reissue').hide();
            $('#Increase').hide();
            $('#reason').hide();
            if (bank == "Y") {
                $('#Bank1').show();
                $('#Bank2').show();
            }
            if (cell == "Y") {
                $('#CellPhone').show();
            }
            if (address == "Y") {
                $('#Addr').show();
            }
            if (name == "Y") {
                $('#Name').show();
            }
            if (password == "Y") {
                $('#Pwd').show();
            }
            if (bank == "N") {
                $('#Bank1').hide();
                $('#Bank2').hide();
            }
            if (cell == "N") {
                $('#CellPhone').hide();
            }
            if (address == "N") {
                $('#Addr').hide();
            }
            if (name == "N") {
                $('#Name').hide();
            }
            if (password == "N") {
                $('#Pwd').hide();
            }
        }

        function OnClientRadioChanged(sender, args) {
            var category = "Increase";
            var period;
            var others;
            period = sender.get_value();

            fn_IncreaseCategory(category, period, others);
        }
        function OnClientRadioChanged2(sender, args) {
            var category = "Increase";
            var period;
            var others;
            others = sender.get_value();

            fn_IncreaseCategory(category, period, others);
        }

        function OnClientCheckedChanged(sender, args) {
            var category = "Change";
            var bank;
            var cell;
            var address;
            var name;
            var password;
            var checkitem = sender.get_checked();
            if ($find('<%= RadrdoBank.ClientID %>').get_checked())
                bank = "Y";
            else
                bank = "N";
            if ($find('<%= RadrdoCellPhone.ClientID %>').get_checked())
               cell = "Y"
           else
               cell = "N";
           if ($find('<%= RadrdoAddr.ClientID %>').get_checked())
               address = "Y"
           else
               address = "N";
           if ($find('<%= RadrdoName.ClientID %>').get_checked())
               name = "Y"
           else
               name = "N";
<%--           if ($find('<%= RadrdoPwd.ClientID %>').get_checked())
               password = "Y"
           else
               password = "N";--%>

           if (category == "Change") fn_ChangeCategory(category, bank, cell, address, name, password);
       }

       function fn_keyPress(sender, args) {
           var text = sender.get_value() + args.get_keyCharacter();
           if (!text.match('^[0-9-,]+$'))           
               args.set_cancel(true);
       }


      
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <h3>Category <span class="text_red">*</span></h3>
        <div class="doc_style2 mb20">
            <telerik:RadDropDownList ID="RadDbCategory" runat="server" Width="100%" DefaultMessage="--- Select ---" AutoPostBack="false" OnClientSelectedIndexChanged="fn_Category" DropDownWidth="880px">
                <Items>
                    <telerik:DropDownListItem Text="Re-issue Corporate Card" Value="Re-issue" />
                    <telerik:DropDownListItem Text="Increase of Credit Limit" Value="Increase" />
                    <telerik:DropDownListItem Text="Change information of Corporate Card" Value="Change" />
                </Items>
            </telerik:RadDropDownList>
        </div>

        <div id="Reissue" class="data_type1" style="display: none">
            <h3>For Re-issue Corporate Card</h3>
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Date of Birth</th>
                        <td>
                            <telerik:RadDatePicker runat="server" ID="RadDatePicker1" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR"
                                MinDate="1900-01-01" MaxDate="2050-12-31" >
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="1900-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <th>Reason</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadReasonValue" Width="100%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            새카드번호가 컨커에 등록 될 수 있도록 <b>080-653-0880</b> 으로 연락하셔서 등록해 주시기 바랍니다.

                        </td>
                    </tr>

                </tbody>
            </table>
        </div>

        <div id="Increase" class="data_type1" style="display: none">
            <h3>For Increase of Credit Limit</h3>
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                    <col style="width: 25%;" />
                    <col style="width: 25%;" />
                    <col style="width: 25%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Card Number</th>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="RadtxtCardNumber" MaxLength="19" Width="50%">
                                <ClientEvents OnKeyPress="fn_keyPress" />
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Period for increase</th>
                        <td colspan="3">
                            <telerik:RadButton runat="server" ID="RadrdoTemporary" ButtonType="ToggleButton" ToggleType="Radio" Text="Temporary" Value="Temporary" GroupName="Period" OnClientCheckedChanged="OnClientRadioChanged" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="RadrdoPermanent" ButtonType="ToggleButton" ToggleType="Radio" Text="Permanent" Value="Permanent" GroupName="Period" OnClientCheckedChanged="OnClientRadioChanged" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr id="rowStartingDate" style="display: none" value="Temporary">
                        <th>Starting Date</th>
                        <td>
                            <telerik:RadDatePicker runat="server" ID="RaddpStardata" Calendar-ShowRowHeaders="false" CssClass="" Width="100px" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#ff748c" />
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                        <th>Period</th>
                        <td>
                            <telerik:RadComboBox runat="server" ID="RadcbPeriod" CssClass="sel" Width="97%" AutoPostBack="false">
                                <Items>
                                    <telerik:RadComboBoxItem Text="1M (1개월)" Value="1" Selected="true" />
                                    <telerik:RadComboBoxItem Text="2M (2개월)" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr id="rowIncreaseAmount" style="display: none" value="Temporary">
                        <th>Increase Amount</th>
                        <td colspan="3">
                            <telerik:RadNumericTextBox runat="server" ID="RadtxtInAmount" InputType="Number" NumberFormat-AllowRounding="false" Width="98%">
                                <ClientEvents OnKeyPress="fn_keyPress" />
                            </telerik:RadNumericTextBox>
                        </td>
                    </tr>
                    <tr id="rowIncreaseAmount2" style="display: none" value="Permanent">
                        <th>Increase Amount</th>
                        <td colspan="3">
                            <telerik:RadButton runat="server" ID="RadrdoAmount1" ButtonType="ToggleButton" ToggleType="Radio" Text="7,500,000" Value="7,500,000" GroupName="AmountInt" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="RadrdoAmount2" ButtonType="ToggleButton" ToggleType="Radio" Text="10,000,000" Value="10,000,000" GroupName="AmountInt" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Reason</th>
                        <td colspan="3">
                            <telerik:RadButton runat="server" ID="RadrdoReason1" ButtonType="ToggleButton" ToggleType="Radio" Text="Business Trip" GroupName="Reason" Value="B" OnClientCheckedChanged="OnClientRadioChanged2" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="RadrdoReason2" ButtonType="ToggleButton" ToggleType="Radio" Text="Seminar" GroupName="Reason" Value="S" OnClientCheckedChanged="OnClientRadioChanged2" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="RadrdoReason3" ButtonType="ToggleButton" ToggleType="Radio" Text="Others" GroupName="Reason" Value="O" OnClientCheckedChanged="OnClientRadioChanged2" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr id="Others">
                        <th>Detailed description</th>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="RadtxtOthers" Width="100%"></telerik:RadTextBox>
                        </td>
                    </tr>


                </tbody>
            </table>
            <div id="reason" style="display: none">
                <table>
                    <colgroup>
                        <col style="width: 25%;" />
                        <col />
                        <col style="width: 25%;" />
                        <col style="width: 25%;" />
                        <col style="width: 25%;" />
                    </colgroup>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>

        <div id="change" style="display: none">
            <h3>For Change information of Corporate Card</h3>
            <div class="data_type1">
                <table>
                    <colgroup>
                        <col style="width: 30%;" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>Card Number</th>
                            <td>
                                <telerik:RadTextBox runat="server" ID="RadTextCardNumber2" MaxLength="19" CssClass="input" Width="50%">
                                    <ClientEvents OnKeyPress="fn_keyPress" />
                                </telerik:RadTextBox>
                                <%-- <telerik:RadNumericTextBox runat="server" ID="RadTextCardNumber2" NumberFormat-AllowRounding="false" NumberFormat-DecimalDigits="0" Width="50%" MaxLength="19">
                                     <ClientEvents OnKeyPress="fn_keyPress" />
                                </telerik:RadNumericTextBox>--%>
                            </td>
                        </tr>
                        <tr>
                            <th>Please Select the item for changement</th>
                            <td>
                                <telerik:RadButton runat="server" ID="RadrdoBank" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="BANK" Value="Bank" OnClientCheckedChanged="OnClientCheckedChanged" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="RadrdoCellPhone" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Cell Phone" Value="CellPhone" OnClientCheckedChanged="OnClientCheckedChanged" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="RadrdoAddr" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Address" Value="Addr" OnClientCheckedChanged="OnClientCheckedChanged" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="RadrdoName" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Name" Value="Name" OnClientCheckedChanged="OnClientCheckedChanged" AutoPostBack="false"></telerik:RadButton>
                                <telerik:RadButton runat="server" ID="RadrdoPwd" CssClass="check" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Password" Value="Pwd" OnClientCheckedChanged="OnClientCheckedChanged" AutoPostBack="false" Visible="false"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr id="Bank1" style="display: none">
                            <th>Bank Name</th>
                            <td>
                                <telerik:RadTextBox runat="server" ID="RadtxtBankName" CssClass="input" Width="98%"></telerik:RadTextBox></td>
                        </tr>
                        <tr id="Bank2" style="display: none">
                            <th>Bank Account Number</th>
                            <td>
                                <telerik:RadTextBox runat="server" ID="RadtxtBankNumber" CssClass="input" Width="98%">
                                    <ClientEvents OnKeyPress="fn_keyPress" />
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr id="CellPhone" style="display: none">
                            <th>Cell Phone Number</th>
                            <td>
                                <telerik:RadTextBox runat="server" ID="RadtxtCell" Width="98%">
                                    <ClientEvents OnKeyPress="fn_keyPress" />
                                </telerik:RadTextBox></td>
                        </tr>
                        <tr id="Addr" style="display: none">
                            <th>Address</th>
                            <td>
                                <telerik:RadTextBox runat="server" ID="RadtxtAddr" CssClass="input" Width="98%"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr id="Name" style="display: none">
                            <th>Name</th>
                            <td>
                                <telerik:RadTextBox runat="server" ID="RadtxtName" CssClass="input" Width="98%"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr id="Pwd" style="display: none">
                            <th>New Password</th>
                            <td>
                                <telerik:RadTextBox runat="server" ID="RadtxtNewpwd" CssClass="input" Width="98%"></telerik:RadTextBox>
                                <br />
                                <span style="color: #FF284D">주의</span> : 비밀번호 변경시 해외에서의 현금서비스 거래 불가하오니 출국예정자는 반드시 훼손재발급 신청바랍니다.
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddPeriod" runat="server" />
    <input type="hidden" id="hddChangement" runat="server" />    
    <input type="hidden" id="hddReuse" runat="server" />    
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

