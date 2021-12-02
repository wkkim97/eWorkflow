using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;


public partial class Approval_Document_StrategicChangeReport : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private enum StrategicChkItem
    {
        Price,
        Interruption,
        Capacity,
        Product
    }

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

    #region Page_Load
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
    #endregion

    #region InitPageInfo
    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0019";
        //hddProcessID.Value = "P000000304";

        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {
        DTO_DOC_STRATEGIC_CHANGE_REPORT doc;

        if (!hddProcessID.Value.Equals(string.Empty))
        {
            using (StrategicChangeReportMgr mgr = new StrategicChangeReportMgr())
                doc = mgr.SelectStrategicChangeReport(hddProcessID.Value);
            if (doc != null)
            {
                RadtxtProduct.Text = doc.PRODUCT_NAME;

                if (doc.STRATEGIC_CHANGE_CODE == "" || doc.STRATEGIC_CHANGE_CODE ==null)
                {
                    //// Doing nithing!
                }else { 

                    string[] chkItem = doc.STRATEGIC_CHANGE_CODE.Split(new string[] { "/" }, StringSplitOptions.None);
                    for (int i = 0; i < chkItem.Length; i++)
                    {
                        if (chkItem[i].ToString().Equals(StrategicChkItem.Price.ToString()))
                        {
                            productTb.Attributes.CssStyle.Add("display", "inline");
                            RadchkPrice.Checked = true;                        
                            RadtxtQty.Text = Convert.ToString(doc.PRE_SALE_QTY);
                            RadtxtPreSale.Value = (double?)doc.PRE_PRICE;
                            RadtxtChangeSale.Value = (double?)doc.NEW_PRICE;
                            if (doc.IS_TIE_IN_SALE == "Y")
                                RadchkTie.Checked = true;
                        }
                        if (chkItem[i].ToString().Equals(StrategicChkItem.Interruption.ToString()))
                            RadchkInerruption.Checked = true;
                        if (chkItem[i].ToString().Equals(StrategicChkItem.Capacity.ToString()))
                            RadchkCapacity.Checked = true;
                        if (chkItem[i].ToString().Equals(StrategicChkItem.Product.ToString()))
                            RadchkProduct.Checked = true;
                    }
                }
                RadtxtPurpose.Text = doc.PURPOSE_JUSTIFICATION;
                webMaster.DocumentNo = doc.DOC_NUM;
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
        if (RadchkPrice.Checked == true)             
            productTb.Attributes.CssStyle.Add("display", "inline");            
        

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

    #region DoRequest
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
    #endregion

    #region DocumentSave
    private string DocumentSave(string ApprovalStatus)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_STRATEGIC_CHANGE_REPORT doc = null;
        string processId = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_STRATEGIC_CHANGE_REPORT();
        doc.PROCESS_ID = hddProcessID.Value;
        doc.PROCESS_STATUS = ApprovalStatus; //
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = this.Sessions.UserID.ToString();
        doc.SUBJECT = this.RadtxtProduct.Text; ;  // category:Type
        webMaster.Subject = doc.SUBJECT;
        doc.COMPANY_CODE = this.Sessions.CompanyCode.ToString();
        doc.ORGANIZATION_NAME = this.Sessions.OrgName.ToString();
        doc.LIFE_CYCLE = webMaster.LifeCycle.ToString();
        doc.PRODUCT_NAME = RadtxtProduct.Text;

        doc.IS_DISUSED = "N"; //보존연한 
        doc.CREATOR_ID = this.Sessions.UserID.ToString();
        doc.CREATE_DATE = DateTime.Now;

        if (RadchkPrice.Checked)
        {
            doc.STRATEGIC_CHANGE_CODE = RadchkPrice.Value + "/";
            if (RadtxtQty.Text.Equals(string.Empty))
                doc.PRE_SALE_QTY = 0;
            else
                doc.PRE_SALE_QTY = Convert.ToInt32(RadtxtQty.Text);
            doc.PRE_PRICE = (decimal?)RadtxtPreSale.Value;
            doc.NEW_PRICE = (decimal?)RadtxtChangeSale.Value;
            if (RadchkTie.Checked)
                doc.IS_TIE_IN_SALE = "Y";
            else
                doc.IS_TIE_IN_SALE = "N";
        }
        if (RadchkInerruption.Checked)
            doc.STRATEGIC_CHANGE_CODE = doc.STRATEGIC_CHANGE_CODE + RadchkInerruption.Value + "/";
        if (RadchkCapacity.Checked)
            doc.STRATEGIC_CHANGE_CODE = doc.STRATEGIC_CHANGE_CODE + RadchkCapacity.Value + "/";
        if (RadchkProduct.Checked)
            doc.STRATEGIC_CHANGE_CODE = doc.STRATEGIC_CHANGE_CODE + RadchkProduct.Value + "/";

        doc.PURPOSE_JUSTIFICATION = RadtxtPurpose.Text;

        using(StrategicChangeReportMgr mgr = new StrategicChangeReportMgr())
        {
            processId = mgr.MergeStrategicChangeReport(doc);
        }
        return processId;
    } 
    #endregion

    #region ValidationCheck
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        string message = string.Empty;
        if (approvalStatus == ApprovalUtil.ApprovalStatus.Request)
        {
            //if (RadchkPrice.Checked == false && RadchkInerruption.Checked == false && RadchkCapacity.Checked == false && RadchkProduct.Checked == false)
            //    message += "Strategic Change Check";
            //if (RadchkPrice.Checked == true)
            //{
            if (RadtxtProduct.Text.Length <= 0)
                message += "Product Name";
        }
                //if(RadtxtQty.Text.Length <= 0)
                //    message += message.IsNullOrEmptyEx() ? "Pre Sale Qty" : ",Pre Sale Qty";
                //if (RadtxtPreSale.Text.Length <= 0)
                //    message += message.IsNullOrEmptyEx() ? "Pre Sell Sale" : ",Pre Sell Sale";
                //if (RadtxtChangeSale.Text.Length <= 0)
                //    message += message.IsNullOrEmptyEx() ? "Change Sell Sale" : ",Change Sell Sale";
        //    }    
        //    if (RadtxtPurpose.Text.Length <= 0)
        //        message += message.IsNullOrEmptyEx() ? "Purpose and Justfication" : ",Purpose and Justfication";
        //}
        //if (approvalStatus == ApprovalUtil.ApprovalStatus.Saved)
        //{
        //     if (RadchkPrice.Checked == false && RadchkInerruption.Checked == false && RadchkCapacity.Checked == false && RadchkProduct.Checked == false)
        //        message += "Strategic Change Check";
        //}
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;
      
    } 
    #endregion
}