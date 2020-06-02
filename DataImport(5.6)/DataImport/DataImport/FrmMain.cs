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

        private void btnInputCrossing_Click(object sender, EventArgs e)
        {
            FrmCrossingImport frmCrossing = new FrmCrossingImport();
            frmCrossing.ShowDialog();            
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
        }

        private void btnInputPoles_Click(object sender, EventArgs e)
        {
            FrmPoleImport frmPole = new FrmPoleImport();
            frmPole.ShowDialog();
        }

        private void btnInputDangers_Click(object sender, EventArgs e)
        {
            FrmDangerImport frmDanger = new FrmDangerImport();
            frmDanger.ShowDialog();
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

        private void bnt_e_danger_Click(object sender, EventArgs e)
        {
      
            labTips.Visible = true;
            labTips.Refresh();
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//鼠标为忙碌状态
            //危机用 3 重大用 2 一版用1表示
            string sql_update = "";
            sql_update = "update r_dangers A set level = case";

            // 35kV 交流	
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='公路' and A.CZJL<7*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='公路' and A.CZJL>7*0.65 AND A.CZJL<7 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='铁路' and A.CZJL<7.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='铁路' and A.CZJL>7.5*0.65 AND A.CZJL<7.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='通航河流' and A.CZJL<6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='通航河流' and A.CZJL>6*0.65 AND A.CZJL<6 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='管道' and A.CZJL<4*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='管道' and A.CZJL>4*0.65 AND A.CZJL<4 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<4*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>4*0.65 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<4 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>4 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<4*1.5 then 1";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='地面' and A.CZJL<6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='地面' and A.CZJL>6*0.65 AND A.CZJL<6 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='建筑物' and A.CZJL<5.4*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND A.YHLX='建筑物' and A.CZJL>5.4*0.65 AND A.CZJL<5.4 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND (A.YHLX='承力索或接触线' OR A.YHLX='不通航河流' OR A.YHLX='索道' OR A.YHLX='电力线' )  and A.CZJL<3*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流35kV' AND (A.YHLX='承力索或接触线' OR A.YHLX='不通航河流' OR A.YHLX='索道' OR A.YHLX='电力线' )  and A.CZJL>3*0.65 AND A.CZJL<3 then 2";

            // 110kV 交流	
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='公路' and A.CZJL<7*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='公路' and A.CZJL>7*0.65 AND A.CZJL<7 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='铁路' and A.CZJL<7.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='铁路' and A.CZJL>7.5*0.65 AND A.CZJL<7.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='通航河流' and A.CZJL<6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='通航河流' and A.CZJL>6*0.65 AND A.CZJL<6 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='管道' and A.CZJL<4*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='管道' and A.CZJL>4*0.65 AND A.CZJL<4 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<4*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>4*0.65 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<4 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>4 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<4*1.5 then 1";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='地面' and A.CZJL<6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='地面' and A.CZJL>6*0.65 AND A.CZJL<6 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='建筑物' and A.CZJL<5.4*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND A.YHLX='建筑物' and A.CZJL>5.4*0.65 AND A.CZJL<5.4 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND (A.YHLX='承力索或接触线' OR A.YHLX='不通航河流' OR A.YHLX='索道' OR A.YHLX='电力线' )  and A.CZJL<3*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流110kV' AND (A.YHLX='承力索或接触线' OR A.YHLX='不通航河流' OR A.YHLX='索道' OR A.YHLX='电力线' )  and A.CZJL>3*0.65 AND A.CZJL<3 then 2";


            // 220kV 交流	
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='公路' and A.CZJL<8*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='公路' and A.CZJL>8*0.65 AND A.CZJL<8 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='铁路' and A.CZJL<8.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='铁路' and A.CZJL>8.5*0.65 AND A.CZJL<8.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='通航河流' and A.CZJL<7*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='通航河流' and A.CZJL>7*0.65 AND A.CZJL<7 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='管道' and A.CZJL<5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='管道' and A.CZJL>5*0.65 AND A.CZJL<5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<4.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>4.5*0.65 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<4.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>4.5 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<4.5*1.5 then 1";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='地面' and A.CZJL<6.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='地面' and A.CZJL>6.5*0.65 AND A.CZJL<6.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='建筑物' and A.CZJL<6.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND A.YHLX='建筑物' and A.CZJL>6.5*0.65 AND A.CZJL<6.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND (A.YHLX='承力索或接触线' OR A.YHLX='不通航河流' OR A.YHLX='索道' OR A.YHLX='电力线' )  and A.CZJL<4*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流220kV' AND (A.YHLX='承力索或接触线' OR A.YHLX='不通航河流' OR A.YHLX='索道' OR A.YHLX='电力线' )  and A.CZJL>4*0.65 AND A.CZJL<4 then 2";

            // 330kV 交流          	
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='公路' and A.CZJL<9.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='公路' and A.CZJL>9.0*0.65 AND A.CZJL<9.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='铁路' and A.CZJL<9.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='铁路' and A.CZJL>9.5*0.65 AND A.CZJL<9.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='通航河流' and A.CZJL<8.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='通航河流' and A.CZJL>8.0*0.65 AND A.CZJL<8.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='管道' and A.CZJL<6.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='管道' and A.CZJL>6.0*0.65 AND A.CZJL<6.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<5.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>5.5*0.65 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<5.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>5.5 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<5.5*1.5 then 1";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='地面' and A.CZJL<7.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='地面' and A.CZJL>7.5*0.65 AND A.CZJL<7.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='建筑物' and A.CZJL<7.6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND A.YHLX='建筑物' and A.CZJL>7.6*0.65 AND A.CZJL<7.6 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND (A.YHLX='承力索或接触线' OR A.YHLX='不通航河流' OR A.YHLX='索道' OR A.YHLX='电力线' )  and A.CZJL<5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流330kV' AND (A.YHLX='承力索或接触线' OR A.YHLX='不通航河流' OR A.YHLX='索道' OR A.YHLX='电力线' )  and A.CZJL>5*0.65 AND A.CZJL<5 then 2";

            // 500kV 交流	
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='公路' and A.CZJL<14.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='公路' and A.CZJL>14.0*0.65 AND A.CZJL<14.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='铁路' and A.CZJL<14.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='铁路' and A.CZJL>14.0*0.65 AND A.CZJL<14.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='通航河流' and A.CZJL<9.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='通航河流' and A.CZJL>9.5*0.65 AND A.CZJL<9.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='管道' and A.CZJL<7.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='管道' and A.CZJL>7.5*0.65 AND A.CZJL<7.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<7.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>7.0*0.65 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<7.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>7.0 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<7.0*1.5 then 1";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='地面' and A.CZJL<11.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='地面' and A.CZJL>11.0*0.65 AND A.CZJL<11.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='建筑物' and A.CZJL<10.3*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND A.YHLX='建筑物' and A.CZJL>10.3*0.65 AND A.CZJL<10.3 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND (A.YHLX='承力索或接触线' OR A.YHLX='电力线' )  and A.CZJL<6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND (A.YHLX='承力索或接触线' OR A.YHLX='电力线' )  and A.CZJL>6*0.65 AND A.CZJL<6 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND ( A.YHLX='不通航河流' OR A.YHLX='索道')  and A.CZJL<6.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流500kV' AND ( A.YHLX='不通航河流' OR A.YHLX='索道'  )  and A.CZJL>6.5*0.65 AND A.CZJL<6.5 then 2";

            // 750kV 交流	
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='公路' and A.CZJL<19.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='公路' and A.CZJL>19.5*0.65 AND A.CZJL<19.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='铁路' and A.CZJL<19.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='铁路' and A.CZJL>19.5*0.65 AND A.CZJL<19.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='通航河流' and A.CZJL<11.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='通航河流' and A.CZJL>11.5*0.65 AND A.CZJL<11.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='管道' and A.CZJL<9.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='管道' and A.CZJL>9.5*0.65 AND A.CZJL<9.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<8.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>8.5*0.65 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<8.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>8.5 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<8.5*1.5 then 1";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='地面' and A.CZJL<15.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='地面' and A.CZJL>15.5*0.65 AND A.CZJL<15.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='建筑物' and A.CZJL<13.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='建筑物' and A.CZJL>13.0*0.65 AND A.CZJL<13.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='承力索或接触线' and A.CZJL<7*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='承力索或接触线'   and A.CZJL>7*0.65 AND A.CZJL<7 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='不通航河流'  and A.CZJL<8*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='不通航河流'  and A.CZJL>8*0.65 AND A.CZJL<8 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='索道'  and A.CZJL<11*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='索道'  and A.CZJL>11*0.65 AND A.CZJL<11 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='电力线'  and A.CZJL<7*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流750kV' AND A.YHLX='电力线'  and A.CZJL>7*0.65 AND A.CZJL<7 then 2";

            // ±500kV 直流	
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='公路' and A.CZJL<16.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='公路' and A.CZJL>16.0*0.65 AND A.CZJL<16.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='铁路' and A.CZJL<16.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='铁路' and A.CZJL>16.0*0.65 AND A.CZJL<16.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='通航河流' and A.CZJL<12.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='通航河流' and A.CZJL>12.0*0.65 AND A.CZJL<12.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='管道' and A.CZJL<9*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='管道' and A.CZJL>9*0.65 AND A.CZJL<9 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<7*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>7*0.65 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<7 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>7 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<7*1.5 then 1";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='地面' and A.CZJL<12.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='地面' and A.CZJL>12.5*0.65 AND A.CZJL<12.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='建筑物' and A.CZJL<14.9*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='建筑物' and A.CZJL>14.9*0.65 AND A.CZJL<14.9 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='承力索或接触线' and A.CZJL<7.6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='承力索或接触线'   and A.CZJL>7.6*0.65 AND A.CZJL<7.6 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='不通航河流'  and A.CZJL<7.6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='不通航河流'  and A.CZJL>7.6*0.65 AND A.CZJL<7.6 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='索道'  and A.CZJL<6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='索道'  and A.CZJL>6*0.65 AND A.CZJL<6 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='电力线'  and A.CZJL<7.6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流500kV' AND A.YHLX='电力线'  and A.CZJL>7.6*0.65 AND A.CZJL<7.6 then 2";

            // ±660kV 直流	
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='公路' and A.CZJL<18.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='公路' and A.CZJL>18.0*0.65 AND A.CZJL<18.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='铁路' and A.CZJL<18.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='铁路' and A.CZJL>18.0*0.65 AND A.CZJL<18.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='通航河流' and A.CZJL<12.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='通航河流' and A.CZJL>12.5*0.65 AND A.CZJL<12.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='管道' and A.CZJL<14*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='管道' and A.CZJL>14*0.65 AND A.CZJL<14 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<10.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>10.5*0.65 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<10.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>10.5 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<10.5*1.5 then 1";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='地面' and A.CZJL<16.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='地面' and A.CZJL>16.0*0.65 AND A.CZJL<16.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='建筑物' and A.CZJL<16.3*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='建筑物' and A.CZJL>16.3*0.65 AND A.CZJL<16.3 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='承力索或接触线' and A.CZJL<10*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='承力索或接触线'   and A.CZJL>10*0.65 AND A.CZJL<10 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='不通航河流'  and A.CZJL<10*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='不通航河流'  and A.CZJL>10*0.65 AND A.CZJL<10 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='索道'  and A.CZJL<8*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='索道'  and A.CZJL>8*0.65 AND A.CZJL<8 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='电力线'  and A.CZJL<8*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流660kV' AND A.YHLX='电力线'  and A.CZJL>8*0.65 AND A.CZJL<8 then 2";

            // ±800kV 直流	
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='公路' and A.CZJL<21.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='公路' and A.CZJL>21.5*0.65 AND A.CZJL<21.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='铁路' and A.CZJL<21.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='铁路' and A.CZJL>21.5*0.65 AND A.CZJL<21.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='通航河流' and A.CZJL<15.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='通航河流' and A.CZJL>15.0*0.65 AND A.CZJL<15.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='管道' and A.CZJL<17*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='管道' and A.CZJL>17*0.65 AND A.CZJL<17 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<13.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>13.5*0.65 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<13.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>13.5 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<13.5*1.5 then 1";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='地面' and A.CZJL<19.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='地面' and A.CZJL>19.0*0.65 AND A.CZJL<19.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='建筑物' and A.CZJL<18.8*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='建筑物' and A.CZJL>18.8*0.65 AND A.CZJL<18.8 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='承力索或接触线' and A.CZJL<13*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='承力索或接触线'   and A.CZJL>13*0.65 AND A.CZJL<13 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='不通航河流'  and A.CZJL<12.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='不通航河流'  and A.CZJL>12.5*0.65 AND A.CZJL<12.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='索道'  and A.CZJL<10.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='索道'  and A.CZJL>10.5*0.65 AND A.CZJL<10.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='电力线'  and A.CZJL<10.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流800kV' AND A.YHLX='电力线'  and A.CZJL>10.5*0.65 AND A.CZJL<10.5 then 2";

            // 1000kV 交流	
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='公路' and A.CZJL<27.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='公路' and A.CZJL>27.0*0.65 AND A.CZJL<27.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='铁路' and A.CZJL<27.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='铁路' and A.CZJL>27.0*0.65 AND A.CZJL<27.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='通航河流' and A.CZJL<14.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='通航河流' and A.CZJL>14.0*0.65 AND A.CZJL<14.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='管道' and A.CZJL<18*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='管道' and A.CZJL>18*0.65 AND A.CZJL<18 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<14.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>14.0*0.65 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<14.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>14.0 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<14.0*1.5 then 1";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='地面' and A.CZJL<22.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='地面' and A.CZJL>22.0*0.65 AND A.CZJL<22.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='建筑物' and A.CZJL<17.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='建筑物' and A.CZJL>17.0*0.65 AND A.CZJL<17.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='承力索或接触线' and A.CZJL<10*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='承力索或接触线'   and A.CZJL>10*0.65 AND A.CZJL<10 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='不通航河流'  and A.CZJL<10*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='不通航河流'  and A.CZJL>10*0.65 AND A.CZJL<10 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='索道'  and A.CZJL<13*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)='交流1000kV' AND A.YHLX='索道'  and A.CZJL>13*0.65 AND A.CZJL<13 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='电力线'  and A.CZJL<10*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '交流1000kV' AND A.YHLX='电力线'  and A.CZJL>10*0.65 AND A.CZJL<10 then 2";

            // ±1100kV 直流	
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='公路' and A.CZJL<28.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='公路' and A.CZJL>28.5*0.65 AND A.CZJL<28.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='铁路' and A.CZJL<28.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='铁路' and A.CZJL>28.5*0.65 AND A.CZJL<28.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='通航河流' and A.CZJL<19.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='通航河流' and A.CZJL>19.0*0.65 AND A.CZJL<19.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='管道' and A.CZJL<22*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='管道' and A.CZJL>22*0.65 AND A.CZJL<22 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<14*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>14*0.65 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<14 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>14 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<14*1.5 then 1";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='地面' and A.CZJL<25.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='地面' and A.CZJL>25.0*0.65 AND A.CZJL<25.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='建筑物' and A.CZJL<22.6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='建筑物' and A.CZJL>22.6*0.65 AND A.CZJL<22.6 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='承力索或接触线' and A.CZJL<19.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='承力索或接触线'   and A.CZJL>19.5*0.65 AND A.CZJL<19.5 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='不通航河流'  and A.CZJL<15.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='不通航河流'  and A.CZJL>15.0*0.65 AND A.CZJL<15.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='索道'  and A.CZJL<13*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='索道'  and A.CZJL>13*0.65 AND A.CZJL<13 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='电力线'  and A.CZJL<13*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='电力线'  and A.CZJL>13*0.65 AND A.CZJL<13 then 2";

            // ±440kV 直流	
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='公路' and A.CZJL<13.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='公路' and A.CZJL>13.0*0.65 AND A.CZJL<13.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='铁路' and A.CZJL<25.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='铁路' and A.CZJL>25.0*0.65 AND A.CZJL<25.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='通航河流' and A.CZJL<11.5*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='通航河流' and A.CZJL>11.5*0.65 AND A.CZJL<11.5 then 2";
            // sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='管道' and A.CZJL<22*0.65 then 3";
            // sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='管道' and A.CZJL>22*0.65 AND A.CZJL<22 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<7.0*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>7.0*0.65 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<7.0 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='树木' and SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))>7.0 AND SQRT(POWER(A.CZJL,2)+POWER(A.SPJL,2))<7.0*1.5 then 1";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='地面' and A.CZJL<12*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='地面' and A.CZJL>12*0.65 AND A.CZJL<12 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='建筑物' and A.CZJL<10.3*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='建筑物' and A.CZJL>10.3*0.65 AND A.CZJL<10.3 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='承力索或接触线' and A.CZJL<6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='承力索或接触线'   and A.CZJL>6*0.65 AND A.CZJL<6 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='不通航河流'  and A.CZJL<8.6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='不通航河流'  and A.CZJL>8.6*0.65 AND A.CZJL<8.6 then 2";
            // sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='索道'  and A.CZJL<13*0.65 then 3";
            // sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流1100kV' AND A.YHLX='索道'  and A.CZJL>13*0.65 AND A.CZJL<13 then 2";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='电力线'  and A.CZJL<8.6*0.65 then 3";
            sql_update += " when (select dydj from t_lines B where A.XLMC=B.NAME LIMIT 1)= '直流440kV' AND A.YHLX='电力线'  and A.CZJL>8.6*0.65 AND A.CZJL<8.6 then 2";

            sql_update += " else 0 end";


            try
            {
                int flag = DbHelper.ExecuteNonQuery(CommandType.Text, sql_update, null);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
                MessageBox.Show("更新成功!", "提示：", MessageBoxButtons.OK, MessageBoxIcon.None);
                labTips.Visible = false;
                
            }
            catch 
            {
                this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
                labTips.Visible = false;
                MessageBox.Show("SQL执行异常，请查看数据库配置是否正确!", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }
           

        }

   
    }
}
