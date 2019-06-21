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
        #region T4生成
        //public IUserInfoDal userInfoDal
        //{
        //    get { return AbstractDALFactory.GetUserInfoDal(); }
        //}
        //public IOrderInfoDal orderInfoDal
        //{
        //    get { return AbstractDALFactory.GetOrderInfoDal(); }
        //} 
        #endregion

        public int SaveChanges()
        {
            return DBContentFactory.GetCurrentDBContent().SaveChanges();
        }
    }
}
