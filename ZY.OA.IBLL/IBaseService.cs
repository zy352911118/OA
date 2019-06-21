using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.IDAL;

namespace ZY.OA.IBLL
{
    public interface IBaseService<T> where T : class, new()
    {

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> GetEntities(Expression<Func<T, bool>> whereLambda);


        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetPageEntities<s>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderByLambda, bool isAsc);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        T Add(T Entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        bool Update(T Entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        bool Delete(T Entity);

        /// <summary>
        /// 批量逻辑删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteListBylogical(List<int> ids);
    }
}
