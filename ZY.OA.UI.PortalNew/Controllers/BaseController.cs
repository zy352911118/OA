using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZY.OA.Common;
using ZY.OA.IBLL;
using ZY.OA.Model;
using ZY.OA.Model.Enum;

namespace ZY.OA.UI.PortalNew.Controllers
{
    public class BaseController : Controller
    {

        public UserInfo userInfo { get; set; }
        private bool isExp = false;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //Action执行之前判断memcache是否有值（用户是否已经登录）
            if (Request.Cookies["usersLoginId"] != null)
            {
                string usersLoginId = Request.Cookies["usersLoginId"].Value;
                object obj = memcachedHelper.Get(usersLoginId);
                if (obj != null)
                {
                    userInfo = SerializerHelper.DeSerializerToObject<UserInfo>(obj.ToString());//反序列化
                    //模拟Session的滑动过期时间
                    memcachedHelper.Update(usersLoginId, obj, DateTime.Now.AddMinutes(20));
                    isExp = true;
                    //zhengyu可越狱
                    if (userInfo.UName=="zhengyu")
                    {
                        return;
                    }
                    //获取请求的绝对路径和请求方式
                    string requestUrl = Request.Url.AbsolutePath.ToLower();
                    string httpMethod = Request.HttpMethod;
                    //通过容器对象来创建对象，因基类注入不了
                    IApplicationContext ctx = ContextRegistry.GetContext();
                    IActionInfoService actionInfoService = ctx.GetObject("ActionInfoService") as IActionInfoService;
                    IUserInfoService UserInfoService = ctx.GetObject("UserInfoService") as IUserInfoService;
                    ActionInfo actionInfo = actionInfoService.GetEntities(a => a.Url == requestUrl && a.HttpMethd == httpMethod&&a.DelFlag==(short)DelFlagEnum.Normal).FirstOrDefault();
                    if (actionInfo == null)
                    {
                        filterContext.Result = new RedirectResult("/Error.html");
                        //Response.Redirect("/Error.html");
                        return;
                    }
                    //第1条线.用户---权限
                    //登录用户
                    UserInfo loginUser = UserInfoService.GetEntities(u => u.ID == userInfo.ID).FirstOrDefault();
                    //判断登录用户请求的地址是否有权限
                    ActionInfo userActionOne = (from r in loginUser.R_UserInfo_ActionInfo
                                        where r.ActionInfoID == actionInfo.ID && r.HasPermission == true
                                        select r.ActionInfo).FirstOrDefault();
                    if (userActionOne == null)
                    {
                        //第2条线.用户---角色---权限
                        //判断登录用户请求的地址是否有权限
                        ActionInfo userActionTwo = (from r in loginUser.RoleInfo
                                                    from a in r.ActionInfo
                                                    where a.ID == actionInfo.ID
                                                    select a).FirstOrDefault();
                        if (userActionTwo == null)
                        {
                            //filterContext.Result = new RedirectResult("/ActionError.html");
                            //Response.Redirect("/ActionError.html");
                            filterContext.Result = new ContentResult() {Content="您没有此权限!请联系管理员" };         return;               
                        }
                    }
                    
                }
            }
            if (!isExp)
            {
                RedirectToAct.RedirectTo();
                //filterContext.HttpContext.Response.Redirect("/UserLogin/Index?return="+Request.Url);
                return;
            }
        }
    }
}