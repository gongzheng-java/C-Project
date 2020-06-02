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
using System.Text.RegularExpressions;

namespace DataImport
{
    public partial class FrmPoleImport : Form
    {
        string xlmc; //线路名称
        string ywdw; //运维单位
        string ywbm; //运维部门
        int xlid; //线路ID
        //导入文件选择
        string[] fileArr = null;

        public FrmPoleImport()
        {
            InitializeComponent();
        }

        //导入杆塔数据
        private void Bulk_Import_Poles(string[] fileArr)
        {
            int nCount = 0;
            try
            {
                //定义一个开始时间
                DateTime startTime = DateTime.Now;
                
                CoordCovert dataConvert = new CoordCovert();

                for (int i = 0; i < fileArr.Length; i++)
                {                    
                    string fileName = fileArr[i];
                    //因为文件比较大，所有使用StreamReader的效率要比使用File.ReadLines高
                    int nRow = -1;
                    using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
                    {
                        while (!sr.EndOfStream)
                        {
                            nRow++;
                            string readStr = sr.ReadLine();//读取一行数据
                            string[] strs = readStr.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);//将读取的字符串按"制表符/t“和”“分割成数组
                            if (strs.Length < 7)
                                continue;

                            string sxh = strs[0];//顺序号
                            string X = strs[1]; // 
                            string Y = strs[2];
                            string TJGC = strs[3];//塔基高程
                            string TDGC = strs[4]; //塔顶高程
                            string TX = strs[5]; //塔型
                            string TGH = strs[6];//杆塔号

                            if (nRow == 0 && (containChinese(sxh) || containChinese(X)))
                                continue;

                            //去掉杆子中包含的括号
                            if (TGH.Contains('(') )
                                TGH =  TGH.Split(new char[] { '(' })[0].Trim();
                            if (TGH.Contains('（'))
                                TGH =  TGH.Split(new char[] { '（' })[0].Trim();
                            

                            int n_DH = int.Parse(cboxDH.Text.Trim());
                            double[] lon_lat = dataConvert.UTMXYToToLonLat(double.Parse(X), double.Parse(Y), n_DH, false);

                            double JD = lon_lat[0];
                            double WD = lon_lat[1];
                            double GC = double.Parse(TJGC);
                            double GTQG = double.Parse(TDGC) - GC;

                            ///计算档距
                            int dj = 0; //档距，即前后杆塔之间的水平距离                            
                            string t_sql = "";
                            if (TGH.Contains('+'))
                            {
                                //TGH = (TGH.Split(new char[] { '+' })[0]).Trim();
                                t_sql = string.Format("select jd,wd from t_poles where xlmc='{0}' and name='{1}'", xlmc, (TGH.Split(new char[] { '+' })[0]).Trim() + "#");
                            }
                            else
                                t_sql = string.Format("select jd,wd from t_poles where xlmc='{0}' and name='{1}'", xlmc, (int.Parse(TGH) - 1) + "#");                            
                           
                            DataSet ds =  DbHelper.GetDataSet(CommandType.Text, t_sql, null);
                            
                            if (ds.Tables[0]!=null && ds.Tables[0].Rows.Count>0)  //如果能读到数据，一行一行地读
                            {
                                double t_jd = double.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
                                double t_wd = double.Parse(ds.Tables[0].Rows[0].ItemArray[1].ToString());                           
                                dj = (int)dataConvert.GetDistance(JD,WD,t_jd,t_wd);                            
                            }

                            //有重复的不插入
                            string sql = string.Format("select * from t_poles where name='{0}' and xlmc='{1}'", TGH + "#",xlmc);
                            if (DbHelper.Count(CommandType.Text, sql, null) >= 1)
                                continue;                   

                            string cmdText = string.Format("insert into t_poles(XLID,XLMC,NAME,GTXH,YWDW,YWBM,GTQG,JD,WD,GC,SXH,DJ) " +
                                                 "values({0},'{1}','{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},{11})", xlid, xlmc, TGH + "#",
                                                 TX, ywdw, ywbm, GTQG, JD, WD, GC,sxh,dj);
                            DbHelper.ExecuteNonQuery(CommandType.Text, cmdText, null);
                            nCount++;               
                        }
                    }
                }

                //string mysql = "update t_poles AA join ( Select NAME,(@rowNum:=@rowNum+1) as rowNo From t_poles a,(Select (@rowNum :=0) ) b where a.xlmc = '复奉线(安徽)' Order by a.name asc) CC SET AA.SXH = CC.rowNO WHERE AA.XLMC = '复奉线(安徽)' and AA.NAME = CC.NAME";
                //DbHelper.ExecuteNonQuery(CommandType.Text, mysql, null);

                //结束时间-开始时间=总共花费的时间
                TimeSpan ts = DateTime.Now - startTime;
                lblInfo.Text = "导入状态：" + "共花费时间:" + ts.ToString() + " 导入记录条数：" + nCount;
                Array.Clear(fileArr, 0, fileArr.Length);
                txtFileName.Text = "";
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message + " :" + nCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " :" + nCount );
            }

        }

        private bool containChinese(string input)
        {
            string pattern = "[\u4e00-\u9fbb]";
            return Regex.IsMatch(input, pattern);
        }

        private void frmPoleImport_Load(object sender, EventArgs e)
        {
            cboxDH.SelectedIndex = 5;//默认带号50
            
            string sql = "select distinct name from t_lines";
            DbHelper.BindToCombox(cboxXLMC,sql);

            cboxYWDW.Text = "安徽省电力公司";


            sql = "select distinct name from sys_dept";
            DbHelper.BindToCombox(cboxYWBM, sql);

            cboxYWBM.Text = "检修分公司";

            rbtnSingle.Checked = true;
        }

        private void cboxXLMC_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = cboxXLMC.Text;
            if (val.Trim()=="")
                return;

            string sql = string.Format("select id from t_lines where NAME = '{0}'", val);
            string id = DbHelper.ExecuteScalar(CommandType.Text, sql, null).ToString();
            txtXLBH.Text = id;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboxYWBM_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            //导入文件选择            
            bool isPath = false;
            if (rbtnMulti.Checked)
                isPath = true;
            else
                isPath = false;

            fileArr = UtilHelper.GetFileList(isPath);
            if (fileArr != null && fileArr.Length > 0)
            {
                if (isPath)
                    txtFileName.Text = Path.GetDirectoryName(fileArr[0]);
                else
                    txtFileName.Text = fileArr[0];
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (txtFileName.Text.Trim() == "")
            {
                MessageBox.Show("请选择导入文件或文件夹");
                return;
            }
            //参数设置
            xlmc = cboxXLMC.Text.Trim();
            ywdw = cboxYWDW.Text.Trim();
            ywbm = cboxYWBM.Text.Trim();            
            string xlbh = txtXLBH.Text.Trim();
            string dh = cboxDH.Text ;
            if (xlmc == "" || ywdw == "" || ywbm == "" || xlbh == "" || dh =="")
            {
                MessageBox.Show("关键数据不能为空");
                return;
            }
            xlid = int.Parse(txtXLBH.Text);

            Bulk_Import_Poles(fileArr);
        }
    }
}
