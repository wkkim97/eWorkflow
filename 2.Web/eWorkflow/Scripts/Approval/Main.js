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


function fn_GetDocCurrentIndex(index) {
    var $ulDoc = $("[id$=ulDocList]");
    return $ulDoc.attr("currentindex");
}

function fn_GetUlDoc() {
    return $("[id$=ulDocList]"); 
}



function fn_OnClickDocPrev()
{ 
    var cur = fn_GetDocCurrentIndex();
    var prev = parseInt(cur) - 1;
    if (prev == 0) {
        return;
    }
 
    $("#hdivDocArea_" + cur).css({ float: "left", opacity: 1 }).animate({ opacity: 0, left: "-500px" }, {
        duration: 500,
        step: function (now, fx) {
            $(this).css({ display: "none" });
        }
    });
    $("#hdivDocArea_" + prev).css({ float: "left", opacity: 0 }).animate({ opacity: 1, left: "500px" }, {
        duration: 500,
        step: function (now, fx) {
            $(this).css({ display: "" });
        }
    });;
    fn_SetDocCurrentIndex(prev);

}

function fn_OnClickDocNext() {
    var cur = fn_GetDocCurrentIndex();
    var next = parseInt(cur) + 1;
    if (next > fn_GetDocLastIndex()) {
        return;
    } else {
        
    }

    $("#hdivDocArea_" + cur).css({ opacity: 1 })
        .animate({ opacity: 0 },
            {
                duration: 500,
                step: function (now, fx) {
                $(this).css({ display: "none" });
            }
        });
    $("#hdivDocArea_" + next).css({ opacity: 0 }).animate({ opacity: 1, left: "500px" }, {
        duration: 500,
        step: function (now, fx) {
            $(this).css({ display: "" });
        }
    });;
    fn_SetDocCurrentIndex(next);
}

function fn_PageClick()
{
    // 임시저장
    if ( $(this).hasClass("m1") )
    {
        location.href = "/eWorks/Approval/List/SavedList.aspx";
    }
    // 결재대기
    else if ( $(this).hasClass("m2") )
    {
        location.href = "/eWorks/Approval/List/Todolist.aspx";
    }
    // 결재진행
    else if ($(this).hasClass("m3")) {
        location.href = "/eWorks/Approval/List/ApproveList.aspx";
    }
}

 
$(function () {
    $("#pDocPrev").bind({ click: fn_OnClickDocPrev });
    $("#pDocNext").bind({ click: fn_OnClickDocNext });
    $(".rig_con dd").bind({
        click: fn_PageClick,
        mouseover: function () { $(this).css({ cursor: "pointer" }) }
    });

    // Tootip 표시
    $("[id$=ulDocList]").find("li").bind(
            { 
                mouseout: function () {
                    fn_HideContent($(".doc_over"));
                }
            }
        );
 
    
    $("[id$=ulDocList]").find("li").hover(function () {
        var discription = $(this).attr("dec");

        if (discription.length > 0) {
            var pointLeft = $(this).width() - 60;
            var pointTop = $(this).height() - 30;
            var divRound = $(".main_left")[0];
            var $doc = $(".doc_over");

            $doc.html(htmlUtil.htmlDecode(discription));

            if (window.innerWidth < this.offsetLeft + divRound.offsetLeft + pointLeft + 330) {
                $doc.css({ left: (this.offsetLeft - pointLeft) + "px" });
                $doc.css({ top: (this.offsetTop + divRound.offsetTop + pointTop) + "px" });
                $doc.addClass('reflect');
            } else {
                $doc.css({ left: (this.offsetLeft + divRound.offsetLeft + pointLeft) + "px" });
                $doc.css({ top: (this.offsetTop + divRound.offsetTop + pointTop) + "px" });
                $doc.removeClass('reflect');
            }

            $doc.stop(true, true).fadeTo(400, 1);
        }
    });

    $(".main_left").bind(
        {
            mouseout: function () {
                fn_HideContent($(".doc_over"));
            }
        }
    );
});
  
function fn_HideContent(d) {
 
    d.stop(true, true).fadeOut(400, 0);
}
