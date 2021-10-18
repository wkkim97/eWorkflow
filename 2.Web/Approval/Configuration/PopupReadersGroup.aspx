<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopupReadersGroup.aspx.cs" Inherits="Approval.Configuration.PopupReadersGroup" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>User List</title>
</head>
<body>
    <form id="form1" runat="server">
		<div>
			<telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
			<div>
				<telerik:radtextbox width="195px" id="RadSearch" runat="server" label="Search : " emptymessage="type here" autopostback="true">
				</telerik:radtextbox>
				<telerik:radbutton id="RadButton1" runat="server" text="OK">
					<Icon SecondaryIconCssClass="rbOk" SecondaryIconRight="4" SecondaryIconTop="3"></Icon>
				</telerik:radbutton>
			</div>
			<div>
				<telerik:RadGrid runat="server" ID="RadGridUserList" AllowPaging="true" AllowSorting="true" EnableEmbeddedSkins="false" AllowMultiRowSelection="true" AutoGenerateColumns="false" Culture="ko-KR">
					<MasterTableView ShowHeadersWhenNoRecords="true">
						<Columns>
							<telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn">
							<ItemTemplate>
							  <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="ToggleRowSelection"
								AutoPostBack="True" />
							</ItemTemplate>
							<HeaderTemplate>
							  <asp:CheckBox ID="headerChkbox" runat="server" OnCheckedChanged="ToggleSelectedState"
								AutoPostBack="True" />
							</HeaderTemplate>
						  </telerik:GridTemplateColumn>
							<telerik:GridBoundColumn DataField="USER_ID" HeaderText="USER_ID" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
							<telerik:GridBoundColumn DataField="FULL_NAME" HeaderText="NAME" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
						</Columns>
					</MasterTableView>
					<ClientSettings>
						<Selecting AllowRowSelect="true" />
					</ClientSettings>
				</telerik:RadGrid>
			</div>
			<div>
				<telerik:RadButton ID="RadOk" runat="server" Text="OK">
					<Icon SecondaryIconCssClass="rbOk" SecondaryIconRight="4" SecondaryIconTop="3"></Icon>
				</telerik:RadButton>
				<telerik:RadButton ID="RadCancel" runat="server" Text="Cancel">
					<Icon SecondaryIconCssClass="rbCancel" SecondaryIconRight="4" SecondaryIconTop="3"></Icon>
				</telerik:RadButton>
			</div>
		</div>
    </form>
</body>
</html>
