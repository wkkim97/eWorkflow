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
    public class BmskPriceMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeBmskPrice(Dto.DTO_DOC_BMSK_PRICE doc, List<Dto.DTO_DOC_BMSK_PRICE_LIST> list)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string processId = doc.PROCESS_ID;
                    if (processId.NullObjectToEmptyEx().Length == 0)
                    {
                        using (Dao.CommonDao dao = new Dao.CommonDao())
                        {
                            processId = dao.GetNewProcessID();
                            doc.PROCESS_ID = processId;
                        }
                    }
                    using (BmskPriceDao dao = new BmskPriceDao())
                    {
                        dao.MergeBmskPrice(doc);

                        dao.DeleteBmskPriceListAll(processId);

                        foreach (Dto.DTO_DOC_BMSK_PRICE_LIST item in list)
                        {
                            item.PROCESS_ID = processId;
                            dao.InsertBmskPriceList(item);
                        }
                    }

                    scope.Complete();
                    return processId;
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBmskPriceList(string processId, int index)
        {
            try
            {
                using (BmskPriceDao dao = new BmskPriceDao())
                {
                    dao.DeleteBmskPriceList(processId, index);
                }
            }
            catch
            {
                throw;
            }
        }

        public Tuple<Dto.DTO_DOC_BMSK_PRICE
            , List<Dto.DTO_DOC_BMSK_PRICE_LIST>> SelectBmskPrice(string processId)
        {
            try
            {
                using (BmskPriceDao dao = new BmskPriceDao())
                {
                    Dto.DTO_DOC_BMSK_PRICE doc = dao.SelectBmskPrice(processId);

                    List<Dto.DTO_DOC_BMSK_PRICE_LIST> list = dao.SelectBmskPriceList(processId);

                    return new Tuple<Dto.DTO_DOC_BMSK_PRICE
                        ,List<Dto.DTO_DOC_BMSK_PRICE_LIST>>(doc, list);
                }   
            }
            catch
            {
                throw;
            }
        }
    }
}
