using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProAgil.Api.Dtos;
using ProAgil.Domain.Identity;

namespace ProAgil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _config { get; }
        private IMapper _mapper { get; }
        private UserManager<User> _userManager { get; }
        private SignInManager<User> _signInManager { get; }
        public UserController(
            IConfiguration config,
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IMapper mapper
        )
        {
            this._mapper = mapper;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._config = config;
        }

        [HttpGet("GetUser")]

        public async Task<IActionResult> GetUser()
        {
            return Ok(new UserDto());
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                var user = this._mapper.Map<User>(userDto);
                
                var result = await this._userManager.CreateAsync(user, userDto.Password);

                var userReturn = _mapper.Map<UserDto>(user);

                if(result.Succeeded)
                {
                    return Created("GetUser", userReturn);
                }
                return BadRequest(result.Errors);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao efetuar registrar usuário { ex.Message }");   
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            try
            {
                var user = await this._userManager.FindByNameAsync(loginDto.UserName);
                var result =  await this._signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if(result.Succeeded)
                {
                    var appUser = await this._userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == loginDto.UserName.ToUpper());

                    var userToReturn = this._mapper.Map<UserLoginDto>(appUser);

                    return Ok(new {
                        token = GenerateJWToken(appUser).Result,
                        user = userToReturn
                    });
                } 

                return BadRequest();  
                    
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized, $"Erro ao efetuar login { ex.Message }");   
            }
        }

        private async Task<string> GenerateJWToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName )
            };

            var roles = await this._userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this._config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}