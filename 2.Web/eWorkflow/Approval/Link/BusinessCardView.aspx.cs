using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using DNSoft.eWF.FrameWork.Web;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Approval.Mgr;

public partial class Approval_Link_BusinessCardView : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var processId = Request["processId"].NullObjectToEmptyEx();
            this.hddProcessId.Value = processId;
            MasterPage master = Page.Master;
            Control control = master.FindControl("form1");
            if (control is System.Web.UI.HtmlControls.HtmlForm)
            {
                (control as System.Web.UI.HtmlControls.HtmlForm).Style.Add("overflow", "hidden");
            }
            
            SelectDisplayNameCard(processId);
        }
    }

    private void SelectDisplayNameCard(string processId)
    {
        DTO_DOC_BUSINESS_CARD doc;
        if (!hddProcessId.Value.Equals(String.Empty))
        {
            using (Bayer.eWF.BSL.Approval.Mgr.BusinessCardMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.BusinessCardMgr())
            {
                doc = mgr.SelectBusinessCardDisplayNameCard(hddProcessId.Value);
            }
            if (doc != null)
            {
                var korDivision = doc.KOR_DIVISION_NAME;
                var engDivison = doc.ENG_DIVISION_NAME;
                char sp = '(';
                if (korDivision == "")
                {
                    korDivision = "(";
                    engDivison = "(";
                }
                string[] korDiv = korDivision.Split(sp);
                string[] engDiv = engDivison.Split(sp);

                #region LEADING_SUBGROUP Image set
                HtmlImage img = new HtmlImage();
                HtmlImage eImg = new HtmlImage();
                switch (doc.LEADING_SUBGROUP)
                {
                    case "BCS":
                        //img.Src = eImg.Src = "/eWorks/Styles/Images/bcs.png";
                        //logoImgArea.Controls.Add(img);
                        //englogoImgArea.Controls.Add(eImg);
                        divNameCardList1.Style.Remove("margin");
                        divNameCardList2.Style.Remove("margin");
                        divNameCardList1.Style.Add("margin", "20px 0px 5px 10px");
                        divNameCardList2.Style.Add("margin", "15px 0px 5px 10px");
                        break;
                    case "BMS":
                        //img.Src = eImg.Src = "/eWorks/Styles/Images/bms.png";
                        //logoImgArea.Controls.Add(img);
                        //englogoImgArea.Controls.Add(eImg);
                        divNameCardList1.Style.Remove("margin");
                        divNameCardList2.Style.Remove("margin");
                        divNameCardList1.Style.Add("margin", "20px 0px 5px 10px");
                        divNameCardList2.Style.Add("margin", "15px 0px 5px 10px");
                        break;
                    case "BHC":
                        //img.Src = eImg.Src = "/eWorks/Styles/Images/bhc.png";
                        //logoImgArea.Controls.Add(img);
                        //englogoImgArea.Controls.Add(eImg);
                        divNameCardList1.Style.Remove("margin");
                        divNameCardList2.Style.Remove("margin");
                        divNameCardList1.Style.Add("margin", "20px 0px 5px 10px");
                        divNameCardList2.Style.Add("margin", "15px 0px 5px 10px");
                        break;
                    /*default:
                        img.Src = eImg.Src = "/eWorks/Styles/Images/bcs.png";
                        logoImgArea.Controls.Add(img);
                        englogoImgArea.Controls.Add(eImg);
                        divNameCardList1.Style.Remove("margin");
                        divNameCardList2.Style.Remove("margin");
                        divNameCardList1.Style.Add("margin", "20px 0px 5px 10px");
                        divNameCardList2.Style.Add("margin", "10px 0px 5px 10px");
                        break;*/
                }
                #endregion

                //labelKorLogo.Text = doc.LEADING_SUBGROUP;
                labelKorName.Text = doc.KOR_NAME;
                switch (doc.COLOR_CODE)
                {
                    case "Raspberry":
                        labelKorName.CssClass = "font-raspberry";
                        labelEngName.CssClass = "font-raspberrye";
                        break;
                    case "Blue":
                        labelKorName.CssClass = "font-blue";
                        labelEngName.CssClass = "font-bluee";
                        break;
                    case "Green":
                        labelKorName.CssClass = "font-green";
                        labelEngName.CssClass = "font-greene";
                        break;
                    case "Mid Purple":
                        labelKorName.CssClass = "font-midpurple";
                        labelEngName.CssClass = "font-midpurplee";
                        break;
                }
                

                if (doc.KOR_JOB_TITLE.Length > 0)
                    if (doc.KOR_TITLE_NAME.Length > 0)
                        labelKorTitle.Text = doc.KOR_JOB_TITLE + " / " + doc.KOR_TITLE_NAME;
                    else
                        labelKorTitle.Text = doc.KOR_JOB_TITLE;
                else
                    labelKorTitle.Text = doc.KOR_TITLE_NAME;


               // if (doc.KOR_DEPARTMENT.Length > 0)
               // {
                    //labelKorDepartment.Text = doc.KOR_DEPARTMENT;
                    if(korDiv[1].Length >0) { 
                        if (korDiv[1].Substring(0, 4) == "0016")
                            labelKorDepartment.Text = doc.KOR_DEPARTMENT;
                        else
                            labelKorDepartment.Text = doc.KOR_DEPARTMENT;
                        if (korDiv[1].Substring(0, 4) == "0016")
                            labelKorDivision.Text = "";
                        else
                            labelKorDivision.Text = korDiv[0];
                    }
                // }
                // else
                // {
                
               // }
                labelKorCompany.Text = doc.COMPANY_NAME;
                labelKorAddress1.Text = doc.KOR_ADDRESS1;
                labelKorAddress2.Text = doc.KOR_ADDRESS2;
                labelKorTel.Text = "전 화 : " + doc.TEL_OFFICE.Replace("-"," ").Replace("/"," ");
                if (doc.FAX.Replace("-", " ").Replace("/", " ").Length > 4)
                {
                    labelKorFax.Text = "팩 스 : " + doc.FAX.Replace("-", " ").Replace("/", " ");
                }

                
                labelKorMobile.Text = "핸드폰 : " + doc.MOBILE.Replace("-", " ").Replace("/", " "); 
                labelKorEmail.Text = doc.E_MAIL;
                labelKorWebAddress.Text = doc.HOMEPAGE;

                //labelEngLogo.Text = doc.LEADING_SUBGROUP;
                labelEngName.Text = doc.ENG_NAME;
                labelEngTitle.Text = doc.ENG_TITLE;
                if (korDiv[1].Length > 0)
                {
                    // if (doc.ENG_DEPARTMENT.Length > 0){
                    if (engDiv[1].Substring(0, 4) == "0016")
                        //labelEngDepartment.Text = doc.ENG_DEPARTMENT;
                        labelEngDepartment.Text = doc.ENG_DEPARTMENT;
                    else
                        labelEngDepartment.Text = doc.ENG_DEPARTMENT;
                    // }
                    // else
                    //  {
                    if (engDiv[1].Substring(0, 4) == "0016")
                        labelEngDivision.Text = "";
                    else
                        labelEngDivision.Text = engDiv[0];
                    // }
                }  

                labelEngCompany.Text = Sessions.CompanyName;
                labelEngAddress1.Text = doc.ENG_ADDRESS1;
                labelEngAddress2.Text = doc.ENG_ADDRESS2;
                labelEngTel.Text = "Tel : " + "+82 " + doc.TEL_OFFICE.Replace("-", " ").Replace("/", " ").Remove(0, 1);
                if (doc.FAX.Replace("-", " ").Replace("/", " ").Length > 4)
                {
                    //labelEngFax.Text = "Fax : " + "+82 " + doc.FAX.Replace("-", " ").Replace("/", " ").Remove(0, 1);
                }
                
                labelEngMobile.Text = "Mobile : " + "+82 " + doc.MOBILE.Replace("-", " ").Replace("/", " ").Remove(0, 1); 
                labelEngEmail.Text = doc.E_MAIL;
                labelEngWebAddress.Text = doc.HOMEPAGE;

            }
        }
    }

}