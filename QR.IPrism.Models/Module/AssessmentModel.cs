using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace QR.IPrism.Models.Module
{
   public class AssessmentModel
    {
       public string StaffId { get; set; }
        public string AssessmentID { get; set; }
        public string ManagerStaffNo { get; set; }
        public string ManagerStaffName { get; set; }
        public string AssessmentType { get; set; }
        public string AssessmentStatus { get; set; }
        public DateTime? AssessmentDate { get; set; }
        public Decimal TotalScore { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(4000, MinimumLength = 50, ErrorMessage = "Invalid")]
        public string AdditionalComments { get; set; }
        public string DelayReasonComments { get; set; }
        public string CabinComments { get; set; }
        public int Pax_Load_F { get; set; }
        public int Pax_Load_J { get; set; }
        public int Pax_Load_Y { get; set; }
        public string ReasonForNonSubmission { get; set; }
        public string ReasonforNonsubmissionID { get; set; }
        public string IsActive { get; set; }
        public string IDPCount { get; set; }
        public string CompetencyID { get; set; }
        public string CompetencyName { get; set; }
        public string CompetencyActive { get; set; }
        public int Score { get; set; }
        public string Comments { get; set; }
        public int IsRecorded { get; set; }

        public string FlightDetID { get; set; }
        public string AssesseeCrewDetID { get; set; }
        public string AssessorCrewDetID { get; set; }
        public string AssessorStaffName { get; set; }
        public string AssessorStaffNo { get; set; }
        public string AssesseeStaffName { get; set; }
        public string AssesseeStaffNo { get; set; }
        public string AssesseeGrade { get; set; }
        public string AssessorGrade { get; set; }
        public string AssesseeGradeSince { get; set; }
        public string AssessorGradeSince { get; set; }
        public string SectorFrom { get; set; }
        public string SectorTo { get; set; }
        public string FlightNumber { get; set; }
        public DateTime? FlightDate { get; set; }
        public int Infants { get; set; }
        public string CrewPosition { get; set; }
        public string AircraftType { get; set; }
        public string AssesseeDesignation { get; set; }
        public string LevelId { get; set; }
        public string LevelName { get; set; }
        public string RequirementId { get; set; }
        public string RequirementName { get; set; }
        public string RequirementDesc { get; set; }
        public string POBriefingComment { get; set; }
        public string POOnBoardComment { get; set; }
        public string CSDCSBriefingComment { get; set; }
        public string CSDCSOnBoardComment { get; set; }
        public string Rating { get; set; }
        public string Dispute { get; set; }
        public string Isrequirementselected { get; set; }
        public string RequestReason { get; set; }
        public string RequestedBy { get; set; }
        public string CreatedBy { get; set; }
        public string IdpTypeID { get; set; }
        public string AutoSave { get; set; }
        

        public IList<ObjectiveModel> Objectives { get; set; }
        public IList<RatingGuideLineModel> RatingGuidelines { get; set; }
        public List<RatingGuideLineModel> RatingForDropDown { get; set; }
        public IList<FileModel> Attachments { get; set; }

        //PO Assessment Fields
        public IList<UserContextModel> StaffDetails { get; set; }
        public IList<FlightInfoModel> FlightDetails { get; set; }
        //public IList<AssessmentSearchModel> AssessmentDetails { get; set; }
        public string LastAssessment { get; set; }
        public string AssessmentsScheduled { get; set; }
        public DateTime ExpAssmtDate { get; set; }


    }
}
