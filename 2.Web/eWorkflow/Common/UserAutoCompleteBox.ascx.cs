using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_UserAutoCompleteBox : System.Web.UI.UserControl
{
    private string _approvalType = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
 
    }
 
    public Telerik.Web.UI.AutoCompleteBoxEntryCollection GetEntries()
    {
        return autoCompleteUserBox.Entries;
    }

    public string ApprovalType
    {
        get
        {
            return _approvalType;
        }
        set
        {
            _approvalType = value;
        }

    }


}