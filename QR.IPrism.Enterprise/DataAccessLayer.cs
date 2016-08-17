namespace QR.IPrism.Enterprise
{
    #region Namespaces
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.OracleClient;
    using System.Threading.Tasks;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
    using QR.IPrism.Utility;
    #endregion

    /// <summary>
    /// DBFRAMEwork to interact the DB in standart format
    /// </summary>
    public class DBFramework
    {
        #region Variables

        /// <summary>
        /// Variable to get the value of Database
        /// </summary>
        public Database database;

        /// <summary>
        /// Variable to get the value of DbConnection
        /// </summary>
        private DbConnection connection;

        private OracleConnection oracleConnection { get; set; }

        /// <summary>
        /// Variable to get the value of DbTransaction
        /// </summary>
        private DbTransaction transaction;

        private OracleTransaction oracleTransaction { get; set; }

        /// <summary>
        /// Variable to get the value of connectionString
        /// </summary>
        private string connectionString;

        /// <summary>
        /// Variable to get the value of DbCommand
        /// </summary>
        private DbCommand command;

        /// <summary>
        /// Variable to get the value of OracleCommand
        /// </summary>
        private OracleCommand oracleCommand;
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DBFramework"/> class.
        /// </summary>
        /// <param name="connString">value of connection string</param>
        public DBFramework(string connString)
        {
            this.connectionString = connString.Equals(Constants.CONNECTION_STR) ? Constants.CONNECTION_STR : Constants.AIMS_CONNECTION_STR;
            try
            {
                if (this.database == null)
                {
                    //if (connString.Equals(Constants.CONNECTION_STR) || connString.Equals(Constants.AIMS_CONNECTION_STR))
                    //{
                    DatabaseProviderFactory factory = new DatabaseProviderFactory();
                    if (ConfigurationManager.ConnectionStrings[this.connectionString].ToString().IndexOf("User ID") > -1
                        && ConfigurationManager.ConnectionStrings[this.connectionString].ToString().IndexOf("Password") > -1)
                    {
                        this.database = factory.Create(this.connectionString);
                    }
                    else
                    {
                        this.database = factory.Create(Common.Decrypt(ConfigurationManager.ConnectionStrings[this.connectionString].ToString(), ConfigurationManager.AppSettings[this.connectionString].ToString()));
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBFramework"/> class.
        /// </summary>
        //public DBFramework()
        //{
        //    try
        //    {
        //        if (this.database == null)
        //        {
        //            this.database = DatabaseFactory.CreateDatabase();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion
        #region Properties

      
        #endregion
        #region Class Members

        /// <summary> 
        /// To get the ConnectionString from Config  
        /// </summary>
        /// <param name="connString">Connection string key name</param>
        /// <returns>Returns Connect string value from config</returns>
        /// <remarks>
        /// </remarks> 
        public static string GetConnectionString(string connString)
        {
            string connectionString = string.Empty;
            ConnectionStringSettings connectionSettings = ConfigurationManager.ConnectionStrings[connString];
            //if (connString.Equals(Constants.CONNECTION_STR))
            //{
            if (ConfigurationManager.ConnectionStrings[connString].ToString().IndexOf("User ID") > -1
                            && ConfigurationManager.ConnectionStrings[connString].ToString().IndexOf("Password") > -1)
            {
                connectionString = connectionSettings.ConnectionString;
            }
            else
            {
                connectionString = Common.Decrypt(ConfigurationManager.ConnectionStrings[connString].ConnectionString, ConfigurationManager.AppSettings[Constants.IPRISM_CNSP].ToString());
            }
            //}
            return connectionString;
        }

        /// <summary>
        ///  To get the Database provider name    
        /// </summary>
        /// <returns>The Db name,Username and Password</returns>
        public string GetDBProviderName()
        {
            string connectionString = string.Empty;
            ConnectionStringSettings connectionSettings = ConfigurationManager.ConnectionStrings[Constants.IPRISM_CNSP];
            connectionString = connectionSettings.ProviderName;
            return connectionString;
        }

        /// <summary> 
        /// To get the Database name        
        /// </summary>
        /// <returns>Returns Database Name</returns>
        /// <remarks>
        /// </remarks>  
        public string GetDBName()
        {
            string dbname = string.Empty;
            if (this.connection == null || this.connection.State == ConnectionState.Closed)
            {
                this.connection = this.database.CreateConnection();
                dbname = this.connection.DataSource;
                this.connection.Close();
            }
            else
            {
                dbname = this.connection.DataSource;
            }

            return dbname;
        }

        /// <summary> 
        /// Create the Db connection        
        /// </summary>
        /// <returns>Returns DbConnection</returns>
        /// <remarks>
        /// </remarks> 
        public DbConnection CreateConnection()
        {
            if (this.connection == null || this.connection.State == ConnectionState.Closed)
            {
                this.connection = this.database.CreateConnection();
                this.connection.ConnectionString = GetConnectionString(this.connectionString);
                this.connection.Open();
            }

            return this.connection;
        }

        /// <summary> 
        /// Create the Db connection        
        /// </summary>
        /// <returns>Returns DbConnection</returns>
        /// <remarks>
        /// </remarks> 
        public DbConnection CreateOracleConnection()
        {
            if (this.oracleConnection == null || this.oracleConnection.State == ConnectionState.Closed)
            {
                this.oracleConnection = new OracleConnection(GetConnectionString(this.connectionString));
                this.oracleConnection.Open();
            }

            return this.oracleConnection;
        }

        /// <summary>Create Db connection   
        /// </summary>
        /// <param name="connectionString">Connection string key Name</param>
        /// <returns>Returns DataBase Connection</returns>
        /// <remarks>
        /// </remarks> 
        public DbConnection CreateConnection(string connectionString)
        {
            if (this.connection == null || this.connection.State == ConnectionState.Closed)
            {
                this.connection = this.database.CreateConnection();
                this.connection.ConnectionString = connectionString;
                this.connection.Open();
            }

            return this.connection;
        }

        /// <summary>Create Db connection   
        /// </summary>
        /// <param name="connectionString">Connection string key Name</param>
        /// <returns>Returns DataBase Connection</returns>
        /// <remarks>
        /// </remarks> 
        public OracleConnection CreateOracleConnection(string connectionString)
        {
            if (this.oracleConnection == null || this.oracleConnection.State == ConnectionState.Closed)
            {
                this.oracleConnection = new OracleConnection(connectionString);
                this.oracleConnection.Open();
            }

            return this.oracleConnection;
        }
        /// <summary>
        /// Create the transaction object        
        /// </summary>
        /// <returns>Returns DbTransaction object of the current connection</returns>
        /// <remarks>
        /// </remarks> 
        public DbTransaction BeginTransaction()
        {
            if (this.connection != null && this.connection.State == ConnectionState.Open)
            {
                this.transaction = this.connection.BeginTransaction();
            }

            return this.transaction;
        }

        /// <summary>
        /// Rollback the operation in the transaction object          
        /// </summary>
        public void RollBackTransaction()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
            }
        }

        /// <summary> 
        /// Commit the set of operation in the transaction object
        /// </summary>
        public void CommitTransaction()
        {
            if (this.transaction != null)
            {
                this.transaction.Commit();
            }
        }


        /// <summary>
        /// Create the transaction object        
        /// </summary>
        /// <returns>Returns DbTransaction object of the current connection</returns>
        /// <remarks>
        /// </remarks> 
        public OracleTransaction BeginOracleTransaction()
        {
            if (this.oracleConnection != null && this.oracleConnection.State == ConnectionState.Open)
            {
                this.oracleTransaction = this.oracleConnection.BeginTransaction();
            }

            return this.oracleTransaction;
        }

        /// <summary>
        /// Rollback the operation in the transaction object          
        /// </summary>
        public void RollBackOracleTransaction()
        {
            if (this.oracleTransaction != null)
            {
                this.oracleTransaction.Rollback();
            }
        }

        /// <summary> 
        /// Commit the set of operation in the transaction object
        /// </summary>
        public void CommitOracleTransaction()
        {
            if (this.oracleTransaction != null)
            {
                this.oracleTransaction.Commit();
            }
        }

        /// <summary>
        /// Close the DB connection   
        /// </summary>
        public void CloseOracleConnection()
        {
            if (this.oracleConnection != null)
            {
                if (this.oracleConnection.State == ConnectionState.Open)
                {
                    this.oracleConnection.Close();
                }

                this.oracleConnection.Dispose();
                this.oracleConnection = null;
            }
            GC.Collect();
        }
        /// <summary>
        /// Close the DB connection   
        /// </summary>
        public void CloseConnection()
        {
            if (this.connection != null)
            {
                if (this.connection.State == ConnectionState.Open)
                {
                    this.connection.Close();
                }

                this.connection.Dispose();
                this.connection = null;
            }
            GC.Collect();
        }

        /// <summary>
        /// Create the command object in Db connection
        /// </summary>
        /// <param name="connection">Db connection object</param>
        /// <returns>Return Dbcommand object</returns>
        public DbCommand CreateCommand(DbConnection connection)
        {
            this.command = connection.CreateCommand();
            return this.command;
        }
        public OracleCommand CreateOracleCommand(OracleConnection connection)
        {
            this.oracleCommand = connection.CreateCommand();
            return this.oracleCommand;
        }
        /// <summary>
        /// Dispose the command object
        /// </summary>
        public void CloseCommand()
        {
            if (this.command != null)
            {
                this.command.Dispose();
                this.command = null;
            }
        }
        /// <summary>
        /// Dispose the command object
        /// </summary>
        public void CloseOracleCommand()
        {
            if (this.oracleCommand != null)
            {
                this.oracleCommand.Dispose();
                this.oracleCommand = null;
            }
        }

        /// <summary>
        /// Load the CommandParameter objects into DbCommand parameter.
        /// </summary>
        /// <param name="dbcommand">Name of the Dbcommand object as ref</param>
        /// <param name="parameters">Name of theCommandParameter List</param>
        public void LoadParameters(ref DbCommand dbcommand, ref List<CommandParameter> parameters)
        {
            if (parameters != null)
            {
                for (int i = 0; i <= parameters.Count - 1; i++)
                {
                    IDbDataParameter param = dbcommand.CreateParameter();
                    CommandParameter par = default(CommandParameter);
                    par = parameters[i];

                    param.ParameterName = par.ParameterName;
                    param.DbType = par.ParameterType;
                    if (par.ParameterLength > 0)
                    {
                        param.Size = par.ParameterLength;
                    }

                    dbcommand.Parameters.Add(param);
                    dbcommand.Parameters[par.ParameterName].Value = par.ParameterValue;
                    dbcommand.Parameters[par.ParameterName].Direction = par.ParameterDirection;
                }
            }
        }
        /// <summary>
        /// Load the CommandParameter objects into DbCommand parameter.
        /// </summary>
        /// <param name="dbcommand">Name of the Dbcommand object as ref</param>
        /// <param name="parameters">Name of theCommandParameter List</param>
        public void LoadParameters(ref OracleCommand dbcommand, ref List<OracleCommandParameter> parameters)
        {
            if (parameters != null)
            {
                for (int i = 0; i <= parameters.Count - 1; i++)
                {
                    OracleParameter param = new OracleParameter();
                    OracleCommandParameter par = default(OracleCommandParameter);
                    par = parameters[i];

                    param.ParameterName = par.ParameterName;
                    param.OracleType = par.ParameterType;
                    if (par.ParameterLength > 0)
                    {
                        param.Size = par.ParameterLength;
                    }
                    dbcommand.Parameters.Add(param);
                    dbcommand.Parameters[par.ParameterName].Value = par.ParameterValue;
                    dbcommand.Parameters[par.ParameterName].Direction = par.ParameterDirection;
                }
            }
        }

        /// <summary>
        /// Load the CommandParameter objects into DbCommand parameter.
        /// </summary>
        /// <param name="dbcommand">Name of the Dbcommand object as ref</param>
        /// <param name="parameters">Name of theCommandParameter List</param>
        public void LoadCursorParameters(ref DbCommand dbcommand, ref List<CommandCursorParameter> parameters)
        {
            if (parameters != null)
            {
                foreach (var par in parameters)
                {
                    dbcommand.Parameters.Add(new OracleParameter(par.ParameterName, OracleType.Cursor, 0, ParameterDirection.Output, true,
                                                                                               0, 0, String.Empty, DataRowVersion.Default, Convert.DBNull));
                }
            }
        }

        /// <summary>
        /// Add the Parameter objects into CommandParameter.
        /// </summary>
        /// <param name="parName">Name of the Dbcommand object as ref</param>
        /// <param name="parValue">parameter Value object</param>
        /// <param name="parType">DB TYPE of the parameter</param>
        /// <returns>Return the CommandParameter object</returns>
        public CommandParameter AddParameter(string parName, object parValue, DbType parType)
        {
            CommandParameter param = new CommandParameter();
            param.ParameterName = parName;
            param.ParameterType = parType;
            if (parValue == null && parType == DbType.String)
            {
                param.ParameterValue = string.Empty;
            }
            else if (parValue == null && parType == DbType.DateTime)
            {
                param.ParameterValue = System.DBNull.Value;
            }
            else
            {
                param.ParameterValue = parValue;
            }

            param.ParameterLength = 0;
            param.ParameterDirection = ParameterDirection.Input;
            return param;
        }

        /// <summary>
        /// Add the Parameter objects into CommandParameter.
        /// </summary>
        /// <param name="parName">Name of the Dbcommand object as ref</param>
        /// <param name="parValue">parameter Value object </param>
        /// <param name="parType">DB TYPE of the parameter </param>
        /// <param name="parSize">size of the parameter </param>
        /// <param name="parDirection">direction of the parameter</param>
        /// <returns>Return the CommandParameter object</returns>
        public CommandParameter AddParameter(string parName, object parValue, DbType parType, int parSize, ParameterDirection parDirection)
        {
            CommandParameter param = new CommandParameter();
            param.ParameterName = parName;
            param.ParameterType = parType;
            if (parValue == null && parType == DbType.String)
            {
                param.ParameterValue = string.Empty;
            }
            else
            {
                param.ParameterValue = parValue;
            }

            param.ParameterLength = parSize;
            param.ParameterDirection = parDirection;
            return param;
        }

        /// <summary>Fetch the set of records through Querystring
        /// </summary>
        /// <param name="queryString">Db Query</param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public IDataReader ExecuteReader(string queryString, string refCursorName)
        {
            try
            {
                this.connection = this.CreateConnection();
                if (this.command == null)
                {
                    if (!string.IsNullOrEmpty(refCursorName))
                    {
                        this.command = this.database.GetStoredProcCommand(queryString, refCursorName);
                        this.command.Connection = this.connection;
                    }
                    else
                    {
                        this.command = this.CreateCommand(this.connection);
                    }
                }

                DbCommand dbcommand = this.command;
                dbcommand.CommandType = CommandType.Text;
                dbcommand.CommandText = queryString;
                if (this.transaction != null)
                {
                    dbcommand.Transaction = this.transaction;
                }

                return dbcommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Fetch the set of records through Querystring
        /// </summary>
        /// <param name="queryString">Db Query</param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<IDataReader> ExecuteReaderAsync(string queryString, string refCursorName)
        {
            try
            {
                this.connection = this.CreateConnection();
                if (this.command == null)
                {
                    if (!string.IsNullOrEmpty(refCursorName))
                    {
                        this.command = this.database.GetStoredProcCommand(queryString, refCursorName);
                        this.command.Connection = this.connection;
                    }
                    else
                    {
                        this.command = this.CreateCommand(this.connection);
                    }
                }

                DbCommand dbcommand = this.command;
                dbcommand.CommandType = CommandType.Text;
                dbcommand.CommandText = queryString;
                if (this.transaction != null)
                {
                    dbcommand.Transaction = this.transaction;
                }

                return await dbcommand.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Fetch the set of records through Querystring
        /// </summary>
        /// <param name="queryString">Db Query</param>
        /// <param name="conn">Name of the Db connection</param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public IDataReader ExecuteReader(string queryString, ref DbConnection conn, string refCursorName)
        {
            this.connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    if (!string.IsNullOrEmpty(refCursorName))
                    {
                        this.command = this.database.GetStoredProcCommand(queryString, refCursorName);
                        this.command.Connection = this.connection;
                    }
                    else
                    {
                        this.command = this.CreateCommand(this.connection);
                    }
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                conn = this.connection;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }


        /// <summary>Fetch the set of records through Querystring
        /// </summary>
        /// <param name="queryString">Db Query</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects</param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public IDataReader ExecuteReader(string queryString, List<CommandParameter> cmdparameters, string refCursorName)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    if (!string.IsNullOrEmpty(refCursorName))
                    {
                        this.command = this.database.GetStoredProcCommand(queryString, refCursorName);
                        this.command.Connection = this.connection;
                    }
                    else
                    {
                        this.command = this.CreateCommand(this.connection);
                    }
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }


        /// <summary>Fetch the set of records through Querystring
        /// </summary>
        /// <param name="queryString">Db Query</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects</param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<IDataReader> ExecuteReaderAsync(string queryString, List<CommandParameter> cmdparameters, string refCursorName)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    if (!string.IsNullOrEmpty(refCursorName))
                    {
                        this.command = this.database.GetStoredProcCommand(queryString, refCursorName);
                        this.command.Connection = this.connection;
                    }
                    else
                    {
                        this.command = this.CreateCommand(this.connection);
                    }
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);
                return await command.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }


        /// <summary>Fetch the set of records through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the StoredProcedure Name</param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public IDataReader ExecuteSPReader(string queryString, string refCursorName)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    if (!string.IsNullOrEmpty(refCursorName))
                    {
                        this.command = this.database.GetStoredProcCommand(queryString, refCursorName);
                        this.command.Connection = this.connection;
                    }
                    else
                    {
                        this.command = this.CreateCommand(this.connection);
                    }
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Fetch the set of records through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the StoredProcedure Name</param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<IDataReader> ExecuteSPReaderAsync(string queryString, string refCursorName)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    if (!string.IsNullOrEmpty(refCursorName))
                    {
                        this.command = this.database.GetStoredProcCommand(queryString, refCursorName);
                        this.command.Connection = this.connection;
                    }
                    else
                    {
                        this.command = this.CreateCommand(this.connection);
                    }
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                return await command.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Fetch the set of records through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the StoredProcedure Name</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public IDataReader ExecuteSPReader(string queryString, List<CommandParameter> cmdparameters, string refCursorName)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }
                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }
                this.LoadParameters(ref command, ref cmdparameters);
                command.Parameters.Add(new OracleParameter(refCursorName, OracleType.Cursor, 0, ParameterDirection.Output, true,
                                                                                                0, 0, String.Empty, DataRowVersion.Default, Convert.DBNull));
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Fetch the set of records through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the StoredProcedure Name</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<IDataReader> ExecuteSPReaderAsync(string queryString, List<CommandParameter> cmdparameters, string refCursorName)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }
                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);
                command.Parameters.Add(new OracleParameter(refCursorName, OracleType.Cursor, 0, ParameterDirection.Output, true,
                                                                                                0, 0, String.Empty, DataRowVersion.Default, Convert.DBNull));
                return await command.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Fetch the set of records through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the StoredProcedure Name</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<IDataReader> ExecuteSPReaderOracleAsync(string queryString, List<OracleCommandParameter> cmdparameters, string refCursorName)
        {
            DbConnection connection = this.CreateOracleConnection();
            try
            {
                if (this.oracleCommand == null)
                {
                    this.oracleCommand = this.CreateOracleCommand(this.oracleConnection);
                }
                OracleCommand command = this.oracleCommand;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.oracleTransaction != null)
                {
                    command.Transaction = this.oracleTransaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);
                command.Parameters.Add(new OracleParameter(refCursorName, OracleType.Cursor, 0, ParameterDirection.Output, true,
                                                                                                0, 0, String.Empty, DataRowVersion.Default, Convert.DBNull));
                return await command.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseOracleCommand();

            }
        }
        /// <summary>Fetch the set of records through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the StoredProcedure Name</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<IDataReader> ExecuteSPReaderOracleAsync(string queryString, List<OracleCommandParameter> cmdparameters)
        {
            DbConnection connection = this.CreateOracleConnection();
            try
            {
                if (this.oracleCommand == null)
                {
                    this.oracleCommand = this.CreateOracleCommand(this.oracleConnection);
                }
                OracleCommand command = this.oracleCommand;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.oracleTransaction != null)
                {
                    command.Transaction = this.oracleTransaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);
                return await command.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseOracleCommand();

            }
        }

        /// <summary>Fetch the set of records through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the StoredProcedure Name</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public IDataReader ExecuteSPReaderOracle(string queryString, List<OracleCommandParameter> cmdparameters)
        {
            DbConnection connection = this.CreateOracleConnection();
            try
            {
                if (this.oracleCommand == null)
                {
                    this.oracleCommand = this.CreateOracleCommand(this.oracleConnection);
                }
                OracleCommand command = this.oracleCommand;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.oracleTransaction != null)
                {
                    command.Transaction = this.oracleTransaction;
                }
                this.LoadParameters(ref command, ref cmdparameters);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseOracleCommand();

            }
        }

        /// <summary>Fetch the set of records through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the StoredProcedure Name</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public IDataReader ExecuteSPReader(string queryString, List<CommandParameter> cmdParameters, List<CommandCursorParameter> cmdCursorParameters)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }
                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }
                this.LoadParameters(ref command, ref cmdParameters);
                this.LoadCursorParameters(ref command, ref cmdCursorParameters);

                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }
        /// <summary>Fetch the set of records through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the StoredProcedure Name</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<IDataReader> ExecuteSPReaderAsync(string queryString, List<CommandParameter> cmdParameters, List<CommandCursorParameter> cmdCursorParameters)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }
                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }
                this.LoadParameters(ref command, ref cmdParameters);
                this.LoadCursorParameters(ref command, ref cmdCursorParameters);
                return await command.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }


        /// <summary>Update the set of records through Query
        /// </summary>
        /// <param name="queryString">DB Query text</param>
        /// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        /// <remarks>
        /// </remarks> 
        public int ExecuteNonQuery(string queryString)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = this.CreateConnection();
                }

                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }
        /// <summary>Update the set of records asynchronously through Query 
        /// </summary>
        /// <param name="queryString">DB Query text</param>
        /// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<int> ExecuteNonQueryAsyc(string queryString)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = this.CreateConnection();
                }

                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Update the set of records through Query with parameters
        /// </summary>
        /// <param name="queryString">DB Query text</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        /// <remarks>
        /// </remarks> 
        public int ExecuteNonQuery(string queryString, List<CommandParameter> cmdparameters)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = this.CreateConnection();
                }

                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);

                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Update the set of records asynchronously through Query with parameters
        /// </summary>
        /// <param name="queryString">DB Query text</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<int> ExecuteNonQueryAsync(string queryString, List<CommandParameter> cmdparameters)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = this.CreateConnection();
                }

                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Update the set of records through StoredProcedure
        /// </summary>
        /// <param name="queryString">DB Query text</param>
        /// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        /// <remarks>
        /// </remarks> 
        public int ExecuteSPNonQuery(string queryString)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = this.CreateConnection();
                }

                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Update the set of records asynchronously through StoredProcedure
        /// </summary>
        /// <param name="queryString">DB Query text</param>
        /// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<int> ExecuteSPNonQueryAsync(string queryString)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = this.CreateConnection();
                }

                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }
        /// <summary>Update the set of records through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the Stored Procedure</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        /// <remarks>
        /// </remarks> 
        public int ExecuteSPNonQuery(string queryString, List<CommandParameter> cmdparameters)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = this.CreateConnection();
                }

                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Update the set of records asynchronously through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the Stored Procedure</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<int> ExecuteSPNonQueryAsync(string queryString, List<CommandParameter> cmdparameters)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = this.CreateConnection();
                }

                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Update the set of records through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the Stored Procedure</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        /// <remarks>
        /// </remarks> 
        public DbCommand ExecuteSPNonQueryParm(string queryString, List<CommandParameter> cmdparameters)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = this.CreateConnection();
                }

                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);
                command.ExecuteNonQuery();
                return command;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        public DbCommand ExecuteSPNonQueryParmTest(string queryString, List<CommandParameter> cmdparameters)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = this.CreateConnection();
                }

                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }


                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;

                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                foreach (var para in cmdparameters)
                {
                    this.database.AddInParameter(command, para.ParameterName, para.ParameterType, para.ParameterValue);
                }
                this.database.ExecuteNonQuery(command);
                //this.LoadParameters(ref command, ref cmdparameters);
                //command.ExecuteNonQuery();
                return command;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }
        /// <summary>Update the set of records asynchronously through StoredProcedure
        /// </summary>
        /// <param name="queryString">Name of the Stored Procedure</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<DbCommand> ExecuteSPNonQueryParmAsync(string queryString, List<CommandParameter> cmdparameters)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = this.CreateConnection();
                }

                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);
                await command.ExecuteNonQueryAsync();
                return command;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }
        ///// <summary>Update the set of records asynchronously through StoredProcedure
        ///// </summary>
        ///// <param name="queryString">Name of the Stored Procedure</param>
        ///// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        ///// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        ///// <remarks>
        ///// </remarks> 
        //public async void ExecuteFileUploadsync(string queryString, List<OracleCommandParameter> cmdparameters)
        //{
        //    Oracle.DataAccess.Client.OracleConnection oracleConnection = null;
        //    Oracle.DataAccess.Client.OracleCommand oracleCommand = null;

        //    try
        //    {
        //        //Initialize Oracle Server Connection
        //        oracleConnection = new Oracle.DataAccess.Client.OracleConnection(GetConnectionString(this.connectionString));

        //        //Initialize OracleCommand object for insert.
        //        oracleCommand = new Oracle.DataAccess.Client.OracleCommand();

        //        oracleCommand.CommandText = queryString;
        //        oracleCommand.CommandType = CommandType.StoredProcedure;
        //        oracleCommand.Connection = oracleConnection;
        //        //We are passing Name and Blob byte data as Oracle parameters.
        //        this.LoadParameters(ref oracleCommand, ref cmdparameters);

        //        //Open connection and execute insert query.
        //        oracleConnection.Open();

        //        await oracleCommand.ExecuteNonQueryAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        oracleConnection.Close();
        //        oracleConnection.Dispose();
        //        oracleCommand.Dispose();
        //        oracleCommand = null;
        //    }
        //}

        /// <summary>Execute the aggregate commands through Query
        /// </summary>
        /// <param name="queryString">DB Query text</param>
        /// <returns>Return the object. </returns>
        /// <remarks>
        /// </remarks> 
        public object ExecuteScalar(string queryString)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Execute the aggregate commands asynchronously through Query
        /// </summary>
        /// <param name="queryString">DB Query text</param>
        /// <returns>Return the object. </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<object> ExecuteScalarAsync(string queryString)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                return await command.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Execute the aggregate commands through Query
        /// </summary>
        /// <param name="queryString">DB Query text</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <returns>Return the object. </returns>
        /// <remarks>
        /// </remarks> 
        public object ExecuteScalar(string queryString, List<CommandParameter> cmdparameters)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);
                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Execute the aggregate commands asynchronously through Query
        /// </summary>
        /// <param name="queryString">DB Query text</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <returns>Return the object. </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<object> ExecuteScalarAsync(string queryString, List<CommandParameter> cmdparameters)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);
                return await command.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Execute the aggregate commands through Query
        /// </summary>
        /// <param name="queryString">Name of the Stored Procedure</param>
        /// <returns>Return the object. </returns>
        /// <remarks>
        /// </remarks> 
        public object ExecuteSPScalar(string queryString)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                command.ExecuteScalar();
                return command;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Execute the aggregate commands asynchronously through Query
        /// </summary>
        /// <param name="queryString">Name of the Stored Procedure</param>
        /// <returns>Return the object. </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<object> ExecuteSPScalarAsync(string queryString)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                await command.ExecuteScalarAsync();
                return command;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Execute the aggregate commands through Query
        /// </summary>
        /// <param name="queryString">Name of the Stored Procedure</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <returns>Return the object. </returns>
        /// <remarks>
        /// </remarks> 
        public object ExecuteSPScalar(string queryString, List<CommandParameter> cmdparameters)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);
                command.ExecuteScalar();
                return command;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Execute the aggregate commands through Query
        /// </summary>
        /// <param name="queryString">Name of the Stored Procedure</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <returns>Return the object. </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<object> ExecuteSPScalarAsync(string queryString, List<CommandParameter> cmdparameters)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                this.LoadParameters(ref command, ref cmdparameters);
                await command.ExecuteScalarAsync();
                return command;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }


        /// <summary>Fetch the set of records through Inline query
        /// </summary>
        /// <param name="queryString">Name of the StoredProcedure Name</param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public IDataReader ExecuteQuerySPReader(string queryString)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }


        /// <summary>Fetch the set of records asynchronously through Inline query
        /// </summary>
        /// <param name="queryString">Name of the StoredProcedure Name</param>
        /// <param name="refCursorName">Name of the Reference Cursor, if it is Oracle StoredProcedure. Otherwise empty</param>
        /// <returns>Return the Idatareader object </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<IDataReader> ExecuteQuerySPReaderAsync(string queryString)
        {
            DbConnection connection = this.CreateConnection();
            try
            {
                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                return await command.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }


        /// <summary>Update the set of records through Inline Query
        /// </summary>
        /// <param name="queryString">Name of the Stored Procedure</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        /// <remarks>
        /// </remarks> 
        public int ExecuteQuerySPNonQuery(string queryString)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = this.CreateConnection();
                }

                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }


                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        /// <summary>Update the set of records through Inline Query
        /// </summary>
        /// <param name="queryString">Name of the Stored Procedure</param>
        /// <param name="cmdparameters">Name of the List of CommandParameter objects </param>
        /// <returns>Return the value of 1 if success. Otherwise returns 0. </returns>
        /// <remarks>
        /// </remarks> 
        public async Task<int> ExecuteQuerySPNonQueryAsync(string queryString)
        {
            try
            {
                if (this.connection == null)
                {
                    this.connection = this.CreateConnection();
                }

                if (this.command == null)
                {
                    this.command = this.CreateCommand(this.connection);
                }

                DbCommand command = this.command;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }


                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.command.Dispose();
                this.command = null;
            }
        }

        #endregion
    }
}