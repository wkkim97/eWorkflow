<%@ Page Language="C#" CodeFile="FaceBook_ver2.aspx.cs" Inherits="Facebook_in" AutoEventWireup="true" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="UTF-8"/>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Bayer e-Workflow</title>
	<meta content="IE=edge" http-equiv="X-UA-Compatible"/>
	<meta name="viewport" content="width=device-width, initial-scale=1"/>
	<meta name="description" content="Bayer, Bayer e-Workflow" />
	<meta name="keywords" content="Bayer, Bayer e-Workflow"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="shortcut icon" href="/eWorks/Styles/images/e_workflow_icon.ico"/>
    <link href="/eWorks/Styles/css/style.min.css" rel="stylesheet" />
    <script src="/eWorks/Scripts/jquery/jquery-1.11.1.min.js"></script>
     
    
    <style type="text/css">
        
        .categoryFieldset {
            float: left;
            width: 400px;
            height: 100px;
            margin-bottom:10px;
            margin-left:10px;
            padding:5px;
            border:1px dotted;
        }
        img{
            height:100px;
            width:100%;
        }
 

        .categoryDescription {
            width: 300px;
            
        }
        .categoryimage {
            width: 100px;
            
        }

        .itemSeparator {
            color: Olive; 
            font-weight: bold;
        }
        .shipperFieldset {
            width: 300px;
        } 
        .shipperCell {
            width: 50%;
        }
        .demo-container {
            max-width:1040px;
            margin-left:10px;
        }
        .categoryImage {

    width: 25%;

    text-align:right;

}
     
    </style>

</head>
 
<body> 
   
    <form id="form1" runat="server">
     <div class="demo-container">   
<h2>BCS Facebook</h2>
<br />
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecoratedControls="All" DecorationZoneID="demo-container" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ListViewPanel1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ListViewPanel1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" MinDisplayTime="0" />
        <asp:Panel ID="ListViewPanel1" runat="server">    

        <telerik:RadListView ID="RadListView2" runat="server" ItemPlaceholderID="ProductsHolder" OnNeedDataSource="RadListView2_NeedDataSource" AllowPaging="true" PageSize="10">            
            <ItemTemplate>
               <fieldset class="categoryFieldset">
               <table >
                        <tr>
                            <td class="categoryDescription">
                                <table >
                                    <tr>
                                        <td>
                                            <%# Eval("FULL_NAME")%><br />
                                            <%# Eval("CONTACT")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="categoryimage">
                                <img src="<%# Eval("USER_ID") %>" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </ItemTemplate>
        </telerik:RadListView> 
        </asp:Panel>
         <table cellpadding="0" cellspacing="6" width="100%">
            <tr>
                <td style="width: 76%">
                    <telerik:RadDataPager ID="RadDataPager1" runat="server" PagedControlID="RadListView2" 
                        PageSize="6">
                        <Fields>
                            <telerik:RadDataPagerButtonField FieldType="FirstPrev"></telerik:RadDataPagerButtonField>
                            <telerik:RadDataPagerButtonField FieldType="Numeric" PageButtonCount="5"></telerik:RadDataPagerButtonField>
                            <telerik:RadDataPagerButtonField FieldType="NextLast"></telerik:RadDataPagerButtonField>
                        </Fields>
                    </telerik:RadDataPager>
                </td>
                <td style="width: 24%;">
                  <%--  <asp:Label ID="lblSort1" AssociatedControlID="ddList1" runat="server" Text="Sort by:" CssClass="sortLabel"></asp:Label>

                    <telerik:RadComboBox ID="ddList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddList1_SelectedIndexChanged">

                        <Items>

                            <telerik:RadComboBoxItem Text="-Select field to sort-" Value=""></telerik:RadComboBoxItem>

                            <telerik:RadComboBoxItem Text="Name" Value="ProductName"></telerik:RadComboBoxItem>

                            <telerik:RadComboBoxItem Text="Quantity" Value="QuantityPerUnit"></telerik:RadComboBoxItem>

                            <telerik:RadComboBoxItem Text="Price" Value="UnitPrice"></telerik:RadComboBoxItem>

                            <telerik:RadComboBoxItem Text="Units" Value="UnitsInStock"></telerik:RadComboBoxItem>

                            <telerik:RadComboBoxItem Text="Available" Value="Discontinued"></telerik:RadComboBoxItem>

                        </Items>

                    </telerik:RadComboBox>--%>

                </td>

            </tr>

        </table>

    </div> 

      <%--<telerik:RadButton ID="radBtnDepartureCity" runat="server" AutoPostBack="false" OnClientClicked='openpop'
                                Width="16px" Height="16px" CssClass="btn_grid">
                                <Image ImageUrl="/eworks/Styles/images/ico_newwin.png" IsBackgroundImage="true" />
     </telerik:RadButton>
     <telerik:RadWindow ID="RadWindow1" runat="server" Width="700" Height="800" RestrictionZoneID="RestrictionZoneID"
        Behaviors="Default" VisibleOnPageLoad="true" NavigateUrl="http://localhost:56680/eWorks/Board/FaceBook.aspx"
        EnableShadow="true">

    </telerik:RadWindow>--%>

            
    </form> 
</body> 
</html>