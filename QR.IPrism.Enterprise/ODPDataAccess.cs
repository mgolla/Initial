using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace QR.IPrism.Enterprise
{
    public class ODPDataAccess
    {

        private OracleConnection OdpConnection { get; set; }
        private OracleTransaction OdpTransaction { get; set; }
        private OracleCommand OdpCommand;
        public ODPDataAccess(string conectionStr)
        {
            try
            {
                if (!string.IsNullOrEmpty(conectionStr))
                {
                    this.OdpConnection = new OracleConnection(ConfigurationManager.ConnectionStrings[conectionStr].ConnectionString);
                    this.OdpConnection.Open();
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Close the DB connection   
        /// </summary>
        public void CloseConnection()
        {
            this.OdpCommand.Dispose();
            this.OdpCommand = null;
            if (this.OdpConnection != null)
            {
                if (this.OdpConnection.State == ConnectionState.Open)
                {
                    this.OdpConnection.Close();
                }

                this.OdpConnection.Dispose();
                this.OdpConnection = null;
            }
            GC.Collect();
        }
        #region Reader
        /// <summary>Fetch the set of records through Querystring
        /// </summary>
        /// <param name="queryString">Db Query</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects</param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public IDataReader ExecuteReader(string queryString, List<ODPCommandParameter> cmdparameters)
        {
            try
            {
                if (this.OdpCommand != null)
                    this.OdpCommand = null;

                this.OdpCommand = new OracleCommand();
                this.OdpCommand.Connection = this.OdpConnection;
                this.OdpCommand.CommandType = CommandType.StoredProcedure;

                this.OdpCommand.CommandText = queryString;
                this.LoadParameters(cmdparameters);
                return this.OdpCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>Fetch the set of records through Querystring
        /// </summary>
        /// <param name="queryString">Db Query</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<IDataReader> ExecuteSPReaderAsync(string queryString, List<ODPCommandParameter> cmdparameters)
        {
            try
            {
                if (this.OdpCommand != null)
                    this.OdpCommand = null;

                this.OdpCommand = new OracleCommand();
                this.OdpCommand.Connection = this.OdpConnection;
                this.OdpCommand.CommandType = CommandType.StoredProcedure;

                this.OdpCommand.CommandText = queryString;
                this.LoadParameters(cmdparameters);
                return await this.OdpCommand.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>Update the set of records asynchronously through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the Stored Procedure</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<OracleCommand> ExecuteSPNonQueryParmAsync(string queryString, List<ODPCommandParameter> cmdparameters)
        {
            try
            {
                if (this.OdpCommand != null)
                    this.OdpCommand = null;

                this.OdpCommand = new OracleCommand();
                this.OdpCommand.Connection = this.OdpConnection;
                this.OdpCommand.CommandType = CommandType.StoredProcedure;

                this.OdpCommand.CommandText = queryString;
                this.LoadParameters(cmdparameters);
                await this.OdpCommand.ExecuteNonQueryAsync();
                return this.OdpCommand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Execute
        public int ExecuteSPNonQuery(string queryString, List<ODPCommandParameter> cmdparameters)
        {
            try
            {
                if (this.OdpCommand != null)
                    this.OdpCommand = null;

                this.OdpCommand = new OracleCommand();
                this.OdpCommand.Connection = this.OdpConnection;
                this.OdpCommand.CommandType = CommandType.StoredProcedure;

                this.OdpCommand.CommandText = queryString;
                this.LoadParameters(cmdparameters);
                return this.OdpCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Load the CommandParameter objects into DbCommand parameter.
        /// </summary>
        /// <param name="dbcommand">Name of the Dbcommand object as ref</param>
        /// <param name="parameters">Name of theCommandParameter List</param>
        public void LoadParameters(List<ODPCommandParameter> parameters)
        {
            if (parameters != null)
            {
                foreach (ODPCommandParameter par in parameters)
                {
                    OracleParameter param = new OracleParameter();
                    param.ParameterName = par.ParameterName;
                    param.Value = par.ParameterValue;
                    param.OracleDbType = par.ParameterType;
                    param.Direction = par.ParameterDirection;
                    if (par.ParameterLength > 0)
                    {
                        param.Size = par.ParameterLength;
                    }
                    this.OdpCommand.Parameters.Add(param);
                }
            }
        }
    }
}
