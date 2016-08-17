using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.BusinessObjects.Implementation;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.ViewModels;

namespace QR.IPrism.Adapter.Implementation
{
    public class POAssessmentAdapter : IPOAssessmentAdapter
    {
        private readonly IPOAssessmentDao _poAssessmentDao = new POAssessmentDao();

        /// <summary>
        /// Function that returns ILIST results for PO Assessment List 
        /// </summary>
        /// <param name="Inputs"></param>
        /// <returns>ILIST with search results for PO Assessment List </returns>
        /// 
        public async Task<AssessmentViewModel> GetPOAssessmentDataAsync(AssessmentSearchRequestFilterModel filterInput)
        {
            //Define variables 
            List<AssessmentModel> poAssessmentmodelList = new List<AssessmentModel>();
            AssessmentViewModel vm = new AssessmentViewModel();


            //List<AssessmentEO> poAssessmentEoList = await _poAssessmentDao.GetPOAssmtListAsync(Mapper.Map(filterInput, new AssessmentSearchRequestFilterEO()));                        
            //Mapper.Map<List<AssessmentEO>, List<AssessmentModel>>(poAssessmentEoList, poAssessmentmodelList);

            //List<AssessmentEO> poAssessmentCSDList = await _poAssessmentDao.GetCSDListAsync(Mapper.Map(filterInput, new AssessmentSearchRequestFilterEO()));                  
            //Mapper.Map<List<AssessmentEO>, List<AssessmentModel>>(poAssessmentCSDList, poAssessmentmodelList);

            //List<AssessmentEO> poAssessmentCSList = await _poAssessmentDao.GetCSListAsync(Mapper.Map(filterInput, new AssessmentSearchRequestFilterEO()));                  
            //Mapper.Map<List<AssessmentEO>, List<AssessmentModel>>(poAssessmentCSList, poAssessmentmodelList);


            //vm.POAssessmentScheduleDetails = poAssessmentmodelList;
            //vm.POAssessmentCSDList = poAssessmentmodelList;
            //vm.POAssessmentCSList = poAssessmentmodelList;


            return vm;
        }
    }
}
