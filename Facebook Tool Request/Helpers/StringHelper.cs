using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.Helpers
{
    public class StringHelper
    {
        public static bool CheckStringIsNumber(string content)
        {
            try
            {
                int num = Convert.ToInt32(content);
                return true;
            }
            catch
            {
            }
            return false;
        }
    }
}
