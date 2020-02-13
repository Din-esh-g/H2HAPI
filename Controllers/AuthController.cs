using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewProjectAPI.Models;
using NewProjectAPI.Repo;

namespace NewProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
    private readonly IAuthRepo _authRepo;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    public AuthController(IAuthRepo authRepo, IConfiguration configuration, IMapper mapper)
    {
      _authRepo = authRepo;
      _configuration = configuration;
      _mapper = mapper;
    }

   // [HttpPost("register")]

    ////public async Task<IActionResult> Register(string username, string password)
    ////{
    //// username = username.ToLower();
    ////  if (await _authRepo.UserExist(username))
    ////    return BadRequest("User is already exist. ");

    ////  var userTocreate = new User
    ////  {
    ////    Username = username,
    ////  };


    ////  var CreatedUser = await _authRepo.Register(userTocreate, password);
    ////  return StatusCode(201);
    ////}
    ///

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDTO userDTO)
    {
      userDTO.Username = userDTO.Username.ToLower();
      if (await _authRepo.UserExist(userDTO.Username))
        return BadRequest("User is already exist. ");

      //var userTocreate = new Users
      //{
      //  Username = userDTO.Username,
      //};

      var userTocreate = _mapper.Map<Users>(userDTO);


      var createdUser = await _authRepo.Register(userTocreate, userDTO.Password);
      //  return StatusCode(201);
      var userToReturn = _mapper.Map<UserForDetails>(createdUser);

      return CreatedAtRoute("GetUser", new { controller = "Users", id = createdUser.Id}, userToReturn);
    }

    [HttpPost("login")]

    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
      //Checking user
      var userFromRepo = await _authRepo.Login(loginDTO.Username.ToLower(), loginDTO.Password);
      if (userFromRepo == null)
        return Unauthorized();
      //If user is present
      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
        new Claim(ClaimTypes.Name, userFromRepo.Username)

      };
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds

      };

      var tokenHandler = new JwtSecurityTokenHandler();

      var token = tokenHandler.CreateToken(tokenDescriptor);

      //This is for the navbar photo
      var user = _mapper.Map<UserListDTO>(userFromRepo);


      return Ok(new
      {
        //Returning user extra for nav bar photo 
        token = tokenHandler.WriteToken(token), user
      });

    }

  }
}
