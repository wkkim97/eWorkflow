// 결재 버튼 Event 후처리( 메일 발송)
function fn_sendMail(processid, sendmailtype, senderAddress) {
    try {
        //var text_hh = WCFSERVICE + "/MailServices.svc/InvokeSendMail/" + processid + "/" + sendmailtype + "/" + senderAddress;
        //alert(text_hh)
        $.get(WCFSERVICE + "/MailServices.svc/InvokeSendMail/" + processid + "/" + sendmailtype + "/" + senderAddress, function (data) { });
    }
    catch (exception) { }
}

// 후처리 서비스
function fn_AfterTreatment(serviceNameUri, processid) {
    try {
        $.get(WCFSERVICE + "/AfterTreatmentServices.svc/" + serviceNameUri + "/" + processid, function (data) { });
    }
    catch (exception) { }
}