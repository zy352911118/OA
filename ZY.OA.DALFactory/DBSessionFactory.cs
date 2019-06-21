using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.IDAL;

namespace ZY.OA.DALFactory
{
   public  class DBSessionFactory
    {
        public static IDBSession GetCurrentDBSession()
        {
            //一次请求共用一个DBSession
            IDBSession dbs = CallContext.GetData("DBSession") as IDBSession;
            if (dbs==null)
            {
                dbs = new DBSession();
                CallContext.SetData("DBSession",dbs);
            }
            return dbs;
        }
    }
}
