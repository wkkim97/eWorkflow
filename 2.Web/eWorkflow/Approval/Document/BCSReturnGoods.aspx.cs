using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eWF.FrameWork.Web;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Dao;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using System.Web.Script.Serialization;
using Telerik.Web.UI;

public partial class Approval_Document_BCSReturnGoods : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_BCS_RETURN_GOODS_GRID = "VIEWSTATE_BCS_RETURN_GOODS_GRID";
    string Radio;
    string type;

    #region OnPreInit Event
    /// <summary>
    /// 최초 페이지 로드시 호출
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        var masterPage = Master;
        this.webMaster = (masterPage as Master_eWorks_Document);
        base.OnPreInit(e);
    }
    #endregion

    #region Page Load Event
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                ViewState[VIEWSTATE_BCS_RETURN_GOODS_GRID] = new List< DTO_DOC_BCS_RETURN_GOODS_GRID>();
                this.radGrdReturnGoods.DataSource = (List<DTO_DOC_BCS_RETURN_GOODS_GRID>)ViewState[VIEWSTATE_BCS_RETURN_GOODS_GRID];
                this.radGrdReturnGoods.DataBind();

                InitPageInfo();
            }
            PageLoadInfo();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion

    #region InitPageInfo
    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0034";
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        //hddProcessID.Value = "";

        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {
        DTO_DOC_BCS_RETURN_GOODS doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            string processId = this.hddProcessID.Value;
            using (Bayer.eWF.BSL.Approval.Mgr.BCSReturnGoodsMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.BCSReturnGoodsMgr())
            {
                doc = mgr.SelectBcsReturnGoods(hddProcessID.Value);
                this.hddProcessStatus.Value = doc.PROCESS_STATUS;

                if (doc != null)
                {

                    this.radRdoBG3.Visible = false;
                    if (doc.BG == radRdoBG1.Value)
                    {
                        this.radRdoBG1.Checked = true;
                        
                    }
                    else if (doc.BG == radRdoBG2.Value)
                    {
                        this.radRdoBG2.Checked = true;
                        
                    }
                    else if (doc.BG == radRdoBG3.Value)   // BVS
                    {
                        this.radRdoBG3.Checked = true;
                        
                    }
                    else if (doc.BG == radRdoBG4.Value)
                        this.radRdoBG4.Checked = true;


                    //2014.12.12 Distribution channel 필드추가
                    if (doc.DISTRIBUTION == radRdoFM.Value) this.radRdoFM.Checked = true;
                    else if (doc.DISTRIBUTION == radRdoNH.Value) this.radRdoNH.Checked = true;

                    if (doc.TYPE == radRdoType1.Value)
                    {
                        this.radRdoType1.Checked = true;
                        this.radRdoType1.Visible = true;
                        this.RadtextReason.Text = "K61 - KR expired return";
                    }
                    else if (doc.TYPE == radRdoType2.Value)
                    {
                        this.radRdoType2.Checked = true;
                        this.radRdoType2.Visible = true;
                        this.RadDropUnexpired.SelectedValue = doc.REASON;
                    }
                    else if (doc.TYPE == radRdoType3.Value)
                    {
                        this.radRdoType3.Checked = true;
                        this.radRdoType3.Visible = true;
                        this.RadDropPaper.SelectedValue = doc.REASON;
                    }else if(doc.TYPE == radRdoType4.Value)
                    {
                        this.radRdoType4.Checked = true;
                        this.RadtextReason.Text = "K61 - KR expired return";
                    }
                    else if (doc.TYPE == radRdoType5.Value)
                    {
                        this.radRdoType5.Checked = true;
                        this.RadDropUnexpired.SelectedValue = doc.REASON;
                    }
                    else if (doc.TYPE == radRdoType6.Value)
                    {
                        this.radRdoType6.Checked = true;
                        this.RadDropPaper.SelectedValue = doc.REASON;
                    }
                    else if (doc.TYPE == radRdoType7.Value)
                    {
                        this.radRdoType7.Checked = true;
                        this.RadDropUnexpired.SelectedValue = doc.REASON;
                    }

                    //2014.12.16 컬럼 추가
                    this.RadtextRemark.Text = doc.REMARK;

                    List<DTO_DOC_BCS_RETURN_GOODS_LIST> lists = mgr.SelectBcsReturnGoodsList(hddProcessID.Value);  // DB에서 가져온 리스트
                    List<DTO_DOC_BCS_RETURN_GOODS_GRID> gridList = new List<DTO_DOC_BCS_RETURN_GOODS_GRID>(); // 그리드에 뿌릴 DTO List
                    SetVisible();
                    if (doc.TYPE == this.radRdoType1.Value)
                    {
                        foreach (DTO_DOC_BCS_RETURN_GOODS_LIST item in lists)
                        {
                            DTO_DOC_BCS_RETURN_GOODS_GRID griditem = new DTO_DOC_BCS_RETURN_GOODS_GRID();
                            griditem.PROCESS_ID = item.PROCESS_ID;
                            griditem.CREATOR_ID = item.CREATOR_ID;
                            griditem.IDX = item.IDX;
                            griditem.CUSTOMER_CODE = item.CUSTOMER_CODE;
                            griditem.CUSTOMER_NAME = item.CUSTOMER_NAME;
                            griditem.PRODUCT_CODE = item.CUR_PRODUCT_CODE;
                            griditem.PRODUCT_NAME = item.CUR_PRODUCT_NAME;
                            griditem.QTY = item.CUR_QTY;
                            //griditem.REASON = item.CUR_REASON;
                            griditem.INVOICE_PRICE = item.CUR_UNIT_PRICE;
                            griditem.AMOUNT = item.CUR_AMOUNT;

                            gridList.Add(griditem);
                        }
                        gridList.ToList();
                        ViewState[VIEWSTATE_BCS_RETURN_GOODS_GRID] = gridList;
                        this.radGrdReturnGoods.DataSource = gridList;
                        this.radGrdReturnGoods.DataBind();
                    }

                    if (doc.TYPE == this.radRdoType4.Value || doc.TYPE == this.radRdoType5.Value || doc.TYPE == this.radRdoType6.Value || doc.TYPE == this.radRdoType7.Value)
                    {
                        foreach (DTO_DOC_BCS_RETURN_GOODS_LIST item in lists)
                        {
                            DTO_DOC_BCS_RETURN_GOODS_GRID griditem = new DTO_DOC_BCS_RETURN_GOODS_GRID();
                            griditem.PROCESS_ID = item.PROCESS_ID;
                            griditem.CREATOR_ID = item.CREATOR_ID;
                            griditem.IDX = item.IDX;
                            griditem.CUSTOMER_CODE = item.CUSTOMER_CODE;
                            griditem.CUSTOMER_NAME = item.CUSTOMER_NAME;
                            griditem.CUSTOMER_NAME_NEW = item.CUSTOMER_NAME_NEW;
                            griditem.PRODUCT_CODE = item.CUR_PRODUCT_CODE;
                            griditem.PRODUCT_NAME = item.CUR_PRODUCT_NAME;
                            griditem.RETURN_PRICE = item.RETURN_PRICE;
                            griditem.RETURN_PRICE_NEW = item.RETURN_PRICE_NEW;
                            griditem.QTY = item.CUR_QTY;
                            griditem.DIFFERENCE = item.DIFFERENCE;
                            griditem.TOTAL_AMOUNT = item.TOTAL_AMOUNT;
                            gridList.Add(griditem);
                        }
                        gridList.ToList();
                        ViewState[VIEWSTATE_BCS_RETURN_GOODS_GRID] = gridList;
                        this.radGrdReturnGoods.DataSource = gridList;
                        this.radGrdReturnGoods.DataBind();
                    }
                    if (doc.TYPE == this.radRdoType2.Value || doc.TYPE == this.radRdoType3.Value)
                    {
                        foreach (GridColumn column in radGrdReturnGoods.Columns)
                        {
                            if (column.UniqueName == "CU_RE")
                                (column as GridBoundColumn).Display = true;
                        }
                        bool multi = true;              // 그리드에  2줄로 넣기 위한 구분자
                        for (int i = 0; i < lists.Count; i++)
                        {
                            if (multi)
                            {
                                DTO_DOC_BCS_RETURN_GOODS_GRID griditem = new DTO_DOC_BCS_RETURN_GOODS_GRID();
                                griditem.CU_RE = "C";
                                griditem.PROCESS_ID = lists[i].PROCESS_ID;
                                griditem.CREATOR_ID = lists[i].CREATOR_ID;
                                griditem.IDX = lists[i].IDX;
                                griditem.CUSTOMER_CODE = lists[i].CUSTOMER_CODE;
                                griditem.CUSTOMER_NAME = lists[i].CUSTOMER_NAME;
                                griditem.PRODUCT_CODE = lists[i].CUR_PRODUCT_CODE;
                                griditem.PRODUCT_NAME = lists[i].CUR_PRODUCT_NAME;
                                griditem.QTY = lists[i].CUR_QTY;
                                //griditem.REASON = lists[i].CUR_REASON;
                                griditem.INVOICE_PRICE = lists[i].CUR_UNIT_PRICE;
                                griditem.AMOUNT = lists[i].CUR_AMOUNT;

                                gridList.Add(griditem);   // Current Row
                            }
                            if (multi)
                            {
                                DTO_DOC_BCS_RETURN_GOODS_GRID griditem = new DTO_DOC_BCS_RETURN_GOODS_GRID();
                                griditem.CU_RE = "R";
                                griditem.IDX = lists[i].IDX;
                                griditem.CUSTOMER_CODE = lists[i].CUSTOMER_CODE;
                                griditem.CUSTOMER_NAME = lists[i].CUSTOMER_NAME;
                                griditem.PRODUCT_CODE = lists[i].REP_PRODUCT_CODE;
                                griditem.PRODUCT_NAME = lists[i].REP_PRODUCT_NAME;
                                griditem.QTY = lists[i].REP_QTY;
                                //griditem.REASON = lists[i].REP_REASON;
                                griditem.INVOICE_PRICE = lists[i].REP_UNIT_PRICE;
                                griditem.AMOUNT = lists[i].REP_AMOUNT;

                                gridList.Add(griditem);   // TO-BE Row
                            }
                        }
                        gridList.ToList();
                        ViewState[VIEWSTATE_BCS_RETURN_GOODS_GRID] = gridList;
                        this.radGrdReturnGoods.DataSource = gridList;
                        this.radGrdReturnGoods.DataBind();
                    }

                    if (!ClientScript.IsStartupScriptRegistered("setVisible"))
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisible", "setVisible('" + doc.BG + "','" + doc.TYPE + "');", true);
                    webMaster.DocumentNo = doc.DOC_NUM;
                }                
            }
        }
    }
    #endregion

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        if (this.hddGridItems.Value != "")
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_DOC_BCS_RETURN_GOODS_GRID> items = (List<DTO_DOC_BCS_RETURN_GOODS_GRID>)serializer.Deserialize<List<DTO_DOC_BCS_RETURN_GOODS_GRID>>(this.hddGridItems.Value);

            ViewState[VIEWSTATE_BCS_RETURN_GOODS_GRID] = items;
            this.radGrdReturnGoods.DataSource = items;
            this.radGrdReturnGoods.DataBind();
        }

        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdReturnGoods);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radGrdReturnGoods, this.radGrdReturnGoods);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_BcsReturnGoods_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoType1, radGrdReturnGoods, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoType2, radGrdReturnGoods, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoType3, radGrdReturnGoods, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoType4, radGrdReturnGoods, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoType5, radGrdReturnGoods, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoType6, radGrdReturnGoods, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoType7, radGrdReturnGoods, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoFM, radGrdReturnGoods, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radRdoNH, radGrdReturnGoods, null);
        

        Radio = GetSelectedBG();
        type = GetSelectedType();

        if (!ClientScript.IsStartupScriptRegistered("setVisible"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisible", "setVisible('" + Radio + "','" + type + "');", true);

       
    }
    #endregion

    #region Get Radio Value
    private string GetSelectedBG()
    {
        string Bg = string.Empty;
        foreach (Control control in divBG.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    Bg = (control as RadButton).Value;
                    break;
                }
            }
        }
        return Bg;
    }

    private string GetSelectedType()
    {
        string type = string.Empty;
        foreach (Control control in divType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    type = (control as RadButton).Value;
                    break;
                }
            }
        }
        return type;
    }

    private string GetSelectedTypeText()
    {
        string type = string.Empty;
        foreach (Control control in divType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    type = (control as RadButton).Text;
                    break;
                }
            }
        }
        return type;
    } 

    #endregion

    void Approval_Document_BcsReturnGoods_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("SetCustomer"))
        {
            string[] values = e.Argument.Split(new char[] { ':' });
            if (values.Length > 3)
            {
                string idx = values[1];
                string code = values[2];
                string name = values[3];
                string parvw = values[4];

                UpdateCustomerGridData(Convert.ToInt32(idx), code, name, parvw);

            }
        }
        else if (e.Argument.StartsWith("SetCurProduct"))
        {
            string[] values = e.Argument.Split(new char[] { ':' });
            if (values.Length > 5)
            {
                string Gindex = values[1];
                string idx = values[2];
                string code = values[3];
                string name = values[4];
                string price = values[5];

                UpdateCurProductGridData(false, Convert.ToInt32(Gindex), Convert.ToInt32(idx), code, name, price);
            }
        }
    }

    #region [ Add Row ]
    private void UpdateCustomerGridData(int idx, string customercode, string customername,string parvw)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BCS_RETURN_GOODS_GRID> items = (List<DTO_DOC_BCS_RETURN_GOODS_GRID>)serializer.Deserialize<List<DTO_DOC_BCS_RETURN_GOODS_GRID>>(this.hddGridItems.Value);

        if (idx < 999)
        {
            var list = (from item in items
                           where item.IDX == idx
                           select item).FirstOrDefault();
            if (list != null)
            {
                list.CUSTOMER_CODE = customercode;
                list.CUSTOMER_NAME = customername;
                list.PARVW = parvw;

            }
        }

        ViewState[VIEWSTATE_BCS_RETURN_GOODS_GRID] = items;
        this.radGrdReturnGoods.DataSource = items;
        this.radGrdReturnGoods.DataBind();

    }

    // Product Update Grid 
    private void UpdateCurProductGridData(bool addRow, int Gindex, int idx, string productcode, string productname, string invoiceprice)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BCS_RETURN_GOODS_GRID> items = (List<DTO_DOC_BCS_RETURN_GOODS_GRID>)serializer.Deserialize<List<DTO_DOC_BCS_RETURN_GOODS_GRID>>(this.hddGridItems.Value);
               
        if (idx < 999)
        {
            var list = (from item in items
                        where item.GRID_INDEX == Gindex                         
                        select item).FirstOrDefault();
            if (list != null)
            {
                list.PRODUCT_CODE = productcode;
                list.PRODUCT_NAME = productname;
                //
                list.INVOICE_PRICE = Convert.ToDecimal(invoiceprice);
            }
        }

        ViewState[VIEWSTATE_BCS_RETURN_GOODS_GRID] = items;
        this.radGrdReturnGoods.DataSource = items;
        this.radGrdReturnGoods.DataBind();

    }

    #endregion

    // Grid ReSET
    protected void radRdoType1_Click(object sender, EventArgs e)
    {
        SetVisible();
        

        ViewState[VIEWSTATE_BCS_RETURN_GOODS_GRID] = new List<DTO_DOC_BCS_RETURN_GOODS_GRID>();
        this.radGrdReturnGoods.DataSource = (List<DTO_DOC_BCS_RETURN_GOODS_GRID>)ViewState[VIEWSTATE_BCS_RETURN_GOODS_GRID];
        this.radGrdReturnGoods.DataBind();
    }

    private void SetVisible()
    {
        this.radGrdReturnGoods.MasterTableView.GetColumn("RETURN_PRICE").Display = false;
        this.radGrdReturnGoods.MasterTableView.GetColumn("RETURN_PRICE_NEW").Display = false;
        this.radGrdReturnGoods.MasterTableView.GetColumn("QTY").Display = false;
        this.radGrdReturnGoods.MasterTableView.GetColumn("DIFFERENCE").Display = false;
        this.radGrdReturnGoods.MasterTableView.GetColumn("TOTAL_AMOUNT").Display = false;
        this.radGrdReturnGoods.MasterTableView.GetColumn("INVOICE_PRICE").Display = false;
        this.radGrdReturnGoods.MasterTableView.GetColumn("AMOUNT").Display = false;
        this.radGrdReturnGoods.MasterTableView.GetColumn("CUSTOMER_NAME_NEW").Display = false;

        if (this.radRdoType1.Checked == true || this.radRdoType4.Checked == true|| this.radRdoType5.Checked == true|| this.radRdoType6.Checked == true || this.radRdoType7.Checked == true)
        {
            this.radGrdReturnGoods.MasterTableView.GetColumn("QTY").Display = true;
            this.radGrdReturnGoods.MasterTableView.GetColumn("INVOICE_PRICE").Display = true;
            this.radGrdReturnGoods.MasterTableView.GetColumn("AMOUNT").Display = true;

            foreach (GridColumn column in radGrdReturnGoods.Columns)
            {
                if (column.UniqueName == "CU_RE")
                    (column as GridBoundColumn).Display = false;
            }
           
            if (this.radRdoType4.Checked == true || this.radRdoType5.Checked == true || this.radRdoType6.Checked == true || this.radRdoType7.Checked == true)
            {
                this.radGrdReturnGoods.MasterTableView.GetColumn("QTY").Display = false;
                this.radGrdReturnGoods.MasterTableView.GetColumn("INVOICE_PRICE").Display = false;
                this.radGrdReturnGoods.MasterTableView.GetColumn("AMOUNT").Display = false;

                this.radGrdReturnGoods.MasterTableView.GetColumn("RETURN_PRICE").Display = true;
                this.radGrdReturnGoods.MasterTableView.GetColumn("RETURN_PRICE_NEW").Display = true;
                this.radGrdReturnGoods.MasterTableView.GetColumn("QTY").Display = true;
                this.radGrdReturnGoods.MasterTableView.GetColumn("DIFFERENCE").Display = true;
                this.radGrdReturnGoods.MasterTableView.GetColumn("TOTAL_AMOUNT").Display = true;
                if(this.radRdoType7.Checked == true) this.radGrdReturnGoods.MasterTableView.GetColumn("CUSTOMER_NAME_NEW").Display = true;
                else this.radGrdReturnGoods.MasterTableView.GetColumn("CUSTOMER_NAME_NEW").Display = false;
            }
        }

        if (this.radRdoType2.Checked == true || this.radRdoType3.Checked == true)
        {
            this.radGrdReturnGoods.MasterTableView.GetColumn("QTY").Display = true;
            this.radGrdReturnGoods.MasterTableView.GetColumn("INVOICE_PRICE").Display = true;
            this.radGrdReturnGoods.MasterTableView.GetColumn("AMOUNT").Display = true;

            foreach (GridColumn column in radGrdReturnGoods.Columns)
            {
                if (column.UniqueName == "CU_RE")
                    (column as GridBoundColumn).Display = true;
            }
        } 
    }

    protected void radGrdReturnGoods_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Remove"))
            {
                int index = Convert.ToInt32(e.CommandArgument);

                using (BCSReturnGoodsMgr Mgr = new BCSReturnGoodsMgr())
                {
                    Mgr.DeleteBcsReturnGoodsListByIndex(this.hddProcessID.Value, index);
                }
                List<DTO_DOC_BCS_RETURN_GOODS_GRID> list = (List<DTO_DOC_BCS_RETURN_GOODS_GRID>)ViewState[VIEWSTATE_BCS_RETURN_GOODS_GRID];
                list.RemoveAll(p => p.IDX == index);

                this.radGrdReturnGoods.DataSource = list;
                this.radGrdReturnGoods.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    protected override void DoRequest()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Request))
            {
                string ApprovalStatus = hddProcessID.Value.Length > 0 ? ApprovalUtil.ApprovalStatus.Saved.ToString() : ApprovalUtil.ApprovalStatus.Temp.ToString();
                hddProcessID.Value = DocumentSave(ApprovalStatus);
                webMaster.ProcessID = hddProcessID.Value;
                base.DoRequest();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }


    #region Approval
    protected override void DoApproval()
    {
        // TODO :

        base.DoApproval();
    } 
    #endregion

    #region DoSave
    protected override void DoSave()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Saved))
            {
                hddProcessID.Value = DocumentSave(ApprovalUtil.ApprovalStatus.Saved.ToString());
                webMaster.ProcessID = hddProcessID.Value;
                base.DoSave();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    } 
    #endregion

    #region Validation Check
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (!(this.radRdoBG1.Checked || this.radRdoBG2.Checked || this.radRdoBG3.Checked || this.radRdoBG4.Checked))
                message += "BG";
            //if (radRdoBG1.Checked == true && radRdoFM.Checked == false && radRdoNH.Checked == false)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Distribution channel";
            if (!( this.radRdoType4.Checked || this.radRdoType5.Checked || this.radRdoType6.Checked || this.radRdoType7.Checked))
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Type";
            //if (this.radGrdReturnGoods.Items.Count < 1)
            //    message += message.IsNullOrEmptyEx() ? "Grid Item" : ",Grid Item";
        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    } 
    #endregion

    private string DocumentSave(string processStatus)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_BCS_RETURN_GOODS doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_BCS_RETURN_GOODS();
        doc.PROCESS_STATUS = processStatus;
        doc.PROCESS_ID = hddProcessID.Value;
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = Sessions.UserID;
        doc.SUBJECT = GetSelectedBG() + " / " + GetSelectedTypeText();   //subject            
        webMaster.Subject = doc.SUBJECT;


        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.CREATE_DATE = DateTime.Now;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CREATOR_ID = Sessions.UserID;
        doc.IS_DISUSED = "N";

        string SelectedBG = GetSelectedBG();
        doc.BG = SelectedBG;

        //2014.12.12 컬럼 추가 : Distribuution channel 필드추가 
        if (SelectedBG == "CP")
        {
            if (radRdoFM.Checked) doc.DISTRIBUTION = radRdoFM.Value;
            else if (radRdoNH.Checked) doc.DISTRIBUTION = radRdoNH.Value;
        }

        string SelectedType = GetSelectedType();
        doc.TYPE = SelectedType;
        if (SelectedType == "Expired") doc.REASON = "K61";
        else if (SelectedType == "Unexpired") doc.REASON = this.RadDropUnexpired.SelectedValue;
        else if (SelectedType == "Paper") doc.REASON = this.RadDropPaper.SelectedValue;
        else if (SelectedType == "Expired_2020") doc.REASON = "K61";
        else if (SelectedType == "Unexpired_2020") doc.REASON = this.RadDropUnexpired.SelectedValue;
        else if (SelectedType == "Paper_2020") doc.REASON = this.RadDropPaper.SelectedValue;
        else if (SelectedType == "Customer_2020") doc.REASON = this.RadDropUnexpired.SelectedValue;


        //2014.12.16 컬럼추가
        doc.REMARK = this.RadtextRemark.Text;

        List<DTO_DOC_BCS_RETURN_GOODS_LIST> dbLists = new List<DTO_DOC_BCS_RETURN_GOODS_LIST>(); // 실제 DB상의 DTO

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BCS_RETURN_GOODS_GRID> GridItem = (List<DTO_DOC_BCS_RETURN_GOODS_GRID>)serializer.Deserialize<List<DTO_DOC_BCS_RETURN_GOODS_GRID>>(this.hddGridItems.Value);

        if (this.radRdoType1.Checked == true)   // Expired Good
        {
            foreach (DTO_DOC_BCS_RETURN_GOODS_GRID list in GridItem)
            {
                DTO_DOC_BCS_RETURN_GOODS_LIST dbList = new DTO_DOC_BCS_RETURN_GOODS_LIST();
                dbList.PROCESS_ID = this.hddProcessID.Value;
                dbList.CREATOR_ID = Sessions.UserID;
                dbList.IDX = list.IDX;
                dbList.CUSTOMER_CODE = list.CUSTOMER_CODE;
                dbList.CUSTOMER_NAME = list.CUSTOMER_NAME;
                dbList.CUR_PRODUCT_CODE = list.PRODUCT_CODE;
                dbList.CUR_PRODUCT_NAME = list.PRODUCT_NAME;
                dbList.CUR_QTY = list.QTY;
                //dbList.CUR_REASON = list.REASON;
                dbList.CUR_UNIT_PRICE = list.INVOICE_PRICE;
                dbList.CUR_AMOUNT = list.AMOUNT;

                dbLists.Add(dbList);
            }
            dbLists.ToList();
        }
        else if (this.radRdoType2.Checked == true || this.radRdoType3.Checked == true)
        {
            for (int i = 0; i < GridItem.Count; i = i + 2)
            {
                DTO_DOC_BCS_RETURN_GOODS_LIST dbList = new DTO_DOC_BCS_RETURN_GOODS_LIST();
                //Current
                dbList.PROCESS_ID = this.hddProcessID.Value;
                dbList.CREATOR_ID = Sessions.UserID;
                dbList.IDX = GridItem[i].IDX;
                dbList.CUSTOMER_CODE = GridItem[i].CUSTOMER_CODE;
                dbList.CUSTOMER_NAME = GridItem[i].CUSTOMER_NAME;
                dbList.CUR_PRODUCT_CODE = GridItem[i].PRODUCT_CODE;
                dbList.CUR_PRODUCT_NAME = GridItem[i].PRODUCT_NAME;
                dbList.CUR_QTY = GridItem[i].QTY;
                //dbList.CUR_REASON = GridItem[i].REASON;   // 컬럼 삭제 (요청자 : 김우경님)
                dbList.CUR_UNIT_PRICE = GridItem[i].INVOICE_PRICE;
                dbList.CUR_AMOUNT = GridItem[i].AMOUNT;
                //Replacement
                dbList.CUSTOMER_CODE = GridItem[i].CUSTOMER_CODE;
                dbList.CUSTOMER_NAME = GridItem[i].CUSTOMER_NAME;
                dbList.REP_PRODUCT_CODE = GridItem[i + 1].PRODUCT_CODE;
                dbList.REP_PRODUCT_NAME = GridItem[i + 1].PRODUCT_NAME;
                dbList.REP_QTY = GridItem[i + 1].QTY;
                //dbList.REP_REASON = GridItem[i + 1].REASON;
                dbList.REP_UNIT_PRICE = GridItem[i + 1].INVOICE_PRICE;
                dbList.REP_AMOUNT = GridItem[i + 1].AMOUNT;

                dbLists.Add(dbList);
            }
            dbLists.ToList();
        }else if (this.radRdoType4.Checked == true || this.radRdoType5.Checked == true || this.radRdoType6.Checked == true || this.radRdoType7.Checked == true)   // Expired Good
        {
            foreach (DTO_DOC_BCS_RETURN_GOODS_GRID list in GridItem)
            {
                DTO_DOC_BCS_RETURN_GOODS_LIST dbList = new DTO_DOC_BCS_RETURN_GOODS_LIST();
                dbList.PROCESS_ID = this.hddProcessID.Value;
                dbList.CREATOR_ID = Sessions.UserID;
                dbList.IDX = list.IDX;
                dbList.CUSTOMER_CODE = list.CUSTOMER_CODE;
                dbList.CUSTOMER_NAME = list.CUSTOMER_NAME;
                dbList.CUSTOMER_NAME_NEW = list.CUSTOMER_NAME_NEW;
                dbList.CUR_PRODUCT_CODE = list.PRODUCT_CODE;
                dbList.CUR_PRODUCT_NAME = list.PRODUCT_NAME;
                dbList.RETURN_PRICE = list.RETURN_PRICE;
                dbList.RETURN_PRICE_NEW = list.RETURN_PRICE_NEW;
                dbList.DIFFERENCE = list.DIFFERENCE;
                dbList.TOTAL_AMOUNT = list.TOTAL_AMOUNT;
                dbList.CUR_QTY = list.QTY;
                //dbList.CUR_REASON = list.REASON;
                dbList.CUR_UNIT_PRICE = 0;
                dbList.CUR_AMOUNT = 0;
                dbLists.Add(dbList);
            }
            dbLists.ToList();
        }

        using (Bayer.eWF.BSL.Approval.Mgr.BCSReturnGoodsMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.BCSReturnGoodsMgr())
        {
            processID = mgr.MergeBcsReturnGoods(doc, dbLists);
        }
        return processID;
    }

    protected void radGrdReturnGoods_PreRender(object sender, EventArgs e)
    {
        if (this.radRdoType2.Checked == true || this.radRdoType3.Checked == true)
        {
            for (int rowIndex = radGrdReturnGoods.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridDataItem row = radGrdReturnGoods.Items[rowIndex];
                GridDataItem previousRow = radGrdReturnGoods.Items[rowIndex + 1];

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (row["IDX"].Text == previousRow["IDX"].Text)
                    {
                        row["IDX"].RowSpan = previousRow["IDX"].RowSpan < 2 ? 2 : previousRow["IDX"].RowSpan + 1;
                        previousRow["IDX"].Attributes.CssStyle.Add("Display", "none");
                    }

                    if (row["IDX"].Text == previousRow["IDX"].Text)
                    {
                        row["CUSTOMER_NAME"].RowSpan = previousRow["CUSTOMER_NAME"].RowSpan < 2 ? 2 : previousRow["CUSTOMER_NAME"].RowSpan + 1;
                        previousRow["CUSTOMER_NAME"].Attributes.CssStyle.Add("Display", "none");                        
                    }

                    if (row["IDX"].Text == previousRow["IDX"].Text)
                    {
                        row["REMOVE_BUTTON"].RowSpan = previousRow["REMOVE_BUTTON"].RowSpan < 2 ? 2 : previousRow["REMOVE_BUTTON"].RowSpan + 1;
                        previousRow["REMOVE_BUTTON"].Attributes.CssStyle.Add("Display", "none");
                    }
                }
                
            }
        }
    }

}
