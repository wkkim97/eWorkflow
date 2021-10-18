using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_ConfigConditionBox : System.Web.UI.UserControl
{ 
    public event EventHandler RemoveUserControl;
  
    protected void radBtnDelete_Click(object sender, EventArgs e)
    {
        this.RemoveUserControl(sender, e);
    }

    public string FieldName
    {
        get { return this.radTxtField.Text; }
        set { this.radTxtField.Text = value; }
    }

    public string ConditionText
    {
        get
        {
            return this.radComboCondition.SelectedItem.Text;
        }
    }

    public string ConditionValue
    {
        get
        {
            return this.radComboCondition.SelectedItem.Value;
        }
        set
        {
            for (int i = 0; i < this.radComboCondition.Items.Count; i++)
            {
                if (this.radComboCondition.Items[i].Value.Equals(value))
                {
                    this.radComboCondition.Items[i].Selected = true;
                }
            }
        }
    }

    public string Value
    {
        get
        {
            return this.radTxtValue.Text;
        }
        set
        {
            this.radTxtValue.Text = value;
        }
    }
    
}