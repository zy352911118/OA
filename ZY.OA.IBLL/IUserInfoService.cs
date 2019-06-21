using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.Model;
using ZY.OA.Model.SearchModel;

namespace ZY.OA.IBLL
{
    public partial interface IUserInfoService : IBaseService<UserInfo>
    {
        //多条件搜索
        IQueryable<UserInfo> GetPageEntityBySearch(UserSearchParms userSearchParms);
        //设置用户角色
        bool SetUserRoleInfo(int userID, List<int> RoleIdList);
        //为特殊用户分配权限
        bool SetUserActionInfo(int actionId, int userId, bool HasPermission);
        //清除某用户的某条权限
        bool ClearUserActionInfo(int userId, int actionId);
    }

    
    
}
