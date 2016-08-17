using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Models;
using QR.IPrism.Models.Shared;

namespace QR.IPrism.Adapter.Helper
{
    public enum LookupTypeCodes
    {
        TitleCode,
        GenderCode,
        CountryCode,
        Cities,
        AirportCodes,
        CurrecncyCodes,
        AppConfig,
        TimeFormat,
        HousingSearchRequestCode,
        HousingRequestTypeByStaff,
        HousingSearchStatus,
        EVRAllStatus,
        //EVRAllSectors,
        ReportAbout,
        CategoryForReportAbout,
        SubCategoryForReportAbout,
        DeptForReportAbout,
        SubCategoRYForReportAbout,
        AssmtGrade,
        AssmtStatus,
        PendingAssessment,
        GradeCsCsd,
        //SectoFrom,
        //SectorTo,
        Sector,
        eVROwnerNonOwner,
        ReasonForRecNonAsmt
//        KafouSearch
    }
    delegate Task<IEnumerable<LookupModel>> LoadLookup();
}
