using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using System.Text;

public partial class Approval_Link_CreditDebitNoteView : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request["processid"].NullObjectToStringEx("").Length > 0)
                {
                    string processId = Request["processid"].ToString();
                    //string processId = "P000000821";
                    SelectCreditDebitNote(processId);
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void SelectCreditDebitNote(string processId)
    {
        using (CreditDebitNoteMgr mgr = new CreditDebitNoteMgr())
        {
            CreditDebitNoteViewDto dto = mgr.SelectCreditDebitNoteView(processId);
            string type = dto.TYPE;
            if (type.Equals("CREDIT"))
            {
                this.lblType.Text = "Credit Note";
            }
            else
            {
                this.lblType.Text = "Debit Note";
            }
            this.lblDocNo.Text = "No. " + dto.DOC_NUM;
            //좌측상단 
            string[] arrAddress = dto.TO_ADDRESS.Split(new char[] { ',' });
            string disAddress = string.Empty;
            foreach (string address in arrAddress)
            {
                disAddress += address + "<br/>";
            }
            this.lblToAddress.Text = "<b>" + dto.TO_NAME + "</b>" + "<br/>" + disAddress;

            //우측상단
            arrAddress = dto.COMPANY_ADDRESS.Split(new char[] { ',' });
            disAddress = string.Empty;
            foreach (string address in arrAddress)
            {
                disAddress += address + "<br/>";
            }
            this.lblCompanAddress.Text = "<b>" + dto.COMPANY_NAME + "</b>" + "<br/>" + disAddress;

            this.lblAttn.Text = "Attn : " + (dto.ATTN_LIST.EndsWith("/") ? dto.ATTN_LIST.Remove(dto.ATTN_LIST.Length - 1, 1) : dto.ATTN_LIST);
            this.lblCc.Text = "Cc : " + (dto.CC_LIST.EndsWith("/") ? dto.CC_LIST.Remove(dto.CC_LIST.Length - 1, 1) : dto.CC_LIST);
            this.lblInvoiceDate.Text = dto.INVOICE_DATE.ToString("yyyy-MM-dd");
            this.lblDueDate.Text = "<b>" + dto.DUE_DATE.ToString("yyyy-MM-dd") + "</b>";

            //this.lblAmount.Text = dto.TOTAL_AMOUNT.ToString("#,##0");
            this.lblTotalAmount.Text = dto.TOTAL_AMOUNT.ToString("#,##0.##");
            this.lblLocalAmount.Text = dto.LOCAL_AMOUNT.ToString("#,##0");
            this.lblCurrency.Text = "(" + dto.CURRENCY + ")";

            List<DTO_DOC_CREDIT_DEBIT_NOTE_DESC> items = mgr.SelectCreditDebitNoteDesc(processId);
            if (items.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                html.Append("<table style='width: 100%;margin:0 0 0 0'>");
                html.Append("<colgroup>");
                html.Append("<col style='width: 30px;' />");
                html.Append("<col />");
                html.Append("<col style='width: 150px;' />");
                html.Append("<col style='width: 120px;' />");
                html.Append("</colgroup>");
                foreach (DTO_DOC_CREDIT_DEBIT_NOTE_DESC item in items)
                {
                    html.Append("<tr>");
                    html.Append("<td></td>");
                    html.Append("<td>" + item.DESCRIPTION + "</td>");
                    html.Append("<td><span style='font-size: 13pt; display: inline-block; width: 120px; float: right;text-align:right'>" + item.AMOUNT.ToString("#,###.##") + "</span></td>");
                    html.Append("<td></td>");
                    html.Append("</tr>");
                }
                html.Append("</table>");
                divItem.InnerHtml = html.ToString();
            }

            if (dto.COMPANY_ID.Equals("0695"))
            {
                this.divBKL.Visible = true;
                this.divBCS.Visible = false;
                this.divBMSG.Visible = false;
            }
            else if (dto.COMPANY_ID.Equals("0963"))
            {
                this.divBKL.Visible = false;
                this.divBCS.Visible = true;
                this.divBMSG.Visible = false;
            }
            else
            {
                this.divBKL.Visible = false;
                this.divBCS.Visible = false;
                this.divBMSG.Visible = true;
            }
        }
    }
}