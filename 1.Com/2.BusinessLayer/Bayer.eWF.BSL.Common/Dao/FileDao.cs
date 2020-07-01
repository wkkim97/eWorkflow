using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bayer.eWF.BSL.Common.Dao
{
    public class FileDao : DaoBase
    {

        #region SelectFileList
        public List<Dto.DTO_ATTACH_FILES> SelectFileList(string processId, string attachType)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@ATTACH_FILE_TYPE", attachType);
                    var result = context.Database.SqlQuery<Dto.DTO_ATTACH_FILES>(FileContext.USP_SELECT_ATTACH_FILES, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        } 
        #endregion

        #region InsertAttachFIles
        public void InsertAttachFIles(Dto.DTO_ATTACH_FILES file)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[12];
                    parameters[0] = new SqlParameter("@IDX", file.IDX);
                    parameters[1] = new SqlParameter("@PROCESS_ID", file.PROCESS_ID);
                    parameters[2] = new SqlParameter("@SEQ", file.SEQ);
                    parameters[3] = new SqlParameter("@ATTACH_FILE_TYPE", file.ATTACH_FILE_TYPE);
                    parameters[4] = new SqlParameter("@COMMENT_IDX", file.COMMENT_IDX);
                    parameters[5] = new SqlParameter("@DISPLAY_FILE_NAME", file.DISPLAY_FILE_NAME);
                    parameters[6] = new SqlParameter("@FILE_SIZE", file.FILE_SIZE);
                    parameters[7] = new SqlParameter("@SAVED_FILE_NAME", file.SAVED_FILE_NAME);
                    parameters[8] = new SqlParameter("@FILE_PATH", file.FILE_PATH);
                    parameters[9] = new SqlParameter("@FILE_URL", (object)file.FILE_URL ?? DBNull.Value);
                    parameters[10] = new SqlParameter("@CREATOR_ID", file.CREATOR_ID);
                    parameters[11] = new SqlParameter("@CREATE_DATE", file.CREATE_DATE);

                    context.Database.ExecuteSqlCommand(FileContext.USP_INSERT_ATTACH_FILES, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region SelectFileStorage
        public Dto.DTO_FILESTORAGE SelectFileStorage(string subSystemType)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@SubSystemTypeID", subSystemType);
                    var result = context.Database.SqlQuery<Dto.DTO_FILESTORAGE>(FileContext.USP_SELECT_FILESTORAGE, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region SelectFileList
        public  Dto.DTO_ATTACH_FILES SelectFileInfo(int attachIDX)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@IDX", attachIDX);
                    var result = context.Database.SqlQuery<Dto.DTO_ATTACH_FILES>(FileContext.USP_SELECT_ATTACH_FILE_INFO, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion


        #region DeleteAttachFIles
        /// <summary>
        /// 첨부파일 삭제 
        /// </summary>
        /// <param name="idxs">삭제 대상 IDX. ex)"1|2|3"</param>
        public void DeleteAttachFIles(string idxs)
        {
            try
            {
                using (context = new CommonContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@IDX", idxs);

                    context.Database.ExecuteSqlCommand(FileContext.USP_DELETE_ATTACH_FILES, parameters);
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
