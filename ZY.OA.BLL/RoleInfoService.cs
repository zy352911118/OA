using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.DALFactory;
using ZY.OA.IBLL;
using ZY.OA.Model;
using ZY.OA.Model.Enum;
using ZY.OA.Model.SearchModel;

namespace ZY.OA.BLL
{
   public partial class RoleInfoService:BaseService<RoleInfo>, IRoleInfoService
    {
        
        /// <summary>
        /// 根据搜索分页显示角色信息
        /// </summary>
        /// <param name="roleSearchParms"></param>
        /// <returns></returns>
        public IQueryable<RoleInfo> GetPageEntityBySearch(RoleSearchParms roleSearchParms)
        {
            short Normal=(short)DelFlagEnum.Normal;//删除标记为正常

            var temp= DBSession.RoleInfoDal.GetEntities(r => r.DelFlag == Normal);//查询所有的角色信息

            if (!string.IsNullOrEmpty(roleSearchParms.RoleName))
            {
                temp = temp.Where(r => r.RoleName.Contains(roleSearchParms.RoleName));//如果有角色名称 则筛选
            }

            if (!string.IsNullOrEmpty(roleSearchParms.Remark))
            {
                temp = temp.Where(r=>r.Remark.Contains(roleSearchParms.Remark));//如果有备注 则筛选
            }

            roleSearchParms.total = temp.Count();//筛选完之后的总条数

            return temp = temp.OrderBy<RoleInfo, int>(r => r.ID)
                             .Skip(roleSearchParms.pageSize * (roleSearchParms.pageIndex - 1))
                             .Take(roleSearchParms.pageSize);//分页后的数据
        }
    }
}
