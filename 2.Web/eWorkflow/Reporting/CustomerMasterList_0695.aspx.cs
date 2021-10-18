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



public partial class Reporting_CustomerMasterList_0695 : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
       // this.radgrdCustomerMaster.DataBind();
        GridSource();
        this.radgrdCustomerMaster.DataBind();
    }

    #endregion


    #region GridSource
    private void GridSource()
    {
        string company = string.Empty;
        //foreach (Control control in divComapnyCode.Controls)
        //{
        //    if (control is RadButton)
        //    {
        //        if ((control as RadButton).Checked)
        //            company += (control as RadButton).Value + ",";
        //    }
        //}

        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            radgrdCustomerMaster.DataSource = mgr.SelectCustomerMaster("0695", this.radDropDown_BU.SelectedValue, this.RadDropDownList_PARVW.SelectedValue, this.RadDropDown_VISIBILITY.SelectedValue, this.RadDropDown_KPIS.SelectedValue, this.radTxtKeyword.Text);
        }
    }
    #endregion

   

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
       // this.hhdChkBu.Value = "";
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
                customer.PARVW = selecteditem["PARVW"].ToString();
                customer.COMPANY_CODE = selecteditem["COMPANY_CODE"].ToString();
                customer.BU = selecteditem["BU"].ToString();
                customer.LEVEL = "S";
                customer.CUSTOMER_NAME_KR = selecteditem["CUSTOMER_NAME_KR"].ToString();
                customer.VISIBILITY = "Y";
                //customer.SALES_RATE = Convert.ToDouble(selecteditem["SALES_RATE"] == string.Empty ? 0 : Convert.ToDouble(selecteditem["SALES_RATE"].ToString()));
                customer.SALES_RATE = 0;
                customer.SCORING_RATE = 0;
                customer.UPDATER_ID = this.Sessions.UserID;
                customer.CREDIT_LIMIT = Convert.ToDecimal(selecteditem["CREDIT_LIMIT"] == string.Empty ? 0 : Convert.ToDecimal(selecteditem["CREDIT_LIMIT"].ToString()));
                customer.MORTAGE = Convert.ToDecimal(selecteditem["MORTAGE"] == string.Empty ? 0 : Convert.ToDecimal(selecteditem["MORTAGE"].ToString()));
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


    protected void radgrdCustomerMaster_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridDataItem item = null;

        if (e.Item is GridDataItem)
            item = (e.Item as GridDataItem);
        
        if (item != null)
        {             
            RadButton btnKPIS = item["KPIS_D"].FindControl("radBtnKPIS") as RadButton;
            if (item["KPIS"].Text.Equals("Y"))
                btnKPIS.Visible = false;
            else btnKPIS.Visible = true;

            RadButton btnDel = item["REMOVE_BUTTON"].FindControl("btnRemove") as RadButton;
            if (item["VISIBILITY"].Text.Equals("Y")){
                btnDel.Visible = true; btnDel.Text = "D";
            }else{
                btnDel.Visible = true; btnDel.Text = "A";
            }

                
            



        }
    }
    protected void radgrdCustomerMaster_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "UpdateCount")
        {
            DTO_CUSTOMER_KPIS customer = new DTO_CUSTOMER_KPIS();
            customer.BCNC_CODE = this.CustomerCode_TB.Text;
            customer.BIZR_NO = this.License_TB.Text;
            customer.BCNC_NM = this.CustomerName_KOR_TB.Text;
            customer.BCNC_RPRSNTV = this.CEO_TB.Text;
            customer.BCNC_SE = this.TYPE_TB.Text;
            customer.TELNO = this.Tel_TB.Text;
            customer.POST_CODE = this.PostCode_TB.Text;
            customer.POST_ADRES = this.Address_TB.Text;
            customer.CREATOR_ID = this.Sessions.UserID;
            customer.CREATE_DATE = DateTime.Now;

            using (Bayer.eWF.BSL.Common.Dao.CommonDao dao = new Bayer.eWF.BSL.Common.Dao.CommonDao())
            {
                dao.InsertCustomer_KPIS(customer);
            }
        }

        if (e.CommandName.Equals("Remove"))
        {
            //int index = Convert.ToInt32(e.CommandArgument);


            using (CommonMgr mgr = new CommonMgr())
            {
                string[] values = e.CommandArgument.ToString().Split(new char[] { '|' });
                if (values.Length > 2)
                {
                    string customerCode = values[0];
                    string BU = values[1];
                    string parvw = values[2];
                    List<DTO_CUSTOMER> list = new List<DTO_CUSTOMER>();
                    DTO_CUSTOMER customer = new DTO_CUSTOMER();
                    customer.CUSTOMER_CODE = customerCode;
                    customer.PARVW = parvw;
                    customer.COMPANY_CODE = "";
                    customer.BU = BU;
                    customer.LEVEL = "";
                    customer.CUSTOMER_NAME_KR = "";
                    customer.VISIBILITY = "N";

                    customer.SALES_RATE = 0;
                    customer.SCORING_RATE = 0;
                    customer.UPDATER_ID = this.Sessions.UserID;
                    customer.CREDIT_LIMIT = 0;
                    customer.MORTAGE = 0;
                    customer.UPDATE_DATE = DateTime.Now;
                    list.Add(customer);
                    mgr.UpdateCustomerMaster(list);

                }

             }
         }
        if (e.CommandName.Equals("Relive"))
         {
            //int index = Convert.ToInt32(e.CommandArgument);
            
            
            using (CommonMgr mgr = new CommonMgr())
            {
                string[] values = e.CommandArgument.ToString().Split(new char[] { '|' });
                if (values.Length > 2)
                {
                    string customerCode = values[0];
                    string BU = values[1];
                    string parvw = values[2];
                    List<DTO_CUSTOMER> list = new List<DTO_CUSTOMER>();
                    DTO_CUSTOMER customer = new DTO_CUSTOMER();
                    customer.CUSTOMER_CODE = customerCode;
                    customer.PARVW = parvw;
                    customer.COMPANY_CODE = "";
                    customer.BU = BU;
                    customer.LEVEL = "";
                    customer.CUSTOMER_NAME_KR = "";
                    customer.VISIBILITY = "A";

                    customer.SALES_RATE = 0;
                    customer.SCORING_RATE = 0;
                    customer.UPDATER_ID = this.Sessions.UserID;
                    customer.CREDIT_LIMIT = 0;
                    customer.MORTAGE = 0;
                    customer.UPDATE_DATE = DateTime.Now;
                    list.Add(customer);
                    mgr.UpdateCustomerMaster(list);

                }
                
            }
         }
         GridSource();
         this.radgrdCustomerMaster.DataBind();
    }


    protected void btnDownload_Click(object sender, EventArgs e)
    {
        string company = string.Empty;
        //foreach (Control control in divComapnyCode.Controls)
        //{
        //    if (control is RadButton)
        //    {
        //        if ((control as RadButton).Checked)
        //            company += (control as RadButton).Value + ",";
        //    }
        //}

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
            excel.DataSource = mgr.SelectCustomerMaster("0695", this.radDropDown_BU.SelectedValue, this.RadDropDownList_PARVW.SelectedValue, this.RadDropDown_VISIBILITY.SelectedValue, this.RadDropDown_KPIS.SelectedValue, this.radTxtKeyword.Text);
        }
        excel.DataBind();
        excel.RenderControl(new HtmlTextWriter(Response.Output));

        Response.Flush();
        Response.End();
    }
}

