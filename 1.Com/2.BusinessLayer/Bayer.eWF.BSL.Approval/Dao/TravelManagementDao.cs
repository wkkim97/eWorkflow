using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class TravelManagementDao : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergeTravelManagement(Dto.DTO_DOC_TRAVEL_MANAGEMENT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_TRAVEL_MANAGEMENT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void InsertTravelManagementTraveler(Dto.DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER traveler)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(traveler);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_TRAVEL_MANAGEMENT_TRAVELER, parameters);
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
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", index);
                    parameters[2] = new SqlParameter("@TRAVELER_TYPE", type);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_TRAVEL_MANAGEMENT_TRAVELER, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteTravelManagementTravelerAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_TRAVEL_MANAGEMENT_TRAVELER_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void InsertTravelManagementRoute(Dto.DTO_DOC_TRAVEL_MANAGEMENT_ROUTE route)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(route);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_TRAVEL_MANAGEMENT_ROUTE, parameters);
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
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", index);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_TRAVEL_MANAGEMENT_ROUTE, parameters);

                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteTravelManagementRouteAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_TRAVEL_MANAGEMENT_ROUTE_ALL, parameters);

                }
            }
            catch
            {
                throw;
            }
        }

        public void InsertTravelManagementAccommodation(Dto.DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION accommodation)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(accommodation);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_TRAVEL_MANAGEMENT_ACCOMMODATION, parameters);
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
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", index);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_TRAVEL_MANAGEMENT_ACCOMMODATION, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteTravelManagementAccommodationAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_TRAVEL_MANAGEMENT_ACCOMMODATION_ALL, parameters);
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
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_TRAVEL_MANAGEMENT>(ApprovalContext.USP_SELECT_TRAVEL_MANAGEMENT, parameters);

                    return result.First();
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
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER>(ApprovalContext.USP_SELECT_TRAVEL_MANAGEMENT_TRAVELER, parameters);

                    return result.ToList();
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
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_TRAVEL_MANAGEMENT_ROUTE>(ApprovalContext.USP_SELECT_TRAVEL_MANAGEMENT_ROUTE, parameters);

                    return result.ToList();
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
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION>(ApprovalContext.USP_SELECT_TRAVEL_MANAGEMENT_ACCOMMODATION, parameters);

                    return result.ToList();

                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdateTravelManagementRequestToAgency(string processId, string updaterId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@UPDATER_ID", updaterId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_UPDATE_TRAVEL_MANAGEMENT_REQUEST_TO_AGENCY, parameters);
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
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_TRAVEL_MANAGEMENT_CALENDAR>(ApprovalContext.USP_SELECT_TRAVEL_MANAGEMENT_CALENDAR, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
