using System.Linq.Expressions;

namespace APICatalogo.RepositorioGenerico
{
  public interface IRepository<T>
  {
    //cuidado para não violar o principio ISP(obrigar o cliente a implantar um metodo que ele não precisa
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);

    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
  }
}
