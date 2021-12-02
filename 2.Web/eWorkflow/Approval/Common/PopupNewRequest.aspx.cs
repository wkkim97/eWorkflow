using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Common.Mgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Data;

public partial class Approval_Common_PopupNewRequest : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetDocumentList("BMSG", RadTreeViewNewRequest);
        }
    }

    private void SetDocumentList(string companyCode, RadTreeView treeview)
    {
        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            List<DocumentListDto> documents = mgr.SelectDocumentList(companyCode);

            var roots = (from r in documents where r.CATEGORY_CODE == "0000" select r);

            //Add Category
            foreach (var root in roots)
            {
                RadTreeNode node = new RadTreeNode();
                node.Text = root.CATEGORY_NAME;
                node.Value = root.DOCUMENT_ID;
                treeview.Nodes.Add(node);

                //Category별 Document
                var docCategories = (from d in documents where d.CATEGORY_CODE == root.DOCUMENT_ID select d);
                foreach (var doc in docCategories)
                {
                    RadTreeNode data = new RadTreeNode();
                    data.Text = doc.DOC_NAME;
                    data.Value = doc.DOCUMENT_ID;
                    data.Attributes.Add("formName", doc.FORM_NAME);
                    node.Nodes.Add(data);
         

                }

                node.Expanded = true;
                
            }
         
        }

    }


}