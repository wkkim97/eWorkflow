using Bayer.eWF.BSL.Reporting.Dto;
using Bayer.eWF.BSL.Reporting.Mgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Net;

public partial class ReturnGoods_ReturnGoods : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                GridSource();
                GridBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    #region GridSource
    private void GridSource()
    {
        List<DTO_REPORTING_RETURN_GOODS> list = null;

        using (ReportingMgr mgr = new ReportingMgr())
        {
            list = mgr.SelectReturnGoods("U", this.Sessions.UserID);
        }
        if (list != null)
            grdReturnGoods.DataSource = list;
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

            GridDataItem item = (e.Item as GridDataItem);
            item["CUSTOMER_NAME"].Text = item["CUSTOMER_NAME"].Text + "<br/> (" + item["CUSTOMER_CODE"].Text + ")";
            item["PRODUCT_NAME"].Text = item["PRODUCT_NAME"].Text + "<br/> (" + item["PRODUCT_CODE"].Text + ")";
        }
    }

    //protected void grdReturnGoods_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    //{
    //    try
    //    {
    //        List<DTO_REPORTING_RETURN_GOODS> list = new List<DTO_REPORTING_RETURN_GOODS>();
    //        string status = hhdUpdateType.Value;

    //        foreach (GridBatchEditingCommand command in e.Commands)
    //        {
    //            Hashtable selectitem = command.NewValues;
    //            if (selectitem["CHECKBOX"].ToString() == "false") continue;

    //            DTO_REPORTING_RETURN_GOODS item = new DTO_REPORTING_RETURN_GOODS();
    //            item.IDX = Convert.ToInt32(selectitem["IDX"].ToString());
    //            item.TYPE = selectitem["TYPE"].ToString();
    //            item.STATUS = selectitem["STATUS"].ToString();
    //            item.SHIPTO_CODE = (selectitem["SHIPTO_CODE"] == null ? "" : selectitem["SHIPTO_CODE"].ToString());
    //            //item.PRODUCT_NAME = selectitem["PRODUCT_NAME"].ToString();
    //            item.INVOICE_PRICE = Convert.ToDecimal(selectitem["INVOICE_PRICE"].ToString());
    //            item.WHOLESALES_MANAGER_STATUS = (selectitem["WHOLESALES_MANAGER"] != null && selectitem["WHOLESALES_MANAGER"].ToString().Equals(this.Sessions.UserID) ? status : (selectitem["WHOLESALES_MANAGER_STATUS"] != null ? selectitem["WHOLESALES_MANAGER_STATUS"].ToString().Trim() : ""));
    //            item.WHOLESALES_SPECIALIST_STATUS = (selectitem["WHOLESALES_SPECIALIST"] != null && selectitem["WHOLESALES_SPECIALIST"].ToString().Equals(this.Sessions.UserID) ? status : (selectitem["WHOLESALES_SPECIALIST_STATUS"] != null ? selectitem["WHOLESALES_SPECIALIST_STATUS"].ToString().Trim() : ""));
    //            item.SALES_ADMIN_STATUS = (selectitem["SALES_ADMIN"] != null && selectitem["SALES_ADMIN"].ToString().Equals(this.Sessions.UserID) ? status : (selectitem["SALES_ADMIN_STATUS"] != null ? selectitem["SALES_ADMIN_STATUS"].ToString().Trim() : ""));
    //            item.UPDATE_ID = this.Sessions.UserID;

    //            list.Add(item);
    //        }

    //        UpdateReturnGoods(list);
    //        GridSource();
    //        GridBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}


    private void UpdateReturnGoods(List<DTO_REPORTING_RETURN_GOODS> list)
    {
        try
        {
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

    protected void btnApproval_Click(object sender, EventArgs e)
    {
        try
        {
            List<DTO_REPORTING_RETURN_GOODS> goodlists = new List<DTO_REPORTING_RETURN_GOODS>();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_REPORTING_RETURN_GOODS> lists = (List<DTO_REPORTING_RETURN_GOODS>)serializer.Deserialize<List<DTO_REPORTING_RETURN_GOODS>>(this.hhdCheckItem.Value);

            string status = hhdUpdateType.Value;

            foreach (DTO_REPORTING_RETURN_GOODS item in lists)
            {
                DTO_REPORTING_RETURN_GOODS good = new DTO_REPORTING_RETURN_GOODS();
                good.IDX = Convert.ToInt32(item.IDX);
                good.TYPE = item.TYPE;
                good.STATUS = item.STATUS;
                good.SHIPTO_CODE = (item.SHIPTO_CODE == null ? "" : item.SHIPTO_CODE);
                good.INVOICE_PRICE = Convert.ToDecimal(item.INVOICE_PRICE);
                good.WHOLESALES_MANAGER_STATUS = (item.WHOLESALES_MANAGER != null && item.WHOLESALES_MANAGER.ToString().Equals(this.Sessions.UserID) ? status : (item.WHOLESALES_MANAGER_STATUS != null ? item.WHOLESALES_MANAGER_STATUS.ToString().Trim() : ""));

                good.WHOLESALES_SPECIALIST_STATUS = (item.WHOLESALES_SPECIALIST != null && item.WHOLESALES_SPECIALIST.ToString().Equals(this.Sessions.UserID) ? status : (item.WHOLESALES_SPECIALIST_STATUS != null ? item.WHOLESALES_SPECIALIST_STATUS.ToString().Trim() : ""));

                good.SALES_ADMIN_STATUS = (item.SALES_ADMIN != null && item.SALES_ADMIN.ToString().Equals(this.Sessions.UserID) ? status : (item.SALES_ADMIN_STATUS != null ? item.SALES_ADMIN_STATUS.ToString().Trim() : ""));

                good.UPDATE_ID = Sessions.UserID;
                goodlists.Add(good);
            }
            goodlists.ToList();

            UpdateReturnGoods(goodlists);
            //2015-01-19 추가
            SendPushMail();
            GridSource();
            GridBind();
        }
        catch (Exception ex)
        {            
            throw ex;
        }
        
    }

    /// <summary>
    /// 결재할 사용자에게 메일 발송
    /// </summary>
    private void SendPushMail()
    {
        try
        {
            //string senderid = this.Sessions.UserID;
            //string senderaddress = this.Sessions.MailAddress;
            //string sendmailtype = "ReturnGoods";
            //string wcfUrl = DNSoft.eW.FrameWork.eWBase.GetConfig("//WCFServices/HostURL");
            //string serviceUrl = string.Format("{0}/MailServices.svc/SendNoticeMail/{1}/{2}/{3}", wcfUrl, sendmailtype, senderid, senderaddress);
            //HttpWebRequest request = WebRequest.Create(serviceUrl) as HttpWebRequest;
            //using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            //{
            //    if (response.StatusCode != HttpStatusCode.OK)
            //    {
            //        throw new Exception("Send PushMail Fail");
            //    }
            //}


            string senderid = this.Sessions.UserID;
            string senderaddress = this.Sessions.MailAddress;
            string sendmailtype = "ReturnGoods";
            string wcfUrl = DNSoft.eW.FrameWork.eWBase.GetConfig("//WCFServices/HostURL");
            string serviceUrl = string.Format("{0}/MailServices.svc/SendNoticeMailApprover/{1}/{2}/{3}", wcfUrl, sendmailtype, senderid, senderaddress);
            HttpWebRequest request = WebRequest.Create(serviceUrl) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Send PushMail Fail");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}