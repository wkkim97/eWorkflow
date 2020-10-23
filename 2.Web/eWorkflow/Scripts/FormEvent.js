function fn_WindowOnLoad(changeDomain){
      	
	try{
	    
	    // 포탈과 연동을 위해 Document.Domain을 일치 시킨다.
	    // Message, Confirm, Information은 제외(Open Modal에서 window.dialogArguments 전달이 않됨)
	    if(changeDomain != false)
	    {	
			//var docDomain = document.domain;	    
			//docDomain = docDomain.substr(docDomain.indexOf(".") + 1);	    
			//document.domain = docDomain;	
	    }
	
		// 1. 에러 메시지가 있는 경우 팝업 출력
		if ( document.getElementById("errorMessage").value.length > 0 )
			fn_OpenErrorMessage(document.getElementById("errorMessage").value);
		
		// 2. Information이 있는 경우 팝업 출력
		if ( document.getElementById("informationMessage").value.length > 0 )
			fn_OpenInformation(document.getElementById("informationMessage").value);
		
		// 3. Confirm이 있는 경우 팝업 출력
		if ( document.getElementById("confirmMessage").value.length > 0 )
			return fn_OpenConfirm(document.getElementById("confirmMessage").value);
        //alert("333333");
		// 4. 팝업창 닫기
        var strClose = ""
        strClose = document.getElementById("winClosed").value;
        console.log(strClose);
        if (strClose == "closed") {
            alert("windowonload");
			if ( document.getElementById("winClosedReturn").value.length > 0 )
					window.returnValue = document.getElementById("winClosedReturn").value;
			window.close();
			return;
		}
		// 5. 백스페이스로 이전 페이지로 가는 것을 막는다.
		//document.onkeydown = fn_PreventNavigateBack;
		
		// 6. 오른쪽 마우스 처리
	    //document.oncontextmenu = fn_PreventRightMouse;
	 
	}
	catch(exception){}
	
	try{
		// 7. 폼 Unload 확인
		document.onbeforeunload = fn_ClosingCheck;
		
		// 8. 사용자 정의 폼 로드 함수 호출
		FormLoad();
	 
	}
    catch (exception) {  } //위두함수에는 exception처리가 있으면 안됨.. 함수가 존재하지 않을수도 있기땜시...
}

/*****************************************
작성목적	: Window_OnUnLoad 이벤트 발생전에 발생되는 이벤트, 페이지 닫기 취소를 할 수 있다.
		  OnBeforeUnLoad 이벤트에 별도의 이벤트 핸들러를 연결하여 처리한다.
		  ex) window.onbeforeunload = fnClosingChecker;
*****************************************/
function fn_ClosingCheck(){
    try {
        var strMsg = FormBeforeUnLoad();
		if ( strMsg.length > 0 ){
			strMsg = "\n" + strMsg;
			return strMsg;
		}
	}
    catch (exception) { alert(exception.message);}
}

/*****************************************
작성목적	: 텍스트 박스 이외는 backspace 입력을 제한한다.
*****************************************/
function fn_PreventNavigateBack(){
	//작업은 하지만.... "뒤로"버튼을 누르면.. 우짜면 되지????
	//메뉴자체를 없애든지 해야할듯하네.... 메뉴자체를 없애면 또 반항할 무리들이 많을낀데... 고민스럽따..-_-
	var strTagType;
	if ( window.event.keyCode == 8 ){
		if ( window.event.srcElement.tagName.toUpperCase() == "INPUT" ){
			strTagType = window.event.srcElement.getAttribute("type").toUpperCase();
			if ( strTagType == "TEXT"){
				window.event.returnValue = true;
				return;
			}
		}
		else if( window.event.srcElement.tagName.toUpperCase() == "TEXTAREA" ){
			window.event.returnValue = true;
			return;
		}
		else{
			// 일단 TextArea에서의 벡스페이스가 않되기 때문에 이 기능을 막는다. - 정기남			window.event.returnValue = false;
		}
	}
	else{
		window.event.returnValue = true;
	}
}

/*****************************************
작성목적	: 오른쪽 마우스 이벤틀를 막는다.
*****************************************/
function fn_PreventRightMouse(){
	window.event.returnValue = false;	
}

/*****************************************
작성목적	: 오른쪽 마우스 이벤틀를 막는다.
*****************************************/
function fn_SetCursor(toggle){
	//var nTemp = document.all.length;
	var oTemp = document.body;
	if(oTemp != null)
	{
		if(toggle){
			oTemp.style.cursor = "wait";
		}
		else{
			oTemp.style.cursor = "default";
		}
	}
}

