<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserAutoCompleteBox.ascx.cs" Inherits="Common_UserAutoCompleteBox" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script type="text/javascript">
    
    var _nameCtrl = new ActiveXObject('Name.nameCtrl.1');

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
        _nameCtrl.ShowOOUI(sipUri, 0, 15, 15);
    }

    function HideOOUI(sipUri) {
        _nameCtrl.HideOOUI();
    }


    function fn_OnEntryAdded(sender, args) {
        var entry = args.get_entry();
        if (entry.get_value()) {
            var user = JSON.parse(entry.get_value());
            if (_nameCtrl.PresenceEnabled) {
                var token = args.get_entry().get_token();
                _nameCtrl.OnStatusChange = onStatusChange;
                var userAddress = user.MAIL_ADDRESS;
                var userId = user.USER_ID;
                if (token.addEventListener) {
                    //token.addEventListener("click", setCheckedValues, false);
                    token.addEventListener('mouseover', function (e) { 
                        _nameCtrl.ShowOOUI(userAddress, 0, e.clientX - 10, e.clientY - 10);
                    });
                    token.addEventListener('mouseout', function () { _nameCtrl.HideOOUI(); })
                }
                else {
                    token.attachEvent("onmouseover", function (e) { _nameCtrl.ShowOOUI(userAddress, 0, e.clientX - 10, e.clientY - 10); });
                    token.attachEvent("onmouseout", function () { _nameCtrl.HideOOUI(); });
                }

                //token.addEventListener('mouseover', function () { _nameCtrl.ShowOOUI(userAddress, 0, 15, 15); });
                //token.addEventListener('mouseout', function () { _nameCtrl.HideOOUI(); })
                //if (userAddress.startsWith('a'))
                //    userAddress = 'loki-park@dotnetsoft.co.kr';
                //else if (userAddress.startsWith('b'))
                //    userAddress = 'cypher@dotnetsoft.co.kr';
                //else if (userAddress.startsWith('w'))
                //    userAddress = 'jaewoos@dotnetsoft.co.kr';
                //else if (userAddress.startsWith('j'))
                //    userAddress = 'soo@dotnetsoft.co.kr';
                //else if (userAddress.startsWith('y'))
                //    userAddress = 'amsmus@dotnetsoft.co.kr';
                //else
                //    userAddress = 'zest1116@dotnetsoft.co.kr';
                var status = _nameCtrl.GetStatus(userAddress, userId);
                $telerik.$(args.get_entry().get_token()).prepend("<span class='lync_status' id='pre_" + userId + "' onmouseover=ShowOOUI('" + userAddress + "') onmouseout=HideOOUI('" + userAddress + "') />")
            }
        }
    }
    function fn_GetEntries()
    {
        var autoObj = $find("<%= autoCompleteUserBox.ClientID %>");
     
        for (var i = 0; i < autoObj.get_entries()._array.length; i++) {
            _entryList.push(JSON.parse(autoObj.get_entries().getEntry(i).get_value()));
        }
        return _entryList;
    }
     
</script> 

<telerik:RadAutoCompleteBox ID="autoCompleteUserBox" runat="server" Width="100%" BorderStyle="None" OnClientEntryAdded="fn_OnEntryAdded" 
    DropDownWidth="280px" DropDownHeight="140px" DataTextField="Text" DataValueField="Unique"   InputType="Token" AllowCustomEntry="false">
     <TextSettings SelectionMode="Single" /> 
    <WebServiceSettings Method="SearchUserByName" Path="/eworks/Common/Interface/XmlHttpProcess.aspx" />
</telerik:RadAutoCompleteBox>
