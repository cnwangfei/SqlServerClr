using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    // Url编码
    public static SqlString UrlEncode(SqlString input)
    {
        if (input == SqlString.Null || input.Value == null)
        {
            return new SqlString("");
        }
        string encoded = Uri.EscapeDataString(input.Value);
        return new SqlString(encoded);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    // Url解码
    public static SqlString UrlDecode(SqlString input)
    {
        if (input == SqlString.Null || input.Value == null)
        {
            return new SqlString("");
        }
        string decoded = Uri.UnescapeDataString(input.Value);
        return new SqlString(decoded);
    }
};

