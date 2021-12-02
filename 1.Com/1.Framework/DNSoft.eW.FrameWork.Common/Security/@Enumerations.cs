using System;
using System.Collections.Generic;
using System.Text;

namespace DNSoft.eW.FrameWork.Common.Security
{
    #region ADS 열거형
    /// <summary>
    /// <b>The type of logon operation to perform</b>
    /// </summary>
    internal enum WindowLogonType : int
    {
        /// <summary>
        /// This logon type is intended for users who will be interactively using the computer, such as a user being logged on by a terminal server, remote shell, or similar process. This logon type has the additional expense of caching logon information for disconnected operations; therefore, it is inappropriate for some client/server applications, such as a mail server.
        /// </summary>
        LOGON32_LOGON_INTERACTIVE = 2,
        /// <summary>
        /// This logon type is intended for high performance servers to authenticate plaintext passwords. The LogonUser function does not cache credentials for this logon type.
        /// </summary>
        LOGON32_LOGON_NETWORK = 3,
        /// <summary>
        /// This logon type is intended for batch servers, where processes may be executing on behalf of a user without their direct intervention. This type is also for higher performance servers that process many plaintext authentication attempts at a time, such as mail or Web servers. The LogonUser function does not cache credentials for this logon type.
        /// </summary>
        LOGON32_LOGON_BATCH = 4,
        /// <summary>
        /// Indicates a service-type logon. The account provided must have the service privilege enabled.
        /// </summary>
        LOGON32_LOGON_SERVICE = 5,
        /// <summary>
        /// This logon type is for GINA DLLs that log on users who will be interactively using the computer. This logon type can generate a unique audit record that shows when the workstation was unlocked.
        /// </summary>
        LOGON32_LOGON_UNLOCK = 7,
        /// <summary>
        /// This logon type preserves the name and password in the authentication package, which allows the server to make connections to other network servers while impersonating the client. A server can accept plaintext credentials from a client, call LogonUser, verify that the user can access the system across the network, and still communicate with other servers.
        /// </summary>
        LOGON32_LOGON_NETWORK_CLEARTEXT = 8,
        /// <summary>
        /// This logon type allows the caller to clone its current token and specify new credentials for outbound connections. The new logon session has the same local identifier but uses different credentials for other network connections.
        /// This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider.
        /// </summary>
        LOGON32_LOGON_NEW_CREDENTIALS = 9,
    }

    /// <summary>
    /// <b>윈도우 인증 타입</b>
    /// </summary>
    internal enum WindowLogonProviderType : int
    {
        /// <summary>
        /// Use the standard logon provider for the system. The default security provider is negotiate, unless you pass NULL for the domain name and the user name is not in UPN format. In this case, the default provider is NTLM.
        /// Windows 2000 : The default security provider is NTLM. 
        /// </summary>
        LOGON32_PROVIDER_DEFAULT = 0,
        /*//
        /// <summary>
        /// not used
        /// </summary>
        LOGON32_PROVIDER_WINNT35 = 1,
        //*/
        /// <summary>
        /// Use the NTLM logon provider.
        /// </summary>
        LOGON32_PROVIDER_WINNT40 = 2,
        /// <summary>
        /// Use the negotiate logon provider. 
        /// </summary>
        LOGON32_PROVIDER_WINNT50 = 3,
    }

    /// <summary>
    /// <b>The SECURITY_IMPERSONATION_LEVEL enumeration type contains values that specify security impersonation levels.</b><br/>
    /// Security impersonation levels govern the degree to which a server process can act on behalf of a client process.</br>
    /// </summary>
    internal enum SecurityImpersonationLevel : int
    {
        /// <summary>
        /// The server process cannot obtain identification information about the client and it cannot impersonate the client. It is defined with no value given, and thus, by ANSI C rules, defaults to a value of zero. 
        /// </summary>
        SecurityAnonymous = 0,
        /// <summary>
        /// The server process can obtain information about the client, such as security identifiers and privileges, but it cannot impersonate the client. This is useful for servers that export their own objects ? for example, database products that export tables and views. Using the retrieved client-security information, the server can make access-validation decisions without being able to utilize other services using the client's security context. 
        /// </summary>
        SecurityIdentification = 1,
        /// <summary>
        /// The server process can impersonate the client's security context on its local system. The server cannot impersonate the client on remote systems. 
        /// </summary>
        SecurityImpersonation = 2,
        /// <summary>
        /// The server process can impersonate the client's security context on remote systems.
        /// </summary>
        SecurityDelegation = 3
    }
    #endregion

    /// <summary>
    /// <b>AD 개체 유형</b>
    /// </summary>
    public enum AdObjectClass : byte
    {
        /// <summary>
        /// 사용자
        /// </summary>
        User = 0,
        /// <summary>
        /// 그룹
        /// </summary>
        Group = 1
    }

    /// <summary>
    /// <b>AD 그룹범위</b>
    /// </summary>
    public enum AdGroupScope : int
    {
        /// <summary>
        /// 글로벌
        /// Specifies a group that can contain accounts from the same domain 
        /// and other global groups from the same domain. 
        /// This type of group can be exported to a different domain
        /// </summary>
        Global = 0x00000002, //ADS_GROUP_TYPE_GLOBAL_GROUP
        /// <summary>
        /// 도메인 로컬
        /// Specifies a group that can contain accounts from any domain in the forest, 
        /// other domain local groups from the same domain, global groups from any domain in the forest, 
        /// and universal groups. 
        /// This type of group cannot be included in access-control lists of resources in other domains.
        /// This type of group is intended for use with the LDAP provider
        /// </summary>
        DomainLocal = 0x00000004, //ADS_GROUP_TYPE_DOMAIN_LOCAL_GROUP
        /// <summary>
        /// 유니버설
        /// Specifies a group that can contain accounts from any domain, global groups from any domain, 
        /// and other universal groups. 
        /// This type of group cannot contain domain local groups.
        /// </summary>
        Universal = 0x00000008 //ADS_GROUP_TYPE_UNIVERSAL_GROUP
    }

    /// <summary>
    /// <b>AD 그룹 종류</b>
    /// </summary>
    public enum AdGroupType : int
    {
        /// <summary>
        /// 보안그룹
        /// Specifies a group that is security enabled. 
        /// This group can be used to apply an access-control list on 
        /// an ADSI object or a file system. 
        /// </summary>
        Security = -2147483648, //ADS_GROUP_TYPE_SECURITY_ENABLED
        /// <summary>
        /// 배포그룹
        /// </summary>
        Distribution = 0
    }
}