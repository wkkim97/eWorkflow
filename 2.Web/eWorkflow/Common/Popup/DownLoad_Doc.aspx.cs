using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Configuration.Dto;
using Bayer.eWF.BSL.Configuration.Mgr;
using Telerik.Web.UI;

using DNSoft.eW.FrameWork;

public partial class Common_Popup_DetailSearch : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                string userid = Request["userid"].NullObjectToEmptyEx();
                InitControls(userid);
            }
            
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    #region InitControls
    private void InitControls(string userid)
    {
        List<DTO_USER_CONFIG_DOC_SORT> docList = null;
        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            docList = mgr.SelectAdminDocList_detailreport(userid);
            radListDocument.DataSource = docList;
            radListDocument.DataBind();
        }
    }
    #endregion
    #region btnDownload_NEW_Click

    protected void btnDownload_NEW_Click(object sender, EventArgs e)
    {
        string chk = null;
        DateTime FROM_DATE = new DateTime(1900, 01, 01);
        DateTime TO_DATE = new DateTime(2050, 01, 01);
        string DOCUMENT_ID_SE = this.radListDocument.SelectedValue.ToString() ;
        if (this.radFromDate.IsEmpty )
        {
            
        }
        else
        {
            FROM_DATE = Convert.ToDateTime(this.radFromDate.SelectedDate.ToString());
        }
        if (this.radToDate.IsEmpty)
        {
           
        }
        else
        {
            TO_DATE = Convert.ToDateTime(this.radToDate.SelectedDate.ToString());
        }

        
        string filename = "REPORTING_DOCUMENT_" + DOCUMENT_ID_SE + ".xls";
        // This actually makes your HTML output to be downloaded as .xls file
        Response.Clear();
        Response.ClearContent();
        //Response.ContentType = "application/octet-stream";
        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
        //Response.Charset = "utf-8";
        
        // Create a dynamic control, populate and render it
        GridView excel = new GridView();
        
        //using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
       // {
          
         //   excel.DataSource = mgr.SelectAdminReportingDocumentList(DOCUMENT_ID_SE,FROM_DATE ,TO_DATE);
            //  excel.DataSource = mgr.Download_Detail_Report(DOCUMENT_ID_SE, FROM_DATE, TO_DATE);
            //excel.DataSource = mgr.SelectAdminReportingDocumentList("D0036");
       // }
        System.IO.StringWriter stringWriter = new System.IO.StringWriter();
        using (Bayer.eWF.BSL.Common.Dao.CommonDao mgr = new Bayer.eWF.BSL.Common.Dao.CommonDao())
        {

            //excel.DataSource = mgr.SelectAdminReportingDocumentList(DOCUMENT_ID_SE, FROM_DATE, TO_DATE);
              excel.DataSource = mgr.Download_Detail_Report(DOCUMENT_ID_SE, FROM_DATE, TO_DATE);
            //excel.DataSource = mgr.SelectAdminReportingDocumentList("D0036");
        }
        excel.DataBind();

        Response.Charset = "utf-8";
        //string encoding = Request.ContentEncoding.HeaderName;        
        //Response.Write("<meta http-equiv='Content-Type' content='text/html; charset=" + encoding + "'>");
        //Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
        string encoding = Request.ContentEncoding.HeaderName;
        Response.ContentType = "application/unknown";
        Response.Write("<meta http-equiv='Content-Type' content='text/html; charset=" + encoding + "'>");

        stringWriter.WriteLine("");
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(stringWriter);
        //stringWriter.WriteLine("");
        excel.RenderControl(hw);
        Response.Write(stringWriter.ToString());
        Response.Flush();
        Response.End();
    }
    #endregion
}