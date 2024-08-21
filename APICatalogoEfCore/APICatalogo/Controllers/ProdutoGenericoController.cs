using APICatalogo.Models;
using APICatalogo.RepositorioGenerico;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICatalogo.DTOs;
using AutoMapper;
using APICatalogo.Pagination;
using Newtonsoft.Json;

[Route("[controller]")]
[ApiController]
public class ProdutoGenericoController : ControllerBase
{

  //Aqui eu poderia usar apenas o repositório específico
  //Pois como ele implementa IRepository ele contém todos
  //os métodos do repositório genérico e também o método específico
  //sendo suficiente para realizar todas as operações 

  private readonly IUnitOfwork _uof;
  private readonly IMapper _mapper;


  public ProdutoGenericoController(IUnitOfwork uof, IMapper mapper)
  {
    _uof = uof;
    _mapper = mapper;
  }


  [HttpGet("pagination")]
  public async Task< ActionResult<IEnumerable<ProdutoDto>>> GetAsync([FromQuery] ProdutosParameters produtosParameters)
  {
    var produtos = await  _uof.ProdutoRepositoryGenerico.GetProdutosAsync(produtosParameters);

    var metadata = new
    {
      produtos.TotalCount,
      produtos.PageSize,
      produtos.CurrentPage,
      produtos.TotalPages,
      produtos.HasNext,
      produtos.HasPrevious
    };

    Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

    var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
    return Ok(produtosDto);
  }

  [HttpGet("filter/preco/pagination")]
  public async Task< ActionResult<IEnumerable<ProdutoDto>>> GetProdutosFilterPreco([FromQuery] ProdutoFiltroPreco
                                                                                   produtosFilterParameters)
  {
    var produtos = await _uof.ProdutoRepositoryGenerico.GetProdutosFiltroPrecoAsync(produtosFilterParameters);
    return ObterProdutos(produtos);
  }
  private ActionResult<IEnumerable<ProdutoDto>> ObterProdutos(PagedList<Produto> produtos)
  {
    var metadata = new
    {
      produtos.TotalCount,
      produtos.PageSize,
      produtos.CurrentPage,
      produtos.TotalPages,
      produtos.HasNext,
      produtos.HasPrevious
    };

    Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
    var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
    return Ok(produtosDto);
  }




  [HttpGet]
  public async Task< ActionResult<IEnumerable<ProdutoDto>>> Get()
  {
    var produtos =await  _uof.ProdutoRepositoryGenerico.GetAllAsync();
    if (produtos is null)
    {
      return NotFound();
    }
    var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);

    return Ok(produtosDto);
  }

  [HttpGet("produtos/{id}")]
  public async Task< ActionResult<IEnumerable<ProdutoDto>>> GetProdutosCategoria(int id)
  {
    var produtos = await  _uof.ProdutoRepositoryGenerico.GetProdutosPorCategoriaAsync(id);

    if (produtos is null)
      return NotFound();

    var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);

    return Ok(produtosDto);
  }



  [HttpGet("{id}", Name = "ObterProdutoGenerico")]
  public async Task <ActionResult<ProdutoDto>> Get(int id)
  {
    var produto = await _uof.ProdutoRepositoryGenerico.GetAsync(c => c.ProdutoId == id);
    if (produto is null)
    {
      return NotFound("Produto não encontrado...");
    }

    var produtoDto = _mapper.Map<ProdutoDto>(produto);
    return Ok(produtoDto);
  }

  [HttpPost]
  public async Task< ActionResult<ProdutoDto>> Post(ProdutoDto produtoDto)
  {
    if (produtoDto is null)
      return BadRequest();

    var produto = _mapper.Map<Produto>(produtoDto);

    var novoProduto = _uof.ProdutoRepositoryGenerico.Create(produto);
    await _uof.CommitAsync();

    var novoProdutoDto = _mapper.Map<ProdutoDto>(novoProduto);

    return new CreatedAtRouteResult("ObterProdutoGenerico",
        new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
  }

  [HttpPut("{id:int}")]
  public async Task< ActionResult<ProdutoDto>> Put(int id, ProdutoDto produtoDto)
  {
    if (id != produtoDto.ProdutoId)
      return BadRequest();//400

    var produto = _mapper.Map<Produto>(produtoDto);

    var produtoAtualizado =  _uof.ProdutoRepositoryGenerico.Update(produto);
   await  _uof.CommitAsync();

    var produtoAtualizadoDto  = _mapper.Map<ProdutoDto>(produtoAtualizado);

    return Ok(produtoAtualizadoDto);
  }

  [HttpDelete("{id:int}")]
  public async Task< ActionResult<ProdutoDto>> Delete(int id)
  {
    var produto = await _uof.ProdutoRepositoryGenerico.GetAsync(p => p.ProdutoId == id);
    if (produto is null)
    {
      return NotFound("Produto não encontrado...");
    }

    var produtoDeletado = _uof.ProdutoRepositoryGenerico.Delete(produto);
    await _uof.CommitAsync();

    var produtoDeletadoDto = _mapper.Map<ProdutoDto>(produtoDeletado);
    return Ok(produtoDeletadoDto);
  }
}
