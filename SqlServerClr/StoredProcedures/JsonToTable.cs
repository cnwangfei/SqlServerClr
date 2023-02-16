using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    // result:0.失败 1.成功 msg:具体消息
    public static void JsonToTable(SqlString inJsonStr, SqlString inClumnsStr, out SqlBoolean outResult, out SqlString outMsg)
    {
        try
        {

            // 检查入参
            if (inJsonStr.IsNull || inJsonStr.Value == "")
            {
                outResult = SqlBoolean.False;
                outMsg = "err:入参inJsonStr不能为空";
                SqlContext.Pipe.Send(outMsg.Value);
                return;
            }
            string[] columnArray = null;
            // 按入参inClumnsStr查询列，inClumnsStr为空则查询所有列
            if (inClumnsStr.IsNull || inClumnsStr.Value == "")
            {
                columnArray =  new string[0] { };
            }
            else
            {
                string nameStr;
                nameStr = inClumnsStr.Value;
                // 去除字符串中的空格，回车，换行符，制表符
                nameStr = nameStr.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                columnArray = nameStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            /*
            if (columns == null && columns == SqlString.Null && columns.Value != null)
            {
                columnArray = columns.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            */
            // 反序列化json
            Object jsonObj;
            try
            {
                jsonObj = JsonConvert.DeserializeObject(inJsonStr.Value);
            }
            catch (Exception e)
            {
                outResult = SqlBoolean.False;
                outMsg = String.Format("err:反序列化失败,{0}", e.ToString());
                SqlContext.Pipe.Send(outMsg.Value);
                return;
            }
            JArray jArray = jsonObj as JArray;

            JObject jObject = jsonObj as JObject;
            if (jArray == null)
            {
                if (jObject == null)
                {
                    outResult = SqlBoolean.False;
                    outMsg = "err:入参inJsonStr不是标准Json格式";
                    SqlContext.Pipe.Send(outMsg.Value);
                    return;
                }
                else
                    jArray = new JArray(jObject);

            }
            else
                jObject = jArray[0] as JObject;

            SqlDataRecord rec = null;
            // 列名
            foreach (JObject item in jArray)
            {
                IEnumerable<JProperty> jPropertys = jObject.Properties();
                List<SqlMetaData> sqlMetaDatas = new List<SqlMetaData>();
                if (columnArray.Length > 0)// 有参数columns的，用columns作为列名
                {
                    foreach (string col in columnArray)
                    {
                        bool flag = false;// 检查有没有指定的列名
                        foreach (JProperty jProperty in jPropertys)
                        {
                            if (col == jProperty.Name)
                            {
                                sqlMetaDatas.Add(new SqlMetaData(jProperty.Name, SqlDbType.NVarChar, 4000));
                                flag = true;
                            }
                        }
                        if (!flag)
                        {
                            outResult = SqlBoolean.False;
                            outMsg = String.Format("err:Json中无[{0}]列", col);
                            SqlContext.Pipe.Send(outMsg.Value);
                            return;
                            /*有的Json生产者，在同一个接口中针对不同的状态，会出现缺少字段的情况，这里不能返回错误，需要返回null
                            sqlMetaDatas.Add(new SqlMetaData(col, SqlDbType.NVarChar, 4000));
                            */
                        }
                    }
                }
                else // 参数columns为空的，显示所有json列
                {
                    foreach (JProperty jProperty in jPropertys)
                    {
                        sqlMetaDatas.Add(new SqlMetaData(jProperty.Name, SqlDbType.NVarChar, 4000));
                    }
                }
                /*
                foreach (JProperty jProperty in jPropertys)
                {
                    //区分大小写
                    if (columnArray == null || columnArray.Length == 0 || (columnArray.Length > 0 && Array.IndexOf(columnArray, jProperty.Name) >= 0))
                    {
                        sqlMetaDatas.Add(new SqlMetaData(jProperty.Name, SqlDbType.NVarChar, 4000));
                    }
                }
                */
                rec = new SqlDataRecord(sqlMetaDatas.ToArray());
                break;
            }
            // 列数据
            SqlContext.Pipe.SendResultsStart(rec);
            {
                foreach (JObject item in jArray)
                {
                    IEnumerable<JProperty> jPropertys = item.Properties();
                    int index = 0;
                    if (columnArray.Length > 0)// 有参数columns的，查询columns指定数据
                    {
                        foreach (string col in columnArray)
                        {
                            //bool flag = false;// 检查有没有指定的列名
                            foreach (JProperty jProperty in jPropertys)
                            {
                                if (col == jProperty.Name)
                                {
                                    rec.SetSqlString(index, jProperty.Value.ToString().Trim());
                                    //flag = true;
                                    index++;
                                    break;
                                }
                            }
                            /*
                            if (!flag)
                            {   // 有的Json生产者，在同一个接口中针对不同的状态，会出现缺少字段的情况，这里需要返回null
                                rec.SetSqlString(index, null);
                                index++;
                            }
                            */
                        }
                    }
                    else// 参数columns为空的，显示所有json数据
                    {
                        foreach (JProperty jProperty in jPropertys)
                        {
                            rec.SetSqlString(index, jProperty.Value.ToString().Trim());
                            index++;
                        }
                    }
                    /*
                    int index = 0;
                    foreach (JProperty jProperty in jPropertys)
                    {
                        if (columnArray == null || columnArray.Length == 0 || (columnArray.Length > 0 && Array.IndexOf(columnArray, jProperty.Name) >= 0))
                        {
                            rec.SetSqlString(index, jProperty.Value.ToString());
                            index++;
                        }
                    }
                    */
                    // send new record/row
                    SqlContext.Pipe.SendResultsRow(rec);
                }
            }
            SqlContext.Pipe.SendResultsEnd();    // finish sending

            outResult = SqlBoolean.True;
            outMsg = "成功";
        }
        catch(Exception e)
        {

            outResult = SqlBoolean.False;
            outMsg = String.Format("err:dll异常,{0}", e.ToString());
            SqlContext.Pipe.Send(outMsg.Value);
            return;
        }
    }
};
