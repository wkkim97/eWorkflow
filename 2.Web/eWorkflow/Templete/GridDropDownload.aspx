<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GridDropDownload.aspx.cs" Inherits="Templete_GridDropDownload" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
          <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
<telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False">
  <MasterTableView>
    <Columns>
      <telerik:GridBoundColumn UniqueName="ContactName" ReadOnly="True" HeaderText="ContactName"
        DataField="ContactName" />
        <telerik:GridBoundColumn UniqueName="TABLE_NAME" DataField="TABLE_NAME" ></telerik:GridBoundColumn>
      <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Country">
        <ItemTemplate>
          <asp:Label ID="Label1" runat="server"> 
          </asp:Label>
        </ItemTemplate>
        <EditItemTemplate>
          <asp:DropDownList ID="List1" runat="server" />
        </EditItemTemplate>
      </telerik:GridTemplateColumn>
      <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" />
    </Columns>
  </MasterTableView>
</telerik:RadGrid>
    </form>
</body>
</html>
