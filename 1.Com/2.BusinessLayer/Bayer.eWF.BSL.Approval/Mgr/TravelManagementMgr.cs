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
    public class TravelManagementMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeTravelManagement(Dto.DTO_DOC_TRAVEL_MANAGEMENT doc, List<Dto.DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER> travelers,
            List<Dto.DTO_DOC_TRAVEL_MANAGEMENT_ROUTE> routes, List<Dto.DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION> accommodations)
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
                    using (TravelManagementDao dao = new TravelManagementDao())
                    {
                        dao.MergeTravelManagement(doc);

                        dao.DeleteTravelManagementTravelerAll(processId);

                        foreach (Dto.DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER traveler in travelers)
                        {
                            traveler.PROCESS_ID = processId;
                            dao.InsertTravelManagementTraveler(traveler);
                        }

                        dao.DeleteTravelManagementRouteAll(processId);

                        foreach (Dto.DTO_DOC_TRAVEL_MANAGEMENT_ROUTE route in routes)
                        {
                            route.PROCESS_ID = processId;
                            dao.InsertTravelManagementRoute(route);
                        }

                        dao.DeleteTravelManagementAccommodationAll(processId);

                        foreach (Dto.DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION accommodation in accommodations)
                        {
                            accommodation.PROCESS_ID = processId;
                            dao.InsertTravelManagementAccommodation(accommodation);
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

        public Dto.DTO_DOC_TRAVEL_MANAGEMENT SelectTravelManagement(string processId)
        {
            try
            {
                using (TravelManagementDao dao = new TravelManagementDao())
                {
                    return dao.SelectTravelManagement(processId);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER> SelectTravelManagementTraveler(string processId)
        {
            try
            {
                using (TravelManagementDao dao = new TravelManagementDao())
                {
                    return dao.SelectTravelManagementTraveler(processId);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_TRAVEL_MANAGEMENT_ROUTE> SelectTravelManagementRoute(string processId)
        {
            try
            {
                using (TravelManagementDao dao = new TravelManagementDao())
                {
                    return dao.SelectTravelManagementRoute(processId);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION> SelectTravelManagementAccommodation(string processId)
        {
            try
            {
                using (TravelManagementDao dao = new TravelManagementDao())
                {
                    return dao.SelectTravelManagementAccommodation(processId);
                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdateTravelManagementRequestToAgency(string processId, string updateId)
        {
            try
            {
                using (TravelManagementDao dao = new TravelManagementDao())
                {
                    dao.UpdateTravelManagementRequestToAgency(processId, updateId);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteTravelManagementTraveler(string processId, int index, string type)
        {
            try
            {
                using (TravelManagementDao dao = new TravelManagementDao())
                {
                    dao.DeleteTravelManagementTraveler(processId, index, type);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteTravelManagementRoute(string processId, int index)
        {
            try
            {
                using (TravelManagementDao dao = new TravelManagementDao())
                {
                    dao.DeleteTravelManagementRoute(processId, index);

                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteTravelManagementAccommodation(string processId, int index)
        {
            try
            {
                using (TravelManagementDao dao = new TravelManagementDao())
                {
                    dao.DeleteTravelManagementAccommodation(processId, index);

                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_TRAVEL_MANAGEMENT_CALENDAR SelectTravelManagementCalendar(string processId)
        {
            try
            {
                using (TravelManagementDao dao = new TravelManagementDao())
                {
                    return dao.SelectTravelManagementCalendar(processId);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
