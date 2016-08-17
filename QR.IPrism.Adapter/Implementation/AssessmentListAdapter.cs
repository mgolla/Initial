/*********************************************************************
 * Name          : AssessmentListAdapter.cs
 * Description   : Implements IHousingAdapter.
 * Create Date   : 29th Feb 2016
 * Last Modified : 29th Feb 2016
 * Copyright By  : Qatar Airways
 *********************************************************************/

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
using QR.IPrism.Models.ViewModels;

namespace QR.IPrism.Adapter.Implementation
{
    public class AssessmentListAdapter : IAssessmentListAdapter
    {
        #region Private Variables
        private readonly IAssessmentListDao _assessmentListDao = new AssessmentListDao();

        #endregion

        #region IAssessmentListAdapter Implementation

        /// <summary>
        /// Gets the Assessment request search result from DAO layer.
        /// </summary>
        /// <param name="inputs">AssessmentList request filter</param>
        /// <returns>List of AssessmentSearchModel</returns>
        public async Task<IEnumerable<AssessmentSearchModel>> GetAssessmentListResultAsync(String id)
        {
            //AssessmentSearchRequestFilterModel inputs = new AssessmentSearchRequestFilterModel();
            //inputs.AssessorUserID = id;
           // var filter = Mapper.Map(inputs, new AssessmentSearchRequestFilterEO());
            return Mapper.Map(await _assessmentListDao.GetAssessmentListResultAsync(id), new List<AssessmentSearchModel>());
        }

        public async Task<AssessmentPOViewModel> GetPOAssessmentDataAsync(AssessmentSearchRequestFilterModel filterInput)
        {
            //Define variables 
            List<AssessmentSearchModel> cscsdModelList = new List<AssessmentSearchModel>();
            AssessmentPOViewModel vm = new AssessmentPOViewModel();

            List<AssessmentSearchEO> cscsdEOList = await _assessmentListDao.GetCSDCSListResultAsync(Mapper.Map(filterInput, new AssessmentSearchRequestFilterEO()));
            Mapper.Map<List<AssessmentSearchEO>, List<AssessmentSearchModel>>(cscsdEOList, cscsdModelList);


            //vm.POAssessmentScheduled = GetAssessmentListResultAsync(filterInput.AssessorUserID).Result.ToList();
            vm.POAssessmentCSDList = cscsdModelList.Where(u => u.Grade.Equals("csd", StringComparison.OrdinalIgnoreCase)).ToList();
            vm.POAssessmentCSList = cscsdModelList.Where(u => u.Grade.Equals("cs", StringComparison.OrdinalIgnoreCase)).ToList();

            return vm;
        }

        //public async Task<AssessmentPOViewModel> GetSearchCSDCSResultAsync(AssessmentSearchRequestFilterModel filterInput)
        //{
        //    //Define variables 

        //    List<AssessmentSearchModel> cscsdModelList = new List<AssessmentSearchModel>();

        //    AssessmentPOViewModel vm = new AssessmentPOViewModel();
        //    if (filterInput.Grade == "--All--")
        //    {
        //        filterInput.Grade = "";
        //    }

        //    List<AssessmentSearchEO> cscsdEOList = await _assessmentListDao.GetCSDCSListResultAsync(Mapper.Map(filterInput, new AssessmentSearchRequestFilterEO()));
        //    Mapper.Map<List<AssessmentSearchEO>, List<AssessmentSearchModel>>(cscsdEOList, cscsdModelList);

        //    vm.POAssessmentScheduled = cscsdModelList.Where(u => (u.Grade.ToUpper() != CrewGrade.CSD || u.Grade.ToUpper() != CrewGrade.CS)).ToList(); ;
        //    vm.POAssessmentCSDList = cscsdModelList.Where(u => u.Grade.ToUpper() == CrewGrade.CSD).ToList();
        //    vm.POAssessmentCSList = cscsdModelList.Where(u => u.Grade.ToUpper() == CrewGrade.CS).ToList();

        //    return vm;
        //}

        public void CancelSchedluedAsmnt(string requestId)
        {
            _assessmentListDao.CancelSchedluedAsmnt(requestId);
        }

        public async Task<ResponseModel> ScheduleAssessment(AssessmentModel inputs)
        {
            var filter = Mapper.Map(inputs, new AssessmentEO());
            return Mapper.Map(await _assessmentListDao.ScheduleAssessment(filter), new ResponseModel());
        }

        public async Task<IEnumerable<IGrouping<string, AssessmentModel>>> ViewAssmtAsync(string id)
        {
            var result = Mapper.Map(await _assessmentListDao.ViewAssmtAsync(id), new List<AssessmentModel>());
            return result.GroupBy(i => i.CompetencyName).ToList();
        }

        # endregion

        public async Task<IEnumerable<AssessmentSearchModel>> GetPreviousAssessmentsAsync(AssessmentSearchModel input, string staffId)
        {
            var filter = Mapper.Map(input, new AssessmentSearchEO());
            return Mapper.Map(await _assessmentListDao.GetPreviousAssessmentsAsync(filter, staffId), new List<AssessmentSearchModel>());
        }
    }
}
