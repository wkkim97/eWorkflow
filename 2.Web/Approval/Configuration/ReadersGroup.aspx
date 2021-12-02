<%@ Language="C#" MasterPageFile="~/Master/eWorks_Main.Master" AutoEventWireup="true" CodeBehind="ReadersGroup.aspx.cs" Inherits="Approval.Configuration.ReadersGroup" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="server">
	<style type="text/css">
		#GroupArea { display:table; }
		.readersGroup { display: table-row; }
		.cell  {display: table-cell; width:500px; background-color:yellow;  }
		.cell1 {display: table-cell; width:200px; background-color:aquamarine;}
		.RadButton { margin-left:5px; }
	</style>
	<telerik:RadCodeBlock runat="server" ID="rdbScripts">
		<script type="text/javascript">
			
			function fn_showModalUserList(sender, args) {
				var wnd = $find("<%= modalUserList.ClientID %>");
				//wnd.setUrl("eWorks/Configuration/PopupReadersGroup.aspx");
				wnd.show();
				sender.set_autoPostBack(false);
			}

		</script>
	</telerik:RadCodeBlock>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" runat="server">
	<h3>Readers Group List</h3>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" runat="server">
	
	<div id="GroupArea">

		<telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

		<div class="readersGroup">
			<div class="cell1">								
				<h2>Group List</h2>					
			</div>
			<div class="cell">
				<telerik:RadButton ID="RadReset" runat="server" Text="Reset">
					<Icon SecondaryIconCssClass="rbRefresh" SecondaryIconRight="4" SecondaryIconTop="3"></Icon>
				</telerik:RadButton>
				<telerik:RadButton ID="RadSave" runat="server" Text="Save">
					<Icon SecondaryIconCssClass="rbSave" SecondaryIconRight="4" SecondaryIconTop="3"></Icon>
				</telerik:RadButton>
				<telerik:RadButton ID="RadDelete" runat="server" Text="Delete">
					<Icon SecondaryIconCssClass="rbCancel" SecondaryIconRight="4" SecondaryIconTop="3"></Icon>
				</telerik:RadButton> 
			</div>
		</div>
		<div class="readersGroup">
			<div class="cell1">
				 * 목록 리스트
			</div>
			<div class="cell">
				<span>
					<telerik:RadTextBox Width="195px" ID="RadGroupName" runat="server" Label="Group Name : " EmptyMessage="type here" InvalidStyleDuration="100" AutoPostBack="true">
					</telerik:RadTextBox>
				</span>
				<span>
					<telerik:RadButton ID="RadAdd" runat="server" Text="ADD" OnClientClicked="fn_showModalUserList();" >
						<Icon SecondaryIconCssClass="rbAdd" SecondaryIconRight="4" SecondaryIconTop="3"></Icon>
					</telerik:RadButton>
					<telerik:RadButton ID="RadRemove" runat="server" Text="Remove">
						<Icon SecondaryIconCssClass="rbRemove" SecondaryIconRight="4" SecondaryIconTop="3"></Icon>
					</telerik:RadButton>
				</span>
				<div>
					<telerik:RadGrid runat="server" ID="RadGridReadersGroup" AllowPaging="true" AllowSorting="true" EnableEmbeddedSkins="false" AllowFilteringByColumn="false" GridLines="None" AllowMultiRowSelection="true" Skin="Default" AutoGenerateColumns="false">
						<MasterTableView ShowHeadersWhenNoRecords="true">
							<Columns>
								<telerik:GridBoundColumn DataField="USER_ID" HeaderText="USER_ID" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
								<telerik:GridBoundColumn DataField="FULL_NAME" HeaderText="NAME" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
								<telerik:GridButtonColumn ConfirmText="Delete this Item?" ButtonType="ImageButton" CommandName="Delete" Text="Delete">
									<HeaderStyle Width="10px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" CssClass="MyImageButton"></ItemStyle>
								</telerik:GridButtonColumn>
							</Columns>
						</MasterTableView>
						<ClientSettings>
							<Selecting AllowRowSelect="true" />
						</ClientSettings>
					</telerik:RadGrid>

					<%--ADD input--%>
					<telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
						<Windows>
							<telerik:RadWindow ID="modalUserList" runat="server" Width="500" Height="600" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" Skin="Default" NavigateUrl="eWorks/Configuration/PopupReadersGroup.aspx" Title="User List" Modal="true" AutoSize="true" VisibleStatusbar="false" VisibleTitlebar="true">
							</telerik:RadWindow>
						</Windows>
					</telerik:RadWindowManager>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" runat="server">
</asp:Content>
