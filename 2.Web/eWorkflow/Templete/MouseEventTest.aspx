<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MouseEventTest.aspx.cs" Inherits="Templete_MouseEventTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" language="JavaScript">

        var cX = 0; var cY = 0; var rX = 0; var rY = 0;
        function UpdateCursorPosition(e) { cX = e.pageX; cY = e.pageY; }
        function UpdateCursorPositionDocAll(e) { cX = event.clientX; cY = event.clientY; }
        if (document.all) { document.onmousemove = UpdateCursorPositionDocAll; }
        else { document.onmousemove = UpdateCursorPosition; }
        function AssignPosition(d) {
            if (self.pageYOffset) {
                rX = self.pageXOffset;
                rY = self.pageYOffset;
            }
            else if (document.documentElement && document.documentElement.scrollTop) {
                rX = document.documentElement.scrollLeft;
                rY = document.documentElement.scrollTop;
            }
            else if (document.body) {
                rX = document.body.scrollLeft;
                rY = document.body.scrollTop;
            }
            if (document.all) {
                cX += rX;
                cY += rY;
            }
            d.style.left = (cX + 10) + "px";
            d.style.top = (cY + 10) + "px";
        }
        function HideContent(d) {
            if (d.length < 1) { return; }
            document.getElementById(d).style.display = "none";
        }
        function ShowContent(d) {
            if (d.length < 1) { return; }
            var dd = document.getElementById(d);
            AssignPosition(dd);
            dd.style.display = "block";
        }
 
        //-->
    </script>
</head>
<body>
    <form id="form1" runat="server">



        <a
            onmouseover="ShowContent('uniquename3'); return true;"
            onmouseout="HideContent('uniquename3'); return true;"
            href="javascript:ShowContent('uniquename3')">[show on mouseover, hide on mouseout]
        </a>
        <div
            id="uniquename3"
            style="display: none; position: absolute; border-style: solid; background-color: white; padding: 5px;">
            Content goes here.
        </div>
    </form>
</body>
</html>
