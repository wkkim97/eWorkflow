using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Bayer.eWF.BSL.Common.Mgr
{
    public class CommonMgr : MgrBase
    {
        #region SelectDocumentList
        public List<Dto.DocumentListDto> SelectDocumentList(string companyCode)
        {
            try
            {
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectDocumentList(companyCode);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region SelectBusinessPlace
        public List<Dto.DTO_BUSINESS_PLACE> SelectBusinessPlace(string companyCode)
        {
            try
            {
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectBusinessPlace(companyCode);
                }
            }
            catch(Exception ex)
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
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectCompanyList(keyWord);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Bayer 3개 회사만 조회
        /// </summary>
        /// <returns></returns>
        public List<Dto.DTO_COMPANY> SelectBayerCompanyList()
        {
            try
            {
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectBayerCompanyList();
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
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectCustomer(companycode, bu, parvw, keyword, level);
                }
            }
            catch(Exception ex)
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
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectMembershipList(keyWord);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [ Product ]

        public List<Dto.DTO_PRODUCT> SelectProduct(string company, string bu, string keyword, string baseprice)
        {
            return SelectProduct(company, bu, keyword, baseprice, "", "");
        }

        public List<Dto.DTO_PRODUCT> SelectProduct(string company, string bu, string keyword, string baseprice, string sampleType, string existSample)
        {
            try
            {
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectProduct(company, bu, keyword, baseprice, sampleType, existSample);
                }
            }
            catch
            {
                throw;
            }
        }

        //회사코드 별로 BU 가져오기
        public List<string> SelectProductMasterBu(string type)
        {
            try
            {
                using (Dao.CommonDao dao = new Dao.CommonDao())
                {
                    return dao.SelectProductMasterBu(type);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [ Country & City ]

        public List<Dto.DTO_COUNTRY_CITY> SelectCountry()
        {
            try
            {
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectCountry();
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
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectCity(country);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [ Doctor & Pharmacy ]

        public List<Dto.DTO_DOCTOR> SelectDoctor(string keyword)
        {
            try
            {
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectDoctor(keyword);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_PHARMACY> SelectPharmacy(string keyword)
        {
            try
            {
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectPharmacy(keyword);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [CommonPIActivity]
        public List<Dto.DTO_COMMON_PIACTIVITY> SelectCommonPIActivity(int year)
        {
            try
            {
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectCommonPIActivity(year);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [CommonAHPIActivity]
        public List<Dto.DTO_COMMON_AH_PIACTIVITY> SelectCommonAHPIActivity(int year)
        {
            try
            {
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectCommonAHPIActivity(year);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [CustomerMaster]
        public List<Dto.DTO_CUSTOMER> SelectCustomerMaster(string companycode, string bu,string parvw,string visibility,string kpis, string keyword)
        {
            try
            {
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectCustomerMaster(companycode, bu,parvw,visibility,kpis, keyword);
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
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectCustomerMaster_KPIS(companycode, bu, keyword);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateCustomerMaster(List<Dto.DTO_CUSTOMER> list)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (CommonDao dao = new CommonDao())
                    {
                        foreach (DTO_CUSTOMER customer in list)
                        {
                            dao.UpdateCustomerMaster(customer);
                        }
                        scope.Complete();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public void UpdateCustomerMaster_KPIS(List<Dto.DTO_CUSTOMER_KPIS> list)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (CommonDao dao = new CommonDao())
                    {
                        foreach (DTO_CUSTOMER_KPIS customer in list)
                        {
                            dao.UpdateCustomerMaster_KPIS(customer);
                        }
                        scope.Complete();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public void DeleteCustomerMaster_KPIS(string customercode,string user_id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (CommonDao dao = new CommonDao())
                    {

                        dao.DeleteCustomerMaster_KPIS(customercode, user_id);
                        
                        scope.Complete();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public List<string> SelectCustomerMasterBu(string companycode)
        {
            try
            {
                using (Dao.CommonDao dao = new Dao.CommonDao())
                {
                    return dao.SelectCustomerMasterBu(companycode);
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
                using (CommonDao dao = new CommonDao())
                {
                    dao.UpdateCustomerMasterCreditLimit(customercode, parvw, creditlimit, updaterid);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [ProductMaster]
         public List<Dto.DTO_PRODUCT> SelectProductMaster(string companytype, string butype,string visibility,string kpis, string keyword)
        {
            try
            {
                using (CommonDao dao = new CommonDao())
                {
                    return dao.SelectProductMaster(companytype, butype,visibility,kpis, keyword);
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
                 using (CommonDao dao = new CommonDao())
                 {
                     return dao.SelectProductMaster_KPIS(companytype, butype, keyword);
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public void UpdateProductMaster(List<Dto.DTO_PRODUCT> list)
         {
             try
             {
                 using (TransactionScope scope = new TransactionScope())
                 {
                     using (CommonDao dao = new CommonDao())
                     {
                         foreach (DTO_PRODUCT item in list)
                         {
                             dao.UpdateProductMaster(item);
                         }
                         scope.Complete();
                     }
                 }
             }
             catch
             {
                 throw;
             }
         }
         public void UpdateProductMaster_KPIS(List<Dto.DTO_PRODUCT_KPIS> list)
         {
             try
             {
                 using (TransactionScope scope = new TransactionScope())
                 {
                     using (CommonDao dao = new CommonDao())
                     {
                         foreach (DTO_PRODUCT_KPIS item in list)
                         {
                             dao.UpdateProductMaster_KPIS(item);
                         }
                         scope.Complete();
                     }
                 }
             }
             catch
             {
                 throw;
             }
         }
         public void DeleteProductMaster_KPIS(string product_code,string user_id)
         {
             try
             {
                 using (TransactionScope scope = new TransactionScope())
                 {
                     using (CommonDao dao = new CommonDao())
                     {

                         dao.DeleteProductMaster_KPIS(product_code, user_id);
                         
                         scope.Complete();
                     }
                 }
             }
             catch
             {
                 throw;
             }
         }
        #endregion

         #region [CountryMaster]
         public List<Dto.DTO_COUNTRY> SelectCountryMaster(string keyword)
         {
             try
             {
                 using (CommonDao dao = new CommonDao())
                 {
                     return dao.SelectCountryMaster(keyword);
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
                 using (Dao.CommonDao dao = new CommonDao())
                 {
                     return dao.SelectAdminDocList(user_id);
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
                 using (Dao.CommonDao dao = new CommonDao())
                 {
                     return dao.SelectAdminDocList_detailreport(user_id);
                 }
             }
             catch
             {
                 throw;
             }
         }

        public List<Dto.DTO_LOGIN_HISTORY> SelectLoginHistory(string user_id)
         {
             try
             {
                 using (Dao.CommonDao dao = new CommonDao())
                 {
                     return dao.SelectLoginHistory(user_id);
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
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {
                        dao.InsertLoginHistory(strUserID, ClientIP, loginUserName, machineName);

                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
