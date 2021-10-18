using Approval.Common;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Common.Mgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Approval.Configuration
{
    public partial class Configuration : DNSoft.eWF.FrameWork.Web.PageBase
    {
        private const string prefixControl = "bxCondition";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    InitControl();
                }
                else
                {
                    AddAndRemoveConditionBox();
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
                    this.divJobTitle.Controls.Add(radBtnJob);
                }
            }
        }

        #region [ Condition관련 ]

        private Control GetPostBackControl(Page page)
        {
            Control control = null;

            string ctrlName = page.Request["__EVENTTARGET"];

            //string ctrlName = page.Request.Params.Get("__EVENTTARGET");

            if (string.IsNullOrEmpty(ctrlName))
            {
                foreach (string strCtrl in page.Request.Form)
                {
                    Control ctrl = page.FindControl(strCtrl);
                    if (ctrl is RadButton)
                    {
                        control = ctrl; break;
                    }
                }
            }
            else
            {
                control = page.FindControl(ctrlName);
            }

            return control;
        }

        private bool InDeletedList(string controlID)
        {
            string[] deletedList = this.ltlRemoved.Text.Split('|');
            for (int i = 0; i < deletedList.Count() - 1; i++)
            {
                if (controlID.ToLower().Equals(deletedList[i].ToLower()))
                    return true;
            }
            return false;
        }

        protected void HandleRemoveConditionControl(object sender, EventArgs args)
        {
            ConfigConditionBox bxCondition = (ConfigConditionBox)((sender as RadButton).Parent);
            phConfiguration.Controls.Remove(bxCondition);

            this.ltlRemoved.Text += bxCondition.ID + "|";
            this.ltlCount.Text = Convert.ToString(Convert.ToInt32(this.ltlCount.Text) - 1);
        }



        private void AddAndRemoveConditionBox()
        {
            Control control = GetPostBackControl(this.Page);

            if (control != null)
            {
                if (control.ID.Equals("radBtnAddCondition"))
                    this.ltlCount.Text = Convert.ToString(Convert.ToInt32(ltlCount.Text) + 1);
                else if (control.ID.Equals("btnNewConfiguration"))
                    this.ltlCount.Text = "1";

            }

            phConfiguration.Controls.Clear();

            int iControlID = 0;
            int iCount = Convert.ToInt32(this.ltlCount.Text);
            for (int i = 0; i < iCount; i++)
            {
                ConfigConditionBox bxCondition = (ConfigConditionBox)this.LoadControl("/eWorks/Common/ConfigConditionBox.ascx");
                while (InDeletedList(prefixControl + iControlID.ToString()))
                {
                    iControlID++;
                }

                bxCondition.ID = prefixControl + iControlID.ToString();

                bxCondition.RemoveUserControl += HandleRemoveConditionControl;

                phConfiguration.Controls.Add(bxCondition);

                iControlID++;
            }
        }

        #endregion
    }
}