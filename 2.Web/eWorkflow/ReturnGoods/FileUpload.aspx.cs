using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Telerik.Web.UI;
using System.Data.OleDb;
using System.Data;
using Bayer.eWF.BSL.Reporting.Mgr;
using Bayer.eWF.BSL.Reporting.Dto;
using System.Net;

public partial class ReturnGoods_FileUpload : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.grdExcelFile.DataSource = string.Empty;
            GridBind();
        }
    }

    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.Equals("GridClear")) //그리드 초기화 (비우기)
        {
            this.grdExcelFile.DataSource = string.Empty;
            GridBind();
        }
        else if (e.Argument.Equals("ExcelUpload")) //엑셀 업로드
        {
            InsertExcelTemp();
            GridSource(); 
            GridBind();
        }
    }

    #region 클릭 이벤트
    /// <summary>
    /// Update 버튼 클릭 이벤트
    /// (엑셀에 포함된 리스트를 DB에 업데이트한다.)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdateExcel_Click(object sender, EventArgs e)
    {
        InsertReturnGoods();
        SendPushMail();
        // 그리드 초기화
        this.grdExcelFile.DataSource = string.Empty;
        GridBind();
    }

    protected void btnDeleteData_Click(object sender, EventArgs e)
    {
        DeleteExcelTemp();
        GridSource();
        GridBind();
    }
    #endregion

    protected void grdExcelFile_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        GridSource();
    }
    protected void grdExcelFile_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (e.Item as GridDataItem);
            if (item["CUSTOMER_NAME"].Text.Equals("&nbsp;") || item["PRODUCT_NAME"].Text.Equals("&nbsp;"))
            {
                item.ForeColor = System.Drawing.Color.Red;                
            }
            item["CUSTOMER_NAME"].Text = item["CUSTOMER_NAME"].Text + "<br/> (" + item["CUSTOMER_CODE"].Text + ")";
            item["PRODUCT_NAME"].Text = item["PRODUCT_NAME"].Text + "<br/> (" + item["PRODUCT_CODE"].Text + ")";
        }
    }

    #region GridSource
    /// <summary>
    /// 그리드 리스트 가져오기
    /// </summary>
    private void GridSource()
    {         
        //엑셀 업데이트 리스트 가져오기
        using (ReportingMgr mgr = new ReportingMgr())
        {
            List<DTO_REPORTING_RETURN_GOODS>  list = mgr.SelectReturnGoodsExcel(this.Sessions.UserID);

             if (list != null)
             {
                 this.grdExcelFile.DataSource = list;
             }           
        }
        

        
    }
    private void GridBind()
    {
        this.grdExcelFile.DataBind();
    }
    #endregion

    #region DB관련

    /// <summary>
    /// 엑셀에 포함된 리스트를 Temp DB에 업데이트
    /// </summary>
    private void InsertExcelTemp()
    {
        string uploadpath = string.Empty;
        try
        {
            if (this.RadAsyncUpload.UploadedFiles.Count > 0)
            {
                uploadpath = ExcelUploadTemp(this.RadAsyncUpload.UploadedFiles[0]);

                //엑셀에 포함된 리스트를 Temp DB에 업데이트
                using (ReportingMgr mgr = new ReportingMgr())
                {
                    List<DTO_REPORTING_RETURN_GOODS_EXCEL> list = GetDataSetFromExcel(uploadpath);

                    mgr.InsertReturnGoodsExcel(list);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 엑셀에 포함된 리스트를 Temp DB에 업데이트
    /// </summary>
    private void DeleteExcelTemp()
    {
        try
        {
            //엑셀에 포함된 리스트를 Temp DB에 업데이트
            using (ReportingMgr mgr = new ReportingMgr())
            {
                List<DTO_REPORTING_RETURN_GOODS> list = new List<DTO_REPORTING_RETURN_GOODS>();
                foreach(GridDataItem selectitem in this.grdExcelFile.SelectedItems)
                {
                    DTO_REPORTING_RETURN_GOODS item = new DTO_REPORTING_RETURN_GOODS();
                    item.IDX = Convert.ToInt32(selectitem["IDX"].Text);
                    item.CREATE_ID = this.Sessions.UserID;
                    list.Add(item);
                }
                mgr.DeleteReturnGoodsExcel(list);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    ///Temp DB를 업로드함
    /// </summary>
    private void InsertReturnGoods()
    {
        try
        {
            //엑셀에 포함된 리스트를 Temp DB에 업데이트
            using (ReportingMgr mgr = new ReportingMgr())
            {
                mgr.InsertReturnGoods(this.Sessions.UserID);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region 엑셀 데이터 가져오기 관련
    /// <summary>
    /// 엑셀파일을 임시폴더에 추가한다.
    /// </summary>
    /// <param name="file">업로드할 파일정보 (telerik file)</param>
    private string ExcelUploadTemp(UploadedFile file)
    {
        Stream oStm = null;
        FileStream oFileStream = null;
        byte[] buffer = null;

        string strReturn = string.Empty;

        try
        {
            string strTempUploadPath = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/WebServer/FileMgr/UploadTempFolder");
            string strUserID = this.Sessions.UserID;
            string strUploadFolder = string.Format(@"{0}\{1}\FileUpload\", strTempUploadPath, strUserID);

            buffer = new byte[file.ContentLength];
            using (oStm = file.InputStream)
            {
                int nbytesRead = oStm.Read(buffer, 0, Convert.ToInt32(file.ContentLength));
                if (!Directory.Exists(strUploadFolder))
                    Directory.CreateDirectory(strUploadFolder);
                else
                {
                    foreach (string path in Directory.GetFiles(strUploadFolder))
                    {
                        File.Delete(path);
                    }
                }

                oFileStream = new FileStream(strUploadFolder + file.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                oFileStream.Write(buffer, 0, nbytesRead);
            }
            strReturn = strUploadFolder + file.FileName;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (oStm != null)
            {
                oStm.Close();
                oStm.Dispose();
                oStm = null;
            }
            if (oFileStream != null)
            {
                oFileStream.Close();
                oFileStream.Dispose();
                oFileStream = null;
            } 
        }
        return strReturn;
    }

    /// <summary>
    /// 엑셀로부터 리스트 항목 가져오기
    /// </summary>
    /// <param name="filepath">임시폴더의 엑셀 파일 위치</param>
    /// <returns></returns>
    public List<DTO_REPORTING_RETURN_GOODS_EXCEL> GetDataSetFromExcel(string filepath)
    {
        List<DTO_REPORTING_RETURN_GOODS_EXCEL> list = null;
        try
        {
            DataSet data = new DataSet();
            //AccessDatabaseEngine.exe, AccessDatabaseEngine_X64.exe 설치 필요.
            string connectionString = string.Format("provider=Microsoft.ACE.OLEDB.12.0; data source={0};Extended Properties=\"Excel 12.0\"", filepath);

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();

                DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataTable dataTable = new DataTable();
                    string query = string.Format("SELECT * FROM [{0}]", dt.Rows[0]["TABLE_NAME"].ToString());
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
                    adapter.Fill(dataTable);
                    data.Tables.Add(dataTable);
                }
                con.Close();
                con.Dispose();
            }

            list = ChangeDataFormat(data);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }

        return list;
    }

    /// <summary>
    /// dataset을 리스트 형태로 변환한다.
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    private List<DTO_REPORTING_RETURN_GOODS_EXCEL> ChangeDataFormat(DataSet ds)
    {
        List<DTO_REPORTING_RETURN_GOODS_EXCEL> datalist = new List<DTO_REPORTING_RETURN_GOODS_EXCEL>();

        try
        {
            if (ds != null)
            {
                DateTime nowdt = DateTime.Now;
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    //컬럼 확인
                    if (dt.Columns["SN"] == null){ throw new Exception("[SN] 컬럼을 찾을 수 없습니다."); }
                    else if (dt.Columns["Date"] == null) { throw new Exception("[Date] 컬럼을 찾을 수 없습니다."); }
                    else if (dt.Columns["Div"] == null) { throw new Exception("[Div] 컬럼을 찾을 수 없습니다."); }
                    else if (dt.Columns["Customer Code"] == null) { throw new Exception("[Customer Code] 컬럼을 찾을 수 없습니다."); }
                    else if (dt.Columns["Product Code"] == null) { throw new Exception("[Product Code] 컬럼을 찾을 수 없습니다."); }
                    else if (dt.Columns["Batch"] == null) { throw new Exception("[Batch] 컬럼을 찾을 수 없습니다."); }
                    else if (dt.Columns["Q'ty"] == null) { throw new Exception("[Q'ty] 컬럼을 찾을 수 없습니다."); }
                    else if (dt.Columns["Expiry"] == null) { throw new Exception("[Expiry] 컬럼을 찾을 수 없습니다."); }
                    else if (dt.Columns["S1"] == null) { throw new Exception("[S1] 컬럼을 찾을 수 없습니다."); }
                    else if (dt.Columns["S2"] == null) { throw new Exception("[S2] 컬럼을 찾을 수 없습니다."); }
                    else if (dt.Columns["Invoice Price"] == null) { throw new Exception("[Invoice Price] 컬럼을 찾을 수 없습니다."); }
                    else if (dt.Columns["Reason"] == null) { throw new Exception("[Reason] 컬럼을 찾을 수 없습니다."); }
                    else if (dt.Columns["VC 담당자"] == null) { throw new Exception("[VC 담당자] 컬럼을 찾을 수 없습니다."); }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["SN"].ToString().Equals("")) continue;
                            DTO_REPORTING_RETURN_GOODS_EXCEL item = new DTO_REPORTING_RETURN_GOODS_EXCEL();
                            item.SN = dr["SN"].ToString().Trim();
                            item.DATE = Convert.ToDateTime(dr["Date"].ToString().Trim());
                            item.DIV = dr["Div"].ToString().Trim();
                            item.CUSTOMER_CODE = dr["Customer Code"].ToString().Trim();
                            item.PRODUCT_CODE = dr["Product Code"].ToString().Trim();
                            item.BATCH = dr["Batch"].ToString().Trim();
                            item.QTY = Convert.ToDouble(dr["Q'ty"].ToString().Trim());                            
                            item.EXPIRY = Convert.ToDateTime(dr["Expiry"].ToString().Trim());
                            item.S1 = dr["S1"].ToString().Trim();
                            item.S2 = dr["S2"].ToString().Trim();
                            item.INVOICE_PRICE = (!dr["Invoice Price"].ToString().Trim().Equals("") ? Convert.ToDecimal(dr["Invoice Price"].ToString().Trim()) : 0);
                            item.REASON = dr["Reason"].ToString().Trim();
                            item.VC_MANAGER = dr["VC 담당자"].ToString().Trim();
                            item.CREATE_DATE = nowdt;
                            item.CREATE_ID = this.Sessions.UserID;

                            datalist.Add(item);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { }
        return datalist;
    }
    #endregion


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