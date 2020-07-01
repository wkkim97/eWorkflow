using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dao
{
    public class CommonDao : DaoBase
    {

        #region [Document List]

        public List<Dto.DocumentListDto> SelectDocumentList(string companyCode)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@COMPANY_CODE", companyCode);
                    var result = context.Database.SqlQuery<Dto.DocumentListDto>(CommonContext.USP_SELECT_DOCUMENT_LIST, parameters); 

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [Business Place]

        public List<Dto.DTO_BUSINESS_PLACE> SelectBusinessPlace(string companyCode)
        {
            try
            {
                using (context = new CommonContext())
                {
                    //var result = context.Database.SqlQuery<Dto.DTO_BUSINESS_PLACE>(CommonContext.USP_SELECT_BUSINESS_PLACE);

                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@COMPANY_CODE", companyCode);
                    var result = context.Database.SqlQuery<Dto.DTO_BUSINESS_PLACE>(CommonContext.USP_SELECT_BUSINESS_PLACE, parameters); 


                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [ Company ]

        public List<Dto.DTO_COMPANY> SelectCompanyList(string keyWord)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@KEYWORD", keyWord);

                    return context.Database.SqlQuery<Dto.DTO_COMPANY>(CommonContext.USP_SELECT_COMPANY, parameters).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 바이엘 3개 회사만 조회
        /// </summary>
        /// <returns></returns>
        public List<Dto.DTO_COMPANY> SelectBayerCompanyList()
        {
            try
            {
                using (context = new CommonContext())
                {
                    return context.Database.SqlQuery<Dto.DTO_COMPANY>(CommonContext.USP_SELECT_BAYER_COMPANY).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [Customer]

        public List<Dto.DTO_CUSTOMER> SelectCustomer(string companycode, string bu, string parvw, string keyword, string level)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@COMPANY_CODE", companycode);
                    parameters[1] = new SqlParameter("@BU", bu);
                    parameters[2] = new SqlParameter("@PARVW", parvw);
                    parameters[3] = new SqlParameter("@KEYWORD", keyword);
                    parameters[4] = new SqlParameter("@LEVEL", level);

                    var result = context.Database.SqlQuery<Dto.DTO_CUSTOMER>(CommonContext.USP_SELECT_CUSTOMER, parameters);
                    return result.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        public void InsertCustomer(Dto.DTO_CUSTOMER doc)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(CommonContext.USP_INSERT_CUSTOMER_MASTER, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertCustomer_KPIS(Dto.DTO_CUSTOMER_KPIS doc)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(CommonContext.USP_INSERT_CUSTOMER_MASTER_KPIS, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        #endregion

        #region [ Membership ]

        public List<Dto.DTO_MEMBERSHIP> SelectMembershipList(string keyWord)
        {
            try
            {
                using (context = new CommonContext())
                {
                    var result = context.Database.SqlQuery<Dto.DTO_MEMBERSHIP>(CommonContext.USP_SELECT_MEMBERSHIP);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [ Product ]

        public List<Dto.DTO_PRODUCT> SelectProduct(string company, string bu, string keyword, string baseprice, string sampleType, string existSample)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[6];
                    parameters[0] = new SqlParameter("@COMPANY_CODE", company);
                    parameters[1] = new SqlParameter("@BU", bu);
                    parameters[2] = new SqlParameter("@KEYWORD", keyword);
                    parameters[3] = new SqlParameter("@BASE_PRICE", baseprice);
                    parameters[4] = new SqlParameter("@SAMPLE_TYPE", sampleType);
                    parameters[5] = new SqlParameter("@EXIST_SAMPLE", existSample);

                    var result = context.Database.SqlQuery<Dto.DTO_PRODUCT>(CommonContext.USP_SELECT_PRODUCT, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public void InsertProduct(Dto.DTO_PRODUCT doc)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(CommonContext.USP_INSERT_PRODUCT_MASTER, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertProduct_KPIS(Dto.DTO_PRODUCT_KPIS doc)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(CommonContext.USP_INSERT_PRODUCT_MASTER_KPIS, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        


        #endregion

        #region [ Country & City ]

        public List<Dto.DTO_COUNTRY_CITY> SelectCountry()
        {
            try
            {
                using (context = new CommonContext())
                {
                    var result = context.Database.SqlQuery<Dto.DTO_COUNTRY_CITY>(CommonContext.USP_SELECT_COUNTRY);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_COUNTRY_CITY> SelectCity(string country)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@COUNTRY", country);
                    var result = context.Database.SqlQuery<Dto.DTO_COUNTRY_CITY>(CommonContext.USP_SELECT_CITY, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        } 

        #endregion

        #region doctor 
        public List<Dto.DTO_DOCTOR> SelectDoctor(string keyword)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];                    
                    parameters[0] = new SqlParameter("@KEYWORD", keyword);

                    var result = context.Database.SqlQuery<Dto.DTO_DOCTOR>(CommonContext.USP_SELECT_DOCTOR, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw ;
            }
        }
        #endregion

        #region pharmacy
        public List<Dto.DTO_PHARMACY> SelectPharmacy(string keyword)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@KEYWORD", keyword);

                    var result = context.Database.SqlQuery<Dto.DTO_PHARMACY>(CommonContext.USP_SELECT_PHARMACY, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [commonPIActivity]
        public List<Dto.DTO_COMMON_PIACTIVITY> SelectCommonPIActivity(int year)
        {
            try
            {
                using(context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@YEAR", year);

                    var result = context.Database.SqlQuery<Dto.DTO_COMMON_PIACTIVITY>(CommonContext.USP_SELECT_COMMON_P_I_ACTIVITY, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }           
        }
        #endregion

        #region [commonAHPIActivity]
        public List<Dto.DTO_COMMON_AH_PIACTIVITY> SelectCommonAHPIActivity(int year)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@YEAR", year);

                    var result = context.Database.SqlQuery<Dto.DTO_COMMON_AH_PIACTIVITY>(CommonContext.USP_SELECT_COMMON_AH_P_I_ACTIVITY, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion


        #region [Customer Master]
        public List<Dto.DTO_CUSTOMER> SelectCustomerMaster(string companycode, string bu, string parvw, string visibility, string kpis, string keyword)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[6];
                    parameters[0] = new SqlParameter("@COMPANY_TYPE", companycode);
                    parameters[1] = new SqlParameter("@BU_TYPE", bu);
                    parameters[2] = new SqlParameter("@PARVW", parvw);
                    parameters[3] = new SqlParameter("@VISIBILITY", visibility);
                    parameters[4] = new SqlParameter("@KPIS", kpis);
                    parameters[5] = new SqlParameter("@KEYWORD", keyword);

                    var result = context.Database.SqlQuery<Dto.DTO_CUSTOMER>(CommonContext.USP_SELECT_CUSTOMER_MASTER, parameters);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dto.DTO_CUSTOMER_KPIS> SelectCustomerMaster_KPIS(string companycode, string bu, string keyword)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@COMPANY_TYPE", companycode);
                    parameters[1] = new SqlParameter("@BU_TYPE", bu);
                    parameters[2] = new SqlParameter("@KEYWORD", keyword);

                    var result = context.Database.SqlQuery<Dto.DTO_CUSTOMER_KPIS>(CommonContext.USP_SELECT_CUSTOMER_MASTER_KPIS, parameters);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustomerMaster(Dto.DTO_CUSTOMER customer)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(customer);

                    context.Database.ExecuteSqlCommand(CommonContext.USP_UPDATE_CUSTOMER_MASTER, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCustomerMaster_KPIS(Dto.DTO_CUSTOMER_KPIS customer)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(customer);

                    context.Database.ExecuteSqlCommand(CommonContext.USP_UPDATE_CUSTOMER_MASTER_KPIS, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteCustomerMaster_KPIS(string customercode,string user_id)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@BCNC_CODE", customercode);
                    parameters[1] = new SqlParameter("@UPDATER_ID", user_id);

                    context.Database.ExecuteSqlCommand(CommonContext.USP_DELETE_CUSTOMER_MASTER_KPIS, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> SelectCustomerMasterBu(string companycode)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@COMPANY_CODE", companycode);

                    var result = context.Database.SqlQuery<string>(CommonContext.USP_SELECT_CUSTOMER_BU_LIST, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdateCustomerMasterCreditLimit(string customercode, string parvw, decimal creditlimit, string updaterid)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[4];
                    parameters[0] = new SqlParameter("@CUSTOMER_CODE", customercode);
                    parameters[1] = new SqlParameter("@PARVW", parvw);
                    parameters[2] = new SqlParameter("@CREDIT_LIMIT", creditlimit);
                    parameters[3] = new SqlParameter("@UPDATER_ID", updaterid);

                    context.Database.ExecuteSqlCommand(CommonContext.USP_UPDATE_CUSTOMER_MASTER_CREDIT_LIMIT, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SelectProductMaster
        public List<Dto.DTO_PRODUCT> SelectProductMaster(string companytype, string butype, string visibility, string kpis, string keyword)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@COMPANY_TYPE", companytype);
                    parameters[1] = new SqlParameter("@BU_TYPE", butype);
                    parameters[2] = new SqlParameter("@VISIBILITY", visibility);
                    parameters[3] = new SqlParameter("@KPIS", kpis);
                    parameters[4] = new SqlParameter("@KEYWORD", keyword);


                    var result = context.Database.SqlQuery<Dto.DTO_PRODUCT>(CommonContext.USP_SELECT_PRODUCT_MASTER, parameters);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Dto.DTO_PRODUCT_KPIS> SelectProductMaster_KPIS(string companytype, string butype, string keyword)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@COMPANY_TYPE", companytype);
                    parameters[1] = new SqlParameter("@BU_TYPE", butype);                   
                    parameters[2] = new SqlParameter("@KEYWORD", keyword);


                    var result = context.Database.SqlQuery<Dto.DTO_PRODUCT_KPIS>(CommonContext.USP_SELECT_PRODUCT_MASTER_KPIS, parameters);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         // 회사코드별로  BU 가져오기
         public List<string> SelectProductMasterBu(string type)
         {
             try
             {
                 using (context = new CommonContext())
                 {
                     SqlParameter[] parameters = new SqlParameter[1];
                     parameters[0] = new SqlParameter("@SEARCH_TYPE", type);

                     var result = context.Database.SqlQuery<string>(CommonContext.USP_SELECT_PRODUCT_BU_LIST, parameters);

                     return result.ToList();
                 }
             }
             catch
             {
                 throw;
             }
         }
        #endregion

        #region UpdateProductMaster
         public void UpdateProductMaster(Dto.DTO_PRODUCT item)
         {
             try
             {
                 using (context = new CommonContext())
                 {
                     SqlParameter[] parameters = ParameterMapper.Mapping(item);

                     context.Database.ExecuteSqlCommand(CommonContext.USP_UPDATE_PRODUCT_MASTER, parameters);
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         public void UpdateProductMaster_KPIS(Dto.DTO_PRODUCT_KPIS item)
         {
             try
             {
                 using (context = new CommonContext())
                 {
                     SqlParameter[] parameters = ParameterMapper.Mapping(item);

                     context.Database.ExecuteSqlCommand(CommonContext.USP_UPDATE_PRODUCT_MASTER_KPIS, parameters);
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
        #endregion
         #region DeleteProductMaster_KPIS
         public void DeleteProductMaster_KPIS(string product_code,string cwid)
         {
             try
             {
                 using (context = new CommonContext())
                 {
                     SqlParameter[] parameters = new SqlParameter[2];
                     parameters[0] = new SqlParameter("@PRDUCT_CODE", product_code);
                     parameters[1] = new SqlParameter("@UPDATER_ID", cwid);

                     context.Database.ExecuteSqlCommand(CommonContext.USP_DELETE_PRODUCT_MASTER_KPIS, parameters);
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         #endregion

        #region [Country Master]
        public List<Dto.DTO_COUNTRY> SelectCountryMaster(string keyword)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@KEYWORD", keyword);

                    var result = context.Database.SqlQuery<Dto.DTO_COUNTRY>(CommonContext.USP_SELECT_COUNTRY_MASTER, parameters);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

         public List<Dto.DTO_USER_CONFIG_DOC_SORT> SelectAdminDocList(string user_id)
         {
             try
             {
                 using (context = new CommonContext())
                 {
                     SqlParameter[] parameters = new SqlParameter[1];
                     parameters[0] = new SqlParameter("@USER_ID", user_id);
                     var result = context.Database.SqlQuery<Dto.DTO_USER_CONFIG_DOC_SORT>(CommonContext.USP_SELECT_ADMIN_DOC_LIST, parameters);

                     return result.ToList();
                 }
             }
             catch
             {
                 throw;
             }
         }
         public List<Dto.DTO_USER_CONFIG_DOC_SORT> SelectAdminDocList_detailreport(string user_id)
         {
             try
             {
                 using (context = new CommonContext())
                 {
                     SqlParameter[] parameters = new SqlParameter[1];
                     parameters[0] = new SqlParameter("@USER_ID", user_id);
                     var result = context.Database.SqlQuery<Dto.DTO_USER_CONFIG_DOC_SORT>(CommonContext.USP_SELECT_ADMIN_DOC_LIST_DETAILREPORT, parameters);

                     return result.ToList();
                 }
             }
             catch
             {
                 throw;
             }
         }
         public DataTable Download_Detail_Report(string DocumentID, DateTime? FROM_DATE, DateTime? TO_DATE)
         {
             DataTable dt = new DataTable();
             try
             {                           
                 using (context = new CommonContext())
                 {
                     var conn = context.Database.Connection;
                     var cmd = conn.CreateCommand();
                     var connectionState = conn.State;
                     if (connectionState != ConnectionState.Open)
                         conn.Open();
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.CommandText = "USP_REPORTING_DOCUMENT";
                     cmd.Parameters.Add(new SqlParameter ("@DOCUMENT_ID",DocumentID));
                     cmd.Parameters.Add(new SqlParameter("@FROM_DATE", FROM_DATE));
                     cmd.Parameters.Add(new SqlParameter("@TO_DATE", TO_DATE));


                   //  SqlParameter[] parameters = new SqlParameter[3];

                    // parameters[0] = new SqlParameter("@DOCUMENT_ID", DocumentID);
                    // parameters[1] = new SqlParameter("@FROM_DATE", FROM_DATE);
                    // parameters[2] = new SqlParameter("@TO_DATE", TO_DATE);
                    // cmd.Parameters.Add(parameters);
                    
                     
                     using (var reader = cmd.ExecuteReader())
                     {
                         dt.Load(reader);
                         
                     }
                     if (connectionState != ConnectionState.Open)
                         conn.Close();


                 }
             }
             catch(Exception ex)
             {
                 throw ex;
             }
             finally
             {                 
                     
             }
             return dt;

         }


         #region LOGIN_HISTORY

         public List<Dto.DTO_LOGIN_HISTORY> SelectLoginHistory(string user_id)
         {
             try
             {
                 using (context = new CommonContext())
                 {
                     SqlParameter[] parameters = new SqlParameter[1];
                     parameters[0] = new SqlParameter("@USER_ID", user_id);

                     var result = context.Database.SqlQuery<Dto.DTO_LOGIN_HISTORY>(CommonContext.USP_SELECT_LOGIN_HISTORY, parameters);

                     return result.ToList();
                 }
             }
             catch
             {
                 throw;
             }
         }

         public void InsertLoginHistory(string strUserID, string ClientIP, string loginUserName, string machineName)
         {
             try
             {
                 using (context = new CommonContext())
                 {
                     SqlParameter[] parameters = new SqlParameter[4];
                     parameters[0] = new SqlParameter("@USER_ID", strUserID);
                     parameters[1] = new SqlParameter("@CLIENTIP", ClientIP);
                     parameters[2] = new SqlParameter("@WINDOWUSERNAME", loginUserName);
                     parameters[3] = new SqlParameter("@WINDOWDOMAINNAME", machineName);

                     context.Database.ExecuteSqlCommand(CommonContext.USP_INSERT_LOGIN_HISTORY, parameters);
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         } 


         #endregion
         public ConnectionState connectionState { get; set; }
    }
}
