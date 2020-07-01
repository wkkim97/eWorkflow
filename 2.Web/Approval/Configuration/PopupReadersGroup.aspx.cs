using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Configuration;
using Telerik.Web.UI;

namespace Approval.Configuration
{
	public partial class PopupReadersGroup : DNSoft.eWF.FrameWork.Web.PageBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				if (!this.IsPostBack)
				{
					InitControls();
				}
			}
			catch (Exception ex)
			{
				this.errorMessage = ex.ToString();
			}
		}

		private void InitControls()
		{
			List<Bayer.eWF.BSL.Configuration.Dto.DTO_READERS_GROUP_USER_NAME> userlist = null;
			//UserList Binding
			using (Bayer.eWF.BSL.Configuration.Dao.ConfigurationDao uList = new Bayer.eWF.BSL.Configuration.Dao.ConfigurationDao())
			{				
				userlist = uList.SelectReadersGroupUser();
				RadGridUserList.DataSource = userlist;
				RadGridUserList.DataBind();
			}

		}
		//Check Box
		protected void ToggleRowSelection(object sender, EventArgs e)
		{
			((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
			bool checkHeader = true;
			foreach (GridDataItem dataItem in RadGridUserList.MasterTableView.Items)
			{
				if (!(dataItem.FindControl("CheckBox1") as CheckBox).Checked)
				{
					checkHeader = false;
					break;
				}
			}
			GridHeaderItem headerItem = RadGridUserList.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
			(headerItem.FindControl("headerChkbox") as CheckBox).Checked = checkHeader;
		}
		protected void ToggleSelectedState(object sender, EventArgs e)
		{
			CheckBox headerCheckBox = (sender as CheckBox);
			foreach (GridDataItem dataItem in RadGridUserList.MasterTableView.Items)
			{
				(dataItem.FindControl("CheckBox1") as CheckBox).Checked = headerCheckBox.Checked;
				dataItem.Selected = headerCheckBox.Checked;
			}
		}
	}
}