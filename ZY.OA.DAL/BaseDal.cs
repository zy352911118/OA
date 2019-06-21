using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.Model;
using ZY.OA.Model.Enum;

namespace ZY.OA.DAL
{
   public class BaseDal<T> where T:class,new()
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        DbContext db = DBContentFactory.GetCurrentDBContent();
        public IQueryable<T> GetEntities(Expression<Func<T, bool>> whereLambda)
        {

            return db.Set<T>().Where(whereLambda).AsQueryable();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetPageEntities<s>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderByLambda, bool isAsc)
        {
            if (isAsc)
            {
                var temp = db.Set<T>().Where(whereLambda)
                                     .OrderBy<T, s>(orderByLambda)
                                     .Skip(pageSize * (pageIndex - 1))
                                     .Take(pageSize).AsQueryable();
                total = db.Set<T>().Where(whereLambda).Count();
                return temp;
            }
            else
            {
                var temp = db.Set<T>().Where(whereLambda)
                                     .OrderByDescending<T, s>(orderByLambda)
                                     .Skip(pageSize * (pageIndex - 1))
                                     .Take(pageSize).AsQueryable();
                total = db.Set<T>().Where(whereLambda).Count();
                return temp;
            }
            
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public T Add(T Entity)
        {
            db.Set<T>().Add(Entity);
            //db.SaveChanges();
            return Entity;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool Update(T Entity)
        {
            db.Entry(Entity).State = System.Data.Entity.EntityState.Modified;
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool Delete(T Entity)
        {
            db.Entry(Entity).State = System.Data.Entity.EntityState.Deleted;
            return true;
        }
        /// <summary>
        /// 批量逻辑删除
        /// </summary>
        /// <param name="ids">要删除的ID集合</param>
        /// <returns></returns>
        public int DeleteListBylogical(List<int> ids)
        {
            foreach (var id in ids)
            {
                var entity= db.Set<T>().Find(id);
                db.Entry(entity).Property("DelFlag").CurrentValue =(short)DelFlagEnum.deleted;
                db.Entry(entity).Property("DelFlag").IsModified = true;
            }
            return ids.Count;
        }
    }
}
