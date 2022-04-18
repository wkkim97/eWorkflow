<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="BusinessCard.aspx.cs" Inherits="Approval_Document_BusinessCard" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">    
    <style type="text/css">
        .windowscroll {
            overflow-y: hidden;
            overflow-x: hidden;
        }
        .font-blue{
            color:#0091DF !important;
        }
        .font-raspberry{
            color:#D30F4B !important;
        }
        .font-midpurple{
            color:#624963 !important;
        }
         .font-green{
            color:#66B512  !important;
        }
    </style>
    <script src="../../Scripts/Approval/List.js"></script>
    <script type="text/javascript">
       

        function fn_DoRequest(sender, args) {
            return true;
            
        }

        function fn_DoSave(sender, args) {
            return true;
        }

        //English Division Popup 창 띄우기
        function fn_OpenEngDivision(sender, Args) {
            var wnd = $find("<%= radWinPopupEngDivision.ClientID %>");
            wnd.setUrl("/eWorks/Common/Popup/PopupEngDivision.aspx");
            wnd.show();
            sender.set_autoPostBack(false);
        }
        //Korean Division Popup 창 띄우기
        function fn_OpenKorDivision(sender, Args) {
            var wnd = $find("<%= radWinPopupKorDivision.ClientID %>");
            wnd.setUrl("/eWorks/Common/Popup/KorDivision.aspx");
            wnd.show();
            sender.set_autoPostBack(false);
        }

        function fn_OnDisplayNameCard(sender, args) {
            $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Display");
        }
        function OpenDisplayNameCard(sender, args) {
            var argument = args.get_eventArgument();
            if (argument == "Display") {
                var wndCard = $find("<%= radWinPopupDisplayNameCard.ClientID %>");
                var processId = $('#<%= hddProcessID.ClientID %>').val();
                wndCard.setUrl("/eWorks/Approval/Link/BusinessCardView.aspx?processId=" + processId);
                wndCard.show();
            }

        }

        //English Division Popup 창 닫기
        function fn_ClientCloseEng(oWnd, args) {
            var item = args.get_argument();
            if (item != null) {
                var txtdiv = $find("<%= radtxtEnOrg.ClientID%>");
                txtdiv.set_value(item.CODE_NAME + " (" + item.SUB_CODE + ")");
                $('#<%=hddEngDivisionCode.ClientID%>').val(item.SUB_CODE);
            }
        }

        //Korean Division Popup 창 닫기
        function fn_ClientCloseKor(oWnd, args) {
            var item = args.get_argument();
            if (item != null) {
                var txtdiv = $find("<%= radtxtKoOrg.ClientID%>");
                    txtdiv.set_value(item.CODE_NAME + " (" + item.SUB_CODE + ")");
                    $('#<%=hddKorDivisionCode.ClientID%>').val(item.SUB_CODE);
                }
            }

            function keyPress(sender, args) {
                var text = sender.get_value() + args.get_keyCharacter();
                
                if (!text.match('^[0-9-\x20]+$'))
                    args.set_cancel(true);
            }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <!-- doc Style -->
    <div class="doc_style">
        <h3>The Information</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col style="width: 35%;" />
                    <col />
                </colgroup>
                <thead>
                    <tr>
                        <th></th>
                        <th>English</th>
                        <th>korean</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <th>Name <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadTextBox ID="radtxtEnName" runat="server" Width="100%"></telerik:RadTextBox></td>
                        <td>
                            <telerik:RadTextBox ID="radtxtKoName" runat="server" Width="100%"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <th>Name Color <span class="text_red">*</span></th>
                        <td colspan="2">
                            <telerik:RadDropDownList ID="RadDropDownColor" runat="server" Width="200px">
                                <Items>
                                    <telerik:DropDownListItem CssClass="font-midpurple" Text="Mid Purple" />      
                                    <telerik:DropDownListItem  CssClass="font-raspberry" Text="Raspberry" />
                                    <telerik:DropDownListItem  CssClass="font-blue" Text="Blue" />
                                    <telerik:DropDownListItem CssClass="font-green" Text="Green" />
                              
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                        
                    </tr>
                    <tr>
                        <th>Title <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadTextBox ID="radtxtTitle" runat="server" Width="100%"></telerik:RadTextBox></td>
                        <td>
                            <telerik:RadDropDownList ID="radDropKorTitle" runat="server" DataTextField="CODE_NAME" DataValueField="SUB_CODE"></telerik:RadDropDownList>
                            <telerik:RadTextBox ID="radtxtKOR_JOB_TITLE" runat="server" Width="170px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Organization <span class="text_red">*</span></th>
                        <td>
                            <div style="margin-bottom: 3px;">
                                <span style="display: inline-block; width: 65px; text-align: right">Division <span class="text_red">*</span></span>
                                <telerik:RadTextBox ID="radtxtEnOrg" runat="server" Width="200px"></telerik:RadTextBox>
                                <telerik:RadButton ID="radBtnEnDivision" runat="server" AutoPostBack="false" OnClientClicked='fn_OpenEngDivision'
                                    Width="18px" Height="18px" CssClass="btn_grid">
                                    <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                                </telerik:RadButton>
                            </div>
                            <span style="display: inline-block; width: 65px; text-align: right">Department</span>
                            <telerik:RadTextBox ID="radtxtEnDepartment" runat="server" Width="200px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <div style="margin-bottom: 3px;">
                                <span style="display: inline-block; width: 65px; text-align: right">Division <span class="text_red">*</span></span>
                                <telerik:RadTextBox ID="radtxtKoOrg" runat="server" Width="200px"></telerik:RadTextBox>
                                <telerik:RadButton ID="radBtnKoDivision" runat="server" AutoPostBack="false" OnClientClicked='fn_OpenKorDivision'
                                    Width="18px" Height="18px" CssClass="btn_grid">
                                    <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                                </telerik:RadButton>
                            </div>
                            <span style="display: inline-block; width: 65px; text-align: right">Department</span>
                            <telerik:RadTextBox ID="radtxtKoDepartment" runat="server" Width="200px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <%--2022.04.15 comment out below line, INC15142408--%>
<%--                        <th>Tel.of office <span class="text_red">*</span></th>--%>
                        <%--2022.04.15 add below line, INC15142408--%>
                        <th>Tel.of office <span class="text_red"></span></th>
                        <td colspan="2">
                            <telerik:RadDropDownList ID="radDropTelCode" runat="server" Width="50px">
                                <Items>
                                    <telerik:DropDownListItem Text="02" />
                                    <telerik:DropDownListItem Text="070" />
                                    <telerik:DropDownListItem Text="031" />
                                    <telerik:DropDownListItem Text="032" />
                                    <telerik:DropDownListItem Text="033" />
                                    <telerik:DropDownListItem Text="041" />
                                    <telerik:DropDownListItem Text="042" />
                                    <telerik:DropDownListItem Text="043" />
                                    <telerik:DropDownListItem Text="044" />
                                    <telerik:DropDownListItem Text="051" />
                                    <telerik:DropDownListItem Text="052" />
                                    <telerik:DropDownListItem Text="053" />
                                    <telerik:DropDownListItem Text="054" />
                                    <telerik:DropDownListItem Text="055" />
                                    <telerik:DropDownListItem Text="061" />
                                    <telerik:DropDownListItem Text="062" />
                                    <telerik:DropDownListItem Text="063" />
                                    <telerik:DropDownListItem Text="064" />
                                </Items>
                            </telerik:RadDropDownList>
                            <telerik:RadTextBox ID="radTxtTelNum" runat="server" Width="200px">
                                <ClientEvents OnKeyPress="keyPress" />
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Mobile phone</th>
                        <td colspan="2">
                            <telerik:RadDropDownList ID="radDropMobileCode" runat="server" Width="50px">
                                <Items>
                                    <telerik:DropDownListItem Text="010" />
                                    <telerik:DropDownListItem Text="011" />
                                    <telerik:DropDownListItem Text="016" />
                                    <telerik:DropDownListItem Text="017" />
                                    <telerik:DropDownListItem Text="018" />
                                    <telerik:DropDownListItem Text="019" />
                                </Items>
                            </telerik:RadDropDownList>
                            <telerik:RadTextBox ID="radTxtMobileNum" runat="server" Width="200px">
                                <ClientEvents OnKeyPress="keyPress" />
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Fax number</th>
                        <td colspan="2">
                            <telerik:RadDropDownList ID="radDropFaxCode" runat="server" Width="50px">
                                <Items>
                                    <telerik:DropDownListItem Text="02" />
                                    <telerik:DropDownListItem Text="070" />
                                    <telerik:DropDownListItem Text="031" />
                                    <telerik:DropDownListItem Text="032" />
                                    <telerik:DropDownListItem Text="033" />
                                    <telerik:DropDownListItem Text="041" />
                                    <telerik:DropDownListItem Text="042" />
                                    <telerik:DropDownListItem Text="043" />
                                    <telerik:DropDownListItem Text="044" />
                                    <telerik:DropDownListItem Text="051" />
                                    <telerik:DropDownListItem Text="052" />
                                    <telerik:DropDownListItem Text="053" />
                                    <telerik:DropDownListItem Text="054" />
                                    <telerik:DropDownListItem Text="055" />
                                    <telerik:DropDownListItem Text="061" />
                                    <telerik:DropDownListItem Text="062" />
                                    <telerik:DropDownListItem Text="063" />
                                    <telerik:DropDownListItem Text="064" />
                                </Items>
                            </telerik:RadDropDownList>
                            <telerik:RadTextBox ID="radTxtFaxNum" runat="server" Width="200px">
                                <ClientEvents OnKeyPress="keyPress" />
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>E-mail address <span class="text_red">*</span></th>
                        <td colspan="2">
                            <telerik:RadTextBox ID="radtxtEmailAddress" runat="server" Width="200px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Address & Zip <span class="text_red">*</span></th>
                        <td colspan="2">
                            <telerik:RadDropDownList ID="radDropAddZip" runat="server" DataTextField="BUSINESS_PLACE_NAME" DataValueField="BUSINESS_PLACE_ID"></telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Quantity <span class="text_red">*</span></th>
                        <td colspan="2">
                            <telerik:RadNumericTextBox ID="radtxtQuantity" runat="server" Width="50px" NumberFormat-DecimalDigits="0" Value="2"></telerik:RadNumericTextBox>
                            <%--<telerik:RadTextBox ID="radtxtQuantity" runat="server" Width="50px" Text="2">
                                <ClientEvents OnKeyPress="keyPress" />
                            </telerik:RadTextBox>--%>
                            Box
                        </td>
                    </tr>
                    <%-- %><tr>
                        <th>
                            Logo
                        </th>
                        <td colspan="2">
                            <asp:Label ID="radlbLogo" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
                </tbody>
            </table>
        </div>
<%--                    <% --2022.04.15 comment out below, INC15142408-- %> comment out, end--%>
<%--        <div class="doc_style2 bold">
            Note : Information to be included in the business card are only business relevant information.
                <br />
            <span style="padding-left: 35px;">Thus please neither enter private phone numbers nor private e-mail addresses.</span>
        </div>--%>
        <br />
        <telerik:RadButton ID="radBtnDisplayNameCard" runat="server" Text="Display Name card" OnClientClicked="fn_OnDisplayNameCard" AutoPostBack="false"
            EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-blue btn-size3 bold">
        </telerik:RadButton>
    </div>

    <%--Popup Division--%>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinPopupEngDivision" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Division" Width="380px" Height="630px" Behaviors="Default" OnClientClose="fn_ClientCloseEng" NavigateUrl="./PopupEngDivision.aspx"></telerik:RadWindow>
            <telerik:RadWindow ID="radWinPopupKorDivision" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Division" Width="380px" Height="630px" Behaviors="Default" OnClientClose="fn_ClientCloseKor" NavigateUrl="./PopupEngDivision.aspx"></telerik:RadWindow>
            <telerik:RadWindow ID="radWinPopupDisplayNameCard" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="DisplayNameCard" Width="500px" Height="640px" Behaviors="Default" CssClass="windowscroll" NavigateUrl="/eWorks/Approval/Link/BusinessCardView.aspx"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div id="hiddenArea" runat="server">
        <input type="hidden" id="hddProcessID" runat="server" />
        <input type="hidden" id="hddProcessStatus" runat="server" />
        <input type="hidden" id="hddDocumentID" runat="server" />
        <input type="hidden" id="hddReuse" runat="server" />
        <input type="hidden" id="hddAddRow" runat="server" value="N" />

        <input type="hidden" id="hddEngDivisionCode" runat="server" />
        <input type="hidden" id="hddKorDivisionCode" runat="server" />
    </div>
</asp:Content>

