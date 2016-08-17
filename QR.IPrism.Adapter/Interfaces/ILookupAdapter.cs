using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Models.Shared;

namespace QR.IPrism.Adapter.Interfaces
{
    public interface ILookupAdapter
    {
        Task<IEnumerable<LookupModel>> GetLookupListAsync(string lookupTypeCode);
        Task<IEnumerable<LookupModel>> GetLookupListAsync(LookupSearchModel search);
        Task<IEnumerable<LookupModel>> GetFilteredDataAsync(string lookupTypeCode, string filterText);
        Task<IEnumerable<LookupModel>> GetAutoCompleteAsync(string autoCompleteCategory, string searchText);
        Task<IEnumerable<LookupModel>> GetAutoCompleteAsync(string autoCompleteCategory, string searchText, string filterText);
        //Task<IEnumerable<LookupModel>> GetReasonForRecNonAsmtAsync();
        //Task<IEnumerable<LookupModel>> GetHousingRequestTypeAsync();
        //Task<IEnumerable<LookupModel>> GetHousingStatusAsync();
        //Task<IEnumerable<LookupModel>> GetAllVRStatus();
        //Task<IEnumerable<LookupModel>> GetAllVRSectors();
        //Task<IEnumerable<LookupModel>> GetEVRReportAbout();
        //Task<IEnumerable<LookupModel>> GetDeptListForReportAbout();
        Task<IEnumerable<LookupModel>> GetGradeAsync();
        Task<IEnumerable<LookupModel>> GetGradeByLoggedPersonAsync( string grade);
        Task<IEnumerable<LookupModel>> GetAssmtStatusAsync();
        Task<IEnumerable<LookupModel>> GetPendingAssessmentAsync();
        Task<IEnumerable<LookupModel>> GetGradeCSDCS();
        //Task<IEnumerable<LookupModel>> GetSectorFrom();
        //Task<IEnumerable<LookupModel>> GetSectorTo();
    }
}
