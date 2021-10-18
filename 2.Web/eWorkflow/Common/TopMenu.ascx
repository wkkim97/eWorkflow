<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TopMenu.ascx.cs" Inherits="Common_TopMenu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script src="/eWorks/Scripts/Common/TopMenu.min.js"></script>
<link href="/eWorks/Styles/css/menubar.css" rel="stylesheet" />
 <script type="text/javascript">
        function openpop() {
            var wnd = $find("<%= winPopupCity.ClientID %>");
             wnd.set_title('Organization')
             wnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close);
             wnd.set_height(800);
             wnd.set_width(540);
             wnd.setUrl("http://localhost:56680/eWorks/Board/FaceBook_ver3.aspx");
             wnd.show();
        }
        function OnClientClose() {
            
        }
    </script>
 <telerik:RadWindowManager ID="RadWindowManager1" runat="server" OnClientClose="OnClientClose">
        <Windows>           
            <telerik:RadWindow ID="winPopupCity" runat="server" NavigateUrl="http://localhost:56680/eWorks/Board/FaceBook.aspx" Title="User List" Modal="true" Width="450" Height="250" VisibleStatusbar="false" Skin="Metro">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

<div id="head">
    <div class="head_top">

        <img alt="Menu" id="m-menu-btn" style="width:24px;" src="/eWorks/Styles/images/menu.svg" >

        <h1><a id="hrefHome" runat="server"><img src="/eWorks/Styles/images/ci.png" alt="e-workflow"></a></h1>
        
        <h1 class="bayer_ci">
            <img src="/eWorks/Styles/images/bayer_small.png" alt="bayer">
        </h1>

        <div class="topmenu">
            <%--<p style="display:none;"> <a href="javascript:openRadWinNewRequest();">Request Test</a>&nbsp;&nbsp;</p>--%>
            <p>
                [ <span id="hspanOrgName" runat="server"></span> ] 
                <strong><span id="hspanUser" runat="server"></span></strong>&nbsp;&nbsp;<span id="spanExtendedMenu" runat="server" ></span>
				<a href="/eWorks/Manage/Authentication/LogOut.aspx">LogOut1</a>
                <a href="javascript:openpop()">[Org.]</a>
            </p>
        </div>

    </div>
    <div class="gnb_all2">
        
        <img id="m-close-btn" style="width:18px;" src="/eWorks/Styles/images/close.svg" alt="close">
        
        <ul id="ulMenubar" runat="server"> 
        </ul>
       
    </div>

</div>
<script>
    $('body').click(function (e) {
        if($('.gnb_all2').hasClass('active')) $('.gnb_all2').removeClass('active');
    });
    $('#m-menu-btn').click(function (e) {
        console.log("1")
        $('.gnb_all2').addClass('active');
        e.stopPropagation();
    });
    $('.gnb_all2').click(function (e) {
        e.stopPropagation();
    })
    $('#m-close-btn').click(function () {
        $('.gnb_all2').removeClass('active');
    });
    $(window).resize(function () {
        handleMenuHeight();
    });
    $(document).ready(function () {
        handleMenuHeight();
    });

    function handleMenuHeight() {
        var win_width = $(window).width();
        /*
        if (win_width < 1130) {
            var height = $('body').outerHeight() > $(window).height() ? $('body').outerHeight() : $(window).height();
            height = height < 927 ? 927 : height;
            $('.gnb_all2').css('height', height);
        } else {
            $('.gnb_all2').css('height', 'inherit');
        }*/
    }
</script>
<!-- //head -->
