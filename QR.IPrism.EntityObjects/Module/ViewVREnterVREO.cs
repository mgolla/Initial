using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{

    public class VRDeptEnterVREO
    {
        public string DeptId
        {
            get;
            set;
        }

        public string DeptNameCode
        {
            get;
            set;
        }

        public string IsActive
        {
            get;
            set;
        }
    }

    public class VRCrewEnterVREO
    {

        public string CrewDetailsId
        {
            get;
            set;
        }
    }

    public class ViewVREnterVREO
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

        public string StaffNumber
        {
            get;
            set;
        }

        public string VrInstanceId
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

        public string ReportAboutID
        {
            get;
            set;
        }

        public string CategoryId
        {
            get;
            set;
        }

        public string SubCategoryId
        {
            get;
            set;
        }

        public string IsCritical
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

        public string IsCabInClassNA
        {
            get;
            set;
        }

        public string Comments
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



        public string IsNotIfNotReq
        {
            get;
            set;
        }


        public string IsInfoVr
        {
            get;
            set;
        }

        public string IsVrRestricted
        {
            get;
            set;
        }

        public IList<VRDeptEnterVREO> VRDeptEnterVR
        {
            get;
            set;
        }

        public IList<EVRPassengerEO> VRPassengerEnterVR
        {
            get;
            set;
        }

        public IList<VRCrewEnterVREO> VRCrewEnterVR
        {
            get;
            set;
        }

        public IList<DocumentEO> VRDocEnterVR
        {
            get;
            set;
        }
    }
}
