<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Main.master" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="Approval_main"  %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" Runat="Server">
    <script src="/eWorks/Scripts/Approval/List.min.js"></script>
    <script src="/eWorks/Scripts/Approval/Main.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderHead" Runat="Server"> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HolderBody" Runat="Server">
    <!-- main_con -->
	<div id="main_con">

        <div class="main_left">
            <ul class="doc_list" id="ulDocList" runat="server">
            </ul>
            <p class="doc_prev" id="pDocPrev"  style="cursor:pointer">
                <img src="/eWorks/Styles/images/main_prev.png" alt="pre" /></p>
            <p class="doc_next"  id="pDocNext" style="cursor:pointer">
                <img src="/eWorks/Styles/images/main_next.png" alt="pre" /></p>

        </div>
        <div  class="main_right">
			<dl class="rig_con"> 
				<dd class="m1"><img src="/eWorks/Styles/images/main_txt1.png"  alt="임시저장" /><a href="#" class="num" style="align-items:center" id="aSavedCnt" runat="server">0</a></dd>
				<dd class="m2"><img src="/eWorks/Styles/images/main_txt2.png"  alt="결제대기" /><a href="#" class="num" style="align-items:center;" id="aTodoCnt" runat="server">0</a></dd>
				<dd class="m3"><img src="/eWorks/Styles/images/main_txt3.png"  alt="결제진행" /><a href="#" class="num" style="align-items:center;" id="aProcessCnt" runat="server">0</a></dd>
			</dl>
			<div class="notice">
				<ul id="ulNotice" runat="server">
					<h2>Notice</h2> 
				</ul>
				<p class="more"><a href="#" id="AddNotice" runat="server"><img src="/eWorks/Styles/images/main_more.png"  alt="more" /></a></p>
			</div>
		</div>
	
	</div> 
<%--    <telerik:RadWindow ID="winBoard" runat="server" Modal="true"
        VisibleStatusbar="false"
        VisibleTitlebar="true" >
    </telerik:RadWindow>--%>
    
    <!-- //main_con -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HolderBottom" Runat="Server">
</asp:Content>

