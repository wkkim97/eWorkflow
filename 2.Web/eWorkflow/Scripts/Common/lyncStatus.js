//var _nameCtrl = new ActiveXObject('Name.nameCtrl.1');
var _nameCtrl = null;

$(document).ready(function () {

    try {
        if (window.ActiveXObject) {
            nameCtrl = new ActiveXObject("Name.NameCtrl");

        } else {
            nameCtrl = CreateNPApiOnWindowsPlugin("application/x-sharepoint-uc");
        }
    }
    catch (ex) { }
});


function onStatusChange(name, status, id) {
    //alert(name + ", " + status + ", " + id);
    var lyncpresencecolor = "gray";
    switch (status) {
        case 0:
            document.getElementById("pre_" + id).style.backgroundColor = "#5DD255";
            break;
        case 1:
            document.getElementById("pre_" + id).style.backgroundColor = "#B6CFD8";
            break;
        case 2:
            document.getElementById("pre_" + id).style.backgroundColor = "#FFD200";
            break;
        case 3:
            document.getElementById("pre_" + id).style.backgroundColor = "#D00E0D";
            break;
        case 4:
            document.getElementById("pre_" + id).style.backgroundColor = "#FFD200";
            break;
        case 5:
            document.getElementById("pre_" + id).style.backgroundColor = "#D00E0D";
            break;
        case 6:
            document.getElementById("pre_" + id).style.backgroundColor = "#D00E0D";
            break;
        case 7:
            document.getElementById("pre_" + id).style.backgroundColor = "#D00E0D";
            break;
        case 8:
            //document.getElementById("pre_" + id).style.borderLeft = "dashed";
            //document.getElementById("pre_" + id).style.borderLeftWidth = "10px";
            document.getElementById("pre_" + id).style.backgroundColor = "#E57A79";
            break;
        case 9:
            document.getElementById("pre_" + id).style.backgroundColor = "#D00E0D";
            break;
        case 15:
            document.getElementById("pre_" + id).style.backgroundColor = "#D00E0D";
            break;
        case 16:
            document.getElementById("pre_" + id).style.backgroundColor = "#FFD200";
            break;
        default:
            document.getElementById("pre_" + id).style.backgroundColor = "#B6CFD8";
            break;
    }

}

function ShowOOUI(sipUri) {
    var zoomLevel = screen.deviceXDPI / screen.logicalXDPI;
    var oouiX =15, oouiY = 15;
    oouiX = event.clientX;
    oouiY = event.clientY;
    _nameCtrl.ShowOOUI(sipUri, 0, oouiX, oouiY);
}

function HideOOUI(sipUri) {
    _nameCtrl.HideOOUI();
}
