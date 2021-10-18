using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dao;
using Telerik.Web.UI;
using System.Web.Script.Serialization;
using System.Collections;

public partial class Reporting_ProductMasterList_KPIS : DNSoft.eWF.FrameWork.Web.PageBase
{
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                //var company = Sessions.CompanyCode;
                //SelectProductMaster(company, "");

                initControls();
            }
            
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }    

    private void initControls()
    {
        //this.GrdProductMaster.DataSource = string.Empty;
        gridSource();
        this.GrdProductMaster.DataBind();
    }

    

    protected void GrdProductMaster_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            gridSource();            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void gridSource()
    {
        //companycode
       


         using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            this.GrdProductMaster.DataSource = mgr.SelectProductMaster_KPIS("0695","", this.radTxtKeyword.Text);            
        }
    }
    
    #endregion

    #region 찾기 
    protected void radBtnSearch_Click(object sender, EventArgs e)
    {
        gridSource();
        this.GrdProductMaster.DataBind();
       // this.hhdChkBu.Value = "";
    } 
    #endregion

    

    protected void GrdProductMaster_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        try
        {
            List<DTO_PRODUCT_KPIS> list = new List<DTO_PRODUCT_KPIS>();

            foreach (GridBatchEditingCommand Command  in e.Commands)
            {
                Hashtable selectitem = new Hashtable();
                selectitem = Command.NewValues;

                DTO_PRODUCT_KPIS item = new DTO_PRODUCT_KPIS();
                item.PRDUCT_CODE = selectitem["PRDUCT_CODE"].ToString();
                item.PRODUCT_NAME_KR = "";
                item.STD_CODE = selectitem["STD_CODE"].ToString();
                item.REPRSNT_CODE = selectitem["REPRSNT_CODE"].ToString();
                item.REPRSNT_NM = selectitem["REPRSNT_NM"].ToString();
                item.REPORT_YN = selectitem["REPORT_YN"].ToString();
                item.PACKNG_QY = Convert.ToDecimal(selectitem["PACKNG_QY"].ToString());
                item.C_STD_CODE = "";
                item.C_PRODUCT_KOR_NM = "";
                item.C_REPRSNT_CODE = "";
                item.C_PEPRSNT_NM = "";
                item.C_PACKNG_QY = Convert.ToDecimal(selectitem["C_PACKNG_QY"].ToString());
                item.MULTIPLICATION = Convert.ToDecimal(selectitem["MULTIPLICATION"].ToString()); ;
                
                
                
                //item.USE_SAMPLE_DC = (selectitem["USE_SAMPLE_DC"] == null ? "" : selectitem["USE_SAMPLE_DC"].ToString());
                //item.INVOICE_PRICE = Convert.ToDecimal(selectitem["INVOICE_PRICE"] == string.Empty? 0 : Convert.ToDecimal(selectitem["INVOICE_PRICE"].ToString()));
                //item.NET1_PRICE = Convert.ToDecimal(selectitem["NET1_PRICE"] == string.Empty ? 0 : Convert.ToDecimal(selectitem["NET1_PRICE"].ToString()));
                //item.NET2_PRICE = Convert.ToDecimal(selectitem["NET2_PRICE"] == string.Empty ? 0 : Convert.ToDecimal(selectitem["NET2_PRICE"].ToString()));

                //item.INVOICE_PRICE_NH = Convert.ToDecimal(selectitem["INVOICE_PRICE_NH"] == string.Empty ? 0 : Convert.ToDecimal(selectitem["INVOICE_PRICE_NH"].ToString()));
                //item.NET1_PRICE_NH = Convert.ToDecimal(selectitem["NET1_PRICE_NH"] == string.Empty ? 0 : Convert.ToDecimal(selectitem["NET1_PRICE_NH"].ToString()));
                //item.NET2_PRICE_NH = Convert.ToDecimal(selectitem["NET2_PRICE_NH"] == string.Empty ? 0 : Convert.ToDecimal(selectitem["NET2_PRICE_NH"].ToString()));
                item.UPDATER_ID = this.Sessions.UserID;
                item.UPDATE_DATE = DateTime.Now;

                list.Add(item);
            }

            UpdataProductMaster_KPIS(list);

            gridSource();
            this.GrdProductMaster.DataBind();
            this.informationMessage = "Update 돼었습니다.";
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
    }

    private void UpdataProductMaster_KPIS(List<DTO_PRODUCT_KPIS> list)
    {
        try
        {
            using(CommonMgr mgr = new CommonMgr())
            {
                mgr.UpdateProductMaster_KPIS(list);
            }
        }
        catch (Exception ex)
        {            
            throw ex;
        }
    }
   
    protected void GrdProductMaster_ItemCommand(object sender, GridCommandEventArgs e)
    {
       
        if (e.CommandName.Equals("Remove"))
        {
            using (CommonMgr mgr = new CommonMgr())
            {
                mgr.DeleteProductMaster_KPIS(e.CommandArgument.ToString(), this.Sessions.UserID);
            }
        }
        gridSource();
        this.GrdProductMaster.DataBind();

    }
}