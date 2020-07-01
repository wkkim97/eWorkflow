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

public partial class Approval_Document_Donation : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{

    private const string VIEWSTATE_KEY = "VIEWSTATE_KEY_DONATION";

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
                ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_DONATION_PRODUCT>();
                this.radGrdProduct.DataSource = (List<DTO_DOC_DONATION_PRODUCT>)ViewState[VIEWSTATE_KEY];
                this.radGrdProduct.DataBind();
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
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0023";
        //hddProcessID.Value = "P000000478";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {

        DTO_DOC_DONATION doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            string processId = this.hddProcessID.Value;
            using (Bayer.eWF.BSL.Approval.Mgr.DonationMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.DonationMgr())
            {
                doc = mgr.SelectDonation(hddProcessID.Value);
                this.hddProcessStatus.Value = doc.PROCESS_STATUS;

                if (doc != null)
                {
                    string[] typeItem = new string[]{};
                    string[] purposeItem = new string[] { };
                    string[] categoryItem = new string[] { };

                    if(doc.TYPE.IsNotNullOrEmptyEx())
                    {
                        typeItem = doc.TYPE.Split(new string[] { "/" }, StringSplitOptions.None);
                    }
                    if(doc.PURPOSE.IsNotNullOrEmptyEx())
                    {
                        purposeItem = doc.PURPOSE.Split(new string[] { "/" }, StringSplitOptions.None);
                    }
                    if(doc.CATEGORY.IsNotNullOrEmptyEx())
                    {
                        categoryItem = doc.CATEGORY.Split(new string[] { "/" }, StringSplitOptions.None);
                    }
                    
                    for (int i = 0; i < typeItem.Length; i++)
                    {
                        if (typeItem[i].ToString().Equals(this.radChkType1.Value))
                            this.radChkType1.Checked = true;
                        if (typeItem[i].ToString().Equals(this.radChkType2.Value))
                        {
                            this.radChkType2.Checked = true;

                            if (doc.PRODUCT_LOCATION == "서울")
                                this.radRdoLocation1.Checked = true;
                            else if (doc.PRODUCT_LOCATION == "대전")
                                this.radRdoLocation2.Checked = true;
                            else if (doc.PRODUCT_LOCATION == "대구")
                                this.radRdoLocation3.Checked = true;
                            else if (doc.PRODUCT_LOCATION == "부산")
                                this.radRdoLocation4.Checked = true;
                            else if (doc.PRODUCT_LOCATION == "광주")
                                this.radRdoLocation5.Checked = true;
                            else if (doc.PRODUCT_LOCATION == "안성공장")
                                this.radRdoLocation6.Checked = true;
                            else if (doc.PRODUCT_LOCATION == "기타")
                                this.radRdoLocation7.Checked = true;

                            List<DTO_DOC_DONATION_PRODUCT> products = mgr.SelectDonationProduct(processId);
                            ViewState[VIEWSTATE_KEY] = products;

                            this.radGrdProduct.DataSource = products;
                            this.radGrdProduct.DataBind();
                        }
                        if (typeItem[i].ToString().Equals(this.radChkType3.Value))
                            this.radChkType3.Checked = true;

                    }

                    for (int i = 0; i < purposeItem.Length; i++)
                    {
                        if (purposeItem[i].ToString().Equals(this.radChkPurpose1.Value))
                            this.radChkPurpose1.Checked = true;
                        if (purposeItem[i].ToString().Equals(this.radChkPurpose2.Value))
                            this.radChkPurpose2.Checked = true;
                        if (purposeItem[i].ToString().Equals(this.radChkPurpose3.Value))
                            this.radChkPurpose3.Checked = true;
                    }

                    for (int i = 0; i < categoryItem.Length; i++ )
                    {
                        if (categoryItem[i].ToString().Equals(this.radChkHealthCare.Value))
                            this.radChkHealthCare.Checked = true;
                        if (categoryItem[i].ToString().Equals(this.radChkEducational.Value))
                            this.radChkEducational.Checked = true;
                        if (categoryItem[i].ToString().Equals(this.radChkCharity.Value))
                            this.radChkCharity.Checked = true;
                        if (categoryItem[i].ToString().Equals(this.radChkOthers.Value))
                            this.radChkOthers.Checked = true;
                    }

                    this.radTxtValue.Value = (double?)doc.VALUE;
                    this.radTxtExplanation.Text = doc.EXPLANATION;
                    this.radTxtRecipient.Text = doc.RECIPIENT;
                    this.radTxtAddress.Text = doc.ADDRESS;
                    this.radTxtTel.Text = doc.TELEPHONE;
                    this.radTxtEmail.Text = doc.E_MAIL;
                    webMaster.DocumentNo = doc.DOC_NUM;
                }
            }
        }
    }
    #endregion

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
        string product = "N";

        if (this.radChkType2.Checked) product = "Y";
        else product = "N";

        if (!this.hddGridItems.Value.IsNullOrEmptyEx()) UpdateGridData();
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radBtnAdd, radGrdProduct, null);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(radChkPurpose2, radGrdProduct, null);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdProduct);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_Donation_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";
        if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + product + "');", true);
    }

    void Approval_Document_Donation_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("ApplyProduct"))
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
        else if (e.Argument.StartsWith("Rebind"))
        {
            UpdateGridData(999, "", "");
        }
    }
    #endregion

    #region [Add Button Event]

    private void UpdateGridData()
    {
        UpdateGridData(999, string.Empty, string.Empty);
    }


    private void UpdateGridData(int idx, string code, string name)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_DONATION_PRODUCT> items = (List<DTO_DOC_DONATION_PRODUCT>)serializer.Deserialize<List<DTO_DOC_DONATION_PRODUCT>>(this.hddGridItems.Value);


        

        if (idx < 999)
        {
            var itemProduct = (from item in items
                               where item.IDX == idx
                               select item).FirstOrDefault();

            if (itemProduct != null)
            {
                itemProduct.PRODUCT_CODE = code;
                itemProduct.PRODUCT_NAME = name;
            }
        }


        ViewState[VIEWSTATE_KEY] = items;
        this.radGrdProduct.DataSource = items;
        this.radGrdProduct.DataBind();
    }

    #endregion


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

    protected override void DoApproval()
    {
        // TODO :

        base.DoApproval();
    }

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


    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        UpdateGridData();
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (!(radChkType1.Checked || radChkType2.Checked || radChkType3.Checked))
                message += "Type";
            if (radTxtValue.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Value[KRW]" : ",Value[KRW]";
            //if (!(radChkPurpose1.Checked || radChkPurpose2.Checked || radChkPurpose3.Checked))
            //    message += message.IsNullOrEmptyEx() ? "Purpose" : ",Purpose";
            //if (radTxtExplanation.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Explanation" : ",Explanation";
            //if (radTxtTel.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Tel" : ",Tel";
            if(radTxtRecipient.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Recipient" : ",Recipient";
            //if(radTxtAddress.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Address" : ",Address";
            //if (radTxtEmail.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "E-mail" : ",E-mail";
            if (!(radChkHealthCare.Checked || radChkEducational.Checked || radChkCharity.Checked || radChkOthers.Checked))
                message += message.IsNullOrEmptyEx() ? "Category" : ",Category";

            //if (radChkType2.Checked)
            //{
            //    if (!(radRdoLocation1.Checked || radRdoLocation2.Checked || radRdoLocation3.Checked || radRdoLocation4.Checked || radRdoLocation5.Checked || radRdoLocation6.Checked || radRdoLocation7.Checked))
            //        message += message.IsNullOrEmptyEx() ? "Location" : ",Location";

            //    if (this.radGrdProduct.Items.Count < 1)
            //        message += message.IsNullOrEmptyEx() ? "Product, Fair Market Value, Qty" : ",Product, Fair Market Value, Qty";
            //}

        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    }

    private string GetChkType()
    {
        string type = string.Empty;
        if (radChkType1.Checked) type = type + radChkType1.Value + "/";
        if (radChkType2.Checked) type = type + radChkType2.Value + "/";
        if (radChkType3.Checked) type = type + radChkType3.Value + "/";
        return type;
    }

    private string DocumentSave(string processStatus)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_DONATION doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_DONATION();
        doc.PROCESS_STATUS = processStatus;
        doc.PROCESS_ID = hddProcessID.Value;
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = Sessions.UserID;
        doc.SUBJECT = GetChkType();
        webMaster.Subject = doc.SUBJECT;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.CREATE_DATE = DateTime.Now;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CREATOR_ID = Sessions.UserID;
        doc.IS_DISUSED = "N";

        if (radChkType1.Checked)
            doc.TYPE = doc.TYPE + radChkType1.Value + "/";
        if (radChkType2.Checked)
        {
            doc.TYPE = doc.TYPE + radChkType2.Value + "/";

            if (this.radRdoLocation1.Checked)
                doc.PRODUCT_LOCATION = this.radRdoLocation1.Value;
            else if (this.radRdoLocation2.Checked)
                doc.PRODUCT_LOCATION = this.radRdoLocation2.Value;
            else if (this.radRdoLocation3.Checked)
                doc.PRODUCT_LOCATION = this.radRdoLocation3.Value;
            else if (this.radRdoLocation4.Checked)
                doc.PRODUCT_LOCATION = this.radRdoLocation4.Value;
            else if (this.radRdoLocation5.Checked)
                doc.PRODUCT_LOCATION = this.radRdoLocation5.Value;
            else if (this.radRdoLocation6.Checked)
                doc.PRODUCT_LOCATION = this.radRdoLocation6.Value;
            else if (this.radRdoLocation7.Checked)
                doc.PRODUCT_LOCATION = this.radRdoLocation7.Value;


            if (this.radGrdProduct.MasterTableView.Items.Count > 0)
            {
                GridFooterItem itemTotal = (GridFooterItem)this.radGrdProduct.MasterTableView.GetItems(GridItemType.Footer)[0];
                doc.TOTAL_AMOUNT = Convert.ToInt32((itemTotal["AMOUNT"].Text.Replace(",", "")));
            }
            else
                doc.TOTAL_AMOUNT = 0;
        }
        if (radChkType3.Checked)
            doc.TYPE = doc.TYPE + radChkType3.Value + "/";

        doc.VALUE = Convert.ToInt32(radTxtValue.Text);

        if (radChkPurpose1.Checked)
            doc.PURPOSE = doc.PURPOSE + radChkPurpose1.Value + "/";
        if (radChkPurpose2.Checked)
            doc.PURPOSE = doc.PURPOSE + radChkPurpose2.Value + "/";
        if (radChkPurpose3.Checked)
            doc.PURPOSE = doc.PURPOSE + radChkPurpose3.Value + "/";

        doc.EXPLANATION = radTxtExplanation.Text;
        doc.RECIPIENT = radTxtRecipient.Text;
        doc.ADDRESS = radTxtAddress.Text;
        doc.TELEPHONE = radTxtTel.Text;
        doc.E_MAIL = radTxtEmail.Text;

        if (radChkHealthCare.Checked)
            doc.CATEGORY = doc.CATEGORY + radChkHealthCare.Value + "/";
        if (radChkEducational.Checked)
            doc.CATEGORY = doc.CATEGORY + radChkEducational.Value + "/";
        if (radChkCharity.Checked)
            doc.CATEGORY = doc.CATEGORY + radChkCharity.Value + "/";
        if (radChkOthers.Checked)
            doc.CATEGORY = doc.CATEGORY + radChkOthers.Value + "/";

        List<DTO_DOC_DONATION_PRODUCT> products = (List<DTO_DOC_DONATION_PRODUCT>)ViewState[VIEWSTATE_KEY];

        foreach (DTO_DOC_DONATION_PRODUCT product in products)
        {
            product.PROCESS_ID = this.hddProcessID.Value;
            product.CREATOR_ID = Sessions.UserID;
        }

        using (Bayer.eWF.BSL.Approval.Mgr.DonationMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.DonationMgr())
        {
            processID = mgr.MergeDonation(doc, products);
        }
        return processID;
    }

    protected void radGrdProduct_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            int index = Convert.ToInt32(e.CommandArgument);

            using (DonationMgr mgr = new DonationMgr())
            {
                mgr.DeleteDonationProductByIndex(this.hddProcessID.Value, index);
            }
            List<DTO_DOC_DONATION_PRODUCT> list = (List<DTO_DOC_DONATION_PRODUCT>)ViewState[VIEWSTATE_KEY];

            list.RemoveAll(p => p.IDX == index);

            this.radGrdProduct.DataSource = list;
            this.radGrdProduct.DataBind();
        }

    }
}
