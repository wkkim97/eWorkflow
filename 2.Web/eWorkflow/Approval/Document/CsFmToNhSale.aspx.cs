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

public partial class Approval_Document_CsFmToNhSlaes : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_CS_FM_TO_NH_SALE_PRODUCT_KEY = "VIEWSTATE_CS_FM_TO_NH_SALE_PRODUCT_KEY";


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
                this.hddCompanyCode.Value = Sessions.CompanyCode; //회사코드 설정
                ViewState[VIEWSTATE_CS_FM_TO_NH_SALE_PRODUCT_KEY] = new List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>();
                this.radGrdSampleItemList.DataSource = (List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>)ViewState[VIEWSTATE_CS_FM_TO_NH_SALE_PRODUCT_KEY];
                this.radGrdSampleItemList.DataBind();
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
        this.hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        this.hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        this.hddDocumentID.Value = "D0048";
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        //this.hddProcessID.Value = "P000000664";

        InitControls();
    }

    private void InitControls()
    {
        DTO_DOC_CS_FM_TO_NH_SALE doc;

        if (!this.hddProcessID.Value.IsNullOrEmptyEx())
        {
            using (CsFmToNhSaleMgr mgr = new CsFmToNhSaleMgr())
            {
                 doc = mgr.SelectCsFmToNhSale(this.hddProcessID.Value);

                this.radTxtSoldTo.Text = doc.SOLD_TO_CUSTOMER_FM_NAME;
                this.hddSOLD_TO_CUSTOMER_FM.Value = doc.SOLD_TO_CUSTOMER_FM;
                //this.radTxtShipTo.Text = doc.SHIP_TO_CUSTOMER_NH_NAME;
                //this.hddSHIP_TO_CUSTOMER_NH.Value = doc.SHIP_TO_CUSTOMER_NH;

                //Product
                List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT> products = mgr.SelectCsFmToNhSaleProduct(this.hddProcessID.Value);
                ViewState[VIEWSTATE_CS_FM_TO_NH_SALE_PRODUCT_KEY] = products;

                this.radGrdSampleItemList.DataSource = products;
                this.radGrdSampleItemList.DataBind();

                webMaster.DocumentNo = doc.DOC_NUM;
            }
        }
    }


    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 

        if (this.hddGridItems.Value != "")
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT> products = (List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>)serializer.Deserialize<List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>>(this.hddGridItems.Value);

            ViewState[VIEWSTATE_CS_FM_TO_NH_SALE_PRODUCT_KEY] = products;
            this.radGrdSampleItemList.DataSource = products;
            this.radGrdSampleItemList.DataBind();
        }

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdSampleItemList);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_SampleRequest_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";

    }

    void Approval_Document_SampleRequest_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            UpdateGridData();
        }
        else if (e.Argument.StartsWith("Ship_to_nh"))
        {
            string[] values = e.Argument.Split(new char[] { ':' });
            if (values.Length > 3)
            {
                string idx = values[1];
                string code = values[2];
                string name = values[3];

                UpdateGridData(Convert.ToInt32(idx), code, name);
            }
        }
    }

    private void UpdateGridData()
    {
        UpdateGridData(999, string.Empty, string.Empty);
    }

    private void UpdateGridData(int idx, string code, string name)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT> products = (List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>)serializer.Deserialize<List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>>(this.hddGridItems.Value);

        if (idx < 999)
        {
            var ship_to_nh = (from product in products
                              where product.IDX == idx
                              select product).FirstOrDefault();
            if (ship_to_nh != null)
            {
                ship_to_nh.SHIP_TO_CUSTOMER_NH = code;
                ship_to_nh.SHIP_TO_CUSTOMER_NH_NAME = name;
            }
        }


        ViewState[VIEWSTATE_CS_FM_TO_NH_SALE_PRODUCT_KEY] = products;
        this.radGrdSampleItemList.DataSource = products;
        this.radGrdSampleItemList.DataBind();

    }

    #region [ 문서상단 버튼 ]

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {

        string message = string.Empty;

        if (this.GetSelectedBU().IsNullOrEmptyEx())
            message += message.IsNullOrEmptyEx() ? "Business Unit" : ", Business Unit";

        if (this.radTxtSoldTo.Text.IsNullOrEmptyEx())
            message += message.IsNullOrEmptyEx() ? "Soldto Customer (FM)" : ", Soldto Customer (FM)";

        //if (this.radTxtShipTo.Text.IsNullOrEmptyEx())
        //    message += message.IsNullOrEmptyEx() ? "Shipto Customer (NH)" : ", Shipto Customer (NH)";

        //Prodcut
        if (this.radGrdSampleItemList.MasterTableView.Items.Count < 1)
            message += message.IsNullOrEmptyEx() ? "Product" : ", Product";

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + message;
            return false;
        }
        else return true;
    }

    private string GetSelectedBU()
    {
        string bu = string.Empty;
        foreach (Control control in divBU.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    bu = (control as RadButton).Value;
                    break;
                }
            }
        }
        return bu;
    }

    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_CS_FM_TO_NH_SALE doc = new DTO_DOC_CS_FM_TO_NH_SALE();
        doc.PROCESS_ID = this.hddProcessID.Value;
        doc.SUBJECT = this.radTxtSoldTo.Text;  //+ " / "+this.radTxtShipTo.Text; ;
        webMaster.Subject = doc.SUBJECT;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.BU = this.radBtnCP.Value;
        doc.SOLD_TO_CUSTOMER_FM  = this.hddSOLD_TO_CUSTOMER_FM.Value;
        doc.SOLD_TO_CUSTOMER_FM_NAME = this.radTxtSoldTo.Text;
        //doc.SHIP_TO_CUSTOMER_NH = this.hddSHIP_TO_CUSTOMER_NH.Value;
        //doc.SHIP_TO_CUSTOMER_NH_NAME = this.radTxtShipTo.Text;
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;


        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT> products = (List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>)serializer.Deserialize<List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>>(this.hddGridItems.Value);
        foreach (DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT product in products)
        {

            product.PROCESS_ID = this.hddProcessID.Value;
            product.CREATOR_ID = Sessions.UserID;
        }

        using (CsFmToNhSaleMgr mgr = new CsFmToNhSaleMgr())
        {
            return mgr.MergeCsFmToNhSale(doc, products);
        }

      }


    protected void radGrdSampleItemList_PreRender(object sender, EventArgs e)
    {
        if (radGrdSampleItemList.FindControl(radGrdSampleItemList.MasterTableView.ClientID + "_LOCATION_NAME") != null)
        {
            RadDropDownList DropLocation = radGrdSampleItemList.FindControl(radGrdSampleItemList.MasterTableView.ClientID + "_LOCATION_NAME").FindControl("radDropLocation") as RadDropDownList;
            DropLocation.DataTextField = "CODE_NAME";
            DropLocation.DataValueField = "SUB_CODE";
            DropLocation.DataBind();
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

    protected void radGrdSampleItemList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Remove"))
            {
                int index = Convert.ToInt32(e.CommandArgument);

                using (CsFmToNhSaleMgr Mgr = new  CsFmToNhSaleMgr())
                {
                    Mgr.DeleteCsFmToNhSlaeProductsByIndex(this.hddProcessID.Value, index);
                }
                List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT> list = (List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>)ViewState[VIEWSTATE_CS_FM_TO_NH_SALE_PRODUCT_KEY];
                list.RemoveAll(p => p.IDX == index);

                this.radGrdSampleItemList.DataSource = list;
                this.radGrdSampleItemList.DataBind();
            }
            if (e.CommandName.Equals("RemoveALL"))
            {

                ViewState[VIEWSTATE_CS_FM_TO_NH_SALE_PRODUCT_KEY] = new List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>();
                this.radGrdSampleItemList.DataSource = (List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>)ViewState[VIEWSTATE_CS_FM_TO_NH_SALE_PRODUCT_KEY];
                this.radGrdSampleItemList.Rebind();

            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    private void GridReset()
    {
        ViewState[VIEWSTATE_CS_FM_TO_NH_SALE_PRODUCT_KEY] = new List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>();
        this.radGrdSampleItemList.DataSource = (List<DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>)ViewState[VIEWSTATE_CS_FM_TO_NH_SALE_PRODUCT_KEY];
        this.radGrdSampleItemList.Rebind();


    }

    protected void radGrdSampleItemList_ItemBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;

        }
    }

    #endregion

}