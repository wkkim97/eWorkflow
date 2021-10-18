using System;
using System.Data;

namespace DNSoft.eW.FrameWork.Common
{
	/// <summary>
	/// <b>TypeMgr에 대한 요약 설명입니다.</b><br/>
    /// </summary>
    /// <remarks>
	/// - 작  성  자 : 인터데브 유승호<br/>
	/// - 최초작성일 : 2009년 01월 16일<br/>
	/// - 최종수정자 : 유승호<br/>
	/// - 최종수정일 : 2009년 01월 16일<br/>
	/// - 주요변경로그<br/>
	/// 2009.01.16 생성<br/>
    /// </remarks>
	public class TypeMgr
	{
		private TypeMgr()
		{
			//
			// TODO: 여기에 생성자 논리를 추가합니다.
			//
		}

		/// <summary>
		/// <b>SqlDbType추출</b><br/>
        /// </summary>
        /// <remarks>
		/// - 작  성  자 : 인터데브 유승호<br/>
		/// - 최초작성일 : 2009년 01월 16일<br/>
		/// - 최종수정자 : 유승호<br/>
		/// - 최종수정일 : 2009년 01월 16일<br/>
		/// - 주요변경로그<br/>
		/// 2009.01.16 생성<br/>
        /// </remarks>
		/// <param name="Value">값</param>
		/// <param name="oType">SqlDbType</param>
		/// <returns>결과스트링</returns>
		public static string GetValue(object Value, SqlDbType oType)
		{
			string strValue = "";
			if ( Value == DBNull.Value )
			{
				return string.Empty;
			}

			try
			{
				strValue = Value.ToString();
				switch (oType)
				{
					case SqlDbType.BigInt :
					case SqlDbType.Int :
					case SqlDbType.SmallInt :
					case SqlDbType.TinyInt :
						if ( CheckDigit(strValue) != true )
							return string.Empty;
						else
							return strValue;
					default:
						return strValue;
				}
			}
			catch ( Exception ex )
			{
				//throw Exception
				throw ex;
			}
		}

		/// <summary>
		/// <b>Digit체크</b><br/>
        /// </summary>
        /// <remarks> 
		/// - 작  성  자 : 인터데브 유승호<br/>
		/// - 최초작성일 : 2009년 01월 16일<br/>
		/// - 최종수정자 : 유승호<br/>
		/// - 최종수정일 : 2009년 01월 16일<br/>
		/// - 주요변경로그<br/>
		/// 2009.01.16 생성<br/>
        /// </remarks>
		/// <param name="Value">값</param>
		/// <returns>결과 Bool값</returns>
		private static bool CheckDigit(string Value)
		{
			
			try
			{
				for ( int i = 0 ; i < Value.Length ; i++ )
				{
					if ( Char.IsDigit(Value[i]) != true )
						return false;
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return true;
		}
	}
}
