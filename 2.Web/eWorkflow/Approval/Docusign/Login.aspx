<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Docusign_Login" %>

<!doctype html>
<html>
<head>
    <title>Docusign Test</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
</head>
<body>
    전자 승인을 위해 Docusign 로그인 페이지로 이동합니다.
    <input type="hidden" id="integrationKey" runat="server" />
    <input type="hidden" id="redirect" runat="server" />
    <input type="hidden" id="userEmail" runat="server" />
    <input type="hidden" id="state" runat="server" />
    <script>

        var integrationKey = document.getElementById("integrationKey").value;
        var redirect = document.getElementById("redirect").value;
        var userEmail = document.getElementById("userEmail").value;
        var state = document.getElementById("state").value;
        var addr = "https://account-d.docusign.com/oauth/auth?response_type=code&scope=signature&client_id=" + integrationKey + "&redirect_uri=" + redirect + "&login_hint=" + userEmail + "&state=" + state;
        location.href = addr;

    </script>
</body>
</html>
