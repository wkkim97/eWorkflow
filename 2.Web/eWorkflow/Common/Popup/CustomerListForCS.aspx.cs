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

public partial class Common_Popup_CustomerListForCS : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string creditlimit = Request["creditlimit"].NullObjectToStringEx("N");

        string level = Request["level"].NullObjectToStringEx("N");

        if (creditlimit.Equals("Y"))
            this.radGridCustomerList.MasterTableView.GetColumn("CREDIT_LIMIT").Display = true;
        else
            this.radGridCustomerList.MasterTableView.GetColumn("CREDIT_LIMIT").Display = false;

        this.hddLevel.Value = level;
        if (!IsPostBack)
        {
            var companycode = Sessions.CompanyCode;
            var bu = Request["bu"].NullObjectToEmptyEx();
            var parvw = Request["parvw"].NullObjectToEmptyEx();

            this.hddBu.Value = bu;
            this.hddParvw.Value = parvw;

            SelectCustomer(companycode, bu, parvw, "", level);
        }
    }

    private List<DTO_CUSTOMER> GetCustomerList(string companycode, string bu, string parvw, string keyword, string level)
    {
        try
        {
            using (CommonMgr mgr = new CommonMgr())
            {
                return mgr.SelectCustomer(companycode, bu, parvw, keyword, level);
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    private void SelectCustomer(string companycode, string bu, string parvw, string keyword, string level)
    {
        try
        {
            this.radGridCustomerList.DataSource = GetCustomerList(companycode, bu, parvw, keyword, level);
            this.radGridCustomerList.Rebind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void radGridCustomerList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            this.radGridCustomerList.DataSource = GetCustomerList(Sessions.CompanyCode, this.hddBu.Value, this.hddParvw.Value, this.radTxtKeyword.Text, this.hddLevel.Value);
            this.radGridCustomerList.Rebind();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
        
    }
    protected void radBtnSearch_Click(object sender, EventArgs e)
    {
        SelectCustomer(Sessions.CompanyCode, this.hddBu.Value, this.hddParvw.Value, this.radTxtKeyword.Text, this.hddLevel.Value);
    }
}