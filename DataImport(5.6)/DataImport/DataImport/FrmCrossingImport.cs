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
    public partial class FrmCrossingImport : Form
    {
        string xlmc; //线路名称   
        int xlid; //线路ID
        //导入文件选择
        string[] fileArr = null;

        public FrmCrossingImport()
        {
            InitializeComponent();
        }

        private void FrmCrossingImport_Load(object sender, EventArgs e)
        {
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

        private void btnExeInput_Click(object sender, EventArgs e)
        {
            //参数设置
            xlmc = cboxXLMC.Text.Trim();          
            string xlbh = txtXLBH.Text.Trim();
            if (xlmc == "" || xlbh == "")
            {
                MessageBox.Show("关键数据不能为空");
                return;
            }
            xlid = int.Parse(txtXLBH.Text);
            

            //导入文件选择
            string[] fileArr = null;
            bool isPath = false;
            if (rbtnMulti.Checked)
                isPath = true;
            else
                isPath = false;
            fileArr = UtilHelper.GetFileList(isPath);

            Bulk_Import_Crossing(fileArr);
        }

        private void Bulk_Import_Crossing(string[] fileArray)
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
                            
                            string jkqd = ""; //区段
                            string xhcgtjl = ""; //距小号塔距离
                            string wd = ""; //纬度
                            string jd = ""; //经度
                            string gc = ""; //高程
                            string wxdmc = ""; //危险点名称
                            string spjl = ""; //水平距离
                            string czjl = ""; //垂直距离
                            string kjjl = ""; //空间距离
                            string ddjl = ""; //对地距离
                            string version = ""; //数据版本

                            string line_jd = ""; //导线点经度
                            string line_wd = "";//导线点维度
                            string line_gc = "";//导线点高程
                   
                            if (strs.Length == 8) //缺高程、空间距离、对地距离
                            {
                                jkqd = strs[1]; //区段
                                xhcgtjl = strs[2]; //距小号塔距离
                                wd = strs[3]; //纬度
                                jd = strs[4]; //经度
                                gc = "null"; //高程
                                wxdmc = strs[5]; //危险点名称
                                spjl = strs[6]; //水平距离
                                czjl = strs[7]; //垂直距离
                            }
                            else if (strs.Length == 9) //缺空间距离、对地距离
                            {
                                jkqd = strs[1]; //区段
                                xhcgtjl = strs[2]; //距小号塔距离
                                wd = strs[3]; //纬度
                                jd = strs[4]; //经度
                                gc = strs[5]; //高程
                                wxdmc = strs[6]; //危险点名称
                                spjl = strs[7]; //水平距离
                                czjl = strs[8]; //垂直距离
                            }
                            else if (strs.Length == 10) //缺对地距离
                            {
                                jkqd = strs[1]; //区段
                                xhcgtjl = strs[2]; //距小号塔距离
                                wd = strs[3]; //纬度
                                jd = strs[4]; //经度
                                gc = strs[5]; //高程
                                wxdmc = strs[6]; //危险点名称
                                spjl = strs[7]; //水平距离
                                czjl = strs[8]; //垂直距离
                                kjjl = strs[9]; //空间距离
                                //jkjl = "null"; //对地距离
                            }
                            else if (strs.Length == 11)
                            {
                                jkqd = strs[1]; //区段
                                xhcgtjl = strs[2]; //距小号塔距离
                                wd = strs[3]; //纬度
                                jd = strs[4]; //经度
                                gc = strs[5]; //高程
                                wxdmc = strs[6]; //危险点名称
                                spjl = strs[7]; //水平距离
                                czjl = strs[8]; //垂直距离
                                kjjl = strs[9]; //空间距离
                                //jkjl = strs[10]; //对地距离
                            }
                            else if (strs.Length == 17)
                            {
                                jkqd = strs[1]; //区段
                                xhcgtjl = strs[2]; //距小号塔距离
                                wd = strs[3]; //纬度
                                jd = strs[4]; //经度
                                gc = strs[5]; //高程
                                wxdmc = strs[6]; //危险点名称
                                spjl = strs[7]; //水平距离
                                czjl = strs[8]; //垂直距离
                                kjjl = strs[9]; //空间距离
                                //jkjl = strs[10]; //对地距离

                                line_jd = strs[12];
                                line_wd = strs[13];
                                line_gc = strs[14];
                            }
                            else
                                continue;

                            string qsh ="";
                            if (jkqd.Length>0)
                            {
                                string[] arr = jkqd.Split(new char[] { ')' });
                                qsh = arr[0].Substring(1);
                            }
                            txtStart.Text = qsh;

                            //有重复的不插入
                            //string sql = string.Format("select * from t_poles where name='{0}'", TGH + "#");
                            //if (DbHelper.Count(CommandType.Text, sql, null) >= 1)
                            //    continue;
                            version = dtVersion.Value.Year.ToString();

                            string cmdText = string.Format("insert into r_crosses(XLID,XLMC,NAME,JKLX,JKQD,XHCGT,XHCGTJL,SPJL,CZJL,KJJL,JD,WD,GC,VERSION,LINE_JD,LINE_WD,LINE_GC) " +
                                                 "values({0},'{1}','{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16})", xlid, xlmc, wxdmc,
                                                 wxdmc, jkqd, qsh, xhcgtjl, spjl, czjl, kjjl, jd, wd, gc, version,line_jd,line_wd,line_gc);

                            //交跨
                            //string cmdText = string.Format("update r_crosses set line_jd={0}, line_wd={1},line_gc={2} where xlmc='复奉线' and name='{3}'and jkqd='{4}' and xhcgtjl={5}",
                            //                                line_jd, line_wd, line_gc, wxdmc, jkqd, xhcgtjl);

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

                //更新指定线路的顺序号               
                //string sql_update = "UPDATE t_poles AA JOIN(SELECT NAME,(@rowNum := @rowNum + 1) AS rowNo FROM t_poles a,(SELECT(@rowNum:= 0)) b ";
                //sql_update = sql_update + " WHERE a.xlmc = @xlmc ORDER BY a.NAME ASC) CC";
                //sql_update = sql_update + " SET AA.SXH = CC.rowNO WHERE AA.XLMC = @xlmc AND AA.NAME = CC.NAME";
                //MySqlParameter[] ps ={ new MySqlParameter("@compartName", xlmc) };
                //DbHelper.ExecuteNonQuery(CommandType.Text, sql_update, ps);


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
            if (txtFileName.Text.Trim() == "")
            {
                MessageBox.Show("请选择导入文件或文件夹");
                return;
            }

            //参数设置
            xlmc = cboxXLMC.Text.Trim();
            string xlbh = txtXLBH.Text.Trim();
            if (xlmc == "" || xlbh == "")
            {
                MessageBox.Show("关键数据不能为空");
                return;
            }
            xlid = int.Parse(txtXLBH.Text);


            //导入文件
             Bulk_Import_Crossing(fileArr);
        }
    }
}
