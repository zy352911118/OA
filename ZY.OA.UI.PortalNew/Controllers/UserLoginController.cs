using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZY.OA.Common;
using ZY.OA.IBLL;
using ZY.OA.Model.Enum;

namespace ZY.OA.UI.PortalNew.Controllers
{
    public class UserLoginController : Controller
    {
       public IUserInfoService UserInfoService { get; set; }
        public ActionResult Index()
        {          
           CheckCookiesInfo(Request["return"]);
                        
            return View();
        }

        //验证码
        public ActionResult ShowCode()
        {
            ValidateCode validateCode = new ValidateCode();
            string code= validateCode.CreateValidateCode(4);
            //将验证码存入Session
            Session["VCode"] = code;
            byte[] codeBuffer = validateCode.CreateValidateGraphic(code);
            return File(codeBuffer, "image/jpeg");
        }

        //处理登录
        public ActionResult ProcessLogin()
        {
            //判断验证码是否正确
           string SessionCode = Session["VCode"] as string;
            Session["VCode"] = null;
           string txtCode=Request["code"];
            if (string.IsNullOrEmpty(SessionCode)||SessionCode!= txtCode)
            {
                return Content("no,验证码输入有误!");
            }
            //判断用户名密码是否正确
           string userName= Request["login"];
           string userPwd= Request["pwd"];
           short delNormal = (short)DelFlagEnum.Normal;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPwd))
            {
                var userInfo = UserInfoService.GetEntities(u => u.UName == userName && u.Pwd == userPwd && u.DelFlag == delNormal).FirstOrDefault();
                if (userInfo != null)
                {
                    //模拟的SessionId
                    string usersLoginId = Guid.NewGuid().ToString();
                    //Session["userInfo"] = userInfo;
                    //将用户信息写入memcache，模拟SessionId
                    memcachedHelper.set(usersLoginId, SerializerHelper.SerializerToString(userInfo), DateTime.Now.AddMinutes(20));
                    //将模拟的SessionId写入客户端Cookie中
                    Response.Cookies["usersLoginId"].Value = usersLoginId;
                    //如果用户选择“记住我”，则将用户信息存入Cookie
                    if (!string.IsNullOrEmpty(Request["rememberMe"]))
                    {
                        HttpCookie cookie1 = new HttpCookie("ck1",userInfo.UName);
                        HttpCookie cookie2 = new HttpCookie("ck2", MD5Helper.GetMd5String((MD5Helper.GetMd5String(userInfo.Pwd))));
                        cookie1.Expires = DateTime.Now.AddDays(7);
                        cookie2.Expires = DateTime.Now.AddDays(7);
                        Response.Cookies.Add(cookie1);
                        Response.Cookies.Add(cookie2);
                    }
                    string Url = Request["returnUrl"];
                    if (!string.IsNullOrEmpty(Url))
                    {
                        return Content("ok,"+ Url);
                    }
                    else
                    {
                        return Content("ok,");
                    }
                    
                }
                else
                {
                    return Content("no,用户名或密码错误！");
                }
            }
            else
            {
                return Content("no,用户名和密码不能为空！");
            }
        }

        //检查Cookies的值是否正确，正确则直接跳转
        public void CheckCookiesInfo(string url)
        {
            var ck1 = Request.Cookies["ck1"];
            var ck2 = Request.Cookies["ck2"];
            if (ck1!=null&&ck2!=null)
            {
                string userName = ck1.Value;
                string Pwd = ck2.Value;
                short delFlag = (short)DelFlagEnum.Normal;
               var userInfo=UserInfoService.GetEntities(u => u.UName == userName && u.DelFlag == delFlag).FirstOrDefault();
                if (userInfo!=null)
                {
                    if (Pwd== MD5Helper.GetMd5String(MD5Helper.GetMd5String(userInfo.Pwd)))
                    {
                        string usersLoginId = Guid.NewGuid().ToString();
                        Response.Cookies["usersLoginId"].Value = usersLoginId;
                        memcachedHelper.set(usersLoginId, SerializerHelper.SerializerToString(userInfo),DateTime.Now.AddMinutes(20));
                        if (!string.IsNullOrEmpty(url))
                        {
                            Response.Redirect(url);//如果验证用户记住了密码，并通过，则跳转至访问页
                        }
                        else
                        {
                            Response.Redirect("/Home/Index");//如果验证用户记住了密码，并通过，且直接访问的登录页，则跳转首页
                        }                    
                        
                    }
                }
                Response.Cookies["ck1"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["ck2"].Expires = DateTime.Now.AddDays(-1);
            }
        }

        //退出登录
        public ActionResult LoginOut()
        {
            if (Request.Cookies["usersLoginId"] != null)
            {
                memcachedHelper.Delete(Request.Cookies["usersLoginId"].Value);
            }
            Response.Cookies["ck1"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["ck2"].Expires = DateTime.Now.AddDays(-1);

            return RedirectToAction("Index");
        }
    }
}