<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopMenu.ascx.cs" Inherits="Approval.Common.TopMenu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<div>
    <div>
        <a href="/eWorks/Approval/Main.aspx">eWorkFlow</a>
        <a href="/eWorks/Manage/Authentication/LogOut.aspx">LogOut</a>

        <telerik:RadMenu ID="radTopMenu" Style="z-index: 4" EnableRoundedCorners="true" EnableShadows="false" EnableTextHTMLEncoding="true" Skin="Silk" runat="server"></telerik:RadMenu> 
    </div>
</div>




