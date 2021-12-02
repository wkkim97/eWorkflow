using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 

public class DNAutoCompleteBoxDataItem : Telerik.Web.UI.AutoCompleteBoxItemData
{
    public DNAutoCompleteBoxDataItem()
    {
        
    }

    private string _unique;

    private string _mailaddress;

    public string Unique
    {
        set
        {
            this._unique = value;
        }
        get
        {
            return _unique;
        }
    }

    public string MailAddress
    {
        set
        {
            this._mailaddress = value;
        }
        get
        {
            return _mailaddress;
        }
    }
}
 