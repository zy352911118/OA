using System;
using System.Collections.Generic;
using System.IO;
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
    public class ActionInfoController : BaseController
    {
        public IActionInfoService ActionInfoService { get; set; }
        public IRoleInfoService RoleInfoService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        //获取权限信息
        public ActionResult GetActionInfo()
        {
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int total = 0;
            string Remark = Request["Remark"];
            string HttpMethd = Request["HttpMethd"];
            string ActionName = Request["ActionName"];
            ActionInfoSearchParms actionInfoSearchParms = new ActionInfoSearchParms()
            {
                pageIndex = pageIndex,
                pageSize = pageSize,
                total = total,
                Remark = Remark,
                HttpMethd = HttpMethd,
                ActionName = ActionName
            };
            var ActionInfos = ActionInfoService.GetPageEntityBySearch(actionInfoSearchParms);
            return Content(SerializerHelper.SerializerToString(new { rows = ActionInfos, total = actionInfoSearchParms.total }));
        }

        //添加权限信息
        public ActionResult AddActionInfo()
        {
            return View();
        }
        //添加权限信息
        [HttpPost]
        public ActionResult AddActionInfo(ActionInfo actionInfo)
        {
            if (!string.IsNullOrEmpty(actionInfo.ActionName) && !string.IsNullOrEmpty(actionInfo.Url) && !string.IsNullOrEmpty(Request["Sort"]))
            {
                int sort = 0;
                int.TryParse(Request["Sort"], out sort);
                short normal = (short)DelFlagEnum.Normal;
                actionInfo.Url = Request["Url"].ToLower();
                actionInfo.Sort = sort;
                actionInfo.SubTime = DateTime.Now;
                actionInfo.ModfiedOn = DateTime.Now;
                actionInfo.DelFlag = normal;
                actionInfo.IsMenu = Request["isOrNo"] == "true" ? true : false;
                actionInfo.MenuIcon = Request["ImgSrc"];
                ActionInfo action = ActionInfoService.Add(actionInfo);
                if (action != null)
                {
                    return Content("ok");
                }
            }
            return Content("no");
        }
        //上传图片
        public ActionResult UpLoadFile()
        {
            HttpPostedFileBase file = Request.Files["MenuIcon"];//获取客户端上传的文件
            if (file != null)
            {
                string fileExt = Path.GetExtension(file.FileName); //获取扩展名
                string[] fileExtArray = { ".BMP", ".JPG", ".JPEG", ".PNG", ".GIF" };
                bool b = false;
                string thumbpath = "";
                foreach (string Ext in fileExtArray)
                {
                    if (fileExt.ToUpper() == Ext)
                    {
                        b = true;
                        //源图路径
                        string path = "/UpLoadImg/" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "/";
                        Directory.CreateDirectory(Path.GetDirectoryName(Request.MapPath(path)));//创建源路径
                        string originalpath = path + Guid.NewGuid().ToString() + file.FileName;
                        string originalImagePath = Request.MapPath(originalpath);
                        file.SaveAs(originalImagePath);
                        //缩略图路径
                        string tbpath = "/UploadImgThumbnail/" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "/";
                        Directory.CreateDirectory(Path.GetDirectoryName(Request.MapPath(tbpath)));//创建缩略图路径
                        thumbpath = tbpath + Guid.NewGuid().ToString() + file.FileName;
                        string thumbnailPath = Request.MapPath(thumbpath);
                        ImageClass.MakeThumbnail(originalImagePath, thumbnailPath, 100, 100, "HW");
                    }
                }
                if (b)
                {
                    return Content("ok:" + thumbpath);
                }
                else
                {
                    return Content("no:");
                }
            }
            else
            {
                return Content("no:");
            }


        }

        //编辑权限信息
        public ActionResult EditAction()
        {
            int id = int.Parse(Request["id"]);
            ActionInfo actionInfo = ActionInfoService.GetEntities(a => a.ID == id).FirstOrDefault();
            if (actionInfo != null)
            {
                ViewBag.actionInfo = actionInfo;
            }
            return View();
        }

        //编辑权限信息
        [HttpPost]
        public ActionResult EditAction(ActionInfo actionInfo)
        {
            if (!string.IsNullOrEmpty(actionInfo.ActionName) && !string.IsNullOrEmpty(actionInfo.Url) && !string.IsNullOrEmpty(Request["Sort"]))
            {
                int sort = 0;
                int.TryParse(Request["Sort"], out sort);
                actionInfo.Url = Request["Url"].ToLower();
                actionInfo.Sort = sort;
                actionInfo.ModfiedOn = DateTime.Now;
                actionInfo.IsMenu = Request["isOrNo"] == "true" ? true : false;
                if (actionInfo.IsMenu == true)
                {
                    actionInfo.MenuIcon = Request["ImgSrc"];
                }
                else
                {
                    actionInfo.MenuIcon = "";
                }

                if (ActionInfoService.Update(actionInfo))
                {
                    return Content("ok");
                }
            }
            return Content("no");
        }

        //删除权限信息
        [HttpPost]
        public ActionResult DeleteActionInfo()
        {
            string actionIds = Request["actionIds"];
            if (!string.IsNullOrEmpty(actionIds))
            {
                string[] actionIdArray = actionIds.Split(',');
                List<int> actionIdList = new List<int>();
                foreach (string id in actionIdArray)
                {
                    actionIdList.Add(int.Parse(id));
                }
                if (ActionInfoService.DeleteListBylogical(actionIdList))
                {
                    return Content("ok");
                }
            }
            return Content("no");
        }

        //设置权限角色展示
        public ActionResult SetActionRoleInfo()
        {
            int actionId = int.Parse(Request["id"]);
            short Normal = (short)DelFlagEnum.Normal;
            //获取要设置角色的权限信息
            ActionInfo actionInfo = ActionInfoService.GetEntities(a => a.ID == actionId).FirstOrDefault();
            //获取所有的角色信息
            List<RoleInfo> RoleInfoList = RoleInfoService.GetEntities(r => r.DelFlag == Normal).ToList();
            List<int> actionRoleId = actionInfo.RoleInfo.Select(r => r.ID).ToList();
            ViewBag.actionInfo = actionInfo;
            ViewBag.RoleInfoList = RoleInfoList;
            ViewBag.actionRoleId = actionRoleId;
            return View();
        }

        //设置权限角色
        public ActionResult SetActionRole()
        {
           int actionId= Request["actionInfoId"]==null?0:int.Parse(Request["actionInfoId"]);
           string [] allKeys= Request.Form.AllKeys;
            List<int> roleIdlist = new List<int>();
            foreach (var Key in allKeys)
            {
                if (Key.StartsWith("CKB_"))
                {
                    roleIdlist.Add(int.Parse(Request[Key]));
                }
            }
            if (ActionInfoService.SetActionRoleInfo(actionId, roleIdlist))
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