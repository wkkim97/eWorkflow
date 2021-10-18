using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNSoft.eWF.FrameWork.Web
{ 
    public class ApprovalUtil : IDisposable
    {
        /// <summary>
        /// Display rule of the button by status and person
        /// </summary>
        private static int[][] StatusMetrix = new int[][]
                                            { new int[] { 1, 0 , 0 , 0, 0, 0, 0, 0, 1 , 1 , 0, 0 }, // Requester New
                                              new int[] { 0, 0 , 0 , 0, 0, 1, 0, 1, 1 , 0 , 0, 0 }, // Requester On Going
                                              new int[] { 0, 0 , 0 , 0, 1, 0, 0, 0, 1 , 0 , 1, 1 }, // Requester Completed
                                              new int[] { 0, 1 , 1 , 1, 0, 0, 0, 0, 1 , 0 , 0, 0 }, // Approver On Going
                                              new int[] { 0, 0 , 0 , 0, 1, 0, 0, 0, 1 , 0 , 1, 0 }, // Approver Completed
                                              new int[] { 0, 0 , 0 , 0, 1, 0, 1, 0, 1 , 0 , 1, 0 }, // Recipient Completed
                                              new int[] { 0, 0 , 0 , 0, 1, 0, 0, 0, 1 , 0 , 1, 0 },  // Reviewer Completed
                                              new int[] { 0, 0 , 0 , 0, 0, 0, 0, 0, 1 , 0 , 0, 1 },  // Default
                                              new int[] { 0, 0 , 0 , 0, 0, 0, 0, 0, 1 , 0 , 0, 0 }  // None
                                            };
        /// <summary>
        /// 결재 작성시 버튼
        /// </summary>
        public enum ApprovalButtons : int { 
            REQUEST =0 , 
            APPROVAL, 
            FORWARDAPPRVAL, 
            REJECT, 
            FORWARD, 
            RECALL, 
            WITHDRAW, 
            REMIND, 
            EXIT, 
            SAVE, 
            INPUTCOMMENT ,
            REUSE
        }

        /// <summary>
        /// 작성화면 상태
        /// </summary>
        public  enum ApprovalViewStatus : int { 
            NEW_REQUESTER = 0, 
            ON_GOING_REQUESTER, 
            COMPLETED_REQUESTER, 
            ON_GOING_APPROVER, 
            COMPLETED_APPROVER, 
            COMPLETED_RECIPIENT, 
            COMPLETED_REVIEWER,
            DEFAULT,
            EXIT
        }

        public static int[] GetApprovalButtonAuthList(int index)
        {
            return StatusMetrix[index];
        }

        /// <summary>
        /// 문서 상태 
        /// </summary>
        public enum ApprovalStatus
        {
            Temp = 0,
            Request = 1,
            Processing = 2,
            Completed = 3,
            Reject = 4,
            Saved = 5,
            Recall = 6,
            Withdraw = 7
        }

        /// <summary>
        /// 첨부 파일
        /// </summary>
        public enum AttachFileType
        {
            Temp    // 임시저장
            ,Common // 일반첨부
            ,Quotation // 출장보고서 견적서
            ,VisaApplication  
            ,Comment //Recipient가 Input Comment입력시 첨부
        }

        /// <summary>
        ///  결재자 진행 상태
        /// </summary> 
        public struct ProcessStatus
        {
            public static string DRAFTER = "D";
            public static string CURRENT_APPROVER = "C";
            public static string AWAITER = "W";
            public static string ACEPTER = "A";
            public static string REJECTER = "R";
        }

        /// <summary>
        /// 결재선 타입
        /// </summary>
        public struct ApprovalType
        {
            public static string DRAFTER = "D";
            public static string APPROVER = "A";
            public static string RECIPIENT = "R";
            public static string REVIEWER = "V"; 
        }

        /// <summary>
        /// 결재자 타입
        /// </summary>
        public struct ApproverType
        {
            public static string DEFAULT = "D";
            public static string INSERT = "I";
            public static string BEFORE = "B";
            public static string AFTER = "A";
            public static string FORWARD = "F";
        }

        /// <summary>
        /// 메일 발송 여부
        /// </summary>
        public struct SentMail
        {
            public static string SEND = "Y";
            public static string NONE = "N"; 
        }

        /// <summary>
        /// 로그 타입
        /// </summary>
        public enum LogType
        {
            Forward
            ,Recall
            ,InputComment
            ,Withdraw
        }

        /// <summary>
        /// 메일발송 타입
        /// </summary>
        public enum SendMailType
        {
            CurrentApprover
            ,FinalApproval
            ,InputComment
            ,Reject
            ,Forward
            ,Withdraw
            ,Remind
        }

        /// <summary>
        /// 사용자 Role
        /// </summary>
        public struct UserRole
        {
            public static string Admin = "A";
            public static string Special = "S";
            public static string Design = "D";
            public static string None = "N";
        }

        /// <summary>
        /// 메인 화면 뷰타입
        /// </summary>
        public enum MainVIewType
        {
            A,
            B
        }
         
        public void Dispose()
        {
            //자원해제        
            this.Dispose();
        }
    }
}
