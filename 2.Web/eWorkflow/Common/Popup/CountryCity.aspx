<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="CountryCity.aspx.cs" Inherits="Common_Popup_CountryCity" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <script type="text/javascript">
        function returnToParent(sender, args) {

            var item = $find('<%= radDdlCity.ClientID %>').get_selectedItem();
            if (item) {
                var city = item.get_value();
                GetRadWindow().close(city);
            }



        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.radWindow;
            else if (window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function cancelAndClose(sender, args) {
            var oWindow = GetRadWindow();
            oWindow.close();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" runat="Server">
    <div class="align_center pt20">
        <telerik:RadDropDownList ID="radDdlCountry" runat="server" DefaultMessage="---Select---" DataTextField="COUNTRY" DataValueField="COUNTRY" AutoPostBack="true"
            DropDownHeight="150px"
            OnSelectedIndexChanged="radDdlCountry_SelectedIndexChanged">
        </telerik:RadDropDownList>
        <telerik:RadDropDownList ID="radDdlCity" runat="server" DefaultMessage="---Select---" DataTextField="CITY" DataValueField="CITY"
            DropDownHeight="150px">
        </telerik:RadDropDownList>
    </div>
    <br />
    <br />
    <br />
    <br />
    <div class="align_center pt20">
        <telerik:RadButton ID="radBtnOk" runat="server" Text="Ok"
            EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" ButtonType="LinkButton" CssClass="btn btn-blue btn-size3 bold"
            OnClientClicked="returnToParent">
        </telerik:RadButton>
        <telerik:RadButton ID="radBtnCancel" runat="server" Text="Cancel"
            EnableEmbeddedBaseStylesheet="false" EnableEmbeddedSkins="false" ButtonType="LinkButton" CssClass="btn btn-blue btn-size3 bold"
            OnClientClicked="cancelAndClose">
        </telerik:RadButton>
    </div>

</asp:Content>

