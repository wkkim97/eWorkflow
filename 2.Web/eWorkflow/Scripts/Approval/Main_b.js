function fn_ShowBoard(idx) {
    if (idx == undefined) idx = '';
    var owin = GetRadWindowManager();
    owin.set_title('Notice');
    owin.set_visibleStatusbar(false);
    owin.setSize(700, 560);
    owin.add_close(fn_winBoardClose);
    owin.open("/eWorks/Board/Edit.aspx?idx=" + idx);
}

function fn_winBoardClose(sender, args) {
    if (args.get_argument() != null || args.get_argument() != undefined) {
        if (args.get_argument().returnValue) {
            document.location.reload();
        }
    }
}

function fn_SetDocCurrentIndex(index)
{
    var $ulDoc = $("[id$=ulDocList]");
    $ulDoc.attr("currentindex", index);
}

function fn_GetDocLastIndex() {
    var $ulDoc = $("[id$=ulDocList]");
    return $ulDoc.attr("lastindex");
}


function fn_GetDocCurrentIndex() {
    var $ulDoc = $("[id$=ulDocList]");
    return $ulDoc.attr("currentindex");
}

function fn_GetUlDoc() {
    return $("[id$=ulDocList]"); 
}
 
function fn_PageClick() {
    // 임시저장
    if ($(this).hasClass("m1")) {
        location.href = "/eWorks/Approval/List/SavedList.aspx";
    }
        // 결재대기
    else if ($(this).hasClass("m2")) {
        location.href = "/eWorks/Approval/List/Todolist.aspx";
    }
        // 결재진행
    else if ($(this).hasClass("m3")) {
        location.href = "/eWorks/Approval/List/ApproveList.aspx";
    }
}

function fn_OnClickDoc() {
    var target = $(this).attr("href");
    var curIndex = fn_GetDocCurrentIndex();

    $(".num a[index^=" + curIndex + "] img").attr("src", "../styles/images/ico_po_off.png").css({ opacity: 0.3 }).animate({ opacity: 1 }, { duration: 200 });

    var $doc = fn_GetUlDoc();
    $doc.find("div").css({ opacity: 1 })
        .animate({ opacity: 0 },
            {
                duration: 200,
                step: function (now, fx) {
                    $(this).css({ display: "none" });
                }
            });

    $(target).css({ opacity: 0 }).animate({ opacity: 1, left: "500px" }, {
        duration: 200,
        step: function (now, fx) {
            $(this).css({ display: "" });
        }
    });
    $(this).find("img").attr("src", "../styles/images/ico_po_on.png").css({ opacity: 0.3 }).animate({ opacity: 1 }, { duration: 200 });
    fn_SetDocCurrentIndex($(target).attr("index"));
}
 
$(function () {
    $(".num a").bind({
        click:fn_OnClickDoc  
    });

    $(".rig_con dd").bind({
        click: fn_PageClick,
        mouseover: function () { $(this).css({ cursor: "pointer" }) }
    });

});
 