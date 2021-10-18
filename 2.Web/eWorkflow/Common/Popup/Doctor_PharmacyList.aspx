<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="Doctor_PharmacyList.aspx.cs" Inherits="Common_Popup_Doctor_PharmacyList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function returnToParent(sender, args) {
            var item = {};            
            //doctor
            if ($find("<%= RadrdoDoctor.ClientID %>").get_checked()) {
                var gridSelectedItems = $find("<%= RadGrdDoctor.ClientID %>").get_masterTableView().get_selectedItems();                
                if (gridSelectedItems.length == 0) {
                    fn_OpenDocInformation('선택한 항목이 없습니다.');
                    sender.set_autoPostBack(false);
                }
                else {
                    var row = gridSelectedItems[0];
                    item.radioValue = $('#<%= hddRadioValue.ClientID%>').val();
                    item.INS_NAME = row.get_cell("INS_NAME").innerHTML;
                    item.INS_CODE = row.get_cell("INS_CODE").innerHTML;
                    item.DOC_NAME = row.get_cell("DOC_NAME").innerHTML;
                    item.DOC_CODE = row.get_cell("DOC_CODE").innerHTML;
                    item.SPECIALITY = row.get_cell("SPECIALITY").innerHTML;                    
                    var oWnd = GetRadWindow();                    
                    GetRadWindow().close(item);
                }
            }
            //pharmacy
            if ($('#<%= hddBu.ClientID%>').val() == 'CC') {
            <%--if ($find("<%= RadrdoPharmacy.ClientID %>").get_checked()) {--%>
                var gridSelectedItems = $find("<%= RadGrdPharmacy.ClientID %>").get_masterTableView().get_selectedItems();
                if (gridSelectedItems.length == 0) {
                    fn_OpenDocInformation('선택한 항목이 없습니다.');
                    sender.set_autoPostBack(false);
                }
                else {
                    var row = gridSelectedItems[0];
                    item.radioValue = $('#<%= hddRadioValue.ClientID%>').val();
                    item.PHAR_NAME = row.get_cell("PHAR_NAME").innerHTML;
                    item.PHAR_CODE = row.get_cell("PHAR_CODE").innerHTML;
                    var oWnd = GetRadWindow();            
                    GetRadWindow().close(item);
                }
            }
            if ($('#<%= hddRadioValue.ClientID%>').val() == "") {
                fn_OpenDocInformation('Please Select Term');
                sender.set_autoPostBack(false);
            }
            
        }        

        function cancelAndClose(sender, args) {
            var oWnd = GetRadWindow();
            oWnd.close();
        }

        function fn_GridDisplay(sender, args) {
            var radioValue = sender.get_value();
            if (radioValue = 'Doctor') {
                $('#Doctor').show();
                $('#pharmacy').hide();
            }
            if (radioValue = 'pharmacy') {
                $('#Doctor').hide();
                $('#pharmacy').show();
            }
        }

        function fn_OnRowDblClick(sender, args) {
            var item = {};            
            if ($find("<%= RadrdoDoctor.ClientID %>").get_checked()) {
                var gridSelectedItems = $find("<%= RadGrdDoctor.ClientID %>").get_masterTableView().get_selectedItems();
                var row = gridSelectedItems[0];
                item.radioValue = $('#<%= hddRadioValue.ClientID%>').val();
                item.INS_NAME = row.get_cell("INS_NAME").innerHTML;
                item.INS_CODE = row.get_cell("INS_CODE").innerHTML;
                item.DOC_NAME = row.get_cell("DOC_NAME").innerHTML;
                item.DOC_CODE = row.get_cell("DOC_CODE").innerHTML;
                item.SPECIALITY = row.get_cell("SPECIALITY").innerHTML;
            }

            if ($('#<%= hddBu.ClientID%>').val() == 'CC') {
                if ($find("<%= RadrdoPharmacy.ClientID %>").get_checked()) {
                    var gridSelectedItems = $find("<%= RadGrdPharmacy.ClientID %>").get_masterTableView().get_selectedItems();
                    var row = gridSelectedItems[0];
                    item.radioValue = $('#<%= hddRadioValue.ClientID%>').val();
                    item.PHAR_NAME = row.get_cell("PHAR_NAME").innerHTML;
                    item.PHAR_CODE = row.get_cell("PHAR_CODE").innerHTML;
                }
            }
            GetRadWindow().close(item);
        }

        function onEnter(e) {
            var theKey = 0;
            e = (window.event) ? event : e;
            theKey = (e.keyCode) ? e.keyCode : e.charCode;
            if (theKey == "13") {
                $find("<%=radBtnSearch.ClientID %>").click();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrdDoctor">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrdDoctor" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div id="wrap">
        <div id="pop_content">
            <h3>Search Term :
                <telerik:RadButton runat="server" ID="RadrdoDoctor" GroupName="term" ButtonType="ToggleButton" ToggleType="Radio" Text="Doctor" Value="Doctor" OnCheckedChanged="RadrdoDoctor_CheckedChanged"></telerik:RadButton>
                <telerik:RadButton runat="server" ID="RadrdoPharmacy" GroupName="term" ButtonType="ToggleButton" ToggleType="Radio" Text="Pharmacy" Value="Pharmacy" OnCheckedChanged="RadrdoDoctor_CheckedChanged"></telerik:RadButton>
            </h3>
            <br />
            <div class="align_right">
                <telerik:RadTextBox ID="radTxtKeyword" runat="server" ClientEvents-OnKeyPress="onEnter"></telerik:RadTextBox>
                <telerik:RadButton ID="radBtnSearch" runat="server" Text="Search"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-gray btn-size2 bold" OnClick="radBtnSearch_Click">
                </telerik:RadButton>
            </div>
            <div class="data_type2 pt20">
                <div id="docter" style="display: none" runat="server">
                    <telerik:RadGrid ID="RadGrdDoctor" runat="server" AllowPaging="true" AutoGenerateColumns="false" Height="400px" Skin="EXGrid" OnNeedDataSource="RadGrdDoctor_NeedDataSource" >
                        <ClientSettings EnablePostBackOnRowClick="true">
                            <Selecting AllowRowSelect="true" />
                        </ClientSettings>
                        <HeaderStyle HorizontalAlign="Left" />
                        <MasterTableView>
                            <Columns>
                                <telerik:GridBoundColumn DataField="INS_NAME" HeaderText="INS NAME"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="INS_CODE" HeaderText="INS CODE"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DOC_NAME" HeaderText="DOC NAME"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DOC_CODE" HeaderText="DOC CODE"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="SPECIALITY" HeaderText="SPECIALITY"></telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings>
                            <ClientEvents OnRowDblClick="fn_OnRowDblClick" />
                        </ClientSettings>
                        <PagerStyle PageSizeControlType="None" />
                    </telerik:RadGrid>
                </div>
                <div id="pharmacy" style="display: none" runat="server">
                    <telerik:RadGrid ID="RadGrdPharmacy" runat="server" AllowPaging="true" AutoGenerateColumns="false" Height="400px" Skin="EXGrid" OnNeedDataSource="RadGrdPharmacy_NeedDataSource">
                        <ClientSettings EnablePostBackOnRowClick="true">
                            <Selecting AllowRowSelect="true" />
                        </ClientSettings>
                        <MasterTableView>
                            <Columns>
                                <telerik:GridBoundColumn DataField="CLIENT_CODE" HeaderText="CLIENT_CODE" Display="false"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PHAR_NAME" HeaderText="PHAR NAME"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PHAR_CODE" HeaderText="PHAR CODE"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="STATE" HeaderText="STATE"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="POSTAL_CITY" HeaderText="CITY"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LINE_1_ADDRESS" HeaderText="ADDRESS"></telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings>
                            <ClientEvents OnRowDblClick="fn_OnRowDblClick" />
                        </ClientSettings>
                        <PagerStyle PageSizeControlType="None" />
                    </telerik:RadGrid>
                </div>
            </div>
            <div class="align_center pt20">
                <telerik:RadButton ID="radBtnOk" runat="server" Text="Ok"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-blue btn-size3 bold" OnClientClicked="returnToParent">
                </telerik:RadButton>
                <telerik:RadButton ID="radBtnCancel" runat="server" Text="Cancel"
                    ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" CssClass="btn btn-darkgray btn-size3 bold" OnClientClicked="cancelAndClose">
                </telerik:RadButton>
            </div>
        </div>
    </div>
    <div id="hiddenArea">
        <input type="hidden" id="hddRadioValue" runat="server" />
        <input type="hidden" id="hddBu" runat="server" />
    </div>
</asp:Content>

