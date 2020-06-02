using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;//�������ݿ������ռ�
using System.Configuration;//����configuration�����ռ�
	
namespace XXXXXXXX
{
    /// <summary>
    /// ���ݿ��������Connection����
    /// </summary>
    class DBHelper
    {
        //���ݿ������ַ���
        private readonly string connString = ConfigurationManager.ConnectionStrings["XXX"].ToString();

        //�������ݿ����Ӷ���
        private SqlConnection con;
        //���������������
        public SqlCommand cmd;

        /// <summary>
        /// ��ʼ�����Ӷ�����������
        /// </summary>
        public DBHelper()
        {
            con = new SqlConnection(connString);
            cmd = con.CreateCommand();
        }

        /// <summary>
        /// �����ݿ�����
        /// </summary>
        public void ConnOpen()
        {
            con.Open();
        }

        /// <summary>
        /// ִ����,ɾ,��SQL���ķ���
        /// </summary>
        /// <param name="sql">����SQL�����Ϊ����</param>
	/// <returns>����int����ֵ</returns>
        public int ExecuteNonQuery(string sql)
        {
            cmd = new SqlCommand(sql, con);
            int result = Convert.ToInt32( cmd.ExecuteNonQuery());
            return result;
        }


        /// <summary>
        /// ִ�в�ѯ����ֵSQL���ķ�������int�͵�����
        /// </summary>
        /// <param name="sql">����SQL�����Ϊ����</param>
	/// <returns>����int����ֵ</returns>
        public int ExecuteScalarBySql(string sql)
        {
            cmd = new SqlCommand(sql, con);
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }


        /// <summary>
        /// ִ�в�ѯ����ֵSQL���ķ�������string�͵�����
        /// </summary>
        /// <param name="sql">����SQL�����Ϊ����</param>
	/// <returns>����string����ֵ</returns>
        public string NewExecuteScalar(string sql)
        {
            cmd = new SqlCommand(sql, con);
            string result = cmd.ExecuteScalar().ToString();
            return result;
        }


        /// <summary>
        /// ����ֵΪSqlDataReader����ķ���
        /// </summary>
        /// <param name="sql">����SQL�����Ϊ����</param>
        /// <returns>SqlDataReader����</returns>
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
        /// �ر����ݿ�ķ���
        /// </summary>
        /// <returns>�ж��Ƿ�ر�</returns>
        public bool CloseDataBase()
        {
            con.Close();
            Dispose();
            return true;
        }


        /// <summary>
        /// �ͷ���Դ
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