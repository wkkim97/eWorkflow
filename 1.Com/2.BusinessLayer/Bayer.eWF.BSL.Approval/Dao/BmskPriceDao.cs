using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class BmskPriceDao : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergeBmskPrice(Dto.DTO_DOC_BMSK_PRICE doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BMSK_PRICE, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void InsertBmskPriceList(Dto.DTO_DOC_BMSK_PRICE_LIST list)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(list);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_BMSK_PRICE_LIST, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBmskPriceListAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BMSK_PRICE_LIST_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBmskPriceList(string processId, int index)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", index);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BMSK_PRICE_LIST, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_BMSK_PRICE SelectBmskPrice(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BMSK_PRICE>(ApprovalContext.USP_SELECT_BMSK_PRICE, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_BMSK_PRICE_LIST> SelectBmskPriceList(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BMSK_PRICE_LIST>(ApprovalContext.USP_SELECT_BMSK_PRICE_LIST, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
