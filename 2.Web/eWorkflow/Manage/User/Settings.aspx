<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Manage_User_Settings" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
	<script src="/eWorks/Scripts/Common/Settings.js"></script>
	<script type="text/javascript">
	    function fn_RowDropping(sender, args) {
	        if (sender.get_id() == "<%=grdDoclist.ClientID %>") {

		        var node = args.get_destinationHtmlElement();

		        if (!isChildOf('<%=grdDoclist.ClientID %>', node)) {
        		    args.set_cancel(true);
        		}

            }
        }

        function isChildOf(parentId, element) {
            while (element) {
                if (element.id && element.id.indexOf(parentId) > -1) {
                    return true;
                }
                element = element.parentNode;
            }
            return false;
        }
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" runat="Server">
	<telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server" Skin="Silk"></telerik:RadAjaxLoadingPanel>
	<telerik:RadAjaxManager ID="ajaxMgr" runat="server">
		<AjaxSettings>
			<telerik:AjaxSetting AjaxControlID="btnMainTypeSave">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="hdivMainview" LoadingPanelID="loadingPanel" />
				</UpdatedControls>
			</telerik:AjaxSetting>
			<telerik:AjaxSetting AjaxControlID="grdDoclist">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="grdDoclist" LoadingPanelID="loadingPanel" />
					<telerik:AjaxUpdatedControl ControlID="hddDocumentList" LoadingPanelID="loadingPanel" />
				</UpdatedControls>
			</telerik:AjaxSetting>
			<telerik:AjaxSetting AjaxControlID="btnOrderSave">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="grdDoclist" LoadingPanelID="loadingPanel" />
					<telerik:AjaxUpdatedControl ControlID="hddDocumentList" LoadingPanelID="loadingPanel" />
				</UpdatedControls>
			</telerik:AjaxSetting>
		</AjaxSettings>
	</telerik:RadAjaxManager>
	<h1>Configuration Settings</h1>
	<ul class="tab">
		<li class="on"><a href="#tabAreaMainView">Main View</a></li>
		<li><a href="#tabAreaDocOrder">Order Setting [Document LIst]</a></li>
		<li><a href="#tabAbsence">Delegation</a></li>
		<li><a href="#tabLoginHistory">Login History</a></li>
	</ul>
	<div id="tabAreaMainView" class="pt20">
		<div class="data_type1" id="hdivMainview" runat="server">
			<table>
				<tbody>
					<tr>
						<th>A Type</th>
						<th>B Type</th>
					</tr>
					<tr>
						<td>
							<input type="radio" class="radio" name="type" value="A" id="rdoTypeA" runat="server" />
							Select
						</td>
						<td>
							<input type="radio" class="radio" name="type" value="B" id="rdoTypeB" runat="server" />
							Select
						</td>
					</tr>
					<tr>
						<td>
							<p style="text-align: center;">
								<img src="/eWorks/Styles/Images/main_a_s.png" alt="Main A Type" />
							</p>
						</td>
						<td>
							<p style="text-align: center;">
								<img src="/eWorks/Styles/Images/main_b_s.png" alt="Main B Type" />
							</p>
						</td>
					</tr>
				</tbody>
			</table>
		</div>
		<div class="align_center pt20">
			<telerik:RadButton ID="btnMainTypeSave" OnClick="btnMainTypeSave_Click" CssClass="btn btn-blue btn-size3 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Text="Save" runat="server"></telerik:RadButton>
		</div>
	</div>
	<div id="tabAreaDocOrder" class="pt20">
		<div class="data_type1 pt20">
			<telerik:RadGrid runat="server" AllowPaging="True" ID="grdDoclist" OnRowDrop="grdDoclist_RowDrop" AllowMultiRowSelection="true" AutoGenerateColumns="False" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" AllowSorting="true" GridLines="None">
				<MasterTableView Width="100%" DataKeyNames="DOCUMENT_ID">
					<Columns>
						<telerik:GridDragDropColumn HeaderStyle-Width="18px" Visible="false">
						</telerik:GridDragDropColumn>
						<telerik:GridBoundColumn HeaderText="Document Name" DataField="DOC_NAME">
						</telerik:GridBoundColumn>
						<telerik:GridBoundColumn HeaderText="Sort Number" DataField="SORT">
						</telerik:GridBoundColumn>
					</Columns>
					<NoRecordsTemplate>
						<div style="height: 30px; cursor: pointer;">No items to view</div>
					</NoRecordsTemplate>
					<HeaderStyle HorizontalAlign="Left" />
				</MasterTableView>

				<ClientSettings AllowRowsDragDrop="True">

					<Selecting AllowRowSelect="True" EnableDragToSelectRows="false"></Selecting>

					<ClientEvents OnRowDropping="fn_RowDropping"></ClientEvents>

				</ClientSettings>

			</telerik:RadGrid>
		</div>
		<div class="align_center pt20">
			<telerik:RadButton ID="btnOrderSave" OnClick="btnOrderSave_Click" CssClass="btn btn-blue btn-size3 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Text="Save" runat="server"></telerik:RadButton>
		</div>
		<input type="hidden" id="hddDocumentList" runat="server" />
	</div>
	<div id="tabAbsence" class="pt20" style="width: 100%; height: 100%">
		<iframe class="data_type1 pt20" frameborder="0" style="width: 100%; height: 350px;" src="/eworks/Approval/Common/PopupAbsence.aspx"></iframe>
	</div>
	<div id="tabLoginHistory" class="pt20">
		<div class="data_type1 pt20">
			<telerik:RadGrid runat="server" AllowPaging="True" ID="RadGrdLoginHistory"  AutoGenerateColumns="False" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Skin="EXGrid" AllowSorting="true" GridLines="None" OnItemDataBound="RadGrdLoginHistory_ItemDataBound">
				<MasterTableView Width="100%">
					<Columns>						
						<telerik:GridBoundColumn HeaderText="IP Address" DataField="CLIENTIP" UniqueName="CLIENTIP" HeaderStyle-Width="30%">
						</telerik:GridBoundColumn>
						<telerik:GridBoundColumn HeaderText="PCNAME" DataField="WINDOWDOMAINNAME" UniqueName="WINDOWDOMAINNAME" Display="false" HeaderStyle-Width="30%">
						</telerik:GridBoundColumn>
						<telerik:GridBoundColumn HeaderText="Connection Date" DataField="CREATE_DATE" DataFormatString="{0:yyyy-MM-dd HH:mm tt}"  HeaderStyle-Width="40%">
						</telerik:GridBoundColumn>
						<telerik:GridBoundColumn HeaderText="Windows Account" DataField="WINDOWUSERNAME" HeaderStyle-Width="30%">
						</telerik:GridBoundColumn>
					</Columns>
					<NoRecordsTemplate>
						<div style="height: 30px; cursor: pointer;">No items to view</div>
					</NoRecordsTemplate>
					<HeaderStyle HorizontalAlign="Center" />
				</MasterTableView>
			</telerik:RadGrid>
		</div>
	</div>

</asp:Content>

