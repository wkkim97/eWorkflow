﻿using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class BKLPriceAndMarginDao : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergeBKLPriceAndMargin(Dto.DTO_DOC_BKL_PRICE_AND_MARGIN doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BKL_PRICE_AND_MARGIN, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeBKLPriceAndMarginCustomer(Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER customer)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(customer);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BKL_PRICE_AND_MARGIN_CUSTOMER, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeBKLPriceAndMarginHospital(Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL hospital)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(hospital);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BKL_PRICE_AND_MARGIN_HOSPITAL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeBKLPriceAndMarginProduct(Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BKL_PRICE_AND_MARGIN_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBKLPriceAndMarginCustomerAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BKL_PRICE_AND_MARGIN_CUSTOMER_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBKLPriceAndMarginHospitalAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BKL_PRICE_AND_MARGIN_HOSPITAL_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBKLPriceAndMarginProduct(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BKL_PRICE_AND_MARGIN_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_BKL_PRICE_AND_MARGIN SelectBKLPriceAndMargin(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN>(ApprovalContext.USP_SELECT_BKL_PRICE_AND_MARGIN, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER> SelectBKLPriceAndMarginCustomer(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER>(ApprovalContext.USP_SELECT_BKL_PRICE_AND_MARGIN_CUSTOMER, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL> SelectBKLPriceAndMarginHospital(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL>(ApprovalContext.USP_SELECT_BKL_PRICE_AND_MARGIN_HOSPITAL, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT> SelectBKLPriceAndMarginProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>(ApprovalContext.USP_SELECT_BKL_PRICE_AND_MARGIN_PRODUCT, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBKLPriceAndMarginProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BKL_PRICE_AND_MARGIN_PRODUCT_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        public void Update_Dealno(string processId,string dealo, string user_id)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@DEAL_NO", dealo);
                    parameters[2] = new SqlParameter("@CREATOR_ID", user_id);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BKL_PRICE_AND_MARGIN_DEAL_NO, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
