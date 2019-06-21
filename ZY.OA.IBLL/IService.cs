 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.Model;

namespace ZY.OA.IBLL
{

    public partial interface IActionInfoService : IBaseService<ActionInfo>
    {
       
    }

    public partial interface IMenuInfoService:IBaseService<MenuInfo>
    {
    }		
	
	public partial interface IOrderInfoService:IBaseService<OrderInfo>
    {
    }		
	
	public partial interface IR_UserInfo_ActionInfoService:IBaseService<R_UserInfo_ActionInfo>
    {
    }		
	
	public partial interface IRoleInfoService:IBaseService<RoleInfo>
    {
    }		
	
	public partial interface IUserInfoService:IBaseService<UserInfo>
    {
    }		
	
	public partial interface IUserInfoExtService:IBaseService<UserInfoExt>
    {
    }		
	

}