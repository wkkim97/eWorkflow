
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Configuration.Dto;
using Bayer.eWF.BSL.Configuration.Mgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Configuration_Configuration : DNSoft.eWF.FrameWork.Web.PageBase
{
    public enum ConfigCondition
    {
        Equals,
        GreaterThan,
        LessThan,
        StartWith,
        NotStartWith,
        Include,
        NotInclude,
        IsNull,
        IsNotNull
    }

    public enum ApprovalType
    {
        ApprovalLevel = 1,
        JobTitle = 2,
        LimitAmount = 3,
    }

    public enum ApprovalLocation
    {
        Before = 1,
        After = 2,
        Recipient = 3,
        Reviewer = 4,
    }

    private List<ConditionData> lstCondition = null;
    private const string CONDITION_VIEWSTATE_NAME = "LIST_CONDITION";

    protected override void OnInit(EventArgs e)
    {
        InitControl();
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                this.lstCondition = (List<ConditionData>)ViewState[CONDITION_VIEWSTATE_NAME];
                BindingDocumentList();
            }
            else
            {
                if (ViewState[CONDITION_VIEWSTATE_NAME] == null)
                {
                    this.lstCondition = new List<ConditionData>();
                    ViewState[CONDITION_VIEWSTATE_NAME] = this.lstCondition;
                }
                else
                    this.lstCondition = (List<ConditionData>)ViewState[CONDITION_VIEWSTATE_NAME];
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.Message;
        }
    }

    /// <summary>
    /// Page로딩 시점에 코드관련 값을 가져와 컨트롤 초기화
    /// </summary>
    private void InitControl()
    {
        using (CodeMgr mgr = new CodeMgr())
        {
            //Company
            List<DTO_CODE_SUB> companies = mgr.SelectCodeSubList("S001");
            this.divCompany.Controls.Clear();
            foreach (DTO_CODE_SUB company in companies)
            {
                RadButton radBtnCompany = new RadButton();
                radBtnCompany.ButtonType = RadButtonType.ToggleButton;
                radBtnCompany.ToggleType = ButtonToggleType.CheckBox;
                radBtnCompany.Text = company.CODE_NAME;
                radBtnCompany.Value = company.SUB_CODE;
                radBtnCompany.AutoPostBack = false;
                radBtnCompany.ID = "radBtnCompany" + company.SUB_CODE;
                radBtnCompany.GroupName = "Company";
                radBtnCompany.Attributes.Add("style", "margin-right:15px");
                this.divCompany.Controls.Add(radBtnCompany);
            }

            //Job Title
            List<DTO_CODE_SUB> jobTitles = mgr.SelectCodeSubList("S005");
            this.divJobTitle.Controls.Clear();
            foreach (DTO_CODE_SUB code in jobTitles)
            {
                RadButton radBtnJob = new RadButton();
                radBtnJob.ButtonType = RadButtonType.ToggleButton;
                radBtnJob.ToggleType = ButtonToggleType.Radio;
                radBtnJob.Text = code.CODE_NAME;
                radBtnJob.Value = code.SUB_CODE;
                radBtnJob.AutoPostBack = false;
                radBtnJob.ID = "radBtnJobTitle" + code.SUB_CODE;
                radBtnJob.GroupName = "JobTitle";
                radBtnJob.Attributes.Add("class", "spanJobTitle");
                radBtnJob.Enabled = false;
                this.divJobTitle.Controls.Add(radBtnJob);
            }

            //Category
            List<DTO_CODE_SUB> categories = mgr.SelectCodeSubList("S002");
            foreach (DTO_CODE_SUB category in categories)
            {
                this.radCmbCategory.Items.Add(new DropDownListItem(category.CODE_NAME, category.SUB_CODE));
            }
            if (this.radCmbCategory.Items.Count > 0) this.radCmbCategory.SelectedIndex = 0;

            //Information Classification
            List<DTO_CODE_SUB> infoClassifications = mgr.SelectCodeSubList("S003");
            foreach (DTO_CODE_SUB classification in infoClassifications)
            {
                this.radCmbInfoClassification.Items.Add(new DropDownListItem(classification.CODE_NAME, classification.SUB_CODE));
            }
            if (this.radCmbInfoClassification.Items.Count > 0) this.radCmbInfoClassification.SelectedIndex = 0;

            //Retention Period
            List<DTO_CODE_SUB> retentionPeriods = mgr.SelectCodeSubList("S006");
            foreach (DTO_CODE_SUB retention in retentionPeriods)
            {
                this.radCmbRetentionPeriod.Items.Add(new DropDownListItem(retention.CODE_NAME, retention.SUB_CODE));
            }
            if (this.radCmbRetentionPeriod.Items.Count > 0) this.radCmbRetentionPeriod.SelectedIndex = 2;

        }
    }

    #region [ Inner Method ]

    private void InitRadGrid()
    {
        //Condition Grid초기화
        this.radGrdCondition.DataSource = string.Empty;
        this.radGrdCondition.DataBind();

        //Before 
        this.radGrdBeforeApprover.DataSource = string.Empty;
        this.radGrdBeforeApprover.DataBind();

        //After
        this.radGrdAfterApprover.DataSource = string.Empty;
        this.radGrdAfterApprover.DataBind();

        //Recipient
        this.radGrdRecipient.DataSource = string.Empty;
        this.radGrdRecipient.DataBind();

        //Reviewer
        this.radGrdReviewer.DataSource = string.Empty;
        this.radGrdReviewer.DataBind();


    }

    private void InitInputScreen()
    {
        this.radTxtDocName.Text = "";
        this.radTxtDataOwner.Text = "";
        this.radTxtTableName.Text = "";
        this.radTxtFormName.Text = "";
        foreach (Control control in this.divCompany.Controls)
        {
            if (control is RadButton)
                (control as RadButton).Checked = false;
        }
        this.radCmbCategory.SelectedIndex = 0;
        //this.radCmbReadersGroup.SelectedIndex = 0;
        this.radTxtPrefix.Text = "";
        this.radCmbInfoClassification.SelectedIndex = 0;
        this.radTxtServiceName.Text = "";
        this.radCmbRetentionPeriod.SelectedIndex = 0;
        this.radTxtDescription.Text = "";

        this.radBtnForwardYes.Checked = true;
        this.radBtnAddApproverNo.Checked = true;
        this.radBtnShowDocumentListYes.Checked = true;
        this.radBtnAddReviewerYes.Checked = true;
        this.radTxtReviewerDescription.Text = "";

        this.radBtnApprovalLevel.Checked = true;
        this.radTxtApprovalLevel.Text = "1";

        this.radDdlApprovalType.SelectedIndex = 0;
        this.radTxtUserId.Text = "";
        this.radBtnIsMandatory.Checked = false;

        InitRadGrid();

    }

    #endregion

    #region [ 문서목록 조회 ]

    private List<DTO_CONFIG> SelectDocumentList()
    {
        using (ConfigurationMgr mgr = new ConfigurationMgr())
        {
            return mgr.SelectConfigurationList();
        }
    }

    private void BindingDocumentList()
    {
        this.radGrdDocList.DataSource = SelectDocumentList();
        this.radGrdDocList.DataBind();

        InitRadGrid();

    }

    protected void radGrdDocList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        this.radGrdDocList.DataSource = SelectDocumentList();
        this.radGrdDocList.Rebind();
    }

    #endregion

    #region [ 결재문서 목록 Grid Events ]

    protected void radGrdDocList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.radGrdDocList.Items.Count > 0)
        {
            GridDataItem item = this.radGrdDocList.SelectedItems[0] as GridDataItem;
            if (item != null)
            {
                string documentId = item["DOCUMENT_ID"].Text;

                SelectDocument(documentId);
                SelectCondition(documentId);
            }
        }
    }

    #endregion

    #region [ 문서상세 조회 - 좌측Grid에서 선택시]

    /// <summary>
    /// Before 결재자 목록
    /// </summary>
    /// <param name="documentId"></param>
    private void SelectBeforeApproverCondition(string documentId)
    {
        try
        {
            using (ConfigurationMgr mgr = new ConfigurationMgr())
            {
                List<DTO_CONFIG_APPROVER> beforeApprovers = mgr.SelectConfigurationApprover(documentId, ApprovalLocation.Before.ToString().Substring(0, 1));
                this.radGrdBeforeApprover.DataSource = beforeApprovers;
                this.radGrdBeforeApprover.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.Message;
        }
    }

    /// <summary>
    /// After 결재자 목록
    /// </summary>
    /// <param name="documentId"></param>
    private void SelectAfterApproverCondition(string documentId)
    {
        try
        {
            using (ConfigurationMgr mgr = new ConfigurationMgr())
            {
                List<DTO_CONFIG_APPROVER> afterApprovers = mgr.SelectConfigurationApprover(documentId, ApprovalLocation.After.ToString().Substring(0, 1));
                this.radGrdAfterApprover.DataSource = afterApprovers;
                this.radGrdAfterApprover.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.Message;
        }
    }

    /// <summary>
    /// Recipient 목록
    /// </summary>
    /// <param name="documentId"></param>
    private void SelectRecipientCondition(string documentId)
    {
        try
        {
            using (ConfigurationMgr mgr = new ConfigurationMgr())
            {
                List<DTO_CONFIG_RECIPIENT> recipients = mgr.SelectConfigurationRecipient(documentId);
                this.radGrdRecipient.DataSource = recipients;
                this.radGrdRecipient.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    /// <summary>
    /// Reviewer 목록
    /// </summary>
    /// <param name="documentId"></param>
    private void SelectReviewerCondition(string documentId)
    {
        try
        {
            using (ConfigurationMgr mgr = new ConfigurationMgr())
            {
                List<DTO_CONFIG_REVIEWER> reviewers = mgr.SelectConfigurationReviewer(documentId);
                this.radGrdReviewer.DataSource = reviewers;
                this.radGrdReviewer.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    private void SelectCondition(string documentId)
    {
        this.radGrdCondition.DataSource = string.Empty;
        this.radGrdCondition.DataBind();

        SelectBeforeApproverCondition(documentId);
        SelectAfterApproverCondition(documentId);
        SelectRecipientCondition(documentId);
        SelectReviewerCondition(documentId);

        using (ConfigurationMgr mgr = new ConfigurationMgr())
        {
            List<DbTableColumnDto> columns = mgr.SelectDbTableColumn(this.radTxtTableName.Text);
            this.radDdlFieldName.DataTextField = "ColumnName";
            this.radDdlFieldName.DataValueField = "ColumnId";

            this.radDdlFieldName.DataSource = columns;
            this.radDdlFieldName.DataBind();
        }

        ViewState[CONDITION_VIEWSTATE_NAME] = null;
        this.radBtnAddCondition.Enabled = true;
        this.radBtnSaveCondition.Enabled = true;
    }

    private void SelectDocument(string documentId)
    {
        using (ConfigurationMgr mgr = new ConfigurationMgr())
        {
            //Configuration
            DTO_CONFIG config = mgr.SelectConfiguration(documentId);

            if (config != null)
            {
                this.hddConditionIdx.Value = "0";
                this.hddMaxConditionIdx.Value = "0";
                this.hddConditionList.Value = "";
                this.hddDocumentId.Value = config.DOCUMENT_ID;
                this.radTxtDocName.Text = config.DOC_NAME;
                this.radTxtTableName.Text = config.TABLE_NAME;
                this.hddDataOwnerCode.Value = config.DATA_OWNER;
                this.radTxtDataOwner.Text = config.DATA_OWNER_NAME;
                this.radCmbCategory.SelectedValue = config.CATEGORY_CODE;
                this.radCmbReadersGroup.SelectedValue = config.READERS_GROUP_CODE;
                this.radTxtPrefix.Text = config.PREFIX_DOC_NUM;
                this.radCmbInfoClassification.SelectedValue = config.CLASSIFICATION_INFO;
                this.radTxtServiceName.Text = config.AFTER_TREATMENT_SERVICE;
                this.radCmbRetentionPeriod.SelectedValue = config.RETENTION_PERIOD.ToString();
                this.radTxtDescription.Text = config.DOC_DESCRIPTION.Replace("&lt;", "<").Replace("&gt;", ">");
                this.radTxtFormName.Text = config.FORM_NAME.Replace(".aspx", "");

                if (config.FORWARD_YN.Equals("Y")) { radBtnForwardYes.Checked = true; radBtnForwardNo.Checked = false; }
                else { radBtnForwardYes.Checked = false; radBtnForwardNo.Checked = true; }

                if (config.ADD_ADDTIONAL_APPROVER.Equals("Y"))
                {
                    radBtnAddApproverYes.Checked = true; radBtnAddApproverNo.Checked = false;
                    this.radDdlAddApproverPosition.SelectedValue = config.ADD_APPROVER_POSITION;
                    this.radDdlAddApproverPosition.Enabled = true;
                }
                else
                {
                    radBtnAddApproverYes.Checked = false; radBtnAddApproverNo.Checked = true;
                    this.radDdlAddApproverPosition.Enabled = false;
                }

                if (config.DISPLAY_DOC_LIST.Equals("Y")) { radBtnShowDocumentListYes.Checked = true; radBtnShowDocumentListNo.Checked = false; }
                else { radBtnShowDocumentListYes.Checked = false; radBtnShowDocumentListNo.Checked = true; }

                if (config.ADD_REVIEWER.Equals("Y")) { radBtnAddReviewerYes.Checked = true; radBtnAddReviewerNo.Checked = false; }
                else { radBtnAddReviewerYes.Checked = false; radBtnAddReviewerNo.Checked = true; }

                radTxtReviewerDescription.Text = config.ADD_REVIEWER_DESCRIPTION;

                if (config.APPROVAL_TYPE.Equals(((int)ApprovalType.ApprovalLevel).ToString("0000")))
                {
                    this.radBtnApprovalLevel.Checked = true;
                    this.radBtnJobTitle.Checked = false;
                    this.radBtnLimitAmount.Checked = false;
                    this.radTxtApprovalLevel.Text = config.APPROVAL_LEVEL.ToString();
                    this.radTxtApprovalLevel.Enabled = true;
                    foreach (Control control in this.divJobTitle.Controls)
                    {
                        if (control is RadButton)
                            (control as RadButton).Enabled = false;
                    }
                }
                else if (config.APPROVAL_TYPE.Equals(((int)ApprovalType.JobTitle).ToString("0000")))
                {
                    this.radBtnApprovalLevel.Checked = false;
                    this.radBtnJobTitle.Checked = true;
                    this.radBtnLimitAmount.Checked = false;
                    this.radTxtApprovalLevel.Enabled = false;
                    foreach (Control control in this.divJobTitle.Controls)
                    {
                        if (control is RadButton)
                        {
                            (control as RadButton).Enabled = true;
                            if (config.JOB_TITLE_CODE.Equals((control as RadButton).Value))
                            {
                                (control as RadButton).Checked = true;

                            }
                        }
                    }
                }
                else
                {
                    this.radBtnApprovalLevel.Checked = false;
                    this.radBtnJobTitle.Checked = false;
                    this.radBtnLimitAmount.Checked = true;
                }

            }

            //Company조회
            List<DTO_CONFIG_COMPANY> companies = mgr.SelectConfigurationCompany(documentId);
            foreach (Control control in this.divCompany.Controls)
            {
                if (control is RadButton)
                {
                    RadButton btnCompany = (control as RadButton);
                    btnCompany.Checked = false;
                    var company = companies.Where(c => c.COMPANY_CODE == btnCompany.Value);

                    if (company.ToList().Count > 0) btnCompany.Checked = true;
                }
            }
        }
    }

    #endregion

    #region [ Condition Handling ]

    [Serializable]
    private class ConditionData
    {
        public string APPROVAL_LOCATION { get; set; }
        public string IS_MANDATORY { get; set; }
        public int CONDITION_SEQ { get; set; }
        public string FIELD_NAME { get; set; }
        public string CONDITION { get; set; }
        public string VALUE { get; set; }
        public string OPTION { get; set; }
    }

    private string RemoveAndOrString(string strSql)
    {
        string result = strSql.Trim();
        if (result.EndsWith("And"))
            result = result.Substring(0, result.LastIndexOf("And") - 1);
        else if (result.EndsWith("Or"))
            result = result.Substring(0, result.LastIndexOf("Or") - 1);

        return result;
    }

    private string AppendConditionString(string strSqlTemp, ConditionData data)
    {

        ConfigCondition configCodition = (ConfigCondition)Enum.Parse(typeof(ConfigCondition), data.CONDITION);
        string fieldName = data.FIELD_NAME;
        string[] arryFieldName = fieldName.Split(new char[] { ' ' });
        string strValue = string.Empty;
        if (arryFieldName[1].Contains("char")) strValue = "'" + data.VALUE.Trim() + "'";
        else strValue = data.VALUE.Trim();
        switch (configCodition)
        {
            case ConfigCondition.Equals: strSqlTemp += arryFieldName[0] + "=" + strValue;
                break;
            case ConfigCondition.GreaterThan: strSqlTemp += arryFieldName[0] + ">=" + strValue;
                break;
            case ConfigCondition.LessThan: strSqlTemp += arryFieldName[0] + "<" + strValue;
                break;
            case ConfigCondition.StartWith: strSqlTemp += "CHARINDEX(" + strValue + ", " + arryFieldName[0] + ") = 1";
                break;
            case ConfigCondition.NotStartWith: strSqlTemp += "CHARINDEX(" + strValue + ", " + arryFieldName[0] + ") = 0";
                break;
            case ConfigCondition.Include: strSqlTemp += "CHARINDEX(" + strValue + ", " + arryFieldName[0] + ") >= 1";
                break;
            case ConfigCondition.NotInclude: strSqlTemp += "CHARINDEX(" + strValue + ", " + arryFieldName[0] + ") = 0";
                break;
            case ConfigCondition.IsNull: strSqlTemp += arryFieldName[0] + " IS NULL";
                break;
            case ConfigCondition.IsNotNull: strSqlTemp += arryFieldName[0] + " IS NOT NULL";
                break;
        }

        return strSqlTemp;
    }

    private string[] CheckDisplayAndSqlCondition(string userId, List<ConditionData> list)
    {
        string[] values = new string[2];
        //values[0] -> Display Condition
        //values[1] -> Sql Condition;
        if (list.Count < 1) return values;
        string beforeOption = string.Empty;


        string strSql = string.Empty;
        string strSqlTemp = string.Empty;
        strSqlTemp = AppendConditionString(strSqlTemp, list[0]);
        if (list.Count == 1)
            strSql = "(" + strSqlTemp + ") ";
        list.RemoveAt(0);
        if (list.Count > 0)
        {
            beforeOption = list[0].OPTION.Trim();
        }
        int index = 0;
        foreach (ConditionData data in list)
        {

            if (data.OPTION.Trim().Equals(beforeOption))
            {

                if (list.Count - 1 == index) //Option이 변하지 않은경우
                {
                    strSqlTemp += (" " + data.OPTION + " ");
                    strSqlTemp = AppendConditionString(strSqlTemp, data);
                    strSql += "(" + strSqlTemp + ") ";
                }
                else
                {
                    strSqlTemp += (" " + data.OPTION + " ");
                    strSqlTemp = AppendConditionString(strSqlTemp, data);

                }

            }
            else
            {
                strSql += "(" + strSqlTemp + ") ";
                strSql += (" " + data.OPTION + " ");
                strSqlTemp = string.Empty;
                strSqlTemp = AppendConditionString(strSqlTemp, data);
                if (list.Count - 1 == index)
                    strSql += "(" + strSqlTemp + ") ";
            }
            beforeOption = data.OPTION.Trim();
            index++;
        }
        //foreach (ConditionData data in list)
        //{
        //    if (data.CONDITION_SEQ == excludeSeq)
        //    {
        //        if (list.Count - 1 == index) //Option이 변하지 않은경우
        //        {
        //            //삭제처리 때문에 And/Or가 붙어있는경우가 있다.
        //            strSqlTemp = RemoveAndOrString(strSqlTemp);
        //            strSql += "(" + strSqlTemp + ") ";
        //        }
        //        beforeOption = data.OPTION.Trim();
        //        index++;
        //        continue;
        //    }
        //    if (data.OPTION.Trim().Equals(beforeOption))
        //    {
        //        strSqlTemp = AppendConditionString(strSqlTemp, data);


        //        if (list.Count - 1 == index) //Option이 변하지 않은경우
        //            strSql += "(" + strSqlTemp + ") ";
        //        else
        //            strSqlTemp += " " + data.OPTION.Trim() + " ";

        //    }
        //    else
        //    {
        //        strSqlTemp = AppendConditionString(strSqlTemp, data);

        //        strSql += "(" + strSqlTemp + ") " + (list.Count - 1 > index ? data.OPTION.Trim() : "");
        //        strSqlTemp = string.Empty;
        //    }
        //    beforeOption = data.OPTION.Trim();
        //    index++;
        //}

        values[0] = "if (" + strSql + ") then '" + userId + "'";
        values[1] = "CASE WHEN " + strSql + " THEN '" + userId + "' END";

        return values;
    }

    /// <summary>
    /// Approver Condition저장
    /// </summary>
    /// <param name="conditions"></param>
    private void SaveApproverCondition(List<ConditionData> conditions)
    {
        DTO_CONFIG_APPROVER approver = new DTO_CONFIG_APPROVER();
        List<DTO_CONFIG_APPROVER_CONDITION> listCondition = new List<DTO_CONFIG_APPROVER_CONDITION>();

        int index = 1;
        int seq = 1;
        if (string.IsNullOrEmpty(this.hddConditionIdx.Value) || this.hddConditionIdx.Value.Equals("0")) //신규
        {
            if (this.radDdlApprovalType.SelectedValue.Equals("B"))
            {
                if (this.radGrdBeforeApprover.MasterTableView.Items.Count > 0)
                    index = Convert.ToInt32(this.radGrdBeforeApprover.MasterTableView.Items[0]["MAX_CONDITION_INDEX"].Text) + 1;
            }
            else
            {
                if (this.radGrdAfterApprover.MasterTableView.Items.Count > 0)
                    index = Convert.ToInt32(this.radGrdAfterApprover.MasterTableView.Items[0]["MAX_CONDITION_INDEX"].Text) + 1;
            }
        }
        else
            index = Convert.ToInt32(this.hddConditionIdx.Value);

        if (this.radBtnIsMandatory.Checked) //Is Mandatory
        {
            ConditionData data = conditions[0];

            approver.DOCUMENT_ID = this.hddDocumentId.Value;
            approver.CONDITION_INDEX = index;
            approver.IS_MANDATORY = "Y";
            approver.APPROVAL_LOCATION = this.radDdlApprovalType.SelectedValue;
            approver.APPROVER_ID = this.hddApprovalUserCode.Value;
            approver.APPROVER_NAME = this.radTxtUserId.Text;
            approver.DISPLAY_CONDITION = this.radTxtUserId.Text + "(Mandatory)";
            approver.SQL_CONDITION = "CASE WHEN 1 = 1 THEN '" + this.hddApprovalUserCode.Value + "' END";
            approver.CREATOR_ID = Sessions.UserID;
        }
        else
        {
            foreach (ConditionData data in conditions)
            {
                DTO_CONFIG_APPROVER_CONDITION condition = new DTO_CONFIG_APPROVER_CONDITION();
                condition.DOCUMENT_ID = this.hddDocumentId.Value;
                condition.CONDITION_INDEX = index;
                condition.CONDITION_SEQ = seq;
                condition.APPROVAL_LOCATION = this.radDdlApprovalType.SelectedValue;
                condition.FIELD_NAME = data.FIELD_NAME.Split(new char[] { ' ' })[0];
                condition.CONDITION = data.CONDITION;
                condition.VALUE = data.VALUE;
                condition.OPTION = data.OPTION;
                condition.CREATOR_ID = Sessions.UserID;

                listCondition.Add(condition);
                seq++;
            }

            string[] results = CheckDisplayAndSqlCondition(this.hddApprovalUserCode.Value, conditions);

            approver.DOCUMENT_ID = this.hddDocumentId.Value;
            approver.CONDITION_INDEX = index;
            approver.IS_MANDATORY = "N";
            approver.APPROVAL_LOCATION = this.radDdlApprovalType.SelectedValue;
            approver.APPROVER_ID = this.hddApprovalUserCode.Value;
            approver.APPROVER_NAME = this.radTxtUserId.Text;
            approver.DISPLAY_CONDITION = results[0];
            approver.SQL_CONDITION = results[1];
            approver.CREATOR_ID = Sessions.UserID;

        }
        using (ConfigurationMgr mgr = new ConfigurationMgr())
        {
            mgr.MergeConfigurationApprover(approver, listCondition);
        }
    }

    /// <summary>
    /// Recipient Condition저장
    /// </summary>
    /// <param name="conditions"></param>
    private void SaveRecipientCondition(List<ConditionData> conditions)
    {
        DTO_CONFIG_RECIPIENT recipient = new DTO_CONFIG_RECIPIENT();
        List<DTO_CONFIG_RECIPIENT_CONDITION> listCondition = new List<DTO_CONFIG_RECIPIENT_CONDITION>();

        int index = 1;
        int seq = 1;
        if (string.IsNullOrEmpty(this.hddConditionIdx.Value) || this.hddConditionIdx.Value.Equals("0")) //신규
        {
            if (this.radGrdRecipient.MasterTableView.Items.Count > 0)
                index = Convert.ToInt32(this.radGrdRecipient.MasterTableView.Items[0]["MAX_CONDITION_INDEX"].Text) + 1;
        }
        else
            index = Convert.ToInt32(this.hddConditionIdx.Value);

        if (this.radBtnIsMandatory.Checked) //Is Mandatory
        {
            ConditionData data = conditions[0];

            recipient.DOCUMENT_ID = this.hddDocumentId.Value;
            recipient.CONDITION_INDEX = index;
            recipient.IS_MANDATORY = "Y";
            recipient.RECIPIENT_ID = this.hddApprovalUserCode.Value;
            recipient.RECIPIENT_NAME = this.radTxtUserId.Text;
            recipient.DISPLAY_CONDITION = this.radTxtUserId.Text + "(Mandatory)";
            recipient.SQL_CONDITION = "CASE WHEN 1 = 1 THEN '" + this.hddApprovalUserCode.Value + "' END";
            recipient.CREATOR_ID = Sessions.UserID;
        }
        else
        {
            foreach (ConditionData data in conditions)
            {
                DTO_CONFIG_RECIPIENT_CONDITION condition = new DTO_CONFIG_RECIPIENT_CONDITION();
                condition.DOCUMENT_ID = this.hddDocumentId.Value;
                condition.CONDITION_INDEX = index;
                condition.CONDITION_SEQ = seq;
                condition.FIELD_NAME = data.FIELD_NAME.Split(new char[] { ' ' })[0];
                condition.CONDITION = data.CONDITION;
                condition.VALUE = data.VALUE;
                condition.OPTION = data.OPTION;
                condition.CREATOR_ID = Sessions.UserID;

                listCondition.Add(condition);
                seq++;
            }

            string[] results = CheckDisplayAndSqlCondition(this.hddApprovalUserCode.Value, conditions);

            recipient.DOCUMENT_ID = this.hddDocumentId.Value;
            recipient.CONDITION_INDEX = index;
            recipient.IS_MANDATORY = "N";
            recipient.RECIPIENT_ID = this.hddApprovalUserCode.Value;
            recipient.RECIPIENT_NAME = this.radTxtUserId.Text;
            recipient.DISPLAY_CONDITION = results[0];
            recipient.SQL_CONDITION = results[1];
            recipient.CREATOR_ID = Sessions.UserID;

        }
        using (ConfigurationMgr mgr = new ConfigurationMgr())
        {
            mgr.MergeConfigurationRecipient(recipient, listCondition);
        }
    }

    /// <summary>
    /// Reviewer Condition저장
    /// </summary>
    /// <param name="conditions"></param>
    private void SaveReviewerCondition(List<ConditionData> conditions)
    {
        DTO_CONFIG_REVIEWER reviewer = new DTO_CONFIG_REVIEWER();
        List<DTO_CONFIG_REVIEWER_CONDITION> listCondition = new List<DTO_CONFIG_REVIEWER_CONDITION>();

        int index = 1;
        int seq = 1;
        if (string.IsNullOrEmpty(this.hddConditionIdx.Value) || this.hddConditionIdx.Value.Equals("0")) //신규
        {
            if (this.radGrdReviewer.MasterTableView.Items.Count > 0)
                index = Convert.ToInt32(this.radGrdReviewer.MasterTableView.Items[0]["MAX_CONDITION_INDEX"].Text) + 1;
        }
        else
            index = Convert.ToInt32(this.hddConditionIdx.Value);

        if (this.radBtnIsMandatory.Checked) //Is Mandatory
        {
            ConditionData data = conditions[0];

            reviewer.DOCUMENT_ID = this.hddDocumentId.Value;
            reviewer.CONDITION_INDEX = index;
            reviewer.IS_MANDATORY = "Y";
            reviewer.REVIEWER_ID = this.hddApprovalUserCode.Value;
            reviewer.REVIEWER_NAME = this.radTxtUserId.Text;
            reviewer.DISPLAY_CONDITION = this.radTxtUserId.Text + "(Mandatory)";
            reviewer.SQL_CONDITION = "CASE WHEN 1 = 1 THEN '" + this.hddApprovalUserCode.Value + "' END";
            reviewer.CREATOR_ID = Sessions.UserID;
        }
        else
        {
            foreach (ConditionData data in conditions)
            {
                DTO_CONFIG_REVIEWER_CONDITION condition = new DTO_CONFIG_REVIEWER_CONDITION();
                condition.DOCUMENT_ID = this.hddDocumentId.Value;
                condition.CONDITION_INDEX = index;
                condition.CONDITION_SEQ = seq;
                condition.FIELD_NAME = data.FIELD_NAME.Split(new char[] { ' ' })[0];
                condition.CONDITION = data.CONDITION;
                condition.VALUE = data.VALUE;
                condition.OPTION = data.OPTION;
                condition.CREATOR_ID = Sessions.UserID;

                listCondition.Add(condition);
                seq++;
            }

            string[] results = CheckDisplayAndSqlCondition(this.hddApprovalUserCode.Value, conditions);

            reviewer.DOCUMENT_ID = this.hddDocumentId.Value;
            reviewer.CONDITION_INDEX = index;
            reviewer.IS_MANDATORY = "N";
            reviewer.REVIEWER_ID = this.hddApprovalUserCode.Value;
            reviewer.REVIEWER_NAME = this.radTxtUserId.Text;
            reviewer.DISPLAY_CONDITION = results[0];
            reviewer.SQL_CONDITION = results[1];
            reviewer.CREATOR_ID = Sessions.UserID;

        }
        using (ConfigurationMgr mgr = new ConfigurationMgr())
        {
            mgr.MergeConfigurationReviewer(reviewer, listCondition);
        }
    }

    /// <summary>
    /// 조건 저장
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>


    #endregion

    #region [ 최상단 버튼 이벤트 ]

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            using (ConfigurationMgr mgr = new ConfigurationMgr())
            {
                if (!mgr.SelectExistsTable(this.radTxtTableName.Text))
                {
                    this.informationMessage = "테이블을 확인바랍니다.";
                    return;
                }


                DTO_CONFIG config = new DTO_CONFIG();
                if (string.IsNullOrEmpty(this.hddDocumentId.Value)) //신규
                    config.DOCUMENT_ID = mgr.CreateDocumentId();
                else
                    config.DOCUMENT_ID = this.hddDocumentId.Value;

                config.DOC_NAME = this.radTxtDocName.Text;
                config.TABLE_NAME = this.radTxtTableName.Text.ToUpper();
                config.DATA_OWNER = this.hddDataOwnerCode.Value;
                config.PREFIX_DOC_NUM = this.radTxtPrefix.Text;
                config.FORM_NAME = this.radTxtFormName.Text + ".aspx";
                config.READERS_GROUP_CODE = "";
                config.CATEGORY_CODE = this.radCmbCategory.SelectedValue;
                config.RETENTION_PERIOD = this.radCmbRetentionPeriod.SelectedValue;
                config.FORWARD_YN = this.radBtnForwardYes.Checked ? "Y" : "N";
                config.CLASSIFICATION_INFO = this.radCmbInfoClassification.SelectedValue;
                config.AFTER_TREATMENT_SERVICE = this.radTxtServiceName.Text;
                //ApprovalType
                if (this.radBtnApprovalLevel.Checked) config.APPROVAL_TYPE = ((int)ApprovalType.ApprovalLevel).ToString("0000");
                else if (this.radBtnJobTitle.Checked) config.APPROVAL_TYPE = ((int)ApprovalType.JobTitle).ToString("0000");
                else config.APPROVAL_TYPE = ((int)ApprovalType.LimitAmount).ToString("0000");

                config.APPROVAL_LEVEL = this.radBtnApprovalLevel.Checked ? Convert.ToInt32(this.radTxtApprovalLevel.Text) : 0;
                if (this.radBtnJobTitle.Checked)
                {
                    foreach (Control control in this.divJobTitle.Controls)
                    {
                        if (control is RadButton)
                        {
                            if (((RadButton)control).Checked)
                                config.JOB_TITLE_CODE = ((RadButton)control).Value;
                        }
                    }
                }
                else
                    config.JOB_TITLE_CODE = null;

                config.ADD_ADDTIONAL_APPROVER = this.radBtnAddApproverYes.Checked ? "Y" : "N";
                if (this.radBtnAddApproverYes.Checked)
                    config.ADD_APPROVER_POSITION = this.radDdlAddApproverPosition.SelectedValue;

                config.ADD_REVIEWER = this.radBtnAddReviewerYes.Checked ? "Y" : "N";
                if (this.radBtnAddReviewerYes.Checked) config.ADD_REVIEWER_DESCRIPTION = this.radTxtReviewerDescription.Text;
                else config.ADD_REVIEWER_DESCRIPTION = string.Empty;

                config.DOC_DESCRIPTION = this.radTxtDescription.Text;
                config.DOC_IMAGE_NAME = null;
                config.DOC_IMAGE_PATH = null;
                config.DISPLAY_DOC_LIST = this.radBtnShowDocumentListYes.Checked ? "Y" : "N";

                config.CREATOR_ID = Sessions.UserID;

                List<DTO_CONFIG_COMPANY> companies = new List<DTO_CONFIG_COMPANY>();

                foreach (Control control in this.divCompany.Controls)
                {
                    if (control is RadButton)
                    {
                        if ((control as RadButton).Checked)
                        {
                            DTO_CONFIG_COMPANY company = new DTO_CONFIG_COMPANY();
                            company.DOCUMENT_ID = config.DOCUMENT_ID;
                            company.COMPANY_CODE = (control as RadButton).Value;
                            company.CREATOR_ID = Sessions.UserID;

                            companies.Add(company);
                        }
                    }
                }

                mgr.MergeConfiguration(config, companies);
                //this.informationMessage = "결재문서 기본설정이 저장되었습니다.";
                if (!ClientScript.IsStartupScriptRegistered("ShowInformation"))
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowInformation", "ShowInformation('결재문서 기본설정이 저장되었습니다.');", true);


            }
            InitInputScreen();
            BindingDocumentList();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.Message;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        using (ConfigurationMgr mgr = new ConfigurationMgr())
        {
            mgr.DeleteConfiguration(this.hddDocumentId.Value);
        }
        BindingDocumentList();
        //this.informationMessage = "결재문서 설정정보 전체를 삭제했습니다.";
        if (!ClientScript.IsStartupScriptRegistered("ShowInformation"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowInformation", "ShowInformation('결재문서 설정정보 전체를 삭제했습니다.');", true);
    }

    #endregion

    #region [ GridCondition Event ]

    protected void radGrdCondition_PreRender(object sender, EventArgs e)
    {

        if (this.radGrdCondition.FindControl(this.radGrdCondition.MasterTableView.ClientID + "_FIELD_NAME") != null)
        {
            RadDropDownList ddlDesc = this.radGrdCondition.FindControl(this.radGrdCondition.MasterTableView.ClientID + "_FIELD_NAME").FindControl("radGrdDdlFieldName") as RadDropDownList;

            using (ConfigurationMgr mgr = new ConfigurationMgr())
            {
                List<DbTableColumnDto> columns = mgr.SelectDbTableColumn(this.radTxtTableName.Text);
                ddlDesc.DataTextField = "ColumnName";
                ddlDesc.DataValueField = "ColumnId";

                ddlDesc.DataSource = columns;
                ddlDesc.DataBind();
            }

        }

    }

    protected void radGrdCondition_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (IsPostBack)
        {
            //ViewState[CONDITION_VIEWSTATE_NAME] = this.lstCondition;
            this.lstCondition = (List<ConditionData>)ViewState[CONDITION_VIEWSTATE_NAME];
            this.radGrdCondition.DataSource = this.lstCondition;
        }

    }

    protected void radGrdCondition_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Remove")
            {

                GridDataItem item = e.Item as GridDataItem;
                

                int seqCondition = Convert.ToInt32(e.CommandArgument);


                List<ConditionData> list = (List<ConditionData>)ViewState[CONDITION_VIEWSTATE_NAME];
                List<ConditionData> listNew = new List<ConditionData>();
                foreach (ConditionData data in list)
                {
                    if (data.CONDITION_SEQ == seqCondition) continue;
                    listNew.Add(data);
                }

                string[] results = CheckDisplayAndSqlCondition(this.hddApprovalUserCode.Value, listNew);
                if (seqCondition > 0)
                {
                    using (ConfigurationMgr mgr = new ConfigurationMgr())
                    {
                        string documentId = this.hddDocumentId.Value;
                        int index = Convert.ToInt32(this.hddConditionIdx.Value);

                        switch (this.radDdlApprovalType.SelectedIndex)
                        {
                            case 0:
                                DTO_CONFIG_APPROVER approverBefore = new DTO_CONFIG_APPROVER();
                                approverBefore.DOCUMENT_ID = this.hddDocumentId.Value;
                                approverBefore.CONDITION_INDEX = index;
                                approverBefore.IS_MANDATORY = "N";
                                approverBefore.APPROVAL_LOCATION = this.radDdlApprovalType.SelectedValue;
                                approverBefore.APPROVER_ID = this.hddApprovalUserCode.Value;
                                approverBefore.APPROVER_NAME = this.radTxtUserId.Text;
                                approverBefore.DISPLAY_CONDITION = results[0];
                                approverBefore.SQL_CONDITION = results[1];
                                approverBefore.CREATOR_ID = Sessions.UserID;
                                mgr.DeleteConfigurationApproverCondition(approverBefore, documentId, "B", index, seqCondition);
                                break;
                            case 1:
                                DTO_CONFIG_APPROVER approverAfter = new DTO_CONFIG_APPROVER();
                                approverAfter.DOCUMENT_ID = this.hddDocumentId.Value;
                                approverAfter.CONDITION_INDEX = index;
                                approverAfter.IS_MANDATORY = "N";
                                approverAfter.APPROVAL_LOCATION = this.radDdlApprovalType.SelectedValue;
                                approverAfter.APPROVER_ID = this.hddApprovalUserCode.Value;
                                approverAfter.APPROVER_NAME = this.radTxtUserId.Text;
                                approverAfter.DISPLAY_CONDITION = results[0];
                                approverAfter.SQL_CONDITION = results[1];
                                approverAfter.CREATOR_ID = Sessions.UserID;
                                mgr.DeleteConfigurationApproverCondition(approverAfter, documentId, "A", index, seqCondition);
                                break;
                            case 2:
                                DTO_CONFIG_RECIPIENT recipient = new DTO_CONFIG_RECIPIENT();
                                recipient.DOCUMENT_ID = this.hddDocumentId.Value;
                                recipient.CONDITION_INDEX = index;
                                recipient.IS_MANDATORY = "N";
                                recipient.RECIPIENT_ID = this.hddApprovalUserCode.Value;
                                recipient.RECIPIENT_NAME = this.radTxtUserId.Text;
                                recipient.DISPLAY_CONDITION = results[0];
                                recipient.SQL_CONDITION = results[1];
                                recipient.CREATOR_ID = Sessions.UserID;
                                mgr.DeleteConfigurationRecipientCondition(recipient, documentId, index, seqCondition);
                                break;
                            case 3:
                                DTO_CONFIG_REVIEWER reviewer = new DTO_CONFIG_REVIEWER();
                                reviewer.DOCUMENT_ID = this.hddDocumentId.Value;
                                reviewer.CONDITION_INDEX = index;
                                reviewer.IS_MANDATORY = "N";
                                reviewer.REVIEWER_ID = this.hddApprovalUserCode.Value;
                                reviewer.REVIEWER_NAME = this.radTxtUserId.Text;
                                reviewer.DISPLAY_CONDITION = results[0];
                                reviewer.SQL_CONDITION = results[1];
                                reviewer.CREATOR_ID = Sessions.UserID;
                                mgr.DeleteConfigurationReviewerCondition(reviewer, documentId, index, seqCondition);
                                break;
                        }
                        SelectCondition(documentId);
                    }
                }

            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    #endregion

    #region [ Before Approver Grid Event ]

    protected void radGrdBeforeApprover_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.radGrdBeforeApprover.SelectedItems.Count > 0)
        {
            GridDataItem item = this.radGrdBeforeApprover.SelectedItems[0] as GridDataItem;
            if (item != null)
            {
                string documentId = this.hddDocumentId.Value;
                string location = item["APPROVAL_LOCATION"].Text;
                int index = Convert.ToInt32(item["CONDITION_INDEX"].Text);
                string mandatory = item["IS_MANDATORY"].Text;
                string approverId = item["APPROVER_ID"].Text;
                string approverName = item["APPROVER_NAME"].Text;
                int maxIdx = Convert.ToInt32(item["MAX_CONDITION_INDEX"].Text);

                this.hddConditionIdx.Value = index.ToString();
                this.hddMaxConditionIdx.Value = maxIdx.ToString();
                this.radDdlApprovalType.SelectedValue = location;
                this.radTxtUserId.Text = approverName;
                this.hddApprovalUserCode.Value = approverId;
                this.radBtnIsMandatory.Checked = (mandatory == "Y") ? true : false; ;

                using (ConfigurationMgr mgr = new ConfigurationMgr())
                {
                    List<ConditionData> lstCondition = new List<ConditionData>();
                    foreach (DTO_CONFIG_APPROVER_CONDITION condition in mgr.SelectConfigApproverCondition(documentId, location, index))
                    {
                        ConditionData data = new ConditionData();
                        data.APPROVAL_LOCATION = location;
                        data.IS_MANDATORY = mandatory;
                        data.CONDITION_SEQ = condition.CONDITION_SEQ;
                        data.FIELD_NAME = condition.FIELD_NAME;
                        data.CONDITION = condition.CONDITION;
                        data.VALUE = condition.VALUE;
                        data.OPTION = condition.OPTION;
                        lstCondition.Add(data);
                    }

                    ViewState[CONDITION_VIEWSTATE_NAME] = lstCondition;
                    this.radGrdCondition.Rebind();

                    this.radGrdAfterApprover.SelectedIndexes.Clear();
                    this.radGrdRecipient.SelectedIndexes.Clear();
                    this.radGrdReviewer.SelectedIndexes.Clear();
                }

            }
        }
    }

    protected void radGrdBeforeApprover_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            try
            {
                string documentId = this.hddDocumentId.Value;
                int index = Convert.ToInt32(e.CommandArgument);
                using (ConfigurationMgr mgr = new ConfigurationMgr())
                {
                    mgr.DeleteConfigurationApprover(documentId, this.radDdlApprovalType.SelectedValue, index);
                }

                //Before Approver재조회
                SelectBeforeApproverCondition(documentId);
                //condition grid초기화
                InitCondition();
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.ToString();
            }
        }

    }

    protected void radGrdBeforeApprover_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //GridDataItem dataBoundItem = e.Item as GridDataItem;
            //string displayCondition = dataBoundItem["DISPLAY_CONDITION"].Text;
            //displayCondition.Replace("Mandatory", "<font color='blue'>Mandatory</font>");
            //displayCondition.Replace("if", "<font color='blue'>if</font>").Replace("then", "<font color='blue'>then</font>");
            //dataBoundItem["DISPLAY_CONDITION"].Text = displayCondition;
            //string sqlCondition = dataBoundItem["SQL_CONDITION"].Text;
            //sqlCondition.Replace("CASE", "<font color='sliver'>CASE</font>");
            //sqlCondition.Replace("WHEN", "<font color='sliver'>WHEN</font>").Replace("THEN", "<font color='sliver'>THEN</font>");
            //dataBoundItem["SQL_CONDITION"].Text = dataBoundItem["SQL_CONDITION"].Text.Replace("CASE", "<font color='sliver'>CASE</font>").Replace("WHEN", "<font color='sliver'>WHEN</font>").Replace("THEN", "<font color='sliver'>THEN</font>"); ;
        }
    }

    #endregion

    //private List<ConditionData> AddDataRow(List<DTO_CONFIG_APPROVER_CONDITION> list)
    //{
    //    List<ConditionData> lstCondition = new List<ConditionData>();
    //    foreach (DTO_CONFIG_APPROVER_CONDITION condition in list)
    //    {
    //        ConditionData data = new ConditionData();
    //        data.APPROVAL_LOCATION = condition.APPROVAL_LOCATION;
    //        data.IS_MANDATORY = "N";
    //        data.CONDITION_SEQ = condition.CONDITION_SEQ;
    //        data.FIELD_NAME = condition.FIELD_NAME;
    //        data.CONDITION = condition.CONDITION;
    //        data.VALUE = condition.VALUE;
    //        data.OPTION = condition.OPTION;
    //        lstCondition.Add(data);
    //    }

    //    return lstCondition;

    //}

    #region [ After Approver Grid Event ]
    protected void radGrdAfterApprover_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.radGrdAfterApprover.SelectedItems.Count > 0)
        {

            GridDataItem item = this.radGrdAfterApprover.SelectedItems[0] as GridDataItem;
            if (item != null)
            {
                string documentId = this.hddDocumentId.Value;
                string location = item["APPROVAL_LOCATION"].Text;
                int index = Convert.ToInt32(item["CONDITION_INDEX"].Text);
                string mandatory = item["IS_MANDATORY"].Text;
                string approverid = item["APPROVER_ID"].Text;
                string approverName = item["APPROVER_NAME"].Text;
                int maxIdx = Convert.ToInt32(item["MAX_CONDITION_INDEX"].Text);

                this.hddConditionIdx.Value = index.ToString();
                this.hddMaxConditionIdx.Value = maxIdx.ToString();
                this.radDdlApprovalType.SelectedValue = location;
                this.radTxtUserId.Text = approverName;
                this.hddApprovalUserCode.Value = approverid;
                this.radBtnIsMandatory.Checked = (mandatory == "Y") ? true : false;

                using (ConfigurationMgr mgr = new ConfigurationMgr())
                {
                    List<ConditionData> lstCondition = new List<ConditionData>();
                    foreach (DTO_CONFIG_APPROVER_CONDITION condition in mgr.SelectConfigApproverCondition(documentId, location, index))
                    {
                        ConditionData data = new ConditionData();
                        data.APPROVAL_LOCATION = location;
                        data.IS_MANDATORY = mandatory;
                        data.CONDITION_SEQ = condition.CONDITION_SEQ;
                        data.FIELD_NAME = condition.FIELD_NAME;
                        data.CONDITION = condition.CONDITION;
                        data.VALUE = condition.VALUE;
                        data.OPTION = condition.OPTION;
                        lstCondition.Add(data);
                    }

                    ViewState[CONDITION_VIEWSTATE_NAME] = lstCondition;
                    this.radGrdCondition.Rebind();
                    this.radGrdBeforeApprover.SelectedIndexes.Clear();
                    this.radGrdRecipient.SelectedIndexes.Clear();
                    this.radGrdReviewer.SelectedIndexes.Clear();
                }

            }
        }
    }

    protected void radGrdAfterApprover_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            try
            {
                string documentId = this.hddDocumentId.Value;
                int index = Convert.ToInt32(e.CommandArgument);
                using (ConfigurationMgr mgr = new ConfigurationMgr())
                {
                    mgr.DeleteConfigurationApprover(documentId, this.radDdlApprovalType.SelectedValue, index);
                }

                SelectAfterApproverCondition(documentId);
                //condition grid초기화
                InitCondition();
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.ToString();
            }
        }

    }

    #endregion

    #region [ Recipient Grid Event ]

    protected void radGrdRecipient_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.radGrdRecipient.SelectedItems.Count > 0)
        {
            GridDataItem item = this.radGrdRecipient.SelectedItems[0] as GridDataItem;
            if (item != null)
            {
                string documentId = this.hddDocumentId.Value;
                int index = Convert.ToInt32(item["CONDITION_INDEX"].Text);
                string mandatory = item["IS_MANDATORY"].Text;
                string recipientId = item["RECIPIENT_ID"].Text;
                string recipientName = item["RECIPIENT_NAME"].Text;
                int maxIdx = Convert.ToInt32(item["MAX_CONDITION_INDEX"].Text);


                this.hddConditionIdx.Value = index.ToString();
                this.hddMaxConditionIdx.Value = maxIdx.ToString();
                this.radDdlApprovalType.SelectedValue = "R";
                this.radTxtUserId.Text = recipientName;
                this.hddApprovalUserCode.Value = recipientId;
                this.radBtnIsMandatory.Checked = (mandatory == "Y") ? true : false;

                using (ConfigurationMgr mgr = new ConfigurationMgr())
                {
                    List<ConditionData> lstCondition = new List<ConditionData>();
                    foreach (DTO_CONFIG_RECIPIENT_CONDITION condition in mgr.SelectConfigRecipientCondition(documentId, index))
                    {
                        ConditionData data = new ConditionData();
                        data.APPROVAL_LOCATION = "R";
                        data.IS_MANDATORY = mandatory;
                        data.CONDITION_SEQ = condition.CONDITION_SEQ;
                        data.FIELD_NAME = condition.FIELD_NAME;
                        data.CONDITION = condition.CONDITION;
                        data.VALUE = condition.VALUE;
                        data.OPTION = condition.OPTION;
                        lstCondition.Add(data);
                    }

                    ViewState[CONDITION_VIEWSTATE_NAME] = lstCondition;
                    this.radGrdCondition.Rebind();

                    this.radGrdBeforeApprover.SelectedIndexes.Clear();
                    this.radGrdAfterApprover.SelectedIndexes.Clear();
                    this.radGrdReviewer.SelectedIndexes.Clear();

                }
            }
        }
    }

    protected void radGrdRecipient_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            try
            {
                string documentId = this.hddDocumentId.Value;
                int index = Convert.ToInt32(e.CommandArgument);
                using (ConfigurationMgr mgr = new ConfigurationMgr())
                {
                    mgr.DeleteConfigurationRecipient(documentId, index);
                }

                SelectRecipientCondition(documentId);
                //condition grid초기화
                InitCondition();
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.ToString();
            }
        }

    }

    #endregion

    #region [ Reviewer Grid Event ]

    protected void radGrdReviewer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.radGrdReviewer.SelectedItems.Count > 0)
        {
            GridDataItem item = this.radGrdReviewer.SelectedItems[0] as GridDataItem;
            if (item != null)
            {
                string documentId = this.hddDocumentId.Value;
                int index = Convert.ToInt32(item["CONDITION_INDEX"].Text);
                string mandatory = item["IS_MANDATORY"].Text;
                string reviewerId = item["REVIEWER_ID"].Text;
                string reviewerName = item["REVIEWER_NAME"].Text;
                this.hddConditionIdx.Value = index.ToString();
                this.radDdlApprovalType.SelectedValue = "V";
                this.radTxtUserId.Text = reviewerName;
                this.hddApprovalUserCode.Value = reviewerId;
                this.radBtnIsMandatory.Checked = (mandatory == "Y") ? true : false;

                using (ConfigurationMgr mgr = new ConfigurationMgr())
                {
                    List<ConditionData> lstCondition = new List<ConditionData>();
                    foreach (DTO_CONFIG_REVIEWER_CONDITION condition in mgr.SelectConfigReviewerCondition(documentId, index))
                    {
                        ConditionData data = new ConditionData();
                        data.APPROVAL_LOCATION = "V";
                        data.IS_MANDATORY = mandatory;
                        data.CONDITION_SEQ = condition.CONDITION_SEQ;
                        data.FIELD_NAME = condition.FIELD_NAME;
                        data.CONDITION = condition.CONDITION;
                        data.VALUE = condition.VALUE;
                        data.OPTION = condition.OPTION;
                        lstCondition.Add(data);
                    }
                    ViewState[CONDITION_VIEWSTATE_NAME] = lstCondition;
                    this.radGrdCondition.Rebind();

                    this.radGrdBeforeApprover.SelectedIndexes.Clear();
                    this.radGrdAfterApprover.SelectedIndexes.Clear();
                    this.radGrdRecipient.SelectedIndexes.Clear();

                }
            }
        }
    }

    protected void radGrdReviewer_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            try
            {
                string documentId = this.hddDocumentId.Value;
                int index = Convert.ToInt32(e.CommandArgument);
                using (ConfigurationMgr mgr = new ConfigurationMgr())
                {
                    mgr.DeleteConfigurationReviewer(documentId, index);
                }

                SelectReviewerCondition(documentId);
                //condition grid초기화
                InitCondition();
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.ToString();
            }
        }

    }

    #endregion

    #region [ New/Save/Add Button Event ]
    protected void radBtnSaveCondition_Click(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(this.hddConditionList.Value)) return;
        this.lstCondition = (List<ConditionData>)ViewState[CONDITION_VIEWSTATE_NAME];
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<ConditionData> newData = (List<ConditionData>)serializer.Deserialize<List<ConditionData>>(this.hddConditionList.Value);

        if (this.radBtnIsMandatory.Checked)
        {
            this.lstCondition = newData;
        }
        else
        {
            foreach (ConditionData data in newData)
            {
                for (int i = 0; i < this.lstCondition.Count; i++)
                {
                    if (data.CONDITION_SEQ == this.lstCondition[i].CONDITION_SEQ)
                    {
                        this.lstCondition[i].FIELD_NAME = data.FIELD_NAME;
                        this.lstCondition[i].CONDITION = data.CONDITION;
                        this.lstCondition[i].VALUE = data.VALUE;
                        this.lstCondition[i].OPTION = data.OPTION;
                    }
                }
            }
        }
        switch (this.radDdlApprovalType.SelectedValue)
        {
            case "R":
                SaveRecipientCondition(this.lstCondition);
                break;
            case "V":
                SaveReviewerCondition(this.lstCondition);
                break;
            default:
                SaveApproverCondition(this.lstCondition);
                break;
        }

        SelectCondition(this.hddDocumentId.Value);

        //초기화
        this.radTxtUserId.Text = "";
        ViewState[CONDITION_VIEWSTATE_NAME] = null;
        this.hddConditionIdx.Value = "0";
        this.hddMaxConditionIdx.Value = "0";
        this.hddConditionList.Value = "";
        this.radTxtValue.Text = "";
        this.radDdlCondition.SelectedIndex = 0;

    }

    protected void radBtnAddCondition_Click(object sender, EventArgs e)
    {
        //List<ConditionData> lstConditoin = (List<ConditionData>)ViewState[CONDITION_VIEWSTATE_NAME];

        this.lstCondition = (List<ConditionData>)ViewState[CONDITION_VIEWSTATE_NAME];
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<ConditionData> newData = (List<ConditionData>)serializer.Deserialize<List<ConditionData>>(this.hddConditionList.Value);

        foreach (ConditionData data in newData)
        {
            for (int i = 0; i < this.lstCondition.Count; i++)
            {
                if (data.CONDITION_SEQ == this.lstCondition[i].CONDITION_SEQ)
                {
                    this.lstCondition[i].FIELD_NAME = data.FIELD_NAME;
                    this.lstCondition[i].CONDITION = data.CONDITION;
                    this.lstCondition[i].VALUE = data.VALUE;
                    this.lstCondition[i].OPTION = data.OPTION;
                }
            }
        }


        var lastData = this.lstCondition.OrderByDescending(d => d.CONDITION_SEQ).FirstOrDefault();
        int newSeq = 1;
        if (lastData != null)
        {
            newSeq = Convert.ToInt32(lastData.CONDITION_SEQ);
            newSeq++;
        }


        ConditionData condition = new ConditionData();
        condition.CONDITION_SEQ = newSeq;
        condition.FIELD_NAME = this.radDdlFieldName.SelectedText;
        condition.CONDITION = this.radDdlCondition.SelectedValue;
        condition.VALUE = this.radTxtValue.Text;
        condition.OPTION = this.radDdlOption.SelectedValue;
        lstCondition.Add(condition);

        ViewState[CONDITION_VIEWSTATE_NAME] = lstCondition;
        this.radGrdCondition.Rebind();
    }

    protected void radDdlApprovalType_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        ViewState[CONDITION_VIEWSTATE_NAME] = new List<ConditionData>();
        this.radGrdCondition.Rebind();

        this.radTxtUserId.Text = string.Empty;
        this.hddApprovalUserCode.Value = string.Empty;
        this.radGrdBeforeApprover.SelectedIndexes.Clear();
        this.radGrdAfterApprover.SelectedIndexes.Clear();
        this.radGrdRecipient.SelectedIndexes.Clear();
        this.radGrdReviewer.SelectedIndexes.Clear();
    }

    private void InitCondition()
    {
        this.radGrdCondition.DataSource = string.Empty;
        this.radGrdCondition.DataBind();
        this.lstCondition = new List<ConditionData>();
        ViewState[CONDITION_VIEWSTATE_NAME] = this.lstCondition;
        this.radTxtUserId.Text = string.Empty;
        this.hddApprovalUserCode.Value = string.Empty;
        this.radBtnIsMandatory.Checked = false;
        this.radDdlFieldName.Enabled = true;
        this.radDdlCondition.Enabled = true;
        this.radTxtValue.ReadOnly = false;
        this.radDdlOption.Enabled = true;

    }

    protected void radBtnIsMandatory_CheckedChanged(object sender, EventArgs e)
    {
        this.radDdlFieldName.Enabled = !(sender as RadButton).Checked;
        this.radDdlCondition.Enabled = !(sender as RadButton).Checked;
        this.radTxtValue.ReadOnly = (sender as RadButton).Checked;
        this.radDdlOption.Enabled = !(sender as RadButton).Checked;
        this.radBtnAddCondition.Enabled = !(sender as RadButton).Checked;

        this.radGrdCondition.DataSource = string.Empty;
        this.radGrdCondition.DataBind();

        this.radGrdBeforeApprover.SelectedIndexes.Clear();
        this.radGrdAfterApprover.SelectedIndexes.Clear();
        this.radGrdRecipient.SelectedIndexes.Clear();
        this.radGrdReviewer.SelectedIndexes.Clear();
    }

    protected void radBtnNewCondition_Click(object sender, EventArgs e)
    {
        this.lstCondition = new List<ConditionData>();
        ViewState[CONDITION_VIEWSTATE_NAME] = this.lstCondition;
        this.radGrdCondition.DataSource = lstCondition;
        this.radGrdCondition.DataBind();
    }
    #endregion
}