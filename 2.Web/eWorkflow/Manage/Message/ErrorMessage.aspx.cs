using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Reflection;

using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;

namespace DNSoft.eW.Web.Common.Message
{
    /// <summary>
    /// <b>■ 클래스 DNSoft.eW.Web.Common.Message.ErrorMessage 설명 : </b><br/>
    /// - 작  성  자 : Microsoft ewadmin<br/>
    /// - 최초작성일 : 2008-12-22 오후 7:09:06<br/>
    /// - 최종수정자 : Microsoft ewadmin<br/>
    /// - 최종수정일 : 2008-12-22 오후 7:09:06<br/>
    /// - 주요변경로그<br/>
    ///		2008-12-22 오후 7:09:06 생성<br/>
    /// </summary>
    public partial class ErrorMessage : System.Web.UI.Page //DNSoft.eW.FrameWork.Web.PageBase
    {
        /// <summary>
        /// <b>■ 페이지 DNSoft.eW.Web.Common.Message.ErrorMessage의 Load 이벤트 : Page 초기화에 필요한 작업을 합니다.</b><br/>
        /// - 최초작성자 : Microsoft ewadmin<br/>
        /// - 최초작성일 : 2008-12-22 오후 7:09:06<br/>		
        /// - 주요변경로그<br/>
        ///		2008-12-22 오후 7:09:06 생성<br/> 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e )
        {
  
            try
            {
				SetDic();
                if ( !( this.IsPostBack ) )
                {
                    //-------------------------------------------------------------------------------
                    // Page가 처음 로드되었을 때 필요한 처리를 이곳에서 합니다.
                    //-------------------------------------------------------------------------------
                    SetMsgDic();
                }
            }

            catch ( Exception ex )
            {
             
            }

            finally
            {
            
            }
        }

		#region SetDic처리
		///<summary>
		///<b>■다국어처리</b>
		///-최초작성자 : 닷넷소프트 조주희<br/>
		///-최초작성일 : 2010-03-10<br/>
        /// - 최종수정자 :  닷넷소프트 김학수<br/>
        /// - 최종수정일 : 2011.09.06<br/>
        /// - 주요변경로그<br/>"
		/// </summary>
		protected void SetDic()
		{ 
			try
			{
                this.hspanError.InnerText = "오류";
                this.hspanViewDetail.InnerText = "상세";
				this.hspanConfirm.InnerText = "확인";
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

        /// <summary>
        /// <b>■ 페이지 DNSoft.eW.Web.Common.Message.ErrorMessage의 OnInit 이벤트 : Page 에서 사용하는 Control의 Event를 등록합니다.</b><br/>
        /// - 최초작성자 : Microsoft ewadmin<br/>
        /// - 최초작성일 : 2008-12-22 오후 7:09:06<br/>		
        /// - 주요변경로그<br/>
        ///		2008-12-22 오후 7:09:06 생성<br/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit( EventArgs e )
        {
    
            try
            {
                //-------------------------------------------------------------------------------
                // 컨트롤에 필요한 이벤트를 이곳에서 등록합니다.
                //-------------------------------------------------------------------------------
            }

            catch ( Exception ex )
            {
             
            }

            finally
            {
                base.OnInit( e );
        
            }
        }

        #region 메세지 세팅(Dictionary, Message)
        /// <summary>
        /// <b>■Dictionary(Message) 메세지 셋팅</b><br/>
        /// - 추가수정자 : 류승태<br/>
        /// - 추가수정일 : 2009년 2월 2일<br/>
        /// - 내		용 : 다국어Label변경<br/>
        /// - 주요변경로그<br/>
        /// </summary>
        private void SetMsgDic()
        {
            try
            {
                ////////////////////////////////
                ///// 메시지 처리
                ///////////////////////////////


                ////////////////////////////////
                ///// 사전 처리
                ////////////////////////////////
                //this.btnDetail.Value = Dic( "EWM_MAIL_DETAIL" ); // 자세히
                //this.liClose.Text      = Dic( "EWM_MAIL_CLOSE" );    // 닫기
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }
        #endregion

    }
}
