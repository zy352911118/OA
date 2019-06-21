 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.DAL;
using ZY.OA.IDAL;

namespace ZY.OA.DALFactory
{
	public partial class DBSession: IDBSession
    {
	 	public IActionInfoDal ActionInfoDal
        {
            get { return AbstractDALFactory.GetActionInfoDal(); }
        }
	 	public IMenuInfoDal MenuInfoDal
        {
            get { return AbstractDALFactory.GetMenuInfoDal(); }
        }
	 	public IOrderInfoDal OrderInfoDal
        {
            get { return AbstractDALFactory.GetOrderInfoDal(); }
        }
	 	public IR_UserInfo_ActionInfoDal R_UserInfo_ActionInfoDal
        {
            get { return AbstractDALFactory.GetR_UserInfo_ActionInfoDal(); }
        }
	 	public IRoleInfoDal RoleInfoDal
        {
            get { return AbstractDALFactory.GetRoleInfoDal(); }
        }
	 	public IUserInfoDal UserInfoDal
        {
            get { return AbstractDALFactory.GetUserInfoDal(); }
        }
	 	public IUserInfoExtDal UserInfoExtDal
        {
            get { return AbstractDALFactory.GetUserInfoExtDal(); }
        }
	}
}