using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    // 使用Json路径查询Json Str
    public static SqlString QueryJsonStrWithJsonPath(SqlString input, SqlString path)
    {
        if (input == SqlString.Null || input.Value == null)
        {
            return new SqlString("");
        }
        JObject jsonObj = (JObject)JsonConvert.DeserializeObject(input.Value);
        JToken t = jsonObj.SelectToken(path.Value,false);// 当path不匹配时不要报错
        // 有的Json生产者，在同一个接口中针对不同的状态，会出现缺少字段的情况，这里不能返回错误，需要返回null
        if (t==null)
        {
            return new SqlString(null);
        }
        string re = t.ToString().Trim();
        return new SqlString(re);
    }
};

