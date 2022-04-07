using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midas.Data;
using Midas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Midas.Controllers
{
    public class UsuarioController : Controller
    {
        readonly MidasContext _midasContext;
        readonly UserManager<ApplicationUser> _gerenciadorUsuario;

        public UsuarioController(MidasContext midasContext, UserManager<ApplicationUser> gerenciadorUsuario)
        {
            _midasContext = midasContext;
            _gerenciadorUsuario = gerenciadorUsuario;
        }

        [HttpGet]
        public IActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Novo([FromBody] ApplicationUser usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { err = "NS000", msg = "Há campos vazios" });
                }
                var usuarioNovo = new ApplicationUser { UserName = usuario.UserName, Email = usuario.Email, CodigoUnico = usuario.CodigoUnico,
                    EnderecoID = 1 };
                var usuarioCriado = await _gerenciadorUsuario.CreateAsync(usuarioNovo, usuario.Senha);

                if (usuarioCriado.Succeeded)
                {
                    return CreatedAtAction("Usuario/edit/", usuario.Id);
                }
                else
                {
                    return StatusCode(500, usuarioCriado.Errors);
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet]
        [Route("[controller]/{usuario}/existe")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> ExisteUsuario([FromRoute] string usuario, CancellationToken cancellationToken)
        {
            try
            {
                int existeUsuario = await _midasContext.Usuario.AsNoTracking().
                    Where(user => user.UserName == usuario).CountAsync(cancellationToken);
                if (existeUsuario > 0)
                {
                    return Ok(new { existeUsuario = true });
                }
                return Ok(new { existeUsuario = false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

    }
}
