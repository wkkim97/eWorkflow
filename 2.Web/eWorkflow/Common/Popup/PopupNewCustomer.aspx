<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="PopupNewCustomer.aspx.cs" Inherits="Common_Popup_PopupNewCustomer" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" Runat="Server">
	<script type="text/javascript">
		function CloseWnd(sender, args) {
			var oWnd = GetRadWindow();
			oWnd.close();
		}

		function fn_Save(sender, args) {
			var callBackFunction = Function.createDelegate(sender, function (argument) {

				if (argument) {
					var CheckedValue = '';
					var controls = $('#<%= divBU.ClientID %>').children();
					for (var i = 0; i < controls.length; i++) {
						var bu = controls[i];                                       //BU 
						if ($find(bu.id).get_checked()) {
							CheckedValue = $find(bu.id)._value;
						}
					}
					$('#<%= hhdBu.ClientID%>').val(CheckedValue);
					this.click();
				}
			});
			fn_OpenConfirm("신규로 등록 하시겠습니까?", callBackFunction);

			args.set_cancel(true);
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" Runat="Server">
	<telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
	<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
		<AjaxSettings>
			<telerik:AjaxSetting AjaxControlID="radDdlCompany">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="divBU" />
				</UpdatedControls>
			</telerik:AjaxSetting>
		</AjaxSettings>
	</telerik:RadAjaxManager>
	<h2>NEW Customer</h2>
	<br />
	<div class="doc_style" id="divContorls" runat="server">
		<div class="data_type1">
			<table>
				<colgroup>
					<col style="width: 40%;" />
					<col />
				</colgroup>
				<tbody>
					<tr>
						<th>Company</th>
						<td>
							<telerik:RadDropDownList ID="radDdlCompany" DefaultMessage="----Select----" runat="server" DropDownWidth="250px" AutoPostBack="true" OnItemSelected="radDdlCompany_ItemSelected" >
								<Items>
									<telerik:DropDownListItem Text="Bayer CropScience Ltd." Value="0963" />
									<telerik:DropDownListItem Text="Bayer Korea Ltd." Value="0695" />
									<telerik:DropDownListItem Text="Bayer Material Science Ltd." Value="1117" Visible="false" />
								</Items>
							</telerik:RadDropDownList>
						</td>
					</tr>
					<tr>
						<th>BU</th>
						<td>
							<div id="divBU" runat="server" style="display: inline-block; float: left"></div>
						</td>
					</tr>
					<tr>
						<th>Parvw</th>
						<td>
							<telerik:RadDropDownList ID="RadDdlParvw" DefaultMessage="----Select----" runat="server" DropDownWidth="200px" AutoPostBack="false">
								<Items>
									<telerik:DropDownListItem Text="IE" Value="IE" />
									<telerik:DropDownListItem Text="IF" Value="IF" />									
								</Items>
							</telerik:RadDropDownList>
						</td>
					</tr>
					<tr>
						<th>CHANNEL</th>
						<td>
							<telerik:RadDropDownList ID="radDdlChannel" DefaultMessage="----Select----" runat="server" DropDownWidth="200px" AutoPostBack="false">
								<Items>
									<telerik:DropDownListItem Text="FM" Value="FM" />
									<telerik:DropDownListItem Text="NH" Value="NH" />									
								</Items>
							</telerik:RadDropDownList>
						</td>
					</tr>
					<tr>
						<th>Customer Code</th>
						<td>
							<telerik:RadTextBox runat="server" ID="radTextCustomertCode" Width="100%"></telerik:RadTextBox>
						</td>
					</tr>
					<tr>
						<th>Customer Name</th>
						<td>
							<telerik:RadTextBox runat="server" ID="radTextCustomertName" Width="100%"></telerik:RadTextBox>
						</td>
					</tr>
					<tr>
						<th>Customer Korea Name</th>
						<td>
							<telerik:RadTextBox runat="server" ID="radTextCustomertKName" Width="100%"></telerik:RadTextBox>
						</td>
					</tr>
				</tbody>
			</table>
		</div>
	</div>
	<div class="align_center pt20">
		<telerik:RadButton ID="radBtnSave" runat="server" Text="SAVE"
			ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
			CssClass="btn btn-blue btn-size3 bold" OnClientClicking="fn_Save" OnClick="radBtnSave_Click">
		</telerik:RadButton>
		<telerik:RadButton ID="radBtnClose" runat="server" Text="CLOSE"
			ButtonType="LinkButton" EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false"
			CssClass="btn btn-darkgray btn-size3 bold" OnClientClicked="CloseWnd">
		</telerik:RadButton>
	</div>
	<input type="hidden" id="hhdBu" runat="server" />
</asp:Content>

