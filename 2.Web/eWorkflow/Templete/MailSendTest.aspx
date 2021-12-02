<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MailSendTest.aspx.cs" Inherits="Templete_MailSendTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <script src="/eWorks/Scripts/jquery/jquery-1.11.1.min.js"></script>
    <script src="../Scripts/Common.js"></script>
    <script type="text/javascript">
        function fn_SendMail()
        { 
            alert("333");
            $.get("http://by0y9n.bayer.cnb/eWorkServices/MailServices.svc/InvokeSendMail/P000039458/CurrentApprover/youngwoo.lee@bayer.com", function (data) {}); 
        }

        function fn_ShowDocument(formName, documentid, processid) {
            var nWidth = 935;
            var nHeight = 680;
            var left = (screen.width / 2) - (nWidth / 2);
            var top = (screen.height / 2) - (nHeight / 2) - 10;

            var url = fn_GetWebRoot() + "Approval/Document/" + formName + "?processid=" + processid + "&documentid=" + documentid;
            window.open(url, "", "width=" + nWidth + "px, height=" + nHeight + "px, top=" + top + "px, left=" + left + "px,location=no,titlebar=no,status=no,scrollbars=yes,menubar=no,toolbar=no,directories=no,resizable=no,copyhistory=no");
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" OnClick="Button1_Click" runat="server" Text="Button" />
        <input type="button" onclick="fn_SendMail()" id="btnSendmail" title="SendMail" />

          Dear “{0}”<br/>
              <br/>
              Please find enclosed a link to an e-workflow document, and be so kind to verify (Approval or Reject) it as quickly as possible. <br/>
              <br/>
              <a href="#" onclick="fn_ShowDocument('CreditDebitMemo.aspx',  'D0006' , 'P000000727'); ">Document Link</a><br/> 
              <br/>
           <%-- <script src="../Scripts/Approval/List.js"></script>--%>
    </div>
    </form>
</body>
</html>
