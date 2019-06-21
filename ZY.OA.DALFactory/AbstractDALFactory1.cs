 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.IDAL;

namespace ZY.OA.DALFactory
{
   public partial class AbstractDALFactory
    {
	 	public static IActionInfoDal GetActionInfoDal()
        {
            return Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".ActionInfoDal") as IActionInfoDal;
        }
	 	public static IMenuInfoDal GetMenuInfoDal()
        {
            return Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".MenuInfoDal") as IMenuInfoDal;
        }
	 	public static IOrderInfoDal GetOrderInfoDal()
        {
            return Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".OrderInfoDal") as IOrderInfoDal;
        }
	 	public static IR_UserInfo_ActionInfoDal GetR_UserInfo_ActionInfoDal()
        {
            return Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".R_UserInfo_ActionInfoDal") as IR_UserInfo_ActionInfoDal;
        }
	 	public static IRoleInfoDal GetRoleInfoDal()
        {
            return Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".RoleInfoDal") as IRoleInfoDal;
        }
	 	public static IUserInfoDal GetUserInfoDal()
        {
            return Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".UserInfoDal") as IUserInfoDal;
        }
	 	public static IUserInfoExtDal GetUserInfoExtDal()
        {
            return Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".UserInfoExtDal") as IUserInfoExtDal;
        }
	}
}