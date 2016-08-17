using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace QR.IPrism.Enterprise
{
    public class ODPCommandParameter
    {
        /// <summary>
        /// Parameter name in stored procedure
        /// </summary>
        public string ParameterName;

        /// <summary>
        /// Parameter value to stored procedure
        /// </summary>
        public object ParameterValue;

        /// <summary>
        /// Parameter datatype in stored procedure
        /// </summary>
        public OracleDbType ParameterType;

        /// <summary>
        /// Parameter length in stored procedure
        /// </summary>
        public int ParameterLength;

        /// <summary>
        /// Parameter direction in stored procedure
        /// </summary>
        public ParameterDirection ParameterDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODPCommandParameter"/> class.
        /// </summary>
        /// <param name="pname">parameter name</param>
        /// <param name="pvalue">value of the parameter</param>
        /// <param name="pdirection">input or output parameter</param>
        public ODPCommandParameter(string pname, object pvalue, ParameterDirection pdirection)
        {
            this.ParameterDirection = pdirection;
            this.ParameterName = pname;
            this.ParameterValue = pvalue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODPCommandParameter"/> class.
        /// </summary>
        /// <param name="pname">parameter name</param>
        /// <param name="pvalue">value of the parameter</param>
        /// <param name="pdirection">input or output parameter</param>
        /// <param name="ptype">datatype of the parameter</param>
        public ODPCommandParameter(string pname, object pvalue, ParameterDirection pdirection, OracleDbType ptype)
        {
            this.ParameterDirection = pdirection;
            this.ParameterName = pname;
            this.ParameterValue = pvalue;
            this.ParameterType = ptype;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODPCommandParameter"/> class.
        /// </summary>
        /// <param name="pname">parameter name</param>
        /// <param name="pdirection"> input or output parameter</param>
        /// <param name="ptype">datatype of the parameter</param>
        public ODPCommandParameter(string pname, ParameterDirection pdirection, OracleDbType ptype)
        {
            this.ParameterDirection = pdirection;
            this.ParameterName = pname;
            this.ParameterType = ptype;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODPCommandParameter"/> class.
        /// </summary>
        /// <param name="pname">parameter name</param>
        /// <param name="pdirection">input or output parameter</param>
        /// <param name="ptype">datatype of the parameter</param>
        /// <param name="plength">length of the parameter</param>
        public ODPCommandParameter(string pname, ParameterDirection pdirection, OracleDbType ptype, int plength)
        {
            this.ParameterDirection = pdirection;
            this.ParameterName = pname;
            this.ParameterType = ptype;
            this.ParameterLength = plength;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODPCommandParameter"/> class.
        /// </summary>
        /// <param name="pname">parameter name</param>
        /// <param name="pvalue">value of the parameter</param>
        /// <param name="pdirection">input or output parameter</param>
        /// <param name="ptype">datatype of the parameter</param>
        /// <param name="plength">length of the parameter</param>
        public ODPCommandParameter(string pname, object pvalue, ParameterDirection pdirection, OracleDbType ptype, int plength)
        {
            this.ParameterDirection = pdirection;
            this.ParameterName = pname;
            this.ParameterValue = pvalue;
            this.ParameterType = ptype;
            this.ParameterLength = plength;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODPCommandParameter"/> class.
        /// </summary>
        public ODPCommandParameter()
        {
        }
    }
}
