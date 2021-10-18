<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="ReadersGroup.aspx.cs" Inherits="Configuration_ReadersGroup" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="server">
    <script type="text/javascript">
        // User List 정보 창 열기
        function fn_PopupShow(sender, Args) {
            var wnd = $find("<%= modalUserList.ClientID %>");
            wnd.setUrl("/eWorks/Common/Popup/UserList.aspx?multiselect=Y");
            wnd.show();
            sender.set_autoPostBack(false);
        }
        //-----------------------------------
        //그리드 클라이언트 데이터 업데이트
        //-----------------------------------
        function fn_UpdataGridData(data) {
            var list = [];
            var masterTable = $find('<%= RadGridRdsGroup.ClientID %>').get_masterTableView();
            var dataItems = masterTable.get_dataItems();
            var is_val = false;

            for (var i = 0; i < dataItems.length; i++) {
                var userid = dataItems[i].get_cell("USER_ID").innerText;
                var fullname = dataItems[i].get_cell("FULL_NAME").innerText;

                var userlist = {
                    USER_ID: null,
                    FULL_NAME: null
                }
                userlist.USER_ID = userid;
                userlist.FULL_NAME = fullname;
                list.push(userlist);
            }
            $('#<%= __VIEWSTATE.ClientID%>').val(JSON.stringify(list));

            if (data) {
                for (var i = 0; i < data.length; i++) {
                    var userlist = {
                        USER_ID: null,
                        FULL_NAME: null
                    }
                    if (list.length > 0) {
                        for (var j = 0; j < list.length; j++) {
                            if (data[i].USER_ID == list[j].USER_ID) {
                                is_val = true;
                                break;
                            }
                        }
                    }
                    if (is_val == false) {
                        userlist.USER_ID = data[i].USER_ID;
                        userlist.FULL_NAME = data[i].FULL_NAME;
                        list.push(userlist);
                    }
                }                
                $('#<%= __VIEWSTATE.ClientID%>').val(JSON.stringify(list));
                is_val = false;
            }
            return true;
        }

        // POPUP GRID Close Event
        function OnClientClose(sender, args) {
            var uListData = args.get_argument();
            if (uListData != null) {
                fn_UpdataGridData(uListData);
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("Rebind");
            }
            else
                return false;
            }

            // 전체저장
            function fn_OnSaveClicked(sender, args) {
                var valid = checkValidation();

                if (valid) {
                    fn_UpdataGridData(null);
                }
                else
                    sender.set_autoPostBack(false);
            }

            //그룹 리스트의 로우 삭제 
            function fn_OnRemoveClicked(sender, args) {
                var gridDel = $find('<%= RadGrid1.ClientID %>').get_masterTableView().get_selectedItems();
                if (gridDel.length == 0) {
                    fn_OpenInformation("No Selected Item");
                    sender.set_autoPostBack(false);
                }                
            }
            // 그룹 리스트 삭제
            function fn_OnDelClicked(sender, args) {
                var SelectedItem = $find('<%= radGrdGroupList.ClientID %>').get_masterTableView().get_selectedItems();
               
                var callBackFunction = Function.createDelegate(sender, function (shouldSubmit) {
                    if (shouldSubmit) {
                        if (SelectedItem.length == 0) {
                            fn_OpenInformation("No Selected Item");
                            sender.set_autoPostBack(false);
                            return;
                        }
                        sender.set_autoPostBack(true);
                        this.click();
                    }
                });
                fn_OpenConfirm("정말 삭제 하시겠습니까?", callBackFunction);
                sender.set_autoPostBack(false);
            }

            function checkValidation() {
                var isValid = true;
                var emptyMessage = "";

                //Grid Data
                var gridValue = $find('<%= RadGridRdsGroup.ClientID %>').get_masterTableView().get_dataItems().length;
                if (gridValue == 0)
                    emptyMessage = "Grid에 Data가 선택되지 않았습니다.";

                //Group Name
<%--                var inputValue = $find('<%= RadTbGroupName.ClientID %>').get_value();
                if (isEmpty(inputValue))
                    emptyMessage = "Group Name이 지정되지 않았습니다.";--%>

                if (isEmpty(emptyMessage))
                    return true;
                else {
                    fn_OpenInformation(emptyMessage);                    
                    return false;
                }
            }

            function isEmpty(str) {
                return (!str || 0 == str.length);
            }

            // 리더스그룹 리스트의 선택한 로우의 code를 hiddin에 넣어준다.
            function get_Rowvalue(sender, args) {
                var grid = sender;
                var MasterTable = grid.get_masterTableView();
                var row = MasterTable.get_dataItems()[args.get_itemIndexHierarchical()];
                var cell = MasterTable.getCellByColumnUniqueName(row, "DOCUMENT_ID");
                var selectedGroupcode = cell.innerText;   // 선택한 그룹 코드
                 
                $('#<%= hddGroupCode.ClientID%>').val(selectedGroupcode);
                
            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="server">

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridReadersGroup" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <%--왼쪽--%>
    <telerik:RadSplitter runat="server" ID="radSpliterMain" Width="100%" Height="800px" VisibleDuringInit="false">
        <telerik:RadPane runat="server" ID="radLeftPane" Width="300px" Scrolling="Y">
            <%--리더스 그룹 목록 리스트--%>
            <telerik:RadGrid ID="radGrdGroupList" AutoGenerateColumns="false" runat="server" HeaderStyle-HorizontalAlign="Left"
                Width="100%" Height="100%" Skin="EXGrid" OnSelectedIndexChanged="radGrdGroupList_SelectedIndexChanged">
                <MasterTableView ShowHeadersWhenNoRecords="true" ItemStyle-Wrap="false">
                    <Columns>
                        <telerik:GridBoundColumn Display="false" UniqueName="DOCUMENT_ID" DataField="DOCUMENT_ID" HeaderText="DOCUMENT_ID" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="DOC_NAME" DataField="DOC_NAME" HeaderText="Document" HeaderStyle-Width="200px" 
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true">
                    <Selecting AllowRowSelect="true" />
                    <ClientEvents OnRowClick="get_Rowvalue" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadPane>

        <telerik:RadSplitBar ID="Radsplitbar1" runat="server">
        </telerik:RadSplitBar>

        <telerik:RadPane runat="server" ID="radRightPane">
            <div id="doc_content">
                <div class="doc_style">
                    <div class="align_right pb10" style="margin-top: 10px; margin-right: 10px;">
                        <telerik:RadButton ID="RadBtnNew" runat="server" ButtonType="LinkButton" Text="NEW" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size1 bold" OnClick="RadBtnNew_Click"></telerik:RadButton>
                        <telerik:RadButton ID="RadBtnSave" runat="server" ButtonType="LinkButton" Text="SAVE" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size1 bold" OnClientClicked="fn_OnSaveClicked" OnClick="RadBtnSave_Click"></telerik:RadButton>
                        <%--<telerik:RadButton ID="RadBtnDel" runat="server" ButtonType="LinkButton" Text="DELETE" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size1 bold" OnClientClicking="fn_OnDelClicked" OnClick="RadBtnDel_Click"></telerik:RadButton>--%>
                    </div>
                </div>
                <h3><%--NEW NAME :                                         
                    <telerik:RadTextBox runat="server" ID="RadTbGroupName" Width="300" Text=""></telerik:RadTextBox>--%>
                    <div class="title_btn">
                        <telerik:RadButton ID="RadButton1" runat="server" ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" Text="RESET" OnClick="RadBtnReset_Click"></telerik:RadButton>
                        <telerik:RadButton ID="RadButton2" runat="server" ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" Text="ADD" OnClientClicked="fn_PopupShow"></telerik:RadButton>
                    </div>
                </h3>
                <%--리더스 그룹 클라이언트 리스트--%>
                <telerik:RadGrid runat="server" ID="RadGridRdsGroup" AllowPaging="false" AllowSorting="true" AllowMultiRowSelection="true" AutoGenerateColumns="false" Culture="ko-KR" OnNeedDataSource="RadGridRdsGroup_NeedDataSource" Width="100%" Skin="EXGrid" HeaderStyle-HorizontalAlign="Left">
                    <MasterTableView ShowHeadersWhenNoRecords="true">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="USER_ID" DataField="USER_ID" HeaderText="UserId" HeaderStyle-Width="40%" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="FULL_NAME" DataField="FULL_NAME" HeaderText="Name" HeaderStyle-Width="55%" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                            <telerik:GridClientDeleteColumn ConfirmText="Do you want to delete this Item ?" CommandName="Delete" Text="Delete" ButtonType="ImageButton" ImageUrl="~/Styles/images/ico_del.png">
                            </telerik:GridClientDeleteColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                </telerik:RadGrid>
                <br />
                <h3>
                 <asp:Label runat="server" ID="lbUserList"></asp:Label>
                <div class="title_btn">
                    <telerik:RadButton ID="RadBtnRemove" runat="server" ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold" Text="REMOVE" OnClientClicked="fn_OnRemoveClicked" OnClick="RadBtnRemove_Click"></telerik:RadButton>
                </div>
            </h3>
            <telerik:RadGrid runat="server" ID="RadGrid1" AllowPaging="false" AllowSorting="true" AllowMultiRowSelection="true" AutoGenerateColumns="false" Culture="ko-KR" OnNeedDataSource="RadGrid1_NeedDataSource" OnDeleteCommand="RadGrid1_DeleteCommand" Width="100%" Skin="EXGrid">
                <MasterTableView ShowHeadersWhenNoRecords="true">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="40px" UniqueName="chkYN">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkboxbody" runat="server" OnCheckedChanged="ToggleRowSelection" AutoPostBack="true" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox ID="headerChkbox" runat="server" OnCheckedChanged="ToggleSelectedState" AutoPostBack="true" />
                            </HeaderTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="USER_ID" DataField="USER_ID" HeaderText="UserId" HeaderStyle-Width="40%" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="FULL_NAME" DataField="FULL_NAME" HeaderText="Name" HeaderStyle-Width="55%" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                        <telerik:GridButtonColumn CommandName="Delete" Text="X" UniqueName="DeleteColumn" ConfirmText="Do you want to delete this Item ?" ButtonType="ImageButton" ImageUrl="~/Styles/images/ico_del.png" />
                    </Columns>
                </MasterTableView>
                <ClientSettings>
                    <Selecting AllowRowSelect="True"></Selecting>
                </ClientSettings>
            </telerik:RadGrid>
            </div>
            <%--ADD POPUP--%>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" >
                <Windows>
                    <telerik:RadWindow ID="modalUserList" runat="server" NavigateUrl="/eWorks/Common/Popup/UserList.aspx" Title="User List" Modal="true" Width="500" Height="700" VisibleStatusbar="false" Skin="Metro" OnClientClose="OnClientClose">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
        </telerik:RadPane>
    </telerik:RadSplitter>

    <div id="divHiddenArea" runat="server">
        <input type="hidden" runat="server" id="__VIEWSTATE" />
        <%--추가한 user--%>
        <input type="hidden" runat="server" id="hddGroupCode" />
        <%--Group Code--%>
        <input type="hidden" runat="server" id="hddState" />
        <%--작성 상태값--%>
    </div>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="server">
</asp:Content>
