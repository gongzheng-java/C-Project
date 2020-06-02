using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;//引入数据库命名空间
using System.Configuration;//引入configuration命名空间
	
namespace XXXXXXXX
{
    /// <summary>
    /// 数据库连接类和Connection对象
    /// </summary>
    class DBHelper
    {
        //数据库连接字符串
        private readonly string connString = ConfigurationManager.ConnectionStrings["XXX"].ToString();

        //定义数据库连接对象
        private SqlConnection con;
        //定义数据命令对象
        public SqlCommand cmd;

        /// <summary>
        /// 初始化连接对象和命令对象
        /// </summary>
        public DBHelper()
        {
            con = new SqlConnection(connString);
            cmd = con.CreateCommand();
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void ConnOpen()
        {
            con.Open();
        }

        /// <summary>
        /// 执行增,删,改SQL语句的方法
        /// </summary>
        /// <param name="sql">传入SQL语句作为参数</param>
	/// <returns>返回int类型值</returns>
        public int ExecuteNonQuery(string sql)
        {
            cmd = new SqlCommand(sql, con);
            int result = Convert.ToInt32( cmd.ExecuteNonQuery());
            return result;
        }


        /// <summary>
        /// 执行查询单个值SQL语句的方法返回int型的数据
        /// </summary>
        /// <param name="sql">传入SQL语句作为参数</param>
	/// <returns>返回int类型值</returns>
        public int ExecuteScalarBySql(string sql)
        {
            cmd = new SqlCommand(sql, con);
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }


        /// <summary>
        /// 执行查询单个值SQL语句的方法返回string型的数据
        /// </summary>
        /// <param name="sql">传入SQL语句作为参数</param>
	/// <returns>返回string类型值</returns>
        public string NewExecuteScalar(string sql)
        {
            cmd = new SqlCommand(sql, con);
            string result = cmd.ExecuteScalar().ToString();
            return result;
        }


        /// <summary>
        /// 返回值为SqlDataReader对象的方法
        /// </summary>
        /// <param name="sql">传入SQL语句作为参数</param>
        /// <returns>SqlDataReader对象</returns>
        public SqlDataReader GetDataReaderbySql(string sql)
        {
            try
            {
                cmd = new SqlCommand(sql, con);
                return cmd.ExecuteReader();
            }
            catch
            {
                return null;
            }

        }


        /// <summary>
        /// 关闭数据库的方法
        /// </summary>
        /// <returns>判断是否关闭</returns>
        public bool CloseDataBase()
        {
            con.Close();
            Dispose();
            return true;
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }


        protected virtual void Dispose(bool bDispose)
        {
            if (!bDispose)
                return;
            if (con.State != ConnectionState.Closed)
            {
				con.Close();
                con.Dispose();  
                cmd = null;
                con = null;
            }
        }
    }
}