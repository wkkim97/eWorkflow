using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Bayer.eWF.BSL.Common.Dto;


public partial class Common_Popup_PopupNewProduct : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

	
	protected void radDdlCompany_ItemSelected(object sender, DropDownListEventArgs e)
	{
		string type = radDdlCompany.SelectedValue;

		using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
		{
			List<string> list = mgr.SelectProductMasterBu(type);

			foreach (string bu in list)
			{
				RadButton btn = new RadButton();
				btn.ButtonType = RadButtonType.ToggleButton;
				btn.ToggleType = ButtonToggleType.Radio;
				btn.AutoPostBack = false;
				btn.Text = bu;
				btn.Value = bu;
				btn.ID = "radBtn" + bu;
				btn.GroupName = "radBtnBU";
				this.divBU.Controls.Add(btn);
			}
		}
	}


	#region Product 신규 데이터 등록
	protected void radBtnSave_Click(object sender, EventArgs e)
	{
		DTO_PRODUCT doc = new DTO_PRODUCT();
		doc.PRODUCT_CODE = this.radTextProductCode.Text;
		doc.PRODUCT_NAME = this.radTextProductName.Text;
		doc.COMPANY_CODE = radDdlCompany.SelectedValue;
		doc.BASE_PRICE = (decimal?)this.radNumAmount.Value;
        doc.PRODUCT_NAME_KR = this.radTextProductName_KOR.Text;
		doc.CREATOR_ID = this.Sessions.UserID;
		doc.CREATE_DATE = DateTime.Now;
		doc.BU = this.hhdBu.Value;

		using (Bayer.eWF.BSL.Common.Dao.CommonDao dao = new Bayer.eWF.BSL.Common.Dao.CommonDao())
		{
			dao.InsertProduct(doc);
		}
		this.ClientWindowClose("true");
	} 
	#endregion

	
}