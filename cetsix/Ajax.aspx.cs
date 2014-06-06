using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
//using DBS;
using System.Text.RegularExpressions;
using cetsix;
using System.Text;


public partial class cetsix_Ajax : System.Web.UI.Page
{
    private SQLHelper db = new SQLHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        object obj = Request.Form.Get("type");

        //请求参数错误
       // if (obj == null || obj.ToString() == "" || DataHelper.GetUserCode() == "") { Response.Write("erorr"); Response.End(); }//return value end

        string type = obj.ToString();

        obj = null;

        //添加图片
        if (type == "get")
            type = GetSearch(Request.Form.Get("words"));

        Response.Write(type);
        Response.End();
    }

    /// <summary>
    /// 是否为英文
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private bool Isengchar(string str)
    {
        Regex r = new Regex("^[A-Za-z]+$");//构造表达式

        Match m = r.Match(str);//匹配源文本

        if (m.Success)
        {
            r = null;
            m = null;
            return true;
        }
        r = null;
        m = null;
        return false;
    }

    private string GetSearch(string word)
    {
        if (word == null || word == "") return "";

        word = word.Trim().ToLower();

        DataTable dt = null;

        string sql = "";

        //英文
        if (Isengchar(word))
        {
            sql = string.Format("select top 10  words, meaning,lx from cetsix where words like  '%{0}%'", word);
        }
        //汉字
        else
        {
            sql = string.Format("select top 10  words, meaning,lx from cetsix where  meaning like '%{0}%' ", word);
        }

        dt = db.GetDataTable(sql);

        sql = null;
        word = null;

        if (dt != null && dt.Rows.Count > 0)
        {
            int length = dt.Rows.Count;

            for (int i = 0; i < length; i++)
            {
                dt.Rows[i]["meaning"] = HttpUtility.UrlEncode(dt.Rows[i]["meaning"].ToString().Replace("/r", "<br/>").Replace("/n", "")).Replace("+", "%20");
                dt.Rows[i]["lx"] = HttpUtility.UrlEncode(dt.Rows[i]["lx"].ToString().Replace("/r", "<br/>").Replace("/n", "")).Replace("+", "%20");
            }
        }
        return  DataTable2Json(dt);//to json(这个方法可以用.net的接送序列化dll)
    }




        #region dataTable转换成Json格式
        /// <summary>  
        /// dataTable转换成Json格式  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append(dt.TableName);
            jsonBuilder.Append("\":[");
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        #endregion dataTable转换成Json格式
        #region DataSet转换成Json格式
        /// <summary>  
        /// DataSet转换成Json格式  
        /// </summary>  
        /// <param name="ds">DataSet</param> 
        /// <returns></returns>  
        public static string Dataset2Json(DataSet ds)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                json.Append("{\"");
                json.Append(dt.TableName);
                json.Append("\":");
                json.Append(DataTable2Json(dt));
                json.Append("}");
            } return json.ToString();
        }
        #endregion

        /// <summary>
        /// Msdn
        /// </summary>
        /// <param name="jsonName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(string jsonName, DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
    }
