using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using System.Collections;
using System.Text;
using DNSoft.eW.FrameWork;



public partial class Reporting_CustomerMasterList_KPIS : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                InitControls();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion

    #region InitControls
    private void InitControls()
    {
        //this.radgrdCustomerMaster.DataSource = string.Empty;
        GridSource();
        this.radgrdCustomerMaster.DataBind();

    }

    #endregion


    #region GridSource
    private void GridSource()
    {
        string company = string.Empty;
        

        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            radgrdCustomerMaster.DataSource = mgr.SelectCustomerMaster_KPIS("", "", this.radTxtKeyword.Text);
        }
    }
    #endregion

    public void Reporting_CustomerMasterList_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Update"))
        {
            string[] values = e.Argument.Split(new char[] { ':' });
            if (values.Length > 2)
            {                
                string customerCode = values[1];
                string creditLimit = values[2];
                string parvw = values[3];
                UpdateCustomerMasterCreditLimit(customerCode, parvw, Convert.ToDecimal(creditLimit), Sessions.UserID);
                GridSource();
                this.radgrdCustomerMaster.DataBind();
                
            }
        }
    }

    protected void radgrdCustomerMaster_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            GridSource();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //this.radgrdCustomerMaster.Rebind();
    }

    protected void radBtnSearch_Click(object sender, EventArgs e)
    {
        GridSource();
        this.radgrdCustomerMaster.DataBind();
        this.hhdChkBu.Value = "";
    }

    protected void radgrdCustomerMaster_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        try
        {
            List<DTO_CUSTOMER_KPIS> list = new List<DTO_CUSTOMER_KPIS>();
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                Hashtable selecteditem = command.NewValues;
                DTO_CUSTOMER_KPIS customer = new DTO_CUSTOMER_KPIS();
                customer.BCNC_CODE = selecteditem["BCNC_CODE"].ToString();
                customer.BIZR_NO = selecteditem["BIZR_NO"].ToString();
                
                customer.BCNC_RPRSNTV = selecteditem["BCNC_RPRSNTV"].ToString();
                customer.BCNC_SE = selecteditem["BCNC_SE"].ToString();
                customer.TELNO = selecteditem["TELNO"].ToString();
                customer.POST_CODE = selecteditem["POST_CODE"].ToString();
                customer.POST_ADRES = selecteditem["POST_ADRES"].ToString();
                customer.BCNC_NM = "";
                

                //customer.LEVEL = "S";
                //customer.SALES_RATE = Convert.ToDouble(selecteditem["SALES_RATE"] == string.Empty ? 0 : Convert.ToDouble(selecteditem["SALES_RATE"].ToString()));
                //customer.SCORING_RATE = Convert.ToDouble(selecteditem["SCORING_RATE"] == string.Empty ? 0 : Convert.ToDouble(selecteditem["SCORING_RATE"].ToString()));
                //customer.SALES_RATE = Convert.ToDouble(selecteditem["SALES_RATE"] == null ? 0 : Convert.ToDouble(selecteditem["SALES_RATE"].ToString()));
                //customer.SCORING_RATE = Convert.ToDouble(selecteditem["SCORING_RATE"] == null ? 0 : Convert.ToDouble(selecteditem["SCORING_RATE"].ToString()));
                //customer.SALES_RATE = 0;
                //customer.SCORING_RATE = 0;
                customer.UPDATER_ID = this.Sessions.UserID;
                //customer.CREDIT_LIMIT = Convert.ToDecimal(selecteditem["CREDIT_LIMIT"] == string.Empty ? 0 : Convert.ToDecimal(selecteditem["CREDIT_LIMIT"].ToString()));
                customer.UPDATE_DATE = DateTime.Now;
                
                list.Add(customer);
                
                
            }

            UpdateCustomerMaster(list);
            GridSource();
            this.radgrdCustomerMaster.DataBind();          
            
            

        }
        catch (Exception ex)
        {
            throw ex;
            
        }

    }

    private void UpdateCustomerMaster(List<DTO_CUSTOMER_KPIS> list)
    {
        try
        {
            using (CommonMgr mgr = new CommonMgr())
            {
                mgr.UpdateCustomerMaster_KPIS(list);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
  
    protected void radGrdProduct_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            //int index = Convert.ToInt32(e.CommandArgument);

            using (CommonMgr mgr = new CommonMgr())
            {
                mgr.DeleteCustomerMaster_KPIS(e.CommandArgument.ToString(),this.Sessions.UserID);
            }
            GridSource();
            this.radgrdCustomerMaster.DataBind();
            
        }

    }


























    private void UpdateCustomerMasterCreditLimit(string customercode, string parvw, decimal creditlimit, string updaterid)
    {
        try
        {
            using (CommonMgr mgr = new CommonMgr())
            {
                mgr.UpdateCustomerMasterCreditLimit(customercode, parvw, creditlimit, updaterid);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

   

    protected void radgrdCustomerMaster_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridDataItem item = null;

        //if (e.Item is GridDataItem)
        //    item = (e.Item as GridDataItem);
        
        //if (item != null)
        //{          
        //    RadButton btnSap = item["SAP"].FindControl("radBtnSap") as RadButton;
        //    Control label = item.FindControl("lblLevel");
        //    if ((label as Label).Text.Equals("S"))
        //        btnSap.Visible = true;
        //    else btnSap.Visible = false;

        //}
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
       
        string yearmonth = DateTime.Now.ToString("yyyy-MM-dd_");
        string filename = yearmonth + "CustomerMasterList.xls";

        // This actually makes your HTML output to be downloaded as .xls file
        Response.Clear();
        Response.ClearContent();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
        Response.Charset = "euc-kr";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr"); 

        // Create a dynamic control, populate and render it
        GridView excel = new GridView();
        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            excel.DataSource = mgr.SelectCustomerMaster_KPIS("", "", "");
        }
        excel.DataBind();
        excel.RenderControl(new HtmlTextWriter(Response.Output));

        Response.Flush();
        Response.End();
    }
}

