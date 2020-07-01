<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentBaseFooter.ascx.cs" Inherits="Approval.Common.DocumentBaseFooter" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

     <div >

          <telerik:RadPanelBar runat="server" ID="RadPanelBar1" Skin="Silk" Width="100%" >

              <Items>

                  <telerik:RadPanelItem Text="Reviewer" Expanded="true" runat="server">
                      <ContentTemplate>
                          <div>Reviewer List</div>
                      </ContentTemplate>
                  </telerik:RadPanelItem>
                  <telerik:RadPanelItem Text="Attachment" Expanded="true" runat="server">
                      <ContentTemplate>
                          <div>Reviewer List</div>
                      </ContentTemplate>
                  </telerik:RadPanelItem>

                  <telerik:RadPanelItem Text="Recipient" Expanded="true" runat="server">
                      <ContentTemplate>
                          <div>Reviewer List</div>
                      </ContentTemplate>
                  </telerik:RadPanelItem>

                  <telerik:RadPanelItem Text="Reviewer" Expanded="true" runat="server">
                      <ContentTemplate>
                          <div>Reviewer List</div>
                      </ContentTemplate>
                  </telerik:RadPanelItem>

                  <telerik:RadPanelItem Text="Log" Expanded="true" runat="server">
                      <ContentTemplate>
                          <div>Reviewer List</div>
                      </ContentTemplate>
                  </telerik:RadPanelItem>

              </Items>
              <CollapseAnimation Duration="0" Type="None" /> 
              <ExpandAnimation Duration="0" Type="None" />
          </telerik:RadPanelBar>

     </div>