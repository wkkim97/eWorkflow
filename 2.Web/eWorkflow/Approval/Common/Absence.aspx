<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="Absence.aspx.cs" Inherits="Approval_Common_Absence" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
            function OpenPopupAbsence()
            {
                //window.radopen("./PopupAbsence.aspx", "RadWndAbsence");

                var oWnd = $find("<%=RadWndAbsence.ClientID%>");
                oWnd.setUrl("./PopupAbsence.aspx");
                oWnd.show();
            }
        </script>
    </telerik:RadScriptBlock>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="Server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="Server">
    <telerik:RadWindowManager runat="server" ID="RadWindowManager">
        <Windows>
            <telerik:RadWindow ID="RadWndAbsence" VisibleStatusbar="false" runat="server" ShowContentDuringLoad="false" Title="Absence Register" Width="820px" Height="350px" Behaviors="Default" Skin="Metro" NavigateUrl="./PopupAbsence.aspx"></telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <button style="width: 100px; margin-bottom: 3px;" onclick="OpenPopupAbsence(); return false;">Absence</button>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="Server">
</asp:Content>

