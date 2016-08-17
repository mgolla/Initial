using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{

    public class VRCrewDetailEO
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

    public class VRCrewCountEO
    {

        public string CrewCount
        {
            get;
            set;
        }
    }

    public class VRPassengerDetailEO
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

    public class VRDocumentDetailEO
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

    public class VRDeptDetailEO
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
