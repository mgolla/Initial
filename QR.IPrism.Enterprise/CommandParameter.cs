#region "Namespaces"
using System.Data;
#endregion

/// <summary> 
/// Class To Set Values For Command Parameters 
/// </summary> 
public class CommandParameter
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
    public DbType ParameterType;

    /// <summary>
    /// Parameter length in stored procedure
    /// </summary>
    public int ParameterLength;

    /// <summary>
    /// Parameter direction in stored procedure
    /// </summary>
    public ParameterDirection ParameterDirection;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandParameter"/> class.
    /// </summary>
    /// <param name="pname">parameter name</param>
    /// <param name="pvalue">value of the parameter</param>
    /// <param name="pdirection">input or output parameter</param>
    public CommandParameter(string pname, object pvalue, ParameterDirection pdirection)
    {
        this.ParameterDirection = pdirection;
        this.ParameterName = pname;
        this.ParameterValue = pvalue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandParameter"/> class.
    /// </summary>
    /// <param name="pname">parameter name</param>
    /// <param name="pvalue">value of the parameter</param>
    /// <param name="pdirection">input or output parameter</param>
    /// <param name="ptype">datatype of the parameter</param>
    public CommandParameter(string pname, object pvalue, ParameterDirection pdirection, DbType ptype)
    {
        this.ParameterDirection = pdirection;
        this.ParameterName = pname;
        this.ParameterValue = pvalue;
        this.ParameterType = ptype;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandParameter"/> class.
    /// </summary>
    /// <param name="pname">parameter name</param>
    /// <param name="pdirection"> input or output parameter</param>
    /// <param name="ptype">datatype of the parameter</param>
    public CommandParameter(string pname, ParameterDirection pdirection, DbType ptype)
    {
        this.ParameterDirection = pdirection;
        this.ParameterName = pname;
        this.ParameterType = ptype;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandParameter"/> class.
    /// </summary>
    /// <param name="pname">parameter name</param>
    /// <param name="pdirection">input or output parameter</param>
    /// <param name="ptype">datatype of the parameter</param>
    /// <param name="plength">length of the parameter</param>
    public CommandParameter(string pname, ParameterDirection pdirection, DbType ptype, int plength)
    {
        this.ParameterDirection = pdirection;
        this.ParameterName = pname;
        this.ParameterType = ptype;
        this.ParameterLength = plength;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandParameter"/> class.
    /// </summary>
    /// <param name="pname">parameter name</param>
    /// <param name="pvalue">value of the parameter</param>
    /// <param name="pdirection">input or output parameter</param>
    /// <param name="ptype">datatype of the parameter</param>
    /// <param name="plength">length of the parameter</param>
    public CommandParameter(string pname, object pvalue, ParameterDirection pdirection, DbType ptype, int plength)
    {
        this.ParameterDirection = pdirection;
        this.ParameterName = pname;
        this.ParameterValue = pvalue;
        this.ParameterType = ptype;
        this.ParameterLength = plength;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandParameter"/> class.
    /// </summary>
    public CommandParameter()
    {
    }
}
