using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Xml;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Collections.Generic;

using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Common;
using Bayer.eWF.BSL.Common.Dto;

using Telerik.Web.UI;

public partial class Common_Interface_XmlHttpProcess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

        }
        catch (Exception ex)
        {
            Response.Write("<response><error><![CDATA[" + ex.Message + "]]></error></response>");
        }
        finally
        {

        }
    }

    #region 사용자이름, 이메일, ID로 사용자 검색
    /// <summary>
    /// <b>■사용자이름, 이메일, ID로 사용자 검색</b><br/>
    /// - 작  성  자 : 닷넷소프트 김학진<br/>
    /// - 최초작성일 : 2009.12.01<br/>
    /// - 최종수정자 : <br/>
    /// - 최종수정일 : <br/>
    /// - 주요변경로그<br/>
    /// 2009.12.01 생성<br/>	
    /// </summary>
    /// <returns>Xml String</returns>
    [WebMethod]
    public static AutoCompleteBoxData SearchUserByName(object context)
    {
        AutoCompleteBoxData res = new AutoCompleteBoxData();

        try
        {
            List<DNAutoCompleteBoxDataItem> result = new List<DNAutoCompleteBoxDataItem>();
            string searchString = ((Dictionary<string, object>)context)["Text"].ToString();
            // 사용자 검색				
            using (Bayer.eWF.BSL.Common.Mgr.UserMgr oUser = new Bayer.eWF.BSL.Common.Mgr.UserMgr())
            {
                List<SmallUserInfoDto> list = oUser.SelectUserList(searchString);

                foreach (SmallUserInfoDto dto in list)
                {
                    DNAutoCompleteBoxDataItem item = new DNAutoCompleteBoxDataItem();
                    System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string value = json.Serialize(dto);

                    item.Value = value; // dto.USER_ID;
                    item.Text = dto.FULL_NAME;
                    item.MailAddress = dto.MAIL_ADDRESS;
                    item.Unique = dto.USER_ID;
                    result.Add(item);
                }



            }

            res.Items = result.ToArray();

            return res;
        }
        catch
        {
            throw;
        }
        finally
        {
        }
    }

    /// <summary>
    /// 2014.11.01 Global사용자 포함
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [WebMethod]
    public static AutoCompleteBoxData SearchGlobalUserByName(object context)
    {
        AutoCompleteBoxData res = new AutoCompleteBoxData();

        try
        {
            List<DNAutoCompleteBoxDataItem> result = new List<DNAutoCompleteBoxDataItem>();
            string searchString = ((Dictionary<string, object>)context)["Text"].ToString();
            // 사용자 검색				
            using (Bayer.eWF.BSL.Common.Mgr.UserMgr oUser = new Bayer.eWF.BSL.Common.Mgr.UserMgr())
            {
                List<SmallUserInfoDto> list = oUser.SelectUserGlobalList(searchString);

                foreach (SmallUserInfoDto dto in list)
                {
                    DNAutoCompleteBoxDataItem item = new DNAutoCompleteBoxDataItem();
                    System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string value = json.Serialize(dto);

                    item.Value = value; // dto.USER_ID;
                    item.Text = dto.FULL_NAME;
                    item.MailAddress = dto.MAIL_ADDRESS;
                    item.Unique = dto.USER_ID;
                    result.Add(item);
                }
            }

            res.Items = result.ToArray();

            return res;
        }
        catch
        {
            throw;
        }
        finally
        {
        }
    }

    /// <summary>
    /// 2014.11.13 Customer조회
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [WebMethod]
    public static AutoCompleteBoxData SearchCustomer(RadAutoCompleteContext context)
    {
        AutoCompleteBoxData res = new AutoCompleteBoxData();

        try
        {
            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();
            string searchString = context["Text"].ToString();
            string company = context["company"].ToString();
            string bu = context["bu"].ToString();
            string parvw = context["parvw"].ToString();
            string level = context["level"].ToString();
            // 사용자 검색				
            using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
            {
                List<DTO_CUSTOMER> list = mgr.SelectCustomer(company, bu, parvw, searchString, level);

                foreach (DTO_CUSTOMER dto in list)
                {
                    AutoCompleteBoxItemData item = new AutoCompleteBoxItemData();

                    item.Value = dto.CUSTOMER_CODE; // dto.USER_ID;
                    item.Text = dto.CUSTOMER_NAME + "(" + dto.CUSTOMER_CODE + ")";
                    result.Add(item);
                }
            }

            res.Items = result.ToArray();

            return res;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// 2014.11.13 Product조회
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [WebMethod]
    public static AutoCompleteBoxData SearchProduct(RadAutoCompleteContext context)
    {
        AutoCompleteBoxData res = new AutoCompleteBoxData();
        try
        {
            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();
            string searchString = context["Text"].ToString();
            string company = context["company"].ToString();
            string bu = context["bu"].ToString();
            string baseprice = context["baseprice"].ToString();

            // 제품 검색				
            using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
            {
                List<DTO_PRODUCT> list = mgr.SelectProduct(company, bu, searchString, baseprice);

                foreach (DTO_PRODUCT dto in list)
                {
                    AutoCompleteBoxItemData item = new AutoCompleteBoxItemData();

                    item.Value = dto.PRODUCT_CODE; // dto.USER_ID;
                    item.Text = dto.PRODUCT_NAME + "(" + dto.PRODUCT_CODE + ")";
                    result.Add(item);
                }
            }

            res.Items = result.ToArray();

            return res;
        }
        catch
        {
            throw;
        }
    }

    #endregion

    #region Country List
    /// <summary>
    /// 2015.04.09 Country조회
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [WebMethod]
    public static AutoCompleteBoxData SearchCountry(RadAutoCompleteContext context)
    {
        AutoCompleteBoxData res = new AutoCompleteBoxData();

        try
        {
            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();
            string searchString = context["Text"].ToString();
            // 사용자 검색				
            using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
            {
                List<DTO_COUNTRY> list = mgr.SelectCountryMaster(searchString);

                foreach (DTO_COUNTRY dto in list)
                {
                    AutoCompleteBoxItemData item = new AutoCompleteBoxItemData();

                    item.Value = dto.COUNTRY_CODE + ":" + dto.EFPIA_FLAG ;
                    item.Text = dto.COUNTRY_NAME + "(" + dto.ISO_CODE + ")";
                    result.Add(item);
                }
            }

            res.Items = result.ToArray();

            return res;
        }
        catch
        {
            throw;
        }
    }
    #endregion
}