using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_ApproveMenuBar : System.Web.UI.UserControl
{
    public event EventHandler btnRequestClick;
    public event EventHandler btnApprovalClick;
    public event EventHandler btnFowardApprovalClick;
    public event EventHandler btnRejectClick;
    public event EventHandler btnFowardClick;
    public event EventHandler btnRecallClick;
    public event EventHandler btnWithdrawClick;
    public event EventHandler btnRemindClick;
    public event EventHandler btnExitClick;
    public event EventHandler btnSaveClick;
    public event EventHandler btnInputCommandClick;
    public event EventHandler btnReUseClick;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region [ Property ]

    bool visibleAdditionalApproval;
    public bool VisibleAdditionalApproval
    {
        get { return this.visibleAdditionalApproval; }
        set
        {
            this.visibleAdditionalApproval = value;
            this.rowAdditionalApproval.Visible = this.visibleAdditionalApproval;
        }
    }

    public Telerik.Web.UI.AutoCompleteBoxEntryCollection GetAdditionalApproval
    {
        get
        {
            return this.ucAdditionalApprover.GetEntries();
        }
    }

    public void AddAdditionaApproval(Telerik.Web.UI.AutoCompleteBoxEntryCollection collection)
    {
        foreach (Telerik.Web.UI.AutoCompleteBoxEntry entry in collection)
        {
            
        }
    }

    #endregion

    #region Buttons Click Event
    protected void btnRequest_Click(object sender, EventArgs e)
    {
        btnRequestClick(sender, e);
    }
    protected void btnApproval_Click(object sender, EventArgs e)
    {
        btnApprovalClick(sender, e);
    }
    protected void btnForwardApproval_Click(object sender, EventArgs e)
    {
        btnFowardApprovalClick(sender, e);
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        btnRejectClick(sender, e);
    }
    protected void btnForward_Click(object sender, EventArgs e)
    {
        btnFowardClick(sender, e);
    }
    protected void btnRecall_Click(object sender, EventArgs e)
    {
        btnRecallClick(sender, e);
    }
    protected void btnWithdraw_Click(object sender, EventArgs e)
    {
        btnWithdrawClick(sender, e);
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        btnExitClick(sender, e);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        btnSaveClick(sender, e);
    }
    protected void btnInputCommand_Click(object sender, EventArgs e)
    {
        btnInputCommandClick(sender, e);
    }

    protected void btnRemind_Click(object sender, EventArgs e)
    {
        btnRemindClick(sender, e);
    }

    protected void btnReUse_Click(object sender, EventArgs e)
    {
        btnReUseClick(sender, e);
    }


    #endregion
}