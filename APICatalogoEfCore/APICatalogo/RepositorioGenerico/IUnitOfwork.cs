namespace APICatalogo.RepositorioGenerico
{
  public interface IUnitOfwork
  {
    IProdutoRepositoryGenerico ProdutoRepositoryGenerico { get; }
    ICategoriaRepositoryGenerico CategoriaRepositoryGenerico { get; }
        Task CommitAsync();
    }
}
