using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI.Grid;
using Telerik.Web.UI;

public partial class Templete_GridDropDownload : System.Web.UI.Page
{
    public object[] Country_values = { new ListItem("Germany", "German"), new ListItem("England", "English"), new ListItem("Spain", "Spanish"), new ListItem("United States", "American") };

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        System.Data.SqlClient.SqlDataAdapter sdaTemp = null;
        System.Data.DataSet dsTemp = null;
        try
        {
            dsTemp = new DataSet();
            //연결정보가져오기
            sdaTemp = new SqlDataAdapter("eWorkflow.DBO.USP_SELECT_CONFIG", DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/SQLServer/ConnectionString"));
            sdaTemp.SelectCommand.CommandType = CommandType.StoredProcedure;

            sdaTemp.SelectCommand.Parameters.AddWithValue("@DOCUMENT_ID", "D0013");
            sdaTemp.Fill(dsTemp);
            sdaTemp.SelectCommand.Parameters.Clear();
            RadGrid1.DataSource = dsTemp.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (sdaTemp != null) sdaTemp.Dispose();
        }
 
     
    }

    private void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is Telerik.Web.UI.GridEditableItem && e.Item.IsInEditMode)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            // access/modify the edit item template settings here
            DropDownList list = item.FindControl("List1") as DropDownList;
            list.DataSource = Country_values;
            list.DataBind();

 
        }
        else if (e.Item is GridDataItem && !e.Item.IsInEditMode && Page.IsPostBack)
        {
            GridDataItem item = e.Item as GridDataItem;
            Label label = item.FindControl("Label1") as Label;
            // update the label value
          
        }
    }

    private void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridEditableItem editedItem = e.Item as GridEditableItem;
        DropDownList list = editedItem.FindControl("List1") as DropDownList;
        Session["updatedValue"] = list.SelectedValue;
    }
}