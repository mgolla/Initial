/*********************************************************************
 * Name         : CrewProfileDao.cs
 * Description  : Implementation of ICrewProfileDao.
 * Create Date  : 22nd Feb 2016
 * Last Modified: 22nd Feb 2016
 * Copyright By : Qatar Airways
 *********************************************************************/

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
using Oracle.DataAccess.Client;

namespace QR.IPrism.BusinessObjects.Implementation
{
    public class CrewProfileDao : ICrewProfileDao
    {
        /// <summary>
        /// Gets Training History for logged in user.
        /// Shows all the History of the tranings.
        /// </summary>
        /// <param name="string">staffNumber</param>
        /// <returns>List of Training History Details</returns>

        public async Task<List<TrainingHistoryEO>> GetCrewTrainingHistoryAsync(string staffNumber)
        {
            List<TrainingHistoryEO> responseList = new List<TrainingHistoryEO>();
            TrainingHistoryEO response = null;
            List<ODPCommandParameter> parametersList = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList = new List<ODPCommandParameter>();
                    parametersList.Add(new ODPCommandParameter("pi_staffno", staffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("cur_trainingdets", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_crewprofile.get_crew_trainingdet", parametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new TrainingHistoryEO();

                            response.Name = objReader["coursename"] != null && objReader["coursename"].ToString() != string.Empty ? (System.String)(objReader["coursename"]) : default(System.String);
                            response.NoOfDays = objReader["noofdays"] != null && objReader["noofdays"].ToString() != string.Empty ? Convert.ToDecimal(objReader["noofdays"]) : 0;                            
                            response.Date = objReader["coursestartdate"] != null && objReader["coursestartdate"].ToString() != string.Empty ? Common.OracleDateFormateddMMMyyyy(Convert.ToDateTime(objReader["coursestartdate"])) : string.Empty;

                            responseList.Add(response);
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
            
            return responseList;
        }


        public async Task<List<QualnVisaEO>> GetCrewQualnVisaAsync(string staffNumber)
        {
            List<QualnVisaEO> responseList = new List<QualnVisaEO>();
            QualnVisaEO response = null;
            List<ODPCommandParameter> parametersList = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList = new List<ODPCommandParameter>();
                    parametersList.Add(new ODPCommandParameter("pi_staffno", staffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("cur_qualification", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_crewprofile.get_crew_qual", parametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new QualnVisaEO();

                            response.Qualification = objReader["qualification"] != null && objReader["qualification"].ToString() != string.Empty ? (System.String)(objReader["qualification"]) : default(System.String);
                            response.ValidFrom = objReader["validfrom"] != null && objReader["validfrom"].ToString() != string.Empty ? Common.OracleDateFormateddMMMyyyy(Convert.ToDateTime(objReader["validfrom"])) : string.Empty;
                            response.ValidTo = objReader["validto"] != null && objReader["validto"].ToString() != string.Empty ? Common.OracleDateFormateddMMMyyyy(Convert.ToDateTime(objReader["validto"])) : string.Empty;
                            
                            responseList.Add(response);
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

            return responseList;
        }


        public async Task<List<IDPEO>> GetCrewIDPAsync(string staffNumber)
        {
            List<IDPEO> responseList = new List<IDPEO>();
            IDPEO response = null;
            List<ODPCommandParameter> parametersList = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList = new List<ODPCommandParameter>();
                    parametersList.Add(new ODPCommandParameter("pi_staffno", staffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("cur_idps", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_crewprofile.get_crew_idps", parametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new IDPEO();

                            response.IDPTypeName = objReader["idp_typename"] != null && objReader["idp_typename"].ToString() != string.Empty ? (System.String)(objReader["idp_typename"]) : default(System.String);

                            responseList.Add(response);
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

            return responseList;
        }


        public async Task<List<FileEO>> GetCrewMyDocAsync(string staffNumber)
        {
            List<FileEO> responseList = new List<FileEO>();
            FileEO response = null;
            List<ODPCommandParameter> parametersList = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList = new List<ODPCommandParameter>();
                    parametersList.Add(new ODPCommandParameter("pi_staffno", staffNumber, ParameterDirection.Input, OracleDbType.Varchar2));

                    //// Waiting: Add the Stored Procedure name and Cursor once ready.
                    //using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("", parametersList, ""))
                    //{
                    //    while (objReader.Read())
                    //    {
                    //        response = new FileEO();
                    //        // Waiting: Get the Column names once they are ready by SP team.
                    //        response.FileName = objReader[""] != null && objReader[""].ToString() != string.Empty ? (System.String)(objReader[""]) : default(System.String);
                    //        response.FileContent = objReader[""] as byte[];

                    //        responseList.Add(response);
                    //    }
                    //}
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

            return responseList;
        }


        public async Task<List<DestinationsVisitedEO>> GetCrewDstVstdAsync(string staffNumber)
        {
            List<DestinationsVisitedEO> responseList = new List<DestinationsVisitedEO>();
            DestinationsVisitedEO response = null;
            List<ODPCommandParameter> parametersList = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList = new List<ODPCommandParameter>();
                    parametersList.Add(new ODPCommandParameter("pi_staffno", staffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("cur_destinations", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_crewprofile.get_crew_destination_mig", parametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new DestinationsVisitedEO();
                            response.City = objReader["sectorto"] != null && objReader["sectorto"].ToString() != string.Empty ? (System.String)(objReader["sectorto"]) : default(System.String);
                            response.LastVisitedDate = objReader["lastvisit"] != null && objReader["lastvisit"].ToString() != string.Empty ? Common.OracleDateFormateddMMMyyyy(Convert.ToDateTime(objReader["lastvisit"])) : string.Empty;
                            response.NoOfVisits = Convert.ToInt32(objReader["visits"] != null && objReader["visits"].ToString() != string.Empty ? (objReader["visits"]) : 0);

                            responseList.Add(response);
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
                        
            return responseList;
        }

        public async Task<List<CareerPathEO>> GetCrewCareerPathAsync(string staffNumber)
        {

            List<CareerPathEO> responseList = new List<CareerPathEO>();
            CareerPathEO response = null;
            List<ODPCommandParameter> parametersList = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList = new List<ODPCommandParameter>();
                    parametersList.Add(new ODPCommandParameter("pi_staffno", staffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("cur_promotiondets", ParameterDirection.Output, OracleDbType.RefCursor));
                    //using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_crewprofile.get_crew_careerpath", parametersList))
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_crewprofile.get_crew_careerpath_mig", parametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new CareerPathEO();
                            response.FromGrade = objReader["fromgrade"] != null && objReader["fromgrade"].ToString() != string.Empty ? (System.String)(objReader["fromgrade"]) : default(System.String);
                            response.ToGrade = objReader["tograde"] != null && objReader["tograde"].ToString() != string.Empty ? (System.String)(objReader["tograde"]) : default(System.String);
                            response.EffectiveDate = objReader["effectivedate"] != null ? (DateTime?)Convert.ToDateTime(objReader["effectivedate"]) : null;
                            response.DOJ = objReader["doj"] != null ? (DateTime?)Convert.ToDateTime(objReader["doj"]) : null;

                            responseList.Add(response);
                        }
                    }

                    responseList = responseList.OrderByDescending(x => x.EffectiveDate).Reverse().ToList();
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

            return responseList;
        }
    }
}
