/*
작성목적 : Web Root Path를 반환한다.
*/
function fn_GetWebServer(){
	try{
		return WEBSERVER;
	} catch(exception) {fn_OpenErrorMessage(exception.description);}
}
function fn_GetWebRoot(){
	try {
		// WEBROOT 는 FrameWork.Web.PageBase 에서 설정된다. 그게 없는 경우의 처리
		var WebRootValue = null;

		try { WebRootValue = WEBROOT; } catch (exception) { }
		//WEBROOT 이 있는 경우에는 그것을 return 한다.
		if (WebRootValue != null) {
			return WEBROOT;
		}

		//WEBROOT 가 없는 경우에는 document.location.href 의 정보를 사용한다.
		//아래부분은 좀더 봐야 할듯
		var strHref = document.location.href;
		var strPath;
		var arrPath;
		if(strHref.substring(0, 4).toUpperCase() == "HTTP") {strPath = strHref.substring(7, strHref.length);}
		else {strPath = strHref.substring(8, strHref.length);}
		arrPath = strPath.split("/");
		return "/" + arrPath[1] + "/";
	} catch(exception) {fn_OpenErrorMessage(exception.description);}
}
/*
작성목적	: Document Path를 반환한다. 
*/
function fn_DocumentPath() {
    try {
        var strHref = "";
        var strPath = "";
        var arrPath = null;
        strHref = document.location.href;
        arrPath = strHref.split("/");
        for (var i = 0 ; i < arrPath.length - 1 ; i++) { strPath += arrPath[i] + "/"; }
        return strPath;
    } catch (exception) { fn_OpenErrorMessage(exception.description); }
}
function fn_MessageBoxStyle() {
    return "dialogWidth:" + DIALOGWIDTH + "px;dialogHeight:" + (DIALOGHEIGHT - DIALOGSMALLHEIGHT) + "px;status=no;scroll=no";
}
/*
작성목적	: 에러 메시지 상자를 띠운다.
Parameter	: sInfo - 출력할 메시지
*/
function fn_OpenErrorMessage(strTemp) {
    try {
        fn_SetCursor(false);
        var strImsi;
        if (strTemp == null) { strImsi = document.getElementById("errorMessage").value; }
        else { strImsi = strTemp; }
       // fn_SetProgressbar(false);

        // TODO : OLD CODE
        window.showModalDialog(fn_GetWebRoot() + "Manage/Message/ErrorMessage.aspx", strImsi, fn_MessageBoxStyle());
        //  TODO : NEW CODE
        
       // window.parent.modalWin.ShowMessage(strImsi , (DIALOGHEIGHT - DIALOGSMALLHEIGHT), DIALOGWIDTH, "errorMessage");
    } catch (exception) { alert(exception.message); }
}

/*
작성목적	: 작업정보 상자를 띠운다.
Parameter	: sInfo - 출력할 메시지
*/
function fn_OpenInformation(sInfo, alertCallBackFn) {
    try { 
        radalert(sInfo, 330, 180, 'Information', alertCallBackFn); 
    } catch (exception) { fn_OpenErrorMessage(exception.description); }
}

/*
작성목적	: 질문 상자를 띠운다.
Parameter	: sInfo - 출력할 메시지
Return		: "ok", "cancel"
*/
function fn_OpenConfirm(sInfo, confirmCallBackFn) {
    try { 
        return radconfirm(sInfo, confirmCallBackFn, 330, 180, null, 'Confirm');  
    } catch (exception) { fn_OpenErrorMessage(exception.description); }
}


function fn_ModalConfirm(sInfo, arrfnc) {
    try {
        modalWin.ShowConfirmationMessage(sInfo, (DIALOGHEIGHT - DIALOGSMALLHEIGHT), DIALOGWIDTH, "알림", arrfnc);
    } catch (exception) { fn_OpenErrorMessage(exception.description); }
}

/*
작성목적	: 팝업창을 띠운다.
Parameter	: sUrl - 띠울 URL
			  sFrame - 띠울 Frame
			  sFeature - 창 속성
*/
function fn_OpenDialog(sUrl, sFrame, sFeature) {
    return window.open(sUrl, sFrame, sFeature);
}
/*
작성목적	: "답장,전체답장,전달"아이콘 클릭 시 Frame.aspx의 'ContentFrame'에서 페이지가 열린다. 
Parameter	: sUrl - 띠울 URL
			  sFrame - 띠울 Frame
			  sFeature - 창 속성
*/
function fn_LinkDialog(sUrl, sFrame, sFeature) {
    //        var getPath;
    //        getPath=fn_GetWebRoot() + sUrl + sFrame + sFeature;
    //        window.parent.frames['ContentFrame'].document.location.href  = getPath;
    window.parent.frames['ContentFrame'].document.location.href = sUrl;

}
/*
작성목적	: 팝업창을 띠운다.
Parameter	: sUrl - 띠울 URL
			  sFeature - 창 속성
*/
function fn_OpenModalDialog(sUrl, sParam, sFeature) {
    try {
        var strReturn = "";
        if (sFeature != null) { strReturn = window.showModalDialog(fn_GetWebRoot() + "Common/Dialog/ModalDialog.html?" + sUrl, sParam, sFeature); }
        else { strReturn = window.showModalDialog(fn_GetWebRoot() + "Common/Dialog/ModalDialog.html?" + sUrl, null, sParam); }
        // TODO : NEW CODE
        if (strReturn == undefined) strReturn = $("#winClosedReturn").val();
        return strReturn;
    } catch (exception) { fn_OpenErrorMessage(exception.description); }
}

/*
작성목적	: 문자열을 숫자로 변환한다.
Parameter	: sNum - 숫자 문자열
Return		: 숫자
*/
function fn_GetInt(sNum){
	try{
		for(var i = 0 ; i < sNum ; i++ ){
			if ( sNum.substring(0, 1) == 0 ) {sNum = sNum.substring(1, sNum.length);}
			else {return parseInt(sNum);}
		}
		return parseInt(sNum);
	} catch (exception) {}	
}
/*
작성목적	: 숫자를 2자리 문자열로 변환한다.
Parameter	: iNum - 2자리 이하 숫자
Return		: 2자리 숫자 문자열
*/
function fn_LeadingZero(iNum){
	var strReturn;
	try{
		if ( iNum < 10 ) {strReturn = "0" + iNum;}
		else {strReturn = "" + iNum;}
	} catch (exception) {}
	return strReturn;
}
/*
작성목적	: PostBack을 일으킨다
Parameter	: targetForm - Submit 대상 폼
			  eventTarget - Target Element
			  eventArgs - 이벤트 파라메터
*/
function fn_RaisePostBack(targetForm, eventTarget, eventArgs) {
    try {
        targetForm.__EVENTTARGET.value = eventTarget.split("$").join(":");
        targetForm.__EVENTARGUMENT.value = eventArgs;
        targetForm.submit();
    } catch (exception) { fn_OpenErrorMessage(exception.description); }
}
/*
작성목적	: 나모 웹에디터컨트롤에서 생성한 HTML을 인코딩한다
*/
function fn_encodeHtml(html) {
    try {
        var encodedHtml;
        encodedHtml = escape(html);
        encodedHtml = encodedHtml.replace(/\//g, "%2F");
        encodedHtml = encodedHtml.replace(/\?/g, "%3F");
        encodedHtml = encodedHtml.replace(/=/g, "%3D");
        encodedHtml = encodedHtml.replace(/&/g, "%26");
        encodedHtml = encodedHtml.replace(/@/g, "%40");
        return encodedHtml;
    } catch (exception) { fn_OpenErrorMessage(exception.description); }
}
/*
작성목적	: 나모 웹에디터컨트롤로 HTML 을 로드하기 위해서 디코딩한다
*/
function fn_decodeHtml(html) {
    try {
        var decodeHtml;
        decodeHtml = unescape(html);
        return decodeHtml;
    } catch (exception) { fn_OpenErrorMessage(exception.description); }
}
 
/*=======================================================================
Function명  : GetCookie(name)
내용        : 생성된 쿠키 가져오기
Parameter	: name -> 쿠키명
Return		: 쿠키값 
========================================================================*/
function GetCookie(name) {
    var aCookie = document.cookie.split("; ");
    for (var i = 0; i < aCookie.length; i++) {
        var aCrumb = aCookie[i].split("=");
        if (name == aCrumb[0])
            return aCrumb[1];
    }
    return null;
}

function getFirstChild(node) {
    if (node.nodeType == 1) return node;
    if (node.childNodes.length <= 0) return node;
    var child1 = node.firstChild;
    while (child1.nodeType != 1) {
        child1 = child1.nextSibling;
    }
    return child1;
}
function SearchEvent() {
    var func = SearchEvent.caller;
    while (func != null) {
        var arg = func.arguments[0];
        if (arg) {
            if (String(arg.constructor).indexOf('Event') > -1) {
                return arg
            }
        }
        func = func.caller;
    } return null
}

function callAjax(sPage, sParam, bAsync, sType) {
    var strReturnValue;
    $.ajax({
        type: "POST",
        url: sPage,
        data: sParam,
        async: bAsync,
        dataType: sType,
        success: function (data) {
            strReturnValue = data;
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status == 0) {
                console.log('You are offline!!n Please Check Your Network.');
            }
            else if (xhr.status == 200) {
                return strReturnValue = textStatus;
            }
        }
    });
    return strReturnValue;
}
 
function cancelEvent(e) {
    var pEvent = (typeof (e) == "undefined") ? window.event : e;
    if (typeof (pEvent.preventDefault) == "undefined") { pEvent.returnValue = false; }
    else { pEvent.preventDefault(); }
}
function cancelEventBubble(e) {
    if (typeof (e.stopPropagation) == "undefined") { e.cancelBubble = true; }
    else { e.stopPropagation(); }
}
function CHashTable() {
    var pHash = new Object();
    this.Append = function (key, value, type) {
        if (type != null) {
            switch (type.toUpperCase()) {
                case "CDATA":
                    value = "<![CDATA[" + value + "]]>";
                    break;
                case "ENC":
                    value = encodeURIComponent(value);
                    break;
            }
        }
        pHash[key] = value;
    }
    this.DicGetValue = function (key) {
        return pHash[key];
    }
    this.DicRemove = function (key) {
        var temp = pHash[key];
        delete pHash[key];
        return temp;
    }
    this.getKeys = function () {
        var keys = new Array();
        for (var key in pHash) { keys.push(key); }
        return keys;
    }
    this.getValues = function () {
        var values = new Array();
        for (var key in pHash) { values.push(pHash[key]); }
        return values;
    }
    this.SetDicToArray = function () { }
    this.GetString = function () {
        try {
            var params = "";
            for (var key in pHash) { params += key.toString() + "=" + pHash[key] + "&"; }
            return params.substring(0, params.length - 1);
        } catch (exception) { }
    }
    this.DicSetKeyArray = function (pArray) {
        var i = 0;
        for (i = 0; i < pArray.length; i++) {
            this.Append(pArray[i], "");
        }
    }
    this.GetKeyString = function () {
        try {
            var params = "";
            for (var key in pHash) { params += key.toString() + "&"; }
            return params.substring(0, params.length - 1);
        } catch (exception) { }
    }
    this.DicGetXml = function () { }
}
 
function GetTopMenuParam() {
    var menuid = fn_GetHtmlObject("hhdMenuID").value;
    return "parentMenuID=" + menuid;
}

function fn_Trim(sourceString) {
    var strResult;
    strResult = sourceString.replace(/\s/g, "");
    return strResult;
}
/*
내용 : String 의 양쪽공백을 모두 제거한다.
*/
function fn_RLTrim(strSource) {
    return strSource.replace(/(^\s*)|(\s*$)/g, "");
}



/*
작성목적	: 해당메세지 가져오기
Return	    : 메세지배열컬렉션
*/
function fn_Msg(messageID, subSystemType, parameters, callbackFnc) {
    try {
        //메세지 받아오기
        var strTemp;
        var arrTemp;

        arrTemp = fn_MsgT(subSystemType, messageID);

        //메세지 분기
        if (arrTemp != null) {
            strTemp = arrTemp[3];
            if (parameters != null) { //파라미터가 없는경우{	
                for (var i = 0 ; i < parameters.length ; i++) { strTemp = strTemp.replace("{" + i + "}", parameters[i]); }
            }
            switch (arrTemp[2]) {
                case "01":
                    fn_OpenInformation(strTemp + "|^|" + arrTemp[4]);
                    break;
                case "02":
                    if (callbackFnc != undefined)
                        fn_ModalConfirm(strTemp + "|^|" + arrTemp[4], callbackFnc);
                    else
                        return fn_OpenConfirm(strTemp + "|^|" + arrTemp[4]);
                    break;
                case "03":
                    fn_OpenErrorMessage(strTemp + "|^|" + arrTemp[4]);
                    break;
            }
        }
    } catch (exception) { fn_OpenErrorMessage(exception.description); }
}

/*
작성목적	: 해당메세지 가져오기
Return	    : 메세지배열컬렉션
*/
function fn_MsgT(subSystemType, messageID) {
    try {
        var sSubSystemType;
        if (subSystemType == null || subSystemType == "")
            sSubSystemType = "";
        else
            sSubSystemType = subSystemType;
        var sUrl = fn_GetWebRoot() + "Manage/Message/Msg.aspx?SubSystemType=" + sSubSystemType + "&MessageID=" + messageID;
        var sType = "xml";
        var pReturnValue = null;
        pReturnValue = callAjax(sUrl, "", false, sType);
        if (typeof (pReturnValue) == "undefine" || pReturnValue == null) {
            alert("no data error"); return null;
        }
        var oXmlDoc = pReturnValue;
        var oElemList = $(oXmlDoc).find("ROOT");
        var arrTemp = new Array(5);
        arrTemp[0] = $(oElemList).find("SubSystemType").text(); //SubSystemType
        arrTemp[1] = oElemList.find("MessageID").text(); //MessageID
        arrTemp[2] = oElemList.find("MessageType").text(); //MessageType
        arrTemp[3] = oElemList.find("DisplayMessage").text(); //DisplayMessage
        arrTemp[4] = oElemList.find("SummaryMessage").text(); //SummaryMessage

        return arrTemp;
    } catch (exception) { fn_OpenErrorMessage(exception.description); }
}

/************************************************************************
함수명  : fn_MsgMixed()
작성목적 : 
Parameter : subSystemType, messageID, parameters
Return  : String
작 성 자 : 닷넷소프트  
최초작성일 : 2011-08-18
수 정 자    :
최종작성일 : 
수정내역 : 다국어 처리
*************************************************************************/
function fn_MsgMixed(subSystemType, messageID, parameters) {
    try {
        var sSubSystemType;
        var sReturn;

        if (subSystemType == null || subSystemType == "")
            sSubSystemType = "";
        else
            sSubSystemType = subSystemType;

        var sUrl = fn_GetWebRoot() + "Manage/Message/Msg.aspx?SubSystemType=" + sSubSystemType + "&MessageID=" + messageID;
        var sType = "xml";
        var pReturnValue = null;
        pReturnValue = callAjax(sUrl, "", false, sType);
        if (typeof (pReturnValue) == "undefine" || pReturnValue == null) {
            alert("no data error"); return null;
        }
        var oXmlDoc = pReturnValue;
        var oElemList = $(oXmlDoc).find("ROOT");
        var arrTemp = new Array(5);
        arrTemp[0] = $(oElemList).find("SubSystemType").text(); //SubSystemType
        arrTemp[1] = oElemList.find("MessageID").text(); //MessageID
        arrTemp[2] = oElemList.find("MessageType").text(); //MessageType
        arrTemp[3] = oElemList.find("DisplayMessage").text(); //DisplayMessage
        arrTemp[4] = oElemList.find("SummaryMessage").text(); //SummaryMessage

        //닷넷소프트 김학수 다국어 Mixed 지원 기능 추가
        sReturn = arrTemp[3];
        if (parameters != null) {
            for (var i = 0; i < parameters.length; i++) { sReturn = sReturn.replace("{" + i + "}", parameters[i]); }
        }

        return sReturn;
    } catch (exception) { fn_OpenErrorMessage(exception.description); }
}


/*
작성목적	: 해당Dictionary 가져오기
Return	    : Dictionary 컬렉션
*/
function fn_Dic(dictionaryList) {
    try {
        var strImsi = replaceAll(dictionaryList, ",", "^");
        var sUrl = fn_GetWebRoot() + "Manage/Message/Dic.aspx?Dic=" + strImsi;
        var sType = "xml";
        var pReturnValue = null;
        pReturnValue = callAjax(sUrl, "", false, sType);
        if (typeof (pReturnValue) == "undefine" || pReturnValue == null) {
            alert("no data error"); return null;
        }
        var oXmlDoc = pReturnValue;
        var oElemList = $(oXmlDoc).find("ROOT>DNGridDic");
        var strJamsi = $(oElemList).text(); //Dictionary
        var arrTemp;
        strJamsi = strJamsi.substring(0, strJamsi.length - 1);
        arrTemp = strJamsi.split('^');
        return arrTemp;
    } catch (exception) { fn_OpenErrorMessage(exception.description); }
}

function callAjax(sPage, sParam, bAsync, sType) {
    var strReturnValue;
    $.ajax({
        type: "POST",
        url: sPage,
        data: sParam,
        async: bAsync,
        dataType: sType,
        success: function (data) {
            strReturnValue = data;
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status == 0) {
                console.log('You are offline!!n Please Check Your Network.');
            }
            else if (xhr.status == 200) {
                return strReturnValue = textStatus;
            }
        }
    });
    return strReturnValue;
}
/*
작성목적	: 사용자 정보보기(새창)
*/
function replaceAll(oldStr, findStr, repStr) {
    try {
        var srchNdx = 0;  // srchNdx will keep track of where in the whole line of oldStr are we searching.
        var newStr = "";  // newStr will hold the altered version of oldStr.
        while (oldStr.indexOf(findStr, srchNdx) != -1) {// As long as there are strings to replace, this loop will run. 
            newStr += oldStr.substring(srchNdx, oldStr.indexOf(findStr, srchNdx)); // Put it all the unaltered text from one findStr to the next findStr into newStr.
            newStr += repStr; // Instead of putting the old string, put in the new string instead. 
            srchNdx = (oldStr.indexOf(findStr, srchNdx) + findStr.length); // Now jump to the next chunk of text till the next findStr.           
        }
        newStr += oldStr.substring(srchNdx, oldStr.length); // Put whatever's left into newStr.             
        return newStr;
    } catch (exception) { fn_OpenErrorMessage(exception.description); }
}

function DictionaryClass() {
    var m_dictionary = new ActiveXObject("Scripting.Dictionary");

    var m_arrDicKeyArr;
    var m_arrDicArr;
    var m_nDicCnt;

    this.Append = DicAppend;
    this.GetString = DicGetString;
    this.GetXml = DicGetXml;

    // Dic에 Key, Value를 추가한다.    
    function DicAppend(key, val, valType) {
        try {
            var strKey = key;
            var strVal = val;
            var strValType = valType;

            //key 값이 없는 경우 리턴
            if (strKey == null) {
                fn_OpenErrorMessage("Dictionary Key값이 없습니다.");
                return;
            }
            // null인 경우 공백을 삽입
            if (strVal == null) { strVal = ""; }

            // value 타입이 있는 경우
            if (strValType != null) {
                switch (strValType.toUpperCase()) {
                    case "CDATA":
                        strVal = "<![CDATA[" + strVal + "]]>";
                        break;

                    case "ENC":
                        strVal = encodeURIComponent(strVal);
                        break;
                }
            }

            // 이미 key 값이 있는 경우 값을 Update 한다.
            if (m_dictionary.Exists(strKey)) {
                m_dictionary.Item(strKey) = strVal;
            } else {
                m_dictionary.add(strKey, strVal);
            }
        } catch (exception) { fn_OpenErrorMessage(exception.description); }
    }

    // 현재 Dic의 내용을 배열로 저장한다.
    function SetDicToArray(oDic) {
        try {
            m_arrDicKeyArr = new VBArray(m_dictionary.Keys()).toArray();
            m_arrDicArr = new VBArray(m_dictionary.Items()).toArray();
            m_nDicCnt = m_dictionary.Count;
        } catch (exception) { fn_OpenErrorMessage(exception.description); }
    }

    // Dic을 문자열로 반환한다.
    function DicGetString() {
        var params = ""
        try {
            // 배열로 저장
            SetDicToArray();
            for (var i = 0; i < m_nDicCnt; i++) {
                params += m_arrDicKeyArr[i] + "=" + m_arrDicArr[i] + "&";
            }
            Init();
            return params.substring(0, params.length - 1);
        } catch (exception) { fn_OpenErrorMessage(exception.description); }
    }

    // Dic을 XMl로 반환한다.
    function DicGetXml() {
        var params = ""
        try {
            // 배열로 저장
            SetDicToArray();
            for (var i = 0; i < m_nDicCnt; i++) {
                params += "<" + m_arrDicKeyArr[i] + ">" + m_arrDicArr[i] + "</" + m_arrDicKeyArr[i] + ">";
            }
            Init();
            return "<ROOT>" + params + "</ROOT>";
        } catch (exception) { fn_OpenErrorMessage(exception.description); }
    }

    // Dic을 초기화 한다.
    function Init() {
        try {
            if (m_dictionary.Count > 0) m_dictionary.RemoveAll();
            m_arrDicKeyArr = null;
            m_arrDicArr = null;
            m_nDicCnt = null;
        } catch (exception) { fn_OpenErrorMessage(exception.description); }
    }
}
