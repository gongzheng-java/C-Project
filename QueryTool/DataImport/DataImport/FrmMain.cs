using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace DataImport
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

    

        private void Form1_Load(object sender, EventArgs e)
        {
            
            string server="", database="", user="", password="";
            ConfigManager config = new ConfigManager();
            config.list_DB_Info(out server,out database,out user,out password);
            DbHelper.m_Server = server;
            DbHelper.m_Database = database;
            DbHelper.m_User = user;
            DbHelper.m_Password = password;

            //初始化线路信息
            string sql_query = "select XLID, XLMC FROM t_poles GROUP BY XLID ORDER BY XLID";
            DataSet XLMC_Data = DbHelper.GetDataSet(CommandType.Text, sql_query, null);
           
            this.cboXLMC.DataSource = XLMC_Data.Tables[0];//绑定要展示的结果集 如学生查询结果集： 学生名称 学生ID
            this.cboXLMC.DisplayMember = "XLMC";//显示值 如 学生名称
            this.cboXLMC.ValueMember = "XLID";//实际值 如 学生ID
            
       
            
        }

        //查询
        private void btnQuery_Click(object sender, EventArgs e)
        {

            //获取选中线路的名称
            int XLID = Convert.ToInt32(this.cboXLMC.SelectedValue);
            //根据名称查询塔编号
            string sql_query = string.Format("SELECT `NAME` FROM t_poles WHERE XLID='{0}' ORDER BY `NAME`+0 ", XLID);
            DataSet XLMC_Data = DbHelper.GetDataSet(CommandType.Text, sql_query, null);
            DataTable dt = XLMC_Data.Tables[0];
            ////所有塔个数
            //int count = dt.Rows.Count;
            ////获取第一个塔编码
            //int firstNum = Convert.ToInt32(UtilHelper.strConvertToInt(dt.Rows[0][0].ToString()));
            //// MessageBox.Show(firstNum.ToString());
            ////获取最后一个塔编号
            //int lastNum = Convert.ToInt32(UtilHelper.strConvertToInt(dt.Rows[count - 1][0].ToString()));
            ////MessageBox.Show(lastNum.ToString());

            //将塔编号数据转换成int集合
            List<int> dataList = new List<int>();
            foreach (DataRow r in dt.Rows)
            {
                dataList.Add(Convert.ToInt32(UtilHelper.strConvertToInt(r.ItemArray[0].ToString())));
            }

         //数据展示处理
         List<HashSet<int>> reslut= dataHandle(dataList);
         List<string> reslutStr = new List<string>();
            foreach(HashSet<int> t in reslut){
                int max=t.Max();
                int min=t.Min();
                string str = "连续区间：" + min + " ---> " + max;
                reslutStr.Add(str);
               
            }
           //绑定到datagridview
            this.dgvShow.DataSource = reslutStr.Select(x => new { Value = x }).ToList();
        }

        /**
         * 数据处理
         * 
         */
        private List<HashSet<int>> dataHandle(List<int> dataList)
        {
            //返回结果集
            Dictionary<string, object> d = new Dictionary<string, object>();
            //是否连续标识
            Boolean flag = true;

            //处理后的数据集
            List<HashSet<int>> reslutList = new List<HashSet<int>>();
            HashSet<int> temList = null;//用于去重
            for (int i = 0; i < dataList.Count - 1; i++)
            {
                if (i == 0)
                {
                    temList = new HashSet<int>();
                }

                int before = dataList[i];
                int last = dataList[i + 1];

                if ((before + 1) != last)
                {
                    Console.WriteLine("不连续");
                    if (i == 0)
                    {//当第一个数据就不连续，将第一个数据单独放到一个集合里面
                        temList.Add(before);
                    }
                    reslutList.Add(temList);
                    temList = new HashSet<int>();
                    temList.Add(last);
                    flag = false;
                }
                else
                {
                    Console.WriteLine("连续");
                    temList.Add(before);
                    temList.Add(last);
                }
                //当遍历到倒数第二个数据时(当前i的值只能遍历到倒数第二个数)，存在两个逻辑，如下
                if (last == dataList[dataList.Count - 1])
                {
                    temList.Add(last);//1.倒数第二个数和最后一个数不连续
                    reslutList.Add(temList);//2.当倒数第二个数和最后一个数连续，但是和前一个不连续
                }
            }

            //添加返回数据
            d.Add("flag", flag);
            d.Add("data", reslutList);
            return reslutList;
        }

       


     

  

        private void btnExit_Click(object sender, EventArgs e)
        {     
            Application.Exit();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            FrmDBConfig dbconfig = new FrmDBConfig();
            dbconfig.ShowDialog();
        }

        

      

      

       

        
   
    }
}
