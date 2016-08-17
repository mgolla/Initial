using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class EVRCrewViewModel
    {
        public string VrId
        {
            get;
            set;
        }

        public int VrNo
        {
            get;
            set;
        }

        public string FlightNumber
        {
            get;
            set;
        }

        public DateTime ActDeptTime
        {
            get;
            set;
        }

        public DateTime ActArrTime
        {
            get;
            set;
        }

        public string SectorFrom
        {
            get;
            set;
        }

        public string SectorTo
        {
            get;
            set;
        }

        public string AirCraftRegNo
        {
            get;
            set;
        }

        public string AirCraftType
        {
            get;
            set;
        }

        public string StaffNumber
        {
            get;
            set;
        }
        public string FirstName
        {
            get;
            set;
        }

        public string MiddleName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string Grade
        {
            get;
            set;
        }

        public string CrewCount
        {
            get;
            set;
        }
        public string Facts
        {
            get;
            set;
        }

        public string Action
        {
            get;
            set;
        }

        public string Result
        {
            get;
            set;
        }
        public string IsNew
        {
            get;
            set;
        }

        public string IsCSR
        {
            get;
            set;
        }
        public string IsOHS
        {
            get;
            set;
        }

        public string IsPO
        {
            get;
            set;
        }

        public string IsCritical
        {
            get;
            set;
        }

        public string CrewComments
        {
            get;
            set;
        }

        public string VrStatusName
        {
            get;
            set;
        }

        public int PassengerLoadFC
        {
            get;
            set;
        }
        public int PassengerLoadJC
        {
            get;
            set;
        }

        public int PassengerLoadYC
        {
            get;
            set;
        }

        public int InfantLoadFC
        {
            get;
            set;
        }

        public int InfantLoadJC
        {
            get;
            set;
        }

        public int InfantLoadYC
        {
            get;
            set;
        }

        public int SeatCapacityFC
        {
            get;
            set;
        }

        public int SeatCapacityJC
        {
            get;
            set;
        }

        public int SeatCapacityYC
        {
            get;
            set;
        }

        public string IsGroominCheck
        {
            get;
            set;
        }

        public string IsCSDCSBriefed
        {
            get;
            set;
        }

        public string GroomingCheckComment
        {
            get;
            set;
        }

        public string CSDCSBriefedComment
        {
            get;
            set;
        }

        public string FlightRoute
        {
            get;
            set;
        }

        public string IsCabInClassFC
        {
            get;
            set;
        }

        public string IsCabInClassJC
        {
            get;
            set;
        }

        public string IsCabInClassYC
        {
            get;
            set;
        }

        public string ReportAboutName
        {
            get;
            set;
        }
        public string CategoryName
        {
            get;
            set;
        }

        public string SubCategoryName
        {
            get;
            set;
        }

        public IList<VRCrewDetailModel> VRCrewDetail
        {
            get;
            set;
        }

        public IList<VRCrewDetailModel> VRAllCrewDetail
        {
            get;
            set;
        }

        public IList<VRPassengerDetailModel> VRPassengerDetail
        {
            get;
            set;
        }
        public IList<VRDocumentDetailModel> VRDocumentDetail
        {
            get;
            set;
        }
        public IList<VRDeptDetailModel> VRDeptDetail
        {
            get;
            set;
        }
        public IList<VRActionResolutionModel> vrAtDeptList
        {
            get;
            set;
        }
        public string IsFromAIMS { get; set; }
        public string IsManuallyEntered { get; set; }
        public bool IsFerry { get; set; }
    }
}
