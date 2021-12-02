<%@ Page Language="C#" CodeFile="FaceBook_ver3.aspx.cs" Inherits="Facebook_in" AutoEventWireup="true" %>

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
            text-align: right;
        }
     
    </style>

</head>
 
<body> 

    <form id="form1" runat="server">

     <div class="demo-container">   
<h2>BCS Facebook</h2>

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
   <asp:Button Text="Export to PDF" ID="Button1" runat="server" OnClick="Button1_Click"></asp:Button>

    <telerik:RadTreeList ID="RadTreeList1" runat="server" OnNeedDataSource="RadTreeList1_NeedDataSource"
        ParentDataKeyNames="ORG" DataKeyNames="title" AllowPaging="true" PageSize="200" 
        AutoGenerateColumns="false" AllowSorting="true" Width="500px">
        <ExportSettings>

            <Pdf PageLeftMargin="20" PageRightMargin="20"></Pdf>

        </ExportSettings>
        <Columns>
            <telerik:TreeListBoundColumn DataField="title" UniqueName="title" HeaderText="ORG" Display="false">
            </telerik:TreeListBoundColumn>            
            <telerik:TreeListTemplateColumn DataField ="FULL_NAME" UniqueName="FULL_NAME" HeaderText="Organization">
                <ItemTemplate>
                      <table style="width:420px">
                          <colgroup>
    <col />
    <col />
  </colgroup>

                        <tr>
                            <td class="categoryDescription" style="width:300px">
                               
                                            <%# Eval("FULL_NAME")%>
                                            <br />
                                            <%# Eval("CONTACT")%>
                               
                            </td>
                            <td class="categoryimage">
                                <image src="<%# Eval("USER_ID") %>" onerror="this.src='/eWorks/Styles/images/No_Image.jpg';" />
                                
                               <%--<img src="/eWorks/Styles/images/bcs.png" />--%>
                            </td>
                        </tr>
                    </table>
                    

                </ItemTemplate>
            </telerik:TreeListTemplateColumn>
            
              <telerik:TreeListBoundColumn DataField="ORG" UniqueName="ORG" HeaderText="Category ID" Display="false">
            </telerik:TreeListBoundColumn>

            


        </Columns>
    
    </telerik:RadTreeList>


       

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