using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Midas.Data;
using Midas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Midas.API
{   /*[Authorize]*/
    [Area("api")]
    [Route("[area]/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {
        readonly MidasContext _midasContext;
        //readonly Logger<EnderecoController> log;
        public EnderecoController(MidasContext midasContext)
        {
            _midasContext = midasContext;
        }
        // GET: api/<EnderecoController>
        [HttpGet("{cep}")]
        public async Task<ActionResult<Endereco>> Get([FromRoute] int cep, CancellationToken cancellationToken)
        {
            try
            {
                //log.LogInformation("vamo testar essa porra {1}", await _midasContext.Endereco.Where(d => d.Codigu_Unico == cep).ToListAsync(cancellationToken));
                return Ok(await _midasContext.Endereco.Where(d => d.CodigoUnico == cep).ToListAsync(cancellationToken));
            }
            catch(Exception ex)
            {
                if(cancellationToken.IsCancellationRequested)
                {
                    return Ok();
                }
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<EnderecoController>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EnderecoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<EnderecoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EnderecoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
