using System.Web;
using System.Web.Mvc;
using ZY.OA.UI.PortalNew.Models;

namespace ZY.OA.UI.PortalNew
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MyHandleErrorAttribute());
           
        }
    }
}
