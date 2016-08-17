using AutoMapper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.BusinessObjects.Data_Layer.Shared;
using QR.IPrism.BusinessObjects.Implementation;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using QR.IPrism.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Implementation
{
    public class AssessmentSearchAdapter : IAssessmentSearchAdapter
    {
        /// <summary>
        /// Function that returns ILIST results for searched assessment list
        /// </summary>
        /// <param name="input"></param>
        /// <returns>ILIST with search results for Searched list assessments</returns>
        ///
        private readonly IAssessmentSearchDao _assmtSearchdao = new AssessmentSearchDao();
        public async Task<IEnumerable<AssessmentSearchModel>> GetAssmtSearchResultAsync(AssessmentSearchRequestFilterModel assmtSearchFilterModel)
        {
            if (assmtSearchFilterModel.Grade == "--All--")
            {
                assmtSearchFilterModel.Grade = "";
            }
            if (assmtSearchFilterModel.AssessmentStatus == "--All--")
            {
                assmtSearchFilterModel.AssessmentStatus = "";
            }
            var filter = Mapper.Map(assmtSearchFilterModel, new AssessmentSearchRequestFilterEO());
            return Mapper.Map(await _assmtSearchdao.GetAssmtSearchResultAsync(filter), new List<AssessmentSearchModel>());
        }

        public async Task<List<AssessmentModel>> GetAllPreviousAssessment(string id)
        {
            return Mapper.Map(await _assmtSearchdao.GetAllPreviousAssessment(id), new List<AssessmentModel>());
        }

        public async Task<IEnumerable<AssessmentSearchModel>> SavedUnscheduledAssessmentAsync(AssessmentSearchModel filter)
        {
            var input = Mapper.Map(filter, new AssessmentSearchEO());
            return Mapper.Map(await _assmtSearchdao.SavedUnscheduledAssessmentAsync(input), new List<AssessmentSearchModel>());
        }
        public async Task<ResponseModel> ValidateUnscheduledData(AssessmentSearchModel filter)
        {
            var input = Mapper.Map(filter, new AssessmentSearchEO());
            return Mapper.Map(await _assmtSearchdao.ValidateUnscheduledData(input), new ResponseModel());
        }

        public async Task<List<AssessmentSearchModel>> GetCrewExpectedAsmnt(AssessmentSearchRequestFilterModel assmtSearchFilterModel)
        {
            var filter = Mapper.Map(assmtSearchFilterModel, new AssessmentSearchRequestFilterEO());


            if (!String.IsNullOrWhiteSpace(filter.FlightNumber) &&
                !string.IsNullOrWhiteSpace(filter.SectorTo) &&
                !string.IsNullOrWhiteSpace(filter.ToDate.ToString()))
            {
                List<AssessmentSearchRequestFilterEO> crewDetails = await _assmtSearchdao.GetCrewOnBoard(filter);
                string staffOnBoard = GetCrewOnBoard(crewDetails);
                filter.StaffNumber = staffOnBoard;
                return DisplayCrewAssessmentInfo(filter, crewDetails).Result;
            }
            else
            {
                return DisplayCrewAssessmentInfo(filter, null).Result;
            }
        }

        private async Task<List<AssessmentSearchModel>> DisplayCrewAssessmentInfo(
            AssessmentSearchRequestFilterEO filter, List<AssessmentSearchRequestFilterEO> crewDetails)
        {
            List<AssessmentSearchEO> crewAsmntDates = await _assmtSearchdao.GetCrewExpectedAsmnt(filter);
            foreach (AssessmentSearchEO asmnt in crewAsmntDates)
            {
                DateTime asmntDate = Convert.ToDateTime(asmnt.ToDate);
                DateTime fromDate = Convert.ToDateTime(asmnt.FromDate);
                asmnt.AssessmentDate =
                    string.Format("{0:dd-MMM-yyyy}", asmntDate) + " ("
                    + Convert.ToString(DateTime.Today.Subtract(asmntDate).Days) + ")";
                asmnt.ExpectedDate =
                    string.Format("{0:dd-MMM-yyyy}", fromDate) + " ("
                    + Convert.ToString(fromDate.Subtract(DateTime.Today).Days) + ")";

                if (crewDetails != null)
                {
                    var crew = crewDetails.FirstOrDefault(i => i.StaffNumber == asmnt.StaffName.Split('(')[1].ToString().Replace(")", ""));
                    asmnt.StaffName = (crew.Grade == "CD" ? "CSD" : crew.Grade )+ " - " + crew.StaffName + "(" + crew.StaffNumber + ")"; 
                }
            }

            return Mapper.Map(crewAsmntDates, new List<AssessmentSearchModel>());
        }

        protected string GetCrewOnBoard(List<AssessmentSearchRequestFilterEO> crewDetails)
        {
            try
            {
                string staffNoCOB = string.Empty;

                foreach (var item in crewDetails)
                {
                    staffNoCOB += item.StaffNumber + ",";
                }

                //if (crewDetails != null && crewDetails.Count > 0)
                //{
                //    for (int crew = 0; crew < crewDetails.Count; crew++)
                //    {
                //        staffNoCOB += Convert.ToString(crewDetails[crew].Split('(')[1].Split(')')[0]);
                //        staffNoCOB += ",";
                //    }
                //}

                return staffNoCOB;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return string.Empty;
        }


    }
}
