using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.DAL;
using ZY.OA.DALFactory;
using ZY.OA.IDAL;

namespace ZY.OA.BLL
{
   public abstract class BaseService<T> where T:class,new()
    {
        public IDBSession DBSession
        {
            get {return DBSessionFactory.GetCurrentDBSession(); }
        }      
         
        public IBaseDal<T> currentDal;
        public BaseService()
        {
            SetCurrentDal();
        }
        public abstract void SetCurrentDal();

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> GetEntities(Expression<Func<T, bool>> whereLambda)
        {
            return currentDal.GetEntities(whereLambda);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetPageEntities<s>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderByLambda, bool isAsc)
        {

            return currentDal.GetPageEntities(pageSize,pageIndex,out total,whereLambda,orderByLambda,isAsc);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public T Add(T Entity)
        {
            currentDal.Add(Entity);
            DBSession.SaveChanges();
            return Entity; 
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool Update(T Entity)
        {
            currentDal.Update(Entity);
            return DBSession.SaveChanges()>0;
            
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool Delete(T Entity)
        {
            currentDal.Delete(Entity);
            return DBSession.SaveChanges() > 0;
        }

        /// <summary>
        /// 批量逻辑删除
        /// </summary>
        /// <param name="ids">要删除的id集合</param>
        /// <returns></returns>
        public bool DeleteListBylogical(List<int> ids)
        {
            currentDal.DeleteListBylogical(ids);    
                  
            return DBSession.SaveChanges() > 0;
        }
    }
}
