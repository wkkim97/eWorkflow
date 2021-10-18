<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Document.Master" AutoEventWireup="true" CodeBehind="DocSample.aspx.cs" Inherits="Approval.Approval.Document.DocSample" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderDocumentBody" runat="server">
    <div style="width: 100%;">
        <table style="border: 0; padding: 0; margin: 0;width: 100%;">
            <tr>
                <th>Subject</th>
                <td>
                    <telerik:RadTextBox ID="txtSubject" runat="server" Width="100%"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <th>DIV</th>
                <td>
                    <telerik:RadButton ID="RadButton18" runat="server"  ToggleType="Radio"   ButtonType="ToggleButton" GroupName="Radio"  AutoPostBack="false" Width="100%">
                        <ToggleStates>
                            <telerik:RadButtonToggleState Text="SM" Value="SM" Selected="true" PrimaryIconCssClass="rbToggleRadioChecked"></telerik:RadButtonToggleState>
                            <telerik:RadButtonToggleState Text="HH" Value="HH" PrimaryIconCssClass="rbToggleRadio"></telerik:RadButtonToggleState>
                            <telerik:RadButtonToggleState Text="WH" PrimaryIconCssClass="rbToggleRadio"></telerik:RadButtonToggleState>
                            <telerik:RadButtonToggleState Text="R&I" PrimaryIconCssClass="rbToggleRadio"></telerik:RadButtonToggleState>
                            <telerik:RadButtonToggleState Text="AH" PrimaryIconCssClass="rbToggleRadio"></telerik:RadButtonToggleState>
                            <telerik:RadButtonToggleState Text="CC" PrimaryIconCssClass="rbToggleRadio"></telerik:RadButtonToggleState>
                            <telerik:RadButtonToggleState Text="DC" PrimaryIconCssClass="rbToggleRadio"></telerik:RadButtonToggleState>
                        </ToggleStates>

                    </telerik:RadButton>
                </td>
            </tr>
            <tr>
                <th>Settlement Type</th>
                <td>
                    <telerik:RadButton ID="RadButton1" runat="server" ToggleType="Radio" GroupName="StandardButton"
                        ButtonType="StandardButton" AutoPostBack="false">
                        <ToggleStates>
                            <telerik:RadButtonToggleState Text="AR"></telerik:RadButtonToggleState>
                            <telerik:RadButtonToggleState Text="Product"></telerik:RadButtonToggleState>
                        </ToggleStates>
                    </telerik:RadButton>
                </td>
            </tr>
            <tr>
                <th>Description</th>
                <td>
                    <telerik:RadTextBox ID="txtDescription" TextMode="MultiLine" Height="300px" Width="100%" runat="server"></telerik:RadTextBox></td>
            </tr>
        </table>
    </div>
    <input type="hidden" id="HddProcessID" runat="server" />
</asp:Content>
