using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common
{
    public class NoticeContext : WorkflowContext
    {
        public const string USP_SELECT_BOARD_NOTICE = "[dbo].[USP_SELECT_BOARD_NOTICE] @ROW_NUM";

        public const string USP_SELECT_BOARD_NOTICE_ITEM = "[dbo].[USP_SELECT_BOARD_NOTICE_ITEM] @IDX";

        public const string USP_DELETE_BOARD_NOTICE_ITEM = "[dbo].[USP_DELETE_BOARD_NOTICE_ITEM] @IDX"; 

        public const string USP_MERGE_BOARD_NOTICE = "[dbo].[USP_MERGE_BOARD_NOTICE] @IDX, @SUBJECT, @BODY, @CREATE_ID, @CREATE_DATE, @UPDATE_ID, @UPDATE_DATE";
    }
}
