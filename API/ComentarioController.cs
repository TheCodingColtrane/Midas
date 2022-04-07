using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midas.Data;
using Midas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Midas.API
{
    [Area("api")]
    [Route("[area]/[controller]")]
    [ApiController]
    [ResponseCache(Duration = 180, Location = ResponseCacheLocation.Client, NoStore = false)]
    public class ComentarioController : ControllerBase
    {
        readonly MidasContext _midasContext;
   
        public ComentarioController(MidasContext midasContext)
        {
            _midasContext = midasContext;
            _midasContext.ChangeTracker.AutoDetectChangesEnabled = false;

        }

        [HttpGet]
        public async Task<ActionResult<Departamento>> Get(CancellationToken cancellationToken)
        {
            try
            {
                
                return Ok(await _midasContext.Departamento.ToListAsync(cancellationToken));
                
            }

            catch(Exception ex)
            {
                if(cancellationToken.IsCancellationRequested)
                {
                    return Ok("A requisição foi cancelada");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Comentario>> Post([FromBody] Comentario comentario, CancellationToken cancellationToken)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _midasContext.ChangeTracker.AutoDetectChangesEnabled = true;
                    var existeProduto = _midasContext.Produto.Find(comentario.ProdutoID);
                    if(existeProduto.ID is 0)
                    {
                        return NotFound("O comentário enviado não está relacionado a nenhum produto. O produto não existe");
                    }
                    else
                    {
                        comentario.PublicadoEm = DateTime.Now;
                        var novoComentario = await _midasContext.Comentario.AddAsync(comentario, cancellationToken);
                        await _midasContext.SaveChangesAsync(cancellationToken);
                        return Ok(comentario);

                    }
                  
                }

                return BadRequest("houve um erro no envio dos dados");
            }

            catch(Exception ex)
            {
                if(cancellationToken.IsCancellationRequested)
                {
                    return Ok("Requisição cancelada");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
            
        }
    }
}
