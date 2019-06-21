using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.Model;
using ZY.OA.Model.SearchModel;

namespace ZY.OA.IBLL
{
    public partial interface IActionInfoService : IBaseService<ActionInfo>
    {
        IQueryable<ActionInfo> GetPageEntityBySearch(ActionInfoSearchParms actionInfoSearchParms);
        bool SetActionRoleInfo(int actionId, List<int> roleIdlist);
    }
}
