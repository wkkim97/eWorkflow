<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FileUpload.ascx.cs" Inherits="Common_FileUpload" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxManager ID="RadAjaxManager1" OnAjaxRequest="RadAjaxManager1_AjaxRequest" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="uploadFileAttach" />
                <telerik:AjaxUpdatedControl ControlID="hhdAttachFiles" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<input type="hidden" id="hhdPreAttachFileList" runat="server" />    
<input type="hidden" id="hhdUploadFolder" runat="server" />
<input type="hidden" id="hhdAttachFiles" runat="server" />
<input type="hidden" id="hhdTotalFileSize" runat="server" />
<input type="hidden" id="hhdBigMailAttachYN" runat="server" value="N" />
<input type="hidden" id="hhdUploadComplete" runat="server" />
<input type="hidden" id="hhdDeleteList" runat="server" />
<input type="hidden" id="hddProcessID" runat="server" />
<input type="hidden" id="hddUserID" runat="server" />
<telerik:RadCodeBlock ID="codeBlock" runat="server">
    <script type="text/javascript">
        Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };
        function onValidationFileUpload(sender, args) {
            $("#spanUploadValidationMsg").html("<p>'" + args.get_fileName() + "'은 첨부할수 없습니다.</p>").fadeIn("slow");
            setTimeout("hiddenFileUploadTextArea()", 2000);

        }
        var BIGATTACHFILESIZE = 100000000;
        var normalFilesSize = new Array();
        var bigFilesSize = new Array();
        var _attachFileList = [];
        var fileIndex = 0;
        //var isDuplicateFile = false;
        function onProgressUpdating(sender, args) {

            //리스트박스에 진행율 표시
            var data = args.get_data();
            var percents = data.percent;
            try {
                var fileID = $(args.get_row()).attr("fileID");

                //$("#progressbar_" + fileID).progressbar({
                //    value: percents
                //});

                //파일 사이즈
                //if (percents <= 100)
                $("#spanFileSize_" + fileID).html(byteConvertor(data.fileSize));
                normalFilesSize[fileID] = data.fileSize;

            } catch (e)
            { alert(e.message); }

        }

        function isUploadComplete() {
            if ($("[id$=hhdUploadComplete]").val() == "true") return true;
            else return false;
        }

        function isAttachFile() {
            var upload = $find("<%= lvAttachFiles.ClientID %>");
                var fileLength = 0;

                //if (upload._dataSource != null) {
                if (_attachFileList.length > 0) {
                    fileLength = _attachFileList.length;
                    if (fileLength > 0) return true;
                    else return false;
                }
                else
                    return false;
            }

            function getUploadedFiles() {
                //var upload = $find("<%= lvAttachFiles.ClientID %>");
                var source = _attachFileList;
                return source;
            }

            function byteConvertor(bytes) {
                try {
                    bytes = parseInt(bytes);
                    var s = ['bytes', 'KB', 'MB', 'GB', 'TB', 'PB'];
                    var e = Math.floor(Math.log(bytes) / Math.log(1024));
                    if (e == "-Infinity") return "0 " + s[0];
                    else return (bytes / Math.pow(1024, Math.floor(e))).toFixed(2) + " " + s[e];
                } catch (e)
                { alert(e.message); }
            }

            function onAttachFileUploading(sender, args) {
                var row = args.get_row();
                try {
                    var filesize = $(row).attr("fileSize");
                    if (filesize == '0') {
                        args.set_cancel(true);
                    }

                } catch (e)
                { alert(e.message); }
            }

            function onAttachFileUploaded(sender, args) {
                try {
                    SetAttachFileSizeArea();

                }
                catch (e) {
                }
            }

            function onAttachFileRemoved(sender, args) {

                SetAttachFileSizeArea();
            }

            function onAttachFilesSelected(sender, args) {
                callUpdateGridData();
                fileIndex = sender._currentIndex;
                if ($find("<%= lvAttachFiles.ClientID %>")._dataSource != null)
                    fileIndex = $find("<%= lvAttachFiles.ClientID %>")._dataSource.length;
                var files = sender._uploadedFiles;
                if (files.length > 0) {
                    //기존에 추가된 파일들의 마지막 Index 프로퍼티를 가져온다.
                    fileIndex = files[files.length - 1].fileInfo.Index;
                    //새로 선택된 파일들의 시작 FileID를 위해 +1;
                    fileIndex++;
                }
            }

            function onAttachFileSelected(sender, args) {
                var row = args.get_row();
                var typeName = "일반";
                var type = "N";
                //파일사이즈 체크
                var inputField = args.get_fileInputField();

                if (inputField) {
                    if (inputField.files.length > 0) {
                        for (var i = 0; i < inputField.files.length; i++) {
                            if (inputField.files[i].name == row.innerText) {
                                if (inputField.files[i].size < 1) {
                                    $(row).attr("FileSize", inputField.files[i].size);
                                    type = "F";
                                    typeName = "실패";
                                }
                            }
                        }
                    }
                }
                //--------------------------------->


                //for (var fileindex in sender._uploadedFiles) {
                //    if (sender._uploadedFiles[fileindex].fileInfo.FileName == args.get_fileName()) {
                //        isDuplicateFile = true;
                //    }
                //}

                $(row).attr("FileID", fileIndex);
                //listview에 추가
                var dataAttachFiles = [];
                var fileName = args.get_fileName();

                //alert(fileName);
                fileName = fileName.replace("+", "_");
                //alert(fileName);

                var processid = document.getElementById("<%= hddProcessID.ClientID %>").value;
                //dataAttachFiles[0] = {
                //    IDX : '',
                //    FileID: fileIndex,
                //    FileName: fileName,
                //    FileSize: "0",
                //    FileType: type,
                //    FileTypeName: typeName,
                //    AttachMentID: ""
                //}

                dataAttachFiles[0] = {
                    IDX: fileIndex
                    , SEQ: fileIndex
                    , PROCESS_ID: processid
                    , DISPLAY_FILE_NAME: fileName
                    , SAVED_FILE_NAME: fileName
                    , FILE_SIZE: 0
                    , ATTACH_FILE_TYPE: type
                    , FILE_PATH: ''
                    , COMMENT_IDX: 0
                }

                appendListViewData(dataAttachFiles, false);
                _attachFileList.push(dataAttachFiles[0]);
                //$("#progressbar_" + fileIndex).progressbar({
                //    value: 0
                //});
                //fileIndex를 증가
                fileIndex++;
            }

            //첨부파일 목록을 append
            function appendListViewData(data, isPreMail) {
                var listview = $find("<%= lvAttachFiles.ClientID %>");
                listview.appendData(data);

                //이전메일 첨부인경우 모두 100%
                //if (isPreMail) {
                //    for (var index in data) {
                //        var id = data[index].FileID;
                //        $("#progressbar_" + id).progressbar({
                //            value: 100
                //        });
                //    }
                //}
            }

            function SetAttachFileSizeArea() {
                var totalBytes = 0;
                try {
                    for (var index in normalFilesSize) {
                        totalBytes += normalFilesSize[index];
                    }
                    document.getElementById("<%= hhdTotalFileSize.ClientID %>").value = totalBytes;
                    $("#spanFileSize").html(byteConvertor(totalBytes));


                    totalBytes = 0;
                    for (var index in bigFilesSize) {
                        totalBytes += bigFilesSize[index];
                    }
                    document.getElementById("<%= hhdTotalFileSize.ClientID %>").value = totalBytes;
                    $("#spanBigFileSize").html(byteConvertor(totalBytes));
                } catch (e) {
                    alert(e.message)
                }
            }

            function btnAttachFile_Clicked(sender, args) {
                // IE8에서 정상 작동되지 않음.
                // $(".ruFileInput").trigger("click");

                if ($('.attach_area').attr("class") == "attach_area") {
                    if ($('.attach_area').length == 1) {
                        $('.attach_area')[0].src = $('.attach_area')[0].src.replace("-out", "-in");
                        $('#divAttachArea').show();
                        $('.attach_area').addClass('on');
                    }
                }
            }

            //첨부파일 삭제
            function btnDeleteAttach_Clicked(sender, args) {
                callUpdateGridData();
                var callBackFunction = Function.createDelegate(sender, function (shouldSubmit) {
                    if (shouldSubmit) {
                        var cntChecked = $('.check_row:checked').size();
                        var oDeleteList = $("[id$=hhdDeleteList]");
                        var oGridList = $find("<%=lvAttachFiles.ClientID%>")._dataSource;

                        if (cntChecked > 0) {
                            var upload = $find("<%= uploadFileAttach.ClientID %>");
                            var checkedList = $('.check_row:checked');
                            for (var i = 0; i < checkedList.length; i++) {
                                var item = checkedList[i];
                                var fileId = item.value;
                                var fileName = item.getAttribute('filename');
                                oDeleteList.val(oDeleteList.val() + fileName + "*" + fileId + "|");
                                $('#tr_' + fileId).remove();

                                //fileID는 유일함으로 일반/대용량을 삭제한다.
                                delete normalFilesSize[fileId];

                                _attachFileList.pop(fileId);

                                //radasycupload에서 삭제
                                var inputs = upload._uploadedFiles
                                var child = $('.ruInputs').children();
                                //var inputs = upload._getRowCount();
                                //for (var j = inputs.length - 1; j >= 0; j--) {
                                //    var index = inputs[j].fileInfo.Index;
                                //    if (index == fileId)
                                //        upload.deleteFileInputAt(j);
                                //}
                                for (var j = child.length - 1; j >= 0; j--) {

                                    if ($(child[j]).attr('fileid')) {
                                        var index = $(child[j]).attr('fileid');
                                        if (index == fileId) {
                                            upload.deleteFileInputAt(j);
                                        }
                                    }
                                }


                            }
                            upload.updateClientState();
                            SetAttachFileSizeArea();

                            $find('<%=RadAjaxManager1.ClientID%>').ajaxRequest("DeleteAttachFile");


                        }

                        //sender.set_autoPostBack(true);

                    } else {
                        $find('<%=RadAjaxManager1.ClientID%>').ajaxRequest("DeleteAttachFile");
                    }
                });

                fn_OpenConfirm("Are you sure you want to delete the selected attachment?", callBackFunction);


            }



            $(document).ready(function () {

                var attachFiles = document.getElementById("<%= hhdAttachFiles.ClientID %>").value;

              if (attachFiles.length > 0) {
                  var preFiles = JSON.parse(attachFiles);

                  //기존 파일 크기 표시
                  for (var index in preFiles) {
                      var file = preFiles[index];
                      normalFilesSize[file.IDX] = parseInt(file.FILE_SIZE);

                      file.FILE_SIZE = byteConvertor(parseInt(file.FILE_SIZE));

                  }
                  setTimeout(function () {
                      appendListViewData(preFiles, true);
                      SetAttachFileSizeArea();
                  }, 1);



              }

          });

                function onAttachFilesUploaded() {
                    $("[id$=hhdUploadComplete]").val("false");
                    $find('<%=RadAjaxManager1.ClientID%>').ajaxRequest('UploadFiles');
                $("[id$=hhdUploadComplete]").val("true");
            }

            function callUpdateGridData() {
                if (typeof fn_UpdateGridDataFromUploader == 'function') {
                    fn_UpdateGridDataFromUploader();
                }
            }

            function fn_checkAllChange(sender) {
                var rows = $('.check_row');
                //var rows = document.getElementsByClassName('check_row');
                for (var i = 0; i < rows.length; i++) {
                    rows[i].checked = sender.checked;
                }
            }

            function fn_TempFileDownload(fileName) {
                var strUrl = null;
                var userId = $('#<%= hddUserID.ClientID %>').val();
                try {

                    strUrl = "/eWorks/Common/AttachFileDownload.aspx?FILENAME=" + fileName + "&USERID=" + userId;
                    //alert(strUrl);
                    //frames.filedownframe.location.href = strUrl;
                    //console.log(window.frames["filedownframe"]);
                    ////window.open(strUrl);
                    //console.log(strUrl);
                    //document.getElementById("filedownframe")[0].src = strUrl;
                    location.href = strUrl;
                   //window.frames["filedownframe"].location.href = strUrl;
                }
                catch (exception) {
                    console.log(exception);
                    alert(exception.description);
                    //fn_OpenErrorMessage(exception.description);
                }
            }
    </script>
</telerik:RadCodeBlock>

<div id="divFIleUloadArea" runat="server">
    <table style="width: 100%; padding: 0px 10px 0px 10px;">
        <tr>
            <td>
                <div id="areaAttachButton" style="margin: 2px 2px 2px 2px">
                    <telerik:RadButton ID="btnAttachFile" runat="server" AutoPostBack="false" Visible="false" OnClientClicked="btnAttachFile_Clicked" Text="Add">
                    </telerik:RadButton>
                    <ul>
                        <li style="float: left;">
                            <telerik:RadAsyncUpload runat="server" ID="uploadFileAttach" HideFileInput="true" MultipleFileSelection="Automatic"
                                ManualUpload="false"
                                OnFileUploaded="uploadFileAttach_FileUploaded"
                                OnClientValidationFailed="onValidationFileUpload"
                                OnClientProgressUpdating="onProgressUpdating"
                                OnClientFileUploading="onAttachFileUploading"
                                OnClientFileUploaded="onAttachFileUploaded"
                                OnClientFilesUploaded="onAttachFilesUploaded"
                                OnClientFileUploadRemoved="onAttachFileRemoved"
                                OnClientFileSelected="onAttachFileSelected"
                                OnClientFilesSelected="onAttachFilesSelected"
                                Width="100%">
                                <Localization Select="Add" />
                            </telerik:RadAsyncUpload>
                        </li>
                        <li style="float: left; margin-left: 5px;">
                            <telerik:RadButton ID="btnDeleteAttachFile" runat="server" AutoPostBack="false" OnClientClicked="btnDeleteAttach_Clicked" Text="Remove">
                            </telerik:RadButton>
                        </li>
                        <li style="float: right;">
                            <div id="divFileSize" style="display:none">
                                <span id="spanFileSize" style="color: blue">0KB</span> <span id="spanFileSizeTotal"></span>
                            </div>
                        </li>
                        <li style="clear: both;"></li>
                    </ul>
                </div>
                <div id="divAttachArea">
                    <table style='font-size: 12px; font-family: Arial, Verdana, Gulim; color: #000; width: 100%;' cellspacing="0">
                        <colgroup>
                            <col style="width: 50px" />
                            <col />
                            <col style="width: 150px" />
                            <col style="width: 80px" />
                            <col style="width: 60px" />
                        </colgroup>
                        <thead style="background-color: #c3c3c3; height: 10px">
                            <tr style="height: 10px">
                                <th>
                                    <input type="checkbox" id="checkAll" name="checkAll" value="0" onclick="fn_checkAllChange(this)"  /></th>
                                <th>file Name</th>

                                <th>Size</th>
                                <th></th>
                            </tr>
                        </thead>
                    </table>
                    <telerik:RadListView ID="lvAttachFiles" runat="server" Width="100%" Height="100%">
                        <LayoutTemplate>
                            <div id="divAttachFiles" style="height: 100px; border: 1px solid #c3c3c3;">
                                <div id="divScrollArea" style="height: 100px; overflow: auto;">
                                    <table id="tblAttachFiles" style='font-size: 12px; font-family: Arial, Verdana, Gulim; color: #000; width: 100%;' cellspacing="0">
                                        <colgroup>
                                            <col style="width: 50px" />
                                            <col />
                                            <col style="width: 150px" />

                                            <col style="width: 60px" />
                                        </colgroup>
                                        <tbody id="itemContainer">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </LayoutTemplate>
                        <ClientSettings>
                            <DataBinding ItemPlaceHolderID="itemContainer">
                                <ItemTemplate>
                                        <tr id="tr_#= IDX #" class="tr_attach" style="height:11px">
                                            <td style="text-align:center;">
                                                <input class="check_row" type="checkbox" id="chkFile_#= IDX #" name="check_row" value="#= IDX #" filename="#= DISPLAY_FILE_NAME #"></input>
                                            </td>
                                            <td>
                                                <a href="javascript:fn_TempFileDownload('#= DISPLAY_FILE_NAME #')" style="padding-left:4px;"><span>#= DISPLAY_FILE_NAME #</span></a>
                                                <input type="hidden" value="#= IDX #" />
                                            </td>
                                          
                                            <td style="text-align:right">
                                                <span id="spanFileSize_#= SEQ #">#= FILE_SIZE #</span>
                                            </td>
                                            <td style="text-align:center;display:none;">
                                                <span id="spanFileType_#= IDX #" value="#= ATTACH_FILE_TYPE #" class="FileType_#= ATTACH_FILE_TYPE #">#= ATTACH_FILE_TYPE #</span></td>
                                        </tr>
                                </ItemTemplate>
                            </DataBinding>
                        </ClientSettings>
                    </telerik:RadListView>

                    <div id="divFileList" style="width: 100%; height: 10; overflow: hidden; position: relative; border: 0px solid #c3c3c3; padding: 0px;">
                        <telerik:RadProgressManager runat="server" ID="RadProgressManager1" />


                    </div>
                    <telerik:RadProgressArea runat="server" ID="RadProgressArea1" />
                </div>

            </td>
        </tr>
    </table>
</div>
<div id="divFileDownloadArea" runat="server">
    <telerik:RadListView ID="viewfileList" runat="server" Width="100%" Height="100">
        <ItemTemplate>
            <a href="javascript:fn_FileDownload(<%#Eval("IDX")%>)" style="padding-left: 4px;"><span><%# Eval("DISPLAY_FILE_NAME") %></span></a>
        </ItemTemplate>
    </telerik:RadListView>
</div>


