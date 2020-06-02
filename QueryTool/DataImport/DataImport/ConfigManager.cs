using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Configuration;
using System.Xml;
using System.Windows.Forms;

namespace DataImport
{
    class ConfigManager
    {
        private string xml_File = Application.StartupPath + "\\dbconfig.xml";
        private XmlDocument m_XmlDoc; //xml文档      

        public ConfigManager() 
        {
            try
            {
                m_XmlDoc = new XmlDocument();
                m_XmlDoc.Load(xml_File);
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        public void list_DB_Info(out string server, out string database, out string username, out string password)
        {
            server = "";
            database = "";
            username = "";
            password = "";
            try
            {
                //遍历xml      
                XmlNodeList nodeList = m_XmlDoc.SelectNodes("/datasources/mysql/*");
                foreach (XmlNode nd in nodeList)
                {
                    if (nd.Name == "server")
                        server = nd.InnerText;
                    else if(nd.Name == "database")
                        database = nd.InnerText;
                    else if (nd.Name == "user")
                        username = nd.InnerText;
                    else if (nd.Name == "password")
                        password = nd.InnerText;                  
                } 
            }
            catch {               
            }
        }

        public void update_DB_Info(string server, string database, string username, string password)
        {  
            try
            {
                //遍历xml      
                XmlNodeList nodeList = m_XmlDoc.SelectNodes("/datasources/mysql/*");
                foreach (XmlNode nd in nodeList)
                {
                    if (nd.Name == "server")
                        nd.InnerText = server;
                    else if (nd.Name == "database")
                        nd.InnerText = database;
                    else if (nd.Name == "user")
                       nd.InnerText = username;
                    else if (nd.Name == "password")
                       nd.InnerText = password;
                }
                m_XmlDoc.Save(xml_File);
            }
            catch
            {
            }
        }

        //System.Configuration.Configuration config = null;
        //public ConfigManager()
        //{
        //     config=ConfigurationManager.OpenExeConfiguration(
        //     ConfigurationUserLevel.None);
        //}
        
        ///// <summary>
        ///// //添加键值
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        //public void AddAppSetting(string key, string value)
        //{
            
        //    config.AppSettings.Settings.Add(key, value);
        //    config.Save();
        //}
        
        ///// <summary>
        ///// //修改键值
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        //public void SaveConnectionStrings(string key, string value)
        //{
        //    //config.AppSettings.Settings.Remove(key);
        //    //config.AppSettings.Settings.Add(key, value);

        //    config.ConnectionStrings.ConnectionStrings.Remove(key);
        //    ConnectionStringSettings settings = new ConnectionStringSettings();
        //    settings.Name=key;
        //    settings.ConnectionString =value;
        //    config.ConnectionStrings.ConnectionStrings.Add(settings);

        //    config.Save(ConfigurationSaveMode.Modified);
        //    ConfigurationManager.RefreshSection("connectionStrings");
            
        //}
        
        ///// <summary>
        ///// //获得键值
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public string GetConnectionStrings(string key)
        //{
        //    return  config.ConnectionStrings.ConnectionStrings[key].ToString();
        //}
        
        ///// <summary>
        ///// //移除键值
        ///// </summary>
        ///// <param name="key"></param>
        //public void DelAppSetting(string key)
        //{
        //    config.AppSettings.Settings.Remove(key);
        //    config.Save();
        //}

        //public ArrayList GetXmlElements(string strElem)
        //{
        //    ArrayList list = new ArrayList();
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
        //    XmlNodeList listNode = xmlDoc.SelectNodes(strElem);
        //    foreach (XmlElement el in listNode)
        //    {
        //        list.Add(el.InnerText);
        //    }
        //    return list;
        //}

    }
}
