using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Common.Mgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eW.FrameWork;
using Telerik.Web.UI;

public partial class Common_Popup_CompanyList : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string allowMultiSelect = Request["AllowMultiSelect"].NullObjectToStringEx("false");
            if (allowMultiSelect.ToLower().Equals("true"))
            {
                this.radGrdCompany.AllowMultiRowSelection = true;
                this.radGrdCompany.Columns[0].Display = true;
            }
            else
            {
                this.radGrdCompany.AllowMultiRowSelection = false;
                this.radGrdCompany.Columns[0].Display = false;
            }

            SelectCompanyList("");
        }
    }

    private List<DTO_COMPANY> GetCompanyList(string keyWord)
    {
        try
        {
            using (CommonMgr mgr = new CommonMgr())
            {
                return mgr.SelectCompanyList(keyWord);
            }
        }
        catch
        {
            throw;
        }
    }

    private void SelectCompanyList(string keyWord)
    {
        try
        {
            this.radGrdCompany.DataSource = GetCompanyList(keyWord);
            this.radGrdCompany.DataBind();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    protected void radGrdCompany_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            this.radGrdCompany.DataSource = GetCompanyList(this.radTxtKeyword.Text);
            this.radGrdCompany.Rebind();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    protected void radBtnSearch_Click(object sender, EventArgs e)
    {
        SelectCompanyList(this.radTxtKeyword.Text);
    }
    protected void radGrdCompany_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem gridItem = e.Item as GridDataItem;
            foreach (GridColumn column in radGrdCompany.MasterTableView.RenderColumns)
            {
                if (column is GridBoundColumn)
                {
                    if (column.UniqueName.Equals("NAME"))
                    {

                        gridItem[column.UniqueName].ToolTip = gridItem["ADDRESS"].Text;
                            //gridItem.OwnerTableView.DataKeyValues[gridItem.ItemIndex]["ADDRESS"].ToString();

                    }
                }
            }
        }

    }
}