 
function fn_MenuClicked(sender, args) {
    linkURL(args.get_item().get_value());
}
 
function openRadWinNewRequest() {
    window.radopen("/eWorks/Approval/Common/PopupNewRequest.aspx", "RadWindowNewRequest");
}

function openSettings() {
    window.radopen("/eWorks/Manage/User/Settings.aspx", "winSettings");
}

$(document).ready(function () {
    $(".gnb_all2").extendedMenu();
});
 
$.fn.extendedMenu = function () {
    var el = $(this);

    /* for IE 6 */
    $("li", el).mouseover(function () {
        $(this).addClass("hover");
    }).mouseout(function () {
        $(this).removeClass("hover");
    });

    /*  keyboard accessible */
    $("a", el).focus(function () {
        $(this).parents("li").addClass("hover");
    }).blur(function () {
        $(this).parents("li").removeClass("hover");
    });
}