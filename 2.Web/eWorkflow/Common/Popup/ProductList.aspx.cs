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

public partial class Common_Popup_ProductList : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var company = Sessions.CompanyCode;
            var bu = Request["bu"].NullObjectToEmptyEx();
            var existPrice = Request["baseprice"].NullObjectToEmptyEx();
            var sampleType = Request["sampletype"].NullObjectToEmptyEx();
            var existSample = Request["existsample"].NullObjectToEmptyEx();


            this.hddBu.Value = bu;
            this.hddBasePrice.Value = existPrice;
            this.hddSampletype.Value = sampleType;
            this.hddExistsample.Value = existSample;
                       
            

            SelectProduct(company, bu, "", existPrice,sampleType,existSample);
        }

        
    }

    private List<DTO_PRODUCT> GetProductList(string company, string bu, string keyword, string baseprice, string sampleType, string existSample)
    {
        try
        {
            using (CommonMgr mgr = new CommonMgr())
            {                
                return mgr.SelectProduct(company, bu, keyword, baseprice,sampleType,existSample);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void SelectProduct(string company, string bu, string keyword, string baseprice, string sampleType, string existSample)
    {
        try
        {
            this.radGridProductList.DataSource = GetProductList(company, bu, keyword, baseprice, sampleType, existSample);
            this.radGridProductList.DataBind();
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }


    protected void radGridProductList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            this.radGridProductList.DataSource = GetProductList(Sessions.CompanyCode, this.hddBu.Value, this.radTxtKeyword.Text, this.hddBasePrice.Value, this.hddSampletype.Value, this.hddExistsample.Value);
            //this.radGridProductList.Rebind();
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    protected void radBtnSearch_Click(object sender, EventArgs e)
    {
        SelectProduct(Sessions.CompanyCode, this.hddBu.Value, this.radTxtKeyword.Text, this.hddBasePrice.Value, this.hddSampletype.Value, this.hddExistsample.Value);
    }

}