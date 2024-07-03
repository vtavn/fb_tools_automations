using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.core.Enum
{
    public class GetContentStatusChrome
    {
        public static string GetContent(StatusChromeAccount status)
        {
            string result = "";
            switch (status)
            {
                case StatusChromeAccount.ChromeClosed:
                    result = Language.GetValue("Không tìm thấy chrome!");
                    break;
                case StatusChromeAccount.Checkpoint:
                    result = "Checkpoint!";
                    break;
                case StatusChromeAccount.NoInternet:
                    result = Language.GetValue("Không có kết nối Internet!");
                    break;
                case StatusChromeAccount.Blocked:
                    result = Language.GetValue("Facebook Blocked!");
                    break;
            }
            return result;
        }
    }
}
