using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common
{
    public class FileContext : WorkflowContext
    {
        public const string USP_INSERT_ATTACH_FILES = "[dbo].[USP_INSERT_ATTACH_FILES] @IDX, @PROCESS_ID, @SEQ, @ATTACH_FILE_TYPE, @COMMENT_IDX, @DISPLAY_FILE_NAME, @SAVED_FILE_NAME, @FILE_SIZE, @FILE_PATH, @FILE_URL, @CREATOR_ID, @CREATE_DATE";

        public const string USP_SELECT_ATTACH_FILES = "[dbo].[USP_SELECT_ATTACH_FILES] @PROCESS_ID, @ATTACH_FILE_TYPE";

        public const string USP_SELECT_ATTACH_FILE_INFO = "[dbo].[USP_SELECT_ATTACH_FILE_INFO] @IDX";

        public const string USP_SELECT_FILESTORAGE = "[eManage].[dbo].[USP_SELECT_FILESTORAGE] @SubSystemTypeID";

        public const string USP_DELETE_ATTACH_FILES = "[dbo].[USP_DELETE_ATTACH_FILES] @IDX";
    }
}
