<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DocumentBaseFooter.ascx.cs" Inherits="Common_DocumentBaseFooter" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Common/UserAutoCompleteBox.ascx" TagPrefix="uc1" TagName="UserAutoCompleteBox" %>
<%@ Register Src="~/Common/FileUpload.ascx" TagPrefix="uc1" TagName="FileUpload" %>



<div > 
        <telerik:RadPanelBar runat="server" ID="panelFooter"  Width="100%" > 
            <Items> 
                <telerik:RadPanelItem Text="Append Reviewer" Value="AddReviewer" Expanded="true" runat="server">
                    <ContentTemplate>
                        <uc1:UserAutoCompleteBox runat="server" ID="UserAutoCompleteBox" />
                    </ContentTemplate>
                </telerik:RadPanelItem>
                <telerik:RadPanelItem Text="Attachment" Value="Attachment" Expanded="true" runat="server">
                    <ContentTemplate>
                        <uc1:FileUpload runat="server" id="FileUpload" />
                    </ContentTemplate>
                </telerik:RadPanelItem>

                <telerik:RadPanelItem Text="Recipient" Value="Recipient" Expanded="true" runat="server">
                    <ContentTemplate>
                        <div>Reviewer List</div>
                    </ContentTemplate>
                </telerik:RadPanelItem>

                <telerik:RadPanelItem Text="Reviewer" Value="Reviewer" Expanded="true" runat="server">
                    <ContentTemplate>
                        <div>Reviewer List</div>
                    </ContentTemplate>
                </telerik:RadPanelItem>

                <telerik:RadPanelItem Text="Log"  Value="Log" Expanded="true" runat="server">
                    <ContentTemplate>
                        <div>Reviewer List</div>
                    </ContentTemplate>
                </telerik:RadPanelItem>

            </Items>
            <CollapseAnimation Duration="0" Type="None" /> 
            <ExpandAnimation Duration="0" Type="None" />
        </telerik:RadPanelBar> 
    </div>