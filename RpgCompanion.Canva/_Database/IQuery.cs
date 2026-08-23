namespace RpgCompanion.Core;

using System.Linq.Expressions;

public interface IQuery<T>
{
    IQuery<T> Filter(Expression<Func<T, bool>> predicate);
    IQuery<T> Sort(Expression<Func<T, object>> keySelector, bool descending = false);
    IQuery<T> Skip(int count);
    IQuery<T> Take(int count);

    IQuery<T> Include<TIncluded>(Expression<Func<T, Rel<TIncluded>>> selector) where TIncluded : IEntity;

    Task<List<T>> ExecuteAsync();
    Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> projection);
}
