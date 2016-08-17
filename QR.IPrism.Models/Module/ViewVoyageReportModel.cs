using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class VRCrewDetailModel
    {

        public string StaffNumber
        {
            get;
            set;
        }


        public string StaffName
        {
            get;
            set;
        }


        public string Grade
        {
            get;
            set;
        }

        public string CrewPosition
        {
            get;
            set;
        }

        public string IsActingCSD
        {
            get;
            set;
        }

        public string AnnouncementLanguage
        {
            get;
            set;
        }

    }

    public class VRCrewCountModel
    {

        public string CrewCount
        {
            get;
            set;
        }
    }

    public class VRPassengerDetailModel
    {
        #region "Public properties"

        public string FFPNumber
        {
            get;
            set;
        }


        public string PassengerName
        {
            get;
            set;
        }


        public string SeatNumber
        {
            get;
            set;
        }

        #endregion
    }

    public class VRDocumentDetailModel
    {

        public string VrDocName
        {
            get;
            set;
        }

        public string VrDocPath
        {
            get;
            set;
        }

        public byte[] VrDocContent
        {
            get;
            set;
        }


        public string VrDocId
        {
            get;
            set;
        }

    }

    public class VRDeptDetailModel
    {

        public string DeptId
        {
            get;
            set;
        }


        public string DeptName
        {
            get;
            set;
        }


        public string SectionId
        {
            get;
            set;
        }

        public string SectionName
        {
            get;
            set;
        }
        public string DeptType
        {
            get;
            set;
        }
    }
}
