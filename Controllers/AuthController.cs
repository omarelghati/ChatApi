using ChatApi.Context;
using ChatApi.Models;
using ChatApi.Uitilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ChatApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private ChatContext _context;

        public AuthController(ChatContext context)
        {
            _context = context;
        }
        // GET api/values
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginDTO userLogin)
        {
            if (userLogin == null)
            {
                return BadRequest("Invalid client request");
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == userLogin.Username && u.Password == userLogin.Password);
            if (user.NotNull())
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                //var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userId", user.Id.ToString())
                }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
                };
                var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
                user.Token = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { user.Token });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}