using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.RepositorioGenerico
{
  public interface ICategoriaRepositoryGenerico : IRepository<Categoria>
  {
        Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParams);
  }
}
