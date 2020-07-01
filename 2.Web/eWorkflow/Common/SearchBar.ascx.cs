using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_SearchBar : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string SearchType
    {
        get { return this.cboSearchType.SelectedValue; }
        set { cboSearchType.SelectedValue = value; }
    }

    public string SearchText
    {
        get { return this.txtSearchText.Text; }
        set { this.txtSearchText.Text = value; }
    }

    public string UseDateYN
    {
        get { return this.chkUseDateSearch.Checked ? "Y" : "N"; }
        set
        {
            if (value == "Y")
            {
                this.chkUseDateSearch.Checked = true;
            }
            else
            {
                this.chkUseDateSearch.Checked = false;
            }
        }
    }

    public DateTime? StartDate
    {
        get { return this.dtStartDate.SelectedDate; }
    
    }

    public DateTime? EndDate
    {
        get { return this.dtEndDate.SelectedDate; }
   
    }

}