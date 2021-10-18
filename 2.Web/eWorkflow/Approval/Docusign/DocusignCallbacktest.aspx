<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocusignCallbacktest.aspx.cs" Inherits="Docusign_DocusignCallback_test" %>

<!doctype html>
<html>
<head>
    <title>Docusign Test</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <script src="/eWorks/Scripts/jquery/jquery-1.11.1.min.js"></script>
</head>
<body>

    문서를 생성중입니다.<br />
    잠시만 기다려 주세요.

    <input id="documentId" type="hidden" value="" runat="server"/>
    <input id="callbackResult" type="hidden" value="" runat="server"/>
    <input id="filePath" type="hidden" value="" runat="server"/>
    <input id="fileName" type="hidden" value="" runat="server"/>
    <input id="cdName" type="hidden" value="" runat="server"/>
    <input id="cdEmail" type="hidden" value="" runat="server"/>

    <div id="errorMessage" runat="server"></div>
    <script>

        getAccessToken(function (accessToken) {
            //createEnvelope(accessToken);
            goLivetest(accessToken);
        });

        function getAccessToken(cb) {
            var loginURL = "/eWorks/Approval/Docusign/Logintest.aspx?documentId=" + document.getElementById("documentId").value;
            var redirectToken = document.getElementById("callbackResult").value;
            if (!redirectToken) location.href = loginURL;
            var svcUrl = "/eWorks/Approval/Docusign/DocusignCallbacktest.aspx/GetAccessToken";
            var data = {
                redirectToken: redirectToken
            }
            $.ajax({
                type: "POST",
                url: svcUrl,
                dataType: "json",
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    try {
                        var response = JSON.parse(response.d);
                        if (response.success && response.code == 200) {
                            localStorage.setItem('EWF_DS_ACCESS_TOKEN', response.data);
                            if (cb) cb(response.data);
                        } else {
                            console.log(response);
                            alert("Token 조회 에러\nDocusign 로그인으로 이동합니다\n" + response.message || '');
                            location.href = loginURL;
                        }
                    } catch(e) {
                        console.log(response.d);
                        alert("이거야:"+response.d);
                        window.close();
                    }
                },
                error: function (e) {
                    console.log(e);
                    alert("네트워크 접속 에러");
                    window.close();
                }
            });
        }

        function getAccountId(accessToken) {
            var svcUrl = "/eWorks/Approval/Docusign/DocusignCallbacktest.aspx/GetAccountId";
            var data = {
                accessToken: accessToken
            }
            $.ajax({
                type: "POST",
                url: svcUrl,
                dataType: "json",
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    console.log(response)
                    var response = JSON.parse(response.d);
                    if (response.success && response.code == 200) {
                        localStorage.setItem('EWF_DS_ACCOUNT', JSON.stringify(response.data));
                    } else {
                        alert("Token 조회 에러\nDocusign 로그인으로 이동합니다\n" + response.message);
                        location.href = loginURL;
                    }
                },
                error: function (e) {
                    console.log(e);
                    alert("네트워크 접속 에러\nDocusign 로그인으로 이동합니다");
                    location.href = loginURL;
                }
            });
        }

        function createEnvelope(accessToken) {
            var svcUrl = "/eWorks/Approval/Docusign/DocusignCallbacktest.aspx/CreateEnvelope";
            var data = {
                accessToken: accessToken,
                filePath: document.getElementById("filePath").value,
                fileName: document.getElementById("fileName").value,
                cdName: document.getElementById("cdName").value,
                cdEmail: document.getElementById("cdEmail").value
            }
            $.ajax({
                type: "POST",
                url: svcUrl,
                dataType: "json",
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    try {
                        var response = JSON.parse(response.d);
                        if (response.success && (response.code == 200 || response.code == 201)) {
                            //alert("승인 문서 생성이 완료되었습니다\n문서 결제 페이지로 이동합니다");
                            location.href = response.data;
                        } else {
                            console.log(response);
                            alert("문서 생성이 실패하였습니다\n" + response.message || '');
                            window.close();
                        }
                    } catch (e) {
                        console.log(e);
                        alert(e);
                        window.close();
                    }
                },
                error: function (e) {
                    console.log(response.d);
                    alert(response.d);
                    window.close();
                }
            });
        }



        function goLivetest(accessToken) {
            var svcUrl = "/eWorks/Approval/Docusign/DocusignCallbacktest.aspx/goLivetest";
            var data = {
                accessToken: accessToken,
                envelopeId:"8c7dbe19-e32d-48f4-9c07-32aa65add402"
            }
            $.ajax({
                type: "POST",
                url: svcUrl,
                dataType: "json",
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    try {
                        var response = JSON.parse(response.d);
                        if (response.success && (response.code == 200 || response.code == 201)) {
                            //alert("승인 문서 생성이 완료되었습니다\n문서 결제 페이지로 이동합니다");
                            alert(response);
                            //location.href = response.data;
                        } else {
                            console.log(response);
                            alert("문서 생성이 실패하였습니다\n" + response.message || '');
                            window.close();
                        }
                        //console.log(accessToken);
                        //console.log(envelopeId);
                        //console.log(response.data);
                    } catch (e) {
                        console.log(e);
                        alert("이거야2:"+e);
                        window.close();
                    }
                },
                error: function (e) {
                    console.log(response.d);
                    alert("이거야3:"+response.d);
                    window.close();
                }
            });
        }



    </script>
</body>
</html>