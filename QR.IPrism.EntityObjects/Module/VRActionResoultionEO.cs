using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class VRActionResolutionEO 
    {

        public string VrAtDeptID
        {
            get;
            set;
        }


        public string VRId
        {
            get;
            set;
        }


        public int VRNo
        {
            get;
            set;
        }


        public string DeptID
        {
            get;
            set;
        }


        public string DeptCode
        {
            get;
            set;
        }


        public string DepartmentName
        {
            get;
            set;
        }


        public string StaffName
        {
            get;
            set;
        }


        public string Comment
        {
            get;
            set;
        }


        public DateTime? CommentDate
        {
            get;
            set;
        }
    }

    public class VRActionResolutionInputEO 
    {

        public string VRId
        {
            get;
            set;
        }


        public string DeptID
        {
            get;
            set;
        }


        public string UserType
        {
            get;
            set;
        }
    }
}
