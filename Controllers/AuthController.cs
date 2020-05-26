using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Task.Data;
using Task.Dtos;
using Task.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username, userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}

//System.InvalidOperationException: Endpoint Task.Controllers.ToDosController.AddToDo(Task) contains authorization metadata, but a middleware was not found that supports authorization.Configure your application startup by adding app.UseAuthorization() inside the call to Configure(..) in the application startup code.The call to app.UseAuthorization() must appear between app.UseRouting() and app.UseEndpoints(...). at Microsoft.AspNetCore.Routing.EndpointMiddleware.ThrowMissingAuthMiddlewareException(Endpoint endpoint) at Microsoft.AspNetCore.Routing.EndpointMiddleware.Invoke(HttpContext httpContext) at Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware.Invoke(HttpContext context) at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context) HEADERS ======= Accept: application/json, text/plain, */* Accept-Encoding: gzip, deflate, br Accept-Language: en-US,en;q=0.9 Cache-Control: no-cache Connection: keep-alive Content-Length: 52 Content-Type: application/json Cookie: __RequestVerificationToken=DDVGXbfUAFqTCuMAPpKko38-4dptkHnW5dXdZfoDLtcHvyfoAy8SyK64oerzKILvnzHC-7YxekHNzF8kEl9CI4sD9abNhn--2FehtD3AX_M1 Host: localhost:5468 Pragma: no-cache Referer: http://localhost:5468/addtodo User-Agent: Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36 Origin: http://localhost:5468 Sec-Fetch-Site: same-origin Sec-Fetch-Mode: cors Sec-Fetch-Dest: empty 