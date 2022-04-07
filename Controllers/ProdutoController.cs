using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midas.Data;
using Midas.Data.Consultas;
using Midas.Interfaces;
using Midas.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
namespace Midas.Controllers
{
    public class ProdutoController : Controller
    {
        readonly MidasContext _midasContext;
        List<Endereco> _enderecos = new();
        IEndereco endereco = null;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        public ProdutoController(MidasContext midasContext)
        {
            _midasContext = midasContext;
            _midasContext.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        [HttpGet]
        [Route("[controller]/{id}/{nome}")]
        public async Task<ActionResult<QProduto>> Index(int id, string nome, CancellationToken cancellationToken)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    endereco = new Endereco();
                    ViewBag.endereco = await endereco.Get(User.FindFirst(ClaimTypes.NameIdentifier).Value, _midasContext);
                }

                else
                {
                    ViewBag.endereco = null;
                }

                bool existeProduto = await _midasContext.Produto.Where(p => p.ID == id).AnyAsync(cancellationToken);
                if (existeProduto)
                {
                    var dados = await _midasContext.Produto.Join(_midasContext.Imagem, prod => prod.ImagemID, img => img.ID,
                        (prod, img) => new { prod, img }).Join(_midasContext.Estoque, _prod => _prod.prod.ID, estoque => estoque.ProdutoID,
                        (_prod, estoque) => new { _prod, estoque }).Join(_midasContext.Promocao, __prod => __prod._prod.prod.PromocaoID,
                        promo => promo.ID, (__prod, promo) => new { __prod, promo }).Where(dado => dado.__prod._prod.prod.ID == id).
                        Select(dados => new
                        {
                            dados.__prod._prod.prod.ID,
                            dados.__prod._prod.img.Diretorio,
                            imgNome = dados.__prod._prod.img.Nome,
                            dados.__prod._prod.img.Formato,
                            dados.__prod._prod.prod.Nome,
                            dados.__prod._prod.prod.Valor,
                            dados.promo.Porcentagem,
                            dados.__prod._prod.prod.Descricao,
                            dados.__prod._prod.prod.Especificacao_Tecnica,
                            dados.__prod.estoque.Quantidade,
                            dados.__prod.estoque.PrevisaoChegada,
                        }).FirstOrDefaultAsync(cancellationToken);

                    StringBuilder imagemNomeJuntoComExtensao = new($"{dados.imgNome}.{dados.Formato}");
                    string caminho = Path.Combine(dados.Diretorio, imagemNomeJuntoComExtensao.ToString());
                    List<QProduto> produto = new();
                    produto.Add(new QProduto
                    {
                        ID = dados.ID,
                        Arquivo = File(await System.IO.File.ReadAllBytesAsync(caminho, cancellationToken), dados.Formato == "png" ? "image/png" : "image/jpeg"),
                        Nome = dados.Nome,
                        Valor = dados.Valor,
                        Porcentagem = dados.Porcentagem,
                        Descricao = dados.Descricao,
                        Especificacao_Tecnica = dados.Especificacao_Tecnica,
                        EstoqueQuantidade = dados.Quantidade,
                        EstoquePrevisaoChegada = dados.PrevisaoChegada,
                    });

                    ViewBag.comentarios = await _midasContext.Comentario.Join(_midasContext.Usuario, comment => comment.UsuarioID, user => user.Id, 
                        (comment, user) => new { comment, user }).Where(C => C.comment.ProdutoID == id && C.comment.Mensagem != null).
                        Select(dados => new QProduto
                        {
                            ID = dados.comment.ID,
                            Nome = dados.user.UserName,
                            ComentarioID = (int)dados.comment.ComentarioID,
                            Mensagem = dados.comment.Mensagem,
                            ComentarioPublicadoEm = dados.comment.PublicadoEm,
                            ComentarioFoiAlterado = dados.comment.FoiAlterado,
                            ComentarioFoiRecomendado = dados.comment.FoiRecomendado,
                            ComentarioUsuarioID = dados.comment.UsuarioID
                        }).ToListAsync(cancellationToken);

                    return View(produto);
                }

                else
                {
                    return NotFound("O produto foi removido ou não está mais à venda. Contacte o vendedor para mais detalhes.");
                }

            }

            catch (Exception ex)
            {
                return View(ex.Message);
            }

        }

       
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Novo()
        {

            endereco = new Endereco();
            ViewBag.endereco = await endereco.Get(User.FindFirst(ClaimTypes.NameIdentifier).Value, _midasContext);
            return View();
        }

        [HttpGet]
        [Route("/search")]
        public async Task<ActionResult<QProduto>> Search(string q, CancellationToken cancellationToken, int limite = 30, int p = 1)
        {
            double div = limite % 30;
            if (div > 0 || limite > 120)
            {
                return NoContent();
            }

            if (User.Identity.IsAuthenticated)
            {
                endereco = new Endereco();
                ViewBag.endereco = await endereco.Get(User.FindFirst(ClaimTypes.NameIdentifier).Value, _midasContext);
            }

            else
            {
                ViewBag.endereco = null;
            }


            var dados = await _midasContext.Produto.Join(_midasContext.Imagem, prod => prod.ImagemID, img => img.ID,
                  (prod, img) => new { prod, img }).Join(_midasContext.Promocao, produto => produto.prod.PromocaoID,
                  promo => promo.ID, (produto, promo) => new { produto, promo }).Join(_midasContext.Estoque, _produto => _produto.produto.prod.ID,
                  est => est.ProdutoID, (_produto, est) => new { _produto, est }).
                  Where(dado => dado._produto.produto.prod.Nome.Contains(q) && dado._produto.produto.prod.PromocaoID >= 0).
                    Select(dados => new
                    {
                        dados._produto.produto.prod.ID,
                        dados._produto.produto.prod.Valor,
                        dados._produto.produto.prod.Nome,
                        dados._produto.produto.img.Diretorio,
                        imgNome = dados._produto.produto.img.Nome,
                        dados._produto.produto.img.Formato,
                        dados._produto.promo.Porcentagem,
                        dados.est.Quantidade
                    }).Skip(p > 1 ? limite * p : 0).Take(limite).OrderBy(p => p.Nome).ToListAsync(cancellationToken);


            List<QProduto> produtosEncontrados = new();

            foreach (var dado in dados)
            {
                StringBuilder imagemNomeJuntoComExtensao = new($"{dado.imgNome}.{dado.Formato}");
                string caminho = Path.Combine(dado.Diretorio, imagemNomeJuntoComExtensao.ToString());
                produtosEncontrados.Add(new QProduto
                {
                    ID = dado.ID,
                    Valor = dado.Valor,
                    Nome = dado.Nome,
                    Arquivo = File(await System.IO.File.ReadAllBytesAsync(caminho, cancellationToken), dado.Formato == "png" ? "image/png" : "image/jpeg"),
                    PromocaoPorcentagem = dado.Porcentagem,
                    EstoqueQuantidade = dado.Quantidade
                });
            }

            return View(produtosEncontrados);

        }

        [HttpGet]
        [Route("/carrinho")]
        public async Task<IActionResult> Carrinho(CancellationToken cancellationToken)
        {

            if (HttpContext.Request.Cookies.ContainsKey("mdc"))
            {
                string cookieData = Request.Cookies["mdc"];
                string[] cookieSortedData = cookieData.Split("|").Where(x => x != "").ToArray();
                int qtdProdutosNoCarrinho = cookieSortedData.Length;
                if(qtdProdutosNoCarrinho > 0)
                {
                    int linha = 0;
                    int[] imgIds = new int[qtdProdutosNoCarrinho / 5];
                    StringBuilder imgSrc = new();

                    for (int i = 0; i < qtdProdutosNoCarrinho; i += 5)
                    {
                        if (i + 5 == qtdProdutosNoCarrinho)
                        {
                            imgSrc.Append(cookieSortedData[i]);
                            imgIds[linha] = Convert.ToInt32(cookieSortedData[i]);
                        }
                        else
                        {
                            imgSrc.Append(cookieSortedData[i]).Append(',');
                            imgIds[linha] = Convert.ToInt32(cookieSortedData[i]);
                        }
                        linha++;
                    }
                    linha = 0;

                    var imgs = await _midasContext.Produto.AsNoTracking().Join(_midasContext.Imagem.AsNoTracking(), prod => prod.ImagemID, img => img.ID,
                        (prod, img) => new { prod, img }).Join(_midasContext.Estoque.AsNoTracking(), _prod => _prod.prod.ID, est => est.ProdutoID, (_prod, est) =>
                         new { _prod, est }).Where(c => imgIds.Contains(c._prod.prod.ID)).Select(dados => new
                         {
                             Imagem = Path.Combine(dados._prod.img.Diretorio, $"{dados._prod.img.Nome}.{dados._prod.img.Formato}"),
                             dados.est.Quantidade,
                             dados.est.PrevisaoChegada
                         }).ToArrayAsync(cancellationToken);
                    qtdProdutosNoCarrinho = imgs.Length;

                    string[,] carrinhoProdutos = new string[qtdProdutosNoCarrinho, 6];
                    StringBuilder imageURIFormat = new();
                    imgSrc.Clear();
                    for (int i = 0; i < imgs.Length; i++)
                    {
                        imageURIFormat.Append(Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync(imgs[i].Imagem, cancellationToken)));/*.Append(imgs[i].Formato == "png" ? "image/png" : "image/jpeg")*/;
                        carrinhoProdutos[i, 0] = imageURIFormat.ToString();
                        carrinhoProdutos[i, 1] = cookieSortedData[linha];
                        carrinhoProdutos[i, 2] = cookieSortedData[linha + 1];
                        carrinhoProdutos[i, 3] = cookieSortedData[linha + 2];
                        carrinhoProdutos[i, 4] = cookieSortedData[linha + 3];
                        carrinhoProdutos[i, 5] = cookieSortedData[linha + 4];
                        imageURIFormat.Clear();
                        imgSrc.Clear();
                        linha += 5;
                    }

                    return View(carrinhoProdutos);
                }

                else
                {
                    return View(Array.Empty<string>());
                }

            }
            else
            {
                return View(Array.Empty<string>());
            }
        }

        [HttpGet]
        [Route("[controller]/cart/{productId}")]
        public bool Cart(string productId)
        {
            StringBuilder carrinho = new();
            if (!Request.Cookies.ContainsKey("mdc"))
            {
                carrinho.Append(productId).Append('|');
                Response.Cookies.Append("mdc", carrinho.ToString(), new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Secure = true,
                    MaxAge = TimeSpan.FromDays(365),
                    HttpOnly = false
                });
                return true;
            }
            else
            {
                string cartProducts = Request.Cookies["mdc"];
                if (cartProducts.Contains(productId))
                {
                    return false;
                }
                carrinho.Append(cartProducts).Append(productId).Append('|');
                Response.Cookies.Append("mdc", carrinho.ToString(), new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Secure = true,
                    MaxAge = TimeSpan.FromDays(365),
                    HttpOnly = false
                });

                return true;

            }
        }

    }
}
