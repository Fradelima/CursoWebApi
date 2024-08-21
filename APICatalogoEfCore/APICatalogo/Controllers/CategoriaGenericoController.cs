
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Pagination;

using APICatalogo.RepositorioGenerico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

[Route("[controller]")]
  [ApiController]
  public class CategoriaGenericoController : ControllerBase
  {
  private readonly IUnitOfwork _uof;


  public CategoriaGenericoController(IUnitOfwork uof)

  {
    _uof = uof;
    
  }


    [HttpGet("filter/nome/pagination")]
    public async Task< ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas(
                                   [FromQuery] CategoriasFiltroNome categoriasFiltro)
    {
        var categoriasFiltradas =  await _uof.CategoriaRepositoryGenerico
                                     .GetCategoriasFiltroNomeAsync(categoriasFiltro);

        return ObterCategorias(categoriasFiltradas);

    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.Count,
            categorias.PageSize,
            categorias.PageCount,
            categorias.TotalItemCount,
            categorias.HasNextPage,
            categorias.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var categoriasDto = categorias.ToCategoriaDtoList();
        return Ok(categoriasDto);
    }



    [HttpGet]
    [Authorize]
  public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
  {
    var categorias = await _uof.CategoriaRepositoryGenerico.GetAllAsync();
    var categoriasDto = categorias.ToCategoriaDtoList();

    

    return Ok(categoriasDto);
  }
  [Authorize(Policy = "Admin")]
  [HttpGet("{id:int}", Name = "ObterCategoriaGenerico")]
  public async Task< ActionResult<CategoriaDTO>> Get(int id)
  {
    var categoria = await _uof.CategoriaRepositoryGenerico.GetAsync(c => c.CategoriaId == id);

    if (categoria is null)
    {
      //_logger.LogWarning($"Categoria com id= {id} não encontrada...");
      return NotFound($"Categoria com id= {id} não encontrada...");
    }


    var categoriaDTO = categoria.ToCategoriaDTO();
   
    return Ok(categoriaDTO);
  }
  [Authorize]
  [HttpPost]
  public  async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
  {
    if (categoriaDto is null)
    {
      //_logger.LogWarning($"Dados inválidos...");
      return BadRequest("Dados inválidos");
    }
    var categoria = categoriaDto.ToCategoria();
   

    var categoriaCriada = _uof.CategoriaRepositoryGenerico.Create(categoria);
     await _uof.CommitAsync();

    var NovacategoriaDto = categoriaCriada.ToCategoriaDTO();
    

    return new CreatedAtRouteResult("ObterCategoriaGenerico",
        new { id = NovacategoriaDto.CategoriaId },
        NovacategoriaDto);
  }

  [HttpPut("{id:int}")]
  public async Task< ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
  {
    if (id != categoriaDto.CategoriaId)
    {
      //_logger.LogWarning($"Dados inválidos...");
      return BadRequest("Dados inválidos");
    }
    var categoria = categoriaDto.ToCategoria();
    //var categoria = new Categoria()
    //{
    //  CategoriaId = categoriaDto.CategoriaId,
    //  Nome = categoriaDto.Nome,
    //  ImagemUrl = categoriaDto.ImagemUrl
    //};

     var categoriaAtualizada = _uof.CategoriaRepositoryGenerico.Update(categoria);
     await _uof.CommitAsync();

    var novaCategoriaDto= categoria.ToCategoriaDTO();

   

    return Ok(novaCategoriaDto);
  }

  [HttpDelete("{id:int}")]
  public async Task< ActionResult> Delete(int id)
  {
    var categoria = await _uof.CategoriaRepositoryGenerico.GetAsync(c => c.CategoriaId == id);

    if (categoria is null)
    {
      //_logger.LogWarning($"Categoria com id={id} não encontrada...");
      return NotFound($"Categoria com id={id} não encontrada...");
    }

    var categoriaExcluida = _uof.CategoriaRepositoryGenerico.Delete(categoria);
     await _uof.CommitAsync();

    var categoriaExcluidaDto = categoria.ToCategoriaDTO();

   
    return Ok(categoriaExcluidaDto);

  }

}



 

