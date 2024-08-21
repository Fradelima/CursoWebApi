
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.RepositorioGenerico
{
  public class ProdutoRepositoryGenerico : Repository<Produto>, IProdutoRepositoryGenerico
  {
    public ProdutoRepositoryGenerico(CatalogoDbEfPowerContext context) : base(context)
    {
    }

    public async Task< PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
    {
            //IQueryable<T> é apropriado quando você deseja realizar consultas de forma
            //eficiente em uma fonte de dados que pode ser consultada diretamente, como
            //um banco de dados. Ele suporta a consulta diferida e permite que as
            //consultas sejam traduzidas em consultas SQL eficientes quando você está
            //trabalhando com um provedor de banco de dados, como o Entity Framework.
            //------------------------------------------------------------------------
            //IEnumerable<T> é uma interface mais geral que representa uma coleção de
            //objetos em memória. Ela não oferece suporte a consultas diferidas ou tradução
            //de consultas SQL. Isso significa que, ao usar IEnumerable, você primeiro traz
            //todos os dados para a memória e, em seguida, aplica consultas, o que pode ser
            //menos eficiente para grandes conjuntos de dados.

            var produtos = await GetAllAsync();
            var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();

      var Resultado = PagedList<Produto>.ToPagedList(produtosOrdenados,
                 produtosParameters.PageNumber, produtosParameters.PageSize);

      return Resultado;
    }


    public async Task< PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutoFiltroPreco produtosFiltroParams)
    {
            var produtos = await GetAllAsync();

        if (produtosFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
      {
        if (produtosFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
        {
          produtos = produtos.Where(p => p.Preco > produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
        }
        else if (produtosFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
        {
          produtos = produtos.Where(p => p.Preco < produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
        }
        else if (produtosFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
        {
          produtos = produtos.Where(p => p.Preco == produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
        }
      }
          var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos.AsQueryable(),
                                                                     produtosFiltroParams.PageNumber,
                                                                     produtosFiltroParams.PageSize);
            return produtosFiltrados;
    }

    public async Task< IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
    {
            var produtos = await GetAllAsync();
            var produtosCategoria = produtos.Where(p => p.CategoriaId == id);
            return produtosCategoria;
        }
  }
}
