using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Authoritiy.IServices.BaseServices
{
    public interface IBaseEntity<T> where T : class, new()
    {
        Task<T> Add(T model);

        Task<List<T>> Query(Expression<Func<T, bool>> whereLambda);

        Task<T> GetModelAsync(Expression<Func<T, bool>> whereLambda);

        Task<int> Count(Expression<Func<T, bool>> whereLambda);

        Task<List<T>> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true);

        Task<int> DelBy(Expression<Func<T, bool>> delWhere);

        Task<int> Modfiy(T Entity);

        Task<int> UpdateContext(T Model);

        IQueryable<T> GetAll(Expression<Func<T, bool>> whereLambda);
    }
}
