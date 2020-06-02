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
       

        public static int strConvertToInt(string str)
        {

            int index1 = str.LastIndexOf("+");
            if(index1!=-1){
                str = str.Substring(0, index1);
                return Convert.ToInt32(str);
            }

            int index2=str.LastIndexOf("#");
            if(index2<=0){
                MessageBox.Show("塔编号不是以 # 结尾！", "运行异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentNullException("参数异常:塔编号不是以 # 结尾！");
            }
            str = str.Substring(0, index2);
            return Convert.ToInt32(str);
        }

    }
}
