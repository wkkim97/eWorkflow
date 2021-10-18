using Bayer.eWF.BSL.Approval.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using System.Web.Script.Serialization;
using Bayer.eWF.BSL.Approval.Mgr;
using Telerik.Web.UI;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dto;

public partial class Approval_Document_ComplianceReviewForPromotionlMaterial : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_KEY = "VIEWSTATE_KEY";

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

                List<DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL> initItem = new List<DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL>();

                ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL>();
                this.radGrdPromtionalMaterial.DataSource = (List<DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL>)ViewState[VIEWSTATE_KEY];
                this.radGrdPromtionalMaterial.DataBind();

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
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0041";
        //hddProcessID.Value = "P000000356";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }

    private void InitControls()
    {
        //this.radNumAMT.ReadOnly = true;

        if (this.hddProcessID.Value.IsNotNullOrEmptyEx())
        {
            using (ComplianceReviewForPromotionalMaterialMgr mgr = new ComplianceReviewForPromotionalMaterialMgr())
            {


                Tuple<DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL, List<DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL>> documents = mgr.SelectCompliacneReviewForPromotionalMaterial(this.hddProcessID.Value);

                DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL doc = documents.Item1;
                if (doc == null) return;
                this.hddProcessStatus.Value = doc.PROCESS_STATUS;

                //Sales Group
                foreach (Control control in this.divBU.Controls)
                {
                    if (control is RadButton)
                    {
                        if ((control as RadButton).Value.Equals(doc.BU))
                        {
                            (control as RadButton).Checked = true; break;
                        }
                    }
                }
                //this.radTxtPromotionalMaterial.Text = doc.PROMOTIONAL_MATERIAL;
                //this.radNumUnitPrice.Value = (double?)doc.UNIT_PRICE;
                //this.radNumQTY.Value = (int?)doc.QTY;
                //this.radNumAMT.Value = (double?)doc.AMT;


                //MATERIAL_TYPE
                foreach (Control control in this.divMaterialType.Controls)
                {
                    if (control is RadButton)
                    {
                        if ((control as RadButton).Value.Equals(doc.MATERIAL_TYPE))
                        {
                            (control as RadButton).Checked = true; break;
                        }
                    }
                }

                //TARGET_AUDIENCE
                foreach (Control control in this.divTargetAudience.Controls)
                {
                    if (control is RadButton)
                    {
                        if ((control as RadButton).Value.Equals(doc.TARGET_AUDIENCE))
                        {
                            (control as RadButton).Checked = true; break;
                        }
                    }
                }

                this.radTxtPurpose.Text = doc.PURPOSE;

                webMaster.DocumentNo = doc.DOC_NUM;

                //Product
                List<DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT> products = mgr.SelectCompliacneReviewForPromotionalMaterialProduct(this.hddProcessID.Value);
                foreach (DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT product in products)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Value = product.PRODUCT_CODE;
                    entry.Text = product.PRODUCT_NAME;
                    this.radAcomProduct.Entries.Add(entry);
                }

                this.radGrdPromtionalMaterial.DataSource = documents.Item2;
                this.radGrdPromtionalMaterial.DataBind();

                if (!(doc.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || doc.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString())))
                {
                    if (products.Count < 1)
                    {
                        this.radAcomProduct.Visible = false;
                        this.lblNotProduct.Visible = true;
                    }

                }
            }
        }
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
        if (!this.hddGridItems.Value.IsNullOrEmptyEx()) UpdateGridData();
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdPromtionalMaterial);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radGrdPromtionalMaterial, this.radGrdPromtionalMaterial);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_ComplianceReviewForPromotionlMaterial_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";

    }

    void Approval_Document_ComplianceReviewForPromotionlMaterial_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.Equals("Rebind"))
        {
            UpdateGridData();
        }

    }

    #region [ Add Row Event ]

    private void UpdateGridData()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL> items = (List<DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL>)serializer.Deserialize<List<DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL>>(this.hddGridItems.Value);

        ViewState[VIEWSTATE_KEY] = items;
        this.radGrdPromtionalMaterial.DataSource = items;
        this.radGrdPromtionalMaterial.DataBind();

    }


    #endregion

    #region [ Grid Event ]
    protected void radGrdPromtionalMaterial_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            UpdateGridData();
            int index = Convert.ToInt32(e.CommandArgument);

            using (ComplianceReviewForPromotionalMaterialMgr mgr = new ComplianceReviewForPromotionalMaterialMgr())
            {
                mgr.DeleteBHCPromotionalMaterial(this.hddProcessID.Value, index);
            }

            List<DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL> list = (List<DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL>)ViewState[VIEWSTATE_KEY];

            list.RemoveAll(p => p.IDX == index);

            this.radGrdPromtionalMaterial.DataSource = list;
            this.radGrdPromtionalMaterial.DataBind();
        }
    }
    #endregion


    #region [ 문서상단 버튼 ]


    /// <summary>
    /// Sales Group반환
    /// </summary>
    /// <returns></returns>
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

    private string GetSelectedMaterialType()
    {
        string MaterialType = string.Empty;
        foreach (Control control in divMaterialType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    MaterialType = (control as RadButton).Value;
                    break;
                }
            }
        }
        return MaterialType;
    }

    private string GetSelectedTargetAudience()
    {
        string TargetAudience = string.Empty;
        foreach (Control control in divTargetAudience.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    TargetAudience = (control as RadButton).Value;
                    break;
                }
            }
        }
        return TargetAudience;
    }

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            //BU
            if (GetSelectedBU().IsNullOrEmptyEx())
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "BU";
            }

            //if (radTxtPromotionalMaterial.Text.IsNullOrEmptyEx())
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Promotional Material";

            //if (!radNumUnitPrice.Value.HasValue || radNumUnitPrice.Value == 0)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Unit Price";

            //if (!radNumQTY.Value.HasValue || radNumQTY.Value == 0)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Quantity";

            //Material Type
            if (GetSelectedMaterialType().IsNullOrEmptyEx())
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Material Type";
            }

            if (GetSelectedTargetAudience().IsNullOrEmptyEx())
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Target Audience";
            }

            //Purpose
            if (radTxtPurpose.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";

            //Promotional Material
            if (this.radGrdPromtionalMaterial.MasterTableView.Items.Count < 1)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Promotional Material";

        }
        else if (status == ApprovalUtil.ApprovalStatus.Saved)
        {
        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + message;
            return false;
        }
        else return true;
    }

    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL doc = new DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL();
        doc.PROCESS_ID = this.hddProcessID.Value;
        string subject = string.Empty;
        string title = string.Empty;
        string subpurpose = string.Empty;
        string salesGroup = GetSelectedBU();
        //string PromotionlMaterial = this.radTxtPromotionalMaterial.Text;
        string MaterialType = GetSelectedMaterialType();
        string TargetAudience = GetSelectedTargetAudience();

        doc.SUBJECT = salesGroup + "/ " + MaterialType + "/ " + TargetAudience ;
        webMaster.Subject = doc.SUBJECT;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        string purpose = string.Empty;

        doc.BU = salesGroup;
       // doc.PROMOTIONAL_MATERIAL = PromotionlMaterial;
       // doc.UNIT_PRICE = (decimal?)this.radNumUnitPrice.Value;
       // doc.QTY = (int?)this.radNumQTY.Value;
        doc.MATERIAL_TYPE = MaterialType;
        doc.TARGET_AUDIENCE = TargetAudience;
        doc.PURPOSE = this.radTxtPurpose.Text;
        

        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;


        List<DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT> products = new List<DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT>();

        foreach (AutoCompleteBoxEntry entry in this.radAcomProduct.Entries)
        {
            DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT product = new DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT();
            product.PROCESS_ID = this.hddProcessID.Value;
            product.PRODUCT_CODE = entry.Value;
            product.PRODUCT_NAME = entry.Text;
            product.CREATOR_ID = Sessions.UserID;
            products.Add(product);
        }

        List<DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL> details = (List<DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL>)ViewState[VIEWSTATE_KEY];

        decimal? total = 0;
        foreach (DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL detail in details)
        {
            total += (detail.AMOUNT.HasValue ? detail.AMOUNT : 0);
            detail.PROCESS_ID = this.hddProcessID.Value;
            detail.CREATOR_ID = Sessions.UserID;
        }
        doc.TOTAL_AMOUNT = total;

        using (ComplianceReviewForPromotionalMaterialMgr mgr = new ComplianceReviewForPromotionalMaterialMgr())
        {
            return mgr.MergeComplianceReviewForPromotionalMaterial(doc, products, details);
        }

  

    }
    protected override void DoSave()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Saved))
            {
                hddProcessID.Value = SaveDocument(ApprovalUtil.ApprovalStatus.Saved);
                webMaster.ProcessID = this.hddProcessID.Value;
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
}