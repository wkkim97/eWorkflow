
function fn_GetWebServer() {
    try {
        return WEBSERVER
    } catch (n) {
        fn_OpenErrorMessage(n.description)
    }
}

function fn_GetWebRoot() {
    var t, n, i, r;
    try {
        t = null;
        try {
            t = WEBROOT
        } catch (u) { }
        return t != null ? WEBROOT : (n = document.location.href, i = n.substring(0, 4).toUpperCase() == "HTTP" ? n.substring(7, n.length) : n.substring(8, n.length), r = i.split("/"), "/" + r[1] + "/")
    } catch (u) {
        fn_OpenErrorMessage(u.description)
    }
}

function fn_DocumentPath() {
    var n;
    try {
        var i = "",
            r = "",
            t = null;
        for (i = document.location.href, t = i.split("/"), n = 0; n < t.length - 1; n++) r += t[n] + "/";
        return r
    } catch (u) {
        fn_OpenErrorMessage(u.description)
    }
}

function fn_MessageBoxStyle() {
    return "dialogWidth:" + DIALOGWIDTH + "px;dialogHeight:" + (DIALOGHEIGHT - DIALOGSMALLHEIGHT) + "px;status=no;scroll=no"
}

function fn_OpenErrorMessage(n) {
    try {
        fn_SetCursor(!1);
        var t;
        t = n == null ? document.getElementById("errorMessage").value : n;
        alert(t);
        //window.showModalDialog(fn_GetWebRoot() + "Manage/Message/ErrorMessage.aspx", t, "dialogWidth:" + DIALOGWIDTH + "px;dialogHeight:350px;status=no;scroll=no")
    } catch (i) {
        alert(i.message)
    }
}

function fn_OpenInformation(n, t) {
    try {
        if ($telerik.isIE8) {
            function i() {
                radalert(n, 330, 180, "Information*", t);
                Sys.Application.remove_load(i)
            }
            Sys.Application.add_load(i)
        } else radalert(n, 330, 180, "Information", t)
    } catch (r) {
        fn_OpenErrorMessage(r.description)
    }
}

function fn_OpenDocInformation(n, t) {
    try {
        radalert(n, 330, 180, "Information", t)
    } catch (i) {
        fn_OpenErrorMessage(i.description)
    }
}

function fn_OpenConfirm(n, t) {
    try {
        return radconfirm(n, t, 330, 180, null, "Confirm")
    } catch (i) {
        fn_OpenErrorMessage(i.description)
    }
}

function fn_ModalConfirm(n, t) {
    try {
        modalWin.ShowConfirmationMessage(n, DIALOGHEIGHT - DIALOGSMALLHEIGHT, DIALOGWIDTH, "알림", t)
    } catch (i) {
        fn_OpenErrorMessage(i.description)
    }
}

function fn_OpenDialog(n, t, i) {
    return window.open(n, t, i)
}

function fn_LinkDialog(n) {
    window.parent.frames.ContentFrame.document.location.href = n
}

function fn_OpenModalDialog(n, t, i) {
    try {
        var r = "";
        return r = i != null ? window.showModalDialog(fn_GetWebRoot() + "Common/Dialog/ModalDialog.html?" + n, t, i) : window.showModalDialog(fn_GetWebRoot() + "Common/Dialog/ModalDialog.html?" + n, null, t), r == undefined && (r = $("#winClosedReturn").val()), r
    } catch (u) {
        fn_OpenErrorMessage(u.description)
    }
}

function fn_GetInt(n) {
    try {
        for (var t = 0; t < n; t++)
            if (n.substring(0, 1) == 0) n = n.substring(1, n.length);
            else return parseInt(n);
        return parseInt(n)
    } catch (i) { }
}

function fn_LeadingZero(n) {
    var t;
    try {
        t = n < 10 ? "0" + n : "" + n
    } catch (i) { }
    return t
}

function fn_RaisePostBack(n, t, i) {
    try {
        n.__EVENTTARGET.value = t.split("$").join(":");
        n.__EVENTARGUMENT.value = i;
        n.submit()
    } catch (r) {
        fn_OpenErrorMessage(r.description)
    }
}

function fn_encodeHtml(n) {
    try {
        var t;
        return t = escape(n), t = t.replace(/\//g, "%2F"), t = t.replace(/\?/g, "%3F"), t = t.replace(/=/g, "%3D"), t = t.replace(/&/g, "%26"), t.replace(/@/g, "%40")
    } catch (i) {
        fn_OpenErrorMessage(i.description)
    }
}

function fn_decodeHtml(n) {
    try {
        return unescape(n)
    } catch (t) {
        fn_OpenErrorMessage(t.description)
    }
}

function GetCookie(n) {
    for (var i, r = document.cookie.split("; "), t = 0; t < r.length; t++)
        if (i = r[t].split("="), n == i[0]) return i[1];
    return null
}

function getFirstChild(n) {
    if (n.nodeType == 1 || n.childNodes.length <= 0) return n;
    for (var t = n.firstChild; t.nodeType != 1;) t = t.nextSibling;
    return t
}

function SearchEvent() {
    for (var n = SearchEvent.caller, t; n != null;) {
        if (t = n.arguments[0], t && String(t.constructor).indexOf("Event") > -1) return t;
        n = n.caller
    }
    return null
}

function callAjax(n, t, i, r) {
    var u;
    return $.ajax({
        type: "POST",
        url: n,
        data: t,
        async: i,
        dataType: r,
        success: function (n) {
            u = n
        },
        error: function (n, t) {
            if (n.status == 0) console.log("You are offline!!n Please Check Your Network.");
            else if (n.status == 200) return u = t
        }
    }), u
}

function cancelEvent(n) {
    var t = typeof n == "undefined" ? window.event : n;
    typeof t.preventDefault == "undefined" ? t.returnValue = !1 : t.preventDefault()
}

function cancelEventBubble(n) {
    typeof n.stopPropagation == "undefined" ? n.cancelBubble = !0 : n.stopPropagation()
}

function CHashTable() {
    var n = {};
    this.Append = function (t, i, r) {
        if (r != null) switch (r.toUpperCase()) {
            case "CDATA":
                i = "<![CDATA[" + i + "]\]>";
                break;
            case "ENC":
                i = encodeURIComponent(i)
        }
        n[t] = i
    };
    this.DicGetValue = function (t) {
        return n[t]
    };
    this.DicRemove = function (t) {
        var i = n[t];
        return delete n[t], i
    };
    this.getKeys = function () {
        var t = [];
        for (var i in n) t.push(i);
        return t
    };
    this.getValues = function () {
        var t = [];
        for (var i in n) t.push(n[i]);
        return t
    };
    this.SetDicToArray = function () { };
    this.GetString = function () {
        var t, i;
        try {
            t = "";
            for (i in n) t += i.toString() + "=" + n[i] + "&";
            return t.substring(0, t.length - 1)
        } catch (r) { }
    };
    this.DicSetKeyArray = function (n) {
        for (var t = 0, t = 0; t < n.length; t++) this.Append(n[t], "")
    };
    this.GetKeyString = function () {
        var t, i;
        try {
            t = "";
            for (i in n) t += i.toString() + "&";
            return t.substring(0, t.length - 1)
        } catch (r) { }
    };
    this.DicGetXml = function () { }
}

function GetTopMenuParam() {
    var n = fn_GetHtmlObject("hhdMenuID").value;
    return "parentMenuID=" + n
}

function fn_Trim(n) {
    return n.replace(/\s/g, "")
}

function fn_RLTrim(n) {
    return n.replace(/(^\s*)|(\s*$)/g, "")
}

function fn_Msg(n, t, i, r) {
    var f, u, e;
    try {
        if (u = fn_MsgT(t, n), u != null) {
            if (f = u[3], i != null)
                for (e = 0; e < i.length; e++) f = f.replace("{" + e + "}", i[e]);
            switch (u[2]) {
                case "01":
                    fn_OpenInformation(f + "|^|" + u[4]);
                    break;
                case "02":
                    if (r != undefined) fn_ModalConfirm(f + "|^|" + u[4], r);
                    else return fn_OpenConfirm(f + "|^|" + u[4]);
                    break;
                case "03":
                    fn_OpenErrorMessage(f + "|^|" + u[4])
            }
        }
    } catch (o) {
        fn_OpenErrorMessage(o.description)
    }
}

function fn_MsgT(n, t) {
    var f;
    try {
        f = n == null || n == "" ? "" : n;
        var e = fn_GetWebRoot() + "Manage/Message/Msg.aspx?SubSystemType=" + f + "&MessageID=" + t,
            u = null;
        if (u = callAjax(e, "", !1, "xml"), typeof u == "undefine" || u == null) return alert("no data error"), null;
        var o = u,
            r = $(o).find("ROOT"),
            i = new Array(5);
        return i[0] = $(r).find("SubSystemType").text(), i[1] = r.find("MessageID").text(), i[2] = r.find("MessageType").text(), i[3] = r.find("DisplayMessage").text(), i[4] = r.find("SummaryMessage").text(), i
    } catch (s) {
        fn_OpenErrorMessage(s.description)
    }
}

function fn_MsgMixed(n, t, i) {
    var s, e, f;
    try {
        s = n == null || n == "" ? "" : n;
        var h = fn_GetWebRoot() + "Manage/Message/Msg.aspx?SubSystemType=" + s + "&MessageID=" + t,
            o = null;
        if (o = callAjax(h, "", !1, "xml"), typeof o == "undefine" || o == null) return alert("no data error"), null;
        var c = o,
            u = $(c).find("ROOT"),
            r = new Array(5);
        if (r[0] = $(u).find("SubSystemType").text(), r[1] = u.find("MessageID").text(), r[2] = u.find("MessageType").text(), r[3] = u.find("DisplayMessage").text(), r[4] = u.find("SummaryMessage").text(), e = r[3], i != null)
            for (f = 0; f < i.length; f++) e = e.replace("{" + f + "}", i[f]);
        return e
    } catch (l) {
        fn_OpenErrorMessage(l.description)
    }
}

function fn_Dic(n) {
    try {
        var r = replaceAll(n, ",", "^"),
            u = fn_GetWebRoot() + "Manage/Message/Dic.aspx?Dic=" + r,
            t = null;
        if (t = callAjax(u, "", !1, "xml"), typeof t == "undefine" || t == null) return alert("no data error"), null;
        var f = t,
            e = $(f).find("ROOT>DNGridDic"),
            i = $(e).text();
        return i = i.substring(0, i.length - 1), i.split("^")
    } catch (o) {
        fn_OpenErrorMessage(o.description)
    }
}

function callAjax(n, t, i, r) {
    var u;
    return $.ajax({
        type: "POST",
        url: n,
        data: t,
        async: i,
        dataType: r,
        success: function (n) {
            u = n
        },
        error: function (n, t) {
            if (n.status == 0) console.log("You are offline!!n Please Check Your Network.");
            else if (n.status == 200) return u = t
        }
    }), u
}

function replaceAll(n, t, i) {
    try {
        for (var r = 0, u = ""; n.indexOf(t, r) != -1;) u += n.substring(r, n.indexOf(t, r)), u += i, r = n.indexOf(t, r) + t.length;
        return u + n.substring(r, n.length)
    } catch (f) {
        fn_OpenErrorMessage(f.description)
    }
}

function DictionaryClass() {
    function e(t, i, r) {
        try {
            var f = t,
                u = i,
                e = r;
            if (f == null) {
                fn_OpenErrorMessage("Dictionary Key값이 없습니다.");
                return
            }
            if (u == null && (u = ""), e != null) switch (e.toUpperCase()) {
                case "CDATA":
                    u = "<![CDATA[" + u + "]\]>";
                    break;
                case "ENC":
                    u = encodeURIComponent(u)
            }
            n.Exists(f) ? n.Item(f) = u : n.add(f, u)
        } catch (o) {
            fn_OpenErrorMessage(o.description)
        }
    }

    function u() {
        try {
            t = new VBArray(n.Keys()).toArray();
            i = new VBArray(n.Items()).toArray();
            r = n.Count
        } catch (u) {
            fn_OpenErrorMessage(u.description)
        }
    }

    function o() {
        var e = "",
            n;
        try {
            for (u(), n = 0; n < r; n++) e += t[n] + "=" + i[n] + "&";
            return f(), e.substring(0, e.length - 1)
        } catch (o) {
            fn_OpenErrorMessage(o.description)
        }
    }

    function s() {
        var e = "",
            n;
        try {
            for (u(), n = 0; n < r; n++) e += "<" + t[n] + ">" + i[n] + "<\/" + t[n] + ">";
            return f(), "<ROOT>" + e + "<\/ROOT>"
        } catch (o) {
            fn_OpenErrorMessage(o.description)
        }
    }

    function f() {
        try {
            n.Count > 0 && n.RemoveAll();
            t = null;
            i = null;
            r = null
        } catch (u) {
            fn_OpenErrorMessage(u.description)
        }
    }
    var n = new ActiveXObject("Scripting.Dictionary"),
        t, i, r;
    this.Append = e;
    this.GetString = o;
    this.GetXml = s
}

function GetRadWindow() {
    var n = null;
    return window.radWindow ? n = window.radWindow : window.frameElement == null ? n = window : window.frameElement.radWindow && (n = window.frameElement.radWindow), n
}

function fn_FileDownload(n) {
    var t = null;
    try {
        t = "/eWorks/Common/AttachFileDownload.aspx?IDX=" + n;
        frames.filedownframe.location.href = t
    } catch (i) {
        fn_OpenErrorMessage(i.description)
    }
}

function TruncateDecimals(n, t) {
    var i = n.toString(),
        r = i.indexOf("."),
        f = r == -1 ? i.length : 1 + r + t,
        u = i.substr(0, f),
        e = isNaN(u) ? 0 : u;
    return parseFloat(e)
}

function fn_OnGridKeyPress(n, t) {
    var i = t.which ? t.which : event.keyCode,
        u = 0,
        e = $(n).attr("DecimalDigits"),
        r, f, o;
    if (e && (u = parseInt(e)), r = $(n).attr("AllowNegative"), r = r == "true" ? !0 : !1, r) {
        if (i != 45)
            if (u == 0) {
                if ((i == 46 || i > 31) && (i < 48 || i > 57)) return !1
            } else if (i != 46 && i > 31 && (i < 48 || i > 57)) return !1
    } else if (u == 0) {
        if ((i == 46 || i > 31) && (i < 48 || i > 57)) return !1
    } else if (i != 46 && i > 31 && (i < 48 || i > 57)) return !1;
    return (f = event.srcElement.value, o = /^\d*[.]\d*$/, o.test(f) && i == 46) ? !1 : r && f.substr(0, 1) == "-" && i == 45 ? !1 : void 0
}

function fn_ShowDocument(n, t, i, r) {
    try {
        if (n == "" || n == undefined || n == "&nbsp;") {
            fn_OpenInformation("폼양식 페이지가 정의되지 않았습니다.");
            return
        }
        var u = 935,
            f = 680,
            e = screen.width / 2 - u / 2,
            o = screen.height / 2 - f / 2 - 10,
            s = fn_GetWebRoot() + "Approval/Document/" + n + "?processid=" + i + "&documentid=" + t + "&reuse=" + r;
        //IT resource 때문에 추가 됨
        if (n.indexOf("https") >= 0) {
            window.open(n, "_blank");
        } else {
            window.open(s, "", "width=" + u + "px, height=" + f + "px, top=" + o + "px, left=" + e + "px,location=no,titlebar=no,status=no,scrollbars=yes,menubar=no,toolbar=no,directories=no,resizable=no,copyhistory=no")
        }
    } catch (h) {
        fn_OpenErrorMessage(h.description)
    }
}

function endsWith(n, t) {
    return n.indexOf(t, n.length - t.length) !== -1
}

function addThousandsSeparator(n, t) {
    var r = n,
        i;
    return parseFloat(n) && (n = new String(parseFloat(n).toFixedDown(t)), i = n.split("."), i[0] = i[0].split("").reverse().join("").replace(/(\d{3})(?!$)/g, "$1,").split("").reverse().join(""), i[1] ? t == 0 ? r = i.join(".") : t == 1 ? r = i[1] == "0" ? i[0] : i.join(".") : t == 2 ? (endsWith(i[1], "0") && (i[1] = i[1].substring(0, 1)), r = i.join(".")) : t == 3 && (endsWith(i[1], "00") ? i[1] = i[1].substring(0, 1) : endsWith(i[1], "0") && (i[1] = i[1].substring(0, 2)), r = i.join(".")) : r = i.join(".")), r.length < 1 && (r = "0"), r
}

function fn_OnGridNumFocus(n) {
    var t = n.value;
    n.value = t.replace(/,/gi, "").replace(/ /gi, "");
    $(n).select()
}

function setNumberFormat(n) {
    var r = 0,
        u = $(n).attr("DecimalDigits"),
        t, i;
    u && (r = parseInt(u));
    t = n.value;
    i = 0;
    !isNaN(parseFloat(t)) && isFinite(t) && (i = n.value);
    n.value = addThousandsSeparator(i, r)
}
Number.prototype.toFixedDown = function (n) {
    var i = new RegExp("(\\d+\\.\\d{" + n + "})(\\d)"),
        t = this.toString().match(i);
    return t ? parseFloat(t[1]) : this.valueOf()
};
var htmlUtil = function () {
    function u() {
        t = {};
        n = {};
        f({
            "&amp;": "&",
            "&gt;": ">",
            "&lt;": "<",
            "&quot;": '"',
            "&#39;": "'"
        })
    }

    function f(u) {
        var o = [],
            s = [],
            f, e;
        for (f in u) e = u[f], n[f] = e, t[e] = f, o.push(e), s.push(f);
        i = new RegExp("(" + o.join("|") + ")", "g");
        r = new RegExp("(" + s.join("|") + "|&#[0-9]{1,5};)", "g")
    }

    function e(n) {
        var r = function (n, i) {
            return t[i]
        };
        return n ? String(n).replace(i, r) : n
    }

    function o(t) {
        var i = function (t, i) {
            return i in n ? n[i] : String.fromCharCode(parseInt(i.substr(2), 10))
        };
        return t ? String(t).replace(r, i) : t
    }
    var i, r, t, n;
    return u(), {
        htmlEncode: e,
        htmlDecode: o
    }
}();


///*
//작성목적 : Web Root Path를 반환한다.
//*/
//function fn_GetWebServer() {
//    try {
//        return WEBSERVER;
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
//function fn_GetWebRoot() {
//    try {
//        // WEBROOT 는 FrameWork.Web.PageBase 에서 설정된다. 그게 없는 경우의 처리
//        var WebRootValue = null;
//
//        try { WebRootValue = WEBROOT; } catch (exception) { }
//        //WEBROOT 이 있는 경우에는 그것을 return 한다.
//        if (WebRootValue != null) {
//            return WEBROOT;
//        }
//
//        //WEBROOT 가 없는 경우에는 document.location.href 의 정보를 사용한다.
//        //아래부분은 좀더 봐야 할듯
//        var strHref = document.location.href;
//        var strPath;
//        var arrPath;
//        if (strHref.substring(0, 4).toUpperCase() == "HTTP") { strPath = strHref.substring(7, strHref.length); }
//        else { strPath = strHref.substring(8, strHref.length); }
//        arrPath = strPath.split("/");
//        return "/" + arrPath[1] + "/";
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
///*
//작성목적	: Document Path를 반환한다. 
//*/
//function fn_DocumentPath() {
//    try {
//        var strHref = "";
//        var strPath = "";
//        var arrPath = null;
//        strHref = document.location.href;
//        arrPath = strHref.split("/");
//        for (var i = 0 ; i < arrPath.length - 1 ; i++) { strPath += arrPath[i] + "/"; }
//        return strPath;
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
//function fn_MessageBoxStyle() {
//    return "dialogWidth:" + DIALOGWIDTH + "px;dialogHeight:" + (DIALOGHEIGHT - DIALOGSMALLHEIGHT) + "px;status=no;scroll=no";
//}
///*
//작성목적	: 에러 메시지 상자를 띠운다.
//Parameter	: sInfo - 출력할 메시지
//*/
//function fn_OpenErrorMessage(strTemp) {
//    try {
//        fn_SetCursor(false);
//        var strImsi;
//        if (strTemp == null) { strImsi = document.getElementById("errorMessage").value; }
//        else { strImsi = strTemp; }
//        // fn_SetProgressbar(false);
//
//        // TODO : OLD CODE
//        window.showModalDialog(fn_GetWebRoot() + "Manage/Message/ErrorMessage.aspx", strImsi, "dialogWidth:" + DIALOGWIDTH + "px;dialogHeight:350px;status=no;scroll=no");
//        //  TODO : NEW CODE
//
//        // window.parent.modalWin.ShowMessage(strImsi , (DIALOGHEIGHT - DIALOGSMALLHEIGHT), DIALOGWIDTH, "errorMessage");
//    } catch (exception) { alert(exception.message); }
//}
//
///*
//작성목적	: 작업정보 상자를 띠운다.
//Parameter	: sInfo - 출력할 메시지
//*/
//function fn_OpenInformation(sInfo, alertCallBackFn) {
//    try {
//        if ($telerik.isIE8) {
//            function f() { radalert(sInfo, 330, 180, 'Information*', alertCallBackFn); 
//            Sys.Application.remove_load(f); } Sys.Application.add_load(f)
//        } else
//            radalert(sInfo, 330, 180, 'Information', alertCallBackFn);
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
//
//function fn_OpenDocInformation(sInfo, alertCallBackFn) {
//    try {
//        radalert(sInfo, 330, 180, 'Information', alertCallBackFn);
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
//
///*
//작성목적	: 질문 상자를 띠운다.
//Parameter	: sInfo - 출력할 메시지
//Return		: "ok", "cancel"
//*/
//function fn_OpenConfirm(sInfo, confirmCallBackFn) {
//    try {
//        return radconfirm(sInfo, confirmCallBackFn, 330, 180, null, 'Confirm');
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
//
//
//function fn_ModalConfirm(sInfo, arrfnc) {
//    try {
//        modalWin.ShowConfirmationMessage(sInfo, (DIALOGHEIGHT - DIALOGSMALLHEIGHT), DIALOGWIDTH, "알림", arrfnc);
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
//
///*
//작성목적	: 팝업창을 띠운다.
//Parameter	: sUrl - 띠울 URL
//			  sFrame - 띠울 Frame
//			  sFeature - 창 속성
//*/
//function fn_OpenDialog(sUrl, sFrame, sFeature) {
//    return window.open(sUrl, sFrame, sFeature);
//}
///*
//작성목적	: "답장,전체답장,전달"아이콘 클릭 시 Frame.aspx의 'ContentFrame'에서 페이지가 열린다. 
//Parameter	: sUrl - 띠울 URL
//			  sFrame - 띠울 Frame
//			  sFeature - 창 속성
//*/
//function fn_LinkDialog(sUrl, sFrame, sFeature) {
//    //        var getPath;
//    //        getPath=fn_GetWebRoot() + sUrl + sFrame + sFeature;
//    //        window.parent.frames['ContentFrame'].document.location.href  = getPath;
//    window.parent.frames['ContentFrame'].document.location.href = sUrl;
//
//}
///*
//작성목적	: 팝업창을 띠운다.
//Parameter	: sUrl - 띠울 URL
//			  sFeature - 창 속성
//*/
//function fn_OpenModalDialog(sUrl, sParam, sFeature) {
//    try {
//        var strReturn = "";
//        if (sFeature != null) { strReturn = window.showModalDialog(fn_GetWebRoot() + "Common/Dialog/ModalDialog.html?" + sUrl, sParam, sFeature); }
//        else { strReturn = window.showModalDialog(fn_GetWebRoot() + "Common/Dialog/ModalDialog.html?" + sUrl, null, sParam); }
//        // TODO : NEW CODE
//        if (strReturn == undefined) strReturn = $("#winClosedReturn").val();
//        return strReturn;
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
//
///*
//작성목적	: 문자열을 숫자로 변환한다.
//Parameter	: sNum - 숫자 문자열
//Return		: 숫자
//*/
//function fn_GetInt(sNum) {
//    try {
//        for (var i = 0 ; i < sNum ; i++) {
//            if (sNum.substring(0, 1) == 0) { sNum = sNum.substring(1, sNum.length); }
//            else { return parseInt(sNum); }
//        }
//        return parseInt(sNum);
//    } catch (exception) { }
//}
///*
//작성목적	: 숫자를 2자리 문자열로 변환한다.
//Parameter	: iNum - 2자리 이하 숫자
//Return		: 2자리 숫자 문자열
//*/
//function fn_LeadingZero(iNum) {
//    var strReturn;
//    try {
//        if (iNum < 10) { strReturn = "0" + iNum; }
//        else { strReturn = "" + iNum; }
//    } catch (exception) { }
//    return strReturn;
//}
///*
//작성목적	: PostBack을 일으킨다
//Parameter	: targetForm - Submit 대상 폼
//			  eventTarget - Target Element
//			  eventArgs - 이벤트 파라메터
//*/
//function fn_RaisePostBack(targetForm, eventTarget, eventArgs) {
//    try {
//        targetForm.__EVENTTARGET.value = eventTarget.split("$").join(":");
//        targetForm.__EVENTARGUMENT.value = eventArgs;
//        targetForm.submit();
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
///*
//작성목적	: 나모 웹에디터컨트롤에서 생성한 HTML을 인코딩한다
//*/
//function fn_encodeHtml(html) {
//    try {
//        var encodedHtml;
//        encodedHtml = escape(html);
//        encodedHtml = encodedHtml.replace(/\//g, "%2F");
//        encodedHtml = encodedHtml.replace(/\?/g, "%3F");
//        encodedHtml = encodedHtml.replace(/=/g, "%3D");
//        encodedHtml = encodedHtml.replace(/&/g, "%26");
//        encodedHtml = encodedHtml.replace(/@/g, "%40");
//        return encodedHtml;
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
///*
//작성목적	: 나모 웹에디터컨트롤로 HTML 을 로드하기 위해서 디코딩한다
//*/
//function fn_decodeHtml(html) {
//    try {
//        var decodeHtml;
//        decodeHtml = unescape(html);
//        return decodeHtml;
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
//
///*=======================================================================
//Function명  : GetCookie(name)
//내용        : 생성된 쿠키 가져오기
//Parameter	: name -> 쿠키명
//Return		: 쿠키값 
//========================================================================*/
//function GetCookie(name) {
//    var aCookie = document.cookie.split("; ");
//    for (var i = 0; i < aCookie.length; i++) {
//        var aCrumb = aCookie[i].split("=");
//        if (name == aCrumb[0])
//            return aCrumb[1];
//    }
//    return null;
//}
//
//function getFirstChild(node) {
//    if (node.nodeType == 1) return node;
//    if (node.childNodes.length <= 0) return node;
//    var child1 = node.firstChild;
//    while (child1.nodeType != 1) {
//        child1 = child1.nextSibling;
//    }
//    return child1;
//}
//function SearchEvent() {
//    var func = SearchEvent.caller;
//    while (func != null) {
//        var arg = func.arguments[0];
//        if (arg) {
//            if (String(arg.constructor).indexOf('Event') > -1) {
//                return arg
//            }
//        }
//        func = func.caller;
//    } return null
//}
//
//function callAjax(sPage, sParam, bAsync, sType) {
//    var strReturnValue;
//    $.ajax({
//        type: "POST",
//        url: sPage,
//        data: sParam,
//        async: bAsync,
//        dataType: sType,
//        success: function (data) {
//            strReturnValue = data;
//        },
//        error: function (xhr, textStatus, errorThrown) {
//            if (xhr.status == 0) {
//                console.log('You are offline!!n Please Check Your Network.');
//            }
//            else if (xhr.status == 200) {
//                return strReturnValue = textStatus;
//            }
//        }
//    });
//    return strReturnValue;
//}
//
//function cancelEvent(e) {
//    var pEvent = (typeof (e) == "undefined") ? window.event : e;
//    if (typeof (pEvent.preventDefault) == "undefined") { pEvent.returnValue = false; }
//    else { pEvent.preventDefault(); }
//}
//function cancelEventBubble(e) {
//    if (typeof (e.stopPropagation) == "undefined") { e.cancelBubble = true; }
//    else { e.stopPropagation(); }
//}
//function CHashTable() {
//    var pHash = new Object();
//    this.Append = function (key, value, type) {
//        if (type != null) {
//            switch (type.toUpperCase()) {
//                case "CDATA":
//                    value = "<![CDATA[" + value + "]]>";
//                    break;
//                case "ENC":
//                    value = encodeURIComponent(value);
//                    break;
//            }
//        }
//        pHash[key] = value;
//    }
//    this.DicGetValue = function (key) {
//        return pHash[key];
//    }
//    this.DicRemove = function (key) {
//        var temp = pHash[key];
//        delete pHash[key];
//        return temp;
//    }
//    this.getKeys = function () {
//        var keys = new Array();
//        for (var key in pHash) { keys.push(key); }
//        return keys;
//    }
//    this.getValues = function () {
//        var values = new Array();
//        for (var key in pHash) { values.push(pHash[key]); }
//        return values;
//    }
//    this.SetDicToArray = function () { }
//    this.GetString = function () {
//        try {
//            var params = "";
//            for (var key in pHash) { params += key.toString() + "=" + pHash[key] + "&"; }
//            return params.substring(0, params.length - 1);
//        } catch (exception) { }
//    }
//    this.DicSetKeyArray = function (pArray) {
//        var i = 0;
//        for (i = 0; i < pArray.length; i++) {
//            this.Append(pArray[i], "");
//        }
//    }
//    this.GetKeyString = function () {
//        try {
//            var params = "";
//            for (var key in pHash) { params += key.toString() + "&"; }
//            return params.substring(0, params.length - 1);
//        } catch (exception) { }
//    }
//    this.DicGetXml = function () { }
//}
//
//function GetTopMenuParam() {
//    var menuid = fn_GetHtmlObject("hhdMenuID").value;
//    return "parentMenuID=" + menuid;
//}
//
//function fn_Trim(sourceString) {
//    var strResult;
//    strResult = sourceString.replace(/\s/g, "");
//    return strResult;
//}
///*
//내용 : String 의 양쪽공백을 모두 제거한다.
//*/
//function fn_RLTrim(strSource) {
//    return strSource.replace(/(^\s*)|(\s*$)/g, "");
//}
//
//
//
///*
//작성목적	: 해당메세지 가져오기
//Return	    : 메세지배열컬렉션
//*/
//function fn_Msg(messageID, subSystemType, parameters, callbackFnc) {
//    try {
//        //메세지 받아오기
//        var strTemp;
//        var arrTemp;
//
//        arrTemp = fn_MsgT(subSystemType, messageID);
//
//        //메세지 분기
//        if (arrTemp != null) {
//            strTemp = arrTemp[3];
//            if (parameters != null) { //파라미터가 없는경우{	
//                for (var i = 0 ; i < parameters.length ; i++) { strTemp = strTemp.replace("{" + i + "}", parameters[i]); }
//            }
//            switch (arrTemp[2]) {
//                case "01":
//                    fn_OpenInformation(strTemp + "|^|" + arrTemp[4]);
//                    break;
//                case "02":
//                    if (callbackFnc != undefined)
//                        fn_ModalConfirm(strTemp + "|^|" + arrTemp[4], callbackFnc);
//                    else
//                        return fn_OpenConfirm(strTemp + "|^|" + arrTemp[4]);
//                    break;
//                case "03":
//                    fn_OpenErrorMessage(strTemp + "|^|" + arrTemp[4]);
//                    break;
//            }
//        }
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
//
///*
//작성목적	: 해당메세지 가져오기
//Return	    : 메세지배열컬렉션
//*/
//function fn_MsgT(subSystemType, messageID) {
//    try {
//        var sSubSystemType;
//        if (subSystemType == null || subSystemType == "")
//            sSubSystemType = "";
//        else
//            sSubSystemType = subSystemType;
//        var sUrl = fn_GetWebRoot() + "Manage/Message/Msg.aspx?SubSystemType=" + sSubSystemType + "&MessageID=" + messageID;
//        var sType = "xml";
//        var pReturnValue = null;
//        pReturnValue = callAjax(sUrl, "", false, sType);
//        if (typeof (pReturnValue) == "undefine" || pReturnValue == null) {
//            alert("no data error"); return null;
//        }
//        var oXmlDoc = pReturnValue;
//        var oElemList = $(oXmlDoc).find("ROOT");
//        var arrTemp = new Array(5);
//        arrTemp[0] = $(oElemList).find("SubSystemType").text(); //SubSystemType
//        arrTemp[1] = oElemList.find("MessageID").text(); //MessageID
//        arrTemp[2] = oElemList.find("MessageType").text(); //MessageType
//        arrTemp[3] = oElemList.find("DisplayMessage").text(); //DisplayMessage
//        arrTemp[4] = oElemList.find("SummaryMessage").text(); //SummaryMessage
//
//        return arrTemp;
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
//
///************************************************************************
//함수명  : fn_MsgMixed()
//작성목적 : 
//Parameter : subSystemType, messageID, parameters
//Return  : String
//작 성 자 : 닷넷소프트  
//최초작성일 : 2011-08-18
//수 정 자    :
//최종작성일 : 
//수정내역 : 다국어 처리
//*************************************************************************/
//function fn_MsgMixed(subSystemType, messageID, parameters) {
//    try {
//        var sSubSystemType;
//        var sReturn;
//
//        if (subSystemType == null || subSystemType == "")
//            sSubSystemType = "";
//        else
//            sSubSystemType = subSystemType;
//
//        var sUrl = fn_GetWebRoot() + "Manage/Message/Msg.aspx?SubSystemType=" + sSubSystemType + "&MessageID=" + messageID;
//        var sType = "xml";
//        var pReturnValue = null;
//        pReturnValue = callAjax(sUrl, "", false, sType);
//        if (typeof (pReturnValue) == "undefine" || pReturnValue == null) {
//            alert("no data error"); return null;
//        }
//        var oXmlDoc = pReturnValue;
//        var oElemList = $(oXmlDoc).find("ROOT");
//        var arrTemp = new Array(5);
//        arrTemp[0] = $(oElemList).find("SubSystemType").text(); //SubSystemType
//        arrTemp[1] = oElemList.find("MessageID").text(); //MessageID
//        arrTemp[2] = oElemList.find("MessageType").text(); //MessageType
//        arrTemp[3] = oElemList.find("DisplayMessage").text(); //DisplayMessage
//        arrTemp[4] = oElemList.find("SummaryMessage").text(); //SummaryMessage
//
//        //닷넷소프트 김학수 다국어 Mixed 지원 기능 추가
//        sReturn = arrTemp[3];
//        if (parameters != null) {
//            for (var i = 0; i < parameters.length; i++) { sReturn = sReturn.replace("{" + i + "}", parameters[i]); }
//        }
//
//        return sReturn;
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
//
//
///*
//작성목적	: 해당Dictionary 가져오기
//Return	    : Dictionary 컬렉션
//*/
//function fn_Dic(dictionaryList) {
//    try {
//        var strImsi = replaceAll(dictionaryList, ",", "^");
//        var sUrl = fn_GetWebRoot() + "Manage/Message/Dic.aspx?Dic=" + strImsi;
//        var sType = "xml";
//        var pReturnValue = null;
//        pReturnValue = callAjax(sUrl, "", false, sType);
//        if (typeof (pReturnValue) == "undefine" || pReturnValue == null) {
//            alert("no data error"); return null;
//        }
//        var oXmlDoc = pReturnValue;
//        var oElemList = $(oXmlDoc).find("ROOT>DNGridDic");
//        var strJamsi = $(oElemList).text(); //Dictionary
//        var arrTemp;
//        strJamsi = strJamsi.substring(0, strJamsi.length - 1);
//        arrTemp = strJamsi.split('^');
//        return arrTemp;
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
//
//function callAjax(sPage, sParam, bAsync, sType) {
//    var strReturnValue;
//    $.ajax({
//        type: "POST",
//        url: sPage,
//        data: sParam,
//        async: bAsync,
//        dataType: sType,
//        success: function (data) {
//            strReturnValue = data;
//        },
//        error: function (xhr, textStatus, errorThrown) {
//            if (xhr.status == 0) {
//                console.log('You are offline!!n Please Check Your Network.');
//            }
//            else if (xhr.status == 200) {
//                return strReturnValue = textStatus;
//            }
//        }
//    });
//    return strReturnValue;
//}
///*
//작성목적	: 사용자 정보보기(새창)
//*/
//function replaceAll(oldStr, findStr, repStr) {
//    try {
//        var srchNdx = 0;  // srchNdx will keep track of where in the whole line of oldStr are we searching.
//        var newStr = "";  // newStr will hold the altered version of oldStr.
//        while (oldStr.indexOf(findStr, srchNdx) != -1) {// As long as there are strings to replace, this loop will run. 
//            newStr += oldStr.substring(srchNdx, oldStr.indexOf(findStr, srchNdx)); // Put it all the unaltered text from one findStr to the next findStr into newStr.
//            newStr += repStr; // Instead of putting the old string, put in the new string instead. 
//            srchNdx = (oldStr.indexOf(findStr, srchNdx) + findStr.length); // Now jump to the next chunk of text till the next findStr.           
//        }
//        newStr += oldStr.substring(srchNdx, oldStr.length); // Put whatever's left into newStr.             
//        return newStr;
//    } catch (exception) { fn_OpenErrorMessage(exception.description); }
//}
//
//function DictionaryClass() {
//    var m_dictionary = new ActiveXObject("Scripting.Dictionary");
//
//    var m_arrDicKeyArr;
//    var m_arrDicArr;
//    var m_nDicCnt;
//
//    this.Append = DicAppend;
//    this.GetString = DicGetString;
//    this.GetXml = DicGetXml;
//
//    // Dic에 Key, Value를 추가한다.    
//    function DicAppend(key, val, valType) {
//        try {
//            var strKey = key;
//            var strVal = val;
//            var strValType = valType;
//
//            //key 값이 없는 경우 리턴
//            if (strKey == null) {
//                fn_OpenErrorMessage("Dictionary Key값이 없습니다.");
//                return;
//            }
//            // null인 경우 공백을 삽입
//            if (strVal == null) { strVal = ""; }
//
//            // value 타입이 있는 경우
//            if (strValType != null) {
//                switch (strValType.toUpperCase()) {
//                    case "CDATA":
//                        strVal = "<![CDATA[" + strVal + "]]>";
//                        break;
//
//                    case "ENC":
//                        strVal = encodeURIComponent(strVal);
//                        break;
//                }
//            }
//
//            // 이미 key 값이 있는 경우 값을 Update 한다.
//            if (m_dictionary.Exists(strKey)) {
//                m_dictionary.Item(strKey) = strVal;
//            } else {
//                m_dictionary.add(strKey, strVal);
//            }
//        } catch (exception) { fn_OpenErrorMessage(exception.description); }
//    }
//
//    // 현재 Dic의 내용을 배열로 저장한다.
//    function SetDicToArray(oDic) {
//        try {
//            m_arrDicKeyArr = new VBArray(m_dictionary.Keys()).toArray();
//            m_arrDicArr = new VBArray(m_dictionary.Items()).toArray();
//            m_nDicCnt = m_dictionary.Count;
//        } catch (exception) { fn_OpenErrorMessage(exception.description); }
//    }
//
//    // Dic을 문자열로 반환한다.
//    function DicGetString() {
//        var params = ""
//        try {
//            // 배열로 저장
//            SetDicToArray();
//            for (var i = 0; i < m_nDicCnt; i++) {
//                params += m_arrDicKeyArr[i] + "=" + m_arrDicArr[i] + "&";
//            }
//            Init();
//            return params.substring(0, params.length - 1);
//        } catch (exception) { fn_OpenErrorMessage(exception.description); }
//    }
//
//    // Dic을 XMl로 반환한다.
//    function DicGetXml() {
//        var params = ""
//        try {
//            // 배열로 저장
//            SetDicToArray();
//            for (var i = 0; i < m_nDicCnt; i++) {
//                params += "<" + m_arrDicKeyArr[i] + ">" + m_arrDicArr[i] + "</" + m_arrDicKeyArr[i] + ">";
//            }
//            Init();
//            return "<ROOT>" + params + "</ROOT>";
//        } catch (exception) { fn_OpenErrorMessage(exception.description); }
//    }
//
//    // Dic을 초기화 한다.
//    function Init() {
//        try {
//            if (m_dictionary.Count > 0) m_dictionary.RemoveAll();
//            m_arrDicKeyArr = null;
//            m_arrDicArr = null;
//            m_nDicCnt = null;
//        } catch (exception) { fn_OpenErrorMessage(exception.description); }
//    }
//}
//
//function GetRadWindow() {
//    
//    var oWindow = null;
//    if (window.radWindow)
//        oWindow = window.radWindow;
//    else if (window.frameElement == null)
//        oWindow = window;
//    else if (window.frameElement.radWindow)
//        oWindow = window.frameElement.radWindow;
//    
//    return oWindow;
//}
//
//function fn_FileDownload(idx) {
//    //alert("333");
//    var strUrl = null;
//    try {
//
//        strUrl = "/eWorks/Common/AttachFileDownload.aspx?IDX=" + idx;
//        //single device - file
//        //frames["filedownframe"].location.href = strUrl;
//        window.open(strUrl);
//    }
//    catch (exception) {
//        //alert(exception);
//        fn_OpenErrorMessage(exception.description);
//    }
//}
//
//function TruncateDecimals(num, digits) {
//    var numS = num.toString(),
//        decPos = numS.indexOf('.'),
//        substrLength = decPos == -1 ? numS.length : 1 + decPos + digits,
//        trimmedResult = numS.substr(0, substrLength),
//        finalResult = isNaN(trimmedResult) ? 0 : trimmedResult;
//
//    return parseFloat(finalResult);
//}
//
//function fn_OnGridKeyPress(sender, evt) {
//    
//    var charCode = (evt.which) ? evt.which : event.keyCode;
//    
//    var digit = 0;
//    var digits = $(sender).attr('DecimalDigits');
//    if (digits) digit = parseInt(digits);
//    var allowNegative = $(sender).attr('AllowNegative');
//    if (allowNegative == 'true') allowNegative = true;
//    else allowNegative = false;
//    if (allowNegative) {
//        if (charCode != 45) {
//            if (digit == 0) {
//                if ((charCode == 46 || charCode > 31) && (charCode < 48 || charCode > 57))
//                    return false;
//
//            }
//            else {
//                if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
//                    return false;
//            }
//        }
//    } else {
//        if (digit == 0) {
//            if ((charCode == 46 || charCode > 31) && (charCode < 48 || charCode > 57))
//                return false;
//        }
//        else {
//            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
//                return false;
//        }
//    }
//    // Textbox value       
//    var _value = event.srcElement.value;
//    // 소수점(.)이 두번 이상 나오지 못하게
//    var _pattern0 = /^\d*[.]\d*$/; // 현재 value값에 소수점(.) 이 있으면 . 입력불가
//    if (_pattern0.test(_value)) {
//        if (charCode == 46) {
//            return false;
//        }
//    }
//    if (allowNegative) {
//        if (_value.substr(0, 1) == '-') {
//            if (charCode == 45) return false;
//        }
//    }
//}
//
//function fn_ShowDocument(formName, documentid, processid, reuse) {
//    try {
//        
//        if (formName == '' || formName == undefined || formName == '&nbsp;') {
//            fn_OpenInformation("폼양식 페이지가 정의되지 않았습니다.");
//            return;
//        }
//        var nWidth = 935;
//        var nHeight = 680;
//        var left = (screen.width / 2) - (nWidth / 2);
//        var top = (screen.height / 2) - (nHeight / 2) - 10;
//
//        var url = fn_GetWebRoot() + "Approval/Document/" + formName + "?processid=" + processid + "&documentid=" + documentid + "&reuse=" + reuse;
//        if (formname.indexOf("https") >= 0) url = formName;
//        window.open(url, "", "width=" + nWidth + "px, height=" + nHeight + "px, top=" + top + "px, left=" + left + "px,location=no,titlebar=no,status=no,scrollbars=yes,menubar=no,toolbar=no,directories=no,resizable=no,copyhistory=no");
//    }
//    catch (exception) {
//        fn_OpenErrorMessage(exception.description);
//    }
//}
//
//
//
//function endsWith(str, suffix) {
//    return str.indexOf(suffix, str.length - suffix.length) !== -1;
//}
//
//Number.prototype.toFixedDown = function (digits) {
//    var re = new RegExp("(\\d+\\.\\d{" + digits + "})(\\d)"),
//        m = this.toString().match(re);
//    return m ? parseFloat(m[1]) : this.valueOf();
//};
//
//function addThousandsSeparator(input, digit) {
//    var output = input;
//    if (parseFloat(input)) {
//        input = new String(parseFloat(input).toFixedDown(digit)); // so you can perform string operations
//        var parts = input.split("."); // remove the decimal part
//        parts[0] = parts[0].split("").reverse().join("").replace(/(\d{3})(?!$)/g, "$1,").split("").reverse().join("");
//        if (parts[1]) {
//            if (digit == 0) {
//                output = parts.join(".");
//            } else if (digit == 1) {
//                if (parts[1] == '0') output = parts[0];
//                else output = parts.join(".");
//            } else if (digit == 2) {
//                if (endsWith(parts[1], '0')) parts[1] = parts[1].substring(0, 1);
//                output = parts.join(".");
//            } else if (digit == 3) {
//                if (endsWith(parts[1], '00')) parts[1] = parts[1].substring(0, 1);
//                else if (endsWith(parts[1], '0')) parts[1] = parts[1].substring(0, 2);
//                output = parts.join(".");
//            }
//        } else
//            output = parts.join(".");
//    }
//    if (output.length < 1) output = '0';
//    return output;
//}
//
//function fn_OnGridNumFocus(sender) {
//    var value = sender.value;
//
//    sender.value = value.replace(/,/gi, '').replace(/ /gi, '');
//    $(sender).select();
//}
//
//function setNumberFormat(sender) {
//    var digit = 0;
//    var digits = $(sender).attr('DecimalDigits');
//    if (digits) digit = parseInt(digits);
//    var strNum = sender.value;
//    var number = 0;
//    if (!isNaN(parseFloat(strNum)) && isFinite(strNum))
//        number = sender.value;
//    sender.value = addThousandsSeparator(number, digit);
//
//}
//
//// HTML Encode, Decode 스크립트
//var htmlUtil = (function () {
//    var charToEntityRegex,
//        entityToCharRegex,
//        charToEntity,
//        entityToChar;
//
//    function resetCharacterEntities() {
//        charToEntity = {};
//        entityToChar = {};
//        // add the default set
//        addCharacterEntities({
//            '&amp;': '&',
//            '&gt;': '>',
//            '&lt;': '<',
//            '&quot;': '"',
//            '&#39;': "'"
//        });
//    }
//
//    function addCharacterEntities(newEntities) {
//        var charKeys = [],
//            entityKeys = [],
//            key, echar;
//        for (key in newEntities) {
//            echar = newEntities[key];
//            entityToChar[key] = echar;
//            charToEntity[echar] = key;
//            charKeys.push(echar);
//            entityKeys.push(key);
//        }
//        charToEntityRegex = new RegExp('(' + charKeys.join('|') + ')', 'g');
//        entityToCharRegex = new RegExp('(' + entityKeys.join('|') + '|&#[0-9]{1,5};' + ')', 'g');
//    }
//
//    function htmlEncode(value) {
//        var htmlEncodeReplaceFn = function (match, capture) {
//            return charToEntity[capture];
//        };
//
//        return (!value) ? value : String(value).replace(charToEntityRegex, htmlEncodeReplaceFn);
//    }
//
//    function htmlDecode(value) {
//        var htmlDecodeReplaceFn = function (match, capture) {
//            return (capture in entityToChar) ? entityToChar[capture] : String.fromCharCode(parseInt(capture.substr(2), 10));
//        };
//
//        return (!value) ? value : String(value).replace(entityToCharRegex, htmlDecodeReplaceFn);
//    }
//
//    resetCharacterEntities();
//
//    return {
//        htmlEncode: htmlEncode,
//        htmlDecode: htmlDecode
//    };
//})();
