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
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Client, NoStore = false)]
    public class DepartamentoController : ControllerBase
    {
        readonly MidasContext _midasContext;
   
        public DepartamentoController(MidasContext midasContext)
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
    }
}
