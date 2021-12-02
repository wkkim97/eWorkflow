<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="Configuration.aspx.cs" Inherits="Configuration_Configuration" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="server">
    <style type="text/css">
        .uppercase {
            text-transform: uppercase;
        }
    </style>
    <script type="text/javascript">

        function isEmpty(str) {
            return (!str || 0 === str.length);
        }

        function capitaliseFirstLetter(value) {
            return value.charAt(0).toUpperCase() + value.slice(1).toLowerCase();
        }

        function checkValidation() {

            var isValid = true;
            var emptyMessage = "";

            //문서명
            var inputValue = $find('<%= radTxtDocName.ClientID %>').get_element().value;
            if (isEmpty(inputValue))
                emptyMessage = "Document Name \r\n";

            //Table Name
            inputValue = $find('<%= radTxtTableName.ClientID %>').get_value();
            if (isEmpty(inputValue))
                emptyMessage = emptyMessage + "Table Name \r\n";

            //회사선택여부
            var companies = $('#<%= divCompany.ClientID %>').children();
            var checked = false;
            for (var i = 0; i < companies.length; i++) {
                var chkCompany = companies[i];
                if ($find(chkCompany.id).get_checked()) { checked = true; break; }
            }
            if (!checked) emptyMessage = emptyMessage + "Company \r\n"

            //Prefix
            inputValue = $find('<%= radTxtPrefix.ClientID %>').get_element().value;
            if (isEmpty(inputValue))
                emptyMessage = emptyMessage + "Prefix Of Doc.\r\n";

            //Approval Level선택시
            if ($find('<%= radBtnApprovalLevel.ClientID%>').get_checked()) {
                inputValue = $find('<%= radTxtApprovalLevel.ClientID%>').get_value();
                //if (isEmpty(inputValue))
                //    emptyMessage = emptyMessage + "Approval Level \r\n";
            }

            //Job Title선택시
            if ($find('<%= radBtnJobTitle.ClientID%>').get_checked()) {
                var titles = $('#<%= divJobTitle.ClientID %>').children();
                
                checked = false;
                for (var i = 0; i < titles.length; i++) {
                    var title = titles[i];
                    if ($find(title.id).get_checked()) { checked = true; break; }
                }
                if (!checked) emptyMessage = emptyMessage + "Job Title \r\n";
            }


            if (isEmpty(emptyMessage)) return true;
            else {
                fn_OpenInformation(emptyMessage);
                return false;
            }
        }

        function fn_OnNewClicked(sender, args) {

            $('#<%= hddDocumentId.ClientID %>').val('');
            $('#<%= hddConditionIdx.ClientID %>').val('');
            $('#<%= hddConditionList.ClientID %>').val('');

            $find('<%= radTxtDocName.ClientID %>').clear();
            $find('<%= radTxtDataOwner.ClientID %>').clear();
            $find('<%= radTxtTableName.ClientID %>').clear();
            $find('<%= radTxtFormName.ClientID %>').clear();

            var companies = $('#<%= divCompany.ClientID %>').children();
            for (var i = 0; i < companies.length; i++) {
                var chkCompany = companies[i];
                $find(chkCompany.id).set_checked(false);
            }

            $find('<%= radCmbCategory.ClientID %>').get_items().getItem(0).select();
            //Readers group 추가----------------------------------
            $find('<%= radTxtPrefix.ClientID %>').clear();
            $find('<%= radCmbInfoClassification.ClientID %>').get_items().getItem(0).select();
            $find('<%= radCmbRetentionPeriod.ClientID %>').get_items().getItem(0).select();
            $find('<%= radTxtDescription.ClientID %>').clear();
            $find('<%= radBtnForwardYes.ClientID %>').set_checked(true);
            $find('<%= radBtnAddApproverNo.ClientID %>').set_checked(true);
            $find('<%= radBtnShowDocumentListYes.ClientID %>').set_checked(true);
            $find('<%= radBtnAddReviewerYes.ClientID %>').set_checked(true);
            $find('<%= radTxtReviewerDescription.ClientID %>').clear();
            $find('<%= radBtnApprovalLevel.ClientID %>').set_checked(true);
            $find('<%= radTxtApprovalLevel.ClientID %>').set_value(1);
            $find('<%= radDdlApprovalType.ClientID %>').get_items().getItem(0).select();
            $find('<%= radTxtUserId.ClientID %>').clear();
            $find('<%= radBtnIsMandatory.ClientID %>').set_checked(false);
            $find('<%= radBtnAddCondition.ClientID %>').set_enabled(false);
            $find('<%= radBtnSaveCondition.ClientID %>').set_enabled(false);

            $find('<%=radBtnAddCondition.ClientID %>').set_enabled(false);
            $find('<%=radBtnSaveCondition.ClientID %>').set_enabled(false);

            var viewBeforeApprover = $find('<%= radGrdBeforeApprover.ClientID %>').get_masterTableView();
            viewBeforeApprover.set_dataSource([]);
            viewBeforeApprover.dataBind();

            var viewAfterApprover = $find('<%= radGrdAfterApprover.ClientID %>').get_masterTableView();
            viewAfterApprover.set_dataSource([]);
            viewAfterApprover.dataBind();

            var viewRecipient = $find('<%= radGrdRecipient.ClientID %>').get_masterTableView();
            viewRecipient.set_dataSource([]);
            viewRecipient.dataBind();

            var viewReviewer = $find('<%= radGrdReviewer.ClientID %>').get_masterTableView();
            viewReviewer.set_dataSource([]);
            viewReviewer.dataBind();

        }

        function Encode() {
            var value = $find('<%= radTxtDescription.ClientID%>').get_value();
            value = value.replace('<', "&lt;");
            value = value.replace('>', "&gt;");
            $find('<%= radTxtDescription.ClientID%>').set_value(value);
        }

        function fn_OnSaveClicked(sender, args) {
            var valid = checkValidation();
            Encode();
            if (valid)
                sender.set_autoPostBack(true);
            else
                sender.set_autoPostBack(false);

        }

        function confirmCallBack(args) {
            fn_OpenInformation(args);
        }

        function fn_OnDeleteClicked(sender, args) {

        }

        /*
            JobTitle 항목을 Enable/Disable한다.
        */
        function SetEnableJobTitle(enable) {
            var titles = $('#<%= divJobTitle.ClientID%>').children();
            for (var i = 0; i < titles.length; i++) {
                var title = titles[i];
                $find(title.id).set_enabled(enable);
            }
        }

        function fn_OnApprovalLevelClicked(sender, args) {
            $find('<%= radTxtApprovalLevel.ClientID %>').enable();
            SetEnableJobTitle(false);

        }

        function fn_OnJobTitleClicked(sender, args) {
            $find("<%= radTxtApprovalLevel.ClientID %>").disable();
            SetEnableJobTitle(true);
        }

        function fn_TableNameBlur(sender, args) {
            var txtFormName = $find('<%= radTxtFormName.ClientID %>');
            if (isEmpty(txtFormName.get_textBoxValue())) {
                var txtTableName = $find('<%= radTxtTableName.ClientID %>');
                var strFormName = txtTableName.get_textBoxValue();
                strFormName = txtTableName.get_textBoxValue().replace('TB_DOC_', '');
                var arrString = strFormName.split('_');
                strFormName = "";
                for (var i = 0; i < arrString.length; i++)
                    strFormName = strFormName + capitaliseFirstLetter(arrString[i]);

                txtFormName.set_value(strFormName);
            }
        }

        function fn_OnAddConditionClicked(sender, args) {

            var documentId = $('#<%= hddDocumentId.ClientID%>').val();
            if (isEmpty(documentId)) {
                fn_OpenInformation("결재문서를 조회 후 등록바랍니다.")
                sender.set_autoPostBack(false);
            } else {
                var ddlCondition = $find('<%= radDdlCondition.ClientID %>');
                var condition = ddlCondition.get_selectedItem().get_value();
                if (condition != "IsNull" && condition != "IsNotNull") {
                    var value = $find('<%= radTxtValue.ClientID %>').get_element().value;
                    if (isEmpty(value)) {
                        fn_OpenInformation('Value를 입력 바랍니다.');
                        sender.set_autoPostBack(false); return false;
                    }
                }
                SetApprovalLine();
                sender.set_autoPostBack(true);
            }
        }

        function SetApprovalLine() {
            var list = [];

            if ($find('<%= radBtnIsMandatory.ClientID%>').get_checked()) {
                var conObj = {
                    APPROVAL_LOCATION: null,
                    IS_MANDATORY: null,
                    CONDITION_SEQ: null,
                    FIELD_NAME: null,
                    CONDITION: null,
                    VALUE: null,
                    OPTION: null,
                }
                conObj.CONDITION_SEQ = 1;
                conObj.APPROVAL_LOCATION = $find('<%= radDdlApprovalType.ClientID%>').get_selectedItem().get_value();
                conObj.IS_MANDATORY = true;
                conObj.FIELD_NAME = "";
                conObj.CONDITION = "";
                conObj.VALUE = "";
                conObj.OPTION = "";
                list.push(conObj);

            } else {
                var grdCondition = $find('<%= radGrdCondition.ClientID %>');
                var masterTable = grdCondition.get_masterTableView();
                grdCondition.get_batchEditingManager().saveChanges(masterTable);
                var dataItems = masterTable.get_dataItems();
                for (var i = 0; i < dataItems.length; i++) {

                    var seq = dataItems[i].get_cell("CONDITION_SEQ").innerText;
                    var filedName = dataItems[i].get_cell("FIELD_NAME").children[0].innerText;
                    var condition = dataItems[i].get_cell("CONDITION").children[0].innerText;
                    var value = dataItems[i].get_cell("VALUE").children[0].innerText;
                    var option = dataItems[i].get_cell("OPTION").children[0].innerText;
                    var conObj = {
                        APPROVAL_LOCATION: null,
                        IS_MANDATORY: null,
                        CONDITION_SEQ: null,
                        FIELD_NAME: null,
                        CONDITION: null,
                        VALUE: null,
                        OPTION: null,
                    }
                    conObj.APPROVAL_LOCATION = $find('<%= radDdlApprovalType.ClientID%>').get_selectedItem().get_value();
                    conObj.CONDITION_SEQ = seq;
                    conObj.IS_MANDATORY = false;
                    conObj.FIELD_NAME = filedName;
                    conObj.CONDITION = condition;
                    conObj.VALUE = value;
                    conObj.OPTION = option;

                    list.push(conObj);
                }

            }
            $('#<%= hddConditionList.ClientID%>').val(JSON.stringify(list));
            return list.length;
        }
        function fn_OnNewConditionClicked(sender, args) {
            $find('<%= radTxtUserId.ClientID %>').clear();
            $find('<%= radTxtValue.ClientID %>').clear();
            $('#<%= hddConditionIdx.ClientID %>').val('');
            $('#<%= hddConditionList.ClientID %>').val('');

            $find('<%= radGrdCondition.ClientID %>').get_masterTableView().set_dataSource([]);
            $find('<%= radGrdCondition.ClientID %>').get_masterTableView().dataBind();
            //선택초기화

            $find('<%= radGrdBeforeApprover.ClientID %>').clearSelectedItems();
            $find('<%= radGrdAfterApprover.ClientID %>').clearSelectedItems();
            $find('<%= radGrdRecipient.ClientID %>').clearSelectedItems();
            $find('<%= radGrdReviewer.ClientID %>').clearSelectedItems();
        }

        function fn_OnSaveConditionClicking(sender, args) {

            var userId = $find("<%= radTxtUserId.ClientID %>").get_element().value;

            if (isEmpty(userId)) {

                fn_OpenInformation('사용자를 입력바랍니다.');
                args.set_cancel(true);
            } else {
                var iConditions = SetApprovalLine();
                if (!$find('<%= radBtnIsMandatory.ClientID%>').get_checked() && iConditions < 1) {
                    fn_OpenInformation('Condition을 입력바랍니다.')
                    args.set_cancel(true);
                }
            }
            
        }

        function fn_OnConditionSelectedIndexChanged(sender, args) {
            var selectedValue = sender.get_selectedItem().get_value();
            if (selectedValue == "IsNull" || selectedValue == "IsNotNull") {
                $find('<%= radTxtValue.ClientID %>').disable();
                $find('<%= radTxtValue.ClientID %>').clear();
            } else {
                $find('<%= radTxtValue.ClientID %>').enable();
            }
        }

        var clickedKey = null;
        var clickedGrid = null;
        function openConfirmPopUp(index, type) {
           
            clickedKey = parseInt(index);
            clickedGrid = type;
            fn_OpenConfirm('Do you want to delete this Item ?', confirmCallBackFn);

            return false;

        }

<%--        function fn_OnIsMandatoryCheckedChanged(sender, args) {
            $find('<%= radDdlFieldName.ClientID %>').set_enabled(!args.get_checked());
            $find('<%= radDdlCondition.ClientID %>').set_enabled(!args.get_checked());
            $find('<%= radTxtValue.ClientID %>').set_enabled(!args.get_checked());
            $find('<%= radDdlOption.ClientID %>').set_enabled(!args.get_checked());
            $find('<%= radBtnAddCondition.ClientID %>').set_enabled(!args.get_checked());

            if (args.get_checked()) {
                $find('<%= radGrdCondition.ClientID %>').get_masterTableView().set_dataSource([]);
                $find('<%= radGrdCondition.ClientID %>').get_masterTableView().dataBind();
            }

            
        }--%>

        function confirmCallBackFn(arg) {
            if (arg) {
                if (clickedGrid == 0)
                    $find('<%= radGrdCondition.ClientID %>').get_masterTableView().fireCommand("Remove", clickedKey);
                else if (clickedGrid == 1)
                    $find('<%= radGrdBeforeApprover.ClientID %>').get_masterTableView().fireCommand("Remove", clickedKey);
                else if (clickedGrid == 2)
                    $find('<%= radGrdAfterApprover.ClientID %>').get_masterTableView().fireCommand("Remove", clickedKey);
                else if (clickedGrid == 3)
                    $find('<%= radGrdRecipient.ClientID %>').get_masterTableView().fireCommand("Remove", clickedKey);
                else if (clickedGrid == 4)
                    $find('<%= radGrdReviewer.ClientID %>').get_masterTableView().fireCommand("Remove", clickedKey);
}
}
function fn_OpenUser(sender, Args) {
    var wnd = $find("<%= radWinUser.ClientID %>");
    if (sender.get_value() == 'DataOwner')
        wnd.set_title('DataOwner');
    else
        wnd.set_title('Approver');
    wnd.setUrl("/eWorks/Common/Popup/UserList.aspx");
    wnd.show();
    sender.set_autoPostBack(false);
}

function OnClientClose_new(sender, args) {
    
    var uListData = args.get_argument();
    if (uListData != null) {
        if (uListData.length > 0) {
            
            var user = uListData[0];
            if (sender.get_title().toLowerCase() == 'dataowner') {
                $find('<%= radTxtDataOwner.ClientID %>').set_value(user.FULL_NAME);
                $('#<%= hddDataOwnerCode.ClientID %>').val(user.USER_ID);
            } else {
                $find('<%= radTxtUserId.ClientID %>').set_value(user.FULL_NAME);
                $('#<%= hddApprovalUserCode.ClientID %>').val(user.USER_ID);
            }
        }
    }
}
        function fn_OnAddApproverChecked(sender, args) {
            if (sender.get_value() == 'Y') {
                if (args.get_checked()) $find('<%= radDdlAddApproverPosition.ClientID %>').set_enabled(true);
            } else {
                if (args.get_checked()) $find('<%= radDdlAddApproverPosition.ClientID %>').set_enabled(false);
            }
        }

        function ShowInformation(text) {
            alert(text);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="server">
    
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radGrdDocList">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divHidden" />
                    <telerik:AjaxUpdatedControl ControlID="divRight" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radGrdBeforeApprover">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divHidden" />
                    <telerik:AjaxUpdatedControl ControlID="divCondition" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdBeforeApprover" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdAfterApprover" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdRecipient" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdReviewer" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radGrdAfterApprover">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divHidden" />
                    <telerik:AjaxUpdatedControl ControlID="divCondition" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdBeforeApprover" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdAfterApprover" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdRecipient" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdReviewer" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radGrdRecipient">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divHidden" />
                    <telerik:AjaxUpdatedControl ControlID="divCondition" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdBeforeApprover" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdAfterApprover" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdRecipient" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdReviewer" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radGrdReviewer">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divHidden" />
                    <telerik:AjaxUpdatedControl ControlID="divCondition" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdBeforeApprover" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdAfterApprover" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdRecipient" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdReviewer" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radBtnSaveCondition">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divHidden" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdBeforeApprover" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdAfterApprover" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdRecipient" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdReviewer" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radBtnAddCondition">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radGrdCondition" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radGrdCondition">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divHidden" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdBeforeApprover" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdAfterApprover" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdRecipient" />
                    <telerik:AjaxUpdatedControl ControlID="radGrdReviewer" />             
                    
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    
    <telerik:RadSplitter runat="server" ID="radSpliterMain" Width="100%" Height="800px" VisibleDuringInit="false">
        <telerik:RadPane runat="server" ID="radLeftPane" Width="300px" Scrolling="None">
            <telerik:RadGrid ID="radGrdDocList" AutoGenerateColumns="false" runat="server" HeaderStyle-HorizontalAlign="Center"
                Width="100%" Height="100%" 
                Skin="EXGrid"
                OnSelectedIndexChanged="radGrdDocList_SelectedIndexChanged" OnNeedDataSource="radGrdDocList_NeedDataSource">
                <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                </ClientSettings>
                <MasterTableView ShowHeadersWhenNoRecords="true" ItemStyle-Wrap="false">
                    <Columns>
                        <telerik:GridBoundColumn DataField="DOCUMENT_ID" HeaderText="Document" UniqueName="DOCUMENT_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DOC_NAME" HeaderText="Document" UniqueName="DOC_NAME" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="false"></telerik:GridBoundColumn>
                    </Columns>

                </MasterTableView>

            </telerik:RadGrid>
        </telerik:RadPane>
        <telerik:RadSplitBar ID="Radsplitbar1" runat="server">
        </telerik:RadSplitBar>
        <telerik:RadPane runat="server" ID="radRightPane">
            <div id="doc_content">
                <div class="doc_style">
                    <div class="align_right pb10" style="margin-top: 5px">
                        <telerik:RadButton runat="server" ID="btnNew" Text="New Configuration" OnClientClicked="fn_OnNewClicked" AutoPostBack="false"
                            ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size1 bold">
                        </telerik:RadButton>
                        <telerik:RadButton runat="server" ID="btnSave" Text="Save" OnClientClicked="fn_OnSaveClicked" OnClick="btnSave_Click"
                            ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size1 bold">
                        </telerik:RadButton>
                        <telerik:RadButton runat="server" ID="btnDelete" Text="Delete" OnClientClicked="fn_OnDeleteClicked" OnClick="btnDelete_Click"
                            ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size1 bold">
                        </telerik:RadButton>
                    </div>
                </div>
                <div id="divRight" runat="server" style="width: 99%; vertical-align: middle; margin-left: 5px">
                    <div class="doc_style">

                        <h4>General Information</h4>
                        <div class="config_type1">
                            <table>
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                    <col style="width: 25%" />
                                    <col style="width: 25%" />
                                </colgroup>
                                <tr>
                                    <th>Document Name</th>
                                    <td>
                                        <telerik:RadTextBox runat="server" ID="radTxtDocName" Width="100%"></telerik:RadTextBox>
                                    </td>
                                    <th>Data Owner</th>
                                    <td>
                                        <telerik:RadTextBox runat="server" ID="radTxtDataOwner" Width="90%" ReadOnly="true"></telerik:RadTextBox>
                                        <telerik:RadButton ID="radBtnDataOwner" runat="server" AutoPostBack="false" Width="18px" Height="18px" Value="DataOwner"
                                            CssClass="btn_grid" OnClientClicked="fn_OpenUser">
                                            <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                                        </telerik:RadButton>
                                        <input type="hidden" id="hddDataOwnerCode" runat="server" />

                                    </td>
                                </tr>
                                <tr>
                                    <th>TableName</th>
                                    <td>
                                        <telerik:RadTextBox runat="server" ID="radTxtTableName" Width="100%" CssClass="uppercase">
                                            <ClientEvents OnBlur="fn_TableNameBlur" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <th>Form Name</th>
                                    <td>
                                        <telerik:RadTextBox runat="server" ID="radTxtFormName" Width="80%"></telerik:RadTextBox>.aspx
                                    </td>
                                </tr>
                                <tr>
                                    <th>Company</th>
                                    <td>
                                        <div id="divCompany" runat="server"></div>
                                    </td>
                                    <th>Category</th>
                                    <td>
                                        <telerik:RadDropDownList ID="radCmbCategory" runat="server">
                                        </telerik:RadDropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Readers Group</th>
                                    <td>
                                        <telerik:RadComboBox ID="radCmbReadersGroup" runat="server">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="" Value="" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                    <th>Prefix of Doc.</th>
                                    <td>
                                        <telerik:RadTextBox ID="radTxtPrefix" runat="server"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Information Classification</th>
                                    <td>
                                        <telerik:RadDropDownList ID="radCmbInfoClassification" runat="server"></telerik:RadDropDownList>
                                    </td>
                                    <th>Retention Period</th>
                                    <td>
                                        <telerik:RadDropDownList ID="radCmbRetentionPeriod" runat="server">
                                        </telerik:RadDropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Service Name</th>
                                    <td colspan="3">
                                        <telerik:RadTextBox ID="radTxtServiceName" runat="server" Width="100%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr style="height: 60px">
                                    <th>Document Description</th>
                                    <td colspan="3">
                                        <telerik:RadTextBox ID="radTxtDescription" runat="server" TextMode="MultiLine" Width="100%" Height="55px"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="config_type1">
                            <h4>Option</h4>
                            <table>
                                <colgroup>
                                    <col style="width: 16%" />
                                    <col />
                                    <col style="width: 16%" />
                                    <col style="width: 16%" />
                                    <col style="width: 16%" />
                                    <col style="width: 16%" />
                                </colgroup>
                                <tr>
                                    <th>Forward</th>
                                    <td>
                                        <telerik:RadButton ID="radBtnForwardYes" runat="server" ButtonType="ToggleButton" ToggleType="Radio" Text="Yes" Value="Y" GroupName="Forward" Checked="true" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton ID="radBtnForwardNo" runat="server" ButtonType="ToggleButton" ToggleType="Radio" Text="No" Value="N" GroupName="Forward" AutoPostBack="false"></telerik:RadButton>
                                    </td>
                                    <th>Add Approver</th>
                                    <td>
                                        <telerik:RadButton ID="radBtnAddApproverYes" runat="server" ButtonType="ToggleButton" ToggleType="Radio" Text="Yes" Value="Y" GroupName="AddApprover" AutoPostBack="false" OnClientCheckedChanged="fn_OnAddApproverChecked"></telerik:RadButton>
                                        <telerik:RadButton ID="radBtnAddApproverNo" runat="server" ButtonType="ToggleButton" ToggleType="Radio" Text="No" Value="N" GroupName="AddApprover" Checked="true" AutoPostBack="false" OnClientCheckedChanged="fn_OnAddApproverChecked"></telerik:RadButton>
                                        <telerik:RadDropDownList ID="radDdlAddApproverPosition" runat="server" Width="150px" Enabled="false">
                                            <Items>
                                                <telerik:DropDownListItem Text="After Request" Value="0001"/>
                                                <telerik:DropDownListItem Text="After ApprovalLine" Value="0002" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                    </td>
                                    <th>Show Document List</th>
                                    <td>
                                        <telerik:RadButton ID="radBtnShowDocumentListYes" runat="server" ButtonType="ToggleButton" ToggleType="Radio" Text="Yes" GroupName="ShowDocumentList" Checked="true" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton ID="radBtnShowDocumentListNo" runat="server" ButtonType="ToggleButton" ToggleType="Radio" Text="No" GroupName="ShowDocumentList" AutoPostBack="false"></telerik:RadButton>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Add Reviewer</th>
                                    <td>
                                        <telerik:RadButton ID="radBtnAddReviewerYes" runat="server" ButtonType="ToggleButton" ToggleType="Radio" Text="Yes" GroupName="AddReviewer" Checked="true" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton ID="radBtnAddReviewerNo" runat="server" ButtonType="ToggleButton" ToggleType="Radio" Text="No" GroupName="AddReviewer" AutoPostBack="false"></telerik:RadButton>

                                    </td>
                                    <th>Reviewer Description</th>
                                    <td colspan="3">
                                        <telerik:RadTextBox ID="radTxtReviewerDescription" runat="server" Width="100%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="config_type1">
                            <h4>Approval Line</h4>
                            <table>
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <th>Default Line
                                    </th>
                                    <td>
                                        <table style="padding: 0 0 0 0; margin: 0 0 0 0">
                                            <tr>
                                                <td>
                                                    <telerik:RadButton runat="server" ID="radBtnApprovalLevel" ButtonType="ToggleButton" ToggleType="Radio"
                                                        Text="Approval Level" GroupName="ApprovalType" Checked="true" AutoPostBack="false" Value="0001"
                                                        OnClientClicked="fn_OnApprovalLevelClicked">
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadNumericTextBox runat="server" ID="radTxtApprovalLevel" Type="Number" NumberFormat-DecimalDigits="0" MinValue="0" MaxValue="10" Value="1"></telerik:RadNumericTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadButton runat="server" ID="radBtnJobTitle" ButtonType="ToggleButton" ToggleType="Radio"
                                                        Text="Job Title" GroupName="ApprovalType" AutoPostBack="false" Value="0002"
                                                        OnClientClicked="fn_OnJobTitleClicked">
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <div id="divJobTitle" runat="server"></div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadButton runat="server" ID="radBtnLimitAmount" ButtonType="ToggleButton" ToggleType="Radio"
                                                        Text="Limit Amount" GroupName="ApprovalType" AutoPostBack="false" Value="0003">
                                                    </telerik:RadButton>
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="config_type1">
                            <h4>Additional Approver/Recipient/Reviewer</h4>
                            <div id="divCondition" runat="server">
                                <table>
                                    <colgroup>
                                        <col style="width: 25%" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <th>Approval Type
                                        </th>
                                        <td>
                                            <telerik:RadDropDownList ID="radDdlApprovalType" runat="server" OnSelectedIndexChanged="radDdlApprovalType_SelectedIndexChanged" AutoPostBack="true">
                                                <Items>
                                                    <telerik:DropDownListItem Text="Before Approver" Value="B" />
                                                    <telerik:DropDownListItem Text="After Approver" Value="A" />
                                                    <telerik:DropDownListItem Text="Recipient" Value="R" />
                                                    <telerik:DropDownListItem Text="Reviewer" Value="V" />
                                                </Items>
                                            </telerik:RadDropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>User
                                        </th>
                                        <td>
                                            <telerik:RadTextBox ID="radTxtUserId" runat="server" Width="200px" ReadOnly="true"></telerik:RadTextBox>
                                            <telerik:RadButton ID="RadButton1" runat="server" AutoPostBack="false" Width="18px" Height="18px" Value="ApprovalUser"
                                                CssClass="btn_grid" OnClientClicked="fn_OpenUser">
                                                <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
                                            </telerik:RadButton>
                                            <input type="hidden" id="hddApprovalUserCode" runat="server" />

                                            <telerik:RadButton ID="radBtnIsMandatory" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Mandatory"
                                                OnCheckedChanged="radBtnIsMandatory_CheckedChanged">
                                            </telerik:RadButton>
                                            &nbsp;
                                            <telerik:RadButton ID="radBtnNewCondition" runat="server" Text="New" OnClientClicked="fn_OnNewConditionClicked" OnClick="radBtnNewCondition_Click"
                                                ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-green btn-size1 bold">
                                            </telerik:RadButton>
                                            <telerik:RadButton ID="radBtnSaveCondition" runat="server" Text="Save" 
                                                OnClientClicking="fn_OnSaveConditionClicking" 
                                                ButtonType="LinkButton" OnClick="radBtnSaveCondition_Click" 
                                                EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-green btn-size1 bold">
                                            </telerik:RadButton>

                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Condition 
                                        </th>
                                        <td>
                                            <table style="border: none; padding: 0 0 0 0">
                                                <tr style="border: none; padding: 0 0 0 0">
                                                    <td style="width: 50%; border: none; padding: 0 0 0 10px">
                                                        <telerik:RadDropDownList ID="radDdlFieldName" runat="server" Width="100%"></telerik:RadDropDownList>
                                                    </td>
                                                    <td style="width: 150px; border: none; padding: 0 0 0 0">
                                                        <telerik:RadDropDownList runat="server" ID="radDdlCondition" Width="100%" OnClientSelectedIndexChanged="fn_OnConditionSelectedIndexChanged">
                                                            <Items>
                                                                <telerik:DropDownListItem Text="" Value="" />
                                                                <telerik:DropDownListItem Text="Equals" Value="Equals" />
                                                                <telerik:DropDownListItem Text="GreaterThan" Value="GreaterThan" />
                                                                <telerik:DropDownListItem Text="LessThan" Value="LessThan" />
                                                                <telerik:DropDownListItem Text="StartWith" Value="StartWith" />
                                                                <telerik:DropDownListItem Text="NotStartWith" Value="NotStartWith" />
                                                                <telerik:DropDownListItem Text="Include" Value="Include" />
                                                                <telerik:DropDownListItem Text="NotInclude" Value="NotInclude" />
                                                                <telerik:DropDownListItem Text="IsNull" Value="IsNull" />
                                                                <telerik:DropDownListItem Text="IsNotNull" Value="IsNotNull" />
                                                            </Items>
                                                        </telerik:RadDropDownList>
                                                    </td>
                                                    <td style="width: 50%; border: none; padding: 0 0 0 0">
                                                        <telerik:RadTextBox runat="server" ID="radTxtValue" Width="100%"></telerik:RadTextBox>
                                                    </td>
                                                    <td style="width: 100px; border: none; padding: 0 0 0 0">
                                                        <telerik:RadDropDownList runat="server" ID="radDdlOption">
                                                            <Items>
                                                                <telerik:DropDownListItem Text="And" Value="And" />
                                                                <telerik:DropDownListItem Text="Or" Value="Or" Selected="true" />
                                                            </Items>
                                                        </telerik:RadDropDownList>
                                                    </td>
                                                    <td style="width: 40px; border: none; padding: 0 10px 0 3px">
                                                        <telerik:RadButton runat="server" ID="radBtnAddCondition" Text="+" OnClientClicked="fn_OnAddConditionClicked" OnClick="radBtnAddCondition_Click"
                                                            Enabled="false" Width="40px" Height="12px"
                                                            ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-red btn-size1 bold">
                                                        </telerik:RadButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5" style="border: none; margin-bottom: 3px">
                                                        <telerik:RadGrid ID="radGrdCondition" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center"
                                                            OnPreRender="radGrdCondition_PreRender" OnItemCommand="radGrdCondition_ItemCommand" OnNeedDataSource="radGrdCondition_NeedDataSource">
                                                            <MasterTableView EditMode="Batch" InsertItemDisplay="Bottom" ClientDataKeyNames="CONDITION_SEQ">
                                                                <BatchEditingSettings EditType="Row" OpenEditingEvent="Click" />
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="CONDITION_SEQ" HeaderText="Seq" HeaderStyle-Width="40px" UniqueName="CONDITION_SEQ" ReadOnly="true">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="FIELD_NAME" HeaderText="Field" HeaderStyle-Width="50%">
                                                                        <ItemTemplate>
                                                                            <%#DataBinder.Eval(Container.DataItem, "FIELD_NAME")%>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadDropDownList ID="radGrdDdlFieldName" runat="server" Width="100%"></telerik:RadDropDownList>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="CONDITION" HeaderText="Condition" HeaderStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <%#DataBinder.Eval(Container.DataItem, "CONDITION")%>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadDropDownList ID="radGrdDdlCondition" runat="server" SelectedValue='<%#Eval("CONDITION") %>' Width="100%">
                                                                                <Items>
                                                                                    <telerik:DropDownListItem Text="" Value="" />
                                                                                    <telerik:DropDownListItem Text="Equals" Value="Equals" />
                                                                                    <telerik:DropDownListItem Text="GreaterThan" Value="GreaterThan" />
                                                                                    <telerik:DropDownListItem Text="LessThan" Value="LessThan" />
                                                                                    <telerik:DropDownListItem Text="StartWith" Value="StartWith" />
                                                                                    <telerik:DropDownListItem Text="NotStartWith" Value="NotStartWith" />
                                                                                    <telerik:DropDownListItem Text="Include" Value="Include" />
                                                                                    <telerik:DropDownListItem Text="NotInclude" Value="NotInclude" />
                                                                                    <telerik:DropDownListItem Text="IsNull" Value="IsNull" />
                                                                                    <telerik:DropDownListItem Text="IsNotNull" Value="IsNotNull" />
                                                                                </Items>
                                                                            </telerik:RadDropDownList>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridBoundColumn DataField="VALUE" HeaderText="Value" UniqueName="VALUE" HeaderStyle-Width="50%">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="OPTION" HeaderText="Option" HeaderStyle-Width="150px">
                                                                        <ItemTemplate>
                                                                            <%#DataBinder.Eval(Container.DataItem, "OPTION")%>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <telerik:RadDropDownList ID="radGrdDdlOption" SelectedValue='<%#Eval("OPTION") %>' runat="server" Width="100%">
                                                                                <Items>
                                                                                    <telerik:DropDownListItem Text="And" Value="And" />
                                                                                    <telerik:DropDownListItem Text="Or" Value="Or" Selected="true" />
                                                                                </Items>
                                                                            </telerik:RadDropDownList>
                                                                        </EditItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="REMOVE_BUTTON" HeaderStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="Button1" runat="server" CommandName="Remove" OnClientClick='<%# String.Format("return openConfirmPopUp({0}, 0);",Eval("CONDITION_SEQ"))%> '
                                                                                ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>


                                                                </Columns>

                                                            </MasterTableView>

                                                        </telerik:RadGrid>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>


                            </div>
                        </div>
                        <div class="config_type1">
                            <h4>Before Approver</h4>
                            <telerik:RadGrid ID="radGrdBeforeApprover" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" Width="100%"
                                 Skin="EXGrid" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
                                OnSelectedIndexChanged="radGrdBeforeApprover_SelectedIndexChanged" OnItemCommand="radGrdBeforeApprover_ItemCommand" OnItemDataBound="radGrdBeforeApprover_ItemDataBound">
                                <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true">
                                    <Selecting AllowRowSelect="true" />
                                   
                                </ClientSettings>
                                <MasterTableView ShowHeadersWhenNoRecords="true" DataKeyNames="CONDITION_INDEX" HeaderStyle-Wrap="false">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="CONDITION_INDEX" HeaderText="" UniqueName="CONDITION_INDEX" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="APPROVAL_LOCATION" HeaderText="Location" UniqueName="APPROVAL_LOCATION" Display="false" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="IS_MANDATORY" HeaderText="Mandatory" UniqueName="IS_MANDATORY" Display="false" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="APPROVER_ID" HeaderText="ApproverId" UniqueName="APPROVER_ID" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="APPROVER_NAME" HeaderText="Approver" UniqueName="APPROVER_NAME" HeaderStyle-Width="30%"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DISPLAY_CONDITION" HeaderText="Condition" UniqueName="DISPLAY_CONDITION" HeaderStyle-Width="40%"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="SQL_CONDITION" HeaderText="SQL" UniqueName="SQL_CONDITION" HeaderStyle-Width="30%"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="MAX_CONDITION_INDEX" HeaderText="MaxIdx" UniqueName="MAX_CONDITION_INDEX" Display="false" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="REMOVE_BEFORE_APPROVER" HeaderText="" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove"
                                                    OnClientClick='<%# String.Format("return openConfirmPopUp({0}, 1);",Eval("CONDITION_INDEX"))%> '
                                                    ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>

                                </MasterTableView>
                            </telerik:RadGrid>
                            <div style="width: 100%; height: 10px;"></div>
                            <h4>After Approver</h4>
                            <telerik:RadGrid ID="radGrdAfterApprover" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" Width="100%"
                                 Skin="EXGrid" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
                                AllowMultiRowSelection="false" OnSelectedIndexChanged="radGrdAfterApprover_SelectedIndexChanged" OnItemCommand="radGrdAfterApprover_ItemCommand">
                                <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true">
                                    <Selecting AllowRowSelect="true" />
                                    
                                </ClientSettings>
                                <MasterTableView ShowHeadersWhenNoRecords="true" DataKeyNames="CONDITION_INDEX" HeaderStyle-Wrap="false">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="CONDITION_INDEX" HeaderText="" UniqueName="CONDITION_INDEX" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="APPROVAL_LOCATION" HeaderText="Location" UniqueName="APPROVAL_LOCATION" Display="false" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="IS_MANDATORY" HeaderText="Mandatory" UniqueName="IS_MANDATORY" Display="false" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="APPROVER_ID" HeaderText="ApproverId" UniqueName="APPROVER_ID" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="APPROVER_NAME" HeaderText="Approver" UniqueName="APPROVER_NAME" HeaderStyle-Width="30%"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DISPLAY_CONDITION" HeaderText="Condition" UniqueName="DISPLAY_CONDITION" HeaderStyle-Width="40%"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="SQL_CONDITION" HeaderText="SQL" UniqueName="SQL_CONDITION" HeaderStyle-Width="30%"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="MAX_CONDITION_INDEX" HeaderText="MaxIdx" UniqueName="MAX_CONDITION_INDEX" Display="false" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="REMOVE_AFTER_APPROVER" HeaderText="" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" OnClientClick='<%# String.Format("return openConfirmPopUp({0}, 2);",Eval("CONDITION_INDEX"))%> '
                                                    ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>

                            <h4>Recipient</h4>
                            <telerik:RadGrid ID="radGrdRecipient" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" Width="100%"
                                Skin="EXGrid"
                                AllowMultiRowSelection="false" OnSelectedIndexChanged="radGrdRecipient_SelectedIndexChanged" OnItemCommand="radGrdRecipient_ItemCommand">
                                <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true">
                                    <Selecting AllowRowSelect="true" />
                                </ClientSettings>
                                <MasterTableView ShowHeadersWhenNoRecords="true" DataKeyNames="CONDITION_INDEX" HeaderStyle-Wrap="false">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="CONDITION_INDEX" HeaderText="" UniqueName="CONDITION_INDEX" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="IS_MANDATORY" HeaderText="Mandatory" UniqueName="IS_MANDATORY" Display="false" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="RECIPIENT_ID" HeaderText="RecipientId" UniqueName="RECIPIENT_ID" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="RECIPIENT_NAME" HeaderText="Recipient" UniqueName="RECIPIENT_NAME" HeaderStyle-Width="30%"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DISPLAY_CONDITION" HeaderText="Condition" UniqueName="DISPLAY_CONDITION" HeaderStyle-Width="40%"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="SQL_CONDITION" HeaderText="SQL" UniqueName="SQL_CONDITION" HeaderStyle-Width="30%"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="MAX_CONDITION_INDEX" HeaderText="MaxIdx" UniqueName="MAX_CONDITION_INDEX" Display="false" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="REMOVE_RECIPIENT" HeaderText="" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" OnClientClick='<%# String.Format("return openConfirmPopUp({0}, 3);",Eval("CONDITION_INDEX"))%> '
                                                    ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                            <h4>Reviewer</h4>
                            <telerik:RadGrid ID="radGrdReviewer" runat="server" AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" Width="100%"
                                Skin="EXGrid"
                                AllowMultiRowSelection="false" OnSelectedIndexChanged="radGrdReviewer_SelectedIndexChanged" OnItemCommand="radGrdReviewer_ItemCommand">
                                <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true">
                                    <Selecting AllowRowSelect="true" />
                                </ClientSettings>
                                <MasterTableView ShowHeadersWhenNoRecords="true" DataKeyNames="CONDITION_INDEX" HeaderStyle-Wrap="false">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="CONDITION_INDEX" HeaderText="" UniqueName="CONDITION_INDEX" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="IS_MANDATORY" HeaderText="Mandatory" UniqueName="IS_MANDATORY" Display="false" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="REVIEWER_ID" HeaderText="ReviewerId" UniqueName="REVIEWER_ID" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="REVIEWER_NAME" HeaderText="Reviewer" UniqueName="REVIEWER_NAME" HeaderStyle-Width="30%"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DISPLAY_CONDITION" HeaderText="Condition" UniqueName="DISPLAY_CONDITION" HeaderStyle-Width="40%"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="SQL_CONDITION" HeaderText="SQL" UniqueName="SQL_CONDITION" HeaderStyle-Width="30%"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="MAX_CONDITION_INDEX" HeaderText="MaxIdx" UniqueName="MAX_CONDITION_INDEX" Display="false" HeaderStyle-Width="30px"></telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="REMOVE_REVIEWER" HeaderText="" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" OnClientClick='<%# String.Format("return openConfirmPopUp({0}, 4);",Eval("CONDITION_INDEX"))%> '
                                                    ImageUrl="~/Styles/images/ico_del.png" BorderStyle="None" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>
                    </div>

                </div>
            </div>
            <div id="divHidden" runat="server">
                <input type="hidden" id="hddDocumentId" runat="server" />
                <input type="hidden" id="hddConditionIdx" runat="server" />
                <input type="hidden" id="hddMaxConditionIdx" runat="server" />
                <input type="hidden" id="hddConditionList" runat="server" />
            </div>
        </telerik:RadPane>
    </telerik:RadSplitter>
     <telerik:RadWindowManager ID="RadWindowManager2" runat="server"  OnClientClose="OnClientClose_new">
        <Windows>
            <telerik:RadWindow ID="radWinUser"  runat="server" NavigateUrl="/eWorks/Common/Popup/PopupUserList.aspx" Title="User List" Modal="true" Width="500" Height="700" VisibleStatusbar="false" Skin="Metro">
            </telerik:RadWindow>
        </Windows>

    </telerik:RadWindowManager>
    

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="server">
   
</asp:Content>
