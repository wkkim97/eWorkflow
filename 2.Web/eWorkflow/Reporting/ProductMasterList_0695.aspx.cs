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

public partial class Reporting_ProductMasterList_0695 : DNSoft.eWF.FrameWork.Web.PageBase
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
            this.GrdProductMaster.DataSource = mgr.SelectProductMaster("0695", this.radDropDown_BU.SelectedValue, this.RadDropDown_VISIBILITY.SelectedValue, this.RadDropDown_KPIS.SelectedValue, this.radTxtKeyword.Text);            
        }
    }
    
    #endregion

    #region 찾기 
    protected void radBtnSearch_Click(object sender, EventArgs e)
    {
        gridSource();
        this.GrdProductMaster.DataBind();
        this.hhdChkBu.Value = "";
    } 
    #endregion

    

    protected void GrdProductMaster_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        try
        {
            List<DTO_PRODUCT> list = new List<DTO_PRODUCT>();

            foreach (GridBatchEditingCommand Command  in e.Commands)
            {
                Hashtable selectitem = new Hashtable();
                selectitem = Command.NewValues;                

                DTO_PRODUCT item = new DTO_PRODUCT();
                item.PRODUCT_CODE = selectitem["PRODUCT_CODE"].ToString();
                item.PRODUCT_NAME = selectitem["PRODUCT_NAME"].ToString();
                item.PRODUCT_NAME_KR = selectitem["PRODUCT_NAME_KR"].ToString();
                item.COMPANY_CODE = selectitem["COMPANY_CODE"].ToString();
                item.BU = selectitem["BU"].ToString();
                item.BASE_PRICE = Convert.ToDecimal(selectitem["BASE_PRICE"].ToString());
                item.SAMPLE_CODE = selectitem["SAMPLE_CODE"].ToString();
                item.SAMPLE_TYPE = selectitem["SAMPLE_TYPE"].ToString();
                item.USE_SAMPLE_DC = "";
                item.MRP_FLAG = (selectitem["MRP_FLAG"] == null? "" : selectitem["MRP_FLAG"].ToString());
                item.MARGIN = Convert.ToDouble(selectitem["MARGIN"].ToString());

                item.INVOICE_PRICE = 0;
                item.NET1_PRICE = 0;
                item.NET2_PRICE = 0;

                item.INVOICE_PRICE_NH = 0;
                item.NET1_PRICE_NH = 0;
                item.NET2_PRICE_NH = 0;
                item.VISIBILITY = "Y";

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

            UpdataProductMaster(list);

            gridSource();
            this.GrdProductMaster.DataBind();
            this.informationMessage = "Update 돼었습니다.";
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
    }

    private void UpdataProductMaster(List<DTO_PRODUCT> list)
    {
        try
        {
            using(CommonMgr mgr = new CommonMgr())
            {
                mgr.UpdateProductMaster(list);
            }
        }
        catch (Exception ex)
        {            
            throw ex;
        }
    }
    protected void GrdProductMaster_ItemDataBound(object sender, GridItemEventArgs e)
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
            if (item["VISIBILITY"].Text.Equals("Y"))
            {
                btnDel.Visible = true; btnDel.Text = "D";
            }
            else
            {
                btnDel.Visible = true; btnDel.Text = "A";
            }
        }
    }
    protected void GrdProductMaster_ItemCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName == "UpdateCount")
        {
            DTO_PRODUCT_KPIS customer = new DTO_PRODUCT_KPIS();
            customer.PRDUCT_CODE = this.ProductCode_TB.Text;
            customer.STD_CODE = this.STD_TB.Text;
            customer.REPRSNT_CODE = this.REPRSNT_TB.Text;
            customer.REPRSNT_NM = this.REPRSNT_NM_TB.Text;
            customer.REPORT_YN = this.Report_TB.SelectedValue;

            if (this.Qty_TB.Text.Length > 0 && this.Qty_TB.Text != "0")
                customer.PACKNG_QY = Convert.ToDecimal(this.Qty_TB.Text);
            else
                customer.PACKNG_QY = 0;

            if (this.Qty_1_TB.Text.Length > 0 && this.Qty_1_TB.Text != "0")
                customer.C_PACKNG_QY = Convert.ToDecimal(this.Qty_1_TB.Text);
            else
                customer.C_PACKNG_QY = 0;

            //customer.PACKNG_QY = Convert.ToDecimal(this.Qty_TB.Text);
            //customer.C_PACKNG_QY = Convert.ToDecimal(this.Qty_1_TB.Text);
           // customer.C_PEPRSNT_NM = "";
           // customer.C_PRODUCT_KOR_NM = "";
           // customer.C_REPRSNT_CODE = "";
           // customer.C_STD_CODE = "";
           
            customer.CREATOR_ID = this.Sessions.UserID;
            customer.CREATE_DATE = DateTime.Now;

            using (Bayer.eWF.BSL.Common.Dao.CommonDao dao = new Bayer.eWF.BSL.Common.Dao.CommonDao())
            {
                dao.InsertProduct_KPIS(customer);
            }
        }


        string visibility;
        if (e.CommandName.Equals("Remove"))
        {
            visibility="N";
            using (CommonMgr mgr = new CommonMgr())
            {
                string[] values = e.CommandArgument.ToString().Split(new char[] { '|' });
                if (values.Length > 1)
                {
                    string customerCode = values[0];
                    string BU = values[1];
                    List<DTO_PRODUCT> list = new List<DTO_PRODUCT>();
                    DTO_PRODUCT item = new DTO_PRODUCT();
                    item.PRODUCT_CODE = customerCode;
                    item.PRODUCT_NAME = "";
                    item.PRODUCT_NAME_KR = "";
                    item.COMPANY_CODE = "0695";
                    item.BU = BU;
                    item.BASE_PRICE = 0;
                    item.SAMPLE_CODE = "";
                    item.SAMPLE_TYPE = "";
                    item.USE_SAMPLE_DC = "";
                    item.MRP_FLAG = "";
                    item.MARGIN = 0;
                    item.INVOICE_PRICE = 0;
                    item.NET1_PRICE = 0;
                    item.NET2_PRICE = 0;
                    item.INVOICE_PRICE_NH = 0;
                    item.NET1_PRICE_NH = 0;
                    item.NET2_PRICE_NH = 0;
                    item.VISIBILITY = visibility;
                    item.UPDATER_ID = this.Sessions.UserID;
                    item.UPDATE_DATE = DateTime.Now;
                    list.Add(item);
                    mgr.UpdateProductMaster(list);
                    

                }

            }
        }else
        {
            visibility = "A";
            using (CommonMgr mgr = new CommonMgr())
            {
                string[] values = e.CommandArgument.ToString().Split(new char[] { '|' });
                if (values.Length > 1)
                {
                    string customerCode = values[0];
                    string BU = values[1];
                    List<DTO_PRODUCT> list = new List<DTO_PRODUCT>();
                    DTO_PRODUCT item = new DTO_PRODUCT();
                    item.PRODUCT_CODE = customerCode;
                    item.PRODUCT_NAME = "";
                    item.PRODUCT_NAME_KR = "";
                    item.COMPANY_CODE = "0695";
                    item.BU = BU;
                    item.BASE_PRICE = 0;
                    item.SAMPLE_CODE = "";
                    item.SAMPLE_TYPE = "";
                    item.USE_SAMPLE_DC = "";
                    item.MRP_FLAG = "";
                    item.MARGIN = 0;
                    item.INVOICE_PRICE = 0;
                    item.NET1_PRICE = 0;
                    item.NET2_PRICE = 0;
                    item.INVOICE_PRICE_NH = 0;
                    item.NET1_PRICE_NH = 0;
                    item.NET2_PRICE_NH = 0;
                    item.VISIBILITY = visibility;
                    item.UPDATER_ID = this.Sessions.UserID;
                    item.UPDATE_DATE = DateTime.Now;
                    list.Add(item);
                    mgr.UpdateProductMaster(list);
                    

                }

            }
        }
        gridSource();
        this.GrdProductMaster.DataBind();
        

    }
}