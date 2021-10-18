using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Common.Dto;
using System.Collections.Generic;
using Telerik.Web.UI;

public partial class Manage_User_Settings : DNSoft.eWF.FrameWork.Web.PageBase
{
    private List<DTO_USER_CONFIG_DOC_SORT> _documentList = null;
    public List<DTO_USER_CONFIG_DOC_SORT> Documentlist
    {
        get
        {
            if (_documentList == null)
                _documentList = JsonConvert.JsonListDeserialize<DTO_USER_CONFIG_DOC_SORT>(hddDocumentList.Value);
            return _documentList;
        }
        set
        {
            _documentList = value;
            hddDocumentList.Value = JsonConvert.toJson<DTO_USER_CONFIG_DOC_SORT>(_documentList);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
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
        MainTypeBind();
        DocumentListBind();
        LoginHistoryBind();
    }

    private void LoginHistoryBind()
    {
        List<Bayer.eWF.BSL.Common.Dto.DTO_LOGIN_HISTORY> doc = null;
        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            doc = mgr.SelectLoginHistory(Sessions.UserID);

        }
        RadGrdLoginHistory.DataSource = doc;
        RadGrdLoginHistory.DataBind();
    }

    private void DocumentListBind()
    {
        List<DTO_USER_CONFIG_DOC_SORT> doc = null;

        using (Bayer.eWF.BSL.Common.Mgr.UserConfigMgr mgr = new Bayer.eWF.BSL.Common.Mgr.UserConfigMgr())
        {
            doc = mgr.SelectUserConfigDocList(Sessions.UserID);
        }

        Documentlist = doc;

        grdDoclist.PageSize = 100;
        grdDoclist.DataSource = doc;
        grdDoclist.DataBind();
    }

    private void MainTypeBind()
    {

        #region SelectUserConfig 주석처리
        /*
        DTO_USER_CONFIG cfg = null;

        using (Bayer.eWF.BSL.Common.Mgr.UserConfigMgr mgr = new Bayer.eWF.BSL.Common.Mgr.UserConfigMgr())
        {
            cfg = mgr.SelectUserConfig(Sessions.UserID);   //cfg.MAIN_VIEWTYPE
        }*/

        #endregion

        if (rdoTypeA.Value.Equals(Sessions.MainViewType))
        {
            rdoTypeA.Checked = true;
        }
        else if (rdoTypeB.Value.Equals(Sessions.MainViewType))
        {
            rdoTypeB.Checked = true;
        }
    }

    protected void btnMainTypeSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdoTypeA.Checked)
            {
                Sessions.MainViewType = rdoTypeA.Value;
            }
            else
            {
                Sessions.MainViewType = rdoTypeB.Value;
            }

            using (Bayer.eWF.BSL.Common.Mgr.UserConfigMgr mgr = new Bayer.eWF.BSL.Common.Mgr.UserConfigMgr())
            {
                mgr.UpdateMainViewType(Sessions.UserID, Sessions.MainViewType);
            }

            base.Session["UserSessionClass"] = Sessions;
            RadWindowManager win = (RadWindowManager)Master.FindControl("masterWinMgr");
            win.RadAlert("로그아웃 후 로그인을 해주시기 바랍니다.", 400, 120, "complete", "");
            // this.informationMessage = "로그아웃 후 로그인을 해주시기 바랍니다.";
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    protected void grdDoclist_RowDrop(object sender, Telerik.Web.UI.GridDragDropEventArgs e)
    {
        if (string.IsNullOrEmpty(e.HtmlElement))
        {

            if ((e.DestDataItem == null && Documentlist.Count == 0) || e.DestDataItem != null && e.DestDataItem.OwnerGridID == grdDoclist.ClientID)
            {

                int destinationIndex = -1;

                if (e.DestDataItem != null)
                {
                    DTO_USER_CONFIG_DOC_SORT d = GetDocItem(e.DestDataItem.GetDataKeyValue("DOCUMENT_ID").ToString());
                    destinationIndex = d != null ? Documentlist.IndexOf(d) : -1;
                }

                if (e.DropPosition == GridItemDropPosition.Above && e.DestDataItem.ItemIndex > e.DraggedItems[0].ItemIndex)
                {
                    destinationIndex -= 1;
                }

                if (e.DropPosition == GridItemDropPosition.Below && e.DestDataItem.ItemIndex < e.DraggedItems[0].ItemIndex)
                {
                    destinationIndex += 1;
                }

                List<DTO_USER_CONFIG_DOC_SORT> orderToMove = new System.Collections.Generic.List<DTO_USER_CONFIG_DOC_SORT>();
                foreach (GridDataItem draggedItem in e.DraggedItems)
                {
                    DTO_USER_CONFIG_DOC_SORT tmpOrder = GetDocItem(draggedItem.GetDataKeyValue("DOCUMENT_ID").ToString());
                    if (tmpOrder != null)
                        orderToMove.Add(tmpOrder);

                }

                foreach (DTO_USER_CONFIG_DOC_SORT move in orderToMove)
                {
                    Documentlist.Remove(move);
                    Documentlist.Insert(destinationIndex, move);
                }
                Documentlist = ClearingOrder(Documentlist);
                grdDoclist.DataSource = Documentlist;
                grdDoclist.Rebind();
                //int destinationItemIndex = destinationIndex - (grdDoclist.PageSize * grdDoclist.CurrentPageIndex);
                //e.DestinationTableView.Items[destinationItemIndex].Selected = true;

            }

        }
    }

    private List<DTO_USER_CONFIG_DOC_SORT> ClearingOrder(List<DTO_USER_CONFIG_DOC_SORT> doc)
    {
        int order = 1;
        foreach (DTO_USER_CONFIG_DOC_SORT item in doc)
        {
            item.SORT = order;
            order++;
        }
        return doc;
    }

    private DTO_USER_CONFIG_DOC_SORT GetDocItem(string dataKey)
    {
        return Documentlist.Find(x => x.DOCUMENT_ID.Contains(dataKey));
    }

    protected void btnOrderSave_Click(object sender, EventArgs e)
    {
        try
        {
            using (Bayer.eWF.BSL.Common.Mgr.UserConfigMgr mgr = new Bayer.eWF.BSL.Common.Mgr.UserConfigMgr())
            {
                mgr.UpdateOrderingDocument(Documentlist);
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    protected void RadGrdLoginHistory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (e.Item as GridDataItem);
            item["CLIENTIP"].Text = item["CLIENTIP"].Text + " (" + item["WINDOWDOMAINNAME"].Text + ")";
        }
    }
}