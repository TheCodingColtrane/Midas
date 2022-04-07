using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Midas.Data;
using Midas.Data.Consultas;
using Midas.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Midas.API
{
    [Area("api")]
    [Route("[area]/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProdutoController : ControllerBase
    {

        readonly MidasContext _midasContext; 
        //SqlConnection _sql;
        //readonly IServiceProvider _serviceProvider;
        const string _pastaUploadArquivos = @"C:\uploads\produtos\";
        //private Logger<ProdutoController> _logger;
        enum QtdRegistros
        {

        }
        public ProdutoController(MidasContext midasContext/*, IServiceProvider serviceProvider*//*, Logger<ProdutoController> logger*/)
        {
            _midasContext = midasContext;
            _midasContext.ChangeTracker.AutoDetectChangesEnabled = false;
            //_serviceProvider = serviceProvider;
            //_sql = new(_serviceProvider.GetRequiredService<IDbConnection>().ConnectionString);
            //_logger = logger;

        }

        /// <summary>
        /// Retorna todos os produtos
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>List<object></object></returns>
        [HttpGet]
        public async Task<ActionResult<Produto>> Get(CancellationToken cancellationToken)
        {
            try
            {
                //using SqlCommand sqlCommand = new("select * from endereco", _sql);
                //await _sql.OpenAsync(cancellationToken);
                //using var reader = await sqlCommand.ExecuteReaderAsync(cancellationToken);
                //int qtdRegistros = reader.VisibleFieldCount;
                //string[] dadosSql = new string[qtdRegistros];
                //uint cont = 0;

                //while (await reader.ReadAsync(cancellationToken))
                //{
                //    dadosSql[cont] = reader["CodigoUnico"].ToString();
                //    cont++;
                //}

                var dados = await _midasContext.Produto.AsNoTracking().Join(_midasContext.Imagem, P => P.ImagemID, I => I.ID,
                (prod, img) => new { prod, img }).Select(dado => new
                {
                    dado.img.Diretorio,
                    dado.img.Nome,
                    dado.prod.ID,
                    dado.img.Formato,
                    dado.prod.Valor,
                    produtoNome = dado.prod.Nome
                }).ToListAsync(cancellationToken);

                List<object> dadosProdutos = new();
                foreach (var dado in dados)
                {
                    string caminho = Path.Combine(dado.Diretorio, dado.Nome);
                    byte[] arquivo = await System.IO.File.ReadAllBytesAsync(@$"{caminho}.{dado.Formato}", cancellationToken);
                    var arquivoRetornado = GetImagem(arquivo, dado.Formato, Guid.NewGuid().ToString());
                    dadosProdutos.Add(new
                    {
                        dado.ID,
                        NomeProd = dado.produtoNome,
                        ValorProd = dado.Valor,
                        ImagemBase64Prod = Convert.ToBase64String(arquivo),
                        FormatoProd = dado.Formato.Replace(".", "")
                    });
                }

                return Ok(dadosProdutos);

            }

            catch (Exception ex)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Ok("Requisição cancelada");
                }

                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ImagemController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> Get(int? id, string idComprador, CancellationToken cancellationToken)
        {
            try
            {

                if (id is null || id is 0)
                {
                    return BadRequest("Não foi fornecido id do produto.");
                }

                if (idComprador is null)

                {
                   
                    var dados = await _midasContext.Produto.AsNoTracking().Join(_midasContext.Imagem.AsNoTracking(), prod => prod.ImagemID, img => img.ID,
                    (prod, img) => new { prod, img }).Join(_midasContext.Estoque.AsNoTracking(), _prod => _prod.prod.ID, estoque => estoque.ProdutoID,
                    (_prod, estoque) => new { _prod, estoque }).Join(_midasContext.Promocao.AsNoTracking(), produto => produto._prod.prod.PromocaoID,
                    promocao => promocao.ID, (produto, promocao) => new { produto, promocao }).Where(dado => dado.produto._prod.prod.ID == id).
                    Select(dados => new 
                    {
                        dados.produto._prod.prod.ID,
                        dados.produto._prod.img.Diretorio, 
                       imgNome = dados.produto._prod.img.Nome,
                        dados.produto._prod.img.Formato,
                        dados.produto._prod.prod.Nome,
                        dados.produto._prod.prod.Valor,
                        dados.promocao.Porcentagem,
                        dados.produto._prod.prod.Descricao,
                        dados.produto._prod.prod.Especificacao_Tecnica,
                        dados.produto.estoque.Quantidade,
                        dados.produto.estoque.PrevisaoChegada
                    }).FirstOrDefaultAsync(cancellationToken);
                    StringBuilder imagemNomeJuntoComExtensao = new($"{dados.imgNome}.{dados.Formato}");
                    string caminho = Path.Combine(dados.Diretorio, imagemNomeJuntoComExtensao.ToString());
                    List<QProduto> produto = new();
                    produto.Add(new QProduto {
                        ID = dados.ID,
                        Arquivo = File(await System.IO.File.ReadAllBytesAsync(caminho, cancellationToken), dados.Formato == "png" ? "image/png" : "image/jpeg"),
                        Nome = dados.Nome,
                        Valor = dados.Valor,
                        Porcentagem = dados.Porcentagem,
                        Descricao = dados.Descricao,
                        Especificacao_Tecnica = dados.Especificacao_Tecnica,
                        EstoqueQuantidade = dados.Quantidade,
                        EstoquePrevisaoChegada = dados.PrevisaoChegada
                    });
                    
                    return Ok(produto);
                }
                else
                {
                    var dados = await _midasContext.Produto.AsNoTracking().Join(_midasContext.Imagem.AsNoTracking(), prod => prod.ImagemID, img => img.ID,
                    (prod, img) => new { prod, img }).Join(_midasContext.Estoque.AsNoTracking(), _prod => _prod.prod.ID, estoque => estoque.ProdutoID,
                    (_prod, estoque) => new { _prod, estoque }).Join(_midasContext.Promocao.AsNoTracking(), __prod => __prod._prod.prod.PromocaoID, promo => promo.ID,
                    (__prod, promo) => new { __prod, promo }).Join(_midasContext.Usuario.AsNoTracking(), produto => produto.__prod._prod.prod.AnuncianteID,
                    user => user.Id, (produto, user) => new { produto, user }).Join(_midasContext.Endereco.AsNoTracking(), usuario => usuario.user.EnderecoID,
                    end => end.ID, (usuario, end) => new { usuario, end }).Where(dado => dado.usuario.user.Id == idComprador
                    && dado.usuario.produto.__prod._prod.prod.ID == id).Select(dados => new
                    {
                        dados.usuario.produto.__prod._prod.prod.ID,
                        dados.usuario.produto.__prod._prod.img.Diretorio,
                        imgNome = dados.usuario.produto.__prod._prod.img.Nome,
                        dados.usuario.produto.__prod._prod.img.Formato,
                        dados.usuario.produto.__prod._prod.prod.Nome,
                        dados.usuario.produto.__prod._prod.prod.Valor,
                        dados.usuario.produto.promo.Porcentagem,
                        dados.usuario.produto.__prod._prod.prod.Descricao,
                        dados.usuario.produto.__prod._prod.prod.Especificacao_Tecnica,
                        dados.usuario.produto.__prod.estoque.Quantidade,
                        dados.usuario.produto.__prod.estoque.PrevisaoChegada,
                        dados.end.CodigoUnico


                    }).FirstOrDefaultAsync(cancellationToken);

                    StringBuilder imagemNomeJuntoComExtensao = new($"{dados.imgNome}.{dados.Formato}");
                    string caminho = Path.Combine(dados.Diretorio, imagemNomeJuntoComExtensao.ToString());
                    List<object> produto = new();
                    produto.Add(new
                    {
                        dados.ID,
                        img = File(await System.IO.File.ReadAllBytesAsync(caminho, cancellationToken), dados.Formato == "png" ? "image/png" : "image/jpeg"),
                        dados.Nome,
                        dados.Valor,
                        dados.Porcentagem,
                        dados.Descricao,
                        dados.Especificacao_Tecnica,
                        dados.Quantidade,
                        dados.PrevisaoChegada,
                        dados.CodigoUnico
                    });

                    return Ok(produto);
                }


            }

            catch (Exception ex)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Ok("A requisição foi cancelada.");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("promocao")]
        public async Task<ActionResult> Promocao(CancellationToken cancellationToken)
        {
            try
            {

                return Ok(await _midasContext.Produto.AsNoTracking().Join(_midasContext.Imagem.AsNoTracking(), p => p.ImagemID, i => i.ID,
                (prod, img) => new { prod, img }).Join(_midasContext.Promocao.AsNoTracking(), _prod => _prod.prod.PromocaoID, promo => promo.ID,
                (_prod, promo) => new { _prod, promo }).Where(dado => dado._prod.prod.PromocaoID > 0).
                Select(dados => new {
                    dados._prod.img.Diretorio,
                    dados._prod.img.Nome, 
                    dados._prod.prod.Valor, 
                    produtoNome = dados._prod.prod.Nome,
                    dados.promo.Porcentagem
                
                }).ToListAsync(cancellationToken));

            }
            catch (Exception ex)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Ok();
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST api/<ImagemController>
        [HttpPost]
        [Consumes("multipart/form-data", new string[] { "application/json" })]
        public async Task<ActionResult> Post([FromForm] string jsonProduto, [FromForm] IFormFile upload, CancellationToken cancellationToken)
        {
            
            if (string.IsNullOrEmpty(jsonProduto) || upload.Length < 1)
            {
                return BadRequest("Houve um erro. Nnehum dado foi enviado");
            }
            Produto produto = JsonSerializer.Deserialize<Produto>(jsonProduto);
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Algum campo esta vazio.");
                }

                if (!Directory.Exists(_pastaUploadArquivos))
                {
                    Directory.CreateDirectory(_pastaUploadArquivos);
                    DirectoryInfo di = new(_pastaUploadArquivos);
                    di.Attributes &= ~FileAttributes.ReadOnly;

                }

                StringBuilder caminhoProdutoImagem = new(Path.Combine(_pastaUploadArquivos, produto.Dir));
                if (!Directory.Exists(caminhoProdutoImagem.ToString()))

                {
                    Directory.CreateDirectory(caminhoProdutoImagem.ToString());
                    DirectoryInfo di = new(caminhoProdutoImagem.ToString());
                    di.Attributes &= ~FileAttributes.ReadOnly;

                }
                await _midasContext.Database.BeginTransactionAsync(cancellationToken);
                using MemoryStream ms = new();
                await upload.CopyToAsync(ms, cancellationToken);
                Imagem imagem = new();
                imagem.Diretorio = caminhoProdutoImagem.ToString();
                imagem.Formato = upload.ContentType == "image/png" ? ".png" : ".jpeg";
                imagem.Nome = Guid.NewGuid().ToString();
                imagem.Tamanho = upload.Length;
                var novaImagem = await _midasContext.Imagem.AddAsync(imagem, cancellationToken);
                await _midasContext.SaveChangesAsync(cancellationToken);
                string caminhoProdutoNome = Path.Combine(caminhoProdutoImagem.ToString(), imagem.Nome);
                await System.IO.File.WriteAllBytesAsync(@$"{caminhoProdutoNome}.{imagem.Formato}", ms.ToArray(), cancellationToken);
                produto.CadastradoEm = DateTime.Now;
                produto.ImagemID = imagem.ID;
                await _midasContext.Produto.AddAsync(produto, cancellationToken);
                await _midasContext.SaveChangesAsync(cancellationToken);
                await _midasContext.Database.CommitTransactionAsync(cancellationToken);
                return Ok(new { success = true });
            }

            catch (DbUpdateException ex)
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return Ok("Requisição cancelada");
                    }
                    await _midasContext.Database.RollbackTransactionAsync(cancellationToken);
                    if (!string.IsNullOrEmpty(produto.Dir) && Directory.Exists($"{_pastaUploadArquivos}\\{produto.Dir}"))
                    {
                        Directory.Delete(@$"{_pastaUploadArquivos}\{produto.Dir}", true);
                    }
                    return StatusCode(500, ex.Message);
                }
                catch (Exception)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return Ok("Requisição cancelada");
                    }

                    if (!string.IsNullOrEmpty(produto.Dir) && Directory.Exists($"{_pastaUploadArquivos}\\{produto.Dir}"))
                    {
                        Directory.Delete(@$"{_pastaUploadArquivos}\{produto.Dir}", true);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, "Houve um erro na atualização da base.");
                }
            }

        }

        [HttpGet("Suggestions")]
        public async Task<ActionResult<Produto>> Suggestions(string q, CancellationToken cancellationToken)

        {
            if(q.Length > 300 || q.Length < 2)
            {
                return NoContent();
            }

            try
            {
                return Ok(await _midasContext.Produto.Where(p => p.Nome.StartsWith(q)).Select(dado => new { dado.Nome })
                    .Take(10).OrderBy(ob => ob.Nome).ToArrayAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                if(cancellationToken.IsCancellationRequested)
                {
                    return Ok();
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PUT api/<ImagemController>/5
        [HttpGet("[controller]/{id}/stock-check-avalaibility")]
        public async Task<ActionResult> Availability(int id, int qty, CancellationToken cancellationToken)
        {
            try
            {
    
                var stockAvalaible = await _midasContext.Estoque.Where(c => c.ProdutoID == id).
                    Select(dado => new { dado.Quantidade, dado.ProdutoID }).SingleOrDefaultAsync(cancellationToken);
                if (stockAvalaible.ProdutoID > 0)
                {

                    if(stockAvalaible.Quantidade == qty)
                    {
                        return Ok(new { stockAvalaibility = true });
                    }

                    if(stockAvalaible.Quantidade > 0)
                    {
                        return Ok(new { stockAalaibility = false, stockAvalaible = stockAvalaible.Quantidade });
                    }

                    else
                    {
                        return Ok(new { stockAvalaibility = false, stockAvalaible = 0 });
                    }
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception)
            {
                if(cancellationToken.IsCancellationRequested)
                {
                    return NoContent();
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Houve um erro na pesquisa das informações solicitadas.");
            }
          
        }

        // DELETE api/<ImagemController>/5
        

        private FileContentResult GetImagem(byte[] bytes, string tipoConteudo, string nome)
        {
            string formato = tipoConteudo == "jpeg" ? "image/jpeg" : "image/png";
            return File(bytes, formato, nome);
        }
    }
}
