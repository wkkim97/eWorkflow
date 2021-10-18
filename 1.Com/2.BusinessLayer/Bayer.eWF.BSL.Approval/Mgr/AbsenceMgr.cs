using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Bayer.eWF.BSL.Approval.Dao;
using Bayer.eWF.BSL.Approval.Dto;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class AbsenceMgr : MgrBase
    {
        #region [Absence]

        /// <summary>
        /// Absence 저장
        /// </summary>
        /// <param name="absence"></param>
        public void InsertAbsence(Dto.DTO_ABSENCE absence)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using(Dao.AbsenceDao dao = new Dao.AbsenceDao())
                    {
                        dao.InsertAbsence(absence);
                            
                    }
                    scope.Complete();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Absence List 조회
        /// </summary>
        /// <returns></returns>
        public List<Dto.DTO_ABSENCE> SelectAbsenceList(string userid)
        {
            try
            {
                using (AbsenceDao dao = new AbsenceDao())
                {
                    return dao.SelectAbsenceList(userid);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Absence 1건 조회
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Dto.DTO_ABSENCE SelectAbsence(int idx)
        {
            try
            {
                using (AbsenceDao dao = new AbsenceDao())
                {
                    return dao.SelectAbsence(idx);
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Absence 삭제
        /// </summary>
        /// <param name="absence"></param>
        public void DeleteAbsence(int idx)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (AbsenceDao dao = new AbsenceDao())
                    {
                        dao.DeleteAbsence(idx);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateAbsence(Dto.DTO_ABSENCE absence)
        {
            try
            {
                using(TransactionScope scope = new TransactionScope())
                {
                    using (AbsenceDao dao = new AbsenceDao())
                    {
                        dao.UpdateAbsence(absence);
                    }
                    scope.Complete();
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
