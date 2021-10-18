using Bayer.eWF.BSL.Reporting.Dto;
using Bayer.eWF.BSL.Reporting.Mgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class ReturnGoods_ReturnPending : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected override void OnInit(EventArgs e)
    {
        
        using (Bayer.eWF.BSL.Reporting.Mgr.ReportingMgr mgr = new Bayer.eWF.BSL.Reporting.Mgr.ReportingMgr())
        {
            List<string> list = mgr.SelectReturnGoodsDiv();

            foreach (string div in list)
            {
                RadButton btn = new RadButton();
                btn.ButtonType = RadButtonType.ToggleButton;
                btn.ToggleType = ButtonToggleType.CheckBox;
                btn.AutoPostBack = false;
                btn.Text = div.Trim();
                btn.Value = div.Trim();
                btn.ID = "radBtn" + div;
                btn.Checked = false;
                this.divDIV.Controls.Add(btn);
            }
        }
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
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
        this.radgrdPending.DataSource = string.Empty;
        GridBind();
    }  

    private void GridSource()
    {
        string div = string.Empty;
        foreach (Control control in divDIV.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                    div += (control as RadButton).Value.Trim() + ",";
            }
        }

        List<DTO_REPORTING_RETURN_GOODS> list = null;

        using (ReportingMgr mgr = new ReportingMgr())
        {
            list = mgr.SelectReturnGoodsPending(this.Sessions.UserID, div);
        }
        if (list != null)
            radgrdPending.DataSource = list;
    }

    private void GridBind()
    {
        radgrdPending.DataBind();
    }


    #region NeedDataSource
    protected void radgrdPending_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        GridSource();
    } 
    #endregion

    #region ItemDataBound
    protected void radgrdPending_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (e.Item as GridDataItem);
            item["CUSTOMER_NAME"].Text = item["CUSTOMER_NAME"].Text + "<br/> (" + item["CUSTOMER_CODE"].Text + ")";
            item["PRODUCT_NAME"].Text = item["PRODUCT_NAME"].Text + "<br/> (" + item["PRODUCT_CODE"].Text + ")";
        }
    } 
    #endregion


    #region 엑셀 다운로드
    protected void btnDownload_Click(object sender, EventArgs e)
    {
         string div = string.Empty;
        foreach (Control control in divDIV.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                    div += (control as RadButton).Value.Trim() + ",";
            }
        }

        string yearmonth = DateTime.Now.ToString("yyyy-MM-dd_hh");
        string filename = yearmonth + "Pending.xls";

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
        using (ReportingMgr mgr = new ReportingMgr())
        {
            excel.DataSource = mgr.SelectReturnGoodsPending(this.Sessions.UserID, div);
        }        
        excel.DataBind();
        excel.RenderControl(new HtmlTextWriter(Response.Output));

        Response.Flush();
        Response.End();
    } 
    #endregion
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GridSource();
        GridBind();
    }
}