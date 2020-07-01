using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using DNSoft.eWF.FrameWork.Web;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dto;

public partial class Common_Popup_CostCenter : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            var classcode = Request["classcode"].NullObjectToEmptyEx();
            this.hddClassCode.Value = classcode;
            SelectCodeSubList(classcode);
        }
    }

    private List<DTO_CODE_SUB> GetCodeSubList(string classCode)
    {
        try
        {
            using (CodeMgr mgr = new CodeMgr())
            {
                return mgr.SelectCodeSubList(classCode);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void SelectCodeSubList(string classCode)
    {
        try
        {
            this.radGrdCostCenter.DataSource = GetCodeSubList(classCode);
            this.radGrdCostCenter.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void radGrdCostCenter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            this.radGrdCostCenter.DataSource = GetCodeSubList(this.hddClassCode.Value);
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}