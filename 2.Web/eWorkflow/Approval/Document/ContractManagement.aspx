<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="ContractManagement.aspx.cs" Inherits="Approval_Document_ContractManagement" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
<script type="text/javascript">

    function pageLoad() {

        //alert("START");
        $('#divPrivacyInformation').hide();
        $('#divPIPA').hide();
        $('#divContract').show();

        var value = '';
        if ($find('<%= radRdoPrivacyInformationYes.ClientID %>').get_checked())
            value = $find('<%= radRdoPrivacyInformationYes.ClientID %>').get_value();
        else if ($find('<%= radRdoPrivacyInformationNo.ClientID %>').get_checked())
            value = $find('<%= radRdoPrivacyInformationNo.ClientID %>').get_value();

        var v_pipa = '';
        if ($find('<%= radRdoPIPA.ClientID %>').get_checked())
            v_pipa = $find('<%= radRdoPIPA.ClientID %>').get_value();

        if (value != '')
        {
            $('#divPrivacyInformation').show();
        }
        console.log(v_pipa);
        if (v_pipa != '')
        {
            $('#divPIPA').show();
            $('#divContract').hide();
        }
    }
    function fn_selectType() {
        if ($find('<%= radRdoPIPA.ClientID %>').get_checked()) {
            $('#divPIPA').show();
            $('#divContract').hide();
            
        }
        else {
            $('#divPIPA').hide();
            $('#divContract').show();

        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <!-- doc Style -->
    <div class="doc_style">
        <div class="data_type1">
        <h3>ConRADS</h3>
            <table>
                <colgroup>
                    <col style="width: 27%;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Contract Type<br />(표준계약서 양식 사용유무) <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadButton runat="server" ID="radRdoStandard" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Standard" Value="Standard" GroupName="ContractType" AutoPostBack="false" OnClientClicked="fn_selectType"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoNonStandard" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Non-Standard" Value="Non" GroupName="ContractType" AutoPostBack="false" OnClientClicked="fn_selectType"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoD2D" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="D2D" Value="D2D" GroupName="ContractType" AutoPostBack="false" OnClientClicked="fn_selectType"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoPIPA" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="PIPA" Value="PIPA" GroupName="ContractType" AutoPostBack="false" OnClientClicked="fn_selectType" ></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Essential Contract<br />(중요계약-150억 이상) <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadButton runat="server" ID="radRdoEssYes" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Yes" Value="Y" GroupName="Essiontial" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoEssNo" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="No" Value="N" GroupName="Essiontial" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Intragroup<br />(바이엘 계열사간 계약) <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadButton runat="server" ID="radRdoIntraYes" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Yes" Value="Y" GroupName="Intragroup" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoIntraNo" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="No" Value="N" GroupName="Intragroup" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Cross-Border<br />(국외계약) <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadButton runat="server" ID="radRdoCrossYes" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Yes" Value="Y" GroupName="Cross" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoCrossNo" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="No" Value="N" GroupName="Cross" AutoPostBack="false"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <th>Contract Category <span class="text_red">*</span></th>
                        <td colspan="3">
                            <telerik:RadButton runat="server" ID="radRdoCategory6" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Wholesale/Distribution/Sales Transaction" Value="WDS" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoCategory7" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Promotion" Value="PRO" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoCategory8" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Service" Value="SER" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoCategory1" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Purchase Contract" Value="PO" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoCategory2" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="TradeMark/IP-related (특허 / 상품 관련)" Value="IP" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoCategory3" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Clinical trial related (임상시험 관련)" Value="CTR" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoCategory5" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Alliance contract" Value="Alliance" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            <telerik:RadButton runat="server" ID="radRdoCategory9" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Data Privacy" Value="DP" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            <!--INC0004937695-->
                            <telerik:RadButton runat="server" ID="radRdoCategory10" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="IT Service" Value="IT_Service" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            <!--INC0004937695-->
                            <telerik:RadButton runat="server" ID="radRdoCategory4" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Others (그 외)" Value="Others" GroupName="ContractCategory" AutoPostBack="false"></telerik:RadButton>
                            
                         </td>
                    </tr>

                </tbody>
            </table>
        </div>
        <div id="divPrivacyInformation" class="data_type1" style="display:none">
            <table>
                <colgroup>
                    <col style="width: 27%" />
                    <col />
                </colgroup>
                <tbody>
                <tr>
                    <th>Contracts related privacy information
                        (개인정보 관련된 계약인 경우) </th>
                    <td>
                        <telerik:RadButton runat="server" ID="radRdoPrivacyInformationYes" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="Yes" Value="Y" GroupName="PrivacyInformatio" AutoPostBack="false"></telerik:RadButton>
                        <telerik:RadButton runat="server" ID="radRdoPrivacyInformationNo" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="No" Value="N" GroupName="PrivacyInformatio" AutoPostBack="false"></telerik:RadButton>
                    </td>
                </tr>
                 </tbody>              
            </table>
        </div>
        <div id="divPIPA" class="data_type1" style="display:none"  >
            <table>
                <colgroup>
                    <col style="width: 27%" />
                    <col />
                </colgroup>
                <tbody>
                <tr>
                    <th>Purpose of DP Request<br />(DP 기안목적)</th>
                    <td>
                        <telerik:RadButton runat="server" ID="radRdoPIPAPurpose1" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="개인정보활용동의서검토" Value="개인정보활용동의서검토" GroupName="PIPAPurpose" AutoPostBack="false"></telerik:RadButton>
                        <telerik:RadButton runat="server" ID="radRdoPIPAPurpose2" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="연간 개인정보보유현황 보고 Privacy Data Master(PriDAM)" Value="연간 개인정보보유현황 보고 Privacy Data Master(PriDAM)" GroupName="PIPAPurpose" AutoPostBack="false"></telerik:RadButton>
                        <telerik:RadButton runat="server" ID="radRdoPIPAPurpose3" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Text="개인정보 이전관련 법률자문 PDLT Request" Value="개인정보 이전관련 법률자문 PDLT Request" GroupName="PIPAPurpose" AutoPostBack="false"></telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <th>Event/Program Title<br />(행사명 혹은 프로그램명)</th>
                    <td>
                        <telerik:RadTextBox runat="server" ID="radTextPIPAEvent" Width="90%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Contract (관련 계약 여부) Number</th>
                    <td>
                        * 관련 계약서가 있다면 CLMS, Con-RADS 번호를 기입하거나, 계약서를 첨부해주시기 바랍니다.<br />
                        <telerik:RadTextBox runat="server" ID="radTextPIPAContract" Width="90%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Purpose of PI collection<br /> (수집목적)</th>
                    <td>
                        <div id="divPIPAPI" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radChkPIPAPI1" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="거래처 혹은 협력사 등록" Value="거래처 혹은 협력사 등록">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkPIPAPI2" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="신용평가" Value="신용평가">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkPIPAPI3" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="판촉행사" Value="판촉행사">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkPIPAPI4" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="임상시험" Value="임상시험">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkPIPAPI5" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="환자지원프로그램" Value="환자지원프로그램">
                            </telerik:RadButton>
                        </div>
                        기타 : <telerik:RadTextBox runat="server" ID="radTextPIPAPI6" Width="90%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Target<br /> (수집대상)</th>
                    <td>
                        <div id="divPIPATarget" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radChkPIPATarget1" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="일반인" Value="일반인">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkPIPATarget2" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="보건의료전문가" Value="보건의료전문가">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkPIPATarget3" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="환자" Value="환자">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkPIPATarget4" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="바이엘 직원" Value="바이엘 직원">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkPIPATarget5" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="14세 미만 포함여부" Value="14세 미만 포함여부">
                            </telerik:RadButton>
                        </div>
                        
                    </td>
                </tr>
                <tr>
                    <th>Collection Category <br /> (수집항목)</th>
                    <td>
                        <div id="divPIPACollection" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radChkPIPACollection1" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="일반정보 (성명, 전화번호, 이메일, 주소, 성별, 나이 등)" Value="일반정보 (성명, 전화번호, 이메일, 주소, 성별, 나이 등)">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkPIPACollection2" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="민감정보(사상.신념, 노동조합.정당의 가입.탈퇴, 정치적 견해, 건강, 성생활 등에 관한 정보, 유전정보, 법죄경력자료 등)" Value="민감정보(사상.신념, 노동조합.정당의 가입.탈퇴, 정치적 견해, 건강, 성생활 등에 관한 정보, 유전정보, 법죄경력자료 등)">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkPIPACollection3" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="고유식별정보(주민등록번호, 여권번호, 운전면허번호, 외국인등록번호 등)" Value="고유식별정보(주민등록번호, 여권번호, 운전면허번호, 외국인등록번호 등)">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radChkPIPACollection4" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="마케팅 활용 동의 여부 포함" Value="마케팅 활용 동의 여부 포함">
                            </telerik:RadButton>                            
                        </div>
                        
                    </td>
                 </tr>
                 <tr>
                    <th>Data Archiving System  <br /> (수집정보 보관소)</th>
                    <td>
                        <div id="divPIPAArchiving" runat="server" style="width: 100%; margin: 0 0 0 0">
                            <telerik:RadButton ID="radCheckPIPAArchiving1" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="이메일" Value="이메일">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radCheckPIPAArchiving2" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="공유 폴더" Value="공유 폴더">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radCheckPIPAArchiving3" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="false"
                                Text="공용 네트워크 (Sharepoint, Teams 등)" Value="공용 네트워크 (Sharepoint, Teams 등)">
                            </telerik:RadButton>
                            
                        </div>
                        기타 : <telerik:RadTextBox runat="server" ID="radTextPIPAArchiving4" Width="90%"></telerik:RadTextBox>
                    </td>
                 </tr>
                 <tr>
                     <th>Access Right in Data Archiving System<br />(수집정보 보관소 접근 권한자)</th>
                     <td>
                         <telerik:RadTextBox runat="server" ID="radTextPIPAPermission" Width="90%"></telerik:RadTextBox>
                     </td>
                 </tr>
                 <tr>
                     <th>Estimated PI Volume of the Event/Program <br />(해당 행사 및 프로그램 관련 수집정보 총예상인수)</th>
                     <td>
                         <telerik:RadTextBox runat="server" ID="radTextPIPAVolume" Width="90%"></telerik:RadTextBox>
                     </td>
                 </tr>
                 <tr>
                     <th>Data Retention & Destruction Period  <br />(수집 정보 보유 및 파기 기간)</th>
                     <td>
                         예) 수집 목적을 달성시 즉시 파기 or 1년, 2년....
                         <telerik:RadTextBox runat="server" ID="radTextPIPARetention" Width="90%"></telerik:RadTextBox>
                     </td>
                 </tr>
                 <tr>
                    <th>Data Transfer to the 3rd Party <br />수집 정보의 외부 업체 공유 및 전달</th>
                    <td>
                        * 3rd Party 에 제공되면, 공유정보와 목적을 기술 하여 주세요
                        <telerik:RadTextBox runat="server" ID="radTextPIPA3RDPARTY" Width="90%"></telerik:RadTextBox>
                    </td>
                 </tr>
                 <tr>
                    <th>Data Transfer to the overseas  <br />수집 정보의 해외 이전 여부(서버 포함))</th>
                    <td>
                        * 해외 에 제공되면, 공유정보와 목적을 기술 하여 주세요
                        <telerik:RadTextBox runat="server" ID="radTextPIPAOversea" Width="90%"></telerik:RadTextBox>
                    </td>
                 </tr>
                 </tbody>              
            </table>

        </div>


        <div class="data_type1" id="divContract" style="display:none">
            <table>
                <colgroup>
                    <col style="width: 27%;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Title<br />(계약제목) <span class="text_red">*</span></th>
                        <td >
                            <telerik:RadTextBox runat="server" ID="radTxtTitle" Width="90%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Term<br />(계약기간) <span class="text_red">*</span></th>
                        <td >
                            <telerik:RadDatePicker ID="radDateFrom" runat="server" Width="100px" Calendar-ShowRowHeaders="false" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            </telerik:RadDatePicker>
                            ~
                            <telerik:RadDatePicker ID="radDateTo" runat="server" Width="100px" Calendar-ShowRowHeaders="false" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <th>Contract Partner<br />(계약상대방) <span class="text_red">*</span></th>
                        <td >
                            <telerik:RadTextBox runat="server" ID="radTxtContractPartner" Width="90%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Contract Value<br />(KRW)계약금액 <span class="text_red">*</span></th>
                        <td >
                            <telerik:RadNumericTextBox ID="radTxtContractValue" runat="server" Width="50%" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <!-- //doc Style -->
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="hddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

