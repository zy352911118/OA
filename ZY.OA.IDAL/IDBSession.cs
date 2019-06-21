using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.OA.IDAL
{
   public partial interface IDBSession
    {
        #region T4生成
        //IUserInfoDal userInfoDal { get; }
        //IOrderInfoDal orderInfoDal { get; } 
        #endregion

        int SaveChanges();
    }
}
