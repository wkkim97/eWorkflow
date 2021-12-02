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
using System.Data.SqlClient;
using System.Data;
using Telerik.Web.UI;

public partial class Approval_Common_TestBrowser : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.RadTextBox1.Text = Request["DocName"];
        this.RadTextBox2.Text = Request["DocID"];
    }


}