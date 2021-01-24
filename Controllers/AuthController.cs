using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.api.Dtos;
using devbook.api.Data;
using devbook.api.dtos;
using devbook.api.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace devbook.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;

        private readonly IMapper _mapper;
        public AuthController(IAuthRepository repository, IConfiguration configuration,  IMapper mapper)
        {
            _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
        }
    [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            userForRegisterDTO.Username = userForRegisterDTO.Username.ToLower();
            if (await _repository.UserExists(userForRegisterDTO.Username)) {return BadRequest("Username already in use");}
            var userToCreate = _mapper.Map<User>(userForRegisterDTO);
            var createdUser = await _repository.Register(userToCreate, userForRegisterDTO.Password);

            var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);

            return CreatedAtRoute("GetUser", new {controller = "Users", id = createdUser.Id}, userToReturn);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDto)
        {
            var userFromRepo = await _repository.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            if (userFromRepo == null)
                return Unauthorized();
            var claimsArray = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimsArray),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token2 = tokenHandler.CreateToken(tokenDescriptor);
      


            return Ok(new
            {
                token = tokenHandler.WriteToken(token2)
             
            });
        }

        
    }
}