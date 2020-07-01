<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="ItResource.aspx.cs" Inherits="Approval_Document_ItResource" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">

        function pageLoad() {

            var category = getCheckedCategory();
            if (category == 'Hardware') {
                var type = getHardwareType();
                var res = getHardwareResource();
                setHardware(type, res);
            } else if (category == 'Software') {
                var soft = getSoftware();
                setSoftware(soft);
            } else if (category == 'ITService') {
                var type = getITServiceType();
                setITService(type);
            }
        }

        function getCheckedCategory() {
            var value = '';
            if ($find('<%= radBtnHardware.ClientID%>').get_checked())
                value = $find('<%= radBtnHardware.ClientID%>').get_value();
            else if ($find('<%= radBtnSoftware.ClientID%>').get_checked())
                value = $find('<%= radBtnSoftware.ClientID%>').get_value();
            else if ($find('<%= radBtnITService.ClientID %>').get_checked())
                value = $find('<%= radBtnITService.ClientID%>').get_value();
            else if ($find('<%= radBtnBYOS.ClientID %>').get_checked())
                value = $find('<%= radBtnBYOS.ClientID%>').get_value();
    return value;
}

function getHardwareType() {
    var type = '';

    if ($find('<%= radBtnLoss.ClientID%>').get_checked())
                type = $find('<%= radBtnLoss.ClientID%>').get_value();
            else if ($find('<%= radBtnTrouble.ClientID %>').get_checked())
                type = $find('<%= radBtnTrouble.ClientID%>').get_value();
            else if ($find('<%= radBtnNew.ClientID %>').get_checked())
                type = $find('<%= radBtnNew.ClientID%>').get_value();
    return type;
}

function getHardwareResource() {
    var res = '';
    if ($find('<%= radBtnLoss.ClientID %>').get_checked() || $find('<%= radBtnTrouble.ClientID %>').get_checked()) {
                if ($find('<%= radBtnHardwareMobile.ClientID %>').get_checked())
                    res = $find('<%= radBtnHardwareMobile.ClientID %>').get_value();
                else if ($find('<%= radBtnHardwareComputer.ClientID %>').get_checked())
                    res = $find('<%= radBtnHardwareComputer.ClientID %>').get_value();
                else if ($find('<%= radBtnHardwareIpad.ClientID %>').get_checked())
                    res = $find('<%= radBtnHardwareIpad.ClientID %>').get_value();
    }
    else if ($find('<%= radBtnNew.ClientID %>').get_checked()) {
                if ($find('<%= radBtnHardwareComputer.ClientID %>').get_checked())
                    res = $find('<%= radBtnHardwareComputer.ClientID %>').get_value();
                else if ($find('<%= radBtnHardwareETC.ClientID %>').get_checked())
                    res = $find('<%= radBtnHardwareETC.ClientID %>').get_value();
        }
    return res;
}

function getITServiceType() {
    var type = '';
    if ($find('<%= radBtnSvcTypeEmail.ClientID%>').get_checked())
        type = $find('<%= radBtnSvcTypeEmail.ClientID %>').get_value();
    else if ($find('<%= radBtnSvcTypeIPT.ClientID%>').get_checked())
        type = $find('<%= radBtnSvcTypeIPT.ClientID %>').get_value();
    else if ($find('<%= radBtnSvcTypeMailBox.ClientID%>').get_checked())
        type = $find('<%= radBtnSvcTypeMailBox.ClientID %>').get_value();
    else if ($find('<%= radBtnSvcTypeMDM.ClientID%>').get_checked())
        type = $find('<%= radBtnSvcTypeMDM.ClientID %>').get_value();
    else if ($find('<%= radBtnSvcTypeRetal.ClientID%>').get_checked())
        type = $find('<%= radBtnSvcTypeRetal.ClientID %>').get_value();
    return type;
}

function getSoftware() {
    var soft = '';
    if ($find('<%= radBtnSoftHwp.ClientID%>').get_checked())
                soft = $find('<%= radBtnSoftHwp.ClientID %>').get_value();
            else if ($find('<%= radBtnSoftAcrobatPRO.ClientID %>').get_checked())
                soft = $find('<%= radBtnSoftAcrobatPRO.ClientID %>').get_value();
            else if ($find('<%= radBtnSoftAcrobatSTD.ClientID %>').get_checked())
                soft = $find('<%= radBtnSoftAcrobatSTD.ClientID %>').get_value();
            else if ($find('<%= radBtnSoftETC.ClientID %>').get_checked())
                soft = $find('<%= radBtnSoftETC.ClientID %>').get_value();
    return soft;
}

function setHardware(valType, valRes) {
    setCategory('Hardware');
    setHardwareType(valType, valRes);
}

function setSoftware(value) {
    setCategory('Software');
    setSoftwareInfo(value);
}

function setITService(value) {
    setCategory('ITService');
    setServiceInfo(value);
}
//Category선택시
function setCategory(value) {
    if (value == 'Hardware') {
        $('#divHardware').show();
        $('#divSoftware').hide();
        $('#divITService').hide();
        $('#title').text('Hardware')
    } else if (value == 'Software') {
        $('#divHardware').hide();
        $('#divSoftware').show();
        $('#divITService').hide();
        $('#title').text('Software')
    }
    else if (value == 'ITService') {
        $('#divHardware').hide();
        $('#divSoftware').hide();
        $('#divITService').show();
        $('#title').text('IT Service')
    }
    else if (value == 'BYOS') {
        $('#divHardware').hide();
        $('#divSoftware').hide();
        $('#divITService').hide();
        $('#divBYOS').show();
        $('#title').text('BYOS(Bring Your Own SmartPhone)')
    }
}

function setHardwareType(valType, valRes) {
    if (valType == 'New') {
        var btnMobile = $find('<%= radBtnHardwareMobile.ClientID %>');
                if (btnMobile) btnMobile.set_visible(false);
                var btnETC = $find('<%= radBtnHardwareETC.ClientID %>');
                if (btnETC) btnETC.set_visible(true);
                var btnComp = $find('<%= radBtnHardwareComputer.ClientID %>');
                if (btnComp) btnComp.set_visible(true);
                var btnIpad = $find('<%= radBtnHardwareIpad.ClientID %>');
                if (btnIpad) btnIpad.set_visible(false);
            }
            else {
                var btnMobile = $find('<%= radBtnHardwareMobile.ClientID %>')
                if (btnMobile) btnMobile.set_visible(true);
                var btnETC = $find('<%= radBtnHardwareETC.ClientID %>');
                if (btnETC) btnETC.set_visible(false);
                var btnComp = $find('<%= radBtnHardwareComputer.ClientID %>');
                if (btnComp) btnComp.set_visible(true);
                var btnIpad = $find('<%= radBtnHardwareIpad.ClientID %>');
                if (btnIpad) btnIpad.set_visible(true);
            }

            $("div[type='HardwareInfo']").hide();
            if (valType && valRes)
                $('#div' + valType + valRes).show();
        }
        //버튼이벤트
        function fn_OnCategoryChanged(sender, args) {
            if (args.get_checked())
                setCategory(sender.get_value());
        }

        function fn_OnHardwareTypeChanged(sender, args) {
            if (args.get_checked()) {
                var btnMobile = $find('<%= radBtnHardwareMobile.ClientID %>');
                if (btnMobile) btnMobile.set_checked(false);
                var btnComputer = $find('<%= radBtnHardwareComputer.ClientID %>');
                if (btnComputer) btnComputer.set_checked(false);
                var btnETC = $find('<%= radBtnHardwareETC.ClientID %>');
                if (btnETC) btnETC.set_checked(false);
                var btnIpad = $find('<%= radBtnHardwareIpad.ClientID %>');
                if (btnIpad) btnIpad.set_checked(false);
                setHardwareType(sender.get_value());
            }
        }

        function fn_OnHardwareResChanged(sender, args) {
            if (args.get_checked()) {
                var type;
                if ($find('<%= radBtnLoss.ClientID %>').get_checked()) type = $find('<%= radBtnLoss.ClientID %>').get_value();
                else if ($find('<%= radBtnTrouble.ClientID %>').get_checked()) type = $find('<%= radBtnTrouble.ClientID %>').get_value();
                else if ($find('<%= radBtnNew.ClientID %>').get_checked()) type = $find('<%= radBtnNew.ClientID %>').get_value();
            setHardwareType(type, sender.get_value());
        }
    }
    //Software
    function setSoftwareInfo(value) {
        $("div[type='SoftwareInfo']").hide();
        if (value == 'Hwp2010') {
            $('#divSoftInfoHwp2010').show();
        } else if (value == 'ETC') {
            $('#divSoftInfoETC').show();
        } else if (value == 'AcrobatSTD' || value == 'AcrobatPRO') {
            $('#divSoftInfoAcrobat').show();
        }

<%--            var documentNo = $("<%= HddProcessStatus.ClientID %>").val();
            alert(documentNo);
            var btnPeriod = $find('<%= radRdoPeriod.ClientID%>');
            if (btnPeriod) {
                if (btnPeriod.get_checked()) {
                    $find('<%= radDatSoftFrom.ClientID %>').set_enabled(true);
                    $find('<%= radDatSoftTo.ClientID %>').set_enabled(true);
                } else {
                    $find('<%= radDatSoftFrom.ClientID %>').set_enabled(false);
                    $find('<%= radDatSoftTo.ClientID %>').set_enabled(false);
                }
            }--%>
        }

        function fn_OnSoftwareChanged(sender, args) {
            if (args.get_checked()) {
                setSoftwareInfo(sender.get_value());
            }
        }

        //IT Service
        function setServiceInfo(value) {
            $("div[type='ServiceInfo']").hide();
            if (value)
                $('#divSvcInfo' + value).show();

            if (value == 'RentalPC' || value == 'Email')
                $('#rowServicePeriod').show();
            else
                $('#rowServicePeriod').hide();

            if (value == 'Email')
                $('#rowSvcAccountName').show();
            else
                $('#rowSvcAccountName').hide();
        }
        function fn_OnServiceChanged(sender, args) {
            if (args.get_checked()) {
                setServiceInfo(sender.get_value());
            }
        }

        function fn_OnPermanetCheckedChanged(sender, args) {
            if (args.get_checked()) {
                if (sender.get_value() == 'Y') {
                    $find('<%= radDatSoftFrom.ClientID %>').set_enabled(false);
                    $find('<%= radDatSoftTo.ClientID %>').set_enabled(false);
                } else {
                    $find('<%= radDatSoftFrom.ClientID %>').set_enabled(true);
                    $find('<%= radDatSoftTo.ClientID %>').set_enabled(true);
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
                </colgroup>
                <tr>
                    <th>Category
                    </th>
                    <td>
                        <div id="divCategory" runat="server" style="margin: 0 0 0 0">
                            <telerik:RadButton ID="radBtnHardware" runat="server" Text="Hardware" Value="Hardware" AutoPostBack="false" Visible="false"
                                ButtonType="ToggleButton" ToggleType="Radio" GroupName="Category" OnClientCheckedChanged="fn_OnCategoryChanged">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnSoftware" runat="server" Text="Software" Value="Software" AutoPostBack="false"
                                ButtonType="ToggleButton" ToggleType="Radio" GroupName="Category" OnClientCheckedChanged="fn_OnCategoryChanged">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnITService" runat="server" Text="IT Service" Value="ITService" AutoPostBack="false"
                                ButtonType="ToggleButton" ToggleType="Radio" GroupName="Category" OnClientCheckedChanged="fn_OnCategoryChanged">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnBYOS" runat="server" Text="BYOS(Bring Your Own SmartPhone)" Value="BYOS" AutoPostBack="false" Visible="false"
                                ButtonType="ToggleButton" ToggleType="Radio" GroupName="Category" OnClientCheckedChanged="fn_OnCategoryChanged">
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <h3 id="title"></h3>
        <div id="divHardware" class="data_type1" style="display: none">
            <table>
                <colgroup>
                    <col style="width: 25%" />
                    <col />
                </colgroup>
                <tr>
                    <th>Type
                    </th>
                    <td>
                        <telerik:RadButton ID="radBtnLoss" runat="server" Text="분실" Value="Loss" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="HardwareType" OnClientCheckedChanged="fn_OnHardwareTypeChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnTrouble" runat="server" Text="파손(고장)" Value="Trouble" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="HardwareType" OnClientCheckedChanged="fn_OnHardwareTypeChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnNew" runat="server" Text="신규" Value="New" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="HardwareType" OnClientCheckedChanged="fn_OnHardwareTypeChanged">
                        </telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <th>Resource
                    </th>
                    <td>
                        <telerik:RadButton ID="radBtnHardwareMobile" runat="server" Text="Mobile" Value="Mobile" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="HardwareResource" OnClientCheckedChanged="fn_OnHardwareResChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnHardwareIpad" runat="server" Text="아이패드" Value="Ipad" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="HardwareResource" OnClientCheckedChanged="fn_OnHardwareResChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnHardwareComputer" runat="server" Text="Computer" Value="Computer" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="HardwareResource" OnClientCheckedChanged="fn_OnHardwareResChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnHardwareETC" runat="server" Text="ETC(PC Accessary)" Value="ETC" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="HardwareResource" OnClientCheckedChanged="fn_OnHardwareResChanged">
                        </telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <th>Information
                    </th>
                    <td>
                        <div id="divLossComputer" type="HardwareInfo" style="display: none">
                            컴퓨터 분실의 경우 불가항력에 의한 분실 (도난 제외)이 아니라면 직원은 회사에게 사용기간에 의해 계산된 prorata값을 지불하셔야 합니다.
                            <br>
                            아래 내용을 작성해 주시면 새 제품으로 변경해 드립니다.<br>
                            <font style="text-decoration: underline">결제 완료 후,</font>
                            <br>
                            정확한 금액 및 계좌정보는 별도로 알려드리겠습니다
                            <table>
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2">
                                        정보보안사고경위 등록번호(Serial No.)<telerik:RadTextBox ID="radTxtComLossNum" runat="server" Width="100%"></telerik:RadTextBox>
                                       
                                        <br />
                                        *만약 작성하지 않으셨다면 <a href="http://sp-coll-bbs.ap.bayer.cnb/sites/000141/Teamsites/PIPA/Lists/InsecaReport/AllItems.aspx" target="_blank">link</a>을 클릭하여 작성하셔야 합니다.
                                    </td>
                                </tr>
                                
                                <tr style="display:none">
                                    
                                    <th>분실사유</th>
                                    <td>
                                        <telerik:RadButton ID="radBtnComCareless" runat="server" Text="부주의" Value="Careless" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="ComLossReason">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnComStolen" runat="server" Text="도난" Value="Stolen" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="ComLossReason">
                                        </telerik:RadButton>
                                        &nbsp;<font style="color: blue">(도난일 경우 경찰에서 발행한 도난확인서가 필요합니다.)</font>
                                    </td>
                                   
                                </tr>
                                
                                <tr>
                                    <th>신규모델</th>
                                    <td>
                                        <telerik:RadButton ID="radBtnComSmallNotebook" runat="server" Text="Notebook-Small" Value="SmallNotebook" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="LossComModel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnComBigNotebook" runat="server" Text="Notebook-Big" Value="BigNotebook" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="LossComModel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnComDesktop" runat="server" Text="Desktop" Value="Desktop" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="LossComModel">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divLossIpad" type="HardwareInfo" style="display: none">
                            아이패드 분실의 경우 불가항력에 의한 분실 (도난 제외)이 아니라면 직원은 회사에게 사용기간에 의해 계산된<br />
                            prorata값을 지불하셔야 합니다.
                            <br />
                            아래 내용을 작성해 주시면 새 제품으로 변경해 드립니다.<br />
                            <font style="text-decoration: underline">결재 완료 후, </font>
                            <br />
                            정확한 금액 및 계좌정보는 별도로 알려드리겠습니다
                            <table>
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2">
                                        정보보안사고경위 등록번호(Serial No.)<telerik:RadTextBox ID="radTxtIpadLossNum" runat="server" Width="100%"></telerik:RadTextBox>
                                       
                                        <br />
                                        *만약 작성하지 않으셨다면 <a href="http://sp-coll-bbs.ap.bayer.cnb/sites/000141/Teamsites/PIPA/Lists/InsecaReport/AllItems.aspx" target="_blank">link</a>을 클릭하여 작성하셔야 합니다.
                                    </td>
                                </tr>
                                
                                <tr style="display:none">
                                    <th>분실사유</th>
                                    <td>
                                        <telerik:RadButton ID="radBtnIpadCareless" runat="server" Text="부주의" Value="Careless" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="iPadLossReason">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnIpadStolen" runat="server" Text="도난" Value="Stolen" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="iPadLossReason">
                                        </telerik:RadButton>
                                        &nbsp;<font style="color: blue">(도난일 경우 경찰에서 발행한 도난확인서가 필요합니다.)</font>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <th>신규모델</th>
                                    <td>
                                        <telerik:RadTextBox ID="radTxtIpadModel" runat="server" Width="100%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divLossMobile" type="HardwareInfo" style="display: none">
                            본인 부주의로 인한 기기 분실의 경우 잔여할부금을 완납 후 새 기기로 교체 가능합니다.<br />
                            <font style="text-decoration: underline">결재 완료 후, </font>
                            <br />
                            정확한 잔여할부금 및 계좌정보는 별도로 알려드리겠습니다
                            <table>
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2">
                                         정보보안사고경위 등록번호(Serial No.)<telerik:RadTextBox ID="radTxtMobileLossNum" runat="server" Width="100%"></telerik:RadTextBox>
                                       
                                        <br />
                                        *만약 작성하지 않으셨다면 <a href="http://sp-coll-bbs.ap.bayer.cnb/sites/000141/Teamsites/PIPA/Lists/InsecaReport/AllItems.aspx" target="_blank">link</a>을 클릭하여 작성하셔야 합니다.
                                    </td>
                                </tr>
                                
                                <tr style="display:none">
                                    <th>분실사유</th>
                                    <td>
                                        <telerik:RadButton ID="radBtnMobCareless" runat="server" Text="부주의" Value="Careless" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="MobLossReason">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnMobStolen" runat="server" Text="도난" Value="Stolen" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="MobLossReason">
                                        </telerik:RadButton>
                                        &nbsp;<font style="color: blue">(도난일 경우 경찰에서 발행한 도난확인서가 필요합니다.)</font>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <th>신규 모델 및 색상<a href="http://sp-coll-bbs.ap.bayer.cnb/sites/000141/Teamsites/NEW%20e-workflow/LINKED_DATA/Mobile_Model.xlsx" target="_blank"> (link)</a></th>
                                    <td>
                                        <telerik:RadTextBox ID="radTxtMobileModel" runat="server" Width="100%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <th>색상</th>
                                    <td>
                                        <telerik:RadTextBox ID="radTxtMobileColor" runat="server" Width="50%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>

                        </div>

                        <div id="divTroubleComputer" type="HardwareInfo" style="display: none">
                            본인 부주의로 인한 PC 파손 혹은 고장의 경우 Helpdesk에서 PC를 확인하여 수리 및 교체 여부를 확인 후 진행하게 됩니다.
                            <br />
                            수리의 경우 추후 Helpdesk에서 견적서를 전달 받으셔서 SRM을 작성하시면 됩니다.
                            <br />
                            본인 부주의로 인한 파손(고장)의 경우 본인 부담금(20%) 이 발생하며 금액 및 계좌정보를 별도로 안내해드리겠습니다.
                            <br />
                            <table>
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <th>증상 및 파손 사유
                                    </th>
                                    <td>
                                        <telerik:RadTextBox ID="radTxtComState" runat="server" Width="100%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                            ※ IT부서에서 PC를 확인 후 수리 혹은 교체가 결정되면 진행됩니다
                        </div>

                        <div id="divTroubleMobile" type="HardwareInfo" style="display: none">
                            <b>서비스센터에서 견적서를 받아 스캔파일을 첨부 하셔야 합니다. </b>
                            <br>
                            <font style="text-decoration: underline">결재 완료 후,</font>
                            <br>
                            기기 파손 고장의 경우 잔여 할부금과 견적서를 비교 후 수리 혹은 교체 여부 결정에 따라 본인 부담금(20%) 입금을 위한 계좌정보를 별도로 안내해드리겠습니다.
                            <br>
                            수리의 경우 법인카드로 결제후 concur 로 정산하셔야 합니다.(Categroy:Repair & Maintenance Service)
                            <br />
                            <table>
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <th>증상
                                    </th>
                                    <td>
                                        <telerik:RadTextBox ID="radTxtMobileState" runat="server" Width="100%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divTroubleIpad" type="HardwareInfo" style="display: none">
                            <b>서비스센터에서 견적서를 받아 스캔파일을 첨부 하셔야 합니다. </b>
                            <br />
                            기기 파손, 고장의 경우 잔여 할부금과 견적서를 비교 후 수리 혹은 교체 여부 결정에 따라 본인 부담금 (20%) 입금을 위한 계좌정보를 별도로 안내해드리겠습니다.
                            <br />
                            <font style="text-decoration: underline">결재 완료 후, </font>
                            <br />
                            수리의 경우 법인카드로 결제 후 Concur로 정산하셔야 합니다. (Category : Repair & Maintenance Service)
                            <br />
                            <table>
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <th>증상
                                    </th>
                                    <td>
                                        <telerik:RadTextBox ID="radTxtIpadState" runat="server" Width="100%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div id="divNewETC" type="HardwareInfo" style="display: none">
                            PC 악세서리의 경우 우선 재고현황을 확인하셔야 합니다. - <a href="http://sp-coll-bbs.ap.bayer.cnb/sites/000141/Teamsites/NEW%20e-workflow/LINKED_DATA/Stock_PCAccessary.xlsx"
                                target="_blank">재고현황</a><br />
                            만약 재고 수량이 없을 경우에는 <a href="http://go/srm" target="_blank">SRM</a>을 통하여 구매요청을 해주시기 바랍니다. 
                            <table>
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <th>모델</th>
                                    <td>
                                        <telerik:RadButton ID="radBtnNewModelMouse" runat="server" Text="Mouse" Value="Mouse" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="NewModel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnNewModelAdaptor" runat="server" Text="Adapter" Value="Adapter" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="NewModel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnNewModelKeyboard" runat="server" Text="Keyboard" Value="Keyboard" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="NewModel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnNewModelMonitor" runat="server" Text="Monitor" Value="Monitor" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="NewModel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnNewModelDocking" runat="server" Text="Docking" Value="Docking" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="NewModel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnNewModelDVD" runat="server" Text="DVD" Value="DVD" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="NewModel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnNewModelBattery" runat="server" Text="Battery" Value="Battery" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="NewModel">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                                <tr>
                                    <th>사유</th>
                                    <td>
                                        <telerik:RadTextBox ID="radTxtNewETCPurpose" runat="server" Width="100%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div id="divNewComputer" type="HardwareInfo" style="display: none">
                            <b>Functional PC (장비용 기기관리용)</b> 특정목적으로 사용되는 장비 또는 기기를 위해서 사용되는 PC입니다.
                            <br />
                            사용목적과 해당 장비 또는 기기를 판단할 수 있는 정보를 기입해 주시면 IT에서 검토후 OM팀과 제공여부를 회신 드리도록 하겠습니다.
                            <br />
                            <b>Standard PC(일반PC)</b> 신규 PC 신청이 필요한 경우 아래 Standard PC에서 모델명과 사유를 기재 바랍니다.<br>
                            예) 생산직-> 사무직 전환으로 인하여 업무용 PC가 필요 등.
                            <br />
                            * 신규 입사자의 경우 On boarding을 통하여 PC지급이 되며, 기존 사용 PC의 사용기한이 지났을 경우에는 총무부로 문의 해주시면 됩니다. 
                            <table>
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <th>Type</th>
                                    <td>
                                        <telerik:RadButton ID="radBtnComTypeStandard" runat="server" Text="Standard PC" Value="Standard" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="NewComType">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnComTypeFunctional" runat="server" Text="Functional PC" Value="Functional" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="NewComType">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                                <tr>
                                    <th>모델
                                    </th>
                                    <td>
                                        <telerik:RadButton ID="radBtnNewComSmallNotebook" runat="server" Text="Notebook-Small" Value="SmallNotebook" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="LossComModel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnNewComBigNotebook" runat="server" Text="Notebook-Big" Value="BigNotebook" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="LossComModel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnNewComDesktop" runat="server" Text="Desktop" Value="Desktop" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="LossComModel">
                                        </telerik:RadButton>

                                    </td>
                                </tr>
                                <tr>
                                    <th>Purpose
                                    </th>
                                    <td>
                                        <telerik:RadTextBox ID="radTxtNewComPurpose" runat="server" Width="100%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>


        <div id="divSoftware" class="data_type1" style="display: none">
            <table>
                <colgroup>
                    <col style="width: 25%" />
                    <col />
                </colgroup>
                <tr>
                    <th>Software
                    </th>
                    <td>
                        <telerik:RadButton ID="radBtnSoftHwp" runat="server" Text="HWP 2010 (한글 2010)" Value="Hwp2010" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="Software" OnClientCheckedChanged="fn_OnSoftwareChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnSoftAcrobatSTD" runat="server" Text="Acrobat Writer-std" Value="AcrobatSTD" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="Software" OnClientCheckedChanged="fn_OnSoftwareChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnSoftAcrobatPRO" runat="server" Text="Acrobat Writer-pro" Value="AcrobatPRO" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="Software" OnClientCheckedChanged="fn_OnSoftwareChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnSoftETC" runat="server" Text="ETC(직접입력)" Value="ETC" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="Software" OnClientCheckedChanged="fn_OnSoftwareChanged">
                        </telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <th>Information
                    </th>
                    <td>
                        <div id="divSoftInfoHwp2010" type="SoftwareInfo" style="display: none">
                            해당 Software는 Bayer Korea, Cropscience 에서 사용가능하며 신청 후 부서장의 승인 후 Helpdesk에서 원격으로 설치해 드립니다.
                            <br />
                            사용자당 <b>비용(Yearly 50,000 KRW)</b> 이 발생하기 때문에(개별로 부서에 직접 청구되진 않습니다.)
                            <br />
                            반드시 업무목적으로 사용하시기 바랍니다. 1회성 또는 단기간 사용시 공용 인터넷PC를 사용하시거나 사용후 #6262를 통해서 삭제 요청해 주시기 바랍니다. 

                        </div>
                        <div id="divSoftInfoAcrobat" type="SoftwareInfo" style="display: none">
                            해당 Software는 Bayer Korea, Cropscience 에서 사용가능하며 신청 후 부서장의 승인 후 Helpdesk에서 원격으로 설치해 드립니다.<br />
                            사용자당 <b>비용(Yealy 12 ~ 23 Euro)</b>이 발생하기 때문에(개별로 부서에 직접 청구되진 않습니다.)
                            <br />
                            반드시 업무목적으로 사용하시기 바랍니다. 1회성 또는 단기간 사용시 공용 인터넷PC를 사용하시거나 사용후 #6262를 통해서 삭제 요청해 주시기 바랍니다. 
                        </div>
                        <div id="divSoftInfoETC" type="SoftwareInfo" style="display: none">
                            Bayer in Korea 에서 Standard PC제공시 기본적으로 제공되는 S/W 외 추가적으로 필요한 Software는 해당 Software Security 와 License Compliance에 적용을 받게 됩니다.<br>
                            따라서 요청되는 Software는 반드시 업무목적으로 사용되어야 하며 License가 필요한 S/W의 경우는 적합성 검토 후 보유한 License가 없을 경우 반드시 구매부서를 통해서 구입 후 설치가 가능합니다. (법인카드 X)
                            <br>
                            가능한 자세한 업무목적과 구매를 원하시는 S/W 이름 Ver. 제조사등 자세히 기술해 주시면 빠른 시간 내 답변을 드리도록 하겠습니다.
                            <br>
                            추가적으로 보안상의 이유로 Free ware의 설치는 지양하여 필요에 따서 검토 후 설치가 가능합니다. 
                            <br>
                            Printer driver 등은 #6262를 통해서 요청하시면 됩니다 
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Purpose </br>
                        (ETC 직접입력 시 구체적인 프로그램명과 라이선스 여부를 기입해주시기 바랍니다.)
                    </th>
                    <td>
                        <%--<telerik:RadTextBox ID="radTxtSoftPurpose" runat="server" Width="100%"></telerik:RadTextBox>--%>
                        <telerik:RadTextBox ID="radTxtSoftPurpose" TextMode="MultiLine" Height="80px" Width="100%" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>사용기한</th>
                    <td>
                        <telerik:RadButton ID="radRdoPeriod" runat="server" Text="단기" Value="N" GroupName="Permanent" OnClientCheckedChanged="fn_OnPermanetCheckedChanged"
                            AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radRdoPermanent" runat="server" Text="영구" Value="Y" Checked="true" GroupName="Permanent" OnClientCheckedChanged="fn_OnPermanetCheckedChanged"
                            AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                        </telerik:RadButton>
                        <telerik:RadDatePicker runat="server" ID="radDatSoftFrom" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR" Enabled="false">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                        </telerik:RadDatePicker>
                        ~ 
                        <telerik:RadDatePicker runat="server" ID="radDatSoftTo" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR" Enabled="false">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                        </telerik:RadDatePicker>
                    </td>
                </tr>
            </table>
        </div>


        <div id="divITService" class="data_type1" style="display: none">
            <table>
                <colgroup>
                    <col style="width: 25%" />
                    <col />
                </colgroup>
                <tr>
                    <th>Type</th>
                    <td>
                        <telerik:RadButton ID="radBtnSvcTypeEmail" runat="server" Text="Bayer e-mail &amp; account" Value="Email" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="ServiceType" OnClientCheckedChanged="fn_OnServiceChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnSvcTypeMailBox" runat="server" Text="Non-personnel mail box" Value="MailBox" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="ServiceType" OnClientCheckedChanged="fn_OnServiceChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnSvcTypeMDM" runat="server" Text="MDM(Mobile device management" Value="MDM" AutoPostBack="false"  Visible="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="ServiceType" OnClientCheckedChanged="fn_OnServiceChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnSvcTypeRetal" runat="server" Text="Rental PC(Max 2months)" Value="RentalPC" AutoPostBack="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="ServiceType" OnClientCheckedChanged="fn_OnServiceChanged">
                        </telerik:RadButton>
                        <telerik:RadButton ID="radBtnSvcTypeIPT" runat="server" Text="IPT(IP Telephone)" Value="IPT" AutoPostBack="false" Visible="false"
                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="ServiceType" OnClientCheckedChanged="fn_OnServiceChanged">
                        </telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <th>Information</th>
                    <td>
                        <div id="divSvcInfoEmail" type="ServiceInfo" style="display: none">
                            기본적으로 HR을 통해 회사에 입사하는 직원은 On-boarding 절차를 통해 해당 메일 서비스를 받게 됩니다.<br />
                            On-boarding 절차를 제외한(도급계약 Or Part time job)별도의 Bayer e-mail 계정 신청의 경우는 실제 사용자 이름과 목적을 작성하여 주시기 바랍니다.
                        </div>
                        <div id="divSvcInfoMailBox" type="ServiceInfo" style="display: none">
                            개인이 사용하는 mail box 외 공용사용의 목적이나 특수목적의 mail box가 필요한 경우 사용이 가능합니다.<br />
                            <font style="text-decoration: underline">결재 완료 후,</font>
                            <br />
                            <a href="http://sp-coll-bbs.ap.bayer.cnb/sites/000141/Teamsites/NEW%20e-workflow/LINKED_DATA/How%20to%20create%20Non%20Personal%20email%20box.docx" target="_blank">Link </a>을 클릭하여 Non personal Mail box을 생성하셔야 합니다.
                        </div>
                        <div id="divSvcInfoMDM" type="ServiceInfo" style="display: none">
                            MDM 은 Bayer 법인 소유의 idevice(iphone, ipad)기기중 Bayer 네트워크를 통해 Data(Email, Contact, Calendar, SharePoint, Net drive)에 접근하기 위한 보안 프로그램입니다.<br />
                            해당 서비스는 비용이 발생하는 서비스이며 개별로 비용을 청구하지는 않습니다. 
                            <table>
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <th>Model</th>
                                    <td>
                                        <telerik:RadButton ID="radBtnSvcMDMIPhone" runat="server" Text="IPhone" Value="IPhone" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="MDMModel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnSvcMDMIPad" runat="server" Text="IPad" Value="IPad" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="MDMModel">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Mac Address</th>
                                    <td>
                                        <telerik:RadTextBox ID="radTxtSvcMDMMac" runat="server"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Serial Number</th>
                                    <td>
                                        <telerik:RadTextBox ID="radTxtSvcMDMSerial" runat="server"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Phone number
                                    </th>
                                    <td>
                                        <telerik:RadTextBox ID="radTxtSvcMDMPhone" runat="server"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divSvcInfoRentalPC" type="ServiceInfo" style="display: none">
                            단기 직원이나 교육목적으로 최장 2달간 임대PC를 대여해 드립니다.
                            <br />
                            반드시 부득이한 업무목적으로 Standard PC지급되어야 하거나 또는 부득이한 사유로 별도의 PC가 필요한 경우에 해당 합니다.<br />
                            해당 PC는 사용기한이 지난 PC를 재활용하고 있어 성능이 다소 부족할 수 있으나 일반적인 업무를 진행함에 있어서는 지장이 없습니다.
                        </div>

                        <div id="divSvcInfoIPT" type="ServiceInfo" style="display: none">
                            좀 더 효율적인 IT Service 위하여 2017년 9월 30일부로 e-workflow내 IT resource / IPT service 를 종료합니다.<br />
                            신규 IP전화기 개통 및 변경 프로세스는<br />
                             1.	전화기가 없는 경우는 총무팀(Eunjung.Park)에 문의하여 기기 수령  <br />
                             2. 기기를 보유하신 상태에서 <a href="http://sp-coll-bbs.bayer-ag.com/sites/030836/_layouts/15/WopiFrame.aspx?sourcedoc=/sites/030836/Tip/IPT_Request_v3.5.xlsx&action=default" target="_blank"> <font style="color: blue">IP전화기 개통/변경 신청서를 다운로드 및 작성</font></a>하시어 ap.servicedesk@bayer.com 으로 <br />
                            &nbsp;&nbsp;&nbsp;&nbsp; 서비스 요청해주시기 바랍니다. <br />
                         
                            IP전화기개통 및 사용자변경 등에 관련해서는 #6262, ap.servicedesk@bayer.com으로 문의 주시기 바랍니다. 
                            <table>
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td colspan="2">
                                        <telerik:RadButton ID="radBtnSvcExistIPT" runat="server" Text="전화기보유" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="CheckBox">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Mac Address 
                                    </th>
                                    <td>
                                        <telerik:RadTextBox ID="radTxtSvcIPTMac" runat="server"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>모델</th>
                                    <td>
                                        <telerik:RadButton ID="radBtnSvcIPTNormal" runat="server" Text="일반 유저용" Value="NormalUser" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="ServiceIPTModel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="radBtnSvcIPTManager" runat="server" Text="비서 Or Manager" Value="Manager" AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="Radio" GroupName="ServiceIPTModel">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr id="rowSvcAccountName" style="display: none">
                    <th>Full Name
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtSvcAccountName" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Purpose</th>
                    <td>
                        <telerik:RadTextBox ID="radTxtSvcPurpose" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="rowServicePeriod" style="display: none">
                    <th>사용기한</th>
                    <td>
                        <telerik:RadDatePicker runat="server" ID="radDatSvcFrom" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                        </telerik:RadDatePicker>
                        ~ 
                        <telerik:RadDatePicker runat="server" ID="radDatSvcTo" Calendar-ShowRowHeaders="false" Width="100px" Culture="ko-KR">
                            <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                        </telerik:RadDatePicker>
                    </td>
                </tr>
            </table>
        </div>

         <div id="divBYOS" class="data_type1" style="display: none">
            <table>
                <colgroup>
                    <col style="width: 25%" />
                    <col />
                </colgroup>
                <tr>
                    <th>Information
                    </th>
                    <td>
                        <div id="divBYOS_Information" type="BYOSInfo" style="display: none">
                            <b>BYOS ( Bring your own SmartPhone)</b>는 여러분들 소유의 개인 스마트폰에서 회사의 이메일, 일정, 연락처 등에 유연하게 접근할 수 있도록 해줍니다. 그리고 여러분의 스마트폰에서 개인 데이터와 회사 데이터가 분리되는 안전한 솔루션을 제공합니다. BYOS 기기에는 바이엘에서 관리하는 캡슐 어플리케이션이 설치되며, 기본적인 규칙이 준수되지 않을 경우 바이엘은 서비스를 중단할 수 있습니다. <br />
                            여러분이 업무 목적으로 사용한 음성/데이터 사용료는 Concur 시스템을 통해서 비용을 청구하실 수 있습니다. <br />
                            Concur 관련 문의는 <a href='mailto:FASS.TRAVELEXPENSES.KOREA@BAYER.COM'>FASS.TRAVELEXPENSES.KOREA@BAYER.COM</a> 으로 연락 주시기 바랍니다. <br /><br />

                            이미지 추가      
                            

                            BYOS 신청을 원하실 경우, 아래 링크된 BYOS 이용 약관을 읽어 보신 후 동의를 하실 경우에만 아래 “BYOS 이용 약관에 동의합니다”에 체크해주시기 바랍니다. <br/><br/> 
                            이와 관련 문의 사항은 <a href='mailto:itservicedesk@bayer.com'>itservicedesk@bayer.com</a>
                            으로 연락 주시기 바랍니다. <br/>
                            <table style="border:0">
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr >
                                    <td colspan="2">
                                        <te
                                        <telerik:RadButton ID="radBtnSvcBYOS_AGREE" runat="server" Text="BYOS 이용 약관에 동의합니다. " AutoPostBack="false"
                                            ButtonType="ToggleButton" ToggleType="CheckBox">
                                        </telerik:RadButton>
                                    </td>
                                    <td> BYOS 이용 약관 살펴보기 (추후 약관 링크 걸기) </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>


    </div>
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="HddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

