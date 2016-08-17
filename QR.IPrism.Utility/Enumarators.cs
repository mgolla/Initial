using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Utility
{
    public static class Constants
    {
        public const string CONNECTION_STR = "OracleConnectionString";
        public const string AIMS_CONNECTION_STR = "AIMSOracleConnectionString";
        public const string ORACLE_CONNECTION_STRING_ODP = "OracleConnectionStringODP";
        public const string IPRISM_CNSP = "iPrismCNSP";
        public const string ACTIVE = "Y";
        public const string IN_ACTIVE = "N";
        public const int DohaTimeOffset = 3;
        public const string DECREPT_KEY = "JUST5212";
        public const string DATEFORMAT = "dd-MMM-yyyy";
        public enum AssessmentStatus
        {
            Complete,
            Dispute,
            Incomplete,
            ScheduledSaved,
            DelayedSaved,
            RequestedSaved
        }

    }



    public static class IsDataLoaded
    {
        public const int Yes = 1;
        public const int No = 2;
        public const int Loading = 0;
    }

    public static class TimeFormat
    {
        public const int UTCTime = 1;
        public const int DOHATime = 2;
        public const int LocalTime = 3;
    }
    public static class RosterType
    {
        public const int Weekly = 1;
        public const int Monthly = 2;

        public const string WeeklyRosterDays = "WeeklyRosterDays";
        public const string RightValue = "RIGHTVALUE";
        public const string LeftValue = "LEFTVALUE";


    }
    public static class CrewGrade
    {
        public const string CP = "CP";
        public const string PO = "PO";
        public const string FO = "FO";
        public const string CSD = "CSD";
        public const string CD = "CD";
        public const string CS = "CS";
        public const string F1 = "F1";
        public const string F2 = "F2";
    }

    public static class FilterButton
    {
        public const int Next = 1;
        public const int Previous = 2;
        public const int Other = 3;
    }
    public static class SOSCursor
    {
        public const int economyFlag = 1;
        public const int nfEconomyFlag = 2;
        public const int nfPremium = 3;
        public const int piFlag = 4;
        public const int ngFlag = 5;
    }
    public static class SOSType
    {
        public const int Economy = 1;
        public const int Premium = 2;
    }
    public static class MealType
    {
        public const String Meal1 = "1";
        public const String Meal2 = "2";
        public const String Meal3 = "3";
        public const String Meal4 = "4";
        public const String MealFC = "FC";
        public const String MealJC = "JC";
    }
    public static class MovieSnackType
    {
        public const String MSnack1 = "1";
        public const String MSnack2 = "2";
    }
    public static class BlanketType
    {
        public const String BlanketJC = "JC";
        public const String BlanketFC = "FC";
    }

    public static class HousingConstants
    {
        public const String Housing = "Housing";
        public const String EntityType = "EntityType";
        public const String Guest_Accommodation = "Guest Accommodation";
        public const String Swap_Rooms = "Swap Rooms";
        public const String Severity_Medium = "Medium";
        public const String Severity_High = "High";
        public const String Swap_Req_Approval = "Swap Room Request Recipient Approval";
        public const String Swap_Flatmate_Approval = "Swap Room Flatmate Approval Request";
        public const String Swap_Req = " Swap rooms request";
        public const String CA = "Change Accommodation";
        public const String MI = "Moving In";
        public const String GA = "Guest Accommodation";
        public const String SR = "Swap Rooms";
        public const String MO = "Moving Out of Company Accommodation";
        public const String SO = "Stay Out Request";
    }

    public static class UploadType
    {
        public const String Housing = "Housing";
        public const String EVRSave = "EVRSave";
        public const String RecordAssessment = "RecAsmt";
        public const String Kafou = "Kafousave";
    }

    public static class NotificationStatus
    {
        public const String Approved = "A";
        public const String Approved_Y = "Y";
        public const String Submitted = "U";
        public const String Rejected = "R";
        public const String Rejected_N = "N";
        public const String In_Progress = "I";
        public const String Closed = "C";
        public const String Sent_For_Approval = "S";
        public const String Assigned = "O";
        public const String Transferred = "T";
        public const String Done = "D";
    }

    public static class NewsType
    {
        public const int DepartmentNews = 1;
        public const int AirlineNews = 2;
    }
    public static class FlightPrefix
    {
        public const String Prefix = "QR";
    }
    public static class ODSFlightLoad
    {
        public const String CodeCommon = "ODS";
        public const String ODSUserName = "ODSUserName";
        public const String ODSPassword = "ODSPassword";
        public const String ODSAppCode = "ODSAppCode";

        public const String ODSUserNameValue = "QHOME";
        public const String ODSPasswordValue = "PRISMUSER";
        public const String ODSAppCodeValue = "YlrriUiNZ7Zbu9adcGQL+g==";
    }

    public static class Assessment
    {
        public const String Assessments = "Assessments";
        public const String DELAYED = "Delayed";
        public const String DELAYEDSAVE = "DelayedSaved";
        public const String SCHSAVE = "ScheduledSaved";
        public const String COMPLETED = "Complete";
        public const String ELAPSED = "Elapsed";

        public const String SCHEDULED = "Scheduled";
        public const String YES = "Y";
        public const String NO = "N";
        public const String EE = "EE";
        public const String DEFAULT = "Default";
        public const String COM = "COM";
        public const String DEV = "DEV";
        public const String SD = "SD";
        public const String SEE = "SEE";
        public const String SEEDESC = "Significantly Exceeds Expectations";
        public const String EEDESC = "Exceeds Expectations";
        public const String COMDESC = "Competent";
        public const String DEVDESC = "Development Required";
        public const String SDDESC = "Significantly Development Required";
        public const String SCHEDULEDSAVE = "ScheduledSaved";

    }
}
