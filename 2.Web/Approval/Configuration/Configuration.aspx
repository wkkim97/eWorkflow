<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.Master" AutoEventWireup="true" CodeBehind="Configuration.aspx.cs" Inherits="Approval.Configuration.Configuration" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Common/ConfigConditionBox.ascx" TagPrefix="ConfigCondition" TagName="ConfigCondition" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="server">
    <style type="text/css">
        table.general {
            width: 100%;
        }

            table.general td {
                padding: 3px 3px 3px 3px;
            }

                table.general td:nth-child(odd) {
                    background: #F5F4F4;
                    width: 200px;
                    text-align: right;
                    padding-right: 10px;
                }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="server">
    <telerik:RadSplitter runat="server" ID="radSpliterMain" Width="100%" Height="600px">
        <telerik:RadPane runat="server" ID="radLeftPane" Width="250px">
        </telerik:RadPane>
        <telerik:RadSplitBar ID="Radsplitbar1" runat="server" >
        </telerik:RadSplitBar>
        <telerik:RadPane runat="server" ID="radRightPane">
            <div style="width: 99%; vertical-align: middle;">
                <div style="float: right">
                    <telerik:RadButton runat="server" ID="btnSave" Text="Save"></telerik:RadButton>
                    <telerik:RadButton runat="server" ID="btnDelete" Text="Delete"></telerik:RadButton>
                </div>
                <div>
                    <fieldset style="width: 100%">
                        <legend>General Information</legend>
                        <table class="general">
                            <tr>
                                <td>Name</td>
                                <td colspan="3">
                                    <telerik:RadTextBox runat="server" ID="radTxtDocName" Width="100%"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>TableName</td>
                                <td>
                                    <telerik:RadTextBox runat="server" ID="radTxtTableName" Width="100%"></telerik:RadTextBox>
                                </td>
                                <td>Data Owner</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Company</td>
                                <td></td>
                                <td>Category</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Readers Group</td>
                                <td></td>
                                <td>Prefix of Doc.</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Information Classification</td>
                                <td></td>
                                <td>Document Image</td>
                                <td></td>
                            </tr>
                            <tr style="height: 60px">
                                <td>Document Description</td>
                                <td colspan="3"></td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>Option</legend>
                        <table class="general">
                            <tr>
                                <td>Forward</td>
                                <td></td>
                                <td>Add Approver</td>
                                <td></td>
                                <td>Show Document List</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Add Reviewer</td>
                                <td></td>
                                <td>Reviewer Description</td>
                                <td></td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>Approval Line</legend>
                        <table class="general">
                            <tr>
                                <td>Default Line
                                </td>
                                <td>
                                    <table style="padding: 0 0 0 0; margin: 0 0 0 0">
                                        <tr>
                                            <td>
                                                <telerik:RadButton runat="server" ID="radBtnApprovalLevel" ButtonType="ToggleButton" ToggleType="Radio"
                                                    Text="Approval Level" GroupName="ApprovalType" AutoPostBack="false">
                                                </telerik:RadButton>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox runat="server" ID="radTxtApprovalLevel"></telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadButton runat="server" ID="radBtnJobTitle" ButtonType="ToggleButton" ToggleType="Radio"
                                                    Text="Job Title" GroupName="ApprovalType" AutoPostBack="false">
                                                </telerik:RadButton>
                                            </td>
                                            <td>
                                                <div id="divJobTitle" runat="server"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadButton runat="server" ID="radBtnLimitAmount" ButtonType="ToggleButton" ToggleType="Radio"
                                                    Text="Limit Amount" GroupName="ApprovalType" AutoPostBack="false">
                                                </telerik:RadButton>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>Additional Approver/Recipient/Reviewer</legend>
                        <ul style="float: right">
                            <telerik:RadButton runat="server" ID="radBtnAddCondition" Text="Add"></telerik:RadButton>
                        </ul>
                        <div style="height: 150px; overflow-y: scroll">
                            <table style="width: 100%">
                                <tr>
                                    <th style="width: 150px">Field</th>
                                    <th style="width: 100%">Condition</th>
                                    <th style="width: 150px">Value</th>
                                    <th style="width: 100px">Optiion</th>
                                    <th style="width: 40px"></th>
                                </tr>
                            </table>
                            <asp:PlaceHolder ID="phConfiguration" runat="server" />
                        </div>
                        <%--                <ul style="height: 120px; overflow: auto; overflow-y: scroll">
                </ul>--%>
                    </fieldset>
                </div>

                <!--The text value determines how many items are initially displayed on the page-->
                <asp:Literal ID="ltlCount" runat="server" Text="0" Visible="false" />
                <asp:Literal ID="ltlRemoved" runat="server" Visible="false" />

            </div>
        </telerik:RadPane>
    </telerik:RadSplitter>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="server">
</asp:Content>
