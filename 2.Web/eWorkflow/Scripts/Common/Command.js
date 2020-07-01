function fn_RequestClicked(sender, args) {
    if (typeof fn_DoRequest == 'function') {
        if (fn_DoRequest(sender, args)) {
            sender.set_autoPostBack(true);
            return;
        }
    }
    else {
        sender.set_autoPostBack(true);
        return;
    }
    sender.set_autoPostBack(false);
}

function fn_ApprovalClicked(sender, args) {
    sender.set_autoPostBack(true);
}

function fn_FowardApprovalClicked(sender, args) {
    sender.set_autoPostBack(true);
}

function fn_RejectClicked(sender, args) {
    sender.set_autoPostBack(true);
}

function fn_FowardClicked(sender, args) {
    sender.set_autoPostBack(true);
}

function fn_RecallClicked(sender, args) {
    var callBackFunction = Function.createDelegate(sender, function (shouldSubmit) {
        if (shouldSubmit) {
            sender.set_autoPostBack(true);
            this.click();
        }
    });

    fn_OpenConfirm("Are you sure you want to recall this document?", callBackFunction);
    sender.set_autoPostBack(false);

}

function fn_WithdrawClicked(sender, args) {
    sender.set_autoPostBack(true);
}

function fn_RemindClicked(sender, args) {
    fn_OpenConfirm("Do you want to send the remider mail?", function (args) {
        if (args) {
            var processid = $("[id$=hddProcessID]").val();
            fn_sendMail(processid, 'Remind',"kr_workflow@bayer.com");
            var oWnd = GetRadWindow();
            oWnd.close();
        }
    });

    sender.set_autoPostBack(false);
}

function fn_ExitClicked(sender, args) {
    window.close();
    sender.set_autoPostBack(false);
}

function fn_SaveClicked(sender, args) {
    if (typeof fn_DoSave == 'function') {
        if (fn_DoSave(sender, args)) {
            sender.set_autoPostBack(true);
            return;
        }
    }
    else {
        sender.set_autoPostBack(true);
        return;
    }
    sender.set_autoPostBack(false);
}

function fn_InputCommentClicked(sender, args) {
    sender.set_autoPostBack(true);
}

function fn_HelpClicked() {
    var documentId = $("[id$=hddDocumentID]").val();    
    var url = HelpURL + "/" + documentId + ".docx";
    window.open(url, '_blank');
    return false;
}

function fn_CloseApprovalLine(sender, args) {
    if (args.get_argument() != null || args.get_argument() != undefined) {
        if (args.get_argument().returnValue) {
            window.location.reload();
            window.close();
        }
    }
}


function fn_DocumentCofirmClose() {
    fn_OpenConfirm("Document is saved.<BR> Do you want to close this document?"
                    , function (args) {
                        if (args) {
                            var oWnd = GetRadWindow();
                            oWnd.close();
                        }
                    }
    );
}

function fn_Close() {
    var oWnd = GetRadWindow();
    oWnd.close();

}
