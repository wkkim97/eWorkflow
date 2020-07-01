<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master"  AutoEventWireup="true" CodeFile="CustomerComplaintHandling.aspx.cs" Inherits="Approval_Document_CustomerComplaintHandling" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function pageLoad() {
            var addRow = $('#<%= hddAddRow.ClientID %>').val();
                       
            if (addRow == 'Y') {
                var grid = $find('<%=radGrdCustomerComplaintHandling.ClientID%>');

                if (grid.get_masterTableView()) {
                    var dataItems = grid.get_masterTableView().get_dataItems();
                    if (dataItems.length > 0) {
                        grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
                    }
                }
                $('#<%= hddAddRow.ClientID %>').val('Y');
            }
        }

        function fn_UpdateGridDataFromUploader() {
            fn_UpdateGridData(false);
        }

        function fn_DoRequest(sender, args) {
            return fn_UpdateGridData(null);
        }

        function fn_DoSave(sender, args) {
            return fn_UpdateGridData(null);
        }

        //-----------------------------------
        // 그리드 클라이언트 데이터 업데이트
        //-----------------------------------
        function fn_UpdateGridData(data) {
            var list = [];
            var masterTable = $find('<%= radGrdCustomerComplaintHandling.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var is_val = false;
            var maxIdx = 0;

            for (var i = 0; i < dataItems.length; i++) {

                var idx = dataItems[i].get_cell("IDX").innerText.trim();
                var ProductCode = dataItems[i].get_cell("PRODUCT_CODE").innerText.trim();
                var ProductName = dataItems[i].get_cell("PRODUCT_NAME").innerText.trim();

                var demand_code = dataItems[i].get_cell("DEMAND_CODE").innerText.trim();

                var ctrlDemand = dataItems[i].findControl('radDropDemand');

                var demand_name = null;
                if (ctrlDemand)
                    demand_name = ctrlDemand._selectedText;
                else
                    demand_name = dataItems[i].get_cell("DEMAND_NAME").children[0].innerText.trim();

                var Population_No = dataItems[i].get_cell("POPULATION_NO").innerText.trim();

                var qty = '0';
                if (dataItems[i].findElement('radGrdNumQty'))
                    qty = dataItems[i].findElement('radGrdNumQty').value.replace(/,/gi, '').replace(/ /gi, '');
                else
                    qty = dataItems[i].get_cell("ISSUE_QTY").children[0].innerText.trim().replace(/,/gi, '').replace(/ /gi, '');


                var product = {
                    IDX: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    DEMAND_CODE: null,
                    DEMAND_NAME: null,
                    POPULATION_NO: null,
                    ISSUE_QTY: null,

                }
                maxIdx = idx;
                product.IDX = parseInt(idx);
                product.PRODUCT_CODE = ProductCode;
                product.PRODUCT_NAME = ProductName;
                product.DEMAND_CODE = demand_code;
                product.DEMAND_NAME = demand_name;
                product.POPULATION_NO = Population_No;

                product.ISSUE_QTY = qty;

                list.push(product);
            }
            $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));

            if (data) {
                    
                var product = {
                    IDX: null,
                    PRODUCT_CODE: null,
                    PRODUCT_NAME: null,
                    DEMAND_CODE: null,
                    DEMAND_NAME: null,
                    POPULATION_NO: null,
                    ISSUE_QTY: null,

                }
                for (var i = 0; i < dataItems.length; i++) {
                    var ProductCode = dataItems[i].get_cell("PRODUCT_CODE").innerText;
                    var ProductName = dataItems[i].get_cell("PRODUCT_NAME").innerText;

                    if (data.PRODUCT_CODE == ProductCode && data.PRODUCT_NAME == ProductName) {
                        fn_OpenDocInformation('동일한 product 가 존재합니다.');
                        var is_val = true;
                        break;
                    }
                }
               
                if (is_val == false) {
                    maxIdx++;
                    product.IDX = parseInt(maxIdx);
                    product.PRODUCT_CODE = data.PRODUCT_CODE;
                    product.PRODUCT_NAME = data.PRODUCT_NAME + ' (' + data.PRODUCT_CODE + ')';
                    product.DEMAND_CODE = "";
                    product.DEMAND_NAME = "";
                    product.POPULATION_NO = "";
                    product.ISSUE_QTY = 0;
                    
                    list.push(product);
                   
                    $('#<%= hddGridItems.ClientID%>').val(JSON.stringify(list));
                    

                }
                is_val = false;
            }
            
            return true;
        }

        function getSelectedBG() {
            var controls = $('#<%= divBG.ClientID %>').children();
            var selectedValue;

            for (var i = 0; i < controls.length; i++) {
                var bu = controls[i];
                if ($find(bu.id).get_checked()) {
                    selectedValue = $find(bu.id).get_value();
                    break;
                }
            }
            return selectedValue;
        }

        var selectedValue;
        function fn_OpenProduct(sender, args) {

            fn_UpdateGridData(null);
            var dataItems = $find('<%= radGrdCustomerComplaintHandling.ClientID %>').get_masterTableView().get_dataItems();

            if (dataItems.length > 0) {
                for (var i = 0; i < dataItems.length; i++) {
                    
                    var ProductCode = dataItems[i].get_cell("PRODUCT_CODE").innerText.trim();
                    var ProductName = dataItems[i].get_cell("PRODUCT_NAME").innerText.trim();
                    if (ProductCode == '' || ProductName == '') {
                        fn_OpenDocInformation('자료를 입력하시기 바랍니다.');
                        return false;
                    }
                }
            }
            var selectedValue = getSelectedBG();
            var selectedCategoryValue = "";

            // Invoice price는 'I', base price 는 'Y'를 이용한다.
            // procedure eManage.dbo.USP_SELECT_PRODUCT 수정

           selectedCategoryValue = "I";

           var wnd = $find("<%= radWinPopupCurProduct.ClientID %>");
           if (selectedValue == "") {
               fn_OpenDocInformation("사업그룹을 선택하세요.");
                return;
           }
           wnd.setUrl("/eWorks/Common/Popup/ProductList.aspx?bu=" + selectedValue);
           wnd.show();
        }


        function fn_setProduct(oWnd, args) {
            var product = args.get_argument();

            if (product != null) {
                
                fn_UpdateGridData(product); 
                
                $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("Rebind");
                
            }
        }


        //-----------------------------
        //  Grid Delete Event
        //-----------------------------
        function openConfirmPopUp(IDX) {
            clickedKey = parseInt(IDX);
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);
            fn_UpdateGridData(null);

            return false;
        }

        function confirmCallBackFn(arg) {
            if (arg) {
                var masterTable = $find('<%= radGrdCustomerComplaintHandling.ClientID %>').get_masterTableView();
                masterTable.fireCommand("Remove", clickedKey);
            }
        }

        //-----------------------------
        //  Grid Focus
        //-----------------------------
        function openGridRowForEdit(sender, args) {
            var grid = $find('<%=radGrdCustomerComplaintHandling.ClientID%>');

            var dataItems = grid.get_masterTableView().get_dataItems();
            if (dataItems.length > 0) {
                grid.get_batchEditingManager().openRowForEdit(dataItems[dataItems.length - 1].get_element());
            }
        }
        function fn_OnGridKeyUp(sender) {
            var row = $find('<%= radGrdCustomerComplaintHandling.ClientID %>').get_batchEditingManager().get_currentlyEditedRow();
            
            if (row == null) return;
            
            var dataItem = $find(row.id);
            
            if (dataItem) {
                
                var qty = 0

                var ctlQty = dataItem.findElement('radGrdNumQty');
                if (ctlQty) qty = ctlQty.value.replace(/,/gi, '').replace(/ /gi, '');

                //alert(sender);
                //alert(sender.value);
                //sender.value = 0;
            }
        }
        function fn_OnGridNumBlur(sender, args) {
            setNumberFormat(sender);
        }

    function fn_OpenCustomer(sender, args) {
        var wnd = $find("<%= radWinPopupCustomer.ClientID %>");
        wnd.setUrl("/eWorks/Common/Popup/CustomerListForCS.aspx");
        wnd.show();
        sender.set_autoPostBack(false);
            
    }

    function fn_setCustomer(oWnd, args) {
        var item = args.get_argument();

        if (item != null) {
                
            var txt = $find("<%= radGrdTxtCustomer.ClientID %>");
            txt.set_value(item.CUSTOMER_NAME.trim() + "(" + item.CUSTOMER_CODE + ")_" + item.PARVW);
            $('#<%= hddCustomerCode.ClientID %>').val(item.CUSTOMER_CODE);
            $('#<%= hddCustomerType.ClientID %>').val(item.PARVW);
            //$('#<%= Labelcustomertype.ClientID %>').text($('#<%= hddCustomerType.ClientID %>').val());
                
            $find('<%= RadAjaxManager.GetCurrent(Page).ClientID %>').ajaxRequest("removegrid");
        }
        else
            return false;
    }

    // Visible
    function fn_VisibleDiv(sender, args) {
        var type = sender.get_value();
        setVisible('', type);
    }
    function setVisible(value, type) {

        if (type == 'Effect') {
            $('#divEffect').show();
            $('#divQuality').hide();
            $('#divDelivery').hide();
        }
        if (type == 'Quality') {
            $('#divEffect').hide();
            $('#divQuality').show();
            $('#divDelivery').hide();
        }
        if (type == 'Delivery') {
            $('#divEffect').hide();
            $('#divQuality').hide();
            $('#divDelivery').show();
        }
    }

    function dateSelected(sender, args) {
        var dtIssueDate = $find('<%= radDatIssueDate.ClientID%>').get_selectedDate();

        var dtToday = new Date();

        dtIssueDate = new Date(dtIssueDate.getFullYear(), dtIssueDate.getMonth()+1, dtIssueDate.getDate());

        dtToday = new Date(dtToday.getFullYear(), dtToday.getMonth()+1, dtToday.getDate());

        var diff = dtToday.getTime() - dtIssueDate.getTime();
        diff = Math.ceil(diff / (1000 * 3600 * 24));
        diff = diff + "일"

        var txtPeriod = $find("<%= radTxtIssueExpiredPeriod.ClientID%>");
        txtPeriod.set_value(diff);
    }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <div class="data_type1">
            <table>
                <colgroup>
                    <col />
                    <col style="width: 75%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th>사업그룹 <span class="text_red">*</span></th>
                        <td>
                            <div id="divBG" runat="server">
                                <telerik:RadButton ID="radRdoBG1" runat="server" Text="작물보호" Value="CP" GroupName="BG" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoBG2" runat="server" Text="원제사업" Value="IS" GroupName="BG" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoBG4" runat="server" Text="환경과학" Value="ES" GroupName="BG" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" >
                                </telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>불만의 종류 <span class="text_red">*</span></th>
                        <td>
                            <div id="divType" runat="server">
                                <telerik:RadButton ID="radRdoType1" runat="server" Text="약효미흡/약해발생" Value="Effect" GroupName="Type" ButtonType="ToggleButton" ToggleType="Radio" AutoPostBack="false" OnClientClicked="fn_VisibleDiv">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoType2" runat="server" Text="품질(제품의 이화학적 특성 및 포장상태 등)" Value="Quality" GroupName="Type" ButtonType="ToggleButton" ToggleType="Radio" AutoPostBack="false" OnClientClicked="fn_VisibleDiv">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoType3" runat="server" Text="배송서비스 등" Value="Delivery" GroupName="Type" ButtonType="ToggleButton" ToggleType="Radio" AutoPostBack="false" OnClientClicked="fn_VisibleDiv">
                                </telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>영업 지점의 요구사항 <span class="text_red">*</span></th>
                        <td>
                            <telerik:RadTextBox ID="radTxtDemand" runat="server" TextMode="MultiLine" Height="40px" Width="100%" EmptyMessage="지점의 요구사항을 입력합니다."  ></telerik:RadTextBox>
                        </td>
                    </tr>

                </tbody>
            </table>
        </div>
        <h3>Product List
        <div class="title_btn">
            <telerik:RadButton ID="radBtnAdd" runat="server" Text="Add" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" AutoPostBack="false"
                ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" OnClientClicked="fn_OpenProduct">
            </telerik:RadButton>
        </div>
        </h3>
    </div>
    <telerik:RadGrid ID="radGrdCustomerComplaintHandling" runat="server" AutoGenerateColumns="false" 
        HeaderStyle-HorizontalAlign="Center"
        EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" GridLines="None" AllowAutomaticUpdates="true"
         OnPreRender="radGrdCustomerComplaintHandling_PreRender" OnItemCommand="radGrdCustomerComplaintHandling_ItemCommand" 
        OnItemDataBound="radGrdCustomerComplaintHandling_ItemBound"
         HeaderStyle-CssClass="grid_header">
        <MasterTableView EnableHeaderContextMenu="true" EditMode="Batch" DataKeyNames="IDX">
            <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
            <Columns>
                <telerik:GridBoundColumn DataField="IDX" HeaderStyle-Width="5px" UniqueName="IDX" ItemStyle-HorizontalAlign="Center" ReadOnly="true"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PRODUCT_CODE" HeaderText="" UniqueName="PRODUCT_CODE" Display="false" ReadOnly="true"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PRODUCT_NAME" HeaderText="Description" UniqueName="PRODUCT_NAME" HeaderStyle-Width="40%" ReadOnly="true"></telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="DEMAND_CODE" HeaderText="" HeaderStyle-Width="40px" UniqueName="DEMAND_CODE" Display="false">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="DEMAND_NAME" UniqueName="DEMAND_NAME" HeaderText="요구사항" HeaderStyle-Width="80px">
                    <ItemTemplate><%# Eval("DEMAND_NAME")%></ItemTemplate>
                    <EditItemTemplate>
                        <telerik:RadDropDownList ID="radDropDemand" runat="server" Width="100%" DropDownWidth="195px"></telerik:RadDropDownList>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn Display="true" DataField="POPULATION_NO" UniqueName="POPULATION_NO" HeaderText="모집단번호" HeaderStyle-Width="15%">
                    <ItemStyle HorizontalAlign="center" />
                    <ItemTemplate><%# Eval("POPULATION_NO")%></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="radGrdTxtPopultationNo" runat="server" Width="100%" CssClass="input"></asp:TextBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn DataField="ISSUE_QTY" UniqueName="ISSUE_QTY" HeaderText="이슈수량(병/봉)" HeaderStyle-Width="15%" DataType="System.Decimal">
                    <ItemStyle HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("ISSUE_QTY")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="radGrdNumQty" runat="server" Width="100%" CssClass="input align_right"
                            onblur="return fn_OnGridNumBlur(this)" onfocus="return fn_OnGridNumFocus(this)"
                            onkeypress="return fn_OnGridKeyPress(this, event)"
                            onkeyup="fn_OnGridKeyUp(this)"
                            DecimalDigits="0" AllowNegative="false">                                
                        </asp:TextBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="10px" HeaderText="" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" ImageUrl="~/Styles/images/ico_del.png"
                            OnClientClick='<%# String.Format("return openConfirmPopUp({0});",Eval("IDX"))%> ' BorderStyle="None" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
<%--    <div id="CuRe" style="display: none">C :<span class="text_red">Current</span> / R :<span class="text_red">Replacement</span></div>--%>
    <br />
        <h3 style="background-color:darkblue; color:white; text-align:center" >고객불만 보고서 (A) </h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col />
                    <col style="width: 75%;" />
                </colgroup>
                <tbody>
                    <tr><th>이 보고서는 제품의 생물적, 물리적, 화학적 품질과 관련되며, 문제해결과 품질개선 그리고 통계 자료로 활용하기 위한 목적이며, 바이엘크롭사이언스(주)<br />
                            직원이면 누구나 발생 보고 할 수 있으며, 어느 누구도 이를 문제 삼을 수 없습니다.<br />
                            본 보고서는 당사의 제품을 사용한 고객이 민원을 제기한 경우 작성하여 보고합니다.<br />
                            단, 문의사항은 제외합니다. 고객불만 접수 5 근무일 이내에 이 보고서를 작성할 것을 권고합니다. <br /></th></tr>
                    <tr><td style="text-align:center">품질 및 배송서비스 관련 민원은 고객불만보고서(A)만 작성하여 보고합니다. </td> </tr>
                </tbody>
            </table>
        </div>
        <h3>불만 제기 고객 정보</h3>
        <div class="data_type1">
            <table>
                <tbody>
                    <tr>
                        <th>고객 유형 (택1) <span class="text_red">*</span></th>
                        <td colspan="3" >
                            <div id="divCustomerType" runat="server">
                                <telerik:RadButton ID="radRdoNH" runat="server" Text="농협" Value="NH" GroupName="CustomerType" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoFM" runat="server" Text="시판(도매/소매)" Value="FM" GroupName="CustomerType" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoOthers" runat="server" Text="기타" Value="Others" GroupName="CustomerType" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoFarmer" runat="server" Text="농민/소비자" Value="Farmer" GroupName="CustomerType" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" Visible="false" >
                                </telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>농협/시판 Code/Name <span class="text_red">*</span></th>
                        <td colspan="3" >
                            <telerik:RadTextBox ID="radGrdTxtCustomer" runat="server" ReadOnly="true" AutoPostBack="false" Width="90%">
                            </telerik:RadTextBox>
                            <telerik:RadButton ID="radGrdBtnCustomer" runat="server" CssClass="btn_grid" AutoPostBack="false" Width="18px" Height="18px" OnClientClicked="fn_OpenCustomer">
                                <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                            </telerik:RadButton>
                            <div style="display:none;width:100%">
                                <asp:Label ID="Labelcustomertype" runat="server" ></asp:Label>
                            </div>
                        </td>
                    </tr>
 <%--                   <tr>
                        <th>농민/소비자 정보 (입력)<span class="text_red">*</span></th>
                        <td style="width:100px">
                            <telerik:RadTextBox ID="radTxtFarmerName" runat="server" TextMode="SingleLine" Height="20px" Width="100px" EmptyMessage="홍길동" ></telerik:RadTextBox>
                        </td>
                        <td style="width:150px">
                            <telerik:RadTextBox ID="radTxtFarmerTelNo" runat="server" TextMode="SingleLine" Height="20px" Width="150px" EmptyMessage="전화번호" ></telerik:RadTextBox>
                        </td>
                        <td style="width:360px">
                            <telerik:RadTextBox ID="radTxtFarmerAddress" runat="server" TextMode="SingleLine" Height="20px" Width="300px" EmptyMessage="주소" ></telerik:RadTextBox>
                        </td>
                    </tr>--%>
                    <tr>
                        <th>발생지역 (시도읍면동) <span class="text_red">*</span></th>
                        <td colspan="3" >
                             <telerik:RadTextBox ID="radTxtIssueArea" runat="server" TextMode="SingleLine" Height="20px" Width="100%" EmptyMessage="강원도 삼척시 원덕읍 월천리" ></telerik:RadTextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <h3>이슈의 접수 및 내용</h3>
        <div class="data_type1">
            <table>
                <tbody>
                    <tr>
                        <th style="width:250px">불만 최초 접수일 <span class="text_red">*</span></th>
                        <td style="width:200px">
                            <telerik:RadDatePicker ID="radDatIssueDate" runat="server" Width="100px" Calendar-ShowRowHeaders="false"
                                MinDate="2017-01-01" MaxDate="2050-12-31" Culture="ko-KR">
                                <DatePopupButton ImageUrl="../../Styles/images/ico_calendar.png" HoverImageUrl="../../Styles/images/ico_calendar.png" />
                                <Calendar runat="server" RangeMinDate="2017-01-01" RangeMaxDate="2050-12-31">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday"></telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                                <ClientEvents OnDateSelected="dateSelected" />
                            </telerik:RadDatePicker>
                        </td>
                        <th style="width:250px">경과일 <span class="text_red">*</span></th>
                        <td style="width:200px">
                            <telerik:RadTextBox ID="radTxtIssueExpiredPeriod" runat="server" TextMode="SingleLine" Height="20px" Width="200px" EmptyMessage="자동기록" ></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>불만접수 방법 <span class="text_red">*</span></th>
                        <td colspan="3" >
                            <div id="divIssueCollectigMethod" runat="server">
                                <telerik:RadButton ID="radRdoMail" runat="server" Text="메일 및 문자 접수" Value="Mail" GroupName="IssueCollectionMethod" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoTel" runat="server" Text="전화접수" Value="Tel" GroupName="IssueCollectionMethod" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio">
                                </telerik:RadButton>
                                <telerik:RadButton ID="radRdoRegularVist" runat="server" Text="정기적 거래처 순회 방문시" Value="RegularVisit" GroupName="IssueCollectionMethod" AutoPostBack="false" ButtonType="ToggleButton" ToggleType="Radio" >
                                </telerik:RadButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>불만접수 후 조치사항 <span class="text_red">*</span></th>
                        <td colspan="3" >
                             <telerik:RadTextBox ID="radTxtActionAfterReception" runat="server" TextMode="SingleLine" Height="20px" Width="100%" EmptyMessage="직접 입력" ></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th style="width:250px">접촉 대상자 <span class="text_red">*</span></th>
                        <td style="width:200px">
                            <telerik:RadTextBox ID="radTxtContact" runat="server" TextMode="SingleLine" Height="20px" Width="200px" EmptyMessage="직접 입력" ></telerik:RadTextBox>
                        </td>
                        <th style="width:250px">동행직원 <span class="text_red">*</span></th>
                        <td style="width:200px">
                            <telerik:RadTextBox ID="radTxtEmployee" runat="server" TextMode="SingleLine" Height="20px" Width="200px" EmptyMessage="동행직원"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>방문하여 조치한 내용 <span class="text_red">*</span></th>
                        <td colspan="3" >
                             <telerik:RadTextBox ID="radTxtActionAfterVisit" runat="server" TextMode="MultiLine" Height="40px" Width="100%" EmptyMessage="직접 입력" ></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <th rowspan="2">불만의 구체적인 내용 <span class="text_red">*</span></th>
                        <td colspan="3" >
                            <div id="divEffect" style="display: none">
                                <telerik:RadDropDownList ID="RadDropEffect" runat="server" Width="500" DefaultMessage="--- Select ---" AutoPostBack="false">
                                    <Items>
                                    <telerik:DropDownListItem Text="01. 약효미흡" Value="E_01" /> 
                                    <telerik:DropDownListItem Text="02. 약해" Value="E_02" /> 
                                    <telerik:DropDownListItem Text="03. 기타" Value="E_03" /> 
                                    </Items>
                                </telerik:RadDropDownList>
                            </div>
                            <div id="divQuality" style="display: none">
                                <telerik:RadDropDownList ID="RadDropQuality" runat="server" Width="500" DefaultMessage="--- Select ---" AutoPostBack="false">
                                    <Items>
                                    <telerik:DropDownListItem Text="01. 액상제품 누액" Value="Q_01" /> 
                                    <telerik:DropDownListItem Text="02. 입제/수화제 누분(파봉)" Value="Q_02" /> 
                                    <telerik:DropDownListItem Text="03. 모집단/유효기간 에러" Value="Q_03" /> 
                                    <telerik:DropDownListItem Text="04. 용중량 및 수량 부족(빈박스)" Value="Q_04" /> 
                                    <telerik:DropDownListItem Text="05. 무거리/불순물(impurity) 혼입" Value="Q_05" /> 
                                    <telerik:DropDownListItem Text="06. 교차 오염 (Cross contamination)" Value="Q_06" /> 
                                    <telerik:DropDownListItem Text="07. 가스발생 / 부풀어오름 / 핀홀없음" Value="Q_07" /> 
                                    <telerik:DropDownListItem Text="08. 라벨 없음" Value="Q_08" /> 
                                    <telerik:DropDownListItem Text="09. 라벨디자인 실패 (사용설명서 오류,오타, 바코드인식, 용량 식별불능)" Value="Q_09" /> 
                                    <telerik:DropDownListItem Text="10. 라벨 롤의 풀림" Value="Q_10" /> 
                                    <telerik:DropDownListItem Text="11. 라벨,포장지 낙서" Value="Q_11" /> 
                                    <telerik:DropDownListItem Text="12. 병의 수축 / 포장재질의 불량" Value="Q_12" /> 
                                    <telerik:DropDownListItem Text="13. 입제의 분진 과다" Value="Q_13" /> 
                                    <telerik:DropDownListItem Text="14. 유효성분의 분해" Value="Q_14" /> 
                                    <telerik:DropDownListItem Text="15. 제품의 물리성 불량 (확산성, 분산성, 수화성, 현수성 등)" Value="Q_15" /> 
                                    <telerik:DropDownListItem Text="16. 외관불량 (침전,침착, Caking 및 결정화(Crystalization))" Value="Q_16" /> 
                                    <telerik:DropDownListItem Text="17. 제품의 수축(미생물제)" Value="Q_17" /> 
                                    <telerik:DropDownListItem Text="18. 기타" Value="Q_18" /> 
                                    </Items>
                                </telerik:RadDropDownList>
                            </div>
                            <div id="divDelivery" style="display: none">
                                <telerik:RadDropDownList ID="RadDropDelivery" runat="server" Width="500" DefaultMessage="--- Select ---" AutoPostBack="false" >
                                    <Items>
                                    <telerik:DropDownListItem Text="01. 적재불량" Value="D_01" /> 
                                    <telerik:DropDownListItem Text="02. 운송중 파손/손상" Value="D_02" /> 
                                    <telerik:DropDownListItem Text="03. 배송지연" Value="D_03" /> 
                                    <telerik:DropDownListItem Text="04. 배송지 오류" Value="D_04" /> 
                                    <telerik:DropDownListItem Text="05. 기타" Value="D_05" /> 
                                    </Items>
                                </telerik:RadDropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th style="width:50px">Remarks <span class="text_red">*</span></th>
                        <td colspan="2" >
                             <telerik:RadTextBox ID="radTxtRemark" runat="server" TextMode="SingleLine" Height="20px" Width="100%" EmptyMessage="직접 입력" ></telerik:RadTextBox>
                        </td>
                    </tr>

                </tbody>
            </table>
        </div>

        <h3>고객불만 보고서 (B) </h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col />
                    <col style="width: 75%;" />
                </colgroup>
                <tbody>
                    <tr><th colspan="2">본 조사서는 당사의 제품을 사용한 고객이 약효미흡 및 약해에 대해 민원을 제기한 경우 작성하여 보고한다.<br />
                            본 조사는 고객불만 접수 5일 이내에 작성할 것을 권고한다. </th></tr>
                    <tr><td colspan="2" style="text-align:center">약효/약해 관련 민원은 고객불만보고서 (A)와 (B)를 모두 작성하여 보고 합니다. </td> </tr>
                    <tr>
                        <th><a id="lnkBCS-COM-006" href="http://sp-coll-bbs.ap.bayer.cnb/sites/000141/Teamsites/NEW%20e-workflow/LINKED_DATA/%EA%B3%A0%EA%B0%9D%EB%B6%88%EB%A7%8C%EB%B3%B4%EA%B3%A0%EC%84%9C_B.xlsx?Web=1" target="_blank">고객불만보고서 양식(B)</a> </th>
                        <td>
                            <a id="lnkBCS-COM-006" href="http://sp-coll-bbs.ap.bayer.cnb/sites/000141/Teamsites/NEW%20e-workflow/LINKED_DATA/%EA%B3%A0%EA%B0%9D%EB%B6%88%EB%A7%8C%EB%B3%B4%EA%B3%A0%EC%84%9C_B.xlsx?Web=1" target="_blank">고객불만보고서 양식(B)</a> - SharePoint에서 다운받아 작성하고 별도첨부
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="radWinPopupCurProduct" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Product" Width="550px" Height="500px" Behaviors="Default" NavigateUrl="./ProductList.aspx" OnClientClose="fn_setProduct"></telerik:RadWindow>
        </Windows>
         <Windows>
            <telerik:RadWindow ID="radWinPopupCustomer" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Customer" Width="550px" Height="500px" Behaviors="Default" NavigateUrl="./customerforcs.aspx" OnClientClose="fn_setCustomer"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div id="hiddenArea" runat="server">
        <input type="hidden" id="hddProcessID" runat="server" />
        <input type="hidden" id="hddProcessStatus" runat="server" />
        <input type="hidden" id="hddDocumentID" runat="server" />
        <input type="hidden" id="hddGridItems" runat="server" />
        <input type="hidden" id="hddCurProductCode" runat="server" />
        <input type="hidden" id="hddRepProductCode" runat="server" />
        <input type="hidden" id="hddCustomerCode" runat="server" />
        <input type="hidden" id="hddCustomerType" runat="server" />
        <input type="hidden" id="hddReuse" runat="server" />
        <input type="hidden" id="hddAddRow" runat="server" value="N" />
    </div>
</asp:Content>

