using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Enterprise;
using QR.IPrism.Utility;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using System.Data;
using System.Data.OracleClient;
using System.Data.Common;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace QR.IPrism.BusinessObjects.Implementation
{
    public class EVRDao : IEVRDao
    {
        #region PRIVATE METHODS
        private VRActionResolutionEO PopulateActionResolution(IDataRecord drVrDept)
        {
            VRActionResolutionEO vrActionResolution = new VRActionResolutionEO();
            vrActionResolution.VrAtDeptID = drVrDept["vratdeptid"] != DBNull.Value ?
               drVrDept["vratdeptid"].ToString() : string.Empty;
            vrActionResolution.VRId = drVrDept["vrid"] != DBNull.Value ?
                    drVrDept["vrid"].ToString() : string.Empty;
            vrActionResolution.VRNo = int.Parse(drVrDept["vrno"].ToString());

            vrActionResolution.DeptID = drVrDept["deptid"] != DBNull.Value ?
               drVrDept["deptid"].ToString() : string.Empty;
            vrActionResolution.DeptCode = drVrDept["deptcode"] != DBNull.Value ?
               drVrDept["deptcode"].ToString() : string.Empty;
            vrActionResolution.DepartmentName = drVrDept["deptname"] != DBNull.Value ?
              drVrDept["deptname"].ToString() : string.Empty;

            vrActionResolution.Comment = drVrDept["comments"] != DBNull.Value ?
               drVrDept["comments"].ToString() : string.Empty;
            vrActionResolution.StaffName = drVrDept["StaffName"] != DBNull.Value ?
               drVrDept["StaffName"].ToString() : string.Empty;

            DateTime CommentDate;
            DateTime.TryParse(drVrDept["CommentDate"].ToString(), out CommentDate);
            vrActionResolution.CommentDate = CommentDate;
            return vrActionResolution;
        }
        #endregion

        #region PUBLIC METHODS

        public async Task<List<EVRResultEO>> GetEVRSearchResultAsyc(EVRRequestFilterEO inputs)
        {
            List<EVRResultEO> response = new List<EVRResultEO>();
            EVRResultEO evrRes = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {

                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("pi_vrno", String.IsNullOrWhiteSpace(inputs.evrId) ? "" : (inputs.evrId), ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_flight_number", inputs.FlightNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_from_date", Common.ToOracleDate(inputs.EvrSearchFromDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("pi_to_date", Common.ToOracleDate(inputs.EvrSearchToDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("pi_sector_from", inputs.SectorFrom, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_sector_to", inputs.SectorTo, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_vr_status", inputs.Status, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_staffno", inputs.StaffId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_vrdetail", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_evr.get_vr_proc", parameterList))
                {
                    while (objReader.Read())
                    {
                        evrRes = new EVRResultEO();

                        evrRes.VRId = objReader["vrid"] != null ? objReader["vrid"].ToString() : string.Empty;
                        evrRes.VRNo = objReader["vrno"] != null && (objReader["vrno"].ToString() != string.Empty) ? Convert.ToInt32(objReader["vrno"].ToString()) : 0;
                        evrRes.FlightNumber = objReader["flightnumber"] != null ? objReader["flightnumber"].ToString() : string.Empty;
                        evrRes.Sector = objReader["sector"] != null ? objReader["sector"].ToString() : string.Empty;
                        evrRes.ATD_UTC = objReader["actdeptdate"] != null && objReader["actdeptdate"].ToString() != string.Empty ? Convert.ToDateTime(objReader["actdeptdate"]) : default(DateTime);
                        evrRes.Status = objReader["vrmasterstatusname"] != null ? objReader["vrmasterstatusname"].ToString() : string.Empty;
                        evrRes.Department = objReader["ownerdept"] != null ? objReader["ownerdept"].ToString() : string.Empty;
                        evrRes.ReportAbout = objReader["reportaboutname"] != null ? objReader["reportaboutname"].ToString() : string.Empty;
                        evrRes.FlightDetailId = objReader["flightdetsid"] != null ? objReader["flightdetsid"].ToString() : string.Empty;

                        response.Add(evrRes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return response;
        }

        private List<ODPCommandParameter> getInsertParams(EVRReportMainEO inputs, string staffId, string staffNumber, string staffUserId)
        {
            List<ODPCommandParameter> parameterList = new List<ODPCommandParameter>();
            parameterList.Add(new ODPCommandParameter("pi_flight_dets_id", inputs.vrInsertUpdate.FlightDetId, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_report_about_id", inputs.vrInsertUpdate.ReportAbtId, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_category_id", inputs.vrInsertUpdate.CategoryId, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_sub_category_id", inputs.vrInsertUpdate.SubCategoryId, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_critical", inputs.vrInsertUpdate.IsCritical, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_comments1", inputs.vrInsertUpdate.vrFactComment ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_comments2", inputs.vrInsertUpdate.vrActionComment ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_comments3", inputs.vrInsertUpdate.vrResultComment ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_info_vr", inputs.vrInsertUpdate.IsInfoVr, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_vr_restricted", inputs.vrInsertUpdate.IsVrRestricted, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_notification_not_req", inputs.vrInsertUpdate.IsNotIfNotReq, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_cabin_class_fc", inputs.vrInsertUpdate.IsCabinFirstClass, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_cabin_class_jc", inputs.vrInsertUpdate.IsCabinBusinessClass, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_cabin_class_yc", inputs.vrInsertUpdate.IsCabinEconomyClass, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_cabin_class_na", inputs.vrInsertUpdate.IsCabinClassNA, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_rule_set_changed", inputs.vrInsertUpdate.IsRuleSetChanged, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_admin", inputs.vrInsertUpdate.IsAdmin, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_user_id", staffUserId, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_staff_number", staffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_status_name", inputs.vrInsertUpdate.Status, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_dept_id", inputs.vrInsertUpdate.NODId, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_wf_id", "", ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_new", inputs.vrInsertUpdate.IsNew, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_csr", inputs.vrInsertUpdate.IsCSR, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_ohs", inputs.vrInsertUpdate.IsOHS, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_pm", inputs.vrInsertUpdate.IsPO, ParameterDirection.Input, OracleDbType.Varchar2));

            parameterList.Add(new ODPCommandParameter("po_vr_id", ParameterDirection.Output, OracleDbType.Varchar2, 50));
            parameterList.Add(new ODPCommandParameter("po_vr_no", ParameterDirection.Output, OracleDbType.Decimal, 10));
            parameterList.Add(new ODPCommandParameter("po_vr_instanceid", ParameterDirection.Output, OracleDbType.Varchar2, 50));

            return parameterList;
        }

        private List<ODPCommandParameter> getUpdateParams(EVRReportMainEO inputs, string staffId, string staffNumber, string staffUserId)
        {
            List<ODPCommandParameter> parameterList = new List<ODPCommandParameter>();

            parameterList.Add(new ODPCommandParameter("pi_vr_id", inputs.vrInsertUpdate.VrId, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_flight_dets_id", inputs.vrInsertUpdate.FlightDetId, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_report_about_id", inputs.vrInsertUpdate.ReportAbtId, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_category_id", inputs.vrInsertUpdate.CategoryId, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_sub_category_id", inputs.vrInsertUpdate.SubCategoryId, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_critical", inputs.vrInsertUpdate.IsCritical, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_comments1", inputs.vrInsertUpdate.vrFactComment ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_comments2", inputs.vrInsertUpdate.vrActionComment ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_comments3", inputs.vrInsertUpdate.vrResultComment ?? String.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_info_vr", inputs.vrInsertUpdate.IsInfoVr, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_vr_restricted", inputs.vrInsertUpdate.IsVrRestricted, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_notification_not_req", inputs.vrInsertUpdate.IsNotIfNotReq, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_cabin_class_fc", inputs.vrInsertUpdate.IsCabinFirstClass, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_cabin_class_jc", inputs.vrInsertUpdate.IsCabinBusinessClass, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_cabin_class_yc", inputs.vrInsertUpdate.IsCabinEconomyClass, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_cabin_class_na", inputs.vrInsertUpdate.IsCabinClassNA, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_rule_set_changed", inputs.vrInsertUpdate.IsRuleSetChanged, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_admin", inputs.vrInsertUpdate.IsAdmin, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_user_id", staffUserId, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_new", inputs.vrInsertUpdate.IsNew, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_csr", inputs.vrInsertUpdate.IsCSR, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_ohs", inputs.vrInsertUpdate.IsOHS, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_is_pm", inputs.vrInsertUpdate.IsPO, ParameterDirection.Input, OracleDbType.Varchar2));

            parameterList.Add(new ODPCommandParameter("pi_staff_number", staffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_status_name", inputs.vrInsertUpdate.Status, ParameterDirection.Input, OracleDbType.Varchar2));
            parameterList.Add(new ODPCommandParameter("pi_dept_id", inputs.vrInsertUpdate.NODId, ParameterDirection.Input, OracleDbType.Varchar2));

            return parameterList;
        }

        // Please note that the document save is moved to SharedDAO as a generic method.
        public async Task<EVRInsertOutputEO> InsertVoyageReportAsyc(EVRReportMainEO inputs, string staffId, string staffNumber, string staffUserId)
        {
            EVRInsertOutputEO evrInsertResponse = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                DbCommand command;
                parameterList = new List<ODPCommandParameter>();
                evrInsertResponse = new EVRInsertOutputEO();

                if (inputs.vrInsertUpdate.InsertType.Equals("Insert",StringComparison.OrdinalIgnoreCase))
                {
                    parameterList = getInsertParams(inputs, staffId, staffNumber, staffUserId);
                    command = await dbframework.ExecuteSPNonQueryParmAsync("ivrs_enter_vr_pkg.insertvrmaster", parameterList);

                    evrInsertResponse.VrId = command.Parameters["po_vr_id"] != null ? command.Parameters["po_vr_id"].Value.ToString() : string.Empty;
                    evrInsertResponse.VrNo = command.Parameters["po_vr_no"] != null ? command.Parameters["po_vr_no"].Value.ToString() : string.Empty;
                    evrInsertResponse.VrInstanceId = command.Parameters["po_vr_instanceid"] != null ? command.Parameters["po_vr_instanceid"].Value.ToString() : string.Empty;
                }
                else if (inputs.vrInsertUpdate.InsertType.Equals("Update", StringComparison.OrdinalIgnoreCase))
                {
                    dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
                    parameterList = getUpdateParams(inputs, staffId, staffNumber, staffUserId);
                    command = await dbframework.ExecuteSPNonQueryParmAsync("prism_mig_evr.updatevrmaster_mig", parameterList);
                }

                //Insert VR Passenger
                if (inputs.VRPassengers != null)
                {
                    command = null;

                    foreach (EVRPassengerEO vrPassenger in inputs.VRPassengers)
                    {
                        dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
                        if (vrPassenger != null)
                        {
                            parameterList = null;
                            parameterList = new List<ODPCommandParameter>();
                            parameterList.Add(new ODPCommandParameter("pi_ffp_number", vrPassenger.FFNo, ParameterDirection.Input, OracleDbType.Varchar2));
                            parameterList.Add(new ODPCommandParameter("pi_passenger_name", vrPassenger.PsngrName, ParameterDirection.Input, OracleDbType.Varchar2));
                            parameterList.Add(new ODPCommandParameter("pi_seat_number", vrPassenger.SeatNo, ParameterDirection.Input, OracleDbType.Varchar2));

                            if (inputs.vrInsertUpdate.InsertType.ToUpper() == "INSERT")
                            {
                                parameterList.Add(new ODPCommandParameter("pi_vr_id", evrInsertResponse.VrId, ParameterDirection.Input, OracleDbType.Varchar2));
                            }
                            else if (inputs.vrInsertUpdate.InsertType.ToUpper() == "UPDATE")
                            {
                                parameterList.Add(new ODPCommandParameter("pi_vr_id", inputs.vrInsertUpdate.VrId, ParameterDirection.Input, OracleDbType.Varchar2));
                            }

                            parameterList.Add(new ODPCommandParameter("pi_user_id", staffId, ParameterDirection.Input, OracleDbType.Varchar2));
                            parameterList.Add(new ODPCommandParameter("po_flag", ParameterDirection.Output, OracleDbType.Varchar2, 100));

                            command = await dbframework.ExecuteSPNonQueryParmAsync("ivrs_enter_vr_pkg.InsertVRPassenger", parameterList);
                        }
                    }
                }

                // Insert / Update VR Crew
                if (inputs.vrInsertUpdate.VrCrewsId != null && inputs.vrInsertUpdate.VrCrewsId != string.Empty)
                {
                    parameterList = null;
                    parameterList = new List<ODPCommandParameter>();
                    dbframework = new ODPDataAccess(Constants.CONNECTION_STR);
                    if (inputs.vrInsertUpdate.InsertType.ToUpper() == "INSERT")
                    {
                        parameterList.Add(new ODPCommandParameter("pi_vr_id", evrInsertResponse.VrId, ParameterDirection.Input, OracleDbType.Varchar2));
                    }
                    else if (inputs.vrInsertUpdate.InsertType.ToUpper() == "UPDATE")
                    {
                        parameterList.Add(new ODPCommandParameter("pi_vr_id", inputs.vrInsertUpdate.VrId, ParameterDirection.Input, OracleDbType.Varchar2));
                    }

                    parameterList.Add(new ODPCommandParameter("pi_crew_details_id", inputs.vrInsertUpdate.VrCrewsId, ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("pi_user_id", staffUserId, ParameterDirection.Input, OracleDbType.Varchar2));

                    command = await dbframework.ExecuteSPNonQueryParmAsync("ivrs_enter_vr_pkg.InsertVRCrew", parameterList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }
            return evrInsertResponse;
        }

        public async Task<List<EVRDraftOutputEO>> GetDraftVRForUserAsyc(string flightDetsId, string userId)
        {
            List<EVRDraftOutputEO> response = new List<EVRDraftOutputEO>();
            EVRDraftOutputEO evrDraftsRes = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            List<CommandCursorParameter> commandCursorParameter = null;

            try
            {
                commandCursorParameter = new List<CommandCursorParameter>();

                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("pi_flight_dets_id", flightDetsId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_user_id", userId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_ref_cur_draft_vr", ParameterDirection.Output, OracleDbType.RefCursor));

                //using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("ivrs_enter_vr_pkg.getdraftvrforuser", parameterList))
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_evr.getdraftvrforuser_mig", parameterList))
                {
                    while (objReader.Read())
                    {
                        evrDraftsRes = new EVRDraftOutputEO();

                        evrDraftsRes.VRId = objReader["vrid"] != null ? objReader["vrid"].ToString() : string.Empty;
                        evrDraftsRes.FlightDetsId = objReader["flightdetsid"] != null ? objReader["flightdetsid"].ToString() : string.Empty;
                        evrDraftsRes.VRNo = objReader["vrno"] != null ? objReader["vrno"].ToString() : string.Empty;
                        evrDraftsRes.ReportAbtId = objReader["reportaboutid"] != null ? objReader["reportaboutid"].ToString() : string.Empty;
                        evrDraftsRes.ReportAbtName = objReader["reportaboutname"] != null ? objReader["reportaboutname"].ToString() : string.Empty;
                        evrDraftsRes.CategoryId = objReader["categoryid"] != null ? objReader["categoryid"].ToString() : string.Empty;
                        evrDraftsRes.CategoryName = objReader["categoryname"] != null ? objReader["categoryname"].ToString() : string.Empty;
                        evrDraftsRes.SubCategoryId = objReader["subcategoryid"] != null ? objReader["subcategoryid"].ToString() : string.Empty;
                        evrDraftsRes.SubCategoryName = objReader["subcategoryname"] != null ? objReader["subcategoryname"].ToString() : string.Empty;
                        evrDraftsRes.VRMasterStatusId = objReader["vrmasterstatusid"] != null ? objReader["vrmasterstatusid"].ToString() : string.Empty;
                        evrDraftsRes.VRMasterStatusName = objReader["vrmasterstatusname"] != null ? objReader["vrmasterstatusname"].ToString() : string.Empty;
                        evrDraftsRes.OwnerDept = objReader["ownerdept"] != null ? objReader["ownerdept"].ToString() : string.Empty;
                        evrDraftsRes.VRInstaceId = objReader["vrinstanceid"] != null ? objReader["vrinstanceid"].ToString() : string.Empty;

                        response.Add(evrDraftsRes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return response;
        }

        public async Task<List<EVRResultEO>> GetLastTenVRs(string userId)
        {
            List<EVRResultEO> response = new List<EVRResultEO>();
            EVRResultEO evrListRes = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            List<CommandCursorParameter> commandCursorParameter = null;

            try
            {
                commandCursorParameter = new List<CommandCursorParameter>();
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_staffno", userId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("prc_vr_details", ParameterDirection.Output, OracleDbType.RefCursor));


                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_evr.get_last_ten_vr", parameterList))
                {
                    while (objReader.Read())
                    {
                        evrListRes = new EVRResultEO();
                        evrListRes.VRId = objReader["vrid"] != null ? objReader["vrid"].ToString() : string.Empty;
                        evrListRes.VRNo = objReader["vrno"] != null ? Convert.ToInt32(objReader["vrno"].ToString()) : 0;
                        evrListRes.FlightNumber = objReader["flightnumber"] != null ? objReader["flightnumber"].ToString() : string.Empty;
                        evrListRes.FlightDetailId = objReader["flightdetsid"] != null ? objReader["flightdetsid"].ToString() : string.Empty;
                        evrListRes.ATD_UTC = objReader["actdeptdate"] != null && objReader["actdeptdate"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["actdeptdate"].ToString()) : (DateTime?)null;

                        evrListRes.Sector = objReader["sector"] != null ? objReader["sector"].ToString() : string.Empty;
                        evrListRes.Department = objReader["ownerdept"] != null ? objReader["ownerdept"].ToString() : string.Empty;
                        evrListRes.ReportAbout = objReader["reportaboutname"] != null ? objReader["reportaboutname"].ToString() : string.Empty;
                        response.Add(evrListRes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return response;
        }


        public async Task<List<EVRListsEO>> GetVRLastTenFlightAsyc(string userId)
        {
            List<EVRListsEO> response = new List<EVRListsEO>();
            EVRListsEO evrListRes = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            List<CommandCursorParameter> commandCursorParameter = null;

            try
            {
                commandCursorParameter = new List<CommandCursorParameter>();

                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_crewdetails_id", userId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("prc_vr_details", ParameterDirection.Output, OracleDbType.RefCursor));

                //using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("ivrs_enter_vr_pkg.get_vr_last_ten_flight", parameterList, commandCursorParameter))
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_evr.get_vr_last_ten_flight", parameterList))
                {
                    while (objReader.Read())
                    {
                        evrListRes = new EVRListsEO();

                        evrListRes.FLIGHTDETSID = objReader["FLIGHTDETSID"] != null ? objReader["FLIGHTDETSID"].ToString() : string.Empty;
                        evrListRes.FLIGHTNUMBER = objReader["FLIGHTNUMBER"] != null ? objReader["FLIGHTNUMBER"].ToString() : string.Empty;
                        evrListRes.SECTOR = objReader["SECTOR"] != null ? objReader["SECTOR"].ToString() : string.Empty;

                        evrListRes.ATD_UTC = objReader["ACTDEPTDATE"] != null && objReader["ACTDEPTDATE"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["ACTDEPTDATE"].ToString()) : (DateTime?)null;

                        evrListRes.ATA_UTC = objReader["ACTARRDATE"] != null && objReader["ACTARRDATE"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["ACTARRDATE"].ToString()) : (DateTime?)null;

                        evrListRes.AircraftRegNo = objReader["AIRCRAFTREGNO"] != null ? objReader["AIRCRAFTREGNO"].ToString() : string.Empty;
                        evrListRes.AircraftType = objReader["AIRCRAFTTYPE"] != null ? objReader["AIRCRAFTTYPE"].ToString() : string.Empty;
                        evrListRes.VRCount = objReader["VRCOUNT"] != null && (objReader["VRCOUNT"].ToString() != string.Empty) ? Convert.ToInt32(objReader["VRCOUNT"].ToString()) : 0;

                        response.Add(evrListRes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return response;
        }

        public async Task<List<EVRPendingEO>> GetPendingVRForUserAsyc(string userId)
        {
            List<EVRPendingEO> response = new List<EVRPendingEO>();
            EVRPendingEO evrListRes = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            List<CommandCursorParameter> commandCursorParameter = null;

            try
            {
                commandCursorParameter = new List<CommandCursorParameter>();

                parameterList = new List<ODPCommandParameter>();

                var fromDate = string.Empty;

                parameterList.Add(new ODPCommandParameter("pi_flight_number", "", ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_atd_date", OracleDate.Null, ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("pi_sector_from", "", ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_sector_to", "", ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_staffno", userId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_enterevr_flight", ParameterDirection.Output, OracleDbType.RefCursor));


                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_evr.get_enterevr_flight_proc", parameterList))
                {
                    while (objReader.Read())
                    {
                        evrListRes = new EVRPendingEO();

                        evrListRes.FLIGHTDETSID = objReader["FLIGHTDETSID"] != null ? objReader["FLIGHTDETSID"].ToString() : string.Empty;
                        evrListRes.FLIGHTNUMBER = objReader["FLIGHTNUMBER"] != null ? objReader["FLIGHTNUMBER"].ToString() : string.Empty;
                        evrListRes.ACTDEPTDATE = objReader["ACTDEPTDATE"] != null && objReader["ACTDEPTDATE"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["ACTDEPTDATE"].ToString()) : (DateTime?)null;
                        evrListRes.ISFROMAIMS = objReader["ISFROMAIMS"] != null ? objReader["ISFROMAIMS"].ToString() : string.Empty;
                        evrListRes.ISMANUALLYENTERED = objReader["ISMANUALLYENTERED"] != null ? objReader["ISMANUALLYENTERED"].ToString() : string.Empty;
                        evrListRes.SECTOR = objReader["SECTOR"] != null ? objReader["SECTOR"].ToString() : string.Empty;
                        evrListRes.SECTORFROM = objReader["SECTORFROM"] != null ? objReader["SECTORFROM"].ToString() : string.Empty;
                        evrListRes.SECTORTo = objReader["SECTORTO"] != null ? objReader["SECTORTO"].ToString() : string.Empty;
                        evrListRes.MODIFIEDDATE = objReader["MODIFIEDDATE"] != null && objReader["MODIFIEDDATE"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["MODIFIEDDATE"].ToString()) : (DateTime?)null;
                        evrListRes.ACTARRDATE = objReader["ACTARRDATE"] != null && objReader["ACTARRDATE"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["ACTARRDATE"].ToString()) : (DateTime?)null;
                        evrListRes.AIRCRAFTREGNO = objReader["AIRCRAFTREGNO"] != null ? objReader["AIRCRAFTREGNO"].ToString() : string.Empty;
                        evrListRes.AIRCRAFTTYPEID = objReader["AIRCRAFTTYPEID"] != null ? objReader["AIRCRAFTTYPEID"].ToString() : string.Empty;
                        evrListRes.SCHEARRDATE = objReader["SCHEARRDATE"] != null && objReader["SCHEARRDATE"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["SCHEARRDATE"].ToString()) : (DateTime?)null;
                        evrListRes.SCHEDEPTDATE = objReader["SCHEDEPTDATE"] != null && objReader["SCHEDEPTDATE"].ToString() != String.Empty ?
                            Convert.ToDateTime(objReader["SCHEDEPTDATE"].ToString()) : (DateTime?)null;
                        evrListRes.ISFERRY = objReader["ISFERRY"] != null ? objReader["ISFERRY"].ToString() : string.Empty;
                        evrListRes.AIRCRAFTTYPE = objReader["AIRCRAFTTYPE"] != null ? objReader["AIRCRAFTTYPE"].ToString() : string.Empty;
                        evrListRes.SUBMITTEDVRS = objReader["SUBMITTEDVRS"] != null && (objReader["SUBMITTEDVRS"].ToString() != string.Empty) ? Convert.ToInt32(objReader["SUBMITTEDVRS"].ToString()) : 0;
                        evrListRes.DRAFTVRS = objReader["DRAFTVRS"] != null && (objReader["DRAFTVRS"].ToString() != string.Empty) ? Convert.ToInt32(objReader["DRAFTVRS"].ToString()) : 0;
                        evrListRes.DRAFTVRSBYUSER = objReader["DRAFTVRSBYUSER"] != null && (objReader["DRAFTVRSBYUSER"].ToString() != string.Empty) ? Convert.ToInt32(objReader["DRAFTVRSBYUSER"].ToString()) : 0;
                        evrListRes.NOVRS = objReader["NOVRS"] != null ? objReader["NOVRS"].ToString() : string.Empty;
                        evrListRes.NOVRSBYUSER = objReader["NOVRSBYUSER"] != null ? objReader["NOVRSBYUSER"].ToString() : string.Empty;
                        evrListRes.VRNOTSUBMITTEDS = objReader["VRNOTSUBMITTEDS"] != null ? objReader["VRNOTSUBMITTEDS"].ToString() : string.Empty;
                        evrListRes.SUBMITTEDVRSBYUSER = objReader["SUBMITTEDVRSBYUSER"] != null ? objReader["SUBMITTEDVRSBYUSER"].ToString() : string.Empty;
                        evrListRes.DELAYTAGS = objReader["DELAY_TAGS"] != null ? objReader["DELAY_TAGS"].ToString() : string.Empty;
                        evrListRes.POSCREWCOUNT = objReader["POS_CREWCOUNT"] != null ? objReader["POS_CREWCOUNT"].ToString() : string.Empty;

                        response.Add(evrListRes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return response;
        }

        public async void UpdateNoVRAsyc(string flightDetId, string crewDetailId, string userId)
        {
            //string result = string.Empty;
            ODPDataAccess sqldbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    List<ODPCommandParameter> perametersList = new List<ODPCommandParameter>();

                    perametersList.Add(new ODPCommandParameter("pi_flightdetailid", flightDetId, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_createdby", userId, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_isadmin", "N", ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_crewdetailsid", crewDetailId, ParameterDirection.Input, OracleDbType.Varchar2));

                    DbCommand command = await sqldbframework.ExecuteSPNonQueryParmAsync("ivrs_enter_vr_pkg.UpdateNoVR", perametersList);
                    //result = "Success UpdateNoVR";
                }
                catch (Exception ex)
                {
                    //result = "Failed UpdateNoVR";
                    throw ex;
                }
                finally
                {
                    sqldbframework.CloseConnection();
                }

                //return result;
            }
        }

        public async Task<List<EVRDraftOutputEO>> GetSubmittedEVRsAsyc(string flightDetId, string crewDetailId, string userId)
        {
            List<EVRDraftOutputEO> response = new List<EVRDraftOutputEO>();
            EVRDraftOutputEO evrDraftsRes = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            List<CommandCursorParameter> commandCursorParameter = null;

            try
            {
                commandCursorParameter = new List<CommandCursorParameter>();

                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("pi_flight_dets_id", flightDetId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_user_id", userId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_isadmin", "N", ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("po_ref_cur_vr", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("ivrs_enter_vr_pkg.getvrsforflight", parameterList))
                {
                    while (objReader.Read())
                    {
                        evrDraftsRes = new EVRDraftOutputEO();

                        evrDraftsRes.VRId = objReader["vrid"] != null ? objReader["vrid"].ToString() : string.Empty;
                        evrDraftsRes.FlightDetsId = objReader["flightdetsid"] != null ? objReader["flightdetsid"].ToString() : string.Empty;
                        evrDraftsRes.VRNo = objReader["vrno"] != null ? objReader["vrno"].ToString() : string.Empty;
                        evrDraftsRes.ReportAbtId = objReader["reportaboutid"] != null ? objReader["reportaboutid"].ToString() : string.Empty;
                        evrDraftsRes.ReportAbtName = objReader["reportaboutname"] != null ? objReader["reportaboutname"].ToString() : string.Empty;
                        evrDraftsRes.CategoryId = objReader["categoryid"] != null ? objReader["categoryid"].ToString() : string.Empty;
                        evrDraftsRes.CategoryName = objReader["categoryname"] != null ? objReader["categoryname"].ToString() : string.Empty;
                        evrDraftsRes.SubCategoryId = objReader["subcategoryid"] != null ? objReader["subcategoryid"].ToString() : string.Empty;
                        evrDraftsRes.SubCategoryName = objReader["subcategoryname"] != null ? objReader["subcategoryname"].ToString() : string.Empty;
                        evrDraftsRes.VRMasterStatusId = objReader["vrmasterstatusid"] != null ? objReader["vrmasterstatusid"].ToString() : string.Empty;
                        evrDraftsRes.VRMasterStatusName = objReader["vrmasterstatusname"] != null ? objReader["vrmasterstatusname"].ToString() : string.Empty;
                        evrDraftsRes.OwnerDept = objReader["ownerdept"] != null ? objReader["ownerdept"].ToString() : string.Empty;

                        response.Add(evrDraftsRes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return response;
        }

        public async Task<EVRCrewViewEO> GetVRDetailsCrewAsyc(VRIdEO vrId)
        {
            EVRCrewViewEO viewVRCrew = new EVRCrewViewEO();
            IList<VRCrewDetailEO> vrCrewDetailList = new List<VRCrewDetailEO>();
            IList<VRCrewDetailEO> vrAllCrewDetailList = new List<VRCrewDetailEO>();
            IList<VRPassengerDetailEO> vrPassengerDetailList = new List<VRPassengerDetailEO>();
            IList<VRDocumentDetailEO> vrDocumentDetailList = new List<VRDocumentDetailEO>();
            IList<VRDeptDetailEO> vrDeptDetailList = new List<VRDeptDetailEO>();
            IList<VRActionResolutionEO> vrActionResolutionList = new List<VRActionResolutionEO>();

            string vrComnts = string.Empty;
            string vrComnts1 = string.Empty;
            string vrComnts2 = string.Empty;
            string vrComnts3 = string.Empty;

            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            List<CommandCursorParameter> commandCursorParameter = null;

            try
            {
                commandCursorParameter = new List<CommandCursorParameter>();

                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_vr_id", vrId.VrId, ParameterDirection.Input, OracleDbType.Varchar2));

                //commandCursorParameter.Add(new CommandCursorParameter("prc_vr_detail", ParameterDirection.Output, OracleType.Cursor));
                //commandCursorParameter.Add(new CommandCursorParameter("prc_crew", ParameterDirection.Output, OracleType.Cursor));
                //commandCursorParameter.Add(new CommandCursorParameter("prc_flight_crew", ParameterDirection.Output, OracleType.Cursor));
                //commandCursorParameter.Add(new CommandCursorParameter("prc_passenger", ParameterDirection.Output, OracleType.Cursor));
                //commandCursorParameter.Add(new CommandCursorParameter("prc_document", ParameterDirection.Output, OracleType.Cursor));
                //commandCursorParameter.Add(new CommandCursorParameter("prc_dept_detail", ParameterDirection.Output, OracleType.Cursor));
                //commandCursorParameter.Add(new CommandCursorParameter("prc_vrActionResolution", ParameterDirection.Output, OracleType.Cursor));

                parameterList.Add(new ODPCommandParameter("po_vr_detail", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("po_crew", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("po_flight_crew", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("po_passenger", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("po_document", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("po_dept_detail", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("po_vractionresolution", ParameterDirection.Output, OracleDbType.RefCursor));

                //using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("ivrs_vr_details_pkg.GetVRDetailsCrew", parameterList, commandCursorParameter))
                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_mig_evr.get_evr_details_proc", parameterList))
                {
                    while (objReader.Read())
                    {
                        //Populate Flight Details.
                        viewVRCrew.FlightNumber = objReader["flightnumber"] != null ? objReader["flightnumber"].ToString() : string.Empty;
                        viewVRCrew.SectorFrom = objReader["sectorfrom"] != null ? objReader["sectorfrom"].ToString() : string.Empty;
                        viewVRCrew.SectorTo = objReader["sectorto"] != null ? objReader["sectorto"].ToString() : string.Empty;
                        DateTime actDeptTime;
                        DateTime.TryParse(objReader["actdeptdate"].ToString(), out actDeptTime);
                        viewVRCrew.ActDeptTime = actDeptTime;
                        DateTime actArrTime;
                        DateTime.TryParse(objReader["actarrdate"].ToString(), out actArrTime);
                        viewVRCrew.ActArrTime = actArrTime;
                        //fixed for 392
                        viewVRCrew.IsFromAIMS = objReader["isfromaims"] != null ? objReader["isfromaims"].ToString() : string.Empty;
                        viewVRCrew.IsManuallyEntered = objReader["ismanuallyentered"] != null ? objReader["ismanuallyentered"].ToString() : string.Empty;

                        viewVRCrew.AirCraftRegNo = objReader["aircraftregno"] != null ? objReader["aircraftregno"].ToString() : string.Empty;
                        viewVRCrew.AirCraftType = objReader["aircrafttype"] != null ? objReader["aircrafttype"].ToString() : string.Empty;

                        viewVRCrew.IsFerry = objReader["isferry"].ToString() == string.Empty ? false :
                            objReader["isferry"].ToString() == "N" ? false : true;

                        //Populate VR Details.
                        viewVRCrew.VrId = objReader["vrid"] != DBNull.Value ? objReader["vrid"].ToString() : string.Empty;

                        viewVRCrew.VrNo = Convert.ToInt32(objReader["vrno"].ToString());

                        viewVRCrew.PassengerLoadFC = objReader["passengerloadfc"] != null ? Convert.ToInt32(objReader["passengerloadfc"]) : 0;
                        viewVRCrew.PassengerLoadJC = objReader["passengerloadjc"] != null ? Convert.ToInt32(objReader["passengerloadjc"]) : 0;
                        viewVRCrew.PassengerLoadYC = objReader["passengerloadyc"] != null ? Convert.ToInt32(objReader["passengerloadyc"]) : 0;
                        viewVRCrew.InfantLoadFC = objReader["infantloadfc"] != null ? Convert.ToInt32(objReader["infantloadfc"]) : 0;
                        viewVRCrew.InfantLoadJC = objReader["infantloadjc"] != null ? Convert.ToInt32(objReader["infantloadjc"]) : 0;
                        viewVRCrew.InfantLoadYC = objReader["infantloadyc"] != null ? Convert.ToInt32(objReader["infantloadyc"]) : 0;
                        viewVRCrew.SeatCapacityFC = objReader["seatcapacityfc"] != null ? Convert.ToInt32(objReader["seatcapacityfc"]) : 0;
                        viewVRCrew.SeatCapacityJC = objReader["seatcapacityjc"] != null ? Convert.ToInt32(objReader["seatcapacityjc"]) : 0;
                        viewVRCrew.SeatCapacityYC = objReader["seatcapacityyc"] != null ? Convert.ToInt32(objReader["seatcapacityyc"]) : 0;
                        viewVRCrew.FlightRoute = objReader["flightroute"] != DBNull.Value ? objReader["flightroute"].ToString() : string.Empty;
                        viewVRCrew.IsGroominCheck = objReader["isgroomingcheck"] != DBNull.Value ? objReader["isgroomingcheck"].ToString() : string.Empty;
                        viewVRCrew.IsCSDCSBriefed = objReader["iscsdcsbriefed"] != DBNull.Value ? objReader["iscsdcsbriefed"].ToString() : string.Empty;
                        viewVRCrew.GroomingCheckComment = objReader["groomingcheckcomment"] != DBNull.Value ? objReader["groomingcheckcomment"].ToString() : string.Empty;
                        viewVRCrew.CSDCSBriefedComment = objReader["csdcsbriefedcomment"] != DBNull.Value ? objReader["csdcsbriefedcomment"].ToString() : string.Empty;

                        viewVRCrew.ReportAboutName = objReader["reportaboutname"] != DBNull.Value ? objReader["reportaboutname"].ToString() : string.Empty;
                        viewVRCrew.CategoryName = objReader["categoryname"] != DBNull.Value ? objReader["categoryname"].ToString() : string.Empty;
                        viewVRCrew.SubCategoryName = objReader["subcategoryname"] != DBNull.Value ? objReader["subcategoryname"].ToString() : string.Empty;

                        viewVRCrew.IsCritical = objReader["iscritical"] != DBNull.Value ? objReader["iscritical"].ToString() : string.Empty;
                        viewVRCrew.IsCabInClassFC = objReader["iscabinclassfc"] != DBNull.Value ? objReader["iscabinclassfc"].ToString() : string.Empty;
                        viewVRCrew.IsCabInClassJC = objReader["iscabinclassjc"] != DBNull.Value ? objReader["iscabinclassjc"].ToString() : string.Empty;
                        viewVRCrew.IsCabInClassYC = objReader["iscabinclassyc"] != DBNull.Value ? objReader["iscabinclassyc"].ToString() : string.Empty;

                        viewVRCrew.IsCSR = objReader["is_csr"] != DBNull.Value ? objReader["is_csr"].ToString() : string.Empty;
                        viewVRCrew.IsOHS = objReader["is_ohs"] != DBNull.Value ? objReader["is_ohs"].ToString() : string.Empty;
                        viewVRCrew.IsPO = objReader["is_po"] != DBNull.Value ? objReader["is_po"].ToString() : string.Empty;

                        vrComnts1 = objReader["comments1"] != DBNull.Value ?
                                                    objReader["comments1"].ToString() : string.Empty;
                        vrComnts2 = objReader["comments2"] != DBNull.Value ?
                                                    objReader["comments2"].ToString() : string.Empty;
                        vrComnts3 = objReader["comments3"] != DBNull.Value ?
                                                    objReader["comments3"].ToString() : string.Empty;
                        viewVRCrew.IsNew = objReader["isnew"] != DBNull.Value ?
                           objReader["isnew"].ToString() : string.Empty;

                        if (viewVRCrew.IsNew.Trim() == "Y")
                        {
                            viewVRCrew.Facts = vrComnts1;
                            viewVRCrew.Action = vrComnts2;
                            viewVRCrew.Result = vrComnts3;
                        }
                        else
                        {
                            vrComnts = vrComnts1 + vrComnts2 + vrComnts3;
                            viewVRCrew.CrewComments = vrComnts;
                        }


                        viewVRCrew.VrStatusName = objReader["vrmasterstatusname"] != DBNull.Value ?
                            objReader["vrmasterstatusname"].ToString() : string.Empty;

                        viewVRCrew.StaffNumber = objReader["staffno"] != DBNull.Value ?
                            objReader["staffno"].ToString() : string.Empty;
                        viewVRCrew.FirstName = objReader["firstname"] != DBNull.Value ?
                            objReader["firstname"].ToString() : string.Empty;
                        viewVRCrew.MiddleName = objReader["middlename"] != DBNull.Value ?
                            objReader["middlename"].ToString() : string.Empty;
                        viewVRCrew.LastName = objReader["lastname"] != DBNull.Value ?
                            objReader["lastname"].ToString() : string.Empty;
                        viewVRCrew.Grade = objReader["grade"] != DBNull.Value ?
                            objReader["grade"].ToString() : string.Empty;

                        // string crewCount = dbCommand.Parameters["crew_count"].Value.ToString();
                        // viewVRCrew.CrewCount = crewCount;
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        //Populate VR Crew Details
                        VRCrewDetailEO vrCrewDetail = new VRCrewDetailEO();
                        vrCrewDetail.StaffNumber = objReader["staffno"] != DBNull.Value ?
                            objReader["staffno"].ToString() : string.Empty;
                        vrCrewDetail.StaffName = objReader["staffname"] != DBNull.Value ?
                            objReader["staffname"].ToString() : string.Empty;
                        vrCrewDetail.Grade = objReader["grade"] != DBNull.Value ?
                            objReader["grade"].ToString() : string.Empty;
                        vrCrewDetailList.Add(vrCrewDetail);
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        //Populate VR Crew Details
                        VRCrewDetailEO vrCrewDetail = new VRCrewDetailEO();
                        vrCrewDetail.StaffNumber = objReader["staffno"] != DBNull.Value ?
                            objReader["staffno"].ToString() : string.Empty;
                        vrCrewDetail.StaffName = objReader["staffname"] != DBNull.Value ?
                            objReader["staffname"].ToString() : string.Empty;
                        vrCrewDetail.Grade = objReader["Grade"] != DBNull.Value ?
                            objReader["Grade"].ToString() : string.Empty;
                        vrCrewDetail.IsActingCSD = objReader["actingcsd"] != DBNull.Value ?
                            objReader["actingcsd"].ToString() : string.Empty;
                        vrCrewDetail.CrewPosition = objReader["cabincrewposition"] != DBNull.Value ?
                            objReader["cabincrewposition"].ToString() : string.Empty;
                        vrCrewDetail.AnnouncementLanguage = objReader["announcelang"] != DBNull.Value ?
                            objReader["announcelang"].ToString() : string.Empty;
                        vrAllCrewDetailList.Add(vrCrewDetail);
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        //Populate VR Passenger Details.
                        VRPassengerDetailEO vrPassengerDetail = new VRPassengerDetailEO();
                        vrPassengerDetail.FFPNumber = objReader["ffpnumber"] != DBNull.Value ?
                            objReader["ffpnumber"].ToString() : string.Empty;
                        vrPassengerDetail.PassengerName = objReader["passengername"] != DBNull.Value ?
                            objReader["passengername"].ToString() : string.Empty;
                        vrPassengerDetail.SeatNumber = objReader["seatnumber"] != DBNull.Value ?
                            objReader["seatnumber"].ToString() : string.Empty;
                        vrPassengerDetailList.Add(vrPassengerDetail);
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        //Populate VR Document Details
                        VRDocumentDetailEO vrDocumentDetail = new VRDocumentDetailEO();
                        vrDocumentDetail.VrDocName = objReader["vrdocname"] != DBNull.Value ?
                            objReader["vrdocname"].ToString() : string.Empty;
                        vrDocumentDetail.VrDocPath = objReader["vrdocpath"] != DBNull.Value ?
                            objReader["vrdocpath"].ToString() : string.Empty;
                        vrDocumentDetail.VrDocContent = objReader["attachmentcontent"] as byte[];

                        // Converting so that it can be read at client side
                        //vrDocumentDetail.VrDocPath = System.Text.Encoding.ASCII.GetString(vrDocumentDetail.VrDocContent);
                        //result.Content = new ByteArrayContent(bytes);


                        vrDocumentDetail.VrDocId = objReader["attachmentid"].ToString();

                        vrDocumentDetailList.Add(vrDocumentDetail);
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        //Populate VR Assigned Department Details
                        VRDeptDetailEO vrDeptDetail = new VRDeptDetailEO();
                        vrDeptDetail.DeptId = objReader["deptid"] != DBNull.Value ?
                            objReader["deptid"].ToString() : string.Empty;
                        vrDeptDetail.DeptName = objReader["deptname"] != DBNull.Value ?
                            objReader["deptname"].ToString() : string.Empty;
                        vrDeptDetail.SectionName = objReader["sectionname"] != DBNull.Value ?
                            objReader["sectionname"].ToString() : string.Empty;
                        vrDeptDetail.DeptType = objReader["depttype"] != DBNull.Value ?
                            objReader["depttype"].ToString() : string.Empty;
                        vrDeptDetailList.Add(vrDeptDetail);
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        //Populate VR Action Resolution Details
                        vrActionResolutionList.Add(PopulateActionResolution(objReader));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            viewVRCrew.VRCrewDetail = vrCrewDetailList;
            viewVRCrew.VRAllCrewDetail = vrAllCrewDetailList;
            viewVRCrew.VRPassengerDetail = vrPassengerDetailList;
            viewVRCrew.VRDocumentDetail = vrDocumentDetailList;
            viewVRCrew.VRDeptDetail = vrDeptDetailList;
            viewVRCrew.vrAtDeptList = vrActionResolutionList;

            return viewVRCrew;
        }

        public async Task<ViewVREnterVREO> GetVRDetailEnterVRAsyc(VRIdEO vrId)
        {
            ViewVREnterVREO viewVREnterVR = new ViewVREnterVREO();
            IList<VRDeptEnterVREO> vrDeptEnterVRList = new List<VRDeptEnterVREO>();
            IList<EVRPassengerEO> vrPassengerEnterVRList = new List<EVRPassengerEO>();
            IList<VRCrewEnterVREO> vrCrewEnterVRList = new List<VRCrewEnterVREO>();
            IList<DocumentEO> vrDocEnterVRList = new List<DocumentEO>();

            string vrComnts = string.Empty;
            string vrComnts1 = string.Empty;
            string vrComnts2 = string.Empty;
            string vrComnts3 = string.Empty;

            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            List<CommandCursorParameter> commandCursorParameter = null;


            try
            {
                commandCursorParameter = new List<CommandCursorParameter>();

                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("pi_vr_id", vrId.VrId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("prc_vr_detail", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("prc_add_dept", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("prc_crew", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("prc_passenger", ParameterDirection.Output, OracleDbType.RefCursor));
                parameterList.Add(new ODPCommandParameter("prc_document", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("ivrs_enter_vr_pkg.GetVRDetails", parameterList))
                {
                    while (objReader.Read())
                    {
                        //Populate VR Details.
                        viewVREnterVR.VrId = objReader["vrid"] != DBNull.Value ?
                            objReader["vrid"].ToString() : string.Empty;
                        viewVREnterVR.VrNo = int.Parse(objReader["vrno"].ToString());
                        viewVREnterVR.VrInstanceId = objReader["vrinstanceid"] != DBNull.Value ?
                            objReader["vrinstanceid"].ToString() : string.Empty;

                        viewVREnterVR.ReportAboutID = objReader["reportaboutid"] != DBNull.Value ?
                            objReader["reportaboutid"].ToString() : string.Empty;
                        viewVREnterVR.CategoryId = objReader["categoryid"] != DBNull.Value ?
                            objReader["categoryid"].ToString() : string.Empty;
                        viewVREnterVR.SubCategoryId = objReader["subcategoryid"] != DBNull.Value ?
                            objReader["subcategoryid"].ToString() : string.Empty;

                        viewVREnterVR.IsCritical = objReader["iscritical"] != DBNull.Value ?
                            objReader["iscritical"].ToString() : string.Empty;
                        viewVREnterVR.IsCabInClassFC = objReader["iscabinclassfc"] != DBNull.Value ?
                            objReader["iscabinclassfc"].ToString() : string.Empty;
                        viewVREnterVR.IsCabInClassJC = objReader["iscabinclassjc"] != DBNull.Value ?
                            objReader["iscabinclassjc"].ToString() : string.Empty;
                        viewVREnterVR.IsCabInClassYC = objReader["iscabinclassyc"] != DBNull.Value ?
                            objReader["iscabinclassyc"].ToString() : string.Empty;
                        viewVREnterVR.IsCabInClassNA = objReader["iscabinclassna"] != DBNull.Value ?
                          objReader["iscabinclassna"].ToString() : string.Empty;

                        viewVREnterVR.IsCSR = objReader["is_csr"] != DBNull.Value ?
                          objReader["is_csr"].ToString() : string.Empty;
                        viewVREnterVR.IsOHS = objReader["is_ohs"] != DBNull.Value ?
                          objReader["is_ohs"].ToString() : string.Empty;
                        viewVREnterVR.IsPO = objReader["is_po"] != DBNull.Value ?
                          objReader["is_po"].ToString() : string.Empty;

                        vrComnts1 = objReader["comments1"] != DBNull.Value ?
                                objReader["comments1"].ToString() : string.Empty;
                        vrComnts2 = objReader["comments2"] != DBNull.Value ?
                                objReader["comments2"].ToString() : string.Empty;
                        vrComnts3 = objReader["comments3"] != DBNull.Value ?
                                                        objReader["comments3"].ToString() : string.Empty;

                        viewVREnterVR.IsNew = objReader["isnew"] != DBNull.Value ?
                           objReader["isnew"].ToString() : string.Empty;

                        // concat values are not required, Each values to be shown separately
                        // if (viewVREnterVR.IsNew.Trim() == "Y")
                        {
                            viewVREnterVR.Facts = vrComnts1;
                            viewVREnterVR.Action = vrComnts2;
                            viewVREnterVR.Result = vrComnts3;
                        }
                        //else
                        {
                            vrComnts = vrComnts1 + vrComnts2 + vrComnts3;
                            viewVREnterVR.Comments = vrComnts;
                        }

                        viewVREnterVR.IsNotIfNotReq = objReader["isnotificationnotreq"] != DBNull.Value ?
                            objReader["isnotificationnotreq"].ToString() : string.Empty;
                        viewVREnterVR.IsInfoVr = objReader["isinfovr"] != DBNull.Value ?
                            objReader["isinfovr"].ToString() : string.Empty;
                        viewVREnterVR.IsVrRestricted = objReader["isvrrestricted"] != DBNull.Value ?
                            objReader["isvrrestricted"].ToString() : string.Empty;

                        viewVREnterVR.StaffNumber = objReader["staffno"] != DBNull.Value ?
                            objReader["staffno"].ToString() : string.Empty;
                        viewVREnterVR.FirstName = objReader["firstname"] != DBNull.Value ?
                            objReader["firstname"].ToString() : string.Empty;
                        viewVREnterVR.MiddleName = objReader["middlename"] != DBNull.Value ?
                            objReader["middlename"].ToString() : string.Empty;
                        viewVREnterVR.LastName = objReader["lastname"] != DBNull.Value ?
                            objReader["lastname"].ToString() : string.Empty;
                        viewVREnterVR.Grade = objReader["grade"] != DBNull.Value ?
                            objReader["grade"].ToString() : string.Empty;
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        VRDeptEnterVREO vrDeptEnterVR = new VRDeptEnterVREO();
                        vrDeptEnterVR.DeptId = objReader["deptid"] != DBNull.Value ?
                            objReader["deptid"].ToString() : string.Empty;
                        vrDeptEnterVR.DeptNameCode = objReader["deptnamecode"] != DBNull.Value ?
                            objReader["deptnamecode"].ToString() : string.Empty;
                        vrDeptEnterVRList.Add(vrDeptEnterVR);
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        //Populate VR Crew Details
                        VRCrewEnterVREO vrCrewEnterVR = new VRCrewEnterVREO();
                        vrCrewEnterVR.CrewDetailsId = objReader["crewdetailsid"] != DBNull.Value ?
                            objReader["crewdetailsid"].ToString() : string.Empty;
                        vrCrewEnterVRList.Add(vrCrewEnterVR);
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        //Populate VR Passenger Details.
                        EVRPassengerEO vrPassengerEnterVR = new EVRPassengerEO();
                        vrPassengerEnterVR.FFNo = objReader["ffpnumber"] != DBNull.Value ?
                            objReader["ffpnumber"].ToString() : string.Empty;
                        vrPassengerEnterVR.PsngrName = objReader["passengername"] != DBNull.Value ?
                            objReader["passengername"].ToString() : string.Empty;
                        vrPassengerEnterVR.SeatNo = objReader["seatnumber"] != DBNull.Value ?
                            objReader["seatnumber"].ToString() : string.Empty;
                        vrPassengerEnterVRList.Add(vrPassengerEnterVR);
                    }

                    objReader.NextResult();
                    while (objReader.Read())
                    {
                        //Populate VR Document Details
                        DocumentEO vrDocEnterVR = new DocumentEO();
                        vrDocEnterVR.DocumentId = objReader["vrdocid"] != DBNull.Value ?
                            objReader["vrdocid"].ToString() : string.Empty;
                        vrDocEnterVR.DocumentName = objReader["vrdocname"] != DBNull.Value ?
                            objReader["vrdocname"].ToString() : string.Empty;
                        //vrDocEnterVR.PAT = objReader["vrdocpath"] != DBNull.Value ?
                        //    objReader["vrdocpath"].ToString() : string.Empty;
                        vrDocEnterVR.DocumentContent = objReader["attachmentcontent"] as byte[];
                        vrDocEnterVRList.Add(vrDocEnterVR);
                    }
                }

                viewVREnterVR.VRDeptEnterVR = vrDeptEnterVRList;
                viewVREnterVR.VRPassengerEnterVR = vrPassengerEnterVRList;
                viewVREnterVR.VRCrewEnterVR = vrCrewEnterVRList;
                viewVREnterVR.VRDocEnterVR = vrDocEnterVRList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return viewVREnterVR;
        }

        public async void DeleteVRAsyc(string VRId)
        {
            ODPDataAccess sqldbframework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    List<ODPCommandParameter> perametersList = new List<ODPCommandParameter>();

                    perametersList.Add(new ODPCommandParameter("pi_vr_id", VRId, ParameterDirection.Input, OracleDbType.Varchar2));

                    DbCommand command = await sqldbframework.ExecuteSPNonQueryParmAsync("ivrs_enter_vr_pkg.deletevr", perametersList);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqldbframework.CloseConnection();
                }
            }
        }

        #endregion
    }
}
