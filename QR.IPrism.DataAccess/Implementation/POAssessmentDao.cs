using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Enterprise;
using QR.IPrism.Utility;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using System.Data;
using Oracle.DataAccess.Client;

namespace QR.IPrism.BusinessObjects.Implementation
{
    public class POAssessmentDao : IPOAssessmentDao
    {
        public async Task<List<AssessmentEO>> GetPOAssmtListAsync(AssessmentSearchRequestFilterEO filter)
        {
            List<AssessmentEO> poAssmtresultList = new List<AssessmentEO>();
            AssessmentEO PoAssmtresult = new AssessmentEO();
            PoAssmtresult.StaffDetails = new List<UserContextEO>();
            PoAssmtresult.FlightDetails = new List<FlightInfoEO>();
           // PoAssmtresult.AssessmentDetails = new List<AssessmentSearchEO>();
            

            ODPDataAccess dbFramework = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> parameterList = null;
                
                try
                {
                    parameterList = new List<ODPCommandParameter>();
                    parameterList.Add(new ODPCommandParameter("", PoAssmtresult.ManagerStaffNo ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("", filter.StaffNumber ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("po_crewsearch", ParameterDirection.Output, OracleDbType.RefCursor));


                    using (IDataReader objReader = await dbFramework.ExecuteSPReaderAsync("pbms_recordassessment_pkg.get_assessmentlist1", parameterList))
                    {
                        while (objReader.Read())
                        {
                            UserContextEO userContextDetails = new UserContextEO();
                            userContextDetails.StaffNumber = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
                            userContextDetails.StaffName = objReader["crewname"] != null ? objReader["crewname"].ToString() : string.Empty;
                           

                            PoAssmtresult.StaffDetails.Add(userContextDetails);
                        }
                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            FlightInfoEO flightDetails = new FlightInfoEO();
                            flightDetails.FlightNumber = objReader["flightno"] != null ? objReader["flightno"].ToString() : string.Empty;
                            flightDetails.SectorFrom = objReader["sectorfrom"] != null ? objReader["sectorfrom"].ToString() : string.Empty;
                            flightDetails.SectorTo = objReader["sectorto"] != null ? objReader["sectorto"].ToString() : string.Empty;
                           

                            PoAssmtresult.FlightDetails.Add(flightDetails);
                        }
                         objReader.NextResult();
                         while (objReader.Read())
                        {

                            PoAssmtresult.AssessmentStatus = objReader["assessmentstatus"] != null ? objReader["assessmentstatus"].ToString() : string.Empty;
                            PoAssmtresult.AssessmentDate = objReader["dateofassessment"] != null ? Convert.ToDateTime(objReader["dateofassessment"]) : default(DateTime);
                            PoAssmtresult.AssesseeGrade = objReader["grade"] != null ? objReader["grade"].ToString() : string.Empty;

                            poAssmtresultList.Add(PoAssmtresult);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbFramework.CloseConnection();
                }

                return poAssmtresultList;
            }

        }

        //public async Task<List<AssessmentEO>> GetCSDListAsync(AssessmentSearchRequestFilterEO filter)
        //{
        //    List<AssessmentEO> poAssmtresultList = new List<AssessmentEO>();
        //    AssessmentEO PoAssmtresult = new AssessmentEO();
        //    PoAssmtresult.StaffDetails = new List<UserContextEO>();


        //    ODPDataAccess dbFramework = new ODPDataAccess(Constants.CONNECTION_STR);
        //    {
        //        List<ODPCommandParameter> parameterList = null;

        //        try
        //        {
        //            parameterList = new List<ODPCommandParameter>();

        //            parameterList.Add(new ODPCommandParameter("", PoAssmtresult.ManagerStaffNo ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
        //            parameterList.Add(new ODPCommandParameter("", filter.StaffNumber ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
        //            parameterList.Add(new ODPCommandParameter("po_csdcslist", ParameterDirection.Output, OracleDbType.RefCursor));


        //            using (IDataReader objReader = await dbFramework.ExecuteSPReaderAsync("pbms_recordassessment_pkg.get_cs_csd_assessmentlist", parameterList))
        //            {
        //                while (objReader.Read())
        //                {
        //                    UserContextEO userContextDetails = new UserContextEO();
        //                    userContextDetails.StaffNumber = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
        //                    userContextDetails.StaffName = objReader["staffname"] != null ? objReader["staffname"].ToString() : string.Empty;


        //                    PoAssmtresult.StaffDetails.Add(userContextDetails);
        //                }
                                              
        //                objReader.NextResult();
        //                while (objReader.Read())
        //                {

        //                    PoAssmtresult.LastAssessmentDate = objReader["lastasmntdate"] != null ? Convert.ToDateTime(objReader["lastasmntdate"]) : default(DateTime);
        //                    PoAssmtresult.ExpAssmtDate = objReader["expectedasmntdate"] != null ? Convert.ToDateTime(objReader["expectedasmntdate"]) : default(DateTime);
        //                    PoAssmtresult.AssessmentsScheduled = objReader["ScheduledDate"] != null ? objReader["ScheduledDate"].ToString() : string.Empty;
        //                    PoAssmtresult.AssessorStaffName = objReader["postaffname"] != null ? objReader["postaffname"].ToString() : string.Empty;
        //                    PoAssmtresult.AssesseeGrade = objReader["grade"] != null ? objReader["grade"].ToString() : string.Empty;
        //                    poAssmtresultList.Add(PoAssmtresult);
        //                }

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            dbFramework.CloseConnection();
        //        }

        //        return poAssmtresultList.Where(u => u.AssesseeGrade == "csd").ToList();
        //    }

        //}

        //public async Task<List<AssessmentEO>> GetCSListAsync(AssessmentSearchRequestFilterEO filter)
        //{
        //    List<AssessmentEO> poAssmtresultList = new List<AssessmentEO>();
        //    AssessmentEO PoAssmtresult = new AssessmentEO();
        //    PoAssmtresult.StaffDetails = new List<UserContextEO>();


        //    ODPDataAccess dbFramework = new ODPDataAccess(Constants.CONNECTION_STR);
        //    {
        //        List<ODPCommandParameter> parameterList = null;

        //        try
        //        {
        //            parameterList = new List<ODPCommandParameter>();

        //            parameterList.Add(new ODPCommandParameter("", PoAssmtresult.ManagerStaffNo ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
        //            parameterList.Add(new ODPCommandParameter("", filter.StaffNumber ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));


        //            using (IDataReader objReader = await dbFramework.ExecuteSPReaderAsync("pbms_recordassessment_pkg.get_cs_csd_assessmentlist", parameterList, "po_csdcslist"))
        //            {
        //                while (objReader.Read())
        //                {
        //                    UserContextEO userContextDetails = new UserContextEO();
        //                    userContextDetails.StaffNumber = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
        //                    userContextDetails.StaffName = objReader["staffname"] != null ? objReader["staffname"].ToString() : string.Empty;


        //                    PoAssmtresult.StaffDetails.Add(userContextDetails);
        //                }

        //                objReader.NextResult();
        //                while (objReader.Read())
        //                {

        //                    PoAssmtresult.LastAssessmentDate = objReader["lastasmntdate"] != null ? Convert.ToDateTime(objReader["lastasmntdate"]) : default(DateTime);
        //                    PoAssmtresult.ExpAssmtDate = objReader["expectedasmntdate"] != null ? Convert.ToDateTime(objReader["expectedasmntdate"]) : default(DateTime);
        //                    PoAssmtresult.AssessmentsScheduled = objReader["ScheduledDate"] != null ? objReader["ScheduledDate"].ToString() : string.Empty;
        //                    PoAssmtresult.AssessorStaffName = objReader["postaffname"] != null ? objReader["postaffname"].ToString() : string.Empty;
        //                    PoAssmtresult.AssesseeGrade = objReader["grade"] != null ? objReader["grade"].ToString() : string.Empty;
        //                    poAssmtresultList.Add(PoAssmtresult);
        //                }

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            dbFramework.CloseConnection();
        //        }

        //        return poAssmtresultList.Where(u => u.AssesseeGrade == "cs").ToList();
        //    }

        //}
    }
}
