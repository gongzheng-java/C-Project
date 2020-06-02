using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DataImport
{
    public class UtilHelper
    {
        public static string[] GetFileList(bool isPath)
        {
            //导入文件选择
            string[] fileArr = null;
            if (isPath) //选择文件夹
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择文件路径";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string foldPath = dialog.SelectedPath;
                    DirectoryInfo theFolder = new DirectoryInfo(foldPath);
                    FileInfo[] dirInfo = theFolder.GetFiles();

                    fileArr = new string[dirInfo.Length];
                    for (int i = 0; i < dirInfo.Length; i++)
                        fileArr[i] = dirInfo[i].FullName;
                }

            }
            else //选择文件
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = true;
                fileDialog.Title = "请选择文件";
                fileDialog.Filter = "文本文档|*.txt";
                //如果用户没有选择文件并确定则直接返回
                if (fileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return null;
                fileArr = fileDialog.FileNames;
            }
            if (fileArr == null || fileArr.Length == 0)
                return null;

            return fileArr;   
        }
    }
}
