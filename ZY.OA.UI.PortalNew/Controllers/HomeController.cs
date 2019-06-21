using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZY.OA.IBLL;
using ZY.OA.Model;
using ZY.OA.Model.Enum;

namespace ZY.OA.UI.PortalNew.Controllers
{
    public class HomeController : BaseController
    {
        public IUserInfoService userInfoService { get; set; }
        public ActionResult Index()
        {
            ViewBag.userInfo = userInfo;
            return View();
        }
        //权限图标过滤
        public ActionResult ActionIconfiltration()
        {
            //正常未删除状态
            short normal = (short)DelFlagEnum.Normal;
            //1.用户--角色--权限
            //根据登录用户查找用户信息
            UserInfo LoginUserInfo = userInfoService.GetEntities(u => u.ID == userInfo.ID).FirstOrDefault();
            //根据角色找到所有的菜单权限
            var UserActions = (from r in LoginUserInfo.RoleInfo
                               from a in r.ActionInfo
                               where (a.DelFlag == normal&&a.IsMenu==true)
                               select a).ToList();
            //2.用户----权限
            //找到登录用户所有的菜单权限
            var Actions = from r in LoginUserInfo.R_UserInfo_ActionInfo
                                        where r.ActionInfo.IsMenu == true
                                        select r.ActionInfo;

            //将两条线的权限合并
            UserActions.AddRange(Actions);
            //查询出被禁止的权限ID
            var rejectActions = (from R in LoginUserInfo.R_UserInfo_ActionInfo
                                 where (R.HasPermission == false)
                                 select R.ActionInfoID).ToList();
            //过滤掉被禁止的权限
            var Allactions = UserActions.Where(a => !rejectActions.Contains(a.ID)).ToList();

            //去重
           var LoginUserAllowActions= Allactions.Distinct(new EqualityComparer());
            //筛选出Index需要的数据
            var returnLinks = from a in LoginUserAllowActions
                       select new { icon = a.MenuIcon, title = a.ActionName, url = a.Url };
            return Json(returnLinks,JsonRequestBehavior.AllowGet);
        }

    }
}