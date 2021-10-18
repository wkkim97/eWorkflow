using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_DocumentBaseFooter : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public Telerik.Web.UI.AutoCompleteBoxEntryCollection GetEntries()
    {
        return UserAutoCompleteBox.GetEntries();
    }
 
}