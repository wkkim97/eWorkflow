using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class AbsenceDao : DaoBase
    {
        #region [Absence]

        public void InsertAbsence(Dto.DTO_ABSENCE absence)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@USER_ID", absence.USER_ID);
                    parameters[1] = new SqlParameter("@APPROVER_ID", absence.APPROVER_ID);
                    parameters[2] = new SqlParameter("@FROM_DATE", absence.FROM_DATE);
                    parameters[3] = new SqlParameter("@TO_DATE", absence.TO_DATE);
                    parameters[4] = new SqlParameter("@DESCRIPTION", absence.DESCRIPTION);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_COMMON_ABSENCE, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteAbsence(int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_ABSENSE, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //ABSENCE List 조회
        public List<Dto.DTO_ABSENCE> SelectAbsenceList(string userid)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@USER_ID", userid);
                    var result = context.Database.SqlQuery<Dto.DTO_ABSENCE>(ApprovalContext.USP_SELECT_ABSENCE_LIST, parameters);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dto.DTO_ABSENCE SelectAbsence(int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@IDX", idx);
                    var result = context.Database.SqlQuery<Dto.DTO_ABSENCE>(ApprovalContext.USP_SELECT_ABSENCE, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdateAbsence(Dto.DTO_ABSENCE absence)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@IDX", absence.IDX);
                    parameters[1] = new SqlParameter("@APPROVER_ID", absence.APPROVER_ID);
                    parameters[2] = new SqlParameter("@FROM_DATE", absence.FROM_DATE);
                    parameters[3] = new SqlParameter("@TO_DATE", absence.TO_DATE);
                    parameters[4] = new SqlParameter("@DESCRIPTION", absence.DESCRIPTION);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_UPDATE_COMMON_ABSENCE, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
