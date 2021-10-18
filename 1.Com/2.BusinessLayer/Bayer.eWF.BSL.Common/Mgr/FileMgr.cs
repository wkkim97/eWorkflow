using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data;
using System.Xml;
using System.IO;
using DNSoft.eW.FrameWork;
using System.Web;
using DNSoft.eWF.FrameWork.Web;

namespace Bayer.eWF.BSL.Common.Mgr
{
    public class FileMgr : MgrBase
    {
        #region SelectAttachFileList : 첨부파일 리스트
        public List<Dto.DTO_ATTACH_FILES> SelectAttachFileList(string processID, string attachType)
        {
            try
            {
                using (FileDao dao = new FileDao())
                {
                    return dao.SelectFileList(processID, attachType);
                }
            }
            catch
            {
                throw;
            }
        }
        
        #endregion

        #region SelectAttachFileInfo : 첨부파일 정보
        public Dto.DTO_ATTACH_FILES SelectAttachFileInfo(int idx)
        {
            try
            {
                using (FileDao dao = new FileDao())
                {
                    return dao.SelectFileInfo(idx);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region AddAttachFile : 파일 첨부하기 
        public void AddAttachFile(string processID, string userid, string uploadFolder, List<DTO_ATTACH_FILES> files)
        { 
            DirectoryInfo oDirInfo = null;
            FileInfo oFileInfo = null;

            string strUserID = userid;
  
            DTO_FILESTORAGE storage;
 
            string strFileName = string.Empty;
            string strFilePath = string.Empty;
            string storageFolder = string.Empty; 
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (FileDao dao = new FileDao())
                    {
                        storage = dao.SelectFileStorage(SubSystemType.Approval.ToString());
                        storageFolder = string.Format(@"{0}\{1}\{2}", storage.FilePath, strUserID, processID);
                       
                        oDirInfo = new DirectoryInfo(uploadFolder);
                        if (oDirInfo.Exists)
                        { 
                            if (files != null)
                            {
                                if (files.Count > 0)
                                {
                                    if (!Directory.Exists(storageFolder))
                                        Directory.CreateDirectory(storageFolder);
                                    
                                    foreach (DTO_ATTACH_FILES f in files)
                                    {
                                        strFileName = f.DISPLAY_FILE_NAME;
                                        oFileInfo = new FileInfo(uploadFolder + HttpUtility.UrlDecode(strFileName));
                                        //	파일이 존재한다면 파일을 DB에 데이타를 입력하고 서버로 이동한다.
                                        if (oFileInfo.Exists)
                                        {
                                            f.ATTACH_FILE_TYPE = ApprovalUtil.AttachFileType.Common.ToString();
                                            f.PROCESS_ID = processID;
                                            f.CREATOR_ID = userid;
                                            f.CREATE_DATE = DateTime.Now;
                                            f.FILE_PATH = string.Format(@"{0}\{1}", storageFolder, strFileName);

                                            dao.InsertAttachFIles(f);
                                            oFileInfo.MoveTo(f.FILE_PATH);
                                        }
                                    }
                                    scope.Complete();
                                }
                            }
                        }
              
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(oDirInfo != null)
                {
                    oDirInfo = null;
                }
                if(oFileInfo != null)
                {
                    oFileInfo = null;
                }
            }
        }

        public void AddAttachFile(string processID, string userid, string uploadFolder, DTO_ATTACH_FILES file)
        {
            DirectoryInfo oDirInfo = null;
            FileInfo oFileInfo = null;

            string strUserID = userid;

            DTO_FILESTORAGE storage;

            string strFileName = string.Empty;
            string strFilePath = string.Empty;
            string storageFolder = string.Empty;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (FileDao dao = new FileDao())
                    {
                        storage = dao.SelectFileStorage(SubSystemType.Approval.ToString());
                        storageFolder = string.Format(@"{0}\{1}\{2}", storage.FilePath, strUserID, processID);

                        oDirInfo = new DirectoryInfo(uploadFolder);
                        if (oDirInfo.Exists)
                        {
                            if (file != null)
                            {
                                if (!Directory.Exists(storageFolder))
                                    Directory.CreateDirectory(storageFolder);
 
                                strFileName = file.DISPLAY_FILE_NAME;
                                oFileInfo = new FileInfo(uploadFolder + HttpUtility.UrlDecode(strFileName));
                                //	파일이 존재한다면 파일을 DB에 데이타를 입력하고 서버로 이동한다.
                                if (oFileInfo.Exists)
                                {
                                    file.PROCESS_ID = processID;
                                    file.CREATOR_ID = userid;
                                    file.CREATE_DATE = DateTime.Now;

                                    string fullFilePath = string.Format(@"{0}\{1}", storageFolder, strFileName);
                                    if(File.Exists(fullFilePath))
                                    {
                                        strFileName = strFileName.Substring(0, strFileName.LastIndexOf('.')) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + strFileName.Substring(strFileName.LastIndexOf('.') );
                                        fullFilePath = string.Format(@"{0}\{1}", storageFolder, strFileName);
                                    }
                                    file.FILE_PATH = fullFilePath;

                                    dao.InsertAttachFIles(file);
                                    oFileInfo.MoveTo(file.FILE_PATH);
                                }

                                scope.Complete();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oDirInfo != null)
                {
                    oDirInfo = null;
                }
                if (oFileInfo != null)
                {
                    oFileInfo = null;
                }
            }
        }
        #endregion


        #region MoveToFile
        public void MoveToFile(string processID, string userid, string targetFolder, List<DTO_ATTACH_FILES> files)
        {
            DirectoryInfo oDirInfo = null;
            FileInfo oFileInfo = null;

            string strUserID = userid;
 
            string strFileName = string.Empty;
            string strFilePath = string.Empty;
            string storageFolder = string.Empty;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (FileDao dao = new FileDao())
                    {
                        storageFolder = targetFolder;

                        foreach (DTO_ATTACH_FILES file in files)
                        {
                            if (file != null)
                            {
                                if (!Directory.Exists(storageFolder))
                                    Directory.CreateDirectory(storageFolder);

                                strFileName = file.DISPLAY_FILE_NAME;
                                oFileInfo = new FileInfo(file.FILE_PATH);
                                //	파일이 존재한다면 파일을 DB에 데이타를 입력하고 서버로 이동한다.
                                if (oFileInfo.Exists)
                                {
                                    file.PROCESS_ID = processID;
                                    file.CREATOR_ID = userid;
                                    file.CREATE_DATE = DateTime.Now;
                                    file.ATTACH_FILE_TYPE = "Temp";
                                    string fullFilePath = string.Format(@"{0}\{1}", storageFolder, strFileName);
                                    if (File.Exists(fullFilePath))
                                    {
                                        strFileName = strFileName.Substring(0, strFileName.LastIndexOf('.')) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + strFileName.Substring(strFileName.LastIndexOf('.') + 1);
                                        fullFilePath = string.Format(@"{0}\{1}", storageFolder, strFileName);
                                    }
                                    file.FILE_PATH = fullFilePath;

                                    dao.InsertAttachFIles(file);
                                    oFileInfo.MoveTo(file.FILE_PATH);
                                } 
                            } 
                        }
                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oDirInfo != null)
                {
                    oDirInfo = null;
                }
                if (oFileInfo != null)
                {
                    oFileInfo = null;
                }
            }
        } 
        #endregion


        #region InsertAttachFile
        public void InsertAttachFile(List<DTO_ATTACH_FILES> files)
        {
            DirectoryInfo oDirInfo = null;
            FileInfo oFileInfo = null;

            string strFileName = string.Empty;
            string strFilePath = string.Empty;

            try
            {
                using (FileDao dao = new FileDao())
                {
                    if (files != null)
                    {
                        if (files.Count > 0)
                        {

                            foreach (DTO_ATTACH_FILES f in files)
                            {
                                oFileInfo = new FileInfo(f.FILE_PATH);
                                //	파일이 존재한다면 파일을 DB에 데이타를 입력하고 서버로 이동한다.
                                if (oFileInfo.Exists)
                                {
                                    dao.InsertAttachFIles(f);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oDirInfo != null)
                {
                    oDirInfo = null;
                }
                if (oFileInfo != null)
                {
                    oFileInfo = null;
                }
            }
        } 
        #endregion

        #region DeleteAttachFIles
        public void DeleteAttachFIles(string idxs)
        {
            try
            {
                using (FileDao dao = new FileDao())
                {
                    dao.DeleteAttachFIles(idxs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            
            }
        }
        #endregion
 
    }
}
