<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="Withholding_Print.aspx.cs" Inherits="Withholding_Print_In" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" Runat="Server">
    <style>
        input.rgFilterBox {
            border-color: #84AD88;
font-weight: 100;
line-height: 23px;
color: #000;
height: 23px;
width: 90%;
padding: 2px 5px 3px;

        }


    </style>
    <script type="text/javascript">
        function FormLoad() {
            fn_InitControl();
        }

        
    </script>
    <script src="/eWorks/Scripts/Approval/List.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" Runat="Server">
    <telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="ajaxMgr" runat="server" OnAjaxRequest="Reporting_AdminDocumentList_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radGrad1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radGrad1" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>           
        </AjaxSettings>
   </telerik:RadAjaxManager>



 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" Runat="Server">
    <div id="content">
        <h2>WithHolding Tax Print</h2>
      
        <div class="board_list pt20" style="min-height:300px">
             <telerik:RadGrid ID="radGrad1" runat="server" AutoGenerateColumns="False" AllowPaging="True" 
                 AllowFilteringByColumn="true" 
                 AllowSorting="true" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" 
                 Skin="EXGrid" GridLines="None" AllowMultiRowSelection="false" 
                 OnItemDataBound="grdSearch_ItemDataBound" OnNeedDataSource="grdSearch_NeedDataSource" >
             <ClientSettings> 
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2"></Scrolling> 
             </ClientSettings> 
                    
             <MasterTableView>            

             <Columns>
               <telerik:GridDateTimeColumn DataField="TRANS_DATE" HeaderText="DATE" UniqueName="HireDate" 
                    HeaderStyle-Width="110px" DataFormatString="{0:yyyy-MM-dd}" AllowFiltering ="false" >   
                   <ItemStyle HorizontalAlign="left" />                
               </telerik:GridDateTimeColumn>

                 
               <telerik:GridHyperLinkColumn datatextFormatString="{0}" DataNavigateUrlFields ="IDX" UniqueName ="NAME" HeaderText="NAME"
                    DataNavigateUrlFormatString ="http://10.43.2.25/ph/PrintOutExcel_EW.asp?IDX={0}" DataTextField ="NAME"
                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                            ShowFilterIcon="false" HeaderStyle-Width="200px" Target="_blank">

               </telerik:GridHyperLinkColumn>
              
               <telerik:GridBoundColumn DataField="ID_NUM" HeaderText="ID NUM"   HeaderStyle-Width="200px" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                            ShowFilterIcon="false">
                   <ItemStyle HorizontalAlign="left" />
               </telerik:GridBoundColumn>
               <telerik:GridBoundColumn DataField="AMOUNT" HeaderText="AMOUNT"   HeaderStyle-Width="200px" AllowFiltering ="false"  >
                   <ItemStyle HorizontalAlign="left" />
               </telerik:GridBoundColumn>
               <telerik:GridBoundColumn DataField="VOUNO" HeaderText="VOUNO"   HeaderStyle-Width="200px" AllowFiltering ="false"  >
                   <ItemStyle HorizontalAlign="left" />
               </telerik:GridBoundColumn>
               
             </Columns>
             </MasterTableView>
             </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
