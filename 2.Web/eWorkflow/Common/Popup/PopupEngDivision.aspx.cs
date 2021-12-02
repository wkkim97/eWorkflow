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

public partial class Common_Popup_PopupEngDivision : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            this.SelectEngDivision();
           
        }
    }

    private void SelectEngDivision()
    {
        SelectEngDivision(true);
    }

    private void SelectEngDivision(bool isInit)
    {
        using(CodeMgr mgr = new CodeMgr())
        {
            string strSUB_CODE = string.Empty;

            if (Sessions.CompanyCode == "0963")
                strSUB_CODE = "S025";
            else if (Sessions.CompanyCode == "2646")
                strSUB_CODE = "S030";
            else strSUB_CODE = "S009";


            List<DTO_CODE_SUB> divisions = mgr.SelectCodeSubList(strSUB_CODE);

            this.radGridengDivision.DataSource = divisions;
            if(isInit)
                this.radGridengDivision.DataBind();
        }
    }

    protected void radWinengDivision_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        this.SelectEngDivision(false);
    }

}