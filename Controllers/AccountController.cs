using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Midas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Midas.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly Logger<AccountController> _logger;

        public AccountController(/*Logger<AccountController> logger, */SignInManager<ApplicationUser> signInManager)
        {
            //_logger = logger;
            _signInManager = signInManager;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/json"), Produces("application/json")]
        public async Task<IActionResult> Login([FromBody] ApplicationUser usuario)
        {
            try
            {
                var usuarioEntrou = await _signInManager.PasswordSignInAsync(usuario.UserName, usuario.Senha, true, true);
                if (User.Identity.IsAuthenticated)
                {
                    return Ok();
                }
                if (usuarioEntrou.Succeeded)
                {
                    return Ok(new { success = true });
                }
                else
                 {
                    return Ok(new { success = false });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

    }
}
