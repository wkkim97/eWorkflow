<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="NewRequest.aspx.cs" Inherits="Approval_Common_NewRequest" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="server">
    <script type="text/javascript">
        function openRadWinNewRequest() {
            window.radopen("PopupNewRequest.aspx", "RadWindowNewRequest");
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="server">
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="RadWindowNewRequest" VisibleStatusbar="false" runat="server"  ShowContentDuringLoad="false" Title="New Request" AutoSize="true" Behaviors="Default" Skin="Metro" NavigateUrl="PopupNewRequest.aspx"></telerik:RadWindow>
            <telerik:RadWindow ID="RadWindowTest" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Test" Width="400px" Height="200px" Behaviors="Default" Skin="Metro" NavigateUrl="Test.aspx"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <button style="width: 150px; margin-bottom: 3px;" onclick="openRadWinNewRequest(); return false;">New Request</button>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="server">
</asp:Content>
