using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eWF.FrameWork.Web;
using DNSoft.eW.FrameWork;

namespace Approval.Approval.Document
{
    public partial class DocSample : DNSoft.eWF.FrameWork.Web.DocBase
    {
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
                this.informationMessage = ex.Message.ToString();
            }
        }

        #region InitPageInfo
        private void InitPageInfo()
        {
            HddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        }
        #endregion

        #region PageLoadInfo
        private void PageLoadInfo()
        {
            DocumentBind();
            FootAreaBind();
        }

        private void DocumentBind()
        {
            
        }

        private void FootAreaBind()
        {

        }

        #endregion 

    }
}