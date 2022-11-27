namespace BusinessLogicLayer.Interfaces;

public interface IProviderBusinessLogic<TEntity> where TEntity : class
{
    IEnumerable<TEntity> ReadAll();

    void Create(TEntity entity);
}