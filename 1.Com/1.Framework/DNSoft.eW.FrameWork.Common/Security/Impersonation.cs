using System;
using System.Security;
using System.Security.Principal;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace DNSoft.eW.FrameWork.Common.Security
{
    /// <summary>
    /// <b>윈도우 로컬 및 도메인 로그온 가장</b><br/>
    /// </summary>
    public class Impersonation
    {
        private bool impersonated = false;

        // private members for holding domain user account credentials
        private string userId = String.Empty;
        private string password = String.Empty;

        /// <summary>
        /// <b>도메인 NETBIOS명 또는 도메인 FQDN</b><br/>
        /// 예) NETBIOS명 : LGCORP, 도메인 FQDN : devenv.com<br/>
        /// </summary>
        private string domain = String.Empty;

        // this will hold the security context for reverting back to the client after impersonation operations are complete
        private WindowsImpersonationContext impersonationContext = null;

        /// <summary>
        /// <b>기본 생성자</b><br/>
        /// Defautl AD를 위한 Impersonation 접속 정보<br/>
        /// </summary>
        public Impersonation() : this("", "", "")
        {
        }

        /// <summary>
        /// <b>생성자</b>
        /// </summary>
        /// <param name="userPrincipalName">사용자주체명. 예) lgcopr\lgeam 또는 lgeam@devenv.com</param>
        /// <param name="password">비밀번호</param>
        public Impersonation(string userPrincipalName, string password)
            : this(userPrincipalName, "", password)
        {
        }

        /// <summary>
        /// <b>생성자</b>
        /// </summary>
        /// <param name="userId">사용자ID(계정)</param>
        /// <param name="domain">도메인 NETBIOS명 또는 도메인 FQDN. 예) NETBIOS명 : LGCORP, 도메인 FQDN : devenv.com</param>
        /// <param name="password">비밀번호</param>
        public Impersonation(string userId, string domain, string password)
        {
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(domain) && string.IsNullOrEmpty(password))
            {
                this.userId = eWBase.GetConfig("//Account/AD/ID");
                this.domain = eWBase.GetConfig("//Account/AD/Domain");
                this.password = eWBase.GetConfig("//Account/AD/PWD");
            }
            else
            {
                this.userId = userId;
                this.domain = domain;
                this.password = password;
            }
        }

        #region 공개 속성
        /// <summary>
        /// <b> get. 현재 윈도우 식별개체</b>
        /// </summary>
        public string CurrentIdentity
        {
            get
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();

                return String.Format("{0},{1}", identity.AuthenticationType, identity.Name);
            }
        }
        #endregion

        #region 공개 메소드
        /// <summary>
        /// <b>Security Context를 Impersonation시킨다</b>
        /// </summary>
        public bool Impersonate()
        {
            try
            {
                // authenticates the domain user account and begins impersonating it
                this.impersonationContext = this.Logon().Impersonate();

                impersonated = true;
            }
            catch (Exception ex)
            {
                impersonated = false;
                throw ex;
            }

            return impersonated;
        }

        /// <summary>
        /// <b>Security Context를 Rollback함</b>
        /// </summary>
        public void Undo()
        {
            if (impersonated)
            {
                // rever back to original security context which was store in the WindowsImpersonationContext instance
                this.impersonationContext.Undo();
            }
        }
        #endregion

        #region 내부 메소드
        /// <summary>
        /// <b>window 인증처리를 함</b>
        /// </summary>
        /// <returns></returns>
        private WindowsIdentity Logon()
        {
            // initialize tokens
            WindowsIdentity winIdentity = null;
            IntPtr pExistingTokenHandle = new IntPtr(0);
            IntPtr pDuplicateTokenHandle = new IntPtr(0);
            pExistingTokenHandle = IntPtr.Zero;
            pDuplicateTokenHandle = IntPtr.Zero;
            bool bRetVal = false;

            try
            {
                bRetVal = LogonUser(this.userId, this.domain, this.password,
                                    (int)WindowLogonType.LOGON32_LOGON_NEW_CREDENTIALS, (int)WindowLogonProviderType.LOGON32_PROVIDER_WINNT50, ref pExistingTokenHandle);

                bRetVal = DuplicateToken(pExistingTokenHandle,
                                         (int)SecurityImpersonationLevel.SecurityImpersonation,
                                         ref pDuplicateTokenHandle);

                // did DuplicateToken fail?
                if (false == bRetVal)
                {
                    int nErrorCode = Marshal.GetLastWin32Error();

                    CloseHandle(pExistingTokenHandle); // close existing handle

                    throw new SecurityException(nErrorCode.ToString() + ":DuplicateToken Failed");
                }
                else
                {
                    // create new identity using new primary token
                    winIdentity = new WindowsIdentity(pDuplicateTokenHandle);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // Free the tokens.
                if (pExistingTokenHandle != IntPtr.Zero)
                    CloseHandle(pExistingTokenHandle);

                if (pDuplicateTokenHandle != IntPtr.Zero)
                    CloseHandle(pDuplicateTokenHandle);
            }

            return winIdentity;
        }
        #endregion

        #region WIN32 API Import
        /// <summary>
        /// <b>로컬 윈도우 시스템에 로그온</b><br/>
        /// http://msdn2.microsoft.com/en-us/library/aa378184.aspx<br/>
        /// </summary>
        /// <param name="lpszUsername"></param>
        /// <param name="lpszDomain"></param>
        /// <param name="lpszPassword"></param>
        /// <param name="dwLogonType"></param>
        /// <param name="dwLogonProvider"></param>
        /// <param name="phToken"></param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
                                             int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        // cretes duplicate token handle
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
                                                 int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(IntPtr handle);
        #endregion
    }
}