using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Bayer.WCF.Service
{
    [ServiceContract]
    public interface IAfterTreatment
    {
        [OperationContract]
        void DoProcessing(string processID);

        [OperationContract]
        void MergeMembershipApplication(string processID);

        [OperationContract]
        void SendDebitCreditNoteAttendees(string processID);

        [OperationContract]
        CreditLimit CalculateCreditLimit(string customerCode, string bgCode, string statusCode, string inputAmount);

        [OperationContract]
        string UpdateCreditLimit(string processID, string customerCode, string bgCode, string statusCode, string updater, string amount, string currentlyId);

        [OperationContract]
        string GetSapCreditLimit(string customerCode,string BU);

        [OperationContract]
        string UpdataFreeGoods(string SchemeProcessId, string Idx, string HowGrid, string Status, string userId);

        [OperationContract]
        void SendToAgencyTravelManagement(string processID);
        
        [OperationContract]
        string CancelToAgencyTravelManagement(string processID);

        [OperationContract]
        string SendToAgencyBusinessCard(string processID);

        [OperationContract]
        void UpdateCollateralStatus(string processID);
    }

    [DataContract]
    public class CreditLimit
    {
        private string result = string.Empty;
        private decimal amount = 0;

        public CreditLimit(string r, decimal amt)
        {
            this.result = r;
            this.amount = amt;
        }

        [DataMember]
        public string Result
        {
            get { return this.result; }
            set { this.result = value; }
        }

        [DataMember]
        public decimal Amount
        {
            get { return this.amount; }
            set { this.amount = value; }
        }
    }
}
