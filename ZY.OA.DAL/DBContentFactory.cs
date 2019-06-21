using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.Model;

namespace ZY.OA.DAL
{
    public class DBContentFactory
    {
        public static DbContext GetCurrentDBContent()
        {
            //一次请求公用一个上下文实例
           DbContext db=CallContext.GetData("DbContext") as DbContext;
            if (db==null)
            {
                db = new DataModelContainer();
                CallContext.SetData("DbContext",db) ;
            }
            return db;  
        }
    }
}
