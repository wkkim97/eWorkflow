<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="SecondarySeal.aspx.cs" Inherits="Approval_Document_SecondarySeal" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        //버튼이벤트
        function fn_OnSealChanged(sender, args) {
            if (args.get_checked())
                setSeal(sender.get_value());
        }

        //Seal Type 선택시
        function setSeal(value) {
            if (value == 'Secondary') {
                $('#rowSealHolder').show();
                $('#title').text('Secondary Seal');
                $('#rowNoteforCorporate').hide();
            } else if (value == 'Corporate') {
                $('#rowSealHolder').hide();
                $('#rowNoteforCorporate').show();
                $('#title').text('Corporate Seal')
            }
        }

        //Docusign seal
        $("document").ready(function () {
            var actionTaker = $(".action-taker-wrapper").attr("userid");
            var currentUser = $("#HolderDocumentBody_UserID").val();
            console.log(actionTaker, currentUser);
            if ($("#ApproveMenuBar_hspanDocumentNo").html().length <= 0) return;
            //if (actionTaker == currentUser) { }
            $('.attachment-wrapper a[href^="javascript:fn_FileDownload"]').each(function (i, e) {
                var documentId = $(e).attr("href").match(/javascript\:fn\_FileDownload\((\d*)\)/)[1];
                console.log("333");
                var documentNo = $("#ApproveMenuBar_hspanDocumentNo").html() || "";
                console.log(documentNo);
                if (documentId) $(e).parent().append('<button type="button" style="float:right;border-radius:2px;background-color:#e7e7e7;padding:2px 5px;" onclick="window.open(\'/eWorks/Approval/Docusign/Login.aspx?documentId=' + documentId + '&documentNo=' + documentNo+ '\',\'_blank\')">Seal</button>')
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%" />
                    <col />
                </colgroup>
                <tr>
                    <th>Seal Type
                    </th>
                    <td>
                        <div id="divSeal" runat="server" style="margin: 0 0 0 0">
                            <telerik:RadButton ID="radBtnSecondary" runat="server" Text="Secondary Seal" Value="Secondary" AutoPostBack="false"
                                ButtonType="ToggleButton" ToggleType="Radio" GroupName="Seal" OnClientCheckedChanged="fn_OnSealChanged">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnCorporate" runat="server" Text="Corporate Seal" Value="Corporate" AutoPostBack="false"
                                ButtonType="ToggleButton" ToggleType="Radio" GroupName="Seal" OnClientCheckedChanged="fn_OnSealChanged">
                            </telerik:RadButton>
                        </div>
                        <div id="rowNoteforCorporate" style="margin-left:10px;display:none">
                            * 법인 인감의 경우 전자도장은 존재 하지 않습니다. 실제 도장을 선택하여 주세요.<br />&nbsp;&nbsp; 전자 도장을 선택하셔도 실제 도장으로 진행됩니다.
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <h3 id="title"></h3>
        <!--<h3>Please select a Sealing of Secondary Seal</h3>-->
        <div class="data_type1" id="divSealHolder">
        <table>
            <colgroup>
                <col />
                <col style="width: 75%;" />
            </colgroup>
                <tbody>
                    <tr>
                        <th>Company <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadButton ID="radBKL" runat="server" Text="BKL" Value="BKL" GroupName="COMPANY_SEAL"  ButtonType="ToggleButton" ToggleType="Radio" OnClick="RadCompany_Click"></telerik:RadButton>
                            <telerik:RadButton ID="radBCS" runat="server" Text="BCS" Value="BCS" GroupName="COMPANY_SEAL"  ButtonType="ToggleButton" ToggleType="Radio" OnClick="RadCompany_Click"></telerik:RadButton>
                            <telerik:RadButton ID="radMKR" runat="server" Text="MKR" Value="MKR" GroupName="COMPANY_SEAL"  ButtonType="ToggleButton" ToggleType="Radio" OnClick="RadCompany_Click"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>날인Type <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadButton ID="RadPaper" runat="server" Text="실제도장" Value="Paper" GroupName="SIGN_TYPE"  ButtonType="ToggleButton" ToggleType="Radio" ></telerik:RadButton>
                            <telerik:RadButton ID="RadElectronic" runat="server" Text="전자도장" Value="Electronic" GroupName="SIGN_TYPE"  ButtonType="ToggleButton" ToggleType="Radio" Enabled="true" ></telerik:RadButton>

                        </td>
                    </tr>
                    <tr id="rowSealHolder" >
                        <th>Seal holder <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadDropDownList ID="radDropSeal" runat="server" Width="300px" DropDownHeight="300px" DefaultMessage="--- Select ---" AutoPostBack="false">
                                    <Items>
                                        <telerik:DropDownListItem Text="BCS-Management" Value="BCS-Management" />          <%--  기존 HQ --%>
                                        <telerik:DropDownListItem Text="BCS-Human Resource" Value="BCS-Human Resource" />  <%--  기존 HR --%>
                                        <telerik:DropDownListItem Text="BCS-Procurement" Value="BCS-Procurement" />  <%--  2020년에 새롭게 추가됨. --%>
                                        <telerik:DropDownListItem Text="BCS-Development-Pyeongtaek" Value="BCS-Development-Pyeongtaek" /> <%--  기존 DEV_P --%>
                                        <telerik:DropDownListItem Text="BCS-PS-Daejeon" Value="BCS-PS-Daejeon" /> <%--  기존 PS_DJ --%>
                                        <telerik:DropDownListItem Text="BCS-Industrial Sales" Value="BCS-Industrial Sales" /> <%--  기존 PS_DJ --%>

                                        <telerik:DropDownListItem Text="BCS-JungBu Branch" Value="BCS-JungBu Branch" />   <%--  기존 JungBu --%>
                                        <telerik:DropDownListItem Text="BCS-ChungCheong Branch" Value="BCS-ChungCheong Branch" />   <%--  기존 ChungCheong --%>
                                        <telerik:DropDownListItem Text="BCS-HoNam Branch" Value="BCS-HoNam Branch" />   <%--  기존 HoNam --%>
                                        <telerik:DropDownListItem Text="BCS-KyungBuk Branch" Value="BCS-KyungBuk Branch" />   <%--  기존 KyungBuk --%>
                                        <telerik:DropDownListItem Text="BCS-KyungNam Branch" Value="BCS-KyungNam Branch" />     <%--  기존 KyungNam --%>
                                        <telerik:DropDownListItem Text="BCS-Jeju Branch" Value="BCS-Jeju Branch" />     <%--  2020년에 새롭게 추가됨--%>

                                        <telerik:DropDownListItem Text="BCS-CFO" Value="BCS-CFO" /> <%--  NEW --%>
                                        <telerik:DropDownListItem Text="BCS-O2C/WSO/CM" Value="BCS-O2C/WSO/CM" /> <%--  NEW --%>
                                        <telerik:DropDownListItem Text="BCS-GFI" Value="BCS-GFI" /> <%--  NEW --%>
                                        <telerik:DropDownListItem Text="BCS-CPL-CFO-CA" Value="BCS-CPL-CFO-CA" /> <%--  NEW --%>


                                        <telerik:DropDownListItem Text="BKL-CFO" Value="BKL-CFO" />  <%--  기존 CFO --%>
                                        <telerik:DropDownListItem Text="BKL-CPL-CFO-O2C/WSO/CM" Value="BKL-CPL-CFO-O2C/WSO/CM" />  <%--  NEW --%>
                                        <telerik:DropDownListItem Text="BKL-CPL-CFO-GFI" Value="BKL-CPL-CFO-GFI" />  <%--  NEW --%>
                                        <telerik:DropDownListItem Text="BKL-CPL-CFO-CA" Value="BKL-CPL-CFO-CA" />  <%--  NEW --%>
                                        <telerik:DropDownListItem Text="BKL-CPL-CFO-QO" Value="BKL-CPL-CFO-QO" />    <%-- BKL-Production(Ansung) 에서 이름변경--%>
                                        <telerik:DropDownListItem Text="BKL-Human Resources" Value="BKL-Human Resources" />     <%--  기존 BKL-HR --%>
                                        <telerik:DropDownListItem Text="BKL-Law Patent & Compliance" Value="BKL-Law Patent & Compliance" /> <%-- 2020년에 새롭게 추가됨. --%>
                                        <telerik:DropDownListItem Text="BKL-Heart Health" Value="BKL-Heart Health" />
                                        <telerik:DropDownListItem Text="BKL-Specialty Medicine" Value="BKL-Specialty Medicine" />
                                        <telerik:DropDownListItem Text="BKL-Womens Healthcare" Value="BKL-Womens Healthcare" />
                                        <telerik:DropDownListItem Text="BKL-Medical" Value="BKL-Medical" />  <%--  기존 MEDICAL --%>
                                        <telerik:DropDownListItem Text="BKL-Market Access" Value="BKL-Market Access" />  <%--  NEW --%>
                                        <telerik:DropDownListItem Text="BKL-Commercial Excellence & Digital" Value="BKL-Commercial Excellence & Digital" />  <%--  NEW --%>
                                       


                                        <telerik:DropDownListItem Text="BKL-Regulatory Affairs" Value="BKL-Regulatory Affairs" />
                                        <telerik:DropDownListItem Text="BKL-Site Management" Value="BKL-Site Management" />
                                        <telerik:DropDownListItem Text="BKL-Radiology" Value="BKL-Radiology" />
                                        <telerik:DropDownListItem Text="BKL-Consumer Health" Value="BKL-Consumer Health" />

                                        <telerik:DropDownListItem Text="MKR-GFI" Value="MKR-GFI" />
                                        <telerik:DropDownListItem Text="MKR-R&D" Value="MKR-R&D" />
                                        <telerik:DropDownListItem Text="MKR-Seed&Trait Regulatory Science" Value="MKR-Seed&Trait Regulatory Science" />
                                        <telerik:DropDownListItem Text="MKR-HR" Value="MKR-HR" />

                                        <telerik:DropDownListItem Text="BCS-HQ" Value="HQ" Visible="false" />
                                        <telerik:DropDownListItem Text="BCS-HQ-SMM" Value="HQS" Visible="false" />
                                        <telerik:DropDownListItem Text="BCS-Development-Seoul" Value="DEV_S" Visible="false" />
                                        <telerik:DropDownListItem Text="BCS-BVS" Value="BVS" Visible="false" />
                                        <telerik:DropDownListItem Text="BCS-HR" Value="HR" Visible="false" />

                                        
                                        <telerik:DropDownListItem Text="Kyunggi branch" Value="KG_B" Visible="false" />
                                        <telerik:DropDownListItem Text="Gangwon branch" Value="GW_B" Visible="false" />
                                        <telerik:DropDownListItem Text="Chungbuk branch" Value="CB_B" Visible="false" />
                                        <telerik:DropDownListItem Text="Chungnam branch" Value="CG_B" Visible="false" />
                                        <telerik:DropDownListItem Text="Junbuk branch" Value="JB_B" Visible="false" />
                                        <telerik:DropDownListItem Text="Junnam branch" Value="JN_B" Visible="false" />
                                        <telerik:DropDownListItem Text="Daegu branch" Value="DG_B" Visible="false" />
                                        <telerik:DropDownListItem Text="Kyungnam branch" Value="KN_B" Visible="false" />
                                        <telerik:DropDownListItem Text="Andong branch" Value="AD_B" Visible="false" />
                                        <telerik:DropDownListItem Text="Jeju branch" Value="JJ_B" Visible="false" />
                                        
                                        
                                    </Items>
                           </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Recipient(제출처) <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtRecipient" Width="98%" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Purpose & Details <span class="text_red">*</span></th>
                        <td>
                            <p style="padding:1%;border:1px solid #ff0000;width:95.5%;margin-bottom:5px;background-color:#fcd2d2"><b>계약서에 날인하시는 경우</b>,<br /> 사전에 ConRADS를 통해 계약서 검토/승인을 득하신 후 ConRADS 승인번호를 함께 기입해주시기 바랍니다</p>
                            <telerik:RadTextBox runat="server" ID="RadtxtPurpose" Width="98%" TextMode="MultiLine" Height="80" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                </tbody>
             </table>
          </div>
<%--2022.01.29, add for change request, start--%>
<%--	문서에 개인정보가 포함될 경우, 개인정보활용동의서를 득한 후 진행하여야 합니다. 특히 주민등록번호가 모두 노출되는 경우, 반드시 법에 의해 요구되는 내용인지 확인하시기 바라며, 기입이 필요한 경우 기안서(날인전/날인후)에는 주민등록번호 뒷자리는 삭제하여 첨부하시기 바랍니다.
	인감날인이 완료된 문서는 반드시 “input comment”를 통해 사후 업로드를 완료 하시기 바랍니다.--%>
    <p class=MsoListParagraph style='margin-left:38.0pt;mso-para-margin-left:0gd;
    text-indent:-.14in;mso-list:l0 level1 lfo2'><![if !supportLists]><span    style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
    Wingdings;color:#C00000;mso-bidi-font-weight:bold'><span style='mso-list:Ignore'>l<span
    style='font:7.0pt "Times New Roman"'>&nbsp; </span></span></span><![endif]>
    <b><span lang=KO style='color:#C00000'>문서에 개인정보가 포함될 경우, 개인정보활용동의서를 득한 후 진행하여야 합니다. 특히 주민등록번호가 모두 노출되어 있는 경우에는 반드시 주민등록번호 뒷자리를 삭제하여 첨부하시기 바랍니다.</span></b></br>
    </p>
    
    <p class=MsoListParagraph style='margin-left:38.0pt;mso-para-margin-left:0gd;
    text-indent:-.14in;mso-list:l0 level1 lfo2'><![if !supportLists]><span
    style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
    Wingdings;color:#C00000;mso-bidi-font-weight:bold'><span style='mso-list:Ignore'>l<span
    style='font:7.0pt "Times New Roman"'>&nbsp; </span></span></span><![endif]><b><span
    lang=KO style='color:#C00000'>인감날인이 완료된 문서는 반드시 “input comment”를 통해 사후 업로드를 완료 하시기 바랍니다.</span></b></p>
<%--2022.01.29, add for change, end--%>
    </div>
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
	<input type="hidden" id="hddAddRow" runat="server" value="N" />
    <input type="hidden" id="UserID" runat="server" value="" />
</asp:Content>

