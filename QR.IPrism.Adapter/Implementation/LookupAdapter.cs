/*********************************************************************
 * Name          : IHousingAdapter.cs
 * Description   : Interface for Getting all of Lookup data from DAO layer.
 * Create Date   : 25th Jan 2016
 * Last Modified : 26th Jan 2016
 * Copyright By  : Qatar Airways
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Adapter.Helper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Models.Shared;
using QR.IPrism.BusinessObjects.Data_Layer.Shared;
using QR.IPrism.EntityObjects.Shared;
using AutoMapper;

namespace QR.IPrism.Adapter.Implementation
{
    public class LookupAdapter : ILookupAdapter
    {
        #region Private Variables
        Dictionary<LookupTypeCodes, LoadLookup> LookupCodesAvailable = new Dictionary<LookupTypeCodes, LoadLookup>();
        private readonly SharedDao _sharedDao = new SharedDao();
        private LookupSearchModel _searchModel { get; set; }
        private string _filterText { get; set; }
        #endregion
        #region Constructor
        public LookupAdapter()
        {
            configureLookup();
        }
        #endregion
        #region Lookup
        /// <summary>
        /// Configures all lookups
        /// </summary>
        private void configureLookup()
        {
            LookupCodesAvailable.Add(LookupTypeCodes.HousingSearchRequestCode, new LoadLookup(GetHousingRequestTypeAsync));
            LookupCodesAvailable.Add(LookupTypeCodes.HousingRequestTypeByStaff, new LoadLookup(GetHousingRequestTypeByStaffAsync));
            LookupCodesAvailable.Add(LookupTypeCodes.HousingSearchStatus, new LoadLookup(GetHousingStatusAsync));
            LookupCodesAvailable.Add(LookupTypeCodes.EVRAllStatus, new LoadLookup(GetAllVRStatus));
            //LookupCodesAvailable.Add(LookupTypeCodes.EVRAllSectors, new LoadLookup(GetAllVRSectors));
            LookupCodesAvailable.Add(LookupTypeCodes.ReportAbout, new LoadLookup(GetEVRReportAbout));
            LookupCodesAvailable.Add(LookupTypeCodes.CategoryForReportAbout, new LoadLookup(GetCategoryForReportAbout));
            LookupCodesAvailable.Add(LookupTypeCodes.SubCategoryForReportAbout, new LoadLookup(GetSubCategoryForCategory));
            LookupCodesAvailable.Add(LookupTypeCodes.DeptForReportAbout, new LoadLookup(GetDeptListForReportAbout));
            LookupCodesAvailable.Add(LookupTypeCodes.AssmtGrade, new LoadLookup(GetGradeAsync));
            LookupCodesAvailable.Add(LookupTypeCodes.AssmtStatus, new LoadLookup(GetAssmtStatusAsync));
            LookupCodesAvailable.Add(LookupTypeCodes.PendingAssessment, new LoadLookup(GetPendingAssessmentAsync));
            LookupCodesAvailable.Add(LookupTypeCodes.GradeCsCsd, new LoadLookup(GetGradeCSDCS));
           // LookupCodesAvailable.Add(LookupTypeCodes.SectoFrom, new LoadLookup(GetSectorFrom));
           // LookupCodesAvailable.Add(LookupTypeCodes.SectorTo, new LoadLookup(GetSectorTo));
            LookupCodesAvailable.Add(LookupTypeCodes.Sector, new LoadLookup(GetSector));
            LookupCodesAvailable.Add(LookupTypeCodes.eVROwnerNonOwner, new LoadLookup(GetEVROwnerNonOwner));
            LookupCodesAvailable.Add(LookupTypeCodes.Cities, new LoadLookup(GetAllCitiesAsync));
            LookupCodesAvailable.Add(LookupTypeCodes.CountryCode, new LoadLookup(GetAllCoutriesAsync));
            LookupCodesAvailable.Add(LookupTypeCodes.AirportCodes, new LoadLookup(GetAllAirportCodesAsync));
            LookupCodesAvailable.Add(LookupTypeCodes.CurrecncyCodes, new LoadLookup(GetAllCurrencyCodesAsync));
            LookupCodesAvailable.Add(LookupTypeCodes.ReasonForRecNonAsmt, new LoadLookup(GetReasonForRecNonAsmtAsync));
        }
        /// <summary>
        /// Loads All Lookup on the basis of the code provided.
        /// Switch statement is not being used to avoid the code cyclomatic complexity
        /// </summary>
        /// <param name="lookupTypeCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LookupModel>> GetLookupListAsync(string lookupTypeCode)
        {
            LookupTypeCodes lookupTypeCodeEnum = (LookupTypeCodes)Enum.Parse(typeof(LookupTypeCodes), lookupTypeCode, true);
            return await LookupCodesAvailable[lookupTypeCodeEnum]();
        }
        /// <summary>
        /// Loads All Lookup on the basis of the code provided and filter text.
        /// Switch statement is not being used to avoid the code cyclomatic complexity
        /// </summary>
        /// <param name="lookupTypeCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LookupModel>> GetFilteredDataAsync(string lookupTypeCode, string filterText)
        {
            IEnumerable<LookupModel> lookupData = await GetLookupListAsync(lookupTypeCode);
            return lookupData.Where(x => x.FilterText.Equals(filterText, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Loads All Lookup on the basis of the code provided and filter text.
        /// Switch statement is not being used to avoid the code cyclomatic complexity
        /// </summary>
        /// <param name="lookupTypeCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LookupModel>> GetAutoCompleteAsync(string autoCompleteCategory, string searchText)
        {
            IEnumerable<LookupModel> lookupData = await GetLookupListAsync(autoCompleteCategory);
            return lookupData.Where(m => m.Value.ToLower().Contains(searchText.ToLower()) || m.Text.ToLower().Contains(searchText.ToLower()));
        }

        /// <summary>
        /// Loads All Lookup on the basis of the code provided and filter text.
        /// Switch statement is not being used to avoid the code cyclomatic complexity
        /// </summary>
        /// <param name="lookupTypeCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LookupModel>> GetAutoCompleteAsync(string autoCompleteCategory, string searchText, string filterText)
        {
            IEnumerable<LookupModel> lookupData = await GetLookupListAsync(autoCompleteCategory);
            return lookupData.Where(m => (m.FilterText.Equals(filterText, StringComparison.OrdinalIgnoreCase)) && (m.Value.ToLower().Contains(searchText.ToLower()) || m.Text.ToLower().Contains(searchText.ToLower())));
        }

        public async Task<IEnumerable<LookupModel>> GetLookupListAsync(LookupSearchModel search)
        {
            this._searchModel = search;
            LookupTypeCodes lookupTypeCodeEnum = (LookupTypeCodes)Enum.Parse(typeof(LookupTypeCodes), search.LookupTypeCode, true);
            return await LookupCodesAvailable[lookupTypeCodeEnum]();
        }
        /// <summary>
        /// Gets Lookup data for Housing request type  
        /// </summary>
        /// <returns>List of LookupModel</returns>
        private async Task<IEnumerable<LookupModel>> GetHousingRequestTypeAsync()
        {
            return Mapper.Map(await _sharedDao.GetHousingRequestTypeAsync(), new List<LookupModel>());
        }

        /// <summary>
        private async Task<IEnumerable<LookupModel>> GetHousingRequestTypeByStaffAsync()
        {
            return Mapper.Map(await _sharedDao.GetHousingRequestTypeByStaffAsync(this._searchModel.StaffNo), new List<LookupModel>());
        }
        /// Gets lookup data for Housing request status
        /// </summary>
        /// <returns>List of LookupModel</returns>
        private async Task<IEnumerable<LookupModel>> GetHousingStatusAsync()
        {
            return Mapper.Map(await _sharedDao.GetHousingStatusAsync(), new List<LookupModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetGradeAsync()
        {
            return Mapper.Map(await _sharedDao.GetGradeAsync(), new List<LookupModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetGradeByLoggedPersonAsync(string grade)
        {
            var list = new List<LookupModel>();

            var data = Mapper.Map(await _sharedDao.GetGradeAsync(), new List<LookupModel>());
            if (grade == "CS" || grade == "CSD")
            {
                list = data.Where(x => x.Text == "F1" || x.Text == "F2").ToList();
            }
            else if (grade != "F1" || grade != "F2")
            {
                list = data;
            }

            return list;
        }


        public async Task<IEnumerable<LookupModel>> GetAssmtStatusAsync()
        {
            return Mapper.Map(await _sharedDao.GetAssmtStatusAsync(), new List<LookupModel>());
        }
        public async Task<IEnumerable<LookupModel>> GetPendingAssessmentAsync()
        {
            return Mapper.Map(await _sharedDao.GetPendingAssessmentAsync(), new List<LookupModel>());
        }
        public async Task<IEnumerable<LookupModel>> GetGradeCSDCS()
        {
            return Mapper.Map(await _sharedDao.GetGradeCSDCS(), new List<LookupModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetSector()
        {
            return Mapper.Map(await _sharedDao.GetSectorAsync(), new List<LookupModel>());
        }

        //public async Task<IEnumerable<LookupModel>> GetSectorFrom()
        //{
        //    return Mapper.Map(await _sharedDao.GetSectorFrom(), new List<LookupModel>());
        //}
        //public async Task<IEnumerable<LookupModel>> GetSectorTo()
        //{
        //    return Mapper.Map(await _sharedDao.GetSectorTo(), new List<LookupModel>());
        //}

        private async Task<IEnumerable<LookupModel>> GetReasonForRecNonAsmtAsync()
        {
            return Mapper.Map(await _sharedDao.GetReasonForRecNonAsmtAsync(), new List<LookupModel>());
        }

        #region eVR Module Service Calls

        public async Task<IEnumerable<LookupModel>> GetAllVRStatus()
        {
            return Mapper.Map(await _sharedDao.GetAllVRStatusAsync(), new List<LookupModel>());
        }

        //public async Task<IEnumerable<LookupModel>> GetAllVRSectors()
        //{
        //    return Mapper.Map(await _sharedDao.GetAllVRSectorsAsync(), new List<LookupModel>());
        //}

        public async Task<IEnumerable<LookupModel>> GetAllCoutriesAsync()
        {
            return Mapper.Map(await _sharedDao.GetAllCoutriesAsync(), new List<LookupModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetAllAirportCodesAsync()
        {
            return Mapper.Map(await _sharedDao.GetAllAirportCodesAsync(), new List<LookupModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetAllCurrencyCodesAsync()
        {
            return Mapper.Map(await _sharedDao.GetAllCurrencyCodesAsync(), new List<LookupModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetAllCitiesAsync()
        {
            return Mapper.Map(await _sharedDao.GetAllCitiesAsync(), new List<LookupModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetEVRReportAbout()
        {
            return Mapper.Map(await _sharedDao.GetAllReportAboutAsync(), new List<LookupModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetCategoryForReportAbout()
        {
            return Mapper.Map(await _sharedDao.GetAllCategoryForReportAboutAsync(_searchModel.SearchText), new List<LookupModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetSubCategoryForCategory()
        {
            return Mapper.Map(await _sharedDao.GetAllCategoryForSubCategoryAsync(_searchModel.SearchText), new List<LookupModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetDeptListForReportAbout()
        {
            return Mapper.Map(await _sharedDao.GetAllDepartForEVRAsync(_searchModel.ArrSearchText), new List<LookupModel>());
        }

        public async Task<IEnumerable<LookupModel>> GetEVROwnerNonOwner()
        {
            return Mapper.Map(await _sharedDao.GetEVROwnerNonOwnerAsyc(_searchModel.ArrSearchText), new List<LookupModel>());
        }

        #endregion

        #endregion
    }
}
