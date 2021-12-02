using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Configuration.Dto;
using Bayer.eWF.BSL.Configuration.Mgr;
using Telerik.Web.UI;

using DNSoft.eW.FrameWork;

public partial class Common_Popup_DetailSearch : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                string userid = Request["userid"].NullObjectToEmptyEx();
                InitControls(userid);
            }
            
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    #region InitControls
    private void InitControls(string userid)
    {
        List<DTO_USER_CONFIG_DOC_SORT> docList = null;
        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            docList = mgr.SelectAdminDocList(userid);
            radListDocument.DataSource = docList;
            radListDocument.DataBind();
        }
    }
    #endregion
}