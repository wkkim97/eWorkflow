using Bayer.WinSvc.PushNoticeMail;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bayer.WinExe.Sap.Master
{
    public partial class frmMain : Form
    {

        bool isJob = false;

        public frmMain(string[] args)
        {
            InitializeComponent();

            if (args.Length > 0) isJob = true;
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            this.isJob = false;
            this.btnCustomer.Enabled = false;
            this.bgWorker.RunWorkerAsync();
        }

        private void GetCustomer()
        {
            try
            {
                SetLabelText(this.lblMessage, "Call Rfc Function");
                RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination("HCP");

                RfcRepository SapRfcRepository = SapRfcDestination.Repository;

                IRfcFunction BapiGetCompanyList = SapRfcRepository.CreateFunction("RFC_READ_TABLE");
                BapiGetCompanyList.SetValue("QUERY_TABLE", "/BAY4/AB_CUST");
                BapiGetCompanyList.SetValue("DELIMITER", "%");

                IRfcTable dataTable = BapiGetCompanyList.GetTable("DATA");

                BapiGetCompanyList.Invoke(SapRfcDestination);
                SetLabelText(this.lblMessage, "Parsing RfcTable");
                List<Dto.DTO_SAP_CUSTOMER> customers = new List<Dto.DTO_SAP_CUSTOMER>();
                SetLabelText(this.lblTotal, dataTable.RowCount.ToString());
                int index = 1;
                StringBuilder log = new StringBuilder();
                foreach (var dataRow in dataTable)
                {
                    string data = dataRow.GetValue("WA").ToString();
                    log.AppendLine(data);
                    string[] array = data.Split(new char[] { '%' });
                    Dto.DTO_SAP_CUSTOMER dto = new Dto.DTO_SAP_CUSTOMER();
                    dto.CUSTOMER_CODE = array[1];
                    dto.CUSTOMER_NAME = array[2];
                    dto.CUSTOMER_NAME_KR = array[3];
                    dto.COMPANY_CODE = "0695";
                    dto.PARVW = array[4];
                    dto.BU = array[7];
                    customers.Add(dto);

                    SetLabelText(this.lblCnt, index.ToString());
                    index++;

                }
                Common.WriteLog("----------------------------------------------");
                Common.WriteLog(log.ToString());
                Common.WriteLog("----------------------------------------------");
                SetLabelText(this.lblMessage, "Inserting eManage.dbo.TB_SAP_CUSTOMER");
                using (Mgr.MasterMgr mgr = new Mgr.MasterMgr())
                {
                    mgr.InsertSapCustomer(customers);
                }
                SetLabelText(this.lblMessage, "Finished!");
            }
            catch (Exception ex)
            {
                Common.WriteLog("Exception - GetCustomer()                     ");
                Common.WriteLog("----------------------------------------------");
                Common.WriteLog("ex - " + ex.ToString());
                Common.WriteLog("----------------------------------------------");
            }
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination("D2R");

            RfcRepository SapRfcRepository = SapRfcDestination.Repository;

            IRfcFunction BapiGetCompanyList = SapRfcRepository.CreateFunction("/BAY0/GZ_KO_PRODUCT_MASTER");

            BapiGetCompanyList.Invoke(SapRfcDestination);

            IRfcTable rfcTblProduct = BapiGetCompanyList.GetTable("T_PRODUCT_TABLE");

            DataTable dtProduct = InsertIntoDataTable(rfcTblProduct);
        }

        private DataTable InsertIntoDataTable(IRfcTable rfcTbl)
        {
            DataTable adoTbl = new DataTable();
            for (int liElement = 0; liElement < rfcTbl.ElementCount; liElement++)
            {
                RfcElementMetadata metadata = rfcTbl.GetElementMetadata(liElement);
                adoTbl.Columns.Add(metadata.Name, GetDataType(metadata.DataType));
            }
            foreach (IRfcStructure row in rfcTbl)
            {
                DataRow drData = adoTbl.NewRow();
                for (int liElement = 0; liElement < rfcTbl.ElementCount; liElement++)
                {
                    RfcElementMetadata metadata = rfcTbl.GetElementMetadata(liElement);

                    switch (metadata.DataType)
                    {
                        case RfcDataType.DATE:
                            drData[metadata.Name] = row.GetString(metadata.Name).Substring(0, 4) + row.GetString(metadata.Name).Substring(5, 2) + row.GetString(metadata.Name).Substring(8, 2);
                            break;
                        case RfcDataType.BCD:
                            drData[metadata.Name] = row.GetDecimal(metadata.Name);
                            break;
                        case RfcDataType.CHAR:
                            drData[metadata.Name] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.STRING:
                            drData[metadata.Name] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.INT2:
                            drData[metadata.Name] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.INT4:
                            drData[metadata.Name] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.FLOAT:
                            drData[metadata.Name] = row.GetDouble(metadata.Name);
                            break;
                        default:
                            drData[metadata.Name] = row.GetString(metadata.Name);
                            break;
                    }
                }
                adoTbl.Rows.Add(drData);
            }
            return adoTbl;
        }

        private Type GetDataType(RfcDataType rfcDataType)
        {
            switch (rfcDataType)
            {
                case RfcDataType.DATE:
                    return typeof(string);
                case RfcDataType.CHAR:
                    return typeof(string);
                case RfcDataType.STRING:
                    return typeof(string);
                case RfcDataType.BCD:
                    return typeof(decimal);
                case RfcDataType.INT2:
                    return typeof(int);
                case RfcDataType.INT4:
                    return typeof(int);
                case RfcDataType.FLOAT:
                    return typeof(double);
                default:
                    return typeof(string);
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            if (isJob)
                this.bgWorker.RunWorkerAsync();
        }

        delegate void SetLabelTextCollback(Label lbl, string text);

        void SetLabelText(Label lbl, string text)
        {
            if (lbl.InvokeRequired)
                this.Invoke(new SetLabelTextCollback(SetLabelText), new object[] { lbl, text });
            else
                lbl.Text = text;
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            GetCustomer();
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.btnCustomer.Enabled = true;
            if (isJob) Application.Exit();
        }

        private void lblCnt_Click(object sender, EventArgs e)
        {

        }

    }
}
