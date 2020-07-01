<%@ Page Language="C#" CodeFile="FaceBook.aspx.cs" Inherits="Facebook_in" AutoEventWireup="true" %>

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
 

</head>
 
<body> 
    <form id="form1" runat="server">
        444444444444444444
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radGrad1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radGrad1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div id="wrap">
         
             <telerik:RadGrid ID="radGrad1" runat="server" AllowPaging="true" AutoGenerateColumns="false" Height="700px" Width="720px"
                 AllowSorting="True" Skin="EXGrid" OnNeedDataSource="grdSearch_NeedDataSource_NEW" >
             <ClientSettings> 
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2"></Scrolling> 
             </ClientSettings> 
                    
             <MasterTableView>            

             <Columns>
               <telerik:GridTemplateColumn DataField="USER_ID" HeaderText="PHOTO" UniqueName="USER_ID"  HeaderStyle-Width="110px">
                    <itemtemplate>
                        <img src="<%# Eval("USER_ID") %>" width="100px" height="100%"/>
                    </itemtemplate>
               </telerik:GridTemplateColumn>
               <telerik:GridBoundColumn DataField="FULL_NAME" HeaderText="NAME" HeaderStyle-Width="300px">
                   <ItemStyle HorizontalAlign="left" />
               </telerik:GridBoundColumn>
               <telerik:GridBoundColumn DataField="CONTACT" HeaderText="Contact"   HeaderStyle-Width="290px">
                   <ItemStyle HorizontalAlign="left" />
               </telerik:GridBoundColumn>
               
             </Columns>
             </MasterTableView>
             </telerik:RadGrid>
    </div> 
            333333333333333333333
    </form> 
</body> 
</html>