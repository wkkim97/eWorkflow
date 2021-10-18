using Bayer.eWF.BSL.Reporting.Dto;
using Bayer.eWF.BSL.Reporting.Mgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class ReturnGoods_Statistics : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                List<int> listyear = Enumerable.Range(2013, Convert.ToInt32(DateTime.Now.Year.ToString()) - 2012).ToList();
                List<int> listmonth = Enumerable.Range(1, 12).ToList();

                this.radListYear.DataSource = listyear;
                this.radListYear.DataBind();
                this.radListMonth.DataSource = listmonth;
                this.radListMonth.DataBind();
                this.radListYear.SelectedText = DateTime.Now.Year.ToString();
                this.radListMonth.SelectedText = DateTime.Now.Month.ToString();

                GridSource();
                GridBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #region PageLoadInfo
    private void PageLoadInfo()
    {
    }
    #endregion


    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.Equals("reload"))
        {
            GridSource();
            GridBind();
        }
    }


    #region GridSource
    private void GridSource()
    {
        grdReturnGoods.DataSource = GetReturnGoodsData();
    }
    private List<DTO_REPORTING_RETURN_GOODS> GetReturnGoodsData()
    {
        List<DTO_REPORTING_RETURN_GOODS> list = null;

        string strType = string.Empty;
        if (radRdoType2.Checked)
            strType = radRdoType2.Value;
        else if (radRdoType3.Checked)
            strType = radRdoType3.Value;
        //else
        //    strType = radRdoType1.Value;

        DateTime selectDT = new DateTime(Convert.ToInt32(this.radListYear.SelectedText), Convert.ToInt32(this.radListMonth.SelectedText), 1);

        using (ReportingMgr mgr = new ReportingMgr())
        {
            list = mgr.SelectReturnGoods(strType, this.Sessions.UserID, selectDT);
        }

        return list;
    }
    #endregion
    #region GridBind
    private void GridBind()
    {
        grdReturnGoods.DataBind();
    }
    #endregion


    protected void grdReturnGoods_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        GridSource();
    }
    protected void grdReturnGoods_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem gridItem = (GridDataItem)e.Item;
            foreach (GridColumn column in grdReturnGoods.MasterTableView.RenderColumns)
            {
                if (column is GridBoundColumn)
                {
                    gridItem[column.UniqueName].ToolTip = gridItem[column.UniqueName].Text;
                }
            }
        }

    }

    protected void radListYear_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        GridSource();
        GridBind();
    }


    protected void btnDownload_Click(object sender, EventArgs e)
    {
        string filename = this.radListYear.SelectedText + this.radListMonth.SelectedText + "ReturnGoods.xls";

        // This actually makes your HTML output to be downloaded as .xls file
        Response.Clear();
        Response.ClearContent();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
        //Response.Charset = "utf-8";

        Response.Charset = "euc-kr";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");

        // Create a dynamic control, populate and render it
        GridView excel = new GridView();
        excel.DataSource = GetReturnGoodsData();
        excel.DataBind();
        excel.RenderControl(new HtmlTextWriter(Response.Output));

        Response.Flush();
        Response.End();
    }


    private void UpdateReturnGoods(string status)
    {
        try
        {
            List<DTO_REPORTING_RETURN_GOODS> list = new List<DTO_REPORTING_RETURN_GOODS>();

            foreach (GridDataItem selectitem in grdReturnGoods.MasterTableView.ChildSelectedItems)
            {
                DTO_REPORTING_RETURN_GOODS item = new DTO_REPORTING_RETURN_GOODS();

                item.IDX = Convert.ToInt32(selectitem["IDX"].Text);
                item.TYPE = selectitem["TYPE"].Text;
                item.STATUS = selectitem["STATUS"].Text;
                item.SHIPTO_CODE = selectitem["SHIPTO_CODE"].Text.Replace("&nbsp;", "");
                item.PRODUCT_NAME = selectitem["PRODUCT_NAME"].Text;
                item.INVOICE_PRICE = (selectitem["INVOICE_PRICE"].Text.Equals("") ? Convert.ToDecimal(selectitem["INVOICE_PRICE"].Text) : 0);
                item.WHOLESALES_MANAGER_STATUS = (selectitem["WHOLESALES_MANAGER"].Text.Equals(this.Sessions.UserID) ? status : selectitem["WHOLESALES_MANAGER_STATUS"].Text.Replace("&nbsp;", ""));
                item.SALES_ADMIN_STATUS = (selectitem["SALES_ADMIN"].Text.Equals(this.Sessions.UserID) ? status : selectitem["SALES_ADMIN_STATUS"].Text.Replace("&nbsp;", ""));
                item.WHOLESALES_SPECIALIST_STATUS = (selectitem["WHOLESALES_SPECIALIST"].Text.Equals(this.Sessions.UserID) ? status : selectitem["WHOLESALES_SPECIALIST_STATUS"].Text.Replace("&nbsp;", ""));

                list.Add(item);
            }

            using (ReportingMgr mgr = new ReportingMgr())
            {
                mgr.UpdateReturnGoods(list);
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }

    }
}