using Alkemy.Disney.Services.Email;
using Alkemy.Disney.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Alkemy.Disney.Controllers.Auth
{

    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailServices _emailServices;

        public AuthController(IConfiguration configuration, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IEmailServices emailServices)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailServices = emailServices;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromForm] LoginDTO model)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKey"));

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (result.Succeeded)
                {
                    ClaimsIdentity claims = new ClaimsIdentity();
                    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = claims,
                        // Nuestro token va a durar un día
                        Expires = DateTime.UtcNow.AddDays(1),
                        // Credenciales para generar el token usando nuestro secretykey y el algoritmo hash 256
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                                    SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var createdToken = tokenHandler.CreateToken(tokenDescriptor);

                    var token = tokenHandler.WriteToken(createdToken);
                    return Ok(token);
                }
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };

                var resp = await _userManager.CreateAsync(user, model.Password);
                if (resp.Succeeded)
                {
                    await _emailServices.SendEmailAsync(model.Email, 
                            "Se ha creado su usuario", "Bienvenido a Disneylandia ...");

                    return Ok("El usuario ha sido creado con éxito");
                }
                else
                {
                    return BadRequest(resp.Errors);
                }
            }

            return BadRequest(ModelState);
        }

        //Para probar el servicio de envio de emails.
        //[HttpPost]
        //[Route("[action]")]
        //public async Task<IActionResult> SendEmail(string email)
        //{
        //    _emailServices.SendEmailAsync(email, "Se ha creado su usuario", "Bienvenido a Disneylandia ...");

        //    return Ok();
        //}
    }
}
