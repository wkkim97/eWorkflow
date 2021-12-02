using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Common.Dto;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eWF.FrameWork.Web; 

public partial class Approval_main : DNSoft.eWF.FrameWork.Web.PageBase
{
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetNoticeList();
            GetApproveCount();
            GetDocList();
            InitControls();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    private void InitControls()
    {
      
        if (Sessions.UserRole.Equals(ApprovalUtil.UserRole.Admin) || Sessions.UserRole.Equals(ApprovalUtil.UserRole.Design))
        {
            AddNotice.Attributes.Add("onclick", "fn_ShowBoard();");
            AddNotice.Visible = true;
        }
        else
        {
            AddNotice.Visible = false;
        }

    } 
    #endregion

    #region GetDocList
    private void GetDocList()
    {
        const int MAX_PAGE = 12;
        List<DTO_USER_CONFIG_DOC_SORT> docList = null;
        using (Bayer.eWF.BSL.Common.Mgr.UserConfigMgr mgr = new Bayer.eWF.BSL.Common.Mgr.UserConfigMgr())
        {
            docList = mgr.SelectUserConfigDocList(Sessions.UserID);
        }
        int pageCount = docList.Count % MAX_PAGE == 0 ? (docList.Count / MAX_PAGE) : (docList.Count / MAX_PAGE) + 1;

        int cur = 1;
        int i = 0;

        for (cur = 1; cur <= pageCount; cur++)
        {

            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("id", "hdivDocArea_" + cur.ToString());
            div.Attributes.Add("index", cur.ToString());
            for (; i < (MAX_PAGE * cur); i++)
            {
                //if (i >= docList.Count) break;
                HtmlGenericControl li = new HtmlGenericControl("li");

                li.Attributes.Add("dec", docList.Count <= i ? "" : docList[i].DOC_DESCRIPTION.Replace(Environment.NewLine, "<br/>"));
                HtmlGenericControl span = new HtmlGenericControl("span");
 
                HtmlGenericControl anchor = new HtmlGenericControl("a");
                if(  docList.Count > i )
                    anchor.Attributes.Add("href", string.Format("javascript:fn_ShowDocument('{0}','{1}',''); ", docList[i].FORM_NAME, docList[i].DOCUMENT_ID));
                anchor.InnerText = docList.Count <= i ? "" : docList[i].DOC_NAME;

                li.Controls.Add(anchor);
                div.Controls.Add(li);

            }

            if (cur > 1) div.Style.Add("display", "none");

            ulDocList.Controls.Add(div);

        }
        // 최초 조회할 인덱스를 정의
        ulDocList.Attributes.Add("currentindex", "1");
        ulDocList.Attributes.Add("lastindex", pageCount.ToString());
    } 
    #endregion

    #region GetApproveCount
    private void GetApproveCount()
    {
        int savedCnt, todoCnt, processCnt;
        using(Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            savedCnt = mgr.GetApprovalCount(Sessions.UserID, "Saved");
            todoCnt = mgr.GetApprovalCount(Sessions.UserID, "Todo");
            processCnt = mgr.GetApprovalCount(Sessions.UserID, "Processing");
        }
 
        aSavedCnt.InnerText = savedCnt.ToString( );
        aTodoCnt.InnerText = todoCnt.ToString( );
        aProcessCnt.InnerText = processCnt.ToString();
    } 
    #endregion

    #region GetNoticeList
    private void GetNoticeList()
    {
        List<DTO_BOARD_NOTICE> notice = null;
        const int ROW_TOP = 6;

        using (Bayer.eWF.BSL.Common.Mgr.NoticeMgr mgr = new Bayer.eWF.BSL.Common.Mgr.NoticeMgr())
        {
            notice = mgr.SelectNoticeList(ROW_TOP);
        }

        if (notice.Count > 0)
        {
            foreach (DTO_BOARD_NOTICE not in notice)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                HtmlGenericControl anchor = new HtmlGenericControl("a");
                anchor.Attributes.Add("href", "#");
                anchor.Attributes.Add("onclick", string.Format("fn_ShowBoard({0});",not.IDX.ToString()));
                anchor.InnerText = not.SUBJECT;

                li.Controls.Add(anchor);
                ulNotice.Controls.Add(li);
            }
        }
        else
        {
            HtmlGenericControl li = new HtmlGenericControl("li");
            HtmlGenericControl anchor = new HtmlGenericControl("a");
            anchor.Attributes.Add("href", "#");
            anchor.InnerText = "No records to display.";

            li.Controls.Add(anchor);
            ulNotice.Controls.Add(li);
        }
    } 
    #endregion
}