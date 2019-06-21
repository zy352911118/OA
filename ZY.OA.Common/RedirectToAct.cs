using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ZY.OA.Common
{
   public  class RedirectToAct
    {
        public static void RedirectTo()
        {
            HttpContext.Current.Response.Redirect("/UserLogin/Index?return="+HttpContext.Current.Request.Url);
        }
    }
}
