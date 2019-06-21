using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZY.OA.Common;

namespace ZY.OA.UI.PortalNew.Models
{
    public class MyHandleErrorAttribute: HandleErrorAttribute
    {   
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            //异常错误信息写入队列
            LogHelper.WriteLog(filterContext.Exception.ToString());
            //跳转至错误页
            filterContext.HttpContext.Response.Redirect("/Error.html");
        }
    }
}