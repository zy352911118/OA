 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.DAL;
using ZY.OA.DALFactory;
using ZY.OA.IBLL;
using ZY.OA.IDAL;
using ZY.OA.Model;

namespace ZY.OA.BLL
{   
	
	public partial class ActionInfoService : BaseService<ActionInfo>, IActionInfoService
    {
        public override void SetCurrentDal()
        {
            this.currentDal =DBSession.ActionInfoDal;
        }
    }
	
	public partial class MenuInfoService : BaseService<MenuInfo>, IMenuInfoService
    {
        public override void SetCurrentDal()
        {
            this.currentDal =DBSession.MenuInfoDal;
        }
    }
	
	public partial class OrderInfoService : BaseService<OrderInfo>, IOrderInfoService
    {
        public override void SetCurrentDal()
        {
            this.currentDal =DBSession.OrderInfoDal;
        }
    }
	
	public partial class R_UserInfo_ActionInfoService : BaseService<R_UserInfo_ActionInfo>, IR_UserInfo_ActionInfoService
    {
        public override void SetCurrentDal()
        {
            this.currentDal =DBSession.R_UserInfo_ActionInfoDal;
        }
    }
	
	public partial class RoleInfoService : BaseService<RoleInfo>, IRoleInfoService
    {
        public override void SetCurrentDal()
        {
            this.currentDal =DBSession.RoleInfoDal;
        }
    }
	
	public partial class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        public override void SetCurrentDal()
        {
            this.currentDal =DBSession.UserInfoDal;
        }
    }
	
	public partial class UserInfoExtService : BaseService<UserInfoExt>, IUserInfoExtService
    {
        public override void SetCurrentDal()
        {
            this.currentDal =DBSession.UserInfoExtDal;
        }
    }
	
}