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
    public class PromocaoController : ControllerBase
    {
        readonly MidasContext _midasContext;
   
        public PromocaoController(MidasContext midasContext)
        {
            _midasContext = midasContext;
            _midasContext.ChangeTracker.AutoDetectChangesEnabled = false;

        }

        [HttpGet]
        public async Task<ActionResult<Promocao>> Get(CancellationToken cancellationToken)
        {
            try
            {
                
                return Ok(await _midasContext.Promocao.Where(pr => pr.EstaAtiva == true).
                    Select(dados => new { dados.ID, dados.Nome, dados.Descricao }).ToListAsync(cancellationToken));
                
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
    }
}
