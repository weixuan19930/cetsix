/*
 * 创建人：卫旋
 * 创建时间：2013-7-29
 * 说明：数据库助手类
 * 版权所有：阿豆工作室
 */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace cetsix
{
    public class SQLHelper
    {
        private SqlConnection conn = null;
        private SqlCommand cmd = null;
        private SqlDataReader sdr = null;
        public SQLHelper()
        {
            string connStr = ConfigurationManager.ConnectionStrings["cetsixConnectionString"].ConnectionString;
            conn = new SqlConnection(connStr);
        }

        private SqlConnection GetConn()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        /// <summary>
        ///  执行不带参数的增删改SQL语句或存储过程
        /// </summary>
        /// <param name="cmdText">增删改SQL语句或存储过程</param>
        /// <param name="ct">命令类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, CommandType ct)
        {
            int res;
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandType = ct;
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return res;
        }

        /// <summary>
        ///  执行带参数的增删改SQL语句或存储过程
        /// </summary>
        /// <param name="cmdText">增删改SQL语句或存储过程</param>
        /// <param name="ct">命令类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, SqlParameter[] paras,CommandType ct)
        {
            int res;
            using (cmd = new SqlCommand(cmdText, GetConn()))
            {
                cmd.CommandType = ct;
                cmd.Parameters.AddRange(paras);
                res = cmd.ExecuteNonQuery();
            }
            return res;
        }

        /// <summary>
        ///  执行查询SQL语句或存储过程
        /// </summary>
        /// <param name="cmdText">查询SQL语句或存储过程</param>
        /// <param name="ct">命令类型</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string cmdText, CommandType ct)
        {
            DataTable dt = new DataTable();
            cmd = new SqlCommand(cmdText, GetConn());
            cmd.CommandType = ct;
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }

        /// <summary>
        ///  执行带参数的查询SQL语句或存储过程
        /// </summary>
        /// <param name="cmdText">查询SQL语句或存储过程</param>
        /// <param name="paras">参数集合</param>
        /// <param name="ct">命令类型</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string cmdText, SqlParameter[] paras,CommandType ct)
        {
            DataTable dt = new DataTable();
            cmd = new SqlCommand(cmdText, GetConn());
            cmd.CommandType = ct;
            cmd.Parameters.AddRange(paras);
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }

        public DataTable GetDataTable(string cmd)
        {
            return new SQLHelper().ExecuteQuery(cmd, CommandType.Text);
        }
    }
}
