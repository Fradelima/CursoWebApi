
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;


namespace APICatalogo.RepositorioGenerico
{
  public class CategoriaRepositoryGenerico : Repository<Categoria>, ICategoriaRepositoryGenerico
  {
    public CategoriaRepositoryGenerico(CatalogoDbEfPowerContext context) : base(context)
    {
    }

        public async Task< IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParams)
        {
            var categorias = await  GetAllAsync();
            var resultado = categorias.AsQueryable();


            if (!string.IsNullOrEmpty(categoriasParams.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasParams.Nome));
            }

            var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasParams.PageNumber, categoriasParams.PageSize); 
               

           return categoriasFiltradas;
        }



    
  }
}
