using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Security
{
    public enum ApplicationTypes
    {
        Secured = 1,
        Open = 2
    };
    public static class SecuredConstants
    {
        public const string UserIdKey = "X_UEI_AMN_PUF_M";
        public const string AdditionalDataKey = "X_AMN_BHP_DWR_Z";
        public const string ClientKey = "X_MNA_SFN_CBK";
        public const string TokenIdKey = "X_MNA_IPMU_CBK";
    }
    public static class ScConstants
    {
        public const string CordovaCookieName = "__i_p_m_c";
        public const string UserIdKey = "X_UEI_AMN_PUF_M";
        public const string AdditionalDataKey = "X_AMN_BHP_DWR_Z";
        public const string UserNotFoundException = "User details are not provided by the client!";
        public const string AuthContext = "___i_p_m_p";
        public const string AuthUserContext = "___i_p_m_p_u_c";
        public const string AuthContextPurpose = "AuthenticationPersistance";
        public const string ClientKey = "xeFBJ3YGERMq2tggwqqMTcNF8pDbX3cUQYl3BD6SR6Bv9uzeMS+bnA==";
        public const string ClientId = "IPM_CLIENT";
    }
}
