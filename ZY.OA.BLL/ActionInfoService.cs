using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.IBLL;
using ZY.OA.Model;
using ZY.OA.Model.Enum;
using ZY.OA.Model.SearchModel;

namespace ZY.OA.BLL
{
   public partial class ActionInfoService:BaseService<ActionInfo>, IActionInfoService
    {
        public IQueryable<ActionInfo> GetPageEntityBySearch(ActionInfoSearchParms actionInfoSearchParms)
        {
        short Normal= (short)DelFlagEnum.Normal;//正常状态
            //查询所有权限信息
            var temp= DBSession.ActionInfoDal.GetEntities(a=>a.DelFlag== Normal);
            if (!string.IsNullOrEmpty(actionInfoSearchParms.Remark))
            {
                temp = temp.Where<ActionInfo>(a=>a.Remark.Contains(actionInfoSearchParms.Remark));
            }
            if (!string.IsNullOrEmpty(actionInfoSearchParms.HttpMethd))
            {
                temp = temp.Where<ActionInfo>(a=>a.HttpMethd.Contains(actionInfoSearchParms.HttpMethd.ToUpper()));
            }
            if (!string.IsNullOrEmpty(actionInfoSearchParms.ActionName))
            {
                temp = temp.Where<ActionInfo>(a=>a.ActionName.Contains(actionInfoSearchParms.ActionName));
            }
            actionInfoSearchParms.total = temp.Count();//总条数
            return temp.OrderBy(a => a.ID)
                      .Skip(actionInfoSearchParms.pageSize * (actionInfoSearchParms.pageIndex - 1))
                      .Take(actionInfoSearchParms.pageSize);
        }
        //设置权限角色
        public bool SetActionRoleInfo(int actionId, List<int> roleIdlist)
        {
            //获取要设置角色的权限信息
           ActionInfo actionInfo= DBSession.ActionInfoDal.GetEntities(a => a.ID == actionId).FirstOrDefault();
            if (actionInfo != null)
            {
                actionInfo.RoleInfo.Clear();//清除该权限的所有角色
                foreach (int roleId in roleIdlist)
                {
                   RoleInfo roleInfo= DBSession.RoleInfoDal.GetEntities(r => r.ID == roleId).FirstOrDefault();
                    actionInfo.RoleInfo.Add(roleInfo);
                }
                return DBSession.SaveChanges() > 0;
            }
            return false;
        }
    }
}
