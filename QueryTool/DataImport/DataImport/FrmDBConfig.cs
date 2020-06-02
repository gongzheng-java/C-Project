using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataImport
{
    public partial class FrmDBConfig : Form
    {
        ConfigManager config = null;

        public FrmDBConfig()
        {
            InitializeComponent();
            
        }

        private void FrmDBConfig_Load(object sender, EventArgs e)
        {
            config = new ConfigManager();
            string datasource,database,user,password;

            config.list_DB_Info(out datasource, out database, out user, out password);
            txtDatabase.Text = database;
            txtServer.Text = datasource;
            txtUser.Text = user;
            txtPasswod.Text = password;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string server, database, user, password;
            server = txtServer.Text.Trim();
            database = txtDatabase.Text.Trim();
            user = txtUser.Text.Trim();
            password = txtPasswod.Text;
            if (server == "" || database == "" || user == "" || password == "")
            {
                MessageBox.Show("关键信息不能为空");
                return;
            }
            config.update_DB_Info(server, database, user, password);

            this.Close();


            //string connectstring = "server="+server+";database="+database+";user="+user+";pwd"+password;

            //config.SaveConnectionStrings("server", connectstring);

            //DbHelper.m_Server = server;
            //DbHelper.m_Database = database;
            //DbHelper.m_User = user;
            //DbHelper.m_Password = password;
            //this.Close();
        }
    }
}
