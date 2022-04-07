using Microsoft.AspNetCore.Mvc;
using Midas.Data;
using Midas.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Midas.Controllers
{
    public class HomeController : Controller
    {
        readonly MidasContext _midasContext;
        readonly UserManager<ApplicationUser> _gerenciadorUsuario;
        public HomeController(MidasContext midasContext, UserManager<ApplicationUser> gerenciadorUsuario)
        {
            _midasContext = midasContext;
            _gerenciadorUsuario = gerenciadorUsuario;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Novo(ApplicationUser usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { err = "NS000", msg = "Há campos vazios" });
                }
                var usuarioCriado = await _gerenciadorUsuario.CreateAsync(usuario, usuario.Senha);

                if(usuarioCriado.Succeeded)
                {
                    return CreatedAtAction("Usuario/edit/", usuario.Id);
                }
                else
                {
                    return StatusCode(500, "algo deu errado");
                }
            }
            
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
          
        }
    }
}
