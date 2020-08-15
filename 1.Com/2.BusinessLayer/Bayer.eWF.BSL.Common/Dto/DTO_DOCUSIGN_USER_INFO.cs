using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dto
{
    class DTO_DOCUSIGN_USER_INFO
    {
        public string sub { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string created { get; set; }
        public string email { get; set; }
        public DTO_DOCUSIGN_USER_ACCOUNT[] accounts { get; set; }

    }
    class DTO_DOCUSIGN_USER_ACCOUNT
    {
        public string account_id { get; set; }
        public bool is_default { get; set; }
        public string account_name { get; set; }
        public string base_uri { get; set; }
    }
    class DTO_ACCESS_TOKEN
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
    class DTO_DOCUSIGN_DOCUMENT
    {
        public string documentId { get; set; }
        public string name { get; set; }
        public string documentBase64 { get; set; }
    }
    class DTO_DOCUSIGN_RECEIPIENTS
    {
        public DTO_DOCUSIGN_SIGNER[] signers { get; set; }
        public DTO_DOCUSIGN_SIGNER[] certifiedDeliveries { get; set; }
    }
    class DTO_DOCUSIGN_SIGNER
    {
        public string name { get; set; }
        public string email { get; set; }
        public string recipientId { get; set; }
        public string routingOrder { get; set; }
    }
    class DTO_DOCUSIGN_ENVELOPE_PAYLOAD
    {
        public string status { get; set; }
        public string emailSubject { get; set; }
        public DTO_DOCUSIGN_DOCUMENT[] documents { get; set; }
        public DTO_DOCUSIGN_RECEIPIENTS recipients { get; set; }
    }
    class DTO_DOCUSIGN_ENVELOPE_CALLBACK
    {
        public string envelopeId { get; set; }
        public string uri { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }
    class DTO_DOCUSIGN_URL_CALLBACK
    {
        public string url { get; set; }
    }
    class DTO_DOCUSIGN_URL_CALLBACK_TEST
    {
        public string envelopeId { get; set; }
        public string documentsUri { get; set; }

        public string url { get; set; }
    }
}
