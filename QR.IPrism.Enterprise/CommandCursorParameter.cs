#region "Namespaces"
using System.Data;
using System.Data.OracleClient;
#endregion

/// <summary> 
/// Class To Set Values For Command Parameters 
/// </summary> 
public class CommandCursorParameter
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
    public OracleType ParameterType;

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
    /// <param name="ptype">datatype of the parameter</param>
    public CommandCursorParameter(string pname, ParameterDirection pdirection, OracleType ptype)
    {
        this.ParameterDirection = pdirection;
        this.ParameterName = pname;
        this.ParameterType = ptype;
    }
}
