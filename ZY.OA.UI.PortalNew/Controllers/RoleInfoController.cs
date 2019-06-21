using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZY.OA.IBLL;
using ZY.OA.Model;
using ZY.OA.Model.Enum;
using ZY.OA.Model.SearchModel;

namespace ZY.OA.UI.PortalNew.Controllers
{
    public class RoleInfoController : BaseController
    {
        public IRoleInfoService RoleInfoService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        //获取角色信息
        public ActionResult GetRoleInfo()
        {
           int pageIndex= Request["page"] == null ? 1 : int.Parse(Request["page"]);
           int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
           int total = 0;
           string RoleName = Request["RoleName"];
           string Remark = Request["Remark"];
            RoleSearchParms roleSearchParms = new RoleSearchParms
            {
                pageIndex = pageIndex,
                pageSize = pageSize,
                total = total,
                RoleName= RoleName,
                Remark= Remark
            };
        //获取搜索后的数据
        var roleInfoList= RoleInfoService.GetPageEntityBySearch(roleSearchParms);
            var temp = from r in roleInfoList
                       select new { ID=r.ID, RoleName=r.RoleName,
                       SubTime =r.SubTime, ModfiedOn =r.ModfiedOn, Remark=r.Remark };
            return Json(new {rows=temp, total=roleSearchParms.total },JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddRoleInfo()
        {
            return View();
        }
        //添加角色信息
        [HttpPost]       
        public ActionResult AddRoleInfo(RoleInfo roleInfo)
        {
            if (!string.IsNullOrEmpty(roleInfo.RoleName))
            {
                short delFlag = (short)DelFlagEnum.Normal;
                roleInfo.SubTime = DateTime.Now;
                roleInfo.ModfiedOn = DateTime.Now;
                roleInfo.DelFlag = delFlag;
                RoleInfo role = RoleInfoService.Add(roleInfo);
                if (role != null)
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
    
      

        //展示要修改的角色信息
        public ActionResult EditRoleInfo()
        {
            int id = int.Parse(Request["ID"]);
            var RoleInfo = RoleInfoService.GetEntities(r => r.ID == id).FirstOrDefault();
            if (RoleInfo != null)
            {
                ViewBag.RoleInfo = RoleInfo;
            }          
            return View();
        }
        //修改角色
        public ActionResult EditRole(RoleInfo role)
        {
            short delFlag = (short)DelFlagEnum.Normal;
            if (!string.IsNullOrEmpty(role.RoleName)&& !string.IsNullOrEmpty(role.Remark))
            {
                role.ModfiedOn = DateTime.Now;
                role.DelFlag = delFlag;
                if (RoleInfoService.Update(role))
                {
                    return Content("ok");
                }              
            }
            return Content("no");
        }
        //根据ID删除角色信息
        public ActionResult DeleteRoleById()
        {
            string IdList = Request["IdList"];
            List<int> ids = new List<int>();
            if (!string.IsNullOrEmpty(IdList))
            {
                string[] idList = IdList.Split(',');
                foreach (var id in idList)
                {
                    ids.Add(int.Parse(id));
                }
                if (RoleInfoService.DeleteListBylogical(ids))
                {
                    return Content("ok");
                }
            }
            return Content("no");            
        }
    }
}