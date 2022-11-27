using System.Linq.Expressions;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Implementations;

public class GenericDALRepository<TEntity>:IDALRepository<TEntity> where TEntity:class
{
    private ProviderContext _providerContext;
    private DbSet<TEntity> _dbSet;
    private ILogger<GenericDALRepository<TEntity>> _logger;

    public GenericDALRepository(ProviderContext providerContext, ILogger<GenericDALRepository<TEntity>> logger)
    {
        _providerContext = providerContext;
        _dbSet = _providerContext.Set<TEntity>();
        _logger = logger;
    }

    public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby, string includeProperties)
    {
        _logger.LogInformation("Beginning building the query for retrieving the data");
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            _logger.LogInformation("Applying the filter for the query");
            query = query.Where(filter);
        }
        
        includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(
            property =>
            {
                _logger.LogInformation($"Including property {property}");
                query = query.Include(property);
            }
        );

        _logger.LogInformation("Retrieving data from the database");
        
        return orderby != null ? orderby(query).ToList() : query.ToList();
    }

    public void Insert(TEntity entity)
    {
        _logger.LogInformation($"Inserting entity of type {typeof(TEntity)}");
        
        _dbSet.Add(entity);
        _providerContext.SaveChanges();
    }
}