using Bayer.eWF.BSL.Reporting.Dto;
using Bayer.eWF.BSL.Reporting.Mgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class ReturnGoods_SAPAmount : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                List<int> listyear = Enumerable.Range(2014, Convert.ToInt32(DateTime.Now.Year.ToString()) - 2013).ToList();
                List<int> listmonth = Enumerable.Range(1, 12).ToList();

                GridSource();
                GridBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }


    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.Equals("ExcelUpload")) //엑셀 업로드
        {
            InsertExcelTemp();
            GridSource();
            GridBind();
        }
        else if (e.Argument.Equals("GridClear")) //그리드 초기화 (비우기)
        {
            GridSource();
            GridBind();
        }
    }

    #region GridSource
    private void GridSource()
    {
        List<DTO_REPORTING_RETURN_GOODS> list = null;

        using (ReportingMgr mgr = new ReportingMgr())
        {
            if (this.RadAsyncUpload.UploadedFiles.Count > 0)
            {
                list = mgr.SelectReturnGoodsSAPExcel(this.Sessions.UserID);
            }
            else
            {
                list = mgr.SelectReturnGoods("D", this.Sessions.UserID);
            }
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
            if (item["SAP_AMOUNT"].Text == "0.000" || item["SAP_AMOUNT"].Text.Equals("&nbsp;") || item["UNIT_PRICE"].Text.Equals("&nbsp;"))
            {
                item.ForeColor = System.Drawing.Color.Red;
            }
        }

    }

    private void UpdateSAPData()
    {
        try
        {
            using (ReportingMgr mgr = new ReportingMgr())
            {
                mgr.UpdateReturnGoodsSAPExcel(this.Sessions.UserID);
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }

    }
    protected void btnSAPAmount_Click(object sender, EventArgs e)
    {
        UpdateSAPData();
        GridSource();
        GridBind();
    }




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
                    List<DTO_REPORTING_RETURN_GOODS_SAP_EXCEL> list = GetDataSetFromExcel(uploadpath);

                    mgr.InsertReturnGoodsSAPExcel(list);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

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
    public List<DTO_REPORTING_RETURN_GOODS_SAP_EXCEL> GetDataSetFromExcel(string filepath)
    {
        List<DTO_REPORTING_RETURN_GOODS_SAP_EXCEL> list = null;
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
    private List<DTO_REPORTING_RETURN_GOODS_SAP_EXCEL> ChangeDataFormat(DataSet ds)
    {
        List<DTO_REPORTING_RETURN_GOODS_SAP_EXCEL> datalist = new List<DTO_REPORTING_RETURN_GOODS_SAP_EXCEL>();

        try
        {
            if (ds != null)
            {
                DateTime nowdt = DateTime.Now;
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Columns.Count > 20)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            DTO_REPORTING_RETURN_GOODS_SAP_EXCEL item = new DTO_REPORTING_RETURN_GOODS_SAP_EXCEL();
                            if (dr[0].ToString().Trim() == "")
                                break;
                            //P2R 시스템 변경으로 -WooKyung Kim(2015.01.14)
                            //item.SHIPTO_CODE = dr[14].ToString().Trim();
                            //item.PRODUCT_CODE = dr[26].ToString().Trim();
                            //item.QTY = Convert.ToDouble(dr[32].ToString().Trim()) * -1;
                            //item.BATCH = dr[90].ToString().Trim();
                            //item.SN = dr[82].ToString().Trim();
                            //item.SAP_AMOUNT = Convert.ToDecimal(dr[48].ToString().Trim()) * -1;
                            //item.UNIT_PRICE = Convert.ToDecimal(dr[40].ToString().Trim());
                            if (dr[12].ToString().Trim() == "" || dr[12].ToString().Trim() == "0002740685")
                            {
                                item.SHIPTO_CODE = dr[10].ToString().Trim();
                            }
                            else
                            {
                                item.SHIPTO_CODE = dr[12].ToString().Trim();
                            }
                            
                            item.PRODUCT_CODE = dr[18].ToString().Trim();
                            item.QTY = Convert.ToDouble(dr[21].ToString().Trim());
                            item.BATCH = dr[26].ToString().Trim();
                            item.SN = dr[25].ToString().Trim();
                            item.SAP_AMOUNT = Convert.ToDecimal(dr[23].ToString().Trim());
                            item.UNIT_PRICE = Convert.ToDecimal(dr[27].ToString().Trim());


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
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        string yearmonth = DateTime.Now.ToString("yyyy-MM-dd_hh");
        string filename = yearmonth + "SapAmount.xls";

        // This actually makes your HTML output to be downloaded as .xls file
        Response.Clear();
        Response.ClearContent();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
        //Response.Charset = "utf-8";

        Response.Charset = "euc-kr";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");

        // Create a dynamic control, populate and render it
        GridView excel = new GridView();
        using (ReportingMgr mgr = new ReportingMgr())
        {
            excel.DataSource = mgr.SelectReturnGoods("D", this.Sessions.UserID);
        }        
        excel.DataBind();
        excel.RenderControl(new HtmlTextWriter(Response.Output));

        Response.Flush();
        Response.End();
    }
}