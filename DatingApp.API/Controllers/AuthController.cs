using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTO;
using DatingApp.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRespository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthRespository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            this._config = config;
            this._repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
        {
            //use data transfer object DTO
            //-- use to map main model to simple object that is return by the view.

            //validate request
            userForRegister.Username = userForRegister.Username.ToLower();
            if (await _repo.UserExists(userForRegister.Username))
                return BadRequest("Username already exists.");

            var userToCreate = _mapper.Map<User>(userForRegister);
            
            var createdUser = await _repo.Register(userToCreate, userForRegister.Password);
            var userToReturn = _mapper.Map<UserForDetailDTO>(createdUser);

            //return StatusCode(201);
            return CreatedAtRoute("GetUser", new{Controller="Users", id= createdUser.Id}, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            //throw new Exception("Computer says no.");

            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName )
            };
            //create signing
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //create token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserForListDTO>(userFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token), user
            });
        }
    }
}