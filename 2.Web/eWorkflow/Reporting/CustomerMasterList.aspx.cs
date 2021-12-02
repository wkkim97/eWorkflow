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



public partial class Reporting_CustomerMasterList : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
        this.radgrdCustomerMaster.DataSource = string.Empty;
        this.radgrdCustomerMaster.DataBind();

    }

    #endregion


    #region GridSource
    private void GridSource()
    {
        string company = string.Empty;
        foreach (Control control in divComapnyCode.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                    company += (control as RadButton).Value + ",";
            }
        }

        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            radgrdCustomerMaster.DataSource = mgr.SelectCustomerMaster("0963", "ALL", "ALL", "ALL", "ALL", this.radTxtKeyword.Text);

            
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
            List<DTO_CUSTOMER> list = new List<DTO_CUSTOMER>();
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                Hashtable selecteditem = command.NewValues;
                DTO_CUSTOMER customer = new DTO_CUSTOMER();
                customer.CUSTOMER_CODE = selecteditem["CUSTOMER_CODE"].ToString();
                customer.CUSTOMER_NAME_KR = selecteditem["CUSTOMER_NAME_KR"].ToString();
                customer.PARVW = selecteditem["PARVW"].ToString();
                customer.COMPANY_CODE = selecteditem["COMPANY_CODE"].ToString();
                customer.BU = selecteditem["BU"].ToString();
                customer.CHANNEL = selecteditem["CHANNEL"].ToString();
                customer.LEVEL = "S";
                //customer.SALES_RATE = Convert.ToDouble(selecteditem["SALES_RATE"] == string.Empty ? 0 : Convert.ToDouble(selecteditem["SALES_RATE"].ToString()));
                //customer.SCORING_RATE = Convert.ToDouble(selecteditem["SCORING_RATE"] == string.Empty ? 0 : Convert.ToDouble(selecteditem["SCORING_RATE"].ToString()));
                //customer.SALES_RATE = Convert.ToDouble(selecteditem["SALES_RATE"] == null ? 0 : Convert.ToDouble(selecteditem["SALES_RATE"].ToString()));
                //customer.SCORING_RATE = Convert.ToDouble(selecteditem["SCORING_RATE"] == null ? 0 : Convert.ToDouble(selecteditem["SCORING_RATE"].ToString()));
                customer.SALES_RATE = 0;
                customer.SCORING_RATE = 0;
                customer.UPDATER_ID = this.Sessions.UserID;
                customer.CREDIT_LIMIT = Convert.ToDecimal(selecteditem["CREDIT_LIMIT"] == string.Empty ? 0 : Convert.ToDecimal(selecteditem["CREDIT_LIMIT"].ToString()));
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

    private void UpdateCustomerMaster(List<DTO_CUSTOMER> list)
    {
        try
        {
            using (CommonMgr mgr = new CommonMgr())
            {
                mgr.UpdateCustomerMaster(list);
            }
        }
        catch (Exception ex)
        {
            throw ex;
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

    protected void radBtnCompanycode1_CheckedChanged(object sender, EventArgs e)
    {
        string company = string.Empty;
        foreach (Control control in divComapnyCode.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                    company += (control as RadButton).Value + ",";
            }
        }
        this.hhdChkBu.Value = company;

        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            List<string> list = mgr.SelectCustomerMasterBu(company);
            this.divBU.Controls.Clear();
            foreach (string bu in list)
            {
                RadButton btn = new RadButton();
                btn.ButtonType = RadButtonType.ToggleButton;
                btn.ToggleType = ButtonToggleType.CheckBox;
                btn.AutoPostBack = false;
                if (bu == string.Empty)
                {
                    btn.Text = "N/A";
                    btn.Value = "N/A";
                }
                else
                {
                    btn.Text = bu;
                    btn.Value = bu;
                }
                btn.ID = "radBtn" + bu;
                btn.Checked = true;
                btn.EnableViewState = true;

                this.divBU.Controls.Add(btn);
            }
        }
    }

    protected void radgrdCustomerMaster_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridDataItem item = null;

        if (e.Item is GridDataItem)
            item = (e.Item as GridDataItem);
        
        if (item != null)
        {          
            RadButton btnSap = item["SAP"].FindControl("radBtnSap") as RadButton;
            Control label = item.FindControl("lblLevel");
            if ((label as Label).Text.Equals("S"))
                btnSap.Visible = true;
            else btnSap.Visible = false;

            RadButton btnKPIS = item["KPIS"].FindControl("radBtnKPIS") as RadButton;
            
            if (item["KPIS"].Text.Equals("Y"))
                btnKPIS.Visible = true;
            else btnKPIS.Visible = false;

        }
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        string company = string.Empty;
        foreach (Control control in divComapnyCode.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                    company += (control as RadButton).Value + ",";
            }
        }

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
            excel.DataSource = mgr.SelectCustomerMaster("0963", "", "", "", "", this.radTxtKeyword.Text);
        }
        excel.DataBind();
        excel.RenderControl(new HtmlTextWriter(Response.Output));

        Response.Flush();
        Response.End();
    }
}

