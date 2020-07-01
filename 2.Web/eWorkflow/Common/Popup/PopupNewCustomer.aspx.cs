using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Bayer.eWF.BSL.Common.Dto;

public partial class Common_Popup_PopupNewCustomer : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
	protected void radDdlCompany_ItemSelected(object sender, Telerik.Web.UI.DropDownListEventArgs e)
	{
		string company = this.radDdlCompany.SelectedValue;

		using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
		{
			List<string> list = mgr.SelectCustomerMasterBu(company);
			this.divBU.Controls.Clear();
			foreach (string bu in list)
			{
				RadButton btn = new RadButton();
				btn.ButtonType = RadButtonType.ToggleButton;
				btn.ToggleType = ButtonToggleType.Radio;
				btn.AutoPostBack = false;
				if (bu == string.Empty)
					btn.Text = "N/A";
				else
					btn.Text = bu;
				btn.Value = bu;
				btn.ID = "radBtn" + bu;
				btn.GroupName = "radBtnBU";
				btn.EnableViewState = true;

				this.divBU.Controls.Add(btn);
			}
		}
	}
	protected void radBtnSave_Click(object sender, EventArgs e)
	{
		DTO_CUSTOMER doc = new DTO_CUSTOMER();
		doc.COMPANY_CODE = this.radDdlCompany.SelectedValue;
		doc.CUSTOMER_CODE = this.radTextCustomertCode.Text;
		doc.PARVW = this.RadDdlParvw.SelectedValue;
        doc.CHANNEL = this.radDdlChannel.SelectedValue;
		doc.CUSTOMER_NAME = this.radTextCustomertName.Text;
		doc.CUSTOMER_NAME_KR = this.radTextCustomertKName.Text;
		doc.BU = this.hhdBu.Value;
		doc.CREATOR_ID = Sessions.UserID;
		doc.CREATE_DATE = DateTime.Now;
		

		using (Bayer.eWF.BSL.Common.Dao.CommonDao dao = new Bayer.eWF.BSL.Common.Dao.CommonDao())
		{
			dao.InsertCustomer(doc);
		}
		this.ClientWindowClose("true");
	}
}