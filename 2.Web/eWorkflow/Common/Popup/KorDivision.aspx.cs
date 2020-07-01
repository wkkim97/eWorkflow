using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eWF.FrameWork.Web;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dto;

public partial class Common_Popup_KorDivision : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.SelectKorDivision();

        }
    }

    private void SelectKorDivision()
    {
        SelectKorDivision(true);
    }

    private void SelectKorDivision(bool isInit)
    {
        using (CodeMgr mgr = new CodeMgr())
        {
            string strSUB_CODE = string.Empty;

            if (Sessions.CompanyCode == "0963")
                strSUB_CODE = "S026";
            else strSUB_CODE = "S010";


            List<DTO_CODE_SUB> divisions = mgr.SelectCodeSubList(strSUB_CODE);

            this.radGridkorDivision.DataSource = divisions;
            if (isInit)
                this.radGridkorDivision.DataBind();
        }
    }

    protected void radGridkorDivision_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        this.SelectKorDivision(false);

    }
}