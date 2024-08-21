
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace APICatalogo.RepositorioGenerico;

public class Repository<T> : IRepository<T> where T : class
{
  protected readonly CatalogoDbEfPowerContext _context;

  public Repository(CatalogoDbEfPowerContext context)
  {
    _context = context;
  }

  // AsNoTracking =
  //Retorna uma nova consulta em que
  // as entidades retornadas não serão armazenadas em cache no DbContext
  public async Task<IEnumerable<T>> GetAllAsync()
  {
    return await _context.Set<T>().AsNoTracking().ToListAsync();
  }
    


  public  async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
  {
    return await _context.Set<T>().FirstOrDefaultAsync(predicate);
  }

  public T Create(T entity)
  {
    _context.Set<T>().Add(entity);
    //_context.SaveChanges();
    return entity;
  }
  public T Update(T entity)
  {
    _context.Set<T>().Update(entity);
    //_context.Entry(entity).State = EntityState.Modified;
    //_context.SaveChanges();
    return entity;
  }
  public T Delete(T entity)
  {
    _context.Set<T>().Remove(entity);
    //_context.SaveChanges();
    return entity;
  }
}
