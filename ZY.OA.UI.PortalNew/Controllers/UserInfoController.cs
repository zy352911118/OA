using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZY.OA.Common;
using ZY.OA.IBLL;
using ZY.OA.Model;
using ZY.OA.Model.Enum;
using ZY.OA.Model.SearchModel;

namespace ZY.OA.UI.PortalNew.Controllers
{
    public class UserInfoController : BaseController
    {
        public IUserInfoService userInfoService { get; set; }
        public IRoleInfoService RoleInfoService { get; set; }
        public IActionInfoService ActionInfoService { get; set; }
        public ActionResult Index()
        {
            return View();
        }


        // 添加用户   
        public ActionResult AddUser(UserInfo userInfo)
        {
            if (!string.IsNullOrEmpty(userInfo.UName) && !string.IsNullOrEmpty(userInfo.Pwd) && !string.IsNullOrEmpty(userInfo.ShowName) && !string.IsNullOrEmpty(userInfo.Remark))
            {
                userInfo.ModfiedOn = DateTime.Now;
                userInfo.SubTime = DateTime.Now;
                UserInfo user = userInfoService.Add(userInfo);
                if (user != null)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else
            {
                return Content("no");
            }
        }
        //获取用户数据
        public ActionResult GetUserInfo()
        {
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int totalCount = 0;
            string schName = Request["schName"];//获取要搜索的用户名
            string schRemark = Request["schRemark"];//获取要搜索的备注
            UserSearchParms userSearchParms = new UserSearchParms()
            {
                pageIndex = pageIndex,
                pageSize = pageSize,
                total = totalCount,
                schName = schName,
                schRemark = schRemark
            };

            var userInfoList = userInfoService.GetPageEntityBySearch(userSearchParms);
            //short delFlag = (short)DelFlagEnum.Normal;
            //var userInfoList = userInfoService.GetPageEntities(pageSize, pageIndex, out totalCount, u => u.DelFlag == delFlag, u => u.ID, true);
            var temp = from u in userInfoList
                       select new { ID = u.ID, UName = u.UName, Pwd = u.Pwd, ShowName = u.ShowName, Remark = u.Remark, ModfiedOn = u.ModfiedOn };

            return Json(new { rows = temp, total = userSearchParms.total }, JsonRequestBehavior.AllowGet);
        }
        //根据ID查询用户的信息
        public ActionResult GetUserById()
        {

            int ID = int.Parse(Request["ID"]);
            UserInfo userInfo = userInfoService.GetEntities(u => u.ID == ID).FirstOrDefault();
            if (userInfo != null)
            {
                return Content(SerializerHelper.SerializerToString(new { Udata = userInfo, msg = "ok" }));
            }
            else
            {
                return Content(SerializerHelper.SerializerToString(new { msg = "no" }));
            }

        }
        //修改用户信息
        public ActionResult EditUser(UserInfo userInfo)
        {
            if (!string.IsNullOrEmpty(userInfo.UName) && !string.IsNullOrEmpty(userInfo.Pwd) && !string.IsNullOrEmpty(userInfo.ShowName) && !string.IsNullOrEmpty(userInfo.Remark))
            {
                UserInfo user = userInfoService.GetEntities(u => u.ID == userInfo.ID).FirstOrDefault();
                if (user != null)
                {
                    user.UName = userInfo.UName;
                    user.Pwd = userInfo.Pwd;
                    user.ShowName = userInfo.ShowName;
                    user.Remark = userInfo.Remark;
                    user.ModfiedOn = DateTime.Now;
                    if (userInfoService.Update(user))
                    {
                        return Content("ok");
                    }
                }

            }
            return Content("no");
        }
        //删除用户信息
        public ActionResult DeleteInfo()
        {
            if (!string.IsNullOrEmpty(Request["ids"]))
            {
                string ids = Request["ids"];
                string[] idsArr = ids.Split(',');
                List<int> idList = new List<int>();
                foreach (var id in idsArr)
                {

                    idList.Add(int.Parse(id));
                }
                if (userInfoService.DeleteListBylogical(idList))
                {
                    return Content("ok");
                }
            }
            return Content("no");

        }
        //为用户分配角色
        public ActionResult SetUserRoleInfo()
        {
            int ID = int.Parse(Request["ID"]);
            //查询出要设置角色的用户信息
            UserInfo userInfo = userInfoService.GetEntities(u => u.ID == ID).FirstOrDefault();
            //查询该用户所拥有的角色的ID
            var userRoleId = (from r in userInfo.RoleInfo
                              select r.ID).ToList();
            //查询所有的角色信息
            var RoleInfos = RoleInfoService.GetEntities(r => r.DelFlag == (short)DelFlagEnum.Normal).ToList();
            ViewBag.userInfo = userInfo;
            ViewBag.userRoleId = userRoleId;
            ViewBag.RoleInfos = RoleInfos;
            return View();
        }
        //为用户分配角色结束
        public ActionResult SetUserRoleInfoOver()
        {
            int userID = int.Parse(Request["userId"]);
            var keys = Request.Form.AllKeys;
            List<int> RoleIdList = new List<int>();
            foreach (var key in keys)
            {
                if (key.StartsWith("ckb_"))
                {
                    int cbkId = int.Parse(key.Replace("ckb_", ""));
                    RoleIdList.Add(cbkId);
                }
            }
            if (userInfoService.SetUserRoleInfo(userID, RoleIdList))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        //设置用户特殊权限展示
        public ActionResult SetUserActionInfo()
        {
            int userId = Request["id"] == null ? 0 : int.Parse(Request["id"]);
            //查询出要设置特殊权限的用户信息
            UserInfo userInfo = userInfoService.GetEntities(u => u.ID == userId).FirstOrDefault();
            //查询所有的权限信息
            List<ActionInfo> actionInfolist = ActionInfoService.GetEntities(a => a.DelFlag == (short)DelFlagEnum.Normal).ToList();
            //查询该用户的权限信息
            List<R_UserInfo_ActionInfo> r_UserInfo_ActionInfo = userInfo.R_UserInfo_ActionInfo.ToList();
            ViewBag.userInfo = userInfo;
            ViewBag.actionInfolist = actionInfolist;
            ViewBag.r_UserInfo_ActionInfo = r_UserInfo_ActionInfo;
            return View();
        }
        //完成对用户特殊权限设置
        public ActionResult SetUserActionInfoOver()
        {
            int actionId = Request["actionId"] == null ? 0 : int.Parse(Request["actionId"]);
            int userId = Request["userId"] == null ? 0 : int.Parse(Request["userId"]);
            bool HasPermission = Request["HasPermission"] == "true" ? true :false;
            if (userInfoService.SetUserActionInfo(actionId, userId, HasPermission))
            {
                return Content("ok");
            }
            else {
                return Content("no");
            }           
        }
        //清除用户权限
        public ActionResult ClearUserAction()
        {
           int userId= Request["userId"] == null ? 0 : int.Parse(Request["userId"]);
           int actionId = Request["actionId"] == null ? 0 : int.Parse(Request["actionId"]);
            if (userInfoService.ClearUserActionInfo(userId, actionId))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }           
        }
    }
}