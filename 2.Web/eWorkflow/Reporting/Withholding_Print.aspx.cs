using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Approval.Dto;
using Telerik.Web.UI;
using System.Web.Script.Serialization;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Dao;
using Bayer.eWF.BSL.Reporting.Dto;

public partial class Withholding_Print_In : DNSoft.eWF.FrameWork.Web.PageBase
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           
            
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }





    protected void grdSearch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        GridSource();
    }

    protected void grdSearch_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DTO_WITHHOLDING_LIST i = e.Item.DataItem as DTO_WITHHOLDING_LIST;
            GridDataItem item = e.Item as GridDataItem;

            //if (i.ATTACHFILEYN.Equals("Y"))
            //{
            //    Image attach = item.FindControl("iconAttach") as Image;
            //    attach.ImageUrl = "/eWorks/Styles/images/Common/icon_attach.gif";
            //}
            
        }
    }
    

    #region InitControls
    private void InitControls()
    {
        GridSource();
        GridBind();
    }

    #endregion

    #region GridSource


    private void GridSource()
    {
 
      
        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
             DateTime? FROM_DATE = new DateTime(1900, 01, 01);
             DateTime? TO_DATE = new DateTime(2050, 12, 31);

            radGrad1.DataSource = mgr.SelectWithHolding("BK", FROM_DATE, TO_DATE);
        }
    }
    #endregion

    #region GridBind
    private void GridBind()
    {
        radGrad1.DataBind();
    }
    #endregion

    public void Reporting_AdminDocumentList_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            GridSource();
            GridBind();
        }
    }

    #region btnSearch_Click
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            GridSource();
            GridBind();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion

}