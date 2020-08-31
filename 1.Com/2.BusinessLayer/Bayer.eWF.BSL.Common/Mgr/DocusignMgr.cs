using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bayer.eWF.BSL.Common.Dto;
using System.IO;
using System.Web;
using System.Text;
using System.Globalization;

namespace Bayer.eWF.BSL.Common.Mgr
{
    static class ExtensionMethods
    {
        public static T ReadAs<T>(this System.Net.Http.HttpContent content)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content.ReadAsStringAsync().Result);
        }
    }
    public class DocusignMgr : MgrBase
    {
        static readonly string proxyURI = DNSoft.eW.FrameWork.eWBase.GetConfig("//eWorkflowInfo/DocusignProxyURI");
        static readonly string baseAuthURI = DNSoft.eW.FrameWork.eWBase.GetConfig("//eWorkflowInfo/DocusignBaseAuthURI");
        static readonly string baseSignURI = DNSoft.eW.FrameWork.eWBase.GetConfig("//eWorkflowInfo/DocusignBaseSignURI");
        static readonly string secretKey = DNSoft.eW.FrameWork.eWBase.GetConfig("//eWorkflowInfo/DocusignSecretKey");
        static readonly string inregrationKey = DNSoft.eW.FrameWork.eWBase.GetConfig("//eWorkflowInfo/DocusignIntegrationKey");
        static readonly string documentPath = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/WebServer/UploadFilePath");

        #region [ Docusign API ]
        public string GetRefreshToken(string accessToken)
        {
            try
            {
                using (var client = getClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Base64Encode(inregrationKey + ":" + secretKey));
                    var values = new Dictionary<string, string>();
                    values.Add("grant_type", "refresh_token");
                    values.Add("code", accessToken);
                    var postData = new FormUrlEncodedContent(values);
                    var response = client.PostAsync(baseAuthURI + "/oauth/token", postData).Result;

                    DTO_ACCESS_TOKEN result = response.Content.ReadAs<DTO_ACCESS_TOKEN>();

                    return String.Format("{{\"success\": true, \"code\": {0}, \"data\": \"{1}\"}}", (int)response.StatusCode, result.access_token);
                }
            }
            catch (Exception e)
            {
                return String.Format("{{\"success\": false, \"message\": \"{0}\"}}", e.ToString());
            }
        }
        public string GetAccountId(string accessToken)
        {
            try
            {
                using (var client = getClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var response = client.GetAsync(baseAuthURI + "/oauth/userinfo").Result;
                    
                    DTO_DOCUSIGN_USER_INFO info = response.Content.ReadAs<DTO_DOCUSIGN_USER_INFO>();

                    return String.Format("{{\"success\": true, \"code\": {0}, \"data\": {{\"accountId\": \"{1}\", \"name\": \"{2}\", \"email\": \"{3}\"}}}}", (int)response.StatusCode, info.accounts[0].account_id, info.name, info.email);

                }
            }
            catch (Exception e)
            {
                return String.Format("{{\"success\": false, \"message\": \"{0}\"}}", e.ToString());
            }
        }
        public string GetAccessToken(string authCode)
        {
            try
            {
                using (var client = getClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Base64Encode(inregrationKey + ":" + secretKey));
                    
                    var values = new Dictionary<string, string>();
                    values.Add("grant_type", "authorization_code");
                    values.Add("code", authCode);
                    var postData = new FormUrlEncodedContent(values);
                    var response = client.PostAsync(baseAuthURI + "/oauth/token", postData).Result;

                    DTO_ACCESS_TOKEN result = response.Content.ReadAs<DTO_ACCESS_TOKEN>();

                    return String.Format("{{\"success\": true, \"code\": {0}, \"data\": \"{1}\"}}", (int)response.StatusCode, result.access_token);
                }
            }
            catch (Exception e)
            {
                return String.Format("{{\"success\": false, \"message\": \"{0}\"}}", e.ToString());
            }
        }
        public string CreateEnvelope(string accessToken, string filePath, string fileName, string cdName, string cdEmail,string documentNo)
        {

            try
            {
                string fixedPath = filePath.Replace("f:\\Project\\eworks\\web\\Storage", documentPath).Replace("F:\\project\\eworks\\web\\storage", documentPath);
            
                FileInfo ofileinfo = new FileInfo(fixedPath);
            
                if (!ofileinfo.Exists) throw new Exception("파일을 찾을 수 없습니다 [" + fixedPath + "]");

                // 한글명일 경우 깨지지 않게 하기 위해
                string filename = HttpUtility.UrlEncode(fileName, new UTF8Encoding()).Replace("+", "%20");
                string path = ofileinfo.FullName;

                byte[] buffer = System.IO.File.ReadAllBytes(path);
                String doc1b64 = Convert.ToBase64String(buffer);

                //사용자 정보 가져오기
                DTO_DOCUSIGN_USER_INFO info = GetUserInfo(accessToken);
                string accountId = "";
                for (int i = 0; i < info.accounts.Length; i++)
                {
                    if (info.accounts[i].is_default)
                    {
                        accountId = info.accounts[i].account_id;
                    }
                }
                string username = info.name;
                string useremail = info.email;
                
                DTO_DOCUSIGN_ENVELOPE_CALLBACK result1;
                //만들어 보내기
                using (var client = getClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    DTO_DOCUSIGN_ENVELOPE_PAYLOAD payload = new DTO_DOCUSIGN_ENVELOPE_PAYLOAD()
                    {
                        status = "sent",
                        emailSubject = String.Format("{0} via eWorkflow", documentNo),
                        documents = new DTO_DOCUSIGN_DOCUMENT[] {
                            new DTO_DOCUSIGN_DOCUMENT() {
                                documentId = "1",
                                name = filename,
                                documentBase64 = doc1b64
                            }
                        },
                        recipients = new DTO_DOCUSIGN_RECEIPIENTS
                        {
                            signers = new DTO_DOCUSIGN_SIGNER[] {
                                new DTO_DOCUSIGN_SIGNER(){
                                    email = useremail,
                                    name = username,
                                    recipientId = "1",
                                    routingOrder = "1"
                                }
                            },
                            certifiedDeliveries = new DTO_DOCUSIGN_SIGNER[] {
                                new DTO_DOCUSIGN_SIGNER()
                                {
                                    email = cdEmail,
                                    name = cdName,
                                    recipientId = "2",
                                    routingOrder = "2"
                                }
                            }
                        }
                    };
                    var stringPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                    var postData = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                        
                    var response = client.PostAsync(baseSignURI + "/restapi/v2.1/accounts/" + accountId + "/envelopes", postData).Result;

                    result1 = response.Content.ReadAs<DTO_DOCUSIGN_ENVELOPE_CALLBACK>();
                    if ((int)response.StatusCode != 201) throw new Exception(result1.message);

                    //만들어진 envelope의 URL 찾기
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    string envelopeId = result1.envelopeId;

                    var payload2 = new { };
                    var stringPayload2 = Newtonsoft.Json.JsonConvert.SerializeObject(payload2);
                    var postData2 = new StringContent(stringPayload2, Encoding.UTF8, "application/json");

                    response = client.PostAsync(baseSignURI + "/restapi/v2.1/accounts/" + accountId + "/envelopes/" + envelopeId + "/views/sender", postData2).Result;

                    DTO_DOCUSIGN_URL_CALLBACK result2 = response.Content.ReadAs<DTO_DOCUSIGN_URL_CALLBACK>();

                    return String.Format("{{\"success\": true, \"code\": {0}, \"data\": \"{1}\"}}", (int)response.StatusCode, result2.url);
                }
            }
            catch (Exception e)
            {
                return String.Format("{{\"success\": false, \"data\": \"{0}\"}}", e.Message);
            }
        }

        public string goLivetest(string accessToken,   string envelopeId)
        {

            try
            {
                
               
                //사용자 정보 가져오기
                DTO_DOCUSIGN_USER_INFO info = GetUserInfo(accessToken);
                string accountId = info.accounts[0].account_id;
                string username = info.name;
                string useremail = info.email;

                //DTO_DOCUSIGN_ENVELOPE_CALLBACK result1;
                //만들어 보내기
                using (var client = getClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var response = client.GetAsync(baseSignURI + "/restapi/v2.1/accounts/" + accountId + "/envelopes/" + envelopeId+ "/custom_fields" ).Result;
                    //response = client.GetAsync(baseSignURI + "/restapi/v2.1/accounts/envelopes/status").Result;

                    DTO_DOCUSIGN_URL_CALLBACK_TEST result2 = response.Content.ReadAs<DTO_DOCUSIGN_URL_CALLBACK_TEST>();

                    return String.Format("{{\"success\": true, \"code\": {0}, \"data\": \"{1}\"}}", (int)response.StatusCode, response);


                }
            }
            catch (Exception e)
            {
                return String.Format("{{\"success\": false, \"message\": \"{0}\"}}", e.Message);
            }
        }


        #endregion
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private static HttpClient getClient()
        {
            // First create a proxy object
            var proxy = new WebProxy
            {
                Address = new Uri(proxyURI),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("AD-BAYER-CNB\\MWCXC", "L0calAppl61!")

            };
            // Now create a client handler which uses that proxy
            var httpClientHandler = new HttpClientHandler
            {
                Proxy = proxy,
            };

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            return new HttpClient(handler: httpClientHandler, disposeHandler: true);
        }
        private DTO_DOCUSIGN_USER_INFO GetUserInfo(string accessToken)
        {
            using (var client = getClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = client.GetAsync(baseAuthURI + "/oauth/userinfo").Result;

                return response.Content.ReadAs<DTO_DOCUSIGN_USER_INFO>();

            }
        }
    }
}
