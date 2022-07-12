namespace GenericRepository.Interfaces;

public interface IRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : class { }