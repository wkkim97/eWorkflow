using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Configuration.Dto;
using Telerik.Web.UI;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Dto;
//using Bayer.WCF.Service;
using System.ServiceModel;
using System.Web.UI.HtmlControls;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Common.Dto;
using System.Text;

public partial class Master_eWorks_Document : DNSoft.eWF.FrameWork.Web.WebBase.DocMasterBase
{
    public DTO_CONFIG _dtoDocCfg = null;

    #region page rendering event
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                InitPage();
            }


            GetDocConfiguration();
            //if (this.hddReUse.Value.Equals("Y")) { this._processstatus = ApprovalUtil.ApprovalStatus.Temp.ToString(); }
            DisplayHeader();
            DisplayFooter();
            InitControls();

            if (!(_processstatus.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || _processstatus.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString())))
            {
                SetEnableControls(HolderDocumentBody);
            }

            //SetControlsReadOnly(); 
        }
        catch (Exception ex)
        {
            this.errorMessage.Value = ex.ToString();
        }
    }

    private void InitPage()
    {
        HtmlInputHidden htmlReuse = HolderDocumentBody.FindControl("hddReuse") as HtmlInputHidden;
        if (htmlReuse.Value == ApprovalUtil.ApprovalStatus.Completed.ToString() || htmlReuse.Value == ApprovalUtil.ApprovalStatus.Reject.ToString())
        {
            HtmlInputHidden htmlProcessId = HolderDocumentBody.FindControl("hddProcessID") as HtmlInputHidden;
            if (htmlReuse.Value == ApprovalUtil.ApprovalStatus.Reject.ToString())
            {
                hddRejectedProcessId.Value = htmlProcessId.Value;
            }
            htmlProcessId.Value = string.Empty;
        }

    }

    /// <summary>
    /// 하위의 모든 UI컨트롤 Enabled 처리
    /// </summary>
    /// <param name="parent">Control Object</param>
    /// <param name="visible">Enabled(true or false)</param>
    public override void SetEnableControls(Control parent, bool visible)
    {
        if (parent.Controls.Count == 0)
        {
            SetEnabled(parent, visible);
            return;
        }
        else
        {
            if (parent is RadGrid)
            {
                foreach (GridColumn column in (parent as RadGrid).Columns)
                {
                    if (column is GridBoundColumn) (column as GridBoundColumn).ReadOnly = !visible;
                    else if (column is GridTemplateColumn)
                    {
                        if ((column as GridTemplateColumn).UniqueName == "REMOVE_BUTTON" || (column as GridTemplateColumn).UniqueName == "REMOVE_BUTTON2")
                            (column as GridTemplateColumn).Display = visible;
                        else (column as GridTemplateColumn).ReadOnly = !visible;
                    }
                }
            }
            else
                foreach (Control c in parent.Controls)
                    SetEnableControls(c, visible);
        }
    }

    #endregion

    #region SetEnableControls
    public void SetEnableControls(Control parent)
    {
        SetEnableControls(parent, false);
    }

    public Control SetEnabled(Control c, bool visible)
    {
        if (c.GetType() == typeof(HtmlGenericControl))
        {
            HtmlGenericControl h = (HtmlGenericControl)c;
            if (visible)
            {
                h.Attributes.Add("display", "");
            }
            else
            {
                h.Attributes.Add("display", "none");
            }
        }
        if (c.GetType() == typeof(RadTextBox))
        {
            RadTextBox txt = (RadTextBox)c;
            int numLines = txt.Text.Split('\n').Length;
            if (numLines > 0)
            {
                Unit unit = new Unit(numLines * 16.00, UnitType.Pixel);
                if (unit.Value > txt.Height.Value)
                    txt.Height = unit;
            }
            txt.ReadOnly = !visible;
            txt.BorderStyle = BorderStyle.None;
            txt.Style.Add("padding-top", "0px");
        }
        else if (c.GetType() == typeof(TextBox))
        {
            TextBox txt = (TextBox)c;
            txt.ReadOnly = !visible;
            txt.BorderStyle = BorderStyle.None;
            txt.Style.Add("padding-top", "0px");
        }
        else if (c.GetType() == typeof(RadButton))
        {
            RadButton btn = (RadButton)c;
            btn.ReadOnly = !visible;
            if (!(btn.ToggleType == ButtonToggleType.Radio || btn.ToggleType == ButtonToggleType.CheckBox))
            {
                btn.Visible = visible;
            }
        }
        else if (c.GetType() == typeof(RadGrid))
        {
            RadGrid grd = (RadGrid)c;
            foreach (GridColumn column in grd.MasterTableView.Columns)
            {
                if (column is GridBoundColumn) (column as GridBoundColumn).ReadOnly = !visible;
                else if (column is GridTemplateColumn)
                {
                    if ((column as GridTemplateColumn).UniqueName == "REMOVE_BUTTON" || (column as GridTemplateColumn).UniqueName == "REMOVE_BUTTON2")
                        (column as GridTemplateColumn).Visible = visible;
                    else (column as GridTemplateColumn).ReadOnly = !visible;
                }
            }
        }
        else if (c.GetType() == typeof(GridTableCell))
        {
            GridTableCell grdCell = (GridTableCell)c;
            grdCell.Enabled = visible;
        }
        else if (c.GetType() == typeof(RadAutoCompleteBox))
        {
            RadAutoCompleteBox auto = (RadAutoCompleteBox)c;

            //auto.ControlStyle.Reset();
            auto.Style.Add("color", "#000!important");
            auto.InputType = RadAutoCompleteInputType.Text;
            auto.TokensSettings.AllowTokenEditing = visible;
            auto.EnableEmbeddedSkins = visible;
            if (!visible)
                auto.OnClientLoad = "OnAutoCOmpleteClientLoad";
        }
        else if (c.GetType() == typeof(RadComboBox))
        {
            RadComboBox cbo = (RadComboBox)c;
            cbo.Enabled = visible;
        }
        else if (c.GetType() == typeof(RadNumericTextBox))
        {
            RadNumericTextBox nTxt = (RadNumericTextBox)c;
            nTxt.ReadOnly = !visible;
            nTxt.BorderStyle = BorderStyle.None;
            nTxt.Style.Add("text-align", "left");
            nTxt.Style.Add("padding-top", "0px");
        }
        else if (c.GetType() == typeof(RadDatePicker))
        {
            RadDatePicker pic = (RadDatePicker)c;
            pic.Enabled = visible;

        }
        else if (c.GetType() == typeof(RadTimePicker))
        {
            RadTimePicker pic = (RadTimePicker)c;
            pic.Enabled = visible;
        }
        else if (c.GetType() == typeof(TimePopupButton))
        {
            CalendarPopupButton time = (CalendarPopupButton)c;
            time.Enabled = visible;
        }
        else if (c.GetType() == typeof(CalendarPopupButton))
        {
            CalendarPopupButton cal = (CalendarPopupButton)c;
            cal.Enabled = visible;
        }
        else if (c.GetType() == typeof(Calendar))
        {
            Calendar cal = (Calendar)c;
            cal.Enabled = visible;
        }
        else if (c.GetType().Name == "DatePickingInput")
        {
            RadDateInput ci = (RadDateInput)c;
            ci.ReadOnly = !visible;
        }
        else if (c.GetType() == typeof(DropDownListItem))
        {
            DropDownListItem d = (DropDownListItem)c;
            d.Enabled = visible;
        }

        return c;
    }
    #endregion

    #region OnPreRender
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }
    #endregion

    #region GetDocConfiguration
    private void GetDocConfiguration()
    {
        if (_documentid.Equals(string.Empty)) throw new Exception("DocumentID가 없습니다.");

        using (Bayer.eWF.BSL.Configuration.Mgr.ConfigurationMgr mgr = new Bayer.eWF.BSL.Configuration.Mgr.ConfigurationMgr())
        {
            _dtoDocCfg = mgr.SelectConfiguration(_documentid);
            if (_dtoDocCfg != null)
            {
                _lifecycle = _dtoDocCfg.RETENTION_PERIOD;
                _lifecycleText = _dtoDocCfg.RETENTION_PERIOD_TEXT;
                _AddAdditionalApproval = _dtoDocCfg.ADD_ADDTIONAL_APPROVER.Equals("Y") ? true : false;
                _ReviwerDesc = _dtoDocCfg.ADD_REVIEWER_DESCRIPTION;
                _allowForward = _dtoDocCfg.FORWARD_YN;

            }
        }
    }
    #endregion

    #region DisplayHeader

    public void DisplayHeader()
    {

        RadButton btnRequest = (RadButton)ApproveMenuBar.FindControl("btnRequest");
        RadButton btnApproval = (RadButton)ApproveMenuBar.FindControl("btnApproval");
        RadButton btnForwardApproval = (RadButton)ApproveMenuBar.FindControl("btnForwardApproval");
        RadButton btnReject = (RadButton)ApproveMenuBar.FindControl("btnReject");
        RadButton btnForward = (RadButton)ApproveMenuBar.FindControl("btnForward");
        RadButton btnRecall = (RadButton)ApproveMenuBar.FindControl("btnRecall");
        RadButton btnWithdraw = (RadButton)ApproveMenuBar.FindControl("btnWithdraw");
        RadButton btnRemind = (RadButton)ApproveMenuBar.FindControl("btnRemind");
        RadButton btnExit = (RadButton)ApproveMenuBar.FindControl("btnExit");
        RadButton btnSave = (RadButton)ApproveMenuBar.FindControl("btnSave");
        RadButton btnInputCommand = (RadButton)ApproveMenuBar.FindControl("btnInputCommand");
        RadButton btnReUse = (RadButton)ApproveMenuBar.FindControl("btnReUse");

        btnRequest.Visible = _authBtnList[(int)ApprovalUtil.ApprovalButtons.REQUEST] == 1 ? true : false;
        btnApproval.Visible = _authBtnList[(int)ApprovalUtil.ApprovalButtons.APPROVAL] == 1 ? true : false;
        btnForwardApproval.Visible = _authBtnList[(int)ApprovalUtil.ApprovalButtons.FORWARDAPPRVAL] == 1 ? true : false;
        btnReject.Visible = _authBtnList[(int)ApprovalUtil.ApprovalButtons.REJECT] == 1 ? true : false;
        if (_allowForward.Equals("Y"))
            btnForward.Visible = _authBtnList[(int)ApprovalUtil.ApprovalButtons.FORWARD] == 1 ? true : false;
        else
            btnForward.Visible = false;
        btnRecall.Visible = _authBtnList[(int)ApprovalUtil.ApprovalButtons.RECALL] == 1 ? true : false;
        btnWithdraw.Visible = _authBtnList[(int)ApprovalUtil.ApprovalButtons.WITHDRAW] == 1 ? true : false;
        btnRemind.Visible = _authBtnList[(int)ApprovalUtil.ApprovalButtons.REMIND] == 1 ? true : false;
        btnExit.Visible = _authBtnList[(int)ApprovalUtil.ApprovalButtons.EXIT] == 1 ? true : false;
        btnSave.Visible = _authBtnList[(int)ApprovalUtil.ApprovalButtons.SAVE] == 1 ? true : false;
        btnInputCommand.Visible = _authBtnList[(int)ApprovalUtil.ApprovalButtons.INPUTCOMMENT] == 1 ? true : false;
        btnReUse.Visible = _authBtnList[(int)ApprovalUtil.ApprovalButtons.REUSE] == 1 ? true : false;

        System.Web.UI.HtmlControls.HtmlGenericControl htmlTitle = (System.Web.UI.HtmlControls.HtmlGenericControl)ApproveMenuBar.FindControl("hspanTitle");
        htmlTitle.InnerText = _dtoDocCfg.DOC_NAME;
        Title = _dtoDocCfg.DOC_NAME;
        Page.Title = this.Title + "-[Bayer eWorks]";

        System.Web.UI.HtmlControls.HtmlGenericControl htmlCompany = (System.Web.UI.HtmlControls.HtmlGenericControl)ApproveMenuBar.FindControl("hspanCompany");
        htmlCompany.InnerText = _company;

        System.Web.UI.HtmlControls.HtmlGenericControl htmlRequester = (System.Web.UI.HtmlControls.HtmlGenericControl)ApproveMenuBar.FindControl("hspanRequester");
        htmlRequester.InnerText = _requester;

        System.Web.UI.HtmlControls.HtmlGenericControl htmlOrganization = (System.Web.UI.HtmlControls.HtmlGenericControl)ApproveMenuBar.FindControl("hspanOrganization");
        htmlOrganization.InnerText = _organization;

        System.Web.UI.HtmlControls.HtmlGenericControl htmlLifeCycle = (System.Web.UI.HtmlControls.HtmlGenericControl)ApproveMenuBar.FindControl("hspanLifeCycle");
        htmlLifeCycle.InnerText = _lifecycleText;

        System.Web.UI.HtmlControls.HtmlGenericControl htmlDocumentNo = (System.Web.UI.HtmlControls.HtmlGenericControl)ApproveMenuBar.FindControl("hspanDocumentNo");
        htmlDocumentNo.InnerText = _documentno;

        if (!(_processstatus.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || _processstatus.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString())))
        {
            this.ApproveMenuBar.VisibleAdditionalApproval = false;
            this.lblReviewerDesc.Visible = false;
        }
        else
        {
            this.ApproveMenuBar.VisibleAdditionalApproval = _AddAdditionalApproval;
            if (_ReviwerDesc.Trim().Length > 0)
                this.lblReviewerDesc.Text = "* " + _ReviwerDesc;
        }



    }

    #endregion

    #region DisplayFooter
    public void DisplayFooter()
    {
        // RadPanelBar panelFooter = (RadPanelBar)DocumentBaseFooter.FindControl("panelFooter");

        foreach (RadPanelItem item in panelFooter.Items)
        {
            switch (item.Value)
            {
                case "AddReviewer":
                    item.Visible = _dtoDocCfg.ADD_REVIEWER.ToUpper() == "Y" && (_processstatus.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || _processstatus.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString())) ? true : false;
                    break;
                case "Attachment":
                    break;
                case "ApprovalLine":
                    item.Visible = _processstatus.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || _processstatus.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString()) ? false : true;
                    break;
                case "Recipient":
                    item.Visible = _processstatus.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || _processstatus.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString()) ? false : true;
                    break;
                case "Reviewer":
                    item.Visible = _processstatus.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || _processstatus.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString()) ? false : true;
                    break;
                case "Log":
                    item.Visible = _processstatus.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || _processstatus.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString()) ? false : true;
                    break;

            }
        }
        CtrlFileUpload.Mode = "UPLOAD";
        CtrlFileUpload.ProcessID = ProcessID;
        if (!(_processstatus.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()) || _processstatus.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString())))
        {
            ApprovalLineBind();
            DocLogListBind();
            CtrlFileUpload.Mode = "DOWNLOAD";
            CtrlFileUpload.ProcessID = ProcessID;
        }


        UserAutoCompleteBox.ApprovalType = "V";


    }

    private void ApprovalLineBind()
    {
        List<DTO_PROCESS_APPROVAL_LIST> appr;
        List<DTO_PROCESS_APPROVAL_LIST> appLine;
        List<DTO_PROCESS_APPROVAL_LIST> recipient;
        List<DTO_PROCESS_APPROVAL_LIST> reviewer;


        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            appr = mgr.SelectProcessApproverList(ProcessID);
            appLine = new List<DTO_PROCESS_APPROVAL_LIST>();
            recipient = new List<DTO_PROCESS_APPROVAL_LIST>();
            reviewer = new List<DTO_PROCESS_APPROVAL_LIST>();

            if (appr.Count > 0)
                this.hddRequester.Value = appr[0].APPROVER_ID;

            foreach (DTO_PROCESS_APPROVAL_LIST item in appr)
            {

                if (item.APPROVAL_TYPE.Equals(ApprovalUtil.ApprovalType.DRAFTER) || item.APPROVAL_TYPE.Equals(ApprovalUtil.ApprovalType.APPROVER))
                {
                    appLine.Add(item);
                    if (item.IDX == 1)
                    {
                        System.Web.UI.HtmlControls.HtmlGenericControl htmlCompany = (System.Web.UI.HtmlControls.HtmlGenericControl)ApproveMenuBar.FindControl("hspanCompany");
                        htmlCompany.InnerText = item.COMPANY_NAME;

                        System.Web.UI.HtmlControls.HtmlGenericControl htmlRequester = (System.Web.UI.HtmlControls.HtmlGenericControl)ApproveMenuBar.FindControl("hspanRequester");
                        htmlRequester.InnerText = item.APPROVER;

                        System.Web.UI.HtmlControls.HtmlGenericControl htmlOrganization = (System.Web.UI.HtmlControls.HtmlGenericControl)ApproveMenuBar.FindControl("hspanOrganization");
                        htmlOrganization.InnerText = item.APPROVER_ORG_NAME;

                    }

                }
                else if (item.APPROVAL_TYPE.Equals(ApprovalUtil.ApprovalType.RECIPIENT))
                {
                    recipient.Add(item);
                }
                else if (item.APPROVAL_TYPE.Equals(ApprovalUtil.ApprovalType.REVIEWER))
                {
                    reviewer.Add(item);
                }
            }

            this.hddRecipients.Value = JsonConvert.toJson(recipient);
        }

        grdApprovalLIne.DataSource = appLine;
        grdApprovalLIne.DataBind();

        viewRecipient.DataSource = recipient;
        viewRecipient.DataBind();

        viewReviewer.DataSource = reviewer;
        viewReviewer.DataBind();
    }

    private void DocLogListBind()
    {
        List<DTO_DOC_LOG_LIST> doc;
        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            doc = mgr.SelectDocLogList(ProcessID);
        }
        grdDocLog.DataSource = doc;
        grdDocLog.DataBind();
    }

    #endregion

    #region InitControls
    private void InitControls()
    {
        winCommand.VisibleOnPageLoad = false;
        winCommand.NavigateUrl = "";
    }
    #endregion

    #region MasterProcessing
    public override void MasterProcessing(string methodName)
    {
        try
        {
            switch (methodName)
            {
                case "DoRequest":
                    ApprovalAddtional();
                    ShowApprovalLine();
                    break;
                case "DoSave":
                    SavedProcessDocument();
                    ApprovalAddtional();
                    SaveAttachFiles();
                    CallClientScript("fn_DocumentCofirmClose()");
                    break;
                case "DoApproval":
                    CallApproval("Approval", ApprovalUtil.ApprovalButtons.APPROVAL.ToString(), "fn_CloseApprovalLine");                   
                    break;
                case "DoForwardApproval":
                    CallFowardApproval("Forward Approval", ApprovalUtil.ApprovalButtons.FORWARDAPPRVAL.ToString(), "fn_CloseApprovalLine");
                    break;
                case "DoReject":
                    CallApproval("Reject", ApprovalUtil.ApprovalButtons.REJECT.ToString(), "fn_CloseApprovalLine");
                    break;
                case "DoForward":
                    CallFoward("Forward ", ApprovalUtil.ApprovalButtons.FORWARDAPPRVAL.ToString(), "fn_CloseApprovalLine");
                    break;
                case "DoRecall":
                    CallRecall();
                    break;
                case "DoWithdraw":
                    CallApproval("Withdraw", ApprovalUtil.ApprovalButtons.WITHDRAW.ToString(), "fn_CloseApprovalLine");
                    break;
                case "DoInputComment":
                    CallInputComment("Input Comment", "fn_CloseApprovalLine");
                    break;
                case "DoReUse":
                    string formname = System.IO.Path.GetFileName(Request.Url.LocalPath);
                    string reuse = ProcessStatus;
                    if (!Page.ClientScript.IsStartupScriptRegistered("fn_ReUseClicked"))
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "fn_ReUseClicked", "fn_ReUseClicked('" + formname + "','" + DocumentID + "','" + ProcessID + "','" + reuse + "');", true);




                    //CallClientScript(fun);                    
                    break;               
            }
            RadButton btnRequest = (RadButton)ApproveMenuBar.FindControl("btnRequest");
            RadButton btnApproval = (RadButton)ApproveMenuBar.FindControl("btnApproval");
            RadButton btnForwardApproval = (RadButton)ApproveMenuBar.FindControl("btnForwardApproval");
            RadButton btnReject = (RadButton)ApproveMenuBar.FindControl("btnReject");
            RadButton btnForward = (RadButton)ApproveMenuBar.FindControl("btnForward");
            RadButton btnRecall = (RadButton)ApproveMenuBar.FindControl("btnRecall");
            RadButton btnWithdraw = (RadButton)ApproveMenuBar.FindControl("btnWithdraw");
            RadButton btnRemind = (RadButton)ApproveMenuBar.FindControl("btnRemind");
            if (methodName == "DoRequest")
            {
                btnRequest.Visible = true;
            }
            else
            {
                btnRequest.Visible = false;
            }
            
            btnApproval.Visible = false;
            btnForwardApproval.Visible = false;
            btnReject.Visible = false;
            btnForward.Visible = false;
            btnRecall.Visible = false;
            btnWithdraw.Visible = false;
            btnRemind.Visible = false;
           
        }
        catch (Exception ex)
        {
            this.errorMessage.Value = ex.ToString();
        }

    }
    #endregion

    #region CallInputComment
    private void CallInputComment(string title, string CloseFunctionName)
    {
        winCommand.Title = title;
        winCommand.VisibleOnPageLoad = true;
        winCommand.Width = 490;
        winCommand.Height = 580;
        winCommand.Behaviors = Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close;          
        winCommand.OnClientClose = CloseFunctionName;
        winCommand.NavigateUrl = string.Format("/eWorks/Approval/Process/InputComment.aspx?processid={0}&documentid={1}"
                                                        , this.ProcessID
                                                        , this.DocumentID
                                                    );
        

    }
    #endregion

    #region CallRecall
    private void CallRecall()
    {
        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            //string comment = string.Format("[Recall]({0}) : {1}", DateTime.Now.ToString("yyyy-MM-dd"), Requester);
            string comment = string.Format("", DateTime.Now.ToString("yyyy-MM-dd"), Requester);
            mgr.UpdateRecall(this.DocumentID, this.ProcessID, comment, RequestID, ApprovalUtil.LogType.Recall.ToString(), ApprovalUtil.ApprovalStatus.Saved.ToString());
        }
        MoveTemp();

    }
    #endregion

    #region MoveTemp
    private void MoveTemp()
    {
        List<DTO_ATTACH_FILES> filelist;
        string strTempUploadPath = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/WebServer/FileMgr/UploadTempFolder");

        strTempUploadPath = string.Format(@"{0}\{1}\{2}", strTempUploadPath, RequestID, ApprovalUtil.AttachFileType.Temp.ToString());
        using (Bayer.eWF.BSL.Common.Mgr.FileMgr mgr = new Bayer.eWF.BSL.Common.Mgr.FileMgr())
        {
            filelist = mgr.SelectAttachFileList(ProcessID, ApprovalUtil.AttachFileType.Common.ToString());
            mgr.MoveToFile(ProcessID, RequestID, strTempUploadPath, filelist);
        }
        ClientWindowClose();
    }
    #endregion

    #region ClientWindowClose
    protected void ClientWindowClose()
    {
        StringBuilder script = new StringBuilder(128);
        try
        {
            script.Append("<script language='javascript'>");
            script.Append("function f() {");
            script.Append("Sys.Application.remove_load(f);");
            script.Append("var oWnd = GetRadWindow();");
            script.Append("window.opener.location.reload();");
            script.Append("oWnd.close();");
            script.Append("}");
            script.Append("Sys.Application.add_load(f);");
            script.Append("</script>");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "RequestComplete", script.ToString());
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (script != null)
            {
                script.Clear();
                script = null;
            }
        }
    }
    #endregion

    #region CallFowardApproval
    private void CallFowardApproval(string title, string buttonType, string CloseFunctionName)
    {
        winCommand.Title = title;
        winCommand.VisibleOnPageLoad = true;
        winCommand.Width = 560;
        winCommand.Height = 590;
       // winCommand.Behaviors = Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close;
        winCommand.Behaviors = Telerik.Web.UI.WindowBehaviors.Move;
        winCommand.OnClientClose = CloseFunctionName;
        winCommand.NavigateUrl = string.Format("/eWorks/Approval/Process/ForwardApproval.aspx?processid={0}&documentid={1}"
                                                        , this.ProcessID
                                                        , this.DocumentID
                                                    );
        
    }
    #endregion

    #region CallFoward
    private void CallFoward(string title, string buttonType, string CloseFunctionName)
    {
        winCommand.Title = title;
        winCommand.VisibleOnPageLoad = true;
        winCommand.Width = 450;
        winCommand.Height = 210;
        //winCommand.Behaviors = Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close;
        winCommand.Behaviors = Telerik.Web.UI.WindowBehaviors.Move;
        winCommand.OnClientClose = CloseFunctionName;
        winCommand.NavigateUrl = string.Format("/eWorks/Approval/Process/Forward.aspx?processid={0}&documentid={1}"
                                                        , this.ProcessID
                                                        , this.DocumentID

                                            );
    }
    #endregion

    #region CallApproval
    private void CallApproval(string title, string buttonType, string CloseFunctionName)
    {
        winCommand.Title = title;
        winCommand.VisibleOnPageLoad = true;
        winCommand.Width = 400;
        winCommand.Height = 300;
       // winCommand.Behaviors = Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close;
        winCommand.Behaviors = Telerik.Web.UI.WindowBehaviors.Move;
        winCommand.OnClientClose = CloseFunctionName;
        winCommand.NavigateUrl = string.Format("/eWorks/Approval/Process/Approve.aspx?processid={0}&documentid={1}&processtype={2}"
                                                        , this.ProcessID
                                                        , this.DocumentID
                                                        , buttonType
                                                    );
    }
    #endregion

    #region ShowApprovalLine
    private void ShowApprovalLine()
    {
        hddXmlFiles.Value = HttpUtility.UrlEncode(CtrlFileUpload.XmlAttachFiles);
        winCommand.Title = "Approval Line";
        winCommand.VisibleOnPageLoad = true;
        winCommand.Width = 490;
        winCommand.Height = 420;
        winCommand.Behaviors = Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close;
        winCommand.OnClientClose = "fn_CloseApprovalLine";
        winCommand.NavigateUrl = string.Format("/eWorks/Approval/Process/Request.aspx?processid={0}&documentid={1}&documentname={2}&subject={3}&folderpath={4}&attachfiles={5}&rejectedid={6}"
                                                        , this.ProcessID
                                                        , this.DocumentID
                                                        , HttpUtility.UrlEncode(this.Title)
                                                        , HttpUtility.UrlEncode(this.Subject)
                                                        , HttpUtility.UrlEncode(CtrlFileUpload.UploadFileTemp)
                                                        , "" // HttpUtility.UrlEncode(CtrlFileUpload.XmlAttachFiles)
                                                        , HttpUtility.UrlEncode(this.hddRejectedProcessId.Value)
                                                    );

    }
    #endregion

    #region UserApprovalAdditional
    /// <summary>
    /// 2014.12.12추가 --Additional Approval
    /// 사용자에 의해 추가된 ...        
    /// </summary>
    private void UserApprovalAdditional()
    {
        AutoCompleteBoxEntryCollection lstAdditonal = this.ApproveMenuBar.GetAdditionalApproval;
        List<Bayer.eWF.BSL.Approval.Dto.DTO_PROCESS_APPROVER_ADDTIONAL> add;
        Bayer.eWF.BSL.Common.Dto.SmallUserInfoDto userInfo;
        int idx = 0;
        try
        {
            add = new List<Bayer.eWF.BSL.Approval.Dto.DTO_PROCESS_APPROVER_ADDTIONAL>();

            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                mgr.DeleteProcessApproveAddtional(ProcessID, ApprovalUtil.ApprovalType.APPROVER);
            }
            
            foreach (AutoCompleteBoxEntry entry in lstAdditonal)
            {
                ++idx;
                Bayer.eWF.BSL.Approval.Dto.DTO_PROCESS_APPROVER_ADDTIONAL item = new Bayer.eWF.BSL.Approval.Dto.DTO_PROCESS_APPROVER_ADDTIONAL();
                userInfo = JsonConvert.JsonDeserialize<Bayer.eWF.BSL.Common.Dto.SmallUserInfoDto>(entry.Value);
                item.PROCESS_ID = ProcessID;
                item.USER_ID = userInfo.USER_ID;
                item.IDX = idx;
                item.CREATE_DATE = DateTime.Now;
                item.APPROVAL_TYPE = ApprovalUtil.ApprovalType.APPROVER;
                add.Add(item);
            }

            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                mgr.InsertProcessApproveAddtional(ProcessID, add);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

        }

    }
    #endregion

    #region ApprovalAddtional
    private void ApprovalAddtional()
    {
        //2014.12.12추가
        if (this._AddAdditionalApproval) UserApprovalAdditional();

        AutoCompleteBoxEntryCollection EntryList = UserAutoCompleteBox.GetEntries();
        List<Bayer.eWF.BSL.Approval.Dto.DTO_PROCESS_APPROVER_ADDTIONAL> add;
        Bayer.eWF.BSL.Common.Dto.SmallUserInfoDto userInfo;
        int idx = 0;
        try
        {
            add = new List<Bayer.eWF.BSL.Approval.Dto.DTO_PROCESS_APPROVER_ADDTIONAL>();

            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                mgr.DeleteProcessApproveAddtional(ProcessID, "T");
            }

            foreach (AutoCompleteBoxEntry entry in EntryList)
            {
                ++idx;
                Bayer.eWF.BSL.Approval.Dto.DTO_PROCESS_APPROVER_ADDTIONAL item = new Bayer.eWF.BSL.Approval.Dto.DTO_PROCESS_APPROVER_ADDTIONAL();
                userInfo = JsonConvert.JsonDeserialize<Bayer.eWF.BSL.Common.Dto.SmallUserInfoDto>(entry.Value);
                item.PROCESS_ID = ProcessID;
                item.USER_ID = userInfo.USER_ID;
                item.IDX = idx;
                item.CREATE_DATE = DateTime.Now;
                item.APPROVAL_TYPE = "T";
                // UserAutoCompleteBox.ApprovalType; Request시점에 사용자에 의해 추가된 Reviewer는 Approver테이블에 추가 되므로 중복 조회를 방지하기위해
                // Delegation된 사용자가 Addtional에 'V'로 추가 되고 조회 해야하므로
                add.Add(item);

            }

            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                mgr.InsertProcessApproveAddtional(ProcessID, add);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

        }
    }
    #endregion

    #region SaveAttachFiles
    private void SaveAttachFiles()
    {
        try
        {
            CtrlFileUpload.ProcessID = ProcessID;
            CtrlFileUpload.SaveAttach(ProcessID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

        }


    }
    #endregion

    #region SavedProcessDocument
    private void SavedProcessDocument()
    {
        DTO_PROCESS_DOCUMENT docProc = new DTO_PROCESS_DOCUMENT();
        docProc.PROCESS_ID = ProcessID;
        docProc.DOCUMENT_ID = DocumentID;
        docProc.DOC_NAME = _dtoDocCfg.DOC_NAME;
        docProc.SUBJECT = Subject;
        docProc.DOC_NUM = "";
        docProc.PROCESS_STATUS = ApprovalUtil.ApprovalStatus.Saved.ToString();
        docProc.REQUEST_DATE = DateTime.Now;
        docProc.COMPANY_CODE = CompanyCode;
        docProc.REQUESTER_ID = RequestID;
        docProc.CURRENT_APPROVER = String.Empty;
        docProc.FINAL_APPROVER = String.Empty;
        docProc.REJECTED_PROCESS_ID = this.hddRejectedProcessId.Value;
        docProc.CREATE_DATE = DateTime.Now;


        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            mgr.InsertProcessDocument(docProc);
        }
    }
    #endregion

    #region CallClientScript
    protected void CallClientScript(string strFnc)
    {
        string radconfirmscript = "<script language='javascript'>function f(){ " + strFnc + "; Sys.Application.remove_load(f);}; Sys.Application.add_load(f);</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "radconfirm", radconfirmscript);
    }
    #endregion

    #region 프로퍼티


    public override event EventHandler btnRequestClicked
    {
        add
        {
            lock (this)
            {
                ApproveMenuBar.btnRequestClick += value;
            }
        }
        remove { }  //lock (this) { ApproveMenuBar.btnRequestClick -= value; }
    }

    public override event EventHandler btnApprovalClicked
    {
        add { lock (this) { ApproveMenuBar.btnApprovalClick += value; } }
        remove { }
    }

    public override event EventHandler btnFowardApprovalClicked
    {
        add { lock (this) { ApproveMenuBar.btnFowardApprovalClick += value; } }
        remove { }
    }

    public override event EventHandler btnRejectClicked
    {
        add { lock (this) { ApproveMenuBar.btnRejectClick += value; } }
        remove { }
    }

    public override event EventHandler btnFowardClicked
    {
        add { lock (this) { ApproveMenuBar.btnFowardClick += value; } }
        remove { }
    }

    public override event EventHandler btnRecallClicked
    {
        add { lock (this) { ApproveMenuBar.btnRecallClick += value; } }
        remove { }
    }

    public override event EventHandler btnWithdrawClicked
    {
        add { lock (this) { ApproveMenuBar.btnWithdrawClick += value; } }
        remove { }
    }

    public override event EventHandler btnExitClicked
    {
        add { lock (this) { ApproveMenuBar.btnExitClick += value; } }
        remove { }
    }

    public override event EventHandler btnSaveClicked
    {
        add { lock (this) { ApproveMenuBar.btnSaveClick += value; } }
        remove { }
    }

    public override event EventHandler btnInputCommandClick
    {
        add { lock (this) { ApproveMenuBar.btnInputCommandClick += value; } }
        remove { }
    }

    public override event EventHandler btnRemindClicked
    {
        add { lock (this) { ApproveMenuBar.btnRemindClick += value; } }
        remove { }
    }

    public override event EventHandler btnReUseClicked
    {
        add { lock (this) { ApproveMenuBar.btnReUseClick += value; } }
        remove { }
    }
    #endregion

    #region grdDocLog_ItemDataBound
    protected void grdDocLog_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DTO_DOC_LOG_LIST i = e.Item.DataItem as DTO_DOC_LOG_LIST;
            GridDataItem item = e.Item as GridDataItem;


            Image attach = item.FindControl("iconAttach") as Image;
            if (i.FILE_IDX > 0)
            {
                attach.ImageUrl = "/eWorks/Styles/images/Common/icon_attach.gif";

            }
            else
            {
                attach.ImageUrl = "/eWorks/Styles/images/dot.png";
            }
        }
    }
    #endregion
}
