/*********************************************************************
 * Name         : HousingDao.cs
 * Description  : Implementation of IHousingDao.
 * Create Date  : 25th Jan 2016
 * Last Modified: 26th Jan 2016
 * Copyright By : Qatar Airways
 *********************************************************************/

using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.Enterprise;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using Oracle.DataAccess.Client;

namespace QR.IPrism.BusinessObjects.Implementation
{
    public class HousingDao : IHousingDao
    {
        /// <summary>
        /// Gets Housing Request details for logged in user.
        /// Shows all of request raised by user in past specify to filter.
        /// </summary>
        /// <param name="inputs">Housing Request Filter</param>
        /// <returns>List of Housing Details</returns>
        public async Task<List<HousingEO>> GetHousingSearchResultAsyc(HousingRequestFilterEO inputs)
        {
            List<HousingEO> response = new List<HousingEO>();
            HousingEO housing = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_request_type", inputs.RequestType ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_request_from_Date", Common.ToOracleDate(inputs.FromDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("pi_request_to_Date", Common.ToOracleDate(inputs.ToDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("pi_req_status", inputs.Status ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_staffno", inputs.StaffId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("prc_ser_request", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_housing.get_req_proc", parameterList))
                {
                    while (objReader.Read())
                    {
                        housing = new HousingEO();
                        housing.RequestNumber = objReader["REQUSET_NO"] != null ? objReader["REQUSET_NO"].ToString() : string.Empty;
                        housing.RequestId = objReader["REQUEST_ID"] != null ? objReader["REQUEST_ID"].ToString() : string.Empty;
                        housing.RequestType = objReader["REQUEST_TYPE"] != null ? objReader["REQUEST_TYPE"].ToString() : string.Empty;
                        housing.RequestStatus = objReader["REQEST_STATUS"] != null ? objReader["REQEST_STATUS"].ToString() : string.Empty;
                        housing.RequestDate = objReader["REQUESTED_DATE"] != null ? Convert.ToDateTime(objReader["REQUESTED_DATE"]) : default(DateTime);
                        housing.RequestDateClose = objReader["REQUEST_DATEOF_CLOSE"] != null &&
                        !String.IsNullOrWhiteSpace(objReader["REQUEST_DATEOF_CLOSE"].ToString()) ? Convert.ToDateTime(objReader["REQUEST_DATEOF_CLOSE"]) : default(DateTime);

                        response.Add(housing);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }


            return response;
        }

        /// <summary>
        /// Gets all the vacant building/flats/bedrooms details.
        /// </summary>
        /// <returns>List of housing details</returns>
        public async Task<IEnumerable<BuildingEO>> GetHousingVacantBuildingAsync(string staffId)
        {
            List<BuildingEO> response = new List<BuildingEO>();
            BuildingEO building = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("pi_staffno", staffId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_key", Constants.DECREPT_KEY, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_vacantbldg", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_housing.get_vacant_bldg_by_accom_rules", parameterList))
                {
                    while (objReader.Read())
                    {
                        building = new BuildingEO();
                        building.BuildingDetailSid = objReader["BUILDINGDETAILSID"] != null ? objReader["BUILDINGDETAILSID"].ToString() : string.Empty;
                        building.BuildingName = objReader["BUILDINGNAME"] != null ? objReader["BUILDINGNAME"].ToString() : string.Empty;
                        building.BuildingNumber = objReader["BUILDINGNUMBER"] != null ? objReader["BUILDINGNUMBER"].ToString() : string.Empty;
                        building.FlatId = objReader["FLATID"] != null ? objReader["FLATID"].ToString() : string.Empty;
                        building.FlatNumber = objReader["FLATNUMBER"] != null ? objReader["FLATNUMBER"].ToString() : string.Empty;
                        building.FlatType = objReader["FLAT_Type_Text"] != null ? objReader["FLAT_Type_Text"].ToString() : string.Empty;
                        building.BedroomNo = objReader["BEDROOM_NO"] != null ? objReader["BEDROOM_NO"].ToString() : string.Empty;
                        building.BedroomDetailsId = objReader["BEDROOM_DETAILS_ID"] != null ? objReader["BEDROOM_DETAILS_ID"].ToString() : string.Empty;
                        building.TelephoneNo = objReader["TELEPHONE_NUMBER"] != null ? objReader["TELEPHONE_NUMBER"].ToString() : string.Empty;
                        building.BuildingFacilities = objReader["Building_Facilities"] != null ? objReader["Building_Facilities"].ToString() : string.Empty;
                        building.FloorNumber = objReader["FloorNumber"] != null ? objReader["FloorNumber"].ToString() : string.Empty;
                        building.RoomCount = objReader["Room_Count"] != null ? objReader["Room_Count"].ToString() : string.Empty;
                        response.Add(building);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }

            return response;
        }

        /// <summary>
        /// Gets all the swap room building/flats/bedrooms details.
        /// </summary>
        /// <returns>List of housing details</returns>
        public async Task<IEnumerable<BuildingEO>> GetOccupBldgForSwapRoomsAsync(string staffId)
        {
            List<BuildingEO> response = new List<BuildingEO>();
            BuildingEO building = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("pi_staffno", staffId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_key", Constants.DECREPT_KEY, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_occupiedbldg", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_housing.get_occupy_bldg_by_accom_rules", parameterList))
                {
                    while (objReader.Read())
                    {
                        building = new BuildingEO();
                        building.BuildingDetailSid = objReader["BUILDINGDETAILSID"] != null ? objReader["BUILDINGDETAILSID"].ToString() : string.Empty;
                        building.BuildingName = objReader["BUILDINGNAME"] != null ? objReader["BUILDINGNAME"].ToString() : string.Empty;
                        building.BuildingNumber = objReader["BUILDINGNUMBER"] != null ? objReader["BUILDINGNUMBER"].ToString() : string.Empty;
                        building.FlatId = objReader["FLATID"] != null ? objReader["FLATID"].ToString() : string.Empty;
                        building.FlatNumber = objReader["FLATNUMBER"] != null ? objReader["FLATNUMBER"].ToString() : string.Empty;
                        building.FlatType = objReader["FLAT_TYPE_TEXT"] != null ? objReader["FLAT_TYPE_TEXT"].ToString() : string.Empty;
                        building.BedroomNo = objReader["BEDROOM_NO"] != null ? objReader["BEDROOM_NO"].ToString() : string.Empty;
                        building.BedroomDetailsId = objReader["BEDROOM_DETAILS_ID"] != null ? objReader["BEDROOM_DETAILS_ID"].ToString() : string.Empty;
                        building.TelephoneNo = objReader["TELEPHONE_NUMBER"] != null ? objReader["TELEPHONE_NUMBER"].ToString() : string.Empty;
                        building.BuildingFacilities = objReader["Building_Facilities"] != null ? objReader["Building_Facilities"].ToString() : string.Empty;
                        building.FloorNumber = objReader["FloorNumber"] != null ? objReader["FloorNumber"].ToString() : string.Empty;
                        building.RoomCount = objReader["Room_Count"] != null ? objReader["Room_Count"].ToString() : string.Empty;
                        response.Add(building);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }

            return response;
        }

        /// <summary>
        /// Gets Housing Flat Nationality
        /// </summary>
        /// <param name="inputFlat">flat number</param>
        /// <returns>Nationality</returns>
        public async Task<BuildingEO> GetHousingFlatNationalityAsync(string flatId)
        {
            BuildingEO response = new BuildingEO();
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("p_key", Constants.DECREPT_KEY, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_flat_id", flatId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_nationality", ParameterDirection.Output, OracleDbType.Varchar2, 128));
                parameterList.Add(new ODPCommandParameter("po_grade", ParameterDirection.Output, OracleDbType.Varchar2, 128));

                DbCommand command = await ODPDataAccess.ExecuteSPNonQueryParmAsync("hm_manage_abinitio_accom.get_existing_nationality", parameterList);
                response.Nationality = !command.Parameters["po_nationality"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? command.Parameters["po_nationality"].Value.ToString() : string.Empty;
                response.Grade = !command.Parameters["po_grade"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? command.Parameters["po_grade"].Value.ToString() : string.Empty;
                response.FlatId = flatId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }

            return response;
        }

        /// <summary>
        /// Creates a new housing request for move in.
        /// </summary>
        /// <returns>Response details</returns>
        public async Task<ResponseEO> InsertHousingServiceRequests(HousingEO inputs, string staffId)
        {
            if (inputs.StayOut == null)
            {
                inputs.StayOut = new HousingStayOutEO();
            }
            if (inputs.Guests == null)
            {
                inputs.Guests = new HousingGuestEO();
            }

            ResponseEO response = new ResponseEO();
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("PI_REQ_TYPE", inputs.RequestId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_CREW_ID", staffId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_STATUS", inputs.RequestStatus ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_REASON", inputs.RequestReason ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_REQ_DATE", Common.ToOracleDate(inputs.RequestDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("PI_REQ_DATE_CLOSE", Common.ToOracleDate(inputs.RequestDateClose), ParameterDirection.Input, OracleDbType.Date));

                parameterList.Add(new ODPCommandParameter("PI_DEPT", HousingConstants.Housing, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_GUEST_NAME", inputs.Guests.GuestName ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_GUEST_GENDER", inputs.Guests.GuestGender ?? string.Empty, ParameterDirection.Input, OracleDbType.NChar));
                parameterList.Add(new ODPCommandParameter("PI_RELATION", inputs.Guests.Relationship ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_CHECKIN_DATE", Common.ToOracleDate(inputs.Guests.CheckinDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("PI_CHECKOUT_DATE", Common.ToOracleDate(inputs.Guests.CheckoutDate), ParameterDirection.Input, OracleDbType.Date));

                parameterList.Add(new ODPCommandParameter("PI_NOOF_DAYS", inputs.Guests.NoOfDays, ParameterDirection.Input, OracleDbType.Int32));
                parameterList.Add(new ODPCommandParameter("PI_EXT_CHECKOUT_DATE", Common.ToOracleDate(inputs.Guests.ExtendedCheckoutDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("PI_OVER_STAYED_STATUS", inputs.Guests.OverStayedStatus ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_REQ_ITEM", inputs.RequestedItemId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_QTY", 0, ParameterDirection.Input, OracleDbType.Int32));

                parameterList.Add(new ODPCommandParameter("PI_BLDG_NEW", inputs.BuildingDetails.BuildingDetailSid ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_FLAT_NEW", inputs.BuildingDetails.FlatId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_BEDROOM_NEW", inputs.BuildingDetails.BedroomDetailsId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("PI_MAIN_TYPE", inputs.MaintainanceTypeId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_MAIN_SUB_TYPE", inputs.MaintainanceSubTypeId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("PI_BUILDING_NAME", inputs.BuildingDetails.BuildingName ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_AD_AREA", inputs.BuildingDetails.Area ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_AD_STREET", inputs.BuildingDetails.StreetNo ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_POST_NO", inputs.BuildingDetails.PostBoxNo ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_AD_MOB_NO", inputs.MobileNo ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_LAND_NO", inputs.LandLineNo ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));

                //Start - Added for Housing Requests - ChangeAccommodation, Swap Rooms, Stay Out Request
                parameterList.Add(new ODPCommandParameter("PI_FRND_STAFF_ID", inputs.StayOut.FriendStaffId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_SWAPROOM_CAT_ID", inputs.StayOut.SwapRoomCategoryId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_IS_PAINTED_CLEANED", inputs.StayOut.SwapRoomIsPaintedCleaned ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_STAY_OUT_REQ_TYPE_ID", inputs.StayOut.StayOutRequestTypeId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_STAY_OUT_FROM_DATE", Common.ToOracleDate(inputs.StayOut.StayOutRequestFromDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("PI_STAY_OUT_TO_DATE", Common.ToOracleDate(inputs.StayOut.StayOutRequestToDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("PI_STAY_OUT_RELATION", inputs.StayOut.StayOutCrewRelationId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_STAY_OUT_REL_NAME", inputs.StayOut.StayOutCrewRelationName ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                //End - Added for Housing Requests - ChangeAccommodation, Swap Rooms, Stay Out Request

                parameterList.Add(new ODPCommandParameter("PI_CREATED_BY", inputs.CreatedBy ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_MOD_BY", inputs.ModifiedBy ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PO_MSG", ParameterDirection.Output, OracleDbType.Varchar2, 500));
                parameterList.Add(new ODPCommandParameter("PO_REQ_ID", ParameterDirection.Output, OracleDbType.Varchar2, 100));
                parameterList.Add(new ODPCommandParameter("PO_REQ_DET_ID", ParameterDirection.Output, OracleDbType.Varchar2, 100));
                parameterList.Add(new ODPCommandParameter("PO_REQ_NO", ParameterDirection.Output, OracleDbType.Varchar2, 100));

                Oracle.DataAccess.Client.OracleCommand command = await ODPDataAccess.ExecuteSPNonQueryParmAsync("SERVICE_REQUEST_PKG.insert_serv_req", parameterList);
                response.Message = !command.Parameters["PO_MSG"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? command.Parameters["PO_MSG"].Value.ToString() : string.Empty;
                response.ResponseId = !command.Parameters["PO_REQ_ID"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? command.Parameters["PO_REQ_ID"].Value.ToString() : string.Empty;
                response.ResponseDetailsId = !command.Parameters["PO_REQ_DET_ID"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? command.Parameters["PO_REQ_DET_ID"].Value.ToString() : string.Empty;
                response.RequestNumber = !command.Parameters["PO_REQ_NO"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? command.Parameters["PO_REQ_NO"].Value.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }

            return response;
        }
       
        /// <summary>
        /// Gets existing accommodation details of logged in user
        /// </summary>
        /// <param name="crewdetailId">logged in user id</param>
        /// <returns>Housing details</returns>
        public async Task<HousingEO> GetExistingAccommAsync(string crewdetailId)
        {
            HousingEO housing = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_crewdetailid", crewdetailId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("prc_existaccomm", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("prc_own_accomm", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("housing_changeaccommod_pkg.get_existing_accomm", parameterList))
                {
                    while (objReader.Read())
                    {
                        housing = new HousingEO();
                        housing.BuildingDetails = new BuildingEO();
                        housing.Guests = new HousingGuestEO();
                        housing.BuildingDetails.BuildingName = objReader["bldgname"] != null ? objReader["bldgname"].ToString() : string.Empty;
                        housing.BuildingDetails.BuildingDetailSid = objReader["buildingdetailsid"] != null ? objReader["buildingdetailsid"].ToString() : string.Empty;
                        housing.BuildingDetails.BuildingNumber = objReader["bldgnum"] != null ? objReader["bldgnum"].ToString() : string.Empty;
                        housing.BuildingDetails.StreetNo = objReader["streetno"] != null ? objReader["streetno"].ToString() : string.Empty;
                        housing.BuildingDetails.Area = objReader["bldgarea"] != null ? objReader["bldgarea"].ToString() : string.Empty;
                        housing.BuildingDetails.PostBoxNo = objReader["pincode"] != null ? objReader["pincode"].ToString() : string.Empty;
                        housing.BuildingDetails.FlatNumber = objReader["flatnumber"] != null ? objReader["flatnumber"].ToString() : string.Empty;
                        housing.BuildingDetails.FlatId = objReader["flatid"] != null ? objReader["flatid"].ToString() : string.Empty;
                        housing.BuildingDetails.BedroomNo = objReader["bedroom_no"] != null ? objReader["bedroom_no"].ToString() : string.Empty;
                        housing.BuildingDetails.HouseIncharge = objReader["HOINCHARGE"] != null ? objReader["HOINCHARGE"].ToString() : string.Empty;
                        housing.Guests.CheckinDate = objReader["startdate"] != null && objReader["startdate"].ToString() != string.Empty ? Convert.ToDateTime(objReader["startdate"]) : (DateTime?)(null);
                        housing.Guests.CheckoutDate = objReader["enddate"] != null && objReader["enddate"].ToString() != string.Empty ? Convert.ToDateTime(objReader["enddate"]) : (DateTime?)(null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }
            return housing;
        }

        /// <summary>
        /// Get request details by request id
        /// </summary>
        /// <param name="requestNo">request number</param>
        /// <returns>Housing details</returns>
        public async Task<HousingNotificationEO> GetServiceRequestDetails(string requestNo)
        {
            HousingNotificationEO housing = new HousingNotificationEO();
            housing.Messages = new List<MessageEO>();
            housing.Files = new List<FileEO>();

            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            List<CommandCursorParameter> commandCursorParameter = null;

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_req_no", requestNo, ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("po_serv_req_det", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("po_comments", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("po_attachments", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_housing.get_req_all_details_proc_mig", parameterList))
                {
                    while (objReader.Read())
                    {
                        housing.RequestDetails = new HousingEO()
                        {
                            BuildingDetails = new BuildingEO(),
                            Guests = new HousingGuestEO(),
                            StayOut = new HousingStayOutEO()
                        };
                        housing.RequestDetails.RequestNumber = objReader["REQUSET_NO"] != null ? objReader["REQUSET_NO"].ToString() : string.Empty;
                        housing.RequestDetails.RequestDetailsID = objReader["hsr_request_det_id"] != null ? objReader["hsr_request_det_id"].ToString() : string.Empty;
                        housing.RequestDetails.RequestId = objReader["hsr_request_id"] != null ? objReader["hsr_request_id"].ToString() : string.Empty;
                        housing.RequestDetails.RequestTypeID = objReader["REQUEST_TYPE_ID"] != null ? objReader["REQUEST_TYPE_ID"].ToString() : string.Empty;
                        housing.RequestDetails.RequestType = objReader["REQUEST_TYPE"] != null ? objReader["REQUEST_TYPE"].ToString() : string.Empty;
                        housing.RequestDetails.RequestDate = objReader["REQUESTED_DATE"] != null && (objReader["REQUESTED_DATE"].ToString() != string.Empty) ?
                            Convert.ToDateTime(objReader["REQUESTED_DATE"]) : (DateTime?)null;
                        housing.RequestDetails.RequestReason = objReader["REQUESTREASON"] != null ? objReader["REQUESTREASON"].ToString() : string.Empty;
                        housing.RequestDetails.RequestStatus = objReader["REQEST_STATUS"] != null ? objReader["REQEST_STATUS"].ToString() : string.Empty;
                        housing.RequestDetails.CrewId = objReader["CREWDETAILSID"] != null ? objReader["CREWDETAILSID"].ToString() : string.Empty;
                        housing.RequestDetails.StaffNo = objReader["STAFFNO"] != null ? objReader["STAFFNO"].ToString() : string.Empty;
                        housing.RequestDetails.StaffName = objReader["STAFF_NAME"] != null ? objReader["STAFF_NAME"].ToString() : string.Empty;

                        housing.RequestDetails.Guests.GuestName = objReader["hsr_guest_name"] != null ? objReader["hsr_guest_name"].ToString() : string.Empty;
                        housing.RequestDetails.Guests.GuestGender = objReader["hsr_guest_gender"] != null ? objReader["hsr_guest_gender"].ToString() : string.Empty;
                        housing.RequestDetails.Guests.Relationship = objReader["RELATIONSHIP"] != null ? objReader["RELATIONSHIP"].ToString() : string.Empty;
                        housing.RequestDetails.Guests.RelationType = objReader["RELATIONSHIP_TY"] != null ? objReader["RELATIONSHIP_TY"].ToString() : string.Empty;
                        housing.RequestDetails.Guests.CheckinDate = objReader["hsr_checkin_date"] != null && (objReader["hsr_checkin_date"].ToString() != string.Empty) ?
                            Convert.ToDateTime(objReader["hsr_checkin_date"]) : (DateTime?)null;
                        housing.RequestDetails.Guests.CheckoutDate = objReader["hsr_checkout_date"] != null && (objReader["hsr_checkout_date"].ToString() != string.Empty) ?
                           Convert.ToDateTime(objReader["hsr_checkout_date"]) : (DateTime?)null;
                        housing.RequestDetails.Guests.ExtendedCheckoutDate = objReader["hsr_ext_checkout_date"] != null && (objReader["hsr_ext_checkout_date"].ToString() != string.Empty) ?
                         Convert.ToDateTime(objReader["hsr_ext_checkout_date"]) : (DateTime?)null;

                        housing.RequestDetails.Guests.NoOfDays = objReader["HSR_NOOF_DAYS"] != null && objReader["HSR_NOOF_DAYS"].ToString() != string.Empty ?
                            Convert.ToInt32(objReader["HSR_NOOF_DAYS"].ToString()) : 0;


                        housing.RequestDetails.BuildingDetails.BuildingDetailSid = objReader["hsr_buildingdetailsid_new"] != null ? objReader["hsr_buildingdetailsid_new"].ToString() : string.Empty;
                        housing.RequestDetails.BuildingDetails.BuildingName = objReader["BUILDINGNAME"] != null ? objReader["BUILDINGNAME"].ToString() : string.Empty;
                        housing.RequestDetails.BuildingDetails.FlatId = objReader["hsr_flatid_new"] != null ? objReader["hsr_flatid_new"].ToString() : string.Empty;
                        housing.RequestDetails.BuildingDetails.FlatNumber = objReader["FLATNUMBER"] != null ? objReader["FLATNUMBER"].ToString() : string.Empty;
                        housing.RequestDetails.BuildingDetails.BedroomDetailsId = objReader["hsr_bed_room_id"] != null ? objReader["hsr_bed_room_id"].ToString() : string.Empty;
                        housing.RequestDetails.BuildingDetails.BedroomNo = objReader["BEDROOM_NO"] != null ? objReader["BEDROOM_NO"].ToString() : string.Empty;

                        if (String.IsNullOrWhiteSpace(housing.RequestDetails.BuildingDetails.BuildingName))
                        {
                            housing.RequestDetails.BuildingDetails.BuildingName = objReader["HSR_BUILDINGNAME"] != null ? objReader["HSR_BUILDINGNAME"].ToString() : string.Empty;
                        }

                        housing.RequestDetails.BuildingDetails.Area = objReader["HSR_AD_AREA"] != null ? objReader["HSR_AD_AREA"].ToString() : string.Empty;
                        housing.RequestDetails.BuildingDetails.StreetNo = objReader["HSR_AD_STREETNO"] != null ? objReader["HSR_AD_STREETNO"].ToString() : string.Empty;
                        housing.RequestDetails.BuildingDetails.PostBoxNo = objReader["HSR_AD_POSTBOX_NO"] != null ? objReader["HSR_AD_POSTBOX_NO"].ToString() : string.Empty;
                        housing.RequestDetails.MobileNo = objReader["HSR_AD_MOBILE_NO"] != null ? objReader["HSR_AD_MOBILE_NO"].ToString() : string.Empty;
                        housing.RequestDetails.LandLineNo = objReader["HSR_AD_LAND_NO"] != null ? objReader["HSR_AD_LAND_NO"].ToString() : string.Empty;


                        //Start - Added for Housing Requests - ChangeAccommodation, Swap Rooms, Stay Out Request
                        housing.RequestDetails.StayOut.FriendStaffId = objReader["HSR_FRIEND_STAFF_ID"] != null ? objReader["HSR_FRIEND_STAFF_ID"].ToString() : string.Empty;
                        housing.RequestDetails.StayOut.FriendStaffName = objReader["FRNDSTAFFNAME"] != null ? objReader["FRNDSTAFFNAME"].ToString() : string.Empty;
                        housing.RequestDetails.StayOut.SwapRoomCategoryId = objReader["HSR_SWAP_CATEGORY_ID"] != null ? objReader["HSR_SWAP_CATEGORY_ID"].ToString() : string.Empty;
                        housing.RequestDetails.StayOut.SwapRoomCategory = objReader["HSR_SWAP_CATEGORY"] != null ? objReader["HSR_SWAP_CATEGORY"].ToString() : string.Empty;
                        housing.RequestDetails.StayOut.SwapRoomIsPaintedCleaned = objReader["hsr_painted_cleaned"] != null ? objReader["hsr_painted_cleaned"].ToString() : string.Empty;

                        housing.RequestDetails.StayOut.StayOutRequestTypeId = objReader["HSR_STAY_OUT_REQ_TYPE_ID"] != null ? objReader["HSR_STAY_OUT_REQ_TYPE_ID"].ToString() : string.Empty;
                        housing.RequestDetails.StayOut.StayOutRequestType = objReader["STAYOUT_TYPE"] != null ? objReader["STAYOUT_TYPE"].ToString() : string.Empty;

                        housing.RequestDetails.StayOut.StayOutRequestFromDate = objReader["HSR_STAY_OUT_FROM_DATE"] != null && (objReader["HSR_STAY_OUT_FROM_DATE"].ToString() != string.Empty) ? Convert.ToDateTime(objReader["HSR_STAY_OUT_FROM_DATE"]) : (DateTime?)null;
                        housing.RequestDetails.StayOut.StayOutRequestToDate = objReader["HSR_STAY_OUT_TO_DATE"] != null && (objReader["HSR_STAY_OUT_TO_DATE"].ToString() != string.Empty) ? Convert.ToDateTime(objReader["HSR_STAY_OUT_TO_DATE"]) : (DateTime?)null;

                        housing.RequestDetails.StayOut.StayOutCrewRelationId = objReader["HSR_STAY_OUT_RELATION"] != null ? objReader["HSR_STAY_OUT_RELATION"].ToString() : string.Empty;
                        housing.RequestDetails.StayOut.StayOutCrewRelation = objReader["RELATIONS_LIST"] != null ? objReader["RELATIONS_LIST"].ToString() : string.Empty;

                        housing.RequestDetails.StayOut.StayOutCrewRelationName = objReader["HSR_STAY_OUT_REL_NAME"] != null ? objReader["HSR_STAY_OUT_REL_NAME"].ToString() : string.Empty;
                    }

                    objReader.NextResult();

                    while (objReader.Read())
                    {
                        var msg = new MessageEO();
                        msg.Id = objReader["commentid"] != null ? objReader["commentid"].ToString() : string.Empty;
                        msg.Message = objReader["commentdescription"] != null ? objReader["commentdescription"].ToString() : string.Empty;
                        msg.CreatedDate = objReader["commentdate"] != null ? objReader["commentdate"].ToString() : string.Empty;
                        msg.CreatedBy = objReader["crewname"] != null ? objReader["crewname"].ToString() : string.Empty;
                        housing.Messages.Add(msg);
                    }

                    objReader.NextResult();

                    while (objReader.Read())
                    {
                        var file = new FileEO();
                        file.FileId = objReader["attachmentid"] != null ? objReader["attachmentid"].ToString() : string.Empty;
                        file.ClassKeyId = objReader["alertid"] != null ? objReader["alertid"].ToString() : string.Empty;
                        file.FileName = objReader["attachmentname"] != null ? objReader["attachmentname"].ToString() : string.Empty;
                        file.EntityTypeId = objReader["entity_type"] != null ? objReader["entity_type"].ToString() : string.Empty;
                        file.FileContent = objReader["attachmentcontent"] as byte[];
                        housing.Files.Add(file);                            
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }
            return housing;
        }

        /// <summary>
        /// Cancel housing request
        /// </summary>
        /// <param name="requestId">by request id</param>
        public async void CancelHousingRequest(string requestId)
        {
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_req_id", requestId, ParameterDirection.Input, OracleDbType.Varchar2));
                await ODPDataAccess.ExecuteSPNonQueryParmAsync("SERVICE_REQUEST_PKG.cancel_request", parameterList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get crew entitlements list details 
        /// </summary>
        /// <param name="staffId">crew id</param>
        /// <returns></returns>
        public async Task<CrewInfoEO> GetCrewEntitlements(string staffId)
        {
            CrewInfoEO response = new CrewInfoEO()
            {
                CrewEntitlements = new List<CrewEntitlementEO>()
            };

            List<CrewEntitlementEO> crewEntitlement = new List<CrewEntitlementEO>();

            CrewEntitlementEO row = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("P_CREW_ID", staffId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("P_ISABINITIO", ParameterDirection.Output, OracleDbType.Varchar2, 500));
                parameterList.Add(new ODPCommandParameter("P_ISABENTITLED", ParameterDirection.Output, OracleDbType.Varchar2, 100));
                parameterList.Add(new ODPCommandParameter("P_ISCREWENTITLED", ParameterDirection.Output, OracleDbType.Varchar2, 100));
                parameterList.Add(new ODPCommandParameter("P_ISCOUPLE", ParameterDirection.Output, OracleDbType.Varchar2, 100));
                parameterList.Add(new ODPCommandParameter("PRC_CREW_ENTITLEMENTS", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("SERVICE_REQUEST_PKG.get_Crew_Entitlements", parameterList))
                {
                    while (objReader.Read())
                    {
                        row = new CrewEntitlementEO();
                        row.EntitlementYear = objReader["ENTITLEMENTYEAR"] != null ? objReader["ENTITLEMENTYEAR"].ToString() : string.Empty;
                        row.EntitlementAvailable = objReader["ENTITLEMENTAVAILABLE"] != null ? objReader["ENTITLEMENTAVAILABLE"].ToString() : string.Empty;
                        row.EntitlementLastUsed = objReader["ENTITLEMENTLASTUSED"] != null ? Convert.ToDateTime(objReader["ENTITLEMENTLASTUSED"]) : (DateTime?)null;
                        crewEntitlement.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }

            response.CrewEntitlements = crewEntitlement;

            return response;
        }

        public async Task<CrewEntitlementEO> GetHousingEntitlements(string staffId, string requestTypeName)
        {
            CrewEntitlementEO response = new CrewEntitlementEO();
            List<ODPCommandParameter> parameterList = new List<ODPCommandParameter>();
            List<CommandCursorParameter> commandCursorParameter = new List<CommandCursorParameter>();
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            //IDataReader objReader = null;

            try
            {
                parameterList.Add(new ODPCommandParameter("pi_req_type_name", requestTypeName, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_staffno", staffId, ParameterDirection.Input, OracleDbType.Varchar2));

                parameterList.Add(new ODPCommandParameter("po_entitle", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("po_change", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("po_guest_entitle", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_housing.get_housing_entitlement", parameterList))
                {
                    response = new CrewEntitlementEO();

                    while (objReader.Read())
                    {
                        response.IsCrewEntitled = objReader["entitle"] != null ? objReader["entitle"].ToString().ToUpper() == "YES" : false;
                        response.Message = objReader["outmessage"] != null ? objReader["outmessage"].ToString() : string.Empty;
                    }

                    objReader.NextResult();

                    while (objReader.Read())
                    {
                        response.EntitlementYear = objReader["ENTITLEMENTYEAR"] != null ? objReader["ENTITLEMENTYEAR"].ToString() : string.Empty;
                        response.EntitlementAvailable = objReader["ENTITLEMENTAVAILABLE"] != null ? objReader["ENTITLEMENTAVAILABLE"].ToString() : string.Empty;
                        response.EntitlementLastUsed = objReader["ENTITLEMENTLASTUSED"] != null && objReader["ENTITLEMENTLASTUSED"].ToString() != string.Empty ? Convert.ToDateTime(objReader["ENTITLEMENTLASTUSED"]) : (DateTime?)null;
                    }

                    objReader.NextResult();

                    while (objReader.Read())
                    {
                        response.Setup_slots = objReader["SETUP_SLOTS"] != null ? objReader["SETUP_SLOTS"].ToString() : string.Empty;
                        response.Setup_days = objReader["SETUP_DAYS"] != null ? objReader["SETUP_DAYS"].ToString() : string.Empty;
                        response.Used_No_of_slots = objReader["NO_OF_SLOTS"] != null ? objReader["NO_OF_SLOTS"].ToString() : string.Empty;
                        response.Used_No_of_days = objReader["NO_OF_DAYS"] != null ? objReader["NO_OF_DAYS"].ToString() : string.Empty;
                        response.Next_No_of_slots = objReader["NEXT_NO_OF_SLOTS"] != null ? objReader["NEXT_NO_OF_SLOTS"].ToString() : string.Empty;
                        response.Next_No_of_days = objReader["NEXT_NO_OF_DAYS"] != null ? objReader["NEXT_NO_OF_DAYS"].ToString() : string.Empty;
                        response.IsEntitledCurrentYear = objReader["CURR_ENTITLE"] != null ? objReader["CURR_ENTITLE"].ToString().ToUpper() == "YES" : false;
                        response.IsEntitledNextYear = objReader["NEXT_ENTITLE"] != null ? objReader["NEXT_ENTITLE"].ToString().ToUpper() == "YES" : false;
                    }

                    // entitle , outmessage 
                    // ENTITLEMENTYEAR	ENTITLEMENTAVAILABLE	ENTITLEMENTLASTUSED
                    // SETUP_SLOTS	SETUP_DAYS	NO_OF_SLOTS	NO_OF_DAYS	NEXT_NO_OF_SLOTS	NEXT_NO_OF_DAYS	CURR_ENTITLE	NEXT_ENTITLE
                }


                //response = new CrewEntitlementEO();
                //response.IsCrewEntitled = true;
                //response.Message = "You are not entitled for this accommodation";
                //response.EntitlementYear = "12/03/2016";
                //response.EntitlementAvailable = "Yes";
                //response.EntitlementLastUsed = new DateTime();

                //response.IsEntitledCurrentYear = true;
                //response.IsEntitledNextYear = true;
                //response.Next_No_of_slots = "25";
                //response.Next_No_of_days = "2";
                //response.Used_No_of_days = "20";
                //response.Used_No_of_slots = "2";
                //response.Setup_slots = "3";
                //response.Setup_days = "30";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //objReader.Close();
                ODPDataAccess.CloseConnection();
            }

            return response;
        }

        public async Task<IEnumerable<HousingGuestEO>> GetGuestDetails(string staffNo)
        {
            List<HousingGuestEO> response = new List<HousingGuestEO>();
            HousingGuestEO row = null;

            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_staffno", staffNo, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_guestdet", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_housing.get_guest_details_iprism_proc", parameterList))
                {
                    while (objReader.Read())
                    {
                        row = new HousingGuestEO();
                        row.GuestName = objReader["relation_name"] != null ? objReader["relation_name"].ToString() : string.Empty;
                        //row.RelationType = objReader["Relation_Type"] != null ? objReader["Relation_Type"].ToString() : string.Empty;
                        row.GuestGender = objReader["GENDER"] != null ? objReader["GENDER"].ToString() : string.Empty;
                        row.Relationship = objReader["relation"] != null ? objReader["relation"].ToString() : string.Empty;
                        response.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }

            return response;
        }

        public async Task<IEnumerable<HousingGuestEO>> GetCrewRelationDetails(string staffNo)
        {
            List<HousingGuestEO> response = new List<HousingGuestEO>();
            HousingGuestEO row = null;

            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_staff_id", staffNo, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("prc_RelationDet", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("SERVICE_REQUEST_PKG.get_Crew_Relations", parameterList))
                {
                    while (objReader.Read())
                    {
                        row = new HousingGuestEO();
                        row.GuestName = objReader["relation_name"] != null ? objReader["relation_name"].ToString() : string.Empty;
                        row.RelationType = objReader["Relation_Type"] != null ? objReader["Relation_Type"].ToString() : string.Empty;
                        row.GuestGender = objReader["GENDER"] != null ? objReader["GENDER"].ToString() : string.Empty;
                        row.Relationship = objReader["relation"] != null ? objReader["relation"].ToString() : string.Empty;
                        row.NameWithRelation = objReader["NAMEWITHRELATION"] != null ? objReader["NAMEWITHRELATION"].ToString() : string.Empty;
                        response.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }

            return response;
        }


        public async Task<bool> IsGuestAccomDateOverlap(string crewDetId, DateTime? fromDate, DateTime? toDate)
        {
            int response = 0;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("pi_crewdetailsid", crewDetId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_checkin_dt", Common.ToOracleDate(fromDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("pi_checkout_dt", Common.ToOracleDate(toDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("po_req_count", ParameterDirection.Output, OracleDbType.Varchar2, 10));

                DbCommand command = await ODPDataAccess.ExecuteSPNonQueryParmAsync("HOUSING_SERVICE_REQUEST_PKG.check_guest_accom_overlap", parameterList);
                response = !command.Parameters["po_req_count"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? Convert.ToInt32(command.Parameters["po_req_count"].Value.ToString()) : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }

            return response > 0 ? true : false;
        }

        public async Task<IEnumerable<BuildingEO>> GetFlatAvailabiltyDetails(string flatNo, string buildingName)
        {
            List<BuildingEO> flats = new List<BuildingEO>();
            BuildingEO row = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_flat_no", flatNo, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_build_name", buildingName, ParameterDirection.Input, OracleDbType.Varchar2, 500));
                parameterList.Add(new ODPCommandParameter("p_key", Constants.DECREPT_KEY, ParameterDirection.Input, OracleDbType.Varchar2, 100));
                parameterList.Add(new ODPCommandParameter("pro_flat_alloc_detail", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("HM_MANAGE_ABINITIO_ACCOM.get_flat_accommodation", parameterList))
                {
                    while (objReader.Read())
                    {
                        row = new BuildingEO();
                        row.StaffId = objReader["staffid"] != null ? objReader["staffid"].ToString() : string.Empty;
                        flats.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }

            return flats;
        }

        public async Task<IEnumerable<UserContextEO>> GetStaffsByFlatId(string flatId, string buildingId)
        {
            List<UserContextEO> result = new List<UserContextEO>();
            UserContextEO row = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("PI_BUILDING_DETAILS_ID", buildingId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("PI_FLAT_ID", flatId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("prc_staff_flats", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("service_request_pkg.get_Flat_StaffDetails", parameterList))
                {
                    while (objReader.Read())
                    {
                        row = new UserContextEO();
                        row.CrewDetailsId = objReader["crewdetailsid"] != null ? objReader["crewdetailsid"].ToString() : string.Empty;
                        row.StaffName = objReader["staffname"] != null ? objReader["staffname"].ToString() : string.Empty;
                        row.StaffNumber = objReader["STAFFNO"] != null ? objReader["STAFFNO"].ToString() : string.Empty;
                        //row.StaffId = objReader["FIRSTNAME"] != null ? objReader["FIRSTNAME"].ToString() : string.Empty;
                        //row.StaffId = objReader["MIDDLENAME"] != null ? objReader["MIDDLENAME"].ToString() : string.Empty;
                        //row.StaffId = objReader["LASTNAME"] != null ? objReader["LASTNAME"].ToString() : string.Empty;
                        row.Grade = objReader["GRADE"] != null ? objReader["GRADE"].ToString() : string.Empty;
                        row.Nationality = objReader["NATIONALITY"] != null ? objReader["NATIONALITY"].ToString() : string.Empty;
                        result.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }

            return result;
        }

        public async Task<IEnumerable<UserContextEO>> GetStaffByBedroomId_Swap(string bedroomId, string buildingId)
        {
            List<UserContextEO> result = new List<UserContextEO>();
            UserContextEO row = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_building_details_id", buildingId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_bedroom_details_id", bedroomId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("prc_staff_berooms", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("service_request_pkg.get_bedroom_staffdetails", parameterList))
                {
                    while (objReader.Read())
                    {
                        row = new UserContextEO();
                        row.CrewDetailsId = objReader["crewdetailsid"] != null ? objReader["crewdetailsid"].ToString() : string.Empty;
                        row.StaffName = objReader["staffname"] != null ? objReader["staffname"].ToString() : string.Empty;
                        row.StaffNumber = objReader["STAFFNO"] != null ? objReader["STAFFNO"].ToString() : string.Empty;
                        //row.StaffId = objReader["FIRSTNAME"] != null ? objReader["FIRSTNAME"].ToString() : string.Empty;
                        //row.StaffId = objReader["MIDDLENAME"] != null ? objReader["MIDDLENAME"].ToString() : string.Empty;
                        //row.StaffId = objReader["LASTNAME"] != null ? objReader["LASTNAME"].ToString() : string.Empty;
                        row.Grade = objReader["GRADE"] != null ? objReader["GRADE"].ToString() : string.Empty;
                        row.Nationality = objReader["NATIONALITY"] != null ? objReader["NATIONALITY"].ToString() : string.Empty;
                        result.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }

            return result;
        }

        public async Task<bool> UpdateFlatMateApproval(string requestNo, string status)
        {
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_req_no", requestNo, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_status", status, ParameterDirection.Input, OracleDbType.Varchar2));
                await ODPDataAccess.ExecuteSPNonQueryParmAsync("SERVICE_REQUEST_PKG.update_flatmate_approv_status", parameterList);
            }
            catch (Exception ex)
            {
                return false;
                throw ex;

            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }
            return true;
        }

        public async Task<HousingNotificationEO> GetHousingNotification(string id, string staffNo)
        {
            HousingNotificationEO response = null;
            List<ODPCommandParameter> parametersList = new List<ODPCommandParameter>();
            List<CommandCursorParameter> commandCursorParameter = new List<CommandCursorParameter>();
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList.Add(new ODPCommandParameter("pi_notification_id", id, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("staffnumber", staffNo, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("prc_serv_req_det", ParameterDirection.Output, OracleDbType.RefCursor));
                    parametersList.Add(new ODPCommandParameter("prc_notification", ParameterDirection.Output, OracleDbType.RefCursor));
                    parametersList.Add(new ODPCommandParameter("prc_existaccomm", ParameterDirection.Output, OracleDbType.RefCursor));
                    parametersList.Add(new ODPCommandParameter("prc_own_accomm", ParameterDirection.Output, OracleDbType.RefCursor));
                    parametersList.Add(new ODPCommandParameter("prc_init_existaccomm", ParameterDirection.Output, OracleDbType.RefCursor));
                    parametersList.Add(new ODPCommandParameter("prc_init_own_accomm", ParameterDirection.Output, OracleDbType.RefCursor));
                
                    response = new HousingNotificationEO();

                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_housing.get_initor_acc_by_ntfyid_proc", parametersList))
                    {

                        while (objReader.Read())
                        {
                            var housing = new HousingEO()
                            {
                                Guests = new HousingGuestEO()
                            };

                            housing.RequestReason = objReader["REQUESTREASON"] != null ? objReader["REQUESTREASON"].ToString() : string.Empty;
                            housing.RequestStatus = objReader["REQEST_STATUS"] != null ? objReader["REQEST_STATUS"].ToString() : string.Empty;
                            housing.CrewId = objReader["CREWDETAILSID"] != null ? objReader["CREWDETAILSID"].ToString() : string.Empty;
                            housing.StaffNo = objReader["STAFFNO"] != null ? objReader["STAFFNO"].ToString() : string.Empty;
                            housing.StaffName = objReader["STAFF_NAME"] != null ? objReader["STAFF_NAME"].ToString() : string.Empty;
                            housing.RequestId = objReader["hsr_request_id"] != null ? objReader["hsr_request_id"].ToString() : string.Empty;
                            housing.RequestNumber = objReader["REQUSET_NO"] != null ? objReader["REQUSET_NO"].ToString() : string.Empty;

                            housing.Guests.GuestName = objReader["hsr_guest_name"] != null ? objReader["hsr_guest_name"].ToString() : string.Empty;
                            housing.Guests.GuestGender = objReader["hsr_guest_gender"] != null ? objReader["hsr_guest_gender"].ToString() : string.Empty;
                            housing.Guests.Relationship = objReader["RELATIONSHIP"] != null ? objReader["RELATIONSHIP"].ToString() : string.Empty;
                            housing.Guests.RelationType = objReader["RELATIONSHIP_TY"] != null ? objReader["RELATIONSHIP_TY"].ToString() : string.Empty;
                            housing.Guests.CheckinDate = objReader["hsr_checkin_date"] != null && (objReader["hsr_checkin_date"].ToString() != string.Empty) ?
                                Convert.ToDateTime(objReader["hsr_checkin_date"]) : (DateTime?)null;
                            housing.Guests.CheckoutDate = objReader["hsr_checkout_date"] != null && (objReader["hsr_checkout_date"].ToString() != string.Empty) ?
                               Convert.ToDateTime(objReader["hsr_checkout_date"]) : (DateTime?)null;
                            housing.Guests.ExtendedCheckoutDate = objReader["hsr_ext_checkout_date"] != null && (objReader["hsr_ext_checkout_date"].ToString() != string.Empty) ?
                             Convert.ToDateTime(objReader["hsr_ext_checkout_date"]) : (DateTime?)null;

                            housing.Guests.NoOfDays = objReader["HSR_NOOF_DAYS"] != null && objReader["HSR_NOOF_DAYS"].ToString() != string.Empty ?
                                Convert.ToInt32(objReader["HSR_NOOF_DAYS"].ToString()) : 0;


                            response.RequestDetails = housing;
                        }

                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            var row = new NotificationDetailsEO();
                            row.Id = objReader["notificationid"] != null ? objReader["notificationid"].ToString() : string.Empty;
                            row.Description = objReader["notificationdesc"] != null ? objReader["notificationdesc"].ToString() : string.Empty;
                            row.Type = objReader["notificationtype"] != null ? objReader["notificationtype"].ToString() : string.Empty;
                            row.Date = objReader["notificationdate"] != null && objReader["notificationdate"].ToString() != string.Empty ?
                               Convert.ToDateTime(objReader["notificationdate"].ToString()) : (DateTime?)null;
                            row.ActionByDate = objReader["actionbydate"] != null && objReader["actionbydate"].ToString() != string.Empty ?
                               Convert.ToDateTime(objReader["actionbydate"].ToString()) : (DateTime?)null;
                            row.Severity = objReader["severity"] != null ? objReader["severity"].ToString() : string.Empty;
                            row.ToCrewId = objReader["notificationtocrewid"] != null ? objReader["notificationtocrewid"].ToString() : string.Empty;
                            row.ToCrewId = objReader["notificationtocrewid"] != null ? objReader["notificationtocrewid"].ToString() : string.Empty;
                            row.FromCrewId = objReader["notificationfromcrewid"] != null ? objReader["notificationfromcrewid"].ToString() : string.Empty;
                            row.Status = objReader["notificationstatus"] != null ? objReader["notificationstatus"].ToString() : string.Empty;
                            row.RequestId = objReader["requestid"] != null ? objReader["requestid"].ToString() : string.Empty;
                            row.RequestGuid = objReader["requestguid"] != null ? objReader["requestguid"].ToString() : string.Empty;

                            response.Notification = row;
                        }

                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            response.ExistingAcc = HousingData(objReader);
                        }

                        objReader.NextResult();
                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            response.InitiatorAcc = HousingData(objReader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }

            return response;
        }

        private HousingEO HousingData(IDataReader objReader)
        {
            var data = new HousingEO();
            data.BuildingDetails = new BuildingEO();
            data.Guests = new HousingGuestEO();

            data.BuildingDetails.BuildingName = objReader["bldgname"] != null ? objReader["bldgname"].ToString() : string.Empty;
            data.BuildingDetails.BuildingDetailSid = objReader["buildingdetailsid"] != null ? objReader["buildingdetailsid"].ToString() : string.Empty;
            data.BuildingDetails.BuildingNumber = objReader["bldgnum"] != null ? objReader["bldgnum"].ToString() : string.Empty;
            data.BuildingDetails.StreetNo = objReader["streetno"] != null ? objReader["streetno"].ToString() : string.Empty;
            data.BuildingDetails.Area = objReader["bldgarea"] != null ? objReader["bldgarea"].ToString() : string.Empty;
            data.BuildingDetails.PostBoxNo = objReader["pincode"] != null ? objReader["pincode"].ToString() : string.Empty;
            data.BuildingDetails.FlatNumber = objReader["flatnumber"] != null ? objReader["flatnumber"].ToString() : string.Empty;
            data.BuildingDetails.FlatId = objReader["flatid"] != null ? objReader["flatid"].ToString() : string.Empty;
            data.BuildingDetails.BedroomNo = objReader["bedroom_no"] != null ? objReader["bedroom_no"].ToString() : string.Empty;
            data.BuildingDetails.HouseIncharge = objReader["HOINCHARGE"] != null ? objReader["HOINCHARGE"].ToString() : string.Empty;
            data.Guests.CheckinDate = objReader["startdate"] != null && objReader["startdate"].ToString() != string.Empty ? Convert.ToDateTime(objReader["startdate"]) : (DateTime?)(null);
            data.Guests.CheckoutDate = objReader["enddate"] != null && objReader["enddate"].ToString() != string.Empty ? Convert.ToDateTime(objReader["enddate"]) : (DateTime?)(null);

            return data;
        }

    }
}
