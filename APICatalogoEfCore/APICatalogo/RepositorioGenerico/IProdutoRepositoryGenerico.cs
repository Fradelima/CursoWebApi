using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.RepositorioGenerico
{
  public interface IProdutoRepositoryGenerico : IRepository<Produto>
  {
     Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
    Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutoFiltroPreco produtosFiltroParams);
    Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
  }
}
