using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_Popup_CountryCity : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitPageInfo();
        }
    }

    private void InitPageInfo()
    {
        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            this.radDdlCountry.DataSource = mgr.SelectCountry();
            this.radDdlCountry.DataBind();

            if (this.radDdlCountry.FindItemByValue("KOREA") != null)
            {
                int defaultIdx = this.radDdlCountry.FindItemByValue("KOREA").Index;

                this.radDdlCountry.SelectedIndex = defaultIdx;

                SelectCity(this.radDdlCountry.SelectedValue);

            }
        }
    }

    private void SelectCity(string country)
    {
        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            this.radDdlCity.DataSource = mgr.SelectCity(country);
            this.radDdlCity.DataBind();
            this.radDdlCity.SelectedIndex = -1;
        }
    }
    protected void radDdlCountry_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
    {
        SelectCity(e.Value);
    }
}