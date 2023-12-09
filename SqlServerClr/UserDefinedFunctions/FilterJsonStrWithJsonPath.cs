using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    // 使用JSONPath条件语法筛选json Str
    // 把筛选结果拼接为数组返回
    public static SqlString FilterJsonStrWithJsonPath(SqlString input, SqlString path)
    {
        if (input == SqlString.Null || input.Value == null)
        {
            return new SqlString("");
        }
        JObject jsonObj = (JObject)JsonConvert.DeserializeObject(input.Value);
        IEnumerable<JToken> filterJsons = jsonObj.SelectTokens(path.Value, false);

        JContainer allJsons = new JArray();

        foreach (JToken item in filterJsons)
        {
            allJsons.Add(item);
        }
        string re = allJsons.ToString().Trim();
        return new SqlString(re);
    }

    // 表函数返回的表结构对象
    private class FilterJsonTable
    {
        public SqlInt32 RowId;
        public SqlString Json;

        public FilterJsonTable(SqlInt32 rowId, SqlString json)
        {
            RowId = rowId;
            Json = json;
        }
    }
    public static void FilterJsonTableRow(Object obj, out SqlInt32 rowId, out SqlString json)
    {
        FilterJsonTable jsonTable = (FilterJsonTable)obj;
        rowId = jsonTable.RowId;
        json = jsonTable.Json;
    }
    [SqlFunction(FillRowMethodName = "FilterJsonTableRow")]
    // 使用JSONPath条件语法筛选json Str
    // 把筛选结果组合为函数表返回
    public static IEnumerable FilterJsonTableWithJsonPath(SqlString input, SqlString path)
    {
        ArrayList resultTable = new ArrayList();
        if (input == SqlString.Null || input.Value == null)
        {
            return resultTable;
        }
        JObject jsonObj = (JObject)JsonConvert.DeserializeObject(input.Value);
        IEnumerable<JToken> filterJsons = jsonObj.SelectTokens(path.Value);

        int i = 0;
        foreach (JToken item in filterJsons)
        {
            i++;
            resultTable.Add(new FilterJsonTable(new SqlInt32(i), new SqlString(item.ToString().Trim())));
        }
        return resultTable;
    } 
};

