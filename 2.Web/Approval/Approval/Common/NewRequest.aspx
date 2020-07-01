<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.Master" AutoEventWireup="true" CodeBehind="NewRequest.aspx.cs" Inherits="Approval.Approval.Common.WebForm1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="server">
    <script type="text/javascript">
        function openRadWinDocumentList() {
            radopen(null, "RadWindowDocumentList")
        }

    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="server">

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="server">
    <button style="width: 150px; margin-bottom: 3px;" onclick="openRadWinDocumentList(); return false;">New Request</button>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="RadWinDocumentList" runat="server" Title="Document List" Width="100px" Height="100px"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="server">
</asp:Content>
