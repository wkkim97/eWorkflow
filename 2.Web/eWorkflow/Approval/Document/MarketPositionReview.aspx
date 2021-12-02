<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="MarketPositionReview.aspx.cs" Inherits="Approval_Document_MarketPositionReview" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">

    <script type="text/javascript">

        //1위,2위,3위 Market Share 합계 구하기
        function CalcMarketShare() {
            var txtMS_1  = $('#<%=RadtxtRank_1.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');
            var txtMS_2 = $('#<%=RadtxtRank_2.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');
            var txtMS_3 = $('#<%=RadtxtRank_3.ClientID%>').val().replace(/,/gi, '').replace(/ /gi, '');
            var txtMS_Total = $('#<%=RadtxtRank_Total.ClientID%>');

            if (txtMS_1 == "") { txtMS_1 = 0; }
            if (txtMS_2 == "") { txtMS_2 = 0; }
            if (txtMS_3 == "") { txtMS_3 = 0; }

            var total = parseFloat(txtMS_1) + parseFloat(txtMS_2) + parseFloat(txtMS_3);

            txtMS_Total.val(total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));

            if (parseFloat(total) >= 75)
                alert("Marketing 전략 관련 pre-consulting & Brand Planning Meeting에 AT BP를 초대하십시오.");
        }    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <h3>Please select a planned Strategic Change</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col />
                    <col style="width: 75%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th>Division / BU <span class="text_red">*</span></th>
                        <td>
                            <table style="margin-bottom: 5px; width: 99%;">
                                <tr>
                                    <th>BKL</th>
                                    <td colspan="3">
                                        <telerik:RadButton runat="server" ID="RadrdoBHP" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="PH" Text="PH" GroupName="BU" AutoPostBack="false" Visible="false"></telerik:RadButton>
                                        <telerik:RadButton runat="server" ID="RadrdoHH" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="HH" Text="HH" GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton runat="server" ID="RadrdoWH" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="WH" Text="WH" GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton runat="server" ID="RadrdoSM" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="SM" Text="SM" GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton runat="server" ID="RadrdoRI" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="R" Text="R" GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton runat="server" ID="RadrdoCC" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="CC" Text="CH" GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton runat="server" ID="RadrdoAH" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="AH" Text="AH" GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton runat="server" ID="RadrdoDC" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="DC" Text="DC" GroupName="BU" AutoPostBack="false" Visible="false"></telerik:RadButton>
                                    </td>
                                </tr>
                                <tr>
                                    <th>BCS</th>
                                    <td>
                                        <telerik:RadButton runat="server" ID="RadrdoCP" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="CP" Text="CP " GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                                        &nbsp;&nbsp;
												<telerik:RadButton runat="server" ID="RadrdoES" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="ES" Text="ES " GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                                        &nbsp;&nbsp;
												<telerik:RadButton runat="server" ID="RadrdoBVS" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="Monsanto" Text="Monsanto" GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton runat="server" ID="RadrdoIS" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="IS" Text="IS" GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                                    </td>
                                    </td>
                                </tr>
<%--                                <tr>
                                    <th>BMS</th>
                                    <td>
                                        <telerik:RadButton runat="server" ID="RadrdoPUR" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="PUR" Text="PUR" GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton runat="server" ID="RadrdoPCS" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="PCS" Text="PCS" GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton runat="server" ID="RadrdoCAS" CssClass="radio" ButtonType="ToggleButton" ToggleType="Radio" Value="CAS" Text="CAS" GroupName="BU" AutoPostBack="false"></telerik:RadButton>
                                    </td>
                                </tr>--%>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <th>Year (전년도 기재) <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadNumericTextBox runat="server" ID="Radtxtyear" InputType="Number"
                                NumberFormat-GroupSeparator=""
                                Width="70px" NumberFormat-DecimalDigits="0" MinValue="1900" MaxValue="2050">
                            </telerik:RadNumericTextBox></td>
                    </tr>
                    <tr>
                        <th>Product <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtProdut" Width="98%" /></td>
                    </tr>
                    <tr>
                        <th>Application / Indication</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtApplication" Width="98%" /></td>
                    </tr>
<%--                    <tr>
                        <th>Competitor</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtCompetitor" Width="98%" /></td>
                    </tr>--%>

                    <tr>
                        <th>Market Share info <span class="text_red">*</span></th>
                        <td>
                        <table>
                            <tr>
                                <td style="width:150px;" rowspan="2">Market Share</td>
                                <td style="width:120px;" >(%)</td>
                                <td style="width:120px;" >Volume</td>
                                <td >Value</td>
                            </tr>
                            <tr>
                                <td><telerik:RadNumericTextBox runat="server" ID="RadtxtShare" MaxLength="3" NumberFormat-DecimalDigits="0" Width="120" EnabledStyle-HorizontalAlign="Right"></telerik:RadNumericTextBox></td>
                                <td><telerik:RadTextBox runat="server" ID="RadtxtVolume" Width="120px" /></td>
                                <td ><telerik:RadTextBox runat="server" ID="RadtxtValue" Width="120px" /></td>
                            </tr>

                            <tr>
                                <td rowspan="2">Market Ranking (%)</td>
                                <td style="text-align:left">1st</td>
                                <td style="text-align:left">2nd</td>
                                <td style="text-align:left">3rd</td>
                                <td style="text-align:left">Total</td>
                            </tr>
                            <tr>
                                <td><telerik:RadNumericTextBox runat="server" ID="RadtxtRank_1"     MaxLength="3" NumberFormat-DecimalDigits="0" Width="120" EnabledStyle-HorizontalAlign="center" ClientEvents-OnBlur="CalcMarketShare"></telerik:RadNumericTextBox></td>
                                <td><telerik:RadNumericTextBox runat="server" ID="RadtxtRank_2"     MaxLength="3" NumberFormat-DecimalDigits="0" Width="120" EnabledStyle-HorizontalAlign="center" ClientEvents-OnBlur="CalcMarketShare"></telerik:RadNumericTextBox></td>
                                <td><telerik:RadNumericTextBox runat="server" ID="RadtxtRank_3"     MaxLength="3" NumberFormat-DecimalDigits="0" Width="120" EnabledStyle-HorizontalAlign="center" ClientEvents-OnBlur="CalcMarketShare"></telerik:RadNumericTextBox></td>
                                <td><telerik:RadNumericTextBox runat="server" ID="RadtxtRank_Total" MaxLength="3" NumberFormat-DecimalDigits="0" Width="80" BorderColor="White" MinValue="0" MaxValue="999" Value="0" EnabledStyle-HorizontalAlign="center" ReadOnlyStyle-HorizontalAlign="center"></telerik:RadNumericTextBox></td>
                            </tr>
                            <tr>
                                <td>Product / <br> Company name</td>
                                <td><telerik:RadTextBox runat="server" ID="RadtxtProductCompany_1" EmptyMessage="1위 제품명/회사명" TextMode="MultiLine" Height="50px" Width="100%" /></td>
                                <td><telerik:RadTextBox runat="server" ID="RadtxtProductCompany_2" EmptyMessage="2위 제품명/회사명" TextMode="MultiLine" Height="50px" Width="100%" /></td>
                                <td><telerik:RadTextBox runat="server" ID="RadtxtProductCompany_3" EmptyMessage="3위 제품명/회사명" TextMode="MultiLine" Height="50px" Width="100%" /></td>
                            </tr>
                        </table>
                        </td>
                    </tr>
                    <tr>
                        <th>Data Source(if any)</th>
                        <td>
                            <telerik:RadTextBox runat="server" ID="RadtxtDataSource" Width="98%"></telerik:RadTextBox></td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="doc_style">
            <h3>Self-Assessment of Abasement of Market Position</h3>
            <div class="data_type1">
                <table>
                    <colgroup>
                        <col />
                        <col style="width: 75%;" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>Risk Cases</th>
                            <th>Question</th>
                            <th style="width: 25px;">YES</th>
                        </tr>
                        <tr>
                            <th>Discrimination<br />
                                차별</th>
                            <td>Application by a dominant company of different conditions to equivalent transactions without objective justification.<br />
                                객관적이고 정당한 근거없이 유사한 거래에 차별화된 조건을 적용하고 있는가? (아래 예시에 해당되는 차별은 ‘No’로 답변)<br />
                                <br />
                                <u>정당한 근거에 의한 차별</u>
                                <br />
                                •고객간에 원가의 차이가 있는 경우. 예: 수송원가 또는 세금 등<br />
                                •규모의 경제. 예: 대량구매리베이트<br />
                                •고객이 제공하는 서비스의 진정한 가치에 대한 보상으로서의 가격 인하. 예: 프로모션, 물류 또는 보관 서비스<br />
                                •신제품 출시 또는 신규 시장 진출시의 가격 인하.  </td>
                            <td>
                                <telerik:RadButton runat="server" ID="RadchkDis" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <th>Excessive Pricing<br />
                                가격남용</th>
                            <td>Sale by dominant company of a product at excessively high price which bears no reasonable relation to product’s economic value.<br />
                                제품의 경제적 가치에 비하여 합리적 사유 없이 지나치게 높은 가격을 설정하고 있는가?  </td>
                            <td>
                                <telerik:RadButton runat="server" ID="RadchkExcess" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <th>Predatory Pricing<br />
                                약탈적가격책정</th>
                            <td>Sale by a dominant company of a product at below cost level in order to drive out competition.<br />
                                경쟁사를 배제하기 위해 원가 수준 이하의 가격을 책정하고 있는가? </td>
                            <td>
                                <telerik:RadButton runat="server" ID="RadchkPre" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <th>Margin Squeeze<br />
                                이윤압착</th>
                            <td>Where dominant company operates at different levels of supply chain, pricing of upstream material at such a level as to drive competitors at downstream level out of business.<br />
                                공급망의 여러단계에서 사업을 하는 경우, 완제품 시장에서 경쟁사를 배제할 수 있는 수준으로 원료의 가격을 책정하였는가? </td>
                            <td>
                                <telerik:RadButton runat="server" ID="RadchkMargin" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <th>Tying and Bundling<br />
                                끼워팔기 및 묶어팔기</th>
                            <td>Requirement by supplier that purchaser of product in which supplier is dominant must also purchase unconnected product from supplier.<br />
                                시장 지배적 지위에 있는 제품을 판매할때 별개의 제품을 함께 구매하도록하고 있는가?  </td>
                            <td>
                                <telerik:RadButton runat="server" ID="RadchkTying" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <th>Loyalty-Inducing Rebates<br />
                                and Discounts<br />
                                충성 유도리베이트 및 할인</th>
                            <td>Rebates offered by a dominant company which have the effect of rewarding loyalty and discouraging customers from switching business to alternative suppliers.<br />
                                충성에 대한 보상을 하거나 또는 경쟁사로 거래를 전환하는 것을 차단하는 효과를 내는 리베이트를 제공하고 있는가?<br />
                                <br />
                                <u>충성유도리베이트예시</u>•성실리베이트:고객 자신의 수요전부 또는 대다수를 구입하는 조건으로 해당고객에게 제공되는 리베이트<br />
                                •목표리베이트: 전년도의 수량 또는 연간 수용비율을 크게 능가하는 수량 또는 연간 수요비율과 같은 특정목표에 도달하는 조건으로 제공되는 리베이트<br />
                                •소급리베이트: 특정목표 수준이상으로 구입한 제품 수량에 적용 할 뿐만 아니라 목표수준에 도달하고 나면 이전구매, 특히 1년단위와같이비교적장기간에대하여소급적용하는리베이트<br />
                                •기하급수적으로 증가하는 할인: 자신의 수요전부 또는 대다수를 구입하도록 유인하기 위해, 고객이 특정구매목표 수준에 도달하면 기하급수적으로 증가하는 할인 </td>
                            <td>
                                <telerik:RadButton runat="server" ID="RadchkLoyaly" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <th>Abusive Exclusive Dealing<br />
                                배타적거래</th>
                            <td>Requirement by dominant supplier that customer buys all or most of its requirements of relevant products from dominant supplier over a significant period.<br />
                                특정 고객수요의 전부 또는 대부분을 상당 기간동안 구매하도록 합의하고 있는가?</td>
                            <td>
                                <telerik:RadButton runat="server" ID="RadchkDealing" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <th>Abusive Refusal to Supply<br />
                                공급거래거절</th>
                            <td>Refusal by a dominant company to supply product to an existing customer without objective justification.<br />
                                객관적이고 정당한 사유없이 기존고객에 대한 공급을 중단했는가?<br />
                                <br />
                                정당한 사유의 예시;<br />
                                •순수한 공급부족•고객의 지급능력에 관한 합법적 사유 </td>
                            <td>
                                <telerik:RadButton runat="server" ID="RadchkSupply" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false"></telerik:RadButton>
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
    <input type="hidden" id="hddReuse" runat="server" />
    <input type="hidden" id="hddAddRow" runat="server" value="N" />
</asp:Content>

