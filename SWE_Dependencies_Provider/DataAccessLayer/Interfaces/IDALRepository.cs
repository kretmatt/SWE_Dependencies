using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces;

public interface IDALRepository<TEntity> where TEntity:class
{
    IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby, string includeProperties);

    void Insert(TEntity entity);
}