using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midas.Data;
using Midas.Data.Consultas;
using Midas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Midas.Controllers
{
    [Authorize]
    public class CompraController : Controller
    {
        private readonly MidasContext _midasContext;
        public CompraController(MidasContext midasContext)
        {
            _midasContext = midasContext;
            _midasContext.ChangeTracker.AutoDetectChangesEnabled = false;
            _midasContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        [HttpGet]
        public async Task<ActionResult<QProduto>> Finalizar(CancellationToken cancellationToken)
        {
            if (Request.Cookies.ContainsKey("mdc"))
            {

                var dadosComprador = await _midasContext.Usuario.Join(_midasContext.Endereco, user => user.EnderecoID, end => end.ID,
                 (user, end) => new { user, end }).Where(dado => dado.user.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value).
                 Select(d => new QProduto
                 {
                     Email = d.user.Email,
                     UserName = d.user.UserName,
                     DataNascimento = d.user.DataNascimento,
                     UsuarioCodigoUnico = d.user.CodigoUnico,
                     Sexo = d.user.Sexo,
                     Pais = d.end.Pais,
                     Estado = d.end.Estado,
                     Cidade = d.end.Cidade,
                     Bairro = d.end.Bairro,
                     Rua = d.end.Rua,
                     Numero = d.end.Numero,
                     Complemento = d.end.Complemento,
                     CodigoUnico = d.end.CodigoUnico
                 }).ToListAsync(cancellationToken);

                string cookieData = Request.Cookies["mdc"];
                string[] cookieSortedData = cookieData.Split("|").Where(x => x != "").ToArray();
                Produto produtos = new();
                string[,] produtosCompra = await produtos.AddImageToCartAsync(cookieSortedData, cancellationToken, _midasContext);
                return View(new QProduto { Dados = dadosComprador, Carrinho = produtosCompra });




            }

            else
            {
                return View(StatusCode(500));

            }

        }

        
    }
}
