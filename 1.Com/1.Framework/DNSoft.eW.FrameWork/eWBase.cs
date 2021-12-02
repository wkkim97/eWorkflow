using System;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DNSoft.eW.FrameWork
{
	/// <summary>
    /// <b>FrameWork의 공통적인 유틸들이 모여있습니다.</b>
	/// </summary>
    public class eWBase : IDisposable
    {
        #region 상수 : 암호화에 필요한 key & 벡터 값 정의

        // DES 알고리즘용
        //private const string _encryptionKey = "8D08C2FA";
        //private const string _encryptionIV = "50E9A4CD";

        //TripleDES 알고리즘용
        /// <summary>
        /// key : 16자리 문자열(한글제외)
        /// </summary>
        private const string _encryptionKey = "DotnetSoft123456"; 
        /// <summary>
        /// IV : 8자리 문자열(한글제외)
        /// </summary>
        private const string _encryptionIV = "Dotnet35";            

        #endregion

        #region 맴버변수
        /// <summary>
        /// Common.config Collection
        /// </summary>
        private static NameValueCollection _configList;
        
        /// <summary>
        /// 초기화 여부
        /// </summary>
		private static bool _bInitialized = false ;
        
        /// <summary>
        /// 자원여부 ( DB Base/Resource )
        /// </summary>
		private static bool _bUseDicDB = false;
        
        /// <summary>
        /// 서브시스템 필드
        /// </summary>
        private static object[] _subSystemFields = null;
        
        /// <summary>
        /// 내부 도메인 목록
        /// </summary>
		private static string[] _innerDomain = null;
        #endregion

        #region 생성자 & Initialize
        /// <summary>
        /// eWBase 생성자
        /// </summary>
		public eWBase()
		{
		}

        /// <summary>
        /// eWBase static 생성자
        /// </summary>
		static eWBase()
		{
			Initialize();
		}

        /// <summary>
        /// IDisposable 구현
        /// </summary>
        public void Dispose()
        {
            //자원해제            
        }

        /// <summary>
        /// 생성 초기화
        /// </summary>
		private static void Initialize()
		{
            Type oSubSystemType = null;

            System.Reflection.FieldInfo[] afiSubSystemType = null;

            try
            {
                if (_configList != null)
                {
                    _configList.Clear();
                }

                _configList = GetKeyValueCollection();

                if (_subSystemFields != null)
                {
                    _subSystemFields = null;
                }

                oSubSystemType = typeof(SubSystemType);
                afiSubSystemType = oSubSystemType.GetFields();

                _subSystemFields = new object[afiSubSystemType.Length - 1];

                for (int n = 1; n < afiSubSystemType.Length; n++)
                {
                    _subSystemFields[n - 1] = afiSubSystemType[n].GetValue(SubSystemType.Common);
                }

                _bInitialized = true;

            }
            catch (Exception ex)
            {
                _bInitialized = false;
                throw new ConfigurationErrorsException("eWBase Initialize is Fail", ex);
            }

            finally
            {
                oSubSystemType = null;
                afiSubSystemType = null;
            }
        }


        #endregion

        #region SetConfigInfo
        /// <summary>
        /// NameValueCollection 자료구조에 Config 정보들을 추가한다<br/>
        /// </summary>
        /// <param name="col">데이터 객체</param>
        /// <param name="configXMLs">로드한Config XML</param>
        /// <param name="key">NameValueCollection Key값</param>
        private static void SetConfigInfo(NameValueCollection col, XmlNodeList configXMLs, string key)
		{
			string strNewKey = string.Empty;

			try
			{
				for (int n = 0; n < configXMLs.Count; n++)
				{
					if (configXMLs[n].HasChildNodes)
					{
						strNewKey = key + "/" + configXMLs[n].Name;

						if (configXMLs[n].ChildNodes[0].NodeType.Equals(XmlNodeType.Text) || configXMLs[n].ChildNodes[0].NodeType.Equals(XmlNodeType.CDATA))
						{
							col.Add(strNewKey, configXMLs[n].ChildNodes[0].InnerText);
						}
						else
						{
							DNSoft.eW.FrameWork.eWBase.SetConfigInfo(col, configXMLs[n].ChildNodes, strNewKey);
						}
					}
				}
			}

			catch (Exception ex)
			{
				string message = ex.Message;

				if (ex.InnerException != null)
				{
					message = String.Concat(message, " ", ex.InnerException.Message);
				}

				throw new ConfigurationErrorsException(message, ex);
			}
        }
        #endregion 

        #region GetAppConfigPath
        /// <summary>
        /// PageBase.Sessions에 넣을 사용자 정보를 가져온다. <br/> 
        /// </summary>
        /// <returns>Common.config 물리적 경로</returns>
		public static string GetAppConfigPath()
		{
			string strAppConfigPath = System.Configuration.ConfigurationManager.AppSettings["AppConfigPath"];

			if (strAppConfigPath == null || strAppConfigPath.Equals(string.Empty))
			{
				FileInfo oFile = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
				strAppConfigPath = oFile.DirectoryName + "\\" + "Common.config";
			}

			if ( strAppConfigPath == null || strAppConfigPath.Equals(string.Empty) )
			{
				throw new ConfigurationErrorsException("AppConfigPath Information (in Web.config File) is not exist!");
			}

			return strAppConfigPath;
        }
        #endregion

        #region GetKeyValueCollection
        /// <summary>
        /// Common.config 정보를 NameValueCollection 데이터구조로 변환하여 리턴한다.<br/>
        /// </summary>
        /// <returns>Common.config 데이터</returns>
		private static NameValueCollection GetKeyValueCollection()
		{
			NameValueCollection nvCol = null;

			string strAppConfigPath = string.Empty;
			string strRefreshOnChange = string.Empty;

			XmlDocument xmlDoc = null;

			bool bIsRefreshOnChange = false;

			FileSystemWatcher fswFileWatcherApp = null;

			FileInfo fiFileInfo = null;

			try
			{
				strAppConfigPath = DNSoft.eW.FrameWork.eWBase.GetAppConfigPath();

				xmlDoc = new XmlDocument();
				xmlDoc.Load(strAppConfigPath);

				nvCol = new NameValueCollection();

				DNSoft.eW.FrameWork.eWBase.SetConfigInfo(nvCol, xmlDoc.SelectSingleNode("configuration").ChildNodes, "/");

				DNSoft.eW.FrameWork.eWBase._bUseDicDB = nvCol["//ServerInfo/WebServer/DicCallType"].Equals("DB");

				strRefreshOnChange = nvCol["//RefreshOnChange"];

				if(strRefreshOnChange != null)
				{
					bIsRefreshOnChange = strRefreshOnChange.ToLower().Equals("true");
				}

				fiFileInfo = new FileInfo(strAppConfigPath);

				if(bIsRefreshOnChange)
				{
					fswFileWatcherApp = new FileSystemWatcher();
					fswFileWatcherApp.Path = fiFileInfo.DirectoryName;
					fswFileWatcherApp.Filter = fiFileInfo.Name;
					fswFileWatcherApp.NotifyFilter = NotifyFilters.LastAccess;
					fswFileWatcherApp.IncludeSubdirectories = false;
					fswFileWatcherApp.Changed += new FileSystemEventHandler(OnChangedConfigFile);
					fswFileWatcherApp.EnableRaisingEvents = true;
				}
			}

			catch ( Exception ex)
			{
				if (nvCol != null) 
				{
					nvCol.Clear();
					nvCol = null;
				}

				_bInitialized = false;

				string message = ex.Message;

				if (ex.InnerException != null)
				{
					message = String.Concat(message, " ", ex.InnerException.Message);
				}

				throw new ConfigurationErrorsException(message, ex);
			}

			finally
			{
				strAppConfigPath = null;
				strRefreshOnChange = null;

				if(xmlDoc != null)
                    xmlDoc.RemoveAll();
				xmlDoc = null;

				fiFileInfo = null;
			}

			return nvCol;
        }
        #endregion

        #region OnChangedConfigFile
        /// <summary>
        /// FileSystemEvent가 발생되면 Common.config값을 다시읽어오는 이벤트 헨들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnChangedConfigFile(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType ==  WatcherChangeTypes.Changed) 
			{
				if (DNSoft.eW.FrameWork.eWBase._configList != null)
				{
					DNSoft.eW.FrameWork.eWBase._configList.Clear();
					DNSoft.eW.FrameWork.eWBase._configList = null;

					DNSoft.eW.FrameWork.eWBase._bInitialized = false;
				}

				DNSoft.eW.FrameWork.eWBase._configList = GetKeyValueCollection();
			}
        }
        #endregion

        #region ReturnFileName 서브시스템타입의 네임스트링
        /// <summary>
        /// 서브시스템타입의 네임스트링<br/>
		/// </summary>
		/// <param name="sub">서브시스템타입(열거형)</param>
		/// <returns>서브시스템타입(문자열)</returns>
		public static string ReturnFileName(SubSystemType sub)
		{
            string filename = "eFrameWork";
            
			try
			{
                filename = sub.ToString();
			}

			catch(Exception ex)
			{
				throw ex;
			}
			return filename;
		}
		#endregion
        
		#region GetLayerString Layer타입스트링
		/// <summary>
        /// Layer타입스트링<br/>
		/// </summary>
		/// <param name="Layer">Layer타입</param>
		/// <returns>Layer명</returns>
		public static string GetLayerString(LayerType Layer)
		{
			string LayerName = "";
			try
			{
				switch(Layer)
				{
						
					case(LayerType.BSL): LayerName = "BSL"; break;
					case(LayerType.DAL): LayerName = "DAL"; break;
					case(LayerType.GUI): LayerName = "GUI"; break;
					case(LayerType.SQL): LayerName = "SQL"; break;
					case(LayerType.USR): LayerName = "USR"; break;
					case(LayerType.DSL): LayerName = "DSL"; break;
					case(LayerType.EXCH): LayerName = "EXC"; break;
					case(LayerType.AD): LayerName = "AD"; break;
					case(LayerType.MQ): LayerName = "MQ"; break;
					case(LayerType.SPS): LayerName = "SPS"; break;
					case(LayerType.WIN): LayerName = "WIN"; break;
					default : LayerName = "ETC"; break;
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return LayerName;
		}
		#endregion

		#region GetSubSystemCode 서브시스템타입의 코드를 리턴
		/// <summary>
        /// 서브시스템타입의 코드를 리턴<br/>
		/// </summary>
		/// <param name="subSystemType">서브시스템타입</param>
		/// <returns>서브시스템코드</returns>
		public static string GetSubSystemCode(SubSystemType subSystemType)
		{
			//int nCode = 0;
			string strCode = string.Empty;
			try
			{
                //nCode = (int)subSystemType;
				switch (subSystemType)
				{
					case SubSystemType.eManage:     strCode = "00"; break; 
					case SubSystemType.Common:	    strCode = "99"; break;
					default:					    strCode = "99"; break;
				}
			}

			catch(Exception ex)
			{
				throw ex;
			}

			return strCode;
		}
		#endregion

		#region ReturnSubSystemType 해당스트링에 따른 서브시스템타입넘겨주기
		/// <summary>
        /// 해당스트링에 따른 서브시스템타입넘겨주기<br/>
		/// </summary>
		/// <param name="subSystemType">서브시스템스트링</param>
		/// <returns>생성된서브시스템타입</returns>
		public static SubSystemType ReturnSubSystemType(string subSystemType)
		{
			//서브시스템타입
			SubSystemType subType = SubSystemType.Common;
            string strValue = subSystemType.ToUpper();

			try
			{
                for (int n = 0; n < _subSystemFields.Length; n++)
                {
                    if (_subSystemFields[n].ToString().ToUpper().Equals(strValue))
                    {
                        subType = (SubSystemType)_subSystemFields[n];
                        break;
                    }

                }
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return subType;
		}
		#endregion

		#region 서브팀별 환경설정정보가져오기-attribute
		/// <summary>
        /// 환경설정 정보 가져오기<br/>
        /// 수정내역<br/>
        /// :컬렉션에서 찾고자 하는 키가 있는지부터 확인, Exception 처리 변경(2010-12-27 김동훈)<br/>
		/// </summary>
		/// <param name="configKey">노드명</param>
		/// <returns>환경설정정보</returns>
		public static string GetConfig(string configKey)
		{
            if (string.IsNullOrEmpty(configKey) == true)
            {
                //throw new System.Configuration.ConfigurationErrorsException("주어진 키이름이 없습니다!");
                return null;
            }

            if (!_bInitialized)
            {
                DNSoft.eW.FrameWork.eWBase.Initialize();
            }

            string strConfigValue = null;
            if (DNSoft.eW.FrameWork.eWBase._configList[configKey] != null)
            {
                strConfigValue = DNSoft.eW.FrameWork.eWBase._configList[configKey];
            }

            //if (string.IsNullOrEmpty(strConfigValue) == true)
            //{
            //    throw new System.Configuration.ConfigurationErrorsException(configKey + "의 값이 존재하지 않습니다!");
            //}

			return strConfigValue ;
		}
		

		/// <summary>
		/// 서브팀별 환경설정 정보 가져오기<br/>
        /// 수정내역<br/>
        ///     : Overloading 정리(2010-12-27 김동훈)
		/// </summary>
		/// <param name="subSystemType">서브시스템코드</param>
		/// <param name="configKey">노드명</param>
		/// <returns>환경설정정보</returns>
		public static string GetConfig(string configKey, string defaultValue)
		{
            if (!_bInitialized)
            {
                DNSoft.eW.FrameWork.eWBase.Initialize();
            }

            //string strConfigValue = DNSoft.eW.FrameWork.eWBase._configList[configKey];

            string strConfigValue = GetConfig(configKey);

			if (string.IsNullOrEmpty(strConfigValue) == true)
			{
				strConfigValue = defaultValue;
			}

			return strConfigValue;	
		}



		/// <summary>
		/// 서브팀별 환경설정 정보 가져오기
		/// </summary>
		/// <param name="subSystemType">서브시스템코드</param>
		/// <param name="configKey">노드명</param>
		/// <returns>환경설정정보</returns>
		public static string[] GetConfigs(string configKey)
		{
			if (!_bInitialized)
			{
				DNSoft.eW.FrameWork.eWBase.Initialize();
			}

			string[] strConfigValue = DNSoft.eW.FrameWork.eWBase._configList.GetValues(configKey);

			if (strConfigValue == null)
			{
				throw new System.Configuration.ConfigurationErrorsException(configKey + "의 값이 존재하지 않습니다!");
			}

			return strConfigValue;
		}


		#endregion

		#region 쿠키 암복호화
		/// <summary>
        /// 쿠키정보 암호화<br/>
        /// </summary>
		/// <remarks>암호화 할때 Key와 IV(Initialization Vector)를 하드코딩하여 간단한 암호화를 수행
        ///    하는 함수, 알고리즘은 DES알고리즘을 사용한다.<br/>
        ///   리턴되는 값은 Base64인코딩된 값이다.<br/>
        ///   암호화 해제할때는 SimpleDecrypt()를 사용한다.<br/>
        /// </remarks>
		/// <param name="stringSource">소스스트링</param>
		/// <returns>인크립션스트링</returns>
		public static string eWEncrypt(string stringSource)
		{   
			EncryptionAlgorithm algorithm = EncryptionAlgorithm.TripleDes;     
			byte[] cipherText = null;
            byte[] key = Encoding.UTF8.GetBytes(_encryptionKey);
            byte[] IV = Encoding.UTF8.GetBytes(_encryptionIV);
			try
			{
				
				Encryptor enc = new Encryptor(algorithm);    
				enc.IV = IV;
				cipherText = enc.Encrypt(Encoding.UTF8.GetBytes(stringSource), key);    
				return Convert.ToBase64String(cipherText);
			}
			catch(Exception ex)
			{
				throw new Exception("Error eWEncrypt()-" + ex.Message);
			}
		}

		/// <summary>
        /// 암호화된 쿠키정보를 TripleDes로 복호화한다.<br/>
		/// </summary>
		/// <param name="stringSource">소스스트링</param>
		/// <returns>디크립션스트링</returns>
		public static string eWDecrypt(string stringSource)
		{
            EncryptionAlgorithm algorithm = EncryptionAlgorithm.TripleDes;
            byte[] key = Encoding.UTF8.GetBytes(_encryptionKey);
            byte[] IV = Encoding.UTF8.GetBytes(_encryptionIV);
			try
			{
				Decryptor dec = new Decryptor(algorithm);   
				dec.IV = IV;
				byte[] encryptText = Convert.FromBase64String(stringSource);
				byte[] plainText = dec.Decrypt(encryptText, key);
				return Encoding.UTF8.GetString(plainText);
			}
			catch(Exception ex)
			{
				throw new Exception("Error eWDecrypt()-" + ex.Message);
			}
		}



		/// <summary>
		///	사용자 Cookie 정보를 암호화 한다.
		/// </summary>
		/// <param name="stringSource">소스스트링</param>
		/// <returns>인크립션스트링</returns>
		public static string UserInfoEncrypt(string stringSource)
		{
			string strReturn = string.Empty;
			string strUsedYN = string.Empty;
			string strLicense = string.Empty;
			string SessionKey = string.Empty;

			try
			{
				strUsedYN = DNSoft.eW.FrameWork.eWBase.GetConfig("//FrameWork/Cryptograph/UsedYN");

				strReturn = stringSource;

				if (strUsedYN.Equals("Y"))
				{
                    strReturn = eWEncrypt(stringSource);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error UserInfoEncrypt()-" + ex.Message);
			}
			return strReturn;
		}

		/// <summary>
		/// 사용자 Cookie 정보를 복호화 한다.
		/// </summary>
		/// <param name="stringSource">소스스트링</param>
		/// <returns>디크립션스트링</returns>
		public static string UserInfoDecrypt(string stringSource)
		{
			string strReturn = string.Empty;
			string strUsedYN = string.Empty;
			string strLicense = string.Empty;
			string SessionKey = string.Empty;

			try
			{
				strUsedYN = DNSoft.eW.FrameWork.eWBase.GetConfig("//FrameWork/Cryptograph/UsedYN");

				strReturn = stringSource;

				// 라이센스 파일 불러오기
				if (strUsedYN.Equals("Y"))
                {
                    strReturn = eWDecrypt(stringSource);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error UserInfoDecrypt()-" + ex.Message);
			}
			return strReturn;
		}

		#endregion	

        #region GetConfigCollection
        /// <summary>
        /// Config Collection 모두 리턴<br/>
        /// </summary>
        /// <remarks>
        /// 2007.03.29 임병태(넥스원) 추가<br/>
        /// IIS Reset없이 시스템 변경 적용하기 위함<br/>
        /// </remarks>
        /// <returns>Config Collection 리턴</returns>
		public static NameValueCollection GetConfigCollection()
		{
			return _configList;
        }
        #endregion

        #region GetLanguageSet
        /// <summary>
        /// GetLanguageSet
        /// </summary>
        /// <returns>"" 리턴</returns>
        public static string GetLanguageSet()
		{
			return "";
        }
        #endregion

      

        #region GetLanguageSet
        /// <summary>
        /// Request.Headers["Accept-Language"]: 요청 해더 언어설정값으로 그룹웨어 언어셋을 얻어온다<br/>
        /// </summary>
        /// <param name="languageValue">언어코드값 ( ex: KO, CH, JA, EN ) </param>
        /// <returns>언어셋</returns>
		public static string GetLanguageSet(string languageValue)
		{
			string strLanguageSet = "en-US";

			try
			{
                languageValue = languageValue.ToUpper();

				if (languageValue.Equals("KO") || languageValue.Equals("KO-KR"))
				{
					strLanguageSet = "ko-KR";
				}

				else if (languageValue.Equals("JA") || languageValue.Equals("JA-JP"))
				{
					strLanguageSet = "ja-JP";
				}

				else if (languageValue.Equals("CN") 
                    || languageValue.Equals("ZH") 
					|| languageValue.Equals("ZH-TW")
					|| languageValue.Equals("ZH-MO")
					|| languageValue.Equals("ZH-SG")
					|| languageValue.Equals("ZH-CN")
					|| languageValue.Equals("ZH-HK"))
				{
					strLanguageSet = "zh-CN";
				}

				else if (languageValue.Equals("EN") || languageValue.Equals("EN-US"))
				{
					strLanguageSet = "en-US";
				}

				else
				{
					strLanguageSet = "en-US";
				}
			}

			catch (Exception ex)
			{
				throw ex;
			}

            //TODO : [Framework] 언어셋 추가 확인 필요
			return strLanguageSet;
        }
        #endregion

        #region UseDicDB
        /// <summary>
        /// 다국어처리가 DB Base 인지 Resource를 사용하는지 true/false를 리턴<br/>
        /// </summary>
		static public bool UseDicDB
		{
			get
			{
				return DNSoft.eW.FrameWork.eWBase._bUseDicDB;
			}
        }
        #endregion

	}
}
