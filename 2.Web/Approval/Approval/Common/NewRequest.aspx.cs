using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Common.Mgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Approval.Approval.Common
{
    public partial class NewRequest : DNSoft.eW.FrameWork.Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void SetDocumentList(string companyCode)
        {
            using(Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
            {
                List<DocumentListDto> List = mgr.SelectDocumentList(companyCode);
                
                if (List != null)
                {
                    

                }
            }
            
        }

    }
}