using System;
using System.Collections;
using DNSoft.eW.FrameWork;

namespace DNSoft.eWF.FrameWork.Web
{
	/// <summary>
    /// <b>Sessions 클래스를 위한 개체 Class</b><br/>
    /// - 수정일 : DotNetSoft <br/>
    /// - 내  용 : <br/>
	/// </summary>
	[Serializable]
	public class AccountInfo 
	{
		
		/// <summary>
        /// <b>Default constructor</b>
		/// </summary>
		public AccountInfo() 
		{
        }

        #region 개인정보

 
        /// <summary>
        /// 사용자ID
        /// </summary>
		public string UserID { get; set; }

        /// <summary>
        /// 사용자이름
        /// </summary>
		public string UserName { get; set; }

        /// <summary>
        /// 조직명
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 회사코드
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 회사명
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 설정된 언어
        /// </summary>
		public string Language { get; set; }

        /// <summary>
        /// 직책
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// 직위
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 메일 주소
        /// </summary>
        public string MailAddress { get; set; }

        /// <summary>
        /// 전화번호
        /// </summary>
        public string Phone { get; set; }
        
        /// <summary>
        /// 모바일
        /// </summary>
        public string Mobile { get; set; }


        /// <summary>
        /// 환경설정 - 메인 초기화면
        /// </summary>
        public string MainViewType { get; set; }

        /// <summary>
        /// 환경설정 - 메인 초기화면 URL
        /// </summary>
        public string MainFormUrl { get; set; }

        /// <summary>
        /// 사용자 Role 
        /// </summary>
        public string UserRole
        {
            get;
            set;
        }

        /// <summary>
        ///  Special 여부
        /// </summary>
        public string IsSpecialUser { get; set; }
  
        #endregion 
		/// <summary>
		/// 리스트 수 설정값
		/// </summary>
		public int ListCount { get; set; }
        /// <summary>
        /// 페이지수 설정값
        /// </summary>
		public int PageCount { get; set; }

        /// <summary>
        /// LEADING_SUBGROUP
        /// </summary>
        public string LeadingSubGroup { get; set; }
	}
}