using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace DNSoft.eWF.FrameWork.Web.WebBase
{
    public abstract class DocMasterBase : System.Web.UI.MasterPage
    {
        protected string _requester = string.Empty;
        protected string _processid = string.Empty;
        protected string _title = string.Empty;
        protected string _organization = string.Empty;
        protected string _company = string.Empty;
        protected string _companyCode = string.Empty;
        protected string _subject = string.Empty;
        protected string _lifecycle = string.Empty;
        protected string _lifecycleText = string.Empty;
        protected string _documentno = string.Empty;
        protected string _documentid = string.Empty;
        protected string _processstatus = string.Empty;
        protected string _requestID = string.Empty;
        protected bool _AddAdditionalApproval = false;
        protected string _ReviwerDesc = string.Empty;
        protected string _allowForward = string.Empty;
        
        protected int[] _authBtnList;
        protected int[] _recipientAuthBtnList;
        protected int[] _reviewerAuthBtnList;

        /// <summary>
        /// 하위의 모든 UI컨트롤 Enabled 처리
        /// </summary>
        /// <param name="parent">Control Object</param>
        /// <param name="visible">Enabled(true or false)</param>
        public virtual void SetEnableControls(Control parent, bool visible)
        {

        }

        #region 프로퍼티
        public string Requester
        {
            get
            {
                return _requester;
            }
            set
            {
                _requester = value;
            }
        }

        public string RequestID
        {
            get
            {
                return _requestID;
            }
            set
            {
                _requestID = value;
            }
        }

        public string ProcessID
        {
            get
            {
                return _processid;
            }
            set
            {
                _processid = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }

        public string Organization
        {
            get
            {
                return _organization;
            }
            set
            {
                _organization = value;
            }
        }

        public string Company
        {
            get
            {
                return _company;
            }
            set
            {
                _company = value;
            }
        }

        public string CompanyCode
        {
            get
            {
                return _companyCode;
            }
            set
            {
                _companyCode = value;
            }
        }

        public string LifeCycle
        {
            get
            {
                return _lifecycle;
            }
            set
            {
                _lifecycle = value;
            }
        }

        public string DocumentNo
        {
            get
            {
                return _documentno;
            }
            set
            {
                _documentno = value;
            }
        }

        public string DocumentID
        {
            get
            {
                return _documentid;
            }
            set
            {
                _documentid = value;
            }

        }

        public string ProcessStatus
        {
            get
            {
                return _processstatus;
            }
            set
            {
                _processstatus = value;
            }
        }

        public virtual void MasterProcessing(string currentMethodName)
        {

        }

        public int[] CommandAuthList
        {
            get
            {
                return _authBtnList;
            }
            set
            {
                _authBtnList = value;
            }
        }

        public int[] RecipientAuthList
        {
            get
            {
                return _recipientAuthBtnList;
            }
            set
            {
                _recipientAuthBtnList = value;
            }
        }

        public int[] ReviewerAuthList
        {
            get
            {
                return _reviewerAuthBtnList;
            }
            set
            {
                _reviewerAuthBtnList = value;
            }
        }

        public string AllowForward
        {
            get { return _allowForward; }
            set { _allowForward = value; }
        }

        public virtual event EventHandler btnRequestClicked
        {
            add {  }
            remove { } 
        }

        public virtual event EventHandler btnApprovalClicked
        {
            add {  }
            remove { }
        }

        public virtual event EventHandler btnFowardApprovalClicked
        {
            add {   }
            remove { }
        }

        public virtual event EventHandler btnRejectClicked
        {
            add {   }
            remove { }
        }

        public virtual event EventHandler btnFowardClicked
        {
            add { }
            remove { }
        }

        public virtual event EventHandler btnRecallClicked
        {
            add { }
            remove { }
        }

        public virtual event EventHandler btnWithdrawClicked
        {
            add { }
            remove { }
        }

        public virtual event EventHandler btnExitClicked
        {
            add {  }
            remove { }
        }

        public virtual event EventHandler btnSaveClicked
        {
            add {   }
            remove { }
        }

        public virtual event EventHandler btnInputCommandClick
        {
            add {  }
            remove { }
        }

        public virtual event EventHandler btnRemindClicked
        {
            add {  }
            remove { }
        }

        public virtual event EventHandler btnReUseClicked
        {
            add { }
            remove { }
        }
        #endregion


    }
}
