
using APICatalogo.Models;

namespace APICatalogo.RepositorioGenerico
{
  public class UnitOfwork : IUnitOfwork
  {
    private IProdutoRepositoryGenerico _produtoRepository;

    private ICategoriaRepositoryGenerico _categoriaRepository;
    public CatalogoDbEfPowerContext _context;

    public UnitOfwork(CatalogoDbEfPowerContext context)
    {
      _context = context;
    }

    public IProdutoRepositoryGenerico ProdutoRepositoryGenerico
    {
      get 
      {
        return _produtoRepository = _produtoRepository ?? new ProdutoRepositoryGenerico(_context);

        //if(_produtoRepository == null)
        //{
        //  new ProdutoRepositoryGenerico(_context);
        //}
        //return _produtoRepository;
      }
    }

    public ICategoriaRepositoryGenerico CategoriaRepositoryGenerico
    {
      get
      {
        return _categoriaRepository = _categoriaRepository ?? new CategoriaRepositoryGenerico(_context);
      }
    }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
    {
      _context.Dispose();
    }
  }
}
