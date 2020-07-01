using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dto;
using Telerik.Web.UI;
using System.Web.Script.Serialization;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Mgr;

public partial class Approval_Document_BmskPrice : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_KEY = "VIEWSTATE_KEY_CUSTOMER";
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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                InitPageInfo();

            }
            PageLoadInfo();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0037";
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        //HddProcessID.Value = "P000000623";

        InitControls();
    }

    private void InitControls()
    {
        if (!this.hddProcessID.Value.IsNullOrEmptyEx())
        {
            try
            {
                using (BmskPriceMgr mgr = new BmskPriceMgr())
                {
                    Tuple<DTO_DOC_BMSK_PRICE, List<DTO_DOC_BMSK_PRICE_LIST>> documents = mgr.SelectBmskPrice(this.hddProcessID.Value);

                    if (documents.Item1.BU.Equals(this.radBtnPCS.Value)) this.radBtnPCS.Checked = true;
                    else if (documents.Item1.BU.Equals(this.radBtnPUR.Value)) this.radBtnPUR.Checked = true;
                    this.radtxtCustomer.Text = documents.Item1.CUSTOMER_NAME;
                    webMaster.DocumentNo = documents.Item1.DOC_NUM;
                    List<DTO_DOC_BMSK_PRICE_LIST> list = documents.Item2;
                    ViewState[VIEWSTATE_KEY] = list;

                    this.radGrdProduct.DataSource = list;
                    this.radGrdProduct.DataBind();


                }
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.ToString();
            }
        }
        else
        {
            ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_BMSK_PRICE_LIST>();
            this.radGrdProduct.DataSource = (List<DTO_DOC_BMSK_PRICE_LIST>)ViewState[VIEWSTATE_KEY];
            this.radGrdProduct.DataBind();
        }
    }

    #region [ Page Load ]
    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
        if (!this.hddGridItems.Value.IsNullOrEmptyEx()) UpdateGridData();
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radGrdProduct, radGrdProduct, radDocLoading);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdProduct, radDocLoading);        
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_BmskPrice_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";

    }

    void Approval_Document_BmskPrice_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            this.informationMessage = "";
            UpdateGridData();
        }
        
    }

    #endregion

    #region [ Grid Event ]

    private void UpdateGridData()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BMSK_PRICE_LIST> items
            = (List<DTO_DOC_BMSK_PRICE_LIST>)serializer.Deserialize<List<DTO_DOC_BMSK_PRICE_LIST>>(this.hddGridItems.Value);

        ViewState[VIEWSTATE_KEY] = items;
        this.radGrdProduct.DataSource = items;
        this.radGrdProduct.DataBind();

    }

    protected void radGrdProduct_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            UpdateGridData();
            int idx = Convert.ToInt32(e.CommandArgument.ToString());

            using (BmskPriceMgr mgr = new BmskPriceMgr())
            {
                mgr.DeleteBmskPriceList(this.hddProcessID.Value, idx);
            }

            List<DTO_DOC_BMSK_PRICE_LIST> list = (List<DTO_DOC_BMSK_PRICE_LIST>)ViewState[VIEWSTATE_KEY];

            list.RemoveAll(p => p.IDX == idx);


            this.radGrdProduct.DataSource = list;
            this.radGrdProduct.DataBind();
        }

    }
    #endregion

    #region [ 문서상단 버튼 ]

    private string GetBu()
    {
        if (this.radBtnPCS.Checked) return this.radBtnPCS.Value;
        else if (this.radBtnPUR.Checked) return this.radBtnPUR.Value;
        else return string.Empty;
    }

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        UpdateGridData();
        string message = string.Empty;
        if (GetBu().IsNullOrEmptyEx())
            message += "BU";
        if(this.radtxtCustomer.Text.Length <= 0)
            message += (message.IsNullOrEmptyEx() ? "" : ",") + "Customer";
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + message;
            return false;
        }
        else return true;
    }

    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_BMSK_PRICE doc = new DTO_DOC_BMSK_PRICE();
        doc.PROCESS_ID = this.hddProcessID.Value;
        doc.SUBJECT = GetBu() + "/"+ this.radtxtCustomer.Text;
        webMaster.Subject = doc.SUBJECT;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.BU = GetBu();
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;
        doc.CUSTOMER_CODE = this.hddCustomerCode.Value;
        doc.CUSTOMER_NAME = this.radtxtCustomer.Text;

        List<DTO_DOC_BMSK_PRICE_LIST> list = (List<DTO_DOC_BMSK_PRICE_LIST>)ViewState[VIEWSTATE_KEY];
        foreach (DTO_DOC_BMSK_PRICE_LIST item in list)
        {
            item.PROCESS_ID = this.hddProcessID.Value;
            item.CREATOR_ID = Sessions.UserID;
        }

        using (BmskPriceMgr mgr = new BmskPriceMgr())
        {
            return mgr.MergeBmskPrice(doc, list);
        }
    }

    protected override void DoSave()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Saved))
            {
                hddProcessID.Value = SaveDocument(ApprovalUtil.ApprovalStatus.Saved);
                webMaster.ProcessID = hddProcessID.Value;
                base.DoSave();
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
                hddProcessID.Value = SaveDocument(hddProcessID.Value.Length > 0 ? ApprovalUtil.ApprovalStatus.Saved : ApprovalUtil.ApprovalStatus.Temp);
                webMaster.ProcessID = hddProcessID.Value;
                base.DoRequest();
            }

        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion

    protected void radGrdProduct_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            string code = item["MATERIAL_NAME"].Text;
            code = code.Insert(code.LastIndexOf("("), "<br/>");
            item["MATERIAL_NAME"].Text = code;
        }
    }
}