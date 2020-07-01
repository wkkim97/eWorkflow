<%@ Page Language="C#" MasterPageFile="~/Master/eWorks_Popup2.master" AutoEventWireup="true" CodeFile="ErrorMessage.aspx.cs" Inherits="DNSoft.eW.Web.Common.Message.ErrorMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" Runat="Server">
 
    <script type="text/javascript" language="javascript">
        var bDetail = true;
        /************************************************************************
        함수명		: FormLoad()
        작성목적	: onLoad 이벤트에서 공통 fn_WindowOnLoad를 수행한 후 호출된다.
        Parameter	: 
        Return		: 
        작 성 자	: 닷넷소프트
        최초작성일	: 2011.01.20
        최종작성일	: 
        수정내역	:
        *************************************************************************/
        function FormLoad() {
            try {
               
                var arrTemp;
                var strTemp;

                //window.returnValue = false;
               
                strTemp = window.dialogArguments;

                if (strTemp == null) {
                   return;
                }
                
                arrTemp = strTemp.split("|^|")
                if (arrTemp[0] != "") document.getElementById("tdDisplayMessageTitle").innerHTML = arrTemp[0];
                if (arrTemp[1] != "" && arrTemp[1] != undefined) {
                    document.getElementById("tdDisplayMessage").innerHTML = arrTemp[1];
                    document.getElementById("btnDetail").style.display = "";
                    document.getElementById("hancDetail").style.display = "";
                 }
                else {
                    document.getElementById("btnDetail").style.display = "none";
                    document.getElementById("hancDetail").style.display = "none";
                    
                }
                if (arrTemp[2] != "") document.getElementById("tdSummaryMessage").innerHTML = arrTemp[2];

                document.getElementById("htabrDetailArea").style.display = "none";
 
                ViewDetail();
            }
            catch (exception) {
                //    순환 호출됨.
                fn_OpenErrorMessage(exception.description);
            }
        }

        function SelectConfirm(select) {
            try {
                window.returnValue = select;
                window.close();
            }

            catch (exception) {
                fn_OpenErrorMessage(exception.description);
            }
        }

        function ViewDetail() {
            var nWindowHeight = 0;
            var nClientHeight = 0;

            try {
                //bDetail = !bDetail;
                var checkValue = document.getElementById("btnDetail").checked;
  
                if (checkValue) {
                    document.getElementById("htabrDetailArea").style.display = "";
                    parent.dialogHeight = (DIALOGHEIGHT - 105) + "px";
                }
                else {
                    document.getElementById("htabrDetailArea").style.display = "none";
                    parent.dialogHeight = (DIALOGHEIGHT - DIALOGSMALLHEIGHT) + "px";
                }

                nWindowHeight = parseInt(parent.dialogHeight.toUpperCase().replace("PX", ""));

                nClientHeight = document.body.clientHeight;
                
                parent.dialogHeight = (nWindowHeight + (nWindowHeight - (nClientHeight + 25)  )).toString() + "px";
            }

            catch (exception) {
                //fn_OpenErrorMessage(exception.description);
            }
        }
 
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" Runat="Server">

    <div class="wrap_pop_info" style="width: 100%;">
        <h3 class="tit_pop_comm"><span id="hspanError" runat="server">에러</span></h3>
        <div id="htabrTitleArea" class="box_comments1" style="width: 378px;">

            <table class="ico_txt" cellpadding="0" border="0" cellspacing="0" width="360">
                <colgroup>
                    <col width="80" />
                    <col width="*" />
                </colgroup>
                <tr >
                    <td>
                        <img src="/eWorks/styles/images/ico_err.png" alt="" /></td>
                    <td>
                        <span id="tdDisplayMessageTitle"></span> 
                    </td>
                </tr>
            </table>
            <div>
                <input type="checkbox" onclick="ViewDetail()" id="btnDetail" style="display: none" /><span id="hspanViewDetail" runat="server">상세보기</span>
            </div>
            <div id="hancDetail" style="display: none">
                <span id="tdSummaryMessage" style="display: none"></span>
            </div>
            <div id="htabrDetailArea" style="display: none;">
                <textarea id="tdDisplayMessage" style="width:100%" cols="" rows="8"></textarea>
            </div>
        </div>

        <!-- 버튼 -->
        <!--blockButton Start-->
        <div style="text-align:right;margin: 10px 10px 10px 10px;" >
            <a class="btn btn-blue btn-size1 bold" onclick="window.close();"><span id="hspanConfirm" runat="server">확인</span></a> 
        </div>

    </div>
 
 
 
</asp:Content>
 