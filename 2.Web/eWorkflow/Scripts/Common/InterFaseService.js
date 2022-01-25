// 결재 버튼 Event 후처리( 메일 발송)

function fn_sendMail(processid, sendmailtype, senderAddress) {

    var result;
    $.ajax({        
        url: WCFSERVICE + "/MailServices.svc/InvokeSendMail/" + processid + "/" + sendmailtype + "/" + senderAddress,
        method:"GET",
        
        async:false,
        success: function (data) {
            console.log(data);
            result = data;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            //console.log(jqXHR);
            //alert("메일이 발송되지 않았습니다)" + jqXHR);
            result = jqXHR;
        }

    });
    return result;

    
    //try {
    //    //var text_hh = WCFSERVICE + "/MailServices.svc/InvokeSendMail/" + processid + "/" + sendmailtype + "/" + senderAddress;
    //    //alert(text_hh)
    //    //$.get("http://ewf.kr.bayer.cnb/eWorkServices/MailServices.svc/InvokeSendMail/P000043710/CurrentApprover/wookyung.kim@bayer.com", function (data) { });
    //    //$.get(WCFSERVICE + "/MailServices.svc/InvokeSendMail/" + processid + "/" + sendmailtype + "/" + senderAddress, function (data) { });
    //
    //    $.ajax(WCFSERVICE + "/MailServices.svc/InvokeSendMail/" + processid + "/" + sendmailtype + "/" + senderAddress)
    //        .done(function (data) {
    //        //alert(data);
    //        return "success";
    //    })
    //        .fail(function (jqXHR, textStatus) {
    //            //alert("에러");
    //            //alert(errormsg);
    //            return "fail";
    //        })
    //        .error(function (xhr, ajaxOptions, thrownError) {
    //            return "errof";
    //    });
    //
    //
    //    //$.get(WCFSERVICE + "/MailServices.svc/InvokeSendMail/" + processid + "/" + sendmailtype + "/" + senderAddress, function (data) {
    //    //    alert("이메일 전송완료");
    //    //}.done(function(data){
    //    //    alert("좋다");
    //    //}));
    //}
    //catch (exception) {
    //    return "error-exception";
    //}
}

// 후처리 서비스
function fn_AfterTreatment(serviceNameUri, processid) {
    $.ajax({
        url: WCFSERVICE + "/AfterTreatmentServices.svc/" + serviceNameUri + "/" + processid,
        method: "GET",

        async: false,
        success: function (data) {
            result = data;
        },
        error: function (jqXHR, textStatus, errorThrown) { result = jqXHR; }

    });
    return result;
    //try {
    //    $.get(WCFSERVICE + "/AfterTreatmentServices.svc/" + serviceNameUri + "/" + processid, function (data) { });
    //}
    //catch (exception) { }
}