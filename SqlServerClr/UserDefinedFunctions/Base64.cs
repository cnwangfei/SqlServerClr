using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    // Base64加密，采用utf8编码方式加密
    public static SqlString Base64Encode(SqlString input)
    {
        if (input == SqlString.Null || input.Value == null)
        {
            return new SqlString("");
        }
        string base64Str = Base64Helper.Base64Encode(input.Value);
        return new SqlString(base64Str);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    // Base64解密，采用utf8编码方式解密
    public static SqlString Base64Decode(SqlString input)
    {
        if (input == SqlString.Null || input.Value == null)
        {
            return new SqlString("");
        }
        string base64Str = Base64Helper.Base64Decode(input.Value);
        return new SqlString(base64Str);
    }
};

