using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.Model;
using ZY.OA.Model.SearchModel;

namespace ZY.OA.IBLL
{
   public partial interface IRoleInfoService:IBaseService<RoleInfo>
    {
        /// <summary>
        /// 根据搜索分页显示角色信息
        /// </summary>
        /// <param name="roleSearchParms"></param>
        /// <returns></returns>
        IQueryable<RoleInfo> GetPageEntityBySearch(RoleSearchParms roleSearchParms);
      
    }
}
