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

namespace DataImport
{
    public partial class FrmDangerImport : Form
    {
        string xlmc; //线路名称   
        int xlid; //线路ID
        string status; //隐患类型，如0瞬时，1大风，2高温，3重冰
        string[] fileArr = null;

        public FrmDangerImport()
        {
            InitializeComponent();
        }

        private void FrmDangerImport_Load(object sender, EventArgs e)
        {
            cboxType.Items.Add("瞬时");
            cboxType.Items.Add("高温");
            cboxType.Items.Add("大风");
            cboxType.Items.Add("重冰");
            //cboxType.Items.Add("倒伏");

            string sql = "select distinct name from t_lines";
            DbHelper.BindToCombox(cboxXLMC, sql);

            rbtnMulti.Checked = true;
        }

        private void cboxXLMC_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = cboxXLMC.Text;
            if (val.Trim() == "")
                return;

            string sql = string.Format("select id from t_lines where NAME = '{0}'", val);
            string id = DbHelper.ExecuteScalar(CommandType.Text, sql, null).ToString();
            txtXLBH.Text = id;
        }     


        private void Bulk_Import_Danger(string[] fileArray)
        {
            if (fileArray == null || fileArray.Length == 0)
                return;
            try
            {
                //定义一个开始时间
                DateTime startTime = DateTime.Now;
                int nCount = 0;
                CoordCovert dataConvert = new CoordCovert();

                for (int i = 0; i < fileArray.Length; i++)
                {
                    string fileName = fileArray[i];
                    //因为文件比较大，所有使用StreamReader的效率要比使用File.ReadLines高
                    using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
                    {
                        while (!sr.EndOfStream)
                        {
                            string readStr = sr.ReadLine();//读取一行数据
                            string[] strs = readStr.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);//将读取的字符串按"制表符/t“和”“分割成数组
                            if (strs.Length < 11)
                                continue;

                            string yhqd = strs[1]; //区段
                            string xhcgtjl = strs[2]; //距小号塔距离
                            string wd = strs[3]; //纬度
                            string jd = strs[4]; //经度
                            string gc = strs[5]; //高程
                            string wxdmc = strs[6]; //危险点名称
                            string spjl = strs[7]; //水平距离
                            string czjl = strs[8]; //垂直距离
                            string kjjl = strs[9]; //空间距离
                            string ddjl = strs[10]; //对地距离

                            string line_jd = strs[18];
                            string line_wd = strs[19];
                            string line_gc = strs[20];

                            string qsh = "";
                            if (yhqd.Length > 0)
                            {
                                string[] arr = yhqd.Split(new char[] { ')' });
                                qsh = arr[0].Substring(1);
                            }
                            txtStart.Text = qsh;

                            //有重复的不插入
                            //string sql = string.Format("select * from t_poles where name='{0}'", TGH + "#");
                            //if (DbHelper.Count(CommandType.Text, sql, null) >= 1)
                            //    continue;
                            string cmdText = string.Format("insert into r_dangers(XLID,XLMC,YHLX,YHQD,XHCGT,XHCGTJL,SPJL,CZJL,DDJL,KJJL,JD,WD,GC,STATUS,LINE_JD,LINE_WD,LINE_GC) " +
                                                 "values({0},'{1}','{2}','{3}','{4}',{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16})", xlid, xlmc, wxdmc,
                                                 yhqd, qsh, xhcgtjl, spjl, czjl,ddjl,kjjl, jd, wd, gc,status,line_jd,line_wd,line_gc);

                            DbHelper.ExecuteNonQuery(CommandType.Text, cmdText, null);
                            nCount++;
                        }
                    }
                }
                //结束时间-开始时间=总共花费的时间
                TimeSpan ts = DateTime.Now - startTime;
                lblInfo.Text = "导入状态：" + "共花费时间:" + ts.ToString() + " 导入记录条数：" + nCount;
                Array.Clear(fileArr, 0, fileArr.Length);
                txtFileName.Text = "";
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
            //参数设置
            xlmc = cboxXLMC.Text.Trim();
            string xlbh = txtXLBH.Text.Trim();
            string tp = cboxType.Text.Trim();
            if (xlmc == "" || xlbh == "" || tp == "")
            {
                MessageBox.Show("关键数据不能为空");
                return;
            }
            xlid = int.Parse(txtXLBH.Text);

            if (tp == "瞬时")
                status = "0";
            else if (tp == "大风")
                status = "1";
            else if (tp == "高温")
                status = "2";
            else if (tp == "重冰")
                status = "3";
            else
                return;

            Bulk_Import_Danger(fileArr);
        }
    }
}
