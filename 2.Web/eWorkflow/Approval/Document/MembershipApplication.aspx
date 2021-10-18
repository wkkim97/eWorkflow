<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.master" AutoEventWireup="true" CodeFile="MembershipApplication.aspx.cs" Inherits="Approval_Document_MembershipApplication" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function fn_DoSave(sender, args) {  
            return true;
        }




    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="Server">
    <div class="doc_style">
        <div id="divCancel" class="align_right pb10" style="display: none;" runat="server">
            <telerik:RadButton ID="RadbtnCancel" runat="server" Text="Cancel" OnClick="RadbtnCancel_Click" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" ButtonType="LinkButton" CssClass="btn btn-blue btn-size1 bold"></telerik:RadButton>
        </div>
        <h3>Information of Trade Association</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>

                <tr id="TACategory" >
                    <th>TA Category  <span class="text_red">*</span></th>
                    <td>
                        <div id="divTACategory" runat="server" style="margin: 0 0 0 0">
                            <telerik:RadButton ID="radBtnWhitelisted" runat="server" Text="Whitelisted" Value="Whitelisted" GroupName="TA"
                                ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                            <telerik:RadButton ID="radBtnNon_Whitelisted" runat="server" Text="Non-Whitelisted" Value="Non-Whitelisted" GroupName="TA"
                                ButtonType="ToggleButton" ToggleType="Radio">
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr> 
                <tr>
                    <th>Name of Association <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtEngName" runat="server" Label="English" Width="90%" LabelWidth="50px"></telerik:RadTextBox>
                        <br />
                        <div style="height: 2px"></div>

                        <telerik:RadTextBox ID="radTxtKorName" runat="server" Label="Korean" Width="90%" LabelWidth="50px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Objective of Association <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtObjective" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Purpose of obtaining membership <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtPurpose" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>President/Secretary <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtPresident" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Membership fee <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadNumericTextBox ID="radNumFee" runat="server" NumberFormat-DecimalDigits="0" EnabledStyle-HorizontalAlign="Right"></telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Address <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtAddress" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Phone No. <span class="text_red">*</span>
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtPhone" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Fax
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtFax" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Homepage
                    </th>
                    <td>
                        <telerik:RadTextBox ID="radTxtHomepage" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </div>
<%--    2014.12.05 제거    
        <h3>Representative information</h3>
        <div class="data_type1">
            <table>
                <colgroup>
                    <col style="width: 25%;" />
                    <col />
                </colgroup>
                <tr>
                    <th>Relevant Business</th>
                    <td>
                        <telerik:RadDropDownList ID="radDdlBusiness" runat="server">
                            <Items>
                                <telerik:DropDownListItem Text="" Value="" />
                                <telerik:DropDownListItem Text="CORP" Value="CORP" />
                                <telerik:DropDownListItem Text="BHC" Value="BHC" />
                                <telerik:DropDownListItem Text="BMSG" Value="BMSG" />
                                <telerik:DropDownListItem Text="BMSK" Value="BMSK" />
                                <telerik:DropDownListItem Text="BCS" Value="BCS" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <th>Job function</th>
                    <td>
                        <telerik:RadTextBox ID="radTxtJobFunction" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Sub-Committee</th>
                    <td>
                        <telerik:RadTextBox ID="radTxtSubCommitte" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </div>--%>
    </div>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
        </Windows>
    </telerik:RadWindowManager>
    <input type="hidden" id="hddProcessID" runat="server" />
    <input type="hidden" id="HddProcessStatus" runat="server" />
    <input type="hidden" id="hddDocumentID" runat="server" />
    <input type="hidden" id="hddReuse" runat="server" />
	<input type="hidden" id="hddAddRow" runat="server" value="N" />
    <input type="hidden" id="hddRequestId" runat="server"  />
</asp:Content>

