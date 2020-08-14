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
                $('#title').text('Secondary Seal')
            } else if (value == 'Corporate') {
                $('#rowSealHolder').hide();
                $('#title').text('Corporate Seal')
            }
        }

        //Docusign seal
        $("document").ready(function () {
            var actionTaker = $(".action-taker-wrapper").attr("userid");
            var currentUser = $("#HolderDocumentBody_UserID").val();
            console.log(actionTaker, currentUser)
            //if (actionTaker == currentUser) { }
            $('.attachment-wrapper a[href^="javascript:fn_FileDownload"]').each(function (i, e) {
                var documentId = $(e).attr("href").match(/javascript\:fn\_FileDownload\((\d*)\)/)[1];
                if (documentId) $(e).parent().append('<button type="button" style="float:right;border-radius:2px;background-color:#e7e7e7;padding:2px 5px;" onclick="window.open(\'/eWorks/Approval/Docusign/Login.aspx?documentId=' + documentId + '\',\'_blank\')">Seal</button>')
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
                            <telerik:RadButton ID="RadElectronic" runat="server" Text="전자도장" Value="Electronic" GroupName="SIGN_TYPE"  ButtonType="ToggleButton" ToggleType="Radio" Enabled="false" ></telerik:RadButton>
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

                                        <telerik:DropDownListItem Text="BCS-JungBu Branch" Value="BCS-JungBu Branch" />   <%--  기존 JungBu --%>
                                        <telerik:DropDownListItem Text="BCS-ChungCheong Branch" Value="BCS-ChungCheong Branch" />   <%--  기존 ChungCheong --%>
                                        <telerik:DropDownListItem Text="BCS-HoNam Branch" Value="BCS-HoNam Branch" />   <%--  기존 HoNam --%>
                                        <telerik:DropDownListItem Text="BCS-KyungBuk Branch" Value="BCS-KyungBuk Branch" />   <%--  기존 KyungBuk --%>
                                        <telerik:DropDownListItem Text="BCS-KyungNam Branch" Value="BCS-KyungNam Branch" />     <%--  기존 KyungNam --%>
                                        <telerik:DropDownListItem Text="BCS-Jeju Branch" Value="BCS-Jeju Branch" />     <%--  2020년에 새롭게 추가됨--%>

                                        <telerik:DropDownListItem Text="BCS-CFO Functions" Value="BCS-CFO Functions" /> <%--  NEW --%>
                                        <telerik:DropDownListItem Text="BCS-O2C/WSO/CM" Value="BCS-O2C/WSO/CM" /> <%--  NEW --%>
                                        <telerik:DropDownListItem Text="BCS-ACCP" Value="BCS-ACCP" /> <%--  NEW --%>
                                        <telerik:DropDownListItem Text="BCS-PRO" Value="BCS-PRO" /> <%--  NEW --%>


                                        <telerik:DropDownListItem Text="BKL-CFO Functions" Value="BKL-CFO Functions" />  <%--  기존 CFO --%>
                                        <telerik:DropDownListItem Text="BKL-CPL-CFO-O2C/WSO/CM" Value="BKL-CPL-CFO-O2C/WSO/CM" />  <%--  NEW --%>
                                        <telerik:DropDownListItem Text="BKL-CPL-CFO-ACCP" Value="BKL-CPL-CFO-ACCP" />  <%--  NEW --%>
                                        <telerik:DropDownListItem Text="BKL-CPL-CFO-PRO" Value="BKL-CPL-CFO-PRO" />  <%--  NEW --%>
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

                                        <telerik:DropDownListItem Text="MKR-Finance" Value="MKR-Finance" />
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
                            <telerik:RadTextBox runat="server" ID="RadtxtPurpose" Width="98%" TextMode="MultiLine" Height="80" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                </tbody>
             </table>
          </div>
    </div>
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
	<input type="hidden" id="hddAddRow" runat="server" value="N" />
    <input type="hidden" id="UserID" runat="server" value="" />
</asp:Content>

