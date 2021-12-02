using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dao;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class CSSalesRecognitionMgr : MgrBase
    {
        #region [MergeCSSalesRecognition]
        public string MergeCSSalesRecognition(Dto.DTO_DOC_CS_SALES_RECOGNITION doc, List<Dto.DTO_DOC_CS_SALES_RECOGNITION_PRODUCT> products)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string processId = doc.PROCESS_ID;
                    if (doc.PROCESS_ID.NullObjectToEmptyEx().Length == 0)
                    {
                        using (Dao.CommonDao dao = new Dao.CommonDao())
                        {
                            processId = dao.GetNewProcessID();
                            doc.PROCESS_ID = processId;
                        }
                    }
                    using (Dao.CSSalesRecognitionDao dao = new Dao.CSSalesRecognitionDao())
                    {
                        dao.MergeCSSalesRecognition(doc);
                        dao.DeleteCSSalesRecognitionProductALL(processId);

                        foreach (Dto.DTO_DOC_CS_SALES_RECOGNITION_PRODUCT product in products)
                        {                           
                            product.PROCESS_ID = processId;
                            dao.MergeCSSalesRecognitionProduct(product);
                        }

                        
                    }
                    scope.Complete();
                    return doc.PROCESS_ID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectCSSalesRecognition]
        public Dto.DTO_DOC_CS_SALES_RECOGNITION SelectCSSalesRecognition(string processId)
        {
            try
            {
                using (Dao.CSSalesRecognitionDao dao = new Dao.CSSalesRecognitionDao())
                {
                    return dao.SelectCSSalesRecognition(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectCSSalesRecognitionProduct]
        public List<Dto.DTO_DOC_CS_SALES_RECOGNITION_PRODUCT> SelectCSSalesRecognitionProduct(string processId)
        {
            try
            {
                using (Dao.CSSalesRecognitionDao dao = new Dao.CSSalesRecognitionDao())
                {
                    return dao.SelectCSSalesRecognitionProduct(processId);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

       
    }
}
