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

public partial class Approval_Document_BcsCompensation : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_BCS_COMPENSATION_PRODUCT_KEY = "VIEWSTATE_BCS_COMPENSATION_PRODUCT_KEY";
    private static List<DTO_CODE_SUB> location = null;

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
                ViewState[VIEWSTATE_BCS_COMPENSATION_PRODUCT_KEY] = new List<DTO_DOC_BCS_COMPENSATION_PRODUCT>();
                this.radGrdSampleItemList.DataSource = (List<DTO_DOC_BCS_COMPENSATION_PRODUCT>)ViewState[VIEWSTATE_BCS_COMPENSATION_PRODUCT_KEY];
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
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0047";
        //hddProcessID.Value = "P000000910";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }

    private void SetDropdownControl()
    {
        using (Bayer.eWF.BSL.Common.Mgr.CodeMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CodeMgr())
        {
            location = mgr.SelectCodeSubList("S018");
        }
    }

    private void InitControls()
    {
        DTO_DOC_BCS_COMPENSATION doc;         
        if (this.hddProcessID.Value.IsNotNullOrEmptyEx())
        {
            using (BCSCompensationMgr mgr = new BCSCompensationMgr())
            {
                doc = mgr.SelectBCSCompensation(this.hddProcessID.Value);
                string over = string.Empty;

                this.radTxtTitle.Text = doc.TITLE;

                if (doc.BU == this.radRdoBU_CP.Value)
                    this.radRdoBU_CP.Checked = true;
                else if (doc.BU == this.radRdoBU_ES.Value)
                    this.radRdoBU_ES.Checked = true;
                else if (doc.BU == this.radRdoBU_IS.Value)
                    this.radRdoBU_IS.Checked = true;

                if (doc.COMPLAINT_TYPE == this.radRdoCT_EP.Value)
                    this.radRdoCT_EP.Checked = true;
                else if (doc.COMPLAINT_TYPE == this.radRdoCT_QL.Value)
                    this.radRdoCT_QL.Checked = true;

                this.radMaskTxtComplaintReportNo.Text = doc.COMPLAINT_REPORT_NO;

                this.radTxtReason.Text  = doc.REASON;

                this.radTxtCompensationName.Text = doc.RECEVING_COMPENSATION_NAME;
                this.radTxtCompensationAddresss.Text = doc.RECEVING_COMPENSATION_ADDRESS;

                foreach (string CompensationType in doc.COMPENSATION_TYPE.Split(new char[] { ';' }))
                {
                    foreach (Control control in this.divCompensationType.Controls)
                    {
                        if (control is RadButton)
                        {
                            if ((control as RadButton).Value.Equals(CompensationType))
                            {
                                (control as RadButton).Checked = true; break;
                            }
                        }
                    }
                }



                foreach (string AttachCheck in doc.ATTACH_CHECK.Split(new char[] { ';' }))
                {
                    foreach (Control control in this.divCheck.Controls)
                    {
                        if (control is RadButton)
                        {
                            if ((control as RadButton).Value.Equals(AttachCheck))
                            {
                                (control as RadButton).Checked = true; break;
                            }
                        }
                    }
                }

                this.radTxtRemark.Text = doc.REMARK;
                //this.radNumAmount.Text = doc.CASH_AMOUNT.ToString();
                this.radNumAmount.Text = String.Format("{0:#,##0}", Convert.ToInt32(doc.CASH_AMOUNT));
                this.radNumProductAmount.Value = (double?)doc.PRODUCT_AMOUNT;
                this.radNumTotalAmount.Value = (double?)doc.TOTAL_AMOUNT;
                webMaster.DocumentNo = doc.DOC_NUM;


                //Product
                List<DTO_DOC_BCS_COMPENSATION_PRODUCT> products = mgr.SelectBCSCompensationProduct(this.hddProcessID.Value);
                ViewState[VIEWSTATE_BCS_COMPENSATION_PRODUCT_KEY] = products;

                this.radGrdSampleItemList.DataSource = products;
                this.radGrdSampleItemList.DataBind();


                
                if (!(doc.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || doc.PROCESS_STATUS.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString())))
                {
                    //if (products.Count < 1)
                    //{
                    //    this.radAcomProduct.Visible = false;
                    //    this.lblNotProduct.Visible = true;
                    //}

                }
                if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + GetSelectedCompensationType() + "','" + GetSelectedComplaintType() + "','" + GetSelectedBU() + "');", true);
            }
        }
        SetDropdownControl();           
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
        
        if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + GetSelectedCompensationType() + "','" + GetSelectedComplaintType() + "','" + GetSelectedBU() + "');", true);

        if (this.hddGridItems.Value != "")
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_DOC_BCS_COMPENSATION_PRODUCT> products = (List<DTO_DOC_BCS_COMPENSATION_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BCS_COMPENSATION_PRODUCT>>(this.hddGridItems.Value);

            ViewState[VIEWSTATE_BCS_COMPENSATION_PRODUCT_KEY] = products;
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
        
        else if (e.Argument.StartsWith("Costcenter"))
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
        List<DTO_DOC_BCS_COMPENSATION_PRODUCT> products = (List<DTO_DOC_BCS_COMPENSATION_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BCS_COMPENSATION_PRODUCT>>(this.hddGridItems.Value);

        if (idx < 999)
        {
            var costcenter = (from product in products
                              where product.IDX == idx
                              select product).FirstOrDefault();
            if (costcenter != null)
            {
                costcenter.COST_CODE = code;
                costcenter.COST_NAME = name;
            }
        }

        ViewState[VIEWSTATE_BCS_COMPENSATION_PRODUCT_KEY] = products;
        this.radGrdSampleItemList.DataSource = products;
        this.radGrdSampleItemList.DataBind();

    }
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (this.radTxtTitle.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Title" : ", Title";
            else if (this.radTxtTitle.Text.Length < 10 || this.radTxtTitle.Text.Length > 50)
                message = "Title:10 글자 이상 50 글자 미만으로 입력";

            if (this.GetSelectedBU().IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "Business Unit" : ", Business Unit";

            if (this.GetSelectedComplaintType().IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "Complaint Type" : ", Complaint Type";

            if (this.GetSelectedCompensationType().IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "Compensation Type" : ", Compensation Type";

            var strBU = this.GetSelectedBU();

            if (strBU != "IS")
            {
                if (this.radChkProduct.Checked)
                {
                    //Prodcut
                    if (this.radGrdSampleItemList.MasterTableView.Items.Count < 1)
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Product";
                }            
            }


            if (this.radMaskTxtComplaintReportNo.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "No of Customer Complaint Report" : ", No of Customer Complaint Report";



            if (this.radChkCash.Checked)
            {
                // Cash Amount
                if (this.radNumAmount.Text.Length <= 0)
                    message += message.IsNullOrEmptyEx() ? "Cash Amount" : ", Cash Amount";
            }
            else
            {
                // Cash Amount
                if (this.radNumAmount.Text.IsNullOrEmptyEx()==false && this.radNumAmount.Text !=  "0")
                    message += message.IsNullOrEmptyEx() ? "cannot input Cash Amount" : ", cannot input Cash Amount";

            }

            var TotalAmount = this.radNumTotalAmount.Value;// +this.radNumProductAmount.Value;

            if (TotalAmount < 1000000)
            {
                //message += message.IsNullOrEmptyEx() ? "Cash 100만원이하" : ", Cash 100만원이하";
                if (radChkCheck1.Checked == false )
                    message += message.IsNullOrEmptyEx() ? "1.증거자료" : ", 1.증거자료"; 
                if (radChkCheck2.Checked == false)
                    message += message.IsNullOrEmptyEx() ? "2.고객불만보고서" : ", 2.고객불만보고서"; 
                if (radChkCheck3.Checked == false )
                    message += message.IsNullOrEmptyEx() ? "3.실무자협의회 회의록(영업지점장 소집)" : ", 3.실무자협의회 회의록(영업지점장 소집)"; 
                if (radChkCheck7.Checked == false)
                    message += message.IsNullOrEmptyEx() ? "7.합의서" : ", 7.합의서";
            }
            else if (TotalAmount >= 1000000 && TotalAmount < 5000000)
            {
                //message += message.IsNullOrEmptyEx() ? "Cash 100만원 초가 500만원 이하" : ", Cash 100만원 초가 500만원 이하";
                if (radChkCheck1.Checked == false)
                    message += message.IsNullOrEmptyEx() ? "1.증거자료" : ", 1.증거자료";
                if (radChkCheck2.Checked == false)
                    message += message.IsNullOrEmptyEx() ? "2.고객불만보고서" : ", 2.고객불만보고서";
                if (radChkCheck4.Checked == false)
                    message += message.IsNullOrEmptyEx() ? "4.고객불만 조사보고서" : ", 4.고객불만 조사보고서";
                if (radChkCheck5.Checked == false)
                    message += message.IsNullOrEmptyEx() ? "5.실무자협의회 회의록(코디네이터 소집)" : ", 5.실무자협의회 회의록(코디네이터 소집)";
                if (radChkCheck7.Checked == false)
                    message += message.IsNullOrEmptyEx() ? "7.합의서" : ", 7.합의서";
            }
            else if (TotalAmount >= 5000000)
            {
                //message += message.IsNullOrEmptyEx() ? "Cash 500만원 초과" : ", Cash 500만원 초과";
                if (radChkCheck1.Checked == false)
                    message += message.IsNullOrEmptyEx() ? "1.증거자료" : ", 1.증거자료";
                if (radChkCheck2.Checked == false)
                    message += message.IsNullOrEmptyEx() ? "2.고객불만보고서" : ", 2.고객불만보고서";
                if (radChkCheck4.Checked == false)
                    message += message.IsNullOrEmptyEx() ? "4.고객불만 조사보고서" : ", 4.고객불만 조사보고서";
                if (radChkCheck6.Checked == false)
                    message += message.IsNullOrEmptyEx() ? "6.PQSC 회의록" : ", 6.PQSC 회의록";
                if (radChkCheck7.Checked == false)
                    message += message.IsNullOrEmptyEx() ? "7.합의서" : ", 7.합의서";
            }

        }


        if (message.Length > 0)
        {
            //if (this.radRdoTitle2.Checked)
            //{
            //    this.divOverLimit.Attributes.Add("style", "display: inline; visibility: visible");
            //    this.divLink.Attributes.Add("style", "display: block; visibility: visible");
            //}
            //else
            //{
            //    this.divOverLimit.Attributes.Add("style", "display: inline; visibility: none");
            //    this.divLink.Attributes.Add("style", "display: block; visibility: none");
            //}

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
   

    private string GetSelectedComplaintType()
    {
        string ct = string.Empty;
        foreach (Control control in divComplaintType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    ct = (control as RadButton).Value;
                    break;
                }
            }
        }
        return ct;
    }
    private string GetSelectedCompensationType()
    {
        string ct = string.Empty;
        foreach (Control control in divCompensationType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    ct = (control as RadButton).Value;
                    break;
                }
            }
        }
        return ct;
    }


    

    protected void radGrdSampleItemList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Remove"))
            {
                int index = Convert.ToInt32(e.CommandArgument);

                using (BCSCompensationMgr Mgr = new BCSCompensationMgr())
                {
                    Mgr.DeleteBCSCompensationProductsByIndex(this.hddProcessID.Value, index);
                }
                List<DTO_DOC_BCS_COMPENSATION_PRODUCT> list = (List<DTO_DOC_BCS_COMPENSATION_PRODUCT>)ViewState[VIEWSTATE_BCS_COMPENSATION_PRODUCT_KEY];
                list.RemoveAll(p => p.IDX == index);

                this.radGrdSampleItemList.DataSource = list;
                this.radGrdSampleItemList.DataBind();
            }
            if (e.CommandName.Equals("RemoveALL"))
            {
                
                ViewState[VIEWSTATE_BCS_COMPENSATION_PRODUCT_KEY] = new List<DTO_DOC_BCS_COMPENSATION_PRODUCT>();
                this.radGrdSampleItemList.DataSource = (List<DTO_DOC_BCS_COMPENSATION_PRODUCT>)ViewState[VIEWSTATE_BCS_COMPENSATION_PRODUCT_KEY];
                this.radGrdSampleItemList.Rebind();
                
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    protected void radChkProduct_ItemSelected(object sender, EventArgs e)
    {
        
        GridReset();
        if (this.GetSelectedBU() == "IS")
        {
            this.radChkProduct.Enabled = false;
            this.radChkProduct.Checked = false;
            

        }
        //if (sender. == "Industrial Sales")
       // {

       // }
    }

    private void GridReset()
    {
        ViewState[VIEWSTATE_BCS_COMPENSATION_PRODUCT_KEY] = new List<DTO_DOC_BCS_COMPENSATION_PRODUCT>();
        this.radGrdSampleItemList.DataSource = (List<DTO_DOC_BCS_COMPENSATION_PRODUCT>)ViewState[VIEWSTATE_BCS_COMPENSATION_PRODUCT_KEY];
        this.radGrdSampleItemList.Rebind();
                

    }



    int StringToInt(string text)
    {
        int value = 0;
        text = text.Replace(",", "");
        if (text.Trim().Length > 0) value = Convert.ToInt32(text);
        return value;
    }
    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_BCS_COMPENSATION doc = new DTO_DOC_BCS_COMPENSATION();
        doc.PROCESS_ID = this.hddProcessID.Value;
        doc.SUBJECT = this.radTxtTitle.Text + " / " + GetSelectedBU() + " / " + GetSelectedComplaintType();
        webMaster.Subject = doc.SUBJECT;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;

        doc.TITLE = this.radTxtTitle.Text;
        doc.BU = GetSelectedBU();
        doc.COMPLAINT_TYPE = GetSelectedComplaintType();

        doc.COMPLAINT_REPORT_NO = this.radMaskTxtComplaintReportNo.Text;

        doc.REASON = this.radTxtReason.Text;
        doc.RECEVING_COMPENSATION_NAME = this.radTxtCompensationName.Text;
        doc.RECEVING_COMPENSATION_ADDRESS = this.radTxtCompensationAddresss.Text;

        doc.COMPENSATION_TYPE = GetSelectedCompensationType();

        string CompensationType = string.Empty;
        foreach (Control control in this.divCompensationType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                    CompensationType += (control as RadButton).Value + ";";
            }
        }
        doc.COMPENSATION_TYPE = CompensationType;
        //doc.CASH_AMOUNT = StringToInt(this.radNumAmount.Text);
        if (this.radNumAmount.Text.IsNullOrEmptyEx()==false)
        {
            doc.CASH_AMOUNT = Convert.ToInt32(this.radNumAmount.Text.Replace(",",""));
        }
        else
        {
            doc.CASH_AMOUNT = 0;
        }


            doc.PRODUCT_AMOUNT = (decimal?)this.radNumProductAmount.Value;

        string AttachCheck = string.Empty;
        foreach (Control control in this.divCheck.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                    AttachCheck += (control as RadButton).Value + ";";
            }
        }

        doc.ATTACH_CHECK = AttachCheck;
        doc.REMARK = this.radTxtRemark.Text;


        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_BCS_COMPENSATION_PRODUCT> products = (List<DTO_DOC_BCS_COMPENSATION_PRODUCT>)serializer.Deserialize<List<DTO_DOC_BCS_COMPENSATION_PRODUCT>>(this.hddGridItems.Value);
        foreach (DTO_DOC_BCS_COMPENSATION_PRODUCT product in products)
        {
            DTO_CODE_SUB code = location.Find(p => p.CODE_NAME == product.LOCATION_NAME.Trim());
            if (code != null)
            {
                product.LOCATION_CODE = code.SUB_CODE;
            }

            product.PROCESS_ID = this.hddProcessID.Value;
            product.CREATOR_ID = Sessions.UserID;
        }

        using (BCSCompensationMgr mgr = new BCSCompensationMgr())
        {
            return mgr.MergeBCSCompensation(doc, products);
        }

    }

    protected void radGrdSampleItemList_PreRender(object sender, EventArgs e)
    {
        if (radGrdSampleItemList.FindControl(radGrdSampleItemList.MasterTableView.ClientID + "_LOCATION_NAME") != null)
        {
            RadDropDownList DropLocation = radGrdSampleItemList.FindControl(radGrdSampleItemList.MasterTableView.ClientID + "_LOCATION_NAME").FindControl("radDropLocation") as RadDropDownList;
            DropLocation.DataTextField = "CODE_NAME";
            DropLocation.DataValueField = "SUB_CODE";
            DropLocation.DataSource = location;
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
    protected void radGrdSampleItemList_ItemBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;

        }
    }
}

