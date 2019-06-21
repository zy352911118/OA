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
    public partial class OrderInfoService : BaseService<OrderInfo>, IOrderInfoService
    {
        #region T4生成
        //public override void SetCurrentDal()
        //{
        //    this.currentDal = DBSession.orderInfoDal;
        //} 
        #endregion
    }
}
