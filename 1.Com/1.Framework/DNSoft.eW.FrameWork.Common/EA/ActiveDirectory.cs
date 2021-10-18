using System;
using System.Data;
using System.Reflection;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Principal;
using System.Security.Permissions;
using System.Text;
using System.Runtime.InteropServices;

using DNSoft.eW.FrameWork.Common.Security;

namespace DNSoft.eW.FrameWork.Common.EA
{
    /// <Summary>
    /// <b>■Data Service Layer을 위한 Active Directory DAL Component</b><br/>
    /// - 작  성  자 : 닷넷소프트<br/>
    /// - 최초작성일 : 2009년 03월 08일<br/>
    /// - 최종수정자 : 이상윤<br/>
    /// - 최종수정일 : 2009년 03월 11일<br/>
    /// - 주요변경로그<br/>
    /// 2009.03.18 생성<br/>
    /// 2009.06.07 (이상윤) DNSoft.eW.FrameWork.eWBase.GetConfig 이용하여 초기값 셋팅하도록 수정
    /// 2009.08.02 (이상윤) Impersonation설정 추가
    /// </Summary>
    /// <Remarks>없음</Remarks>
    // If you incorporate this code into a DLL, be sure to demand FullTrust.
    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    public class ActiveDirectory : IDisposable
    {
        #region WindowsIdentity 구현 관련

        const int LOGON32_PROVIDER_DEFAULT = 0;
        //This parameter causes LogonUser to create a primary token.
        const int LOGON32_LOGON_INTERACTIVE = 2;
        const int LOGON32_LOGON_NETWORK = 3;
        const int SecurityImpersonation = 2;

        //WindowsIdentity 구현 관련
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private unsafe static extern int FormatMessage(int dwFlags, ref IntPtr lpSource,
            int dwMessageId, int dwLanguageId, ref String lpBuffer, int nSize, IntPtr* Arguments);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
            int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        // GetErrorMessage formats and returns an error message
        // corresponding to the input errorCode.
        public unsafe static string GetErrorMessage(int errorCode)
        {
            int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
            int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
            int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

            int messageSize = 255;
            String lpMsgBuf = "";
            int dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

            IntPtr ptrlpSource = IntPtr.Zero;
            IntPtr prtArguments = IntPtr.Zero;
            int retVal = FormatMessage(dwFlags, ref ptrlpSource, errorCode, 0, ref lpMsgBuf, messageSize, &prtArguments);
            if (0 == retVal)
            {
                throw new Exception("Failed to format message for error code " + errorCode + ". ");
            }

            return lpMsgBuf;
        }

        #endregion


        #region Defined for SubSystem
        /// <summary>
        /// 서브시스템타입
        /// </summary>
        private SubSystemType subType = SubSystemType.eManage;

        #endregion


        #region Defined for Active Directory Property, Enumeration

        /// <summary>
        /// The ADS_USER_FLAG_ENUM enumeration defines the flags used for 
        /// setting user properties in the directory. 
        /// These flags correspond to values of the userAccountControl 
        /// attribute in Active Directory when using the LDAP provider, 
        /// and the userFlags attribute when using the WinNT system provider.
        /// </summary>
        public enum ADS_USER_FLAG_ENUM : int
        {
            /// <summary>
            /// The logon script is executed. This flag does not work for the ADSI LDAP provider on either read or write operations. For the ADSI WinNT provider, this flag is read only data, and it cannot be set on user objects.
            /// </summary>
            ADS_UF_SCRIPT = 0x0001,

            /// <summary>
            /// The user account is disabled.
            /// </summary>
            ADS_UF_ACCOUNTDISABLE = 0x0002,

            /// <summary>
            /// The home directory is required. 
            /// </summary>
            ADS_UF_HOMEDIR_REQUIRED = 0X0008,

            /// <summary>
            /// The account is currently locked out. 
            /// </summary>
            ADS_UF_LOCKOUT = 0X0010,

            /// <summary>
            /// No password is required. 
            /// </summary>
            ADS_UF_PASSWD_NOTREQD = 0X0020,

            /// <summary>
            /// The user cannot change the password. You can read this flag, 
            /// but you cannot set it directly. For more information, and a 
            /// code example that shows how to prevent a user from changing 
            /// the password
            /// </summary>
            ADS_UF_PASSWD_CANT_CHANGE = 0X0040,

            /// <summary>
            /// The user can send an encrypted password. 
            /// </summary>
            ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0X0080,

            /// <summary>
            /// This is an account for users whose primary account is in 
            /// another domain. This account provides user access to this domain, 
            /// but not to any domain that trusts this domain. Also known as a 
            /// local user account.
            /// </summary>
            ADS_UF_TEMP_DUPLICATE_ACCOUNT = 0X0100,

            /// <summary>
            /// This is a default account type that represents a typical user. 
            /// </summary>
            ADS_UF_NORMAL_ACCOUNT = 0X0200,

            /// <summary>
            /// This is a permit to trust account for a system domain 
            /// that trusts other domains. 
            /// </summary>
            ADS_UF_INTERDOMAIN_TRUST_ACCOUNT = 0X0800,

            /// <summary>
            /// This is a computer account for a Microsoft® 
            /// Windows® NT Workstation/Windows 2000 Professional or 
            /// Windows NT® Server/Windows 2000 Server that is a member of 
            /// this domain.
            /// </summary>
            ADS_UF_WORKSTATION_TRUST_ACCOUNT = 0X1000,

            /// <summary>
            /// This is a computer account for a system backup 
            /// domain controller that is a member of this domain. 
            /// </summary>
            ADS_UF_SERVER_TRUST_ACCOUNT = 0X2000,

            /// <summary>
            /// When set, the password will not expire on this account. 
            /// </summary>
            ADS_UF_DONT_EXPIRE_PASSWD = 0X10000,

            /// <summary>
            /// This is an MNS logon account. 
            /// </summary>
            ADS_UF_MNS_LOGON_ACCOUNT = 0X20000,

            /// <summary>
            /// When set, this flag will force the user to log on using a smart card. 
            /// </summary>
            ADS_UF_SMARTCARD_REQUIRED = 0X40000,

            /// <summary>
            /// When set, the service account (user or computer account), 
            /// under which a service runs, is trusted for Kerberos delegation. 
            /// Any such service can impersonate a client requesting the service. 
            /// To enable a service for Kerberos delegation, set this flag on 
            /// the userAccountControl property of the service account. 
            /// </summary>
            ADS_UF_TRUSTED_FOR_DELEGATION = 0X80000,

            /// <summary>
            /// When set, the security context of the user will not be 
            /// delegated to a service even if the service account is set 
            /// as trusted for Kerberos delegation. 
            /// </summary>
            ADS_UF_NOT_DELEGATED = 0X100000,

            /// <summary>
            /// Windows 2000/Windows XP:  Restrict this principal 
            /// to use only Data Encryption Standard (DES) encryption types for keys.
            /// </summary>
            ADS_UF_USE_DES_KEY_ONLY = 0x200000,

            /// <summary>
            /// Windows 2000/Windows XP:  This account does not require 
            /// Kerberos preauthentication for logon.
            /// </summary>
            ADS_UF_DONT_REQUIRE_PREAUTH = 0x400000,

            /// <summary>
            /// Windows XP:  The user password has expired. 
            /// UF_PASSWORD_EXPIRED is a bit created by the system, 
            /// using data from the password last set attribute and 
            /// the domain policy. It is read-only and cannot be set. 
            /// To manually set a user password as expired
            /// </summary>
            ADS_UF_PASSWORD_EXPIRED = 0x800000,

            /// <summary>
            /// Windows 2000/Windows XP:  The account is enabled for delegation. 
            /// This is a security-sensitive setting; 
            /// accounts with this option enabled should be tightly controlled. 
            /// This setting enables a service running under the account to assume 
            /// a client identity and authenticate as that user to other remote 
            /// servers on the network.
            /// </summary>
            ADS_UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0x1000000
        }

        /// <summary>
        /// Specifies a group that is security enabled. 
        /// This group can be used to apply an access-control list on 
        /// an ADSI object or a file system. 
        /// </summary>
        private const int ADS_GROUP_TYPE_SECURITY_ENABLED = -2147483648;

        /// <summary>
        /// The ADS_GROUP_TYPE_ENUM enumeration specifies the type of group objects in ADSI.
        /// </summary>
        public enum ADS_GROUP_TYPE_ENUM : int
        {
            /// <summary>
            /// Specifies a group that can contain accounts from the same domain 
            /// and other global groups from the same domain. 
            /// This type of group can be exported to a different domain
            /// </summary>
            ADS_GROUP_TYPE_GLOBAL_GROUP = 0x00000002,

            /// <summary>
            /// Specifies a group that can contain accounts from any domain in the forest, 
            /// other domain local groups from the same domain, global groups from any domain in the forest, 
            /// and universal groups. This type of group cannot be included in access-control lists of resources in other domains.
            /// This type of group is intended for use with the LDAP provider
            /// </summary>
            ADS_GROUP_TYPE_DOMAIN_LOCAL_GROUP = 0x00000004,

            /// <summary>
            /// Specifies a group that can contain accounts from any domain, global groups from any domain, and other universal groups. 
            /// This type of group cannot contain domain local groups.
            /// </summary>
            ADS_GROUP_TYPE_UNIVERSAL_GROUP = 0x00000008
        }

        /// <summary>
		/// LDAP 디렉토리 Path를 셋팅 하거나 반환한다.ex) LDAP://ad01.klotte.com/DC=klotte,DC=com
        /// </summary>
        public string LdapPath { get; set;}

        /// <summary>
        /// 현재의 Domain을 셋팅하거나 반환한다.
        /// </summary>
        public string Domain { get; set;}

        /// <summary>
        /// 현재의 NetBios Domain 을 셋팅하거나 반환한다.
        /// </summary>
        public string NetBiosDomain { get; set;}


        #endregion


        /// <summary>
        /// DirectoryEntry Cache
        /// </summary>
        private static Hashtable directoryEntryCache = Hashtable.Synchronized(new Hashtable());

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ActiveDirectory()
        {
            this.Activate();
        }

        /// <summary>
        /// IDisposable 구현
        /// </summary>
        public void Dispose()
        {
            //자원해제
        }

        #endregion

        #region Activate, DeActivate

        /// <summary>
        /// Activate 할때 기본 LDAP Connection String을 설정한다.
        /// </summary>		
        protected void Activate()
        {
            try
            {
				if (LdapPath == null)
                {
                    LdapPath = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/LdapPath");
                }

				if (Domain == null)
                {
					Domain = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/ADDomain");

                }
				if (NetBiosDomain == null)
                {
					NetBiosDomain = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/NetBiosDomain");
                }
            }
            catch (Exception ex)
            {
                DNSoft.eW.FrameWork.eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
        }

        #endregion

        #region Authentication
        /// <summary>
        /// <b>■해당 사용자의 아이디와 패스워드를 입력받아 인증된 사용자인지를 판단한다.</b><br/>		
        /// - 최초작성자 : 닷넷소프트<br/>
        /// - 최초작성일 : 2009.03.08<br/>
        /// - 주요변경로그<br/>
        /// 2009.03.08 생성 <br/>
        /// </summary>
        /// <param name="domain">도메인(netbios)</param>
        /// <param name="userCN">사용자의 CN값(사용자 ID)</param>
        /// <param name="password">패스워드</param>
        /// <returns>bool</returns>
        public bool IsAuthenticated(string domain, string userCN, string password)
        {
            bool bRet = false;

            string domainAndUsername = domain + @"\" + userCN;
            DirectoryEntry entry = null;
            DirectorySearcher search = null;

            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                //제거예정 : 2010-11-09 김동훈
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                imp = new Impersonation();
                imp.Impersonate();

                entry = new DirectoryEntry(LdapPath, domainAndUsername, password);

                //Bind to the native AdsObject to force authentication.
                Object obj = entry.NativeObject;
                search = new DirectorySearcher(entry);
                search.Filter = "(&(objectClass=user)(sAMAccountName=" + userCN + "))";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    bRet = false;
                }
                else
                {
                    bRet = true;
                }
            }
            catch
            {
                bRet = false;
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                //To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (entry != null) entry.Dispose();
                if (search != null) search.Dispose();
            }
            return bRet;
        }


        /// <summary>
        /// <b>■해당 사용자의 아이디와 패스워드를 입력받아 인증된 사용자인지를 판단한다.</b><br/>		
        /// - 최초작성자 : 닷넷소프트<br/>
        /// - 최초작성일 : 2009.03.08<br/>
        /// - 주요변경로그<br/>
        /// 2009.03.08 생성 <br/>
        /// </summary> 
        /// <param name="userCN">사용자의 CN값(사용자 ID)</param>
        /// <param name="password">패스워드</param>
        /// <returns>bool</returns>
        public bool IsAuthenticated( string userCN, string password)
        {
            bool bRet = false;

          
            DirectoryEntry entry = null;
            DirectorySearcher search = null;
             
            try
            { 
                entry = new DirectoryEntry(LdapPath, userCN, password);

                //Bind to the native AdsObject to force authentication.
                Object obj = entry.NativeObject;
                search = new DirectorySearcher(entry);
                search.Filter = "(&(objectClass=user))";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    bRet = false;
                }
                else
                {
                    bRet = true;
                }
            }
            catch
            {
                bRet = false;
            }
            finally
            {
 
                if (entry != null) entry.Dispose();
                if (search != null) search.Dispose();
            }
            return bRet;
        }
        #endregion

        #region User Management(Create, Delete, SetPassword, Enable, Disable)

        /// <summary>
        /// <b>■ 메일 서버 쿼리하기</b><br/>
        /// - 작  성  자 : 닷넷소프트 정계성<br/>
        /// - 최초작성일 : 2009년 09월 05일<br/>
        /// - 최종수정자 : 정계성<br/>
        /// - 최종수정일 : 2009년 09월 05일<br/>
        /// - 주요변경로그<br/>
        /// 2009년 09월 05일 생성<br/>	 
        /// </summary>
        public string GetMailBoxServer(string exchHomeServerName)
        {
            string[] arrServer = null;
            string strServerName = string.Empty;

            try
            {
                if (!String.IsNullOrEmpty(exchHomeServerName))
                {
                    arrServer = exchHomeServerName.Split('/');

                    strServerName = arrServer[arrServer.Length - 1].ToString();
                    strServerName = strServerName.Replace("cn=", "");
                    strServerName = strServerName.ToLower();
                }
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //To User 
                if (arrServer != null) arrServer = null;
            }
            return strServerName;
        }

        /// <summary>
        /// <b>■ 사용자 언어 가저오기/b><br/>
        /// - 작  성  자 : 닷넷소프트 김학진<br/>
        /// - 최초작성일 : 2009.02.20<br/>
        /// - 최종수정자 : <br/>
        /// - 최종수정일 : <br/>
        /// - 주요변경로그<br/>
        /// 2009.02.20 생성<br/>	 
        /// </summary>
        public string GetUserCulture(string userCulture)
        {
            string[] arrCulture = null;
            string strUserCulture = string.Empty;

            try
            {
                if (!String.IsNullOrEmpty(userCulture))
                {
                    arrCulture = userCulture.Split(',');
                    strUserCulture = arrCulture[0].ToString();
                }
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //To User 
                if (arrCulture != null) arrCulture = null;
            }
            return strUserCulture;
        }



        /// <summary>
        /// 사용자의 정보를 DataTable
        /// </summary>
        /// <param name="userCN">사용자 CN값</param>
        /// <returns>DataTable</returns>
        public DataTable GetUserInfo(string userCN)
        {

            DirectoryEntry oUser = null;
            DataTable dtUser = null;
            DataRow drUser = null;

            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                //제거예정 : 2010-11-09 김동훈
                ////string debug1 = WindowsIdentity.GetCurrent().Name;
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                ////string debug2  = WindowsIdentity.GetCurrent().Name;
                imp = new Impersonation();
                imp.Impersonate();

				dtUser = new DataTable();
                dtUser.Columns.Add("cn", Type.GetType("System.String"));
                dtUser.Columns.Add("sAMAccountName", Type.GetType("System.String"));
                dtUser.Columns.Add("name", Type.GetType("System.String"));
                dtUser.Columns.Add("displayName", Type.GetType("System.String"));
                dtUser.Columns.Add("distinguishedName", Type.GetType("System.String"));
                dtUser.Columns.Add("description", Type.GetType("System.String"));
                dtUser.Columns.Add("company", Type.GetType("System.String"));
                dtUser.Columns.Add("homePhone", Type.GetType("System.String"));
                dtUser.Columns.Add("logonCount", Type.GetType("System.String"));
                dtUser.Columns.Add("mail", Type.GetType("System.String"));
                dtUser.Columns.Add("mailNickName", Type.GetType("System.String"));
                dtUser.Columns.Add("objectGUID", Type.GetType("System.String"));
                dtUser.Columns.Add("homeMDB", Type.GetType("System.String"));
                dtUser.Columns.Add("msExchHomeServerName", Type.GetType("System.String"));
                dtUser.Columns.Add("msExchUserCulture", Type.GetType("System.String"));
                dtUser.Columns.Add("whenCreated", Type.GetType("System.String"));
                dtUser.Columns.Add("whenChanged", Type.GetType("System.String"));
                
				oUser = GetDirectoryEntry(userCN);                
				drUser = dtUser.NewRow();

                drUser["cn"] = oUser.Properties["cn"].Value;
                drUser["sAMAccountName"] = oUser.Properties["sAMAccountName"].Value;
                drUser["name"] = oUser.Properties["name"].Value;
                drUser["displayName"] = oUser.Properties["displayName"].Value;
                drUser["distinguishedName"] = oUser.Properties["distinguishedName"].Value;
                drUser["description"] = oUser.Properties["description"].Value;
                drUser["company"] = oUser.Properties["company"].Value;
                drUser["homePhone"] = oUser.Properties["homePhone"].Value;
                drUser["logonCount"] = oUser.Properties["logonCount"].Value;
                drUser["mail"] = oUser.Properties["mail"].Value;
                drUser["mailNickName"] = oUser.Properties["mailNickName"].Value;
                drUser["objectGUID"] = oUser.Guid.ToString();
                drUser["homeMDB"] = oUser.Properties["homeMDB"].Value;
                drUser["msExchHomeServerName"] = GetMailBoxServer(oUser.Properties["msExchHomeServerName"].Value.ToString());
                //drUser["msExchUserCulture"] = GetUserCulture(oUser.Properties["msExchUserCulture"].Value.ToString());
                drUser["whenCreated"] = oUser.Properties["whenCreated"].Value;
                drUser["whenChanged"] = oUser.Properties["whenChanged"].Value;
                dtUser.Rows.Add(drUser);
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oUser != null) oUser.Dispose();
            }
            return dtUser;
        }

        //안기홍 추가
        public DataTable GetUserInfo(string userCN, string domain, string password)
        {
            DirectoryEntry oUser = null;
            DataTable dtUser = null;
            DataRow drUser = null;

            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                //제거예정 : 2010-11-09 김동훈
                ////string debug1 = WindowsIdentity.GetCurrent().Name;
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                ////string debug2  = WindowsIdentity.GetCurrent().Name;
                imp = new Impersonation();
                imp.Impersonate();

                dtUser = new DataTable();
                dtUser.Columns.Add("cn", Type.GetType("System.String"));
                dtUser.Columns.Add("sAMAccountName", Type.GetType("System.String"));
                dtUser.Columns.Add("name", Type.GetType("System.String"));
                dtUser.Columns.Add("displayName", Type.GetType("System.String"));
                dtUser.Columns.Add("distinguishedName", Type.GetType("System.String"));
                dtUser.Columns.Add("description", Type.GetType("System.String"));
                dtUser.Columns.Add("company", Type.GetType("System.String"));
                dtUser.Columns.Add("homePhone", Type.GetType("System.String"));
                dtUser.Columns.Add("logonCount", Type.GetType("System.String"));
                dtUser.Columns.Add("mail", Type.GetType("System.String"));
                dtUser.Columns.Add("mailNickName", Type.GetType("System.String"));
                dtUser.Columns.Add("objectGUID", Type.GetType("System.String"));
                dtUser.Columns.Add("homeMDB", Type.GetType("System.String"));
                dtUser.Columns.Add("whenCreated", Type.GetType("System.String"));
                dtUser.Columns.Add("whenChanged", Type.GetType("System.String"));

                oUser = GetDirectoryEntry(userCN, domain, password);

                drUser = dtUser.NewRow();
                drUser["cn"] = oUser.Properties["cn"].Value;
                drUser["sAMAccountName"] = oUser.Properties["sAMAccountName"].Value;
                drUser["name"] = oUser.Properties["name"].Value;
                drUser["displayName"] = oUser.Properties["displayName"].Value;
                drUser["distinguishedName"] = oUser.Properties["distinguishedName"].Value;
                drUser["description"] = oUser.Properties["description"].Value;
                drUser["company"] = oUser.Properties["company"].Value;
                drUser["homePhone"] = oUser.Properties["homePhone"].Value;
                drUser["logonCount"] = oUser.Properties["logonCount"].Value;
                drUser["mail"] = oUser.Properties["mail"].Value;
                drUser["mailNickName"] = oUser.Properties["mailNickName"].Value;
                drUser["objectGUID"] = oUser.Guid.ToString();
                drUser["homeMDB"] = oUser.Properties["homeMDB"].Value;
                drUser["whenCreated"] = oUser.Properties["whenCreated"].Value;
                drUser["whenChanged"] = oUser.Properties["whenChanged"].Value;

                dtUser.Rows.Add(drUser);

            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oUser != null) oUser.Dispose();
            }
            return dtUser;
        }


        /// <summary>
        /// 사용자의 속한 그룹리스트 반환(isSecurity를 제외한 컬럼명은 AD상의 Property명과 동일함
        /// </summary>
        /// <param name="userCN">사용자 CN값</param>
        /// <returns>DataTable</returns>
        public DataTable GetGroupsOfUser(string userCN)
        {
            //Comment			
            DirectoryEntry oUser = null;
            DataTable dtGroupList = null;
            DataRow drGroup = null;

            try
            {
                dtGroupList = new DataTable();
                dtGroupList.Columns.Add("cn", Type.GetType("System.String"));
                dtGroupList.Columns.Add("sAMAccountName", Type.GetType("System.String"));
                dtGroupList.Columns.Add("name", Type.GetType("System.String"));
                dtGroupList.Columns.Add("displayName", Type.GetType("System.String"));
                dtGroupList.Columns.Add("distinguishedName", Type.GetType("System.String"));
                dtGroupList.Columns.Add("description", Type.GetType("System.String"));
                dtGroupList.Columns.Add("mail", Type.GetType("System.String"));
                dtGroupList.Columns.Add("mailNickName", Type.GetType("System.String"));
                dtGroupList.Columns.Add("objectGUID", Type.GetType("System.String"));
                dtGroupList.Columns.Add("whenCreated", Type.GetType("System.String"));
                dtGroupList.Columns.Add("whenChanged", Type.GetType("System.String"));
                dtGroupList.Columns.Add("isSecurity", Type.GetType("System.Boolean"));

                oUser = GetDirectoryEntry(userCN);
                Object oGroups = oUser.Invoke("Groups");

                foreach (Object group in (IEnumerable)oGroups)
                {

                    DirectoryEntry oGroup = new DirectoryEntry(group);
                    drGroup = dtGroupList.NewRow();
                    drGroup["cn"] = oGroup.Properties["cn"].Value;
                    drGroup["sAMAccountName"] = oGroup.Properties["sAMAccountName"].Value;
                    drGroup["name"] = oGroup.Properties["name"].Value;
                    drGroup["displayName"] = oGroup.Properties["displayName"].Value;
                    drGroup["distinguishedName"] = oGroup.Properties["distinguishedName"].Value;
                    drGroup["description"] = oGroup.Properties["description"].Value;
                    drGroup["mail"] = oGroup.Properties["mail"].Value;
                    drGroup["mailNickName"] = oGroup.Properties["mailNickName"].Value;
                    drGroup["objectGUID"] = oGroup.Guid.ToString();
                    drGroup["whenCreated"] = oGroup.Properties["whenCreated"].Value;
                    drGroup["whenChanged"] = oGroup.Properties["whenChanged"].Value;
                    drGroup["isSecurity"] = (((int)oGroup.Properties["groupType"].Value & ADS_GROUP_TYPE_SECURITY_ENABLED)
                        == ADS_GROUP_TYPE_SECURITY_ENABLED) ? true : false;

                    dtGroupList.Rows.Add(drGroup);
                }

            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                if (oUser != null) oUser.Dispose();
            }
            return dtGroupList;
        }


        public void CreateUser(string userCN, string userName, string description, string password, string oupath)
        {
            CreateUser(userCN, userCN, userName, description, password, oupath);
        }

        /// <summary>
        /// 사용자를 AD에 생성한다.
        /// </summary>
        /// <param name="userCN">사용자 Common Name</param>
        /// <param name="userName">사용자 이름</param>
        /// <param name="description">사용자 설명</param>
        /// <param name="password">패스워드</param>
        /// <param name="oupath">OU 경로</param>
        /// <example>
        /// 이 예제는 AD 에서 CN이 Test인 사용자를 추가합니다.
        /// <code>
        /// 
        /// DNSoft.eW.FrameWork.TestTeam.EA.ActiveDirectory oAd = null;
        /// 
        /// oAd = new DNSoft.eW.FrameWork.TestTeam.EA.ActiveDirectory();
        /// 
        /// oAd.CreateUser("test2", "테스트2", "설명", "121212", "LDAP://CDIDEV.com/ou=Users,ou=eW,dc=cdidev,dc=com");
        /// 
        /// </code>
        /// </example>
        public void CreateUser(string userCN, string usersamAccountName,
            string userName,
            string description,
            string password,
            string oupath)
        {
            TimeStamp ts = null;
            if (LogHandler.LogStandard)
            {
                ts = new TimeStamp();
                ts.TimeStampStart();
            }

            DirectoryEntry oEnt = null;
            DirectoryEntry oUser = null;

            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                //제거예정 : 2010-11-09 김동훈
                ////string debug1 = WindowsIdentity.GetCurrent().Name;
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                ////string debug2  = WindowsIdentity.GetCurrent().Name;
                imp = new Impersonation();
                imp.Impersonate();

                //already exists
                if (IsExist(userCN))
                {
                    throw new Exception("already exists(sAMAccountName=" + userCN + ")");
                }

                oEnt = new DirectoryEntry(oupath);

                // Use the Add method to add a user in an organizational unit.
                oUser = oEnt.Children.Add("CN=" + userCN, "user");
                // Set the samAccountName, then commit changes to the directory.
                oUser.Properties["samAccountName"].Value = usersamAccountName;
                // Set the UserPrincipalName
                oUser.Properties["UserPrincipalName"].Value = usersamAccountName + @"@" + Domain.ToString();
                oUser.Properties["displayName"].Value = userName;
                oUser.Properties["description"].Value = description;

                //처음으로 로그인 할때 패스워드 셋팅하는 메세지 안 나오게
                //(0=다음 로그인 할때 반드 암호변경,-1=안해도 됨)
                oUser.Properties["pwdLastSet"].Value = -1;
                oUser.CommitChanges();
                //사용자 개체 사용가능하게 만들기
                EnableUser(oUser);
                //패스워드 셋팅
                SetPassword(oUser, password);
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oEnt != null) oEnt.Dispose();
                if (oUser != null) oUser.Dispose();
                if (LogHandler.LogStandard && ts != null) ts.TimeStampEnd(this, MethodInfo.GetCurrentMethod().Name);
                if (ts != null) ts.Dispose();
            }
        }


        /// <summary>
        /// 사용자를 삭제한다.
        /// </summary>
        /// <param name="userCN">사용자의 CN값</param>
        /// <example>
        /// 이 예제는 AD 에서 CN이 Test인 사용자를 제거합니다.
        /// <code>
        /// DNSoft.eW.FrameWork.TestTeam.EA.ActiveDirectory oAd = null;
        /// 
        /// oAd = new DNSoft.eW.FrameWork.TestTeam.EA.ActiveDirectory();
        /// 
        /// oAd.DeleteUser("test");
        /// </code>
        /// </example>
        public void DeleteUser(string userCN)
        {
            //Comment
            DirectoryEntry oUser = null;
            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                //제거예정 : 2010-11-09 김동훈
                ////string debug1 = WindowsIdentity.GetCurrent().Name;
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                ////string debug2  = WindowsIdentity.GetCurrent().Name;
                imp = new Impersonation();
                imp.Impersonate();

                oUser = GetDirectoryEntry(userCN);
                oUser.Parent.Children.Remove(oUser);
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oUser != null) oUser.Dispose();
            }
        }

        /// <summary>
        /// Setting Password
        /// </summary>
        /// <param name="user">사용자의 DirectoryEntry개체</param>
        /// <param name="password">셋팅할 패스워드</param>
        private void SetPassword(DirectoryEntry user, string password)
        {
            //Comment			
            try
            {
                user.Invoke("SetPassword", new object[] { password });
                user.CommitChanges();
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {

            }
        }

        /// <summary>
        /// - 최초작성자 : DotNetSoft 류승태<br/>
        /// - 최초작성일 : 2009-02-24<br/>
        /// - 내		용	: 암호수정함수 SetPassword의 리턴값을 true/false로받게끔
        /// </summary>
        /// <param name="UserCN">UserCN(Account)</param>
        /// <param name="newPassword">새암호</param>
        /// <param name="exError">에러를 받기 위한 Exception</param>
        /// <returns>변경됐으면 true, 안됐으면 False</returns>
        public bool SetPasswordGetBoolean(string UserCN, string newPassword, ref Exception exError)
        {
            bool bReturn = false;

            DirectoryEntry oUser = null;
            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                //제거예정 : 2010-11-09 김동훈
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero); // ChangePassword일때는 없어도 됐는데 SetPassword에서는 필요함
                imp = new Impersonation();
                imp.Impersonate();

                oUser = GetDirectoryEntry(UserCN);
                oUser.Invoke("SetPassword", new object[] { newPassword });
                oUser.CommitChanges();

                bReturn = true;
            }
            catch (Exception ex)
            {
                exError = ex;
                bReturn = false;
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oUser != null) oUser.Dispose();
            }
            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserCN"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public string ChangeUserPassword(string UserCN, string newPassword)
        {
            //Comment			
            DirectoryEntry oUser = null;
            //제거예정 : 2010-11-09 김동훈
             //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;
            string strReturn = "OK";

            try
            {
                //제거예정 : 2010-11-09 김동훈
                 //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                 imp = new Impersonation();
                 imp.Impersonate();

                oUser = GetDirectoryEntry(UserCN);
                oUser.Invoke("SetPassword", new object[] { newPassword });
                oUser.CommitChanges();


            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
                strReturn = ex.ToString();
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oUser != null) oUser.Dispose();
            }
            return strReturn;
        }

        /// <summary>
        /// 사용자 암호변경
        /// </summary>
        /// <param name="UserCN"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        public void ChangeUserPassword(string UserCN, string oldPassword, string newPassword)
        {
            //Comment
            DirectoryEntry oUser = null;
            try
            {
                oUser = GetDirectoryEntry(UserCN);
                oUser.Invoke("ChangePassword", new object[] { oldPassword, newPassword });
                oUser.CommitChanges();
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                if (oUser != null) oUser.Dispose();
            }
        }

        /// <summary>
        /// - 최초작성자 : DotNetSoft 류승태<br/>
        /// - 최초작성일 : 2009-02-09<br/>
        /// - 내		용	: 암호수정함수 ChangeUserPassword의 리턴값을 true/false로받게끔
        /// </summary>
        /// <param name="UserCN">UserCN(Account)</param>
        /// <param name="oldPassword">이전암호</param>
        /// <param name="newPassword">새암호</param>
        /// <param name="exError">에러를 받기 위한 Exception</param>
        /// <returns>변경됐으면 true, 안됐으면 False</returns>
        public bool ChangeUserPasswordGetBoolean(string UserCN, string oldPassword, string newPassword, ref Exception exError)
        {
            bool bReturn = false;

            //Comment
            DirectoryEntry oUser = null;
            try
            {
                oUser = GetDirectoryEntry(UserCN);
                oUser.Invoke("ChangePassword", new object[] { oldPassword, newPassword });
                oUser.CommitChanges();

                bReturn = true;
            }
            catch (Exception ex)
            {
                exError = ex;
                bReturn = false;
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                if (oUser != null) oUser.Dispose();
            }
            return bReturn;
        }

        /// <summary>
        /// 사용자 Enable(기본적으로 생성을 하면 Disable되게 생성이 된다.)
        /// </summary>
        /// <param name="user">사용자의 DirectoryEntryr개체</param>
        private void EnableUser(DirectoryEntry user)
        {
            //Comment


            try
            {
                int val = (int)user.Properties["userAccountControl"].Value;
                user.Properties["userAccountControl"].Value = val & ~(int)ADS_USER_FLAG_ENUM.ADS_UF_ACCOUNTDISABLE;
                user.CommitChanges();
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 사용자 Disable
        /// </summary>
        /// <param name="user">사용자의 DirectoryEntryr개체</param>
        private void DisableUser(DirectoryEntry user)
        {
            //Comment
            TimeStamp ts = null;
            if (LogHandler.LogStandard)
            {
                ts = new TimeStamp();
                ts.TimeStampStart();
            }

            try
            {
                int val = (int)user.Properties["userAccountControl"].Value;
                user.Properties["userAccountControl"].Value = val | (int)ADS_USER_FLAG_ENUM.ADS_UF_ACCOUNTDISABLE;
                user.CommitChanges();
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                if (LogHandler.LogStandard && ts != null) ts.TimeStampEnd(this, MethodInfo.GetCurrentMethod().Name);
                if (ts != null) ts.Dispose();
            }
        }

        /// <summary>
        /// 사용자 계정 종료날짜 지정
        /// </summary>
        /// <param name="user">사용자의 DirectoryEntryr개체</param>
        /// <param name="expireDate">종료날짜</param>
        private void SetUserAccountExpiration(DirectoryEntry user, string expireDate)
        {
            //Comment

            try
            {
                Type type = user.NativeObject.GetType();
                Object adsNative = user.NativeObject;
                type.InvokeMember("AccountExpirationDate", BindingFlags.SetProperty, null,
                    adsNative, new object[] { expireDate });
                user.CommitChanges();
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
            }
        }


        /// <summary>
        /// Active Directory Search / LdapSearch
        /// 2009.02.02 유병현
        /// </summary>
        /// <param name="pKeyword">검색어</param>
        /// <param name="pLdapServer">ldap서버주소</param>
        /// <returns>DataTable</returns>
        public DataTable GetLdapSearch(string pKeyword, string pLdapServer)
        {
            // Ldap Search // 변수 초기화!
            string ldapSearchFilter = string.Format("(|(cn=*{0}*)(mail=*{0}*))", pKeyword);

            DirectoryEntry rootEntry = new DirectoryEntry(String.Format("LDAP://{0}", pLdapServer), "", "", AuthenticationTypes.Anonymous);
            DataTable dtList = null;
            DataRow drUser = null;

            try
            {
                // 테이블 생성(dtList)
                dtList = new DataTable();
                dtList.Columns.Add("ChekcBox", Type.GetType("System.String"));
                dtList.Columns.Add("userid", Type.GetType("System.String"));
                dtList.Columns.Add("username", Type.GetType("System.String"));
                dtList.Columns.Add("usertitle", Type.GetType("System.String"));
                dtList.Columns.Add("mailaddress", Type.GetType("System.String"));
                dtList.Columns.Add("useraccount", Type.GetType("System.String"));
                dtList.Columns.Add("telephone", Type.GetType("System.String"));
                dtList.Columns.Add("mobile", Type.GetType("System.String"));
                dtList.Columns.Add("deptname", Type.GetType("System.String"));
                dtList.Columns.Add("company", Type.GetType("System.String"));

                // DirectorySearcher 
                DirectorySearcher dsearcher = new DirectorySearcher(rootEntry);
                dsearcher.Filter = ldapSearchFilter;
                dsearcher.Sort.PropertyName = "cn";
                dsearcher.Sort.Direction = SortDirection.Ascending;

                // Directory 검색결과를 처리.
                SearchResultCollection srResults = dsearcher.FindAll();
                ResultPropertyCollection srProperty = null;

                foreach (SearchResult srResult in srResults)
                {
                    // 
                    // 테이블에 넣을 데이터 변수를 읽는다.
                    string strUserName = null;
                    string strUserTitle = null;
                    string strMailAddress = null;
                    string strUserAccount = null;
                    string strTelephone = null;
                    string strMobile = null;
                    string strDeptName = null;
                    string strCompany = null;

                    // 검색결과의 프러퍼티를 읽는다.
                    srProperty = srResult.Properties;
                    foreach (string sKey in srProperty.PropertyNames)
                    {
                        // 키에 해당하는 값을 읽는다!
                        StringBuilder sbTemp = new StringBuilder();
                        for (int i = 0; i < srResult.Properties[sKey].Count; i++)
                        {
                            if (i == 0)
                            {
                                sbTemp.AppendFormat("{0}", srResult.Properties[sKey][i].ToString());
                            }
                            else
                            {
                                sbTemp.AppendFormat("/{0}", srResult.Properties[sKey][i].ToString());
                            }
                        }

                        switch (sKey)
                        {
                            case "cn":              // 이름
                                strUserName = sbTemp.ToString();
                                break;
                            case "title":           // 직책
                                strUserTitle = sbTemp.ToString();
                                break;
                            case "ou":              // 부서
                                strDeptName = sbTemp.ToString();
                                break;
                            case "mail":            // 메일주소
                                strMailAddress = sbTemp.ToString();
                                break;
                            case "uid":             // 아이디
                                strUserAccount = sbTemp.ToString();
                                break;
                            case "telephonenumber":       // 전화번호
                                strTelephone = sbTemp.ToString();
                                break;
                            case "mobile":          // 휴대전화
                                strMobile = sbTemp.ToString();
                                break;
                            case "o":               // 회사                                
                                strCompany = sbTemp.ToString();
                                break;
                            default:
                                break;
                        }
                    }

                    // dtList에 행데이터를 지정하고 추가한다..
                    drUser = dtList.NewRow();
                    drUser["userid"] = "";
                    drUser["username"] = strUserName;
                    drUser["usertitle"] = strUserTitle;
                    drUser["mailaddress"] = strMailAddress;
                    drUser["useraccount"] = strUserAccount;
                    drUser["telephone"] = strTelephone;
                    drUser["mobile"] = strMobile;
                    drUser["deptname"] = strDeptName;
                    drUser["company"] = strCompany;

                    dtList.Rows.Add(drUser);
                }
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                if (rootEntry != null) rootEntry.Dispose();
            }
            return dtList;
        }


        /// <summary>
        /// Active Directory Search / GetLdapSearchOrgset
        /// 2010.09.10 박항록 --조회조건 추가
        /// </summary>
        /// <param name="pKeyword">검색어</param>
        /// <param name="pLdapServer">ldap서버주소</param>
        /// <returns>DataTable</returns>
        public DataTable GetLdapSearchOrgset(string pKeyword, string pLdapServer)
        {
            // Ldap Search // 변수 초기화!
            string ldapSearchFilter = string.Format("(|(cn={0})(mail={0}))", pKeyword);

            DataTable dtList = null;
            DataRow drUser = null;
            DirectoryEntry rootEntry = new DirectoryEntry(String.Format("LDAP://{0}", pLdapServer), "", "", AuthenticationTypes.Anonymous);
            DataView viewLIst = null;

            try
            {
                // 테이블 생성(dtList)
                dtList = new DataTable();
 
                dtList.Columns.Add("USERID", Type.GetType("System.String"));
                dtList.Columns.Add("USERACCOUNT", Type.GetType("System.String"));
                dtList.Columns.Add("EMPID", Type.GetType("System.String"));
                dtList.Columns.Add("USERNAME_DISPLAY", Type.GetType("System.String"));
                dtList.Columns.Add("USERNAME_KOR", Type.GetType("System.String"));
                dtList.Columns.Add("USERNAME_ENG", Type.GetType("System.String"));
                dtList.Columns.Add("USERNAME_CHN", Type.GetType("System.String"));
                dtList.Columns.Add("USERNAME_JPN", Type.GetType("System.String"));
                dtList.Columns.Add("COMPANYCODE", Type.GetType("System.String"));
                dtList.Columns.Add("ROOTCD", Type.GetType("System.String"));
                dtList.Columns.Add("DEPTID", Type.GetType("System.String"));
                dtList.Columns.Add("DEPTCD", Type.GetType("System.String"));
                dtList.Columns.Add("JIKWI_DISPLAY", Type.GetType("System.String"));
                dtList.Columns.Add("DEPTNAME_DISPLAY", Type.GetType("System.String"));
                dtList.Columns.Add("DUTYNAME", Type.GetType("System.String"));
                dtList.Columns.Add("DEPTNAME_KOR", Type.GetType("System.String"));
                dtList.Columns.Add("DEPTNAME_ENG", Type.GetType("System.String"));
                dtList.Columns.Add("DEPTNAME_CHN", Type.GetType("System.String"));
                dtList.Columns.Add("DEPTNAME_JPN", Type.GetType("System.String"));
                dtList.Columns.Add("JIKCHAEKID", Type.GetType("System.String"));
                dtList.Columns.Add("JIKCHAEK_DISPLAY", Type.GetType("System.String"));
                dtList.Columns.Add("JIKCHAEK_KOR", Type.GetType("System.String"));
                dtList.Columns.Add("JIKCHAEK_ENG", Type.GetType("System.String"));
                dtList.Columns.Add("JIKCHAEK_CHN", Type.GetType("System.String"));
                dtList.Columns.Add("JIKCHAEK_JPN", Type.GetType("System.String"));
                dtList.Columns.Add("JIKWIID", Type.GetType("System.String"));
                dtList.Columns.Add("JIKWI_KOR", Type.GetType("System.String"));
                dtList.Columns.Add("JIKWI_ENG", Type.GetType("System.String"));
                dtList.Columns.Add("JIKWI_CHN", Type.GetType("System.String"));
                dtList.Columns.Add("JIKWI_JPN", Type.GetType("System.String"));

                dtList.Columns.Add("JIKGEUP_DISPLAY", Type.GetType("System.String"));
                dtList.Columns.Add("JIKGEUP_KOR", Type.GetType("System.String"));
                dtList.Columns.Add("JIKGEUP_ENG", Type.GetType("System.String"));
                dtList.Columns.Add("JIKGEUP_CHN", Type.GetType("System.String"));
                dtList.Columns.Add("JIKGEUP_JPN", Type.GetType("System.String"));
                dtList.Columns.Add("MAILADDRESS", Type.GetType("System.String"));
                dtList.Columns.Add("PHONE", Type.GetType("System.String"));
                dtList.Columns.Add("OFFICEPHONE", Type.GetType("System.String"));
                dtList.Columns.Add("HANDPHONE", Type.GetType("System.String"));
                dtList.Columns.Add("ADDRESS", Type.GetType("System.String"));
                dtList.Columns.Add("FAX", Type.GetType("System.String"));
                dtList.Columns.Add("MAILSERVER", Type.GetType("System.String"));
                dtList.Columns.Add("PHOTOFILE", Type.GetType("System.String"));



                // DirectorySearcher 
                DirectorySearcher dsearcher = new DirectorySearcher(rootEntry);
                dsearcher.Filter = ldapSearchFilter;
                dsearcher.Sort.PropertyName = "cn";
                dsearcher.Sort.Direction = SortDirection.Ascending;

                // Directory 검색결과를 처리.
                SearchResultCollection srResults = dsearcher.FindAll();
                ResultPropertyCollection srProperty = null;

                foreach (SearchResult srResult in srResults)
                {
                    // 
                    // 테이블에 넣을 데이터 변수를 읽는다.
                    string strUserName = null;
                    string strUserTitle = null;
                    string strMailAddress = null;
                    string strUserAccount = null;
                    string strTelephone = null;
                    string strMobile = null;
                    string strDeptName = null;
                    string strCompany = null;

                    // 검색결과의 프러퍼티를 읽는다.
                    srProperty = srResult.Properties;
                    foreach (string sKey in srProperty.PropertyNames)
                    {
                        // 키에 해당하는 값을 읽는다!
                        StringBuilder sbTemp = new StringBuilder();
                        for (int i = 0; i < srResult.Properties[sKey].Count; i++)
                        {
                            if (i == 0)
                            {
                                sbTemp.AppendFormat("{0}", srResult.Properties[sKey][i].ToString());
                            }
                            else
                            {
                                sbTemp.AppendFormat("/{0}", srResult.Properties[sKey][i].ToString());
                            }
                        }

                        switch (sKey)
                        {
                            case "cn":              // 이름
                                strUserName = sbTemp.ToString();
                                break;
                            case "title":           // 직책
                                strUserTitle = sbTemp.ToString();
                                break;
                            case "ou":              // 부서
                                strDeptName = sbTemp.ToString();
                                break;
                            case "mail":            // 메일주소
                                strMailAddress = sbTemp.ToString();
                                break;
                            case "uid":             // 아이디
                                strUserAccount = sbTemp.ToString();
                                break;
                            case "telephonenumber":       // 전화번호
                                strTelephone = sbTemp.ToString();
                                break;
                            case "mobile":          // 휴대전화
                                strMobile = sbTemp.ToString();
                                break;
                            case "o":               // 회사                                
                                strCompany = sbTemp.ToString();
                                break;
                            default:
                                break;
                        }
                    }

                    // dtList에 행데이터를 지정하고 추가한다..
                    drUser = dtList.NewRow();
                    if (strCompany.Split('/').Length > 1)
                        strCompany = strCompany.Split('/')[1].ToString();
                    else
                        strCompany = strCompany.Split('/')[0].ToString();

                    drUser["USERID"] = "";
                    drUser["USERACCOUNT"] = strUserAccount;
                    drUser["EMPID"] = "";
                    drUser["USERNAME_DISPLAY"] = strUserName;
                    drUser["USERNAME_KOR"] = strUserName;
                    drUser["USERNAME_ENG"] = "";
                    drUser["USERNAME_CHN"] = "";
                    drUser["USERNAME_JPN"] = "";
                    drUser["COMPANYCODE"] = strCompany;
                    drUser["ROOTCD"] = "";
                    drUser["DEPTID"] = "";
                    drUser["DEPTCD"] = "";
                    drUser["JIKWI_DISPLAY"] = strUserTitle;
                    drUser["DEPTNAME_DISPLAY"] = strCompany + "-" + strDeptName;
                    drUser["DUTYNAME"] = "";
                    drUser["DEPTNAME_KOR"] = strDeptName;
                    drUser["DEPTNAME_ENG"] = "";
                    drUser["DEPTNAME_CHN"] = "";
                    drUser["DEPTNAME_JPN"] = "";
                    drUser["JIKCHAEKID"] = "";
                    drUser["JIKCHAEK_DISPLAY"] = "";
                    drUser["JIKCHAEK_KOR"] = "";
                    drUser["JIKCHAEK_ENG"] = "";
                    drUser["JIKCHAEK_CHN"] = "";
                    drUser["JIKCHAEK_JPN"] = "";
                    drUser["JIKWIID"] = "";
                    drUser["JIKWI_KOR"] = strUserTitle;
                    drUser["JIKWI_ENG"] = "";
                    drUser["JIKWI_CHN"] = "";
                    drUser["JIKWI_JPN"] = "";
                    drUser["JIKGEUP_DISPLAY"] = "";
                    drUser["JIKGEUP_KOR"] = "";
                    drUser["JIKGEUP_ENG"] = "";
                    drUser["JIKGEUP_CHN"] = "";
                    drUser["JIKGEUP_JPN"] = "";
                    drUser["MAILADDRESS"] = strMailAddress;
                    drUser["PHONE"] = strTelephone;
                    drUser["OFFICEPHONE"] = strTelephone;
                    drUser["HANDPHONE"] = strMobile;
                    drUser["ADDRESS"] = "";
                    drUser["FAX"] = "";
                    drUser["MAILSERVER"] = "";
                    drUser["PHOTOFILE"] = "";

                    dtList.Rows.Add(drUser);

                }
                if (dtList != null && dtList.Rows.Count > 0)
                {
                    viewLIst = dtList.DefaultView;
                    viewLIst.Sort = "COMPANYCODE ASC, USERNAME_KOR ASC, JIKWI_DISPLAY ASC, DEPTNAME_KOR ASC, MAILADDRESS ASC";
                    dtList = viewLIst.ToTable();
                }
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, new Exception(ex.ToString() + " : pKeyword =" + pKeyword + " : pLdapServer=" + pLdapServer, ex), this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                if (rootEntry != null) rootEntry.Dispose();
            }
            return dtList;
        }




        #endregion

        #region Group Management(Create, Delete, AddMember, DelMember)


        /// <summary>
        /// 그룹정보를 가져온다.
        /// </summary>
        /// <param name="groupCN">그룹 CN값</param>
        /// <returns>DataTable</returns>
        public DataTable GetGroupInfo(string groupCN)
        {
            //Comment
            DirectoryEntry oGroup = null;
            DataTable dtGroup = null;
            DataRow drGroup = null;

            try
            {
                dtGroup = new DataTable();
                dtGroup.Columns.Add("cn", Type.GetType("System.String"));
                dtGroup.Columns.Add("sAMAccountName", Type.GetType("System.String"));
                dtGroup.Columns.Add("name", Type.GetType("System.String"));
                dtGroup.Columns.Add("displayName", Type.GetType("System.String"));
                dtGroup.Columns.Add("distinguishedName", Type.GetType("System.String"));
                dtGroup.Columns.Add("description", Type.GetType("System.String"));
                dtGroup.Columns.Add("mail", Type.GetType("System.String"));
                dtGroup.Columns.Add("mailNickName", Type.GetType("System.String"));
                dtGroup.Columns.Add("objectGUID", Type.GetType("System.String"));
                dtGroup.Columns.Add("whenCreated", Type.GetType("System.String"));
                dtGroup.Columns.Add("whenChanged", Type.GetType("System.String"));
                dtGroup.Columns.Add("isSecurity", Type.GetType("System.Boolean"));

                oGroup = GetDirectoryEntry(groupCN);

                drGroup = dtGroup.NewRow();

                drGroup["cn"] = oGroup.Properties["cn"].Value;
                drGroup["sAMAccountName"] = oGroup.Properties["sAMAccountName"].Value;
                drGroup["name"] = oGroup.Properties["name"].Value;
                drGroup["displayName"] = oGroup.Properties["displayName"].Value;
                drGroup["distinguishedName"] = oGroup.Properties["distinguishedName"].Value;
                drGroup["description"] = oGroup.Properties["description"].Value;
                drGroup["mail"] = oGroup.Properties["mail"].Value;
                drGroup["mailNickName"] = oGroup.Properties["mailNickName"].Value;
                drGroup["objectGUID"] = oGroup.Guid.ToString();
                drGroup["whenCreated"] = oGroup.Properties["whenCreated"].Value;
                drGroup["whenChanged"] = oGroup.Properties["whenChanged"].Value;
                drGroup["isSecurity"] = (((int)oGroup.Properties["groupType"].Value & ADS_GROUP_TYPE_SECURITY_ENABLED)
                    == ADS_GROUP_TYPE_SECURITY_ENABLED) ? true : false;

                dtGroup.Rows.Add(drGroup);

            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
            }
            return dtGroup;
        }


        /// <summary>
        /// 그룹의 멤버를 DataTable형태로 가져온다.
        /// </summary>
        /// <param name="groupCN">그룹 CN값</param>
        /// <returns>DataTable</returns>
        public DataTable GetMembersOfGroup(string groupCN)
        {
            //Comment
            DirectoryEntry oGroup = null;
            DataTable dtMemberList = null;
            DataRow drMember = null;

            try
            {
                dtMemberList = new DataTable();
                dtMemberList.Columns.Add("cn", Type.GetType("System.String"));
                dtMemberList.Columns.Add("sAMAccountName", Type.GetType("System.String"));
                dtMemberList.Columns.Add("name", Type.GetType("System.String"));
                dtMemberList.Columns.Add("displayName", Type.GetType("System.String"));
                dtMemberList.Columns.Add("distinguishedName", Type.GetType("System.String"));
                dtMemberList.Columns.Add("description", Type.GetType("System.String"));
                dtMemberList.Columns.Add("objectGUID", Type.GetType("System.String"));

                oGroup = GetDirectoryEntry(groupCN);
                Object oMembers = oGroup.Invoke("Members", null);

                foreach (Object member in (IEnumerable)oMembers)
                {

                    DirectoryEntry oMember = new DirectoryEntry(member);

                    drMember = dtMemberList.NewRow();

                    drMember["cn"] = oMember.Properties["cn"].Value;
                    drMember["sAMAccountName"] = oMember.Properties["sAMAccountName"].Value;
                    drMember["name"] = oMember.Properties["name"].Value;
                    drMember["displayName"] = oMember.Properties["displayName"].Value;
                    drMember["distinguishedName"] = oMember.Properties["distinguishedName"].Value;
                    drMember["description"] = oMember.Properties["description"].Value;
                    drMember["objectGUID"] = oMember.Guid.ToString();

                    dtMemberList.Rows.Add(drMember);

                }

            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                if (oGroup != null) oGroup.Dispose();
            }
            return dtMemberList;
        }

        /// <summary>
        /// 해당 그룹에 멤버로 포함되어 있는지에 대한 체크
        /// </summary>
        /// <param name="groupCN">그룹 CN값</param>
        /// <param name="memberCN">사용자 CN값</param>
        /// <returns>bool</returns>
        public bool IsMember(string groupCN, string memberCN)
        {
            //Comment
            DirectoryEntry oUser = null;
            DirectoryEntry oGroup = null;
            bool bMember = false;

            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                //제거예정 : 2010-11-09 김동훈
                ////string debug1 = WindowsIdentity.GetCurrent().Name;
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                ////string debug2  = WindowsIdentity.GetCurrent().Name;
                imp = new Impersonation();
                imp.Impersonate();

                oGroup = GetDirectoryEntry(groupCN);
                oUser = GetDirectoryEntry(memberCN);

                bMember = (bool)oGroup.Invoke("IsMember", new object[] { oUser.Path });

            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oGroup != null) oGroup.Dispose();
                if (oUser != null) oUser.Dispose();
            }
            return bMember;
        }

        /// <summary>
        /// 그룹을 AD에 생성한다.
        /// </summary>
        /// <param name="groupCN">그룹CN</param>
        /// <param name="groupName">그룹이름</param>
        /// <param name="description">설명</param>
        /// <param name="groupType">그룹종류</param>
        /// <param name="securityEnabled">보안그룹여부</param>
        /// <param name="oupath">OU LDAP Path</param>
        /// <param name="groupGUID">생성된 그룹의 GUID</param>
        public void CreateGroup(string groupCN, string groupName, string description, ADS_GROUP_TYPE_ENUM groupType, bool securityEnabled, string oupath, ref Guid groupGUID)
        {
            CreateGroup(groupCN, groupName, description, groupType, securityEnabled, true, oupath, ref groupGUID);
        }
        /// <summary>
        /// 그룹을 AD에 생성한다.
        /// </summary>
        /// <param name="groupCN">그룹CN</param>
        /// <param name="groupName">그룹이름</param>
        /// <param name="description">설명</param>
        /// <param name="groupType">그룹종류</param>
        /// <param name="securityEnabled">보안그룹여부</param>
        /// <param name="oupath">OU LDAP Path</param>		
        public void CreateGroup(string groupCN, string groupName, string description, ADS_GROUP_TYPE_ENUM groupType, bool securityEnabled, string oupath)
        {
            Guid GroupGUID = Guid.Empty;
            CreateGroup(groupCN, groupName, description, groupType, securityEnabled, oupath, ref GroupGUID);
        }

        public void CreateGroup(string groupCN, string groupName, string description, ADS_GROUP_TYPE_ENUM groupType, bool securityEnabled, bool mailboxEnable, string oupath, ref Guid groupGUID)
        {
            CreateGroup(groupCN, groupCN, groupName, description, groupType, securityEnabled, mailboxEnable, oupath, ref groupGUID);
        }
        /// <summary>
        /// 그룹을 AD에 생성한다.
        /// </summary>
        /// <param name="groupCN">그룹CN</param>
        /// <param name="groupName">그룹이름</param>
        /// <param name="description">설명</param>
        /// <param name="groupType">그룹종류</param>
        /// <param name="securityEnabled">보안그룹여부</param>
        /// <param name="mailboxEnable">메일박스 활성화 여부</param>
        /// <param name="oupath">OU LDAP Path</param>
        /// <param name="groupGUID">생성된 그룹의 GUID</param>
        public void CreateGroup(string groupCN, string groupsamAccountName, string groupName, string description, ADS_GROUP_TYPE_ENUM groupType, bool securityEnabled, bool mailboxEnable, string oupath, ref Guid groupGUID)
        {
            TimeStamp ts = null;
            if (LogHandler.LogStandard)
            {
                ts = new TimeStamp();
                ts.TimeStampStart();
            }

            DirectoryEntry oEnt = null;
            DirectoryEntry oGroup = null;
            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            string smtpMailaddress = string.Empty;
            string strMailAddr = string.Empty;
            string strDomain = string.Empty;


            try
            {
                //제거예정 : 2010-11-09 김동훈
                ////string debug1 = WindowsIdentity.GetCurrent().Name;
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                ////string debug2  = WindowsIdentity.GetCurrent().Name;
                imp = new Impersonation();
                imp.Impersonate();

                //already exists group
                if (IsExist(groupCN))
                {
                    throw new Exception("already exists group(SAMAccountName=" + groupCN + ")");
                }

                oEnt = new DirectoryEntry(oupath);
                oGroup = oEnt.Children.Add("CN=" + groupCN, "group");
                oGroup.Properties["samAccountName"].Value = groupsamAccountName;
                oGroup.Properties["displayName"].Value = groupName;
                oGroup.Properties["description"].Value = description;

                if (securityEnabled)
                {
                    oGroup.Properties["groupType"].Value = (int)groupType | ADS_GROUP_TYPE_SECURITY_ENABLED;
                }
                else
                {
                    oGroup.Properties["groupType"].Value = (int)groupType;
                }
                oGroup.CommitChanges();

                if (mailboxEnable)
                {
                    strDomain = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/MailDomain");
                    smtpMailaddress = "SMTP:" + groupCN + "@" + strDomain;
                    strMailAddr = groupCN + "@" + strDomain;
                    oGroup.Properties["mail"].Add((object)strMailAddr);
                    //					oGroup.Properties["mailNickname"].Add((object)strMailAddr);
                    oGroup.Properties["proxyAddresses"].Add((object)smtpMailaddress);

                    oGroup.CommitChanges();
                }

                ////MailAddresss Enable
                //if(mailboxEnable)
                //{
                //    Interop.CDOEXM.IMailRecipient dl;
                //    dl = (Interop.CDOEXM.IMailRecipient)oGroup.NativeObject;

                //    //dl.Alias = groupCN;
                //    dl.Alias = groupsamAccountName;
                //    dl.AutoGenerateEmailAddresses = true;
                //    dl.MailEnable(null);

                //    oGroup.CommitChanges();
                //}

                groupGUID = oGroup.Guid;


            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (LogHandler.LogStandard && ts != null) ts.TimeStampEnd(this, MethodInfo.GetCurrentMethod().Name);
                if (ts != null) ts.Dispose();
                if (oEnt != null) oEnt.Dispose();
                if (oGroup != null) oGroup.Dispose();
            }
        }



        /// <summary>
        /// 그룹을 AD에 생성한다.
        /// </summary>
        /// <param name="groupCN">그룹CN</param>
        /// <param name="groupName">그룹이름</param>
        /// <param name="description">설명</param>
        /// <param name="groupType">그룹종류</param>
        /// <param name="securityEnabled">보안그룹여부</param>
        /// <param name="mailboxEnable">메일박스 활성화 여부</param>
        /// <param name="oupath">OU LDAP Path</param>
        /// <param name="groupGUID">생성된 그룹의 GUID</param>
        public void CreateGroupDept(string groupCN, string groupName, string description, ADS_GROUP_TYPE_ENUM groupType,
                                    bool securityEnabled, bool mailboxEnable, string oupath, string DeptCode, string ParentDeptCode,
                                    string CompanyName, ref Guid groupGUID)
        {
            TimeStamp ts = null;
            if (LogHandler.LogStandard)
            {
                ts = new TimeStamp();
                ts.TimeStampStart();
            }

            DirectoryEntry oEnt = null;
            DirectoryEntry oGroup = null;
            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;
            string strCompanyName = string.Empty;
            string smtpMailaddress1 = string.Empty;
            string strDomain = string.Empty;

            try
            {
                if (description.ToString() == "") description = " ";
                if (DeptCode.ToString() == "") DeptCode = " ";
                if (ParentDeptCode.ToString() == "") ParentDeptCode = " ";
                if (CompanyName.ToString() == "") CompanyName = " ";

                //제거예정 : 2010-11-09 김동훈
                ////string debug1 = WindowsIdentity.GetCurrent().Name;
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                ////string debug2  = WindowsIdentity.GetCurrent().Name;
                imp = new Impersonation();
                imp.Impersonate();

                //already exists group
                if (IsExist(groupCN))
                {
                    throw new Exception("already exists group(cn=" + groupCN + ")");
                }

                oEnt = new DirectoryEntry(oupath);
                oGroup = oEnt.Children.Add("CN=" + groupCN, "group");
                oGroup.Properties["samAccountName"].Value = groupCN;
                oGroup.Properties["displayName"].Value = groupName;
                oGroup.Properties["description"].Value = description;

                // 2009.06.30 알미늄에서 사용하기 위해 주석처리
                /*
                // 호남석유화학 추가 필드
                oGroup.Properties["ExADSuperiorGroupCode"].Value	= ParentDeptCode;
                oGroup.Properties["ExADInsaCode"].Value				= DeptCode;
//				oGroup.Properties["ExADUseFlag"].Value				= "Y";
                */

                if (securityEnabled)
                {
                    oGroup.Properties["groupType"].Value = (int)groupType | ADS_GROUP_TYPE_SECURITY_ENABLED;
                }
                else
                {
                    oGroup.Properties["groupType"].Value = (int)groupType;
                }
                oGroup.CommitChanges();

                //MailAddresss Enable
                if (mailboxEnable)
                {
                    //Interop.CDOEXM.IMailRecipient dl;
                    //dl = (Interop.CDOEXM.IMailRecipient)oGroup.NativeObject;

                    //생성되는 메일주소가 서버의 Domain 주소와 다를 수 있으므로

                    strDomain = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/MailDomain");

                    // 하드코딩
                    smtpMailaddress1 = "SMTP:" + groupCN + "ABC@lottealdev.co.kr";


                    //dl.Alias =  groupCN;
                    //dl.AutoGenerateEmailAddresses = true;
                    //dl.MailEnable(null);

                    //dl.MailEnable(null);
                    oGroup.Properties["proxyAddresses"].Add((object)smtpMailaddress1);

                    oGroup.CommitChanges();
                }

                groupGUID = oGroup.Guid;


            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (LogHandler.LogStandard && ts != null) ts.TimeStampEnd(this, MethodInfo.GetCurrentMethod().Name);
                if (ts != null) ts.Dispose();
                if (oEnt != null) oEnt.Dispose();
                if (oGroup != null) oGroup.Dispose();
            }
        }

        /// <summary>
        /// AD 그룹 정보를 변경
        /// </summary>
        /// <param name="groupCN">그룹CN</param>
        /// <param name="groupName">그룹이름</param>
        /// <param name="description">설명</param>
        public void UpdateGroup(string groupCN, string groupName, string description)
        {
            DirectoryEntry oGroup = null;
            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                //제거예정 : 2010-11-09 김동훈
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                imp = new Impersonation();
                imp.Impersonate();

                oGroup = GetDirectoryEntry(groupCN);

                oGroup.Properties["description"].Value = description;
                //oGroup.Properties["DisplayName"].Value = description;

                oGroup.CommitChanges();

            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oGroup != null) oGroup.Dispose();
            }
        }

        /// <summary>
        /// AD 그룹 정보를 변경
        /// </summary>
        /// <param name="groupCN">그룹CN</param>
        /// <param name="groupName">그룹이름</param>
        /// <param name="description">설명</param>
        public void UpdateGroupDept(string groupCN, string groupName, string description, string DeptCode, string ParentDeptCode, string CompanyName)
        {
            DirectoryEntry oGroup = null;
            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                if (description.ToString() == "") description = " ";
                if (DeptCode.ToString() == "") DeptCode = " ";
                if (ParentDeptCode.ToString() == "") ParentDeptCode = " ";
                if (CompanyName.ToString() == "") CompanyName = " ";

                //제거예정 : 2010-11-09 김동훈
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                imp = new Impersonation();
                imp.Impersonate();
                oGroup = GetDirectoryEntry(groupCN);

                //oGroup.Properties["description"].Value = description;
                oGroup.Properties["displayName"].Value = groupName;
                oGroup.Properties["description"].Value = description;

                // 2009.06.30 알미늄에서 사용하기 위해 주석처리
                /*
                // 호남석유화학 추가 필드
                oGroup.Properties["ExADSuperiorGroupCode"].Value	= ParentDeptCode;
                oGroup.Properties["ExADInsaCode"].Value				= DeptCode;
                //oGroup.Properties["ExADUseFlag"].Value				= "Y";
                */


                oGroup.CommitChanges();
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();

                //if (ctx != null) ctx = null;
                if (imp != null) imp.Undo();
                if (oGroup != null) oGroup.Dispose();
            }
        }

        /// <summary>
        /// AD 사용자 정보를 변경
        /// </summary>
        /// <param name="groupCN">그룹CN</param>
        /// <param name="groupName">그룹이름</param>
        /// <param name="description">설명</param>
        public void UpdateUserDesc(string UserCN, string UserName, string description)
        {
            DirectoryEntry oUser = null;
            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                //제거예정 : 2010-11-09 김동훈
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                imp = new Impersonation();
                imp.Impersonate();

                oUser = GetDirectoryEntry(UserCN);

                oUser.Properties["description"].Value = UserName;
                //oUser.Properties["DisplayName"].Value = description;

                oUser.CommitChanges();

            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oUser != null) oUser.Dispose();
            }
        }

        /// <summary>
        /// 호남석유화학에서 추가된 AD 사용자 정보를 변경
        /// </summary>
        /// <param name="groupCN">그룹CN</param>
        /// <param name="groupName">그룹이름</param>
        /// <param name="description">설명</param>
        public void UpdateUserHPC(string UserCN, string empID, string deptCode,
                                    string jikwiCode, string jikchaekCode, string jikgeupCode, string companyName, string DeptName)
        {
            DirectoryEntry oUser = null;
            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;
            string strDeptCD = string.Empty;

            try
            {
                if (empID.ToString() == "") empID = " ";
                if (deptCode.ToString() == "") deptCode = " ";
                if (jikwiCode.ToString() == "") jikwiCode = " ";
                if (jikchaekCode.ToString() == "") jikchaekCode = " ";
                if (jikgeupCode.ToString() == "") jikgeupCode = " ";
                if (companyName.ToString() == "") companyName = " ";
                if (DeptName.ToString() == "") DeptName = " ";

                strDeptCD = deptCode.Substring(3, deptCode.Length - 3);

                //제거예정 : 2010-11-09 김동훈
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                imp = new Impersonation();
                imp.Impersonate();

                oUser = GetDirectoryEntry(UserCN);

                oUser.Properties["ExADEmployeeID"].Value = empID;	// 사번
                oUser.Properties["ExADInsaCode"].Value = strDeptCD;	// 부서코드(ORG 제외)
                oUser.Properties["ExADTitleCode"].Value = jikwiCode;	// 직위코드
                oUser.Properties["ExADJikCheckCode"].Value = jikchaekCode;	// 직책코드
                oUser.Properties["ExADGradeCode"].Value = jikgeupCode;	// 직급코드
                oUser.Properties["Department"].Value = DeptName;		// 한글부서명
                oUser.Properties["ExADDeptCode"].Value = deptCode;		// 부서코드(ORG 포함)
                oUser.Properties["ExADUserReserved5"].Value = "P";			// 사용자 구분 (인사사용자-> “P” , 공용계정->”S” , 교육장계정-> “E”)
                oUser.Properties["company"].Value = companyName;	// 사용자 소속 한글 회사명

                oUser.CommitChanges();

            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oUser != null) oUser.Dispose();
            }
        }


        /// <summary>
        /// AD 사용자 정보를 변경
        /// </summary>
        /// <param name="groupCN">사용자CN</param>
        /// <param name="groupName">사용자이름</param>
        /// <param name="description">설명</param>
        public void UpdateUserName(string userCN, string userName, string description)
        {
            //DirectoryEntry oGroup = null;
            DirectoryEntry oUser = null;
            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                //제거예정 : 2010-11-09 김동훈
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                imp = new Impersonation();
                imp.Impersonate();

                //oGroup = GetDirectoryEntry(groupCN);
                oUser = GetDirectoryEntry(userCN);

                //oGroup.Properties["description"].Value = groupName;
                //oUser.Properties["Name"].Value = userName;
                oUser.Properties["displayName"].Value = userName;
                oUser.Properties["description"].Value = description;
                oUser.CommitChanges();
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oUser != null) oUser.Dispose();
            }
        }


        /// <summary>
        /// 그룹삭제
        /// </summary>
        /// <param name="groupCN">삭제할 그룹의 CN값</param>
        public void DeleteGroup(string groupCN)
        {
            DirectoryEntry oGroup = null;
            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                //제거예정 : 2010-11-09 김동훈
                ////string debug1 = WindowsIdentity.GetCurrent().Name;
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                ////string debug2  = WindowsIdentity.GetCurrent().Name;
                imp = new Impersonation();
                imp.Impersonate();

                oGroup = GetDirectoryEntry(groupCN);
                oGroup.Parent.Children.Remove(oGroup);


            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oGroup != null) oGroup.Dispose();
            }
        }

        /// <summary>
        /// 그룹에 대상추가
        /// </summary>
        /// <param name="groupCN">대상 그룹CN값</param>
        /// <param name="memberCN">추가할 멤버 CN값</param>
        public void AddMemberToGroup(string groupCN, string memberCN)
        {
            //Comment
            DirectoryEntry oGroup = null;
            DirectoryEntry oMember = null;
            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                //제거예정 : 2010-11-09 김동훈
                ////string debug1 = WindowsIdentity.GetCurrent().Name;
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                ////string debug2  = WindowsIdentity.GetCurrent().Name;
                imp = new Impersonation();
                imp.Impersonate();

                oGroup = GetDirectoryEntry(groupCN);
                oMember = GetDirectoryEntry(memberCN);
                oGroup.Properties["member"].Add(oMember.Properties["distinguishedName"].Value);
                oGroup.CommitChanges();


            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oGroup != null) oGroup.Dispose();
                if (oMember != null) oMember.Dispose();
            }
        }

        /// <summary>
        /// 그룹웨서 멤버 제거
        /// </summary>
        /// <param name="groupCN">대상 그룹CN값</param>
        /// <param name="memberCN">제거할 멤버의 CN값</param>
        public void RemoveMemberFromGroup(string groupCN, string memberCN)
        {
            //Comment
            DirectoryEntry oGroup = null;
            DirectoryEntry oMember = null;
            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                //제거예정 : 2010-11-09 김동훈
                ////string debug1 = WindowsIdentity.GetCurrent().Name;
                //ctx = WindowsIdentity.Impersonazte(IntPtr.Zero);
                ////string debug2  = WindowsIdentity.GetCurrent().Name;
                imp = new Impersonation();
                imp.Impersonate();

                oGroup = GetDirectoryEntry(groupCN);
                oMember = GetDirectoryEntry(memberCN);
                oGroup.Properties["member"].Remove(oMember.Properties["distinguishedName"].Value);
                oGroup.CommitChanges();


            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oGroup != null) oGroup.Dispose();
                if (oMember != null) oMember.Dispose();
            }
        }

        #endregion

        #region Exchange Task(MailBox Size, CreateMailBox)

        public void CreateUserMailbox(string strUserCN, string strHomeMDB)
        {
            DirectoryEntry oUser = null;

            try
            {
                oUser = GetDirectoryEntry(strUserCN);
                CreateUserMailbox(oUser, strHomeMDB);
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                if (oUser != null) oUser.Dispose();
            }
        }

        private void CreateUserMailbox(DirectoryEntry user, string strHomeMDB)
        {
            try
            {
                //Interop.CDOEXM.IMailboxStore mailbox;
                //mailbox = (Interop.CDOEXM.IMailboxStore)user.NativeObject;
                //mailbox.CreateMailbox(strHomeMDB);
                user.CommitChanges();
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
        }

        public bool CreateUserMailboxFolderBeforeLogon(string strFolderURL, string strLanguage, string strDomain, string strUserAccount, string strUserPassword)
        {
            // Variables.
            System.Net.HttpWebRequest Request = null;
            System.Net.WebResponse Response = null;
            System.Net.CredentialCache MyCredentialCache = null;

            bool bRet;

            try
            {
                MyCredentialCache = new System.Net.CredentialCache();
                MyCredentialCache.Add(new System.Uri(strFolderURL),
                    "NTLM",
                    new System.Net.NetworkCredential(strUserAccount, strUserPassword, strDomain)
                    );

                Request = (System.Net.HttpWebRequest)HttpWebRequest.Create(strFolderURL);
                Request.Credentials = MyCredentialCache;

                // Set Header				
                Request.Headers.Set("Accept-Language", strLanguage);
                Request.Headers.Set("Translate", "f");

                Request.KeepAlive = true;
                Request.PreAuthenticate = true;

                Request.Method = "GET";

                Response = (HttpWebResponse)Request.GetResponse();

                bRet = true;
            }
            catch
            {
                bRet = false;
            }
            finally
            {
                if (Response != null)
                    Response.Close();
            }
            return bRet;
        }

        public bool CreateUserMailboxFolderBeforeLogon(string strFolderURL, string strLanguage)
        {
            // Variables.
            System.Net.HttpWebRequest Request = null;
            System.Net.WebResponse Response = null;

            bool bRet;

            try
            {
                Request = (System.Net.HttpWebRequest)HttpWebRequest.Create(strFolderURL);

                if (Request.PreAuthenticate == false)
                    Request.PreAuthenticate = true;

                Request.Credentials = System.Net.CredentialCache.DefaultCredentials; // System Credential

                // Set Header				
                Request.Headers.Set("Accept-Language", strLanguage);
                Request.Headers.Set("Translate", "f");

                Request.KeepAlive = true;
                Request.Method = "GET";

                Response = (HttpWebResponse)Request.GetResponse();
                bRet = true;
            }
            catch
            {
                bRet = false;
            }
            finally
            {
                if (Response != null)
                    Response.Close();
            }
            return bRet;
        }

        public string[] UserMailBoxQuota(string userCN)
        {
            DirectoryEntry oUser = null;
            DirectoryEntry oMailStore = null;

            string mDBStorageQuota = string.Empty;
            string mDBOverQuotaLimit = string.Empty;
            string mDBOverHardQuotaLimit = string.Empty;

			string[] arrMailBoxQuota = null;

            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
				arrMailBoxQuota = new string[4];

                //제거예정 : 2010-11-09 김동훈
                //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                imp = new Impersonation();
                imp.Impersonate();

                oUser = GetDirectoryEntry(userCN);
                bool bUseDafault = Convert.ToBoolean(oUser.Properties["mDBUseDefaults"].Value);

				arrMailBoxQuota[0] = bUseDafault.ToString();

                if (bUseDafault)
                {
                    //Get MailStore Quota
                    string strMailSotre = Convert.ToString(oUser.Properties["homeMDB"].Value);
                    oMailStore = new DirectoryEntry(LdapPath + "/" + strMailSotre);

                    mDBStorageQuota = Convert.ToString(oMailStore.Properties["mDBStorageQuota"].Value);
                    mDBOverQuotaLimit = Convert.ToString(oMailStore.Properties["mDBOverQuotaLimit"].Value);
                    mDBOverHardQuotaLimit = Convert.ToString(oMailStore.Properties["mDBOverHardQuotaLimit"].Value);
                   // strRet = mDBStorageQuota + "|" + mDBOverQuotaLimit + "|" + mDBOverHardQuotaLimit;
                }
                else
                {
                    //Get Private MailBox Quota
                    mDBStorageQuota = Convert.ToString(oUser.Properties["mDBStorageQuota"].Value);
                    mDBOverQuotaLimit = Convert.ToString(oUser.Properties["mDBOverQuotaLimit"].Value);
                    mDBOverHardQuotaLimit = Convert.ToString(oUser.Properties["mDBOverHardQuotaLimit"].Value);
                    //strRet = mDBStorageQuota + "|" + mDBOverQuotaLimit + "|" + mDBOverHardQuotaLimit;
                }

				arrMailBoxQuota[1] = mDBStorageQuota;
				arrMailBoxQuota[2] = mDBOverQuotaLimit;
				arrMailBoxQuota[3] = mDBOverHardQuotaLimit;
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                ////To User 
                //ctx.Undo();
                if (imp != null) imp.Undo();
                if (oUser != null) oUser.Dispose();
                if (oMailStore != null) oMailStore.Dispose();
            }
			return arrMailBoxQuota;
        }



		// 발신 금지 메일 최대 용량
		public string UserMailBoxQuotaLimit(string userCN)
		{
			string strReturn = "";
			string[] arrQuota = null;

			try
			{
				//strMailBoxQuota = UserMailBoxQuota(userCN);
				arrQuota = UserMailBoxQuota(userCN);

				if (arrQuota.Length > 3)
				{
					if (arrQuota[2].IsNotNullOrEmptyEx())
					{
						strReturn = arrQuota[2];
					}
					
				/*
					// 메일 최대 용량 (수발신 금지)
					if (arrQuota[3].IsNotNullOrEmptyEx())
					{
						strReturn = arrQuota[3];
					}
					else if (arrQuota[2].IsNotNullOrEmptyEx())
					{
						strReturn = arrQuota[2];
					}
					else if (arrQuota[1].IsNotNullOrEmptyEx())
					{
						strReturn = arrQuota[1];
					}
				*/	
				}
			}
			catch (Exception ex)
			{
				eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
			}
			finally
			{

			}
			return strReturn;
		}

        #endregion

        #region Etc...(IsExist, Get Entry, Impersonation)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CN">체크할 Object의 CN값</param>
        /// <returns>bool(존재여부)</returns>
        public bool IsExist(string CN)
        {
            DirectorySearcher search = null;
            bool bRet = false;

            try
            {
                search = new DirectorySearcher();
                search.Filter = "(sAMAccountName=" + CN + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    bRet = false;
                }
                else
                {
                    bRet = true;
                }
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }

            return bRet;
        }

        /// <summary>
        /// CN값을 이용하여 DirectoryEntry반환
        /// </summary>
        /// <param name="CN">Object Common Name</param>
        /// <returns>DirectoryEntry</returns>
        public DirectoryEntry GetDirectoryEntry(string CN)
        {
            DirectoryEntry oDir = null;
            DirectorySearcher search = null;
            string strPath = null;

            try
            {
                strPath = (string)directoryEntryCache[CN];

                if (strPath == null)
                {
                    search = new DirectorySearcher();
                    search.Filter = "(sAMAccountName=" + CN + ")";
                    search.PropertiesToLoad.Add("cn");
                    SearchResult result = search.FindOne();
                    if (null != result)
                    {
                        strPath = result.Path;
                    }
                    else
                    {
                        throw new Exception("not exists object(sAMAccountName=" + CN + ")");
                    }

                    directoryEntryCache[CN] = strPath;
                }

                oDir = new DirectoryEntry(strPath);
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                if (search != null) search.Dispose();
            }
            return oDir;
        }

        /// <summary>
        /// CN값을 이용하여 DirectoryEntry반환
        /// </summary>
        /// <param name="CN">Object Common Name</param>
        /// <param name="domain">Object Common Name</domain>
        /// <param name="password">Object Common Name</password>
        /// <returns>DirectoryEntry</returns>
        //안기홍 추가
        public DirectoryEntry GetDirectoryEntry(string CN, string domain, string password)
        {
            DirectoryEntry oDir = null;
            DirectorySearcher search = null;
            string strPath = null;

            string domainAndUsername = domain + @"\" + CN;
            DirectoryEntry entry = null;
            //제거예정 : 2010-11-09 김동훈
            //WindowsImpersonationContext ctx = null;
            Impersonation imp = null;

            try
            {
                strPath = (string)directoryEntryCache[CN];

                if (strPath == null)
                {
                    //제거예정 : 2010-11-09 김동훈
                    //ctx = WindowsIdentity.Impersonate(IntPtr.Zero);
                    imp = new Impersonation();
                    imp.Impersonate();

                    entry = new DirectoryEntry(LdapPath, domainAndUsername, password);

					//Bind to the native AdsObject to force authentication.
                    Object obj = entry.NativeObject;
                    search = new DirectorySearcher(entry);
                    search.Filter = "(sAMAccountName=" + CN + ")";
                    search.PropertiesToLoad.Add("cn");
                    SearchResult result = search.FindOne();
                    if (null != result)
                    {
                        strPath = result.Path;
                    }
                    else
                    {
                        throw new Exception("not exists object(sAMAccountName=" + CN + ")");
                    }

                    directoryEntryCache[CN] = strPath;
                }

                oDir = new DirectoryEntry(strPath);
            }
            catch (Exception ex)
            {
                eWException.HandleEAADException(subType, ex, this.GetType(), MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                //제거예정 : 2010-11-09 김동훈
                //if (ctx != null) ctx.Undo();
                if (imp != null) imp.Undo();
                if (search != null) search.Dispose();
            }
            return oDir;
        }

        /// <summary>
        /// 제거예정 : 2101-11-09 김동훈 : 호출하는 코드를 찾을수 없음
        /// </summary>
        /// <param name="impersonatedUser"></param>
        /// <param name="tokenHandle"></param>
        /// <param name="dupeTokenHandle"></param>
        //private void ImpersonationStart(ref WindowsImpersonationContext impersonatedUser, ref IntPtr tokenHandle, ref IntPtr dupeTokenHandle)
        //{
        //    string strAdminAccount = string.Empty;
        //    string strAdminPassword = string.Empty;

        //    try
        //    {
        //        //Impersonation	
        //        //IntPtr tokenHandle = new IntPtr(0);
        //        //IntPtr dupeTokenHandle = new IntPtr(0);

        //        strAdminAccount = DNSoft.eW.FrameWork.eWBase.GetConfig("//eWAdmin/Account");
        //        strAdminPassword = DNSoft.eW.FrameWork.eWBase.GetConfig("//eWAdmin/Password");

        //        tokenHandle = IntPtr.Zero;
        //        dupeTokenHandle = IntPtr.Zero;

        //        if (RevertToSelf())
        //        {
        //            if (LogonUser(strAdminAccount, NetBiosDomain, strAdminPassword,
        //                LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref tokenHandle) == true)
        //            {
        //                if (DuplicateToken(tokenHandle, SecurityImpersonation, ref dupeTokenHandle) == false)
        //                {
        //                    CloseHandle(tokenHandle);
        //                    throw new Exception("Exception thrown in trying to duplicate token.");
        //                }
        //            }
        //            else
        //            {
        //                int ret = Marshal.GetLastWin32Error();
        //                throw new Exception("LogonUser failed with error code : " + Convert.ToString(ret) + "\nError: [" + Convert.ToString(ret) + "] " + GetErrorMessage(ret) + "\n");
        //            }

        //        }

        //        // The token that is passed to the following constructor must 
        //        // be a primary token in order to use it for impersonation.
        //        WindowsIdentity newId = new WindowsIdentity(dupeTokenHandle);
        //        impersonatedUser = newId.Impersonate();

        //        // Check the identity.
        //        //string debug2 = "After impersonation: "	+ WindowsIdentity.GetCurrent().Name;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion
    }
}
